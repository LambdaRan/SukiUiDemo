using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.DependencyInjection;
using SukiUiDemo.ViewModels;

namespace SukiUiDemo.Views;

public partial class FirstView : UserControl
{
    public FirstView()
    {
        ViewModel = Ioc.Default.GetRequiredService<FirstViewModel>();
        DataContext = ViewModel;

		InitializeComponent();
    }

    public FirstViewModel ViewModel { get; private set; }
}