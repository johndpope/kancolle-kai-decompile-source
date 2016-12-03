using local.models;
using System;
using UnityEngine;

namespace KCV.Strategy.Rebellion
{
	public class UIRebellionOrgaizeShipBanner : CommonShipBanner
	{
		[SerializeField]
		private UISprite _uiBackground;

		[SerializeField]
		private UILabel _uiIndex;

		[SerializeField]
		private UISlider _uiHpSlider;

		[SerializeField]
		private UILabel _uiLv;

		[SerializeField]
		private UILabel _uiName;

		[SerializeField]
		private CommonShipSupplyState _uiSupplyState;

		[SerializeField]
		private UiStarManager _uiStarManager;

		[SerializeField]
		private BannerShutter _bunnerShutter;

		[SerializeField]
		private GameObject _shipState;

		private int _nIndex;

		private Action<int> OnClickDelegate;

		public int index
		{
			get
			{
				return this._nIndex;
			}
		}

		public static UIRebellionOrgaizeShipBanner Instantiate(UIRebellionOrgaizeShipBanner prefab, Transform parent, int nIndex)
		{
			UIRebellionOrgaizeShipBanner uIRebellionOrgaizeShipBanner = Object.Instantiate<UIRebellionOrgaizeShipBanner>(prefab);
			uIRebellionOrgaizeShipBanner.get_transform().set_parent(parent);
			uIRebellionOrgaizeShipBanner.get_transform().localScaleOne();
			uIRebellionOrgaizeShipBanner.get_transform().localPositionZero();
			uIRebellionOrgaizeShipBanner.Setup(nIndex);
			return uIRebellionOrgaizeShipBanner;
		}

		private bool Setup(int nIndex)
		{
			base.Awake();
			if (this._uiBackground == null)
			{
				Util.FindParentToChild<UISprite>(ref this._uiBackground, base.get_transform(), "Background");
			}
			if (this._uiIndex == null)
			{
				Util.FindParentToChild<UILabel>(ref this._uiIndex, base.get_transform(), "Index");
			}
			if (this._uiHpSlider == null)
			{
				Util.FindParentToChild<UISlider>(ref this._uiHpSlider, base.get_transform(), "HPSlider");
			}
			if (this._uiLv == null)
			{
				Util.FindParentToChild<UILabel>(ref this._uiLv, base.get_transform(), "Lv");
			}
			if (this._uiName == null)
			{
				Util.FindParentToChild<UILabel>(ref this._uiName, base.get_transform(), "Name");
			}
			if (this._uiSupplyState == null)
			{
				Util.FindParentToChild<CommonShipSupplyState>(ref this._uiSupplyState, base.get_transform(), "Materials");
			}
			if (this._bunnerShutter == null)
			{
				Transform transform = base.get_transform().FindChild("BannerShutter");
				if (transform != null)
				{
					this._bunnerShutter = transform.GetComponent<BannerShutter>();
				}
			}
			this._uiStarManager.init(0);
			this._nIndex = nIndex;
			return true;
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Mem.Del(ref this._uiBackground);
			Mem.Del<UILabel>(ref this._uiIndex);
			Mem.Del<UISlider>(ref this._uiHpSlider);
			Mem.Del<UILabel>(ref this._uiLv);
			Mem.Del<UILabel>(ref this._uiName);
			Mem.Del<CommonShipSupplyState>(ref this._uiSupplyState);
			Mem.Del<UiStarManager>(ref this._uiStarManager);
			Mem.Del<int>(ref this._nIndex);
			Mem.Del<BannerShutter>(ref this._bunnerShutter);
			Mem.Del<GameObject>(ref this._shipState);
		}

		public void SetShipData(ShipModel model, int nIndex)
		{
			base.SetShipData(model);
			if (model == null)
			{
				this._uiStarManager.SetStar(0);
				if (this._bunnerShutter != null)
				{
					this._bunnerShutter.SetActive(true);
					if (this._shipState != null)
					{
						UISelectedObject.SelectedOneObjectBlink(this._uiBackground.get_gameObject(), false);
						this._shipState.SetActive(false);
					}
				}
				else
				{
					base.get_transform().localScaleZero();
				}
				return;
			}
			if (this._shipState != null)
			{
				this._shipState.SetActive(true);
			}
			base.get_transform().localScaleOne();
			if (this._bunnerShutter != null)
			{
				this._bunnerShutter.SetFocusLight(false);
				this._bunnerShutter.SetActive(false);
			}
			this._uiIndex.textInt = nIndex;
			this._uiHpSlider.value = Mathe.Rate(0f, (float)model.MaxHp, (float)model.NowHp);
			this._uiHpSlider.foregroundWidget.color = Util.HpLabelColor(model.MaxHp, model.NowHp);
			this._uiLv.textInt = model.Level;
			this._uiName.text = model.Name;
			this._uiSupplyState.setSupplyState(model);
			this._uiSupplyState.SetActive(this._uiSupplyState.isEitherSupplyNeeds);
			this._uiStarManager.SetStar(model.Srate);
		}

		public void SetShipIndex(int index)
		{
			this._nIndex = index;
		}

		public void SetFocus(bool isEnable)
		{
			if (this._bunnerShutter != null && this._clsShipModel == null)
			{
				this._bunnerShutter.SetFocusLight(isEnable);
			}
			else
			{
				UISelectedObject.SelectedOneObjectBlink(this._uiBackground.get_gameObject(), isEnable);
			}
		}

		public void UnsetFocus()
		{
			if (this._bunnerShutter != null)
			{
				this._bunnerShutter.SetFocusLight(false);
			}
			UISelectedObject.SelectedOneObjectBlink(this._uiBackground.get_gameObject(), false);
		}

		public void SetOnClick(Action<int> dele)
		{
			this.OnClickDelegate = dele;
		}

		private void OnClick()
		{
			this.OnClickDelegate.Invoke(this._nIndex);
		}
	}
}
