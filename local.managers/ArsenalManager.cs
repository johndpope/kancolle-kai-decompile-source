using Common.Enum;
using Common.Struct;
using local.models;
using local.utils;
using Server_Common;
using Server_Controllers;
using Server_Models;
using System;
using System.Collections.Generic;

namespace local.managers
{
	public class ArsenalManager : ManagerBase
	{
		private Api_req_Kousyou _api;

		private Mem_useitem _dock_key_item;

		private bool _large;

		private List<BuildDockModel> _docks;

		private List<ShipModel> _breakable_ships;

		private SortKey _pre_sort_key;

		private List<SlotitemModel> _unset_slotitem;

		private List<int> _selected_items = new List<int>();

		public int NumOfKeyPossessions
		{
			get
			{
				return (this._dock_key_item != null) ? this._dock_key_item.Value : 0;
			}
		}

		public bool LargeState
		{
			get
			{
				return this._large;
			}
			set
			{
				this._large = (value && this.LargeEnabled);
			}
		}

		public bool LargeEnabled
		{
			get
			{
				return Comm_UserDatas.Instance.User_basic.Large_dock > 0;
			}
		}

		public SortKey NowSortKey
		{
			get
			{
				return this._pre_sort_key;
			}
		}

		public ArsenalManager()
		{
			this._tanker_manager = new _TankerManager();
			this._api = new Api_req_Kousyou();
			this._UpdateBuildDockData();
			Api_Result<Dictionary<int, Mem_useitem>> api_Result = new Api_get_Member().UseItem();
			if (api_Result.state == Api_Result_State.Success && api_Result.data != null)
			{
				api_Result.data.TryGetValue(49, ref this._dock_key_item);
			}
		}

		public MaterialInfo GetMaxForCreateShip()
		{
			Dictionary<enumMaterialCategory, int> requireMaterials_Max = this._api.GetRequireMaterials_Max((!this.LargeState) ? 0 : 1);
			return new MaterialInfo(requireMaterials_Max);
		}

		public MaterialInfo GetMinForCreateShip()
		{
			Dictionary<enumMaterialCategory, int> requireMaterials_Min = this._api.GetRequireMaterials_Min((!this.LargeState) ? 0 : 1);
			return new MaterialInfo(requireMaterials_Min);
		}

		public BuildDockModel GetDock(int dock_id)
		{
			this._CheckBuildDockState();
			return this._docks.Find((BuildDockModel item) => item.Id == dock_id);
		}

		public BuildDockModel[] GetDocks()
		{
			this._CheckBuildDockState();
			return this._docks.ToArray();
		}

		public bool IsValidCreateShip(int dock_id, bool highSpeed, int fuel, int ammo, int steel, int baux, int dev_kit, int deck_id)
		{
			Dictionary<enumMaterialCategory, int> dictionary = new Dictionary<enumMaterialCategory, int>();
			dictionary.set_Item(enumMaterialCategory.Fuel, fuel);
			dictionary.set_Item(enumMaterialCategory.Bull, ammo);
			dictionary.set_Item(enumMaterialCategory.Steel, steel);
			dictionary.set_Item(enumMaterialCategory.Bauxite, baux);
			dictionary.set_Item(enumMaterialCategory.Dev_Kit, dev_kit);
			return this._api.ValidStart(dock_id, highSpeed, this._large, ref dictionary, deck_id);
		}

