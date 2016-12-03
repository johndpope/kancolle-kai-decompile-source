using local.models;
using System;
using UnityEngine;

namespace KCV.View.Scroll
{
	public class RemodelShipListChild : UIScrollListChild<RemodelListShip>
	{
		[SerializeField]
		private UISprite ShipType;

		[SerializeField]
		private UILabel ShipName;

		[SerializeField]
		private UISprite[] ParamIcons;

		[SerializeField]
		private UILabel Level;

		protected override void InitializeChildContents(RemodelListShip ListItem, bool clickable)
		{
			if (ListItem.Option == RemodelListShip.ListItemOption.Option)
			{
				this.ShipName.text = "はずす";
				this.ShipType.spriteName = string.Empty;
				this.Level.text = string.Empty;
				UISprite[] paramIcons = this.ParamIcons;
				for (int i = 0; i < paramIcons.Length; i++)
				{
					UISprite uISprite = paramIcons[i];
					uISprite.set_enabled(false);
				}
			}
			else
			{
				ShipModel shipModel = ListItem.shipModel;
				this.ShipType.spriteName = "ship" + shipModel.ShipType;
				this.ShipName.text = shipModel.Name;
				this.Level.text = "LV " + shipModel.Level.ToString();
				this.ParamIcons[0].set_enabled(shipModel.PowUpKaryoku > 0);
				this.ParamIcons[1].set_enabled(shipModel.PowUpRaisou > 0);
				this.ParamIcons[2].set_enabled(shipModel.PowUpSoukou > 0);
				this.ParamIcons[3].set_enabled(shipModel.PowUpTaikuu > 0);
			}
		}
	}
}
