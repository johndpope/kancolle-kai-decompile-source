using System;

namespace UniRx
{
	public interface IReactiveProperty<T> : IReadOnlyReactiveProperty<T>, IObservable<T>
	{
		T Value
		{
			get;
			set;
		}
	}
}
