using Common.Enum;
using Server_Common;
using Server_Common.Formats.Battle;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server_Controllers.BattleLogic
{
	public class Exec_SupportAttack : BattleLogicBase<SupportAtack>
	{
		private BattleBaseData _f_Data;

		private BattleBaseData _e_Data;

		private Dictionary<int, BattleShipSubInfo> _f_SubInfo;

		private Dictionary<int, BattleShipSubInfo> _e_SubInfo;

		private Mem_deck supportDeck;

		private List<Mem_ship> supportShips;

		private MissionType supportType;

		private BattleSearchValues[] serchValues;

		private ILookup<int, int> mst_support_data;

		public override BattleBaseData F_Data
		{
			get
			{
				return this._f_Data;
			}
		}

		public override BattleBaseData E_Data
		{
			get
			{
				return this._e_Data;
			}
		}

		public override Dictionary<int, BattleShipSubInfo> F_SubInfo
		{
			get
			{
				return this._f_SubInfo;
			}
		}

		public override Dictionary<int, BattleShipSubInfo> E_SubInfo
		{
			get
			{
				return this._e_SubInfo;
			}
		}

		public Exec_SupportAttack(BattleBaseData enemyData, Dictionary<int, BattleShipSubInfo> enemySubInfo, SearchInfo[] serch, ILookup<int, int> mst_support_type)
		{
			this._e_Data = enemyData;
			this._e_SubInfo = enemySubInfo;
			this.practiceFlag = false;
			this.mst_support_data = mst_support_type;
			this.serchValues = new BattleSearchValues[]
			{
				serch[0].SearchValue,
				serch[1].SearchValue
			};
		}

		public void SelectSupportDeck(List<Mem_deck> targetDeck)
		{
			MissionType ms_type = MissionType.None;
			if (Mst_DataManager.Instance.Mst_mapenemy.get_Item(this._e_Data.Enemy_id).Boss != 0)
			{
				ms_type = MissionType.SupportBoss;
			}
			else
			{
				ms_type = MissionType.SupportForward;
			}
			this.supportType = ms_type;
			if (targetDeck == null || targetDeck.get_Count() == 0)
			{
				return;
			}
			this.supportDeck = Enumerable.FirstOrDefault<Mem_deck>(Enumerable.Select(Enumerable.Where(Enumerable.Select(targetDeck, (Mem_deck deck) => new
			{
				deck = deck,
				mst_misson = Mst_DataManager.Instance.Mst_mission.get_Item(deck.Mission_id)
			}), <>__TranspIdent13 => <>__TranspIdent13.mst_misson.Mission_type == ms_type), <>__TranspIdent13 => <>__TranspIdent13.deck));
		}

		public void SelectSupportDeck(List<Mem_deck> targetDeck, bool isForwardDeck)
		{
			MissionType searchKey2;
			if (Mst_DataManager.Instance.Mst_mapenemy.get_Item(this._e_Data.Enemy_id).Boss != 0)
			{
				searchKey2 = MissionType.SupportBoss;
			}
			else
			{
				searchKey2 = MissionType.SupportForward;
			}
			this.supportType = searchKey2;
			if (targetDeck == null || targetDeck.get_Count() == 0)
			{
				return;
			}
			IOrderedEnumerable<Mem_deck> orderedEnumerable = Enumerable.OrderByDescending<Mem_deck, int>(targetDeck, (Mem_deck x) => x.Mission_id);
			MissionType searchKey = searchKey2;
			this.supportDeck = Enumerable.FirstOrDefault<Mem_deck>(Enumerable.Select(Enumerable.Where(Enumerable.Select(orderedEnumerable, (Mem_deck deck) => new
			{
				deck = deck,
				mst_misson = Mst_DataManager.Instance.Mst_mission.get_Item(deck.Mission_id)
			}), <>__TranspIdent14 => <>__TranspIdent14.mst_misson.Mission_type == searchKey), <>__TranspIdent14 => <>__TranspIdent14.deck));
		}

		public override SupportAtack GetResultData(FormationDatas formation, BattleCommandParams cParam)
		{
			SupportAtack supportAtack = new SupportAtack();
			BattleSupportKinds battleSupportKinds = this.Init(ref supportAtack.Undressing_Flag);
			if (battleSupportKinds == BattleSupportKinds.None)
			{
				return null;
			}
			supportAtack.Deck_Id = this.supportDeck.Rid;
			supportAtack.SupportType = battleSupportKinds;
			if (battleSupportKinds == BattleSupportKinds.AirAtack)
			{
				using (Exec_SupportAirBattle exec_SupportAirBattle = new Exec_SupportAirBattle(this.F_Data, this.F_SubInfo, this.E_Data, this.E_SubInfo, this.practiceFlag))
				{
					supportAtack.AirBattle = exec_SupportAirBattle.GetResultData(formation, cParam);
				}
				if (supportAtack.AirBattle == null)
				{
					return null;
				}
			}
			else if (battleSupportKinds == BattleSupportKinds.Hougeki)
			{
				using (Exec_SupportHougeki exec_SupportHougeki = new Exec_SupportHougeki(this.F_Data, this.F_SubInfo, this.E_Data, this.E_SubInfo, this.practiceFlag))
				{
					supportAtack.Hourai = exec_SupportHougeki.GetResultData<Support_HouRai>(formation);
				}
				if (supportAtack.Hourai == null)
				{
					return null;
				}
			}
			else if (battleSupportKinds == BattleSupportKinds.Raigeki)
			{
				using (Exec_SupportRaigeki exec_SupportRaigeki = new Exec_SupportRaigeki(this.F_Data, this.F_SubInfo, this.E_Data, this.E_SubInfo, this.practiceFlag))
				{
					supportAtack.Hourai = exec_SupportRaigeki.GetResultData<Support_HouRai>(formation);
				}
				if (supportAtack.Hourai == null)
				{
					return null;
				}
			}
			this.supportDeck.ChangeSupported();
			return supportAtack;
		}

		public override void Dispose()
		{
			if (this.F_Data != null)
			{
				this.F_Data.Dispose();
			}
			this.randInstance = null;
		}

		private BattleSupportKinds Init(ref bool[] undressing)
		{
			if (this.mst_support_data == null)
			{
				return BattleSupportKinds.None;
			}
			if (Enumerable.FirstOrDefault<Mem_ship>(this.E_Data.ShipData, (Mem_ship x) => x.Nowhp > 0) == null)
			{
				return BattleSupportKinds.None;
			}
			if (this.supportDeck == null)
			{
				return BattleSupportKinds.None;
			}
			this.supportShips = this.supportDeck.Ship.getMemShip();
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
			using (IEnumerator<IGrouping<int, int>> enumerator = this.mst_support_data.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IGrouping<int, int> current = enumerator.get_Current();
					int key = current.get_Key();
					dictionary.Add(key, 0);
					using (IEnumerator<int> enumerator2 = current.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							int current2 = enumerator2.get_Current();
							dictionary2.Add(current2, key);
						}
					}
				}
			}
			int num = 0;
			int num2 = 0;
			List<int> list = new List<int>();
			List<List<Mst_slotitem>> list2 = new List<List<Mst_slotitem>>();
			using (var enumerator3 = Enumerable.Select(this.supportShips, (Mem_ship obj, int ship_idx) => new
			{
				obj,
				ship_idx
			}).GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					var current3 = enumerator3.get_Current();
					int num3 = dictionary2.get_Item(current3.obj.Stype);
					list.Add(current3.obj.Stype);
					Dictionary<int, int> dictionary3;
					Dictionary<int, int> expr_16B = dictionary3 = dictionary;
					int num4;
					int expr_170 = num4 = num3;
					num4 = dictionary3.get_Item(num4);
					expr_16B.set_Item(expr_170, num4 + 1);
					if (current3.obj.Get_FatigueState() == FatigueState.Exaltation)
					{
						int num5 = (current3.ship_idx != 0) ? 5 : 15;
						num2 += num5;
					}
					if (current3.obj.Get_DamageState() >= DamageState.Tyuuha)
					{
						undressing[current3.ship_idx] = true;
					}
					List<Mst_slotitem> list3 = new List<Mst_slotitem>();
					using (var enumerator4 = Enumerable.Select(current3.obj.Slot, (int rid, int idx) => new
					{
						rid,
						idx
					}).GetEnumerator())
					{
						while (enumerator4.MoveNext())
						{
							var current4 = enumerator4.get_Current();
							if (current4.rid <= 0)
							{
								break;
							}
							Mst_slotitem mst_slotitem = null;
							if (!Mst_DataManager.Instance.Mst_Slotitem.TryGetValue(Comm_UserDatas.Instance.User_slot.get_Item(current4.rid).Slotitem_id, ref mst_slotitem))
							{
								break;
							}
							list3.Add(mst_slotitem);
							if (current3.obj.Onslot.get_Item(current4.idx) > 0)
							{
								FighterInfo.FighterKinds kind = FighterInfo.GetKind(mst_slotitem);
								if (kind == FighterInfo.FighterKinds.BAKU || kind == FighterInfo.FighterKinds.RAIG)
								{
									num++;
								}
							}
						}
					}
					list2.Add(list3);
				}
			}
			int num6;
			if (this.supportType == MissionType.SupportForward)
			{
				num6 = 50 + num2;
			}
			else
			{
				num6 = num2 + 85;
			}
			if (num6 < this.randInstance.Next(100))
			{
				return BattleSupportKinds.None;
			}
			this._f_Data = new BattleBaseData(this.supportDeck, this.supportShips, list, list2);
			this._f_Data.Formation = BattleFormationKinds1.TanJuu;
			this._f_Data.BattleFormation = this.E_Data.BattleFormation;
			List<Mem_ship> memShip = this._f_Data.Deck.Ship.getMemShip();
			this._f_SubInfo = new Dictionary<int, BattleShipSubInfo>();
			for (int i = 0; i < memShip.get_Count(); i++)
			{
				BattleShipSubInfo battleShipSubInfo = new BattleShipSubInfo(i, memShip.get_Item(i));
				this._f_SubInfo.Add(memShip.get_Item(i).Rid, battleShipSubInfo);
			}
			int num7 = dictionary.get_Item(1);
			int num8 = dictionary.get_Item(2);
			int num9 = dictionary.get_Item(3);
			int num10 = dictionary.get_Item(4);
			int num11 = dictionary.get_Item(5);
			int num12 = dictionary.get_Item(6);
			if (num7 >= 3 && num > 0)
			{
				return BattleSupportKinds.AirAtack;
			}
			if (num8 >= 2)
			{
				return BattleSupportKinds.Raigeki;
			}
			if (num9 + num10 >= 4)
			{
				return BattleSupportKinds.Hougeki;
			}
			if (num11 + num12 >= 4)
			{
				return BattleSupportKinds.Raigeki;
			}
			return BattleSupportKinds.Hougeki;
		}

		protected override double getAvoHosei(Mem_ship target)
		{
			return 0.0;
		}
	}
}
