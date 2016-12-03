using Common.Enum;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Server_Models
{
	[DataContract(Name = "mem_room", Namespace = "")]
	public class Mem_room : Model_Base
	{
		[DataMember]
		private int _rid;

		[DataMember]
		private int _furniture1;

		[DataMember]
		private int _furniture2;

		[DataMember]
		private int _furniture3;

		[DataMember]
		private int _furniture4;

		[DataMember]
		private int _furniture5;

		[DataMember]
		private int _furniture6;

		[DataMember]
		private int _bgm_id;

		[DataMember]
		private int _jukebox_bgm_id;

		private static string _tableName = "mem_room";

		public int Rid
		{
			get
			{
				return this._rid;
			}
			private set
			{
				this._rid = value;
			}
		}

		public int Furniture1
		{
			get
			{
				return this._furniture1;
			}
			private set
			{
				this._furniture1 = value;
			}
		}

		public int Furniture2
		{
			get
			{
				return this._furniture2;
			}
			private set
			{
				this._furniture2 = value;
			}
		}

		public int Furniture3
		{
			get
			{
				return this._furniture3;
			}
			private set
			{
				this._furniture3 = value;
			}
		}

		public int Furniture4
		{
			get
			{
				return this._furniture4;
			}
			private set
			{
				this._furniture4 = value;
			}
		}

		public int Furniture5
		{
			get
			{
				return this._furniture5;
			}
			private set
			{
				this._furniture5 = value;
			}
		}

		public int Furniture6
		{
			get
			{
				return this._furniture6;
			}
			private set
			{
				this._furniture6 = value;
			}
		}

		public int Bgm_id
		{
			get
			{
				return this._bgm_id;
			}
			private set
			{
				this._bgm_id = value;
			}
		}

		public static string tableName
		{
			get
			{
				return Mem_room._tableName;
			}
		}

		public int this[FurnitureKinds kind]
		{
			get
			{
				if (kind == FurnitureKinds.Floor)
				{
					return this.Furniture1;
				}
				if (kind == FurnitureKinds.Wall)
				{
					return this.Furniture2;
				}
				if (kind == FurnitureKinds.Window)
				{
					return this.Furniture3;
				}
				if (kind == FurnitureKinds.Hangings)
				{
					return this.Furniture4;
				}
				if (kind == FurnitureKinds.Chest)
				{
					return this.Furniture5;
				}
				return this.Furniture6;
			}
			private set
			{
				if (kind == FurnitureKinds.Floor)
				{
					this.Furniture1 = value;
				}
				else if (kind == FurnitureKinds.Wall)
				{
					this.Furniture2 = value;
				}
				else if (kind == FurnitureKinds.Window)
				{
					this.Furniture3 = value;
				}
				else if (kind == FurnitureKinds.Hangings)
				{
					this.Furniture4 = value;
				}
				else if (kind == FurnitureKinds.Chest)
				{
					this.Furniture5 = value;
				}
				else if (kind == FurnitureKinds.Desk)
				{
					this.Furniture6 = value;
				}
			}
		}

		public Mem_room()
		{
			this.Furniture1 = 1;
			this.Furniture2 = 38;
			this.Furniture3 = 72;
			this.Furniture4 = 102;
			this.Furniture5 = 133;
			this.Furniture6 = 164;
			this.Bgm_id = 101;
			this._jukebox_bgm_id = 101;
		}

		public Mem_room(int deck_rid) : this()
		{
			this.Rid = deck_rid;
		}

		public List<Mst_furniture> getFurnitureDatas()
		{
			List<Mst_furniture> list = new List<Mst_furniture>();
			list.Add(Mst_DataManager.Instance.Mst_furniture.get_Item(this.Furniture1));
			list.Add(Mst_DataManager.Instance.Mst_furniture.get_Item(this.Furniture2));
			list.Add(Mst_DataManager.Instance.Mst_furniture.get_Item(this.Furniture3));
			list.Add(Mst_DataManager.Instance.Mst_furniture.get_Item(this.Furniture4));
			list.Add(Mst_DataManager.Instance.Mst_furniture.get_Item(this.Furniture5));
			list.Add(Mst_DataManager.Instance.Mst_furniture.get_Item(this.Furniture6));
			return list;
		}

		public void SetFurniture(FurnitureKinds kind, int id)
		{
			this[kind] = id;
		}

		public void SetFurniture(FurnitureKinds kind, int id, int bgmId)
		{
			this[kind] = id;
			this.Bgm_id = ((bgmId != 0) ? bgmId : this._jukebox_bgm_id);
		}

		public void SetPortBgmFromJuke(int music_id)
		{
			this.Bgm_id = music_id;
			this._jukebox_bgm_id = music_id;
		}

		protected override void setProperty(XElement element)
		{
			this.Rid = int.Parse(element.Element("_rid").get_Value());
			this.Furniture1 = int.Parse(element.Element("_furniture1").get_Value());
			this.Furniture2 = int.Parse(element.Element("_furniture2").get_Value());
			this.Furniture3 = int.Parse(element.Element("_furniture3").get_Value());
			this.Furniture4 = int.Parse(element.Element("_furniture4").get_Value());
			this.Furniture5 = int.Parse(element.Element("_furniture5").get_Value());
			this.Furniture6 = int.Parse(element.Element("_furniture6").get_Value());
			this.Bgm_id = int.Parse(element.Element("_bgm_id").get_Value());
			this._jukebox_bgm_id = int.Parse(element.Element("_jukebox_bgm_id").get_Value());
		}
	}
}
