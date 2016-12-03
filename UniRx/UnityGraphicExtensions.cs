using System;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UniRx
{
	public static class UnityGraphicExtensions
	{
		public static IObservable<Unit> DirtyLayoutCallbackAsObservable(this Graphic graphic)
		{
			return Observable.Create<Unit>(delegate(IObserver<Unit> observer)
			{
				UnityAction registerHandler = delegate
				{
					observer.OnNext(Unit.Default);
				};
				graphic.RegisterDirtyLayoutCallback(registerHandler);
				return Disposable.Create(delegate
				{
					graphic.UnregisterDirtyLayoutCallback(registerHandler);
				});
			});
		}

		public static IObservable<Unit> DirtyMaterialCallbackAsObservable(this Graphic graphic)
		{
			return Observable.Create<Unit>(delegate(IObserver<Unit> observer)
			{
				UnityAction registerHandler = delegate
				{
					observer.OnNext(Unit.Default);
				};
				graphic.RegisterDirtyMaterialCallback(registerHandler);
				return Disposable.Create(delegate
				{
					graphic.UnregisterDirtyMaterialCallback(registerHandler);
				});
			});
		}

		public static IObservable<Unit> DirtyVerticesCallbackAsObservable(this Graphic graphic)
		{
			return Observable.Create<Unit>(delegate(IObserver<Unit> observer)
			{
				UnityAction registerHandler = delegate
				{
					observer.OnNext(Unit.Default);
				};
				graphic.RegisterDirtyVerticesCallback(registerHandler);
				return Disposable.Create(delegate
				{
					graphic.UnregisterDirtyVerticesCallback(registerHandler);
				});
			});
		}
	}
}
