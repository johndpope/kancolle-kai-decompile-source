using local.models;
using System;
using UnityEngine;

namespace KCV.Organize
{
	public class OrganizeDetail_Status : MonoBehaviour
	{
		[SerializeField]
		private UILabel ShipNameLabel;

		[SerializeField]
		private UILabel LevelLabel;

		[SerializeField]
		private UILabel HPLabel;

		[SerializeField]
		private UISprite HPGauge;

		[SerializeField]
		private UISprite EXPGauge;

		[SerializeField]
		private Transform StarParent;

		private UISprite[] _uiStar;

		public void SetStatus(ShipModel ship)
		{
			this.ShipNameLabel.text = ship.Name;
			this.LevelLabel.textInt = ship.Level;
			this.HPLabel.text = (ship.NowHp + "/" + ship.MaxHp).ToString();
			this.SetHPGauge(ship);
			this.EXPGauge.fillAmount = (float)ship.Exp_Percentage / 100f;
			this.SetStar(ship.Srate);
		}

		private void SetHPGauge(ShipModel ship)
		{
			float fillAmount = (float)ship.NowHp / (float)ship.MaxHp;
			this.HPGauge.fillAmount = fillAmount;
			this.HPGauge.color = Util.HpGaugeColor2(ship.MaxHp, ship.NowHp);
		}

		public void SetStar(int StarNum)
		{
			this._uiStar = new UISprite[5];
			for (int i = 0; i < 5; i++)
			{
				this._uiStar[i] = this.StarParent.get_transform().FindChild("Star" + (i + 1)).GetComponent<UISprite>();
				if (i <= StarNum)
				{
					this._uiStar[i].spriteName = "star_on";
				}
				else
				{
					this._uiStar[i].spriteName = "star";
				}
			}
		}

		private void OnDestroy()
		{
			this.ShipNameLabel = null;
			this.LevelLabel = null;
			this.HPLabel = null;
			this.HPGauge = null;
			this.EXPGauge = null;
			this.StarParent = null;
			this._uiStar = null;
		}
	}
}
