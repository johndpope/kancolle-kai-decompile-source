using System;

namespace UniRx
{
	public interface IMessageBroker
	{
		void Publish<T>(T message);

		IObservable<T> Receive<T>();
	}
}
