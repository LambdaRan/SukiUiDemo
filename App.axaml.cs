using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using QnDesigner.Services;
using SukiUiDemo.AppInfo;
using SukiUiDemo.Services;
using SukiUiDemo.ViewModels;
using SukiUiDemo.Views;
using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SukiUiDemo
{
    public partial class App : Application
    {
		private static readonly NLog.Logger _Logger = NLog.LogManager.GetCurrentClassLogger();
		private static readonly ServiceCollection _serviceCollection = new ServiceCollection();
		public static IServiceProvider Services => Ioc.Default;

		public IAppEnviroment? Enviroment { get; private set; }

		public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            Name = "SukiUiDemo";
			// 加载配置
			InitApp();
			if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
				desktop.MainWindow = new MainWindow();
				desktop.ShutdownRequested += DesktopOnShutdownRequested;
				desktop.Startup += OnAppStartup;
				desktop.Exit += OnAppExist;
			}
            base.OnFrameworkInitializationCompleted();
			// 初始化日志，_Logger 才有
			var log = Services.GetRequiredService<ILogger<App>>();
			log.LogInformation("OnFrameworkInitializationCompleted");
		}

        public void InitApp()
        {
			Enviroment = new AppEnviroment()
			{
				ApplicationName = Assembly.GetEntryAssembly()?.GetName().Name ?? "SukiUiDemo",
				ContentRootPath = AppContext.BaseDirectory,
			};
			_serviceCollection.AddSingleton<AppConfig>();
			_serviceCollection.AddSingleton<MessageService>();
			_serviceCollection.TryAdd(ServiceDescriptor.Singleton(typeof(ICommonServices<>), typeof(CommonServices<>)));

			// 注册窗口 AddTransient每次显示对应的View，都会创建ViewModel
			_serviceCollection.AddTransient<MainWindowViewModel>();

			NavigationService.Register<FirstViewModel, FirstView>();
			_serviceCollection.AddTransient<FirstViewModel>();
			NavigationService.Register<AboutViewModel, AboutView>();
			_serviceCollection.AddTransient<AboutViewModel>();

			var logOptions = new NLogProviderOptions();
			var logConfig = GetNLogConfiguration();
			_serviceCollection.AddLogging(builder => {
				builder.ClearProviders();
				builder.AddNLog(logConfig, logOptions);
				builder.AddFilter<NLogLoggerProvider>("Microsoft", LogLevel.Warning);
			});

			Ioc.Default.ConfigureServices(_serviceCollection.BuildServiceProvider());
		}


		private async void OnAppStartup(object? sender, ControlledApplicationLifetimeStartupEventArgs earg)
		{
			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
			// 捕获UI线程未处理的异常，要在main函数中使用try-catch
			// 注册 GBK 编码
			//Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
			var appcfg = Ioc.Default.GetRequiredService<AppConfig>();
			await appcfg.InitAsync();
			_Logger.Info("OnAppStartup");
		}

		private bool _CanClose = false;
		private async void DesktopOnShutdownRequested(object? sender, ShutdownRequestedEventArgs e)
		{
			_Logger.Info("DestopOnShutdownRequested");
			// 先取消，等保存完数据再关闭
			e.Cancel = !_CanClose;
			if (!_CanClose) {
				// To save 
				await Task.CompletedTask;
				_CanClose = true;
				if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop) {
					desktop.Shutdown();
				}
			}
		}
		private async void OnAppExist(object? sender, ControlledApplicationLifetimeExitEventArgs earg)
		{
			var appcfg = Ioc.Default.GetRequiredService<AppConfig>();
			await appcfg.SaveUserConfigAsync();
			_Logger.Info("OnAppExist");
		}

		// 捕获非UI线程未处理异常
		private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			_Logger.Error(e.ExceptionObject as Exception, "CurrentDomain_UnhandledException:");
			//string eMsg = $"{_ExceptionMessage}\r\n{e.ExceptionObject.ToString()}";
			//MessageBox.Show(eMsg, "CurrentDomain Exception", MessageBoxButton.OK, MessageBoxImage.Information);
		}

		// https://github.com/NLog/NLog/wiki/Configure-from-code
		private NLog.Config.LoggingConfiguration GetNLogConfiguration()
		{
			var config = new NLog.Config.LoggingConfiguration();
			// 文件
			var logfile = new NLog.Targets.FileTarget("logfile")
			{
				FileName = "logs/applog_${date:format=yyyy-MM-dd_HH}.txt",
				Layout = "[${longdate}|${level:uppercase=true}] ${message} ${all-event-properties} ${exception:format=tostring}",
				ArchiveEvery = NLog.Targets.FileArchivePeriod.Day,
				MaxArchiveFiles = 10,
				MaxArchiveDays = 7,
			};
			// 终端
			var logconsole = new NLog.Targets.ConsoleTarget("logconsole")
			{
				Layout = "[${longdate}|${level:uppercase=true}] ${message} ${all-event-properties} ${exception:format=tostring}}"
			};
			// Rules for mapping loggers to targets            
			config.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, logconsole);
			config.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, logfile);
			return config;
		}
	}
}