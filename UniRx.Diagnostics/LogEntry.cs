using System;
using UnityEngine;

namespace UniRx.Diagnostics
{
	public class LogEntry
	{
		public string LoggerName
		{
			get;
			private set;
		}

		public LogType LogType
		{
			get;
			private set;
		}

		public string Message
		{
			get;
			private set;
		}

		public DateTime Timestamp
		{
			get;
			private set;
		}

		public Object Context
		{
			get;
			private set;
		}

		public Exception Exception
		{
			get;
			private set;
		}

		public string StackTrace
		{
			get;
			private set;
		}

		public object State
		{
			get;
			private set;
		}

		public LogEntry(string loggerName, LogType logType, DateTime timestamp, string message, Object context = null, Exception exception = null, string stackTrace = null, object state = null)
		{
			this.LoggerName = loggerName;
			this.LogType = logType;
			this.Timestamp = timestamp;
			this.Message = message;
			this.Context = context;
			this.Exception = exception;
			this.StackTrace = stackTrace;
			this.State = state;
		}

		public override string ToString()
		{
			string text = (this.Exception == null) ? string.Empty : (Environment.get_NewLine() + this.Exception.ToString());
			return string.Concat(new string[]
			{
				"[",
				this.Timestamp.ToString(),
				"][",
				this.LoggerName,
				"][",
				this.LogType.ToString(),
				"]",
				this.Message,
				text
			});
		}
	}
}
