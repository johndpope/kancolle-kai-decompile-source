using Common.Enum;
using Common.Struct;
using local.models;
using local.utils;
using Server_Controllers;
using Server_Models;
using System;
using System.Collections.Generic;

namespace local.managers
{
	public class RepairManager : ManagerBase
	{
		private int _area_id;

		private List<RepairDockModel> _docks;

		private List<ShipModel> _ships;

		private SortKey _pre_sort_key = SortKey.DAMAGE;

		private Mem_useitem _dock_key_item;

		public int NumOfKeyPossessions
		{
			get
			{
				return (this._dock_key_item != null) ? this._dock_key_item.Value : 0;
			}
		}

		public MapAreaModel MapArea
		{
			get
			{
				return ManagerBase._area.get_Item(this._area_id);
			}
		}

		public int ShipsCount
		{
			get
			{
				return this._ships.get_Count();
			}
		}

		public SortKey NowSortKey
		{
			get
			{
				return this._pre_sort_key;
			}
		}

		public RepairManager(int area_id)
		{
			this._area_id = area_id;
			base._CreateMapAreaModel();
			this._UpdateRepairDockData();
			this._UpdateRepairShipList();
			Api_Result<Dictionary<int, Mem_useitem>> api_Result = new Api_get_Member().UseItem();
			if (api_Result.state == Api_Result_State.Success && api_Result.data != null)
			{
				api_Result.data.TryGetValue(49, ref this._dock_key_item);
			}
		}

		public RepairDockModel[] GetDocks()
		{
			return this._docks.ToArray();
		}

		public RepairDockModel GetDockData(int index)
		{
			return this._docks.get_Item(index);
		}

		public RepairDockModel GetDockDataFromID(int id)
		{
			return this._docks.Find((RepairDockModel dock) => dock.Id == id);
		}

		public int GetDockIndexFromDock(RepairDockModel dock)
		{
			return this._docks.IndexOf(dock);
		}

		public bool ChangeSortKey(SortKey new_sort_key)
		{
			if (this._pre_sort_key != new_sort_key)
			{
				this._pre_sort_key = new_sort_key;
				this._ships = DeckUtil.GetSortedList(this._ships, this._pre_sort_key);
				return true;
			}
			return false;
		}

		public ShipModel[] GetShipList()
		{
			return this._ships.ToArray();
		}

		public ShipModel[] GetShipList(int page_no, int count_in_page)
		{
			int num = (page_no - 1) * count_in_page;
			num = Math.Max(num, 0);
			num = Math.Min(num, this._ships.get_Count());
			int num2 = this._ships.get_Count() - num;
			num2 = Math.Max(num2, 0);
			num2 = Math.Min(num2, count_in_page);
			return this._ships.GetRange(num, num2).ToArray();
		}

		public bool IsValidStartRepair(int ship_mem_id)
		{
			return this.IsValidStartRepair(ship_mem_id, false);
		}

		public bool IsValidStartRepair(int ship_mem_id, bool use_repairkit)
		{
			ShipModel shipModel = this._ships.Find((ShipModel x) => x.MemId == ship_mem_id);
			if (shipModel == null)
			{
				return false;
			}
			if (shipModel.TaikyuRate >= 100.0)
			{
				return false;
			}
			if (shipModel.IsInMission() || shipModel.IsInRepair())
			{
				return false;
			}
			if (shipModel.IsBling())
			{
				return false;
			}
			if (shipModel.IsBlingWaitFromEscortDeck())
			{
				return false;
			}
			MaterialInfo resourcesForRepair = shipModel.GetResourcesForRepair();
			if (base.Material.Fuel < resourcesForRepair.Fuel)
			{
				return false;
			}
			if (base.Material.Steel < resourcesForRepair.Steel)
			{
				return false;
			}
			if (use_repairkit && base.Material.RepairKit < 1)
			{
				return false;
			}
			DeckModelBase deck = shipModel.getDeck();
			if (deck != null)
			{
				return !deck.IsEscortDeckMyself() && deck.AreaId == this.MapArea.Id;
			}
			return (shipModel.IsBlingWaitFromDeck() && shipModel.AreaIdBeforeBlingWait == this.MapArea.Id) || this.MapArea.Id == 1;
		}

