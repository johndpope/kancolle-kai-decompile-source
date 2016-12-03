using KCV.Display;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace KCV.View.Scroll
{
	public abstract class UIScrollListParentNew<Model, View> : MonoBehaviour where Model : class where View : UIScrollListChildNew<Model, View>
	{
		protected Model[] models;

		private Vector3[] defaultViewPos;

		protected View[] views;

		private bool swipeBounceBackAnimating;

		public bool controllable;

		[SerializeField, Tooltip("子要素のプレハブを設定してください。")]
		private View childPrefab;

		[SerializeField, Tooltip("スワイプイベントを受け取る為のオブジェクト")]
		private UIDisplaySwipeEventRegion swipeEventRegion;

		[SerializeField, Tooltip("作った要素を入れる場所")]
		private UIGrid container;

		[SerializeField, Tooltip("ユーザが１度に見ることができる要素の最大数を設定してください。予備のオブジェクトの事は考えないでください")]
		private int MAX_USER_VIEW_OBJECTS = 6;

		[SerializeField]
		private int MAX_RELIMINARY_OBJECTS = 1;

		private int maxViewCount;

		private float topDestroyY;

		private float bottomDestroyY;

		protected KeyControl keyController;

		private int rollCount;

		private int cursorIndex;

		private float defaultIntervalTime;

		private Coroutine initCoroutine;

		private UIScrollListParentHandler<View> handler;

		protected View currentChild
		{
			get
			{
				UIToggle activeToggle = UIToggle.GetActiveToggle(this.getListGroupNo());
				return (!(activeToggle == null)) ? activeToggle.GetComponentInParent<View>() : ((View)((object)null));
			}
		}

		public virtual int getListGroupNo()
		{
			return this.GetHashCode();
		}

		public void Init(Model[] models, UIScrollListParentHandler<View> handler)
		{
			this.Init(models, handler, 0);
		}

		public void Init(Model[] models, UIScrollListParentHandler<View> handler, int topIdx)
		{
			Debug.Log("ReplaceNow XD");
		}

		public virtual void SetKeyController(KeyControl keyController)
		{
			if (keyController != null)
			{
				keyController.ClearKeyAll();
				keyController.firstUpdate = true;
			}
			this.keyController = keyController;
		}

		public void SetCamera(Camera camera)
		{
			this.swipeEventRegion.SetEventCatchCamera(camera);
		}

		protected void Update()
		{
			if (this.controllable && this.keyController != null)
			{
				if ((this.keyController.keyState.get_Item(12).holdTime > 1.5f || this.keyController.keyState.get_Item(8).holdTime > 1.5f) && this.keyController.KeyInputInterval == this.defaultIntervalTime)
				{
					this.keyController.KeyInputInterval = this.keyController.KeyInputInterval / 3f;
				}
				else
				{
					this.keyController.KeyInputInterval = this.defaultIntervalTime;
				}
				if (this.keyController.IsUpDown())
				{
					if (!this.swipeBounceBackAnimating)
					{
						this.MovePrev();
					}
				}
				else if (this.keyController.IsDownDown())
				{
					if (!this.swipeBounceBackAnimating)
					{
						this.MoveNext();
					}
				}
				else if (this.keyController.IsMaruDown())
				{
					if (this.currentChild != null)
					{
						View currentChild = this.currentChild;
						currentChild.OnClick();
					}
				}
				else if (this.keyController.IsBatuDown())
				{
					this.handler.OnCancel();
					this.BounceBack();
					View viewByModelIndex = this.GetViewByModelIndex(this.rollCount);
					viewByModelIndex.DoSelect();
				}
				else if (this.keyController.IsSankakuDown())
				{
					this.OnPressSankaku();
				}
			}
		}

		protected virtual void OnPressSankaku()
		{
		}

		private void ProcessSelect()
		{
			if (0 < this.models.Length)
			{
				UIScrollListParentHandler<View> arg_2E_0 = this.handler;
				View currentChild = this.currentChild;
				arg_2E_0.OnSelect(currentChild.modelIndex, this.currentChild);
			}
		}

		private void ProcessFocusChanging(View child)
		{
			this.handler.OnChangeFocus(child.modelIndex, child);
		}

		private void MovePrev()
		{
			if (this.rollCount == 0 && this.cursorIndex == 0)
			{
				return;
			}
			if (0 < this.cursorIndex)
			{
				this.cursorIndex--;
				View viewByModelIndex = this.GetViewByModelIndex(this.rollCount + this.cursorIndex);
				viewByModelIndex.DoSelect();
			}
			else
			{
				this.rollCount--;
				View[] array = this.views;
				for (int i = 0; i < array.Length; i++)
				{
					View child = array[i];
					if (child.modelIndex == this.rollCount + this.maxViewCount)
					{
						this.updateChild(this.rollCount, child, this.GetModel(this.rollCount));
						child.DoSelect();
					}
					child.get_transform().set_localPosition(this.GetDefaultViewPos(child.modelIndex - this.rollCount));
				}
			}
			this.ProcessFocusChanging(this.currentChild);
		}

		private void MoveNext()
		{
			if (this.models.Length <= this.rollCount + this.cursorIndex + 1)
			{
				return;
			}
			if (this.cursorIndex < this.MAX_USER_VIEW_OBJECTS - 1)
			{
				this.cursorIndex++;
				View viewByModelIndex = this.GetViewByModelIndex(this.rollCount + this.cursorIndex);
				viewByModelIndex.DoSelect();
			}
			else
			{
				this.cursorIndex = this.MAX_USER_VIEW_OBJECTS - 1;
				this.rollCount++;
				View[] array = this.views;
				for (int i = 0; i < array.Length; i++)
				{
					View child = array[i];
					int viewIndexByModelIndex = this.GetViewIndexByModelIndex(child.modelIndex - this.rollCount);
					if (viewIndexByModelIndex == this.maxViewCount - 1)
					{
						int num = this.rollCount + this.maxViewCount - 1;
						this.updateChild(num, child, this.GetModel(num));
					}
					if (viewIndexByModelIndex == this.MAX_USER_VIEW_OBJECTS - 1)
					{
						child.DoSelect();
					}
					child.get_transform().set_localPosition(this.defaultViewPos[viewIndexByModelIndex]);
				}
			}
			this.ProcessFocusChanging(this.currentChild);
		}

		private void DestroyChildren()
		{
			using (List<Transform>.Enumerator enumerator = this.container.GetChildList().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Transform current = enumerator.get_Current();
					NGUITools.Destroy(current);
				}
			}
		}

		public void OnChildSelect(UIScrollListChildNew<Model, View> child)
		{
			if (!this.controllable || this.keyController == null)
			{
				return;
			}
			View currentChild = this.currentChild;
			this.cursorIndex = currentChild.modelIndex - this.rollCount;
			this.ProcessSelect();
		}

		private void CalculateDefaultViewPos()
		{
			this.defaultViewPos = new Vector3[this.views.Length];
			int num = 0;
			View[] array = this.views;
			for (int i = 0; i < array.Length; i++)
			{
				View view = array[i];
				this.defaultViewPos[num++] = view.get_transform().get_localPosition();
			}
		}

		[DebuggerHidden]
		private IEnumerator InitCoroutine(int startModelIdx)
		{
			UIScrollListParentNew<Model, View>.<InitCoroutine>c__Iterator52 <InitCoroutine>c__Iterator = new UIScrollListParentNew<Model, View>.<InitCoroutine>c__Iterator52();
			<InitCoroutine>c__Iterator.startModelIdx = startModelIdx;
			<InitCoroutine>c__Iterator.<$>startModelIdx = startModelIdx;
			<InitCoroutine>c__Iterator.<>f__this = this;
			return <InitCoroutine>c__Iterator;
		}

		private void RefreshPosition(float moveY)
		{
			if (moveY < 0f && this.rollCount <= 0)
			{
				this.Relocation(moveY);
				return;
			}
			if (0f < moveY && this.models.Length <= this.rollCount + this.MAX_USER_VIEW_OBJECTS)
			{
				this.Relocation(moveY);
				return;
			}
			if (0f < moveY)
			{
				this.RefreshUpSwipedPostion(moveY);
			}
			else if (moveY < 0f)
			{
				this.RefreshDownSwipedPosition(moveY);
			}
		}

		private void RefreshUpSwipedPostion(float moveY)
		{
			this.Relocation(moveY);
			View[] array = this.views;
			for (int i = 0; i < array.Length; i++)
			{
				View view = array[i];
				if (this.topDestroyY <= view.get_transform().get_localPosition().y)
				{
					this.rollCount++;
					int num = Array.IndexOf<View>(this.views, view);
					int viewIndexByModelIndex = this.GetViewIndexByModelIndex(num - 1);
					Vector3 localPosition = this.views[viewIndexByModelIndex].get_transform().get_localPosition();
					view.UpdateLocalPosition(localPosition.x, localPosition.y - this.GetChildHeight(), localPosition.z);
					int num2 = this.rollCount + this.views.Length - 1;
					Model model = (Model)((object)null);
					if (num2 < this.models.Length)
					{
						model = this.models[num2];
					}
					this.updateChild(num2, view, model);
				}
			}
		}

		private void RefreshDownSwipedPosition(float moveY)
		{
			this.Relocation(moveY);
			int num = this.views.Length - 1;
			while (0 <= num)
			{
				View view = this.views[num];
				if (view.get_transform().get_localPosition().y < this.bottomDestroyY)
				{
					this.rollCount--;
					int num2 = Array.IndexOf<View>(this.views, view);
					int viewIndexByModelIndex = this.GetViewIndexByModelIndex(num2 + 1);
					Vector3 localPosition = this.views[viewIndexByModelIndex].get_transform().get_localPosition();
					view.UpdateLocalPosition(localPosition.x, localPosition.y + this.GetChildHeight(), localPosition.z);
					this.updateChild(this.rollCount, view, this.GetModel(this.rollCount));
				}
				num--;
			}
		}

		private void BounceBack()
		{
			for (int i = 0; i < this.views.Length; i++)
			{
				View view = this.views[i];
				TweenPosition tweenPosition = UITweener.Begin<TweenPosition>(view.get_gameObject(), 0.3f);
				tweenPosition.from = view.get_transform().get_localPosition();
				tweenPosition.to = this.GetDefaultViewPos(view.modelIndex - this.rollCount);
				if (i == this.views.Length - 1)
				{
					this.swipeBounceBackAnimating = true;
					EventDelegate.Set(tweenPosition.onFinished, delegate
					{
						this.swipeBounceBackAnimating = false;
					});
				}
				tweenPosition.PlayForward();
			}
		}

		private void Relocation(float moveY)
		{
			View[] array = this.views;
			for (int i = 0; i < array.Length; i++)
			{
				View view = array[i];
				view.get_transform().set_localPosition(new Vector3(view.get_transform().get_localPosition().x, view.get_transform().get_localPosition().y + moveY, view.get_transform().get_localPosition().z));
			}
		}

		private void updateChild(int nextIndex, View child, Model model)
		{
			child.Init(this, model, nextIndex);
			if (model != null)
			{
				child.Show();
			}
			else
			{
				child.Hide();
			}
		}

		private View GetViewByModelIndex(int modelIdx)
		{
			return this.views[this.GetViewIndexByModelIndex(modelIdx)];
		}

		private Vector3 GetDefaultViewPos(int modelIdx)
		{
			return this.defaultViewPos[this.GetViewIndexByModelIndex(modelIdx)];
		}

		private int GetViewIndexByModelIndex(int modelIdx)
		{
			while (modelIdx < 0)
			{
				modelIdx += this.maxViewCount;
			}
			return modelIdx % this.maxViewCount;
		}

		private float GetChildHeight()
		{
			return this.container.cellHeight;
		}

		private void OnSwipeAction(UIDisplaySwipeEventRegion.ActionType actionType, float deltaX, float deltaY, float movedPercentageX, float movedPercentageY, float elapsedTime)
		{
			if (this.controllable && !this.swipeBounceBackAnimating)
			{
				float num = deltaY;
				float childHeight = this.GetChildHeight();
				if (childHeight < num)
				{
					num = childHeight;
				}
				else if (num < -childHeight)
				{
					num = -childHeight;
				}
				if (actionType != UIDisplaySwipeEventRegion.ActionType.Moving)
				{
					if (actionType == UIDisplaySwipeEventRegion.ActionType.FingerUp)
					{
						this.BounceBack();
						View viewByModelIndex = this.GetViewByModelIndex(this.rollCount);
						viewByModelIndex.DoSelect();
					}
				}
				else
				{
					this.cursorIndex = 0;
					this.RefreshPosition(num);
				}
			}
		}

		private Model GetModel(int modelIdx)
		{
			if (modelIdx < 0 || modelIdx >= this.models.Length)
			{
				return (Model)((object)null);
			}
			return this.models[modelIdx];
		}

		public void SetEnabled(bool enabled)
		{
			base.set_enabled(enabled);
			this.controllable = enabled;
			if (this.views != null)
			{
				for (int i = 0; i < this.views.Length; i++)
				{
					this.views[i].set_enabled(enabled);
				}
			}
		}

		public void SetFocus(bool focused)
		{
			bool enabled = base.get_enabled();
			this.SetEnabled(true);
			View currentChild = this.currentChild;
			if (currentChild != null)
			{
				if (focused)
				{
					currentChild.UpdateHover();
				}
				else
				{
					currentChild.HideHover();
				}
			}
			this.SetEnabled(enabled);
		}

		private void OnDestroy()
		{
			this.OnCallDestroy();
			this.models = null;
			this.defaultViewPos = null;
			if (this.views != null)
			{
				for (int i = 0; i < this.views.Length; i++)
				{
					if (this.views[i] != null)
					{
						this.views[i] = (View)((object)null);
					}
				}
			}
			this.views = null;
			this.childPrefab = (View)((object)null);
			this.swipeEventRegion = null;
			this.container = null;
			this.keyController = null;
			this.initCoroutine = null;
		}

		protected virtual void OnCallDestroy()
		{
		}
	}
}
