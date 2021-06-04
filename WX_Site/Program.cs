using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using NLog.Web;
using Autofac.Extensions.DependencyInjection;

namespace WX_Site
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("启动Main");
#if DEBUG
                var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: false)
    .Build();
#else
            var config = new ConfigurationBuilder()
.AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
.Build();
#endif
                CreateWebHostBuilder(config, args).Build().Run();

            }
            catch (Exception ex)
            {
                //NLog: catch setup errors
                logger.Error(ex, "异常停止进程" + ex.Message);
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }
        public static IWebHostBuilder CreateWebHostBuilder(IConfigurationRoot configurationRoot, string[] args)
        {
            string weburl = configurationRoot.GetValue<string>("WebUrl");
            return WebHost.CreateDefaultBuilder(args)
                    .ConfigureServices(services => services.AddAutofac())
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseIISIntegration().UseUrls(weburl)
                    .UseStartup<Startup>().CaptureStartupErrors(true)
                    .UseNLog();
        }
    }
}