		public bool CreateShip(int dock_id, bool highSpeed, int fuel, int ammo, int steel, int baux, int dev_kit, int deck_id)
		{
			Dictionary<enumMaterialCategory, int> dictionary = new Dictionary<enumMaterialCategory, int>();
			dictionary.set_Item(enumMaterialCategory.Fuel, fuel);
			dictionary.set_Item(enumMaterialCategory.Bull, ammo);
			dictionary.set_Item(enumMaterialCategory.Steel, steel);
			dictionary.set_Item(enumMaterialCategory.Bauxite, baux);
			dictionary.set_Item(enumMaterialCategory.Dev_Kit, dev_kit);
			if (!this._api.ValidStart(dock_id, highSpeed, this._large, ref dictionary, deck_id))
			{
				return false;
			}
			Api_Result<Mem_kdock> api_Result = this._api.Start(dock_id, highSpeed, this._large, dictionary, deck_id);
			if (api_Result.state == Api_Result_State.Success)
			{
				Mem_kdock kdock = api_Result.data;
				int num = this._docks.FindIndex((BuildDockModel item) => item.Id == kdock.Rid);
				this._docks.get_Item(num).__Update__(kdock);
				return true;
			}
			return false;
		}

		public bool IsValidGetCreatedShip(int dock_id)
		{
			return this._api.ValidGetShip(dock_id);
		}

		public IReward_Ship GetCreatedShip(int dock_id)
		{
			if (!this.IsValidGetCreatedShip(dock_id))
			{
				return null;
			}
			int shipMstId = this.GetDock(dock_id).ShipMstId;
			Api_Result<Mem_kdock> ship = this._api.GetShip(dock_id);
			if (ship.state == Api_Result_State.Success)
			{
				base.UserInfo.__UpdateShips__(new Api_get_Member());
				this._breakable_ships = null;
				return new Reward_Ship(shipMstId);
			}
			return null;
		}

		public bool IsValidCreateShip_ChangeHighSpeed(int dock_id)
		{
			return this._api.ValidSpeedChange(dock_id);
		}

		public bool ChangeHighSpeed(int dock_id)
		{
			if (!this._api.ValidSpeedChange(dock_id))
			{
				return false;
			}
			Api_Result<Mem_kdock> api_Result = this._api.SpeedChange(dock_id);
			return api_Result.state == Api_Result_State.Success;
		}

		public bool IsValidOpenNewDock()
		{
			return new Api_req_Member().IsValidKdockOpen();
		}

		public BuildDockModel OpenNewDock()
		{
			Api_Result<Mem_kdock> result = new Api_req_Member().KdockOpen();
			if (result.state == Api_Result_State.Success && result.data != null)
			{
				this._UpdateBuildDockData();
				return this._docks.Find((BuildDockModel dock) => dock.Id == result.data.Rid);
			}
			return null;
		}

		private void _CheckBuildDockState()
		{
			bool flag = false;
			for (int i = 0; i < this._docks.get_Count(); i++)
			{
				BuildDockModel buildDockModel = this._docks.get_Item(i);
				if (buildDockModel.State == KdockStates.CREATE && buildDockModel.GetTurn() == 0)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				this._UpdateBuildDockData();
			}
		}

		private void _UpdateBuildDockData()
		{
			if (this._docks == null)
			{
				this._docks = new List<BuildDockModel>();
			}
			Api_Result<List<Mem_kdock>> api_Result = new Api_get_Member().kdock();
			if (api_Result.state == Api_Result_State.Success)
			{
				ArsenalManager.<_UpdateBuildDockData>c__AnonStorey507 <_UpdateBuildDockData>c__AnonStorey = new ArsenalManager.<_UpdateBuildDockData>c__AnonStorey507();
				<_UpdateBuildDockData>c__AnonStorey.mem_docks = api_Result.data;
				int i;
				for (i = 0; i < <_UpdateBuildDockData>c__AnonStorey.mem_docks.get_Count(); i++)
				{
					BuildDockModel buildDockModel = this._docks.Find((BuildDockModel dock) => dock.Id == <_UpdateBuildDockData>c__AnonStorey.mem_docks.get_Item(i).Rid);
					if (buildDockModel != null)
					{
						buildDockModel.__Update__(<_UpdateBuildDockData>c__AnonStorey.mem_docks.get_Item(i));
					}
					else
					{
						this._docks.Add(new BuildDockModel(<_UpdateBuildDockData>c__AnonStorey.mem_docks.get_Item(i)));
					}
				}
			}
		}

