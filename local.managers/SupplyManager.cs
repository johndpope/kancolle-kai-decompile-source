using Common.Enum;
using Common.Struct;
using local.models;
using local.utils;
using Server_Controllers;
using System;
using System.Collections.Generic;

namespace local.managers
{
	public class SupplyManager : ManagerBase
	{
		private int _area_id;

		private List<ShipModel> _other_ships;

		private DeckModel _selected_deck;

		private List<ShipModel> _target_ships;

		private List<int> _checked_ships;

		private int _fuel_for_supply;

		private int _ammo_for_supply;

		private CheckBoxStatus _checkbox_all_state;

		private CheckBoxStatus[] _checkbox_states;

		private SortKey _pre_sort_key;

		public MapAreaModel MapArea
		{
			get
			{
				return ManagerBase._area.get_Item(this._area_id);
			}
		}

		public DeckModel SelectedDeck
		{
			get
			{
				return this._selected_deck;
			}
		}

		public int[] CheckedShipIndices
		{
			get
			{
				return this._checked_ships.ToArray();
			}
		}

		public int FuelForSupply
		{
			get
			{
				return this._fuel_for_supply;
			}
		}

		public int AmmoForSupply
		{
			get
			{
				return this._ammo_for_supply;
			}
		}

		public ShipModel[] Ships
		{
			get
			{
				return this._target_ships.ToArray();
			}
		}

		public CheckBoxStatus CheckBoxALLState
		{
			get
			{
				return this._checkbox_all_state;
			}
		}

		public CheckBoxStatus[] CheckBoxStates
		{
			get
			{
				return this._checkbox_states;
			}
		}

		public SortKey OtherShipSortKey
		{
			get
			{
				return this._pre_sort_key;
			}
		}

		public SupplyManager(int area_id)
		{
			this._area_id = area_id;
			base._CreateMapAreaModel();
			this._target_ships = new List<ShipModel>();
			this._checked_ships = new List<int>();
			this._Initialize();
		}

		public void InitForDeck(int deck_id)
		{
			if (this._selected_deck != null && this._selected_deck.Id != deck_id)
			{
				this._checked_ships.Clear();
			}
			else if (this._selected_deck == null)
			{
				this._checked_ships.Clear();
			}
			this._selected_deck = base.UserInfo.GetDeck(deck_id);
			if (this._selected_deck != null && this._selected_deck.AreaId == this.MapArea.Id)
			{
				this._target_ships = new List<ShipModel>(this._selected_deck.GetShips());
				this._checkbox_states = new CheckBoxStatus[this._target_ships.get_Count()];
				for (int i = 0; i < this._target_ships.get_Count(); i++)
				{
					ShipModel ship = this._target_ships.get_Item(i);
					this._checkbox_states[i] = this._GetCheckBoxStatus(ship, this._selected_deck);
				}
			}
			else
			{
				this._target_ships = new List<ShipModel>();
				this._checkbox_states = new CheckBoxStatus[0];
			}
			this._CalcMaterialForSupply();
			this._SetCheckBoxAllState();
		}

		public void InitForOther()
		{
			if (this._selected_deck != null)
			{
				this._selected_deck = null;
				this._checked_ships.Clear();
			}
			int count = this._other_ships.get_Count();
			this._target_ships = this._other_ships.GetRange(0, count);
			this._checkbox_states = new CheckBoxStatus[count];
			for (int i = 0; i < count; i++)
			{
				ShipModel ship = this._other_ships.get_Item(i);
				this._checkbox_states[i] = this._GetCheckBoxStatus(ship, null);
			}
			this._CalcMaterialForSupply();
			this._SetCheckBoxAllState();
		}

		public bool ChangeSortKey(SortKey new_sort_key)
		{
			if (this._pre_sort_key != new_sort_key)
			{
				this._pre_sort_key = new_sort_key;
				this._other_ships = DeckUtil.GetSortedList(this._other_ships, this._pre_sort_key);
				if (this._selected_deck == null)
				{
					this.InitForOther();
				}
				return true;
			}
			return false;
		}

		public bool IsShowOther()
		{
			return this._selected_deck == null;
		}

		public bool IsChecked(int memID)
		{
			return this._checked_ships.Contains(memID);
		}

