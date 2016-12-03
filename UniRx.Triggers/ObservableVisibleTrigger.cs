using System;
using UnityEngine;

namespace UniRx.Triggers
{
	[DisallowMultipleComponent]
	public class ObservableVisibleTrigger : ObservableTriggerBase
	{
		private Subject<Unit> onBecameInvisible;

		private Subject<Unit> onBecameVisible;

		private void OnBecameInvisible()
		{
			if (this.onBecameInvisible != null)
			{
				this.onBecameInvisible.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnBecameInvisibleAsObservable()
		{
			Subject<Unit> arg_1B_0;
			if ((arg_1B_0 = this.onBecameInvisible) == null)
			{
				arg_1B_0 = (this.onBecameInvisible = new Subject<Unit>());
			}
			return arg_1B_0;
		}

		private void OnBecameVisible()
		{
			if (this.onBecameVisible != null)
			{
				this.onBecameVisible.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnBecameVisibleAsObservable()
		{
			Subject<Unit> arg_1B_0;
			if ((arg_1B_0 = this.onBecameVisible) == null)
			{
				arg_1B_0 = (this.onBecameVisible = new Subject<Unit>());
			}
			return arg_1B_0;
		}

		protected override void RaiseOnCompletedOnDestroy()
		{
			if (this.onBecameInvisible != null)
			{
				this.onBecameInvisible.OnCompleted();
			}
			if (this.onBecameVisible != null)
			{
				this.onBecameVisible.OnCompleted();
			}
		}
	}
}
