using System;

namespace UniRx
{
	public interface IEventPattern<TSender, TEventArgs>
	{
		TSender Sender
		{
			get;
		}

		TEventArgs EventArgs
		{
			get;
		}
	}
}
