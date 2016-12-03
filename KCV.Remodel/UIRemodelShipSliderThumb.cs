using System;
using UnityEngine;

namespace KCV.Remodel
{
	public class UIRemodelShipSliderThumb : MonoBehaviour
	{
		public enum ActionType
		{
			Move,
			FingerUp
		}

		public enum CheckType
		{
			DraggableY
		}

		public delegate void UIRemodelShipSliderThumbAction(UIRemodelShipSliderThumb.ActionType actionType, UIRemodelShipSliderThumb calledObject);

		public delegate bool UIRemodelShipSliderThumbCheck(UIRemodelShipSliderThumb.CheckType checkType, UIRemodelShipSliderThumb calledObject);

		private UIRemodelShipSliderThumb.UIRemodelShipSliderThumbAction mUIRemodelShipSliderThumbAction;

		private UIRemodelShipSliderThumb.UIRemodelShipSliderThumbCheck mUIRemodelShipSliderThumbCheck;

		public Vector3 mNextDragWorldPosition = Vector3.get_zero();

		public void SetOnUIRemodelShipSliderThumbActionListener(UIRemodelShipSliderThumb.UIRemodelShipSliderThumbAction action)
		{
			this.mUIRemodelShipSliderThumbAction = action;
		}

		public void SetOnUIRemodelShipSliderThumbCheckListener(UIRemodelShipSliderThumb.UIRemodelShipSliderThumbCheck checker)
		{
			this.mUIRemodelShipSliderThumbCheck = checker;
		}

		private void OnAction(UIRemodelShipSliderThumb.ActionType actionType, UIRemodelShipSliderThumb calledObject)
		{
			if (this.mUIRemodelShipSliderThumbAction != null)
			{
				this.mUIRemodelShipSliderThumbAction(actionType, calledObject);
			}
		}

		private bool OnCheck(UIRemodelShipSliderThumb.CheckType checkType, UIRemodelShipSliderThumb calledObject)
		{
			return this.mUIRemodelShipSliderThumbCheck != null && this.mUIRemodelShipSliderThumbCheck(checkType, calledObject);
		}

		private void OnDrag(Vector2 delta)
		{
			Vector2 lastTouchPosition = UICamera.lastTouchPosition;
			Vector2 vector = UICamera.currentCamera.ScreenToWorldPoint(lastTouchPosition);
			this.mNextDragWorldPosition = new Vector3(base.get_transform().get_position().x, vector.y, base.get_transform().get_position().z);
			if (!this.OnCheck(UIRemodelShipSliderThumb.CheckType.DraggableY, this))
			{
				this.OnAction(UIRemodelShipSliderThumb.ActionType.Move, this);
			}
			else
			{
				this.mNextDragWorldPosition = base.get_transform().get_position();
			}
		}

		internal void Release()
		{
			this.mUIRemodelShipSliderThumbAction = null;
			this.mUIRemodelShipSliderThumbCheck = null;
		}
	}
}
