using System;
using System.Collections.Generic;

namespace local.models
{
	public class Reward_Useitems : IReward
	{
		private List<IReward_Useitem> _use_items;

		public IReward_Useitem[] Rewards
		{
			get
			{
				return this._use_items.ToArray();
			}
		}

		public Reward_Useitems()
		{
			this._use_items = new List<IReward_Useitem>();
		}

		public void __Add__(int mst_id, int count)
		{
			int num = this._use_items.FindIndex((IReward_Useitem i) => i.Id == mst_id);
			if (num == -1)
			{
				this._use_items.Add(new Reward_Useitem(mst_id, count));
				this._use_items.Sort((IReward_Useitem a, IReward_Useitem b) => a.Id - b.Id);
			}
			else
			{
				IReward_Useitem reward_Useitem = this._use_items.get_Item(num);
				this._use_items.set_Item(num, new Reward_Useitem(mst_id, count + reward_Useitem.Count));
			}
		}

		public override string ToString()
		{
			string text = string.Format("複数使用アイテム:[ ", new object[0]);
			for (int i = 0; i < this._use_items.get_Count(); i++)
			{
				text += string.Format("({0})", this._use_items.get_Item(i));
				if (i < this._use_items.get_Count() - 1)
				{
					text += ", ";
				}
			}
			return text + "]";
		}
	}
}
