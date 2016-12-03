using Common.Enum;
using local.models;
using System;
using UnityEngine;

namespace KCV.SortieBattle
{
	[RequireComponent(typeof(UIWidget))]
	public class BaseUISortieBattleShipBanner<ShipModelType> : MonoBehaviour where ShipModelType : ShipModel_Battle
	{
		[SerializeField]
		protected UITexture _uiShipTexture;

		[SerializeField]
		protected UISprite _uiDamageIcon;

		[SerializeField]
		protected UISprite _uiDamageMask;

		[SerializeField]
		protected UILabel _uiShipName;

		[SerializeField]
		protected UISlider _uiHPSlider;

		[SerializeField]
		protected int _nBannerSizeX = 256;

		private UIWidget _uiWidget;

		private ShipModelType _clsShipModel;

		public UIWidget widget
		{
			get
			{
				return this.GetComponentThis(ref this._uiWidget);
			}
			protected set
			{
				this._uiWidget = value;
			}
		}

		public ShipModelType shipModel
		{
			get
			{
				return this._clsShipModel;
			}
			protected set
			{
				this._clsShipModel = value;
			}
		}

		private void OnDestroy()
		{
			Mem.Del<UITexture>(ref this._uiShipTexture);
			Mem.Del(ref this._uiDamageIcon);
			Mem.Del(ref this._uiDamageMask);
			Mem.Del<UILabel>(ref this._uiShipName);
			Mem.Del<UISlider>(ref this._uiHPSlider);
			this.OnUnInit();
		}

		protected virtual void VirtualCtor(ShipModelType model)
		{
			this._clsShipModel = model;
		}

		protected virtual void OnUnInit()
		{
		}

		protected virtual void SetShipInfos(ShipModelType model)
		{
		}

		protected virtual void SetShipName(string strName)
		{
			this._uiShipName.text = strName;
		}

		protected virtual void SetShipBannerTexture(Texture2D texture, bool isDamaged, DamageState_Battle iState, bool isEscape)
		{
			this._uiShipTexture.mainTexture = texture;
			this._uiShipTexture.localSize = ResourceManager.SHIP_TEXTURE_SIZE.get_Item((!isDamaged) ? 1 : 2);
			this._uiShipTexture.shader = this.GetBannerShader(iState, isEscape);
		}

		protected virtual void SetHPSliderValue(int nNowHP, int nMaxHP)
		{
			this._uiHPSlider.value = Mathe.Rate(0f, (float)nMaxHP, (float)nNowHP);
			this._uiHPSlider.foregroundWidget.color = Util.HpGaugeColor2(nMaxHP, nNowHP);
		}

		protected virtual void SetUISettings(bool isFriend)
		{
		}

		protected void SetShipIcons(DamageState_Battle iState, bool isGroundFacility, bool isEscape)
		{
			switch (iState)
			{
			case DamageState_Battle.Normal:
			{
				UIWidget arg_3A_0 = this._uiDamageMask;
				float alpha = 0f;
				this._uiDamageIcon.alpha = alpha;
				arg_3A_0.alpha = alpha;
				break;
			}
			case DamageState_Battle.Shouha:
			{
				UIWidget arg_5D_0 = this._uiDamageMask;
				float alpha = 1f;
				this._uiDamageIcon.alpha = alpha;
				arg_5D_0.alpha = alpha;
				this._uiDamageMask.spriteName = "icon-ss_burned_shoha";
				this._uiDamageIcon.spriteName = ((!isGroundFacility) ? "icon-ss_shoha" : "icon-ss_konran");
				break;
			}
			case DamageState_Battle.Tyuuha:
				this._uiDamageMask.alpha = 1f;
				this._uiDamageIcon.alpha = 1f;
				this._uiDamageMask.spriteName = "icon-ss_burned_chuha";
				this._uiDamageIcon.spriteName = ((!isGroundFacility) ? "icon-ss_chuha" : "icon-ss_songai");
				break;
			case DamageState_Battle.Taiha:
				this._uiDamageMask.alpha = 1f;
				this._uiDamageIcon.alpha = 1f;
				this._uiDamageMask.spriteName = "icon-ss_burned_taiha";
				this._uiDamageIcon.spriteName = ((!isGroundFacility) ? "icon-ss_taiha" : "icon-ss_sonsyo");
				break;
			case DamageState_Battle.Gekichin:
				this._uiDamageMask.alpha = 1f;
				this._uiDamageIcon.alpha = 1f;
				this._uiDamageMask.spriteName = "icon-ss_burned_taiha";
				this._uiDamageIcon.spriteName = ((!isGroundFacility) ? "icon-ss_gekichin" : "icon-ss_hakai");
				break;
			}
			if (isEscape)
			{
				this._uiDamageIcon.spriteName = "icon-ss_taihi";
				this._uiDamageIcon.alpha = 1f;
			}
		}

		protected Shader GetBannerShader(DamageState_Battle iState, bool isEscape)
		{
			return (iState != DamageState_Battle.Gekichin && !isEscape) ? SingletonMonoBehaviour<ResourceManager>.Instance.shader.shaderList.get_Item(9) : SingletonMonoBehaviour<ResourceManager>.Instance.shader.shaderList.get_Item(0);
		}
	}
}