		public bool ChangeSortKey(SortKey new_sort_key)
		{
			if (this._pre_sort_key != new_sort_key)
			{
				this._pre_sort_key = new_sort_key;
				this._breakable_ships = null;
				return true;
			}
			return false;
		}

		public ShipModel[] GetShipList()
		{
			if (this._breakable_ships == null)
			{
				this._UpdateShipList();
			}
			return this._breakable_ships.ToArray();
		}

		public ShipModel[] GetShipList(int page_no, int count_in_page)
		{
			if (this._breakable_ships == null)
			{
				this._UpdateShipList();
			}
			int num = (page_no - 1) * count_in_page;
			num = Math.Max(num, 0);
			num = Math.Min(num, this._breakable_ships.get_Count());
			int num2 = this._breakable_ships.get_Count() - num;
			num2 = Math.Max(num2, 0);
			num2 = Math.Min(num2, count_in_page);
			return this._breakable_ships.GetRange(num, num2).ToArray();
		}

		public bool IsValidBreakShip(ShipModel ship)
		{
			return !ship.IsLocked() && !ship.HasLocked() && ship.IsInDeck() < 0 && ship.IsInEscortDeck() < 0 && !ship.IsInRepair() && !ship.IsBling();
		}

		public bool BreakShip(int ship_mem_id)
		{
			Api_Result<object> api_Result = this._api.DestroyShip(ship_mem_id);
			if (api_Result.state == Api_Result_State.Success)
			{
				base.UserInfo.__UpdateShips__(new Api_get_Member());
				this._breakable_ships = null;
				return true;
			}
			return false;
		}

		private void _UpdateShipList()
		{
			List<ShipModel> all_ships = base.UserInfo.__GetShipList__();
			this._breakable_ships = base.GetAreaShips(1, false, false, all_ships);
			this._breakable_ships.AddRange(base.GetDepotShips(all_ships));
			this._breakable_ships = DeckUtil.GetSortedList(this._breakable_ships, this._pre_sort_key);
		}

		public MaterialInfo GetMaxForCreateItem()
		{
			return new MaterialInfo
			{
				Fuel = 300,
				Ammo = 300,
				Steel = 300,
				Baux = 300,
				Devkit = 1
			};
		}

		public MaterialInfo GetMinForCreateItem()
		{
			return new MaterialInfo
			{
				Fuel = 10,
				Ammo = 10,
				Steel = 10,
				Baux = 10,
				Devkit = 0
			};
		}

		public bool IsValidCreateItem(int fuel, int ammo, int steel, int baux)
		{
			Dictionary<enumMaterialCategory, int> dictionary = new Dictionary<enumMaterialCategory, int>();
			dictionary.set_Item(enumMaterialCategory.Fuel, fuel);
			dictionary.set_Item(enumMaterialCategory.Bull, ammo);
			dictionary.set_Item(enumMaterialCategory.Steel, steel);
			dictionary.set_Item(enumMaterialCategory.Bauxite, baux);
			return this.IsValidCreateItem(dictionary);
		}

		public bool IsValidCreateItem(Dictionary<enumMaterialCategory, int> materials)
		{
			if (Comm_UserDatas.Instance.User_basic.IsMaxSlotitem())
			{
				return false;
			}
			MaterialInfo maxForCreateItem = this.GetMaxForCreateItem();
			MaterialInfo minForCreateItem = this.GetMinForCreateItem();
			int num;
			materials.TryGetValue(enumMaterialCategory.Fuel, ref num);
			if (num < minForCreateItem.Fuel || maxForCreateItem.Fuel < num || base.Material.Fuel < num)
			{
				return false;
			}
			materials.TryGetValue(enumMaterialCategory.Bull, ref num);
			if (num < minForCreateItem.Ammo || maxForCreateItem.Ammo < num || base.Material.Ammo < num)
			{
				return false;
			}
			materials.TryGetValue(enumMaterialCategory.Steel, ref num);
			if (num < minForCreateItem.Steel || maxForCreateItem.Steel < num || base.Material.Steel < num)
			{
				return false;
			}
			materials.TryGetValue(enumMaterialCategory.Bauxite, ref num);
			return num >= minForCreateItem.Baux && maxForCreateItem.Baux >= num && base.Material.Baux >= num && base.Material.Devkit >= 1;
		}

