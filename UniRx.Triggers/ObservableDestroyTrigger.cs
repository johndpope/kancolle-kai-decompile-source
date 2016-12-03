using System;
using UnityEngine;

namespace UniRx.Triggers
{
	[DisallowMultipleComponent]
	public class ObservableDestroyTrigger : MonoBehaviour
	{
		private bool calledDestroy;

		private Subject<Unit> onDestroy;

		private void OnDestroy()
		{
			this.calledDestroy = true;
			if (this.onDestroy != null)
			{
				this.onDestroy.OnNext(Unit.Default);
				this.onDestroy.OnCompleted();
			}
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
	}
}
