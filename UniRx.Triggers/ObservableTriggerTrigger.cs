using System;
using UnityEngine;

namespace UniRx.Triggers
{
	[DisallowMultipleComponent]
	public class ObservableTriggerTrigger : ObservableTriggerBase
	{
		private Subject<Collider> onTriggerEnter;

		private Subject<Collider> onTriggerExit;

		private Subject<Collider> onTriggerStay;

		private void OnTriggerEnter(Collider other)
		{
			if (this.onTriggerEnter != null)
			{
				this.onTriggerEnter.OnNext(other);
			}
		}

		public IObservable<Collider> OnTriggerEnterAsObservable()
		{
			Subject<Collider> arg_1B_0;
			if ((arg_1B_0 = this.onTriggerEnter) == null)
			{
				arg_1B_0 = (this.onTriggerEnter = new Subject<Collider>());
			}
			return arg_1B_0;
		}

		private void OnTriggerExit(Collider other)
		{
			if (this.onTriggerExit != null)
			{
				this.onTriggerExit.OnNext(other);
			}
		}

		public IObservable<Collider> OnTriggerExitAsObservable()
		{
			Subject<Collider> arg_1B_0;
			if ((arg_1B_0 = this.onTriggerExit) == null)
			{
				arg_1B_0 = (this.onTriggerExit = new Subject<Collider>());
			}
			return arg_1B_0;
		}

		private void OnTriggerStay(Collider other)
		{
			if (this.onTriggerStay != null)
			{
				this.onTriggerStay.OnNext(other);
			}
		}

		public IObservable<Collider> OnTriggerStayAsObservable()
		{
			Subject<Collider> arg_1B_0;
			if ((arg_1B_0 = this.onTriggerStay) == null)
			{
				arg_1B_0 = (this.onTriggerStay = new Subject<Collider>());
			}
			return arg_1B_0;
		}

		protected override void RaiseOnCompletedOnDestroy()
		{
			if (this.onTriggerEnter != null)
			{
				this.onTriggerEnter.OnCompleted();
			}
			if (this.onTriggerExit != null)
			{
				this.onTriggerExit.OnCompleted();
			}
			if (this.onTriggerStay != null)
			{
				this.onTriggerStay.OnCompleted();
			}
		}
	}
}
