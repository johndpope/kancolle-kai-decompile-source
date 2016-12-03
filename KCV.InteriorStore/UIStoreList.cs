using KCV.View.Scroll;
using local.models;
using System;

namespace KCV.InteriorStore
{
	public class UIStoreList : UIScrollListChild<FurnitureModel>
	{
		public FurnitureModel StoreItem;

		private UILabel _labelName;

		private UILabel _labelPrice;

		private UILabel _labelSoldOut;

		private bool isCheck;

		public bool IsCheckList()
		{
			return this.isCheck;
		}

		protected override void InitializeChildContents(FurnitureModel model, bool clickable)
		{
			this.StoreItem = model;
			this.init();
			this.setList();
		}

		public void init()
		{
			this._labelName = base.get_transform().FindChild("Label_name").GetComponent<UILabel>();
			this._labelPrice = base.get_transform().FindChild("FCoin").GetComponent<UILabel>();
			this._labelSoldOut = base.get_transform().FindChild("SoldOut").GetComponent<UILabel>();
			this.isCheck = false;
		}

		public void setList()
		{
			this._labelName.text = this.StoreItem.Name;
			this._labelPrice.textInt = this.StoreItem.Price;
			this._labelPrice.SetActive(!this.StoreItem.IsPossession());
			this._labelSoldOut.SetActive(this.StoreItem.IsPossession());
		}
	}
}
