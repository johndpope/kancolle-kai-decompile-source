using KCV.Battle.Utils;
using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle
{
	[Serializable]
	public class UITacticalSituationFleetInfos : IDisposable
	{
		[SerializeField]
		private UIWidget _uiWidget;

		[SerializeField]
		private UILabel _uiFleetName;

		[SerializeField]
		private UITexture _uiLine;

		[SerializeField]
		private UIGrid _uiShipsAnchor;

		private FleetType _iFleetType;

		private List<UITacticalSituationShipBanner> _listShipBanners;

		public UIWidget widget
		{
			get
			{
				return this._uiWidget;
			}
		}

		public FleetType fleetType
		{
			get
			{
				return this._iFleetType;
			}
		}

		public void Dispose()
		{
			Mem.Del<UIWidget>(ref this._uiWidget);
			Mem.Del<UILabel>(ref this._uiFleetName);
			Mem.Del<UITexture>(ref this._uiLine);
			Mem.Del<UIGrid>(ref this._uiShipsAnchor);
			Mem.Del<FleetType>(ref this._iFleetType);
			Mem.DelListSafe<UITacticalSituationShipBanner>(ref this._listShipBanners);
		}

		public bool Init(FleetType iType, string strFleetName, List<ShipModel_BattleAll> shipList, UITacticalSituationShipBanner prefab)
		{
			this._iFleetType = iType;
			this._uiFleetName.text = strFleetName;
			this.CreateShipBanners(shipList, prefab);
			return true;
		}

		private void CreateShipBanners(List<ShipModel_BattleAll> shipList, UITacticalSituationShipBanner prefab)
		{
			this._listShipBanners = new List<UITacticalSituationShipBanner>();
			shipList.ForEach(delegate(ShipModel_BattleAll x)
			{
				if (x != null)
				{
					this._listShipBanners.Add(UITacticalSituationShipBanner.Instantiate(prefab, this._uiShipsAnchor.get_transform(), x));
				}
			});
		}
	}
}
