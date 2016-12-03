using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace UniRx
{
	public static class ObserveExtensions
	{
		public static IObservable<TProperty> ObserveEveryValueChanged<TSource, TProperty>(this TSource source, Func<TSource, TProperty> propertySelector, FrameCountType frameCountType = FrameCountType.Update) where TSource : class
		{
			ObserveExtensions.<ObserveEveryValueChanged>c__AnonStorey2EF<TSource, TProperty> <ObserveEveryValueChanged>c__AnonStorey2EF = new ObserveExtensions.<ObserveEveryValueChanged>c__AnonStorey2EF<TSource, TProperty>();
			<ObserveEveryValueChanged>c__AnonStorey2EF.propertySelector = propertySelector;
			<ObserveEveryValueChanged>c__AnonStorey2EF.frameCountType = frameCountType;
			if (source == null)
			{
				return Observable.Empty<TProperty>();
			}
			<ObserveEveryValueChanged>c__AnonStorey2EF.unityObject = (source as Object);
			bool flag = source is Object;
			if (flag && <ObserveEveryValueChanged>c__AnonStorey2EF.unityObject == null)
			{
				return Observable.Empty<TProperty>();
			}
			if (flag)
			{
				return Observable.FromCoroutine<TProperty>((IObserver<TProperty> observer, CancellationToken cancellationToken) => ObserveExtensions.PublishUnityObjectValueChanged<TSource, TProperty>(<ObserveEveryValueChanged>c__AnonStorey2EF.unityObject, <ObserveEveryValueChanged>c__AnonStorey2EF.propertySelector, <ObserveEveryValueChanged>c__AnonStorey2EF.frameCountType, observer, cancellationToken));
			}
			WeakReference reference = new WeakReference(source);
			source = (TSource)((object)null);
			return Observable.FromCoroutine<TProperty>((IObserver<TProperty> observer, CancellationToken cancellationToken) => ObserveExtensions.PublishPocoValueChanged<TSource, TProperty>(reference, <ObserveEveryValueChanged>c__AnonStorey2EF.propertySelector, <ObserveEveryValueChanged>c__AnonStorey2EF.frameCountType, observer, cancellationToken));
		}

		[DebuggerHidden]
		private static IEnumerator PublishPocoValueChanged<TSource, TProperty>(WeakReference sourceReference, Func<TSource, TProperty> propertySelector, FrameCountType frameCountType, IObserver<TProperty> observer, CancellationToken cancellationToken)
		{
			ObserveExtensions.<PublishPocoValueChanged>c__Iterator21<TSource, TProperty> <PublishPocoValueChanged>c__Iterator = new ObserveExtensions.<PublishPocoValueChanged>c__Iterator21<TSource, TProperty>();
			<PublishPocoValueChanged>c__Iterator.cancellationToken = cancellationToken;
			<PublishPocoValueChanged>c__Iterator.sourceReference = sourceReference;
			<PublishPocoValueChanged>c__Iterator.propertySelector = propertySelector;
			<PublishPocoValueChanged>c__Iterator.observer = observer;
			<PublishPocoValueChanged>c__Iterator.frameCountType = frameCountType;
			<PublishPocoValueChanged>c__Iterator.<$>cancellationToken = cancellationToken;
			<PublishPocoValueChanged>c__Iterator.<$>sourceReference = sourceReference;
			<PublishPocoValueChanged>c__Iterator.<$>propertySelector = propertySelector;
			<PublishPocoValueChanged>c__Iterator.<$>observer = observer;
			<PublishPocoValueChanged>c__Iterator.<$>frameCountType = frameCountType;
			return <PublishPocoValueChanged>c__Iterator;
		}

		[DebuggerHidden]
		private static IEnumerator PublishUnityObjectValueChanged<TSource, TProperty>(Object unityObject, Func<TSource, TProperty> propertySelector, FrameCountType frameCountType, IObserver<TProperty> observer, CancellationToken cancellationToken)
		{
			ObserveExtensions.<PublishUnityObjectValueChanged>c__Iterator22<TSource, TProperty> <PublishUnityObjectValueChanged>c__Iterator = new ObserveExtensions.<PublishUnityObjectValueChanged>c__Iterator22<TSource, TProperty>();
			<PublishUnityObjectValueChanged>c__Iterator.unityObject = unityObject;
			<PublishUnityObjectValueChanged>c__Iterator.cancellationToken = cancellationToken;
			<PublishUnityObjectValueChanged>c__Iterator.propertySelector = propertySelector;
			<PublishUnityObjectValueChanged>c__Iterator.observer = observer;
			<PublishUnityObjectValueChanged>c__Iterator.frameCountType = frameCountType;
			<PublishUnityObjectValueChanged>c__Iterator.<$>unityObject = unityObject;
			<PublishUnityObjectValueChanged>c__Iterator.<$>cancellationToken = cancellationToken;
			<PublishUnityObjectValueChanged>c__Iterator.<$>propertySelector = propertySelector;
			<PublishUnityObjectValueChanged>c__Iterator.<$>observer = observer;
			<PublishUnityObjectValueChanged>c__Iterator.<$>frameCountType = frameCountType;
			return <PublishUnityObjectValueChanged>c__Iterator;
		}
	}
}
