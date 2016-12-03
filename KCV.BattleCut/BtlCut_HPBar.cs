using Common.Enum;
using local.managers;
using local.models;
using LT.Tweening;
using System;
using UnityEngine;

namespace KCV.BattleCut
{
	[RequireComponent(typeof(UIWidget))]
	public class BtlCut_HPBar : MonoBehaviour
	{
		[SerializeField]
		private UISlider _uiHPSlider;

		[SerializeField]
		private UILabel _uiHPLabel;

		[SerializeField]
		private UISprite _uiShipIcon;

		[SerializeField]
		private UISprite _uiEscapeIcon;

		[SerializeField]
		private UISprite _uiRepairIcon;

		private int _nShipType;

		private UIWidget _uiWidget;

		private bool _isBattleCut = true;

		public HPData hpData
		{
			get;
			private set;
		}

		public int shipType
		{
			get
			{
				return this._nShipType;
			}
			private set
			{
				this._nShipType = value;
				int num = (value != 9) ? this._nShipType : 8;
				this._uiShipIcon.spriteName = string.Format("icon_ship{0}", num);
				this._uiShipIcon.MakePixelPerfect();
			}
		}

		public UIWidget widget
		{
			get
			{
				return this.GetComponentThis(ref this._uiWidget);
			}
		}

		public static BtlCut_HPBar Instantiate(BtlCut_HPBar prefab, Transform parent, ShipModel_BattleAll model, bool isAfter, BattleManager manager)
		{
			BtlCut_HPBar btlCut_HPBar = Object.Instantiate<BtlCut_HPBar>(prefab);
			btlCut_HPBar.get_transform().set_parent(parent);
			btlCut_HPBar.get_transform().localScaleOne();
			btlCut_HPBar.get_transform().localPositionZero();
			btlCut_HPBar._isBattleCut = false;
			if (isAfter)
			{
				btlCut_HPBar.SetHpBarAfter(model, manager);
			}
			else
			{
				btlCut_HPBar.SetHpBar(model);
			}
			return btlCut_HPBar;
		}

		private void Awake()
		{
			this._uiHPSlider.value = 1f;
			this._isBattleCut = true;
			if (this._uiEscapeIcon != null)
			{
				this._uiEscapeIcon.SetActive(false);
			}
			if (this._uiRepairIcon != null)
			{
				this._uiRepairIcon.SetActive(false);
			}
		}

		private void OnDestroy()
		{
			Mem.Del<UISlider>(ref this._uiHPSlider);
			Mem.Del(ref this._uiShipIcon);
			Mem.Del<int>(ref this._nShipType);
			Mem.Del(ref this._uiEscapeIcon);
			Mem.Del(ref this._uiRepairIcon);
		}

		private void SetHpBar(HPData data, int shipType)
		{
			this.hpData = data;
			if (shipType > 0)
			{
				this.shipType = shipType;
			}
			this._uiHPSlider.value = Mathe.Rate(0f, (float)data.maxHP, (float)data.nowHP);
			this._uiHPSlider.foregroundWidget.color = Util.HpGaugeColor2(this.hpData.maxHP, data.nowHP);
			if (this._uiHPLabel != null)
			{
				this._uiHPLabel.text = string.Format("{0}/{1}", data.nowHP, data.maxHP);
			}
		}

		public void SetHpBar(ShipModel_BattleAll model)
		{
			this.SetHpBar(new HPData(model.MaxHp, model.HpPhaseStart), (!model.IsFriend()) ? -1 : model.ShipType);
			if (model.IsFriend())
			{
				this._uiEscapeIcon.SetActive(model.IsEscape());
			}
			if (this._uiRepairIcon != null)
			{
				this._uiRepairIcon.SetActive(false);
			}
		}

		public void SetHpBarAfter(ShipModel_BattleAll model, BattleManager manager)
		{
			this.SetHpBar(new HPData(model.MaxHp, model.HpEnd), (!model.IsFriend()) ? -1 : model.ShipType);
			if (model.IsFriend())
			{
				this._uiEscapeIcon.SetActive(model.IsEscape());
			}
			if (manager.IsUseRecoverySlotitem(model.TmpId) != ShipRecoveryType.None && this._uiRepairIcon != null)
			{
				this._uiRepairIcon.spriteName = ((!this._isBattleCut) ? "fuki2_set" : "fuki_set");
				this._uiRepairIcon.SetActive(true);
			}
		}

		public void Play()
		{
			this.hpData.attackCnt--;
			this.hpData.nextHP = this.hpData.nowHP - this.hpData.oneAttackDamage[this.hpData.attackCnt];
			this.UpdateDamage();
		}

		private void UpdateDamage()
		{
			LeanTween.value(base.get_gameObject(), this._uiHPSlider.value, Mathe.Rate(0f, (float)this.hpData.maxHP, (float)this.hpData.nextHP), 0.5f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				this._uiHPSlider.value = x;
			});
			base.get_transform().LTValue((float)this.hpData.nowHP, (float)this.hpData.nextHP, 0.5f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				if (this._uiHPLabel != null)
				{
					this._uiHPLabel.text = string.Format("{0}/{1}", (int)x, this.hpData.maxHP);
				}
				this._uiHPSlider.foregroundWidget.color = Util.HpGaugeColor2(this.hpData.maxHP, (int)x);
			});
			this.hpData.nowHP = this.hpData.nextHP;
		}

		public void Hide()
		{
			this._uiHPSlider.SetActive(false);
			if (this._uiShipIcon != null)
			{
				this._uiShipIcon.SetActive(false);
			}
			if (this._uiHPLabel != null)
			{
				this._uiHPLabel.SetActive(false);
			}
		}

		public void SetHPLabelColor(Color color)
		{
			if (this._uiHPLabel != null)
			{
				this._uiHPLabel.color = color;
			}
		}
	}
}
