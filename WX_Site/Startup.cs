using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;
using _2BSite.Database;
using _2BSite.Service;
using Autofac;
using AutoMapper;
using Core.Redis;
using Core.Redis.Configuration;
using Identity.Database.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.WebEncoders;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Senparc.CO2NET;
using Senparc.CO2NET.RegisterServices;
using Senparc.CO2NET.Trace;
using Senparc.Weixin;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP;
using Senparc.Weixin.RegisterServices;
using WX_Site.Filters;

namespace WX_Site
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(env.ContentRootPath)
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               //.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: false, reloadOnChange: true)
               .AddEnvironmentVariables();
            Core.Infrastructure.Global.Configuration = this.Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; private set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            //注册Swagger生成器，定义一个和多个Swagger 文档
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "WX API", Version = "v1" });
                // 为 Swagger JSON and UI设置xml文档注释路径
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
                var xmlPath = Path.Combine(basePath, "WX_Site.xml");
                c.IncludeXmlComments(xmlPath);
            });

            var redisConfiguration = Configuration.GetSection("redisConfiguration").Get<RedisConfiguration>();

            services.AddSingleton(redisConfiguration);
            services.AddSingleton<ICacheClient>(new StackExchangeRedisCacheClient(new Core.Redis.Extensions.NewtonsoftSerializer(),
                redisConfiguration));

            var dbServerConfiguration = Configuration.GetSection("DBServerConfiguration").Get<Core.Infrastructure.DBRW.DBServerConfiguration>();
            Core.Infrastructure.Global.DBRWManager = new Core.Infrastructure.DBRW.DBRWManager(dbServerConfiguration);

            

            services.AddDataProtection();
            string appname = Configuration.GetValue<string>("AppName");
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
             .AddCookie(options =>
             {
                 options.AccessDeniedPath = new PathString("/Home/denied");
                 options.Cookie.Name = appname;
             }
             );
            // 配置跨域处理，允许所有来源
            services.AddCors(options =>
            options.AddPolicy("cors",
            p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

            services.AddControllersWithViews(options =>
            {
                //设计全局错误返回
                options.Filters.Add(typeof(CustomExceptionFilterAttribute)); // an instance
            }).AddNewtonsoftJson(options =>
            {
                //设计全局JSON返回格式
                options.SerializerSettings.ObjectCreationHandling = Newtonsoft.Json.ObjectCreationHandling.Replace;
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                IsoDateTimeConverter timeFormate = new IsoDateTimeConverter();
                timeFormate.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
                options.SerializerSettings.Converters.Add(timeFormate);
                options.SerializerSettings.Formatting = Formatting.Indented;
                options.SerializerSettings.NullValueHandling = NullValueHandling.Include;//必须包含
            });


            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = Configuration.GetValue<string>("SessionRedisConnection");
                options.InstanceName = "urn:" + appname;
            });
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10); //session活期时间
                options.Cookie.HttpOnly = true;//设为httponly
            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //配置 转接头
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            //文字被编码 https://github.com/aspnet/HttpAbstractions/issues/315
            services.Configure<WebEncoderOptions>(options =>
            {
                options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
            });

            /*
             * CO2NET 是从 Senparc.Weixin 分离的底层公共基础模块，经过了长达 6 年的迭代优化，稳定可靠。
             * 关于 CO2NET 在所有项目中的通用设置可参考 CO2NET 的 Sample：
             * https://github.com/Senparc/Senparc.CO2NET/blob/master/Sample/Senparc.CO2NET.Sample.netcore/Startup.cs
             */

            services.AddSenparcGlobalServices(Configuration)//Senparc.CO2NET 全局注册
                    .AddSenparcWeixinServices(Configuration);//Senparc.Weixin 注册
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            //注册NLOG，目前来说不需要
            //builder.Register(s => LogManager.GetCurrentClassLogger()).As<ILogger>().SingleInstance();

            builder.RegisterModule(new ServiceModules());

            this.RegisterAutomapper(builder);

            //可以注入IServiceProvider 来自定义RESOLVE方法
        }

        private void RegisterAutomapper(ContainerBuilder builder)
        {
            builder.Register(context => new MapperConfiguration(configuration =>
            {
                foreach (var profile in context.Resolve<IEnumerable<Profile>>())
                {
                    configuration.AddProfile(profile);
                }
            }))
           .AsSelf()
           .SingleInstance();

            builder.Register(context => context.Resolve<MapperConfiguration>()
                .CreateMapper(context.Resolve))
                .As<IMapper>()
                .InstancePerLifetimeScope();
        }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IOptions<SenparcSetting> senparcSetting, IOptions<SenparcWeixinSetting> senparcWeixinSetting)
        {
            //配置 转接头
            app.UseForwardedHeaders();
            app.UseEnableRequestRewind();
            app.UseSession();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            // 允许所有跨域，cors是在ConfigureServices方法中配置的跨域策略名称
            app.UseCors("cors");
            //InitializeDatabase();
            app.UseStaticFiles();
            app.UseRouting();
            //app.UseCookiePolicy();

            //启用中间件服务生成Swagger作为JSON终结点
            app.UseSwagger();
            //启用中间件服务对swagger-ui，指定Swagger JSON终结点
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "WX API V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=WxOpen}/{action=Index}/{id?}");
            });


            // 启动 CO2NET 全局注册，必须！
            IRegisterService register = RegisterService.Start(senparcSetting.Value)
                                                        //关于 UseSenparcGlobal() 的更多用法见 CO2NET Demo：https://github.com/Senparc/Senparc.CO2NET/blob/master/Sample/Senparc.CO2NET.Sample.netcore/Startup.cs
                                                        .UseSenparcGlobal();

            register.ChangeDefaultCacheNamespace("zjshow");

            var redisConfigurationStr = senparcSetting.Value.Cache_Redis_Configuration;
            var useRedis = !string.IsNullOrEmpty(redisConfigurationStr) && redisConfigurationStr != "#{Cache_Redis_Configuration}#"/*默认值，不启用*/;
            if (useRedis)//这里为了方便不同环境的开发者进行配置，做成了判断的方式，实际开发环境一般是确定的，这里的if条件可以忽略
            {
                /* 说明：
                 * 1、Redis 的连接字符串信息会从 Config.SenparcSetting.Cache_Redis_Configuration 自动获取并注册，如不需要修改，下方方法可以忽略
                /* 2、如需手动修改，可以通过下方 SetConfigurationOption 方法手动设置 Redis 链接信息（仅修改配置，不立即启用）
                 */
                Senparc.CO2NET.Cache.Redis.Register.SetConfigurationOption(redisConfigurationStr);

                //以下会立即将全局缓存设置为 Redis
                Senparc.CO2NET.Cache.Redis.Register.UseKeyValueRedisNow();//键值对缓存策略（推荐）
                                                                          //Senparc.CO2NET.Cache.Redis.Register.UseHashRedisNow();//HashSet储存格式的缓存策略

                //也可以通过以下方式自定义当前需要启用的缓存策略
                //CacheStrategyFactory.RegisterObjectCacheStrategy(() => RedisObjectCacheStrategy.Instance);//键值对
                //CacheStrategyFactory.RegisterObjectCacheStrategy(() => RedisHashSetObjectCacheStrategy.Instance);//HashSet
            }

            register.RegisterTraceLog(ConfigTraceLog);//配置TraceLog
            string appname = Configuration.GetValue<string>("AppName");

            //开始注册微信信息，必须！
            register.UseSenparcWeixin(senparcWeixinSetting.Value, senparcSetting.Value)
                //注册公众号（可注册多个）                                              
                .RegisterMpAccount(senparcWeixinSetting.Value, "【" + appname + "】公众号");
        }


        /// <summary>
        /// 配置微信跟踪日志
        /// </summary>
        private void ConfigTraceLog()
        {
            //这里设为Debug状态时，/App_Data/WeixinTraceLog/目录下会生成日志文件记录所有的API请求日志，正式发布版本建议关闭

            //如果全局的IsDebug（Senparc.CO2NET.Config.IsDebug）为false，此处可以单独设置true，否则自动为true
            SenparcTrace.SendCustomLog("系统日志", "系统启动");//只在Senparc.Weixin.Config.IsDebug = true的情况下生效

            //全局自定义日志记录回调
            SenparcTrace.OnLogFunc = () =>
            {
                //加入每次触发Log后需要执行的代码
            };

            //当发生基于WeixinException的异常时触发
            WeixinTrace.OnWeixinExceptionFunc = ex =>
            {
                //加入每次触发WeixinExceptionLog后需要执行的代码

                //发送模板消息给管理员                             -- DPBMARK Redis
                var eventService = new WX_CommonService.EventService();
                eventService.ConfigOnWeixinExceptionFunc(ex);      // DPBMARK_END
            };
        }

        private void InitializeDatabase()
        {
            var optionsBuilder_HRMS = new DbContextOptionsBuilder<BSiteContext>();
            optionsBuilder_HRMS.UseSqlServer(Core.Infrastructure.Global.DBRWManager.GetMaster(optionsBuilder_HRMS.Options.ContextType.ToString()));
            using (var db_hrms = new BSiteContext(optionsBuilder_HRMS.Options))
            {
                db_hrms.Database.Migrate();
            }

            var optionsBuilder = new DbContextOptionsBuilder<Identity.Database.IdentityDataContext>();
            optionsBuilder.UseSqlServer(Core.Infrastructure.Global.DBRWManager.GetMaster(optionsBuilder.Options.ContextType.ToString()));
            using (var db = new Identity.Database.IdentityDataContext(optionsBuilder.Options))
            {
                db.Database.Migrate();

                var item = db.Users.Where(p => p.LoginName == "administrator").FirstOrDefault();
                if (item == null)
                {
                    var model = new User()
                    {
                        AliasName = "administrator",
                        LoginName = "administrator",
                        Password = "3611388ad27022d57bbd8f3938f70615",//q1w2E#R$
                        IsDisabled = false,
                        IsAdmin = true,
                        CreateTime = DateTime.Now,
                        Creator = "system_generate",
                    };
                    db.Users.Add(model);
                    db.SaveChanges();
                }
            }

        }
    }
}
