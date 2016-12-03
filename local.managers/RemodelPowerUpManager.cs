using Common.Struct;
using local.models;
using Server_Controllers;
using Server_Models;
using System;
using System.Collections.Generic;

namespace local.managers
{
	public abstract class RemodelPowerUpManager : RemodelManagerBase
	{
		private ShipModel _powup_target_ship;

		private Dictionary<int, Mem_slotitem> _slotitems;

		private List<ShipModel> _material_candidate_ships;

		public ShipModel PowupTargetShip
		{
			get
			{
				return this._powup_target_ship;
			}
			set
			{
				if (this._powup_target_ship != value)
				{
					this._powup_target_ship = value;
					this._UpdateCandidateShips();
				}
			}
		}

		public RemodelPowerUpManager(int area_id) : base(area_id)
		{
			this._UpdateCandidateShips();
		}

		public ShipModel[] GetCandidateShips(List<ShipModel> material_ships)
		{
			return this._GetCandidateShip(material_ships).ToArray();
		}

		public ShipModel[] GetCandidateShips(List<ShipModel> material_ships, int page_no, int count_in_page, out int count)
		{
			List<ShipModel> list = this._GetCandidateShip(material_ships);
			count = list.get_Count();
			int num = (page_no - 1) * count_in_page;
			num = Math.Max(num, 0);
			num = Math.Min(num, list.get_Count());
			int num2 = list.get_Count() - num;
			num2 = Math.Max(num2, 0);
			num2 = Math.Min(num2, count_in_page);
			return list.GetRange(num, num2).ToArray();
		}

		public PowUpInfo getPowUpInfo(List<ShipModel> material_ships)
		{
			PowUpInfo result = default(PowUpInfo);
			if (this._powup_target_ship == null)
			{
				return result;
			}
			HashSet<int> hashSet = new HashSet<int>();
			for (int i = 0; i < material_ships.get_Count(); i++)
			{
				ShipModel shipModel = material_ships.get_Item(i);
				if (shipModel != null)
				{
					result.Karyoku += shipModel.PowUpKaryoku;
					result.Raisou += shipModel.PowUpRaisou;
					result.Taiku += shipModel.PowUpTaikuu;
					result.Soukou += shipModel.PowUpSoukou;
					result.Lucky += shipModel.PowUpLucky;
					hashSet.Add(shipModel.MemId);
				}
			}
			result.Karyoku = (int)((double)result.Karyoku * 1.2 + 0.3);
			result.Raisou = (int)((double)result.Raisou * 1.2 + 0.3);
			result.Taiku = (int)((double)result.Taiku * 1.2 + 0.3);
			result.Soukou = (int)((double)result.Soukou * 1.2 + 0.3);
			result.Lucky = (int)((double)result.Lucky * 1.2 + 0.3);
			Api_req_Kaisou api_req_Kaisou = new Api_req_Kaisou();
			result.Taikyu += api_req_Kaisou.GetSameShipPowerupTaikyu(this._powup_target_ship.MemId, hashSet);
			result.Lucky += api_req_Kaisou.GetSameShipPowerupLuck(this._powup_target_ship.MemId, hashSet);
			int num = this._powup_target_ship.MaxHp + result.Taikyu;
			int taik_max = Mst_DataManager.Instance.Mst_ship.get_Item(this._powup_target_ship.MstId).Taik_max;
			int num2 = Math.Min(num, taik_max);
			int num3 = num2 - this._powup_target_ship.MaxHp;
			result.Taikyu = Math.Max(num3, 0);
			result.Karyoku = Math.Min(this._powup_target_ship.KaryokuMax - this._powup_target_ship.Karyoku, result.Karyoku);
			result.Raisou = Math.Min(this._powup_target_ship.RaisouMax - this._powup_target_ship.Raisou, result.Raisou);
			result.Taiku = Math.Min(this._powup_target_ship.TaikuMax - this._powup_target_ship.Taiku, result.Taiku);
			result.Soukou = Math.Min(this._powup_target_ship.SoukouMax - this._powup_target_ship.Soukou, result.Soukou);
			result.Lucky = Math.Min(this._powup_target_ship.LuckyMax - this._powup_target_ship.Lucky, result.Lucky);
			result.RemoveNegative();
			return result;
		}

		public bool IsValidPowerUp(List<ShipModel> material_ships)
		{
			if (this._powup_target_ship == null)
			{
				return false;
			}
			if (material_ships == null)
			{
				return false;
			}
			if (material_ships.FindAll((ShipModel ship) => ship != null).get_Count() <= 0)
			{
				return false;
			}
			if (this.getPowUpInfo(material_ships).IsAllZero())
			{
				return false;
			}
			DeckModelBase deck = this._powup_target_ship.getDeck();
			return deck == null || !deck.IsActionEnd();
		}

