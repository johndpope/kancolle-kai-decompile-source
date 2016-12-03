using KCV.Battle.Production;
using KCV.Battle.Utils;
using local.models.battle;
using System;

namespace KCV.Battle
{
	public class TaskBattleBossInsert : BaseBattleTask
	{
		private BossInsertModel _clsBossInsert;

		protected override bool Init()
		{
			this._clsBossInsert = BattleTaskManager.GetBattleManager().GetBossInsertData();
			if (this._clsBossInsert == null)
			{
				base.ImmediateTermination();
				this.EndPhase(BattleUtils.NextPhase(BattlePhase.BattlePhase_ST));
			}
			else
			{
				ProdBossInsert prodBossInsert = ProdBossInsert.Instantiate(BattleTaskManager.GetPrefabFile().prefabProdBossInsert.GetComponent<ProdBossInsert>(), BattleTaskManager.GetBattleCameras().cutInCamera.get_transform(), this._clsBossInsert.Ship);
				prodBossInsert.Play(delegate
				{
					this.EndPhase(BattleUtils.NextPhase(BattlePhase.BattlePhase_ST));
				});
			}
			return true;
		}

		protected override bool UnInit()
		{
			base.UnInit();
			Mem.Del<BossInsertModel>(ref this._clsBossInsert);
			return true;
		}

		protected override bool Update()
		{
			return this.ChkChangePhase(BattlePhase.BattlePhase_ST);
		}
	}
}
