using System;

namespace UniRx
{
	public interface IScheduler
	{
		DateTimeOffset Now
		{
			get;
		}

		IDisposable Schedule(Action action);

		IDisposable Schedule(TimeSpan dueTime, Action action);
	}
}
