using System;

namespace UniRx
{
	public class ReadOnlyReactiveProperty<T> : IDisposable, IReadOnlyReactiveProperty<T>, IObservable<T>
	{
		private bool canPublishValueOnSubscribe;

		private bool isDisposed;

		private T value = default(T);

		private Subject<T> publisher;

		private IDisposable sourceConnection;

		public T Value
		{
			get
			{
				return this.value;
			}
		}

		public ReadOnlyReactiveProperty(IObservable<T> source)
		{
			this.publisher = new Subject<T>();
			this.sourceConnection = source.Subscribe(delegate(T x)
			{
				this.value = x;
				this.canPublishValueOnSubscribe = true;
				this.publisher.OnNext(x);
			}, new Action<Exception>(this.publisher.OnError), new Action(this.publisher.OnCompleted));
		}

		public ReadOnlyReactiveProperty(IObservable<T> source, T initialValue)
		{
			this.value = initialValue;
			this.publisher = new Subject<T>();
			this.sourceConnection = source.Subscribe(delegate(T x)
			{
				this.value = x;
				this.canPublishValueOnSubscribe = true;
				this.publisher.OnNext(x);
			}, new Action<Exception>(this.publisher.OnError), new Action(this.publisher.OnCompleted));
		}

		public IDisposable Subscribe(IObserver<T> observer)
		{
			if (this.isDisposed)
			{
				observer.OnCompleted();
				return Disposable.Empty;
			}
			if (this.publisher == null)
			{
				this.publisher = new Subject<T>();
			}
			IDisposable result = this.publisher.Subscribe(observer);
			if (this.canPublishValueOnSubscribe)
			{
				observer.OnNext(this.value);
			}
			return result;
		}

		public void Dispose()
		{
			if (!this.isDisposed)
			{
				this.isDisposed = true;
				if (this.sourceConnection != null)
				{
					this.sourceConnection.Dispose();
					this.sourceConnection = null;
				}
				if (this.publisher != null)
				{
					try
					{
						this.publisher.OnCompleted();
					}
					finally
					{
						this.publisher.Dispose();
						this.publisher = null;
					}
				}
			}
		}

		public override string ToString()
		{
			return (this.value != null) ? this.value.ToString() : "null";
		}
	}
}
