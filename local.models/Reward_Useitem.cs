using Server_Models;
using System;

namespace local.models
{
	public class Reward_Useitem : IReward, IReward_Useitem
	{
		private Mst_useitem _mst;

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

		public int Count
		{
			get
			{
				return this._count;
			}
		}

		public Reward_Useitem(int mst_id)
		{
			this._Init(mst_id, 1);
		}

		public Reward_Useitem(int mst_id, int count)
		{
			this._Init(mst_id, count);
		}

		public Reward_Useitem(Mst_useitem mst)
		{
			this._Init(mst, 1);
		}

		public Reward_Useitem(Mst_useitem mst, int count)
		{
			this._Init(mst, count);
		}

		private void _Init(int mst_id, int count)
		{
			Mst_DataManager.Instance.Mst_useitem.TryGetValue(mst_id, ref this._mst);
			this._count = count;
		}

		private void _Init(Mst_useitem mst, int count)
		{
			this._mst = mst;
			this._count = count;
		}

		public override string ToString()
		{
			return string.Format("{0}(ID:{1}) 個数:{2}", this.Name, this.Id, this.Count);
		}
	}
}