		public IReward_Slotitem CreateItem(int fuel, int ammo, int steel, int baux, int deck_id)
		{
			Dictionary<enumMaterialCategory, int> dictionary = new Dictionary<enumMaterialCategory, int>();
			dictionary.set_Item(enumMaterialCategory.Fuel, fuel);
			dictionary.set_Item(enumMaterialCategory.Bull, ammo);
			dictionary.set_Item(enumMaterialCategory.Steel, steel);
			dictionary.set_Item(enumMaterialCategory.Bauxite, baux);
			return this.CreateItem(dictionary, deck_id);
		}

		public IReward_Slotitem CreateItem(Dictionary<enumMaterialCategory, int> materials, int deck_id)
		{
			Api_Result<Mst_slotitem> api_Result = this._api.CreateItem(materials, deck_id);
			if (api_Result.state != Api_Result_State.Success || api_Result.data == null)
			{
				return null;
			}
			this._unset_slotitem = null;
			return new Reward_Slotitem(api_Result.data);
		}

		public List<SlotitemModel> GetSelectedItemsForDetroy()
		{
			if (this._unset_slotitem == null)
			{
				this._UpdateUnsetSlotitems();
			}
			List<SlotitemModel> list = this._unset_slotitem.FindAll((SlotitemModel item) => this._selected_items.IndexOf(item.MemId) >= 0);
			if (list.get_Count() != this._selected_items.get_Count())
			{
				return null;
			}
			return list;
		}

		public bool IsSelected(int slotitem_mem_id)
		{
			int num = this._selected_items.IndexOf(slotitem_mem_id);
			return num != -1;
		}

		public bool ToggleSelectedState(int slotitem_mem_id)
		{
			if (this.IsSelected(slotitem_mem_id))
			{
				this._selected_items.Remove(slotitem_mem_id);
				return false;
			}
			if (this._unset_slotitem == null)
			{
				this._UpdateUnsetSlotitems();
			}
			if (this._unset_slotitem.Find((SlotitemModel item) => item.MemId == slotitem_mem_id) != null)
			{
				this._selected_items.Add(slotitem_mem_id);
				return true;
			}
			return false;
		}

		public bool SelectForDestroy(int slotitem_mem_id)
		{
			if (!this.IsSelected(slotitem_mem_id))
			{
				if (this._unset_slotitem == null)
				{
					this._UpdateUnsetSlotitems();
				}
				if (this._unset_slotitem.Find((SlotitemModel item) => item.MemId == slotitem_mem_id) != null)
				{
					this._selected_items.Add(slotitem_mem_id);
					return true;
				}
			}
			return false;
		}

		public bool UnselectForDestroy(int slotitem_mem_id)
		{
			if (this.IsSelected(slotitem_mem_id))
			{
				this._selected_items.Remove(slotitem_mem_id);
				return true;
			}
			return false;
		}

		public void ClearSelectedState()
		{
			this._selected_items.Clear();
		}

		public SlotitemModel[] GetUnsetSlotitems()
		{
			if (this._unset_slotitem == null)
			{
				this._UpdateUnsetSlotitems();
			}
			return this._unset_slotitem.ToArray();
		}

