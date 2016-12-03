using System;
using UnityEngine;

namespace UniRx.Triggers
{
	[DisallowMultipleComponent]
	public class ObservableCollisionTrigger : ObservableTriggerBase
	{
		private Subject<Collision> onCollisionEnter;

		private Subject<Collision> onCollisionExit;

		private Subject<Collision> onCollisionStay;

		private void OnCollisionEnter(Collision collision)
		{
			if (this.onCollisionEnter != null)
			{
				this.onCollisionEnter.OnNext(collision);
			}
		}

		public IObservable<Collision> OnCollisionEnterAsObservable()
		{
			Subject<Collision> arg_1B_0;
			if ((arg_1B_0 = this.onCollisionEnter) == null)
			{
				arg_1B_0 = (this.onCollisionEnter = new Subject<Collision>());
			}
			return arg_1B_0;
		}

		private void OnCollisionExit(Collision collisionInfo)
		{
			if (this.onCollisionExit != null)
			{
				this.onCollisionExit.OnNext(collisionInfo);
			}
		}

		public IObservable<Collision> OnCollisionExitAsObservable()
		{
			Subject<Collision> arg_1B_0;
			if ((arg_1B_0 = this.onCollisionExit) == null)
			{
				arg_1B_0 = (this.onCollisionExit = new Subject<Collision>());
			}
			return arg_1B_0;
		}

		private void OnCollisionStay(Collision collisionInfo)
		{
			if (this.onCollisionStay != null)
			{
				this.onCollisionStay.OnNext(collisionInfo);
			}
		}

		public IObservable<Collision> OnCollisionStayAsObservable()
		{
			Subject<Collision> arg_1B_0;
			if ((arg_1B_0 = this.onCollisionStay) == null)
			{
				arg_1B_0 = (this.onCollisionStay = new Subject<Collision>());
			}
			return arg_1B_0;
		}

		protected override void RaiseOnCompletedOnDestroy()
		{
			if (this.onCollisionEnter != null)
			{
				this.onCollisionEnter.OnCompleted();
			}
			if (this.onCollisionExit != null)
			{
				this.onCollisionExit.OnCompleted();
			}
			if (this.onCollisionStay != null)
			{
				this.onCollisionStay.OnCompleted();
			}
		}
	}
}
