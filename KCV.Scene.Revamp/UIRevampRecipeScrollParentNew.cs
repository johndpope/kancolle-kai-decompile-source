using DG.Tweening;
using KCV.Utils;
using KCV.View.ScrollView;
using local.managers;
using local.models;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KCV.Scene.Revamp
{
	public class UIRevampRecipeScrollParentNew : UIScrollList<RevampRecipeScrollUIModel, UIRevampRecipeScrollChildNew>
	{
		public enum AnimationType
		{
			SlotIn
		}

		private RevampManager mRevampManager;

		private Action<UIRevampRecipeScrollChildNew> mOnSelectedRecipeListener;

		private Action mOnFinishedSlotInAnimationListener;

		private Action mOnBackListener;

		private KeyControl mKeyController;

		public float duration = 0.3f;

		public Ease easing = 17;

		public void Initialize(RevampManager revampManager)
		{
			base.ChangeImmediateContentPosition(UIScrollList<RevampRecipeScrollUIModel, UIRevampRecipeScrollChildNew>.ContentDirection.Hell);
			this.mRevampManager = revampManager;
			RevampRecipeModel[] array = Enumerable.ToArray<RevampRecipeModel>(this.mRevampManager.GetRecipes());
			List<RevampRecipeScrollUIModel> list = new List<RevampRecipeScrollUIModel>();
			RevampRecipeModel[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				RevampRecipeModel revampRecipeModel = array2[i];
				bool clickable = 0 < this.mRevampManager.GetSlotitemList(revampRecipeModel.RecipeId).Length;
				RevampRecipeScrollUIModel revampRecipeScrollUIModel = new RevampRecipeScrollUIModel(revampRecipeModel, clickable);
				list.Add(revampRecipeScrollUIModel);
			}
			base.Initialize(list.ToArray());
		}

		protected override void OnChangedFocusView(UIRevampRecipeScrollChildNew focusToView)
		{
			if (0 < this.mModels.Length && base.mState == UIScrollList<RevampRecipeScrollUIModel, UIRevampRecipeScrollChildNew>.ListState.Waiting && this.mCurrentFocusView != null)
			{
				int realIndex = this.mCurrentFocusView.GetRealIndex();
				CommonPopupDialog.Instance.StartPopup(realIndex + 1 + "/" + this.mModels.Length, 0, CommonPopupDialogMessage.PlayType.Long);
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			}
		}

		public void PlaySlotInAnimation()
		{
			if (DOTween.IsTweening(UIRevampRecipeScrollParentNew.AnimationType.SlotIn))
			{
				DOTween.Kill(UIRevampRecipeScrollParentNew.AnimationType.SlotIn, false);
			}
			Sequence sequence = TweenSettingsExtensions.SetId<Sequence>(DOTween.Sequence(), UIRevampRecipeScrollParentNew.AnimationType.SlotIn);
			UIRevampRecipeScrollChildNew slot = this.mViews[0];
			UIRevampRecipeScrollChildNew slot2 = this.mViews[1];
			UIRevampRecipeScrollChildNew slot3 = this.mViews[2];
			TweenSettingsExtensions.Append(sequence, this.GenerateSlotInTween(slot));
			TweenSettingsExtensions.Append(sequence, this.GenerateSlotInTween(slot2));
			TweenSettingsExtensions.Append(sequence, this.GenerateSlotInTween(slot3));
			TweenSettingsExtensions.OnComplete<Sequence>(sequence, delegate
			{
				this.NotifyFinishedSlotInAnimation();
			});
			TweenExtensions.PlayForward(sequence);
		}

		protected override bool OnSelectable(UIRevampRecipeScrollChildNew view)
		{
			return view.GetModel().Clickable;
		}

		private Tween GenerateSlotInTween(UIRevampRecipeScrollChildNew slot)
		{
			slot.alpha = 0f;
			slot.get_transform().localPositionX(-300f);
			Sequence sequence = DOTween.Sequence();
			Tween tween = TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMoveX(slot.get_transform(), 0f, this.duration, false), this.easing);
			Tween tween2 = DOVirtual.Float(0f, 1f, this.duration, delegate(float alpha)
			{
				slot.alpha = alpha;
			});
			TweenSettingsExtensions.Append(sequence, tween);
			TweenSettingsExtensions.Join(sequence, tween2);
			TweenSettingsExtensions.OnStart<Sequence>(sequence, delegate
			{
				SoundUtils.PlaySE(SEFIleInfos.SE_019);
			});
			return sequence;
		}

		public void SetOnFinishedSlotInAnimationListener(Action onFinishedSlotInAnimationListener)
		{
			this.mOnFinishedSlotInAnimationListener = onFinishedSlotInAnimationListener;
		}

		private void NotifyFinishedSlotInAnimation()
		{
			if (this.mOnFinishedSlotInAnimationListener != null)
			{
				this.mOnFinishedSlotInAnimationListener.Invoke();
			}
		}

		internal void SetOnSelectedListener(Action<UIRevampRecipeScrollChildNew> onSelectedRecipeListener)
		{
			this.mOnSelectedRecipeListener = onSelectedRecipeListener;
		}

		protected override void OnSelect(UIRevampRecipeScrollChildNew view)
		{
			if (this.mOnSelectedRecipeListener != null)
			{
				this.mOnSelectedRecipeListener.Invoke(view);
			}
		}

		internal void SetCamera(Camera cameraTouchEventCatch)
		{
			base.SetSwipeEventCamera(cameraTouchEventCatch);
		}

		internal KeyControl GetKeyController()
		{
			if (this.mKeyController == null)
			{
				this.mKeyController = new KeyControl(0, 0, 0.4f, 0.1f);
			}
			return this.mKeyController;
		}

		internal void StartControl()
		{
			base.HeadFocus();
			base.ChangeImmediateContentPosition(UIScrollList<RevampRecipeScrollUIModel, UIRevampRecipeScrollChildNew>.ContentDirection.Hell);
			base.StartControl();
		}

		private void Update()
		{
			if (Input.GetKeyDown(112))
			{
				this.PlaySlotInAnimation();
			}
			if (this.mKeyController != null && base.mState == UIScrollList<RevampRecipeScrollUIModel, UIRevampRecipeScrollChildNew>.ListState.Waiting)
			{
				if (this.mKeyController.IsUpDown())
				{
					base.PrevFocus();
				}
				else if (this.mKeyController.IsDownDown())
				{
					base.NextFocus();
				}
				else if (this.mKeyController.IsMaruDown())
				{
					this.mKeyController.ClearKeyAll();
					this.mKeyController.firstUpdate = true;
					base.Select();
				}
				else if (this.mKeyController.IsBatuDown())
				{
					SingletonMonoBehaviour<PortObjectManager>.Instance.BackToActiveScene();
				}
			}
		}

		protected override void OnCallDestroy()
		{
			base.OnCallDestroy();
			this.mRevampManager = null;
			this.mOnSelectedRecipeListener = null;
			this.mOnFinishedSlotInAnimationListener = null;
			this.mOnBackListener = null;
			this.mKeyController = null;
		}

		internal void LockControl()
		{
			base.LockControl();
		}
	}
}
