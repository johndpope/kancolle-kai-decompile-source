using Server_Common.Formats;
using Server_Models;
using System;

namespace local.models
{
	public class AlbumShipModel : ShipModelMst, IAlbumModel
	{
		private User_BookFmt<BookShipData> _fmt;

		public int Id
		{
			get
			{
				return this._fmt.IndexNo;
			}
		}

		public string Detail
		{
			get
			{
				return this._fmt.Detail.Info;
			}
		}

		public int[] MstIDs
		{
			get
			{
				return this._fmt.Ids.ToArray();
			}
		}

		public string ClassTypeName
		{
			get
			{
				return this._fmt.Detail.ClassName;
			}
		}

		public AlbumShipModel(User_BookFmt<BookShipData> fmt)
		{
			this._fmt = fmt;
			this._mst_data = this._fmt.Detail.MstData;
		}

		public bool IsDamaged()
		{
			return this.IsDamaged(base.MstId);
		}

		public bool IsDamaged(int mst_id)
		{
			int num = this._fmt.Ids.IndexOf(mst_id);
			return num >= 0 && this._fmt.State.get_Item(num).get_Item(1) == 1;
		}

		public bool IsMarriage()
		{
			return this.IsMarriage(base.MstId);
		}

		public bool IsMarriage(int mst_id)
		{
			int num = this._fmt.Ids.IndexOf(mst_id);
			return num >= 0 && this._fmt.State.get_Item(num).get_Item(2) == 1;
		}

		public override string ToString()
		{
			return this.ToString(false);
		}

		public string ToString(bool detail)
		{
			string text = string.Format("図鑑ID:{0} {1}(MstId:{2}) {3}", new object[]
			{
				this.Id,
				base.Name,
				base.MstId,
				this.ClassTypeName
			});
			text += ((!this.IsMarriage()) ? " [未婚]" : " [結婚]");
			if (detail)
			{
				text += string.Format(" {0}\n", base.ShipTypeName);
				int[] mstIDs = this.MstIDs;
				for (int i = 0; i < mstIDs.Length; i++)
				{
					text += string.Format(" - 表示する艦ID:{0}({3}) ダメージ絵:{1} {2}\n", new object[]
					{
						mstIDs[i],
						(!this.IsDamaged(mstIDs[i])) ? "ナシ" : "アリ",
						(!this.IsMarriage(mstIDs[i])) ? "未婚" : "結婚",
						Mst_DataManager.Instance.Mst_ship.get_Item(mstIDs[i]).Name
					});
				}
				text += string.Format("火力:{0} 雷装:{1} 対空:{2} 回避:{3} 耐久:{4}", new object[]
				{
					this.Karyoku,
					this.Raisou,
					this.Taiku,
					this.Kaihi,
					this.Taikyu
				});
				text += "\n";
				text += this.Detail;
			}
			return text;
		}
	}
}
