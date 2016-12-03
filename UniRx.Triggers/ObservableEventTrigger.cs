using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UniRx.Triggers
{
	[DisallowMultipleComponent]
	public class ObservableEventTrigger : ObservableTriggerBase, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IBeginDragHandler, IInitializePotentialDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IScrollHandler, IUpdateSelectedHandler, ISelectHandler, IDeselectHandler, IMoveHandler, ISubmitHandler, ICancelHandler
	{
		private Subject<BaseEventData> onDeselect;

		private Subject<AxisEventData> onMove;

		private Subject<PointerEventData> onPointerDown;

		private Subject<PointerEventData> onPointerEnter;

		private Subject<PointerEventData> onPointerExit;

		private Subject<PointerEventData> onPointerUp;

		private Subject<BaseEventData> onSelect;

		private Subject<PointerEventData> onPointerClick;

		private Subject<BaseEventData> onSubmit;

		private Subject<PointerEventData> onDrag;

		private Subject<PointerEventData> onBeginDrag;

		private Subject<PointerEventData> onEndDrag;

		private Subject<PointerEventData> onDrop;

		private Subject<BaseEventData> onUpdateSelected;

		private Subject<PointerEventData> onInitializePotentialDrag;

		private Subject<BaseEventData> onCancel;

		private Subject<PointerEventData> onScroll;

		void IDeselectHandler.OnDeselect(BaseEventData eventData)
		{
			if (this.onDeselect != null)
			{
				this.onDeselect.OnNext(eventData);
			}
		}

		void IMoveHandler.OnMove(AxisEventData eventData)
		{
			if (this.onMove != null)
			{
				this.onMove.OnNext(eventData);
			}
		}

		void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
		{
			if (this.onPointerDown != null)
			{
				this.onPointerDown.OnNext(eventData);
			}
		}

		void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
		{
			if (this.onPointerEnter != null)
			{
				this.onPointerEnter.OnNext(eventData);
			}
		}

		void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
		{
			if (this.onPointerExit != null)
			{
				this.onPointerExit.OnNext(eventData);
			}
		}

		void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
		{
			if (this.onPointerUp != null)
			{
				this.onPointerUp.OnNext(eventData);
			}
		}

		void ISelectHandler.OnSelect(BaseEventData eventData)
		{
			if (this.onSelect != null)
			{
				this.onSelect.OnNext(eventData);
			}
		}

		void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
		{
			if (this.onPointerClick != null)
			{
				this.onPointerClick.OnNext(eventData);
			}
		}

		void ISubmitHandler.OnSubmit(BaseEventData eventData)
		{
			if (this.onSubmit != null)
			{
				this.onSubmit.OnNext(eventData);
			}
		}

		void IDragHandler.OnDrag(PointerEventData eventData)
		{
			if (this.onDrag != null)
			{
				this.onDrag.OnNext(eventData);
			}
		}

		void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
		{
			if (this.onBeginDrag != null)
			{
				this.onBeginDrag.OnNext(eventData);
			}
		}

		void IEndDragHandler.OnEndDrag(PointerEventData eventData)
		{
			if (this.onEndDrag != null)
			{
				this.onEndDrag.OnNext(eventData);
			}
		}

		void IDropHandler.OnDrop(PointerEventData eventData)
		{
			if (this.onDrop != null)
			{
				this.onDrop.OnNext(eventData);
			}
		}

		void IUpdateSelectedHandler.OnUpdateSelected(BaseEventData eventData)
		{
			if (this.onUpdateSelected != null)
			{
				this.onUpdateSelected.OnNext(eventData);
			}
		}

		void IInitializePotentialDragHandler.OnInitializePotentialDrag(PointerEventData eventData)
		{
			if (this.onInitializePotentialDrag != null)
			{
				this.onInitializePotentialDrag.OnNext(eventData);
			}
		}

		void ICancelHandler.OnCancel(BaseEventData eventData)
		{
			if (this.onCancel != null)
			{
				this.onCancel.OnNext(eventData);
			}
		}

		void IScrollHandler.OnScroll(PointerEventData eventData)
		{
			if (this.onScroll != null)
			{
				this.onScroll.OnNext(eventData);
			}
		}

		public IObservable<BaseEventData> OnDeselectAsObservable()
		{
			Subject<BaseEventData> arg_1B_0;
			if ((arg_1B_0 = this.onDeselect) == null)
			{
				arg_1B_0 = (this.onDeselect = new Subject<BaseEventData>());
			}
			return arg_1B_0;
		}

		public IObservable<AxisEventData> OnMoveAsObservable()
		{
			Subject<AxisEventData> arg_1B_0;
			if ((arg_1B_0 = this.onMove) == null)
			{
				arg_1B_0 = (this.onMove = new Subject<AxisEventData>());
			}
			return arg_1B_0;
		}

		public IObservable<PointerEventData> OnPointerDownAsObservable()
		{
			Subject<PointerEventData> arg_1B_0;
			if ((arg_1B_0 = this.onPointerDown) == null)
			{
				arg_1B_0 = (this.onPointerDown = new Subject<PointerEventData>());
			}
			return arg_1B_0;
		}

		public IObservable<PointerEventData> OnPointerEnterAsObservable()
		{
			Subject<PointerEventData> arg_1B_0;
			if ((arg_1B_0 = this.onPointerEnter) == null)
			{
				arg_1B_0 = (this.onPointerEnter = new Subject<PointerEventData>());
			}
			return arg_1B_0;
		}

		public IObservable<PointerEventData> OnPointerExitAsObservable()
		{
			Subject<PointerEventData> arg_1B_0;
			if ((arg_1B_0 = this.onPointerExit) == null)
			{
				arg_1B_0 = (this.onPointerExit = new Subject<PointerEventData>());
			}
			return arg_1B_0;
		}

		public IObservable<PointerEventData> OnPointerUpAsObservable()
		{
			Subject<PointerEventData> arg_1B_0;
			if ((arg_1B_0 = this.onPointerUp) == null)
			{
				arg_1B_0 = (this.onPointerUp = new Subject<PointerEventData>());
			}
			return arg_1B_0;
		}

		public IObservable<BaseEventData> OnSelectAsObservable()
		{
			Subject<BaseEventData> arg_1B_0;
			if ((arg_1B_0 = this.onSelect) == null)
			{
				arg_1B_0 = (this.onSelect = new Subject<BaseEventData>());
			}
			return arg_1B_0;
		}

		public IObservable<PointerEventData> OnPointerClickAsObservable()
		{
			Subject<PointerEventData> arg_1B_0;
			if ((arg_1B_0 = this.onPointerClick) == null)
			{
				arg_1B_0 = (this.onPointerClick = new Subject<PointerEventData>());
			}
			return arg_1B_0;
		}

		public IObservable<BaseEventData> OnSubmitAsObservable()
		{
			Subject<BaseEventData> arg_1B_0;
			if ((arg_1B_0 = this.onSubmit) == null)
			{
				arg_1B_0 = (this.onSubmit = new Subject<BaseEventData>());
			}
			return arg_1B_0;
		}

		public IObservable<PointerEventData> OnDragAsObservable()
		{
			Subject<PointerEventData> arg_1B_0;
			if ((arg_1B_0 = this.onDrag) == null)
			{
				arg_1B_0 = (this.onDrag = new Subject<PointerEventData>());
			}
			return arg_1B_0;
		}

		public IObservable<PointerEventData> OnBeginDragAsObservable()
		{
			Subject<PointerEventData> arg_1B_0;
			if ((arg_1B_0 = this.onBeginDrag) == null)
			{
				arg_1B_0 = (this.onBeginDrag = new Subject<PointerEventData>());
			}
			return arg_1B_0;
		}

		public IObservable<PointerEventData> OnEndDragAsObservable()
		{
			Subject<PointerEventData> arg_1B_0;
			if ((arg_1B_0 = this.onEndDrag) == null)
			{
				arg_1B_0 = (this.onEndDrag = new Subject<PointerEventData>());
			}
			return arg_1B_0;
		}

		public IObservable<PointerEventData> OnDropAsObservable()
		{
			Subject<PointerEventData> arg_1B_0;
			if ((arg_1B_0 = this.onDrop) == null)
			{
				arg_1B_0 = (this.onDrop = new Subject<PointerEventData>());
			}
			return arg_1B_0;
		}

		public IObservable<BaseEventData> OnUpdateSelectedAsObservable()
		{
			Subject<BaseEventData> arg_1B_0;
			if ((arg_1B_0 = this.onUpdateSelected) == null)
			{
				arg_1B_0 = (this.onUpdateSelected = new Subject<BaseEventData>());
			}
			return arg_1B_0;
		}

		public IObservable<PointerEventData> OnInitializePotentialDragAsObservable()
		{
			Subject<PointerEventData> arg_1B_0;
			if ((arg_1B_0 = this.onInitializePotentialDrag) == null)
			{
				arg_1B_0 = (this.onInitializePotentialDrag = new Subject<PointerEventData>());
			}
			return arg_1B_0;
		}

		public IObservable<BaseEventData> OnCancelAsObservable()
		{
			Subject<BaseEventData> arg_1B_0;
			if ((arg_1B_0 = this.onCancel) == null)
			{
				arg_1B_0 = (this.onCancel = new Subject<BaseEventData>());
			}
			return arg_1B_0;
		}

		public IObservable<PointerEventData> OnScrollAsObservable()
		{
			Subject<PointerEventData> arg_1B_0;
			if ((arg_1B_0 = this.onScroll) == null)
			{
				arg_1B_0 = (this.onScroll = new Subject<PointerEventData>());
			}
			return arg_1B_0;
		}

		protected override void RaiseOnCompletedOnDestroy()
		{
			if (this.onDeselect != null)
			{
				this.onDeselect.OnCompleted();
			}
			if (this.onMove != null)
			{
				this.onMove.OnCompleted();
			}
			if (this.onPointerDown != null)
			{
				this.onPointerDown.OnCompleted();
			}
			if (this.onPointerEnter != null)
			{
				this.onPointerEnter.OnCompleted();
			}
			if (this.onPointerExit != null)
			{
				this.onPointerExit.OnCompleted();
			}
			if (this.onPointerUp != null)
			{
				this.onPointerUp.OnCompleted();
			}
			if (this.onSelect != null)
			{
				this.onSelect.OnCompleted();
			}
			if (this.onPointerClick != null)
			{
				this.onPointerClick.OnCompleted();
			}
			if (this.onSubmit != null)
			{
				this.onSubmit.OnCompleted();
			}
			if (this.onDrag != null)
			{
				this.onDrag.OnCompleted();
			}
			if (this.onBeginDrag != null)
			{
				this.onBeginDrag.OnCompleted();
			}
			if (this.onEndDrag != null)
			{
				this.onEndDrag.OnCompleted();
			}
			if (this.onDrop != null)
			{
				this.onDrop.OnCompleted();
			}
			if (this.onUpdateSelected != null)
			{
				this.onUpdateSelected.OnCompleted();
			}
			if (this.onInitializePotentialDrag != null)
			{
				this.onInitializePotentialDrag.OnCompleted();
			}
			if (this.onCancel != null)
			{
				this.onCancel.OnCompleted();
			}
			if (this.onScroll != null)
			{
				this.onScroll.OnCompleted();
			}
		}
	}
}
