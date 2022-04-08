using Microsoft.Extensions.DependencyInjection;
using NLog;
using System;
using System.Windows;

namespace Common.Apps.Services
{

    public class InitServiceOption
    {
        public string? AppName { get; set; }

    }

    //https://docs.microsoft.com/zh-cn/windows/communitytoolkit/mvvm/Ioc
    public class IocService
    {
        public IocService()
        {

        }
        public static void Init(InitServiceOption serviceOption, ServiceCollection services)
        {
            Services = ConfigureServices(serviceOption, services);
        }

#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        public static IServiceProvider Services { get; private set; }
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

        private static IServiceProvider ConfigureServices(InitServiceOption serviceOption, ServiceCollection services)
        {
            services.AddSingleton(serviceOption);

            services.AddSingleton(LogManager.GetCurrentClassLogger());
            services.AddSingleton<ConfigService>();
            services.AddSingleton<AppService>();


            //services.AddTransient<MainViewModel>();
            //services.AddTransient<ProjectsViewModel>();
            //services.AddSingleton<ISettingsService, SettingsService>();
            //services.AddSingleton<IClipboardService, ClipboardService>();
            //services.AddSingleton<IShareService, ShareService>();
            //services.AddSingleton<IEmailService, EmailService>();

            return services.BuildServiceProvider();
        }
    }
}
