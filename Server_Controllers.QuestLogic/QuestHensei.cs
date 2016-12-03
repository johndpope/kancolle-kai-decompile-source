using Common.Enum;
using Server_Common;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server_Controllers.QuestLogic
{
	public class QuestHensei : QuestLogicBase
	{
		private Dictionary<int, Mem_deck> userDeck;

		private Dictionary<int, List<Mem_ship>> userDeckShip;

		private Dictionary<int, Mst_ship> mstShips;

		public QuestHensei()
		{
			this.checkData = base.getCheckDatas(1);
			this.Init();
		}

		public QuestHensei(int questId)
		{
			this.checkData = new List<Mem_quest>();
			Mem_quest mem_quest = Comm_UserDatas.Instance.User_quest.get_Item(questId);
			if (mem_quest.State == QuestState.RUNNING)
			{
				this.checkData.Add(mem_quest);
			}
			this.Init();
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
					bool flag = (bool)base.GetType().InvokeMember(funcName, 256, null, this, null);
					if (flag)
					{
						current.StateChange(this, QuestState.COMPLETE);
						list.Add(current.Rid);
					}
				}
			}
			return list;
		}

		public bool Check_01()
		{
			return Enumerable.Any<List<Mem_ship>>(this.userDeckShip.get_Values(), (List<Mem_ship> x) => x.get_Count() >= 2);
		}

		public bool Check_02()
		{
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(2);
			HashSet<int> target = hashSet;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					Dictionary<int, int> stypeCount = this.getStypeCount(current, target);
					if (stypeCount.get_Item(2) >= 4)
					{
						return true;
					}
				}
			}
			return false;
		}

		public bool Check_03()
		{
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(2);
			HashSet<int> target = hashSet;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					Dictionary<int, int> stypeCount = this.getStypeCount(current, target);
					if (stypeCount.get_Item(2) >= 2 && current.get_Item(0).Stype == 3)
					{
						return true;
					}
				}
			}
			return false;
		}

		public bool Check_04()
		{
			return Enumerable.Any<List<Mem_ship>>(this.userDeckShip.get_Values(), (List<Mem_ship> x) => x.get_Count() >= 6);
		}

		public bool Check_05()
		{
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(3);
			HashSet<int> target = hashSet;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					Dictionary<int, int> stypeCount = this.getStypeCount(current, target);
					if (stypeCount.get_Item(3) >= 2)
					{
						return true;
					}
				}
			}
			return false;
		}

		public bool Check_06()
		{
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(5);
			HashSet<int> target = hashSet;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					Dictionary<int, int> stypeCount = this.getStypeCount(current, target);
					if (stypeCount.get_Item(5) >= 2)
					{
						return true;
					}
				}
			}
			return false;
		}

		public bool Check_07()
		{
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(7);
			hashSet.Add(11);
			hashSet.Add(18);
			hashSet.Add(2);
			HashSet<int> target = hashSet;
			int num = 1;
			int num2 = 2;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					if (current.get_Count() >= 4)
					{
						Dictionary<int, int> stypeCount = this.getStypeCount(current, target);
						if (stypeCount.get_Item(7) + stypeCount.get_Item(11) + stypeCount.get_Item(18) >= num && stypeCount.get_Item(2) >= num2)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		public bool Check_08()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("てんりゅう");
			hashSet.Add("たつた");
			HashSet<string> target = hashSet;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					if (this.containsAllYomi(current, target))
					{
						return true;
					}
				}
			}
			return false;
		}

		public bool Check_09()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("せんだい");
			hashSet.Add("じんつう");
			hashSet.Add("なか");
			HashSet<string> target = hashSet;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					if (this.containsAllYomi(current, target))
					{
						return true;
					}
				}
			}
			return false;
		}

		public bool Check_10()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("みょうこう");
			hashSet.Add("なち");
			hashSet.Add("あしがら");
			hashSet.Add("はぐろ");
			HashSet<string> target = hashSet;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					if (this.containsAllYomi(current, target))
					{
						return true;
					}
				}
			}
			return false;
		}

		public bool Check_11()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("ふそう");
			hashSet.Add("やましろ");
			HashSet<string> target = hashSet;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					if (this.containsAllYomi(current, target))
					{
						return true;
					}
				}
			}
			return false;
		}

		public bool Check_12()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("いせ");
			hashSet.Add("ひゅうが");
			HashSet<string> target = hashSet;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					if (this.containsAllYomi(current, target))
					{
						return true;
					}
				}
			}
			return false;
		}

		public bool Check_13()
		{
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(5);
			hashSet.Add(8);
			hashSet.Add(9);
			HashSet<int> target = hashSet;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					Dictionary<int, int> stypeCount = this.getStypeCount(current, target);
					if (stypeCount.get_Item(5) >= 2 && stypeCount.get_Item(8) + stypeCount.get_Item(9) >= 1)
					{
						return true;
					}
				}
			}
			return false;
		}

		public bool Check_14()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("あかぎ");
			hashSet.Add("かが");
			hashSet.Add("ひりゅう");
			hashSet.Add("そうりゅう");
			HashSet<string> target = hashSet;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					if (this.containsAllYomi(current, target))
					{
						return true;
					}
				}
			}
			return false;
		}

		public bool Check_15()
		{
			return this.userDeckShip.get_Item(2).get_Count() >= 1;
		}

		public bool Check_16()
		{
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(16);
			HashSet<int> target = hashSet;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					Dictionary<int, int> stypeCount = this.getStypeCount(current, target);
					if (stypeCount.get_Item(16) >= 1)
					{
						return true;
					}
				}
			}
			return false;
		}

		public bool Check_17()
		{
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(7);
			hashSet.Add(11);
			hashSet.Add(18);
			HashSet<int> target = hashSet;
			Dictionary<int, int> stypeCount = this.getStypeCount(this.userDeckShip.get_Item(2), target);
			return stypeCount.get_Item(7) + stypeCount.get_Item(11) + stypeCount.get_Item(18) >= 1;
		}

		public bool Check_18()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("こんごう");
			hashSet.Add("ひえい");
			hashSet.Add("はるな");
			hashSet.Add("きりしま");
			HashSet<string> target = hashSet;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					if (current.get_Count() >= 4)
					{
						if (this.containsAllYomi(current, target))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		public bool Check_19()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("ちょうかい");
			hashSet.Add("かこ");
			hashSet.Add("あおば");
			hashSet.Add("ふるたか");
			hashSet.Add("てんりゅう");
			HashSet<string> target = hashSet;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					if (current.get_Count() >= 6)
					{
						if (this.containsAllYomi(current, target))
						{
							int num = 0;
							using (List<Mem_ship>.Enumerator enumerator2 = current.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									Mem_ship current2 = enumerator2.get_Current();
									if (this.mstShips.get_Item(current2.Ship_id).Soku == 10)
									{
										num++;
									}
								}
							}
							if (num == 6)
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		public bool Check_20()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("あかつき");
			hashSet.Add("ひびき");
			hashSet.Add("いかづち");
			hashSet.Add("いなづま");
			HashSet<string> target = hashSet;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					if (current.get_Count() == 4)
					{
						if (this.containsAllYomi(current, target))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		public bool Check_21()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("あたご");
			hashSet.Add("たかお");
			hashSet.Add("ちょうかい");
			hashSet.Add("まや");
			HashSet<string> target = hashSet;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					if (current.get_Count() >= 4)
					{
						if (this.containsAllYomi(current, target))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		public bool Check_22()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("ふそう");
			hashSet.Add("やましろ");
			hashSet.Add("もがみ");
			hashSet.Add("しぐれ");
			HashSet<string> target = hashSet;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					if (current.get_Count() >= 4)
					{
						if (this.containsAllYomi(current, target))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		public bool Check_23()
		{
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(2);
			HashSet<int> target = hashSet;
			HashSet<string> hashSet2 = new HashSet<string>();
			hashSet2.Add("しょうかく");
			hashSet2.Add("ずいかく");
			HashSet<string> target2 = hashSet2;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					if (current.get_Count() >= 4)
					{
						if (this.containsAllYomi(current, target2))
						{
							Dictionary<int, int> stypeCount = this.getStypeCount(current, target);
							if (stypeCount.get_Item(2) >= 2)
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		public bool Check_24()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("ちょうかい");
			hashSet.Add("あおば");
			hashSet.Add("きぬがさ");
			hashSet.Add("かこ");
			hashSet.Add("ふるたか");
			hashSet.Add("てんりゅう");
			HashSet<string> target = hashSet;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					if (current.get_Count() == 6)
					{
						if (this.containsAllYomi(current, target))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		public bool Check_25()
		{
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(13);
			hashSet.Add(14);
			HashSet<int> target = hashSet;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					if (current.get_Count() >= 2)
					{
						Dictionary<int, int> stypeCount = this.getStypeCount(current, target);
						if (stypeCount.get_Item(13) + stypeCount.get_Item(14) >= 2 && stypeCount.get_Item(13) + stypeCount.get_Item(14) == current.get_Count())
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		public bool Check_26()
		{
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(6);
			hashSet.Add(10);
			HashSet<int> target = hashSet;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					if (current.get_Count() >= 4)
					{
						Dictionary<int, int> stypeCount = this.getStypeCount(current, target);
						if (stypeCount.get_Item(6) >= 2 && stypeCount.get_Item(10) >= 2)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		public bool Check_27()
		{
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(13);
			hashSet.Add(14);
			HashSet<int> target = hashSet;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					if (current.get_Count() >= 3)
					{
						Dictionary<int, int> stypeCount = this.getStypeCount(current, target);
						if (stypeCount.get_Item(13) + stypeCount.get_Item(14) >= 3 && stypeCount.get_Item(13) + stypeCount.get_Item(14) == current.get_Count())
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		public bool Check_28()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("あおば");
			hashSet.Add("きぬがさ");
			hashSet.Add("かこ");
			hashSet.Add("ふるたか");
			HashSet<string> target = hashSet;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					if (current.get_Count() >= 4)
					{
						if (this.containsAllYomi(current, target))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		public bool Check_29()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("なち");
			hashSet.Add("あしがら");
			hashSet.Add("たま");
			hashSet.Add("きそ");
			HashSet<string> target = hashSet;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					if (current.get_Count() >= 4)
					{
						if (this.containsAllYomi(current, target))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		public bool Check_30()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("あぶくま");
			hashSet.Add("あけぼの");
			hashSet.Add("うしお");
			hashSet.Add("かすみ");
			hashSet.Add("しらぬい");
			HashSet<string> target = hashSet;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					if (current.get_Count() >= 5)
					{
						if (this.containsAllYomi(current, target))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		public bool Check_31()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("あさしお");
			hashSet.Add("みちしお");
			hashSet.Add("おおしお");
			hashSet.Add("あらしお");
			HashSet<string> target = hashSet;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					if (current.get_Count() == 4)
					{
						if (this.containsAllYomi(current, target))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		public bool Check_32()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("かすみ");
			hashSet.Add("あられ");
			hashSet.Add("かげろう");
			hashSet.Add("しらぬい");
			HashSet<string> target = hashSet;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					if (current.get_Count() == 4)
					{
						if (this.containsAllYomi(current, target))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		public bool Check_33()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("むつき");
			hashSet.Add("きさらぎ");
			hashSet.Add("やよい");
			hashSet.Add("もちづき");
			HashSet<string> target = hashSet;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					if (current.get_Count() == 4)
					{
						if (this.containsAllYomi(current, target))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		public bool Check_34()
		{
			if (this.userDeckShip.get_Item(1).get_Count() == 0)
			{
				return false;
			}
			int level = this.userDeckShip.get_Item(1).get_Item(0).Level;
			return level >= 90 && level <= 99;
		}

		public bool Check_35()
		{
			if (this.userDeckShip.get_Item(1).get_Count() != 6)
			{
				return false;
			}
			int level = this.userDeckShip.get_Item(1).get_Item(0).Level;
			return level >= 100;
		}

		public bool Check_36()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("むつき");
			hashSet.Add("やよい");
			hashSet.Add("うづき");
			hashSet.Add("もちづき");
			HashSet<string> target = hashSet;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					if (current.get_Count() == 4)
					{
						if (this.containsAllYomi(current, target))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		public bool Check_37()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("みょうこう");
			hashSet.Add("なち");
			hashSet.Add("はぐろ");
			HashSet<string> target = hashSet;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					if (current.get_Count() >= 3)
					{
						if (this.containsAllYomi(current, target))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		public bool Check_38()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("そうりゅう");
			HashSet<string> target = hashSet;
			HashSet<int> hashSet2 = new HashSet<int>();
			hashSet2.Add(2);
			HashSet<int> target2 = hashSet2;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					if (current.get_Count() >= 4)
					{
						if (current.get_Item(0).Ship_id == 196)
						{
							if (this.containsAllYomi(current, target))
							{
								Dictionary<int, int> stypeCount = this.getStypeCount(current, target2);
								if (stypeCount.get_Item(2) >= 2)
								{
									return true;
								}
							}
						}
					}
				}
			}
			return false;
		}

		public bool Check_39()
		{
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(13);
			hashSet.Add(14);
			HashSet<int> target = hashSet;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					if (current.get_Count() >= 5)
					{
						if (current.get_Item(0).Stype == 20)
						{
							Dictionary<int, int> stypeCount = this.getStypeCount(current, target);
							if (stypeCount.get_Item(13) + stypeCount.get_Item(14) >= 4)
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		public bool Check_40()
		{
			return this.userDeckShip.get_Item(1).get_Count() >= 1 && this.userDeckShip.get_Item(1).get_Item(0).Ship_id == 319;
		}

		public bool Check_41()
		{
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(196);
			HashSet<int> target = hashSet;
			hashSet = new HashSet<int>();
			hashSet.Add(2);
			HashSet<int> target2 = hashSet;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					if (current.get_Count() >= 4)
					{
						if (current.get_Item(0).Ship_id == 197)
						{
							if (this.containsAllShipId(current, target))
							{
								Dictionary<int, int> stypeCount = this.getStypeCount(current, target2);
								if (stypeCount.get_Item(2) >= 2)
								{
									return true;
								}
							}
						}
					}
				}
			}
			return false;
		}

		public bool Check_42()
		{
			return this.userDeckShip.get_Item(1).get_Count() >= 1 && this.mstShips.get_Item(this.userDeckShip.get_Item(1).get_Item(0).Ship_id).Ctype == 53;
		}

		public bool Check_43()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("ながと");
			hashSet.Add("むつ");
			hashSet.Add("ふそう");
			hashSet.Add("やましろ");
			HashSet<string> target = hashSet;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					if (current.get_Count() >= 4)
					{
						if (this.containsAllYomi(current, target))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		public bool Check_44()
		{
			QuestHensei.<Check_44>c__AnonStorey4E0 <Check_44>c__AnonStorey4E = new QuestHensei.<Check_44>c__AnonStorey4E0();
			<Check_44>c__AnonStorey4E.<>f__this = this;
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(3);
			HashSet<int> target = hashSet;
			QuestHensei.<Check_44>c__AnonStorey4E0 arg_54_0 = <Check_44>c__AnonStorey4E;
			hashSet = new HashSet<int>();
			hashSet.Add(37);
			hashSet.Add(19);
			hashSet.Add(2);
			hashSet.Add(26);
			arg_54_0.enableCtype = hashSet;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					if (current.get_Count() >= 4)
					{
						Dictionary<int, int> stypeCount = this.getStypeCount(current, target);
						if (stypeCount.get_Item(3) >= 1)
						{
							int num = Enumerable.Count<Mem_ship>(current, (Mem_ship x) => <Check_44>c__AnonStorey4E.enableCtype.Contains(<Check_44>c__AnonStorey4E.<>f__this.mstShips.get_Item(x.Ship_id).Ctype));
							if (num == 3)
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		public bool Check_45()
		{
			return this.userDeckShip.get_Item(1).get_Count() >= 1 && this.mstShips.get_Item(this.userDeckShip.get_Item(1).get_Item(0).Ship_id).Yomi == "あかし";
		}

		public bool Check_46()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("ふそう");
			hashSet.Add("やましろ");
			hashSet.Add("もがみ");
			hashSet.Add("しぐれ");
			hashSet.Add("みちしお");
			HashSet<string> target = hashSet;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					if (current.get_Count() >= 5)
					{
						if (this.containsAllYomi(current, target))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		public bool Check_47()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("あしがら");
			HashSet<string> target = hashSet;
			HashSet<int> hashSet2 = new HashSet<int>();
			hashSet2.Add(2);
			hashSet2.Add(3);
			HashSet<int> target2 = hashSet2;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					if (current.get_Count() == 6)
					{
						if (!(this.mstShips.get_Item(current.get_Item(0).Ship_id).Yomi != "かすみ"))
						{
							if (this.containsAllYomi(current, target))
							{
								Dictionary<int, int> stypeCount = this.getStypeCount(current, target2);
								if (stypeCount.get_Item(2) >= 4 && stypeCount.get_Item(3) >= 1)
								{
									return true;
								}
							}
						}
					}
				}
			}
			return false;
		}

		public bool Check_48()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("ふぶき");
			hashSet.Add("しらゆき");
			hashSet.Add("はつゆき");
			hashSet.Add("むらくも");
			HashSet<string> target = hashSet;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					if (current.get_Count() == 4)
					{
						if (this.containsAllYomi(current, target))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		public bool Check_49()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("はつはる");
			hashSet.Add("ねのひ");
			hashSet.Add("わかば");
			hashSet.Add("はつしも");
			HashSet<string> target = hashSet;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					if (current.get_Count() == 4)
					{
						if (this.containsAllYomi(current, target))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		public bool Check_50()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("さつき");
			hashSet.Add("ふみづき");
			hashSet.Add("ながつき");
			HashSet<string> target = hashSet;
			HashSet<int> hashSet2 = new HashSet<int>();
			hashSet2.Add(2);
			HashSet<int> target2 = hashSet2;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					if (current.get_Count() == 4)
					{
						if (this.containsAllYomi(current, target))
						{
							Dictionary<int, int> stypeCount = this.getStypeCount(current, target2);
							if (stypeCount.get_Item(2) == 4)
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		public bool Check_51()
		{
			QuestHensei.<Check_51>c__AnonStorey4E1 <Check_51>c__AnonStorey4E = new QuestHensei.<Check_51>c__AnonStorey4E1();
			<Check_51>c__AnonStorey4E.<>f__this = this;
			if (this.userDeckShip.get_Item(1).get_Count() < 6)
			{
				return false;
			}
			if (this.userDeckShip.get_Item(1).get_Item(0).Ship_id != 427)
			{
				return false;
			}
			QuestHensei.<Check_51>c__AnonStorey4E1 arg_99_0 = <Check_51>c__AnonStorey4E;
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("あおば");
			hashSet.Add("きぬがさ");
			hashSet.Add("かこ");
			hashSet.Add("ふるたか");
			hashSet.Add("てんりゅう");
			hashSet.Add("ゆうばり");
			arg_99_0.enableYomi = hashSet;
			int num = Enumerable.Count<Mem_ship>(this.userDeckShip.get_Item(1), delegate(Mem_ship x)
			{
				Mst_ship mst_ship = null;
				return <Check_51>c__AnonStorey4E.<>f__this.mstShips.TryGetValue(x.Ship_id, ref mst_ship) && <Check_51>c__AnonStorey4E.enableYomi.Contains(mst_ship.Yomi);
			});
			return num == 5;
		}

		public bool Check_52()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("てんりゅう");
			hashSet.Add("たつた");
			HashSet<string> target = hashSet;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					if (current.get_Count() >= 4)
					{
						if (this.containsAllYomi(current, target))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		public bool Check_53()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("ひえい");
			hashSet.Add("きりしま");
			hashSet.Add("ながら");
			hashSet.Add("あかつき");
			hashSet.Add("いかづち");
			hashSet.Add("いなづま");
			HashSet<string> target = hashSet;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					if (current.get_Count() == 6)
					{
						if (this.containsAllYomi(current, target))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		public bool Check_54()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("ひびき");
			hashSet.Add("いかづち");
			hashSet.Add("いなづま");
			HashSet<string> target = hashSet;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					if (current.get_Count() == 4)
					{
						if (current.get_Item(0).Ship_id == 437)
						{
							if (this.containsAllYomi(current, target))
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		public bool Check_55()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("ひびき");
			hashSet.Add("はつしも");
			hashSet.Add("わかば");
			hashSet.Add("さみだれ");
			hashSet.Add("しまかぜ");
			HashSet<string> target = hashSet;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					if (current.get_Count() == 6)
					{
						if (!(this.mstShips.get_Item(current.get_Item(0).Ship_id).Yomi != "あぶくま"))
						{
							if (this.containsAllYomi(current, target))
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		public bool Check_56()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("ひびき");
			hashSet.Add("ゆうぐも");
			hashSet.Add("ながなみ");
			hashSet.Add("あきぐも");
			hashSet.Add("しまかぜ");
			HashSet<string> target = hashSet;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					if (current.get_Count() == 6)
					{
						if (current.get_Item(0).Ship_id == 200)
						{
							if (this.containsAllYomi(current, target))
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		public bool Check_57()
		{
			return false;
		}

		public bool Check_58()
		{
			return false;
		}

		public bool Check_59()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("ずいかく");
			hashSet.Add("ずいほう");
			HashSet<string> target = hashSet;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					if (current.get_Count() == 6)
					{
						if (!(this.mstShips.get_Item(current.get_Item(0).Ship_id).Yomi != "しょうかく"))
						{
							if (this.containsAllYomi(current, target))
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		public bool Check_60()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("しょうかく");
			hashSet.Add("ずいかく");
			hashSet.Add("おぼろ");
			hashSet.Add("あきぐも");
			HashSet<string> target = hashSet;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					if (current.get_Count() >= 4)
					{
						if (this.containsAllYomi(current, target))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		public bool Check_61()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("たま");
			hashSet.Add("きそ");
			HashSet<string> target = hashSet;
			HashSet<int> hashSet2 = new HashSet<int>();
			hashSet2.Add(192);
			hashSet2.Add(193);
			HashSet<int> target2 = hashSet2;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					if (current.get_Count() >= 4)
					{
						if (this.containsAllShipId(current, target2))
						{
							if (this.containsAllYomi(current, target))
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		public bool Check_62()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("くま");
			hashSet.Add("ながら");
			HashSet<string> target = hashSet;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					if (current.get_Count() >= 3)
					{
						if (!(this.mstShips.get_Item(current.get_Item(0).Ship_id).Yomi != "あしがら"))
						{
							if (this.containsAllYomi(current, target))
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		public bool Check_63()
		{
			QuestHensei.<Check_63>c__AnonStorey4E2 <Check_63>c__AnonStorey4E = new QuestHensei.<Check_63>c__AnonStorey4E2();
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("ずいほう");
			HashSet<string> target = hashSet;
			QuestHensei.<Check_63>c__AnonStorey4E2 arg_71_0 = <Check_63>c__AnonStorey4E;
			HashSet<int> hashSet2 = new HashSet<int>();
			hashSet2.Add(108);
			hashSet2.Add(291);
			hashSet2.Add(296);
			hashSet2.Add(109);
			hashSet2.Add(292);
			hashSet2.Add(297);
			arg_71_0.enableId = hashSet2;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					if (current.get_Count() >= 4)
					{
						if (current.get_Item(0).Ship_id == 112 || current.get_Item(0).Ship_id == 462 || current.get_Item(0).Ship_id == 467)
						{
							int num = Enumerable.Count<Mem_ship>(current, (Mem_ship x) => <Check_63>c__AnonStorey4E.enableId.Contains(x.Ship_id));
							if (num == 2)
							{
								if (this.containsAllYomi(current, target))
								{
									return true;
								}
							}
						}
					}
				}
			}
			return false;
		}

		public bool Check_64()
		{
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(82);
			hashSet.Add(88);
			HashSet<int> target = hashSet;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					if (current.get_Count() >= 2)
					{
						if (this.containsAllShipId(current, target))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		public bool Check_65()
		{
			QuestHensei.<Check_65>c__AnonStorey4E3 <Check_65>c__AnonStorey4E = new QuestHensei.<Check_65>c__AnonStorey4E3();
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("ずいほう");
			QuestHensei.<Check_65>c__AnonStorey4E3 arg_8F_0 = <Check_65>c__AnonStorey4E;
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
			arg_8F_0.enableId = hashSet2;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					if (current.get_Count() >= 6)
					{
						if (current.get_Item(0).Ship_id == 112 || current.get_Item(0).Ship_id == 462 || current.get_Item(0).Ship_id == 467)
						{
							int num = Enumerable.Count<Mem_ship>(current, (Mem_ship x) => <Check_65>c__AnonStorey4E.enableId.Contains(x.Ship_id));
							if (num == 5)
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		public bool Check_66()
		{
			QuestHensei.<Check_66>c__AnonStorey4E4 <Check_66>c__AnonStorey4E = new QuestHensei.<Check_66>c__AnonStorey4E4();
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(2);
			HashSet<int> target = hashSet;
			QuestHensei.<Check_66>c__AnonStorey4E4 arg_59_0 = <Check_66>c__AnonStorey4E;
			hashSet = new HashSet<int>();
			hashSet.Add(461);
			hashSet.Add(466);
			hashSet.Add(462);
			hashSet.Add(467);
			arg_59_0.enableId = hashSet;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					if (current.get_Count() >= 4)
					{
						Dictionary<int, int> stypeCount = this.getStypeCount(current, target);
						if (stypeCount.get_Item(2) >= 2)
						{
							int num = Enumerable.Count<Mem_ship>(current, (Mem_ship x) => <Check_66>c__AnonStorey4E.enableId.Contains(x.Ship_id));
							bool result;
							if (num == 2)
							{
								result = true;
								return result;
							}
							result = false;
							return result;
						}
					}
				}
			}
			return false;
		}

		public bool Check_67()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("いすず");
			hashSet.Add("きぬ");
			HashSet<string> target = hashSet;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					if (current.get_Count() >= 3)
					{
						if (this.mstShips.get_Item(current.get_Item(0).Ship_id).Yomi == "なとり")
						{
							bool result;
							if (this.containsAllYomi(current, target))
							{
								result = true;
								return result;
							}
							result = false;
							return result;
						}
					}
				}
			}
			return false;
		}

		public bool Check_68()
		{
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(2);
			hashSet.Add(6);
			hashSet.Add(7);
			hashSet.Add(10);
			hashSet.Add(11);
			hashSet.Add(18);
			HashSet<int> target = hashSet;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					if (current.get_Count() == 6)
					{
						Dictionary<int, int> stypeCount = this.getStypeCount(current, target);
						bool result;
						if (stypeCount.get_Item(2) == 2 && stypeCount.get_Item(7) + stypeCount.get_Item(11) + stypeCount.get_Item(18) == 2 && stypeCount.get_Item(6) + stypeCount.get_Item(10) == 2)
						{
							result = true;
							return result;
						}
						result = false;
						return result;
					}
				}
			}
			return false;
		}

		public bool Check_69()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("あしがら");
			hashSet.Add("おおよど");
			hashSet.Add("あさしも");
			hashSet.Add("きよしも");
			HashSet<string> target = hashSet;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					if (current.get_Count() >= 5)
					{
						if (this.mstShips.get_Item(current.get_Item(0).Ship_id).Yomi == "かすみ")
						{
							bool result;
							if (this.containsAllYomi(current, target))
							{
								result = true;
								return result;
							}
							result = false;
							return result;
						}
					}
				}
			}
			return false;
		}

		public bool Check_70()
		{
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(13);
			hashSet.Add(14);
			HashSet<int> target = hashSet;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					if (current.get_Count() >= 4)
					{
						Dictionary<int, int> stypeCount = this.getStypeCount(current, target);
						if (stypeCount.get_Item(13) + stypeCount.get_Item(14) == current.get_Count())
						{
							if (current.get_Item(0).Level >= 80)
							{
								int num = 0;
								using (List<Mem_ship>.Enumerator enumerator2 = current.GetEnumerator())
								{
									while (enumerator2.MoveNext())
									{
										Mem_ship current2 = enumerator2.get_Current();
										if (current2.Level >= 40)
										{
											num++;
										}
									}
								}
								if (num >= 4)
								{
									return true;
								}
							}
						}
					}
				}
			}
			return false;
		}

		public bool Check_71()
		{
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(82);
			hashSet.Add(88);
			hashSet.Add(411);
			hashSet.Add(412);
			HashSet<int> target = hashSet;
			hashSet = new HashSet<int>();
			hashSet.Add(2);
			HashSet<int> target2 = hashSet;
			using (Dictionary<int, List<Mem_ship>>.ValueCollection.Enumerator enumerator = this.userDeckShip.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_ship> current = enumerator.get_Current();
					if (current.get_Count() == 6)
					{
						if (this.containsAllShipId(current, target))
						{
							Dictionary<int, int> stypeCount = this.getStypeCount(current, target2);
							if (stypeCount.get_Item(2) == 2)
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		private void Init()
		{
			this.userDeck = Comm_UserDatas.Instance.User_deck;
			this.userDeckShip = new Dictionary<int, List<Mem_ship>>();
			using (Dictionary<int, Mem_deck>.Enumerator enumerator = this.userDeck.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<int, Mem_deck> current = enumerator.get_Current();
					this.userDeckShip.Add(current.get_Key(), current.get_Value().Ship.getMemShip());
				}
			}
			this.mstShips = Mst_DataManager.Instance.Mst_ship;
		}

		private bool containsAllYomi(List<Mem_ship> ships, HashSet<string> target)
		{
			int num = Enumerable.Count<Mem_ship>(ships, delegate(Mem_ship x)
			{
				Mst_ship mst_ship = null;
				return this.mstShips.TryGetValue(x.Ship_id, ref mst_ship) && target.Contains(mst_ship.Yomi);
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
	}
}
