using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SukiUiDemo.ViewModels
{
	public class NavigationItem(string label, Type viewModel)
	{
		public string Label { get; private set; } = label;
		public Type ViewModel { get; private set; } = viewModel;

		public NavigationItem(Type viewModel) : this(string.Empty, viewModel)
		{
		}
	}
}
