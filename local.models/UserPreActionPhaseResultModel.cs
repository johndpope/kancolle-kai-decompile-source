using Common.Enum;
using Common.Struct;
using local.managers;
using Server_Common.Formats;
using Server_Controllers;
using Server_Models;
using System;
using System.Collections.Generic;

namespace local.models
{
	public class UserPreActionPhaseResultModel : PhaseResultModel
	{
		private List<MissionResultModel> _mission_results;

		private List<ShipModel> _bling_end_ships;

		private List<EscortDeckModel> _bling_end_escort_decks;

		private Dictionary<int, int> _bling_end_tanker;

		private MaterialInfo _resources;

		private MaterialInfo _resources_monthly_bonus;

		private MaterialInfo _resources_weekly_bonus;

		private List<IReward> _rewards;

		public MaterialInfo Resources
		{
			get
			{
				return this._resources;
			}
		}

		public ShipModel[] BlingEnd_Ship
		{
			get
			{
				return this._bling_end_ships.ToArray();
			}
		}

		public EscortDeckModel[] BlingEnd_EscortDeck
		{
			get
			{
				return this._bling_end_escort_decks.ToArray();
			}
		}

		public Dictionary<int, int> BlingEnd_Tanker
		{
			get
			{
				return this._bling_end_tanker;
			}
		}

		public MissionResultModel[] MissionResults
		{
			get
			{
				return this._mission_results.ToArray();
			}
		}

		public List<IReward> Rewards
		{
			get
			{
				return this._rewards;
			}
		}

		public UserPreActionPhaseResultModel(TurnWorkResult data, ManagerBase manager) : base(data)
		{
			this._bling_end_ships = new List<ShipModel>();
			this._bling_end_escort_decks = new List<EscortDeckModel>();
			this._bling_end_tanker = new Dictionary<int, int>();
			if (this._data.BlingEndShip != null)
			{
				Api_Result<Dictionary<int, Mem_ship>> api_Result = new Api_get_Member().Ship(this._data.BlingEndShip);
				if (api_Result.state == Api_Result_State.Success)
				{
					for (int i = 0; i < this._data.BlingEndShip.get_Count(); i++)
					{
						int num = this._data.BlingEndShip.get_Item(i);
						this._bling_end_ships.Add(new ShipModel(api_Result.data.get_Item(num)));
					}
				}
			}
			if (this._data.BlingEndEscortDeck != null)
			{
				for (int j = 0; j < this._data.BlingEndEscortDeck.get_Count(); j++)
				{
					int num2 = this._data.BlingEndEscortDeck.get_Item(j);
					int area_id = num2;
					EscortDeckModel escortDeck = manager.UserInfo.GetEscortDeck(area_id);
					this._bling_end_escort_decks.Add(escortDeck);
				}
			}
			if (this._data.BlingEndTanker != null)
			{
				using (Dictionary<int, List<Mem_tanker>>.KeyCollection.Enumerator enumerator = this._data.BlingEndTanker.get_Keys().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						int current = enumerator.get_Current();
						this._bling_end_tanker.set_Item(current, this._data.BlingEndTanker.get_Item(current).get_Count());
					}
				}
			}
			this._mission_results = new List<MissionResultModel>();
			if (data.MissionEndDecks != null && data.MissionEndDecks.get_Count() > 0)
			{
				for (int k = 0; k < data.MissionEndDecks.get_Count(); k++)
				{
					int rid = data.MissionEndDecks.get_Item(k).Rid;
					DeckModel deck = manager.UserInfo.GetDeck(rid);
					ShipModel[] ships = deck.GetShips();
					Dictionary<int, int> dictionary = new Dictionary<int, int>();
					for (int l = 0; l < ships.Length; l++)
					{
						ShipModel shipModel = ships[l];
						dictionary.set_Item(shipModel.MemId, shipModel.Exp_Percentage);
					}
					Api_Result<MissionResultFmt> api_Result2 = new Api_req_Mission().Result(rid);
					if (api_Result2.state == Api_Result_State.Success)
					{
						MissionResultFmt data2 = api_Result2.data;
						this._mission_results.Add(new MissionResultModel(data2, manager.UserInfo, dictionary));
					}
				}
			}
			this._resources = new MaterialInfo(this._data.TransportMaterial);
			this._resources_monthly_bonus = new MaterialInfo(this._data.BonusMaterialMonthly);
			this._resources_weekly_bonus = new MaterialInfo(this._data.BonusMaterialWeekly);
			this._rewards = new List<IReward>();
			if (this._data.SpecialItem != null && this._data.SpecialItem.get_Count() > 0)
			{
				for (int m = 0; m < this._data.SpecialItem.get_Count(); m++)
				{
					ItemGetFmt itemGetFmt = this._data.SpecialItem.get_Item(m);
					if (itemGetFmt.Category == ItemGetKinds.Ship)
					{
						this._rewards.Add(new Reward_Ship(itemGetFmt.Id));
					}
					else if (itemGetFmt.Category == ItemGetKinds.SlotItem)
					{
						this._rewards.Add(new Reward_Slotitem(itemGetFmt.Id, itemGetFmt.Count));
					}
					else if (itemGetFmt.Category == ItemGetKinds.UseItem)
					{
						this._rewards.Add(new Reward_Useitem(itemGetFmt.Id, itemGetFmt.Count));
					}
				}
			}
		}

