using KCV.View.Scroll;
using local.models;
using System;
using UnityEngine;

namespace KCV.Arsenal
{
	public class UiItemListChild : UIScrollListChild<SlotitemModel>
	{
		public SlotitemModel Slotitem;

		[SerializeField]
		private UILabel _labelRea;

		[SerializeField]
		private UILabel _labelReaStars;

		[SerializeField]
		private UILabel _labelName;

		[SerializeField]
		private UISprite _check;

		[SerializeField]
		private UISprite _checkBox;

		[SerializeField]
		private UISprite _SlotItem;

		[SerializeField]
		private UISprite _LockIcon;

		private int _itemNum;

		private bool _isCheck;

		public bool IsCheckList()
		{
			return this._isCheck;
		}

		protected override void InitializeChildContents(SlotitemModel model, bool clickable)
		{
			this.Slotitem = model;
			this.Refresh();
			this.setItem();
		}

		public void Refresh()
		{
			this._check.alpha = 0f;
			this._isCheck = false;
		}

		public void setItem()
		{
			string text = string.Empty;
			for (int i = 0; i <= ((this.Slotitem.Rare >= 5) ? 4 : this.Slotitem.Rare); i++)
			{
				text += "★";
			}
			this._labelRea.text = Util.RareToString(this.Slotitem.Rare);
			this._labelReaStars.text = text;
			this._labelName.text = this.Slotitem.Name;
			this._SlotItem.spriteName = "icon_slot" + this.Slotitem.Type4;
			this._LockIcon.spriteName = ((!this.Slotitem.IsLocked()) ? null : "lock_on");
			if (TaskMainArsenalManager.arsenalManager.IsSelected(this.Slotitem.MemId))
			{
				this.UpdateListSet(true);
			}
			else
			{
				this.UpdateListSet(false);
			}
		}

		public void UpdateListSelect(bool enabled)
		{
			this._checkBox.spriteName = ((!enabled) ? "check_bg" : "check_bg_on");
		}

		public void UpdateListSet(bool enabled)
		{
			this._check.alpha = ((!enabled) ? 0f : 1f);
			this._isCheck = enabled;
		}
	}
}
