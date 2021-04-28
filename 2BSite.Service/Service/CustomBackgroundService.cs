using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _2BSite.Service.Service
{
    /// <summary>
    /// 后台定时任务
    /// </summary>
    public class CustomBackgroundService : BackgroundService
    {
        //需要在Startup.cs 里面注册
        //public void ConfigureServices(IServiceCollection services)
        //{
        //    services.AddHostedService<CustomBackgroundService>();
        //    或者
        //    services.AddTransient<IHostedService, CustomBackgroundService>();
        //}
        private readonly ILogger _logger;
        private Timer _timer;//定时器
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        public CustomBackgroundService(ILogger<CustomBackgroundService> logger)
        {
            _logger = logger;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("定时任务启动");
            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    //执行任务 _logger.LogInformation("TimedServiceA DoWork"); 
            //    await Task.Delay(5000, stoppingToken); //延迟暂停5秒 
            //}
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(30));

            _logger.LogInformation("定时任务结束");

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            //_keyValuePairs.Clear();

            _logger.LogInformation("定时清空");
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("定时器停止");
            _timer?.Change(Timeout.Infinite, 0);
            return base.StopAsync(cancellationToken);
        }
        public override void Dispose()
        {
            _timer?.Dispose();
            base.Dispose();
        }
    }

    /// <summary>
    /// 帮助类
    /// </summary>
    public static class BackgroundServicesHelper
    {
        /// <summary> 
        /// 反射取得所有的业务逻辑类
        /// </summary> 
        private static Type[] GetAllChildClass(Type baseType)
        {

            var types = AppDomain.CurrentDomain.GetAssemblies()
                //取得实现了某个接口的类 
                //.SelectMany(a => a.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(ISecurity))))  .ToArray(); 
                //取得继承了某个类的所有子类 
                .SelectMany(a => a.GetTypes().Where(t => t.BaseType == baseType)).ToArray();
            return types;
        }
        /// <summary>
        /// 获取后台服务实现
        /// </summary>
        /// <returns></returns>
        public static Type[] GetAllBackgroundServices()
        {
            return GetAllChildClass(typeof(BackgroundService));
        }
        /// <summary>
        /// 自动增加后台任务.所有继承自BackgroundService的类都会自动运行
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddBackgroundServices(this IServiceCollection services)
        {
            //services.AddHostedService<CustomBackgroundService>(); 
            //asp.net core 应该是这个. 
            //或者  单为方便循环自动创建, 所以改成使用AddTransient 也一样可以使用. 
            //services.AddTransient<IHostedService, CustomBackgroundService>();  
            //services.AddTransient(typeof(Microsoft.Extensions.Hosting.IHostedService),backtype); 
            //var backtypes = BackgroundServicesHelper.GetAllBackgroundService(); 
            //foreach (var backtype in backtypes) 
            //{ 
            //    services.AddTransient(typeof(Microsoft.Extensions.Hosting.IHostedService),backtype); 
            //} 

            var backtypes = GetAllBackgroundServices();
            foreach (var backtype in backtypes)
            {
                services.AddTransient(typeof(Microsoft.Extensions.Hosting.IHostedService), backtype);
            }
            return services;
        }
    }
}
