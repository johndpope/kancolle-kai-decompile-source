using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyTileRoutes : MonoBehaviour
	{
		private GameObject[] TileRoutes;

		private bool[] isEnable;

		private void Awake()
		{
			this.TileRoutes = new GameObject[4];
			for (int i = 0; i < this.TileRoutes.Length; i++)
			{
				this.TileRoutes[i] = base.get_transform().FindChild("TileRoute" + (i + 1)).get_gameObject();
			}
			this.isEnable = new bool[4];
		}

		public void Init(List<int> OpenTileIDs)
		{
			this.RouteUpdate(OpenTileIDs, 1, 13, 15, 0f);
			this.RouteUpdate(OpenTileIDs, 2, 12, 17, 0f);
			this.RouteUpdate(OpenTileIDs, 3, 6, 17, 0f);
			this.RouteUpdate(OpenTileIDs, 4, 14, 16, 0f);
		}

		public void UpdateTileRouteState(List<int> OpenTileIDs)
		{
			this.RouteUpdate(OpenTileIDs, 1, 13, 15, 0.4f);
			this.RouteUpdate(OpenTileIDs, 2, 12, 17, 0.4f);
			this.RouteUpdate(OpenTileIDs, 3, 6, 17, 0.4f);
			this.RouteUpdate(OpenTileIDs, 4, 14, 16, 0.4f);
		}

		public void HideRoute()
		{
			for (int i = 0; i < this.TileRoutes.Length; i++)
			{
				TweenAlpha.Begin(this.TileRoutes[i], 0.2f, 0f);
			}
		}

		public void ShowRoute()
		{
			for (int i = 0; i < this.TileRoutes.Length; i++)
			{
				if (this.isEnable[i])
				{
					TweenAlpha.Begin(this.TileRoutes[i], 0.2f, 1f);
				}
			}
		}

		public List<int> CreateOpenTileIDs()
		{
			List<int> list = new List<int>();
			for (int i = 1; i <= 17; i++)
			{
				if (StrategyTopTaskManager.GetLogicManager().Area.get_Item(i).IsOpen())
				{
					list.Add(i);
				}
			}
			return list;
		}

		private void RouteUpdate(List<int> OpenTileIDs, int RouteNo, int checkID1, int checkID2, float duration)
		{
			if (OpenTileIDs.Exists((int x) => x == checkID1) && OpenTileIDs.Exists((int x) => x == checkID2))
			{
				TweenAlpha.Begin(this.TileRoutes[RouteNo - 1], duration, 1f);
				this.isEnable[RouteNo - 1] = true;
			}
			else
			{
				TweenAlpha.Begin(this.TileRoutes[RouteNo - 1], duration, 0f);
				this.isEnable[RouteNo - 1] = false;
			}
		}
	}
}
