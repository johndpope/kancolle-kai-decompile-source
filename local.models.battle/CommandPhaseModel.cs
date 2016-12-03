using Common.Enum;
using local.managers;
using System;
using System.Collections.Generic;

namespace local.models.battle
{
	public abstract class CommandPhaseModel : BattlePhaseModel
	{
		protected BattleManager _manager;

		protected bool _take_command;

		public abstract int Num
		{
			get;
		}

		public CommandPhaseModel(BattleManager manager)
		{
			this._manager = manager;
			this._data_f = new List<DamageModelBase>();
			this._data_e = new List<DamageModelBase>();
		}

		public bool IsTakeCommand()
		{
			return this._take_command;
		}

		public override List<ShipModel_Defender> GetDefenders(bool is_friend)
		{
			return new List<ShipModel_Defender>();
		}

		public abstract List<BattleCommand> GetPresetCommand();

		public abstract HashSet<BattleCommand> GetSelectableCommands();

		public abstract bool IsValidCommand(List<BattleCommand> command);

		public abstract bool SetCommand(List<BattleCommand> command);

		public override string ToString()
		{
			return string.Format("[戦闘指揮] {0} {1}", (!this.IsTakeCommand()) ? "未指揮" : "指揮済", this.ToString(this.GetPresetCommand()));
		}

		public string ToString(List<BattleCommand> cmds)
		{
			string text = "(";
			for (int i = 0; i < cmds.get_Count(); i++)
			{
				text += cmds.get_Item(i);
				if (i < cmds.get_Count() - 1)
				{
					text += ", ";
				}
			}
			return text + ")";
		}
	}
}
