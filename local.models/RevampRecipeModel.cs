using Server_Models;
using System;

namespace local.models
{
	public class RevampRecipeModel : RevampRecipeModelBase
	{
		private SlotitemModel_Mst _slotitem;

		public SlotitemModel_Mst Slotitem
		{
			get
			{
				return this._slotitem;
			}
		}

		public RevampRecipeModel(Mst_slotitem_remodel mst) : base(mst)
		{
			this._slotitem = new SlotitemModel_Mst(mst.Slotitem_id);
		}

		public override string ToString()
		{
			string text = string.Format("{0}(MstId:{1})", this.Slotitem.Name, this.Slotitem.MstId);
			text += string.Format(" 改修必要資材 燃/弾/鋼/ボ:{0}/{1}/{2}/{3}", new object[]
			{
				base.Fuel,
				base.Ammo,
				base.Steel,
				base.Baux
			});
			return text + string.Format(" 開発資材:{0} 改修資材:{1}", this.DevKit, this.RevKit);
		}
	}
}
