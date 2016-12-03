using Common.Enum;
using local.models;
using LT.Tweening;
using System;
using UnityEngine;

namespace KCV.BattleCut
{
	public class BtlCut_ResultShip : MonoBehaviour
	{
		private const string HP_DRAW_FORMAT = "{0} / {1}";

		[SerializeField]
		private UITexture _uiBannerTexBefore;

		[SerializeField]
		private UITexture _uiBannerTexAfter;

		[SerializeField]
		private UILabel Level;

		[SerializeField]
		private UISlider _uiHPSlider;

		[SerializeField]
		private UISlider _uiEXPSlider;

		[SerializeField]
		private UILabel HPLabel;

		[SerializeField]
		private UISprite DamageSmoke;

		[SerializeField]
		private UISprite DamageIcon;

		private int levelUpCount;

		private ShipModel_BattleResult _clsShipModel;

		private int beforeHP;

		public Action act;

		public static BtlCut_ResultShip Instantiate(BtlCut_ResultShip prefab, Transform parent, Vector3 pos, ShipModel_BattleResult model)
		{
			BtlCut_ResultShip btlCut_ResultShip = Object.Instantiate<BtlCut_ResultShip>(prefab);
			btlCut_ResultShip.get_transform().set_parent(parent);
			btlCut_ResultShip.get_transform().set_localPosition(pos);
			btlCut_ResultShip.get_transform().localScaleOne();
			return btlCut_ResultShip.VitualCtor(model);
		}

		private BtlCut_ResultShip VitualCtor(ShipModel_BattleResult model)
		{
			this._clsShipModel = model;
			int texNum = (!model.DamagedFlgStart) ? 1 : 2;
			this._uiBannerTexBefore.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(this._clsShipModel.MstId, texNum);
			if (model.DmgStateStart == DamageState_Battle.Gekichin || model.IsEscape())
			{
				this._uiBannerTexBefore.shader = SingletonMonoBehaviour<ResourceManager>.Instance.shader.shaderList.get_Item(0);
			}
			if (model.DmgStateStart != DamageState_Battle.Normal)
			{
				this.DamageSmoke.spriteName = string.Format("icon-ss_burned_{0}", model.DmgStateStart.ToString());
				this.DamageSmoke.alpha = 1f;
			}
			this.DamageIcon.spriteName = ((!model.IsEscape()) ? ("icon-ss_" + model.DmgStateStart.ToString()) : "icon-ss_taihi");
			this.DamageIcon.alpha = ((!model.IsEscape()) ? ((model.DmgStateStart == DamageState_Battle.Normal) ? 0f : 1f) : 1f);
			this.Level.textInt = model.Level;
			this.HPLabel.text = string.Format("{0} / {1}", model.HpStart, model.MaxHp);
			this._uiHPSlider.value = Mathe.Rate(0f, (float)model.MaxHp, (float)model.HpStart);
			this._uiHPSlider.foregroundWidget.color = Util.HpGaugeColor2(model.MaxHp, model.HpStart);
			this.beforeHP = model.HpStart;
			this.levelUpCount = 0;
			this._uiEXPSlider.value = Mathe.Rate(0f, 100f, (float)model.ExpInfo.ExpRateBefore);
			return this;
		}

		private void OnDestroy()
		{
			Mem.Del<UITexture>(ref this._uiBannerTexBefore);
			Mem.Del<UITexture>(ref this._uiBannerTexAfter);
			Mem.Del<UILabel>(ref this.Level);
			Mem.Del<UISlider>(ref this._uiHPSlider);
			Mem.Del<UISlider>(ref this._uiEXPSlider);
			Mem.Del<UILabel>(ref this.HPLabel);
			Mem.Del(ref this.DamageSmoke);
			Mem.Del(ref this.DamageIcon);
			Mem.Del<int>(ref this.levelUpCount);
			Mem.Del<ShipModel_BattleResult>(ref this._clsShipModel);
			Mem.Del<int>(ref this.beforeHP);
			Mem.Del<Action>(ref this.act);
		}

		public void UpdateHP()
		{
			this.HPLabel.get_transform().LTValue((float)this._clsShipModel.HpStart, (float)this._clsShipModel.HpEnd, 1f).setEase(LeanTweenType.easeOutQuad).setOnUpdate(delegate(float x)
			{
				int num = Convert.ToInt32(x);
				this._uiHPSlider.value = Mathe.Rate(0f, (float)this._clsShipModel.MaxHp, x);
				this._uiHPSlider.foregroundWidget.color = Util.HpGaugeColor2(this._clsShipModel.MaxHp, num);
				this.HPLabel.text = string.Format("{0} / {1}", num, this._clsShipModel.MaxHp);
			}).setOnComplete(delegate
			{
				this.ShipTexUpdate();
			});
		}

		private void ShipTexUpdate()
		{
			if (this.act != null)
			{
				this.act.Invoke();
			}
			int texNum = (!this._clsShipModel.DamagedFlgEnd) ? 1 : 2;
			this._uiBannerTexAfter.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(this._clsShipModel.MstId, texNum);
			if (this._clsShipModel.DmgStateEnd == DamageState_Battle.Gekichin || this._clsShipModel.IsEscape())
			{
				this._uiBannerTexAfter.shader = SingletonMonoBehaviour<ResourceManager>.Instance.shader.shaderList.get_Item(0);
			}
			this._uiBannerTexAfter.get_transform().ValueTo(0f, 1f, 0.8f, iTween.EaseType.linear, delegate(object x)
			{
				this._uiBannerTexAfter.alpha = Convert.ToSingle(x);
			}, null);
			this._uiBannerTexBefore.get_transform().ValueTo(1f, 0f, 0.8f, iTween.EaseType.linear, delegate(object x)
			{
				this._uiBannerTexBefore.alpha = Convert.ToSingle(x);
			}, null);
			this.DamageSmoke.spriteName = "icon-ss_burned_" + this._clsShipModel.DmgStateEnd.ToString();
			this.DamageIcon.spriteName = ((!this._clsShipModel.IsEscape()) ? ("icon-ss_" + this._clsShipModel.DmgStateEnd.ToString()) : "icon-ss_taihi");
			TweenAlpha.Begin(this.DamageSmoke.get_gameObject(), 0.8f, 1f);
			TweenAlpha.Begin(this.DamageIcon.get_gameObject(), 0.8f, 1f);
		}

		public void UpdateEXPGauge()
		{
			float num = (float)this._clsShipModel.ExpInfo.ExpRateAfter.get_Item(this.levelUpCount) / 100f;
			bool isOver = false;
			if (this._clsShipModel.ExpInfo.ExpRateAfter.get_Count() > this.levelUpCount + 1)
			{
				num = 1f;
				isOver = true;
				this.levelUpCount++;
			}
			this._uiEXPSlider.get_transform().ValueTo(this._uiEXPSlider.value, num, 0.5f, iTween.EaseType.easeOutQuad, delegate(object x)
			{
				this._uiEXPSlider.value = Convert.ToSingle(x);
			}, delegate
			{
				if (isOver)
				{
					this.Level.textInt = this._clsShipModel.Level + this.levelUpCount;
					this._uiEXPSlider.value = 0f;
					this.UpdateEXPGauge();
				}
			});
		}

		public void ShowMVPIcon()
		{
			Util.InstantiatePrefab("SortieMap/BattleCut/MVPIcon", base.get_gameObject(), true).get_transform().localPosition(-100f, 4f, 0f);
		}
	}
}
