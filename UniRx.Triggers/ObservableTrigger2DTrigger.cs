using System;
using UnityEngine;

namespace UniRx.Triggers
{
	[DisallowMultipleComponent]
	public class ObservableTrigger2DTrigger : ObservableTriggerBase
	{
		private Subject<Collider2D> onTriggerEnter2D;

		private Subject<Collider2D> onTriggerExit2D;

		private Subject<Collider2D> onTriggerStay2D;

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (this.onTriggerEnter2D != null)
			{
				this.onTriggerEnter2D.OnNext(other);
			}
		}

		public IObservable<Collider2D> OnTriggerEnter2DAsObservable()
		{
			Subject<Collider2D> arg_1B_0;
			if ((arg_1B_0 = this.onTriggerEnter2D) == null)
			{
				arg_1B_0 = (this.onTriggerEnter2D = new Subject<Collider2D>());
			}
			return arg_1B_0;
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			if (this.onTriggerExit2D != null)
			{
				this.onTriggerExit2D.OnNext(other);
			}
		}

		public IObservable<Collider2D> OnTriggerExit2DAsObservable()
		{
			Subject<Collider2D> arg_1B_0;
			if ((arg_1B_0 = this.onTriggerExit2D) == null)
			{
				arg_1B_0 = (this.onTriggerExit2D = new Subject<Collider2D>());
			}
			return arg_1B_0;
		}

		private void OnTriggerStay2D(Collider2D other)
		{
			if (this.onTriggerStay2D != null)
			{
				this.onTriggerStay2D.OnNext(other);
			}
		}

		public IObservable<Collider2D> OnTriggerStay2DAsObservable()
		{
			Subject<Collider2D> arg_1B_0;
			if ((arg_1B_0 = this.onTriggerStay2D) == null)
			{
				arg_1B_0 = (this.onTriggerStay2D = new Subject<Collider2D>());
			}
			return arg_1B_0;
		}

		protected override void RaiseOnCompletedOnDestroy()
		{
			if (this.onTriggerEnter2D != null)
			{
				this.onTriggerEnter2D.OnCompleted();
			}
			if (this.onTriggerExit2D != null)
			{
				this.onTriggerExit2D.OnCompleted();
			}
			if (this.onTriggerStay2D != null)
			{
				this.onTriggerStay2D.OnCompleted();
			}
		}
	}
}
