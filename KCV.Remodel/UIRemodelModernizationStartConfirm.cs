using Common.Struct;
using KCV.Scene.Port;
using KCV.Utils;
using local.models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Remodel
{
	[SelectionBase]
	public class UIRemodelModernizationStartConfirm : MonoBehaviour, UIRemodelView, IBannerResourceManage
	{
		private const string MAX_POWER_UP_BBCODE = "[e3904d]";

		private const string POWER_UP_BBCODE = "[00a4ff]";

		private const string DEFAULT_COLOR_BBCODE = "[202020]";

		private const string BBCODE_CLOSE = "[-]";

		[SerializeField]
		private UIRemodelModernizationStartConfirmSlot[] mUIRemodelModernizationStartConfirmSlots;

		[SerializeField]
		private UILabel[] mLabel_PrevParams;

		[SerializeField]
		private UILabel[] mLabel_NextParams;

		[SerializeField]
		private Transform[] mTransform_MaxTags;

		[SerializeField]
		private Transform[] mTransform_Arrows;

		[SerializeField]
		private UIButton mButton_Positive;

		[SerializeField]
		private UIButton mButton_Negative;

		[SerializeField]
		private UITexture mTexture_Ship;

		[SerializeField]
		private UITexture mTexture_background;

		[SerializeField]
		public Camera camera;

		private UIPanel popupString;

		[SerializeField]
		private UITexture BehindTexture_Ship;

		private UIWidget mUIWidgetThis;

		private Vector3 showScale = Vector3.get_one();

		private Vector3 hideScale = Vector3.get_one() * 0.5f;

		private ShipModel mEatShipModel;

		private List<ShipModel> mBaitShipmodels;

		private UIButton mButtonCurrentFocus;

		private KeyControl mKeyController;

		private UIButton[] mButtonFocasable;

		private UIButton _BeforeFocus;

		private bool animating;

		private UIPanel PopupString
		{
			get
			{
				if (this.popupString == null)
				{
					this.popupString = GameObject.Find("PopupMessage").GetComponent<UIPanel>();
				}
				return this.popupString;
			}
		}

		private void Awake()
		{
			this.mUIWidgetThis = base.GetComponent<UIWidget>();
			this.mUIWidgetThis.alpha = 0.001f;
			if (this.BehindTexture_Ship == null)
			{
				this.BehindTexture_Ship = GameObject.Find("UIComponents/UIRemodelShipStatus/Texture_Ship").GetComponent<UITexture>();
			}
			this.Hide(false);
		}

		private void Start()
		{
		}

		private void Update()
		{
			if (this.mKeyController != null && base.get_enabled() && !this.animating)
			{
				if (this.mKeyController.IsLeftDown())
				{
					this.ChangeFocusButton(this.mButton_Negative);
				}
				else if (this.mKeyController.IsRightDown())
				{
					this.ChangeFocusButton(this.mButton_Positive);
				}
				else if (this.mKeyController.IsMaruDown())
				{
					if (this.mButtonCurrentFocus.Equals(this.mButton_Negative))
					{
						this.Back();
					}
					else if (this.mButtonCurrentFocus.Equals(this.mButton_Positive))
					{
						this.Forward();
					}
				}
				else if (this.mKeyController.IsBatuDown())
				{
					this.Back();
				}
			}
		}

		public void DrawShip(ShipModel eatShipModel)
		{
			this.mEatShipModel = eatShipModel;
			Point face = this.mEatShipModel.Offsets.GetFace(eatShipModel.IsDamaged());
			this.mTexture_Ship.get_transform().set_localPosition(new Vector3((float)face.x, (float)face.y));
			this.mTexture_Ship.mainTexture = ShipUtils.LoadTexture(this.mEatShipModel, (!this.mEatShipModel.IsDamaged()) ? 9 : 10);
			this.mTexture_Ship.MakePixelPerfect();
			this.mTexture_Ship.get_transform().set_localPosition(Util.Poi2Vec(this.mEatShipModel.Offsets.GetShipDisplayCenter(this.mEatShipModel.IsDamaged())));
		}

		public void Initialize(KeyControl keyController, ShipModel eatShipModel, List<ShipModel> baitShipModels, PowUpInfo powerUpInfo)
		{
			base.set_enabled(true);
			this.camera.get_gameObject().SetActive(true);
			this.mKeyController = keyController;
			this.mEatShipModel = eatShipModel;
			this.mBaitShipmodels = baitShipModels;
			this._BeforeFocus = this.mButton_Positive;
			this.mButtonCurrentFocus = this.mButton_Negative;
			UIRemodelModernizationStartConfirmSlot[] array = this.mUIRemodelModernizationStartConfirmSlots;
			for (int i = 0; i < array.Length; i++)
			{
				UIRemodelModernizationStartConfirmSlot uIRemodelModernizationStartConfirmSlot = array[i];
				uIRemodelModernizationStartConfirmSlot.StopKira();
			}
			for (int j = 0; j < this.mUIRemodelModernizationStartConfirmSlots.Length; j++)
			{
				if (j < baitShipModels.get_Count())
				{
					this.mUIRemodelModernizationStartConfirmSlots[j].Initialize(baitShipModels.get_Item(j));
				}
				else
				{
					this.mUIRemodelModernizationStartConfirmSlots[j].Initialize(null);
				}
			}
			int num = eatShipModel.Karyoku + powerUpInfo.Karyoku;
			if (eatShipModel.KaryokuMax <= eatShipModel.Karyoku)
			{
				this.mTransform_MaxTags[0].set_localScale(Vector3.get_one());
				this.mTransform_Arrows[0].SetActive(false);
				this.mLabel_PrevParams[0].text = "[202020]" + eatShipModel.Karyoku.ToString() + "[-]";
				this.mLabel_NextParams[0].text = string.Empty;
			}
			else if (0 < powerUpInfo.Karyoku)
			{
				if (eatShipModel.KaryokuMax <= num)
				{
					this.mLabel_PrevParams[0].text = "[e3904d]" + eatShipModel.Karyoku.ToString() + "[-]";
					this.mLabel_NextParams[0].text = string.Empty;
					this.mTransform_Arrows[0].SetActive(false);
					this.mTransform_MaxTags[0].set_localScale(Vector3.get_one());
				}
				else
				{
					this.mLabel_PrevParams[0].text = "[202020]" + eatShipModel.Karyoku.ToString() + "[-]";
					this.mLabel_NextParams[0].text = "[00a4ff]" + num.ToString() + "[-]";
					this.mTransform_Arrows[0].SetActive(true);
					this.mTransform_MaxTags[0].set_localScale(Vector3.get_zero());
				}
			}
			else
			{
				this.mLabel_PrevParams[0].text = "[202020]" + eatShipModel.Karyoku.ToString() + "[-]";
				this.mLabel_NextParams[0].text = "[202020]" + eatShipModel.Karyoku.ToString() + "[-]";
				this.mTransform_MaxTags[0].set_localScale(Vector3.get_zero());
				this.mTransform_Arrows[0].SetActive(false);
			}
			int num2 = eatShipModel.Raisou + powerUpInfo.Raisou;
			if (eatShipModel.RaisouMax <= eatShipModel.Raisou)
			{
				this.mTransform_MaxTags[1].set_localScale(Vector3.get_one());
				this.mTransform_Arrows[1].SetActive(false);
				this.mLabel_PrevParams[1].text = "[202020]" + eatShipModel.Raisou.ToString() + "[-]";
				this.mLabel_NextParams[1].text = string.Empty;
			}
			else if (0 < powerUpInfo.Raisou)
			{
				if (eatShipModel.RaisouMax <= num2)
				{
					this.mLabel_PrevParams[1].text = "[e3904d]" + eatShipModel.Raisou.ToString() + "[-]";
					this.mLabel_NextParams[1].text = string.Empty;
					this.mTransform_Arrows[1].SetActive(false);
					this.mTransform_MaxTags[1].set_localScale(Vector3.get_one());
				}
				else
				{
					this.mLabel_PrevParams[1].text = "[202020]" + eatShipModel.Raisou.ToString() + "[-]";
					this.mLabel_NextParams[1].text = "[00a4ff]" + num2.ToString() + "[-]";
					this.mTransform_Arrows[1].SetActive(true);
					this.mTransform_MaxTags[1].set_localScale(Vector3.get_zero());
				}
			}
			else
			{
				this.mLabel_PrevParams[1].text = "[202020]" + eatShipModel.Raisou.ToString() + "[-]";
				this.mLabel_NextParams[1].text = "[202020]" + eatShipModel.Raisou.ToString() + "[-]";
				this.mTransform_MaxTags[1].set_localScale(Vector3.get_zero());
				this.mTransform_Arrows[1].SetActive(false);
			}
			int num3 = eatShipModel.Taiku + powerUpInfo.Taiku;
			if (eatShipModel.TaikuMax <= eatShipModel.Taiku)
			{
				this.mTransform_MaxTags[2].set_localScale(Vector3.get_one());
				this.mTransform_Arrows[2].SetActive(false);
				this.mLabel_PrevParams[2].text = "[202020]" + eatShipModel.Taiku.ToString() + "[-]";
				this.mLabel_NextParams[2].text = string.Empty;
			}
			else if (0 < powerUpInfo.Taiku)
			{
				if (eatShipModel.TaikuMax <= num3)
				{
					this.mLabel_PrevParams[2].text = "[e3904d]" + eatShipModel.Taiku.ToString() + "[-]";
					this.mLabel_NextParams[2].text = string.Empty;
					this.mTransform_Arrows[2].SetActive(false);
					this.mTransform_MaxTags[2].set_localScale(Vector3.get_one());
				}
				else
				{
					this.mLabel_PrevParams[2].text = "[202020]" + eatShipModel.Taiku.ToString() + "[-]";
					this.mLabel_NextParams[2].text = "[00a4ff]" + num3.ToString() + "[-]";
					this.mTransform_Arrows[2].SetActive(true);
					this.mTransform_MaxTags[2].set_localScale(Vector3.get_zero());
				}
			}
			else
			{
				this.mLabel_PrevParams[2].text = "[202020]" + eatShipModel.Taiku.ToString() + "[-]";
				this.mLabel_NextParams[2].text = "[202020]" + eatShipModel.Taiku.ToString() + "[-]";
				this.mTransform_MaxTags[2].set_localScale(Vector3.get_zero());
				this.mTransform_Arrows[2].SetActive(false);
			}
			int num4 = eatShipModel.Soukou + powerUpInfo.Soukou;
			if (eatShipModel.SoukouMax <= eatShipModel.Soukou)
			{
				this.mTransform_MaxTags[3].set_localScale(Vector3.get_one());
				this.mTransform_Arrows[3].SetActive(false);
				this.mLabel_PrevParams[3].text = "[202020]" + eatShipModel.Soukou.ToString() + "[-]";
				this.mLabel_NextParams[3].text = string.Empty;
			}
			else if (0 < powerUpInfo.Soukou)
			{
				if (eatShipModel.SoukouMax <= num4)
				{
					this.mLabel_PrevParams[3].text = "[e3904d]" + eatShipModel.Soukou.ToString() + "[-]";
					this.mLabel_NextParams[3].text = string.Empty;
					this.mTransform_Arrows[3].SetActive(false);
					this.mTransform_MaxTags[3].set_localScale(Vector3.get_one());
				}
				else
				{
					this.mLabel_PrevParams[3].text = "[202020]" + eatShipModel.Soukou.ToString() + "[-]";
					this.mLabel_NextParams[3].text = "[00a4ff]" + num4.ToString() + "[-]";
					this.mTransform_Arrows[3].SetActive(true);
					this.mTransform_MaxTags[3].set_localScale(Vector3.get_zero());
				}
			}
			else
			{
				this.mLabel_PrevParams[3].text = "[202020]" + eatShipModel.Soukou.ToString() + "[-]";
				this.mLabel_NextParams[3].text = "[202020]" + eatShipModel.Soukou.ToString() + "[-]";
				this.mTransform_MaxTags[3].set_localScale(Vector3.get_zero());
				this.mTransform_Arrows[3].SetActive(false);
			}
			int num5 = eatShipModel.Lucky + powerUpInfo.Lucky;
			if (eatShipModel.LuckyMax <= eatShipModel.Lucky)
			{
				this.mTransform_MaxTags[4].set_localScale(Vector3.get_one());
				this.mTransform_Arrows[4].SetActive(false);
				this.mLabel_PrevParams[4].text = "[202020]" + eatShipModel.Lucky.ToString() + "[-]";
				this.mLabel_NextParams[4].text = string.Empty;
			}
			else if (0 < powerUpInfo.Lucky)
			{
				if (eatShipModel.LuckyMax <= num5)
				{
					this.mLabel_PrevParams[4].text = "[e3904d]" + eatShipModel.Lucky.ToString() + "[-]";
					this.mLabel_NextParams[4].text = string.Empty;
					this.mTransform_Arrows[4].SetActive(false);
					this.mTransform_MaxTags[4].set_localScale(Vector3.get_one());
				}
				else
				{
					this.mLabel_PrevParams[4].text = "[202020]" + eatShipModel.Lucky.ToString() + "[-]";
					this.mLabel_NextParams[4].text = "[00a4ff]" + num5.ToString() + "[-]";
					this.mTransform_Arrows[4].SetActive(true);
					this.mTransform_MaxTags[4].set_localScale(Vector3.get_zero());
				}
			}
			else
			{
				this.mLabel_PrevParams[4].text = "[202020]" + eatShipModel.Lucky.ToString() + "[-]";
				this.mLabel_NextParams[4].text = "[202020]" + eatShipModel.Lucky.ToString() + "[-]";
				this.mTransform_MaxTags[4].set_localScale(Vector3.get_zero());
				this.mTransform_Arrows[4].SetActive(false);
			}
			List<UIButton> list = new List<UIButton>();
			list.Add(this.mButton_Negative);
			list.Add(this.mButton_Positive);
			this.mButtonFocasable = list.ToArray();
			this.mButtonFocasable[0].GetComponent<UISprite>().spriteName = "btn_cancel_on";
		}

		private void Forward()
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
			if (this.animating)
			{
				return;
			}
			base.StartCoroutine(this.HideAnimation());
		}

		private void Back()
		{
			if (this.animating)
			{
				return;
			}
			this.Hide();
			UserInterfaceRemodelManager.instance.Back2KindaikaKaishu();
		}

		public void SetKeyController(KeyControl keyController)
		{
			if (keyController != null)
			{
				keyController.ClearKeyAll();
				keyController.firstUpdate = true;
			}
			this.mKeyController = keyController;
		}

		public void Show()
		{
			base.set_enabled(true);
			this.animating = true;
			this.PopupString.set_enabled(false);
			base.get_transform().set_localScale(this.hideScale);
			this.camera.SetActive(true);
			this.ChangeFocusButton(this.mButton_Positive, true);
			this.ChangeFocusButton(this.mButton_Negative, true);
			this.ChangeFocusButton(this.mButton_Positive, true);
			this.ChangeFocusButton(this.mButton_Negative, true);
			TweenScale tweenScale = TweenScale.Begin(base.get_gameObject(), 0.5f, this.showScale);
			tweenScale.animationCurve = UtilCurves.TweenEaseOutBack;
			TweenAlpha.Begin(this.mTexture_background.get_gameObject(), 0.1f, 0.2f);
			TweenAlpha tweenAlpha = TweenAlpha.Begin(base.get_gameObject(), 0.55f, 1f);
			EventDelegate.Set(tweenAlpha.onFinished, delegate
			{
				base.get_gameObject().get_transform().set_localScale(this.showScale);
				this.animating = false;
			});
		}

		public void Hide()
		{
			this.Hide(true);
		}

		public void Hide(bool animation)
		{
			base.set_enabled(false);
			this.mTransform_Arrows[0].SetActive(false);
			this.mTransform_Arrows[1].SetActive(false);
			this.mTransform_Arrows[2].SetActive(false);
			this.mTransform_Arrows[3].SetActive(false);
			this.mTransform_Arrows[4].SetActive(false);
			this.camera.SetActive(false);
			if (animation)
			{
				this.animating = true;
				TweenScale.Begin(base.get_gameObject(), 0.3f, this.hideScale);
				TweenAlpha tweenAlpha = TweenAlpha.Begin(base.get_gameObject(), 0.3f, 0f);
				EventDelegate.Set(tweenAlpha.onFinished, delegate
				{
					this.animating = false;
				});
			}
			else
			{
				this.mUIWidgetThis.alpha = 0.001f;
			}
			this.PopupString.set_enabled(true);
		}

		[DebuggerHidden]
		public IEnumerator HideAnimation()
		{
			UIRemodelModernizationStartConfirm.<HideAnimation>c__IteratorB3 <HideAnimation>c__IteratorB = new UIRemodelModernizationStartConfirm.<HideAnimation>c__IteratorB3();
			<HideAnimation>c__IteratorB.<>f__this = this;
			return <HideAnimation>c__IteratorB;
		}

		private void ChangeFocusButton(UIButton targetButton)
		{
			this.ChangeFocusButton(targetButton, false);
		}

		private void ChangeFocusButton(UIButton targetButton, bool isSirent)
		{
			if (this.mButtonCurrentFocus != null)
			{
				this.mButtonCurrentFocus.SetState(UIButtonColor.State.Normal, true);
			}
			this.mButtonCurrentFocus = targetButton;
			if (this.mButtonCurrentFocus != null)
			{
				this.mButtonCurrentFocus.SetState(UIButtonColor.State.Hover, true);
			}
			if (this._BeforeFocus != targetButton)
			{
				if (!isSirent)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				}
				this._BeforeFocus = targetButton;
			}
		}

		public void OnTouchPositive()
		{
			this.ChangeFocusButton(this.mButton_Positive);
			this.Forward();
		}

		public void OnTouchNegative()
		{
			this.ChangeFocusButton(this.mButton_Negative);
			this.Back();
		}

		public void OnTouchBack()
		{
			if (UserInterfaceRemodelManager.instance.status == ScreenStatus.MODE_KINDAIKA_KAISHU_KAKUNIN)
			{
				this.Back();
			}
		}

		private void OnDestroy()
		{
			if (this.mUIRemodelModernizationStartConfirmSlots != null)
			{
				for (int i = 0; i < this.mUIRemodelModernizationStartConfirmSlots.Length; i++)
				{
					this.mUIRemodelModernizationStartConfirmSlots[i] = null;
				}
			}
			this.mUIRemodelModernizationStartConfirmSlots = null;
			if (this.mLabel_PrevParams != null)
			{
				for (int j = 0; j < this.mLabel_PrevParams.Length; j++)
				{
					if (this.mLabel_PrevParams[j] != null)
					{
						UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabel_PrevParams[j]);
					}
				}
			}
			this.mLabel_PrevParams = null;
			if (this.mLabel_NextParams != null)
			{
				for (int k = 0; k < this.mLabel_NextParams.Length; k++)
				{
					UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabel_NextParams[k]);
				}
			}
			this.mLabel_NextParams = null;
			if (this.mTransform_MaxTags != null)
			{
				for (int l = 0; l < this.mTransform_MaxTags.Length; l++)
				{
					this.mTransform_MaxTags[l] = null;
				}
			}
			this.mTransform_MaxTags = null;
			if (this.mTransform_Arrows != null)
			{
				for (int m = 0; m < this.mTransform_Arrows.Length; m++)
				{
					this.mTransform_Arrows[m] = null;
				}
				this.mTransform_Arrows = null;
			}
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mButton_Positive);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mButton_Negative);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_Ship, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_background, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.popupString);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.BehindTexture_Ship, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mUIWidgetThis);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mButtonCurrentFocus);
			UserInterfacePortManager.ReleaseUtils.Releases(ref this.mButtonFocasable);
			UserInterfacePortManager.ReleaseUtils.Release(ref this._BeforeFocus);
			this.camera = null;
			this.mEatShipModel = null;
			this.mBaitShipmodels.Clear();
			this.mBaitShipmodels = null;
			this.mKeyController = null;
		}

		public CommonShipBanner[] GetBanner()
		{
			List<CommonShipBanner> list = new List<CommonShipBanner>();
			UIRemodelModernizationStartConfirmSlot[] array = this.mUIRemodelModernizationStartConfirmSlots;
			for (int i = 0; i < array.Length; i++)
			{
				UIRemodelModernizationStartConfirmSlot uIRemodelModernizationStartConfirmSlot = array[i];
				CommonShipBanner shipBanner = uIRemodelModernizationStartConfirmSlot.GetShipBanner();
				list.Add(shipBanner);
			}
			return list.ToArray();
		}
	}
}
