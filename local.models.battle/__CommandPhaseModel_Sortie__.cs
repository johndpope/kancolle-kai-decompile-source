using Common.Enum;
using local.managers;
using Server_Controllers;
using System;
using System.Collections.Generic;

namespace local.models.battle
{
	public class __CommandPhaseModel_Sortie__ : CommandPhaseModel
	{
		private Api_req_SortieBattle _req;

		public override int Num
		{
			get
			{
				List<BattleCommand> list;
				return this._req.GetBattleCommand(out list);
			}
		}

		public __CommandPhaseModel_Sortie__(BattleManager manager, Api_req_SortieBattle req) : base(manager)
		{
			this._req = req;
		}

		public override List<BattleCommand> GetPresetCommand()
		{
			List<BattleCommand> list;
			int battleCommand = this._req.GetBattleCommand(out list);
			return list.GetRange(0, battleCommand);
		}

		public override HashSet<BattleCommand> GetSelectableCommands()
		{
			return this._req.GetEnableBattleCommand();
		}

		public override bool IsValidCommand(List<BattleCommand> command)
		{
			return !this._take_command && (command == null || this._req.ValidBattleCommand(command));
		}

		public override bool SetCommand(List<BattleCommand> command)
		{
			if (this._take_command)
			{
				return false;
			}
			if (command != null)
			{
				if (!this._req.ValidBattleCommand(command))
				{
					return false;
				}
				this._req.SetBattleCommand(command);
			}
			this._take_command = true;
			if (this._manager.WarType == enumMapWarType.Normal)
			{
				this._req.DayBattle();
			}
			else if (this._manager.WarType == enumMapWarType.AirBattle)
			{
				this._req.AirBattle();
			}
			this._manager.__createCacheDataAfterCommand__();
			return true;
		}
	}
}
