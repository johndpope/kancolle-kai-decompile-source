using System;
using UnityEngine;

namespace UniRx.Triggers
{
	[DisallowMultipleComponent]
	public class ObservableMouseTrigger : ObservableTriggerBase
	{
		private Subject<Unit> onMouseDown;

		private Subject<Unit> onMouseDrag;

		private Subject<Unit> onMouseEnter;

		private Subject<Unit> onMouseExit;

		private Subject<Unit> onMouseOver;

		private Subject<Unit> onMouseUp;

		private Subject<Unit> onMouseUpAsButton;

		private void OnMouseDown()
		{
			if (this.onMouseDown != null)
			{
				this.onMouseDown.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnMouseDownAsObservable()
		{
			Subject<Unit> arg_1B_0;
			if ((arg_1B_0 = this.onMouseDown) == null)
			{
				arg_1B_0 = (this.onMouseDown = new Subject<Unit>());
			}
			return arg_1B_0;
		}

		private void OnMouseDrag()
		{
			if (this.onMouseDrag != null)
			{
				this.onMouseDrag.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnMouseDragAsObservable()
		{
			Subject<Unit> arg_1B_0;
			if ((arg_1B_0 = this.onMouseDrag) == null)
			{
				arg_1B_0 = (this.onMouseDrag = new Subject<Unit>());
			}
			return arg_1B_0;
		}

		private void OnMouseEnter()
		{
			if (this.onMouseEnter != null)
			{
				this.onMouseEnter.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnMouseEnterAsObservable()
		{
			Subject<Unit> arg_1B_0;
			if ((arg_1B_0 = this.onMouseEnter) == null)
			{
				arg_1B_0 = (this.onMouseEnter = new Subject<Unit>());
			}
			return arg_1B_0;
		}

		private void OnMouseExit()
		{
			if (this.onMouseExit != null)
			{
				this.onMouseExit.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnMouseExitAsObservable()
		{
			Subject<Unit> arg_1B_0;
			if ((arg_1B_0 = this.onMouseExit) == null)
			{
				arg_1B_0 = (this.onMouseExit = new Subject<Unit>());
			}
			return arg_1B_0;
		}

		private void OnMouseOver()
		{
			if (this.onMouseOver != null)
			{
				this.onMouseOver.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnMouseOverAsObservable()
		{
			Subject<Unit> arg_1B_0;
			if ((arg_1B_0 = this.onMouseOver) == null)
			{
				arg_1B_0 = (this.onMouseOver = new Subject<Unit>());
			}
			return arg_1B_0;
		}

		private void OnMouseUp()
		{
			if (this.onMouseUp != null)
			{
				this.onMouseUp.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnMouseUpAsObservable()
		{
			Subject<Unit> arg_1B_0;
			if ((arg_1B_0 = this.onMouseUp) == null)
			{
				arg_1B_0 = (this.onMouseUp = new Subject<Unit>());
			}
			return arg_1B_0;
		}

		private void OnMouseUpAsButton()
		{
			if (this.onMouseUpAsButton != null)
			{
				this.onMouseUpAsButton.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnMouseUpAsButtonAsObservable()
		{
			Subject<Unit> arg_1B_0;
			if ((arg_1B_0 = this.onMouseUpAsButton) == null)
			{
				arg_1B_0 = (this.onMouseUpAsButton = new Subject<Unit>());
			}
			return arg_1B_0;
		}

		protected override void RaiseOnCompletedOnDestroy()
		{
			if (this.onMouseDown != null)
			{
				this.onMouseDown.OnCompleted();
			}
			if (this.onMouseDrag != null)
			{
				this.onMouseDrag.OnCompleted();
			}
			if (this.onMouseEnter != null)
			{
				this.onMouseEnter.OnCompleted();
			}
			if (this.onMouseExit != null)
			{
				this.onMouseExit.OnCompleted();
			}
			if (this.onMouseOver != null)
			{
				this.onMouseOver.OnCompleted();
			}
			if (this.onMouseUp != null)
			{
				this.onMouseUp.OnCompleted();
			}
			if (this.onMouseUpAsButton != null)
			{
				this.onMouseUpAsButton.OnCompleted();
			}
		}
	}
}
