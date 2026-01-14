using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace SukiUiDemo.Common;

public static class SukiUiDemoRun
{
	public static EnsureThreadPool SwitchToThreadpool()
	{
		return new EnsureThreadPool();
	}
}

public struct EnsureThreadPool : INotifyCompletion
{
	static readonly WaitCallback Callback = (state) => ((Action)state!).Invoke();

	[EditorBrowsable(EditorBrowsableState.Never)]
	public EnsureThreadPool GetAwaiter()
	{
		return this;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool IsCompleted => Thread.CurrentThread.IsThreadPoolThread;

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void GetResult()
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void OnCompleted(Action continuation)
	{
#if NET5_0_OR_GREATER || NETCOREAPP3_0_OR_GREATER
		ThreadPool.UnsafeQueueUserWorkItem(ThreadSwitcherWorkItem.GetOrCreate(continuation), false);
#else
            ThreadPool.UnsafeQueueUserWorkItem(Callback, continuation);
#endif
	}
}
