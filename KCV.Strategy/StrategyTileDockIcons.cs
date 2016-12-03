using Common.Enum;
using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyTileDockIcons : MonoBehaviour
	{
		[SerializeField]
		private StrategyDockIcon[] DockIcon;

		[Button("Debug", "Debug", new object[]
		{

		})]
		public int button;

		public void SetDockIcon(MapAreaModel areaModel)
		{
			List<NdockStates> nDockStateList = areaModel.GetNDockStateList();
			for (int i = 0; i < 4; i++)
			{
				if (i < nDockStateList.get_Count())
				{
					this.DockIcon[i].SetDockState(nDockStateList.get_Item(i));
				}
				else
				{
					this.DockIcon[i].SetActive(false);
				}
			}
		}

		public void Debug()
		{
			this.SetDockIcon(StrategyTopTaskManager.GetLogicManager().Area.get_Item(1));
		}
	}
}
