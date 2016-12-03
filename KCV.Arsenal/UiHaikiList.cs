using local.models;
using System;
using UnityEngine;

namespace KCV.Arsenal
{
	public class UiHaikiList : MonoBehaviour
	{
		public SlotitemModel item;

		[SerializeField]
		private UILabel _labelRea;

		[SerializeField]
		private UILabel _labelName;

		[SerializeField]
		private UISprite _check;

		[SerializeField]
		private UISprite _checkBox;

		private int _itemNum;

		private bool isCheck;

		public bool IsCheckList()
		{
			return this.isCheck;
		}

		public void init()
		{
			this._labelRea = base.get_transform().FindChild("LabelRea").GetComponent<UILabel>();
			this._labelName = base.get_transform().FindChild("LabelName").GetComponent<UILabel>();
			this._check = base.get_transform().FindChild("Check").GetComponent<UISprite>();
			this._checkBox = base.get_transform().FindChild("CheckBox").GetComponent<UISprite>();
			this._check.alpha = 0f;
			this.isCheck = false;
		}

		public void setItem(int num, SlotitemModel item)
		{
			this._itemNum = num;
			this.item = item;
			this._labelRea.textInt = this.item.Rare;
			this._labelName.text = this.item.Name;
			if (TaskMainArsenalManager.arsenalManager.IsSelected(this.item.MemId))
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
			if (enabled)
			{
				this._checkBox.spriteName = "check_bg_on";
			}
			else
			{
				this._checkBox.spriteName = "check_bg";
			}
		}

		public void UpdateListSet(bool enabled)
		{
			if (enabled)
			{
				this._check.alpha = 1f;
				this.isCheck = true;
			}
			else
			{
				this._check.alpha = 0f;
				this.isCheck = false;
			}
		}
	}
}
