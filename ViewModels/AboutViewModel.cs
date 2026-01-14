using CommunityToolkit.Mvvm.ComponentModel;
using SukiUiDemo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SukiUiDemo.ViewModels
{
	public partial class AboutViewModel : ViewModelBase<AboutViewModel>
	{
		public AboutViewModel(ICommonServices<AboutViewModel> commonServices)
			: base(commonServices)
		{
		}

		[ObservableProperty]
		private string _AppVersion = "This is AboutView. AppVersion:0.0.1";
	}
}
