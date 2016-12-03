using System;
using UnityEngine;

namespace UniRx.Triggers
{
	public abstract class ObservableTriggerBase : MonoBehaviour
	{
		private bool calledAwake;

		private Subject<Unit> awake;

		private bool calledStart;

		private Subject<Unit> start;

		private bool calledDestroy;

		private Subject<Unit> onDestroy;

		private void Awake()
		{
			this.calledAwake = true;
			if (this.awake != null)
			{
				this.awake.OnNext(Unit.Default);
				this.awake.OnCompleted();
			}
		}

		public IObservable<Unit> AwakeAsObservable()
		{
			if (this.calledAwake)
			{
				return Observable.Return<Unit>(Unit.Default);
			}
			Subject<Unit> arg_31_0;
			if ((arg_31_0 = this.awake) == null)
			{
				arg_31_0 = (this.awake = new Subject<Unit>());
			}
			return arg_31_0;
		}

		private void Start()
		{
			this.calledStart = true;
			if (this.start != null)
			{
				this.start.OnNext(Unit.Default);
				this.start.OnCompleted();
			}
		}

		public IObservable<Unit> StartAsObservable()
		{
			if (this.calledStart)
			{
				return Observable.Return<Unit>(Unit.Default);
			}
			Subject<Unit> arg_31_0;
			if ((arg_31_0 = this.start) == null)
			{
				arg_31_0 = (this.start = new Subject<Unit>());
			}
			return arg_31_0;
		}

		private void OnDestroy()
		{
			this.calledDestroy = true;
			if (this.onDestroy != null)
			{
				this.onDestroy.OnNext(Unit.Default);
				this.onDestroy.OnCompleted();
			}
			this.RaiseOnCompletedOnDestroy();
		}

		public IObservable<Unit> OnDestroyAsObservable()
		{
			if (this == null)
			{
				return Observable.Return<Unit>(Unit.Default);
			}
			if (this.calledDestroy)
			{
				return Observable.Return<Unit>(Unit.Default);
			}
			Subject<Unit> arg_48_0;
			if ((arg_48_0 = this.onDestroy) == null)
			{
				arg_48_0 = (this.onDestroy = new Subject<Unit>());
			}
			return arg_48_0;
		}

		protected abstract void RaiseOnCompletedOnDestroy();
	}
}
