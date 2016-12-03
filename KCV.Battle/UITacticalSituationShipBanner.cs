using KCV.Battle.Utils;
using KCV.SortieBattle;
using local.models;
using System;
using UnityEngine;

namespace KCV.Battle
{
	[RequireComponent(typeof(UIWidget))]
	public class UITacticalSituationShipBanner : BaseUISortieBattleShipBanner<ShipModel_BattleAll>
	{
		[SerializeField]
		private UILabel _uiShipHp;

		public static UITacticalSituationShipBanner Instantiate(UITacticalSituationShipBanner prefab, Transform parent, ShipModel_BattleAll model)
		{
			if (model == null)
			{
				return null;
			}
			UITacticalSituationShipBanner uITacticalSituationShipBanner = Object.Instantiate<UITacticalSituationShipBanner>(prefab);
			uITacticalSituationShipBanner.get_transform().set_parent(parent);
			uITacticalSituationShipBanner.get_transform().localScaleOne();
			uITacticalSituationShipBanner.get_transform().localPositionZero();
			uITacticalSituationShipBanner.VirtualCtor(model);
			return uITacticalSituationShipBanner;
		}

		protected override void VirtualCtor(ShipModel_BattleAll model)
		{
			base.VirtualCtor(model);
			this.SetShipInfos(model);
			this.SetUISettings(model.IsFriend());
		}

		protected override void OnUnInit()
		{
			Mem.Del<UILabel>(ref this._uiShipHp);
		}

		protected override void SetShipInfos(ShipModel_BattleAll model)
		{
			this.SetShipName(model.Name);
			this.SetShipBannerTexture(ShipUtils.LoadBannerTextureInTacticalSituation(model), model.DamagedFlgEnd, model.DmgStateEnd, model.IsEscape());
			base.SetShipIcons(model.DmgStateEnd, model.IsGroundFacility(), model.IsEscape());
			this.SetHPSliderValue(model.HpEnd, model.MaxHp);
		}

		protected override void SetHPSliderValue(int nNowHP, int nMaxHP)
		{
			base.SetHPSliderValue(nNowHP, nMaxHP);
			this._uiShipHp.text = string.Format("{0}/{1}", nNowHP, nMaxHP);
		}

		protected override void SetUISettings(bool isFriend)
		{
			this._uiShipHp.pivot = ((!isFriend) ? UIWidget.Pivot.Left : UIWidget.Pivot.Right);
			this._uiShipHp.get_transform().set_localPosition((!isFriend) ? new Vector3(-428f, -20f, 0f) : new Vector3(428f, -20f, 0f));
			this._uiHPSlider.fillDirection = UIProgressBar.FillDirection.LeftToRight;
			this._uiHPSlider.get_transform().set_localPosition((!isFriend) ? new Vector3(-175f, -35f, 0f) : new Vector3(175f, -35f, 0f));
			this._uiHPSlider.get_transform().set_localRotation((!isFriend) ? Quaternion.Euler(Vector3.get_up() * 180f) : Quaternion.Euler(Vector3.get_zero()));
			this._uiShipName.pivot = ((!isFriend) ? UIWidget.Pivot.Right : UIWidget.Pivot.Left);
			this._uiShipName.get_transform().set_localPosition((!isFriend) ? new Vector3(-175f, -15f, 0f) : new Vector3(175f, -15f, 0f));
			this._uiShipTexture.pivot = ((!isFriend) ? UIWidget.Pivot.TopRight : UIWidget.Pivot.TopLeft);
			this._uiShipTexture.get_transform().localPositionZero();
			this._uiDamageIcon.pivot = ((!isFriend) ? UIWidget.Pivot.TopRight : UIWidget.Pivot.TopLeft);
			this._uiDamageIcon.get_transform().localPositionZero();
			this._uiDamageMask.pivot = ((!isFriend) ? UIWidget.Pivot.TopRight : UIWidget.Pivot.TopLeft);
			this._uiDamageMask.get_transform().localPositionZero();
		}
	}
}