		public SlotitemModel[] GetUnsetSlotitems(int page_no, int count_in_page)
		{
			if (this._unset_slotitem == null)
			{
				this._UpdateUnsetSlotitems();
			}
			int num = (page_no - 1) * count_in_page;
			num = Math.Max(num, 0);
			num = Math.Min(num, this._unset_slotitem.get_Count());
			int num2 = this._unset_slotitem.get_Count() - num;
			num2 = Math.Max(num2, 0);
			num2 = Math.Min(num2, count_in_page);
			return this._unset_slotitem.GetRange(num, num2).ToArray();
		}

		public MaterialInfo GetMaterialsForBreakItem()
		{
			MaterialInfo result = default(MaterialInfo);
			List<SlotitemModel> selectedItemsForDetroy = this.GetSelectedItemsForDetroy();
			for (int i = 0; i < selectedItemsForDetroy.get_Count(); i++)
			{
				SlotitemModel slotitemModel = selectedItemsForDetroy.get_Item(i);
				if (slotitemModel != null)
				{
					result.Fuel += slotitemModel.BrokenFuel;
					result.Ammo += slotitemModel.BrokenAmmo;
					result.Steel += slotitemModel.BrokenSteel;
					result.Baux += slotitemModel.BrokenBaux;
				}
			}
			return result;
		}

		public void GetMaterialsForBreakItem(out int fuel, out int ammo, out int steel, out int baux)
		{
			fuel = (ammo = (steel = (baux = 0)));
			if (this._unset_slotitem == null)
			{
				this._UpdateUnsetSlotitems();
			}
			for (int i = 0; i < this._selected_items.get_Count(); i++)
			{
				int slotitem_mem_id = this._selected_items.get_Item(i);
				SlotitemModel slotitemModel = this._unset_slotitem.Find((SlotitemModel tmp) => tmp.MemId == slotitem_mem_id);
				if (slotitemModel != null)
				{
					fuel += slotitemModel.BrokenFuel;
					ammo += slotitemModel.BrokenAmmo;
					steel += slotitemModel.BrokenSteel;
					baux += slotitemModel.BrokenBaux;
				}
			}
		}

		public bool IsValidBreakItem()
		{
			if (this._selected_items.get_Count() == 0)
			{
				return false;
			}
			if (this._unset_slotitem == null)
			{
				this._UpdateUnsetSlotitems();
			}
			for (int i = 0; i < this._selected_items.get_Count(); i++)
			{
				int slotitem_mem_id = this._selected_items.get_Item(i);
				SlotitemModel slotitemModel = this._unset_slotitem.Find((SlotitemModel tmp) => tmp.MemId == slotitem_mem_id);
				if (slotitemModel.IsLocked())
				{
					return false;
				}
			}
			return true;
		}

		public bool BreakItem()
		{
			Api_Result<object> api_Result = this._api.DestroyItem(this._selected_items);
			if (api_Result.state == Api_Result_State.Success)
			{
				this._unset_slotitem = null;
				this.ClearSelectedState();
				return true;
			}
			return false;
		}

		private void _UpdateUnsetSlotitems()
		{
			this._unset_slotitem = SlotitemUtil.__GetUnsetSlotitems__();
			SlotitemUtil.Sort(this._unset_slotitem, SlotitemUtil.SlotitemSortKey.Type3);
		}

		public MaterialInfo GetMaxForCreateTanker()
		{
			Dictionary<enumMaterialCategory, int> requireMaterials_Max = this._api.GetRequireMaterials_Max(2);
			return new MaterialInfo(requireMaterials_Max);
		}

		public MaterialInfo GetMinForCreateTanker()
		{
			Dictionary<enumMaterialCategory, int> requireMaterials_Min = this._api.GetRequireMaterials_Min(2);
			return new MaterialInfo(requireMaterials_Min);
		}

		public int GetSpointMaxForCreateTanker()
		{
			return this._api.Stratege_Max;
		}

		public int GetSpointMinForCreateTanker()
		{
			return this._api.Stratege_Min;
		}

