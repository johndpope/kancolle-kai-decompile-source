using Common.Enum;
using Server_Common.Formats.Battle;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server_Controllers.BattleLogic
{
	public class Exec_OpeningAtack : Exec_Raigeki
	{
		private int raigType;

		public Exec_OpeningAtack(BattleBaseData myData, Dictionary<int, BattleShipSubInfo> mySubInfo, BattleBaseData enemyData, Dictionary<int, BattleShipSubInfo> enemySubInfo, bool practice) : base(3, myData, mySubInfo, enemyData, enemySubInfo, practice)
		{
		}

		public int getUserAttackShipNum()
		{
			return this.f_AtkIdxs.get_Count();
		}

		public int CanRaigType()
		{
			return this.raigType;
		}

		public override Raigeki GetResultData(FormationDatas formation, BattleCommandParams cParam)
		{
			return base.GetResultData(formation, cParam);
		}

		protected override void setHitHoseiFromBattleCommand()
		{
		}

		protected override void makeAttackerData(bool enemyFlag)
		{
			List<int> list;
			BattleBaseData battleBaseData;
			List<int> list2;
			bool flag;
			if (!enemyFlag)
			{
				list = this.f_AtkIdxs;
				battleBaseData = this.F_Data;
				list2 = this.f_startHp;
				flag = this.isRaigBattleCommand();
			}
			else
			{
				list = this.e_AtkIdxs;
				battleBaseData = this.E_Data;
				list2 = this.e_startHp;
				flag = true;
			}
			if (!flag)
			{
				return;
			}
			using (var enumerator = Enumerable.Select(battleBaseData.ShipData, (Mem_ship obj, int idx) => new
			{
				obj,
				idx
			}).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					var current = enumerator.get_Current();
					list2.Add(current.obj.Nowhp);
					if (this.isValidRaigeki(current.obj, battleBaseData.SlotData.get_Item(current.idx)))
					{
						list.Add(current.idx);
					}
				}
			}
		}

		protected override bool isRaigBattleCommand()
		{
			List<BattleCommand> deckBattleCommand = this.F_Data.GetDeckBattleCommand();
			IEnumerable<BattleCommand> enumerable = Enumerable.Take<BattleCommand>(deckBattleCommand, deckBattleCommand.get_Count() - 1);
			bool flag = Enumerable.Any<BattleCommand>(enumerable, (BattleCommand x) => x == BattleCommand.Raigeki);
			if (flag && deckBattleCommand.get_Item(0) == BattleCommand.Raigeki)
			{
				this.raigType = 1;
			}
			else if (flag)
			{
				this.raigType = 2;
			}
			return flag;
		}

		private bool isValidRaigeki(Mem_ship ship, List<Mst_slotitem> slotitems)
		{
			if (ship.Get_DamageState() > DamageState.Shouha || !ship.IsFight())
			{
				return false;
			}
			if (ship.GetBattleBaseParam().Raig <= 0)
			{
				return false;
			}
			if (!ship.IsEnemy() || this.practiceFlag)
			{
				if (ship.Level >= 10 && Mst_DataManager.Instance.Mst_stype.get_Item(ship.Stype).IsSubmarine())
				{
					return true;
				}
				if (slotitems == null || slotitems.get_Count() == 0)
				{
					return false;
				}
				Mst_slotitem mst_slotitem = Mst_DataManager.Instance.Mst_Slotitem.get_Item(41);
				return slotitems.Contains(mst_slotitem);
			}
			else
			{
				if (Mst_DataManager.Instance.Mst_stype.get_Item(ship.Stype).IsSubmarine())
				{
					Mst_ship mst_ship = Mst_DataManager.Instance.Mst_ship.get_Item(ship.Ship_id);
					return mst_ship.Yomi.Equals("flagship") || mst_ship.Yomi.Equals("elite");
				}
				if (slotitems == null || slotitems.get_Count() == 0)
				{
					return false;
				}
				Mst_slotitem mst_slotitem2 = Mst_DataManager.Instance.Mst_Slotitem.get_Item(541);
				return slotitems.Contains(mst_slotitem2);
			}
		}

		protected override double getAvoHosei(Mem_ship target)
		{
			return base.getAvoHosei(target);
		}
	}
}
