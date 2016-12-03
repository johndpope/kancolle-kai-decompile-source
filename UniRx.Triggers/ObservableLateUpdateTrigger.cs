using System;
using UnityEngine;

namespace UniRx.Triggers
{
	[DisallowMultipleComponent]
	public class ObservableLateUpdateTrigger : ObservableTriggerBase
	{
		private Subject<Unit> lateUpdate;

		private void LateUpdate()
		{
			if (this.lateUpdate != null)
			{
				this.lateUpdate.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> LateUpdateAsObservable()
		{
			Subject<Unit> arg_1B_0;
			if ((arg_1B_0 = this.lateUpdate) == null)
			{
				arg_1B_0 = (this.lateUpdate = new Subject<Unit>());
			}
			return arg_1B_0;
		}

		protected override void RaiseOnCompletedOnDestroy()
		{
			if (this.lateUpdate != null)
			{
				this.lateUpdate.OnCompleted();
			}
		}
	}
}
