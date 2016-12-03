using System;
using UnityEngine;

namespace UniRx.Triggers
{
	[DisallowMultipleComponent]
	public class ObservableTransformChangedTrigger : ObservableTriggerBase
	{
		private Subject<Unit> onBeforeTransformParentChanged;

		private Subject<Unit> onTransformParentChanged;

		private Subject<Unit> onTransformChildrenChanged;

		private void OnBeforeTransformParentChanged()
		{
			if (this.onBeforeTransformParentChanged != null)
			{
				this.onBeforeTransformParentChanged.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnBeforeTransformParentChangedAsObservable()
		{
			Subject<Unit> arg_1B_0;
			if ((arg_1B_0 = this.onBeforeTransformParentChanged) == null)
			{
				arg_1B_0 = (this.onBeforeTransformParentChanged = new Subject<Unit>());
			}
			return arg_1B_0;
		}

		private void OnTransformParentChanged()
		{
			if (this.onTransformParentChanged != null)
			{
				this.onTransformParentChanged.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnTransformParentChangedAsObservable()
		{
			Subject<Unit> arg_1B_0;
			if ((arg_1B_0 = this.onTransformParentChanged) == null)
			{
				arg_1B_0 = (this.onTransformParentChanged = new Subject<Unit>());
			}
			return arg_1B_0;
		}

		private void OnTransformChildrenChanged()
		{
			if (this.onTransformChildrenChanged != null)
			{
				this.onTransformChildrenChanged.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnTransformChildrenChangedAsObservable()
		{
			Subject<Unit> arg_1B_0;
			if ((arg_1B_0 = this.onTransformChildrenChanged) == null)
			{
				arg_1B_0 = (this.onTransformChildrenChanged = new Subject<Unit>());
			}
			return arg_1B_0;
		}

		protected override void RaiseOnCompletedOnDestroy()
		{
			if (this.onBeforeTransformParentChanged != null)
			{
				this.onBeforeTransformParentChanged.OnCompleted();
			}
			if (this.onTransformParentChanged != null)
			{
				this.onTransformParentChanged.OnCompleted();
			}
			if (this.onTransformChildrenChanged != null)
			{
				this.onTransformChildrenChanged.OnCompleted();
			}
		}
	}
}
