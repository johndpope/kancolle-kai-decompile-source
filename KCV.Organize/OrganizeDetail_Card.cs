using local.models;
using System;
using UnityEngine;

namespace KCV.Organize
{
	public class OrganizeDetail_Card : MonoBehaviour
	{
		[SerializeField]
		private UITexture ShipCard;

		[SerializeField]
		private UISprite StateIcon;

		[SerializeField]
		private UISprite FatigueIcon;

		[SerializeField]
		private UISprite FatigueMask;

		[SerializeField]
		private Transform Ring;

		public void SetShipCard(ShipModel ship)
		{
			BaseShipCard component = base.GetComponent<BaseShipCard>();
			component.Init(ship, this.ShipCard);
			component.UpdateFatigue(ship.ConditionState, this.FatigueIcon, this.FatigueMask);
			component.UpdateStateIcon(this.StateIcon);
			if (ship.IsMarriage())
			{
				this.Ring.SetActive(true);
			}
			else
			{
				this.Ring.SetActive(false);
			}
		}

		public void Release()
		{
			Resources.UnloadAsset(this.ShipCard.mainTexture);
		}

		private void OnDestroy()
		{
			this.ShipCard = null;
			this.StateIcon = null;
			this.FatigueIcon = null;
			this.FatigueMask = null;
			this.Ring = null;
		}
	}
}