		public void ClickCheckBoxAll()
		{
			CheckBoxStatus checkbox_all_state = this._checkbox_all_state;
			if (checkbox_all_state == CheckBoxStatus.OFF)
			{
				for (int i = 0; i < this._target_ships.get_Count(); i++)
				{
					ShipModel shipModel = this._target_ships.get_Item(i);
					if (shipModel != null && this.CheckBoxStates[i] != CheckBoxStatus.DISABLE)
					{
						this._ForceOnCheckStatus(shipModel.MemId);
					}
				}
			}
			else if (checkbox_all_state == CheckBoxStatus.ON)
			{
				for (int j = 0; j < this._target_ships.get_Count(); j++)
				{
					ShipModel shipModel2 = this._target_ships.get_Item(j);
					if (shipModel2 != null)
					{
						this._ForceOffCheckStatus(shipModel2.MemId);
					}
				}
			}
			for (int k = 0; k < this._target_ships.get_Count(); k++)
			{
				this._checkbox_states[k] = this._GetCheckBoxStatus(this._target_ships.get_Item(k), this._selected_deck);
			}
			this._CalcMaterialForSupply();
			this._SetCheckBoxAllState();
		}

		public void ClickCheckBox(int memId)
		{
			this._ToggleCheckStatus(memId);
			this._CalcMaterialForSupply();
			if (this.IsShowOther())
			{
				this.InitForOther();
			}
			else
			{
				this.InitForDeck(this._selected_deck.Id);
			}
		}

		public bool IsValidSupply(SupplyType type)
		{
			if (type == SupplyType.Fuel)
			{
				return this.FuelForSupply > 0 && this.FuelForSupply <= base.Material.Fuel;
			}
			if (type == SupplyType.Ammo)
			{
				return this.AmmoForSupply > 0 && this.AmmoForSupply <= base.Material.Ammo;
			}
			return this.IsValidSupply(SupplyType.Fuel) || this.IsValidSupply(SupplyType.Ammo);
		}

		public bool Supply(SupplyType type, out bool use_baux)
		{
			int baux = base.Material.Baux;
			bool result;
			if (type == SupplyType.Fuel)
			{
				result = this._Supply(Api_req_Hokyu.enumHokyuType.Fuel);
			}
			else if (type == SupplyType.Ammo)
			{
				result = this._Supply(Api_req_Hokyu.enumHokyuType.Bull);
			}
			else
			{
				result = this._Supply(Api_req_Hokyu.enumHokyuType.All);
			}
			use_baux = (baux - base.Material.Baux > 0);
			return result;
		}

		private bool _Supply(Api_req_Hokyu.enumHokyuType type)
		{
			Api_Result<bool> api_Result = new Api_req_Hokyu().Charge(this._checked_ships, type);
			if (api_Result.state == Api_Result_State.Success)
			{
				this._checked_ships = new List<int>();
				for (int i = 0; i < this._target_ships.get_Count(); i++)
				{
					this._checkbox_states[i] = this._GetCheckBoxStatus(this._target_ships.get_Item(i), this._selected_deck);
				}
				this._CalcMaterialForSupply();
				this._SetCheckBoxAllState();
				return api_Result.data;
			}
			return false;
		}

		private void _CalcMaterialForSupply()
		{
			this._fuel_for_supply = (this._ammo_for_supply = 0);
			for (int i = 0; i < this._checked_ships.get_Count(); i++)
			{
				int mem_id = this._checked_ships.get_Item(i);
				ShipModel shipModel = this._GetShipModel(mem_id);
				if (shipModel != null)
				{
					MaterialInfo resourcesForSupply = shipModel.GetResourcesForSupply();
					this._fuel_for_supply += resourcesForSupply.Fuel;
					this._ammo_for_supply += resourcesForSupply.Ammo;
				}
			}
		}

		private void _SetCheckBoxAllState()
		{
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < this._checkbox_states.Length; i++)
			{
				if (this._checkbox_states[i] == CheckBoxStatus.DISABLE)
				{
					num++;
				}
				else if (this._checkbox_states[i] == CheckBoxStatus.ON)
				{
					num2++;
				}
			}
			if (this._checkbox_states.Length == num)
			{
				this._checkbox_all_state = CheckBoxStatus.DISABLE;
			}
			else if (this._checkbox_states.Length == num2 + num)
			{
				this._checkbox_all_state = CheckBoxStatus.ON;
			}
			else
			{
				this._checkbox_all_state = CheckBoxStatus.OFF;
			}
		}

