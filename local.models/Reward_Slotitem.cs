using Server_Models;
using System;
using System.Collections.Generic;

namespace local.models
{
	public class Reward_Slotitem : IReward, IReward_Slotitem
	{
		private Mst_slotitem _mst;

		private int _count = 1;

		public int Id
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
				return this._mst.Name;
			}
		}

		public int Rare
		{
			get
			{
				return this._mst.Rare;
			}
		}

		public int Count
		{
			get
			{
				return this._count;
			}
		}

		public string Type3Name
		{
			get
			{
				int num = (this._mst != null) ? this._mst.Type3 : 0;
				KeyValuePair<int, string> keyValuePair;
				if (Mst_DataManager.Instance.GetSlotItemEquipTypeName().TryGetValue(num, ref keyValuePair))
				{
					return (keyValuePair.get_Key() != 1) ? string.Empty : keyValuePair.get_Value();
				}
				return string.Empty;
			}
		}

		public Reward_Slotitem(int mst_id)
		{
			this._Init(mst_id, 1);
		}

		public Reward_Slotitem(int mst_id, int count)
		{
			this._Init(mst_id, count);
		}

		public Reward_Slotitem(Mst_slotitem mst)
		{
			this._Init(mst, 1);
		}

		public Reward_Slotitem(Mst_slotitem mst, int count)
		{
			this._Init(mst, count);
		}

		private void _Init(int mst_id, int count)
		{
			Mst_DataManager.Instance.Mst_Slotitem.TryGetValue(mst_id, ref this._mst);
			this._count = count;
		}

		private void _Init(Mst_slotitem mst, int count)
		{
			this._mst = mst;
			this._count = count;
		}

		public override string ToString()
		{
			return string.Format("{0} {1}(ID:{2}) レア度:{3} 個数:{4}", new object[]
			{
				this.Type3Name,
				this.Name,
				this.Id,
				this.Rare,
				this.Count
			});
		}
	}
}
