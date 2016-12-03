using DG.Tweening;
using KCV.Display;
using System;
using System.Linq;
using UnityEngine;

namespace KCV.View.ScrollView
{
	public abstract class UIScrollList<Model, View> : MonoBehaviour where Model : class where View : MonoBehaviour, UIScrollListItem<Model, View>
	{
		public enum ScrollDirection
		{
			Heaven,
			Hell
		}

		public enum ListState
		{
			Default,
			MoveFromKey,
			MoveFromFinger,
			MoveFromFingerScrolling,
			Waiting,
			Lock
		}

		private enum ViewModelValueState
		{
			EvenUserViewAndModel,
			EvenWorkSpaceViewAndModel,
			BiggerUserView,
			BiggerWorkSpaceView,
			BiggerModel,
			Default
		}

		public enum ContentDirection
		{
			Heaven,
			Hell
		}

		private enum AnimationType
		{
			ListMoveFromFingerScroll,
			RepositionForScroll,
			ContainerMove
		}

		[SerializeField]
		private float mBottomUpContentPosition;

		[SerializeField]
		private float mBottomDownContentPosition;

		[SerializeField]
		protected int mUserViewCount;

		[SerializeField]
		protected Transform mTransform_ContentPosition;

		private UIPanel mPanelContainer;

		[SerializeField]
		protected View[] mViews;

		protected View[] mViews_WorkSpace;

		[SerializeField]
		private UIDisplaySwipeEventRegion mUIDisplaySwipeEventRegion;

		[SerializeField]
		private Ease mListMoveEaseType = 1;

		[SerializeField]
		private float MOVE_ITEM_LEVEL = 10f;

		public float FRAME_MOVE_SECOND = 0.8f;

		private UIScrollList<Model, View>.ContentDirection mContentDirection = UIScrollList<Model, View>.ContentDirection.Hell;

		private float mOuterHead;

		private float mOuterTail;

		private Vector3 mLastTouchedPosition;

		protected View mCurrentFocusView;

		protected float mScrollCheckLevel = 300f;

		protected Model[] mModels;

		private Vector3[] mViewDefaultPositions;

		protected UIScrollList<Model, View>.ListState mState
		{
			get;
			private set;
		}

		protected UIScrollList<Model, View>.ListState GetListState()
		{
			return this.mState;
		}

		protected void LockControl()
		{
			this.ChangeState(UIScrollList<Model, View>.ListState.Lock);
		}

		protected void StartControl()
		{
			this.ChangeState(UIScrollList<Model, View>.ListState.Waiting);
		}

		protected virtual void OnChangedFocusView(View focusToView)
		{
		}

		protected virtual void OnAwake()
		{
		}

		protected virtual void OnStart()
		{
		}

		protected virtual void OnCallDestroy()
		{
		}

		protected virtual void OnUpdate()
		{
		}

		protected virtual bool OnSelectable(View view)
		{
			return true;
		}

		protected virtual void OnSelect(View view)
		{
		}

		private void Awake()
		{
			this.mPanelContainer = this.mTransform_ContentPosition.get_parent().GetComponent<UIPanel>();
			this.MemoryViewRangePositions();
			this.MemoryViewsDefaultPosition();
			this.SettingViewListeners();
			this.mUIDisplaySwipeEventRegion.SetOnSwipeActionJudgeCallBack(new UIDisplaySwipeEventRegion.SwipeJudgeDelegate(this.OnDisplayEvents), true);
			this.OnAwake();
		}

		public void StartStaticWidgetChildren()
		{
			if (this.mPanelContainer == null)
			{
				this.mPanelContainer = this.mTransform_ContentPosition.get_parent().GetComponent<UIPanel>();
			}
			if (this.mPanelContainer != null)
			{
				this.mPanelContainer.widgetsAreStatic = true;
			}
		}

		public void StopStaticWidgetChildren()
		{
			if (this.mPanelContainer == null)
			{
				this.mPanelContainer = this.mTransform_ContentPosition.get_parent().GetComponent<UIPanel>();
			}
			if (this.mPanelContainer != null)
			{
				this.mPanelContainer.widgetsAreStatic = false;
			}
		}

		private void Start()
		{
			this.OnStart();
		}

		private void OnDestroy()
		{
			this.OnCallDestroy();
			this.KillAnimationContainerMove();
			this.KillAnimationListMoveFromFingerScroll();
			this.KillAnimationAndForceCompleteRepositionForScroll();
			this.ReleaseMemberViews();
			this.ReleaseWorksSpaceViews();
			this.mTransform_ContentPosition = null;
			this.mUIDisplaySwipeEventRegion = null;
			this.mCurrentFocusView = (View)((object)null);
			this.mModels = null;
			this.mPanelContainer = null;
		}

		private void Update()
		{
			this.OnUpdate();
		}

		public void HeadFocus()
		{
			UIScrollList<Model, View>.ContentDirection contentDirection = this.mContentDirection;
			if (contentDirection != UIScrollList<Model, View>.ContentDirection.Heaven)
			{
				if (contentDirection == UIScrollList<Model, View>.ContentDirection.Hell)
				{
					if (this.mViewDefaultPositions[0].y < this.mViews_WorkSpace[0].GetTransform().get_localPosition().y)
					{
						this.ChangeFocusView(this.mViews_WorkSpace[1]);
					}
					else
					{
						this.ChangeFocusView(this.mViews_WorkSpace[0]);
					}
				}
			}
			else
			{
				this.ChangeFocusView(this.mViews_WorkSpace[1]);
			}
		}

		public void RemoveFocus()
		{
			this.ChangeFocusView((View)((object)null));
		}

		public void ChangeHeadPage()
		{
			View[] array = new View[this.mViews.Length];
			for (int i = 0; i < this.mViews.Length; i++)
			{
				View view = this.mViews[i];
				if (i < this.mModels.Length)
				{
					view.Initialize(i, this.mModels[i]);
				}
				else
				{
					view.InitializeDefault(i);
				}
				view.GetTransform().set_localPosition(this.mViewDefaultPositions[i]);
				array[i] = view;
			}
			this.mViews_WorkSpace = array;
		}

