using Common.Enum;
using KCV.Battle.Production;
using KCV.Battle.Utils;
using KCV.SortieBattle;
using local.managers;
using System;
using UniRx;
using UnityEngine;

namespace KCV.Battle
{
	public class TaskBattleEscortShipEvacuation : BaseBattleTask
	{
		[SerializeField]
		private Transform _prefabEscortShipEvacuation;

		private ProdEscortShipEvacuation _prodEscortShipEvacuation;

		protected override bool Init()
		{
			BattleManager battleManager = BattleTaskManager.GetBattleManager();
			if (battleManager.GetEscapeCandidate() != null)
			{
				this._prodEscortShipEvacuation = ProdEscortShipEvacuation.Instantiate((!(this._prefabEscortShipEvacuation != null)) ? PrefabFile.Load<ProdEscortShipEvacuation>(PrefabFileInfos.BattleProdEscortShipEvacuation) : this._prefabEscortShipEvacuation.GetComponent<ProdEscortShipEvacuation>(), BattleTaskManager.GetBattleCameras().cutInCamera.get_transform(), BattleTaskManager.GetKeyControl(), BattleTaskManager.GetBattleManager().GetEscapeCandidate(), false);
				this._prodEscortShipEvacuation.Init();
				this._prodEscortShipEvacuation.Play(new DelDecideAdvancingWithdrawalButton(this.DecideAdvancinsWithDrawalBtn));
			}
			else
			{
				if (battleManager.Ships_f[0].DmgStateEnd == DamageState_Battle.Taiha && ShipUtils.HasRepair(battleManager.Ships_f[0]))
				{
					BattleTaskManager.ReqPhase(BattlePhase.AdvancingWithdrawalDC);
				}
				else
				{
					BattleTaskManager.ReqPhase(BattlePhase.AdvancingWithdrawal);
				}
				base.ImmediateTermination();
			}
			return true;
		}

		protected override bool UnInit()
		{
			this._prefabEscortShipEvacuation = null;
			if (this._prodEscortShipEvacuation != null)
			{
				this._prodEscortShipEvacuation.Discard();
			}
			this._prodEscortShipEvacuation = null;
			return true;
		}

		protected override bool Update()
		{
			this._prodEscortShipEvacuation.Run();
			return this.ChkChangePhase(BattlePhase.EscortShipEvacuation);
		}

		private void DecideAdvancinsWithDrawalBtn(UIHexButton btn)
		{
			BattleManager manager = BattleTaskManager.GetBattleManager();
			if (btn.index == 0)
			{
				Observable.Timer(TimeSpan.FromSeconds(0.30000001192092896)).Subscribe(delegate(long _)
				{
					BattleTaskManager.GetBattleManager().SendOffEscapes();
					RetentionData.SetData(BattleUtils.GetRetentionDataAdvancingWithdrawalDC(SortieBattleTaskManager.GetMapManager(), ShipRecoveryType.None));
					if (manager.Ships_f[0].DmgStateEnd == DamageState_Battle.Taiha && ShipUtils.HasRepair(manager.Ships_f[0]))
					{
						BattleTaskManager.ReqPhase(BattlePhase.AdvancingWithdrawalDC);
					}
					else
					{
						BattleTaskManager.ReqPhase(BattlePhase.AdvancingWithdrawal);
					}
				});
			}
			else
			{
				Observable.Timer(TimeSpan.FromSeconds(0.30000001192092896)).Subscribe(delegate(long _)
				{
					RetentionData.SetData(BattleUtils.GetRetentionDataAdvancingWithdrawalDC(SortieBattleTaskManager.GetMapManager(), ShipRecoveryType.None));
					if (manager.Ships_f[0].DmgStateEnd == DamageState_Battle.Taiha && ShipUtils.HasRepair(manager.Ships_f[0]))
					{
						BattleTaskManager.ReqPhase(BattlePhase.AdvancingWithdrawalDC);
					}
					else
					{
						BattleTaskManager.ReqPhase(BattlePhase.AdvancingWithdrawal);
					}
				});
			}
		}
	}
}