		public MaterialInfo GetMonthlyBonus()
		{
			return this._resources_monthly_bonus;
		}

		public MaterialInfo GetWeeklyBonus()
		{
			return this._resources_weekly_bonus;
		}

		public override string ToString()
		{
			string text = string.Format("[ユーザー事前行動フェーズ]:\n", new object[0]);
			MaterialInfo materialInfo = this.Resources;
			text += string.Format(" 資源回収:燃/弾/鋼/ボ:{0}/{1}/{2}/{3} 高速建造/高速修復/開発資材/改修資材:{4}/{5}/{6}/{7}\n", new object[]
			{
				materialInfo.Fuel,
				materialInfo.Ammo,
				materialInfo.Steel,
				materialInfo.Baux,
				materialInfo.BuildKit,
				materialInfo.RepairKit,
				materialInfo.Devkit,
				materialInfo.Revkit
			});
			materialInfo = this.GetMonthlyBonus();
			if (materialInfo.HasPositive())
			{
				text += string.Format(" 月頭ボーナス:燃/弾/鋼/ボ:{0}/{1}/{2}/{3} 高速建造/高速修復/開発資材/改修資材:{4}/{5}/{6}/{7}\n", new object[]
				{
					materialInfo.Fuel,
					materialInfo.Ammo,
					materialInfo.Steel,
					materialInfo.Baux,
					materialInfo.BuildKit,
					materialInfo.RepairKit,
					materialInfo.Devkit,
					materialInfo.Revkit
				});
			}
			materialInfo = this.GetWeeklyBonus();
			if (materialInfo.HasPositive())
			{
				text += string.Format(" 週頭ボーナス:燃/弾/鋼/ボ:{0}/{1}/{2}/{3} 高速建造/高速修復/開発資材/改修資材:{4}/{5}/{6}/{7}\n", new object[]
				{
					materialInfo.Fuel,
					materialInfo.Ammo,
					materialInfo.Steel,
					materialInfo.Baux,
					materialInfo.BuildKit,
					materialInfo.RepairKit,
					materialInfo.Devkit,
					materialInfo.Revkit
				});
			}
			if (this.Rewards != null && this.Rewards.get_Count() > 0)
			{
				text += string.Format("[特別ボーナス] ", new object[0]);
				for (int i = 0; i < this.Rewards.get_Count(); i++)
				{
					text += string.Format("{0} ", this.Rewards.get_Item(i));
				}
			}
			text += string.Format(" -- 移動完了した艦[", new object[0]);
			for (int j = 0; j < this._bling_end_ships.get_Count(); j++)
			{
				ShipModel shipModel = this._bling_end_ships.get_Item(j);
				text += string.Format("{0}(mst:{1},mem:{2})", shipModel.Name, shipModel.MstId, shipModel.MemId);
				if (j < this._bling_end_ships.get_Count() - 1)
				{
					text += ", ";
				}
			}
			text += "]\n";
			text += string.Format(" -- 移動完了した護衛艦隊[", new object[0]);
			for (int k = 0; k < this._bling_end_escort_decks.get_Count(); k++)
			{
				EscortDeckModel escortDeckModel = this._bling_end_escort_decks.get_Item(k);
				text += string.Format("海域{0}の護衛艦隊", escortDeckModel.AreaId);
				if (k < this._bling_end_escort_decks.get_Count() - 1)
				{
					text += ", ";
				}
			}
			text += "]\n";
			text += string.Format(" -- 移動完了した輸送船[", new object[0]);
			using (Dictionary<int, int>.KeyCollection.Enumerator enumerator = this._bling_end_tanker.get_Keys().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int current = enumerator.get_Current();
					text += string.Format("海域{0}に{1}隻", current, this._bling_end_tanker.get_Item(current));
					text += ", ";
				}
			}
			text += "]\n";
			if (this.MissionResults.Length > 0)
			{
				text += "\n";
				for (int l = 0; l < this.MissionResults.Length; l++)
				{
					text += string.Format("[遠征終了]:{0}\n", this.MissionResults[l]);
				}
			}
			return text;
		}
	}
}
