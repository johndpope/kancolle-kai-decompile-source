using local.models;
using Server_Common.Formats;
using Server_Controllers;
using System;
using System.Collections.Generic;

namespace local.managers
{
	public class AlbumManager : ManagerBase
	{
		private Dictionary<int, IAlbumModel> _ship;

		private Dictionary<int, IAlbumModel> _slot;

		private int _ship_last_no;

		private int _slot_last_no;

		public int ShipLastNo
		{
			get
			{
				return this._ship_last_no;
			}
		}

		public int SlotLastNo
		{
			get
			{
				return this._slot_last_no;
			}
		}

		public int ShipCount
		{
			get
			{
				return this._ship.get_Count();
			}
		}

		public int SlotCount
		{
			get
			{
				return this._slot.get_Count();
			}
		}

		public void Init()
		{
			if (this._ship != null && this._slot != null)
			{
				return;
			}
			Api_get_Member api_get_Member = new Api_get_Member();
			api_get_Member.InitBookData();
			Api_Result<Dictionary<int, User_BookFmt<BookShipData>>> api_Result = api_get_Member.PictureShip();
			this._ship = new Dictionary<int, IAlbumModel>();
			if (api_Result.state == Api_Result_State.Success)
			{
				using (Dictionary<int, User_BookFmt<BookShipData>>.Enumerator enumerator = api_Result.data.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<int, User_BookFmt<BookShipData>> current = enumerator.get_Current();
						if (current.get_Value() != null)
						{
							this._ship.set_Item(current.get_Key(), new AlbumShipModel(current.get_Value()));
						}
						this._ship_last_no = Math.Max(this._ship_last_no, current.get_Key());
					}
				}
			}
			Api_Result<Dictionary<int, User_BookFmt<BookSlotData>>> api_Result2 = api_get_Member.PictureSlot();
			this._slot = new Dictionary<int, IAlbumModel>();
			if (api_Result2.state == Api_Result_State.Success)
			{
				using (Dictionary<int, User_BookFmt<BookSlotData>>.Enumerator enumerator2 = api_Result2.data.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						KeyValuePair<int, User_BookFmt<BookSlotData>> current2 = enumerator2.get_Current();
						if (current2.get_Value() != null)
						{
							this._slot.set_Item(current2.get_Key(), new AlbumSlotModel(current2.get_Value()));
						}
						this._slot_last_no = Math.Max(this._slot_last_no, current2.get_Key());
					}
				}
			}
		}

		public IAlbumModel[] GetShips(int start_no, int count)
		{
			List<IAlbumModel> list = new List<IAlbumModel>();
			for (int i = 0; i < count; i++)
			{
				list.Add(this.GetShip(start_no + i));
			}
			return list.ToArray();
		}

		public IAlbumModel[] GetShips()
		{
			return this.GetShips(1, this.ShipLastNo);
		}

		public IAlbumModel GetShip(int album_id)
		{
			IAlbumModel result;
			if (!this._ship.TryGetValue(album_id, ref result))
			{
				result = new AlbumBlankModel(album_id);
			}
			return result;
		}

		public IAlbumModel[] GetSlotitems(int start_no, int count)
		{
			List<IAlbumModel> list = new List<IAlbumModel>();
			for (int i = 0; i < count; i++)
			{
				list.Add(this.GetSlotitem(start_no + i));
			}
			return list.ToArray();
		}

		public IAlbumModel[] GetSlotitems()
		{
			return this.GetSlotitems(1, this.SlotLastNo);
		}

		public IAlbumModel GetSlotitem(int album_id)
		{
			IAlbumModel result;
			if (!this._slot.TryGetValue(album_id, ref result))
			{
				result = new AlbumBlankModel(album_id);
			}
			return result;
		}

		public override string ToString()
		{
			string text = base.ToString();
			text = text + "\n艦のデータ列の最後のNo." + this.ShipLastNo;
			text = text + "\t装備のデータ列の最後のNo." + this.SlotLastNo;
			return text + "\n";
		}
	}
}
