using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_stype : Model_Base
	{
		private int _id;

		private int _sortno;

		private string _name;

		private int _scnt;

		private int _kcnt;

		private List<int> _equip;

		private static string _tableName = "mst_stype";

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

		public int Scnt
		{
			get
			{
				return this._scnt;
			}
			private set
			{
				this._scnt = value;
			}
		}

		public int Kcnt
		{
			get
			{
				return this._kcnt;
			}
			private set
			{
				this._kcnt = value;
			}
		}

		public List<int> Equip
		{
			get
			{
				return this._equip;
			}
			private set
			{
				this._equip = value;
			}
		}

		public static string tableName
		{
			get
			{
				return Mst_stype._tableName;
			}
		}

		protected override void setProperty(XElement element)
		{
			this.Id = int.Parse(element.Element("Id").get_Value());
			this.Sortno = int.Parse(element.Element("Sortno").get_Value());
			this.Name = element.Element("Name").get_Value();
			this.Scnt = int.Parse(element.Element("Scnt").get_Value());
			this.Kcnt = int.Parse(element.Element("Kcnt").get_Value());
		}

		public void SetEquip(List<int> equips)
		{
			if (this.Equip != null)
			{
				return;
			}
			this.Equip = equips;
		}

		public bool IsSubmarine()
		{
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(13);
			hashSet.Add(14);
			HashSet<int> hashSet2 = hashSet;
			return hashSet2.Contains(this.Id);
		}

		public bool IsMother()
		{
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(7);
			hashSet.Add(11);
			hashSet.Add(18);
			HashSet<int> hashSet2 = hashSet;
			return hashSet2.Contains(this.Id);
		}

		public bool IsLandFacillity(int soku)
		{
			return soku == 0;
		}

		public bool IsBattleShip()
		{
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(8);
			hashSet.Add(9);
			hashSet.Add(10);
			hashSet.Add(12);
			HashSet<int> hashSet2 = hashSet;
			return hashSet2.Contains(this.Id);
		}

		public bool IsTrainingShip()
		{
			return this.Id == 21;
		}

		public bool IsKouku()
		{
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(11);
			hashSet.Add(18);
			hashSet.Add(7);
			hashSet.Add(10);
			hashSet.Add(6);
			hashSet.Add(16);
			hashSet.Add(17);
			hashSet.Add(22);
			hashSet.Add(14);
			HashSet<int> hashSet2 = hashSet;
			return hashSet2.Contains(this.Id);
		}
	}
}
