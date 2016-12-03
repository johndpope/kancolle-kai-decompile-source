using Common.Enum;
using KCV.PresetData;
using local.managers;
using local.models;
using Server_Controllers;
using Server_Models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyDebugMenu3 : MonoBehaviour
	{
		private enum AreaGroup
		{
			Previous1,
			Previous2,
			Medium1,
			Medium2,
			Late,
			AllClear
		}

		[SerializeField]
		private UIButtonManager ButtonManager;

		[SerializeField]
		private UILabel Message;

		private Debug_Mod debugMod;

		private UserInfoModel UserInfo;

		[SerializeField]
		private GameObject DebugMenu1;

		private int NextMemID;

		private List<Entity_PresetShip.Param> ShipParamList;

		private PresetDataManager presetDataManager;

		private void Start()
		{
			this.debugMod = new Debug_Mod();
			this.UserInfo = StrategyTopTaskManager.GetLogicManager().UserInfo;
			this.ButtonManager.setButtonDelegate(Util.CreateEventDelegate(this, "OnPushPreset", null));
			TaskStrategyDebug.isControl = true;
			this.presetDataManager = new PresetDataManager();
			this.NextMemID = 2;
			this.ShipParamList = new List<Entity_PresetShip.Param>();
			this.presetDataManager.GetPresetShipParam("初期艦").MemID = 1;
		}

		public void OnDeside()
		{
			this.ButtonManager.setAllButtonEnable(false);
			UIButton[] focusableButtons = this.ButtonManager.GetFocusableButtons();
			UIButton[] array = focusableButtons;
			for (int i = 0; i < array.Length; i++)
			{
				UIButton uIButton = array[i];
				if (uIButton != null)
				{
					uIButton.get_gameObject().SetActive(false);
				}
			}
			this.ButtonManager.nowForcusButton.SetActive(true);
			this.Message.text = "データロード中です";
		}

		public void OnPushPreset()
		{
			int ButtonNo = this.ButtonManager.nowForcusIndex;
			this.OnDeside();
			this.DelayActionFrame(1, delegate
			{
				this.CreateDebugData(ButtonNo);
				Object.Destroy(StrategyTopTaskManager.Instance);
				Application.LoadLevel(Generics.Scene.Strategy.ToString());
			});
		}

		public void CreateDebugData(int ButtonNo)
		{
			Debug.Log(ButtonNo);
			App.CreateSaveDataNInitialize(this.UserInfo.Name, 54, this.UserInfo.Difficulty, false);
			App.CreateSaveDataNInitialize(this.UserInfo.Name, 54, this.UserInfo.Difficulty, false);
			SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck = StrategyTopTaskManager.GetLogicManager().UserInfo.GetDeck(1);
			StrategyTopTaskManager.Instance.GetAreaMng().ChangeFocusTile(1, true);
			UserInfoModel userInfo = StrategyTopTaskManager.GetLogicManager().UserInfo;
			TutorialModel tutorial = StrategyTopTaskManager.GetLogicManager().UserInfo.Tutorial;
			for (int i = 0; i < 20; i++)
			{
				tutorial.SetStepTutorialFlg(i);
			}
			for (int j = 0; j < 99; j++)
			{
				tutorial.SetKeyTutorialFlg(j);
			}
			this.LoadPresetData(ButtonNo);
			Debug_Mod.Add_SlotItemAll();
		}

		private void LoadPresetData(int PresetNo)
		{
			Entity_PresetData.Param presetData = this.presetDataManager.GetPresetData(PresetNo);
			Debug_Mod.SetFleetLevel(presetData.TeitokuLV);
			this.AddMaterialAndItems(presetData);
			Debug_Mod.Add_Tunker(presetData.Tanker);
			this.DeployTanker();
			this.AreaOpen(presetData);
			this.AddDeck(presetData);
			Debug_Mod.SetRebellionPhase(presetData.RebellionPhase);
			this.AddShips(presetData);
			this.SetDeck(presetData);
			this.SetAllShipsLevel(presetData.AllShipLevel);
			if (presetData.AddAllShip)
			{
				Debug_Mod.Add_ShipAll();
			}
		}

		public void ChangeDebugMode()
		{
			this.DebugMenu1.SetActive(true);
			base.get_gameObject().SetActive(false);
			TaskStrategyDebug.isControl = true;
		}

		private void AreaOpen(Entity_PresetData.Param Param)
		{
			for (int i = 0; i < 17; i++)
			{
				this.MapsOpen(i + 1, Param.Area[i]);
			}
		}

		private void MapsOpen(int AreaNo, int OpenNum)
		{
			for (int i = 1; i < OpenNum + 1; i++)
			{
				Debug_Mod.OpenMapArea(AreaNo, i);
			}
		}

		private void DeployTanker()
		{
			StrategyTopTaskManager.CreateLogicManager();
			for (int i = 1; i < 18; i++)
			{
				if (StrategyTopTaskManager.GetLogicManager().Area.get_Item(i).IsOpen())
				{
					StrategyTopTaskManager.GetLogicManager().Deploy(i, 5, new EscortDeckManager(i));
				}
			}
		}

		private void AddDeck(Entity_PresetData.Param Param)
		{
			for (int i = 1; i <= 8; i++)
			{
				if (Param.Deck[i - 1] != 0)
				{
					this.debugMod.Add_Deck(i);
				}
			}
		}

		private void AddDeckShips(Entity_PresetDeck.Param DeckParam)
		{
			for (int i = 0; i < DeckParam.PresetShip.Length; i++)
			{
				if (DeckParam.PresetShip[i] != string.Empty)
				{
					Entity_PresetShip.Param ShipParam = this.presetDataManager.GetPresetShipParam(DeckParam.PresetShip[i]);
					if (!this.ShipParamList.Exists((Entity_PresetShip.Param s) => s == ShipParam))
					{
						this.ShipParamList.Add(ShipParam);
						ShipParam.MemID = this.NextMemID;
						this.NextMemID++;
					}
				}
			}
		}

		private void AddShips(Entity_PresetData.Param Param)
		{
			this.ShipParamList.Add(this.presetDataManager.GetPresetShipParam("初期艦"));
			for (int i = 0; i < Param.Deck.Length; i++)
			{
				if (Param.Deck[i] != 0)
				{
					this.AddDeckShips(this.presetDataManager.GetPresetDeck(Param.Deck[i]));
				}
			}
			List<int> list = new List<int>();
			for (int j = 0; j < this.ShipParamList.get_Count(); j++)
			{
				list.Add(this.ShipParamList.get_Item(j).MstID);
			}
			this.debugMod.Add_Ship(list);
		}

		private void AddSlotItems(StrategyDebugMenu3.AreaGroup group)
		{
			List<int> slot_ids = new List<int>();
			this.debugMod.Add_SlotItem(slot_ids);
		}

		private void SetDeck(Entity_PresetData.Param Param)
		{
			for (int i = 0; i < Param.Deck.Length; i++)
			{
				int num = Param.Deck[i];
				if (num != 0)
				{
					Entity_PresetDeck.Param presetDeck = this.presetDataManager.GetPresetDeck(num);
					List<int> list = new List<int>();
					for (int j = 0; j < presetDeck.PresetShip.Length; j++)
					{
						list.Add(this.presetDataManager.GetPresetShipParam(presetDeck.PresetShip[j]).MemID);
					}
					this.SetDeckShips(i + 1, list);
				}
			}
		}

		private void SetDeckShips(int DeckNo, List<int> memIDs)
		{
			OrganizeManager organizeManager = new OrganizeManager(1);
			for (int i = 0; i < memIDs.get_Count(); i++)
			{
				organizeManager.ChangeOrganize(DeckNo, i, memIDs.get_Item(i));
			}
		}

		private void AddMaterialAndItems(Entity_PresetData.Param Param)
		{
			this.debugMod.Add_Materials(enumMaterialCategory.Fuel, Param.Fuel);
			this.debugMod.Add_Materials(enumMaterialCategory.Bull, Param.Bull);
			this.debugMod.Add_Materials(enumMaterialCategory.Steel, Param.Steel);
			this.debugMod.Add_Materials(enumMaterialCategory.Bauxite, Param.Baux);
			this.debugMod.Add_Materials(enumMaterialCategory.Dev_Kit, Param.Dev_Kit);
			this.debugMod.Add_Materials(enumMaterialCategory.Build_Kit, Param.BuildKit);
			this.debugMod.Add_Materials(enumMaterialCategory.Repair_Kit, Param.RepairKit);
			this.debugMod.Add_Materials(enumMaterialCategory.Revamp_Kit, Param.Revamp_Kit);
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			for (int i = 0; i < 100; i++)
			{
				if ((1 <= i && i <= 3) || (10 <= i && i <= 12) || (49 <= i && i <= 59))
				{
					dictionary.set_Item(i, Param.Items);
				}
			}
			this.debugMod.Add_UseItem(dictionary);
			this.debugMod.Add_Spoint(Param.Spoint);
			this.debugMod.Add_Coin(Param.Coin);
		}

		private void SetAllShipsLevel(int LV)
		{
			if (LV == 0)
			{
				return;
			}
			this.UserInfo = StrategyTopTaskManager.GetLogicManager().UserInfo;
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Get_MstLevel(true);
			for (int i = 1; i < this.UserInfo.ShipCountData().NowCount + 1; i++)
			{
				this.UserInfo.GetShip(i).AddExp(dictionary.get_Item(LV) - this.UserInfo.GetShip(i).Exp);
			}
		}
	}
}
