using System;
using UnityEngine;

namespace UniRx.Triggers
{
	[DisallowMultipleComponent]
	public class ObservableRectTransformTrigger : ObservableTriggerBase
	{
		private Subject<Unit> onRectTransformDimensionsChange;

		private Subject<Unit> onRectTransformRemoved;

		public void OnRectTransformDimensionsChange()
		{
			if (this.onRectTransformDimensionsChange != null)
			{
				this.onRectTransformDimensionsChange.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnRectTransformDimensionsChangeAsObservable()
		{
			Subject<Unit> arg_1B_0;
			if ((arg_1B_0 = this.onRectTransformDimensionsChange) == null)
			{
				arg_1B_0 = (this.onRectTransformDimensionsChange = new Subject<Unit>());
			}
			return arg_1B_0;
		}

		public void OnRectTransformRemoved()
		{
			if (this.onRectTransformRemoved != null)
			{
				this.onRectTransformRemoved.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnRectTransformRemovedAsObservable()
		{
			Subject<Unit> arg_1B_0;
			if ((arg_1B_0 = this.onRectTransformRemoved) == null)
			{
				arg_1B_0 = (this.onRectTransformRemoved = new Subject<Unit>());
			}
			return arg_1B_0;
		}

		protected override void RaiseOnCompletedOnDestroy()
		{
			if (this.onRectTransformDimensionsChange != null)
			{
				this.onRectTransformDimensionsChange.OnCompleted();
			}
			if (this.onRectTransformRemoved != null)
			{
				this.onRectTransformRemoved.OnCompleted();
			}
		}
	}
}
