using local.models;
using System;
using UnityEngine;

namespace KCV.Strategy.Deploy
{
	[RequireComponent(typeof(UIWidget))]
	public class DeployShip : MonoBehaviour
	{
		private UIWidget mWidgetThis;

		[SerializeField]
		private UISprite mSprite_ShipTypeIcon;

		private Vector3 mDefaultLocalPosition;

		private void Awake()
		{
			this.mWidgetThis = base.GetComponent<UIWidget>();
			this.mDefaultLocalPosition = this.mSprite_ShipTypeIcon.get_transform().get_localPosition();
		}

		public void Initialize(ShipModel shipModel)
		{
			int shipType = shipModel.ShipType;
			this.SetShipTypeIcon(shipType);
			TweenPosition tweenPosition = TweenPosition.Begin(this.mSprite_ShipTypeIcon.get_gameObject(), 2f, this.mDefaultLocalPosition + new Vector3(0f, 3f, 0f));
			tweenPosition.from = this.mDefaultLocalPosition;
			tweenPosition.style = UITweener.Style.PingPong;
		}

		public void InitializeDefailt()
		{
			this.mSprite_ShipTypeIcon.spriteName = string.Empty;
			this.mWidgetThis.alpha = 1E-05f;
		}

		private void SetShipTypeIcon(int shipTypeId)
		{
			this.mSprite_ShipTypeIcon.spriteName = string.Format("shipicon_{0}", shipTypeId);
			this.mWidgetThis.alpha = 1f;
		}

		private void SetShipTypeIconColor(Color color)
		{
			this.mSprite_ShipTypeIcon.color = color;
		}
	}
}
