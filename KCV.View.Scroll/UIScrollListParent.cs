using DG.Tweening;
using KCV.Display;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.View.Scroll
{
	public class UIScrollListParent<Model, View> : MonoBehaviour where Model : class where View : UIScrollListChild<Model>
	{
		public delegate void UIScrollListParentAction(ActionType actionType, UIScrollListParent<Model, View> calledObject, UIScrollListChild<Model> actionChild);

		public delegate bool UIScrollListParentCheck(CheckType checkType, UIScrollListParent<Model, View> calledObject, Model checkChild);

		protected Model[] Models;

		private Vector3[] ViewsDefaultLocalPosition;

		protected View[] Views;

		protected View ViewFocus;

		private bool AnimationScrollNow;

		public bool EnableTouchControl = true;

		[SerializeField, Tooltip("子要素のプレハブを設定してください。")]
		private View mPrefab_UIScrollListChild;

		[SerializeField, Tooltip("スワイプイベントを受け取る為のオブジェクト")]
		private UIDisplaySwipeEventRegion mUIDisplaySwipeEventRegion;

		[SerializeField, Tooltip("作った要素を入れる場所")]
		private UIGrid mGridContaints;

		private UIScrollListParent<Model, View>.UIScrollListParentAction mUIScrollListParentAction;

		private UIScrollListParent<Model, View>.UIScrollListParentCheck mUIScrollListParentCheck;

		[SerializeField, Tooltip("ユーザが１度に見ることができる要素の最大数を設定してください。予備のオブジェクトの事は考えないでください")]
		private int MAX_USER_VIEW_OBJECTS = 6;

		[SerializeField]
		private int MAX_RELIMINARY_OBJECTS = 1;

		private Vector3 mVector3_GridContaintsDefaultPosition;

		private bool mUseBottomUpReposition;

		[SerializeField, Tooltip("リスト下にフォーカスが入った時にフォーカス位置を底上げします")]
		private Vector3 mVector3_BottomUpPosition = default(Vector3);

		private int MaxLogicViewObjects;

		private Vector3 mVector3Top;

		private Vector3 mVector3TopDestroyLine;

		private Vector3 mVector3Bottom;

		private Vector3 mVector3BottomDestroyLine;

		protected KeyControl mKeyController;

		private int RollCount;

		private int CursolIndex;

		private float defaultIntervalTime;

		private bool mDefaultClickable = true;

		private Vector3 mVector3_RelocationCache = Vector3.get_zero();

		protected bool AnimationViewPositionNow
		{
			get;
			private set;
		}

		private void Awake()
		{
			this.mVector3_GridContaintsDefaultPosition = this.mGridContaints.get_transform().get_localPosition();
			this.OnAwake();
		}

		private void Start()
		{
			this.OnStart();
		}

		protected virtual void OnAwake()
		{
		}

		protected virtual void OnStart()
		{
		}

		public void SetOnUIScrollListParentAction(UIScrollListParent<Model, View>.UIScrollListParentAction method)
		{
			this.mUIScrollListParentAction = method;
		}

		public void SetOnUIScrollListParentCheck(UIScrollListParent<Model, View>.UIScrollListParentCheck method)
		{
			this.mUIScrollListParentCheck = method;
		}

		protected virtual void Initialize(Model[] models)
		{
			this.mGridContaints.get_transform().set_localPosition(this.mVector3_GridContaintsDefaultPosition);
			this.MaxLogicViewObjects = this.GetUserViewingLength() + this.GetReliminaryLength();
			this.Models = models;
			this.RollCount = 0;
			this.CursolIndex = 0;
			this.AnimationScrollNow = false;
			this.AnimationViewPositionNow = false;
			this.DestroyGridInChildren();
			View viewPrefab = this.GetViewPrefab();
			for (int i = 0; i < this.GetLogicViewingLength(); i++)
			{
				View component = Util.Instantiate(viewPrefab.get_gameObject(), this.mGridContaints.get_gameObject(), false, false).GetComponent<View>();
				component.SetOnActionListener(new UIScrollListChild<Model>.UIScrollListChildAction(this.OnChildAction));
			}
			this.mGridContaints.Reposition();
			this.Views = this.mGridContaints.GetComponentsInChildren<View>();
			this.ViewsDefaultLocalPosition = new Vector3[this.Views.Length];
			int num = 0;
			View[] views = this.Views;
			for (int j = 0; j < views.Length; j++)
			{
				View view = views[j];
				this.ViewsDefaultLocalPosition[num++] = view.cachedTransform.get_localPosition();
			}
			this.OnInitialize(this.RollCount);
		}

		public void EnableBottomUpMode()
		{
			this.mUseBottomUpReposition = true;
		}

		public void DisableBottomUpMode()
		{
			this.mUseBottomUpReposition = false;
		}

		public void UpdateNotify()
		{
			View[] views = this.Views;
			for (int i = 0; i < views.Length; i++)
			{
				View view = views[i];
				bool flag = this.OnCheck(CheckType.Clickable, this, view.Model);
				view.Refresh(view.Model, view);
			}
		}

		protected void Refresh(Model[] models)
		{
			this.Models = models;
			if (this.RollCount >= this.Models.Length)
			{
				int num = (this.RollCount / this.MAX_USER_VIEW_OBJECTS - 1) * this.MAX_USER_VIEW_OBJECTS;
				bool flag = num < this.Models.Length && 0 < num;
				if (flag)
				{
					this.RollCount = num;
				}
				else
				{
					this.RollCount = 0;
				}
			}
			this.AnimationScrollNow = false;
			this.AnimationViewPositionNow = false;
			this.MaxLogicViewObjects = this.GetUserViewingLength() + this.GetReliminaryLength();
			this.OnRefresh(this.RollCount, this.CursolIndex);
		}

		public void RefreshAndFirstFocus(Model[] models)
		{
			this.Models = models;
			this.AnimationScrollNow = false;
			this.AnimationViewPositionNow = false;
			this.MaxLogicViewObjects = this.GetUserViewingLength() + this.GetReliminaryLength();
			this.RollCount = 0;
			this.OnRefresh(0, 0);
		}

		private void OnRefresh(int rollCount, int focusPosition)
		{
			int logicViewingLength = this.GetLogicViewingLength();
			int num = rollCount % logicViewingLength;
			for (int i = 0; i < logicViewingLength; i++)
			{
				View child = this.Views[i];
				int num2 = rollCount + i - num;
				bool flag = i < num;
				if (flag)
				{
					num2 += logicViewingLength;
				}
				this.OnUpdateChild(num2, child, (num2 >= this.Models.Length) ? ((Model)((object)null)) : this.Models[num2]);
			}
			float childHeight = this.GetChildHeight();
			this.mVector3Top = new Vector3(0f, 0f, 0f);
			this.mVector3TopDestroyLine = new Vector3(0f, this.mVector3Top.y + childHeight, 0f);
			this.mVector3Bottom = new Vector3(0f, -childHeight * (float)this.GetLogicViewingLength(), 0f);
			this.mVector3BottomDestroyLine = new Vector3(0f, -childHeight * (float)(this.GetLogicViewingLength() - 1), 0f);
			this.mGridContaints.Reposition();
			this.RefreshPosition(0f, 0f);
			this.mUIDisplaySwipeEventRegion.SetOnSwipeActionJudgeCallBack(new UIDisplaySwipeEventRegion.SwipeJudgeDelegate(this.UIDisplaySwipeEventRegionDelegate));
			int modelSize = this.GetModelSize();
			bool flag2 = rollCount + focusPosition < modelSize;
			if (flag2)
			{
				int num3 = (rollCount + focusPosition) % this.GetLogicViewingLength();
				this.ChangeFocusView(this.Views[num3], false);
				if (this.mUseBottomUpReposition)
				{
					if (this.GetUserViewingLength() / 2 < focusPosition)
					{
						this.BottomUp();
					}
					else
					{
						this.BottomDown();
					}
				}
			}
			else if (0 < modelSize)
			{
				Debug.Log(string.Concat(new object[]
				{
					"HeadIndex:",
					num,
					" focusPos:",
					focusPosition,
					" modSize:",
					modelSize
				}));
				this.ChangeFocusView(this.Views[num], false);
				this.BottomDown();
			}
			View[] views = this.Views;
			for (int j = 0; j < views.Length; j++)
			{
				View view = views[j];
				view.cachedTransform.set_localPosition(this.ViewsDefaultLocalPosition[view.SortIndex - this.RollCount]);
			}
			this.OnFinishedInitialize();
		}

		private void BottomUp()
		{
			ShortcutExtensions.DOLocalMove(this.mGridContaints.get_transform(), this.mVector3_BottomUpPosition, 0.3f, false);
		}

		private void BottomDown()
		{
			ShortcutExtensions.DOLocalMove(this.mGridContaints.get_transform(), this.mVector3_GridContaintsDefaultPosition, 0.3f, false);
		}

		protected virtual void OnChangedModels(Model[] models)
		{
		}

		public void ForceInputKey(KeyControl.KeyName keyName)
		{
			switch (keyName)
			{
			case KeyControl.KeyName.BATU:
				this.OnAction(ActionType.OnBack, this, this.ViewFocus);
				return;
			case KeyControl.KeyName.MARU:
				this.OnKeyPressCircle();
				return;
			case KeyControl.KeyName.SHIKAKU:
			case KeyControl.KeyName.SANKAKU:
			case KeyControl.KeyName.SELECT:
			case KeyControl.KeyName.START:
				IL_2C:
				if (keyName != KeyControl.KeyName.DOWN)
				{
					return;
				}
				if (!this.AnimationViewPositionNow)
				{
					this.ShowChildNext();
				}
				return;
			case KeyControl.KeyName.L:
				this.MoveByView(-this.GetUserViewingLength());
				return;
			case KeyControl.KeyName.R:
				this.MoveByView(this.GetUserViewingLength());
				return;
			case KeyControl.KeyName.UP:
				if (!this.AnimationViewPositionNow)
				{
					this.ShowChildPrev();
				}
				return;
			}
			goto IL_2C;
		}

		public virtual void SetKeyController(KeyControl keyController)
		{
			this.mKeyController = keyController;
			if (this.mKeyController == null)
			{
				if (this.ViewFocus != null)
				{
					this.ViewFocus.StopBlink();
				}
				this.EnableTouchControl = false;
			}
			else
			{
				this.ChangeFocusView(this.ViewFocus, false);
				this.EnableTouchControl = true;
			}
		}

		private void setViewFocusHover(bool isHover)
		{
			if (this.ViewFocus == null)
			{
				return;
			}
			if (isHover)
			{
				this.ViewFocus.Hover();
			}
			else
			{
				this.ViewFocus.RemoveHover();
			}
		}

		public KeyControl GetKeyController()
		{
			if (this.mKeyController == null)
			{
				this.mKeyController = new KeyControl(0, 0, 0.4f, 0.1f);
			}
			this.setViewFocusHover(true);
			this.EnableTouchControl = true;
			return this.mKeyController;
		}

		public int GetModelSize()
		{
			return this.Models.Length;
		}

		public void SetCamera(Camera cam)
		{
			this.mUIDisplaySwipeEventRegion.SetEventCatchCamera(cam);
		}

		protected virtual void OnUpdate()
		{
		}

		private void Update()
		{
			if (this.mKeyController != null)
			{
				if ((this.mKeyController.keyState.get_Item(12).holdTime > 1.5f || this.mKeyController.keyState.get_Item(8).holdTime > 1.5f) && this.mKeyController.KeyInputInterval == this.defaultIntervalTime)
				{
					this.mKeyController.KeyInputInterval = this.mKeyController.KeyInputInterval / 3f;
				}
				else
				{
					this.mKeyController.KeyInputInterval = this.defaultIntervalTime;
				}
				if (this.mKeyController.keyState.get_Item(8).down)
				{
					this.OnKeyPressUp();
				}
				else if (this.mKeyController.keyState.get_Item(12).down)
				{
					this.OnKeyPressDown();
				}
				else if (this.mKeyController.keyState.get_Item(1).down)
				{
					this.OnKeyPressCircle();
				}
				else if (this.mKeyController.keyState.get_Item(0).down)
				{
					this.OnKeyPressCross();
				}
				else if (this.mKeyController.keyState.get_Item(14).down)
				{
					this.OnKeyPressLeft();
				}
				else if (this.mKeyController.keyState.get_Item(10).down)
				{
					this.OnKeyPressRight();
				}
				else if (this.mKeyController.keyState.get_Item(3).down)
				{
					this.OnKeyPressTriangle();
				}
			}
			this.OnUpdate();
		}

		protected virtual void OnKeyPressTriangle()
		{
		}

		protected virtual void OnKeyPressRight()
		{
			if (!this.AnimationViewPositionNow)
			{
				this.ShowChildNextPage();
			}
		}

		protected virtual void OnKeyPressLeft()
		{
			if (!this.AnimationViewPositionNow)
			{
				this.ShowChildPrevPage();
			}
		}

		protected virtual void OnKeyPressUp()
		{
			if (!this.AnimationViewPositionNow)
			{
				this.ShowChildPrev();
			}
		}

		protected virtual void OnKeyPressDown()
		{
			if (!this.AnimationViewPositionNow)
			{
				this.ShowChildNext();
			}
		}

		protected virtual void OnKeyPressL()
		{
			this.MoveByView(-this.GetUserViewingLength());
		}

		protected virtual void OnKeyPressR()
		{
			this.MoveByView(this.GetUserViewingLength());
		}

		protected virtual void OnKeyPressCross()
		{
			this.OnAction(ActionType.OnBack, this, this.ViewFocus);
		}

		protected virtual void OnKeyPressCircle()
		{
			if (!this.mUIDisplaySwipeEventRegion.isDraging && this.ViewFocus != null && this.ViewFocus.Model != null && this.ViewFocus.mIsClickable)
			{
				this.OnAction(ActionType.OnButtonSelect, this, this.ViewFocus);
			}
		}

		protected virtual void OnAction(ActionType actionType, UIScrollListParent<Model, View> calledObject, View actionChild)
		{
			if (this.mUIScrollListParentAction != null)
			{
				this.mUIScrollListParentAction(actionType, calledObject, actionChild);
			}
		}

		public void SetDefaultClickable(bool defaultClickable)
		{
			this.mDefaultClickable = defaultClickable;
		}

		private bool OnCheck(CheckType checkType, UIScrollListParent<Model, View> calledObject, Model checkChild)
		{
			if (this.mUIScrollListParentCheck != null)
			{
				return this.mUIScrollListParentCheck(checkType, calledObject, checkChild);
			}
			return this.mDefaultClickable;
		}

		private void ShowChildPrev()
		{
			if (this.RollCount == 0 && this.CursolIndex == 0)
			{
				return;
			}
			this.CursolIndex--;
			if (0 <= this.CursolIndex)
			{
				int rollCount = this.RollCount;
				int sortIndex = this.ViewFocus.SortIndex;
				if (rollCount < sortIndex)
				{
					View viewFromSortIndex = this.GetViewFromSortIndex(this.ViewFocus.SortIndex - 1);
					this.ChangeFocusView(viewFromSortIndex, false);
				}
				if (this.mUseBottomUpReposition)
				{
					this.BottomDown();
				}
			}
			else
			{
				this.CursolIndex = 0;
				this.RollCount--;
				View[] views = this.Views;
				for (int i = 0; i < views.Length; i++)
				{
					View view = views[i];
					if (view.SortIndex == this.RollCount + this.GetLogicViewingLength())
					{
						this.OnUpdateChild(this.RollCount, view, this.Models[this.RollCount]);
						this.ChangeFocusView(view, false);
					}
					int num = view.SortIndex - this.RollCount;
					Vector3 localPosition = this.ViewsDefaultLocalPosition[num];
					view.cachedTransform.set_localPosition(localPosition);
				}
			}
		}

		private void ShowChildNext()
		{
			if (this.Models.Length - 1 < this.RollCount + this.CursolIndex + 1)
			{
				return;
			}
			this.CursolIndex++;
			if (this.CursolIndex < this.GetUserViewingLength())
			{
				if (this.mUseBottomUpReposition)
				{
					this.BottomUp();
				}
				View viewFromSortIndex = this.GetViewFromSortIndex(this.ViewFocus.SortIndex + 1);
				this.ChangeFocusView(viewFromSortIndex, false);
			}
			else
			{
				this.CursolIndex = this.GetUserViewingLength();
				this.RollCount++;
				View[] views = this.Views;
				for (int i = 0; i < views.Length; i++)
				{
					View view = views[i];
					int loopIndex = this.GetLoopIndex(view.SortIndex, this.GetChildrenViewsCount(), -this.RollCount);
					Vector3 localPosition = this.ViewsDefaultLocalPosition[loopIndex];
					if (loopIndex == this.GetLogicViewingLength() - 1)
					{
						int num = this.RollCount + this.GetLogicViewingLength() - 1;
						if (this.Models.Length <= num)
						{
							this.OnUpdateChild(num, view, (Model)((object)null));
						}
						else
						{
							this.OnUpdateChild(num, view, this.Models[num]);
						}
					}
					if (loopIndex == this.GetUserViewingLength() - 1)
					{
						this.ChangeFocusView(view, false);
					}
					view.cachedTransform.set_localPosition(localPosition);
				}
			}
		}

		private void ShowChildNextPage()
		{
			int num = this.RollCount / this.MAX_USER_VIEW_OBJECTS + ((0 >= this.RollCount % this.MAX_USER_VIEW_OBJECTS) ? 0 : 1);
			int num2 = num + 1;
			int num3 = num2 * this.MAX_USER_VIEW_OBJECTS;
			if (num3 < this.GetModelSize() - this.MAX_USER_VIEW_OBJECTS)
			{
				this.RollCount = num3;
				this.OnInitialize(num3);
				if (this.mUseBottomUpReposition)
				{
					this.BottomDown();
				}
			}
			else
			{
				int num4 = this.GetModelSize() - this.MAX_USER_VIEW_OBJECTS;
				if (0 < num4)
				{
					num3 = num4;
					this.RollCount = num3;
					this.OnInitialize(num3);
				}
			}
		}

		private void ShowChildPrevPage()
		{
			int num = this.RollCount / this.MAX_USER_VIEW_OBJECTS + ((0 >= this.RollCount % this.MAX_USER_VIEW_OBJECTS) ? 0 : 1);
			int num2 = num - 1;
			int num3 = num2 * this.MAX_USER_VIEW_OBJECTS;
			if (0 <= num3)
			{
				this.RollCount = num3;
				this.OnInitialize(num3);
			}
			if (this.mUseBottomUpReposition)
			{
				this.BottomDown();
			}
		}

		private void DestroyGridInChildren()
		{
			using (List<Transform>.Enumerator enumerator = this.mGridContaints.GetChildList().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Transform current = enumerator.get_Current();
					NGUITools.Destroy(current);
				}
			}
		}

		protected virtual void OnChildAction(ActionType actionType, UIScrollListChild<Model> actionChild)
		{
			if (this.mKeyController == null)
			{
				return;
			}
			if (actionType == ActionType.OnTouch)
			{
				if (actionChild.Model != null && actionChild.mIsClickable)
				{
					this.ChangeFocusView((View)((object)actionChild), false);
					this.OnAction(actionType, this, (View)((object)actionChild));
				}
			}
		}

		private View GetViewFromSortIndex(int sortIndex)
		{
			return this.Views[this.GetLoopIndex(sortIndex, this.GetLogicViewingLength(), 0)];
		}

		private View GetViewFromLiteralIndex(int literalIndex)
		{
			return this.Views[this.GetLoopIndex(this.RollCount, this.GetLogicViewingLength(), literalIndex)];
		}

		private void OnInitialize(int startModelIdx)
		{
			int num = startModelIdx % this.GetLogicViewingLength();
			for (int i = 0; i < this.GetLogicViewingLength(); i++)
			{
				View child = this.Views[i];
				int num2 = startModelIdx + i - num;
				if (i < num)
				{
					num2 += this.GetLogicViewingLength();
				}
				this.OnUpdateChild(num2, child, (num2 >= this.Models.Length) ? ((Model)((object)null)) : this.Models[num2]);
			}
			float childHeight = this.GetChildHeight();
			this.mVector3Top = new Vector3(0f, 0f, 0f);
			this.mVector3TopDestroyLine = new Vector3(0f, this.mVector3Top.y + childHeight, 0f);
			this.mVector3Bottom = new Vector3(0f, -childHeight * (float)this.GetLogicViewingLength(), 0f);
			this.mVector3BottomDestroyLine = new Vector3(0f, -childHeight * (float)(this.GetLogicViewingLength() - 1), 0f);
			this.mGridContaints.Reposition();
			this.RefreshPosition(0f, 0f);
			this.mUIDisplaySwipeEventRegion.SetOnSwipeActionJudgeCallBack(new UIDisplaySwipeEventRegion.SwipeJudgeDelegate(this.UIDisplaySwipeEventRegionDelegate));
			if (0 < this.Models.Length && 0 <= num && num < this.Models.Length)
			{
				this.ChangeFocusView(this.Views[num], true);
			}
			View[] views = this.Views;
			for (int j = 0; j < views.Length; j++)
			{
				View view = views[j];
				view.cachedTransform.set_localPosition(this.ViewsDefaultLocalPosition[view.SortIndex - this.RollCount]);
			}
			this.OnFinishedInitialize();
		}

		protected virtual void OnFinishedInitialize()
		{
		}

		protected void ChangeFocusView(View targetView, bool isFirstFocus)
		{
			if (this.ViewFocus != null && this.ViewFocus.Equals(targetView))
			{
				if (this.mKeyController != null)
				{
					this.ViewFocus.Hover();
				}
				return;
			}
			if (this.ViewFocus != null)
			{
				this.ViewFocus.RemoveHover();
			}
			this.ViewFocus = targetView;
			if (this.ViewFocus != null)
			{
				if (this.mKeyController == null)
				{
					this.ViewFocus.StopBlink();
				}
				else
				{
					this.ViewFocus.Hover();
				}
				this.CursolIndex = this.ViewFocus.SortIndex - this.RollCount;
				if (isFirstFocus)
				{
					this.OnAction(ActionType.OnChangeFirstFocus, this, this.ViewFocus);
				}
				else
				{
					this.OnAction(ActionType.OnChangeFocus, this, this.ViewFocus);
				}
			}
		}

		private void RefreshPosition(float distanceX, float distanceY)
		{
			if (distanceY < 0f && this.RollCount <= 0)
			{
				this.Relocation(distanceX, distanceY);
				return;
			}
			if (0f < distanceY && this.GetModelSize() <= this.GetIndexUseViewingBottom() + 1)
			{
				this.Relocation(distanceX, distanceY);
				return;
			}
			if (0f < distanceY)
			{
				this.RefreshUpSwipeedPostion(distanceX, distanceY);
			}
			else if (distanceY < 0f)
			{
				this.RefreshDownSwipedPosition(distanceX, distanceY);
			}
		}

		private void RefreshUpSwipeedPostion(float distanceX, float distanceY)
		{
			this.Relocation(distanceX, distanceY);
			View[] childrenViews = this.GetChildrenViews();
			View[] array = childrenViews;
			for (int i = 0; i < array.Length; i++)
			{
				View view = array[i];
				bool flag = this.IsInDestroyArea(view, DestroyAreaType.Top);
				if (flag)
				{
					this.RollCount++;
					int currentPage = Array.IndexOf<View>(childrenViews, view);
					int loopIndex = this.GetLoopIndex(currentPage, this.GetLogicViewingLength(), -1);
					Vector3 localPosition = view.GenerateConnectToPosition(childrenViews[loopIndex], ConnectType.Bottom);
					view.cachedTransform.set_localPosition(localPosition);
					int num = this.RollCount + childrenViews.Length - 1;
					Model model = (Model)((object)null);
					if (num < this.Models.Length)
					{
						model = this.Models[num];
					}
					this.OnUpdateChild(num, view, model);
				}
			}
		}

		private void RefreshDownSwipedPosition(float distanceX, float distanceY)
		{
			View[] childrenViews = this.GetChildrenViews();
			this.Relocation(distanceX, distanceY);
			int num = childrenViews.Length - 1;
			while (0 <= num)
			{
				View view = childrenViews[num];
				bool flag = this.IsInDestroyArea(view, DestroyAreaType.Botton);
				if (flag)
				{
					this.RollCount--;
					int currentPage = Array.IndexOf<View>(childrenViews, view);
					int loopIndex = this.GetLoopIndex(currentPage, this.GetLogicViewingLength(), 1);
					Vector3 localPosition = view.GenerateConnectToPosition(childrenViews[loopIndex], ConnectType.Top);
					view.cachedTransform.set_localPosition(localPosition);
					int rollCount = this.RollCount;
					Model model = (Model)((object)null);
					if (rollCount >= 0 && rollCount < this.Models.Length)
					{
						model = this.Models[rollCount];
					}
					this.OnUpdateChild(rollCount, view, model);
				}
				num--;
			}
		}

		private void RelocationWithAnimation()
		{
			Sequence sequence = TweenSettingsExtensions.SetId<Sequence>(DOTween.Sequence(), this);
			View[] childrenViews = this.GetChildrenViews();
			for (int i = 0; i < childrenViews.Length; i++)
			{
				View view = childrenViews[i];
				int loopIndex = this.GetLoopIndex(view.SortIndex, this.GetLogicViewingLength(), -this.RollCount);
				Tween tween = ShortcutExtensions.DOLocalMove(view.cachedTransform, this.ViewsDefaultLocalPosition[loopIndex], 0.3f, false);
				TweenSettingsExtensions.Join(sequence, tween);
			}
			this.AnimationViewPositionNow = true;
			TweenSettingsExtensions.OnComplete<Sequence>(sequence, delegate
			{
				if (this.mUseBottomUpReposition)
				{
					this.BottomDown();
				}
				this.AnimationViewPositionNow = false;
			});
		}

		private void Relocation(float distanceX, float distanceY)
		{
			View[] childrenViews = this.GetChildrenViews();
			for (int i = 0; i < childrenViews.Length; i++)
			{
				View view = childrenViews[i];
				float x = view.cachedTransform.get_localPosition().x;
				float num = view.cachedTransform.get_localPosition().y + distanceY;
				float z = view.cachedTransform.get_localPosition().z;
				this.mVector3_RelocationCache.Set(x, num, z);
				view.cachedTransform.set_localPosition(this.mVector3_RelocationCache);
			}
		}

		private bool IsInDestroyArea(View target, DestroyAreaType type)
		{
			if (type != DestroyAreaType.Top)
			{
				return type == DestroyAreaType.Botton && target.get_transform().get_localPosition().y < this.mVector3BottomDestroyLine.y;
			}
			return this.mVector3TopDestroyLine.y <= target.get_transform().get_localPosition().y;
		}

		private void OnUpdateChild(int nextIndex, View child, Model model)
		{
			child.UpdateIndex(nextIndex);
			if (model != null)
			{
				bool clickable = this.OnCheck(CheckType.Clickable, this, model);
				child.Initialize(model, clickable);
				child.Show();
			}
			else
			{
				child.ReleaseModel();
				child.Hide();
			}
		}

		private int GetReliminaryLength()
		{
			return this.MAX_RELIMINARY_OBJECTS;
		}

		private int GetLogicViewingLength()
		{
			return this.MaxLogicViewObjects;
		}

		private int GetUserViewingLength()
		{
			return this.MAX_USER_VIEW_OBJECTS;
		}

		private int GetLoopIndex(int currentPage, int pageLength, int step)
		{
			return (currentPage + pageLength + step % pageLength) % pageLength;
		}

		private int GetIndexUseViewingTop()
		{
			return this.RollCount;
		}

		private int GetIndexLogicViewingTop()
		{
			return this.RollCount;
		}

		private int GetIndexUseViewingBottom()
		{
			if (this.GetUserViewingLength() < this.GetReliminaryLength())
			{
				return this.RollCount + (this.GetLogicViewingLength() - (this.GetLogicViewingLength() - this.GetUserViewingLength())) - 1;
			}
			return this.RollCount + (this.GetLogicViewingLength() - (this.GetLogicViewingLength() - this.GetUserViewingLength())) - 1;
		}

		private int GetIndexLogicViewingBottom()
		{
			return this.RollCount + this.GetChildrenViewsCount();
		}

		private float GetChildHeight()
		{
			return this.mGridContaints.cellHeight;
		}

		private View[] GetChildrenViews()
		{
			return this.Views;
		}

		private int GetChildrenViewsCount()
		{
			return this.GetChildrenViews().Length;
		}

		private void UIDisplaySwipeEventRegionDelegate(UIDisplaySwipeEventRegion.ActionType actionType, float deltaX, float deltaY, float movedPercentageX, float movedPercentageY, float elapsedTime)
		{
			if (!this.AnimationViewPositionNow && !this.AnimationScrollNow && this.EnableTouchControl)
			{
				float childHeight = this.GetChildHeight();
				if (childHeight < Math.Abs(deltaY))
				{
					if (0.3f < deltaY)
					{
						deltaY = childHeight;
					}
					else if (deltaY < -0.3f)
					{
						deltaY = -childHeight;
					}
				}
				switch (actionType)
				{
				case UIDisplaySwipeEventRegion.ActionType.Moving:
					this.CursolIndex = 0;
					this.RefreshPosition(deltaX, deltaY);
					break;
				case UIDisplaySwipeEventRegion.ActionType.FingerUp:
				{
					this.RelocationWithAnimation();
					int loopIndex = this.GetLoopIndex(this.RollCount, this.GetLogicViewingLength(), 0);
					if (loopIndex >= 0 && loopIndex < this.Views.Length)
					{
						this.ChangeFocusView(this.Views[loopIndex], false);
					}
					break;
				}
				}
			}
		}

		private void MoveByView(int moveCount)
		{
			if (!this.AnimationViewPositionNow && !this.AnimationScrollNow && 0 < moveCount)
			{
				this.AnimationScrollNow = true;
				this.SwipeToBottomCoroutine(moveCount, delegate
				{
					this.ChangeFocusView(this.Views[this.GetLoopIndex(this.RollCount, this.GetLogicViewingLength(), 0)], false);
				});
			}
			else if (!this.AnimationViewPositionNow && !this.AnimationScrollNow && moveCount < 0)
			{
				Debug.Log("Move By View else if ::" + moveCount);
				this.AnimationScrollNow = true;
				this.SwipeToTopCoroutine(Math.Abs(moveCount), delegate
				{
					this.ChangeFocusView(this.Views[this.GetLoopIndex(this.RollCount, this.GetLogicViewingLength(), 0)], false);
				});
			}
		}

		private void SwipeToTopCoroutine(int moveCount, Action onFinished)
		{
			int num = this.RollCount - moveCount;
			float num2 = -this.GetChildHeight();
			while (num < this.RollCount && 0 < this.RollCount)
			{
				this.RefreshPosition(0f, num2 / 2f);
			}
			this.RelocationWithAnimation();
			this.AnimationScrollNow = false;
			if (onFinished != null)
			{
				onFinished.Invoke();
			}
		}

		private void SwipeToBottomCoroutine(int moveCount, Action onFinished)
		{
			int num = this.RollCount + moveCount;
			float childHeight = this.GetChildHeight();
			while (this.RollCount < num && this.RollCount + this.GetLogicViewingLength() <= this.GetModelSize())
			{
				this.RefreshPosition(0f, childHeight / 2f);
			}
			this.RelocationWithAnimation();
			this.AnimationScrollNow = false;
			if (onFinished != null)
			{
				onFinished.Invoke();
			}
		}

		protected virtual View GetViewPrefab()
		{
			return this.mPrefab_UIScrollListChild;
		}

		private void OnDestroy()
		{
			bool flag = DOTween.IsTweening(this);
			if (flag)
			{
				DOTween.Kill(this, false);
			}
			this.Models = null;
			this.ViewsDefaultLocalPosition = null;
			this.Views = null;
			this.ViewFocus = (View)((object)null);
			this.mPrefab_UIScrollListChild = (View)((object)null);
			this.mUIDisplaySwipeEventRegion = null;
			this.mGridContaints = null;
			this.mUIScrollListParentAction = null;
			this.mUIScrollListParentCheck = null;
			this.mKeyController = null;
		}
	}
}
