using Common.Enum;
using local.models;
using System;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategySideEscortDeck : MonoBehaviour
	{
		private const string SENKAN = "icon_ship1";

		private const string ZYUNYOU = "icon_ship2";

		private const string KUBO = "icon_ship3";

		private const string KUTIKU = "icon_ship4";

		private const string SENSUI = "icon_ship5";

		private const string SONOTA = "icon_ship6";

		private const string SUIBO = "icon_ship7";

		private const string YOURIKU = "icon_ship8";

		[SerializeField]
		private UIWidget EscortDeckParent;

		[SerializeField]
		private CommonShipBanner Banner;

		[SerializeField]
		private Transform BannerShutter;

		[SerializeField]
		private UISprite[] shipTypeIcons;

		private string[] ShipTypeIconName;

		private static readonly Color32 TaihaColor = new Color32(255, 90, 90, 255);

		private static readonly Color32 TyuuhaColor = new Color32(255, 178, 108, 255);

		private static readonly Color32 ShouhaColor = new Color32(243, 255, 165, 255);

		private void Awake()
		{
			this.CreateShipTypeIconNameArray();
			this.Banner.isUseKira = false;
			this.Banner.isUseSmoke = false;
		}

		public void UpdateEscortDeck(EscortDeckModel deck)
		{
			bool flag = this.Banner.ShipModel == null || this.Banner.ShipModel != deck.GetFlagShip();
			this.Banner.SetShipData(deck.GetFlagShip());
			for (int i = 0; i < this.shipTypeIcons.Length; i++)
			{
				ShipModel ship = deck.GetShip(i + 1);
				if (ship != null)
				{
					this.shipTypeIcons[i].spriteName = this.ShipTypeIconName[ship.ShipType];
					this.ChangeColor(ship, this.shipTypeIcons[i]);
				}
				else
				{
					this.shipTypeIcons[i].spriteName = string.Empty;
				}
			}
			if (flag)
			{
				this.updateView(0.2f);
			}
		}

		public void updateView(float time)
		{
			if (this.Banner.ShipModel == null)
			{
				this.Banner.SetActive(false);
				this.BannerShutter.SetActive(true);
			}
			else
			{
				this.Banner.SetActive(true);
				this.BannerShutter.SetActive(false);
				this.EscortDeckParent.alpha = 0f;
				TweenAlpha.Begin(this.EscortDeckParent.get_gameObject(), time, 1f);
			}
		}

		private void ChangeColor(ShipModel ship, UISprite icon)
		{
			switch (ship.DamageStatus)
			{
			case DamageState.Normal:
				icon.color = Color.get_white();
				break;
			case DamageState.Shouha:
				icon.color = StrategySideEscortDeck.ShouhaColor;
				break;
			case DamageState.Tyuuha:
				icon.color = StrategySideEscortDeck.TyuuhaColor;
				break;
			case DamageState.Taiha:
				icon.color = StrategySideEscortDeck.TaihaColor;
				break;
			}
		}

		private void CreateShipTypeIconNameArray()
		{
			this.ShipTypeIconName = new string[23];
			this.ShipTypeIconName[0] = string.Empty;
			this.ShipTypeIconName[1] = "icon_ship4";
			this.ShipTypeIconName[2] = "icon_ship4";
			this.ShipTypeIconName[3] = "icon_ship2";
			this.ShipTypeIconName[4] = "icon_ship2";
			this.ShipTypeIconName[5] = "icon_ship2";
			this.ShipTypeIconName[6] = "icon_ship2";
			this.ShipTypeIconName[7] = "icon_ship3";
			this.ShipTypeIconName[8] = "icon_ship1";
			this.ShipTypeIconName[9] = "icon_ship1";
			this.ShipTypeIconName[10] = "icon_ship1";
			this.ShipTypeIconName[11] = "icon_ship3";
			this.ShipTypeIconName[12] = "icon_ship1";
			this.ShipTypeIconName[13] = "icon_ship5";
			this.ShipTypeIconName[14] = "icon_ship5";
			this.ShipTypeIconName[15] = "icon_ship6";
			this.ShipTypeIconName[16] = "icon_ship7";
			this.ShipTypeIconName[17] = "icon_ship8";
			this.ShipTypeIconName[18] = "icon_ship3";
			this.ShipTypeIconName[19] = "icon_ship6";
			this.ShipTypeIconName[20] = "icon_ship5";
			this.ShipTypeIconName[21] = "icon_ship2";
			this.ShipTypeIconName[22] = "icon_ship6";
		}
	}
}
