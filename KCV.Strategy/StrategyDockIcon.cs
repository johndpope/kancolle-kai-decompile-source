using Common.Enum;
using System;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyDockIcon : MonoBehaviour
	{
		private UISprite icon;

		private void Awake()
		{
			this.icon = base.GetComponent<UISprite>();
		}

		public void SetDockState(NdockStates state)
		{
			switch (state)
			{
			case NdockStates.NOTUSE:
				this.icon.spriteName = "repair_none";
				break;
			case NdockStates.EMPTY:
				this.icon.spriteName = "repair_blue";
				break;
			case NdockStates.RESTORE:
				this.icon.spriteName = "repair_green";
				break;
			}
		}
	}
}
