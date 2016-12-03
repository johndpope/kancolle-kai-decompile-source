using System;
using System.Threading;

namespace UniRx
{
	public sealed class ScheduledDisposable : IDisposable, ICancelable
	{
		private readonly IScheduler scheduler;

		private volatile IDisposable disposable;

		private int isDisposed;

		public IScheduler Scheduler
		{
			get
			{
				return this.scheduler;
			}
		}

		public IDisposable Disposable
		{
			get
			{
				return this.disposable;
			}
		}

		public bool IsDisposed
		{
			get
			{
				return this.isDisposed != 0;
			}
		}

		public ScheduledDisposable(IScheduler scheduler, IDisposable disposable)
		{
			this.scheduler = scheduler;
			this.disposable = disposable;
		}

		public void Dispose()
		{
			this.Scheduler.Schedule(new Action(this.DisposeInner));
		}

		private void DisposeInner()
		{
			if (Interlocked.Increment(ref this.isDisposed) == 0)
			{
				this.disposable.Dispose();
			}
		}
	}
}
