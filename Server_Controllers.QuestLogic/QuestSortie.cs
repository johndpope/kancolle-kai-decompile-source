using Common.Enum;
using Server_Common;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server_Controllers.QuestLogic
{
	public class QuestSortie : QuestLogicBase
	{
		private BattleWinRankKinds winKind;

		private List<Mem_ship> fShip;

		private List<Mem_ship> enemyShip;

		private int deckRid;

		private int areaId;

		private int mapNo;

		private bool boss;

		private Dictionary<int, Mst_ship> mstShip;

		private int checkType;

		public QuestSortie(int maparea_id, int mapinfo_no, int deckRid, List<Mem_ship> myShip)
		{
			this.areaId = maparea_id;
			this.mapNo = mapinfo_no;
			this.mstShip = Mst_DataManager.Instance.Mst_ship;
			this.checkData = base.getCheckDatas(2);
			this.fShip = myShip;
			this.winKind = BattleWinRankKinds.NONE;
			this.boss = false;
			this.checkType = 1;
			this.deckRid = deckRid;
		}

		public QuestSortie(int maparea_id, int mapinfo_no, BattleWinRankKinds winKind, int deckRid, List<Mem_ship> myShip, List<Mem_ship> enemyShip, bool boss) : this(maparea_id, mapinfo_no, deckRid, myShip)
		{
			this.boss = boss;
			this.winKind = winKind;
			this.enemyShip = enemyShip;
			this.checkType = 2;
		}

		public override List<int> ExecuteCheck()
		{
			List<int> list = new List<int>(this.checkData.get_Count());
			using (List<Mem_quest>.Enumerator enumerator = this.checkData.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mem_quest current = enumerator.get_Current();
					string funcName = base.getFuncName(current);
					bool flag = (bool)base.GetType().InvokeMember(funcName, 256, null, this, new object[]
					{
						current
					});
					if (flag)
					{
						current.StateChange(this, QuestState.COMPLETE);
						list.Add(current.Rid);
					}
				}
			}
			return list;
		}

		public bool Check_01(Mem_quest targetQuest)
		{
			return this.checkType == 2 && this.winKind == BattleWinRankKinds.S;
		}

		public bool Check_02(Mem_quest targetQuest)
		{
			return this.fShip != null;
		}

		public bool Check_03(Mem_quest targetQuest)
		{
			return this.checkType == 2 && (this.areaId == 1 && this.mapNo == 1 && this.boss && Utils.IsBattleWin(this.winKind));
		}

		public bool Check_04(Mem_quest targetQuest)
		{
			return this.checkType == 1 && (this.areaId == 1 && this.mapNo == 2 && this.fShip != null);
		}

		public bool Check_05(Mem_quest targetQuest)
		{
			return this.checkType == 2 && (this.areaId == 1 && this.mapNo == 2 && this.boss && Utils.IsBattleWin(this.winKind));
		}

		public bool Check_06(Mem_quest targetQuest)
		{
			if (this.checkType != 1)
			{
				return false;
			}
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(2);
			HashSet<int> target = hashSet;
			Dictionary<int, int> stypeCount = this.getStypeCount(this.fShip, target);
			return this.fShip.get_Item(0).Stype == 3 && stypeCount.get_Item(2) >= 2;
		}

		public bool Check_07(Mem_quest targetQuest)
		{
			if (this.checkType != 1)
			{
				return false;
			}
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(5);
			HashSet<int> target = hashSet;
			Dictionary<int, int> stypeCount = this.getStypeCount(this.fShip, target);
			return stypeCount.get_Item(5) >= 1;
		}

		public bool Check_08(Mem_quest targetQuest)
		{
			if (this.checkType != 1)
			{
				return false;
			}
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(8);
			hashSet.Add(9);
			hashSet.Add(10);
			HashSet<int> target = hashSet;
			Dictionary<int, int> stypeCount = this.getStypeCount(this.fShip, target);
			return stypeCount.get_Item(8) + stypeCount.get_Item(9) + stypeCount.get_Item(10) >= 1;
		}

		public bool Check_09(Mem_quest targetQuest)
		{
			if (this.checkType != 1)
			{
				return false;
			}
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(7);
			hashSet.Add(11);
			hashSet.Add(18);
			HashSet<int> target = hashSet;
			Dictionary<int, int> stypeCount = this.getStypeCount(this.fShip, target);
			return this.fShip.get_Count() >= 4 && stypeCount.get_Item(7) + stypeCount.get_Item(11) + stypeCount.get_Item(18) >= 1;
		}

		public bool Check_10(Mem_quest targetQuest)
		{
			if (this.checkType != 1)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			base.addCounterIncrementAll(counter);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_11(Mem_quest targetQuest)
		{
			if (this.checkType != 2)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			List<Mem_ship> destroyShip = this.getDestroyShip(this.enemyShip);
			int num = Enumerable.Count<Mem_ship>(destroyShip, (Mem_ship x) => Mst_DataManager.Instance.Mst_stype.get_Item(x.Stype).IsMother());
			if (num == 0)
			{
				return false;
			}
			for (int i = 0; i < num; i++)
			{
				base.addCounterIncrementAll(counter);
			}
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_12(Mem_quest targetQuest)
		{
			if (this.checkType != 2)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			List<Mem_ship> destroyShip = this.getDestroyShip(this.enemyShip);
			List<Mem_ship> arg_43_1 = destroyShip;
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(15);
			Dictionary<int, int> stypeCount = this.getStypeCount(arg_43_1, hashSet);
			if (stypeCount.get_Item(15) == 0)
			{
				return false;
			}
			for (int i = 0; i < stypeCount.get_Item(15); i++)
			{
				base.addCounterIncrementAll(counter);
			}
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_13(Mem_quest targetQuest)
		{
			return this.Check_12(targetQuest);
		}

		public bool Check_14(Mem_quest targetQuest)
		{
			Dictionary<int, int> dictionary = base.isAddCounter(targetQuest.Rid, this.checkData);
			if (this.checkType == 1)
			{
				if (dictionary.ContainsKey(2011))
				{
					dictionary.set_Item(2011, 1);
					base.addCounter(dictionary);
				}
				return base.CheckClearCounter(targetQuest.Rid);
			}
			dictionary.Remove(2011);
			int num = 0;
			int num2 = 0;
			if (this.boss)
			{
				num = 1;
				num2 = ((!Utils.IsBattleWin(this.winKind)) ? 0 : 1);
			}
			int num3 = (this.winKind != BattleWinRankKinds.S) ? 0 : 1;
			if (dictionary.ContainsKey(2012))
			{
				dictionary.set_Item(2012, num);
			}
			if (dictionary.ContainsKey(2013))
			{
				dictionary.set_Item(2013, num2);
			}
			if (dictionary.ContainsKey(2014))
			{
				dictionary.set_Item(2014, num3);
			}
			base.addCounter(dictionary);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_15(Mem_quest targetQuest)
		{
			return this.checkType == 1 && this.deckRid == 2;
		}

		public bool Check_16(Mem_quest targetQuest)
		{
			if (this.checkType != 2)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			if (!Utils.IsBattleWin(this.winKind) || !this.boss)
			{
				return false;
			}
			base.addCounterIncrementAll(counter);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_17(Mem_quest targetQuest)
		{
			return this.Check_11(targetQuest);
		}

		public bool Check_18(Mem_quest targetQuest)
		{
			return this.Check_12(targetQuest);
		}

		public bool Check_19(Mem_quest targetQuest)
		{
			if (this.checkType != 1)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("ちょうかい");
			hashSet.Add("かこ");
			hashSet.Add("あおば");
			hashSet.Add("ふるたか");
			hashSet.Add("てんりゅう");
			HashSet<string> target = hashSet;
			if (this.fShip.get_Count() < 6)
			{
				return false;
			}
			if (!this.containsAllYomi(this.fShip, target))
			{
				return false;
			}
			int num = 0;
			using (List<Mem_ship>.Enumerator enumerator = this.fShip.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mem_ship current = enumerator.get_Current();
					if (this.mstShip.get_Item(current.Ship_id).Soku == 10)
					{
						num++;
					}
				}
			}
			return num == 6;
		}

		public bool Check_20(Mem_quest targetQuest)
		{
			return this.Check_11(targetQuest);
		}

		public bool Check_21(Mem_quest targetQuest)
		{
			return this.Check_12(targetQuest);
		}

		public bool Check_22(Mem_quest targetQuest)
		{
			if (this.checkType != 1)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("あかつき");
			hashSet.Add("ひびき");
			hashSet.Add("いかづち");
			hashSet.Add("いなづま");
			HashSet<string> target = hashSet;
			return this.fShip.get_Count() >= 4 && this.containsAllYomi(this.fShip, target);
		}

		public bool Check_23(Mem_quest targetQuest)
		{
			if (this.checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("あたご");
			hashSet.Add("たかお");
			hashSet.Add("ちょうかい");
			hashSet.Add("まや");
			HashSet<string> target = hashSet;
			return this.fShip.get_Count() >= 4 && this.areaId == 1 && this.mapNo == 4 && this.boss && Utils.IsBattleWin(this.winKind) && this.containsAllYomi(this.fShip, target);
		}

		public bool Check_24(Mem_quest targetQuest)
		{
			if (this.checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("ふそう");
			hashSet.Add("やましろ");
			hashSet.Add("もがみ");
			hashSet.Add("しぐれ");
			HashSet<string> target = hashSet;
			return this.fShip.get_Count() >= 4 && this.areaId == 9 && this.mapNo == 4 && this.boss && Utils.IsBattleWin(this.winKind) && this.containsAllYomi(this.fShip, target);
		}

		public bool Check_25(Mem_quest targetQuest)
		{
			if (this.checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("しょうかく");
			hashSet.Add("ずいかく");
			HashSet<string> target = hashSet;
			return this.fShip.get_Count() >= 2 && this.areaId == 11 && this.mapNo == 4 && this.boss && Utils.IsBattleWin(this.winKind) && this.containsAllYomi(this.fShip, target);
		}

		public bool Check_26(Mem_quest targetQuest)
		{
			if (this.checkType != 2)
			{
				return false;
			}
			if (this.areaId != 7)
			{
				return false;
			}
			if (!this.boss || !Utils.IsBattleWin(this.winKind))
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			base.addCounterIncrementAll(counter);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_27(Mem_quest targetQuest)
		{
			if (this.checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("ちょうかい");
			hashSet.Add("あおば");
			hashSet.Add("きぬがさ");
			hashSet.Add("かこ");
			hashSet.Add("ふるたか");
			hashSet.Add("てんりゅう");
			HashSet<string> target = hashSet;
			return this.fShip.get_Count() == 6 && this.areaId == 7 && this.mapNo == 2 && this.boss && Utils.IsBattleWin(this.winKind) && this.containsAllYomi(this.fShip, target);
		}

		public bool Check_28(Mem_quest targetQuest)
		{
			if (this.checkType != 2)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			List<Mem_ship> destroyShip = this.getDestroyShip(this.enemyShip);
			int num = Enumerable.Count<Mem_ship>(destroyShip, (Mem_ship x) => Mst_DataManager.Instance.Mst_stype.get_Item(x.Stype).IsSubmarine());
			if (num == 0)
			{
				return false;
			}
			for (int i = 0; i < num; i++)
			{
				base.addCounterIncrementAll(counter);
			}
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_29(Mem_quest targetQuest)
		{
			if (this.checkType != 2)
			{
				return false;
			}
			if (this.areaId != 4 || this.mapNo > 4)
			{
				return false;
			}
			if (!this.boss || !Utils.IsBattleWin(this.winKind))
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			base.addCounterIncrementAll(counter);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_30(Mem_quest targetQuest)
		{
			return this.Check_28(targetQuest);
		}

		public bool Check_31(Mem_quest targetQuest)
		{
			if (this.checkType != 2)
			{
				return false;
			}
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(13);
			hashSet.Add(14);
			HashSet<int> target = hashSet;
			if (this.areaId != 11 || this.mapNo != 2 || !this.boss || !Utils.IsBattleWin(this.winKind))
			{
				return false;
			}
			Dictionary<int, int> stypeCount = this.getStypeCount(this.fShip, target);
			return stypeCount.get_Item(13) + stypeCount.get_Item(14) == this.fShip.get_Count() && stypeCount.get_Item(13) + stypeCount.get_Item(14) >= 2;
		}

		public bool Check_32(Mem_quest targetQuest)
		{
			if (this.checkType != 2)
			{
				return false;
			}
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(6);
			hashSet.Add(10);
			HashSet<int> target = hashSet;
			if (this.fShip.get_Count() < 4 || this.areaId != 8 || this.mapNo != 4 || !this.boss || !Utils.IsBattleWin(this.winKind))
			{
				return false;
			}
			Dictionary<int, int> stypeCount = this.getStypeCount(this.fShip, target);
			return stypeCount.get_Item(6) >= 2 && stypeCount.get_Item(10) >= 2;
		}

		public bool Check_33(Mem_quest targetQuest)
		{
			if (this.checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("ふるたか");
			hashSet.Add("かこ");
			hashSet.Add("あおば");
			hashSet.Add("きぬがさ");
			HashSet<string> target = hashSet;
			return this.fShip.get_Count() >= 4 && this.areaId == 11 && this.mapNo == 3 && this.boss && this.winKind == BattleWinRankKinds.S && this.containsAllYomi(this.fShip, target);
		}

		public bool Check_34(Mem_quest targetQuest)
		{
			if (this.checkType != 2)
			{
				return false;
			}
			if (this.areaId != 8 || this.mapNo != 2)
			{
				return false;
			}
			if (!this.boss || !Utils.IsBattleWin(this.winKind))
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			base.addCounterIncrementAll(counter);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_35(Mem_quest targetQuest)
		{
			return this.checkType == 2 && (this.areaId == 9 && this.mapNo == 1 && this.boss && Utils.IsBattleWin(this.winKind));
		}

		public bool Check_36(Mem_quest targetQuest)
		{
			return this.checkType == 2 && (this.areaId == 7 && this.mapNo == 1 && this.boss && Utils.IsBattleWin(this.winKind));
		}

		public bool Check_37(Mem_quest targetQuest)
		{
			return this.checkType == 2 && (this.areaId == 1 && this.mapNo == 4 && this.boss && Utils.IsBattleWin(this.winKind));
		}

		public bool Check_38(Mem_quest targetQuest)
		{
			return this.checkType == 2 && (this.areaId == 9 && this.mapNo == 2 && this.boss && Utils.IsBattleWin(this.winKind));
		}

		public bool Check_39(Mem_quest targetQuest)
		{
			return this.checkType == 2 && (this.areaId == 11 && this.mapNo == 2 && this.boss && Utils.IsBattleWin(this.winKind));
		}

		public bool Check_40(Mem_quest targetQuest)
		{
			if (this.checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("かすみ");
			hashSet.Add("あられ");
			hashSet.Add("かげろう");
			hashSet.Add("しらぬい");
			HashSet<string> target = hashSet;
			return this.areaId == 3 && this.mapNo == 1 && this.fShip.get_Count() >= 4 && this.boss && Utils.IsBattleWin(this.winKind) && this.winKind == BattleWinRankKinds.S && this.containsAllYomi(this.fShip, target);
		}

		public bool Check_41(Mem_quest targetQuest)
		{
			if (this.checkType != 2)
			{
				return false;
			}
			if (this.areaId != 3 || (this.mapNo != 3 && this.mapNo != 4))
			{
				return false;
			}
			if (!this.boss || this.winKind != BattleWinRankKinds.S)
			{
				return false;
			}
			Dictionary<int, int> dictionary = base.isAddCounter(targetQuest.Rid, this.checkData);
			int num = (this.mapNo != 3) ? 0 : 1;
			int num2 = (this.mapNo != 4) ? 0 : 1;
			if (dictionary.ContainsKey(2201))
			{
				dictionary.set_Item(2201, num);
			}
			if (dictionary.ContainsKey(2202))
			{
				dictionary.set_Item(2202, num2);
			}
			base.addCounter(dictionary);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_42(Mem_quest targetQuest)
		{
			return this.checkType == 2 && this.areaId == 4 && this.mapNo == 4 && (this.boss && this.winKind >= BattleWinRankKinds.A);
		}

		public bool Check_43(Mem_quest targetQuest)
		{
			if (this.checkType != 2)
			{
				return false;
			}
			if (this.areaId != 5 || this.mapNo != 2)
			{
				return false;
			}
			if (!this.boss || this.winKind != BattleWinRankKinds.S)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			base.addCounterIncrementAll(counter);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_44(Mem_quest targetQuest)
		{
			if (this.checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("むつき");
			hashSet.Add("きさらぎ");
			hashSet.Add("やよい");
			hashSet.Add("もちづき");
			HashSet<string> target = hashSet;
			return this.areaId == 3 && this.mapNo == 2 && this.fShip.get_Count() >= 4 && this.boss && this.winKind >= BattleWinRankKinds.C && this.containsAllYomi(this.fShip, target);
		}

		public bool Check_45(Mem_quest targetQuest)
		{
			return this.checkType == 2 && this.deckRid == 1 && this.areaId == 11 && this.mapNo == 2 && this.fShip.get_Item(0).Level >= 90 && this.fShip.get_Item(0).Level <= 99 && (this.boss && this.winKind == BattleWinRankKinds.S);
		}

		public bool Check_46(Mem_quest targetQuest)
		{
			return this.checkType == 2 && this.deckRid == 1 && this.areaId == 4 && this.mapNo == 3 && this.fShip.get_Item(0).Level >= 100 && (this.boss && this.winKind == BattleWinRankKinds.S);
		}

		public bool Check_47(Mem_quest targetQuest)
		{
			if (this.checkType != 2)
			{
				return false;
			}
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(10);
			HashSet<int> target = hashSet;
			if (this.areaId != 4 || this.mapNo != 1)
			{
				return false;
			}
			if (this.fShip.get_Count() < 2)
			{
				return false;
			}
			if (!this.boss || !Utils.IsBattleWin(this.winKind))
			{
				return false;
			}
			Dictionary<int, int> stypeCount = this.getStypeCount(this.fShip, target);
			return stypeCount.get_Item(10) >= 2;
		}

		public bool Check_48(Mem_quest targetQuest)
		{
			if (this.checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("むつき");
			hashSet.Add("やよい");
			hashSet.Add("うづき");
			hashSet.Add("もちづき");
			HashSet<string> target = hashSet;
			return this.areaId == 7 && this.mapNo == 1 && this.fShip.get_Count() >= 4 && this.boss && this.winKind >= BattleWinRankKinds.B && this.containsAllYomi(this.fShip, target);
		}

		public bool Check_49(Mem_quest targetQuest)
		{
			if (this.checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("みょうこう");
			hashSet.Add("なち");
			hashSet.Add("はぐろ");
			HashSet<string> target = hashSet;
			return this.areaId == 11 && this.mapNo == 4 && this.fShip.get_Count() >= 3 && this.boss && this.winKind == BattleWinRankKinds.S && this.containsAllYomi(this.fShip, target);
		}

		public bool Check_50(Mem_quest targetQuest)
		{
			if (this.checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("そうりゅう");
			HashSet<string> target = hashSet;
			HashSet<int> hashSet2 = new HashSet<int>();
			hashSet2.Add(2);
			HashSet<int> target2 = hashSet2;
			if (this.areaId != 5 || this.mapNo != 2)
			{
				return false;
			}
			if (this.fShip.get_Item(0).Ship_id != 196)
			{
				return false;
			}
			if (this.fShip.get_Count() < 4)
			{
				return false;
			}
			if (!this.boss || this.winKind != BattleWinRankKinds.S)
			{
				return false;
			}
			if (!this.containsAllYomi(this.fShip, target))
			{
				return false;
			}
			Dictionary<int, int> stypeCount = this.getStypeCount(this.fShip, target2);
			return stypeCount.get_Item(2) >= 2;
		}

		public bool Check_51(Mem_quest targetQuest)
		{
			if (this.checkType != 2)
			{
				return false;
			}
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(196);
			HashSet<int> target = hashSet;
			hashSet = new HashSet<int>();
			hashSet.Add(2);
			HashSet<int> target2 = hashSet;
			if (this.areaId != 4 || this.mapNo != 3)
			{
				return false;
			}
			if (this.fShip.get_Item(0).Ship_id != 197)
			{
				return false;
			}
			if (this.fShip.get_Count() < 4)
			{
				return false;
			}
			if (!this.boss || this.winKind != BattleWinRankKinds.S)
			{
				return false;
			}
			if (!this.containsAllShipId(this.fShip, target))
			{
				return false;
			}
			Dictionary<int, int> stypeCount = this.getStypeCount(this.fShip, target2);
			return stypeCount.get_Item(2) >= 2;
		}

		public bool Check_52(Mem_quest targetQuest)
		{
			return this.checkType == 2 && this.areaId == 7 && this.mapNo == 3 && this.boss && this.winKind == BattleWinRankKinds.S && this.mstShip.get_Item(this.fShip.get_Item(0).Ship_id).Ctype == 53;
		}

		public bool Check_53(Mem_quest targetQuest)
		{
			if (this.checkType != 2)
			{
				return false;
			}
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(7);
			hashSet.Add(3);
			hashSet.Add(2);
			HashSet<int> target = hashSet;
			if (this.areaId != 11 || this.mapNo != 1)
			{
				return false;
			}
			if (!this.boss || this.winKind != BattleWinRankKinds.S)
			{
				return false;
			}
			Dictionary<int, int> stypeCount = this.getStypeCount(this.fShip, target);
			return this.fShip.get_Count() == stypeCount.get_Item(7) + stypeCount.get_Item(3) + stypeCount.get_Item(2) && (stypeCount.get_Item(7) == 1 || stypeCount.get_Item(7) == 2) && stypeCount.get_Item(3) == 1;
		}

		public bool Check_54(Mem_quest targetQuest)
		{
			if (this.checkType != 2)
			{
				return false;
			}
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(3);
			hashSet.Add(2);
			HashSet<int> target = hashSet;
			if (this.areaId != 11 || this.mapNo != 1)
			{
				return false;
			}
			if (this.fShip.get_Item(0).Stype != 3)
			{
				return false;
			}
			if (!this.boss || this.winKind != BattleWinRankKinds.S)
			{
				return false;
			}
			Dictionary<int, int> stypeCount = this.getStypeCount(this.fShip, target);
			return this.fShip.get_Count() == stypeCount.get_Item(3) + stypeCount.get_Item(2) && (stypeCount.get_Item(3) == 1 || stypeCount.get_Item(3) == 2);
		}

		public bool Check_55(Mem_quest targetQuest)
		{
			if (this.checkType != 2)
			{
				return false;
			}
			if (this.areaId != 6 || this.mapNo != 1)
			{
				return false;
			}
			if (!this.boss || this.winKind != BattleWinRankKinds.S)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			base.addCounterIncrementAll(counter);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_56(Mem_quest targetQuest)
		{
			if (this.checkType != 2)
			{
				return false;
			}
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(3);
			hashSet.Add(2);
			HashSet<int> target = hashSet;
			if (this.areaId != 1 || this.mapNo != 4)
			{
				return false;
			}
			if (this.fShip.get_Item(0).Stype != 3)
			{
				return false;
			}
			if (!this.boss || this.winKind != BattleWinRankKinds.S)
			{
				return false;
			}
			Dictionary<int, int> stypeCount = this.getStypeCount(this.fShip, target);
			return this.fShip.get_Count() == stypeCount.get_Item(3) + stypeCount.get_Item(2) && (stypeCount.get_Item(3) >= 1 && stypeCount.get_Item(3) <= 3);
		}

		public bool Check_57(Mem_quest targetQuest)
		{
			if (this.checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("ながと");
			hashSet.Add("むつ");
			hashSet.Add("ふそう");
			hashSet.Add("やましろ");
			HashSet<string> target = hashSet;
			if (this.areaId != 4 || this.mapNo != 1)
			{
				return false;
			}
			if (!this.boss || this.winKind != BattleWinRankKinds.S)
			{
				return false;
			}
			if (!this.containsAllYomi(this.fShip, target))
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			base.addCounterIncrementAll(counter);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_58(Mem_quest targetQuest)
		{
			QuestSortie.<Check_58>c__AnonStorey4E8 <Check_58>c__AnonStorey4E = new QuestSortie.<Check_58>c__AnonStorey4E8();
			<Check_58>c__AnonStorey4E.<>f__this = this;
			if (this.checkType != 2)
			{
				return false;
			}
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(3);
			HashSet<int> target = hashSet;
			QuestSortie.<Check_58>c__AnonStorey4E8 arg_5F_0 = <Check_58>c__AnonStorey4E;
			hashSet = new HashSet<int>();
			hashSet.Add(37);
			hashSet.Add(19);
			hashSet.Add(2);
			hashSet.Add(26);
			arg_5F_0.enableCtype = hashSet;
			if (this.areaId != 5 || this.mapNo != 1)
			{
				return false;
			}
			if (this.fShip.get_Count() < 4)
			{
				return false;
			}
			if (!this.boss || this.winKind != BattleWinRankKinds.S)
			{
				return false;
			}
			Dictionary<int, int> stypeCount = this.getStypeCount(this.fShip, target);
			if (stypeCount.get_Item(3) < 1)
			{
				return false;
			}
			int num = Enumerable.Count<Mem_ship>(this.fShip, (Mem_ship x) => <Check_58>c__AnonStorey4E.enableCtype.Contains(<Check_58>c__AnonStorey4E.<>f__this.mstShip.get_Item(x.Ship_id).Ctype));
			return num == 3;
		}

		public bool Check_59(Mem_quest targetQuest)
		{
			if (this.checkType != 2)
			{
				return false;
			}
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(7);
			hashSet.Add(8);
			hashSet.Add(9);
			hashSet.Add(11);
			hashSet.Add(18);
			HashSet<int> target = hashSet;
			if (this.areaId != 3 || this.mapNo != 3)
			{
				return false;
			}
			if (!this.boss || this.winKind != BattleWinRankKinds.S)
			{
				return false;
			}
			Dictionary<int, int> stypeCount = this.getStypeCount(this.fShip, target);
			return stypeCount.get_Item(8) + stypeCount.get_Item(9) == 2 && stypeCount.get_Item(7) == 1 && stypeCount.get_Item(11) == 0 && stypeCount.get_Item(18) == 0;
		}

		public bool Check_60(Mem_quest targetQuest)
		{
			return this.checkType == 2 && this.areaId == 9 && this.mapNo == 4 && (this.boss && this.winKind >= BattleWinRankKinds.B);
		}

		public bool Check_61(Mem_quest targetQuest)
		{
			if (this.checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("ふそう");
			hashSet.Add("やましろ");
			hashSet.Add("もがみ");
			hashSet.Add("しぐれ");
			hashSet.Add("みちしお");
			HashSet<string> target = hashSet;
			return this.areaId == 5 && this.mapNo == 1 && this.fShip.get_Count() >= 5 && this.boss && this.winKind == BattleWinRankKinds.S && this.containsAllYomi(this.fShip, target);
		}

		public bool Check_62(Mem_quest targetQuest)
		{
			if (this.checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("ふるたか");
			hashSet.Add("かこ");
			hashSet.Add("あおば");
			hashSet.Add("きぬがさ");
			HashSet<string> target = hashSet;
			return this.areaId == 11 && this.mapNo == 3 && this.fShip.get_Count() >= 4 && this.boss && this.winKind == BattleWinRankKinds.S && this.containsAllYomi(this.fShip, target);
		}

		public bool Check_63(Mem_quest targetQuest)
		{
			if (this.checkType != 2)
			{
				return false;
			}
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(2);
			hashSet.Add(7);
			hashSet.Add(11);
			hashSet.Add(18);
			HashSet<int> target = hashSet;
			if (this.areaId != 4 || this.mapNo != 1)
			{
				return false;
			}
			if (this.fShip.get_Count() < 4)
			{
				return false;
			}
			if (!this.boss || this.winKind != BattleWinRankKinds.S)
			{
				return false;
			}
			Dictionary<int, int> stypeCount = this.getStypeCount(this.fShip, target);
			return stypeCount.get_Item(2) >= 2 && stypeCount.get_Item(7) + stypeCount.get_Item(11) + stypeCount.get_Item(18) >= 2;
		}

		public bool Check_64(Mem_quest targetQuest)
		{
			return this.checkType == 2 && this.areaId == 2 && this.mapNo == 3 && (this.boss && this.winKind >= BattleWinRankKinds.B);
		}

		public bool Check_65(Mem_quest targetQuest)
		{
			if (this.checkType != 2)
			{
				return false;
			}
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(2);
			hashSet.Add(3);
			hashSet.Add(5);
			HashSet<int> target = hashSet;
			if (this.areaId != 11 || this.mapNo != 4)
			{
				return false;
			}
			if (this.fShip.get_Count() < 4 || this.fShip.get_Item(0).Stype != 2)
			{
				return false;
			}
			if (!this.boss || this.winKind != BattleWinRankKinds.S)
			{
				return false;
			}
			Dictionary<int, int> stypeCount = this.getStypeCount(this.fShip, target);
			return stypeCount.get_Item(3) == 1 && stypeCount.get_Item(5) == 1 && stypeCount.get_Item(2) == 2;
		}

		public bool Check_66(Mem_quest targetQuest)
		{
			if (this.checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("ふぶき");
			hashSet.Add("しらゆき");
			hashSet.Add("はつゆき");
			hashSet.Add("むらくも");
			HashSet<string> target = hashSet;
			return this.areaId == 11 && this.mapNo == 1 && this.fShip.get_Count() >= 4 && this.boss && Utils.IsBattleWin(this.winKind) && this.containsAllYomi(this.fShip, target);
		}

		public bool Check_67(Mem_quest targetQuest)
		{
			return this.checkType == 2 && this.areaId == 10 && this.mapNo == 3 && (this.boss && this.winKind >= BattleWinRankKinds.B);
		}

		public bool Check_68(Mem_quest targetQuest)
		{
			if (this.checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("はつはる");
			hashSet.Add("ねのひ");
			hashSet.Add("わかば");
			hashSet.Add("はつしも");
			HashSet<string> target = hashSet;
			return this.areaId == 3 && this.mapNo == 1 && this.fShip.get_Count() >= 4 && this.boss && this.winKind == BattleWinRankKinds.S && this.containsAllYomi(this.fShip, target);
		}

		public bool Check_69(Mem_quest targetQuest)
		{
			if (this.checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("さつき");
			hashSet.Add("ふみづき");
			hashSet.Add("ながつき");
			HashSet<string> target = hashSet;
			HashSet<int> hashSet2 = new HashSet<int>();
			hashSet2.Add(2);
			HashSet<int> target2 = hashSet2;
			if (this.areaId != 1 || this.mapNo != 4)
			{
				return false;
			}
			if (this.fShip.get_Count() < 4)
			{
				return false;
			}
			if (!this.boss || this.winKind != BattleWinRankKinds.S)
			{
				return false;
			}
			if (!this.containsAllYomi(this.fShip, target))
			{
				return false;
			}
			Dictionary<int, int> stypeCount = this.getStypeCount(this.fShip, target2);
			return stypeCount.get_Item(2) >= 4;
		}

		public bool Check_70(Mem_quest targetQuest)
		{
			if (this.checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("はつしも");
			hashSet.Add("かすみ");
			hashSet.Add("うしお");
			hashSet.Add("あけぼの");
			HashSet<string> target = hashSet;
			return this.areaId == 11 && this.mapNo == 1 && this.fShip.get_Count() == 6 && this.mstShip.get_Item(this.fShip.get_Item(0).Ship_id).Yomi == "なち" && this.boss && this.winKind == BattleWinRankKinds.S && this.containsAllYomi(this.fShip, target);
		}

		public bool Check_71(Mem_quest targetQuest)
		{
			QuestSortie.<Check_71>c__AnonStorey4E9 <Check_71>c__AnonStorey4E = new QuestSortie.<Check_71>c__AnonStorey4E9();
			if (this.checkType != 2)
			{
				return false;
			}
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(2);
			HashSet<int> target = hashSet;
			QuestSortie.<Check_71>c__AnonStorey4E9 arg_4B_0 = <Check_71>c__AnonStorey4E;
			hashSet = new HashSet<int>();
			hashSet.Add(271);
			hashSet.Add(428);
			arg_4B_0.enableId = hashSet;
			if (this.areaId != 7 || this.mapNo != 2)
			{
				return false;
			}
			if (this.fShip.get_Count() < 4)
			{
				return false;
			}
			if (!this.boss || this.winKind < BattleWinRankKinds.C)
			{
				return false;
			}
			int num = Enumerable.Count<Mem_ship>(this.fShip, (Mem_ship x) => <Check_71>c__AnonStorey4E.enableId.Contains(x.Ship_id));
			if (num != 1)
			{
				return false;
			}
			Dictionary<int, int> stypeCount = this.getStypeCount(this.fShip, target);
			return stypeCount.get_Item(2) >= 3;
		}

		public bool Check_72(Mem_quest targetQuest)
		{
			QuestSortie.<Check_72>c__AnonStorey4EA <Check_72>c__AnonStorey4EA = new QuestSortie.<Check_72>c__AnonStorey4EA();
			<Check_72>c__AnonStorey4EA.<>f__this = this;
			if (this.checkType != 2)
			{
				return false;
			}
			QuestSortie.<Check_72>c__AnonStorey4EA arg_6B_0 = <Check_72>c__AnonStorey4EA;
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("あおば");
			hashSet.Add("きぬがさ");
			hashSet.Add("かこ");
			hashSet.Add("ふるたか");
			hashSet.Add("てんりゅう");
			hashSet.Add("ゆうばり");
			arg_6B_0.enableYomi = hashSet;
			if (this.areaId != 5 || this.mapNo != 1)
			{
				return false;
			}
			if (this.deckRid != 1)
			{
				return false;
			}
			if (this.fShip.get_Count() != 6 || this.fShip.get_Item(0).Ship_id != 427)
			{
				return false;
			}
			if (!this.boss || this.winKind != BattleWinRankKinds.S)
			{
				return false;
			}
			int num = Enumerable.Count<Mem_ship>(this.fShip, delegate(Mem_ship x)
			{
				Mst_ship mst_ship = null;
				return <Check_72>c__AnonStorey4EA.<>f__this.mstShip.TryGetValue(x.Ship_id, ref mst_ship) && <Check_72>c__AnonStorey4EA.enableYomi.Contains(mst_ship.Yomi);
			});
			return num == 5;
		}

		public bool Check_73(Mem_quest targetQuest)
		{
			if (this.checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("あかつき");
			hashSet.Add("ひびき");
			hashSet.Add("いかづち");
			hashSet.Add("いなづま");
			HashSet<string> target = hashSet;
			return this.areaId == 9 && this.mapNo == 2 && this.fShip.get_Count() >= 4 && this.boss && this.winKind >= BattleWinRankKinds.C && this.containsAllYomi(this.fShip, target);
		}

		public bool Check_74(Mem_quest targetQuest)
		{
			if (this.checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("てんりゅう");
			hashSet.Add("たつた");
			HashSet<string> target = hashSet;
			return this.areaId == 9 && this.mapNo == 1 && this.fShip.get_Count() >= 2 && this.boss && this.winKind >= BattleWinRankKinds.C && this.containsAllYomi(this.fShip, target);
		}

		public bool Check_75(Mem_quest targetQuest)
		{
			if (this.checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("ひえい");
			hashSet.Add("きりしま");
			hashSet.Add("ながら");
			hashSet.Add("あかつき");
			hashSet.Add("いかづち");
			hashSet.Add("いなづま");
			HashSet<string> target = hashSet;
			return this.areaId == 5 && this.mapNo == 3 && this.fShip.get_Count() == 6 && this.boss && this.winKind >= BattleWinRankKinds.C && this.containsAllYomi(this.fShip, target);
		}

		public bool Check_76(Mem_quest targetQuest)
		{
			if (this.checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("あかつき");
			hashSet.Add("ひびき");
			hashSet.Add("いかづち");
			hashSet.Add("いなづま");
			HashSet<string> target = hashSet;
			return this.areaId == 9 && this.mapNo == 1 && this.fShip.get_Count() >= 4 && this.boss && this.winKind >= BattleWinRankKinds.A && this.containsAllYomi(this.fShip, target);
		}

		public bool Check_77(Mem_quest targetQuest)
		{
			if (this.checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("ひびき");
			hashSet.Add("はつしも");
			hashSet.Add("わかば");
			hashSet.Add("さみだれ");
			hashSet.Add("しまかぜ");
			return this.areaId == 3 && this.mapNo == 2 && this.fShip.get_Count() == 6 && this.mstShip.get_Item(this.fShip.get_Item(0).Ship_id).Yomi == "あぶくま" && (this.boss && this.winKind >= BattleWinRankKinds.B);
		}

		public bool Check_78(Mem_quest targetQuest)
		{
			if (this.checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("ひびき");
			hashSet.Add("ゆうぐも");
			hashSet.Add("ながなみ");
			hashSet.Add("あきぐも");
			hashSet.Add("しまかぜ");
			return this.areaId == 3 && this.mapNo == 2 && this.fShip.get_Count() == 6 && this.fShip.get_Item(0).Ship_id == 200 && (this.boss && this.winKind == BattleWinRankKinds.S);
		}

		public bool Check_79(Mem_quest targetQuest)
		{
			return this.checkType == 2 && this.areaId == 3 && this.mapNo == 1 && (this.boss && this.winKind >= BattleWinRankKinds.B);
		}

		public bool Check_80(Mem_quest targetQuest)
		{
			if (this.checkType != 2)
			{
				return false;
			}
			if (this.areaId != 13 || (this.mapNo != 2 && this.mapNo != 3))
			{
				return false;
			}
			if (!this.boss || this.winKind < BattleWinRankKinds.B)
			{
				return false;
			}
			Dictionary<int, int> dictionary = base.isAddCounter(targetQuest.Rid, this.checkData);
			int num = (this.mapNo != 2) ? 0 : 1;
			int num2 = (this.mapNo != 3) ? 0 : 1;
			if (dictionary.ContainsKey(2206))
			{
				dictionary.set_Item(2206, num);
			}
			if (dictionary.ContainsKey(2207))
			{
				dictionary.set_Item(2207, num2);
			}
			base.addCounter(dictionary);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_81(Mem_quest targetQuest)
		{
			if (this.checkType != 2)
			{
				return false;
			}
			if (this.areaId != 12)
			{
				return false;
			}
			if (!this.boss || this.winKind < BattleWinRankKinds.B)
			{
				return false;
			}
			Dictionary<int, int> dictionary = base.isAddCounter(targetQuest.Rid, this.checkData);
			int num = (this.mapNo != 1) ? 0 : 1;
			int num2 = (this.mapNo != 2) ? 0 : 1;
			int num3 = (this.mapNo != 3) ? 0 : 1;
			int num4 = (this.mapNo != 4) ? 0 : 1;
			if (dictionary.ContainsKey(2208))
			{
				dictionary.set_Item(2208, num);
			}
			if (dictionary.ContainsKey(2209))
			{
				dictionary.set_Item(2209, num2);
			}
			if (dictionary.ContainsKey(2210))
			{
				dictionary.set_Item(2210, num3);
			}
			if (dictionary.ContainsKey(2211))
			{
				dictionary.set_Item(2211, num4);
			}
			base.addCounter(dictionary);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_82(Mem_quest targetQuest)
		{
			if (this.checkType != 2)
			{
				return false;
			}
			if (this.areaId != 4 || this.mapNo < 1 || this.mapNo > 4)
			{
				return false;
			}
			if (!this.boss || this.winKind < BattleWinRankKinds.B)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			base.addCounterIncrementAll(counter);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_83(Mem_quest targetQuest)
		{
			return this.checkType == 2 && this.areaId == 14 && this.mapNo == 4 && (this.boss && this.winKind >= BattleWinRankKinds.B);
		}

		public bool Check_84(Mem_quest targetQuest)
		{
			return this.checkType == 2 && this.areaId == 2 && this.mapNo == 4 && (this.fShip.get_Item(0).Stype == 7 || this.fShip.get_Item(0).Stype == 11 || this.fShip.get_Item(0).Stype == 18) && (this.boss && this.winKind >= BattleWinRankKinds.A);
		}

		public bool Check_85(Mem_quest targetQuest)
		{
			return this.checkType == 2 && this.areaId == 4 && this.mapNo == 1 && (this.boss && this.winKind >= BattleWinRankKinds.B);
		}

		public bool Check_86(Mem_quest targetQuest)
		{
			if (this.checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("しょうかく");
			hashSet.Add("ずいかく");
			hashSet.Add("おぼろ");
			hashSet.Add("あきぐも");
			HashSet<string> target = hashSet;
			return this.areaId == 5 && this.mapNo == 2 && this.fShip.get_Count() >= 4 && this.boss && this.winKind == BattleWinRankKinds.S && this.containsAllYomi(this.fShip, target);
		}

		public bool Check_87(Mem_quest targetQuest)
		{
			if (this.checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("たま");
			hashSet.Add("きそ");
			HashSet<string> target = hashSet;
			HashSet<int> hashSet2 = new HashSet<int>();
			hashSet2.Add(192);
			hashSet2.Add(193);
			HashSet<int> target2 = hashSet2;
			return this.areaId == 13 && this.mapNo == 2 && this.fShip.get_Count() >= 4 && this.boss && this.winKind == BattleWinRankKinds.S && this.containsAllShipId(this.fShip, target2) && this.containsAllYomi(this.fShip, target);
		}

		public bool Check_88(Mem_quest targetQuest)
		{
			if (this.checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("くま");
			hashSet.Add("ながら");
			HashSet<string> target = hashSet;
			return this.areaId == 8 && this.mapNo == 3 && this.fShip.get_Count() >= 3 && this.mstShip.get_Item(this.fShip.get_Item(0).Ship_id).Yomi == "あしがら" && this.boss && this.winKind >= BattleWinRankKinds.B && this.containsAllYomi(this.fShip, target);
		}

		public bool Check_89(Mem_quest targetQuest)
		{
			if (this.checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("あさしお");
			hashSet.Add("みちしお");
			hashSet.Add("おおしお");
			hashSet.Add("あらしお");
			HashSet<string> target = hashSet;
			return this.areaId == 11 && this.mapNo == 2 && this.fShip.get_Count() >= 4 && this.boss && this.winKind == BattleWinRankKinds.S && this.containsAllYomi(this.fShip, target);
		}

		public bool Check_90(Mem_quest targetQuest)
		{
			return this.checkType == 2 && this.areaId == 5 && this.mapNo == 4 && (this.boss && this.winKind >= BattleWinRankKinds.B);
		}

		public bool Check_91(Mem_quest targetQuest)
		{
			return this.checkType == 2 && this.areaId == 16 && this.mapNo == 3 && (this.boss && this.winKind >= BattleWinRankKinds.B);
		}

		public bool Check_92(Mem_quest targetQuest)
		{
			QuestSortie.<Check_92>c__AnonStorey4EB <Check_92>c__AnonStorey4EB = new QuestSortie.<Check_92>c__AnonStorey4EB();
			if (this.checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("ずいほう");
			HashSet<string> target = hashSet;
			QuestSortie.<Check_92>c__AnonStorey4EB arg_7A_0 = <Check_92>c__AnonStorey4EB;
			HashSet<int> hashSet2 = new HashSet<int>();
			hashSet2.Add(108);
			hashSet2.Add(291);
			hashSet2.Add(296);
			hashSet2.Add(109);
			hashSet2.Add(292);
			hashSet2.Add(297);
			arg_7A_0.enableId = hashSet2;
			if (this.areaId != 1 || this.mapNo != 4)
			{
				return false;
			}
			if (this.fShip.get_Count() < 4)
			{
				return false;
			}
			if (!this.boss || this.winKind != BattleWinRankKinds.S)
			{
				return false;
			}
			if (this.fShip.get_Item(0).Ship_id != 112 && this.fShip.get_Item(0).Ship_id != 462 && this.fShip.get_Item(0).Ship_id != 467)
			{
				return false;
			}
			int num = Enumerable.Count<Mem_ship>(this.fShip, (Mem_ship x) => <Check_92>c__AnonStorey4EB.enableId.Contains(x.Ship_id));
			return num == 2 && this.containsAllYomi(this.fShip, target);
		}

		public bool Check_93(Mem_quest targetQuest)
		{
			QuestSortie.<Check_93>c__AnonStorey4EC <Check_93>c__AnonStorey4EC = new QuestSortie.<Check_93>c__AnonStorey4EC();
			if (this.checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("ずいほう");
			QuestSortie.<Check_93>c__AnonStorey4EC arg_98_0 = <Check_93>c__AnonStorey4EC;
			HashSet<int> hashSet2 = new HashSet<int>();
			hashSet2.Add(108);
			hashSet2.Add(291);
			hashSet2.Add(296);
			hashSet2.Add(109);
			hashSet2.Add(292);
			hashSet2.Add(297);
			hashSet2.Add(117);
			hashSet2.Add(82);
			hashSet2.Add(88);
			arg_98_0.enableId = hashSet2;
			if (this.areaId != 11 || this.mapNo != 4)
			{
				return false;
			}
			if (this.fShip.get_Count() != 6)
			{
				return false;
			}
			if (!this.boss || this.winKind != BattleWinRankKinds.S)
			{
				return false;
			}
			if (this.fShip.get_Item(0).Ship_id != 112 && this.fShip.get_Item(0).Ship_id != 462 && this.fShip.get_Item(0).Ship_id != 467)
			{
				return false;
			}
			int num = Enumerable.Count<Mem_ship>(this.fShip, (Mem_ship x) => <Check_93>c__AnonStorey4EC.enableId.Contains(x.Ship_id));
			return num == 5;
		}

		public bool Check_94(Mem_quest targetQuest)
		{
			if (this.checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("いすず");
			hashSet.Add("きぬ");
			HashSet<string> target = hashSet;
			return this.areaId == 4 && this.mapNo == 2 && this.fShip.get_Count() >= 3 && this.mstShip.get_Item(this.fShip.get_Item(0).Ship_id).Yomi == "なとり" && this.boss && this.winKind >= BattleWinRankKinds.B && this.containsAllYomi(this.fShip, target);
		}

		public bool Check_95(Mem_quest targetQuest)
		{
			if (this.checkType != 2)
			{
				return false;
			}
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(2);
			hashSet.Add(6);
			hashSet.Add(7);
			hashSet.Add(10);
			hashSet.Add(11);
			hashSet.Add(18);
			HashSet<int> target = hashSet;
			if (this.areaId != 3 || this.mapNo != 4)
			{
				return false;
			}
			if (this.fShip.get_Count() != 6)
			{
				return false;
			}
			if (!this.boss || this.winKind != BattleWinRankKinds.S)
			{
				return false;
			}
			Dictionary<int, int> stypeCount = this.getStypeCount(this.fShip, target);
			return stypeCount.get_Item(2) == 2 && stypeCount.get_Item(7) + stypeCount.get_Item(11) + stypeCount.get_Item(18) == 2 && stypeCount.get_Item(6) + stypeCount.get_Item(10) == 2;
		}

		public bool Check_96(Mem_quest targetQuest)
		{
			if (this.checkType != 2)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("あしがら");
			hashSet.Add("おおよど");
			hashSet.Add("あさしも");
			hashSet.Add("きよしも");
			HashSet<string> target = hashSet;
			return this.areaId == 11 && this.mapNo == 2 && this.fShip.get_Count() >= 5 && this.mstShip.get_Item(this.fShip.get_Item(0).Ship_id).Yomi == "かすみ" && this.boss && this.winKind == BattleWinRankKinds.S && this.containsAllYomi(this.fShip, target);
		}

		public bool Check_97(Mem_quest targetQuest)
		{
			return this.checkType == 2 && this.areaId == 7 && this.mapNo == 2 && (this.boss && this.winKind >= BattleWinRankKinds.B);
		}

		public bool Check_98(Mem_quest targetQuest)
		{
			return this.checkType == 2 && !(this.mstShip.get_Item(this.fShip.get_Item(0).Ship_id).Yomi != "やまと") && this.areaId == 12 && this.mapNo == 3 && (this.boss && this.winKind == BattleWinRankKinds.S);
		}

		private bool containsAllYomi(List<Mem_ship> ships, HashSet<string> target)
		{
			int num = Enumerable.Count<Mem_ship>(ships, delegate(Mem_ship x)
			{
				Mst_ship mst_ship = null;
				return this.mstShip.TryGetValue(x.Ship_id, ref mst_ship) && target.Contains(mst_ship.Yomi);
			});
			return Enumerable.Count<string>(target) <= num;
		}

		private bool containsAllShipId(List<Mem_ship> ships, HashSet<int> target)
		{
			int num = Enumerable.Count<Mem_ship>(ships, (Mem_ship x) => target.Contains(x.Ship_id));
			return Enumerable.Count<int>(target) <= num;
		}

		private Dictionary<int, int> getStypeCount(List<Mem_ship> ships, HashSet<int> target)
		{
			Dictionary<int, int> ret = Enumerable.ToDictionary<int, int, int>(target, (int x) => x, (int y) => 0);
			ships.ForEach(delegate(Mem_ship x)
			{
				if (target.Contains(x.Stype))
				{
					Dictionary<int, int> ret;
					Dictionary<int, int> expr_1C = ret = ret;
					int num;
					int expr_24 = num = x.Stype;
					num = ret.get_Item(num);
					expr_1C.set_Item(expr_24, num + 1);
				}
			});
			return ret;
		}

		private List<Mem_ship> getDestroyShip(List<Mem_ship> ships)
		{
			if (ships == null)
			{
				return new List<Mem_ship>();
			}
			return Enumerable.ToList<Mem_ship>(Enumerable.Where<Mem_ship>(ships, (Mem_ship x) => !x.IsFight()));
		}
	}
}
