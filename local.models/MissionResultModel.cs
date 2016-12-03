using Common.Enum;
using Common.Struct;
using Server_Common.Formats;
using System;
using System.Collections.Generic;

namespace local.models
{
	public class MissionResultModel : DeckActionResultModel
	{
		private List<IReward> _rewards;

		public MissionResultKinds result
		{
			get
			{
				return this._mission_fmt.MissionResult;
			}
		}

		public string MissionName
		{
			get
			{
				return this._mission_fmt.MissionName;
			}
		}

		public int Spoint
		{
			get
			{
				return this._mission_fmt.GetSpoint;
			}
		}

		[Obsolete("GetItems()を使用してください")]
		public int ExtraItemCount
		{
			get
			{
				return (this._mission_fmt.GetItems != null) ? this._mission_fmt.GetItems.get_Count() : 0;
			}
		}

		public MissionResultModel(MissionResultFmt fmt, UserInfoModel user_info, Dictionary<int, int> exp_rates_before)
		{
			this._mission_fmt = fmt;
			this._user_info = user_info;
			this._exps = new Dictionary<int, ShipExpModel>();
			base._SetShipExp(exp_rates_before);
			this._rewards = this._InitRewardItems();
		}

		[Obsolete("GetItems()を使用してください")]
		public int GetItemID(int index)
		{
			return (this.ExtraItemCount <= index) ? -1 : this._mission_fmt.GetItems.get_Item(index).Id;
		}

		[Obsolete("GetItems()を使用してください")]
		public int GetItemCount(int index)
		{
			return (this.ExtraItemCount <= index) ? 0 : this._mission_fmt.GetItems.get_Item(index).Count;
		}

		public MaterialInfo GetMaterialInfo()
		{
			return new MaterialInfo(this._mission_fmt.GetMaterials);
		}

		public List<IReward> GetItems()
		{
			return this._rewards;
		}

		private List<IReward> _InitRewardItems()
		{
			List<IReward> list = new List<IReward>();
			int num = (this._mission_fmt.GetItems != null) ? this._mission_fmt.GetItems.get_Count() : 0;
			for (int i = 0; i < num; i++)
			{
				ItemGetFmt itemGetFmt = this._mission_fmt.GetItems.get_Item(i);
				IReward reward = null;
				if (itemGetFmt.Category == ItemGetKinds.UseItem)
				{
					reward = new Reward_Useitem(itemGetFmt.Id, itemGetFmt.Count);
				}
				else if (itemGetFmt.Category == ItemGetKinds.Ship)
				{
					reward = new Reward_Ship(itemGetFmt.Id);
				}
				else if (itemGetFmt.Category == ItemGetKinds.SlotItem)
				{
					reward = new Reward_Slotitem(itemGetFmt.Id, itemGetFmt.Count);
				}
				list.Add(reward);
			}
			return list;
		}

		public override string ToString()
		{
			string text = string.Format("==[遠征結果]==\n", new object[0]);
			text = string.Format("帰還艦隊 ID:{0}\n", base.DeckID);
			ShipModel[] ships = base.Ships;
			for (int i = 0; i < ships.Length; i++)
			{
				ShipModel shipModel = ships[i];
				ShipExpModel shipExpInfo = base.GetShipExpInfo(shipModel.MemId);
				text += string.Format(" {0}(ID:{1}) {2}", shipModel.Name, shipModel.MemId, shipExpInfo);
			}
			text += "\n";
			text += string.Format("遠征結果:{0} 遠征名:{1}\n", this.result, this.MissionName);
			text += string.Format("提督名:{0} Lv{1} [{2}] 艦隊名:{3}\n", new object[]
			{
				base.Name,
				base.Level,
				base.Rank,
				base.FleetName
			});
			MaterialInfo materialInfo = this.GetMaterialInfo();
			text += string.Format("獲得経験値:{0} 獲得資材:燃/弾/鋼/ボ {1}/{2}/{3}/{4}", new object[]
			{
				base.Exp,
				materialInfo.Fuel,
				materialInfo.Ammo,
				materialInfo.Steel,
				materialInfo.Baux
			});
			text += string.Format(" 獲得戦略ポイント:{0}\n", this.Spoint);
			List<IReward> items = this.GetItems();
			for (int j = 0; j < items.get_Count(); j++)
			{
				text += string.Format("獲得アイテム:{0}", items.get_Item(j));
			}
			return text;
		}
	}
}
