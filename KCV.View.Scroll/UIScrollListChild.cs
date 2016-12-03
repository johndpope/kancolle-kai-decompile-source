using System;
using UnityEngine;

namespace KCV.View.Scroll
{
	[RequireComponent(typeof(UIWidget))]
	public class UIScrollListChild<__Model__> : MonoBehaviour where __Model__ : class
	{
		public delegate void UIScrollListChildAction(ActionType actionType, UIScrollListChild<__Model__> targetChild);

		[SerializeField]
		protected UIButton mButton_Action;

		private UIScrollListChild<__Model__>.UIScrollListChildAction mUIScrollListChildAction;

		private UIWidget mWidgetThis;

		private Vector3 mNextLocalPositionCache = Vector3.get_zero();

		public bool mIsClickable
		{
			get;
			private set;
		}

		public Vector2 Size
		{
			get
			{
				return this.mWidgetThis.localSize;
			}
		}

		public __Model__ Model
		{
			get;
			private set;
		}

		public int SortIndex
		{
			get;
			private set;
		}

		public bool IsShown
		{
			get;
			private set;
		}

		public Transform cachedTransform
		{
			get;
			private set;
		}

		private void Awake()
		{
			this.cachedTransform = base.get_transform();
			this.mWidgetThis = base.GetComponent<UIWidget>();
			if (this.mButton_Action != null)
			{
				this.mButton_Action.hover = Util.CursolColor;
				this.mButton_Action.defaultColor = Color.get_white();
				this.mButton_Action.pressed = Color.get_white();
				this.mButton_Action.disabledColor = Color.get_white();
			}
		}

		public void Initialize(__Model__ model, bool clickable)
		{
			this.Model = model;
			this.mIsClickable = clickable;
			this.InitializeChildContents(this.Model, this.mIsClickable);
		}

		public void Refresh(__Model__ model, bool clickable)
		{
			this.Model = model;
			this.mIsClickable = clickable;
			this.RefreshChildContents(this.Model, this.mIsClickable);
		}

		public void UpdateIndex(int nextIndex)
		{
			this.SortIndex = nextIndex;
		}

		protected virtual void InitializeChildContents(__Model__ model, bool isClickable)
		{
		}

		protected virtual void RefreshChildContents(__Model__ model, bool isClickable)
		{
		}

		public virtual void Show()
		{
			this.IsShown = true;
			this.mWidgetThis.alpha = 1f;
			this.OnShowAnimation();
		}

		public virtual void Hide()
		{
			this.IsShown = false;
			this.mWidgetThis.alpha = 0.0001f;
		}

		public void ReleaseModel()
		{
			this.Model = (__Model__)((object)null);
		}

		public virtual void Hover()
		{
			UISelectedObject.SelectedOneObjectBlink(this.mButton_Action.get_gameObject(), true);
		}

		public virtual void RemoveHover()
		{
			UISelectedObject.SelectedOneObjectBlink(this.mButton_Action.get_gameObject(), false);
		}

		public void StopBlink()
		{
			this.RemoveHover();
		}

		public virtual void OnShowAnimation()
		{
		}

		public Vector3 GenerateConnectToPosition(UIScrollListChild<__Model__> target, ConnectType connectType)
		{
			Vector3 localPosition = target.get_transform().get_localPosition();
			if (connectType != ConnectType.Top)
			{
				if (connectType != ConnectType.Bottom)
				{
					this.mNextLocalPositionCache.Set(0f, 0f, 0f);
				}
				else
				{
					this.mNextLocalPositionCache.Set(localPosition.x, localPosition.y - target.Size.y, localPosition.z);
				}
			}
			else
			{
				this.mNextLocalPositionCache.Set(localPosition.x, localPosition.y + this.Size.y, localPosition.z);
			}
			return this.mNextLocalPositionCache;
		}

		public void SetOnActionListener(UIScrollListChild<__Model__>.UIScrollListChildAction method)
		{
			this.mUIScrollListChildAction = method;
		}

		private void OnAction(ActionType actionType, UIScrollListChild<__Model__> targetChild)
		{
			if (this.mUIScrollListChildAction != null)
			{
				this.mUIScrollListChildAction(actionType, targetChild);
			}
		}

		public virtual void OnTouchScrollListChild()
		{
			if (this.Model != null && this.mIsClickable)
			{
				this.OnAction(ActionType.OnTouch, this);
			}
		}

		public void setEnable(bool isEnable)
		{
			this.mButton_Action.set_enabled(isEnable);
		}

		private void OnDestroy()
		{
			this.mButton_Action = null;
			this.mUIScrollListChildAction = null;
			this.mWidgetThis = null;
			this.Model = (__Model__)((object)null);
			this.cachedTransform = null;
			this.OnCallDestroy();
		}

		protected virtual void OnCallDestroy()
		{
		}
	}
}
