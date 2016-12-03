using Common.Enum;
using local.models;
using Server_Common.Formats;
using Server_Controllers;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace local.managers
{
	public class ItemlistManager : ManagerBase
	{
		public class Result
		{
			private List<IReward> _rewards;

			private bool _limit_over;

			public IReward[] Rewards
			{
				get
				{
					return this._rewards.ToArray();
				}
			}

			public Result(List<IReward> rewards, bool limit_over)
			{
				this._rewards = rewards;
				this._limit_over = limit_over;
			}

			public bool IsLimitOver()
			{
				return this._limit_over;
			}
		}

		public const int CABINET_NO = 3;

		private Dictionary<int, List<Mst_item_shop>> _mst_cabinet;

		private List<ItemlistModel> _have_items;

		private Dictionary<ItemlistModel, Mst_item_shop> _cabinet_relations;

		private Dictionary<int, string> _descriptions;

		public List<ItemlistModel> HaveItems
		{
			get
			{
				return this._have_items;
			}
		}

		public ItemlistManager()
		{
		}

		public ItemlistManager(Dictionary<int, List<Mst_item_shop>> mst_cabinet)
		{
			this._mst_cabinet = mst_cabinet;
		}

		public virtual void Init()
		{
			this._Init(false);
		}

		public ItemlistModel GetListItem(int useitem_mst_id)
		{
			return this._have_items.Find((ItemlistModel item) => item.MstId == useitem_mst_id);
		}

		public ItemlistManager.Result UseItem(int useitem_mst_id, bool is_force, ItemExchangeKinds kinds)
		{
			Api_Result<User_ItemUseFmt> api_Result = new Api_req_Member().ItemUse(useitem_mst_id, is_force, kinds);
			if (api_Result.state != Api_Result_State.Success || api_Result.data == null)
			{
				return null;
			}
			bool flag;
			List<IReward> rewards = this._CreateRewardModels(useitem_mst_id, api_Result.data, out flag);
			this.Init();
			return new ItemlistManager.Result(rewards, api_Result.data.CautionFlag);
		}

		public ItemStoreManager CreateStoreManager()
		{
			return new ItemStoreManager(this._mst_cabinet);
		}

		protected void _Init(bool all_item)
		{
			if (this._descriptions == null)
			{
				this._descriptions = Mst_DataManager.Instance.GetUseitemText();
			}
			this._have_items = new List<ItemlistModel>();
			if (this._mst_cabinet == null)
			{
				this._mst_cabinet = Mst_DataManager.Instance.GetMstCabinet();
			}
			List<Mst_item_shop> list = this._mst_cabinet.get_Item(3);
			this._cabinet_relations = new Dictionary<ItemlistModel, Mst_item_shop>();
			Api_Result<Dictionary<int, Mem_useitem>> api_Result = new Api_get_Member().UseItem();
			if (api_Result.state == Api_Result_State.Success)
			{
				Dictionary<int, Mst_useitem> mst_useitem = Mst_DataManager.Instance.Mst_useitem;
				Dictionary<int, Mem_useitem> dictionary = (api_Result.data != null) ? api_Result.data : new Dictionary<int, Mem_useitem>();
				if (all_item)
				{
					using (Dictionary<int, Mst_useitem>.ValueCollection.Enumerator enumerator = mst_useitem.get_Values().GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Mst_useitem current = enumerator.get_Current();
							if (!(current.Name == string.Empty))
							{
								Mem_useitem mem_data = null;
								dictionary.TryGetValue(current.Id, ref mem_data);
								string description;
								this._descriptions.TryGetValue(current.Id, ref description);
								ItemlistModel tmp = new ItemlistModel(current, mem_data, description);
								this._have_items.Add(tmp);
								Mst_item_shop mst_item_shop = list.Find((Mst_item_shop item) => item.Item1_id == tmp.MstId);
								this._cabinet_relations.Add(tmp, mst_item_shop);
							}
						}
					}
					this._have_items.Sort((ItemlistModel a, ItemlistModel b) => (a.MstId <= b.MstId) ? -1 : 1);
				}
				else
				{
					using (List<Mst_item_shop>.Enumerator enumerator2 = list.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							Mst_item_shop current2 = enumerator2.get_Current();
							int num = (current2.Item1_type != 1) ? 0 : current2.Item1_id;
							Mst_useitem mst_useitem2 = null;
							mst_useitem.TryGetValue(num, ref mst_useitem2);
							Mem_useitem mem_data2 = null;
							dictionary.TryGetValue(num, ref mem_data2);
							string empty = string.Empty;
							if (mst_useitem2 != null)
							{
								this._descriptions.TryGetValue(mst_useitem2.Id, ref empty);
							}
							ItemlistModel itemlistModel = new ItemlistModel(mst_useitem2, mem_data2, empty);
							this._have_items.Add(itemlistModel);
							this._cabinet_relations.Add(itemlistModel, current2);
						}
					}
				}
				this.__UpdateCount__();
				return;
			}
			throw new Exception("Logic Error");
		}

		private List<IReward> _CreateRewardModels(int used_useitem_mst_id, User_ItemUseFmt fmt, out bool has_use_item_reward)
		{
			has_use_item_reward = false;
			List<IReward> list = new List<IReward>();
			if (fmt.Material != null && fmt.Material.get_Count() > 0)
			{
				List<IReward_Material> list2 = new List<IReward_Material>();
				Reward_Materials reward_Materials = new Reward_Materials(list2);
				list.Add(reward_Materials);
				this._AddMaterialReward(list2, enumMaterialCategory.Fuel, fmt.Material);
				this._AddMaterialReward(list2, enumMaterialCategory.Bull, fmt.Material);
				this._AddMaterialReward(list2, enumMaterialCategory.Steel, fmt.Material);
				this._AddMaterialReward(list2, enumMaterialCategory.Bauxite, fmt.Material);
				this._AddMaterialReward(list2, enumMaterialCategory.Build_Kit, fmt.Material);
				this._AddMaterialReward(list2, enumMaterialCategory.Repair_Kit, fmt.Material);
				this._AddMaterialReward(list2, enumMaterialCategory.Dev_Kit, fmt.Material);
				this._AddMaterialReward(list2, enumMaterialCategory.Revamp_Kit, fmt.Material);
			}
			if (fmt.GetItem != null && fmt.GetItem.get_Count() > 0)
			{
				using (List<ItemGetFmt>.Enumerator enumerator = fmt.GetItem.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ItemGetFmt current = enumerator.get_Current();
						has_use_item_reward = true;
						Reward_Useitem reward_Useitem = new Reward_Useitem(current.Id, current.Count);
						if (reward_Useitem.Id == 53 && used_useitem_mst_id == 53)
						{
							Reward_PortExtend reward_PortExtend = new Reward_PortExtend();
							list.Add(reward_PortExtend);
						}
						else if (reward_Useitem.Id == 63 && used_useitem_mst_id == 63)
						{
							Reward_DutyExtend reward_DutyExtend = new Reward_DutyExtend(base.UserInfo.MaxDutyExecuteCount);
							list.Add(reward_DutyExtend);
						}
						else
						{
							list.Add(reward_Useitem);
						}
					}
				}
			}
			return list;
		}

		private void _AddMaterialReward(List<IReward_Material> container, enumMaterialCategory mat_type, Dictionary<enumMaterialCategory, int> reward)
		{
			int count;
			if (reward.TryGetValue(mat_type, ref count))
			{
				Reward_Material reward_Material = new Reward_Material(mat_type, count);
				container.Add(reward_Material);
			}
		}

		private void __UpdateCount__()
		{
			Dictionary<int, Mem_slotitem> dictionary = null;
			for (int i = 0; i < this._have_items.get_Count(); i++)
			{
				ItemlistModel itemlistModel = this._have_items.get_Item(i);
				Mst_item_shop mst_cabinet = this._cabinet_relations.get_Item(itemlistModel);
				if (mst_cabinet == null || !mst_cabinet.IsChildReference() || mst_cabinet.Item2_type == 1)
				{
					itemlistModel.__SetOverrideCount__(0);
				}
				else if (mst_cabinet.Item2_type == 2)
				{
					if (dictionary == null)
					{
						dictionary = new Api_get_Member().Slotitem().data;
					}
					int value = Enumerable.Count<KeyValuePair<int, Mem_slotitem>>(dictionary, (KeyValuePair<int, Mem_slotitem> item) => item.get_Value().Slotitem_id == mst_cabinet.Item2_id);
					itemlistModel.__SetOverrideCount__(value);
				}
				else if (mst_cabinet.Item2_type == 3)
				{
					enumMaterialCategory item2_id = (enumMaterialCategory)mst_cabinet.Item2_id;
					int count = base.Material.GetCount(item2_id);
					itemlistModel.__SetOverrideCount__(count);
				}
				else
				{
					itemlistModel.__SetOverrideCount__(0);
				}
			}
		}

		public override string ToString()
		{
			string text = base.ToString();
			text += "\n";
			text += "-- 保有アイテム棚 --\n";
			for (int i = 0; i < this.HaveItems.get_Count(); i++)
			{
				text += string.Format("棚{0} - {1}{2}\n", i, this.HaveItems.get_Item(i), (!this.HaveItems.get_Item(i).IsUsable()) ? string.Empty : "[使用可能]");
			}
			return text + "\n";
		}
	}
}
