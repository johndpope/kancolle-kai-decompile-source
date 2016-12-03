using Common.Enum;
using local.models;
using System;
using UnityEngine;

namespace KCV.Strategy.Rebellion
{
	public class DeckSortieInfo : MonoBehaviour
	{
		[SerializeField]
		private CommonShipBanner ShipBanner;

		[SerializeField]
		private UISprite DeckNumberIcon;

		[SerializeField]
		private UILabel ReasonLabel;

		public void SetDeckInfo(DeckModel model, IsGoCondition Condition)
		{
			this.ShipBanner.SetShipData(model.GetFlagShip());
			this.DeckNumberIcon.spriteName = "icon_deck" + model.Id;
			this.ReasonLabel.text = Util.getCancelReason(Condition);
		}
	}
}
