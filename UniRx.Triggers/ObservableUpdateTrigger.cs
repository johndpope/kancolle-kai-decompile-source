using System;
using UnityEngine;

namespace UniRx.Triggers
{
	[DisallowMultipleComponent]
	public class ObservableUpdateTrigger : ObservableTriggerBase
	{
		private Subject<Unit> update;

		private void Update()
		{
			if (this.update != null)
			{
				this.update.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> UpdateAsObservable()
		{
			Subject<Unit> arg_1B_0;
			if ((arg_1B_0 = this.update) == null)
			{
				arg_1B_0 = (this.update = new Subject<Unit>());
			}
			return arg_1B_0;
		}

		protected override void RaiseOnCompletedOnDestroy()
		{
			if (this.update != null)
			{
				this.update.OnCompleted();
			}
		}
	}
}
