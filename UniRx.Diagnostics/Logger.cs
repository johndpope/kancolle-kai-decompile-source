using System;
using UnityEngine;

namespace UniRx.Diagnostics
{
	public class Logger
	{
		private static bool isInitialized;

		private static bool isDebugBuild;

		protected readonly Action<LogEntry> logPublisher;

		public string Name
		{
			get;
			private set;
		}

		public Logger(string loggerName)
		{
			this.Name = loggerName;
			this.logPublisher = ObservableLogger.RegisterLogger(this);
		}

		public virtual void Debug(object message, Object context = null)
		{
			if (!Logger.isInitialized)
			{
				Logger.isInitialized = true;
				Logger.isDebugBuild = UnityEngine.Debug.get_isDebugBuild();
			}
			if (Logger.isDebugBuild)
			{
				Action<LogEntry> arg_57_0 = this.logPublisher;
				string message2 = (message == null) ? string.Empty : message.ToString();
				arg_57_0.Invoke(new LogEntry(this.Name, 3, DateTime.get_Now(), message2, context, null, null, null));
			}
		}

		public virtual void DebugFormat(string format, params object[] args)
		{
			if (!Logger.isInitialized)
			{
				Logger.isInitialized = true;
				Logger.isDebugBuild = UnityEngine.Debug.get_isDebugBuild();
			}
			if (Logger.isDebugBuild)
			{
				Action<LogEntry> arg_58_0 = this.logPublisher;
				string message = (format == null) ? string.Empty : string.Format(format, args);
				arg_58_0.Invoke(new LogEntry(this.Name, 3, DateTime.get_Now(), message, null, null, null, null));
			}
		}

		public virtual void Log(object message, Object context = null)
		{
			Action<LogEntry> arg_33_0 = this.logPublisher;
			string message2 = (message == null) ? string.Empty : message.ToString();
			arg_33_0.Invoke(new LogEntry(this.Name, 3, DateTime.get_Now(), message2, context, null, null, null));
		}

		public virtual void LogFormat(string format, params object[] args)
		{
			Action<LogEntry> arg_34_0 = this.logPublisher;
			string message = (format == null) ? string.Empty : string.Format(format, args);
			arg_34_0.Invoke(new LogEntry(this.Name, 3, DateTime.get_Now(), message, null, null, null, null));
		}

		public virtual void Warning(object message, Object context = null)
		{
			Action<LogEntry> arg_33_0 = this.logPublisher;
			string message2 = (message == null) ? string.Empty : message.ToString();
			arg_33_0.Invoke(new LogEntry(this.Name, 2, DateTime.get_Now(), message2, context, null, null, null));
		}

		public virtual void WarningFormat(string format, params object[] args)
		{
			Action<LogEntry> arg_34_0 = this.logPublisher;
			string message = (format == null) ? string.Empty : string.Format(format, args);
			arg_34_0.Invoke(new LogEntry(this.Name, 2, DateTime.get_Now(), message, null, null, null, null));
		}

		public virtual void Error(object message, Object context = null)
		{
			Action<LogEntry> arg_33_0 = this.logPublisher;
			string message2 = (message == null) ? string.Empty : message.ToString();
			arg_33_0.Invoke(new LogEntry(this.Name, 0, DateTime.get_Now(), message2, context, null, null, null));
		}

		public virtual void ErrorFormat(string format, params object[] args)
		{
			Action<LogEntry> arg_34_0 = this.logPublisher;
			string message = (format == null) ? string.Empty : string.Format(format, args);
			arg_34_0.Invoke(new LogEntry(this.Name, 0, DateTime.get_Now(), message, null, null, null, null));
		}

		public virtual void Exception(Exception exception, Object context = null)
		{
			Action<LogEntry> arg_33_0 = this.logPublisher;
			string message = (exception == null) ? string.Empty : exception.ToString();
			arg_33_0.Invoke(new LogEntry(this.Name, 4, DateTime.get_Now(), message, context, exception, null, null));
		}

		public virtual void Raw(LogEntry logEntry)
		{
			if (logEntry != null)
			{
				this.logPublisher.Invoke(logEntry);
			}
		}
	}
}
