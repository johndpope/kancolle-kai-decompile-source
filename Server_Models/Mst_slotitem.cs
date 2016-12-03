using System;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_slotitem : Model_Base
	{
		private int _id;

		private int _sortno;

		private string _name;

		private int _type1;

		private int _type2;

		private int _type3;

		private int _api_mapbattle_type3;

		private int _type4;

		private int _taik;

		private int _souk;

		private int _houg;

		private int _raig;

		private int _baku;

		private int _tyku;

		private int _tais;

		private int _houm;

		private int _raim;

		private int _houk;

		private int _saku;

		private int _leng;

		private int _default_exp;

		private int _exp_rate;

		private int _rare;

		private int _broken1;

		private int _broken2;

		private int _broken3;

		private int _broken4;

		private int _flag_houg;

		private int _flag_raig;

		private int _flag_kraig;

		private int _flag_kbaku;

		private int _flag_tyku;

		private int _flag_ktyku;

		private int _flag_saku;

		private int _flag_sakb;

		private static string _tableName = "mst_slotitem";

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

		public int Type1
		{
			get
			{
				return this._type1;
			}
			private set
			{
				this._type1 = value;
			}
		}

		public int Type2
		{
			get
			{
				return this._type2;
			}
			private set
			{
				this._type2 = value;
			}
		}

		public int Type3
		{
			get
			{
				return this._type3;
			}
			private set
			{
				this._type3 = value;
			}
		}

		public int Api_mapbattle_type3
		{
			get
			{
				return this._api_mapbattle_type3;
			}
			private set
			{
				this._api_mapbattle_type3 = value;
			}
		}

		public int Type4
		{
			get
			{
				return this._type4;
			}
			private set
			{
				this._type4 = value;
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

		public int Baku
		{
			get
			{
				return this._baku;
			}
			private set
			{
				this._baku = value;
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

		public int Houm
		{
			get
			{
				return this._houm;
			}
			private set
			{
				this._houm = value;
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

		public int Houk
		{
			get
			{
				return this._houk;
			}
			private set
			{
				this._houk = value;
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

		public int Default_exp
		{
			get
			{
				return this._default_exp;
			}
			private set
			{
				this._default_exp = value;
			}
		}

		public int Exp_rate
		{
			get
			{
				return this._exp_rate;
			}
			private set
			{
				this._exp_rate = value;
			}
		}

		public int Rare
		{
			get
			{
				return this._rare;
			}
			private set
			{
				this._rare = value;
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

		public int Flag_houg
		{
			get
			{
				return this._flag_houg;
			}
			private set
			{
				this._flag_houg = value;
			}
		}

		public int Flag_raig
		{
			get
			{
				return this._flag_raig;
			}
			private set
			{
				this._flag_raig = value;
			}
		}

		public int Flag_kraig
		{
			get
			{
				return this._flag_kraig;
			}
			private set
			{
				this._flag_kraig = value;
			}
		}

		public int Flag_kbaku
		{
			get
			{
				return this._flag_kbaku;
			}
			private set
			{
				this._flag_kbaku = value;
			}
		}

		public int Flag_tyku
		{
			get
			{
				return this._flag_tyku;
			}
			private set
			{
				this._flag_tyku = value;
			}
		}

		public int Flag_ktyku
		{
			get
			{
				return this._flag_ktyku;
			}
			private set
			{
				this._flag_ktyku = value;
			}
		}

		public int Flag_saku
		{
			get
			{
				return this._flag_saku;
			}
			private set
			{
				this._flag_saku = value;
			}
		}

		public int Flag_sakb
		{
			get
			{
				return this._flag_sakb;
			}
			private set
			{
				this._flag_sakb = value;
			}
		}

		public static string tableName
		{
			get
			{
				return Mst_slotitem._tableName;
			}
		}

		public bool IsDentan()
		{
			return this.Type3 == 12 || this.Type3 == 13 || this.Type3 == 93;
		}

		protected override void setProperty(XElement element)
		{
			this.Id = int.Parse(element.Element("Id").get_Value());
			this.Sortno = int.Parse(element.Element("Sortno").get_Value());
			this.Name = element.Element("Name").get_Value();
			string[] array = element.Element("Types").get_Value().Split(new char[]
			{
				','
			});
			this.Type1 = int.Parse(array[0]);
			this.Type2 = int.Parse(array[1]);
			int num = int.Parse(array[2]);
			this.Type3 = this.getRewirteType3No(this.Id, num);
			this.Api_mapbattle_type3 = num;
			this.Type4 = int.Parse(array[3]);
			this.Taik = int.Parse(element.Element("Taik").get_Value());
			this.Souk = int.Parse(element.Element("Souk").get_Value());
			this.Houg = int.Parse(element.Element("Houg").get_Value());
			this.Raig = int.Parse(element.Element("Raig").get_Value());
			this.Baku = int.Parse(element.Element("Baku").get_Value());
			this.Tyku = int.Parse(element.Element("Tyku").get_Value());
			this.Tais = int.Parse(element.Element("Tais").get_Value());
			this.Houm = int.Parse(element.Element("Houm").get_Value());
			this.Raim = int.Parse(element.Element("Raim").get_Value());
			this.Houk = int.Parse(element.Element("Houk").get_Value());
			this.Saku = int.Parse(element.Element("Saku").get_Value());
			this.Leng = int.Parse(element.Element("Leng").get_Value());
			this.Default_exp = int.Parse(element.Element("Default_exp").get_Value());
			this.Exp_rate = int.Parse(element.Element("Exp_rate").get_Value());
			this.Rare = int.Parse(element.Element("Rare").get_Value());
			if (element.Element("Brokens") != null)
			{
				string[] array2 = element.Element("Brokens").get_Value().Split(new char[]
				{
					','
				});
				this.Broken1 = int.Parse(array2[0]);
				this.Broken2 = int.Parse(array2[1]);
				this.Broken3 = int.Parse(array2[2]);
				this.Broken4 = int.Parse(array2[3]);
			}
		}

		private int getRewirteType3No(int id, int nowType3No)
		{
			if (id == 128)
			{
				return 38;
			}
			if (id == 142)
			{
				return 93;
			}
			if (id == 151)
			{
				return 94;
			}
			return nowType3No;
		}
	}
}
