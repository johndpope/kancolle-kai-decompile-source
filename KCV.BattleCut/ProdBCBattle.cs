using local.managers;
using local.models;
using LT.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KCV.BattleCut
{
	[RequireComponent(typeof(UIPanel))]
	public class ProdBCBattle : MonoBehaviour
	{
		[Serializable]
		public class FleetInfos
		{
			[SerializeField]
			private Transform _tra;

			[SerializeField]
			private Transform _traShipHPBarAnchor;

			[SerializeField]
			private BtlCut_UICircleHPGauge _uiCircleGauge;

			private List<BtlCut_HPBar> _listHPBar;

			public Transform transform
			{
				get
				{
					return this._tra;
				}
			}

			public Transform shipHPBarAnchor
			{
				get
				{
					return this._traShipHPBarAnchor;
				}
			}

			public BtlCut_UICircleHPGauge circleGauge
			{
				get
				{
					return this._uiCircleGauge;
				}
			}

			public List<BtlCut_HPBar> hpBars
			{
				get
				{
					if (this._listHPBar == null)
					{
						this._listHPBar = new List<BtlCut_HPBar>(6);
					}
					return this._listHPBar;
				}
				set
				{
					this._listHPBar = value;
				}
			}

			public bool UnInit()
			{
				Mem.Del<Transform>(ref this._tra);
				Mem.Del<Transform>(ref this._traShipHPBarAnchor);
				Mem.Del<BtlCut_UICircleHPGauge>(ref this._uiCircleGauge);
				Mem.DelListSafe<BtlCut_HPBar>(ref this._listHPBar);
				return true;
			}

			public void ShakeGauge()
			{
				this._uiCircleGauge.get_transform().ShakePosition(new Vector3(0.1f, 0.1f, 0f), 1f);
			}

			public void ShakeGauge(Vector3 amount, float time)
			{
				this._uiCircleGauge.get_transform().ShakePosition(amount, time);
			}
		}

		private const float HPGAUGE_DEFAULT_POS_X = -346.5f;

		[SerializeField]
		private List<ProdBCBattle.FleetInfos> _listFleetInfos;

		private UIPanel _uiPanel;

		private bool _isNight;

		private Action _actCallback;

		public UIPanel panel
		{
			get
			{
				return this.GetComponentThis(ref this._uiPanel);
			}
		}

		public static ProdBCBattle Instantiate(ProdBCBattle prefab, Transform parent)
		{
			ProdBCBattle prodBCBattle = Object.Instantiate<ProdBCBattle>(prefab);
			prodBCBattle.get_transform().set_parent(parent);
			prodBCBattle.get_transform().localScaleOne();
			prodBCBattle.get_transform().localPositionZero();
			return prodBCBattle;
		}

		private void Awake()
		{
			this.panel.alpha = 0f;
			for (int i = 0; i < 6; i++)
			{
				this._listFleetInfos.get_Item(0).hpBars.Add(this._listFleetInfos.get_Item(0).shipHPBarAnchor.Find("HPBar" + (i + 1)).GetComponent<BtlCut_HPBar>());
				this._listFleetInfos.get_Item(1).hpBars.Add(this._listFleetInfos.get_Item(1).shipHPBarAnchor.Find("EnemyHPBar" + (i + 1)).GetComponent<BtlCut_HPBar>());
			}
		}

		private void OnDestroy()
		{
			Mem.DelListSafe<ProdBCBattle.FleetInfos>(ref this._listFleetInfos);
			Mem.Del<UIPanel>(ref this._uiPanel);
			Mem.Del<bool>(ref this._isNight);
			Mem.Del<Action>(ref this._actCallback);
		}

		public ProdBCBattle Play(bool isNight, Action callback)
		{
			this._isNight = isNight;
			this._actCallback = callback;
			this.InitBattleData(this._isNight);
			this.Show(Defines.PHASE_FADE_TIME).setOnComplete(delegate
			{
				this.Hide(1.7f, LeanTweenType.easeOutCirc).setOnComplete(delegate
				{
					this.GotoBattleEnd();
				});
				this.UpdateHpGauge();
			});
			return this;
		}

		private bool InitBattleData(bool isNightCombat)
		{
			BattleManager battleManager = BattleCutManager.GetBattleManager();
			BattleData battleData = BattleCutManager.GetBattleData();
			ProdBCBattle.FleetInfos fleetInfos = this._listFleetInfos.get_Item(0);
			ProdBCBattle.FleetInfos fleetInfos2 = this._listFleetInfos.get_Item(1);
			battleData.friendFleetHP.attackCnt = 4;
			battleData.enemyFleetHP.attackCnt = 4;
			battleData.friendFleetHP.ClearOneAttackDamage();
			battleData.enemyFleetHP.ClearOneAttackDamage();
			this._listFleetInfos.ForEach(delegate(ProdBCBattle.FleetInfos x)
			{
				x.circleGauge.SetActive(true);
			});
			this._listFleetInfos.ForEach(delegate(ProdBCBattle.FleetInfos x)
			{
				x.shipHPBarAnchor.SetActive(true);
			});
			this._listFleetInfos.get_Item(0).shipHPBarAnchor.localPositionX(-346.5f);
			this.CalcSumHp();
			for (int i = 0; i < 6; i++)
			{
				ShipModel_BattleAll shipModel_BattleAll = battleManager.Ships_f[i];
				ShipModel_BattleAll shipModel_BattleAll2 = battleManager.Ships_e[i];
				if (shipModel_BattleAll != null)
				{
					fleetInfos.hpBars.get_Item(i).SetHpBar(shipModel_BattleAll);
					fleetInfos.hpBars.get_Item(i).hpData.SetEndHP(shipModel_BattleAll.HpEnd);
					for (int j = 0; j < fleetInfos.hpBars.get_Item(i).hpData.oneAttackDamage.Length; j++)
					{
						battleData.friendFleetHP.oneAttackDamage[j] += fleetInfos.hpBars.get_Item(i).hpData.oneAttackDamage[j];
					}
				}
				else
				{
					fleetInfos.hpBars.get_Item(i).Hide();
				}
				if (shipModel_BattleAll2 != null)
				{
					fleetInfos2.hpBars.get_Item(i).SetHpBar(shipModel_BattleAll2);
					fleetInfos2.hpBars.get_Item(i).hpData.SetEndHP(shipModel_BattleAll2.HpEnd);
					for (int k = 0; k < fleetInfos2.hpBars.get_Item(i).hpData.oneAttackDamage.Length; k++)
					{
						battleData.enemyFleetHP.oneAttackDamage[k] += fleetInfos2.hpBars.get_Item(i).hpData.oneAttackDamage[k];
					}
				}
				else
				{
					fleetInfos2.hpBars.get_Item(i).Hide();
				}
			}
			return true;
		}

		private void CalcSumHp()
		{
			BattleData battleData = BattleCutManager.GetBattleData();
			BattleManager battleManager = BattleCutManager.GetBattleManager();
			IEnumerable<ShipModel_BattleAll> enumerable = Enumerable.Where<ShipModel_BattleAll>(battleManager.Ships_f, (ShipModel_BattleAll x) => x != null);
			battleData.friendFleetHP.maxHP = Enumerable.Sum(Enumerable.Select<ShipModel_BattleAll, int>(enumerable, (ShipModel_BattleAll x) => x.HpStart));
			battleData.friendFleetHP.nowHP = Enumerable.Sum(Enumerable.Select<ShipModel_BattleAll, int>(enumerable, (ShipModel_BattleAll x) => x.HpPhaseStart));
			battleData.friendFleetHP.endHP = Enumerable.Sum(Enumerable.Select<ShipModel_BattleAll, int>(enumerable, (ShipModel_BattleAll x) => x.HpEnd));
			battleData.friendFleetHP.nextHP = battleData.friendFleetHP.nowHP;
			IEnumerable<ShipModel_BattleAll> enumerable2 = Enumerable.Where<ShipModel_BattleAll>(battleManager.Ships_e, (ShipModel_BattleAll x) => x != null);
			battleData.enemyFleetHP.maxHP = Enumerable.Sum(Enumerable.Select<ShipModel_BattleAll, int>(enumerable2, (ShipModel_BattleAll x) => x.HpStart));
			battleData.enemyFleetHP.nowHP = Enumerable.Sum(Enumerable.Select<ShipModel_BattleAll, int>(enumerable2, (ShipModel_BattleAll x) => x.HpPhaseStart));
			battleData.enemyFleetHP.endHP = Enumerable.Sum(Enumerable.Select<ShipModel_BattleAll, int>(enumerable2, (ShipModel_BattleAll x) => x.HpEnd));
			battleData.enemyFleetHP.nextHP = battleData.enemyFleetHP.nowHP;
			this._listFleetInfos.get_Item(0).circleGauge.SetHPGauge(battleData.friendFleetHP.maxHP, battleData.friendFleetHP.nowHP, battleData.friendFleetHP.nowHP);
			this._listFleetInfos.get_Item(1).circleGauge.SetHPGauge(battleData.enemyFleetHP.maxHP, battleData.enemyFleetHP.nowHP, battleData.enemyFleetHP.nowHP);
		}

		private void UpdateHpGauge()
		{
			BattleData battleData = BattleCutManager.GetBattleData();
			if (battleData.friendFleetHP.attackCnt == 3)
			{
				this._listFleetInfos.ForEach(delegate(ProdBCBattle.FleetInfos x)
				{
					x.ShakeGauge(Vector3.get_one() * 0.2f, 0.7f);
				});
			}
			else if (battleData.friendFleetHP.attackCnt != 2)
			{
				this._listFleetInfos.ForEach(delegate(ProdBCBattle.FleetInfos x)
				{
					x.ShakeGauge(new Vector3(0.075f, 0.075f, 0f), 0.15f);
				});
			}
			this.UpdateSumHpGauge(this._listFleetInfos.get_Item(0).circleGauge, battleData.friendFleetHP, new Action(this.UpdateHpGauge));
			this.UpdateSumHpGauge(this._listFleetInfos.get_Item(1).circleGauge, battleData.enemyFleetHP, null);
			for (int i = 0; i < this._listFleetInfos.get_Item(0).hpBars.get_Capacity(); i++)
			{
				if (this._listFleetInfos.get_Item(0).hpBars.get_Item(i).hpData != null)
				{
					this._listFleetInfos.get_Item(0).hpBars.get_Item(i).Play();
				}
				if (this._listFleetInfos.get_Item(1).hpBars.get_Item(i).hpData != null)
				{
					this._listFleetInfos.get_Item(1).hpBars.get_Item(i).Play();
				}
			}
		}

		private void UpdateSumHpGauge(BtlCut_UICircleHPGauge gauge, HPData hpData, Action act)
		{
			hpData.attackCnt--;
			hpData.nextHP -= hpData.oneAttackDamage[3 - hpData.attackCnt];
			if (hpData.attackCnt != 0)
			{
				gauge.SetHPGauge(hpData.maxHP, hpData.nowHP, hpData.nextHP);
				gauge.Play(act);
				hpData.nowHP = hpData.nextHP;
			}
			else
			{
				gauge.SetHPGauge(hpData.maxHP, hpData.nowHP, hpData.endHP);
				gauge.Play(delegate
				{
				});
				hpData.nowHP = hpData.nextHP;
			}
		}

		public void setResultHPModeAdvancingWithdrawal(float y)
		{
			BattleManager battleManager = BattleCutManager.GetBattleManager();
			ProdBCBattle.FleetInfos fleetInfos = this._listFleetInfos.get_Item(0);
			ProdBCBattle.FleetInfos fleetInfos2 = this._listFleetInfos.get_Item(1);
			for (int i = 0; i < 6; i++)
			{
				ShipModel_BattleAll shipModel_BattleAll = battleManager.Ships_f[i];
				if (shipModel_BattleAll != null)
				{
					fleetInfos.hpBars.get_Item(i).SetHpBarAfter(shipModel_BattleAll, battleManager);
				}
			}
			this._listFleetInfos.get_Item(0).shipHPBarAnchor.localPositionX(-140.5f);
			this._listFleetInfos.get_Item(0).shipHPBarAnchor.localPositionY(y);
			this.Show(1f);
			this._listFleetInfos.ForEach(delegate(ProdBCBattle.FleetInfos x)
			{
				x.circleGauge.SetActive(false);
			});
			this._listFleetInfos.get_Item(1).shipHPBarAnchor.SetActive(false);
		}

		public void SetResultHPModeToWithdrawal(float y)
		{
			this._listFleetInfos.get_Item(0).shipHPBarAnchor.localPositionX(-140.5f);
			this._listFleetInfos.get_Item(0).shipHPBarAnchor.localPositionY(y);
			this._listFleetInfos.ForEach(delegate(ProdBCBattle.FleetInfos x)
			{
				x.circleGauge.SetActive(false);
			});
			this._listFleetInfos.get_Item(1).shipHPBarAnchor.SetActive(false);
			this.Show(0.35f);
		}

		private LTDescr Show(float time)
		{
			return this.panel.get_transform().LTValue(0f, 1f, time).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				this.panel.alpha = x;
			});
		}

		public LTDescr Hide(float time)
		{
			return this.panel.get_transform().LTValue(1f, 0f, time).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				this.panel.alpha = x;
			});
		}

		private LTDescr Hide(float time, LeanTweenType iType)
		{
			return this.panel.get_transform().LTValue(1f, 0f, time).setEase(iType).setOnUpdate(delegate(float x)
			{
				this.panel.alpha = x;
			});
		}

		private void GotoBattleEnd()
		{
			if (BattleCutManager.GetBattleManager().HasNightBattle() && !this._isNight)
			{
				BattleCutManager.ReqPhase(BattleCutPhase.WithdrawalDecision);
				return;
			}
			if (this._actCallback != null)
			{
				this._actCallback.Invoke();
			}
		}

		public void GotoResult()
		{
			BattleCutManager.ReqPhase(BattleCutPhase.WithdrawalDecision);
		}
	}
}
