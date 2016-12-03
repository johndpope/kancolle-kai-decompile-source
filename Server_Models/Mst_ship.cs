using Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_ship : Model_Base
	{
		private int _id;

		private int _sortno;

		private int _bookno;

		private string _name;

		private string _yomi;

		private int _stype;

		private int _ctype;

		private int _cnum;

		private int _backs;

		private int _taik;

		private int _taik_max;

		private int _souk;

		private int _souk_max;

		private int _kaih;

		private int _kaih_max;

		private int _tous;

		private int _tous_max;

		private int _sokuh;

		private int _soku;

		private int _leng;

		private int _houg;

		private int _houg_max;

		private int _raig;

		private int _raig_max;

		private int _tyku;

		private int _tyku_max;

		private int _tais;

		private int _tais_max;

		private int _saku;

		private int _saku_max;

		private int _luck;

		private int _luck_max;

		private int _slot_num;

		private int _maxeq1;

		private int _maxeq2;

		private int _maxeq3;

		private int _maxeq4;

		private int _maxeq5;

		private int _defeq1;

		private int _defeq2;

		private int _defeq3;

		private int _defeq4;

		private int _defeq5;

		private int _afterlv;

		private int _afterfuel;

		private int _afterbull;

		private int _aftershipid;

		private int _buildtime;

		private int _broken1;

		private int _broken2;

		private int _broken3;

		private int _broken4;

		private int _powup1;

		private int _powup2;

		private int _powup3;

		private int _powup4;

		private int _use_fuel;

		private int _use_bull;

		private int _fuel_max;

		private int _bull_max;

		private int _raim;

		private int _raim_max;

		private int _append_ship_id;

		private int _event_limited;

		private int _btp;

		private List<int> _maxeq;

		private List<int> _defeq;

		private List<int> _broken;

		private List<int> _powup;

		private static string _tableName = "mst_ship";

		public int Id
		{
			get
			{
				return this._id;
			}
			private set
			{
				this._id = value;
			}
		}

		public int Sortno
		{
			get
			{
				return this._sortno;
			}
			private set
			{
				this._sortno = value;
			}
		}

		public int Bookno
		{
			get
			{
				return this._bookno;
			}
			private set
			{
				this._bookno = value;
			}
		}

		public string Name
		{
			get
			{
				return this._name;
			}
			private set
			{
				this._name = value;
			}
		}

		public string Yomi
		{
			get
			{
				return this._yomi;
			}
			private set
			{
				this._yomi = value;
			}
		}

		public int Stype
		{
			get
			{
				return this._stype;
			}
			private set
			{
				this._stype = value;
			}
		}

		public int Ctype
		{
			get
			{
				return this._ctype;
			}
			private set
			{
				this._ctype = value;
			}
		}

		public int Cnum
		{
			get
			{
				return this._cnum;
			}
			private set
			{
				this._cnum = value;
			}
		}

		public int Backs
		{
			get
			{
				return this._backs;
			}
			private set
			{
				this._backs = value;
			}
		}

		public int Taik
		{
			get
			{
				return this._taik;
			}
			private set
			{
				this._taik = value;
			}
		}

		public int Taik_max
		{
			get
			{
				return this._taik_max;
			}
			private set
			{
				this._taik_max = value;
			}
		}

		public int Souk
		{
			get
			{
				return this._souk;
			}
			private set
			{
				this._souk = value;
			}
		}

		public int Souk_max
		{
			get
			{
				return this._souk_max;
			}
			private set
			{
				this._souk_max = value;
			}
		}

		public int Kaih
		{
			get
			{
				return this._kaih;
			}
			private set
			{
				this._kaih = value;
			}
		}

		public int Kaih_max
		{
			get
			{
				return this._kaih_max;
			}
			private set
			{
				this._kaih_max = value;
			}
		}

		public int Tous
		{
			get
			{
				return this._tous;
			}
			private set
			{
				this._tous = value;
			}
		}

		public int Tous_max
		{
			get
			{
				return this._tous_max;
			}
			private set
			{
				this._tous_max = value;
			}
		}

		public int Sokuh
		{
			get
			{
				return this._sokuh;
			}
			private set
			{
				this._sokuh = value;
			}
		}

		public int Soku
		{
			get
			{
				return this._soku;
			}
			private set
			{
				this._soku = value;
			}
		}

		public int Leng
		{
			get
			{
				return this._leng;
			}
			private set
			{
				this._leng = value;
			}
		}

		public int Houg
		{
			get
			{
				return this._houg;
			}
			private set
			{
				this._houg = value;
			}
		}

		public int Houg_max
		{
			get
			{
				return this._houg_max;
			}
			private set
			{
				this._houg_max = value;
			}
		}

		public int Raig
		{
			get
			{
				return this._raig;
			}
			private set
			{
				this._raig = value;
			}
		}

		public int Raig_max
		{
			get
			{
				return this._raig_max;
			}
			private set
			{
				this._raig_max = value;
			}
		}

		public int Tyku
		{
			get
			{
				return this._tyku;
			}
			private set
			{
				this._tyku = value;
			}
		}

		public int Tyku_max
		{
			get
			{
				return this._tyku_max;
			}
			private set
			{
				this._tyku_max = value;
			}
		}

		public int Tais
		{
			get
			{
				return this._tais;
			}
			private set
			{
				this._tais = value;
			}
		}

		public int Tais_max
		{
			get
			{
				return this._tais_max;
			}
			private set
			{
				this._tais_max = value;
			}
		}

		public int Saku
		{
			get
			{
				return this._saku;
			}
			private set
			{
				this._saku = value;
			}
		}

		public int Saku_max
		{
			get
			{
				return this._saku_max;
			}
			private set
			{
				this._saku_max = value;
			}
		}

		public int Luck
		{
			get
			{
				return this._luck;
			}
			private set
			{
				this._luck = value;
			}
		}

		public int Luck_max
		{
			get
			{
				return this._luck_max;
			}
			private set
			{
				this._luck_max = value;
			}
		}

		public int Slot_num
		{
			get
			{
				return this._slot_num;
			}
			private set
			{
				this._slot_num = value;
			}
		}

		public int Maxeq1
		{
			get
			{
				return this._maxeq1;
			}
			private set
			{
				this._maxeq1 = value;
			}
		}

		public int Maxeq2
		{
			get
			{
				return this._maxeq2;
			}
			private set
			{
				this._maxeq2 = value;
			}
		}

		public int Maxeq3
		{
			get
			{
				return this._maxeq3;
			}
			private set
			{
				this._maxeq3 = value;
			}
		}

		public int Maxeq4
		{
			get
			{
				return this._maxeq4;
			}
			private set
			{
				this._maxeq4 = value;
			}
		}

		public int Maxeq5
		{
			get
			{
				return this._maxeq5;
			}
			private set
			{
				this._maxeq5 = value;
			}
		}

		public int Defeq1
		{
			get
			{
				return this._defeq1;
			}
			private set
			{
				this._defeq1 = value;
			}
		}

		public int Defeq2
		{
			get
			{
				return this._defeq2;
			}
			private set
			{
				this._defeq2 = value;
			}
		}

		public int Defeq3
		{
			get
			{
				return this._defeq3;
			}
			private set
			{
				this._defeq3 = value;
			}
		}

		public int Defeq4
		{
			get
			{
				return this._defeq4;
			}
			private set
			{
				this._defeq4 = value;
			}
		}

		public int Defeq5
		{
			get
			{
				return this._defeq5;
			}
			private set
			{
				this._defeq5 = value;
			}
		}

		public int Afterlv
		{
			get
			{
				return this._afterlv;
			}
			private set
			{
				this._afterlv = value;
			}
		}

		public int Afterfuel
		{
			get
			{
				return this._afterfuel;
			}
			private set
			{
				this._afterfuel = value;
			}
		}

		public int Afterbull
		{
			get
			{
				return this._afterbull;
			}
			private set
			{
				this._afterbull = value;
			}
		}

		public int Aftershipid
		{
			get
			{
				return this._aftershipid;
			}
			private set
			{
				this._aftershipid = value;
			}
		}

		public int Buildtime
		{
			get
			{
				double num = Math.Ceiling((double)this._buildtime / 15.0);
				return ((int)num > 1) ? ((int)num) : 2;
			}
			private set
			{
				this._buildtime = value;
			}
		}

		public int Broken1
		{
			get
			{
				return this._broken1;
			}
			private set
			{
				this._broken1 = value;
			}
		}

		public int Broken2
		{
			get
			{
				return this._broken2;
			}
			private set
			{
				this._broken2 = value;
			}
		}

		public int Broken3
		{
			get
			{
				return this._broken3;
			}
			private set
			{
				this._broken3 = value;
			}
		}

		public int Broken4
		{
			get
			{
				return this._broken4;
			}
			private set
			{
				this._broken4 = value;
			}
		}

		public int Powup1
		{
			get
			{
				return this._powup1;
			}
			private set
			{
				this._powup1 = value;
			}
		}

		public int Powup2
		{
			get
			{
				return this._powup2;
			}
			private set
			{
				this._powup2 = value;
			}
		}

		public int Powup3
		{
			get
			{
				return this._powup3;
			}
			private set
			{
				this._powup3 = value;
			}
		}

		public int Powup4
		{
			get
			{
				return this._powup4;
			}
			private set
			{
				this._powup4 = value;
			}
		}

		public int Use_fuel
		{
			get
			{
				return this._use_fuel;
			}
			private set
			{
				this._use_fuel = value;
			}
		}

		public int Use_bull
		{
			get
			{
				return this._use_bull;
			}
			private set
			{
				this._use_bull = value;
			}
		}

		public int Fuel_max
		{
			get
			{
				return this._fuel_max;
			}
			private set
			{
				this._fuel_max = value;
			}
		}

		public int Bull_max
		{
			get
			{
				return this._bull_max;
			}
			private set
			{
				this._bull_max = value;
			}
		}

		public int Voicef
		{
			get
			{
				return Mst_DataManager.Instance.Mst_ship_resources.get_Item(this.Id).Voicef;
			}
		}

		public int Raim
		{
			get
			{
				return this._raim;
			}
			private set
			{
				this._raim = value;
			}
		}

		public int Raim_max
		{
			get
			{
				return this._raim_max;
			}
			private set
			{
				this._raim_max = value;
			}
		}

		public int Append_ship_id
		{
			get
			{
				return this._append_ship_id;
			}
			private set
			{
				this._append_ship_id = value;
			}
		}

		public int Event_limited
		{
			get
			{
				return this._event_limited;
			}
			private set
			{
				this._event_limited = value;
			}
		}

		public List<int> Maxeq
		{
			get
			{
				return this._maxeq;
			}
		}

		public List<int> Defeq
		{
			get
			{
				return this._defeq;
			}
		}

		public List<int> Broken
		{
			get
			{
				return this._broken;
			}
		}

		public List<int> Powup
		{
			get
			{
				return this._powup;
			}
		}

		public static string tableName
		{
			get
			{
				return Mst_ship._tableName;
			}
		}

		public List<int> GetEquipList()
		{
			Mst_equip_ship mst_equip_ship = null;
			List<int> result;
			if (Mst_DataManager.Instance.Mst_equip_ship.TryGetValue(this.Id, ref mst_equip_ship))
			{
				result = Enumerable.ToList<int>(mst_equip_ship.Equip);
			}
			else
			{
				result = Enumerable.ToList<int>(Mst_DataManager.Instance.Mst_stype.get_Item(this.Stype).Equip);
			}
			return result;
		}

		public double GetLuckUpKeisu()
		{
			if (this.Id == 163)
			{
				return 1.2;
			}
			if (this.Id == 402)
			{
				return 1.6;
			}
			return 0.0;
		}

		public BtpType GetBtpType()
		{
			return (BtpType)this._btp;
		}

		public int GetRemodelDevKitNum()
		{
			if (this.Afterfuel >= 0 && this.Afterfuel <= 4499)
			{
				return 0;
			}
			if (this.Afterfuel >= 4500 && this.Afterfuel <= 5499)
			{
				return 10;
			}
			if (this.Afterfuel >= 5500 && this.Afterfuel <= 6499)
			{
				return 15;
			}
			if (this.Afterfuel >= 6500 && this.Afterfuel <= 999999)
			{
				return 20;
			}
			return 0;
		}

		protected override void setProperty(XElement element)
		{
			char c = ',';
			this.Id = int.Parse(element.Element("Id").get_Value());
			this.Sortno = int.Parse(element.Element("Sortno").get_Value());
			this.Bookno = int.Parse(element.Element("Bookno").get_Value());
			this.Name = element.Element("Name").get_Value();
			if (this.Name.Equals("なし"))
			{
				return;
			}
			this.Yomi = element.Element("Yomi").get_Value();
			this.Stype = int.Parse(element.Element("Stype").get_Value());
			this.Ctype = int.Parse(element.Element("Ctype").get_Value());
			this.Cnum = int.Parse(element.Element("Cnum").get_Value());
			this.Backs = int.Parse(element.Element("Backs").get_Value());
			string[] array = element.Element("Taik").get_Value().Split(new char[]
			{
				c
			});
			this.Taik = int.Parse(array[0]);
			this.Taik_max = int.Parse(array[1]);
			string[] array2 = element.Element("Souk").get_Value().Split(new char[]
			{
				c
			});
			this.Souk = int.Parse(array2[0]);
			this.Souk_max = int.Parse(array2[1]);
			string[] array3 = element.Element("Kaih").get_Value().Split(new char[]
			{
				c
			});
			this.Kaih = int.Parse(array3[0]);
			this.Kaih_max = int.Parse(array3[1]);
			string[] array4 = element.Element("Tous").get_Value().Split(new char[]
			{
				c
			});
			this.Tous = int.Parse(array4[0]);
			this.Tous_max = int.Parse(array4[1]);
			this.Sokuh = int.Parse(element.Element("Sokuh").get_Value());
			this.Soku = int.Parse(element.Element("Soku").get_Value());
			this.Leng = int.Parse(element.Element("Leng").get_Value());
			string[] array5 = element.Element("Houg").get_Value().Split(new char[]
			{
				c
			});
			this.Houg = int.Parse(array5[0]);
			this.Houg_max = int.Parse(array5[1]);
			string[] array6 = element.Element("Raig").get_Value().Split(new char[]
			{
				c
			});
			this.Raig = int.Parse(array6[0]);
			this.Raig_max = int.Parse(array6[1]);
			string[] array7 = element.Element("Tyku").get_Value().Split(new char[]
			{
				c
			});
			this.Tyku = int.Parse(array7[0]);
			this.Tyku_max = int.Parse(array7[1]);
			string[] array8 = element.Element("Tais").get_Value().Split(new char[]
			{
				c
			});
			this.Tais = int.Parse(array8[0]);
			this.Tais_max = int.Parse(array8[1]);
			string[] array9 = element.Element("Saku").get_Value().Split(new char[]
			{
				c
			});
			this.Saku = int.Parse(array9[0]);
			this.Saku_max = int.Parse(array9[1]);
			string[] array10 = element.Element("Luck").get_Value().Split(new char[]
			{
				c
			});
			this.Luck = int.Parse(array10[0]);
			this.Luck_max = int.Parse(array10[1]);
			this.Slot_num = int.Parse(element.Element("Slot_num").get_Value());
			string[] array11 = element.Element("Maxeq").get_Value().Split(new char[]
			{
				c
			});
			this.Maxeq1 = int.Parse(array11[0]);
			this.Maxeq2 = int.Parse(array11[1]);
			this.Maxeq3 = int.Parse(array11[2]);
			this.Maxeq4 = int.Parse(array11[3]);
			this.Maxeq5 = int.Parse(array11[4]);
			string[] array12 = element.Element("Defeq").get_Value().Split(new char[]
			{
				c
			});
			this.Defeq1 = int.Parse(array12[0]);
			this.Defeq2 = int.Parse(array12[1]);
			this.Defeq3 = int.Parse(array12[2]);
			this.Defeq4 = int.Parse(array12[3]);
			this.Defeq5 = int.Parse(array12[4]);
			if (element.Element("After") != null)
			{
				string[] array13 = element.Element("After").get_Value().Split(new char[]
				{
					c
				});
				this.Afterlv = int.Parse(array13[0]);
				this.Afterfuel = int.Parse(array13[1]);
				this.Afterbull = int.Parse(array13[2]);
				this.Aftershipid = int.Parse(array13[3]);
			}
			this.Buildtime = int.Parse(element.Element("Buildtime").get_Value());
			if (element.Element("Broken") != null)
			{
				string[] array14 = element.Element("Broken").get_Value().Split(new char[]
				{
					c
				});
				this.Broken1 = int.Parse(array14[0]);
				this.Broken2 = int.Parse(array14[1]);
				this.Broken3 = int.Parse(array14[2]);
				this.Broken4 = int.Parse(array14[3]);
			}
			if (element.Element("Powup") != null)
			{
				string[] array15 = element.Element("Powup").get_Value().Split(new char[]
				{
					c
				});
				this.Powup1 = int.Parse(array15[0]);
				this.Powup2 = int.Parse(array15[1]);
				this.Powup3 = int.Parse(array15[2]);
				this.Powup4 = int.Parse(array15[3]);
			}
			this.Use_fuel = int.Parse(element.Element("Use_fuel").get_Value());
			this.Use_bull = int.Parse(element.Element("Use_bull").get_Value());
			this.Fuel_max = int.Parse(element.Element("Fuel_max").get_Value());
			this.Bull_max = int.Parse(element.Element("Bull_max").get_Value());
			string[] array16 = element.Element("Raim").get_Value().Split(new char[]
			{
				c
			});
			this.Raim = int.Parse(array16[0]);
			this.Raim_max = int.Parse(array16[1]);
			this.Append_ship_id = int.Parse(element.Element("Append_ship_id").get_Value());
			this.Event_limited = int.Parse(element.Element("Event_limited").get_Value());
			this._btp = int.Parse(element.Element("Btp").get_Value());
		}

		protected override void setArrayItems()
		{
			List<int> list = new List<int>();
			list.Add(this.Maxeq1);
			list.Add(this.Maxeq2);
			list.Add(this.Maxeq3);
			list.Add(this.Maxeq4);
			list.Add(this.Maxeq5);
			this._maxeq = list;
			list = new List<int>();
			list.Add(this.Defeq1);
			list.Add(this.Defeq2);
			list.Add(this.Defeq3);
			list.Add(this.Defeq4);
			list.Add(this.Defeq5);
			this._defeq = list;
			list = new List<int>();
			list.Add(this.Broken1);
			list.Add(this.Broken2);
			list.Add(this.Broken3);
			list.Add(this.Broken4);
			this._broken = list;
			list = new List<int>();
			list.Add(this.Powup1);
			list.Add(this.Powup2);
			list.Add(this.Powup3);
			list.Add(this.Powup4);
			this._powup = list;
		}
	}
}
