using Common.Enum;
using local.utils;
using Server_Common.Formats;
using Server_Models;
using System;

namespace local.models
{
	public class MapEventItemModel
	{
		private MapItemGetFmt.enumCategory _category;

		private int _item_id;

		private int _count;

		public int ItemID
		{
			get
			{
				return this._item_id;
			}
		}

		public int Count
		{
			get
			{
				return this._count;
			}
		}

		public enumMaterialCategory MaterialCategory
		{
			get
			{
				return (enumMaterialCategory)((!this.IsMaterial()) ? 0 : this._item_id);
			}
		}

		public MapEventItemModel(MapItemGetFmt fmt)
		{
			if (fmt == null)
			{
				this._category = MapItemGetFmt.enumCategory.None;
				this._item_id = 0;
				this._count = 0;
			}
			else
			{
				this._category = fmt.Category;
				this._item_id = fmt.Id;
				this._count = fmt.GetCount;
			}
		}

		public bool IsMaterial()
		{
			return this._category == MapItemGetFmt.enumCategory.Material;
		}

		public bool IsUseItem()
		{
			return this._category == MapItemGetFmt.enumCategory.UseItem;
		}

		public override string ToString()
		{
			if (this.IsMaterial())
			{
				string text = Utils.enumMaterialCategoryToString(this.MaterialCategory);
				return string.Format("{0}(id:{1}) Count:{2}", text, this._item_id, this._count);
			}
			if (this.IsUseItem())
			{
				Mst_useitem mst_useitem = Mst_DataManager.Instance.Mst_useitem.get_Item(this.ItemID);
				return string.Format("使用アイテム:{0}(id:{1}) Count:{2}", mst_useitem.Name, this.ItemID, this.Count);
			}
			return string.Format("[Item None]", new object[0]);
		}
	}
}