		public bool StartRepair(int selected_dock_index, int ship_mem_id, bool use_repairkit)
		{
			if (!this.IsValidStartRepair(ship_mem_id, use_repairkit))
			{
				return false;
			}
			RepairDockModel repairDockModel = this._docks.get_Item(selected_dock_index);
			Api_Result<Mem_ndock> api_Result = new Api_req_Nyuukyo().Start(repairDockModel.Id, ship_mem_id, use_repairkit);
			if (api_Result.state == Api_Result_State.Success)
			{
				Mem_ndock data = api_Result.data;
				repairDockModel.__Update__(data);
				if (use_repairkit && this._pre_sort_key == SortKey.DAMAGE)
				{
					this._UpdateRepairShipList();
				}
				return true;
			}
			return false;
		}

		public bool IsValidChangeRepairSpeed(int selected_dock_index)
		{
			RepairDockModel repairDockModel = this._docks.get_Item(selected_dock_index);
			return repairDockModel.State == NdockStates.RESTORE && base.Material.RepairKit >= 1;
		}

		public bool ChangeRepairSpeed(int selected_dock_index)
		{
			RepairDockModel repairDockModel = this._docks.get_Item(selected_dock_index);
			Api_Result<Mem_ndock> api_Result = new Api_req_Nyuukyo().SpeedChange(repairDockModel.Id);
			if (api_Result.state == Api_Result_State.Success)
			{
				Mem_ndock data = api_Result.data;
				repairDockModel.__Update__(data);
				return true;
			}
			return false;
		}

		public bool IsValidOpenNewDock()
		{
			return new Api_req_Member().IsValidNdockOpen(this.MapArea.Id);
		}

		public RepairDockModel OpenNewDock()
		{
			Api_Result<Mem_ndock> result = new Api_req_Member().NdockOpen(this.MapArea.Id);
			if (result.state == Api_Result_State.Success && result.data != null)
			{
				this._UpdateRepairDockData();
				return this._docks.Find((RepairDockModel dock) => dock.Id == result.data.Rid);
			}
			return null;
		}

		public void UpdateRepairDockData()
		{
			this._UpdateRepairDockData();
		}

		private void _UpdateRepairDockData()
		{
			this._docks = new List<RepairDockModel>();
			Dictionary<int, List<Mem_ndock>> data = new Api_get_Member().AreaNdock().data;
			if (data.ContainsKey(this.MapArea.Id))
			{
				List<Mem_ndock> list = data.get_Item(this.MapArea.Id);
				for (int i = 0; i < list.get_Count(); i++)
				{
					RepairDockModel repairDockModel = new RepairDockModel(list.get_Item(i));
					this._docks.Add(repairDockModel);
				}
				this.MapArea.__UpdateNdockData__(list);
			}
		}

		private void _UpdateRepairShipList()
		{
			List<ShipModel> all_ships = base.UserInfo.__GetShipList__();
			this._ships = base.GetAreaShips(this.MapArea.Id, all_ships);
			if (this._area_id == 1)
			{
				this._ships.AddRange(base.GetDepotShips(all_ships));
			}
			this._ships = this._ships.FindAll((ShipModel ship) => ship.NowHp < ship.MaxHp);
			this._ships = DeckUtil.GetSortedList(this._ships, this._pre_sort_key);
		}

		public override string ToString()
		{
			string text = string.Empty;
			text += string.Format("{0}\n", base.ToString());
			for (int i = 0; i < this._docks.get_Count(); i++)
			{
				text += string.Format("Dock{0}:{1}\n", i, this._docks.get_Item(i));
			}
			return text + string.Format("所持している開放キー:{0}\n", this.NumOfKeyPossessions);
		}
	}
}
