using Common.Enum;
using local.models;
using Server_Common.Formats;
using Server_Controllers;
using Server_Models;
using System;
using System.Collections.Generic;

namespace local.managers
{
	public class DutyManager : ManagerBase
	{
		private Api_req_Quest _quest;

		private List<__DutyModel__> _duties;

		public int MaxExecuteCount
		{
			get
			{
				return base.UserInfo.MaxDutyExecuteCount;
			}
		}

		public DutyManager()
		{
			this._quest = new Api_req_Quest();
			this._UpdateDutyData();
		}

		public DutyModel GetDuty(int duty_no)
		{
			return this._duties.Find((__DutyModel__ duty) => duty.No == duty_no);
		}

		public List<DutyModel> GetDutyList(bool is_sorted)
		{
			List<__DutyModel__> range = this._duties.GetRange(0, this._duties.get_Count());
			if (is_sorted)
			{
				range.Sort((__DutyModel__ x, __DutyModel__ y) => this._CompareFunc(x, y));
			}
			return range.ConvertAll<DutyModel>((__DutyModel__ duty) => duty);
		}

		public List<DutyModel> GetDutyList()
		{
			return this.GetDutyList(false);
		}

		public List<DutyModel> GetRunningDutyList()
		{
			return this.GetDutyList(true).FindAll((DutyModel duty) => duty.State == QuestState.RUNNING);
		}

		public List<DutyModel> GetExecutedDutyList()
		{
			return this.GetDutyList(true).FindAll((DutyModel duty) => duty.State == QuestState.RUNNING || duty.State == QuestState.COMPLETE);
		}

		[Obsolete("GetDutyList(is_sorted) を使用してください", false)]
		public DutyModel[] GetDuties(bool is_sorted)
		{
			return this.GetDutyList(is_sorted).ToArray();
		}

		public bool StartDuty(int duty_no)
		{
			DutyModel duty = this.GetDuty(duty_no);
			return duty != null && duty.State == QuestState.WAITING_START && this.GetExecutedDutyList().get_Count() < this.MaxExecuteCount && this._quest.Start(((__DutyModel__)duty).Fmt);
		}

		public bool Cancel(int duty_no)
		{
			DutyModel duty = this.GetDuty(duty_no);
			return duty.State == QuestState.RUNNING && this._quest.Stop(((__DutyModel__)duty).Fmt);
		}

