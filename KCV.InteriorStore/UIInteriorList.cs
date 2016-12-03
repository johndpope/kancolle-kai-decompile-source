using KCV.View.Scroll;
using local.models;
using System;

namespace KCV.InteriorStore
{
	public class UIInteriorList : UIScrollListChild<FurnitureModel>
	{
		public FurnitureModel Interior;

		private UILabel _labelName;

		private UISprite _spEquip;

		private bool isCheck;

		public bool IsCheckList()
		{
			return this.isCheck;
		}

		private void Start()
		{
		}

		protected override void InitializeChildContents(FurnitureModel model, bool clickable)
		{
			this.Interior = model;
			this.init();
			this.setList();
		}

		public void init()
		{
			this._labelName = base.get_transform().FindChild("Label_name").GetComponent<UILabel>();
			this._spEquip = base.get_transform().FindChild("Equip").GetComponent<UISprite>();
			this.isCheck = false;
		}

		public void setList()
		{
			this._labelName.text = this.Interior.Name;
		}

		private void Update()
		{
		}
	}
}
