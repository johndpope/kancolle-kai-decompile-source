using KCV.Battle.Utils;
using KCV.SortieBattle;
using local.models;
using LT.Tweening;
using System;
using UnityEngine;

namespace KCV.Battle
{
	[RequireComponent(typeof(UIPanel))]
	public class UIVeteransReportMVPShip : BaseUISortieBattleShip<ShipModel_BattleResult>
	{
		[SerializeField]
		private float _fShowXOffs = 100f;

		private Vector3 _vTweenTargetPos;

		public float textureAlpha
		{
			get
			{
				return this._uiShipTex.alpha;
			}
			set
			{
				this._uiShipTex.alpha = value;
			}
		}

		public static UIVeteransReportMVPShip Instantiate(UIVeteransReportMVPShip prefab, Transform parent, Vector3 pos, ShipModel_BattleResult model)
		{
			UIVeteransReportMVPShip uIVeteransReportMVPShip = Object.Instantiate<UIVeteransReportMVPShip>(prefab);
			uIVeteransReportMVPShip.get_transform().set_parent(parent);
			uIVeteransReportMVPShip.get_transform().localScaleOne();
			uIVeteransReportMVPShip.get_transform().set_localPosition(pos);
			uIVeteransReportMVPShip.SetShipTexture(model);
			return uIVeteransReportMVPShip;
		}

		protected override void OnUnInit()
		{
			Mem.Del<float>(ref this._fShowXOffs);
			base.OnUnInit();
		}

		protected override void SetShipTexture(ShipModel_BattleResult model)
		{
			if (model == null)
			{
				return;
			}
			base.SetShipTexture(model);
			this._uiShipTex.get_transform().set_localPosition(Util.Poi2Vec(model.Offsets.GetCutinSp1_InBattle(model.IsDamaged())));
			this.SetLovOffset(model);
		}

		protected override void SetLovOffset(ShipModel_BattleResult model)
		{
			LovLevel lovLevel = SortieBattleUtils.GetLovLevel(model);
			Vector3 localScale = Vector3.get_one() * SortieBattleUtils.GetLovScaleMagnification(lovLevel);
			float num = Mathe.Rate(0f, 1f, 1f / (float)(Enum.GetValues(typeof(LovLevel)).get_Length() - 1) * (float)(lovLevel - LovLevel.Normal));
			this._vTweenTargetPos = Vector3.Lerp(this.originPos, this.lovMaxPos, num);
			base.get_transform().set_localScale(localScale);
			base.get_transform().localPositionY(this._vTweenTargetPos.y);
		}

		public void Show(bool isPlayVoice, Action callback)
		{
			if (this.shipModel == null)
			{
				return;
			}
			base.get_transform().LTMoveLocal(this._vTweenTargetPos, 0.5f).setEase(LeanTweenType.easeOutSine).setOnComplete(delegate
			{
				if (isPlayVoice)
				{
					ShipUtils.PlayMVPVoice(this.shipModel);
				}
				Dlg.Call(ref callback);
			});
			base.get_transform().LTValue(0f, 1f, 0.5f).setEase(LeanTweenType.easeOutSine).setOnUpdate(delegate(float x)
			{
				this.panel.alpha = x;
			});
		}
	}
}
