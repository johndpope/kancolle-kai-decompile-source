using System;

namespace UniRx
{
	public class ScheduledNotifier<T> : IObservable<T>, IProgress<T>
	{
		private readonly IScheduler scheduler;

		private readonly Subject<T> trigger = new Subject<T>();

		public ScheduledNotifier()
		{
			this.scheduler = Scheduler.DefaultSchedulers.ConstantTimeOperations;
		}

		public ScheduledNotifier(IScheduler scheduler)
		{
			if (scheduler == null)
			{
				throw new ArgumentNullException("scheduler");
			}
			this.scheduler = scheduler;
		}

		public void Report(T value)
		{
			this.scheduler.Schedule(delegate
			{
				this.trigger.OnNext(value);
			});
		}

		public IDisposable Report(T value, TimeSpan dueTime)
		{
			return this.scheduler.Schedule(dueTime, delegate
			{
				this.trigger.OnNext(value);
			});
		}

		public IDisposable Report(T value, DateTimeOffset dueTime)
		{
			return this.scheduler.Schedule(dueTime, delegate
			{
				this.trigger.OnNext(value);
			});
		}

		public IDisposable Subscribe(IObserver<T> observer)
		{
			if (observer == null)
			{
				throw new ArgumentNullException("observer");
			}
			return this.trigger.Subscribe(observer);
		}
	}
}
