using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.DependencyInjection;
using SukiUiDemo.ViewModels;

namespace SukiUiDemo.Views;

public partial class AboutView : UserControl
{
	public AboutView()
	{
		ViewModel = Ioc.Default.GetRequiredService<AboutViewModel>();
		DataContext = ViewModel;

		InitializeComponent();
	}

	public AboutViewModel ViewModel { get; private set; }
}