		public void ChangePageFromModel(Model model)
		{
			if (this.mModels == null || this.mModels.Length == 0)
			{
				this.ChangeContentPosition(UIScrollList<Model, View>.ContentDirection.Hell);
				return;
			}
			if (this.mModels.Length - 1 == ((!(this.mCurrentFocusView != null)) ? -1 : this.mCurrentFocusView.GetRealIndex()))
			{
				this.ChangeContentPosition(UIScrollList<Model, View>.ContentDirection.Heaven);
			}
			else
			{
				this.ChangeContentPosition(UIScrollList<Model, View>.ContentDirection.Hell);
			}
			int modelIndex = this.GetModelIndex(model);
			if (-1 < modelIndex)
			{
				if (modelIndex + this.mUserViewCount < this.mModels.Length)
				{
					View[] array = new View[this.mViews.Length];
					for (int i = 0; i < this.mViews.Length; i++)
					{
						View view = this.mViews[i];
						int num = modelIndex + i;
						if (num < this.mModels.Length)
						{
							view.Initialize(num, this.mModels[num]);
						}
						else
						{
							view.InitializeDefault(num);
						}
						view.GetTransform().set_localPosition(this.mViewDefaultPositions[i]);
						array[i] = view;
					}
					this.mViews_WorkSpace = array;
					View[] array2 = this.mViews;
					for (int j = 0; j < array2.Length; j++)
					{
						View view2 = array2[j];
						if (this.EqualsModel(model, view2.GetModel()))
						{
							this.ChangeFocusView(view2);
						}
					}
				}
				else
				{
					this.ChangeTailPage();
					View[] array3 = this.mViews;
					for (int k = 0; k < array3.Length; k++)
					{
						View view3 = array3[k];
						if (this.EqualsModel(model, view3.GetModel()))
						{
							this.ChangeFocusView(view3);
						}
					}
				}
			}
			else
			{
				this.ChangeHeadPage();
			}
		}

		protected virtual bool EqualsModel(Model targetA, Model targetB)
		{
			return targetA != null && targetB != null && targetA.Equals(targetB);
		}

		protected virtual int GetModelIndex(Model model)
		{
			return Array.IndexOf<Model>(this.mModels, model);
		}

		public void TailFocusForOnFinishedScroll()
		{
			UIScrollList<Model, View>.ListState mState = this.mState;
			this.ChangeState(UIScrollList<Model, View>.ListState.Waiting);
			int num = this.mUserViewCount;
			int num2 = this.mModels.Length;
			if (num == num2)
			{
				this.ChangeHeadPage();
				this.ChangeFocusView(this.mViews_WorkSpace[this.mModels.Length - 1]);
				if (this.mCurrentFocusView.GetRealIndex() == this.mUserViewCount - 1)
				{
					this.ChangeImmediateContentPosition(UIScrollList<Model, View>.ContentDirection.Heaven);
				}
				else
				{
					this.ChangeImmediateContentPosition(UIScrollList<Model, View>.ContentDirection.Hell);
				}
			}
			else if (num < num2)
			{
				this.ChangeImmediateContentPosition(UIScrollList<Model, View>.ContentDirection.Heaven);
				this.ChangeFocusToUserViewTail();
			}
			else if (num2 < num)
			{
				this.ChangeHeadPage();
				this.ChangeFocusView(this.mViews_WorkSpace[this.mModels.Length - 1]);
				this.ChangeImmediateContentPosition(UIScrollList<Model, View>.ContentDirection.Hell);
			}
			this.ChangeState(mState);
		}

		public void TailFocus()
		{
			UIScrollList<Model, View>.ListState mState = this.mState;
			this.ChangeState(UIScrollList<Model, View>.ListState.Waiting);
			int num = this.mUserViewCount;
			int num2 = this.mModels.Length;
			if (num == num2)
			{
				this.ChangeHeadPage();
				this.ChangeFocusView(this.mViews_WorkSpace[this.mModels.Length - 1]);
				if (this.mCurrentFocusView.GetRealIndex() == this.mUserViewCount - 1)
				{
					this.ChangeImmediateContentPosition(UIScrollList<Model, View>.ContentDirection.Heaven);
				}
				else
				{
					this.ChangeImmediateContentPosition(UIScrollList<Model, View>.ContentDirection.Hell);
				}
			}
			else if (num < num2)
			{
				this.ChangeImmediateContentPosition(UIScrollList<Model, View>.ContentDirection.Heaven);
				this.ChangeTailPage();
				this.ChangeFocusToUserViewTail();
			}
			else if (num2 < num)
			{
				this.ChangeHeadPage();
				this.ChangeFocusView(this.mViews_WorkSpace[this.mModels.Length - 1]);
				this.ChangeImmediateContentPosition(UIScrollList<Model, View>.ContentDirection.Hell);
			}
			this.ChangeState(mState);
		}

		public void TailFocusPage()
		{
			UIScrollList<Model, View>.ListState mState = this.mState;
			this.ChangeState(UIScrollList<Model, View>.ListState.Waiting);
			int num = this.mUserViewCount;
			int num2 = this.mModels.Length;
			if (num == num2)
			{
				this.ChangeHeadPage();
				if (this.mCurrentFocusView.GetRealIndex() == this.mUserViewCount - 1)
				{
					this.ChangeImmediateContentPosition(UIScrollList<Model, View>.ContentDirection.Heaven);
				}
				else
				{
					this.ChangeImmediateContentPosition(UIScrollList<Model, View>.ContentDirection.Hell);
				}
			}
			else if (num < num2)
			{
				this.ChangeImmediateContentPosition(UIScrollList<Model, View>.ContentDirection.Heaven);
				this.ChangeTailPage();
			}
			else if (num2 < num)
			{
				this.ChangeHeadPage();
				this.ChangeImmediateContentPosition(UIScrollList<Model, View>.ContentDirection.Hell);
			}
			this.ChangeState(mState);
		}

		public void TailFocusMember()
		{
			UIScrollList<Model, View>.ViewModelValueState viewModelValueState = this.CompareUserViewModelValue();
			UIScrollList<Model, View>.ViewModelValueState viewModelValueState2 = viewModelValueState;
			if (viewModelValueState2 != UIScrollList<Model, View>.ViewModelValueState.BiggerModel)
			{
				this.ChangeFocusView((View)((object)null));
				this.mCurrentFocusView = this.mViews_WorkSpace[this.mModels.Length - 1];
			}
			else
			{
				this.ChangeFocusView((View)((object)null));
				this.mCurrentFocusView = this.mViews_WorkSpace[this.mUserViewCount - 1];
			}
		}

		protected void ResumeFocus()
		{
			this.ChangeState(UIScrollList<Model, View>.ListState.Waiting);
			this.ChangeFocusView(this.mCurrentFocusView);
		}

		protected void Refresh(Model[] models, bool firstPage)
		{
			int arg_19_0 = (this.mModels == null) ? 0 : this.mModels.Length;
			int arg_29_0 = (models == null) ? 0 : models.Length;
			int arg_57_0 = (!(this.mCurrentFocusView != null)) ? 0 : this.mCurrentFocusView.GetRealIndex();
			this.mModels = models;
			if (firstPage)
			{
				this.ChangeHeadPage();
				this.ChangeFocusToUserViewHead();
			}
			else if (this.mCurrentFocusView != null && this.mCurrentFocusView.GetModel() != null)
			{
				Model[] array = this.mModels;
				for (int i = 0; i < array.Length; i++)
				{
					Model targetB = array[i];
					bool flag = this.EqualsModel(this.mCurrentFocusView.GetModel(), targetB);
					if (flag)
					{
						this.ChangeState(UIScrollList<Model, View>.ListState.Waiting);
						this.ChangePageFromModel(this.mCurrentFocusView.GetModel());
						this.ChangeState(UIScrollList<Model, View>.ListState.Lock);
						int num = Array.IndexOf<Model>(this.mModels, this.mCurrentFocusView.GetModel());
						break;
					}
				}
			}
		}

