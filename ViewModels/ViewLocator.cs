using Avalonia.Controls;
using Avalonia.Controls.Templates;
using CommunityToolkit.Mvvm.ComponentModel;
using SukiUiDemo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SukiUiDemo.ViewModels
{
	public class ViewLocator() : IDataTemplate
	{
		public Control Build(object? param)
		{
			if (param is null) {
				return CreateText("Data is null.");
			}
			if (param is NavigationItem item) {
				return (Control)NavigationService.GetViewInstance(item.ViewModel);
			}
			if (param is ObservableObject) {
				return (Control)NavigationService.GetViewInstance(param.GetType());
			}
			return CreateText($"No View For {param.GetType().Name}.");
		}

		public bool Match(object? data) => data is NavigationItem;

		private static TextBlock CreateText(string text) => new TextBlock { Text = text };
	}
}
