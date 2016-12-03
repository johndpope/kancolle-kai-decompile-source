using DG.Tweening;
using KCV.Scene.Port;
using local.models;
using System;
using UnityEngine;

namespace KCV.Arsenal
{
	[RequireComponent(typeof(UIButtonManager))]
	public class UIArsenalSelector : MonoBehaviour
	{
		public enum SelectType
		{
			None,
			Arsenal,
			Revamp
		}

		private UIButtonManager mButtonManager;

		[Header("Revamp"), SerializeField]
		private Transform mTransform_Revamp;

		[SerializeField]
		private UIButton mButton_Revamp;

		[SerializeField]
		private UITexture mTexture_RevampTextHover;

		[Header("Arsenal"), SerializeField]
		private Transform mTransform_Arsenal;

		[SerializeField]
		private UIButton mButton_Arsenal;

		[SerializeField]
		private UITexture mTexture_ArsenalTextHover;

		[Header("Ship"), SerializeField]
		private Transform mTransform_ShipFrame;

		[SerializeField]
		private UITexture mTexture_FlagShip;

		[Header("Focus"), SerializeField]
		private Transform mTexture_Focus;

		private Vector3 ShipLocate;

		private UIArsenalSelector.SelectType mSelectType;

		private KeyControl mKeyController;

		private ShipModel mFlagShip;

		private bool mIsShown;