		protected void RefreshViews()
		{
			View[] array = this.mViews_WorkSpace;
			for (int i = 0; i < array.Length; i++)
			{
				View view = array[i];
				int realIndex = view.GetRealIndex();
				if (realIndex < this.mModels.Length)
				{
					view.Initialize(realIndex, this.mModels[realIndex]);
				}
				else
				{
					view.InitializeDefault(realIndex);
				}
			}
		}

		protected void Initialize(Model[] models)
		{
			this.mModels = models;
			this.mViews_WorkSpace = this.mViews;
			this.mContentDirection = UIScrollList<Model, View>.ContentDirection.Hell;
			this.ChangeState(UIScrollList<Model, View>.ListState.Lock);
			this.KillAnimationListMoveFromFingerScroll();
			for (int i = 0; i < this.mViews.Length; i++)
			{
				this.mViews[i].GetTransform().set_localPosition(this.mViewDefaultPositions[i]);
			}
			this.RemoveFocus();
			int num = 0;
			View[] array = this.mViews_WorkSpace;
			for (int j = 0; j < array.Length; j++)
			{
				View view = array[j];
				if (num < this.mModels.Length)
				{
					Model model = this.mModels[num];
					view.Initialize(num, model);
				}
				else
				{
					view.InitializeDefault(num);
				}
				num++;
			}
			this.ChangeContentPosition(UIScrollList<Model, View>.ContentDirection.Hell);
			this.LockControl();
		}

		protected void Select()
		{
			if (this.mState != UIScrollList<Model, View>.ListState.Waiting)
			{
				return;
			}
			if (this.mCurrentFocusView == null)
			{
				return;
			}
			bool flag = this.OnSelectable(this.mCurrentFocusView);
			if (flag)
			{
				this.OnSelect(this.mCurrentFocusView);
			}
		}

		protected void NextFocus()
		{
			bool flag = this.mState == UIScrollList<Model, View>.ListState.Waiting;
			if (flag)
			{
				if (this.mModels == null || this.mModels.Length == 0)
				{
					return;
				}
				bool flag2 = this.mCurrentFocusView != null;
				if (flag2)
				{
					this.RepositionNow();
					if (!this.ChangeNextFocus())
					{
						this.ChangeContentPosition(UIScrollList<Model, View>.ContentDirection.Hell);
						this.ChangeHeadPage();
						this.ChangeFocusToUserViewHead();
					}
				}
			}
		}

		public void RepositionImmediate()
		{
			this.PlayAnimationRepositionForScroll(null);
		}

		protected void PrevFocus()
		{
			bool flag = this.mState == UIScrollList<Model, View>.ListState.Waiting;
			if (flag)
			{
				if (this.mModels == null || this.mModels.Length == 0)
				{
					return;
				}
				bool flag2 = this.mCurrentFocusView != null;
				if (flag2)
				{
					this.RepositionNow();
					if (!this.ChangePrevViewFocus())
					{
						if (this.mUserViewCount <= this.mModels.Length)
						{
							this.ChangeContentPosition(UIScrollList<Model, View>.ContentDirection.Heaven);
						}
						else
						{
							this.ChangeContentPosition(UIScrollList<Model, View>.ContentDirection.Hell);
						}
						this.ChangeTailPage();
						this.ChangeFocusToUserViewTail();
					}
				}
			}
		}

		protected void SetSwipeEventCamera(Camera camera)
		{
			this.mUIDisplaySwipeEventRegion.SetEventCatchCamera(camera);
		}

		protected void ChangeFocusToUserViewHead()
		{
			this.ChangeFocusView(this.mViews_WorkSpace[0]);
		}

		protected void PrevPageOrHeadFocus()
		{
			if (this.mState != UIScrollList<Model, View>.ListState.Waiting)
			{
				return;
			}
			if (this.mModels == null || this.mModels.Length == 0)
			{
				return;
			}
			this.RepositionNow();
			if (this.mViews_WorkSpace[0] != this.mCurrentFocusView)
			{
				this.ChangeContentPosition(UIScrollList<Model, View>.ContentDirection.Hell);
				this.ChangeFocusView(this.mViews_WorkSpace[0]);
			}
			else
			{
				this.ChangePage(this.mUserViewCount, -1);
				this.ChangeFocusView(this.mViews_WorkSpace[0]);
			}
		}

		protected void NextPageOrTailFocus()
		{
			if (this.mState != UIScrollList<Model, View>.ListState.Waiting)
			{
				return;
			}
			if (this.mModels == null || this.mModels.Length == 0)
			{
				return;
			}
			this.RepositionNow();
			if (this.mViews_WorkSpace[this.mUserViewCount - 1] != this.mCurrentFocusView && this.mUserViewCount <= this.mModels.Length)
			{
				this.ChangeContentPosition(UIScrollList<Model, View>.ContentDirection.Heaven);
				this.ChangeFocusView(this.mViews_WorkSpace[this.mUserViewCount - 1]);
			}
			else
			{
				bool flag = this.ChangePage(this.mUserViewCount, 1);
				if (flag)
				{
					this.ChangeContentPosition(UIScrollList<Model, View>.ContentDirection.Heaven);
					this.ChangeFocusView(this.mViews_WorkSpace[this.mUserViewCount - 1]);
				}
				else if (this.mUserViewCount < this.mModels.Length)
				{
					this.ChangeContentPosition(UIScrollList<Model, View>.ContentDirection.Heaven);
					this.ChangeFocusView(this.mViews_WorkSpace[this.mUserViewCount - 1]);
				}
				else if (this.mModels.Length == 1)
				{
					this.ChangeContentPosition(UIScrollList<Model, View>.ContentDirection.Hell);
					this.ChangeFocusView(this.mViews_WorkSpace[0]);
				}
				else
				{
					UIScrollList<Model, View>.ViewModelValueState viewModelValueState = this.CompareUserViewModelValue();
					if (viewModelValueState == UIScrollList<Model, View>.ViewModelValueState.BiggerModel)
					{
						this.ChangeContentPosition(UIScrollList<Model, View>.ContentDirection.Heaven);
					}
					else if (this.mUserViewCount < this.mModels.Length)
					{
						this.ChangeContentPosition(UIScrollList<Model, View>.ContentDirection.Hell);
					}
					else
					{
						this.ChangeContentPosition(UIScrollList<Model, View>.ContentDirection.Heaven);
					}
					this.ChangeFocusView(this.mViews_WorkSpace[this.mModels.Length - 1]);
				}
			}
		}

