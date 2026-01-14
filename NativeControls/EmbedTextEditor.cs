using Avalonia.Platform;
using System;

namespace SukiUiDemo.NativeControls;
internal sealed class EmbedTextEditor : IWinNativeControl
{
	public EmbedTextEditor() {
		//Editor = new TextEditor();
	}
	public IPlatformHandle CreateControl(IPlatformHandle parent, Func<IPlatformHandle> createDefault)
	{
		throw new NotImplementedException();
	}
	//public TextEditor Editor { get; private set; }
	//public IPlatformHandle CreateControl(IPlatformHandle parent, Func<IPlatformHandle> createDefault)
	//{
	//	return new WpfNativeControlHandle(Editor, "WpfTextEditor");
	//}
}
