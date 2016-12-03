using System;
using UnityEngine;

namespace UniRx.Triggers
{
	[DisallowMultipleComponent]
	public class ObservableAnimatorTrigger : ObservableTriggerBase
	{
		private Subject<int> onAnimatorIK;

		private Subject<Unit> onAnimatorMove;

		private void OnAnimatorIK(int layerIndex)
		{
			if (this.onAnimatorIK != null)
			{
				this.onAnimatorIK.OnNext(layerIndex);
			}
		}

		public IObservable<int> OnAnimatorIKAsObservable()
		{
			Subject<int> arg_1B_0;
			if ((arg_1B_0 = this.onAnimatorIK) == null)
			{
				arg_1B_0 = (this.onAnimatorIK = new Subject<int>());
			}
			return arg_1B_0;
		}

		private void OnAnimatorMove()
		{
			if (this.onAnimatorMove != null)
			{
				this.onAnimatorMove.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnAnimatorMoveAsObservable()
		{
			Subject<Unit> arg_1B_0;
			if ((arg_1B_0 = this.onAnimatorMove) == null)
			{
				arg_1B_0 = (this.onAnimatorMove = new Subject<Unit>());
			}
			return arg_1B_0;
		}

		protected override void RaiseOnCompletedOnDestroy()
		{
			if (this.onAnimatorIK != null)
			{
				this.onAnimatorIK.OnCompleted();
			}
			if (this.onAnimatorMove != null)
			{
				this.onAnimatorMove.OnCompleted();
			}
		}
	}
}
