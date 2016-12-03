using local.managers;
using local.models;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KCV.SortieMap
{
	public class WobblingIcons : IDisposable
	{
		private List<UIWobblingIcon> _listIcons;

		public List<UIWobblingIcon> wobblingIcons
		{
			get
			{
				return this._listIcons;
			}
		}

		public WobblingIcons(MapManager manager, Transform target)
		{
			WobblingIcons <>f__this = this;
			this._listIcons = new List<UIWobblingIcon>();
			this._listIcons.Add(null);
			Enumerable.Skip<CellModel>(manager.Cells, 1).ForEach(delegate(CellModel x)
			{
				Transform transform = target.FindChild(string.Format("UIWobblingIcon{0}", x.CellNo));
				<>f__this._listIcons.Add((!(transform == null)) ? transform.GetComponent<UIWobblingIcon>() : null);
			});
			if (manager.Map.MstId == 127)
			{
				this.SPProcessMap127();
			}
		}

		private void SPProcessMap127()
		{
			this.wobblingIcons.set_Item(6, this.wobblingIcons.get_Item(5));
			this.wobblingIcons.set_Item(5, null);
			this.wobblingIcons.set_Item(8, null);
		}

		public void DestroyDrawWobblingIcons()
		{
			Enumerable.Where<UIWobblingIcon>(this._listIcons, (UIWobblingIcon x) => x != null && x.isWobbling).ForEach(delegate(UIWobblingIcon x)
			{
				Mem.DelComponentSafe<UIWobblingIcon>(ref x);
			});
		}

		public void Dispose()
		{
			Mem.DelListSafe<UIWobblingIcon>(ref this._listIcons);
		}

		public bool FixedRun()
		{
			for (int i = 0; i < this._listIcons.get_Count(); i++)
			{
				if (this._listIcons.get_Item(i) != null)
				{
					this._listIcons.get_Item(i).FixedRun();
				}
			}
			return true;
		}
	}
}