		public ShipModel PowerUp(List<ShipModel> material_ships, out bool great_success)
		{
			great_success = false;
			if (this._powup_target_ship == null)
			{
				return null;
			}
			if (material_ships == null)
			{
				return null;
			}
			HashSet<int> hashSet = new HashSet<int>();
			for (int i = 0; i < material_ships.get_Count(); i++)
			{
				if (material_ships.get_Item(i) != null)
				{
					hashSet.Add(material_ships.get_Item(i).MemId);
				}
			}
			if (material_ships.FindAll((ShipModel ship) => ship != null).get_Count() != hashSet.get_Count())
			{
				return null;
			}
			Api_Result<int> api_Result = new Api_req_Kaisou().Powerup(this._powup_target_ship.MemId, hashSet);
			base.UserInfo.__UpdateShips__(new Api_get_Member());
			base._UpdateOtherShips();
			this._UpdateCandidateShips();
			this._powup_target_ship = base.UserInfo.GetShip(this._powup_target_ship.MemId);
			this._slotitems = null;
			if (api_Result.state == Api_Result_State.Success)
			{
				if (api_Result.data == 2)
				{
					great_success = true;
					return this._powup_target_ship;
				}
				if (api_Result.data == 1)
				{
					return this._powup_target_ship;
				}
			}
			return null;
		}

		private List<ShipModel> _GetCandidateShip(List<ShipModel> MaterialShips)
		{
			return this._material_candidate_ships.FindAll((ShipModel ship) => MaterialShips.IndexOf(ship) == -1);
		}

		private void _UpdateCandidateShips()
		{
			if (this._powup_target_ship == null)
			{
				this._material_candidate_ships = new List<ShipModel>();
				return;
			}
			if (this._slotitems == null)
			{
				Api_Result<Dictionary<int, Mem_slotitem>> api_Result = new Api_get_Member().Slotitem();
				if (api_Result.state == Api_Result_State.Success && api_Result.data != null)
				{
					this._slotitems = api_Result.data;
				}
			}
			this._material_candidate_ships = base.UserInfo.__GetShipList__();
			List<int> ship_member_ids = base.UserInfo.__GetShipMemIdInAllDecks__();
			this._material_candidate_ships = this._material_candidate_ships.FindAll((ShipModel ship) => !ship.IsLocked() && !ship.__HasLocked__(this._slotitems) && this._powup_target_ship.MemId != ship.MemId && !ship_member_ids.Contains(ship.MemId) && !ship.IsInRepair() && !ship.IsBling() && (!ship.IsBlingWait() || ship.AreaIdBeforeBlingWait == this.AreaId));
		}

		public string ToString(List<ShipModel> MaterialShips)
		{
			string text = string.Empty;
			text += base.ToString();
			text += "\n";
			if (this.PowupTargetShip == null)
			{
				text += string.Format("対象艦: 未設定\n", new object[0]);
			}
			else
			{
				text += string.Format("対象艦: {0}\n", this.PowupTargetShip.ShortName);
			}
			text += "[--餌艦--]\n";
			for (int i = 0; i < MaterialShips.get_Count(); i++)
			{
				ShipModel shipModel = MaterialShips.get_Item(i);
				if (shipModel != null)
				{
					text += string.Format("{0} 餌艦効果(火:{1} 雷:{2} 空:{3} 装:{4})\n", new object[]
					{
						shipModel.ShortName,
						shipModel.PowUpKaryoku,
						shipModel.PowUpRaisou,
						shipModel.PowUpTaikuu,
						shipModel.PowUpSoukou
					});
				}
				else
				{
					text += string.Format("- - -\n", new object[0]);
				}
			}
			if (this._powup_target_ship != null)
			{
				PowUpInfo powUpInfo = this.getPowUpInfo(MaterialShips);
				text += "対象艦のステータス\n";
				if (powUpInfo.Taikyu > 0)
				{
					text += string.Format("耐久:{0}->{1}(+{2}) ", this.PowupTargetShip.MaxHp, this.PowupTargetShip.MaxHp + powUpInfo.Taikyu, powUpInfo.Taikyu);
				}
				text += string.Format("火力:{0}->{1}(+{2}) 雷装:{3}->{4}(+{5}) 対空:{6}->{7}(+{8}) 装甲:{9}->{10}(+{11}) 運:{12}->{13}(+{14})", new object[]
				{
					this.PowupTargetShip.Karyoku,
					this.PowupTargetShip.Karyoku + powUpInfo.Karyoku,
					powUpInfo.Karyoku,
					this.PowupTargetShip.Raisou,
					this.PowupTargetShip.Raisou + powUpInfo.Raisou,
					powUpInfo.Raisou,
					this.PowupTargetShip.Taiku,
					this.PowupTargetShip.Taiku + powUpInfo.Taiku,
					powUpInfo.Taiku,
					this.PowupTargetShip.Soukou,
					this.PowupTargetShip.Soukou + powUpInfo.Soukou,
					powUpInfo.Soukou,
					this.PowupTargetShip.Lucky,
					this.PowupTargetShip.Lucky + powUpInfo.Lucky,
					powUpInfo.Lucky
				});
			}
			return text;
		}
	}
}
