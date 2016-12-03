using System;
using UnityEngine;

namespace UniRx.Triggers
{
	[DisallowMultipleComponent]
	public class ObservableEnableTrigger : ObservableTriggerBase
	{
		private Subject<Unit> onEnable;

		private Subject<Unit> onDisable;

		private void OnEnable()
		{
			if (this.onEnable != null)
			{
				this.onEnable.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnEnableAsObservable()
		{
			Subject<Unit> arg_1B_0;
			if ((arg_1B_0 = this.onEnable) == null)
			{
				arg_1B_0 = (this.onEnable = new Subject<Unit>());
			}
			return arg_1B_0;
		}

		private void OnDisable()
		{
			if (this.onDisable != null)
			{
				this.onDisable.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnDisableAsObservable()
		{
			Subject<Unit> arg_1B_0;
			if ((arg_1B_0 = this.onDisable) == null)
			{
				arg_1B_0 = (this.onDisable = new Subject<Unit>());
			}
			return arg_1B_0;
		}

		protected override void RaiseOnCompletedOnDestroy()
		{
			if (this.onEnable != null)
			{
				this.onEnable.OnCompleted();
			}
			if (this.onDisable != null)
			{
				this.onDisable.OnCompleted();
			}
		}
	}
}
