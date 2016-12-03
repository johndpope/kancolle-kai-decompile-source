using System;
using System.Collections.Generic;
using UniRx.Triggers;
using UnityEngine;

namespace UniRx
{
	public static class DisposableExtensions
	{
		public static T AddTo<T>(this T disposable, ICollection<IDisposable> container) where T : IDisposable
		{
			if (disposable == null)
			{
				throw new ArgumentNullException("disposable");
			}
			if (container == null)
			{
				throw new ArgumentNullException("container");
			}
			container.Add(disposable);
			return disposable;
		}

		public static IDisposable AddTo(this IDisposable disposable, GameObject gameObject)
		{
			if (gameObject == null)
			{
				disposable.Dispose();
				return disposable;
			}
			ObservableDestroyTrigger observableDestroyTrigger = gameObject.GetComponent<ObservableDestroyTrigger>();
			if (observableDestroyTrigger == null)
			{
				observableDestroyTrigger = gameObject.AddComponent<ObservableDestroyTrigger>();
			}
			observableDestroyTrigger.OnDestroyAsObservable().Subscribe(delegate(Unit _)
			{
				disposable.Dispose();
			});
			return disposable;
		}

		public static IDisposable AddTo(this IDisposable disposable, Component gameObjectComponent)
		{
			if (gameObjectComponent == null)
			{
				disposable.Dispose();
				return disposable;
			}
			return disposable.AddTo(gameObjectComponent.get_gameObject());
		}

		public static IDisposable AddTo(this IDisposable disposable, ICollection<IDisposable> container, GameObject gameObject)
		{
			return disposable.AddTo(container).AddTo(gameObject);
		}

		public static IDisposable AddTo(this IDisposable disposable, ICollection<IDisposable> container, Component gameObjectComponent)
		{
			return disposable.AddTo(container).AddTo(gameObjectComponent);
		}
	}
}
