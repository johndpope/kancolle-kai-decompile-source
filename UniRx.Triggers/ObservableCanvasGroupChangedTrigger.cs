using System;
using UnityEngine;

namespace UniRx.Triggers
{
	[DisallowMultipleComponent]
	public class ObservableCanvasGroupChangedTrigger : ObservableTriggerBase
	{
		private Subject<Unit> onCanvasGroupChanged;

		private void OnCanvasGroupChanged()
		{
			if (this.onCanvasGroupChanged != null)
			{
				this.onCanvasGroupChanged.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnCanvasGroupChangedAsObservable()
		{
			Subject<Unit> arg_1B_0;
			if ((arg_1B_0 = this.onCanvasGroupChanged) == null)
			{
				arg_1B_0 = (this.onCanvasGroupChanged = new Subject<Unit>());
			}
			return arg_1B_0;
		}

		protected override void RaiseOnCompletedOnDestroy()
		{
			if (this.onCanvasGroupChanged != null)
			{
				this.onCanvasGroupChanged.OnCompleted();
			}
		}
	}
}
