using Common.Enum;
using Server_Common;
using Server_Models;
using System;

namespace local.models
{
	public class FurnitureModel
	{
		protected Mst_furniture _mst;

		private string _description;

		public int MstId
		{
			get
			{
				return this._mst.Id;
			}
		}

		public string Name
		{
			get
			{
				return this._mst.Title;
			}
		}

		public FurnitureKinds Type
		{
			get
			{
				return (FurnitureKinds)this.TypeId;
			}
		}

		public int TypeId
		{
			get
			{
				return this._mst.Type;
			}
		}

		public int NoInType
		{
			get
			{
				return this._mst.No;
			}
		}

		public int Price
		{
			get
			{
				return this._mst.Price;
			}
		}

		public int Rarity
		{
			get
			{
				return this._mst.Rarity;
			}
		}

		public int SeasonId
		{
			get
			{
				return this._mst.Season;
			}
		}

		public string Description
		{
			get
			{
				return this._description;
			}
		}

		public FurnitureModel(Mst_furniture mst, string description)
		{
			this._mst = mst;
			this._description = description;
		}

		public bool IsSalled()
		{
			return this._mst.Saleflg == 1;
		}

		public bool IsPossession()
		{
			return Comm_UserDatas.Instance.User_furniture.ContainsKey(this.MstId);
		}

		public bool IsNeedWorker()
		{
			return this._mst.IsRequireWorker();
		}

		public virtual bool GetSettingFlg(int deck_id)
		{
			Mem_room mem_room;
			if (Comm_UserDatas.Instance.User_room.TryGetValue(deck_id, ref mem_room))
			{
				switch (this.Type)
				{
				case FurnitureKinds.Floor:
					return mem_room.Furniture1 == this.MstId;
				case FurnitureKinds.Wall:
					return mem_room.Furniture2 == this.MstId;
				case FurnitureKinds.Window:
					return mem_room.Furniture3 == this.MstId;
				case FurnitureKinds.Hangings:
					return mem_room.Furniture4 == this.MstId;
				case FurnitureKinds.Chest:
					return mem_room.Furniture5 == this.MstId;
				case FurnitureKinds.Desk:
					return mem_room.Furniture6 == this.MstId;
				}
			}
			return false;
		}

		public override string ToString()
		{
			return string.Format("{0}(ID:{1}) TypeNo:{2}-{3}", new object[]
			{
				this.Name,
				this.MstId,
				this.TypeId,
				this.NoInType
			});
		}
	}
}
