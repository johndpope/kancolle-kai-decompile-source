using Common.Enum;
using Common.Struct;
using Server_Common;
using Server_Common.Formats;
using Server_Controllers.QuestLogic;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server_Controllers
{
	public class Api_req_PracticeDeck
	{
		private Mem_deck mem_deck;

		private List<Mem_ship> mem_ship;

		private readonly Dictionary<DeckPracticeType, double[]> useMaterial;

		private readonly HashSet<int> submarineGroup;

		private readonly HashSet<int> motherBGroup;

		private readonly Dictionary<int, int> mstLevelUser;

		private readonly Dictionary<int, int> mstLevelShip;

		private Dictionary<DeckPracticeType, Action<PracticeDeckResultFmt>> execFunc;

		public Api_req_PracticeDeck()
		{
			Dictionary<DeckPracticeType, double[]> dictionary = new Dictionary<DeckPracticeType, double[]>();
			dictionary.Add(DeckPracticeType.Normal, new double[]
			{
				0.5,
				0.2
			});
			dictionary.Add(DeckPracticeType.Hou, new double[]
			{
				0.5,
				0.6
			});
			dictionary.Add(DeckPracticeType.Rai, new double[]
			{
				0.55,
				0.65
			});
			dictionary.Add(DeckPracticeType.Taisen, new double[]
			{
				0.55,
				0.4
			});
			dictionary.Add(DeckPracticeType.Kouku, new double[]
			{
				0.65,
				0.55
			});
			dictionary.Add(DeckPracticeType.Sougou, new double[]
			{
				0.7,
				0.7
			});
			this.useMaterial = dictionary;
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(13);
			hashSet.Add(14);
			this.submarineGroup = hashSet;
			hashSet = new HashSet<int>();
			hashSet.Add(11);
			hashSet.Add(7);
			hashSet.Add(18);
			hashSet.Add(14);
			hashSet.Add(16);
			this.motherBGroup = hashSet;
			this.mstLevelShip = Mst_DataManager.Instance.Get_MstLevel(true);
			this.mstLevelUser = Mst_DataManager.Instance.Get_MstLevel(false);
		}

		public double[] GetUseMaterialKeisu(DeckPracticeType type)
		{
			return this.useMaterial.get_Item(type);
		}

		public bool PrackticeDeckCheck(DeckPracticeType type, int deck_rid)
		{
			Mem_deck mem_deck = null;
			if (!Comm_UserDatas.Instance.User_deck.TryGetValue(deck_rid, ref mem_deck))
			{
				return false;
			}
			List<Mem_ship> memShip = mem_deck.Ship.getMemShip();
			if (memShip.get_Count() < 0)
			{
				return false;
			}
			Dictionary<int, Mst_ship> mst_shipDict = Mst_DataManager.Instance.Mst_ship;
			if (type == DeckPracticeType.Normal)
			{
				return true;
			}
			if (type == DeckPracticeType.Hou)
			{
				HashSet<int> disableItem = this.submarineGroup;
				return Enumerable.Any<Mem_ship>(memShip, (Mem_ship x) => !disableItem.Contains(x.Stype));
			}
			if (type == DeckPracticeType.Rai)
			{
				return Enumerable.Any<Mem_ship>(memShip, (Mem_ship x) => mst_shipDict.get_Item(x.Ship_id).Raig > 0);
			}
			if (type == DeckPracticeType.Taisen)
			{
				return Enumerable.Any<Mem_ship>(memShip, (Mem_ship x) => mst_shipDict.get_Item(x.Ship_id).Tais > 0);
			}
			if (type == DeckPracticeType.Kouku)
			{
				HashSet<int> enableItem = this.motherBGroup;
				return Enumerable.Any<Mem_ship>(memShip, (Mem_ship x) => enableItem.Contains(x.Stype));
			}
			return type == DeckPracticeType.Sougou && memShip.get_Count() >= 6;
		}

		public Api_Result<PracticeDeckResultFmt> GetResultData(DeckPracticeType type, int deck_rid)
		{
			Api_Result<PracticeDeckResultFmt> api_Result = new Api_Result<PracticeDeckResultFmt>();
			if (!Comm_UserDatas.Instance.User_deck.TryGetValue(deck_rid, ref this.mem_deck))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			this.mem_ship = this.mem_deck.Ship.getMemShip();
			PracticeDeckResultFmt practiceDeckResultFmt = new PracticeDeckResultFmt();
			practiceDeckResultFmt.PracticeResult.Deck = this.mem_deck;
			practiceDeckResultFmt.PracticeResult.GetSpoint = 0;
			if (this.execFunc == null)
			{
				Dictionary<DeckPracticeType, Action<PracticeDeckResultFmt>> dictionary = new Dictionary<DeckPracticeType, Action<PracticeDeckResultFmt>>();
				dictionary.Add(DeckPracticeType.Normal, new Action<PracticeDeckResultFmt>(this.getPracticeUpInfo_To_Normal));
				dictionary.Add(DeckPracticeType.Hou, new Action<PracticeDeckResultFmt>(this.getPracticeUpInfo_To_Houg));
				dictionary.Add(DeckPracticeType.Rai, new Action<PracticeDeckResultFmt>(this.getPracticeUpInfo_To_Raig));
				dictionary.Add(DeckPracticeType.Taisen, new Action<PracticeDeckResultFmt>(this.getPracticeUpInfo_To_Taisen));
				dictionary.Add(DeckPracticeType.Kouku, new Action<PracticeDeckResultFmt>(this.getPracticeUpInfo_To_Kouku));
				dictionary.Add(DeckPracticeType.Sougou, new Action<PracticeDeckResultFmt>(this.getPracticeUpInfo_To_Sougou));
				this.execFunc = dictionary;
			}
			this.execFunc.get_Item(type).Invoke(practiceDeckResultFmt);
			practiceDeckResultFmt.PracticeResult.MemberLevel = this.updateRecordLevel(practiceDeckResultFmt.PracticeResult.GetMemberExp);
			practiceDeckResultFmt.PracticeResult.LevelUpInfo = this.updateShip(type, practiceDeckResultFmt.PracticeResult.GetShipExp, practiceDeckResultFmt.PowerUpData);
			api_Result.data = practiceDeckResultFmt;
			this.mem_deck.ActionEnd();
			QuestPractice questPractice = new QuestPractice(type, practiceDeckResultFmt);
			questPractice.ExecuteCheck();
			Comm_UserDatas.Instance.User_record.UpdatePracticeCount(BattleWinRankKinds.NONE, false);
			return api_Result;
		}

		private void getPracticeUpInfo_To_Normal(PracticeDeckResultFmt fmt)
		{
			int level = this.mem_ship.get_Item(0).Level;
			double num = Math.Sqrt((double)this.mem_ship.get_Item(0).Level);
			bool flag = Mst_DataManager.Instance.Mst_stype.get_Item(this.mem_ship.get_Item(0).Stype).IsTrainingShip();
			double num2 = 10.0 + Utils.GetRandDouble(0.0, 10.0, 1.0, 1) + Utils.GetRandDouble(0.0, num, 1.0, 1);
			fmt.PracticeResult.GetMemberExp = (int)num2;
			int num3 = (!flag) ? 0 : 1;
			fmt.PracticeResult.GetShipExp = new Dictionary<int, int>();
			fmt.PowerUpData = new Dictionary<int, PowUpInfo>();
			Dictionary<int, Mst_ship> mst_ship = Mst_DataManager.Instance.Mst_ship;
			double difficultShipExpKeisu = this.getDifficultShipExpKeisu();
			double shipExpCommonKeisu = this.getShipExpCommonKeisu();
			using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mem_ship current = enumerator.get_Current();
					fmt.PracticeResult.GetShipExp.Add(current.Rid, 0);
					fmt.PowerUpData.Add(current.Rid, default(PowUpInfo));
					Mst_ship mst_ship2 = mst_ship.get_Item(current.Ship_id);
					double num4 = Math.Sqrt((double)current.Level);
					double max = (double)(14 + num3 * 7);
					double num5 = 20.0 + Utils.GetRandDouble(0.0, max, 1.0, 1) + num + num4;
					num5 = num5 * difficultShipExpKeisu * shipExpCommonKeisu;
					fmt.PracticeResult.GetShipExp.set_Item(current.Rid, (int)num5);
					PowUpInfo powUpInfo = default(PowUpInfo);
					double max2 = 1.0 + (double)num3 * 0.5 + (num + num4) / 20.0;
					powUpInfo.Kaihi = (int)Utils.GetRandDouble(0.0, max2, 1.0, 1);
					Ship_GrowValues battleBaseParam = current.GetBattleBaseParam();
					if (battleBaseParam.Kaihi + powUpInfo.Kaihi > mst_ship2.Kaih_max)
					{
						int num6 = mst_ship2.Kaih_max - mst_ship2.Kaih;
						powUpInfo.Kaihi = num6 - current.Kyouka.get_Item(Mem_ship.enumKyoukaIdx.Kaihi);
					}
					fmt.PowerUpData.set_Item(current.Rid, powUpInfo);
				}
			}
		}

		private void getPracticeUpInfo_To_Houg(PracticeDeckResultFmt fmt)
		{
			int level = this.mem_ship.get_Item(0).Level;
			double num = Math.Sqrt((double)this.mem_ship.get_Item(0).Level);
			bool flag = Mst_DataManager.Instance.Mst_stype.get_Item(this.mem_ship.get_Item(0).Stype).IsTrainingShip();
			fmt.PracticeResult.GetMemberExp = 0;
			int num2 = (!flag) ? 0 : 1;
			fmt.PracticeResult.GetShipExp = new Dictionary<int, int>();
			fmt.PowerUpData = new Dictionary<int, PowUpInfo>();
			double difficultShipExpKeisu = this.getDifficultShipExpKeisu();
			double shipExpCommonKeisu = this.getShipExpCommonKeisu();
			Dictionary<int, Mst_ship> mst_ship = Mst_DataManager.Instance.Mst_ship;
			using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mem_ship current = enumerator.get_Current();
					fmt.PracticeResult.GetShipExp.Add(current.Rid, 0);
					fmt.PowerUpData.Add(current.Rid, default(PowUpInfo));
					if (!this.submarineGroup.Contains(current.Stype))
					{
						Mst_ship mst_ship2 = mst_ship.get_Item(current.Ship_id);
						double num3 = Math.Sqrt((double)current.Level);
						double max = (double)(14 + num2 * 7);
						double num4 = 10.0 + Utils.GetRandDouble(0.0, max, 1.0, 1) + Utils.GetRandDouble(0.0, num, 1.0, 1) + num / 2.0 + num3;
						num4 = num4 * difficultShipExpKeisu * shipExpCommonKeisu;
						fmt.PracticeResult.GetShipExp.set_Item(current.Rid, (int)num4);
						PowUpInfo powUpInfo = default(PowUpInfo);
						double max2 = 1.3 + (double)num2 * 0.2 + (num + num3) / 20.0;
						powUpInfo.Karyoku = (int)Utils.GetRandDouble(0.0, max2, 1.0, 1);
						Ship_GrowValues battleBaseParam = current.GetBattleBaseParam();
						if (battleBaseParam.Houg + powUpInfo.Karyoku > mst_ship2.Houg_max)
						{
							int num5 = mst_ship2.Houg_max - mst_ship2.Houg;
							powUpInfo.Karyoku = num5 - current.Kyouka.get_Item(Mem_ship.enumKyoukaIdx.Houg);
						}
						fmt.PowerUpData.set_Item(current.Rid, powUpInfo);
					}
				}
			}
		}

		private void getPracticeUpInfo_To_Raig(PracticeDeckResultFmt fmt)
		{
			int level = this.mem_ship.get_Item(0).Level;
			double num = Math.Sqrt((double)this.mem_ship.get_Item(0).Level);
			bool flag = Mst_DataManager.Instance.Mst_stype.get_Item(this.mem_ship.get_Item(0).Stype).IsTrainingShip();
			fmt.PracticeResult.GetMemberExp = 0;
			int num2 = (!flag) ? 0 : 1;
			fmt.PracticeResult.GetShipExp = new Dictionary<int, int>();
			fmt.PowerUpData = new Dictionary<int, PowUpInfo>();
			double difficultShipExpKeisu = this.getDifficultShipExpKeisu();
			double shipExpCommonKeisu = this.getShipExpCommonKeisu();
			Dictionary<int, Mst_ship> mst_ship = Mst_DataManager.Instance.Mst_ship;
			using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mem_ship current = enumerator.get_Current();
					fmt.PracticeResult.GetShipExp.Add(current.Rid, 0);
					fmt.PowerUpData.Add(current.Rid, default(PowUpInfo));
					if (mst_ship.get_Item(current.Ship_id).Raig != 0)
					{
						Mst_ship mst_ship2 = mst_ship.get_Item(current.Ship_id);
						double num3 = Math.Sqrt((double)current.Level);
						double max = (double)(16 + num2 * 6);
						double num4 = 15.0 + Utils.GetRandDouble(0.0, max, 1.0, 1) + Utils.GetRandDouble(0.0, num, 1.0, 1) + num / 2.0 + num3;
						num4 = num4 * difficultShipExpKeisu * shipExpCommonKeisu;
						fmt.PracticeResult.GetShipExp.set_Item(current.Rid, (int)num4);
						PowUpInfo powUpInfo = default(PowUpInfo);
						double max2 = 1.3 + (double)num2 * 0.2 + (num + num3) / 20.0;
						powUpInfo.Raisou = (int)Utils.GetRandDouble(0.0, max2, 1.0, 1);
						Ship_GrowValues battleBaseParam = current.GetBattleBaseParam();
						if (battleBaseParam.Raig + powUpInfo.Raisou > mst_ship2.Raig_max)
						{
							int num5 = mst_ship2.Raig_max - mst_ship2.Raig;
							powUpInfo.Raisou = num5 - current.Kyouka.get_Item(Mem_ship.enumKyoukaIdx.Raig);
						}
						fmt.PowerUpData.set_Item(current.Rid, powUpInfo);
					}
				}
			}
		}

		private void getPracticeUpInfo_To_Taisen(PracticeDeckResultFmt fmt)
		{
			int level = this.mem_ship.get_Item(0).Level;
			double num = Math.Sqrt((double)this.mem_ship.get_Item(0).Level);
			bool flag = Mst_DataManager.Instance.Mst_stype.get_Item(this.mem_ship.get_Item(0).Stype).IsTrainingShip();
			fmt.PracticeResult.GetMemberExp = 0;
			int num2 = (!flag) ? 0 : 1;
			fmt.PracticeResult.GetShipExp = new Dictionary<int, int>();
			fmt.PowerUpData = new Dictionary<int, PowUpInfo>();
			double difficultShipExpKeisu = this.getDifficultShipExpKeisu();
			double shipExpCommonKeisu = this.getShipExpCommonKeisu();
			Dictionary<int, Mst_ship> mst_ship = Mst_DataManager.Instance.Mst_ship;
			using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mem_ship current = enumerator.get_Current();
					fmt.PracticeResult.GetShipExp.Add(current.Rid, 0);
					fmt.PowerUpData.Add(current.Rid, default(PowUpInfo));
					Mst_ship mst_ship2 = mst_ship.get_Item(current.Ship_id);
					if (mst_ship2.Tais != 0)
					{
						double num3 = Math.Sqrt((double)current.Level);
						double max = (double)(12 + num2 * 6);
						double num4 = 7.0 + Utils.GetRandDouble(0.0, max, 1.0, 1) + Utils.GetRandDouble(0.0, num, 1.0, 1) + num / 2.0 + num3;
						num4 = num4 * difficultShipExpKeisu * shipExpCommonKeisu;
						fmt.PracticeResult.GetShipExp.set_Item(current.Rid, (int)num4);
						PowUpInfo powUpInfo = default(PowUpInfo);
						double max2 = 1.4 + (double)num2 * 0.3 + (num + num3) / 20.0;
						powUpInfo.Taisen = (int)Utils.GetRandDouble(0.0, max2, 1.0, 1);
						Ship_GrowValues battleBaseParam = current.GetBattleBaseParam();
						if (battleBaseParam.Taisen + powUpInfo.Taisen > mst_ship2.Tais_max)
						{
							int num5 = mst_ship2.Tais_max - mst_ship2.Tais;
							powUpInfo.Taisen = num5 - current.Kyouka.get_Item(Mem_ship.enumKyoukaIdx.Taisen);
						}
						fmt.PowerUpData.set_Item(current.Rid, powUpInfo);
					}
				}
			}
		}

		private void getPracticeUpInfo_To_Kouku(PracticeDeckResultFmt fmt)
		{
			int level = this.mem_ship.get_Item(0).Level;
			double num = Math.Sqrt((double)this.mem_ship.get_Item(0).Level);
			bool flag = Mst_DataManager.Instance.Mst_stype.get_Item(this.mem_ship.get_Item(0).Stype).IsTrainingShip();
			fmt.PracticeResult.GetMemberExp = 0;
			int num2 = (!flag) ? 0 : 1;
			fmt.PracticeResult.GetShipExp = new Dictionary<int, int>();
			fmt.PowerUpData = new Dictionary<int, PowUpInfo>();
			double difficultShipExpKeisu = this.getDifficultShipExpKeisu();
			double shipExpCommonKeisu = this.getShipExpCommonKeisu();
			Dictionary<int, Mst_ship> mst_ship = Mst_DataManager.Instance.Mst_ship;
			using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mem_ship current = enumerator.get_Current();
					fmt.PracticeResult.GetShipExp.Add(current.Rid, 0);
					fmt.PowerUpData.Add(current.Rid, default(PowUpInfo));
					Mst_ship mst_ship2 = mst_ship.get_Item(current.Ship_id);
					if (mst_ship2.Stype != 13)
					{
						if (mst_ship2.Stype == 14)
						{
							List<Mst_slotitem> mstSlotItems = current.GetMstSlotItems();
							bool flag2 = false;
							for (int i = 0; i < mstSlotItems.get_Count(); i++)
							{
								SlotitemCategory slotitem_type = Mst_DataManager.Instance.Mst_equip_category.get_Item(mstSlotItems.get_Item(i).Type3).Slotitem_type;
								if (slotitem_type == SlotitemCategory.Kanjouki || slotitem_type == SlotitemCategory.Suijouki)
								{
									flag2 = true;
									break;
								}
							}
							if (!flag2)
							{
								continue;
							}
						}
						double num3 = Math.Sqrt((double)current.Level);
						double max = (double)(14 + num2 * 7);
						double num4 = 10.0 + Utils.GetRandDouble(0.0, max, 1.0, 1) + Utils.GetRandDouble(0.0, num, 1.0, 1) + num / 2.0 + num3;
						num4 = num4 * difficultShipExpKeisu * shipExpCommonKeisu;
						fmt.PracticeResult.GetShipExp.set_Item(current.Rid, (int)num4);
						Ship_GrowValues battleBaseParam = current.GetBattleBaseParam();
						PowUpInfo powUpInfo = default(PowUpInfo);
						double max2 = 1.5 + (double)num2 * 0.2 + (num + num3) / 20.0;
						powUpInfo.Taiku = (int)Utils.GetRandDouble(0.0, max2, 1.0, 1);
						if (battleBaseParam.Taiku + powUpInfo.Taiku > mst_ship2.Tyku_max)
						{
							int num5 = mst_ship2.Tyku_max - mst_ship2.Tyku;
							powUpInfo.Taiku = num5 - current.Kyouka.get_Item(Mem_ship.enumKyoukaIdx.Tyku);
						}
						if (this.motherBGroup.Contains(current.Stype))
						{
							double max3 = 1.2 + (num + num3) / 20.0;
							powUpInfo.Karyoku = (int)Utils.GetRandDouble(0.0, max3, 1.0, 1);
							if (battleBaseParam.Houg + powUpInfo.Karyoku > mst_ship2.Houg_max)
							{
								int num6 = mst_ship2.Houg_max - mst_ship2.Houg;
								powUpInfo.Karyoku = num6 - current.Kyouka.get_Item(Mem_ship.enumKyoukaIdx.Houg);
							}
						}
						fmt.PowerUpData.set_Item(current.Rid, powUpInfo);
					}
				}
			}
		}

		private void getPracticeUpInfo_To_Sougou(PracticeDeckResultFmt fmt)
		{
			int level = this.mem_ship.get_Item(0).Level;
			double num = Math.Sqrt((double)this.mem_ship.get_Item(0).Level);
			bool flag = Mst_DataManager.Instance.Mst_stype.get_Item(this.mem_ship.get_Item(0).Stype).IsTrainingShip();
			double num2 = 30.0 + Utils.GetRandDouble(0.0, 10.0, 1.0, 1) + num;
			fmt.PracticeResult.GetMemberExp = (int)num2;
			int num3 = (!flag) ? 0 : 1;
			fmt.PracticeResult.GetShipExp = new Dictionary<int, int>();
			fmt.PowerUpData = new Dictionary<int, PowUpInfo>();
			Dictionary<int, Mst_ship> mst_ship = Mst_DataManager.Instance.Mst_ship;
			double difficultShipExpKeisu = this.getDifficultShipExpKeisu();
			double shipExpCommonKeisu = this.getShipExpCommonKeisu();
			using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mem_ship current = enumerator.get_Current();
					fmt.PracticeResult.GetShipExp.Add(current.Rid, 0);
					fmt.PowerUpData.Add(current.Rid, default(PowUpInfo));
					Mst_ship mst_ship2 = mst_ship.get_Item(current.Ship_id);
					double num4 = Math.Sqrt((double)current.Level);
					double num5 = 40.0 + Utils.GetRandDouble(0.0, 10.0, 1.0, 1) + (double)(num3 * 10) + Utils.GetRandDouble(0.0, num, 1.0, 1) + num4;
					num5 = num5 * difficultShipExpKeisu * shipExpCommonKeisu;
					fmt.PracticeResult.GetShipExp.set_Item(current.Rid, (int)num5);
					Ship_GrowValues battleBaseParam = current.GetBattleBaseParam();
					PowUpInfo powUpInfo = default(PowUpInfo);
					double max = 1.0 + (num + num4) / 20.0;
					powUpInfo.Karyoku = (int)Utils.GetRandDouble(0.0, max, 1.0, 1);
					if (battleBaseParam.Houg + powUpInfo.Karyoku > mst_ship2.Houg_max)
					{
						int num6 = mst_ship2.Houg_max - mst_ship2.Houg;
						powUpInfo.Karyoku = num6 - current.Kyouka.get_Item(Mem_ship.enumKyoukaIdx.Houg);
					}
					double max2 = 0.7 + (num + num4) / 20.0;
					powUpInfo.Lucky = (int)Utils.GetRandDouble(0.0, max2, 1.0, 1);
					if (battleBaseParam.Luck + powUpInfo.Lucky > mst_ship2.Luck_max)
					{
						int num7 = mst_ship2.Luck_max - mst_ship2.Luck;
						powUpInfo.Lucky = num7 - current.Kyouka.get_Item(Mem_ship.enumKyoukaIdx.Luck);
					}
					fmt.PowerUpData.set_Item(current.Rid, powUpInfo);
				}
			}
		}

		private Dictionary<int, List<int>> updateShip(DeckPracticeType type, Dictionary<int, int> getShipExp, Dictionary<int, PowUpInfo> powerUp)
		{
			Dictionary<int, List<int>> dictionary = new Dictionary<int, List<int>>();
			double[] array = this.useMaterial.get_Item(type);
			using (List<Mem_ship>.Enumerator enumerator = this.mem_ship.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mem_ship current = enumerator.get_Current();
					int rid = current.Rid;
					Mem_shipBase mem_shipBase = new Mem_shipBase(current);
					Mst_ship mst_data = Mst_DataManager.Instance.Mst_ship.get_Item(mem_shipBase.Ship_id);
					int num = getShipExp.get_Item(rid);
					List<int> list = null;
					int levelupInfo = current.getLevelupInfo(this.mstLevelShip, current.Level, current.Exp, ref num, out list);
					getShipExp.set_Item(rid, num);
					if (getShipExp.get_Item(rid) != 0)
					{
						mem_shipBase.Exp += num;
						mem_shipBase.Level = levelupInfo;
					}
					dictionary.Add(rid, list);
					Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary2 = current.Kyouka;
					if (!powerUp.get_Item(current.Rid).IsAllZero())
					{
						this.addKyoukaValues(powerUp.get_Item(current.Rid), dictionary2);
					}
					int num2 = levelupInfo - current.Level;
					for (int i = 0; i < num2; i++)
					{
						dictionary2 = current.getLevelupKyoukaValue(current.Ship_id, dictionary2);
					}
					mem_shipBase.SetKyoukaValue(dictionary2);
					current.SetRequireExp(mem_shipBase.Level, this.mstLevelShip);
					mem_shipBase.Fuel = (int)((double)mem_shipBase.Fuel - (double)mem_shipBase.Fuel * array[0]);
					mem_shipBase.Bull = (int)((double)mem_shipBase.Bull - (double)mem_shipBase.Bull * array[1]);
					if (mem_shipBase.Fuel < 0)
					{
						mem_shipBase.Fuel = 0;
					}
					if (mem_shipBase.Bull < 0)
					{
						mem_shipBase.Bull = 0;
					}
					current.Set_ShipParam(mem_shipBase, mst_data, false);
				}
			}
			return dictionary;
		}

		private double getShipExpCommonKeisu()
		{
			return 9.5;
		}

		private double getDifficultShipExpKeisu()
		{
			if (Comm_UserDatas.Instance.User_basic.Difficult == DifficultKind.SHI)
			{
				return 1.2;
			}
			if (Comm_UserDatas.Instance.User_basic.Difficult == DifficultKind.KOU)
			{
				return 1.3;
			}
			if (Comm_UserDatas.Instance.User_basic.Difficult == DifficultKind.OTU)
			{
				return 1.4;
			}
			if (Comm_UserDatas.Instance.User_basic.Difficult == DifficultKind.HEI)
			{
				return 1.5;
			}
			if (Comm_UserDatas.Instance.User_basic.Difficult == DifficultKind.TEI)
			{
				return 2.0;
			}
			return 1.0;
		}

		private void addKyoukaValues(PowUpInfo powerUpValues, Dictionary<Mem_ship.enumKyoukaIdx, int> baseKyouka)
		{
			if (powerUpValues.Kaihi > 0)
			{
				Mem_ship.enumKyoukaIdx enumKyoukaIdx;
				Mem_ship.enumKyoukaIdx expr_11 = enumKyoukaIdx = Mem_ship.enumKyoukaIdx.Kaihi;
				int num = baseKyouka.get_Item(enumKyoukaIdx);
				baseKyouka.set_Item(expr_11, num + powerUpValues.Kaihi);
			}
			if (powerUpValues.Karyoku > 0)
			{
				Mem_ship.enumKyoukaIdx enumKyoukaIdx;
				Mem_ship.enumKyoukaIdx expr_3A = enumKyoukaIdx = Mem_ship.enumKyoukaIdx.Houg;
				int num = baseKyouka.get_Item(enumKyoukaIdx);
				baseKyouka.set_Item(expr_3A, num + powerUpValues.Karyoku);
			}
			if (powerUpValues.Raisou > 0)
			{
				Mem_ship.enumKyoukaIdx enumKyoukaIdx;
				Mem_ship.enumKyoukaIdx expr_64 = enumKyoukaIdx = Mem_ship.enumKyoukaIdx.Raig;
				int num = baseKyouka.get_Item(enumKyoukaIdx);
				baseKyouka.set_Item(expr_64, num + powerUpValues.Raisou);
			}
			if (powerUpValues.Taisen > 0)
			{
				Mem_ship.enumKyoukaIdx enumKyoukaIdx;
				Mem_ship.enumKyoukaIdx expr_8F = enumKyoukaIdx = Mem_ship.enumKyoukaIdx.Taisen;
				int num = baseKyouka.get_Item(enumKyoukaIdx);
				baseKyouka.set_Item(expr_8F, num + powerUpValues.Taisen);
			}
			if (powerUpValues.Taiku > 0)
			{
				Mem_ship.enumKyoukaIdx enumKyoukaIdx;
				Mem_ship.enumKyoukaIdx expr_BA = enumKyoukaIdx = Mem_ship.enumKyoukaIdx.Tyku;
				int num = baseKyouka.get_Item(enumKyoukaIdx);
				baseKyouka.set_Item(expr_BA, num + powerUpValues.Taiku);
			}
			if (powerUpValues.Lucky > 0)
			{
				Mem_ship.enumKyoukaIdx enumKyoukaIdx;
				Mem_ship.enumKyoukaIdx expr_E5 = enumKyoukaIdx = Mem_ship.enumKyoukaIdx.Luck;
				int num = baseKyouka.get_Item(enumKyoukaIdx);
				baseKyouka.set_Item(expr_E5, num + powerUpValues.Lucky);
			}
		}

		private int updateRecordLevel(int addExp)
		{
			return Comm_UserDatas.Instance.User_record.UpdateExp(addExp, this.mstLevelUser);
		}
	}
}
