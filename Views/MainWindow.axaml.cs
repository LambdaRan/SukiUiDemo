using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.DependencyInjection;
using SukiUI.Controls;
using SukiUiDemo.ViewModels;

namespace SukiUiDemo.Views
{
    public partial class MainWindow : SukiWindow
    {
        public MainWindow()
        {
			ViewModel = Ioc.Default.GetRequiredService<MainWindowViewModel>();
			DataContext = ViewModel;
			Loaded += OnWindowLoaded;
			Unloaded += OnWindowUnloaded;
			Closing += OnWindowClosing;

			InitializeComponent();

			DataTemplates.Add(new ViewLocator());
        }

		public MainWindowViewModel ViewModel { get; private set; }
		private void OnWindowLoaded(object? sender, RoutedEventArgs e)
		{
			ViewModel.NotificationManager = new WindowNotificationManager(TopLevel.GetTopLevel(this))
			{
				Position = NotificationPosition.TopCenter,
			};
			ViewModel.Subscribe();
		}
		private void OnWindowUnloaded(object? sender, RoutedEventArgs e)
		{
			ViewModel.Unsubscribe();
			ViewModel.MessageService.UnregisterAll(this);
			ViewModel.NotificationManager = null;
		}
		private async void OnWindowClosing(object? sender, WindowClosingEventArgs e)
		{
		}
	}
}