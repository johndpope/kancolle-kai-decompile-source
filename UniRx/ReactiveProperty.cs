using System;
using UnityEngine;

namespace UniRx
{
	[Serializable]
	public class ReactiveProperty<T> : IDisposable, IReactiveProperty<T>, IReadOnlyReactiveProperty<T>, IObservable<T>
	{
		[NonSerialized]
		private bool canPublishValueOnSubscribe;

		[NonSerialized]
		private bool isDisposed;

		[SerializeField]
		private T value;

		[NonSerialized]
		private Subject<T> publisher;

		[NonSerialized]
		private IDisposable sourceConnection;

		public T Value
		{
			get
			{
				return this.value;
			}
			set
			{
				if (value == null)
				{
					if (this.value != null)
					{
						this.SetValue(value);
						this.canPublishValueOnSubscribe = true;
						if (this.isDisposed)
						{
							return;
						}
						if (this.publisher != null)
						{
							this.publisher.OnNext(this.value);
						}
					}
				}
				else if (this.value == null || !this.value.Equals(value))
				{
					this.SetValue(value);
					this.canPublishValueOnSubscribe = true;
					if (this.isDisposed)
					{
						return;
					}
					if (this.publisher != null)
					{
						this.publisher.OnNext(this.value);
					}
				}
			}
		}

		public ReactiveProperty() : this(default(T))
		{
		}

		public ReactiveProperty(T initialValue)
		{
			this.value = default(T);
			base..ctor();
			this.value = initialValue;
			this.canPublishValueOnSubscribe = true;
		}

		public ReactiveProperty(IObservable<T> source)
		{
			this.value = default(T);
			base..ctor();
			this.canPublishValueOnSubscribe = false;
			this.publisher = new Subject<T>();
			this.sourceConnection = source.Subscribe(delegate(T x)
			{
				this.Value = x;
			}, new Action<Exception>(this.publisher.OnError), new Action(this.publisher.OnCompleted));
		}

		public ReactiveProperty(IObservable<T> source, T initialValue)
		{
			this.value = default(T);
			base..ctor();
			this.canPublishValueOnSubscribe = false;
			this.Value = initialValue;
			this.publisher = new Subject<T>();
			this.sourceConnection = source.Subscribe(delegate(T x)
			{
				this.Value = x;
			}, new Action<Exception>(this.publisher.OnError), new Action(this.publisher.OnCompleted));
		}

		protected virtual void SetValue(T value)
		{
			this.value = value;
		}

		public void SetValueAndForceNotify(T value)
		{
			this.SetValue(value);
			if (this.isDisposed)
			{
				return;
			}
			if (this.publisher != null)
			{
				this.publisher.OnNext(this.value);
			}
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
