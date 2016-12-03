using System;
using UniRx.InternalUtil;

namespace UniRx
{
	public sealed class Subject<T> : IDisposable, IObserver<T>, ISubject<T>, ISubject<T, T>, IObservable<T>
	{
		private class Subscription : IDisposable
		{
			private readonly object gate = new object();

			private Subject<T> parent;

			private IObserver<T> unsubscribeTarget;

			public Subscription(Subject<T> parent, IObserver<T> unsubscribeTarget)
			{
				this.parent = parent;
				this.unsubscribeTarget = unsubscribeTarget;
			}

			public void Dispose()
			{
				object obj = this.gate;
				lock (obj)
				{
					if (this.parent != null)
					{
						object observerLock = this.parent.observerLock;
						lock (observerLock)
						{
							ListObserver<T> listObserver = this.parent.outObserver as ListObserver<T>;
							if (listObserver != null)
							{
								this.parent.outObserver = listObserver.Remove(this.unsubscribeTarget);
							}
							else
							{
								this.parent.outObserver = new EmptyObserver<T>();
							}
							this.unsubscribeTarget = null;
							this.parent = null;
						}
					}
				}
			}
		}

		private object observerLock = new object();

		private bool isStopped;

		private bool isDisposed;

		private Exception lastError;

		private IObserver<T> outObserver = new EmptyObserver<T>();

		public bool HasObservers
		{
			get
			{
				return !(this.outObserver is EmptyObserver<T>) && !this.isStopped && !this.isDisposed;
			}
		}

		public void OnCompleted()
		{
			object obj = this.observerLock;
			IObserver<T> observer;
			lock (obj)
			{
				this.ThrowIfDisposed();
				if (this.isStopped)
				{
					return;
				}
				observer = this.outObserver;
				this.outObserver = new EmptyObserver<T>();
				this.isStopped = true;
			}
			observer.OnCompleted();
		}

		public void OnError(Exception error)
		{
			if (error == null)
			{
				throw new ArgumentNullException("error");
			}
			object obj = this.observerLock;
			IObserver<T> observer;
			lock (obj)
			{
				this.ThrowIfDisposed();
				if (this.isStopped)
				{
					return;
				}
				observer = this.outObserver;
				this.outObserver = new EmptyObserver<T>();
				this.isStopped = true;
				this.lastError = error;
			}
			observer.OnError(error);
		}

		public void OnNext(T value)
		{
			this.outObserver.OnNext(value);
		}

		public IDisposable Subscribe(IObserver<T> observer)
		{
			if (observer == null)
			{
				throw new ArgumentNullException("observer");
			}
			Exception ex = null;
			object obj = this.observerLock;
			lock (obj)
			{
				this.ThrowIfDisposed();
				if (!this.isStopped)
				{
					ListObserver<T> listObserver = this.outObserver as ListObserver<T>;
					if (listObserver != null)
					{
						this.outObserver = listObserver.Add(observer);
					}
					else
					{
						IObserver<T> observer2 = this.outObserver;
						if (observer2 is EmptyObserver<T>)
						{
							this.outObserver = observer;
						}
						else
						{
							this.outObserver = new ListObserver<T>(new ImmutableList<IObserver<T>>(new IObserver<T>[]
							{
								observer2,
								observer
							}));
						}
					}
					return new Subject<T>.Subscription(this, observer);
				}
				ex = this.lastError;
			}
			if (ex != null)
			{
				observer.OnError(ex);
			}
			else
			{
				observer.OnCompleted();
			}
			return Disposable.Empty;
		}

		public void Dispose()
		{
			object obj = this.observerLock;
			lock (obj)
			{
				this.isDisposed = true;
				this.outObserver = new DisposedObserver<T>();
			}
		}

		private void ThrowIfDisposed()
		{
			if (this.isDisposed)
			{
				throw new ObjectDisposedException(string.Empty);
			}
		}
	}
}