		private Action<UIArsenalSelector.SelectType> mOnArsenaltypeSelectListener;

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mButton_Revamp);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_RevampTextHover, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mButton_Arsenal);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_ArsenalTextHover, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_FlagShip, false);
			this.mTransform_Arsenal = null;
			this.mTransform_ShipFrame = null;
			this.mTexture_Focus = null;
			this.mButtonManager = null;
			this.mTransform_Revamp = null;
			this.mKeyController = null;
			this.mFlagShip = null;
		}

		private void Update()
		{
			if (this.mKeyController != null)
			{
				if (this.mKeyController.keyState.get_Item(14).down)
				{
					if (this.mIsShown)
					{
						this.ChangeFocus(UIArsenalSelector.SelectType.Arsenal);
					}
				}
				else if (this.mKeyController.keyState.get_Item(10).down)
				{
					if (this.mIsShown)
					{
						this.ChangeFocus(UIArsenalSelector.SelectType.Revamp);
					}
				}
				else if (this.mKeyController.keyState.get_Item(1).down)
				{
					if (this.mOnArsenaltypeSelectListener != null && this.mIsShown && !DOTween.IsTweening(this))
					{
						SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
						this.mOnArsenaltypeSelectListener.Invoke(this.mSelectType);
						this.mKeyController = null;
					}
				}
				else if (this.mKeyController.keyState.get_Item(0).down)
				{
					SingletonMonoBehaviour<PortObjectManager>.Instance.BackToPort();
				}
				else if (this.mKeyController.IsRDown() && SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable)
				{
					SingletonMonoBehaviour<PortObjectManager>.Instance.BackToStrategy();
					this.mKeyController = null;
				}
			}
		}

		private void Awake()
		{
			this.mButtonManager = base.GetComponent<UIButtonManager>();
			this.mButtonManager.IndexChangeAct = delegate
			{
				if (this.mIsShown)
				{
					if (this.mButtonManager.nowForcusButton.Equals(this.mButton_Arsenal))
					{
						this.ChangeFocus(UIArsenalSelector.SelectType.Arsenal);
					}
					else if (this.mButtonManager.nowForcusButton.Equals(this.mButton_Revamp))
					{
						this.ChangeFocus(UIArsenalSelector.SelectType.Revamp);
					}
				}
			};
			this.mTransform_Arsenal.get_transform().set_localPosition(this.newVector3(-720f, 0f, 0f));
			this.mTransform_Revamp.get_transform().set_localPosition(this.newVector3(720f, 0f, 0f));
			this.mTransform_ShipFrame.set_localPosition(this.newVector3(0f, -1024f, 0f));
			this.ChangeFocus(UIArsenalSelector.SelectType.None);
		}

		private Vector3 newVector3(float x, float y, float z)
		{
			return Vector3.get_right() * x + Vector3.get_up() * y + Vector3.get_forward() * z;
		}

		public void Initialize(ShipModel flagShip)
		{
			this.mFlagShip = flagShip;
			this.mTexture_FlagShip.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(flagShip.GetGraphicsMstId(), (!flagShip.IsDamaged()) ? 9 : 10);
			this.ShipLocate = Util.Poi2Vec(new ShipOffset(flagShip.GetGraphicsMstId()).GetShipDisplayCenter(flagShip.IsDamaged())) + Vector3.get_up() * 115f;
		}

		public void SetKeyController(KeyControl keyController)
		{
			this.mKeyController = keyController;
		}

		public void SetOnArsenalSelectedListener(Action<UIArsenalSelector.SelectType> selectListener)
		{
			this.mOnArsenaltypeSelectListener = selectListener;
		}

		public void Show()
		{
			DOTween.Kill(this, false);
			Sequence sequence = DOTween.Sequence();
			Tween tween = TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMove(this.mTransform_Arsenal.get_transform(), this.newVector3(-240f, 0f, 0f), 1f, false), 18);
			Tween tween2 = TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMove(this.mTransform_Revamp.get_transform(), this.newVector3(240f, 0f, 0f), 1f, false), 18);
			Tween tween3 = TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMove(this.mTransform_ShipFrame, this.ShipLocate, 1.2f, false), 27);
			TweenSettingsExtensions.SetId<Sequence>(sequence, this);
			TweenSettingsExtensions.SetId<Tween>(tween, this);
			TweenSettingsExtensions.SetId<Tween>(tween2, this);
			TweenSettingsExtensions.SetId<Tween>(tween3, this);
			TweenSettingsExtensions.Join(TweenSettingsExtensions.Join(TweenSettingsExtensions.Append(sequence, tween), tween2), tween3);
			TweenSettingsExtensions.OnComplete<Sequence>(sequence, delegate
			{
				this.mIsShown = true;
				this.ChangeFocus(UIArsenalSelector.SelectType.Arsenal);
			});
		}

		public void Hide()
		{
			Sequence sequence = DOTween.Sequence();
			Tween tween = TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMove(this.mTransform_Arsenal.get_transform(), this.newVector3(-7200f, 0f, 0f), 1f, false), 18);
			Tween tween2 = TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMove(this.mTransform_Revamp.get_transform(), this.newVector3(720f, 0f, 0f), 1f, false), 18);
			Tween tween3 = TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMove(this.mTransform_ShipFrame, this.newVector3(0f, -1024f, 0f), 0.8f, false), 26);
			TweenSettingsExtensions.SetId<Sequence>(sequence, this);
			TweenSettingsExtensions.SetId<Tween>(tween, this);
			TweenSettingsExtensions.SetId<Tween>(tween2, this);
			TweenSettingsExtensions.SetId<Tween>(tween3, this);
			TweenSettingsExtensions.Join(TweenSettingsExtensions.Join(TweenSettingsExtensions.Append(sequence, tween), tween2), tween3);
		}

		private void ChangeFocus(UIArsenalSelector.SelectType selectType)
		{
			Sequence sequence = TweenSettingsExtensions.SetId<Sequence>(DOTween.Sequence(), this);
			if (this.mSelectType == selectType)
			{
				return;
			}
			switch (this.mSelectType)
			{
			case UIArsenalSelector.SelectType.None:
				this.mTexture_RevampTextHover.alpha = 0f;
				this.mTexture_ArsenalTextHover.alpha = 0f;
				break;
			case UIArsenalSelector.SelectType.Arsenal:
			{
				Tween tween = TweenSettingsExtensions.SetId<Tweener>(DOVirtual.Float(this.mTexture_ArsenalTextHover.alpha, 0f, 0.3f, delegate(float alpha)
				{
					this.mTexture_ArsenalTextHover.alpha = alpha;
				}), this);
				TweenSettingsExtensions.Join(sequence, tween);
				break;
			}
			case UIArsenalSelector.SelectType.Revamp:
			{
				Tween tween = TweenSettingsExtensions.SetId<Tweener>(DOVirtual.Float(this.mTexture_RevampTextHover.alpha, 0f, 0.3f, delegate(float alpha)
				{
					this.mTexture_RevampTextHover.alpha = alpha;
				}), this);
				TweenSettingsExtensions.Join(sequence, tween);
				break;
			}
			}
			this.mSelectType = selectType;
			switch (this.mSelectType)
			{
			case UIArsenalSelector.SelectType.None:
				this.mTexture_RevampTextHover.alpha = 0f;
				this.mTexture_ArsenalTextHover.alpha = 0f;
				break;
			case UIArsenalSelector.SelectType.Arsenal:
			{
				Tween tween2 = ShortcutExtensions.DOLocalMoveX(this.mTexture_Focus.get_transform(), this.mTransform_Arsenal.get_transform().get_localPosition().x, 0.3f, false);
				Tween tween3 = TweenSettingsExtensions.SetId<Tweener>(DOVirtual.Float(this.mTexture_ArsenalTextHover.alpha, 1f, 0.3f, delegate(float alpha)
				{
					this.mTexture_ArsenalTextHover.alpha = alpha;
				}), this);
				TweenSettingsExtensions.Join(sequence, tween3);
				TweenSettingsExtensions.Join(sequence, tween2);
				break;
			}
			case UIArsenalSelector.SelectType.Revamp:
			{
				Tween tween2 = ShortcutExtensions.DOLocalMoveX(this.mTexture_Focus.get_transform(), this.mTransform_Revamp.get_transform().get_localPosition().x, 0.3f, false);
				Tween tween3 = TweenSettingsExtensions.SetId<Tweener>(DOVirtual.Float(this.mTexture_RevampTextHover.alpha, 1f, 0.3f, delegate(float alpha)
				{
					this.mTexture_RevampTextHover.alpha = alpha;
				}), this);
				TweenSettingsExtensions.Join(sequence, tween3);
				TweenSettingsExtensions.Join(sequence, tween2);
				break;
			}
			}
		}

		[Obsolete("SerializeField上で使用")]
		public void OnClickArsenal()
		{
			if (this.mSelectType == UIArsenalSelector.SelectType.Arsenal)
			{
				this.OnSelected(UIArsenalSelector.SelectType.Arsenal);
			}
			else
			{
				this.ChangeFocus(UIArsenalSelector.SelectType.Arsenal);
			}
		}

		[Obsolete("SerializeField上で使用")]
		public void OnClickRevamp()
		{
			if (this.mSelectType == UIArsenalSelector.SelectType.Revamp)
			{
				this.OnSelected(UIArsenalSelector.SelectType.Revamp);
			}
			else
			{
				this.ChangeFocus(UIArsenalSelector.SelectType.Revamp);
			}
		}

		private void OnSelected(UIArsenalSelector.SelectType selectType)
		{
			if (this.mOnArsenaltypeSelectListener != null)
			{
				this.mOnArsenaltypeSelectListener.Invoke(selectType);
			}
		}
	}
}