		private void _ToggleCheckStatus(int memId)
		{
			if (this._checked_ships.Contains(memId))
			{
				this._checked_ships.Remove(memId);
			}
			else
			{
				ShipModel shipModel = this._GetShipModel(memId);
				if (shipModel != null)
				{
					MaterialInfo resourcesForSupply = shipModel.GetResourcesForSupply();
					if (resourcesForSupply.Fuel > 0 || resourcesForSupply.Ammo > 0)
					{
						this._checked_ships.Add(memId);
					}
				}
			}
		}

		private void _ForceOnCheckStatus(int memId)
		{
			if (!this._checked_ships.Contains(memId))
			{
				this._checked_ships.Add(memId);
			}
		}

		private void _ForceOffCheckStatus(int memId)
		{
			if (this._checked_ships.Contains(memId))
			{
				this._checked_ships.Remove(memId);
			}
		}

		private void _Initialize()
		{
			List<ShipModel> all_ships = base.UserInfo.__GetShipList__();
			this._other_ships = base.GetAreaShips(this._area_id, false, true, all_ships);
			if (this._area_id == 1)
			{
				this._other_ships.AddRange(base.GetDepotShips(all_ships));
			}
			this._other_ships = this._other_ships.FindAll((ShipModel ship) => ship.GetResourcesForSupply().HasPositive());
			this._other_ships = DeckUtil.GetSortedList(this._other_ships, this._pre_sort_key);
		}

		private ShipModel _GetShipModel(int mem_id)
		{
			return this._target_ships.Find((ShipModel ship) => ship.MemId == mem_id);
		}

		private CheckBoxStatus _GetCheckBoxStatus(ShipModel ship, DeckModel deck)
		{
			if (ship == null)
			{
				return CheckBoxStatus.DISABLE;
			}
			if (ship.IsInMission() || ship.IsBling())
			{
				return CheckBoxStatus.DISABLE;
			}
			if (!ship.GetResourcesForSupply().HasPositive())
			{
				return CheckBoxStatus.DISABLE;
			}
			if (this._checked_ships.IndexOf(ship.MemId) == -1)
			{
				return CheckBoxStatus.OFF;
			}
			return CheckBoxStatus.ON;
		}

		private string _GetString_CheckedShip()
		{
			string text = string.Format("[選択艦 総数:{0}] ", this._checked_ships.get_Count());
			for (int i = 0; i < this._checked_ships.get_Count(); i++)
			{
				ShipModel shipModel = this._GetShipModel(this._checked_ships.get_Item(i));
				text = text + shipModel.ShortName + " ";
			}
			return text;
		}

		public override string ToString()
		{
			string text = "--SupplyManager--\n";
			text += base.ToString();
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"\n[補給に必要な資材数]燃料/弾薬:",
				this.FuelForSupply,
				"/",
				this.AmmoForSupply,
				"\n"
			});
			text += string.Format("燃料補給:{0} 弾薬補給:{1} まとめて補給:{2}\n", (!this.IsValidSupply(SupplyType.Fuel)) ? "不可" : "可", (!this.IsValidSupply(SupplyType.Ammo)) ? "不可" : "可", (!this.IsValidSupply(SupplyType.All)) ? "不可" : "可");
			if (!this.IsShowOther())
			{
				text += string.Format("選択中の艦隊ID:{0}\n", this.SelectedDeck.Id);
			}
			else
			{
				text += string.Format("他艦選択中\n", new object[0]);
			}
			text += this._GetString_CheckedShip();
			text += "\n";
			text += string.Format("\n{0}\n", this.ToString(this.CheckBoxALLState));
			for (int i = 0; i < this.Ships.Length; i++)
			{
				ShipModel shipModel = this.Ships[i];
				if (shipModel != null)
				{
					text += string.Format("{0}{1} Lv{2}", this.ToString(this.CheckBoxStates[i]), shipModel.ShortName, shipModel.Level);
					text += string.Format(" Fuel:{0}/{1} Ammo:{2}/{3}\n", new object[]
					{
						shipModel.Fuel,
						shipModel.FuelMax,
						shipModel.Ammo,
						shipModel.AmmoMax
					});
				}
			}
			return text + "\n-----------------";
		}

		private string ToString(CheckBoxStatus cb)
		{
			if (cb == CheckBoxStatus.DISABLE)
			{
				return " - ";
			}
			if (cb == CheckBoxStatus.OFF)
			{
				return "[ ]";
			}
			return "[o]";
		}
	}
}
