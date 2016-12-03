using System;

namespace UniRx.Diagnostics
{
	public class ObservableLogger : IObservable<LogEntry>
	{
		private static readonly Subject<LogEntry> logPublisher = new Subject<LogEntry>();

		public static readonly ObservableLogger Listener = new ObservableLogger();

		private ObservableLogger()
		{
		}

		public static Action<LogEntry> RegisterLogger(Logger logger)
		{
			if (logger.Name == null)
			{
				throw new ArgumentNullException("logger.Name is null");
			}
			return new Action<LogEntry>(ObservableLogger.logPublisher.OnNext);
		}

		public IDisposable Subscribe(IObserver<LogEntry> observer)
		{
			return ObservableLogger.logPublisher.Subscribe(observer);
		}
	}
}
