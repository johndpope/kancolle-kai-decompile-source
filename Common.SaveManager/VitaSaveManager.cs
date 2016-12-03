using Common.Enum;
using Common.Struct;
using Server_Common;
using Server_Models;
using Sony.Vita.SavedGame;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;

namespace Common.SaveManager
{
	public class VitaSaveManager : MonoBehaviour
	{
		private static VitaSaveManager _instance;

		private readonly int slotCount = 5;

		private XElement _elements;

		private ResultCode lastErrorCode;

		private ISaveDataOperator operatorInstance;

		private bool isOpen;

		private bool isMainInit;

		public static VitaSaveManager Instance
		{
			get
			{
				if (VitaSaveManager._instance != null)
				{
					return VitaSaveManager._instance;
				}
				Type typeFromHandle = typeof(VitaSaveManager);
				VitaSaveManager vitaSaveManager = Object.FindObjectOfType(typeFromHandle) as VitaSaveManager;
				if (vitaSaveManager == null)
				{
					string text = typeFromHandle.ToString();
					GameObject gameObject = new GameObject(text, new Type[]
					{
						typeFromHandle
					});
					vitaSaveManager = gameObject.GetComponent<VitaSaveManager>();
				}
				if (vitaSaveManager != null)
				{
					VitaSaveManager.Initialise(vitaSaveManager);
				}
				return VitaSaveManager._instance;
			}
		}

		public int SlotCount
		{
			get
			{
				return this.slotCount;
			}
		}

		public XElement Elements
		{
			get
			{
				return this._elements;
			}
			private set
			{
				this._elements = value;
			}
		}

		public bool IsDialogOpen
		{
			get
			{
				return SaveLoad.get_IsDialogOpen();
			}
		}

		public bool IsBusy
		{
			get
			{
				return SaveLoad.get_IsBusy();
			}
		}

		public ResultCode LastErrorCode
		{
			get
			{
				return this.lastErrorCode;
			}
			private set
			{
				this.lastErrorCode = value;
			}
		}

		private static void Initialise(VitaSaveManager instance)
		{
			if (VitaSaveManager._instance == null)
			{
				VitaSaveManager._instance = instance;
				instance.OnInitialize();
				Object.DontDestroyOnLoad(VitaSaveManager._instance.get_gameObject());
			}
			else if (VitaSaveManager._instance != instance)
			{
				Object.DestroyImmediate(instance);
			}
		}

		public void OnInitialize()
		{
			try
			{
				SaveLoad.add_OnGameSaved(new Messages.EventHandler(this.OnSavedGameSaved));
				SaveLoad.add_OnGameLoaded(new Messages.EventHandler(this.OnSavedGameLoaded));
				SaveLoad.add_OnCanceled(new Messages.EventHandler(this.OnSavedGameCanceled));
				SaveLoad.add_OnSaveError(new Messages.EventHandler(this.OnSaveError));
				SaveLoad.add_OnLoadError(new Messages.EventHandler(this.OnLoadError));
				SaveLoad.add_OnLoadNoData(new Messages.EventHandler(this.OnLoadNoData));
				Main.Initialise();
				SaveLoad.SetEmptySlotIconPath(Application.get_streamingAssetsPath() + "/SaveIconEmpty.png");
				SaveLoad.SetSlotCount(this.SlotCount);
				this.isMainInit = true;
			}
			catch
			{
				this.isMainInit = false;
			}
		}

		public void Open(ISaveDataOperator instance)
		{
			this.operatorInstance = instance;
			this.DestroyElements();
			this.isOpen = true;
		}

		public void Close()
		{
			this.operatorInstance = null;
			this.DestroyElements();
			this.isOpen = false;
		}

