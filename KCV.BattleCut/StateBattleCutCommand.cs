using System;

namespace KCV.BattleCut
{
	public class StateBattleCutCommand : BaseBattleCutState
	{
		private CtrlBCCommandSelect _ctrlBCCommandSelect;

		public override bool Init(object data)
		{
			this._ctrlBCCommandSelect = CtrlBCCommandSelect.Instantiate(BattleCutManager.GetPrefabFile().prefabCtrlBCCommandSelect.GetComponent<CtrlBCCommandSelect>(), BattleCutManager.GetSharedPlase(), BattleCutManager.GetBattleManager().GetCommandPhaseModel());
			this._ctrlBCCommandSelect.Play(delegate
			{
				BattleCutManager.ReqPhase(BattleCutPhase.DayBattle);
			});
			return base.Init(data);
		}

		public override bool Terminate(object data)
		{
			Mem.DelComponentSafe<CtrlBCCommandSelect>(ref this._ctrlBCCommandSelect);
			return false;
		}

		public override bool Run(object data)
		{
			if (this._ctrlBCCommandSelect != null)
			{
				this._ctrlBCCommandSelect.Run();
			}
			return base.IsCheckPhase(BattleCutPhase.Command);
		}
	}
}
