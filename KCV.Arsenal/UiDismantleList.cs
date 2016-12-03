using local.models;
using System;
using UnityEngine;

namespace KCV.Arsenal
{
	public class UiDismantleList : MonoBehaviour
	{
		[SerializeField]
		public ShipModel ship;

		[SerializeField]
		private UISprite _teamIcon;

		[SerializeField]
		private UILabel _labelType;

		[SerializeField]
		private UILabel _labelName;

		[SerializeField]
		private UILabel _labelLv;

		[SerializeField]
		private UISprite _check;

		[SerializeField]
		private UISprite _checkBox;

		private int _shipNum;

		private bool isCheck;

		public bool IsCheckList()
		{
			return this.isCheck;
		}

		public void init()
		{
			this._labelType = base.get_transform().FindChild("LabelType").GetComponent<UILabel>();
			this._labelName = base.get_transform().FindChild("LabelName").GetComponent<UILabel>();
			this._labelLv = base.get_transform().FindChild("LabelLevel").GetComponent<UILabel>();
			this._check = base.get_transform().FindChild("Check").GetComponent<UISprite>();
			this._checkBox = base.get_transform().FindChild("CheckBox").GetComponent<UISprite>();
			this._check.alpha = 0f;
			this.isCheck = false;
		}

		public void setShip(int num, ShipModel _ship)
		{
			this._shipNum = num;
			this.ship = _ship;
			this._labelType.text = this.ship.ShipTypeName;
			this._labelName.text = this.ship.Name;
			this._labelLv.textInt = this.ship.Level;
		}

		public void setTeamIcon()
		{
			ShipModel[] shipList = TaskMainArsenalManager.arsenalManager.GetShipList();
			if (shipList[this._shipNum].IsInDeck() == 0)
			{
				this._teamIcon.alpha = 1f;
				this._teamIcon.spriteName = "icon-d1_k";
			}
			else if (shipList[this._shipNum].IsInDeck() == -1)
			{
				this._teamIcon.alpha = 1f;
				this._teamIcon.spriteName = "icon-d" + shipList[this._shipNum].IsInDeck() + "_on";
			}
			else
			{
				this._teamIcon.alpha = 0f;
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
