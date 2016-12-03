using Common.Enum;
using Server_Common;
using Server_Common.Formats.Battle;
using Server_Controllers.BattleLogic;
using Server_Controllers.QuestLogic;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server_Controllers
{
	public class Api_req_PracticeBattle : BattleControllerBase
	{
		private int deckRid;

		private int deckRidEnemy;

		public Api_req_PracticeBattle(int deck_rid, int deck_ridEnemy)
		{
			this.deckRid = deck_rid;
			this.deckRidEnemy = deck_ridEnemy;
			this.init();
		}

		protected override void init()
		{
			this.practiceFlag = true;
			Mem_deck mem_deck = Comm_UserDatas.Instance.User_deck.get_Item(this.deckRid);
			mem_deck.ActionEnd();
			this.userData = this.getUserData(mem_deck);
			this.enemyData = new BattleBaseData(Comm_UserDatas.Instance.User_deck.get_Item(this.deckRidEnemy));
			base.setBattleSubInfo(this.userData, out this.userSubInfo);
			base.setBattleSubInfo(this.enemyData, out this.enemySubInfo);
			this.battleKinds = ExecBattleKinds.None;
			this.battleCommandParams = new BattleCommandParams(this.userData);
		}

		public override void Dispose()
		{
			base.Dispose();
		}

		public override Api_Result<AllBattleFmt> GetDayPreBattleInfo(BattleFormationKinds1 formationKind)
		{
			Api_Result<AllBattleFmt> dayPreBattleInfo = base.GetDayPreBattleInfo(formationKind);
			if (dayPreBattleInfo.state != Api_Result_State.Parameter_Error)
			{
				dayPreBattleInfo.data.DayBattle.Header.E_DeckShip1.Deck_Id = this.deckRidEnemy;
			}
			return dayPreBattleInfo;
		}

		public override Api_Result<AllBattleFmt> DayBattle()
		{
			return base.DayBattle();
		}

		public override Api_Result<AllBattleFmt> NightBattle()
		{
			Api_Result<AllBattleFmt> api_Result = base.NightBattle();
			if (api_Result.state != Api_Result_State.Parameter_Error)
			{
				api_Result.data.NightBattle.Header.E_DeckShip1.Deck_Id = this.deckRidEnemy;
			}
			return api_Result;
		}

		public override Api_Result<BattleResultFmt> BattleResult()
		{
			Api_Result<BattleResultFmt> api_Result = base.BattleResult();
			int count = this.userData.StartHp.get_Count();
			for (int i = 0; i < count; i++)
			{
				this.userData.ShipData.get_Item(i).SetHp(this, this.userData.StartHp.get_Item(i));
			}
			if (api_Result.state == Api_Result_State.Parameter_Error)
			{
				return api_Result;
			}
			Dictionary<int, int> slotExpBattleData = base.GetSlotExpBattleData();
			using (Dictionary<int, int>.Enumerator enumerator = slotExpBattleData.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<int, int> current = enumerator.get_Current();
					Mem_slotitem mem_slotitem = null;
					if (Comm_UserDatas.Instance.User_slot.TryGetValue(current.get_Key(), ref mem_slotitem))
					{
						mem_slotitem.ChangeExperience(current.get_Value());
					}
				}
			}
			QuestPractice questPractice = new QuestPractice(api_Result.data);
			questPractice.ExecuteCheck();
			return api_Result;
		}

		public override void GetBattleResultBase(BattleResultBase out_data)
		{
			base.GetBattleResultBase(out_data);
		}

		private BattleBaseData getUserData(Mem_deck deck)
		{
			List<Mem_ship> memShip = deck.Ship.getMemShip();
			List<List<Mst_slotitem>> list = new List<List<Mst_slotitem>>();
			List<int> list2 = new List<int>();
			using (List<Mem_ship>.Enumerator enumerator = memShip.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mem_ship current = enumerator.get_Current();
					list2.Add(current.Stype);
					IEnumerable<Mst_slotitem> enumerable = Enumerable.Select<int, Mst_slotitem>(Enumerable.Where<int>(current.Slot, (int mem_id) => Comm_UserDatas.Instance.User_slot.ContainsKey(mem_id)), (int mem_id) => Mst_DataManager.Instance.Mst_Slotitem.get_Item(Comm_UserDatas.Instance.User_slot.get_Item(mem_id).Slotitem_id));
					list.Add(Enumerable.ToList<Mst_slotitem>(enumerable));
				}
			}
			return new BattleBaseData(deck, memShip, list2, list);
		}
	}
}
