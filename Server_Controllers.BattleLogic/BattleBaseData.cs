using Common.Enum;
using Server_Common;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server_Controllers.BattleLogic
{
	public class BattleBaseData : IDisposable
	{
		public int Enemy_id;

		public BattleFormationKinds1 Formation;

		public BattleFormationKinds2 BattleFormation;

		public string Enemy_Name;

		public Mem_deck Deck;

		public List<Mem_ship> ShipData;

		public List<List<Mst_slotitem>> SlotData;

		public List<List<int>> SlotLevel;

		public List<bool> LostFlag;

		public Dictionary<int, int[]> SlotExperience;

		public readonly List<int> StartHp;

		private readonly bool haveBattleCommand;

		public BattleBaseData(Mem_deck deck, List<Mem_ship> ships, List<int> stypes, List<List<Mst_slotitem>> slotitems)
		{
			this.Deck = deck;
			this.ShipData = ships;
			this.SlotData = slotitems;
			this.LostFlag = new List<bool>();
			this.StartHp = new List<int>();
			this.SlotLevel = new List<List<int>>();
			this.SlotExperience = new Dictionary<int, int[]>();
			ships.ForEach(delegate(Mem_ship x)
			{
				bool flag = x.Get_DamageState() == DamageState.Taiha;
				this.LostFlag.Add(flag);
				this.StartHp.Add(x.Nowhp);
				List<int> list = null;
				this.setSlotLevel(x.Slot, false, out list, ref this.SlotExperience);
				this.SlotLevel.Add(list);
			});
			this.LostFlag.set_Item(0, false);
			this.haveBattleCommand = true;
		}

		public BattleBaseData(int enemy_id)
		{
			Mst_mapenemy2 mst_mapenemy = Mst_DataManager.Instance.Mst_mapenemy.get_Item(enemy_id);
			this.Enemy_id = enemy_id;
			this.Formation = (BattleFormationKinds1)mst_mapenemy.Formation_id;
			mst_mapenemy.GetEnemyShips(out this.ShipData, out this.SlotData);
			this.StartHp = new List<int>();
			this.Enemy_Name = mst_mapenemy.Deck_name;
			this.SlotLevel = new List<List<int>>();
			this.SlotExperience = new Dictionary<int, int[]>();
			this.ShipData.ForEach(delegate(Mem_ship x)
			{
				this.StartHp.Add(x.Nowhp);
				List<int> list = null;
				this.setSlotLevel(x.Slot, true, out list, ref this.SlotExperience);
				this.SlotLevel.Add(list);
			});
			this.haveBattleCommand = false;
		}

		public BattleBaseData(Mem_deck deck)
		{
			List<Mem_ship> memShip = deck.Ship.getMemShip();
			this.ShipData = new List<Mem_ship>();
			this.SlotData = new List<List<Mst_slotitem>>();
			this.StartHp = new List<int>();
			this.SlotLevel = new List<List<int>>();
			this.SlotExperience = new Dictionary<int, int[]>();
			memShip.ForEach(delegate(Mem_ship x)
			{
				this.SlotData.Add(x.GetMstSlotItems());
				List<int> list = null;
				this.setSlotLevel(x.Slot, false, out list, ref this.SlotExperience);
				this.SlotLevel.Add(list);
				Mst_ship mst_data = Mst_DataManager.Instance.Mst_ship.get_Item(x.Ship_id);
				Mem_shipBase baseData = new Mem_shipBase(x);
				Mem_ship mem_ship = new Mem_ship();
				mem_ship.Set_ShipParamPracticeShip(baseData, mst_data);
				this.ShipData.Add(mem_ship);
				this.StartHp.Add(mem_ship.Nowhp);
			});
			this.Enemy_Name = deck.Name;
			this.haveBattleCommand = false;
		}

		public List<BattleCommand> GetDeckBattleCommand()
		{
			if (!this.haveBattleCommand)
			{
				return null;
			}
			List<BattleCommand> battleCommad = this.ShipData.get_Item(0).GetBattleCommad();
			int battleCommandEnableNum = this.ShipData.get_Item(0).GetBattleCommandEnableNum();
			return Enumerable.ToList<BattleCommand>(Enumerable.Take<BattleCommand>(battleCommad, battleCommandEnableNum));
		}

		public void Dispose()
		{
			this.ShipData.Clear();
			this.SlotData.Clear();
			this.SlotLevel.Clear();
			this.LostFlag.Clear();
			this.StartHp.Clear();
			this.SlotExperience.Clear();
		}

		private void setSlotLevel(List<int> slot_rids, bool areaEnemy, out List<int> outlevel, ref Dictionary<int, int[]> outExp)
		{
			if (areaEnemy)
			{
				outlevel = Enumerable.ToList<int>(Enumerable.Repeat<int>(0, slot_rids.get_Count()));
				return;
			}
			List<int> list = new List<int>();
			using (List<int>.Enumerator enumerator = slot_rids.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int current = enumerator.get_Current();
					Mem_slotitem mem_slotitem = null;
					if (Comm_UserDatas.Instance.User_slot.TryGetValue(current, ref mem_slotitem))
					{
						list.Add(mem_slotitem.Level);
						int[] expr_5F = new int[2];
						expr_5F[0] = mem_slotitem.Experience;
						int[] array = expr_5F;
						outExp.Add(mem_slotitem.Rid, array);
					}
				}
			}
			outlevel = list;
		}
	}
}
