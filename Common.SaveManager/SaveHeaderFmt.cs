using Common.Enum;
using Server_Common;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Common.SaveManager
{
	[DataContract(Name = "member_header", Namespace = "")]
	public class SaveHeaderFmt
	{
		[DataMember]
		public string Nickname;

		[DataMember]
		public DifficultKind Difficult;

		[DataMember]
		public int Level;

		[DataMember]
		public int DeckNum;

		[DataMember]
		public int KdockNum;

		[DataMember]
		public int NdockNum;

		[DataMember]
		public int ShipNum;

		[DataMember]
		public int ShipMax;

		[DataMember]
		public int SlotNum;

		[DataMember]
		public int SlotMax;

		[DataMember]
		public int FurnitureNum;

		public static string tableaName = "member_header";

		public bool SetPropertie()
		{
			Mem_basic user_basic = Comm_UserDatas.Instance.User_basic;
			Mem_record user_record = Comm_UserDatas.Instance.User_record;
			Dictionary<int, Mem_deck> user_deck = Comm_UserDatas.Instance.User_deck;
			Dictionary<int, Mem_kdock> user_kdock = Comm_UserDatas.Instance.User_kdock;
			Dictionary<int, Mem_ndock> user_ndock = Comm_UserDatas.Instance.User_ndock;
			Dictionary<int, Mem_ship> user_ship = Comm_UserDatas.Instance.User_ship;
			Dictionary<int, Mem_slotitem> user_slot = Comm_UserDatas.Instance.User_slot;
			Dictionary<int, Mem_furniture> user_furniture = Comm_UserDatas.Instance.User_furniture;
			this.Nickname = user_basic.Nickname;
			this.Difficult = user_basic.Difficult;
			this.Level = user_record.Level;
			this.DeckNum = user_deck.get_Count();
			this.KdockNum = user_kdock.get_Count();
			this.NdockNum = user_ndock.get_Count();
			this.ShipNum = user_ship.get_Count();
			this.ShipMax = user_basic.Max_chara;
			this.SlotNum = user_slot.get_Count();
			this.SlotMax = user_basic.Max_slotitem;
			this.FurnitureNum = user_furniture.get_Count();
			return true;
		}

		public bool SetPropertie(XElement element)
		{
			try
			{
				this.Nickname = element.Element("Nickname").get_Value();
				this.Difficult = (DifficultKind)((int)Enum.Parse(typeof(DifficultKind), element.Element("Difficult").get_Value()));
				this.Level = int.Parse(element.Element("Level").get_Value());
				this.DeckNum = int.Parse(element.Element("DeckNum").get_Value());
				this.KdockNum = int.Parse(element.Element("KdockNum").get_Value());
				this.NdockNum = int.Parse(element.Element("NdockNum").get_Value());
				this.ShipNum = int.Parse(element.Element("ShipNum").get_Value());
				this.ShipMax = int.Parse(element.Element("ShipMax").get_Value());
				this.SlotNum = int.Parse(element.Element("SlotNum").get_Value());
				this.SlotMax = int.Parse(element.Element("SlotMax").get_Value());
				this.FurnitureNum = int.Parse(element.Element("FurnitureNum").get_Value());
			}
			catch (Exception ex)
			{
				string message = ex.get_Message();
				return false;
			}
			return true;
		}
	}
}
