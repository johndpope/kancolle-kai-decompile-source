using System;
using System.Collections.Generic;

namespace UniRx
{
	public class MessageBroker : IDisposable, IMessageBroker
	{
		public static readonly IMessageBroker Default = new MessageBroker();

		private bool isDisposed;

		private readonly Dictionary<Type, object> notifiers = new Dictionary<Type, object>();

		public void Publish<T>(T message)
		{
			Dictionary<Type, object> dictionary = this.notifiers;
			object obj;
			lock (dictionary)
			{
				if (this.isDisposed)
				{
					return;
				}
				if (!this.notifiers.TryGetValue(typeof(T), ref obj))
				{
					return;
				}
			}
			((Subject<T>)obj).OnNext(message);
		}

		public IObservable<T> Receive<T>()
		{
			Dictionary<Type, object> dictionary = this.notifiers;
			object obj;
			lock (dictionary)
			{
				if (this.isDisposed)
				{
					throw new ObjectDisposedException("MessageBroker");
				}
				if (!this.notifiers.TryGetValue(typeof(T), ref obj))
				{
					obj = new Subject<T>();
					this.notifiers.Add(typeof(T), obj);
				}
			}
			return ((IObservable<T>)obj).AsObservable<T>();
		}

		public void Dispose()
		{
			Dictionary<Type, object> dictionary = this.notifiers;
			lock (dictionary)
			{
				if (!this.isDisposed)
				{
					this.isDisposed = true;
					this.notifiers.Clear();
				}
			}
		}
	}
}
