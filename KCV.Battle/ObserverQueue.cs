using System;
using System.Collections.Generic;

namespace KCV.Battle
{
	public class ObserverQueue<T> : IDisposable
	{
		public const int OBSERVER_DEFAULT_NUM = 32;

		private Queue<T> _queObserver;

		protected Queue<T> observerQueue
		{
			get
			{
				return this._queObserver;
			}
			private set
			{
				this._queObserver = value;
			}
		}

		public int Count
		{
			get
			{
				return this._queObserver.get_Count();
			}
		}

		public ObserverQueue()
		{
			this.Init(32);
		}

		public ObserverQueue(int nObserverDefaultNum)
		{
			this.Init(nObserverDefaultNum);
		}

		public void Dispose()
		{
			this.Dispose(true);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
			}
			Mem.DelQueueSafe<T>(ref this._queObserver);
		}

		private bool Init(int nObserverDefaultNum)
		{
			this.observerQueue = new Queue<T>();
			return true;
		}

		public virtual ObserverQueue<T> Register(T item)
		{
			this.observerQueue.Enqueue(item);
			return this;
		}

		public virtual ObserverQueue<T> Register(params T[] items)
		{
			for (int i = 0; i < items.Length; i++)
			{
				this.observerQueue.Enqueue(items[i]);
			}
			return this;
		}

		public virtual ObserverQueue<T> Register(List<T> items)
		{
			for (int i = 0; i < items.get_Count(); i++)
			{
				this.observerQueue.Enqueue(items.get_Item(i));
			}
			return this;
		}

		public virtual T DeRegister()
		{
			return this.observerQueue.Dequeue();
		}

		public T Peek()
		{
			return this.observerQueue.Peek();
		}

		public void Clear()
		{
			this.observerQueue.Clear();
		}

		public bool Contains(T item)
		{
			return this.observerQueue.Contains(item);
		}
	}
}
