using DG.Tweening;
using KCV.Scene.Port;
using KCV.View.ScrollView;
using System;
using UnityEngine;

namespace KCV.Remodel
{
	[RequireComponent(typeof(UIWidget))]
	public class UIRemodeModernizationShipTargetListChildNew : MonoBehaviour, UIScrollListItem<RemodeModernizationShipTargetListChildNew, UIRemodeModernizationShipTargetListChildNew>
	{
		[SerializeField]
		private CommonShipBanner mCommonShipBanner;

		[SerializeField]
		private UILabel mLabel_Level;

		[SerializeField]
		private UISprite mSprite_Karyoku;

		[SerializeField]
		private UISprite mSprite_Raisou;

		[SerializeField]
		private UISprite mSprite_Soukou;

		[SerializeField]
		private UISprite mSprite_Taikuu;

		[SerializeField]
		private UISprite mSprite_Luck;

		[SerializeField]
		private Transform mLEVEL;

		[SerializeField]
		private Transform mUNSET;

		[SerializeField]
		private UISprite mListBar;

		private UIWidget mWidgetThis;

		private int mRealIndex;

		private RemodeModernizationShipTargetListChildNew mModel;

		private Action<UIRemodeModernizationShipTargetListChildNew> mOnTouchListener;

		private Transform mCachedTransform;

		private Action<CommonShipBanner> mReleaseRequestShipTexture;

		private void Awake()
		{
			this.mWidgetThis = base.GetComponent<UIWidget>();
		}

		public void Initialize(int realIndex, RemodeModernizationShipTargetListChildNew model)
		{
			UITexture uITexture = this.mCommonShipBanner.GetUITexture();
			Texture mainTexture = uITexture.mainTexture;
			uITexture.mainTexture = null;
			UserInterfaceRemodelManager.instance.ReleaseRequestBanner(ref mainTexture);
			this.mWidgetThis.alpha = 1f;
			if (model.mOption == RemodeModernizationShipTargetListChildNew.ListItemOption.UnSet)
			{
				this.mRealIndex = realIndex;
				this.mModel = null;
				this.mCommonShipBanner.SetActive(false);
				this.mSprite_Karyoku.alpha = 0f;
				this.mSprite_Raisou.alpha = 0f;
				this.mSprite_Soukou.alpha = 0f;
				this.mSprite_Taikuu.alpha = 0f;
				this.mSprite_Luck.alpha = 0f;
				this.mLabel_Level.alpha = 0f;
				this.mLabel_Level.text = "はずす";
				this.mLEVEL.set_localScale(Vector3.get_zero());
				this.mUNSET.set_localScale(Vector3.get_one());
			}
			else
			{
				this.mRealIndex = realIndex;
				this.mModel = model;
				this.mCommonShipBanner.SetActive(true);
				this.mSprite_Karyoku.alpha = 1f;
				this.mSprite_Raisou.alpha = 1f;
				this.mSprite_Soukou.alpha = 1f;
				this.mSprite_Taikuu.alpha = 1f;
				this.mSprite_Luck.alpha = 1f;
				this.mLabel_Level.alpha = 1f;
				this.mLEVEL.set_localScale(Vector3.get_one());
				this.mUNSET.set_localScale(Vector3.get_zero());
				if (0 < model.mShipModel.PowUpKaryoku)
				{
					this.mSprite_Karyoku.spriteName = "icon_1_on";
				}
				else
				{
					this.mSprite_Karyoku.spriteName = "icon_1";
				}
				if (0 < model.mShipModel.PowUpRaisou)
				{
					this.mSprite_Raisou.spriteName = "icon_2_on";
				}
				else
				{
					this.mSprite_Raisou.spriteName = "icon_2";
				}
				if (0 < model.mShipModel.PowUpSoukou)
				{
					this.mSprite_Soukou.spriteName = "icon_3_on";
				}
				else
				{
					this.mSprite_Soukou.spriteName = "icon_3";
				}
				if (0 < model.mShipModel.PowUpTaikuu)
				{
					this.mSprite_Taikuu.spriteName = "icon_4_on";
				}
				else
				{
					this.mSprite_Taikuu.spriteName = "icon_4";
				}
				if (0 < model.mShipModel.PowUpLucky)
				{
					this.mSprite_Luck.spriteName = "icon_5_on";
				}
				else
				{
					this.mSprite_Luck.spriteName = "icon_5";
				}
				this.mCommonShipBanner.SetShipData(model.mShipModel);
				this.mCommonShipBanner.GetUITexture().alpha = 1f;
				this.mCommonShipBanner.StopParticle();
				this.mLabel_Level.text = model.mShipModel.Level.ToString();
			}
		}

		public void InitializeDefault(int realIndex)
		{
			if (DOTween.IsTweening(this))
			{
				DOTween.Kill(this, false);
			}
			UITexture uITexture = this.mCommonShipBanner.GetUITexture();
			Texture mainTexture = uITexture.mainTexture;
			uITexture.mainTexture = null;
			UserInterfaceRemodelManager.instance.ReleaseRequestBanner(ref mainTexture);
			this.mModel = null;
			this.mRealIndex = realIndex;
			base.set_name("[" + realIndex + "]");
			this.mWidgetThis.alpha = 1E-09f;
		}

		public int GetRealIndex()
		{
			return this.mRealIndex;
		}

		public RemodeModernizationShipTargetListChildNew GetModel()
		{
			return this.mModel;
		}

		public int GetHeight()
		{
			return 58;
		}

		public void SetOnTouchListener(Action<UIRemodeModernizationShipTargetListChildNew> onTouchListener)
		{
			this.mOnTouchListener = onTouchListener;
		}

		private void OnTouchEvent()
		{
			if (this.mOnTouchListener != null)
			{
				this.mOnTouchListener.Invoke(this);
			}
		}

		public void Hover()
		{
			UISelectedObject.SelectedOneObjectBlink(this.mListBar.get_gameObject(), true);
		}

		public Transform GetTransform()
		{
			if (this.mCachedTransform == null)
			{
				this.mCachedTransform = base.get_transform();
			}
			return this.mCachedTransform;
		}

		public void RemoveHover()
		{
			UISelectedObject.SelectedOneObjectBlink(this.mListBar.get_gameObject(), false);
		}

		public void OnClick()
		{
			this.OnTouchEvent();
		}

		public CommonShipBanner GetShipBanner()
		{
			return this.mCommonShipBanner;
		}

		private void OnDestroy()
		{
			if (DOTween.IsTweening(this))
			{
				DOTween.Kill(this, false);
			}
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabel_Level);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mSprite_Karyoku);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mSprite_Raisou);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mSprite_Soukou);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mSprite_Taikuu);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mSprite_Luck);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mListBar);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mWidgetThis);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mCommonShipBanner, true);
			this.mLEVEL = null;
			this.mUNSET = null;
			this.mModel = null;
			this.mOnTouchListener = null;
			this.mCachedTransform = null;
			this.mReleaseRequestShipTexture = null;
		}
	}
}
