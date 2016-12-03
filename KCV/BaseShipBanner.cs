using Common.Enum;
using KCV.Utils;
using local.models;
using System;
using UnityEngine;

namespace KCV
{
	public class BaseShipBanner : MonoBehaviour
	{
		protected ShipModel _clsShipModel;

		protected ShipModel_Battle _clsShipModelBattle;

		protected IShipModel _clsIShipModel;

		[SerializeField]
		protected UITexture _uiShipTex;

		[SerializeField]
		protected UISprite _uiDamageIcon;

		[SerializeField]
		protected UISprite _uiDamageMask;

		public ShipModel ShipModel
		{
			get
			{
				return this._clsShipModel;
			}
		}

		public ShipModel_Battle ShipModeBattle
		{
			get
			{
				return this._clsShipModelBattle;
			}
		}

		public IShipModel IShipModel
		{
			get
			{
				return this._clsIShipModel;
			}
		}

		protected virtual void Awake()
		{
			this._uiDamageMask.alpha = 0f;
			this._uiDamageIcon.alpha = 0f;
		}

		protected virtual void OnDestroy()
		{
			this._uiShipTex = null;
			this._uiDamageIcon = null;
			this._uiDamageMask = null;
		}

		public virtual void SetShipData(IShipModel model)
		{
			if (model == null)
			{
				return;
			}
			this._clsIShipModel = model;
		}

		public virtual void SetShipData(ShipModel model)
		{
			if (model == null)
			{
				return;
			}
			this._clsShipModel = model;
			int texNum = (!model.IsDamaged()) ? 1 : 2;
			this._Load(model.MstId, texNum);
		}

		public virtual void SetShipData(ShipModel_BattleAll model)
		{
			this._clsIShipModel = model;
			if (model == null)
			{
				return;
			}
			this._uiShipTex.mainTexture = ShipUtils.LoadBannerTexture(model);
			this._uiShipTex.localSize = ResourceManager.SHIP_TEXTURE_SIZE.get_Item(1);
			this._uiShipTex.shader = ((model.DmgStateEnd != DamageState_Battle.Gekichin && !model.IsEscape()) ? SingletonMonoBehaviour<ResourceManager>.Instance.shader.shaderList.get_Item(1) : SingletonMonoBehaviour<ResourceManager>.Instance.shader.shaderList.get_Item(0));
			this.UpdateDamage(model.DmgStateEnd, model.IsEscape());
			this._uiShipTex.MakePixelPerfect();
		}

		public virtual void SetShipData(ShipModel_BattleResult model)
		{
			this._clsIShipModel = model;
			if (model == null)
			{
				return;
			}
			this._uiShipTex.mainTexture = ShipUtils.LoadBannerTexture(model);
			this._uiShipTex.localSize = ResourceManager.SHIP_TEXTURE_SIZE.get_Item((!model.IsDamaged()) ? 1 : 2);
			this._uiShipTex.shader = ((model.DmgStateEnd != DamageState_Battle.Gekichin && !model.IsEscape()) ? SingletonMonoBehaviour<ResourceManager>.Instance.shader.shaderList.get_Item(1) : SingletonMonoBehaviour<ResourceManager>.Instance.shader.shaderList.get_Item(0));
			this.UpdateDamage(model.DmgStateEnd, model.IsEscape());
		}

		protected virtual void _Load(int shipID, int texNum)
		{
			this._uiShipTex.mainTexture = ShipUtils.LoadTexture(shipID, texNum);
			this._uiShipTex.MakePixelPerfect();
		}

		public virtual void UpdateDamage(DamageState state)
		{
			switch (state)
			{
			case DamageState.Normal:
				this._uiDamageMask.alpha = 0f;
				this._uiDamageIcon.alpha = 0f;
				break;
			case DamageState.Shouha:
				this._uiDamageMask.alpha = 1f;
				this._uiDamageIcon.alpha = 1f;
				this._uiDamageMask.spriteName = "icon-ss_burned_shoha";
				this._uiDamageIcon.spriteName = "icon-ss_shoha";
				break;
			case DamageState.Tyuuha:
				this._uiDamageMask.alpha = 1f;
				this._uiDamageIcon.alpha = 1f;
				this._uiDamageMask.spriteName = "icon-ss_burned_chuha";
				this._uiDamageIcon.spriteName = "icon-ss_chuha";
				break;
			case DamageState.Taiha:
				this._uiDamageMask.alpha = 1f;
				this._uiDamageIcon.alpha = 1f;
				this._uiDamageMask.spriteName = "icon-ss_burned_taiha";
				this._uiDamageIcon.spriteName = "icon-ss_taiha";
				break;
			}
		}

		public virtual void UpdateDamage(DamageState_Battle state, bool isEscape)
		{
			switch (state)
			{
			case DamageState_Battle.Normal:
				this._uiDamageMask.alpha = 0f;
				this._uiDamageIcon.alpha = 0f;
				break;
			case DamageState_Battle.Shouha:
				this._uiDamageMask.alpha = 1f;
				this._uiDamageIcon.alpha = 1f;
				this._uiDamageMask.spriteName = "icon-ss_burned_shoha";
				this._uiDamageIcon.spriteName = "icon-ss_shoha";
				break;
			case DamageState_Battle.Tyuuha:
				this._uiDamageMask.alpha = 1f;
				this._uiDamageIcon.alpha = 1f;
				this._uiDamageMask.spriteName = "icon-ss_burned_chuha";
				this._uiDamageIcon.spriteName = "icon-ss_chuha";
				break;
			case DamageState_Battle.Taiha:
				this._uiDamageMask.alpha = 1f;
				this._uiDamageIcon.alpha = 1f;
				this._uiDamageMask.spriteName = "icon-ss_burned_taiha";
				this._uiDamageIcon.spriteName = "icon-ss_taiha";
				break;
			case DamageState_Battle.Gekichin:
				this._uiDamageMask.alpha = 1f;
				this._uiDamageIcon.alpha = 1f;
				this._uiDamageMask.spriteName = "icon-ss_burned_taiha";
				this._uiDamageIcon.spriteName = "icon-ss_gekichin";
				break;
			}
			if (isEscape)
			{
				this._uiDamageIcon.spriteName = "icon-ss_taihi";
				this._uiDamageIcon.alpha = 1f;
			}
		}
	}
}
