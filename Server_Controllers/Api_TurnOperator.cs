using Common.Enum;
using Server_Common;
using Server_Common.Formats;
using Server_Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Server_Controllers
{
	public class Api_TurnOperator
	{
		private Mem_turn turnInstance;

		private Mem_basic basicInstance;

		private Api_get_Member getInstance;

		private Random randInstance;

		public Api_TurnOperator()
		{
			this.turnInstance = Comm_UserDatas.Instance.User_turn;
			this.basicInstance = Comm_UserDatas.Instance.User_basic;
			this.getInstance = new Api_get_Member();
			this.randInstance = new Random();
		}

		public TurnWorkResult ExecTurnStateChange(ITurnOperator instance, bool force, TurnState state)
		{
			if (instance == null)
			{
				return null;
			}
			if (force)
			{
				return this.ExecOwnEndWork();
			}
			if (state == TurnState.CONTINOUS)
			{
				return null;
			}
			if (state == TurnState.TURN_START)
			{
				return this.ExecTurnStartWork();
			}
			if (state == TurnState.OWN_END)
			{
				return this.ExecOwnEndWork();
			}
			if (state == TurnState.ENEMY_START)
			{
				return this.ExecEnemyStartWork();
			}
			if (state == TurnState.ENEMY_END)
			{
				return this.ExecEnemyEndWork();
			}
			if (state == TurnState.TURN_END)
			{
				return this.ExecTurnEndWork();
			}
			return null;
		}

		private TurnWorkResult ExecTurnStartWork()
		{
			IEnumerable<int> enumerable = Enumerable.Select<Mem_ndock, int>(Enumerable.Where<Mem_ndock>(Comm_UserDatas.Instance.User_ndock.get_Values(), (Mem_ndock ndock) => !ndock.IsRecoverEndTime() && ndock.State == NdockStates.RESTORE), (Mem_ndock ndock) => ndock.Ship_id);
			HashSet<int> hashSet = new HashSet<int>(enumerable);
			IEnumerable<int> enumerable2 = Enumerable.Select<Mem_deck, int>(Comm_UserDatas.Instance.User_deck.get_Values(), (Mem_deck x) => x.Ship[0]);
			HashSet<int> hashSet2 = new HashSet<int>(enumerable2);
			using (Dictionary<int, Mem_ship>.ValueCollection.Enumerator enumerator = Comm_UserDatas.Instance.User_ship.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mem_ship current = enumerator.get_Current();
					current.AddTurnRecoveryCond(this, 10);
					bool flag = current.SumLovToTurnStart(hashSet.Contains(current.Rid), hashSet2.Contains(current.Rid));
				}
			}
			TurnWorkResult turnWorkResult = new TurnWorkResult();
			turnWorkResult.ChangeState = TurnState.CONTINOUS;
			turnWorkResult.TransportMaterial = new Dictionary<enumMaterialCategory, int>();
			Dictionary<enumMaterialCategory, int> dictionary = new Dictionary<enumMaterialCategory, int>();
			using (IEnumerator enumerator2 = Enum.GetValues(typeof(enumMaterialCategory)).GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					object current2 = enumerator2.get_Current();
					turnWorkResult.TransportMaterial.Add((enumMaterialCategory)((int)current2), 0);
					dictionary.Add((enumMaterialCategory)((int)current2), 0);
				}
			}
			this.TakeMaterial(ref turnWorkResult.TransportMaterial, ref dictionary);
			this.setSummaryMaterialInfo(dictionary, out turnWorkResult.BonusMaterialMonthly, out turnWorkResult.BonusMaterialWeekly);
			using (Dictionary<int, Mem_deck>.ValueCollection.Enumerator enumerator3 = Comm_UserDatas.Instance.User_deck.get_Values().GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					Mem_deck current3 = enumerator3.get_Current();
					if (current3.MissionState == MissionStates.NONE)
					{
						this.repairShipAutoRecovery(current3.Ship);
					}
				}
			}
			this.getInstance.Deck_Port();
			turnWorkResult.MissionEndDecks = Enumerable.ToList<Mem_deck>(Enumerable.Where<Mem_deck>(Comm_UserDatas.Instance.User_deck.get_Values(), (Mem_deck x) => x.MissionState == MissionStates.END || (x.MissionState == MissionStates.STOP && x.CompleteTime == 0)));
			this.ExecBling_Ship(out turnWorkResult.BlingEndShip);
			this.ExecBling_EscortDeck(out turnWorkResult.BlingEndEscortDeck);
			this.ExecBling_Tanker(out turnWorkResult.BlingEndTanker);
			this.setTurnRewardItem(out turnWorkResult.SpecialItem);
			this.updateRewardItem(turnWorkResult.SpecialItem);
			return turnWorkResult;
		}

		private void TakeMaterial(ref Dictionary<enumMaterialCategory, int> add_mat, ref Dictionary<enumMaterialCategory, int> summaryBase)
		{
			IEnumerable<IGrouping<int, Mem_tanker>> areaEnableTanker = Mem_tanker.GetAreaEnableTanker(Comm_UserDatas.Instance.User_tanker);
			if (Enumerable.Count<IGrouping<int, Mem_tanker>>(areaEnableTanker) == 0)
			{
				return;
			}
			using (IEnumerator<IGrouping<int, Mem_tanker>> enumerator = areaEnableTanker.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IGrouping<int, Mem_tanker> current = enumerator.get_Current();
					Mst_maparea mst_maparea = Mst_DataManager.Instance.Mst_maparea.get_Item(current.get_Key());
					DeckShips ship = Comm_UserDatas.Instance.User_EscortDeck.get_Item(current.get_Key()).Ship;
					mst_maparea.TakeMaterialNum(Comm_UserDatas.Instance.User_mapclear, Enumerable.Count<Mem_tanker>(current), ref add_mat, false, ship);
					mst_maparea.TakeMaterialNum(Comm_UserDatas.Instance.User_mapclear, Enumerable.Count<Mem_tanker>(current), ref summaryBase, true, ship);
				}
			}
			int materialMaxNum = Comm_UserDatas.Instance.User_basic.GetMaterialMaxNum();
			using (Dictionary<enumMaterialCategory, int>.Enumerator enumerator2 = add_mat.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					KeyValuePair<enumMaterialCategory, int> current2 = enumerator2.get_Current();
					int num = 0;
					int num2 = Comm_UserDatas.Instance.User_material.get_Item(current2.get_Key()).Value + current2.get_Value();
					int num3 = materialMaxNum - num2;
					if (num3 >= 0)
					{
						num = current2.get_Value();
					}
					else if (materialMaxNum > Comm_UserDatas.Instance.User_material.get_Item(current2.get_Key()).Value)
					{
						num = materialMaxNum - Comm_UserDatas.Instance.User_material.get_Item(current2.get_Key()).Value;
					}
					Comm_UserDatas.Instance.User_material.get_Item(current2.get_Key()).Add_Material(num);
				}
			}
		}

		private void setSummaryMaterialInfo(Dictionary<enumMaterialCategory, int> baseInfo, out Dictionary<enumMaterialCategory, int> monthlyInfo, out Dictionary<enumMaterialCategory, int> weeklyInfo)
		{
			monthlyInfo = null;
			weeklyInfo = null;
			DateTime dateTime = Comm_UserDatas.Instance.User_turn.GetDateTime();
			if (dateTime.get_Day() == 1)
			{
				double keisu = 5.0;
				double tani = 10.0;
				HashSet<enumMaterialCategory> hashSet = new HashSet<enumMaterialCategory>();
				hashSet.Add(enumMaterialCategory.Fuel);
				hashSet.Add(enumMaterialCategory.Bull);
				hashSet.Add(enumMaterialCategory.Steel);
				hashSet.Add(enumMaterialCategory.Bauxite);
				HashSet<enumMaterialCategory> takeTarget = hashSet;
				monthlyInfo = this.takeBonusMaterial(baseInfo, takeTarget, keisu, tani);
				int num = this.takeBonusDevKit();
				monthlyInfo.Add(enumMaterialCategory.Dev_Kit, num);
			}
			if (dateTime.get_DayOfWeek() == null)
			{
				double keisu = 2.5;
				double tani = 5.0;
				double randDouble = Utils.GetRandDouble(0.0, 2.0, 1.0, 1);
				enumMaterialCategory enumMaterialCategory;
				if (randDouble == 0.0)
				{
					enumMaterialCategory = enumMaterialCategory.Fuel;
				}
				else if (randDouble == 1.0)
				{
					enumMaterialCategory = enumMaterialCategory.Bull;
				}
				else
				{
					enumMaterialCategory = enumMaterialCategory.Steel;
				}
				HashSet<enumMaterialCategory> hashSet = new HashSet<enumMaterialCategory>();
				hashSet.Add(enumMaterialCategory);
				HashSet<enumMaterialCategory> takeTarget2 = hashSet;
				weeklyInfo = this.takeBonusMaterial(baseInfo, takeTarget2, keisu, tani);
			}
		}

		private Dictionary<enumMaterialCategory, int> takeBonusMaterial(Dictionary<enumMaterialCategory, int> baseData, HashSet<enumMaterialCategory> takeTarget, double keisu, double tani)
		{
			Dictionary<enumMaterialCategory, int> dictionary = new Dictionary<enumMaterialCategory, int>();
			Mst_item_limit mst_item_limit = Mst_DataManager.Instance.Mst_item_limit.get_Item(1);
			using (HashSet<enumMaterialCategory>.Enumerator enumerator = takeTarget.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumMaterialCategory current = enumerator.get_Current();
					int materialLimit = mst_item_limit.GetMaterialLimit(Mst_DataManager.Instance.Mst_item_limit, current);
					double num = (double)baseData.get_Item(current) * keisu;
					int num2 = (int)(Math.Ceiling(num) / tani);
					int num3 = (int)(tani * (double)num2);
					int num4 = num3 + Comm_UserDatas.Instance.User_material.get_Item(current).Value;
					int num5 = materialLimit - num4;
					if (num5 < 0)
					{
						num3 = materialLimit - Comm_UserDatas.Instance.User_material.get_Item(current).Value;
						if (num3 < 0)
						{
							num3 = 0;
						}
					}
					dictionary.Add(current, num3);
					Comm_UserDatas.Instance.User_material.get_Item(current).Add_Material(num3);
				}
			}
			return dictionary;
		}

		private int takeBonusDevKit()
		{
			Dictionary<int, Mst_maparea>.ValueCollection values = Mst_DataManager.Instance.Mst_maparea.get_Values();
			Comm_UserDatas instance = Comm_UserDatas.Instance;
			int num = 0;
			using (Dictionary<int, Mst_maparea>.ValueCollection.Enumerator enumerator = values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mst_maparea current = enumerator.get_Current();
					int mapinfo_no = Mst_maparea.MaxMapNum(Comm_UserDatas.Instance.User_basic.Difficult, current.Id);
					int num2 = Mst_mapinfo.ConvertMapInfoId(current.Id, mapinfo_no);
					Mem_mapclear mem_mapclear = null;
					if (instance.User_mapclear.TryGetValue(num2, ref mem_mapclear))
					{
						if (mem_mapclear.State == MapClearState.Cleard)
						{
							num++;
						}
					}
				}
			}
			int num3 = num + 4;
			int value = Comm_UserDatas.Instance.User_material.get_Item(enumMaterialCategory.Dev_Kit).Value;
			int num4 = value + num3;
			int materialLimit = Mst_DataManager.Instance.Mst_item_limit.get_Item(1).GetMaterialLimit(Mst_DataManager.Instance.Mst_item_limit, enumMaterialCategory.Dev_Kit);
			if (num4 > materialLimit)
			{
				num3 = materialLimit - value;
				if (num3 < 0)
				{
					num3 = 0;
				}
			}
			Comm_UserDatas.Instance.User_material.get_Item(enumMaterialCategory.Dev_Kit).Add_Material(num3);
			return num3;
		}

		private void ExecBling_Ship(out List<int> out_fmt)
		{
			out_fmt = new List<int>();
			using (Dictionary<int, Mem_ship>.Enumerator enumerator = Comm_UserDatas.Instance.User_ship.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<int, Mem_ship> current = enumerator.get_Current();
					if (current.get_Value().IsBlingShip() && current.get_Value().BlingTerm())
					{
						out_fmt.Add(current.get_Key());
					}
				}
			}
		}

		private void ExecBling_EscortDeck(out List<int> out_fmt)
		{
			out_fmt = new List<int>();
			using (Dictionary<int, Mem_esccort_deck>.Enumerator enumerator = Comm_UserDatas.Instance.User_EscortDeck.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<int, Mem_esccort_deck> current = enumerator.get_Current();
					if (current.get_Value().BlingTerm())
					{
						out_fmt.Add(current.get_Key());
					}
				}
			}
		}

		private void ExecBling_Tanker(out Dictionary<int, List<Mem_tanker>> out_fmt)
		{
			out_fmt = new Dictionary<int, List<Mem_tanker>>();
			IEnumerable<IGrouping<int, Mem_tanker>> enumerable = Enumerable.GroupBy(Enumerable.Select(Enumerable.Where<Mem_tanker>(Comm_UserDatas.Instance.User_tanker.get_Values(), (Mem_tanker tanker) => tanker.BlingTerm()), (Mem_tanker tanker) => new
			{
				tanker = tanker,
				area_id = ((tanker.Disposition_status != DispositionStatus.NONE) ? tanker.Maparea_id : 0)
			}), <>__TranspIdent1 => <>__TranspIdent1.area_id, <>__TranspIdent1 => <>__TranspIdent1.tanker);
			using (IEnumerator<IGrouping<int, Mem_tanker>> enumerator = enumerable.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IGrouping<int, Mem_tanker> current = enumerator.get_Current();
					out_fmt.Add(current.get_Key(), Enumerable.ToList<Mem_tanker>(current));
				}
			}
		}

		private void repairShipAutoRecovery(DeckShips deck)
		{
			int num = 86;
			int num2 = 19;
			List<Mem_ship> list = deck.getMemShip();
			if (Enumerable.Count<Mem_ship>(list) == 0)
			{
				return;
			}
			if (list.get_Item(0).Stype != num2)
			{
				return;
			}
			DamageState damageState = list.get_Item(0).Get_DamageState();
			if (damageState == DamageState.Tyuuha || damageState == DamageState.Taiha)
			{
				return;
			}
			if (list.get_Item(0).ExistsNdock())
			{
				return;
			}
			if (list.get_Item(0).IsBlingShip())
			{
				return;
			}
			Mem_material mem_material = Comm_UserDatas.Instance.User_material.get_Item(enumMaterialCategory.Fuel);
			Mem_material mem_material2 = Comm_UserDatas.Instance.User_material.get_Item(enumMaterialCategory.Steel);
			if (mem_material.Value == 0 && mem_material2.Value == 0)
			{
				return;
			}
			Mem_ship arg_C7_0 = list.get_Item(0);
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(num);
			Dictionary<int, int> mstSlotItemNum_OrderId = arg_C7_0.GetMstSlotItemNum_OrderId(hashSet);
			int num3 = mstSlotItemNum_OrderId.get_Item(num);
			list = Enumerable.ToList<Mem_ship>(Enumerable.Take<Mem_ship>(list, num3 + 2));
			using (List<Mem_ship>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mem_ship current = enumerator.get_Current();
					if (current.Nowhp < current.Maxhp)
					{
						DamageState damageState2 = current.Get_DamageState();
						if (damageState2 != DamageState.Tyuuha && damageState2 != DamageState.Taiha)
						{
							if (!current.ExistsNdock())
							{
								if (!current.IsBlingShip())
								{
									int ndockTimeSpan = current.GetNdockTimeSpan();
									int num4 = ndockTimeSpan * 30;
									int num5 = 30;
									double num6 = (double)num5 / (double)num4;
									Dictionary<enumMaterialCategory, int> ndockMaterialNum = current.GetNdockMaterialNum();
									int num7 = (int)Math.Ceiling((double)ndockMaterialNum.get_Item(enumMaterialCategory.Fuel) * num6);
									int num8 = (int)Math.Ceiling((double)ndockMaterialNum.get_Item(enumMaterialCategory.Steel) * num6);
									if (mem_material.Value >= num7 && mem_material2.Value >= num8)
									{
										double num9 = (double)(current.Maxhp - current.Nowhp) * num6;
										int num10 = (num9 >= 1.0) ? ((int)num9) : ((int)Math.Ceiling(num9));
										current.SubHp(-num10);
										mem_material.Sub_Material(num7);
										mem_material2.Sub_Material(num8);
									}
								}
							}
						}
					}
				}
			}
		}

		private void setTurnRewardItem(out List<ItemGetFmt> turnReward)
		{
			turnReward = null;
			List<ItemGetFmt> list = new List<ItemGetFmt>();
			DateTime dateTime = this.turnInstance.GetDateTime();
			if (dateTime.get_Month() == 2 && dateTime.get_Day() == 14)
			{
				list.Add(new ItemGetFmt
				{
					Category = ItemGetKinds.UseItem,
					Id = 56,
					Count = 1
				});
			}
			if (list.get_Count() > 0)
			{
				turnReward = list;
			}
		}

		private void updateRewardItem(List<ItemGetFmt> turnReward)
		{
			if (turnReward == null)
			{
				return;
			}
			turnReward.ForEach(delegate(ItemGetFmt x)
			{
				if (x.Category == ItemGetKinds.UseItem)
				{
					Comm_UserDatas.Instance.Add_Useitem(x.Id, x.Count);
				}
			});
		}

		private TurnWorkResult ExecOwnEndWork()
		{
			return new TurnWorkResult
			{
				ChangeState = TurnState.ENEMY_START
			};
		}

		private TurnWorkResult ExecEnemyStartWork()
		{
			return new TurnWorkResult
			{
				ChangeState = TurnState.ENEMY_END
			};
		}

		private TurnWorkResult ExecEnemyEndWork()
		{
			return new TurnWorkResult
			{
				ChangeState = TurnState.TURN_END
			};
		}

		private TurnWorkResult ExecTurnEndWork()
		{
			TurnWorkResult turnWorkResult = new TurnWorkResult();
			turnWorkResult.ChangeState = TurnState.TURN_START;
			this.turnInstance.AddTurn(this);
			if (this.turnInstance.GetDateTime().get_Day() == 1)
			{
				new Api_req_Quest(this).EnforceQuestReset();
			}
			if (Utils.IsTurnOver())
			{
				Mem_history mem_history = new Mem_history();
				mem_history.SetGameOverToTurn(this.turnInstance.Total_turn);
				Comm_UserDatas.Instance.Add_History(mem_history);
			}
			using (Dictionary<int, Mem_ship>.ValueCollection.Enumerator enumerator = Comm_UserDatas.Instance.User_ship.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mem_ship current = enumerator.get_Current();
					current.BlingWaitToStart();
					current.PurgeLovTouchData();
				}
			}
			Comm_UserDatas.Instance.UpdateEscortShipLocale();
			Comm_UserDatas.Instance.UpdateDeckShipLocale();
			List<Mem_deck> list = Enumerable.ToList<Mem_deck>(Comm_UserDatas.Instance.User_deck.get_Values());
			list.ForEach(delegate(Mem_deck x)
			{
				x.ActionStart();
			});
			List<Mst_radingtype> types = Mst_DataManager.Instance.Mst_RadingType.get_Item((int)this.basicInstance.Difficult);
			Mst_radingtype radingRecord = Mst_radingtype.GetRadingRecord(types, this.turnInstance.Total_turn);
			HashSet<int> hashSet = new HashSet<int>();
			if (radingRecord != null)
			{
				double randDouble = Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if ((double)radingRecord.Rading_rate >= randDouble)
				{
					IEnumerable<IGrouping<int, Mem_tanker>> areaEnableTanker = Mem_tanker.GetAreaEnableTanker(Comm_UserDatas.Instance.User_tanker);
					Dictionary<int, RadingKind> radingArea = this.getRadingArea(areaEnableTanker, radingRecord.Rading_type);
					if (radingArea.get_Count() > 0)
					{
						Dictionary<int, List<Mem_tanker>> tankers = Enumerable.ToDictionary<IGrouping<int, Mem_tanker>, int, List<Mem_tanker>>(areaEnableTanker, (IGrouping<int, Mem_tanker> x) => x.get_Key(), (IGrouping<int, Mem_tanker> y) => Enumerable.ToList<Mem_tanker>(y));
						turnWorkResult.RadingResult = this.getRadingResult(radingRecord.Rading_type, radingArea, tankers);
					}
				}
			}
			this.updateRadingEscortShipExp(turnWorkResult.RadingResult);
			if (this.basicInstance.Difficult == DifficultKind.KOU || this.basicInstance.Difficult == DifficultKind.SHI)
			{
				this.addRebellionPoint();
				this.selectRegistanceArea();
			}
			else
			{
				this.selectRegistanceArea();
				this.addRebellionPoint();
			}
			return turnWorkResult;
		}

		private void updateRadingEscortShipExp(List<RadingResultData> radingData)
		{
			HashSet<int> radingArea = new HashSet<int>();
			if (radingData != null && radingData.get_Count() > 0)
			{
				radingData.ForEach(delegate(RadingResultData x)
				{
					radingArea.Add(x.AreaId);
				});
			}
			Dictionary<int, int> mstLevel = ArrayMaster.GetMstLevel();
			using (Dictionary<int, Mem_esccort_deck>.ValueCollection.Enumerator enumerator = Comm_UserDatas.Instance.User_EscortDeck.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mem_esccort_deck current = enumerator.get_Current();
					if (current.Ship.Count() > 0)
					{
						int num = (!radingArea.Contains(current.Maparea_id)) ? 2 : 1;
						List<Mem_ship> memShip = current.Ship.getMemShip();
						using (List<Mem_ship>.Enumerator enumerator2 = memShip.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								Mem_ship current2 = enumerator2.get_Current();
								if (current2.IsFight() && !current2.IsBlingShip())
								{
									double num2 = Math.Sqrt((double)current2.Level);
									int num3 = 0;
									if (num == 1)
									{
										num3 = (int)(num2 * (20.0 + Utils.GetRandDouble(0.0, 80.0, 1.0, 1) + 0.5));
									}
									else
									{
										num3 = (int)(num2 * (1.0 + Utils.GetRandDouble(0.0, 3.0, 1.0, 1) + 0.5));
									}
									Mem_shipBase mem_shipBase = new Mem_shipBase(current2);
									List<int> list = new List<int>();
									int levelupInfo = current2.getLevelupInfo(mstLevel, mem_shipBase.Level, mem_shipBase.Exp, ref num3, out list);
									mem_shipBase.Level = levelupInfo;
									mem_shipBase.Exp += num3;
									int num4 = levelupInfo - current2.Level;
									Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary = current2.Kyouka;
									for (int i = 0; i < num4; i++)
									{
										dictionary = current2.getLevelupKyoukaValue(current2.Ship_id, dictionary);
									}
									mem_shipBase.SetKyoukaValue(dictionary);
									int num5 = 0;
									int num6 = 0;
									mstLevel.TryGetValue(mem_shipBase.Level - 1, ref num5);
									mstLevel.TryGetValue(mem_shipBase.Level + 1, ref num6);
									current2.SetRequireExp(mem_shipBase.Level, mstLevel);
									Mst_ship mst_data = Mst_DataManager.Instance.Mst_ship.get_Item(mem_shipBase.Ship_id);
									current2.Set_ShipParam(mem_shipBase, mst_data, false);
								}
							}
						}
					}
				}
			}
		}

		private List<RadingResultData> getRadingResult(int radingPhase, Dictionary<int, RadingKind> targetArea, Dictionary<int, List<Mem_tanker>> tankers)
		{
			DifficultKind difficult = this.basicInstance.Difficult;
			List<RadingResultData> list = new List<RadingResultData>();
			using (Dictionary<int, RadingKind>.Enumerator enumerator = targetArea.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<int, RadingKind> current = enumerator.get_Current();
					int key = current.get_Key();
					List<Mem_tanker> list2 = tankers.get_Item(key);
					Mst_radingrate mst_radingrate = Mst_DataManager.Instance.Mst_RadingRate.get_Item(key).get_Item(radingPhase);
					int count = list2.get_Count();
					List<Mem_ship> memShip = Comm_UserDatas.Instance.User_EscortDeck.get_Item(key).Ship.getMemShip();
					RadingResultData radingResultData = new RadingResultData();
					radingResultData.AreaId = key;
					radingResultData.AttackKind = current.get_Value();
					radingResultData.BeforeNum = list2.get_Count();
					List<Mem_ship> list3 = null;
					if (memShip.get_Count() > 0)
					{
						radingResultData.FlagShipMstId = memShip.get_Item(0).Ship_id;
						radingResultData.FlagShipDamageState = memShip.get_Item(0).Get_DamageState();
					}
					radingResultData.RadingDamage = this.getRadingDamage(key, current.get_Value(), mst_radingrate, memShip, out radingResultData.DeckAttackPow, out list3);
					radingResultData.BreakNum = this.getRadingTankerLostNum(current.get_Value(), memShip, count, mst_radingrate);
					IEnumerable<Mem_tanker> enumerable = Enumerable.Take<Mem_tanker>(list2, radingResultData.BreakNum);
					if (radingResultData.RadingDamage.get_Count() > 0)
					{
						List<int> list4 = new List<int>();
						using (List<Mem_ship>.Enumerator enumerator2 = memShip.GetEnumerator())
						{
							Mem_ship checkShip;
							while (enumerator2.MoveNext())
							{
								checkShip = enumerator2.get_Current();
								if (Enumerable.Any<RadingDamageData>(radingResultData.RadingDamage, (RadingDamageData x) => x.Rid == checkShip.Rid && x.Damage) && checkShip.Get_DamageState() >= DamageState.Tyuuha)
								{
									list4.Add(checkShip.Ship_id);
								}
							}
						}
						Comm_UserDatas.Instance.UpdateShipBookBrokenClothState(list4);
					}
					using (List<Mem_ship>.Enumerator enumerator3 = list3.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							Mem_ship current2 = enumerator3.get_Current();
							Comm_UserDatas.Instance.User_EscortDeck.get_Item(key).Ship.RemoveShip(current2.Rid);
							Comm_UserDatas.Instance.User_record.AddLostShipCount();
						}
					}
					Comm_UserDatas.Instance.Remove_Ship(list3);
					using (IEnumerator<Mem_tanker> enumerator4 = enumerable.GetEnumerator())
					{
						while (enumerator4.MoveNext())
						{
							Mem_tanker current3 = enumerator4.get_Current();
							Comm_UserDatas.Instance.Remove_Tanker(current3.Rid);
						}
					}
					if (radingResultData.BeforeNum > 0)
					{
						double num = (double)radingResultData.BreakNum / (double)radingResultData.BeforeNum;
						if (num >= 0.5)
						{
							int mapinfo_id = Mst_mapinfo.ConvertMapInfoId(key, 1);
							Mem_history mem_history = new Mem_history();
							mem_history.SetTanker(this.turnInstance.Total_turn, mapinfo_id, num >= 1.0);
							Comm_UserDatas.Instance.Add_History(mem_history);
						}
					}
					list.Add(radingResultData);
				}
			}
			return list;
		}

		private Dictionary<int, RadingKind> getRadingArea(IEnumerable<IGrouping<int, Mem_tanker>> tankerInfo, int radingType)
		{
			Mst_DataManager instance = Mst_DataManager.Instance;
			Dictionary<int, RadingKind> dictionary = new Dictionary<int, RadingKind>();
			using (IEnumerator<IGrouping<int, Mem_tanker>> enumerator = tankerInfo.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IGrouping<int, Mem_tanker> current = enumerator.get_Current();
					int key = current.get_Key();
					int sc = Enumerable.Count<Mem_tanker>(current);
					int req_tanker_num = instance.Mst_maparea.get_Item(key).Req_tanker_num;
					List<Mem_ship> memShip = Comm_UserDatas.Instance.User_EscortDeck.get_Item(key).Ship.getMemShip();
					int ec = 0;
					int ad1 = 0;
					int ad2 = 0;
					memShip.ForEach(delegate(Mem_ship ship)
					{
						if (ship.IsEscortDeffender())
						{
							ec++;
						}
						ad1 += ship.Taiku;
						ad2 += ship.Taisen;
					});
					Mst_radingrate radingRecord = instance.Mst_RadingRate.get_Item(key).get_Item(radingType);
					if (this.isRadingSubmarine(radingRecord, ec, ad2))
					{
						dictionary.Add(key, RadingKind.SUBMARINE_ATTACK);
					}
					else if (this.isRadingAir(radingRecord, sc, ec, ad1))
					{
						dictionary.Add(key, RadingKind.AIR_ATTACK);
					}
				}
			}
			return dictionary;
		}

		private bool isRadingSubmarine(Mst_radingrate radingRecord, int ec, int ad2)
		{
			int submarine_rate = radingRecord.Submarine_rate;
			int submarine_karyoku = radingRecord.Submarine_karyoku;
			int num = submarine_rate - ec * 2;
			double randDouble = Utils.GetRandDouble(1.0, 100.0 + Math.Sqrt((double)ad2), 1.0, 1);
			return (double)num > randDouble && (double)ec < Utils.GetRandDouble(1.0, 10.0, 1.0, 1);
		}

		private bool isRadingAir(Mst_radingrate radingRecord, int sc, int ec, int ad1)
		{
			int air_rate = radingRecord.Air_rate;
			int air_karyoku = radingRecord.Air_karyoku;
			int num = air_rate + sc - ec * 3;
			double randDouble = Utils.GetRandDouble(1.0, 100.0 + Math.Sqrt((double)ad1), 1.0, 1);
			return (double)num > randDouble && (double)ec < Utils.GetRandDouble(1.0, 10.0, 1.0, 1);
		}

		private List<RadingDamageData> getRadingDamage(int area, RadingKind kind, Mst_radingrate rateRecord, List<Mem_ship> targetShips, out int deckPow, out List<Mem_ship> deleteShips)
		{
			List<Mem_ship> list = Enumerable.ToList<Mem_ship>(targetShips);
			int ec = 0;
			double ad1 = 0.0;
			double ad2 = 0.0;
			list.ForEach(delegate(Mem_ship ship)
			{
				if (ship.IsEscortDeffender())
				{
					ec++;
				}
				Ship_GrowValues battleBaseParam = ship.GetBattleBaseParam();
				int num11 = ship.Taiku - battleBaseParam.Taiku;
				ad1 = ad1 + Math.Sqrt((double)battleBaseParam.Taiku) + (double)num11;
				int num12 = ship.Taisen - battleBaseParam.Taisen;
				ad2 = ad2 + Math.Sqrt((double)battleBaseParam.Taisen) + (double)num12;
			});
			int[] radingValues = rateRecord.GetRadingValues(kind);
			double num = (kind != RadingKind.AIR_ATTACK) ? ad2 : ad1;
			int num2 = radingValues[0];
			int num3 = radingValues[1];
			deckPow = (int)num;
			deleteShips = new List<Mem_ship>();
			if (list.get_Count() == 0)
			{
				return new List<RadingDamageData>();
			}
			RadingResultData radingResultData = new RadingResultData();
			radingResultData.DeckAttackPow = (int)num;
			double num4 = (double)num3 - Math.Sqrt((double)ec);
			int num5 = (num4 >= 1.0) ? ((int)Utils.GetRandDouble(0.0, num4, 0.1, 1)) : 0;
			List<RadingDamageData> list2 = new List<RadingDamageData>();
			Dictionary<int, DamageState> dictionary = Enumerable.ToDictionary(Enumerable.Select(list, (Mem_ship x) => new
			{
				rid = x.Rid,
				state = x.Get_DamageState()
			}), key => key.rid, val => val.state);
			for (int i = 0; i < num5; i++)
			{
				if (list.get_Count() == 0)
				{
					return list2;
				}
				RadingDamageData radingDamageData = new RadingDamageData();
				double num6 = (double)(num3 * 5) - num / 5.0 - Math.Sqrt(num);
				int num7 = (int)Utils.GetRandDouble(0.0, (double)(list.get_Count() - 1), 1.0, 1);
				Mem_ship mem_ship = list.get_Item(num7);
				radingDamageData.Rid = mem_ship.Rid;
				if (num6 <= 0.0)
				{
					radingDamageData.Damage = false;
					radingDamageData.DamageState = DamagedStates.None;
				}
				else
				{
					int taik = Mst_DataManager.Instance.Mst_ship.get_Item(mem_ship.Ship_id).Taik;
					int num8 = (int)((double)taik * Utils.GetRandDouble(1.0, num6, 1.0, 1) / 100.0) + 1;
					int num9 = mem_ship.Nowhp - num8;
					if (num9 <= 0)
					{
						if (this.basicInstance.Difficult != DifficultKind.SHI)
						{
							num9 = 1;
						}
						else if (dictionary.get_Item(mem_ship.Rid) != DamageState.Taiha)
						{
							num9 = 1;
						}
						else
						{
							num9 = 0;
						}
					}
					int num10 = mem_ship.Nowhp - num9;
					if (num10 > 0)
					{
						DamageState damageState = mem_ship.Get_DamageState();
						radingDamageData.Damage = true;
						if (num9 == 0)
						{
							int[] array = mem_ship.FindRecoveryItem();
							if (array[0] == -1)
							{
								radingDamageData.DamageState = DamagedStates.Gekichin;
								list.Remove(mem_ship);
								deleteShips.Add(mem_ship);
							}
							else
							{
								mem_ship.SubHp(num10);
								mem_ship.UseRecoveryItem(array, false);
								radingDamageData.DamageState = ((array[1] != 43) ? DamagedStates.Youin : DamagedStates.Megami);
								dictionary.set_Item(mem_ship.Rid, DamageState.Normal);
							}
						}
						else
						{
							mem_ship.SubHp(num10);
							DamageState damageState2 = mem_ship.Get_DamageState();
							if (damageState != damageState2)
							{
								if (damageState2 == DamageState.Taiha)
								{
									radingDamageData.DamageState = DamagedStates.Taiha;
								}
								else if (damageState2 == DamageState.Shouha)
								{
									radingDamageData.DamageState = DamagedStates.Shouha;
								}
								else if (damageState2 == DamageState.Tyuuha)
								{
									radingDamageData.DamageState = DamagedStates.Tyuuha;
								}
							}
							else
							{
								radingDamageData.DamageState = DamagedStates.None;
							}
						}
					}
					else
					{
						radingDamageData.Damage = false;
						radingDamageData.DamageState = DamagedStates.None;
					}
				}
				list2.Add(radingDamageData);
			}
			return list2;
		}

		private int getRadingTankerLostNum(RadingKind kind, List<Mem_ship> ships, int nowTankerNum, Mst_radingrate radingRecord)
		{
			int radingPow = radingRecord.GetRadingPow(kind);
			int num = Enumerable.Count<Mem_ship>(ships, (Mem_ship x) => x.IsEscortDeffender());
			int num2 = 0;
			if (num == 0 || Utils.GetRandDouble(1.0, (double)(12 - num), 1.0, 1) >= 6.0)
			{
				if (num == 6)
				{
					double randDouble = Utils.GetRandDouble(0.0, Math.Sqrt((double)radingPow), 0.1, 1);
					num2 = (int)(randDouble + 0.5);
				}
				else if (num == 4 || num == 5)
				{
					double randDouble2 = Utils.GetRandDouble(0.0, Math.Sqrt((double)(radingPow / 2)), 0.1, 1);
					num2 = (int)(randDouble2 + 0.5);
				}
				else if (num >= 1 && num <= 3)
				{
					double randDouble3 = Utils.GetRandDouble(0.0, Math.Sqrt((double)radingPow), 0.1, 1);
					num2 = (int)(randDouble3 + 1.5);
				}
				else if (num == 0)
				{
					return nowTankerNum;
				}
			}
			if (num2 > nowTankerNum)
			{
				return nowTankerNum;
			}
			return num2;
		}

		private void selectRegistanceArea()
		{
			using (Dictionary<int, Mem_rebellion_point>.ValueCollection.Enumerator enumerator = Comm_UserDatas.Instance.User_rebellion_point.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mem_rebellion_point current = enumerator.get_Current();
					current.StartRebellion(this);
				}
			}
		}

		private void addRebellionPoint()
		{
			RebellionUtils rebellionUtils = new RebellionUtils();
			List<Mst_maparea> list = new List<Mst_maparea>();
			double rpHitProbKeisu = this.getRpHitProbKeisu();
			if (this.turnInstance.GetDateTime().get_Day() == 1)
			{
				list = Enumerable.ToList<Mst_maparea>(Mst_DataManager.Instance.Mst_maparea.get_Values());
			}
			else
			{
				using (Dictionary<int, Mst_maparea>.ValueCollection.Enumerator enumerator = Mst_DataManager.Instance.Mst_maparea.get_Values().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mst_maparea current = enumerator.get_Current();
						double randDouble = Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
						if (randDouble <= rpHitProbKeisu)
						{
							list.Add(current);
						}
					}
				}
			}
			using (List<Mst_maparea>.Enumerator enumerator2 = list.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					Mst_maparea current2 = enumerator2.get_Current();
					int num = rebellionUtils.AddPointTo_RPTable(current2);
				}
			}
		}

		private double getRpHitProbKeisu()
		{
			DifficultKind difficult = Comm_UserDatas.Instance.User_basic.Difficult;
			if (difficult == DifficultKind.TEI)
			{
				return 5.0;
			}
			if (difficult == DifficultKind.HEI)
			{
				return 10.0;
			}
			if (difficult == DifficultKind.OTU)
			{
				return 12.0;
			}
			if (difficult == DifficultKind.KOU)
			{
				return 16.0;
			}
			return 20.0;
		}
	}
}