		public AreaTankerModel GetNonDeploymentTankerCount()
		{
			return this._tanker_manager.GetCounts();
		}

		public bool IsValidCreateTanker(int dock_id, bool highSpeed, int fuel, int ammo, int steel, int baux, int spoint)
		{
			Dictionary<enumMaterialCategory, int> dictionary = new Dictionary<enumMaterialCategory, int>();
			dictionary.set_Item(enumMaterialCategory.Fuel, fuel);
			dictionary.set_Item(enumMaterialCategory.Bull, ammo);
			dictionary.set_Item(enumMaterialCategory.Steel, steel);
			dictionary.set_Item(enumMaterialCategory.Bauxite, baux);
			return this._api.ValidStartTanker(dock_id, highSpeed, ref dictionary, spoint);
		}

		public BuildDockModel CreateTanker(int dock_id, bool highSpeed, int fuel, int ammo, int steel, int baux, int spoint)
		{
			Dictionary<enumMaterialCategory, int> dictionary = new Dictionary<enumMaterialCategory, int>();
			dictionary.set_Item(enumMaterialCategory.Fuel, fuel);
			dictionary.set_Item(enumMaterialCategory.Bull, ammo);
			dictionary.set_Item(enumMaterialCategory.Steel, steel);
			dictionary.set_Item(enumMaterialCategory.Bauxite, baux);
			int num = 0;
			if (highSpeed)
			{
				num = this._api.GetRequireMaterials_Max(2).get_Item(enumMaterialCategory.Build_Kit);
			}
			dictionary.set_Item(enumMaterialCategory.Build_Kit, num);
			if (!this.IsValidCreateTanker(dock_id, highSpeed, fuel, ammo, steel, baux, spoint))
			{
				return null;
			}
			Api_Result<Mem_kdock> api_result = this._api.StartTanker(dock_id, highSpeed, dictionary, spoint);
			if (api_result.state != Api_Result_State.Success || api_result.data == null)
			{
				return null;
			}
			return this._docks.Find((BuildDockModel dock) => dock.Id == api_result.data.Rid);
		}

		public bool IsValidGetCreatedTanker(int dock_id)
		{
			return this._api.ValidGetTanker(dock_id);
		}

		public int GetCreatedTanker(int dock_id)
		{
			if (!this.IsValidGetCreatedTanker(dock_id))
			{
				return 0;
			}
			int tankerCount = this.GetDock(dock_id).TankerCount;
			Api_Result<Mem_kdock> tanker = this._api.GetTanker(dock_id);
			if (tanker.state == Api_Result_State.Success)
			{
				this._tanker_manager.Update();
				return tankerCount;
			}
			return 0;
		}

		public bool IsValid_ChangeHighSpeedTanker(int dock_id)
		{
			return this._api.ValidSpeedChangeTanker(dock_id);
		}

		public bool ChangeHighSpeedTanker(int dock_id)
		{
			if (!this._api.ValidSpeedChangeTanker(dock_id))
			{
				return false;
			}
			Api_Result<Mem_kdock> api_Result = this._api.SpeedChangeTanker(dock_id);
			return api_Result.state == Api_Result_State.Success;
		}

		public override string ToString()
		{
			BuildDockModel[] docks = this.GetDocks();
			string text = string.Empty;
			text += base.ToString();
			text += "\n";
			text += string.Format("状態:{0}\n", (!this.LargeState) ? "通常" : "大型");
			for (int i = 0; i < docks.Length; i++)
			{
				text += string.Format("Dock{0}:{1}\n", i, docks[i]);
			}
			text += string.Format("所持している開放キー:{0}\n", this.NumOfKeyPossessions);
			return text + string.Format("未配備の輸送船数:{0}(この海域への移動中:{1}) - 総数:{2}", this._tanker_manager.GetCounts().GetCount(), this._tanker_manager.GetCounts().GetCountMove(), this._tanker_manager.GetAllCount());
		}
	}
}
