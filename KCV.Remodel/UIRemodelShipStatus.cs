using KCV.Scene.Port;
using KCV.Utils;
using local.models;
using System;
using UnityEngine;

namespace KCV.Remodel
{
	[SelectionBase]
	public class UIRemodelShipStatus : MonoBehaviour, UIRemodelView
	{
		[SerializeField]
		private UITexture shipTexture;

		private ShipModel ship;

		private Vector3 hidePos = new Vector3(-600f, 125f);

		private Vector3 showPos = new Vector3(-462f, 31f);

		private Vector3 showPos2 = new Vector3(-462f, 31f);

		public void Init(ShipModel ship)
		{
			this.ship = ship;
			if (this.shipTexture.mainTexture != null)
			{
				Resources.UnloadAsset(this.shipTexture.mainTexture);
				this.shipTexture.mainTexture = null;
				UIDrawCall.ReleaseInactive();
			}
			this.shipTexture.mainTexture = null;
			this.shipTexture.mainTexture = ShipUtils.LoadTexture(ship);
			this.shipTexture.MakePixelPerfect();
			this.shipTexture.ResizeCollider();
			this.shipTexture.get_transform().set_localPosition(Util.Poi2Vec(new ShipOffset(ship.GetGraphicsMstId()).GetSlotItemCategory(ship.IsDamaged())) + Vector3.get_down() * 20f);
			this.shipTexture.get_transform().set_localScale(Vector3.get_one() * 1.1f);
			this.Show();
		}

		public void Show()
		{
			base.get_gameObject().SetActive(true);
			if (UserInterfaceRemodelManager.instance.focusedDeckModel == null)
			{
				base.get_transform().set_localPosition(this.showPos2);
			}
			else
			{
				base.get_transform().set_localPosition(this.showPos);
			}
		}

		public void ShowMove()
		{
			base.get_transform().set_localPosition(this.showPos);
		}

		public void Hide()
		{
			base.get_transform().set_localPosition(this.hidePos);
			base.get_gameObject().SetActive(false);
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref this.shipTexture, false);
			this.ship = null;
		}
	}
}
