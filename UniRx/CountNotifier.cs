using System;

namespace UniRx
{
	public class CountNotifier : IObservable<CountChangedStatus>
	{
		private readonly object lockObject = new object();

		private readonly Subject<CountChangedStatus> statusChanged = new Subject<CountChangedStatus>();

		private readonly int max;

		public int Max
		{
			get
			{
				return this.max;
			}
		}

		public int Count
		{
			get;
			private set;
		}

		public CountNotifier(int max = 2147483647)
		{
			if (max <= 0)
			{
				throw new ArgumentException("max");
			}
			this.max = max;
		}

		public IDisposable Increment(int incrementCount = 1)
		{
			if (incrementCount < 0)
			{
				throw new ArgumentException("incrementCount");
			}
			object obj = this.lockObject;
			IDisposable result;
			lock (obj)
			{
				if (this.Count == this.Max)
				{
					result = Disposable.Empty;
				}
				else
				{
					if (incrementCount + this.Count > this.Max)
					{
						this.Count = this.Max;
					}
					else
					{
						this.Count += incrementCount;
					}
					this.statusChanged.OnNext(CountChangedStatus.Increment);
					if (this.Count == this.Max)
					{
						this.statusChanged.OnNext(CountChangedStatus.Max);
					}
					result = Disposable.Create(delegate
					{
						this.Decrement(incrementCount);
					});
				}
			}
			return result;
		}

		public void Decrement(int decrementCount = 1)
		{
			if (decrementCount < 0)
			{
				throw new ArgumentException("decrementCount");
			}
			object obj = this.lockObject;
			lock (obj)
			{
				if (this.Count != 0)
				{
					if (this.Count - decrementCount < 0)
					{
						this.Count = 0;
					}
					else
					{
						this.Count -= decrementCount;
					}
					this.statusChanged.OnNext(CountChangedStatus.Decrement);
					if (this.Count == 0)
					{
						this.statusChanged.OnNext(CountChangedStatus.Empty);
					}
				}
			}
		}

		public IDisposable Subscribe(IObserver<CountChangedStatus> observer)
		{
			return this.statusChanged.Subscribe(observer);
		}
	}
}
