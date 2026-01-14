using Avalonia.Controls.Notifications;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QnDesigner.Services;
using SukiUI.Controls;
using SukiUiDemo.Services;
using SukiUiDemo.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SukiUiDemo.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase<MainWindowViewModel>
    {
		private readonly AppConfig _AppCfg;
		public MainWindowViewModel(ICommonServices<MainWindowViewModel> commonServices, AppConfig appCfg)
			:base(commonServices)
		{
			_AppCfg = appCfg;
		}

		#region Navigation
		private List<NavigationItem> _Items = new List<NavigationItem>()
		{
			new NavigationItem("第一", typeof(FirstViewModel)),
			new NavigationItem("关于", typeof(AboutViewModel)),
		};
		public IEnumerable<NavigationItem> Items {
			get => _Items.AsEnumerable();
		}

		[ObservableProperty]
		private object? _ActivePage;

		#endregion

		[RelayCommand]
		private static void OpenUrl(string url) => MiscUtils.OpenUrl(url);

		#region 消息订阅
		public override void Subscribe()
		{
			base.Subscribe();
			MessageService.Register<MainWindowViewModel, StatusMessage>(this, (r, m) => {
				Dispatcher.UIThread.Post(() => {
					r.IsError = m.IsError;
					r.StatusMessage = m.Status;
					if (m.IsToLog) {
						r.AddLog(m.Status);
					}
				});
			});
			MessageService.Register<MainWindowViewModel, Notification>(this, (r, m) => {
				Dispatcher.UIThread.Post(() => { r.NotificationManager?.Show(m); });
			});
			MessageService.Register<MainWindowViewModel, LogMessage>(this, (r, m) => {
				Dispatcher.UIThread.Post(() => { r.AddLog(m.Log); });
			});
		}
		public override void Unsubscribe()
		{
			base.Unsubscribe();
		}
		#endregion

		#region Status
		[ObservableProperty]
		private bool _IsError = false;

		[ObservableProperty]
		private string _StatusMessage = "Ready";
		#endregion

		#region Log
		public int MaxLogLine => 100;
		//public TextEditor? TextEditor { get; set; }

		public void AddLog(string message)
		{
		}
		#endregion

		#region Notification
		public WindowNotificationManager? NotificationManager { get; set; }
		#endregion
	}
}
