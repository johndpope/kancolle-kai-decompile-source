using System;

namespace UniRx.InternalUtil
{
	public class ListObserver<T> : IObserver<T>
	{
		private readonly ImmutableList<IObserver<T>> _observers;

		public ListObserver(ImmutableList<IObserver<T>> observers)
		{
			this._observers = observers;
		}

		public void OnCompleted()
		{
			IObserver<T>[] data = this._observers.Data;
			for (int i = 0; i < data.Length; i++)
			{
				data[i].OnCompleted();
			}
		}

		public void OnError(Exception error)
		{
			IObserver<T>[] data = this._observers.Data;
			for (int i = 0; i < data.Length; i++)
			{
				data[i].OnError(error);
			}
		}

		public void OnNext(T value)
		{
			IObserver<T>[] data = this._observers.Data;
			for (int i = 0; i < data.Length; i++)
			{
				data[i].OnNext(value);
			}
		}

		internal IObserver<T> Add(IObserver<T> observer)
		{
			return new ListObserver<T>(this._observers.Add(observer));
		}

		internal IObserver<T> Remove(IObserver<T> observer)
		{
			int num = Array.IndexOf<IObserver<T>>(this._observers.Data, observer);
			if (num < 0)
			{
				return this;
			}
			if (this._observers.Data.Length == 2)
			{
				return this._observers.Data[1 - num];
			}
			return new ListObserver<T>(this._observers.Remove(observer));
		}
	}
}
