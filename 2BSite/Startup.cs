using _2BSite.Service;
using Autofac;
using AutoMapper;
using Core.Redis;
using Core.Redis.Configuration;
using ExcelReport;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
            services.AddOptions();

            var redisConfiguration = Configuration.GetSection("redisConfiguration").Get<RedisConfiguration>();

            services.AddSingleton(redisConfiguration);
            services.AddSingleton<ICacheClient>(new StackExchangeRedisCacheClient(new Core.Redis.Extensions.NewtonsoftSerializer(),
                redisConfiguration));

            var dbServerConfiguration = Configuration.GetSection("DBServerConfiguration").Get<Core.Infrastructure.DBRW.DBServerConfiguration>();
            Core.Infrastructure.Global.DBRWManager = new Core.Infrastructure.DBRW.DBRWManager(dbServerConfiguration);


            //添加返回压缩
            services.AddResponseCompression();
            services.AddResponseCaching();

            services.AddCors(option =>
            {
                option.AddPolicy("2BSite_cros", policy => policy.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .WithOrigins("http://localhost:5200"));
            });

            
            //添加session
            services.AddSession(options =>
            {
                //本方法的底层缓存方法使用的就是IDistributedCache
                options.IdleTimeout = TimeSpan.FromMinutes(30); //session活期时间
                options.Cookie.HttpOnly = true;//设为httponly
            });




            IMvcBuilder mvcBuilder = services.AddControllersWithViews();
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

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
