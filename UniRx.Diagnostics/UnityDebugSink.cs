using System;
using UnityEngine;

namespace UniRx.Diagnostics
{
	public class UnityDebugSink : IObserver<LogEntry>
	{
		public void OnCompleted()
		{
		}

		public void OnError(Exception error)
		{
		}

		public void OnNext(LogEntry value)
		{
			if (value == null)
			{
				return;
			}
			object context = value.Context;
			switch (value.LogType)
			{
			case 0:
				if (context == null)
				{
					Debug.LogError(value.Message);
				}
				else
				{
					Debug.LogError(value.Message, value.Context);
				}
				break;
			case 2:
				if (context == null)
				{
					Debug.LogWarning(value.Message);
				}
				else
				{
					Debug.LogWarning(value.Message, value.Context);
				}
				break;
			case 3:
				if (context == null)
				{
					Debug.Log(value.Message);
				}
				else
				{
					Debug.Log(value.Message, value.Context);
				}
				break;
			case 4:
				if (context == null)
				{
					Debug.LogException(value.Exception);
				}
				else
				{
					Debug.LogException(value.Exception, value.Context);
				}
				break;
			}
		}
	}
}
