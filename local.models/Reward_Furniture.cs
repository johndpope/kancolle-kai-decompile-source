using Common.Enum;
using Server_Models;
using System;
using System.Collections.Generic;

namespace local.models
{
	public class Reward_Furniture : IReward
	{
		private int _mst_id;

		private Mst_furniture _mst;

		public int MstId
		{
			get
			{
				return this._mst_id;
			}
		}

		public string Name
		{
			get
			{
				return (this._mst == null) ? string.Empty : this._mst.Title;
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
				return (this._mst == null) ? 0 : this._mst.Type;
			}
		}

		public int NoInType
		{
			get
			{
				return (this._mst == null) ? 0 : this._mst.No;
			}
		}

		public Reward_Furniture(int mst_id)
		{
			this._mst_id = mst_id;
		}

		public void __Init__(Dictionary<FurnitureKinds, List<Mst_furniture>> all_mst)
		{
			using (Dictionary<FurnitureKinds, List<Mst_furniture>>.ValueCollection.Enumerator enumerator = all_mst.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mst_furniture> current = enumerator.get_Current();
					this._mst = current.Find((Mst_furniture item) => item.Id == this._mst_id);
					if (this._mst != null)
					{
						break;
					}
				}
			}
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
