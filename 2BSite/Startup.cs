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
            //����
            services.AddOptions();
            //ע��������
            services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));

            var jwtSettings = new JwtSettings();
            //�����õ�key�󶨵�jwtSettingsʵ��
            Configuration.Bind("JwtSettings", jwtSettings);
            //Console.WriteLine(jwtSettings.SecretKey);

            //�����ò�����ʵ��
            var redisConfiguration = Configuration.GetSection("redisConfiguration").Get<RedisConfiguration>();
            //ע��redis����ʵ��
            services.AddSingleton(redisConfiguration);
            //ע��redis����ʵ��
            services.AddSingleton<ICacheClient>(new StackExchangeRedisCacheClient(new Core.Redis.Extensions.NewtonsoftSerializer(),redisConfiguration));
            //��ȡ���ݿ����Ӷ���
            var dbServerConfiguration = Configuration.GetSection("DBServerConfiguration").Get<Core.Infrastructure.DBRW.DBServerConfiguration>();
            //ע�����ݿ����ӹ���
            Core.Infrastructure.Global.DBRWManager = new Core.Infrastructure.DBRW.DBRWManager(dbServerConfiguration);
            //�ֲ�ʽϵͳ��������ͨѶ��Ϣ����Կ
            //�൱�� asp.net�е�machine key
            //string path = Path.Combine(Directory.GetCurrentDirectory(), @"\PersistKeys\Share\hrms-keys\");
            //services.AddDataProtection(options => options.ApplicationDiscriminator = "ruyicang.com")
            //.SetApplicationName("hrms-app")
            //.ProtectKeysWithDpapi()
            //.PersistKeysToFileSystem(new DirectoryInfo(path));
            //.PersistKeysToRedis();
            //��̨����
            //services.AddBackgroundServices();
            //��¼��Ȩ��֤
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

            //��ӷ���ѹ��
            services.AddResponseCompression();
            services.AddResponseCaching();
            //��������
            services.AddCors(option =>
            {
                option.AddPolicy("2BSite_cros", policy => policy.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .WithOrigins("http://localhost:5050"));
            });
            
            //services.AddDistributedMemoryCache();
            //���session
            services.AddSession(options =>
            {
                //�������ĵײ㻺�淽��ʹ�õľ���IDistributedCache
                options.IdleTimeout = TimeSpan.FromMinutes(30); //session����ʱ��
                options.Cookie.HttpOnly = true;//��Ϊhttponly
            });



            //MVCģʽ��Ҫ����
            IMvcBuilder mvcBuilder = services.AddControllersWithViews(options=> 
            {
                //���ȫ�ִ��󷵻�
                options.Filters.Add(typeof(CustomExceptionFilterAttribute)); // an instance
            }).AddNewtonsoftJson(options =>
            {
                //���ȫ��JSON���ظ�ʽ
                options.SerializerSettings.ObjectCreationHandling = Newtonsoft.Json.ObjectCreationHandling.Replace;
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                IsoDateTimeConverter timeFormate = new IsoDateTimeConverter();
                timeFormate.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
                options.SerializerSettings.Converters.Add(timeFormate);
                options.SerializerSettings.Formatting = Formatting.Indented;
                options.SerializerSettings.NullValueHandling = NullValueHandling.Include;//�������
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            //��ͼ��������Ļ����
            services.Configure<WebEncoderOptions>(options =>
            {
                options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
            });

            //�ȱ���
#if DEBUG
            if (Env.IsDevelopment())
            {
                mvcBuilder.AddRazorRuntimeCompilation();
            }
#endif
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            //ע��NLOG��Ŀǰ��˵����Ҫ
            //builder.Register(s => LogManager.GetCurrentClassLogger()).As<ILogger>().SingleInstance();

            builder.RegisterModule(new ServiceModules());

            builder.RegisterModule(new AutofacModule());

            this.RegisterAutomapper(builder);


            //����ע��IServiceProvider ���Զ���RESOLVE����
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
            //��ʼ�����ݿ�
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

            //���Ȩ����֤
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
        //��ʼ�����ݿ�
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
