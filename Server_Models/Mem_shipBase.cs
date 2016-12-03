using Common.Enum;
using Server_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Server_Models
{
	[DataContract(Name = "mem_ship", Namespace = "")]
	public class Mem_shipBase
	{
		[DataMember]
		public int Rid;

		[DataMember]
		public int GetNo;

		[DataMember]
		public int Ship_id;

		[DataMember]
		public int Level;

		[DataMember]
		public int Exp;

		[DataMember]
		public int Nowhp;

		[DataMember]
		public List<int> Slot;

		[DataMember]
		public List<int> Onslot;

		[DataMember]
		public int Exslot;

		[DataMember]
		public int C_houg;

		[DataMember]
		public int C_raig;

		[DataMember]
		public int C_tyku;

		[DataMember]
		public int C_souk;

		[DataMember]
		public int C_kaihi;

		[DataMember]
		public int C_taisen;

		[DataMember]
		public int C_taik;

		[DataMember]
		public int C_taik_powerup;

		[DataMember]
		public int C_luck;

		[DataMember]
		public int Fuel;

		[DataMember]
		public int Bull;

		[DataMember]
		public int Cond;

		[DataMember]
		public int Locked;

		[DataMember]
		public bool Escape_sts;

		[DataMember]
		public Mem_ship.BlingKind BlingType;

		[DataMember]
		public int BlingWaitArea;

		[DataMember]
		public int BlingWaitDeck;

		[DataMember]
		public int Bling_start;

		[DataMember]
		public int Bling_end;

		[DataMember]
		public List<BattleCommand> BattleCommand;

		[DataMember]
		public int Lov;

		[DataMember]
		public Dictionary<byte, byte> Lov_back_processed;

		[DataMember]
		public List<int> Lov_back_value;

		[DataMember]
		public Dictionary<byte, byte> Lov_front_processed;

		[DataMember]
		public List<int> Lov_front_value;

		public Mem_shipBase()
		{
			this.Slot = new List<int>(5);
			this.Onslot = new List<int>(5);
			this.Escape_sts = false;
			this.Bling_start = 0;
			this.Bling_end = 0;
			this.BlingType = Mem_ship.BlingKind.None;
			this.BlingWaitDeck = 0;
			this.Lov_back_processed = new Dictionary<byte, byte>();
			this.Lov_back_value = new List<int>();
			this.Lov_front_processed = new Dictionary<byte, byte>();
			this.Lov_front_value = new List<int>();
			this.Exslot = -2;
		}

		public Mem_shipBase(int rid, int getNo, Mst_ship mst_data) : this()
		{
			this.Rid = rid;
			this.GetNo = getNo;
			this.Ship_id = mst_data.Id;
			this.Level = 1;
			this.Exp = 0;
			this.Nowhp = mst_data.Taik;
			List<int> list = Comm_UserDatas.Instance.Add_Slot(mst_data.Defeq);
			for (int i = 0; i < mst_data.Slot_num; i++)
			{
				if (Enumerable.Count<int>(list) > i)
				{
					this.Slot.Add(list.get_Item(i));
					Mem_slotitem mem_slotitem = Comm_UserDatas.Instance.User_slot.get_Item(list.get_Item(i));
					mem_slotitem.Equip(rid);
				}
				else
				{
					this.Slot.Add(mst_data.Defeq.get_Item(i));
				}
				this.Onslot.Add(mst_data.Maxeq.get_Item(i));
			}
			this.C_houg = 0;
			this.C_raig = 0;
			this.C_tyku = 0;
			this.C_souk = 0;
			this.C_kaihi = 0;
			this.C_taisen = 0;
			this.C_taik = 0;
			this.C_taik_powerup = 0;
			this.C_luck = 0;
			this.Fuel = mst_data.Fuel_max;
			this.Bull = mst_data.Bull_max;
			this.Locked = 0;
			this.Cond = 40;
			this.Lov = 50;
		}

		public Mem_shipBase(Mem_ship base_ship) : this()
		{
			this.Rid = base_ship.Rid;
			this.GetNo = base_ship.GetNo;
			this.Ship_id = base_ship.Ship_id;
			this.Level = base_ship.Level;
			this.Exp = base_ship.Exp;
			this.Nowhp = base_ship.Nowhp;
			this.Slot = Enumerable.ToList<int>(base_ship.Slot);
			this.Onslot = Enumerable.ToList<int>(base_ship.Onslot);
			this.SetKyoukaValue(base_ship.Kyouka);
			this.Fuel = base_ship.Fuel;
			this.Bull = base_ship.Bull;
			this.Locked = base_ship.Locked;
			this.Cond = base_ship.Cond;
			this.Escape_sts = base_ship.Escape_sts;
			this.BlingType = base_ship.BlingType;
			this.Bling_start = base_ship.Bling_start;
			this.Bling_end = base_ship.Bling_end;
			this.BlingWaitArea = base_ship.BlingWaitArea;
			base_ship.GetBattleCommand(out this.BattleCommand);
			this.Lov = base_ship.Lov;
			this.Lov_back_processed = base_ship.Lov_back_processed;
			this.Lov_back_value = base_ship.Lov_back_value;
			this.Lov_front_processed = base_ship.Lov_front_processed;
			this.Lov_front_value = base_ship.Lov_front_value;
			this.Exslot = base_ship.Exslot;
		}

		public Mem_shipBase(int rid, Mst_ship mst_ship, int level, Dictionary<Mem_ship.enumKyoukaIdx, int> kyouka) : this()
		{
			this.Rid = rid;
			this.Ship_id = mst_ship.Id;
			this.Level = level;
			this.Nowhp = mst_ship.Taik;
			for (int i = 0; i < mst_ship.Slot_num; i++)
			{
				this.Slot.Add(mst_ship.Defeq.get_Item(i));
				this.Onslot.Add(mst_ship.Maxeq.get_Item(i));
			}
			this.SetKyoukaValue(kyouka);
			this.Fuel = mst_ship.Fuel_max;
			this.Bull = mst_ship.Bull_max;
			this.Locked = 0;
			this.Cond = 40;
			this.BlingType = Mem_ship.BlingKind.None;
		}

		public void SetKyoukaValue(Dictionary<Mem_ship.enumKyoukaIdx, int> kyouka)
		{
			this.C_houg = kyouka.get_Item(Mem_ship.enumKyoukaIdx.Houg);
			this.C_raig = kyouka.get_Item(Mem_ship.enumKyoukaIdx.Raig);
			this.C_tyku = kyouka.get_Item(Mem_ship.enumKyoukaIdx.Tyku);
			this.C_souk = kyouka.get_Item(Mem_ship.enumKyoukaIdx.Souk);
			this.C_taik = kyouka.get_Item(Mem_ship.enumKyoukaIdx.Taik);
			this.C_taik_powerup = kyouka.get_Item(Mem_ship.enumKyoukaIdx.Taik_Powerup);
			this.C_luck = kyouka.get_Item(Mem_ship.enumKyoukaIdx.Luck);
			this.C_taisen = kyouka.get_Item(Mem_ship.enumKyoukaIdx.Taisen);
			this.C_kaihi = kyouka.get_Item(Mem_ship.enumKyoukaIdx.Kaihi);
		}

		public void setProperty(XElement element)
		{
			this.Rid = int.Parse(element.Element("Rid").get_Value());
			this.GetNo = int.Parse(element.Element("GetNo").get_Value());
			this.Ship_id = int.Parse(element.Element("Ship_id").get_Value());
			this.Level = int.Parse(element.Element("Level").get_Value());
			this.Exp = int.Parse(element.Element("Exp").get_Value());
			this.Nowhp = int.Parse(element.Element("Nowhp").get_Value());
			this.C_houg = int.Parse(element.Element("C_houg").get_Value());
			this.C_raig = int.Parse(element.Element("C_raig").get_Value());
			this.C_tyku = int.Parse(element.Element("C_tyku").get_Value());
			this.C_souk = int.Parse(element.Element("C_souk").get_Value());
			this.C_taik = int.Parse(element.Element("C_taik").get_Value());
			this.C_taik_powerup = int.Parse(element.Element("C_taik_powerup").get_Value());
			this.C_luck = int.Parse(element.Element("C_luck").get_Value());
			this.C_taisen = int.Parse(element.Element("C_taisen").get_Value());
			this.C_kaihi = int.Parse(element.Element("C_kaihi").get_Value());
			this.Fuel = int.Parse(element.Element("Fuel").get_Value());
			this.Bull = int.Parse(element.Element("Bull").get_Value());
			this.Cond = int.Parse(element.Element("Cond").get_Value());
			this.Locked = int.Parse(element.Element("Locked").get_Value());
			this.Escape_sts = bool.Parse(element.Element("Escape_sts").get_Value());
			this.BlingType = (Mem_ship.BlingKind)((int)Enum.Parse(typeof(Mem_ship.BlingKind), element.Element("BlingType").get_Value()));
			this.BlingWaitDeck = int.Parse(element.Element("BlingWaitDeck").get_Value());
			this.BlingWaitArea = int.Parse(element.Element("BlingWaitArea").get_Value());
			this.Bling_start = int.Parse(element.Element("Bling_start").get_Value());
			this.Bling_end = int.Parse(element.Element("Bling_end").get_Value());
			using (IEnumerator<XElement> enumerator = element.Element("Slot").Elements().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					XElement current = enumerator.get_Current();
					this.Slot.Add(int.Parse(current.get_Value()));
				}
			}
			using (var enumerator2 = Enumerable.Select(element.Element("Onslot").Elements(), (XElement obj, int idx) => new
			{
				obj,
				idx
			}).GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					var current2 = enumerator2.get_Current();
					this.Onslot.Add(int.Parse(current2.obj.get_Value()));
				}
			}
			if (element.Element("Exslot") != null)
			{
				this.Exslot = int.Parse(element.Element("Exslot").get_Value());
			}
			else
			{
				this.Exslot = -2;
			}
			if (element.Element("BattleCommand").get_Value() != string.Empty)
			{
				this.BattleCommand = new List<BattleCommand>();
				using (IEnumerator<XElement> enumerator3 = element.Element("BattleCommand").Elements().GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						XElement current3 = enumerator3.get_Current();
						BattleCommand battleCommand = (BattleCommand)((int)Enum.Parse(typeof(BattleCommand), current3.get_Value()));
						this.BattleCommand.Add(battleCommand);
					}
				}
			}
			this.Lov = int.Parse(element.Element("Lov").get_Value());
			using (IEnumerator<XElement> enumerator4 = element.Element("Lov_back_processed").Elements().GetEnumerator())
			{
				while (enumerator4.MoveNext())
				{
					XElement current4 = enumerator4.get_Current();
					XNode firstNode = current4.get_FirstNode();
					XNode nextNode = firstNode.get_NextNode();
					byte b = byte.Parse(((XElement)firstNode).get_Value());
					byte b2 = byte.Parse(((XElement)nextNode).get_Value());
					this.Lov_back_processed.Add(b, b2);
				}
			}
			using (IEnumerator<XElement> enumerator5 = element.Element("Lov_back_value").Elements().GetEnumerator())
			{
				while (enumerator5.MoveNext())
				{
					XElement current5 = enumerator5.get_Current();
					int num = int.Parse(current5.get_Value());
					this.Lov_back_value.Add(num);
				}
			}
			using (IEnumerator<XElement> enumerator6 = element.Element("Lov_front_processed").Elements().GetEnumerator())
			{
				while (enumerator6.MoveNext())
				{
					XElement current6 = enumerator6.get_Current();
					XNode firstNode2 = current6.get_FirstNode();
					XNode nextNode2 = firstNode2.get_NextNode();
					byte b3 = byte.Parse(((XElement)firstNode2).get_Value());
					byte b4 = byte.Parse(((XElement)nextNode2).get_Value());
					this.Lov_front_processed.Add(b3, b4);
				}
			}
			using (IEnumerator<XElement> enumerator7 = element.Element("Lov_front_value").Elements().GetEnumerator())
			{
				while (enumerator7.MoveNext())
				{
					XElement current7 = enumerator7.get_Current();
					int num2 = int.Parse(current7.get_Value());
					this.Lov_front_value.Add(num2);
				}
			}
		}
	}
}
