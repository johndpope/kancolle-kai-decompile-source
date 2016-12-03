using Common.Enum;
using Server_Common;
using Server_Common.Formats;
using Server_Common.Formats.Battle;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Server_Controllers.BattleLogic
{
	public class Exec_BattleResult : BattleLogicBase<BattleResultFmt>, IRebellionPointOperator
	{
		private BattleBaseData _f_Data;

		private BattleBaseData _e_Data;

		private Dictionary<int, BattleShipSubInfo> _f_SubInfo;

		private Dictionary<int, BattleShipSubInfo> _e_SubInfo;

		private readonly Mst_mapenemy2 mst_enemy;

		private readonly Mst_mapinfo mst_mapinfo;

		private readonly ExecBattleKinds battleKinds;

		private List<int> clothBrokenIds;

		private List<Mem_ship> deleteTargetShip;

		private Mem_mapclear cleard;

		private Mst_mapcell2 nowCell;

		private Dictionary<int, int> mst_shiplevel;

		private bool isRebellionBattle;

		private List<MapItemGetFmt> airCellItems;

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

		public Exec_BattleResult(BattleResultBase execBattleData)
		{
			this.mst_shiplevel = Mst_DataManager.Instance.Get_MstLevel(true);
			this._f_Data = execBattleData.MyData;
			this._e_Data = execBattleData.EnemyData;
			this._f_SubInfo = execBattleData.F_SubInfo;
			this._e_SubInfo = execBattleData.E_SubInfo;
			this.practiceFlag = execBattleData.PracticeFlag;
			this.battleKinds = execBattleData.ExecKinds;
			this.clothBrokenIds = new List<int>();
			if (!execBattleData.PracticeFlag)
			{
				this.mst_enemy = Mst_DataManager.Instance.Mst_mapenemy.get_Item(this._e_Data.Enemy_id);
				string text = this.mst_enemy.Maparea_id.ToString();
				string text2 = this.mst_enemy.Mapinfo_no.ToString();
				int num = int.Parse(text + text2);
				this.mst_mapinfo = Mst_DataManager.Instance.Mst_mapinfo.get_Item(num);
				this.deleteTargetShip = new List<Mem_ship>();
				this.cleard = execBattleData.Cleard;
				this.nowCell = execBattleData.NowCell;
			}
			this.isRebellionBattle = execBattleData.RebellionBattle;
			this.airCellItems = execBattleData.GetAirCellItems;
		}

		void IRebellionPointOperator.AddRebellionPoint(int area_id, int addNum)
		{
			throw new NotImplementedException();
		}

		void IRebellionPointOperator.SubRebellionPoint(int area_id, int subNum)
		{
			Comm_UserDatas.Instance.User_rebellion_point.get_Item(area_id).EndInvation(this);
		}

		public override BattleResultFmt GetResultData(FormationDatas formation, BattleCommandParams cParam)
		{
			return this.getData();
		}

		private BattleResultFmt getData()
		{
			BattleResultFmt ret = new BattleResultFmt();
			if (!this.practiceFlag)
			{
				ret.QuestName = this.mst_mapinfo.Name;
			}
			ret.EnemyName = this.E_Data.Enemy_Name;
			this.E_Data.ShipData.ForEach(delegate(Mem_ship x)
			{
				ret.EnemyId.Add(x.Ship_id);
			});
			ret.WinRank = this.getWinRank();
			if (this.isMvpGet(ret.WinRank))
			{
				Dictionary<int, BattleShipSubInfo> subInfoDict = Enumerable.ToDictionary<BattleShipSubInfo, int, BattleShipSubInfo>(this.F_SubInfo.get_Values(), (BattleShipSubInfo key) => key.DeckIdx, (BattleShipSubInfo value) => value);
				int mvp = this.getMvp(ret.WinRank, subInfoDict);
				ret.MvpShip = mvp;
			}
			Mem_record user_record = Comm_UserDatas.Instance.User_record;
			int addValue;
			if (!this.practiceFlag)
			{
				int num = Mst_maparea.MaxMapNum(Comm_UserDatas.Instance.User_basic.Difficult, this.mst_mapinfo.Maparea_id);
				ret.GetBaseExp = this.mst_enemy.Experience;
				ret.GetShipExp = this.getShipExpSortie(ret.WinRank, ret.MvpShip, ret.GetBaseExp);
				addValue = this.getUserExpSortie(ret.WinRank);
				SerializableDictionary<int, List<int>> levelUpInfo = null;
				this.updateShip(ret.WinRank, ret.MvpShip, ret.GetShipExp, out levelUpInfo);
				ret.LevelUpInfo = levelUpInfo;
				bool flag = Utils.IsBattleWin(ret.WinRank);
				bool takeAwayBattle = false;
				if (this.cleard != null && (this.cleard.State == MapClearState.InvationNeighbor || this.cleard.State == MapClearState.InvationOpen))
				{
					takeAwayBattle = true;
				}
				if (flag)
				{
					List<ItemGetFmt> list = new List<ItemGetFmt>();
					ItemGetFmt getShip = null;
					bool flag2 = this.isLastDance(Comm_UserDatas.Instance.User_basic.Difficult);
					if (!flag2)
					{
						this.getRewardShip(ret.WinRank, out getShip);
						if (getShip != null)
						{
							if (Comm_UserDatas.Instance.User_turn.Total_turn <= 100 && Enumerable.Any<Mem_ship>(Comm_UserDatas.Instance.User_ship.get_Values(), (Mem_ship x) => x.Ship_id == getShip.Id))
							{
								this.getRewardShip(ret.WinRank, out getShip);
							}
							if (getShip != null && Enumerable.Any<Mem_ship>(Comm_UserDatas.Instance.User_ship.get_Values(), (Mem_ship x) => x.Ship_id == getShip.Id))
							{
								this.getRewardShip(ret.WinRank, out getShip);
							}
						}
					}
					else
					{
						this.getClearShip(Comm_UserDatas.Instance.User_basic.Difficult, ret.WinRank, out getShip);
					}
					if (getShip != null)
					{
						this.addShip(getShip.Id, flag2);
						list.Add(getShip);
					}
					if (list.get_Count() > 0)
					{
						ret.GetItem = list;
					}
					if (!this.isRebellionBattle)
					{
						List<int> newOpenMapId = null;
						List<int> reOpenMapId = null;
						ret.FirstClear = this.updateMapComp(out newOpenMapId, out reOpenMapId);
						ret.NewOpenMapId = newOpenMapId;
						ret.ReOpenMapId = reOpenMapId;
						if (ret.FirstClear && Utils.IsGameClear())
						{
							user_record.AddClearDifficult(Comm_UserDatas.Instance.User_basic.Difficult);
						}
						else if (ret.FirstClear && this.mst_mapinfo.No == num)
						{
							ItemGetFmt itemGetFmt = new ItemGetFmt();
							itemGetFmt.Id = 57;
							itemGetFmt.Count = 1;
							itemGetFmt.Category = ItemGetKinds.UseItem;
							ret.AreaClearRewardItem = itemGetFmt;
							Comm_UserDatas.Instance.Add_Useitem(itemGetFmt.Id, itemGetFmt.Count);
						}
						if (this.mst_enemy.Boss != 0)
						{
							ret.FirstAreaComplete = this.updateAreaCompHisory(num);
						}
					}
					if (this.mst_enemy.Boss != 0 && this.airCellItems != null)
					{
						ret.GetAirReconnaissanceItems = this.airCellItems;
					}
				}
				else if (this.mst_enemy.Boss != 0 && this.airCellItems != null)
				{
					ret.GetAirReconnaissanceItems = new List<MapItemGetFmt>();
				}
				if (this.isRebellionBattle)
				{
					this.updateRebellion(ret.WinRank);
					ret.FirstClear = false;
				}
				bool rebellionBoss = this.isRebellionBattle && this.mst_enemy.Boss != 0;
				user_record.UpdateSortieCount(ret.WinRank, rebellionBoss);
				if (ret.FirstClear)
				{
					ret.GetSpoint = this.mst_mapinfo.Clear_spoint;
					Comm_UserDatas.Instance.User_basic.AddPoint(ret.GetSpoint);
				}
				this.deleteLostShip();
				if (this.mst_enemy.Boss != 0)
				{
					this.updateHistory(flag, ret.NewOpenMapId, takeAwayBattle);
				}
			}
			else
			{
				ret.GetBaseExp = this.getBaseExpPractice(ret.WinRank);
				ret.GetShipExp = this.getShipExpPractice(ret.WinRank, ret.MvpShip, ret.GetBaseExp);
				addValue = this.getUserExpPractice(ret.WinRank, Comm_UserDatas.Instance.User_record.Level, this.E_Data.ShipData.get_Item(0).Level);
				SerializableDictionary<int, List<int>> levelUpInfo2 = null;
				this.updateShip(ret.WinRank, ret.MvpShip, ret.GetShipExp, out levelUpInfo2);
				this.updateShipPracticeEnemy(ret.GetShipExp, ref levelUpInfo2);
				ret.LevelUpInfo = levelUpInfo2;
				user_record.UpdatePracticeCount(ret.WinRank, true);
			}
			Comm_UserDatas.Instance.UpdateShipBookBrokenClothState(this.clothBrokenIds);
			ret.BasicLevel = user_record.UpdateExp(addValue, Mst_DataManager.Instance.Get_MstLevel(false));
			return ret;
		}

		private bool isLastDance(DifficultKind kind)
		{
			if (this.mst_mapinfo.Maparea_id != 17)
			{
				return false;
			}
			int num = Mst_maparea.MaxMapNum(kind, 17);
			return this.mst_mapinfo.No == num && this.mst_enemy.Boss != 0;
		}

		private void updateRebellion(BattleWinRankKinds winRank)
		{
			if (this.mst_enemy.Boss == 0)
			{
				return;
			}
			int maparea_id = this.mst_mapinfo.Maparea_id;
			if (Utils.IsBattleWin(winRank))
			{
				((IRebellionPointOperator)this).SubRebellionPoint(maparea_id, 0);
				return;
			}
		}

		private bool updateAreaCompHisory(int maxMapNum)
		{
			int firstMap = Mst_mapinfo.ConvertMapInfoId(this.mst_mapinfo.Maparea_id, 1);
			List<Mem_history> list = null;
			if (!Comm_UserDatas.Instance.User_history.TryGetValue(999, ref list))
			{
				list = new List<Mem_history>();
			}
			if (Enumerable.Any<Mem_history>(list, (Mem_history x) => x.MapinfoId == firstMap))
			{
				return false;
			}
			int num = Enumerable.Count<Mem_mapclear>(Comm_UserDatas.Instance.User_mapclear.get_Values(), (Mem_mapclear x) => x.Maparea_id == this.mst_mapinfo.Maparea_id && x.Cleared && x.State == MapClearState.Cleard);
			if (num != maxMapNum)
			{
				return false;
			}
			Mem_history mem_history = new Mem_history();
			mem_history.SetAreaComplete(firstMap);
			Comm_UserDatas.Instance.Add_History(mem_history);
			return true;
		}

		private bool updateMapComp(out List<int> diffMapOpen, out List<int> reOpenMap)
		{
			diffMapOpen = new List<int>();
			reOpenMap = new List<int>();
			if (this.mst_enemy.Boss == 0)
			{
				return false;
			}
			bool result = false;
			Dictionary<int, Mst_mapinfo>.KeyCollection keys = Utils.GetActiveMap().get_Keys();
			if (this.cleard == null)
			{
				this.cleard = new Mem_mapclear(this.mst_mapinfo.Id, this.mst_mapinfo.Maparea_id, this.mst_mapinfo.No, MapClearState.Cleard);
				this.cleard.Insert();
				result = true;
			}
			else
			{
				if (this.cleard.State != MapClearState.InvationOpen && this.cleard.State != MapClearState.InvationNeighbor)
				{
					return this.cleard.State == MapClearState.Cleard && false;
				}
				if (!this.cleard.Cleared)
				{
					result = true;
				}
				this.cleard.StateChange(MapClearState.Cleard);
			}
			List<int> list = null;
			bool flag = new RebellionUtils().MapReOpen(this.cleard, out list);
			Dictionary<int, Mst_mapinfo>.KeyCollection keys2 = Utils.GetActiveMap().get_Keys();
			diffMapOpen = Enumerable.ToList<int>(Enumerable.Except<int>(keys2, keys));
			reOpenMap = Enumerable.ToList<int>(Enumerable.Intersect<int>(list, diffMapOpen));
			using (List<int>.Enumerator enumerator = reOpenMap.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int current = enumerator.get_Current();
					diffMapOpen.Remove(current);
				}
			}
			return result;
		}

		private void updateHistory(bool winFlag, List<int> openMaps, bool takeAwayBattle)
		{
			int total_turn = Comm_UserDatas.Instance.User_turn.Total_turn;
			using (List<int>.Enumerator enumerator = openMaps.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int current = enumerator.get_Current();
					if (Mem_history.IsFirstOpenArea(current))
					{
						Mem_history mem_history = new Mem_history();
						mem_history.SetAreaOpen(total_turn, current);
						Comm_UserDatas.Instance.Add_History(mem_history);
					}
				}
			}
			Mem_history mem_history2 = null;
			if (winFlag)
			{
				int mapClearNum = Mem_history.GetMapClearNum(this.mst_mapinfo.Id);
				int num = Mst_maparea.MaxMapNum(Comm_UserDatas.Instance.User_basic.Difficult, this.mst_mapinfo.Maparea_id);
				if (this.mst_mapinfo.Maparea_id == 17 && this.mst_mapinfo.No == num)
				{
					mem_history2 = new Mem_history();
					mem_history2.SetGameClear(total_turn);
				}
				else if (mapClearNum == 1)
				{
					mem_history2 = new Mem_history();
					mem_history2.SetMapClear(total_turn, this.mst_mapinfo.Id, mapClearNum, this.F_Data.ShipData.get_Item(0).Ship_id);
				}
				else if (mapClearNum <= 3 && takeAwayBattle)
				{
					mem_history2 = new Mem_history();
					mem_history2.SetMapClear(total_turn, this.mst_mapinfo.Id, mapClearNum, this.F_Data.ShipData.get_Item(0).Ship_id);
				}
			}
			if (mem_history2 != null)
			{
				Comm_UserDatas.Instance.Add_History(mem_history2);
			}
		}

		private void getRewardShip(BattleWinRankKinds rank, out ItemGetFmt out_items)
		{
			out_items = null;
			if (rank < BattleWinRankKinds.B)
			{
				return;
			}
			if (Comm_UserDatas.Instance.User_basic.IsMaxChara() || Comm_UserDatas.Instance.User_basic.IsMaxSlotitem())
			{
				return;
			}
			int enemy_type = 0;
			if (this.mst_enemy.Boss == 0)
			{
				enemy_type = 1;
				Dictionary<BattleWinRankKinds, int> dictionary = new Dictionary<BattleWinRankKinds, int>();
				dictionary.Add(BattleWinRankKinds.S, 30);
				dictionary.Add(BattleWinRankKinds.A, 40);
				dictionary.Add(BattleWinRankKinds.B, 30);
				Dictionary<BattleWinRankKinds, int> dictionary2 = dictionary;
				if (this.randInstance.Next(100) < dictionary2.get_Item(rank))
				{
					return;
				}
			}
			else
			{
				enemy_type = 2;
			}
			DifficultKind difficult = Comm_UserDatas.Instance.User_basic.Difficult;
			Mem_turn user_turn = Comm_UserDatas.Instance.User_turn;
			Dictionary<BattleWinRankKinds, int[]> dictionary4;
			if (difficult == DifficultKind.SHI || difficult == DifficultKind.KOU)
			{
				Dictionary<BattleWinRankKinds, int[]> dictionary3 = new Dictionary<BattleWinRankKinds, int[]>();
				dictionary3.Add(BattleWinRankKinds.S, new int[]
				{
					default(int),
					50
				});
				dictionary3.Add(BattleWinRankKinds.A, new int[]
				{
					20,
					70
				});
				dictionary3.Add(BattleWinRankKinds.B, new int[]
				{
					50,
					100
				});
				dictionary4 = dictionary3;
			}
			else if (difficult == DifficultKind.OTU)
			{
				if (user_turn.Total_turn >= 0 && user_turn.Total_turn <= 50)
				{
					Dictionary<BattleWinRankKinds, int[]> dictionary3 = new Dictionary<BattleWinRankKinds, int[]>();
					dictionary3.Add(BattleWinRankKinds.S, new int[]
					{
						default(int),
						45
					});
					dictionary3.Add(BattleWinRankKinds.A, new int[]
					{
						15,
						60
					});
					dictionary3.Add(BattleWinRankKinds.B, new int[]
					{
						50,
						95
					});
					dictionary4 = dictionary3;
				}
				else if (user_turn.Total_turn >= 51 && user_turn.Total_turn <= 100)
				{
					Dictionary<BattleWinRankKinds, int[]> dictionary3 = new Dictionary<BattleWinRankKinds, int[]>();
					dictionary3.Add(BattleWinRankKinds.S, new int[]
					{
						default(int),
						50
					});
					dictionary3.Add(BattleWinRankKinds.A, new int[]
					{
						15,
						65
					});
					dictionary3.Add(BattleWinRankKinds.B, new int[]
					{
						50,
						100
					});
					dictionary4 = dictionary3;
				}
				else
				{
					Dictionary<BattleWinRankKinds, int[]> dictionary3 = new Dictionary<BattleWinRankKinds, int[]>();
					dictionary3.Add(BattleWinRankKinds.S, new int[]
					{
						default(int),
						50
					});
					dictionary3.Add(BattleWinRankKinds.A, new int[]
					{
						20,
						70
					});
					dictionary3.Add(BattleWinRankKinds.B, new int[]
					{
						50,
						100
					});
					dictionary4 = dictionary3;
				}
			}
			else if (difficult == DifficultKind.HEI)
			{
				if (user_turn.Total_turn >= 0 && user_turn.Total_turn <= 50)
				{
					Dictionary<BattleWinRankKinds, int[]> dictionary3 = new Dictionary<BattleWinRankKinds, int[]>();
					dictionary3.Add(BattleWinRankKinds.S, new int[]
					{
						default(int),
						40
					});
					dictionary3.Add(BattleWinRankKinds.A, new int[]
					{
						15,
						55
					});
					dictionary3.Add(BattleWinRankKinds.B, new int[]
					{
						50,
						90
					});
					dictionary4 = dictionary3;
				}
				else if (user_turn.Total_turn >= 51 && user_turn.Total_turn <= 100)
				{
					Dictionary<BattleWinRankKinds, int[]> dictionary3 = new Dictionary<BattleWinRankKinds, int[]>();
					dictionary3.Add(BattleWinRankKinds.S, new int[]
					{
						default(int),
						45
					});
					dictionary3.Add(BattleWinRankKinds.A, new int[]
					{
						15,
						60
					});
					dictionary3.Add(BattleWinRankKinds.B, new int[]
					{
						50,
						95
					});
					dictionary4 = dictionary3;
				}
				else
				{
					Dictionary<BattleWinRankKinds, int[]> dictionary3 = new Dictionary<BattleWinRankKinds, int[]>();
					dictionary3.Add(BattleWinRankKinds.S, new int[]
					{
						default(int),
						45
					});
					dictionary3.Add(BattleWinRankKinds.A, new int[]
					{
						20,
						65
					});
					dictionary3.Add(BattleWinRankKinds.B, new int[]
					{
						50,
						95
					});
					dictionary4 = dictionary3;
				}
			}
			else if (user_turn.Total_turn >= 0 && user_turn.Total_turn <= 100)
			{
				Dictionary<BattleWinRankKinds, int[]> dictionary3 = new Dictionary<BattleWinRankKinds, int[]>();
				dictionary3.Add(BattleWinRankKinds.S, new int[]
				{
					default(int),
					35
				});
				dictionary3.Add(BattleWinRankKinds.A, new int[]
				{
					15,
					50
				});
				dictionary3.Add(BattleWinRankKinds.B, new int[]
				{
					50,
					85
				});
				dictionary4 = dictionary3;
			}
			else if (user_turn.Total_turn >= 101 && user_turn.Total_turn <= 200)
			{
				Dictionary<BattleWinRankKinds, int[]> dictionary3 = new Dictionary<BattleWinRankKinds, int[]>();
				dictionary3.Add(BattleWinRankKinds.S, new int[]
				{
					default(int),
					40
				});
				dictionary3.Add(BattleWinRankKinds.A, new int[]
				{
					15,
					55
				});
				dictionary3.Add(BattleWinRankKinds.B, new int[]
				{
					50,
					90
				});
				dictionary4 = dictionary3;
			}
			else
			{
				Dictionary<BattleWinRankKinds, int[]> dictionary3 = new Dictionary<BattleWinRankKinds, int[]>();
				dictionary3.Add(BattleWinRankKinds.S, new int[]
				{
					default(int),
					45
				});
				dictionary3.Add(BattleWinRankKinds.A, new int[]
				{
					15,
					60
				});
				dictionary3.Add(BattleWinRankKinds.B, new int[]
				{
					50,
					95
				});
				dictionary4 = dictionary3;
			}
			int num = this.randInstance.Next(dictionary4.get_Item(rank)[0], dictionary4.get_Item(rank)[1]) + this.mst_enemy.Geth;
			if (num > dictionary4.get_Item(rank)[1])
			{
				num = dictionary4.get_Item(rank)[1] - 1;
			}
			XElement[] array = Enumerable.ToArray<XElement>(Enumerable.Take<XElement>(Enumerable.Skip<XElement>(Enumerable.OrderBy<XElement, int>(Enumerable.Where<XElement>(Mst_DataManager.Instance.Mst_shipget, (XElement data) => int.Parse(data.Element("Type").get_Value()) == enemy_type), (XElement data) => int.Parse(data.Element("Id").get_Value())), num), 1));
			if (array == null || array.Length == 0)
			{
				return;
			}
			Mst_shipget2 mst_shipget = null;
			Model_Base.SetMaster<Mst_shipget2>(out mst_shipget, array[0]);
			if (mst_shipget.Ship_id <= 0)
			{
				return;
			}
			out_items = new ItemGetFmt();
			out_items.Category = ItemGetKinds.Ship;
			out_items.Count = 1;
			out_items.Id = mst_shipget.Ship_id;
		}

		private void getClearShip(DifficultKind kind, BattleWinRankKinds rank, out ItemGetFmt out_items)
		{
			out_items = null;
			if (!Utils.IsBattleWin(rank))
			{
				return;
			}
			List<int> clearRewardShipList = ArrayMaster.GetClearRewardShipList(kind);
			Dictionary<int, Mst_ship> mst_ship = Mst_DataManager.Instance.Mst_ship;
			int num = 0;
			using (List<int>.Enumerator enumerator = clearRewardShipList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int current = enumerator.get_Current();
					string targetYomi = mst_ship.get_Item(current).Yomi;
					if (!Enumerable.Any<Mem_ship>(Comm_UserDatas.Instance.User_ship.get_Values(), delegate(Mem_ship x)
					{
						int ship_id = x.Ship_id;
						string yomi = mst_ship.get_Item(ship_id).Yomi;
						return yomi.Equals(targetYomi);
					}))
					{
						num = current;
						break;
					}
				}
			}
			if (num == 0)
			{
				return;
			}
			out_items = new ItemGetFmt();
			out_items.Category = ItemGetKinds.Ship;
			out_items.Count = 1;
			out_items.Id = num;
		}

		private void addShip(int mst_id, bool lastDance)
		{
			Comm_UserDatas arg_13_0 = Comm_UserDatas.Instance;
			List<int> list = new List<int>();
			list.Add(mst_id);
			List<int> list2 = arg_13_0.Add_Ship(list);
			if (lastDance)
			{
				Comm_UserDatas.Instance.User_plus.SetRewardShipRid(this, list2.get_Item(0));
			}
		}

		private BattleWinRankKinds getWinRank()
		{
			int count = this.F_Data.ShipData.get_Count();
			List<Mem_ship> shipData = this.F_Data.ShipData;
			List<int> startHp = this.F_Data.StartHp;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			for (int i = 0; i < count; i++)
			{
				if (!shipData.get_Item(i).Escape_sts)
				{
					if (shipData.get_Item(i).Nowhp <= 0)
					{
						num3++;
					}
					num += shipData.get_Item(i).Nowhp;
					num2 += startHp.get_Item(i);
				}
			}
			int count2 = this.E_Data.ShipData.get_Count();
			List<Mem_ship> shipData2 = this.E_Data.ShipData;
			List<int> startHp2 = this.E_Data.StartHp;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			for (int j = 0; j < count2; j++)
			{
				if (shipData2.get_Item(j).Nowhp <= 0)
				{
					num6++;
				}
				num4 += shipData2.get_Item(j).Nowhp;
				num5 += startHp2.get_Item(j);
			}
			if (num3 == 0 && count2 == num6)
			{
				return BattleWinRankKinds.S;
			}
			int num7 = (int)Math.Floor((double)((float)count2 * 0.7f));
			if (num3 == 0 && num6 >= num7 && count2 > 1)
			{
				return BattleWinRankKinds.A;
			}
			if (num3 < num6 && shipData2.get_Item(0).Nowhp <= 0)
			{
				return BattleWinRankKinds.B;
			}
			DamageState damageState = shipData.get_Item(0).Get_DamageState();
			if (count == 1 && damageState == DamageState.Taiha)
			{
				return BattleWinRankKinds.D;
			}
			float num8 = (float)(num2 - num);
			float num9 = (float)(num5 - num4);
			int num10 = (int)(num9 / (float)num5 * 100f);
			int num11 = (int)(num8 / (float)num2 * 100f);
			if ((float)num10 > (float)num11 * 2.5f)
			{
				return BattleWinRankKinds.B;
			}
			if ((float)num10 > (float)num11 * 0.9f)
			{
				return BattleWinRankKinds.C;
			}
			if (count > 1 && count - 1 == num3)
			{
				return BattleWinRankKinds.E;
			}
			return BattleWinRankKinds.D;
		}

		private bool isMvpGet(BattleWinRankKinds kind)
		{
			return kind != BattleWinRankKinds.E && kind != BattleWinRankKinds.NONE;
		}

		private int getMvp(BattleWinRankKinds rank, Dictionary<int, BattleShipSubInfo> subInfoDict)
		{
			int num = Enumerable.Max(Enumerable.Select<BattleShipSubInfo, int>(subInfoDict.get_Values(), (BattleShipSubInfo item) => item.TotalDamage));
			if (subInfoDict.get_Item(0).TotalDamage == num)
			{
				return subInfoDict.get_Item(0).ShipInstance.Rid;
			}
			int num2 = -1;
			int result = 0;
			using (Dictionary<int, BattleShipSubInfo>.ValueCollection.Enumerator enumerator = subInfoDict.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BattleShipSubInfo current = enumerator.get_Current();
					if (current.ShipInstance.IsFight() && current.TotalDamage > num2)
					{
						result = current.ShipInstance.Rid;
						num2 = current.TotalDamage;
					}
				}
			}
			return result;
		}

		private int getBaseExpPractice(BattleWinRankKinds rank)
		{
			int num = 500;
			int num2 = (this.E_Data.ShipData.get_Item(0).Level != 100) ? this.E_Data.ShipData.get_Item(0).Level : 99;
			int num3 = 0;
			if (Enumerable.Count<Mem_ship>(this.E_Data.ShipData) > 1)
			{
				num3 = ((this.E_Data.ShipData.get_Item(1).Level != 100) ? this.E_Data.ShipData.get_Item(1).Level : 99);
			}
			float num4 = (float)this.mst_shiplevel.get_Item(num2) / 100f;
			float num5 = (num3 <= 0) ? 0f : ((float)this.mst_shiplevel.get_Item(num3) / 300f);
			int num6 = (int)(num4 + num5);
			num6 += this.randInstance.Next(4);
			if (num6 > num)
			{
				num6 = (int)((double)num + Math.Sqrt((double)(num6 - num)));
			}
			float num7 = 1f;
			float num8 = 0.8f;
			Dictionary<BattleWinRankKinds, float> dictionary = new Dictionary<BattleWinRankKinds, float>();
			dictionary.Add(BattleWinRankKinds.S, 1.2f * num7);
			dictionary.Add(BattleWinRankKinds.A, 1f * num7);
			dictionary.Add(BattleWinRankKinds.B, 1f * num7);
			dictionary.Add(BattleWinRankKinds.C, 0.8f * num8);
			dictionary.Add(BattleWinRankKinds.D, 0.7f * num8);
			dictionary.Add(BattleWinRankKinds.E, 0.5f * num8);
			Dictionary<BattleWinRankKinds, float> dictionary2 = dictionary;
			return (int)((float)num6 * dictionary2.get_Item(rank));
		}

		private SerializableDictionary<int, int> getShipExpSortie(BattleWinRankKinds rank, int mvpShip, int shipBaseExp)
		{
			SerializableDictionary<int, int> serializableDictionary = new SerializableDictionary<int, int>();
			float num = 1.5f;
			int num2 = 2;
			double num3 = 4.5;
			List<int> list = new List<int>();
			using (Dictionary<int, BattleShipSubInfo>.Enumerator enumerator = this.F_SubInfo.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<int, BattleShipSubInfo> current = enumerator.get_Current();
					Mem_ship shipInstance = current.get_Value().ShipInstance;
					double num4 = (double)((!shipInstance.IsFight()) ? 0 : shipBaseExp);
					if (current.get_Value().DeckIdx == 0)
					{
						num4 *= (double)num;
					}
					num4 *= num3;
					serializableDictionary.Add(shipInstance.Rid, (int)num4);
				}
			}
			if (mvpShip <= 0)
			{
				return serializableDictionary;
			}
			double num5 = (double)(serializableDictionary[mvpShip] * num2);
			serializableDictionary[mvpShip] = (int)num5;
			return serializableDictionary;
		}

		private SerializableDictionary<int, int> getShipExpPractice(BattleWinRankKinds rank, int mvpShip, int shipBaseExp)
		{
			SerializableDictionary<int, int> serializableDictionary = new SerializableDictionary<int, int>();
			float num = 1.5f;
			int num2 = 2;
			double num3 = 0.7;
			double difficultShipExpKeisuToPractice = this.getDifficultShipExpKeisuToPractice();
			double trainingShipExpKeisuToPractice = this.getTrainingShipExpKeisuToPractice();
			double num4 = 7.5;
			List<int> list = new List<int>();
			using (Dictionary<int, BattleShipSubInfo>.Enumerator enumerator = this.F_SubInfo.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<int, BattleShipSubInfo> current = enumerator.get_Current();
					Mem_ship shipInstance = current.get_Value().ShipInstance;
					double num5 = (double)((!shipInstance.IsFight()) ? 0 : shipBaseExp);
					if (current.get_Value().DeckIdx == 0)
					{
						num5 *= (double)num;
					}
					num5 = num5 * difficultShipExpKeisuToPractice * trainingShipExpKeisuToPractice * num4 * num3;
					serializableDictionary.Add(shipInstance.Rid, (int)num5);
				}
			}
			double num6 = (double)shipBaseExp * 0.5;
			double num7 = 3.5;
			using (Dictionary<int, BattleShipSubInfo>.Enumerator enumerator2 = this.E_SubInfo.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					KeyValuePair<int, BattleShipSubInfo> current2 = enumerator2.get_Current();
					Mem_ship shipInstance2 = current2.get_Value().ShipInstance;
					double num8 = (!shipInstance2.IsFight()) ? 0.0 : num6;
					num8 = num8 * difficultShipExpKeisuToPractice * num7 * num3;
					serializableDictionary.Add(shipInstance2.Rid, (int)num8);
				}
			}
			if (mvpShip <= 0)
			{
				return serializableDictionary;
			}
			double num9 = (double)(serializableDictionary[mvpShip] * num2);
			serializableDictionary[mvpShip] = (int)num9;
			return serializableDictionary;
		}

		private double getDifficultShipExpKeisuToPractice()
		{
			DifficultKind difficult = Comm_UserDatas.Instance.User_basic.Difficult;
			if (difficult == DifficultKind.SHI)
			{
				return 1.2;
			}
			if (difficult == DifficultKind.KOU)
			{
				return 1.3;
			}
			if (difficult == DifficultKind.OTU)
			{
				return 1.4;
			}
			if (difficult == DifficultKind.HEI)
			{
				return 1.5;
			}
			if (difficult == DifficultKind.TEI)
			{
				return 2.0;
			}
			return 1.0;
		}

		private double getTrainingShipExpKeisuToPractice()
		{
			int num = Enumerable.Count<Mem_ship>(this.F_Data.ShipData, (Mem_ship x) => x.IsFight() && Mst_DataManager.Instance.Mst_stype.get_Item(x.Stype).IsTrainingShip());
			if (num == 0)
			{
				return 1.0;
			}
			Mem_ship mem_ship = this.F_Data.ShipData.get_Item(0);
			bool flag = Mst_DataManager.Instance.Mst_stype.get_Item(mem_ship.Stype).IsTrainingShip();
			if (flag)
			{
				if (num - 1 == 0)
				{
					if (mem_ship.Level < 9)
					{
						return 1.05;
					}
					if (mem_ship.Level >= 10 && mem_ship.Level <= 29)
					{
						return 1.08;
					}
					if (mem_ship.Level >= 30 && mem_ship.Level <= 59)
					{
						return 1.12;
					}
					if (mem_ship.Level >= 60 && mem_ship.Level <= 99)
					{
						return 1.1;
					}
					return 1.2;
				}
				else
				{
					if (mem_ship.Level < 9)
					{
						return 1.1;
					}
					if (mem_ship.Level >= 10 && mem_ship.Level <= 29)
					{
						return 1.13;
					}
					if (mem_ship.Level >= 30 && mem_ship.Level <= 59)
					{
						return 1.16;
					}
					if (mem_ship.Level >= 60 && mem_ship.Level <= 99)
					{
						return 1.2;
					}
					return 1.25;
				}
			}
			else if (num >= 2)
			{
				if (mem_ship.Level < 9)
				{
					return 1.04;
				}
				if (mem_ship.Level >= 10 && mem_ship.Level <= 29)
				{
					return 1.06;
				}
				if (mem_ship.Level >= 30 && mem_ship.Level <= 59)
				{
					return 1.08;
				}
				if (mem_ship.Level >= 60 && mem_ship.Level <= 99)
				{
					return 1.12;
				}
				return 1.175;
			}
			else
			{
				if (mem_ship.Level < 9)
				{
					return 1.03;
				}
				if (mem_ship.Level >= 10 && mem_ship.Level <= 29)
				{
					return 1.05;
				}
				if (mem_ship.Level >= 30 && mem_ship.Level <= 59)
				{
					return 1.07;
				}
				if (mem_ship.Level >= 60 && mem_ship.Level <= 99)
				{
					return 1.1;
				}
				return 1.15;
			}
		}

		private int getUserExpSortie(BattleWinRankKinds rank)
		{
			Dictionary<BattleWinRankKinds, float[]> dictionary = new Dictionary<BattleWinRankKinds, float[]>();
			dictionary.Add(BattleWinRankKinds.S, new float[]
			{
				1f,
				2f
			});
			dictionary.Add(BattleWinRankKinds.A, new float[]
			{
				0.8f,
				1.5f
			});
			dictionary.Add(BattleWinRankKinds.B, new float[]
			{
				0.5f,
				1.2f
			});
			dictionary.Add(BattleWinRankKinds.C, new float[]
			{
				default(float),
				1f
			});
			dictionary.Add(BattleWinRankKinds.D, new float[]
			{
				default(float),
				1f
			});
			dictionary.Add(BattleWinRankKinds.E, new float[]
			{
				default(float),
				1f
			});
			Dictionary<BattleWinRankKinds, float[]> dictionary2 = dictionary;
			int boss = this.mst_enemy.Boss;
			int num = 0;
			if (boss == 1 && Utils.IsBattleWin(rank))
			{
				num = this.mst_mapinfo.Clear_exp;
			}
			int member_exp = this.mst_mapinfo.Member_exp;
			return (int)((float)member_exp * dictionary2.get_Item(rank)[boss]) + num;
		}

		private int getUserExpPractice(BattleWinRankKinds rank, int myBasicLevel, int enemyBasicLevel)
		{
			Dictionary<BattleWinRankKinds, float> dictionary = new Dictionary<BattleWinRankKinds, float>();
			dictionary.Add(BattleWinRankKinds.S, 2f);
			dictionary.Add(BattleWinRankKinds.A, 1.5f);
			dictionary.Add(BattleWinRankKinds.B, 1.2f);
			dictionary.Add(BattleWinRankKinds.C, 1f);
			dictionary.Add(BattleWinRankKinds.D, 1f);
			dictionary.Add(BattleWinRankKinds.E, 1f);
			Dictionary<BattleWinRankKinds, float> dictionary2 = dictionary;
			List<int[]> list = new List<int[]>();
			list.Add(new int[]
			{
				5,
				80
			});
			list.Add(new int[]
			{
				3,
				60
			});
			list.Add(new int[]
			{
				1,
				40
			});
			list.Add(new int[]
			{
				default(int),
				30
			});
			list.Add(new int[]
			{
				-2,
				20
			});
			list.Add(new int[]
			{
				-3,
				10
			});
			List<int[]> list2 = list;
			int lvaway = enemyBasicLevel - myBasicLevel;
			int[] array = Enumerable.FirstOrDefault<int[]>(list2, (int[] x) => lvaway >= x[0]);
			int num = (array == null) ? list2.get_Item(list2.get_Count() - 1)[1] : array[1];
			return (int)((float)num * dictionary2.get_Item(rank));
		}

		private void updateShip(BattleWinRankKinds rank, int mvpShip, SerializableDictionary<int, int> getShipExp, out SerializableDictionary<int, List<int>> lvupInfo)
		{
			lvupInfo = new SerializableDictionary<int, List<int>>();
			List<Mem_ship> shipData = this.F_Data.ShipData;
			int count = shipData.get_Count();
			bool flag = true;
			double num = 1.0;
			double num2 = 0.0;
			if (!this.practiceFlag)
			{
				flag = this.E_Data.ShipData.Exists((Mem_ship x) => !Mst_DataManager.Instance.Mst_stype.get_Item(x.Stype).IsSubmarine());
				if (!flag)
				{
					num = 0.25;
					num2 = 0.5;
				}
			}
			Dictionary<int, int> dictionary = this.mst_shiplevel;
			for (int i = 0; i < count; i++)
			{
				Mem_ship mem_ship = shipData.get_Item(i);
				Mem_shipBase mem_shipBase = new Mem_shipBase(mem_ship);
				Mst_ship mst_ship = Mst_DataManager.Instance.Mst_ship.get_Item(mem_shipBase.Ship_id);
				int num3 = getShipExp[mem_ship.Rid];
				List<int> value = null;
				int levelupInfo = mem_ship.getLevelupInfo(dictionary, mem_shipBase.Level, mem_shipBase.Exp, ref num3, out value);
				lvupInfo.Add(mem_ship.Rid, value);
				if (!mem_ship.Escape_sts)
				{
					mem_shipBase.Level = levelupInfo;
					mem_shipBase.Exp += num3;
					mem_shipBase.Fuel -= mst_ship.Use_fuel;
					if (mem_shipBase.Fuel < 0)
					{
						mem_shipBase.Fuel = 0;
					}
					int num4 = mst_ship.Use_bull;
					if (this.battleKinds == ExecBattleKinds.DayToNight)
					{
						num4 = (int)Math.Ceiling((double)num4 * 1.5);
					}
					num4 = (int)((double)num4 * num + num2);
					if (!flag && num4 <= 0)
					{
						num4 = 1;
					}
					mem_shipBase.Bull -= num4;
					if (mem_shipBase.Bull < 0)
					{
						mem_shipBase.Bull = 0;
					}
					this.setCondSubValue(ref mem_shipBase.Cond);
					bool mvp = mvpShip == mem_ship.Rid;
					bool flag2 = this.F_SubInfo.get_Item(mem_ship.Rid).DeckIdx == 0;
					this.setCondBonus(rank, flag2, mvp, ref mem_shipBase.Cond);
					int num5 = levelupInfo - mem_ship.Level;
					Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary2 = mem_ship.Kyouka;
					for (int j = 0; j < num5; j++)
					{
						dictionary2 = mem_ship.getLevelupKyoukaValue(mem_ship.Ship_id, dictionary2);
					}
					mem_shipBase.SetKyoukaValue(dictionary2);
					if (mem_ship.Get_DamageState() >= DamageState.Tyuuha)
					{
						this.clothBrokenIds.Add(mem_ship.Ship_id);
					}
					if (this.practiceFlag)
					{
						mem_shipBase.Nowhp = this.F_Data.StartHp.get_Item(i);
					}
					else if (mem_shipBase.Nowhp <= 0)
					{
						this.deleteTargetShip.Add(mem_ship);
					}
					int num6 = 0;
					int num7 = 0;
					dictionary.TryGetValue(mem_shipBase.Level - 1, ref num6);
					dictionary.TryGetValue(mem_shipBase.Level + 1, ref num7);
					mem_ship.SetRequireExp(mem_shipBase.Level, dictionary);
					mem_ship.SumLovToBattle(rank, flag2, mvp, this.F_Data.StartHp.get_Item(i), mem_ship.Nowhp);
					mem_shipBase.Lov = mem_ship.Lov;
					mem_ship.Set_ShipParam(mem_shipBase, mst_ship, false);
				}
			}
		}

		private void updateShipPracticeEnemy(SerializableDictionary<int, int> getShipExp, ref SerializableDictionary<int, List<int>> lvupInfo)
		{
			List<Mem_ship> shipData = this.E_Data.ShipData;
			Dictionary<int, int> mst_level = this.mst_shiplevel;
			using (List<Mem_ship>.Enumerator enumerator = shipData.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mem_ship current = enumerator.get_Current();
					Mem_ship mem_ship = Comm_UserDatas.Instance.User_ship.get_Item(current.Rid);
					Mem_shipBase mem_shipBase = new Mem_shipBase(mem_ship);
					Mst_ship mst_data = Mst_DataManager.Instance.Mst_ship.get_Item(mem_shipBase.Ship_id);
					int num = getShipExp[mem_ship.Rid];
					List<int> value = null;
					int levelupInfo = mem_ship.getLevelupInfo(mst_level, mem_shipBase.Level, mem_shipBase.Exp, ref num, out value);
					lvupInfo.Add(mem_ship.Rid, value);
					mem_shipBase.Level = levelupInfo;
					mem_shipBase.Exp += num;
					int num2 = levelupInfo - mem_ship.Level;
					Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary = mem_ship.Kyouka;
					for (int i = 0; i < num2; i++)
					{
						dictionary = mem_ship.getLevelupKyoukaValue(mem_ship.Ship_id, dictionary);
					}
					mem_shipBase.SetKyoukaValue(dictionary);
					mem_ship.SetRequireExp(mem_shipBase.Level, mst_level);
					mem_ship.Set_ShipParam(mem_shipBase, mst_data, false);
				}
			}
		}

		private void setCondSubValue(ref int cond)
		{
			if (!this.practiceFlag)
			{
				if (this.battleKinds == ExecBattleKinds.DayOnly || this.battleKinds == ExecBattleKinds.DayToNight || this.battleKinds == ExecBattleKinds.NithtToDay)
				{
					int num = 3;
					if (Mem_ship.Get_FatitgueState(cond) >= FatigueState.Distress)
					{
						num += 6;
					}
					cond -= num;
				}
				if (this.battleKinds == ExecBattleKinds.DayToNight || this.battleKinds == ExecBattleKinds.NightOnly || this.battleKinds == ExecBattleKinds.NithtToDay)
				{
					cond -= 2;
				}
			}
			if (0 > cond)
			{
				cond = 0;
			}
		}

		private void setCondBonus(BattleWinRankKinds rank, bool flagship, bool mvp, ref int cond)
		{
			if (rank == BattleWinRankKinds.E)
			{
				return;
			}
			int num = 0;
			switch (rank)
			{
			case BattleWinRankKinds.C:
				num = 1;
				break;
			case BattleWinRankKinds.B:
				num = 2;
				break;
			case BattleWinRankKinds.A:
				num = 3;
				break;
			case BattleWinRankKinds.S:
				num = 4;
				break;
			}
			if (flagship)
			{
				num += 3;
			}
			if (mvp)
			{
				num += 10;
			}
			cond += num;
			if (cond > 100)
			{
				cond = 100;
			}
		}

		private void deleteLostShip()
		{
			if (this.deleteTargetShip.get_Count() == 0)
			{
				return;
			}
			this.deleteTargetShip.ForEach(delegate(Mem_ship x)
			{
				this.F_Data.Deck.Ship.RemoveShip(x.Rid);
				Comm_UserDatas.Instance.User_record.AddLostShipCount();
			});
			Comm_UserDatas.Instance.Remove_Ship(this.deleteTargetShip);
		}

		protected override double getAvoHosei(Mem_ship target)
		{
			return 0.0;
		}

		public override void Dispose()
		{
		}
	}
}