		private void ChangeContentPosition(UIScrollList<Model, View>.ContentDirection contentPosition)
		{
			if (this.mContentDirection == contentPosition)
			{
				return;
			}
			if (this.mModels == null)
			{
				return;
			}
			if (this.mModels.Length < this.mUserViewCount && contentPosition == UIScrollList<Model, View>.ContentDirection.Heaven)
			{
				return;
			}
			this.mContentDirection = contentPosition;
			this.KillAnimationContainerMove();
			UIScrollList<Model, View>.ContentDirection contentDirection = this.mContentDirection;
			if (contentDirection != UIScrollList<Model, View>.ContentDirection.Heaven)
			{
				if (contentDirection == UIScrollList<Model, View>.ContentDirection.Hell)
				{
					TweenSettingsExtensions.SetId<Tweener>(ShortcutExtensions.DOLocalMoveY(this.mTransform_ContentPosition, this.mBottomDownContentPosition, 0.15f, false), UIScrollList<Model, View>.AnimationType.ContainerMove);
				}
			}
			else
			{
				TweenSettingsExtensions.SetId<Tweener>(ShortcutExtensions.DOLocalMoveY(this.mTransform_ContentPosition, this.mBottomUpContentPosition, 0.15f, false), UIScrollList<Model, View>.AnimationType.ContainerMove);
			}
		}

		protected void ChangeImmediateContentPosition(UIScrollList<Model, View>.ContentDirection contentPosition)
		{
			this.KillAnimationContainerMove();
			this.mContentDirection = contentPosition;
			UIScrollList<Model, View>.ContentDirection contentDirection = this.mContentDirection;
			if (contentDirection != UIScrollList<Model, View>.ContentDirection.Heaven)
			{
				if (contentDirection == UIScrollList<Model, View>.ContentDirection.Hell)
				{
					this.mTransform_ContentPosition.localPositionY(this.mBottomDownContentPosition);
				}
			}
			else
			{
				this.mTransform_ContentPosition.localPositionY(this.mBottomUpContentPosition);
			}
		}

		private void SettingViewListeners()
		{
			View[] array = this.mViews;
			for (int i = 0; i < array.Length; i++)
			{
				View view = array[i];
				view.SetOnTouchListener(new Action<View>(this.OnTouchListener));
			}
		}

		private void HeadFocusMember()
		{
			UIScrollList<Model, View>.ContentDirection contentDirection = this.mContentDirection;
			if (contentDirection != UIScrollList<Model, View>.ContentDirection.Heaven)
			{
				if (contentDirection == UIScrollList<Model, View>.ContentDirection.Hell)
				{
					if (this.mViewDefaultPositions[0].y < this.mViews_WorkSpace[0].GetTransform().get_localPosition().y)
					{
						this.ChangeFocusView((View)((object)null));
						this.mCurrentFocusView = this.mViews_WorkSpace[1];
					}
					else
					{
						this.ChangeFocusView((View)((object)null));
						this.mCurrentFocusView = this.mViews_WorkSpace[0];
					}
				}
			}
			else
			{
				this.ChangeFocusView(this.mViews_WorkSpace[1]);
			}
		}

		private void ReleaseMemberViews()
		{
			if (this.mViews != null)
			{
				for (int i = 0; i < this.mViews.Length; i++)
				{
					this.mViews[i] = (View)((object)null);
				}
			}
			this.mViews = null;
		}

		private void ReleaseWorksSpaceViews()
		{
			if (this.mViews_WorkSpace != null)
			{
				for (int i = 0; i < this.mViews_WorkSpace.Length; i++)
				{
					this.mViews_WorkSpace[i] = (View)((object)null);
				}
			}
			this.mViews_WorkSpace = null;
		}

		private void MemoryViewsDefaultPosition()
		{
			this.mViewDefaultPositions = new Vector3[this.mViews.Length];
			for (int i = 0; i < this.mViews.Length; i++)
			{
				this.mViewDefaultPositions[i] = this.mViews[i].GetTransform().get_localPosition();
			}
		}

		private void MemoryViewRangePositions()
		{
			this.mOuterHead = (float)this.mViews[0].GetHeight();
			this.mOuterTail = (float)(-(float)(this.mViews[0].GetHeight() * this.mViews.Length));
		}

		private void OnDisplayEvents(UIDisplaySwipeEventRegion.ActionType actionType, float deltaX, float deltaY, float movePercentageX, float movePercentageY, float elapsedTime)
		{
			if (this.mState != UIScrollList<Model, View>.ListState.Waiting && this.mState != UIScrollList<Model, View>.ListState.MoveFromFinger && this.mState != UIScrollList<Model, View>.ListState.MoveFromFingerScrolling)
			{
				return;
			}
			if (this.mModels == null)
			{
				return;
			}
			if (this.mModels.Length == 0)
			{
				return;
			}
			switch (actionType)
			{
			case UIDisplaySwipeEventRegion.ActionType.Start:
				this.OnPressEvent(UICamera.lastTouchPosition, true, false);
				break;
			case UIDisplaySwipeEventRegion.ActionType.Moving:
				this.OnDragging(new Vector2(deltaX, deltaY));
				break;
			case UIDisplaySwipeEventRegion.ActionType.FingerUp:
				this.OnPressEvent(UICamera.lastTouchPosition, false, false);
				break;
			case UIDisplaySwipeEventRegion.ActionType.FingerUpWithVerticalFlick:
				this.OnPressEvent(UICamera.lastTouchPosition, false, true);
				break;
			}
		}

		private void OnTouchListener(View view)
		{
			if (this.mState != UIScrollList<Model, View>.ListState.Waiting)
			{
				return;
			}
			UIScrollList<Model, View>.ContentDirection contentDirection = this.mContentDirection;
			if (contentDirection != UIScrollList<Model, View>.ContentDirection.Heaven)
			{
				if (contentDirection == UIScrollList<Model, View>.ContentDirection.Hell)
				{
					float num = view.get_transform().get_localPosition().y - (float)view.GetHeight();
					float num2 = (float)(-(float)(view.GetHeight() * this.mUserViewCount));
					if (num <= num2)
					{
						return;
					}
				}
			}
			else
			{
				float y = view.get_transform().get_localPosition().y;
				float num3 = this.mBottomUpContentPosition + y;
				if (0f < num3)
				{
					return;
				}
			}
			this.ChangeFocusView(view);
			this.Select();
		}

