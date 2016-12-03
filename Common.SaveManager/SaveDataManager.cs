using Server_Common;
using Server_Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Linq;

namespace Common.SaveManager
{
	public class SaveDataManager
	{
		private string outDir = "../";

		private static SaveDataManager _instance;

		private readonly int slotCount = 4;

		private Dictionary<int, SaveHeaderFmt> header = new Dictionary<int, SaveHeaderFmt>();

		private Dictionary<int, SaveHeaderFmt> cacheHeader = new Dictionary<int, SaveHeaderFmt>();

		private XElement _elements;

		public string OutDir
		{
			get
			{
				return this.outDir;
			}
			set
			{
				this.OutDir = value;
			}
		}

		public static SaveDataManager Instance
		{
			get
			{
				if (SaveDataManager._instance == null)
				{
					SaveDataManager._instance = new SaveDataManager();
				}
				return SaveDataManager._instance;
			}
			private set
			{
				SaveDataManager._instance = value;
			}
		}

		public int SlotCount
		{
			get
			{
				return this.slotCount;
			}
		}

		public Dictionary<int, SaveHeaderFmt> Header
		{
			get
			{
				return this.header;
			}
			private set
			{
				this.header = value;
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

		public SaveDataManager()
		{
			this.Elements = null;
			this.Header = new Dictionary<int, SaveHeaderFmt>();
			this.cacheHeader = new Dictionary<int, SaveHeaderFmt>();
			for (int i = 1; i <= this.slotCount; i++)
			{
				this.Header.Add(i, null);
				SaveHeaderFmt headerInfo = this.getHeaderInfo(i);
				this.cacheHeader.Add(i, headerInfo);
			}
		}

		public void UpdateHeader()
		{
			for (int i = 1; i <= this.slotCount; i++)
			{
				if (this.cacheHeader.get_Item(i) == null)
				{
					this.cacheHeader.set_Item(i, this.getHeaderInfo(i));
				}
				this.header.set_Item(i, this.cacheHeader.get_Item(i));
			}
		}

		public bool Save(int slot_no)
		{
			if (Comm_UserDatas.Instance.User_basic.Starttime == 0)
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
					xmlWriter.WriteStartElement(this.getTableName(slot_no));
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
							xmlWriter.Flush();
						}
					}
					xmlWriter.WriteEndElement();
					xmlWriter.Flush();
					array = memoryStream.ToArray();
				}
			}
			File.WriteAllBytes(this.getMemberFilePath(slot_no), array);
			this.cacheHeader.set_Item(slot_no, saveHeaderFmt);
			return true;
		}

		public bool Load(int slot_no)
		{
			if (!object.ReferenceEquals(this.Header.get_Item(slot_no), this.cacheHeader.get_Item(slot_no)))
			{
				return false;
			}
			if (!File.Exists(this.getMemberFilePath(slot_no)))
			{
				return false;
			}
			this.Elements = XElement.Load(this.getMemberFilePath(slot_no));
			Comm_UserDatas.Instance.SetUserData();
			this.DestroyElements();
			return true;
		}

		private SaveHeaderFmt getHeaderInfo(int slot_no)
		{
			SaveHeaderFmt saveHeaderFmt = null;
			XElement xElement = null;
			try
			{
				xElement = XElement.Load(this.getMemberFilePath(slot_no)).Element(SaveHeaderFmt.tableaName);
			}
			catch (Exception ex)
			{
				string message = ex.get_Message();
				xElement = null;
			}
			if (xElement != null)
			{
				saveHeaderFmt = new SaveHeaderFmt();
				if (!saveHeaderFmt.SetPropertie(xElement))
				{
					saveHeaderFmt = null;
				}
			}
			return saveHeaderFmt;
		}

		private string getTableName(int slot_no)
		{
			return "member_datas" + slot_no;
		}

		private string getMemberFilePath(int slot_no)
		{
			return this.OutDir + this.getTableName(slot_no) + ".xml";
		}

		private void DestroyElements()
		{
			this.Elements.RemoveAll();
			this.Elements = null;
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
	}
}
