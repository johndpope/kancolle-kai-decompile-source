using KCV.View.Scroll;
using local.models;
using System;
using UnityEngine;

namespace KCV.Arsenal
{
	public class UiDismantleListChild : UIScrollListChild<ShipModel>
	{
		public ShipModel ship;

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

		[SerializeField]
		private UISprite _ShipType;

		[SerializeField]
		private UISprite _LockType;

		private int _shipNum;

		private bool isCheck;

		public bool IsCheckList()
		{
			return this.isCheck;
		}

		protected override void InitializeChildContents(ShipModel model, bool clickable)
		{
			this.ship = model;
			this.refresh();
			this.setShip(model);
		}

		public void refresh()
		{
			this._check.alpha = 0f;
			this.isCheck = false;
		}

		public void setShip(ShipModel _ship)
		{
			this.ship = _ship;
			this._ShipType.spriteName = "ship" + this.ship.ShipType;
			this._labelName.text = this.ship.Name;
			this._labelLv.textInt = this.ship.Level;
			if (this.ship.IsLocked())
			{
				this._LockType.spriteName = "lock_ship";
			}
			else if (this.ship.HasLocked())
			{
				this._LockType.spriteName = "lock_on";
			}
			else
			{
				this._LockType.spriteName = null;
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
