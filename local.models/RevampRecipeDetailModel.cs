using Server_Models;
using System;

namespace local.models
{
	public class RevampRecipeDetailModel : RevampRecipeModelBase
	{
		private Mst_slotitem_remodel_detail _mst_detail;

		private SlotitemModel _slotitem;

		public bool Determined;

		public SlotitemModel Slotitem
		{
			get
			{
				return this._slotitem;
			}
		}

		public override int DevKit
		{
			get
			{
				return (!this.Determined) ? this._mst_detail.Req_material5_1 : this._mst_detail.Req_material5_2;
			}
		}

		public override int RevKit
		{
			get
			{
				return (!this.Determined) ? this._mst_detail.Req_material6_1 : this._mst_detail.Req_material6_2;
			}
		}

		public int RequiredSlotitemId
		{
			get
			{
				return this._mst_detail.Req_slotitem_id;
			}
		}

		public int RequiredSlotitemCount
		{
			get
			{
				return this._mst_detail.Req_slotitems;
			}
		}

		public Mst_slotitem_remodel_detail __mst_detail__
		{
			get
			{
				return this._mst_detail;
			}
		}

		public RevampRecipeDetailModel(Mst_slotitem_remodel mst, Mst_slotitem_remodel_detail mst_detail, SlotitemModel slotitem) : base(mst)
		{
			this._mst_detail = mst_detail;
			this._slotitem = slotitem;
		}

		public bool IsChange()
		{
			return this._mst_detail.Change_flag == 1;
		}

		public override string ToString()
		{
			string text = string.Format("{0}(MstId:{1}) Lv:{2}{3}", new object[]
			{
				this.Slotitem.Name,
				this.Slotitem.MstId,
				this.Slotitem.Level,
				(!this.Determined) ? "  \u3000\u3000\u3000\u3000\u3000 " : " [改修確定化]"
			});
			text += string.Format(" 改修必要資材 燃/弾/鋼/ボ:{0}/{1}/{2}/{3}", new object[]
			{
				base.Fuel,
				base.Ammo,
				base.Steel,
				base.Baux
			});
			text += string.Format(" 開発資材:{0} 改修資材:{1}", this.DevKit, this.RevKit);
			text += string.Format(" 変化{0}", (!this.IsChange()) ? "ナシ" : "アリ");
			if (this.RequiredSlotitemCount > 0)
			{
				Mst_slotitem mst_slotitem;
				Mst_DataManager.Instance.Mst_Slotitem.TryGetValue(this.RequiredSlotitemId, ref mst_slotitem);
				text += string.Format(" 要求スロットアイテム:{0}(MstId:{1}) × {2}", mst_slotitem.Name, mst_slotitem.Id, this.RequiredSlotitemCount);
			}
			return text;
		}
	}
}
