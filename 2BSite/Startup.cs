using _2BSite.App_Start;
using _2BSite.Database;
using _2BSite.Middleware;
using _2BSite.Models;
using _2BSite.Service;
using _2BSite.Service.Service;
using Autofac;
using AutoMapper;
using Core.Redis;
using Core.Redis.Configuration;
using ExcelReport;
using Identity.Database;
using Identity.Database.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.WebEncoders;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace _2BSite
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
          
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            Core.Infrastructure.Global.Configuration = (IConfigurationRoot)(this.Configuration = builder.Build());
            Env = env;
            Configurator.Put(".xls", new Npoi.Report.Driver.NpoiDriver.WorkbookLoader());
            Configurator.Put(".xlsx", new Npoi.Report.Driver.NpoiDriver.WorkbookLoader());
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //配置
            services.AddOptions();
            //注入配置类
            services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));

            var jwtSettings = new JwtSettings();
            //把配置的key绑定到jwtSettings实例
            Configuration.Bind("JwtSettings", jwtSettings);
            //Console.WriteLine(jwtSettings.SecretKey);

            //绑定配置参数到实例
            var redisConfiguration = Configuration.GetSection("redisConfiguration").Get<RedisConfiguration>();
            //注入redis连接实例
            services.AddSingleton(redisConfiguration);
            //注入redis缓存实例
            services.AddSingleton<ICacheClient>(new StackExchangeRedisCacheClient(new Core.Redis.Extensions.NewtonsoftSerializer(),redisConfiguration));
            //获取数据库连接对象
            var dbServerConfiguration = Configuration.GetSection("DBServerConfiguration").Get<Core.Infrastructure.DBRW.DBServerConfiguration>();
            //注入数据库连接管理
            Core.Infrastructure.Global.DBRWManager = new Core.Infrastructure.DBRW.DBRWManager(dbServerConfiguration);
            //分布式系统用来加密通讯消息的密钥
            //相当于 asp.net中的machine key
            //string path = Path.Combine(Directory.GetCurrentDirectory(), @"\PersistKeys\Share\hrms-keys\");
            //services.AddDataProtection(options => options.ApplicationDiscriminator = "ruyicang.com")
            //.SetApplicationName("hrms-app")
            //.ProtectKeysWithDpapi()
            //.PersistKeysToFileSystem(new DirectoryInfo(path));
            //.PersistKeysToRedis();
            //后台服务
            //services.AddBackgroundServices();
            //登录授权验证
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
             .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
             {
                 options.LoginPath = new PathString("/home/login");
                 options.AccessDeniedPath = new PathString("/home/denied");
                 options.Cookie.Name = "2BSite";
                 options.Cookie.Path = "/";
                 //if (Env.IsProduction())
                 //{
                 //    options.Cookie.Domain = ".ruyicang.com";
                 //}
             }
             );

            //添加返回压缩
            services.AddResponseCompression();
            services.AddResponseCaching();
            //跨域配置
            services.AddCors(option =>
            {
                option.AddPolicy("2BSite_cros", policy => policy.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .WithOrigins("http://localhost:5050"));
            });
            
            //services.AddDistributedMemoryCache();
            //添加session
            services.AddSession(options =>
            {
                //本方法的底层缓存方法使用的就是IDistributedCache
                options.IdleTimeout = TimeSpan.FromMinutes(30); //session活期时间
                options.Cookie.HttpOnly = true;//设为httponly
            });



            //MVC模式需要配置
            IMvcBuilder mvcBuilder = services.AddControllersWithViews(options=> 
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
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            //视图中输出中文会编码
            services.Configure<WebEncoderOptions>(options =>
            {
                options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
            });

            //热编译
#if DEBUG
            if (Env.IsDevelopment())
            {
                mvcBuilder.AddRazorRuntimeCompilation();
            }
#endif
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            //注册NLOG，目前来说不需要
            //builder.Register(s => LogManager.GetCurrentClassLogger()).As<ILogger>().SingleInstance();

            builder.RegisterModule(new ServiceModules());

            builder.RegisterModule(new AutofacModule());

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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseCors("2BSite_cros");
            //初始化数据库
            InitializeDatabase();
            app.UseSession();

            app.UseResponseCompression();
            app.UseResponseCaching();
            var cachePeriod = env.IsDevelopment() ? "600" : "604800";
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    // Requires the following import:
                    // using Microsoft.AspNetCore.Http;
                    ctx.Context.Response.Headers.Append("Cache-Control", $"public, max-age={cachePeriod}");
                }
            });

            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            //添加权限验证
            app.UsePermission(new PermissionMiddlewareOption()
            {
                LoginUrl = @"/home/login",
                DeniedUrl = @"/home/denied",
                HomeUrl = @"/home/index",
                WelcomeUrl = @"/home/index",
                NoPermissionUrls = new List<string> { "/infra", "/home/modifypwd","/home/datav",
                    "/modifypwd", "/attendance/exportcheck", "/attendance/export"},
                AllowAnonymousUrls = new List<string> { "/home/logout","/tp/datav", "/logout", "/api", "/print",
                    "/pdf", "/prevalidation/index", "/prevalidation/indexs", "/prevalidation/bootstrapform","/prevalidation/preentry"}
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
        //初始化数据库
        private void InitializeDatabase()
        {
            var optionsBuilder_HRMS = new DbContextOptionsBuilder<BSiteContext>();
            optionsBuilder_HRMS.UseSqlServer(Core.Infrastructure.Global.DBRWManager.GetMaster(optionsBuilder_HRMS.Options.ContextType.ToString()));
            using (var db_hrms = new BSiteContext(optionsBuilder_HRMS.Options))
            {
                db_hrms.Database.Migrate();
            }

            var optionsBuilder = new DbContextOptionsBuilder<IdentityDataContext>();
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
                        Password = "3611388ad27022d57bbd8f3938f70615",//hrms!~@
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
