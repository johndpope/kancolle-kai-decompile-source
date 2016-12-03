using local.models;
using Server_Controllers;
using System;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyDebugMenu2 : MonoBehaviour
	{
		public StrategyDebugButton[] buttons;

		public KeyControl key;

		public UIButtonManager BtnManager;

		public UIButtonManager MagBtnManager;

		private ShipModel ship;

		private Debug_Mod debugMod;

		private int mag;

		private void Update()
		{
			this.key.Update();
			this.buttons[this.BtnManager.nowForcusIndex].Act.Invoke();
		}

		private void Start()
		{
			this.BtnManager.setFocus(0);
			this.buttons[0].Act = new Action(this.ShipSelect);
			this.buttons[1].Act = new Action(this.ShipLevel);
			this.buttons[2].Act = new Action(this.ShipStrength);
			this.key = new KeyControl(0, 0, 0.4f, 0.1f);
			this.mag = 1;
			this.ship = StrategyTopTaskManager.GetLogicManager().UserInfo.GetShip(1);
			this.ShipInit();
			this.debugMod = new Debug_Mod();
		}

		public void SetMag()
		{
			switch (this.MagBtnManager.nowForcusIndex)
			{
			case 0:
				this.mag = 1;
				break;
			case 1:
				this.mag = 10;
				break;
			case 2:
				this.mag = 100;
				break;
			case 3:
				this.mag = 1000;
				break;
			}
		}

		private void ShipSelect()
		{
			if (this.key.IsRightDown() || this.key.IsLeftDown())
			{
				int num = Convert.ToInt32(this.buttons[0].labels[0].text);
				if (this.key.IsRightDown())
				{
					num = (int)Util.RangeValue(num + 1 * this.mag, 0f, 500f);
				}
				else
				{
					num = (int)Util.RangeValue(num - 1 * this.mag, 0f, 500f);
				}
				this.buttons[0].labels[0].textInt = num;
				this.ship = StrategyTopTaskManager.GetLogicManager().UserInfo.GetShip(num);
				this.ShipInit();
			}
		}

		private void ShipInit()
		{
			this.buttons[0].labels[1].text = ((this.ship == null) ? "NONE" : this.ship.Name);
			this.buttons[1].labels[0].text = ((this.ship == null) ? "NONE" : ("LV : " + this.ship.Level.ToString()));
			this.buttons[2].labels[0].text = ((this.ship == null) ? "NONE" : string.Empty);
			this.buttons[2].labels[1].text = ((this.ship == null) ? "NONE" : ("耐久\n" + this.ship.Taikyu.ToString()));
			this.buttons[2].labels[2].text = ((this.ship == null) ? "NONE" : ("火力\n" + this.ship.Karyoku.ToString()));
			this.buttons[2].labels[3].text = ((this.ship == null) ? "NONE" : ("対空\n" + this.ship.Taiku.ToString()));
			this.buttons[2].labels[4].text = ((this.ship == null) ? "NONE" : ("対潜\n" + this.ship.Taisen.ToString()));
			this.buttons[2].labels[5].text = ((this.ship == null) ? "NONE" : ("運\n" + this.ship.Lucky.ToString()));
		}

		private void ShipLevel()
		{
			if (this.key.IsRightDown())
			{
				int num = this.ship.Level;
				num = (int)Util.RangeValue(num + 1 * this.mag, 1f, 150f);
				for (int i = this.ship.Level; i <= num; i++)
				{
					this.ship.AddExp(this.ship.Exp_Next);
				}
				this.ShipInit();
			}
		}

		private void ShipStrength()
		{
		}
	}
}
