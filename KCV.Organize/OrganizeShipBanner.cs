using Common.Enum;
using local.models;
using System;
using UnityEngine;

namespace KCV.Organize
{
	public class OrganizeShipBanner : BaseShipBanner
	{
		public UISprite UiConditionIcon;

		public UISprite UiConditionMask;

		public ParticleSystem KiraPar;

		public int sizeX;

		private void OnValidate()
		{
		}

		public virtual void SetShipData(ShipModel model)
		{
			this.UiConditionIcon = base.get_transform().FindChild("ConditionIcon").GetComponent<UISprite>();
			this.UiConditionMask = base.get_transform().FindChild("ConditionMask").GetComponent<UISprite>();
			UIPanel component = base.get_transform().get_parent().get_parent().get_transform().FindChild("Panel").GetComponent<UIPanel>();
			this.KiraPar = component.get_transform().FindChild("Light").GetComponent<ParticleSystem>();
			this.KiraPar.Stop();
			this.KiraPar.SetActive(false);
			if (model == null)
			{
				return;
			}
			this._clsShipModel = model;
			int texNum = (!model.IsDamaged()) ? 1 : 2;
			this._uiShipTex.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(model.MstId, texNum);
			this.UpdateDamage(model.DamageStatus);
			this.UpdateCondition(model.ConditionState);
		}

		private void UpdateCondition(FatigueState state)
		{
			this.KiraPar.Stop();
			this.KiraPar.SetActive(false);
			switch (state)
			{
			case FatigueState.Exaltation:
				this.UiConditionMask.alpha = 0f;
				this.UiConditionIcon.alpha = 0f;
				this.KiraPar.SetActive(true);
				this.KiraPar.Play();
				break;
			case FatigueState.Normal:
				this.UiConditionMask.alpha = 0f;
				this.UiConditionIcon.alpha = 0f;
				break;
			case FatigueState.Light:
				this.UiConditionMask.alpha = 1f;
				this.UiConditionIcon.alpha = 1f;
				this.UiConditionMask.spriteName = "card-ss_fatigue_1";
				this.UiConditionIcon.spriteName = "icon_fatigue_1";
				break;
			case FatigueState.Distress:
				this.UiConditionMask.alpha = 1f;
				this.UiConditionIcon.alpha = 1f;
				this.UiConditionMask.spriteName = "card-ss_fatigue_2";
				this.UiConditionIcon.spriteName = "icon_fatigue_2";
				break;
			}
		}
	}
}
