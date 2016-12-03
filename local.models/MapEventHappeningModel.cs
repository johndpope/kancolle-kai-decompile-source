using Common.Enum;
using local.utils;
using Server_Common.Formats;
using System;

namespace local.models
{
	public class MapEventHappeningModel
	{
		private int _item_id;

		private int _count;

		private bool _dentan;

		public enumMaterialCategory Material
		{
			get
			{
				return (enumMaterialCategory)this._item_id;
			}
		}

		public int Count
		{
			get
			{
				return this._count;
			}
		}

		public bool Dentan
		{
			get
			{
				return this._dentan;
			}
		}

		public MapEventHappeningModel(MapHappningFmt fmt)
		{
			this._item_id = fmt.Id;
			this._count = fmt.Count;
			this._dentan = fmt.Dentan;
		}

		public override string ToString()
		{
			string text = Utils.enumMaterialCategoryToString(this.Material);
			return string.Format("{0}(id:{1}) Count:{2} 電探効果:{3}", new object[]
			{
				text,
				this._item_id,
				this._count,
				(!this.Dentan) ? "無" : "有"
			});
		}
	}
}