		public bool Save()
		{
			if (this.operatorInstance == null || Comm_UserDatas.Instance.User_basic.Starttime == 0)
			{
				return false;
			}
			if (SaveLoad.get_IsDialogOpen() || this.IsBusy)
			{
				return false;
			}
			SaveHeaderFmt saveHeaderFmt = new SaveHeaderFmt();
			saveHeaderFmt.SetPropertie();
			List<SaveTarget> saveTarget = this.getSaveTarget(saveHeaderFmt);
			byte[] array = null;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (XmlWriter xmlWriter = XmlWriter.Create(memoryStream))
				{
					xmlWriter.WriteStartDocument();
					xmlWriter.WriteStartElement(this.getTableName());
					using (List<SaveTarget>.Enumerator enumerator = saveTarget.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							SaveTarget current = enumerator.get_Current();
							DataContractSerializer dataContractSerializer;
							if (current.IsCollection)
							{
								dataContractSerializer = new DataContractSerializer(current.ClassType, current.TableName + "s", string.Empty);
							}
							else
							{
								dataContractSerializer = new DataContractSerializer(current.ClassType);
							}
							dataContractSerializer.WriteObject(xmlWriter, current.Data);
						}
					}
					xmlWriter.WriteEndElement();
					xmlWriter.Flush();
					array = memoryStream.ToArray();
				}
			}
			SaveLoad.SavedGameSlotParams savedGameSlotParams = default(SaveLoad.SavedGameSlotParams);
			TurnString turnString = Comm_UserDatas.Instance.User_turn.GetTurnString();
			string text = (Comm_UserDatas.Instance.User_plus.GetLapNum() <= 0) ? string.Empty : "★";
			string subTitle = string.Format("{0}{1}の年 {2} {3}日", new object[]
			{
				text,
				turnString.Year,
				turnString.Month,
				turnString.Day
			});
			string nickname = Comm_UserDatas.Instance.User_basic.Nickname;
			string datail = this.getDatail();
			savedGameSlotParams.title = nickname;
			savedGameSlotParams.subTitle = subTitle;
			savedGameSlotParams.detail = datail;
			savedGameSlotParams.iconPath = Application.get_streamingAssetsPath() + "/SaveIcon.png";
			SaveLoad.ControlFlags controlFlags = 0;
			ErrorCode errorCode = SaveLoad.SaveGameList(array, savedGameSlotParams, controlFlags);
			return errorCode == null;
		}

		private string getDatail()
		{
			Comm_UserDatas instance = Comm_UserDatas.Instance;
			string text = string.Empty;
			int num = instance.User_basic.Difficult - DifficultKind.TEI;
			string[] array = new string[]
			{
				"丁",
				"丙",
				"乙",
				"甲",
				"史"
			};
			text = text + "難易度:" + array[num] + "\n";
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"提督レベル:",
				instance.User_basic.UserLevel(),
				"\n"
			});
			text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"艦隊保有数:",
				instance.User_deck.get_Count(),
				"\n"
			});
			text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"艦娘保有数:",
				instance.User_ship.get_Count(),
				"\n"
			});
			text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"戦略ポイント:",
				instance.User_basic.Strategy_point,
				"\n"
			});
			text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"燃料:",
				instance.User_material.get_Item(enumMaterialCategory.Fuel).Value,
				"\n"
			});
			text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"弾薬:",
				instance.User_material.get_Item(enumMaterialCategory.Bull).Value,
				"\n"
			});
			text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"鋼材:",
				instance.User_material.get_Item(enumMaterialCategory.Steel).Value,
				"\n"
			});
			text2 = text;
			return string.Concat(new object[]
			{
				text2,
				"ボーキサイト:",
				instance.User_material.get_Item(enumMaterialCategory.Bauxite).Value,
				"\n"
			});
		}

		public bool IsAllEmpty()
		{
			for (int i = 0; i < this.SlotCount; i++)
			{
				SaveLoad.SavedGameSlotInfo savedGameSlotInfo;
				if (SaveLoad.GetSlotInfo(i, ref savedGameSlotInfo) == null && savedGameSlotInfo.status != 2)
				{
					return false;
				}
			}
			return true;
		}

		public bool Load()
		{
			if (this.operatorInstance == null)
			{
				return false;
			}
			if (SaveLoad.get_IsDialogOpen() || this.IsBusy)
			{
				return false;
			}
			ErrorCode errorCode = SaveLoad.LoadGameList();
			return errorCode == null;
		}

		public bool Delete()
		{
			if (this.operatorInstance == null)
			{
				return false;
			}
			if (SaveLoad.get_IsDialogOpen() || this.IsBusy)
			{
				return false;
			}
			ErrorCode errorCode = SaveLoad.DeleteGameList();
			return errorCode == null;
		}

		private List<SaveTarget> getSaveTarget(SaveHeaderFmt header)
		{
			List<SaveTarget> list = new List<SaveTarget>();
			Comm_UserDatas instance = Comm_UserDatas.Instance;
			list.Add(new SaveTarget(typeof(SaveHeaderFmt), header, SaveHeaderFmt.tableaName));
			list.Add(new SaveTarget(typeof(Mem_basic), instance.User_basic, Mem_basic.tableName));
			list.Add(new SaveTarget(typeof(Mem_newgame_plus), instance.User_plus, Mem_newgame_plus.tableName));
			list.Add(new SaveTarget(typeof(Mem_record), instance.User_record, Mem_record.tableName));
			list.Add(new SaveTarget(typeof(Mem_trophy), instance.User_trophy, Mem_trophy.tableName));
			list.Add(new SaveTarget(typeof(Mem_turn), instance.User_turn, Mem_turn.tableName));
			list.Add(new SaveTarget(typeof(Mem_deckpractice), instance.User_deckpractice, Mem_deckpractice.tableName));
			list.Add(new SaveTarget(typeof(List<Mem_book>), Enumerable.ToList<Mem_book>(instance.Ship_book.get_Values()), "ship_book"));
			list.Add(new SaveTarget(typeof(List<Mem_book>), Enumerable.ToList<Mem_book>(instance.Slot_book.get_Values()), "slot_book"));
			list.Add(new SaveTarget(typeof(List<Mem_deck>), Enumerable.ToList<Mem_deck>(instance.User_deck.get_Values()), Mem_deck.tableName));
			list.Add(new SaveTarget(typeof(List<Mem_esccort_deck>), Enumerable.ToList<Mem_esccort_deck>(instance.User_EscortDeck.get_Values()), Mem_esccort_deck.tableName));
			list.Add(new SaveTarget(typeof(List<Mem_furniture>), Enumerable.ToList<Mem_furniture>(instance.User_furniture.get_Values()), Mem_furniture.tableName));
			list.Add(new SaveTarget(typeof(List<Mem_kdock>), Enumerable.ToList<Mem_kdock>(instance.User_kdock.get_Values()), Mem_kdock.tableName));
			list.Add(new SaveTarget(typeof(List<Mem_mapcomp>), Enumerable.ToList<Mem_mapcomp>(instance.User_mapcomp.get_Values()), Mem_mapcomp.tableName));
			list.Add(new SaveTarget(typeof(List<Mem_mapclear>), Enumerable.ToList<Mem_mapclear>(instance.User_mapclear.get_Values()), Mem_mapclear.tableName));
			list.Add(new SaveTarget(typeof(List<Mem_material>), Enumerable.ToList<Mem_material>(instance.User_material.get_Values()), Mem_material.tableName));
			list.Add(new SaveTarget(typeof(List<Mem_missioncomp>), Enumerable.ToList<Mem_missioncomp>(instance.User_missioncomp.get_Values()), Mem_missioncomp.tableName));
			list.Add(new SaveTarget(typeof(List<Mem_ndock>), Enumerable.ToList<Mem_ndock>(instance.User_ndock.get_Values()), Mem_ndock.tableName));
			list.Add(new SaveTarget(typeof(List<Mem_quest>), Enumerable.ToList<Mem_quest>(instance.User_quest.get_Values()), Mem_quest.tableName));
			list.Add(new SaveTarget(typeof(List<Mem_questcount>), Enumerable.ToList<Mem_questcount>(instance.User_questcount.get_Values()), Mem_questcount.tableName));
			list.Add(new SaveTarget(instance.User_ship.get_Values()));
			list.Add(new SaveTarget(typeof(List<Mem_slotitem>), Enumerable.ToList<Mem_slotitem>(instance.User_slot.get_Values()), Mem_slotitem.tableName));
			list.Add(new SaveTarget(typeof(List<Mem_tanker>), Enumerable.ToList<Mem_tanker>(instance.User_tanker.get_Values()), Mem_tanker.tableName));
			list.Add(new SaveTarget(typeof(List<Mem_useitem>), Enumerable.ToList<Mem_useitem>(instance.User_useItem.get_Values()), Mem_useitem.tableName));
			list.Add(new SaveTarget(typeof(List<Mem_rebellion_point>), Enumerable.ToList<Mem_rebellion_point>(instance.User_rebellion_point.get_Values()), Mem_rebellion_point.tableName));
			list.Add(new SaveTarget(typeof(List<Mem_room>), Enumerable.ToList<Mem_room>(instance.User_room.get_Values()), Mem_room.tableName));
			list.Add(new SaveTarget(typeof(List<int>), Enumerable.ToList<int>(instance.Temp_escortship), "temp_escortship"));
			list.Add(new SaveTarget(typeof(List<int>), Enumerable.ToList<int>(instance.Temp_deckship), "temp_deckship"));
			List<Mem_history> list2 = new List<Mem_history>();
			using (Dictionary<int, List<Mem_history>>.ValueCollection.Enumerator enumerator = instance.User_history.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_history> current = enumerator.get_Current();
					list2.AddRange(current);
				}
			}
			list.Add(new SaveTarget(typeof(List<Mem_history>), list2, Mem_history.tableName));
			return list;
		}

		private void DestroyElements()
		{
			if (this.Elements == null)
			{
				return;
			}
			this.Elements.RemoveAll();
			this.Elements = null;
		}

		private string getTableName()
		{
			return "member_datas";
		}

		private void OnSavedGameSaved(Messages.PluginMessage msg)
		{
			this.operatorInstance.SaveComplete();
		}

		private void OnSavedGameLoaded(Messages.PluginMessage msg)
		{
			byte[] loadedGame = SaveLoad.GetLoadedGame();
			if (loadedGame == null || loadedGame.Length == 0)
			{
				this.OnLoadNoData(msg);
				return;
			}
			using (MemoryStream memoryStream = new MemoryStream(loadedGame))
			{
				XmlReader xmlReader = XmlReader.Create(memoryStream);
				this.Elements = XElement.Load(xmlReader);
				xmlReader.Close();
			}
			if (Comm_UserDatas.Instance.SetUserData())
			{
				this.operatorInstance.LoadComplete();
			}
			else
			{
				this.operatorInstance.LoadError();
			}
			this.DestroyElements();
		}

		private void OnSavedGameCanceled(Messages.PluginMessage msg)
		{
			this.operatorInstance.Canceled();
		}

		private void OnSaveError(Messages.PluginMessage msg)
		{
			ResultCode resultCode = default(ResultCode);
			SaveLoad.GetLastError(ref resultCode);
			this.LastErrorCode = resultCode;
			this.operatorInstance.SaveError();
		}

		private void OnLoadError(Messages.PluginMessage msg)
		{
			ResultCode resultCode = default(ResultCode);
			SaveLoad.GetLastError(ref resultCode);
			this.LastErrorCode = resultCode;
			this.operatorInstance.LoadError();
		}

		private void OnLoadNoData(Messages.PluginMessage msg)
		{
			this.operatorInstance.LoadNothing();
		}

		private void OnDeleted(Messages.PluginMessage msg)
		{
			this.operatorInstance.DeleteComplete();
		}

		private void Update()
		{
			if (this.isOpen && this.isMainInit)
			{
				Main.Update();
			}
		}

		private void Awake()
		{
			VitaSaveManager.Initialise(this);
		}
	}
}
