using Avalonia.Platform;
using System;

namespace SukiUiDemo.NativeControls;

internal interface IWinNativeControl
{
	/// <param name="parent"></param>
	/// <param name="createDefault"></param>
	IPlatformHandle CreateControl(IPlatformHandle parent, Func<IPlatformHandle> createDefault);
}