		private Tween PlayAnimationListMoveFromFingerScroll(float scrollPower, Action onFinished)
		{
			UIScrollList<Model, View>.ScrollDirection scrollDirection = this.CheckDirection(scrollPower);
			float num = (float)this.mViews[0].GetHeight() * this.MOVE_ITEM_LEVEL;
			float movedDistance = 0f;
			return TweenSettingsExtensions.SetId<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.OnComplete<Tweener>(DOVirtual.Float(0f, num, this.FRAME_MOVE_SECOND, delegate(float moveDistance)
			{
				float num2 = moveDistance - movedDistance;
				UIScrollList<Model, View>.ScrollDirection scrollDirection = scrollDirection;
				if (scrollDirection != UIScrollList<Model, View>.ScrollDirection.Heaven)
				{
					if (scrollDirection == UIScrollList<Model, View>.ScrollDirection.Hell)
					{
						this.MoveToHellFromFingerFlick(-num2);
					}
				}
				else
				{
					this.MoveToHeavenFromFingerFlick(num2);
				}
				movedDistance = moveDistance;
			}), delegate
			{
				if (onFinished != null)
				{
					onFinished.Invoke();
				}
			}), this.mListMoveEaseType), UIScrollList<Model, View>.AnimationType.ListMoveFromFingerScroll);
		}

		private void MoveToHellFromFingerFlick(float scrollDistance)
		{
			bool flag = this.mViews_WorkSpace[0].GetRealIndex() == 0;
			if (flag)
			{
				UIScrollList<Model, View>.ContentDirection contentPosition = this.CheckContentDirection(scrollDistance);
				if (this.mUserViewCount < this.mModels.Length)
				{
					this.ChangeContentPosition(contentPosition);
				}
				else
				{
					this.ChangeContentPosition(UIScrollList<Model, View>.ContentDirection.Hell);
				}
				this.KillAnimationListMoveFromFingerScroll();
				this.PlayAnimationRepositionForScroll(new Action(this.OnFinishedAnimationRepositionForScroll));
			}
			else
			{
				UIScrollList<Model, View>.ScrollDirection scrollDirection = this.CheckDirection(scrollDistance);
				if (scrollDirection == UIScrollList<Model, View>.ScrollDirection.Heaven)
				{
					bool flag2 = this.mViews_WorkSpace[0].GetRealIndex() == 0;
					if (flag2)
					{
						this.PlayAnimationRepositionForScroll(new Action(this.OnFinishedAnimationRepositionForScroll));
					}
					else
					{
						this.HeadFocus();
						this.ChangeContentPosition(UIScrollList<Model, View>.ContentDirection.Heaven);
					}
				}
				else if (scrollDirection == UIScrollList<Model, View>.ScrollDirection.Hell)
				{
					this.ChangeContentPosition(UIScrollList<Model, View>.ContentDirection.Hell);
				}
				this.Scroll(scrollDistance);
			}
		}

		private void MoveToHeavenFromFingerFlick(float scrollDistance)
		{
			bool flag = this.mViews_WorkSpace.Length < this.mModels.Length;
			int num;
			if (flag)
			{
				num = this.mUserViewCount;
			}
			else if (this.mModels.Length == 1)
			{
				num = 0;
			}
			else if (this.mModels.Length < this.mUserViewCount)
			{
				num = this.mUserViewCount - 1;
			}
			else
			{
				num = this.mModels.Length - 1;
			}
			UIScrollList<Model, View>.ViewModelValueState viewModelValueState = this.CompareWorkSpaceViewModelValue();
			bool flag2 = false;
			UIScrollList<Model, View>.ViewModelValueState viewModelValueState2 = viewModelValueState;
			if (viewModelValueState2 != UIScrollList<Model, View>.ViewModelValueState.BiggerWorkSpaceView)
			{
				if (viewModelValueState2 == UIScrollList<Model, View>.ViewModelValueState.BiggerModel)
				{
					flag2 = (this.mModels.Length <= this.mViews_WorkSpace[num].GetRealIndex());
				}
			}
			else
			{
				flag2 = (this.mModels.Length <= this.mViews_WorkSpace[this.mUserViewCount].GetRealIndex());
			}
			if (flag2)
			{
				this.KillAnimationListMoveFromFingerScroll();
				this.PlayAnimationRepositionForScroll(new Action(this.OnFinishedAnimationRepositionForScroll));
			}
			else
			{
				UIScrollList<Model, View>.ScrollDirection scrollDirection = this.CheckDirection(scrollDistance);
				if (scrollDirection == UIScrollList<Model, View>.ScrollDirection.Heaven)
				{
					this.ChangeContentPosition(UIScrollList<Model, View>.ContentDirection.Heaven);
				}
				else if (scrollDirection == UIScrollList<Model, View>.ScrollDirection.Hell)
				{
					this.ChangeContentPosition(UIScrollList<Model, View>.ContentDirection.Hell);
				}
				this.Scroll(scrollDistance);
			}
		}

		protected void ChangeFocusView(View view)
		{
			if (this.mCurrentFocusView != null)
			{
				this.mCurrentFocusView.RemoveHover();
			}
			this.mCurrentFocusView = view;
			if (this.mCurrentFocusView != null)
			{
				this.mCurrentFocusView.Hover();
				this.OnChangedFocusView(this.mCurrentFocusView);
			}
		}

		public void StopFocusBlink()
		{
			if (this.mCurrentFocusView != null)
			{
				this.mCurrentFocusView.RemoveHover();
			}
		}

		public void StartFocusBlink()
		{
			View[] array = this.mViews;
			for (int i = 0; i < array.Length; i++)
			{
				View view = array[i];
				view.RemoveHover();
			}
			if (this.mCurrentFocusView != null)
			{
				this.mCurrentFocusView.Hover();
			}
		}

		protected void ChangeState(UIScrollList<Model, View>.ListState state)
		{
			this.mState = state;
		}

		private UIScrollList<Model, View>.ViewModelValueState CompareUserViewModelValue()
		{
			if (this.mModels.Length == this.mUserViewCount)
			{
				return UIScrollList<Model, View>.ViewModelValueState.EvenUserViewAndModel;
			}
			if (this.mUserViewCount < this.mModels.Length)
			{
				return UIScrollList<Model, View>.ViewModelValueState.BiggerModel;
			}
			if (this.mModels.Length < this.mUserViewCount)
			{
				return UIScrollList<Model, View>.ViewModelValueState.BiggerUserView;
			}
			return UIScrollList<Model, View>.ViewModelValueState.Default;
		}

		private UIScrollList<Model, View>.ViewModelValueState CompareWorkSpaceViewModelValue()
		{
			if (this.mModels.Length == this.mViews_WorkSpace.Length)
			{
				return UIScrollList<Model, View>.ViewModelValueState.EvenWorkSpaceViewAndModel;
			}
			if (this.mViews_WorkSpace.Length < this.mModels.Length)
			{
				return UIScrollList<Model, View>.ViewModelValueState.BiggerModel;
			}
			if (this.mModels.Length < this.mViews_WorkSpace.Length)
			{
				return UIScrollList<Model, View>.ViewModelValueState.BiggerWorkSpaceView;
			}
			return UIScrollList<Model, View>.ViewModelValueState.Default;
		}

		protected void ChangeTailPage()
		{
			UIScrollList<Model, View>.ViewModelValueState viewModelValueState = this.CompareUserViewModelValue();
			if (viewModelValueState != UIScrollList<Model, View>.ViewModelValueState.EvenUserViewAndModel)
			{
				bool flag = this.mUserViewCount < this.mModels.Length;
				if (flag)
				{
					for (int i = 0; i < this.mViews.Length; i++)
					{
						this.mViews[i].GetTransform().set_localPosition(this.mViewDefaultPositions[i]);
						int num = this.mModels.Length - this.mUserViewCount + i;
						if (num < this.mModels.Length)
						{
							Model model = this.mModels[num];
							this.mViews[i].Initialize(num, model);
						}
						else
						{
							this.mViews[i].InitializeDefault(num);
						}
					}
					this.mViews_WorkSpace = this.mViews;
				}
			}
		}

		private void ChangeFocusToUserViewTail()
		{
			if (this.mUserViewCount < this.mModels.Length)
			{
				this.ChangeFocusView(this.mViews_WorkSpace[this.mUserViewCount - 1]);
			}
			else
			{
				this.ChangeFocusView(this.mViews_WorkSpace[this.mModels.Length - 1]);
			}
		}

		private bool ChangeNextFocus()
		{
			int num = Array.IndexOf<View>(this.mViews_WorkSpace, this.mCurrentFocusView);
			int realIndex = this.mCurrentFocusView.GetRealIndex();
			int num2 = num + 1;
			int num3 = realIndex + 1;
			bool flag = num3 < this.mModels.Length;
			if (!flag)
			{
				return false;
			}
			bool flag2 = num2 < this.mUserViewCount;
			if (flag2)
			{
				bool flag3 = num2 == this.mUserViewCount - 1;
				if (flag3)
				{
					this.ChangeContentPosition(UIScrollList<Model, View>.ContentDirection.Heaven);
				}
				this.ChangeFocusView(this.mViews_WorkSpace[num2]);
				return true;
			}
			bool flag4 = this.Scroll(1, true);
			if (flag4)
			{
				View view = this.mViews_WorkSpace[this.mUserViewCount - 1];
				this.ChangeFocusView(view);
				return true;
			}
			View view2 = this.mViews_WorkSpace[this.mUserViewCount];
			this.ChangeFocusView(view2);
			return true;
		}

		private bool ChangePrevViewFocus()
		{
			int num = Array.IndexOf<View>(this.mViews_WorkSpace, this.mCurrentFocusView);
			int realIndex = this.mCurrentFocusView.GetRealIndex();
			int num2 = num - 1;
			int num3 = realIndex - 1;
			if (num2 == 0)
			{
				this.ChangeContentPosition(UIScrollList<Model, View>.ContentDirection.Hell);
			}
			if (realIndex == 0 && num == 0)
			{
				return false;
			}
			if (0 <= num2)
			{
				this.ChangeFocusView(this.mViews_WorkSpace[num2]);
				return true;
			}
			if (num2 < 0 && 0 <= num3)
			{
				bool flag = this.Scroll(-1, false);
				if (flag)
				{
					this.ChangeFocusView(this.mViews_WorkSpace[0]);
					return true;
				}
			}
			return false;
		}

		private void OnDragging(Vector2 delta)
		{
			if (this.mState != UIScrollList<Model, View>.ListState.Waiting && this.mState != UIScrollList<Model, View>.ListState.MoveFromFinger)
			{
				return;
			}
			this.RemoveFocus();
			UIScrollList<Model, View>.ViewModelValueState viewModelValueState = this.CompareUserViewModelValue();
			UIScrollList<Model, View>.ScrollDirection scrollDirection = this.CheckDirection(delta.y);
			if (this.mUserViewCount < this.mModels.Length)
			{
				UIScrollList<Model, View>.ScrollDirection scrollDirection2 = scrollDirection;
				if (scrollDirection2 != UIScrollList<Model, View>.ScrollDirection.Heaven)
				{
					if (scrollDirection2 == UIScrollList<Model, View>.ScrollDirection.Hell)
					{
						this.ChangeContentPosition(UIScrollList<Model, View>.ContentDirection.Hell);
					}
				}
				else
				{
					this.ChangeContentPosition(UIScrollList<Model, View>.ContentDirection.Heaven);
				}
			}
			this.RemoveFocus();
			this.ChangeState(UIScrollList<Model, View>.ListState.MoveFromFinger);
			this.Scroll(delta.y);
		}

		private UIScrollList<Model, View>.ContentDirection CheckContentDirection(float value)
		{
			if (0f < value)
			{
				return UIScrollList<Model, View>.ContentDirection.Heaven;
			}
			if (value < 0f)
			{
				return UIScrollList<Model, View>.ContentDirection.Hell;
			}
			return this.mContentDirection;
		}

		private void OnPressEvent(Vector3 eventPosition, bool pressed, bool isFlick)
		{
			if (pressed)
			{
				this.KillAnimationAndForceCompleteRepositionForScroll();
				this.mLastTouchedPosition = eventPosition;
			}
			else
			{
				Vector3 diff = -(this.mLastTouchedPosition - eventPosition);
				if (isFlick)
				{
					this.KillAnimationAndForceCompleteRepositionForScroll();
					this.KillAnimationListMoveFromFingerScroll();
					this.ChangeState(UIScrollList<Model, View>.ListState.MoveFromFingerScrolling);
					this.PlayAnimationListMoveFromFingerScroll(diff.y, delegate
					{
						this.ChangeState(UIScrollList<Model, View>.ListState.Waiting);
						UIScrollList<Model, View>.ContentDirection contentDirection3 = this.CheckContentDirection(diff.y);
						this.ChangeContentPosition(contentDirection3);
						UIScrollList<Model, View>.ContentDirection contentDirection4 = contentDirection3;
						if (contentDirection4 != UIScrollList<Model, View>.ContentDirection.Heaven)
						{
							if (contentDirection4 == UIScrollList<Model, View>.ContentDirection.Hell)
							{
								this.HeadFocus();
							}
						}
						else
						{
							this.TailFocusForOnFinishedScroll();
						}
					});
				}
				else
				{
					UIScrollList<Model, View>.ContentDirection contentDirection = this.CheckContentDirection(diff.y);
					UIScrollList<Model, View>.ContentDirection contentDirection2 = contentDirection;
					if (contentDirection2 != UIScrollList<Model, View>.ContentDirection.Heaven)
					{
						if (contentDirection2 == UIScrollList<Model, View>.ContentDirection.Hell)
						{
							this.ChangeContentPosition(UIScrollList<Model, View>.ContentDirection.Hell);
							this.HeadFocusMember();
							if (this.mCurrentFocusView.GetRealIndex() == 0)
							{
								this.PlayAnimationRepositionForScroll(new Action(this.OnFinishedAnimationRepositionForScroll));
							}
							else
							{
								this.ChangeFocusView(this.mCurrentFocusView);
								this.ChangeState(UIScrollList<Model, View>.ListState.Waiting);
							}
						}
					}
					else
					{
						this.ChangeContentPosition(UIScrollList<Model, View>.ContentDirection.Heaven);
						this.TailFocusMember();
						if (this.mCurrentFocusView.GetRealIndex() == this.mModels.Length - 1)
						{
							this.PlayAnimationRepositionForScroll(new Action(this.OnFinishedAnimationRepositionForScroll));
						}
						else
						{
							this.ChangeFocusView(this.mCurrentFocusView);
							this.ChangeState(UIScrollList<Model, View>.ListState.Waiting);
						}
					}
				}
			}
		}

		private void OnFinishedAnimationRepositionForScroll()
		{
			if (this.mUserViewCount < this.mModels.Length)
			{
				UIScrollList<Model, View>.ContentDirection contentDirection = this.mContentDirection;
				if (contentDirection != UIScrollList<Model, View>.ContentDirection.Heaven)
				{
					if (contentDirection == UIScrollList<Model, View>.ContentDirection.Hell)
					{
						this.ChangeFocusToUserViewHead();
					}
				}
				else
				{
					this.ChangeFocusToUserViewTail();
				}
			}
			else
			{
				this.ChangeFocusToUserViewHead();
			}
			this.ChangeState(UIScrollList<Model, View>.ListState.Waiting);
		}

		private void KillAnimationAndForceCompleteRepositionForScroll()
		{
			if (DOTween.IsTweening(UIScrollList<Model, View>.AnimationType.RepositionForScroll))
			{
				DOTween.Kill(UIScrollList<Model, View>.AnimationType.RepositionForScroll, true);
			}
		}

		public void KillScrollAnimation()
		{
			this.KillAnimationListMoveFromFingerScroll();
			this.KillAnimationAndForceCompleteRepositionForScroll();
		}

		private void KillAnimationContainerMove()
		{
			bool flag = DOTween.IsTweening(UIScrollList<Model, View>.AnimationType.ContainerMove);
			if (flag)
			{
				DOTween.Kill(UIScrollList<Model, View>.AnimationType.ContainerMove, false);
			}
		}

		private void KillAnimationListMoveFromFingerScroll()
		{
			if (DOTween.IsTweening(UIScrollList<Model, View>.AnimationType.ListMoveFromFingerScroll))
			{
				DOTween.Kill(UIScrollList<Model, View>.AnimationType.ListMoveFromFingerScroll, false);
			}
		}

		private Tween PlayAnimationRepositionForScroll(Action onComplete)
		{
			if (DOTween.IsTweening(UIScrollList<Model, View>.AnimationType.RepositionForScroll))
			{
				DOTween.Kill(UIScrollList<Model, View>.AnimationType.RepositionForScroll, true);
			}
			int num = 0;
			View[] array = this.mViews_WorkSpace;
			for (int i = 0; i < array.Length; i++)
			{
				View view = array[i];
				int num2 = Array.IndexOf<View>(this.mViews_WorkSpace, view);
				int num3 = Utils.LoopValue(num2 - 1, 0, this.mViews_WorkSpace.Length);
				int num4 = view.GetRealIndex() + this.mViews_WorkSpace.Length;
				if (num4 < this.mModels.Length && this.mOuterHead < view.GetTransform().get_localPosition().y)
				{
					view.GetTransform().set_localPosition(this.mViews_WorkSpace[num3].GetTransform().get_localPosition());
					view.GetTransform().AddLocalPositionY((float)(-(float)view.GetHeight()));
					view.Initialize(num4, this.mModels[num4]);
					num--;
				}
			}
			bool flag = 0 < num || num < 0;
			if (flag)
			{
				this.mViews_WorkSpace = this.RollReferenceViews(this.mViews_WorkSpace, num);
			}
			Sequence sequence = DOTween.Sequence();
			TweenSettingsExtensions.SetId<Sequence>(sequence, UIScrollList<Model, View>.AnimationType.RepositionForScroll);
			for (int j = 0; j < this.mViews.Length; j++)
			{
				Tween tween = ShortcutExtensions.DOLocalMoveY(this.mViews_WorkSpace[j].GetTransform(), this.mViewDefaultPositions[j].y, 0.3f, false);
				TweenSettingsExtensions.Join(sequence, tween);
			}
			TweenSettingsExtensions.OnComplete<Sequence>(sequence, delegate
			{
				if (onComplete != null)
				{
					onComplete.Invoke();
				}
			});
			return sequence;
		}

		private void PlayAnimationFingerUpReposition(Action onComplete)
		{
			if (DOTween.IsTweening(UIScrollList<Model, View>.AnimationType.RepositionForScroll))
			{
				return;
			}
			Sequence sequence = DOTween.Sequence();
			TweenSettingsExtensions.SetId<Sequence>(sequence, UIScrollList<Model, View>.AnimationType.RepositionForScroll);
			for (int i = 0; i < this.mViews.Length; i++)
			{
				Tween tween = ShortcutExtensions.DOLocalMoveY(this.mViews_WorkSpace[i].GetTransform(), this.mViewDefaultPositions[i].y, 0.3f, false);
				TweenSettingsExtensions.Join(sequence, tween);
			}
			TweenSettingsExtensions.OnComplete<Sequence>(sequence, delegate
			{
				if (onComplete != null)
				{
					onComplete.Invoke();
				}
			});
		}

		private UIScrollList<Model, View>.ScrollDirection CheckDirection(float directionY)
		{
			UIScrollList<Model, View>.ScrollDirection result = (this.mContentDirection != UIScrollList<Model, View>.ContentDirection.Heaven) ? UIScrollList<Model, View>.ScrollDirection.Hell : UIScrollList<Model, View>.ScrollDirection.Heaven;
			if (0f < directionY)
			{
				result = UIScrollList<Model, View>.ScrollDirection.Heaven;
			}
			else if (directionY < 0f)
			{
				result = UIScrollList<Model, View>.ScrollDirection.Hell;
			}
			return result;
		}

		private bool ChangePage(int pageInValue, int changeCount)
		{
			UIScrollList<Model, View>.ScrollDirection scrollDirection = this.CheckDirection((float)(-(float)changeCount));
			for (int i = 0; i < Math.Abs(changeCount) * pageInValue; i++)
			{
				UIScrollList<Model, View>.ScrollDirection scrollDirection2 = scrollDirection;
				if (scrollDirection2 != UIScrollList<Model, View>.ScrollDirection.Heaven)
				{
					if (scrollDirection2 == UIScrollList<Model, View>.ScrollDirection.Hell)
					{
						if (!this.Scroll(1, true))
						{
							this.ChangeContentPosition(UIScrollList<Model, View>.ContentDirection.Heaven);
							return false;
						}
					}
				}
				else
				{
					this.Scroll(-1, true);
					this.ChangeContentPosition(UIScrollList<Model, View>.ContentDirection.Hell);
				}
			}
			return true;
		}

		private bool Scroll(int count, bool paddingRollControl)
		{
			if (this.mState != UIScrollList<Model, View>.ListState.Waiting)
			{
				return false;
			}
			UIScrollList<Model, View>.ScrollDirection scrollDirection = this.CheckDirection((float)count);
			UIScrollList<Model, View>.ScrollDirection scrollDirection2 = scrollDirection;
			if (scrollDirection2 != UIScrollList<Model, View>.ScrollDirection.Heaven)
			{
				if (scrollDirection2 == UIScrollList<Model, View>.ScrollDirection.Hell)
				{
					View[] array = this.mViews_WorkSpace;
					for (int i = 0; i < array.Length; i++)
					{
						View view = array[i];
						if (view.GetRealIndex() == 0)
						{
							return false;
						}
					}
					for (int j = 0; j < this.mViews_WorkSpace.Length; j++)
					{
						View view2 = this.mViews_WorkSpace[j];
						int num = j - count;
						bool flag = false;
						flag |= (num < 0);
						flag |= (this.mViews_WorkSpace.Length <= num);
						if (flag)
						{
							int num2 = view2.GetRealIndex() - this.mViews_WorkSpace.Length + 1 + count;
							bool flag2 = 0 <= num2;
							flag2 &= (num2 < this.mModels.Length);
							if (flag2)
							{
								view2.Initialize(num2, this.mModels[num2]);
							}
							else
							{
								view2.InitializeDefault(num2);
							}
						}
					}
				}
			}
			else
			{
				if (paddingRollControl)
				{
					bool flag3 = this.mModels.Length - 1 < this.mViews_WorkSpace[this.mUserViewCount - 1].GetRealIndex() + count;
					if (flag3)
					{
						return false;
					}
				}
				else
				{
					View[] array2 = this.mViews_WorkSpace;
					for (int k = 0; k < array2.Length; k++)
					{
						View view3 = array2[k];
						bool flag4 = view3.GetRealIndex() == this.mModels.Length - 1;
						if (flag4)
						{
							return false;
						}
					}
				}
				for (int l = 0; l < this.mViews_WorkSpace.Length; l++)
				{
					View view4 = this.mViews_WorkSpace[l];
					int num3 = l - count;
					bool flag5 = false;
					flag5 |= (num3 < 0);
					flag5 |= (this.mViews_WorkSpace.Length <= num3);
					if (flag5)
					{
						int num4 = view4.GetRealIndex() + (this.mViews_WorkSpace.Length - 1) + count;
						bool flag6 = 0 <= num4;
						flag6 &= (num4 < this.mModels.Length);
						if (flag6)
						{
							view4.Initialize(num4, this.mModels[num4]);
						}
						else
						{
							view4.InitializeDefault(num4);
						}
					}
				}
			}
			this.mViews_WorkSpace = this.RollReferenceViews(this.mViews_WorkSpace, -count);
			this.RepositionNow();
			return true;
		}

		private void RepositionNow()
		{
			for (int i = 0; i < this.mViews_WorkSpace.Length; i++)
			{
				this.mViews_WorkSpace[i].GetTransform().set_localPosition(this.mViewDefaultPositions[i]);
			}
		}

		private int ScrollToHeaven(float moveY)
		{
			int num = 0;
			View[] array = this.mViews_WorkSpace;
			for (int i = 0; i < array.Length; i++)
			{
				View view = array[i];
				if (!this.IsViewHeadInRange(view, this.mOuterHead, this.mOuterTail))
				{
					int num2 = Array.IndexOf<View>(this.mViews_WorkSpace, view);
					int num3 = Utils.LoopValue(num2 - 1, 0, this.mViews_WorkSpace.Length);
					int num4 = view.GetRealIndex() + this.mViews_WorkSpace.Length;
					bool flag = num4 < this.mModels.Length;
					if (flag)
					{
						bool flag2 = this.mOuterHead < view.GetTransform().get_localPosition().y;
						if (flag2)
						{
							Model model = this.mModels[num4];
							view.GetTransform().set_localPosition(this.mViews_WorkSpace[num3].GetTransform().get_localPosition());
							view.GetTransform().AddLocalPositionY((float)(-(float)view.GetHeight()));
							view.Initialize(num4, model);
							num--;
						}
					}
					else if (view.GetRealIndex() < this.mModels.Length - this.mUserViewCount)
					{
						view.GetTransform().set_localPosition(this.mViews_WorkSpace[num3].GetTransform().get_localPosition());
						view.GetTransform().AddLocalPositionY((float)(-(float)view.GetHeight()));
						view.InitializeDefault(num4);
						num--;
					}
				}
			}
			return num;
		}

		private int ScrollToHell(float moveY)
		{
			int num = 0;
			View[] array = Enumerable.ToArray<View>(Enumerable.Reverse<View>(this.mViews_WorkSpace));
			for (int i = 0; i < array.Length; i++)
			{
				View view = array[i];
				if (!this.IsViewTailInRange(view, this.mOuterHead, this.mOuterTail))
				{
					int num2 = Array.IndexOf<View>(this.mViews_WorkSpace, view);
					int num3 = Utils.LoopValue(num2 + 1, 0, this.mViews_WorkSpace.Length);
					int num4 = view.GetRealIndex() - this.mViews_WorkSpace.Length;
					bool flag = 0 <= num4;
					if (flag && view.GetTransform().get_localPosition().y - (float)view.GetHeight() < this.mOuterTail)
					{
						view.GetTransform().set_localPosition(this.mViews_WorkSpace[num3].GetTransform().get_localPosition());
						view.GetTransform().AddLocalPositionY((float)view.GetHeight());
						view.Initialize(num4, this.mModels[num4]);
						num++;
					}
				}
			}
			return num;
		}

		private bool Scroll(float moveY)
		{
			if (this.mState != UIScrollList<Model, View>.ListState.Waiting && this.mState != UIScrollList<Model, View>.ListState.MoveFromFinger && this.mState != UIScrollList<Model, View>.ListState.MoveFromFingerScrolling)
			{
				return false;
			}
			UIScrollList<Model, View>.ScrollDirection scrollDirection = this.CheckDirection(moveY);
			View[] array = this.mViews_WorkSpace;
			for (int i = 0; i < array.Length; i++)
			{
				View view = array[i];
				view.GetTransform().AddLocalPositionY(moveY);
			}
			int num = 0;
			UIScrollList<Model, View>.ScrollDirection scrollDirection2 = scrollDirection;
			if (scrollDirection2 != UIScrollList<Model, View>.ScrollDirection.Heaven)
			{
				if (scrollDirection2 == UIScrollList<Model, View>.ScrollDirection.Hell)
				{
					num = this.ScrollToHell(moveY);
				}
			}
			else
			{
				num = this.ScrollToHeaven(moveY);
			}
			bool flag = num != 0;
			if (flag)
			{
				this.mViews_WorkSpace = this.RollReferenceViews(this.mViews_WorkSpace, num);
			}
			return true;
		}

		private View[] RollReferenceViews(View[] views, int rollCount)
		{
			View[] array = new View[views.Length];
			for (int i = 0; i < views.Length; i++)
			{
				int num = Utils.LoopValue(i + rollCount, 0, views.Length);
				array[num] = views[i];
			}
			return array;
		}

		private bool IsViewHeadInRange(View view, float from, float to)
		{
			float y = view.GetTransform().get_localPosition().y;
			return Utils.RangeEqualsIn(y, from, to);
		}

		private bool IsViewTailInRange(View view, float from, float to)
		{
			float currentPosition = view.GetTransform().get_localPosition().y - (float)view.GetHeight();
			return Utils.RangeEqualsIn(currentPosition, from, to);
		}
	}
}
