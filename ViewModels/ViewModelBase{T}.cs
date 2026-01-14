using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using SukiUiDemo.Services;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SukiUiDemo.ViewModels;

public class ViewModelBase<TMode> : ObservableObject
{
	public ViewModelBase(ICommonServices<TMode> commonServices)
	{
		MessageService = commonServices.MessageService;
		Logger = commonServices.Logger;
	}

	public MessageService MessageService { get; }
	public ILogger<TMode> Logger { get; }

	#region logger
	public void LogDebug(string? message, params object?[] args)
	{
		Logger.LogDebug(message, args);
	}
	public void LogInformation(Exception? exception, string? message, params object?[] args)
	{
		Logger.LogInformation(exception, message, args);
	}
	public void LogInformation(string? message, params object?[] args)
	{
		Logger.LogInformation(message, args);
	}
	public void LogWarning(Exception? exception, string? message, params object?[] args)
	{
		Logger.LogWarning(exception, message, args);
	}
	public void LogWarning(string? message, params object?[] args)
	{
		Logger.LogWarning(message, args);
	}
	public void LogError(Exception? exception, string? message, params object?[] args)
	{
		Logger.LogError(exception, message, args);
	}
	public void LogError(string? message, params object?[] args)
	{
		Logger.LogError(message, args);
	}
	#endregion

	#region status message
	public virtual void Subscribe()
	{
		// Nothing
	}
	public virtual void Unsubscribe()
	{
		MessageService.UnregisterAll(this);
	}
	public void WorkStartStatusMessage(string message)
	{
		MessageService.WorkStartStatusMessage(message);
	}
	public void WorkEndStatusMessage(string message)
	{
		MessageService.WorkEndStatusMessage(message);
	}
	public void ShowStatusReady()
	{
		MessageService.ShowStatusNormalMsg("Ready");
	}
	public void ShowStatusNormalMsg(string message)
	{
		MessageService.ShowStatusNormalMsg(message);
	}
	public void ShowStatusErrorMsg(string message)
	{
		MessageService.ShowStatusErrorMsg(message);
	}
	#endregion

	#region Log message

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void ShowLog(string message) => MessageService.ShowLog(message);
	#endregion

	#region Notification message
	public void NotificationInfomation(string title, string message)
		 => MessageService.NotificationInfomation(title, message);
	public void NotificationSuccess(string title, string message)
		=> MessageService.NotificationSuccess(title, message);
	public void NotificationWarning(string title, string message)
		=> MessageService.NotificationWarning(title, message);
	public void NotificationError(string title, string message)
		=> MessageService.NotificationError(title, message);
	#endregion

	#region file/folder picker
	public async ValueTask<string> OpenFilePickerAsync(Control control, string title = "", FilePickerOpenOptions? option = null)
	{
		var topLevel = TopLevel.GetTopLevel(control);
		if (topLevel == null) {
			return string.Empty;
		}
		option = option ?? new FilePickerOpenOptions() 
		{
			AllowMultiple = false,
			Title = title,
		};
		var files = await topLevel.StorageProvider.OpenFilePickerAsync(option).ConfigureAwait(false);
		if (files is null || files.Count == 0) {
			return string.Empty;
		}
		return files[0].Path.LocalPath ?? string.Empty;
	}
	public async ValueTask<string[]?> OpenFilesPickerAsync(Control control, string title = "", FilePickerOpenOptions? option = null)
	{
		var topLevel = TopLevel.GetTopLevel(control);
		if (topLevel == null) {
			return null;
		}
		option = option ?? new FilePickerOpenOptions()
		{
			AllowMultiple = true,
			Title = title,
		};
		var files = await topLevel.StorageProvider.OpenFilePickerAsync(option).ConfigureAwait(false);
		return files?.Select(x => x.Path.LocalPath)?.ToArray();
	}
	public async ValueTask<string> OpenFolderPickerAsync(Control control, string title = "", FolderPickerOpenOptions? option = null)
	{
		var topLevel = TopLevel.GetTopLevel(control);
		if (topLevel == null) {
			return string.Empty;
		}
		option = option ?? new FolderPickerOpenOptions() 
		{
			AllowMultiple = false,
			Title = title,
		};
		var folders = await topLevel.StorageProvider.OpenFolderPickerAsync(option).ConfigureAwait(false);
		if (folders is null || folders.Count == 0) {
			return string.Empty;
		}
		return folders[0].Path.LocalPath! ?? string.Empty;
	}
	public async ValueTask<string[]?> OpenFoldersPickerAsync(Control control, string title = "", FolderPickerOpenOptions? option = null)
	{
		var topLevel = TopLevel.GetTopLevel(control);
		if (topLevel == null) {
			return null;
		}
		option = option ?? new FolderPickerOpenOptions()
		{
			AllowMultiple = true,
			Title = title,
		};
		var folders = await topLevel.StorageProvider.OpenFolderPickerAsync(option).ConfigureAwait(false);
		return folders?.Select(x => x.Path.LocalPath)?.ToArray();
	}
	#endregion
}
