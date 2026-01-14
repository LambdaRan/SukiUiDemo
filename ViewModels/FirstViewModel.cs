using CommunityToolkit.Mvvm.ComponentModel;
using SukiUiDemo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SukiUiDemo.ViewModels
{
	public partial class FirstViewModel : ViewModelBase<FirstViewModel>
	{
		public FirstViewModel(ICommonServices<FirstViewModel> commonServices)
			: base(commonServices)
		{
		}

		[ObservableProperty]
		private string _Content = "This is the First View Model.";
	}
}
