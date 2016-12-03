using Common.Enum;
using Server_Common;
using Server_Common.Formats;
using Server_Controllers.QuestLogic;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Server_Controllers
{
	public class Api_req_Quest : IQuestOperator, Mem_slotitem.IMemSlotIdOperator
	{
		private Dictionary<int, Mst_quest> mst_quest = new Dictionary<int, Mst_quest>();

		private Dictionary<int, List<int>> mst_slotitemchange = new Dictionary<int, List<int>>();

		private Dictionary<int, List<int>> mst_quest_reset = new Dictionary<int, List<int>>();

		public Api_req_Quest()
		{
			IEnumerable<XElement> enumerable = Utils.Xml_Result(Mst_quest.tableName, Mst_quest.tableName, "Id");
			using (IEnumerator<XElement> enumerator = enumerable.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					XElement current = enumerator.get_Current();
					Mst_quest mst_quest = null;
					Model_Base.SetMaster<Mst_quest>(out mst_quest, current);
					this.mst_quest.Add(mst_quest.Id, mst_quest);
				}
			}
			IEnumerable<XElement> enumerable2 = Utils.Xml_Result("mst_quest_slotitemchange", "mst_quest_slotitemchange", "Id");
			using (IEnumerator<XElement> enumerator2 = enumerable2.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					XElement current2 = enumerator2.get_Current();
					int num = int.Parse(current2.Element("Id").get_Value());
					int num2 = int.Parse(current2.Element("Old_slotitem_id").get_Value());
					int num3 = int.Parse(current2.Element("New_slotitem_id").get_Value());
					int num4 = int.Parse(current2.Element("Level_max").get_Value());
					int num5 = int.Parse(current2.Element("Use_crew").get_Value());
					Dictionary<int, List<int>> arg_179_0 = this.mst_slotitemchange;
					int arg_179_1 = num;
					List<int> list = new List<int>();
					list.Add(num2);
					list.Add(num3);
					list.Add(num4);
					list.Add(num5);
					arg_179_0.Add(arg_179_1, list);
				}
			}
			IEnumerable<XElement> enumerable3 = Utils.Xml_Result("mst_questcount_reset", "mst_questcount_reset", string.Empty);
			Dictionary<int, List<int>> dictionary = new Dictionary<int, List<int>>();
			dictionary.Add(1, new List<int>());
			dictionary.Add(2, new List<int>());
			dictionary.Add(3, new List<int>());
			dictionary.Add(4, new List<int>());
			this.mst_quest_reset = dictionary;
			using (IEnumerator<XElement> enumerator3 = enumerable3.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					XElement current3 = enumerator3.get_Current();
					int num6 = int.Parse(current3.Element("Type").get_Value());
					if (num6 != 0)
					{
						int num7 = int.Parse(current3.Element("Id").get_Value());
						this.mst_quest_reset.get_Item(num6).Add(num7);
					}
				}
			}
		}

		public Api_req_Quest(Api_TurnOperator tInstance)
		{
			IEnumerable<XElement> enumerable = Utils.Xml_Result(Mst_quest.tableName, Mst_quest.tableName, "Id");
			using (IEnumerator<XElement> enumerator = enumerable.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					XElement current = enumerator.get_Current();
					Mst_quest mst_quest = null;
					Model_Base.SetMaster<Mst_quest>(out mst_quest, current);
					this.mst_quest.Add(mst_quest.Id, mst_quest);
				}
			}
			IEnumerable<XElement> enumerable2 = Utils.Xml_Result("mst_questcount_reset", "mst_questcount_reset", string.Empty);
			Dictionary<int, List<int>> dictionary = new Dictionary<int, List<int>>();
			dictionary.Add(1, new List<int>());
			dictionary.Add(2, new List<int>());
			dictionary.Add(3, new List<int>());
			dictionary.Add(4, new List<int>());
			this.mst_quest_reset = dictionary;
			using (IEnumerator<XElement> enumerator2 = enumerable2.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					XElement current2 = enumerator2.get_Current();
					int num = int.Parse(current2.Element("Type").get_Value());
					if (num != 0)
					{
						int num2 = int.Parse(current2.Element("Id").get_Value());
						this.mst_quest_reset.get_Item(num).Add(num2);
					}
				}
			}
		}

		void Mem_slotitem.IMemSlotIdOperator.ChangeSlotId(Mem_slotitem obj, int changeId)
		{
			obj.ChangeSlotId(this, changeId);
			obj.ChangeExperience(-obj.Experience);
			obj.ChangeExperience(Mst_DataManager.Instance.Mst_Slotitem.get_Item(changeId).Default_exp);
		}

		public void EnforceQuestReset()
		{
			if (Comm_UserDatas.Instance.User_quest.get_Count() == 0)
			{
				return;
			}
			if (!Comm_UserDatas.Instance.User_turn.ReqQuestReset)
			{
				return;
			}
			this.QuestReset();
			Comm_UserDatas.Instance.User_turn.DisableQuestReset();
			this.mst_quest.Clear();
			this.mst_quest = null;
			this.mst_quest_reset.Clear();
			this.mst_quest_reset = null;
		}

		public Api_Result<List<User_QuestFmt>> QuestList()
		{
			if (Comm_UserDatas.Instance.User_quest.get_Count() == 0)
			{
				Comm_UserDatas.Instance.InitQuest(this, Enumerable.ToList<Mst_quest>(this.mst_quest.get_Values()));
			}
			if (Comm_UserDatas.Instance.User_turn.ReqQuestReset)
			{
				this.QuestReset();
				Comm_UserDatas.Instance.User_turn.DisableQuestReset();
			}
			this.SetEnableList();
			Api_Result<List<User_QuestFmt>> api_Result = new Api_Result<List<User_QuestFmt>>();
			IEnumerable<Mem_quest> enumerable = Enumerable.Where<Mem_quest>(Enumerable.OrderBy<Mem_quest, int>(Comm_UserDatas.Instance.User_quest.get_Values(), (Mem_quest member) => member.Rid), (Mem_quest member) => member.State != QuestState.END && member.State != QuestState.NOT_DISP);
			api_Result.data = new List<User_QuestFmt>();
			int num = Comm_UserDatas.Instance.User_deck.get_Item(1).Ship[0];
			Mem_ship flagShip = Comm_UserDatas.Instance.User_ship.get_Item(num);
			using (IEnumerator<Mem_quest> enumerator = enumerable.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mem_quest current = enumerator.get_Current();
					Mst_quest mstObj = this.mst_quest.get_Item(current.Rid);
					User_QuestFmt user_QuestFmt = new User_QuestFmt(current, mstObj);
					this.slotModelChangeQuestNormalize(flagShip, current, mstObj, user_QuestFmt);
					api_Result.data.Add(user_QuestFmt);
				}
			}
			return api_Result;
		}

		public bool Start(User_QuestFmt fmt)
		{
			Mem_quest mem_quest = null;
			if (!Comm_UserDatas.Instance.User_quest.TryGetValue(fmt.No, ref mem_quest))
			{
				return false;
			}
			if (mem_quest.State != QuestState.WAITING_START)
			{
				return false;
			}
			mem_quest.StateChange(this, QuestState.RUNNING);
			fmt.State = mem_quest.State;
			if (fmt.Category == 1)
			{
				QuestHensei questHensei = new QuestHensei(fmt.No);
				List<int> list = questHensei.ExecuteCheck();
				if (list.Contains(fmt.No))
				{
					fmt.State = QuestState.COMPLETE;
				}
			}
			return true;
		}

		public bool Stop(User_QuestFmt fmt)
		{
			Mem_quest mem_quest = null;
			if (!Comm_UserDatas.Instance.User_quest.TryGetValue(fmt.No, ref mem_quest))
			{
				return false;
			}
			if (mem_quest.State != QuestState.RUNNING)
			{
				return false;
			}
			mem_quest.StateChange(this, QuestState.WAITING_START);
			fmt.State = mem_quest.State;
			return true;
		}

		public List<QuestItemGetFmt> ClearItemGet(User_QuestFmt fmt)
		{
			Mem_quest mem_quest = null;
			if (!Comm_UserDatas.Instance.User_quest.TryGetValue(fmt.No, ref mem_quest))
			{
				return null;
			}
			if (mem_quest.State != QuestState.COMPLETE)
			{
				return null;
			}
			mem_quest.StateChange(this, QuestState.END);
			Mst_questcount mst_questcount = null;
			if (Mst_DataManager.Instance.Mst_questcount.TryGetValue(mem_quest.Rid, ref mst_questcount))
			{
				using (Dictionary<int, bool>.Enumerator enumerator = mst_questcount.Reset_flag.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<int, bool> current = enumerator.get_Current();
						if (current.get_Value())
						{
							Mem_questcount mem_questcount = null;
							if (Comm_UserDatas.Instance.User_questcount.TryGetValue(current.get_Key(), ref mem_questcount))
							{
								mem_questcount.Reset(true);
							}
						}
					}
				}
			}
			Mst_quest mst_quest = this.mst_quest.get_Item(fmt.No);
			List<QuestItemGetFmt> list = new List<QuestItemGetFmt>();
			using (Dictionary<enumMaterialCategory, int>.Enumerator enumerator2 = fmt.GetMaterial.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					KeyValuePair<enumMaterialCategory, int> current2 = enumerator2.get_Current();
					if (current2.get_Key() <= enumMaterialCategory.Bauxite)
					{
						list.Add(this._ItemGet(current2.get_Key(), current2.get_Value()));
					}
				}
			}
			int slotModelChangeId = mst_quest.GetSlotModelChangeId(1);
			int slotModelChangeId2 = mst_quest.GetSlotModelChangeId(2);
			bool useCrewFlag = false;
			bool maxExpFlag = false;
			if (slotModelChangeId > 0)
			{
				maxExpFlag = mst_quest.IsLevelMax(this.mst_slotitemchange.get_Item(slotModelChangeId));
				useCrewFlag = mst_quest.IsUseCrew(this.mst_slotitemchange.get_Item(slotModelChangeId));
			}
			list.Add(this._ItemGet(mst_quest.Get_1_type, mst_quest.Get_1_id, mst_quest.Get_1_count, maxExpFlag, useCrewFlag));
			bool useCrewFlag2 = false;
			bool maxExpFlag2 = false;
			if (slotModelChangeId2 > 0)
			{
				maxExpFlag2 = mst_quest.IsLevelMax(this.mst_slotitemchange.get_Item(slotModelChangeId2));
				useCrewFlag2 = mst_quest.IsUseCrew(this.mst_slotitemchange.get_Item(slotModelChangeId2));
			}
			list.Add(this._ItemGet(mst_quest.Get_2_type, mst_quest.Get_2_id, mst_quest.Get_2_count, maxExpFlag2, useCrewFlag2));
			list = list.FindAll((QuestItemGetFmt item) => item != null);
			return list;
		}

		public bool Debug_StateChange(int no)
		{
			Mem_quest mem_quest = null;
			if (!Comm_UserDatas.Instance.User_quest.TryGetValue(no, ref mem_quest))
			{
				return false;
			}
			if (mem_quest.State != QuestState.RUNNING)
			{
				return false;
			}
			mem_quest.StateChange(this, QuestState.COMPLETE);
			Mst_questcount mst_questcount = null;
			if (!Mst_DataManager.Instance.Mst_questcount.TryGetValue(mem_quest.Rid, ref mst_questcount))
			{
				return true;
			}
			using (Dictionary<int, int>.Enumerator enumerator = mst_questcount.Clear_num.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<int, int> current = enumerator.get_Current();
					Mem_questcount mem_questcount = null;
					if (!Comm_UserDatas.Instance.User_questcount.TryGetValue(current.get_Key(), ref mem_questcount))
					{
						mem_questcount = new Mem_questcount(current.get_Key(), current.get_Value());
						Comm_UserDatas.Instance.User_questcount.Add(current.get_Key(), mem_questcount);
					}
					else
					{
						int num = current.get_Value() - mem_questcount.Value;
						if (num > 0)
						{
							mem_questcount.AddCount(num);
						}
					}
				}
			}
			return true;
		}

		public bool Debug_CompleteToRunningChange(int no)
		{
			Mem_quest mem_quest = null;
			if (!Comm_UserDatas.Instance.User_quest.TryGetValue(no, ref mem_quest))
			{
				return false;
			}
			if (mem_quest.State != QuestState.COMPLETE)
			{
				return false;
			}
			mem_quest.StateChange(this, QuestState.RUNNING);
			return true;
		}

		private void QuestReset()
		{
			List<int> list = null;
			HashSet<int> hashSet = null;
			this.setResetType(out list, out hashSet);
			using (Dictionary<int, Mst_quest>.ValueCollection.Enumerator enumerator = this.mst_quest.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mst_quest current = enumerator.get_Current();
					if (current.Type != 1)
					{
						Mem_quest mem_quest = null;
						if (Comm_UserDatas.Instance.User_quest.TryGetValue(current.Id, ref mem_quest))
						{
							if (mem_quest.State != QuestState.NOT_DISP && list.Contains(current.Type))
							{
								if (current.Torigger == 0)
								{
									mem_quest.StateChange(this, QuestState.WAITING_START);
								}
								else
								{
									Mst_quest mst_quest = null;
									if (!this.mst_quest.TryGetValue(current.Torigger, ref mst_quest))
									{
										mem_quest.StateChange(this, QuestState.NOT_DISP);
									}
									else if (mst_quest.Type != 1)
									{
										mem_quest.StateChange(this, QuestState.NOT_DISP);
									}
									else
									{
										mem_quest.StateChange(this, QuestState.WAITING_START);
									}
								}
							}
						}
					}
				}
			}
			using (HashSet<int>.Enumerator enumerator2 = hashSet.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					int current2 = enumerator2.get_Current();
					List<int> list2 = this.mst_quest_reset.get_Item(current2);
					using (List<int>.Enumerator enumerator3 = list2.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							int current3 = enumerator3.get_Current();
							Mem_questcount mem_questcount = null;
							if (Comm_UserDatas.Instance.User_questcount.TryGetValue(current3, ref mem_questcount))
							{
								mem_questcount.Reset(true);
							}
						}
					}
				}
			}
		}

		private void setResetType(out List<int> reset_type, out HashSet<int> reset_counter_type)
		{
			reset_type = new List<int>();
			reset_counter_type = new HashSet<int>();
			DateTime dateTime = Comm_UserDatas.Instance.User_turn.GetDateTime();
			reset_type.Add(2);
			reset_counter_type.Add(1);
			if (dateTime.get_DayOfWeek() == null)
			{
				reset_type.Add(3);
				reset_counter_type.Add(2);
			}
			reset_type.Add(4);
			reset_type.Add(5);
			if (dateTime.get_Day() == 1)
			{
				reset_type.Add(6);
				reset_counter_type.Add(3);
			}
			if (dateTime.get_Month() == 1 && dateTime.get_Day() == 1)
			{
				reset_type.Add(7);
				reset_counter_type.Add(4);
			}
		}

		private void SetEnableList()
		{
			if (Enumerable.Count<int>(this.mst_quest.get_Keys()) != Enumerable.Count<int>(Comm_UserDatas.Instance.User_quest.get_Keys()))
			{
				IEnumerable<int> enumerable = Enumerable.Except<int>(this.mst_quest.get_Keys(), Comm_UserDatas.Instance.User_quest.get_Keys());
				using (IEnumerator<int> enumerator = enumerable.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						int current = enumerator.get_Current();
						int category = this.mst_quest.get_Item(current).Category;
						Mem_quest mem_quest = new Mem_quest(current, category, QuestState.NOT_DISP);
						Comm_UserDatas.Instance.User_quest.Add(mem_quest.Rid, mem_quest);
					}
				}
			}
			int num = Comm_UserDatas.Instance.User_turn.GetDateTime().get_Day() % 10;
			using (Dictionary<int, Mem_quest>.ValueCollection.Enumerator enumerator2 = Comm_UserDatas.Instance.User_quest.get_Values().GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					Mem_quest current2 = enumerator2.get_Current();
					if (this.mst_quest.get_Item(current2.Rid).Type == 4 && num != 0 && num != 3 && num != 7)
					{
						current2.StateChange(this, QuestState.NOT_DISP);
					}
					else if (this.mst_quest.get_Item(current2.Rid).Type == 5 && num != 2 && num != 8)
					{
						current2.StateChange(this, QuestState.NOT_DISP);
					}
					else if (current2.State == QuestState.NOT_DISP)
					{
						if (this.specialToriggerCheck(this.mst_quest.get_Item(current2.Rid)))
						{
							if (this.mst_quest.get_Item(current2.Rid).Sub_torigger != 0)
							{
								Mem_quest mem_quest2 = null;
								if (!Comm_UserDatas.Instance.User_quest.TryGetValue(this.mst_quest.get_Item(current2.Rid).Sub_torigger, ref mem_quest2))
								{
									continue;
								}
								if (mem_quest2.State != QuestState.END)
								{
									continue;
								}
							}
							Mem_quest mem_quest3 = null;
							if (!Comm_UserDatas.Instance.User_quest.TryGetValue(this.mst_quest.get_Item(current2.Rid).Torigger, ref mem_quest3))
							{
								if (this.mst_quest.get_Item(current2.Rid).Torigger == 0)
								{
									current2.StateChange(this, QuestState.WAITING_START);
								}
							}
							else if (mem_quest3.State == QuestState.END)
							{
								current2.StateChange(this, QuestState.WAITING_START);
							}
						}
					}
				}
			}
		}

		private void slotModelChangeQuestNormalize(Mem_ship flagShip, Mem_quest mem_quest, Mst_quest mst_quest, User_QuestFmt changeFmt)
		{
			HashSet<int> hashSet = new HashSet<int>();
			if (mem_quest.State == QuestState.COMPLETE)
			{
				if (mst_quest.Get_1_type == 15)
				{
					hashSet.Add(mst_quest.Get_1_id);
				}
				if (mst_quest.Get_2_type == 15)
				{
					hashSet.Add(mst_quest.Get_2_id);
				}
			}
			if (hashSet.get_Count() == 0)
			{
				return;
			}
			Mst_questcount mst_questcount = null;
			Mst_DataManager.Instance.Mst_questcount.TryGetValue(mem_quest.Rid, ref mst_questcount);
			HashSet<int> hashSet2 = new HashSet<int>();
			bool flag = false;
			using (HashSet<int>.Enumerator enumerator = hashSet.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int current = enumerator.get_Current();
					int mst_id = this.mst_slotitemchange.get_Item(current).get_Item(0);
					bool maxFlag = mst_quest.IsLevelMax(this.mst_slotitemchange.get_Item(current));
					if (mst_quest.IsUseCrew(this.mst_slotitemchange.get_Item(current)))
					{
						flag = true;
					}
					int num = this.findModelChangeEnableSlotPos(flagShip.Slot, mst_id, maxFlag);
					hashSet2.Add(num);
				}
			}
			if (flag)
			{
				int num2 = 0;
				Mem_useitem mem_useitem = null;
				if (Comm_UserDatas.Instance.User_useItem.TryGetValue(70, ref mem_useitem))
				{
					num2 = mem_useitem.Value;
				}
				if (num2 == 0)
				{
					hashSet2.Add(-1);
				}
			}
			List<string> requireShip = QuestKousyou.GetRequireShip(mem_quest.Rid);
			if (requireShip.get_Count() > 0)
			{
				string yomi = Mst_DataManager.Instance.Mst_ship.get_Item(flagShip.Ship_id).Yomi;
				if (!Enumerable.Any<string>(requireShip, (string x) => x.Equals(yomi)))
				{
					hashSet2.Add(-1);
				}
			}
			if (hashSet2.Contains(-1))
			{
				mem_quest.StateChange(this, QuestState.WAITING_START);
				changeFmt.State = mem_quest.State;
				if (mst_questcount == null)
				{
					return;
				}
				using (HashSet<int>.Enumerator enumerator2 = mst_questcount.Counter_id.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						int current2 = enumerator2.get_Current();
						Mem_questcount mem_questcount = null;
						if (Comm_UserDatas.Instance.User_questcount.TryGetValue(current2, ref mem_questcount))
						{
							mem_questcount.Reset(false);
						}
					}
				}
			}
			else if (hashSet2.Contains(-2))
			{
				changeFmt.InvalidFlag = true;
			}
		}

		private QuestItemGetFmt _ItemGet(enumMaterialCategory material_category, int count)
		{
			QuestItemGetFmt questItemGetFmt = new QuestItemGetFmt();
			questItemGetFmt.Category = QuestItemGetKind.Material;
			questItemGetFmt.Id = (int)material_category;
			questItemGetFmt.Count = count;
			if (questItemGetFmt.Category == QuestItemGetKind.Material)
			{
				enumMaterialCategory id = (enumMaterialCategory)questItemGetFmt.Id;
				Comm_UserDatas.Instance.User_material.get_Item(id).Add_Material(questItemGetFmt.Count);
			}
			return questItemGetFmt;
		}

		private QuestItemGetFmt _ItemGet(int type, int id, int count, bool maxExpFlag, bool useCrewFlag)
		{
			QuestItemGetFmt questItemGetFmt = new QuestItemGetFmt();
			questItemGetFmt.Category = (QuestItemGetKind)type;
			questItemGetFmt.Id = id;
			questItemGetFmt.Count = count;
			questItemGetFmt.IsUseCrewItem = useCrewFlag;
			if (questItemGetFmt.Category == QuestItemGetKind.Material)
			{
				enumMaterialCategory id2 = (enumMaterialCategory)questItemGetFmt.Id;
				Comm_UserDatas.Instance.User_material.get_Item(id2).Add_Material(questItemGetFmt.Count);
			}
			else if (questItemGetFmt.Category == QuestItemGetKind.Deck)
			{
				Comm_UserDatas.Instance.Add_Deck(questItemGetFmt.Id);
			}
			else if (questItemGetFmt.Category == QuestItemGetKind.LargeBuild)
			{
				Comm_UserDatas.Instance.User_basic.OpenLargeDock();
			}
			else if (questItemGetFmt.Category == QuestItemGetKind.Ship)
			{
				Comm_UserDatas.Instance.Add_Ship(questItemGetFmt.createIds());
			}
			else if (questItemGetFmt.Category == QuestItemGetKind.SlotItem)
			{
				Comm_UserDatas.Instance.Add_Slot(questItemGetFmt.createIds());
			}
			else if (questItemGetFmt.Category == QuestItemGetKind.FurnitureBox || questItemGetFmt.Category == QuestItemGetKind.UseItem)
			{
				if (questItemGetFmt.Id == 63)
				{
					questItemGetFmt.IsQuestExtend = Comm_UserDatas.Instance.User_basic.QuestExtend(Mst_DataManager.Instance.Mst_const);
				}
				else
				{
					Comm_UserDatas.Instance.Add_Useitem(questItemGetFmt.Id, questItemGetFmt.Count);
				}
			}
			else if (questItemGetFmt.Category == QuestItemGetKind.Furniture)
			{
				Comm_UserDatas.Instance.Add_Furniture(questItemGetFmt.Id);
			}
			else if (questItemGetFmt.Category == QuestItemGetKind.Exchange)
			{
				questItemGetFmt.FromId = this.mst_slotitemchange.get_Item(id).get_Item(0);
				questItemGetFmt.Id = this.mst_slotitemchange.get_Item(id).get_Item(1);
				int num = Comm_UserDatas.Instance.User_deck.get_Item(1).Ship[0];
				Mem_ship mem_ship = Comm_UserDatas.Instance.User_ship.get_Item(num);
				int num2 = this.findModelChangeEnableSlotPos(mem_ship.Slot, questItemGetFmt.FromId, maxExpFlag);
				if (num2 < 0)
				{
					return null;
				}
				Mem_slotitem obj = Comm_UserDatas.Instance.User_slot.get_Item(mem_ship.Slot.get_Item(num2));
				((Mem_slotitem.IMemSlotIdOperator)this).ChangeSlotId(obj, questItemGetFmt.Id);
				Mem_shipBase baseData = new Mem_shipBase(mem_ship);
				mem_ship.Set_ShipParam(baseData, Mst_DataManager.Instance.Mst_ship.get_Item(mem_ship.Ship_id), false);
				Mem_useitem mem_useitem;
				if (questItemGetFmt.IsUseCrewItem && Comm_UserDatas.Instance.User_useItem.TryGetValue(70, ref mem_useitem))
				{
					mem_useitem.Sub_UseItem(1);
				}
			}
			else if (questItemGetFmt.Category == QuestItemGetKind.Spoint)
			{
				Comm_UserDatas.Instance.User_basic.AddPoint(questItemGetFmt.Count);
			}
			else if (questItemGetFmt.Category == QuestItemGetKind.DeckPractice)
			{
				Comm_UserDatas.Instance.User_deckpractice.StateChange((DeckPracticeType)questItemGetFmt.Id, true);
			}
			else
			{
				if (questItemGetFmt.Category != QuestItemGetKind.Tanker)
				{
					return null;
				}
				Comm_UserDatas.Instance.Add_Tanker(questItemGetFmt.Count);
			}
			return questItemGetFmt;
		}

		private int findModelChangeEnableSlotPos(List<int> slot_rids, int mst_id, bool maxFlag)
		{
			int result = -1;
			for (int i = 0; i < slot_rids.get_Count(); i++)
			{
				int num = slot_rids.get_Item(i);
				if (num > 0)
				{
					Mem_slotitem mem_slotitem = Comm_UserDatas.Instance.User_slot.get_Item(num);
					if (mem_slotitem.Slotitem_id == mst_id)
					{
						if (!maxFlag || mem_slotitem.IsMaxSkillLevel())
						{
							if (!mem_slotitem.Lock)
							{
								return i;
							}
							result = -2;
						}
					}
				}
			}
			return result;
		}

		private bool specialToriggerCheck(Mst_quest mst_record)
		{
			if (mst_record.Id == 423)
			{
				DifficultKind difficult = Comm_UserDatas.Instance.User_basic.Difficult;
				return (difficult == DifficultKind.KOU || difficult == DifficultKind.SHI) && this.isRequireShipLimit(mst_record.Get_1_id, 1);
			}
			return true;
		}

		private bool isRequireShipLimit(int search_id, int limitNum)
		{
			Dictionary<int, Mst_ship> m_ship = Mst_DataManager.Instance.Mst_ship;
			string target = m_ship.get_Item(search_id).Yomi;
			int num = Enumerable.Count<Mem_ship>(Comm_UserDatas.Instance.User_ship.get_Values(), (Mem_ship x) => m_ship.get_Item(x.Ship_id).Yomi.Equals(target));
			return limitNum >= num;
		}
	}
}
