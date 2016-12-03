using Common.Enum;
using DG.Tweening;
using KCV.Scene.Port;
using KCV.View.ScrollView;
using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Interior
{
	public class UIInteriorFurnitureChangeScrollListNew : UIScrollList<UIInteriorFurnitureChangeScrollListChildModelNew, UIInteriorFurnitureChangeScrollListChildNew>
	{
		private enum TweenAnimationType
		{
			ShowHide
		}

		[SerializeField]
		private UITexture mTexture_Header;

		[SerializeField]
		private UILabel mLabel_Genre;

		[SerializeField]
		private UITexture mTexture_TouchBackArea;

		[SerializeField]
		private OnClickEventSender mOnClickEventSender_Back;

		[SerializeField]
		private OnClickEventSender mOnClickEventSender_Next;

		private KeyControl mKeyController;

		private Action<UIInteriorFurnitureChangeScrollListChildNew> mOnSelectedListener;

		private Action<UIInteriorFurnitureChangeScrollListChildNew> mOnChangedListener;

		private Action mOnBackListener;

		public void Show()
		{
			this.mOnClickEventSender_Back.SetClickable(true);
			this.mOnClickEventSender_Next.SetClickable(true);
			if (DOTween.IsTweening(UIInteriorFurnitureChangeScrollListNew.TweenAnimationType.ShowHide))
			{
				DOTween.Kill(UIInteriorFurnitureChangeScrollListNew.TweenAnimationType.ShowHide, false);
			}
			TweenPosition tweenPosition = UITweener.Begin<TweenPosition>(base.get_gameObject(), 0.3f);
			tweenPosition.from = base.get_transform().get_localPosition();
			tweenPosition.to = new Vector3(0f, base.get_transform().get_localPosition().y, base.get_transform().get_localPosition().z);
			tweenPosition.ignoreTimeScale = true;
		}

		public void Hide()
		{
			this.mOnClickEventSender_Back.SetClickable(false);
			this.mOnClickEventSender_Next.SetClickable(false);
			if (DOTween.IsTweening(UIInteriorFurnitureChangeScrollListNew.TweenAnimationType.ShowHide))
			{
				DOTween.Kill(UIInteriorFurnitureChangeScrollListNew.TweenAnimationType.ShowHide, false);
			}
			Sequence sequence = DOTween.Sequence();
			Tween tween = TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMoveX(base.get_transform(), -960f, 0.6f, false), 21);
			TweenSettingsExtensions.Append(sequence, tween);
			TweenSettingsExtensions.SetId<Sequence>(sequence, UIInteriorFurnitureChangeScrollListNew.TweenAnimationType.ShowHide);
		}

		public void Initialize(int deckId, FurnitureKinds furnitureKind, FurnitureModel[] models, Camera camera)
		{
			UIInteriorFurnitureChangeScrollListChildModelNew[] models2 = this.GenerateChildDTOs(deckId, models);
			this.UpdateHeader(furnitureKind);
			base.Initialize(models2);
			base.SetSwipeEventCamera(camera);
		}

		private UIInteriorFurnitureChangeScrollListChildModelNew[] GenerateChildDTOs(int deckId, FurnitureModel[] models)
		{
			List<UIInteriorFurnitureChangeScrollListChildModelNew> list = new List<UIInteriorFurnitureChangeScrollListChildModelNew>();
			for (int i = 0; i < models.Length; i++)
			{
				FurnitureModel model = models[i];
				UIInteriorFurnitureChangeScrollListChildModelNew uIInteriorFurnitureChangeScrollListChildModelNew = new UIInteriorFurnitureChangeScrollListChildModelNew(deckId, model);
				list.Add(uIInteriorFurnitureChangeScrollListChildModelNew);
			}
			return list.ToArray();
		}

		public void RefreshViews()
		{
			base.RefreshViews();
		}

		protected override void OnUpdate()
		{
			if (this.mKeyController != null)
			{
				if (this.mKeyController.IsUpDown())
				{
					base.PrevFocus();
				}
				else if (this.mKeyController.IsDownDown())
				{
					base.NextFocus();
				}
				else if (this.mKeyController.IsLeftDown())
				{
					base.PrevPageOrHeadFocus();
				}
				else if (this.mKeyController.IsRightDown())
				{
					base.NextPageOrTailFocus();
				}
				else if (this.mKeyController.IsMaruDown())
				{
					base.Select();
				}
				else if (this.mKeyController.IsBatuDown())
				{
					this.Back();
				}
			}
		}

		[Obsolete("Inspector上で設定して使用します")]
		public void OnTouchSelect()
		{
			this.mOnClickEventSender_Back.SetClickable(false);
			this.mOnClickEventSender_Next.SetClickable(false);
			base.Select();
		}

		[Obsolete("Inspector上で設定して使用します")]
		public void OnTouchBack()
		{
			this.mOnClickEventSender_Back.SetClickable(false);
			this.mOnClickEventSender_Next.SetClickable(false);
			this.Back();
		}

		private void Back()
		{
			if (this.mOnBackListener != null)
			{
				this.mOnBackListener.Invoke();
			}
		}

		public void SetKeyController(KeyControl keyController)
		{
			this.mKeyController = keyController;
		}

		private void UpdateHeader(FurnitureKinds furnitureKinds)
		{
			switch (furnitureKinds)
			{
			case FurnitureKinds.Floor:
				this.mLabel_Genre.text = "床";
				this.mTexture_Header.color = new Color(0.65882355f, 0.423529416f, 0.4117647f);
				break;
			case FurnitureKinds.Wall:
				this.mLabel_Genre.text = "壁紙";
				this.mTexture_Header.color = new Color(161f, 0.4745098f, 0.356862754f);
				break;
			case FurnitureKinds.Window:
				this.mLabel_Genre.text = "窓枠＋カーテン";
				this.mTexture_Header.color = new Color(0.392156869f, 0.596078455f, 0.5882353f);
				break;
			case FurnitureKinds.Hangings:
				this.mLabel_Genre.text = "装飾";
				this.mTexture_Header.color = new Color(0.470588237f, 0.7058824f, 0.509803951f);
				break;
			case FurnitureKinds.Chest:
				this.mLabel_Genre.text = "家具";
				this.mTexture_Header.color = new Color(0.5882353f, 0.5882353f, 0.392156869f);
				break;
			case FurnitureKinds.Desk:
				this.mLabel_Genre.text = "椅子＋机";
				this.mTexture_Header.color = new Color(0.5254902f, 0.458823532f, 0.5803922f);
				break;
			}
		}

		internal void SetOnSelectedListener(Action<UIInteriorFurnitureChangeScrollListChildNew> onSelectedListener)
		{
			this.mOnSelectedListener = onSelectedListener;
		}

		internal void SetOnChangedItemListener(Action<UIInteriorFurnitureChangeScrollListChildNew> onChangedListener)
		{
			this.mOnChangedListener = onChangedListener;
		}

		protected override void OnChangedFocusView(UIInteriorFurnitureChangeScrollListChildNew focusToView)
		{
			if (this.mOnChangedListener != null)
			{
				this.mOnChangedListener.Invoke(focusToView);
			}
		}

		protected override void OnSelect(UIInteriorFurnitureChangeScrollListChildNew view)
		{
			if (this.mOnSelectedListener != null)
			{
				this.mOnSelectedListener.Invoke(view);
			}
		}

		internal void SetOnBackListener(Action onBackListener)
		{
			this.mOnBackListener = onBackListener;
		}

		public void StartControl()
		{
			base.HeadFocus();
			base.StartControl();
		}

		public void LockControl()
		{
			base.LockControl();
		}

		internal void ResumeControl()
		{
			base.StartControl();
			this.mOnClickEventSender_Back.SetClickable(true);
			this.mOnClickEventSender_Next.SetClickable(true);
		}

		protected override void OnCallDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_Header, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabel_Genre);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_TouchBackArea, false);
			this.mKeyController = null;
			this.mOnSelectedListener = null;
			this.mOnChangedListener = null;
			this.mOnBackListener = null;
		}
	}
}
