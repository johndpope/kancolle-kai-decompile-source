using System;
using UnityEngine;

namespace UniRx.Triggers
{
	[DisallowMultipleComponent]
	public class ObservableFixedUpdateTrigger : ObservableTriggerBase
	{
		private Subject<Unit> fixedUpdate;

		private void FixedUpdate()
		{
			if (this.fixedUpdate != null)
			{
				this.fixedUpdate.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> FixedUpdateAsObservable()
		{
			Subject<Unit> arg_1B_0;
			if ((arg_1B_0 = this.fixedUpdate) == null)
			{
				arg_1B_0 = (this.fixedUpdate = new Subject<Unit>());
			}
			return arg_1B_0;
		}

		protected override void RaiseOnCompletedOnDestroy()
		{
			if (this.fixedUpdate != null)
			{
				this.fixedUpdate.OnCompleted();
			}
		}
	}
}