		public List<IReward> RecieveRewards(int duty_no)
		{
			DutyModel duty = this.GetDuty(duty_no);
			if (duty.State != QuestState.COMPLETE)
			{
				return null;
			}
			List<QuestItemGetFmt> list = this._quest.ClearItemGet(((__DutyModel__)duty).Fmt);
			if (list == null)
			{
				return null;
			}
			List<IReward> list2 = new List<IReward>();
			List<IReward_Material> list3 = null;
			Reward_Useitems reward_Useitems = null;
			for (int i = 0; i < list.get_Count(); i++)
			{
				if (list.get_Item(i).Category == QuestItemGetKind.Material && list.get_Item(i).Count > 0)
				{
					if (list3 == null)
					{
						list3 = new List<IReward_Material>();
						Reward_Materials reward_Materials = new Reward_Materials(list3);
						list2.Add(reward_Materials);
					}
					list3.Add(new Reward_Material((enumMaterialCategory)list.get_Item(i).Id, list.get_Item(i).Count));
				}
				else if (list.get_Item(i).Category == QuestItemGetKind.Deck)
				{
					list2.Add(new Reward_Deck(list.get_Item(i).Id));
					base.UserInfo.__UpdateDeck__(new Api_get_Member());
				}
				else if (list.get_Item(i).Category == QuestItemGetKind.FurnitureBox)
				{
					if (reward_Useitems == null)
					{
						reward_Useitems = new Reward_Useitems();
						list2.Add(reward_Useitems);
					}
					reward_Useitems.__Add__(list.get_Item(i).Id, list.get_Item(i).Count);
				}
				else if (list.get_Item(i).Category == QuestItemGetKind.LargeBuild)
				{
					list2.Add(new Reward_LargeBuild());
				}
				else if (list.get_Item(i).Category == QuestItemGetKind.Ship)
				{
					for (int j = 0; j < list.get_Item(i).Count; j++)
					{
						list2.Add(new Reward_Ship(list.get_Item(i).Id));
					}
					base.UserInfo.__UpdateShips__(new Api_get_Member());
				}
				else if (list.get_Item(i).Category == QuestItemGetKind.SlotItem)
				{
					for (int k = 0; k < list.get_Item(i).Count; k++)
					{
						list2.Add(new Reward_Slotitem(list.get_Item(i).Id));
					}
				}
				else if (list.get_Item(i).Category == QuestItemGetKind.UseItem)
				{
					list2.Add(new Reward_Useitem(list.get_Item(i).Id, list.get_Item(i).Count));
				}
				else if (list.get_Item(i).Category == QuestItemGetKind.Furniture)
				{
					list2.Add(new Reward_Furniture(list.get_Item(i).Id));
				}
				else if (list.get_Item(i).Category == QuestItemGetKind.Exchange)
				{
					list2.Add(new Reward_Exchange_Slotitem(list.get_Item(i).FromId, list.get_Item(i).Id, list.get_Item(i).IsUseCrewItem));
					List<SlotitemModel> slotitemList = base.UserInfo.GetDeck(1).GetFlagShip().SlotitemList;
					for (int l = 0; l < slotitemList.get_Count(); l++)
					{
						if (slotitemList.get_Item(l) != null)
						{
							slotitemList.get_Item(l).__update__();
						}
					}
				}
				else if (list.get_Item(i).Category == QuestItemGetKind.Spoint)
				{
					list2.Add(new Reward_SPoint(list.get_Item(i).Count));
				}
				else if (list.get_Item(i).Category == QuestItemGetKind.DeckPractice)
				{
					list2.Add(new Reward_DeckPracitce(list.get_Item(i).Id));
				}
				else if (list.get_Item(i).Category == QuestItemGetKind.Tanker)
				{
					list2.Add(new Reward_TransportCraft(list.get_Item(i).Count));
					base._UpdateTankerManager();
				}
			}
			List<IReward> list4 = list2.FindAll((IReward reward) => reward is Reward_Furniture);
			if (list4.get_Count() > 0)
			{
				Api_Result<Dictionary<FurnitureKinds, List<Mst_furniture>>> api_Result = new Api_get_Member().FurnitureList();
				if (api_Result.state == Api_Result_State.Success)
				{
					for (int m = 0; m < list4.get_Count(); m++)
					{
						Reward_Furniture reward_Furniture = (Reward_Furniture)list4.get_Item(m);
						reward_Furniture.__Init__(api_Result.data);
					}
				}
			}
			this._UpdateDutyData();
			return list2;
		}

		private bool _UpdateDutyData()
		{
			Api_Result<List<User_QuestFmt>> api_Result = this._quest.QuestList();
			if (api_Result.state != Api_Result_State.Success || api_Result.data == null)
			{
				return false;
			}
			List<User_QuestFmt> data = api_Result.data;
			this._duties = data.ConvertAll<__DutyModel__>((User_QuestFmt fmt) => new __DutyModel__(fmt));
			return true;
		}

		private int _CompareFunc(__DutyModel__ x, __DutyModel__ y)
		{
			int num = this.__CompareState(x, y);
			if (num != 0)
			{
				return num;
			}
			return this.__CompareNo(x, y);
		}

		private int __CompareState(__DutyModel__ x, __DutyModel__ y)
		{
			return y.State - x.State;
		}

		private int __CompareNo(__DutyModel__ x, __DutyModel__ y)
		{
			return x.No - y.No;
		}

		public override string ToString()
		{
			string text = base.ToString();
			text += string.Format("  同時遂行最大数:{0}\n", this.MaxExecuteCount);
			text += "\n";
			text += string.Format("--[任務一覧]--\n", new object[0]);
			text += string.Format(" - 遂行中の任務数:{0}\n", this.GetExecutedDutyList().get_Count());
			List<DutyModel> dutyList = this.GetDutyList();
			for (int i = 0; i < dutyList.get_Count(); i++)
			{
				text += string.Format("{0}\n", dutyList.get_Item(i));
			}
			return text + string.Format("----\n", new object[0]);
		}
	}
}
