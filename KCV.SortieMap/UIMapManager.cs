using local.managers;
using local.models;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KCV.SortieMap
{
	[RequireComponent(typeof(UIPanel))]
	public class UIMapManager : MonoBehaviour
	{
		[SerializeField]
		private UISortieShip.Direction _iDirection = UISortieShip.Direction.Right;

		[SerializeField]
		private bool _isStartDirection;

		[SerializeField]
		private Transform _traRudder;

		[SerializeField]
		private Transform _traBackground;

		[Button("SetBackgroundSize", "SetBackgroundSize", new object[]
		{

		}), SerializeField]
		private int _nSetBackgroundSize = 1;

		private UITexture _uiBackground;

		private UISortieShip _uiSortieShip;

		private UISortieMapCell _uiNowCell;

		private UISortieMapCell _uiNextCell;

		private List<UISortieMapCell> _listMapCell;

		private List<UISortieMapRoot> _listMapRoute;

		private Dictionary<int, Transform> _dicAirRecPoint;

		private WobblingIcons _clsWobblingIcons;

		public List<UISortieMapCell> cells
		{
			get
			{
				return this._listMapCell;
			}
			private set
			{
				this._listMapCell = value;
			}
		}

		public List<UISortieMapRoot> routes
		{
			get
			{
				return this._listMapRoute;
			}
			private set
			{
				this._listMapRoute = value;
			}
		}

		public Dictionary<int, Transform> airRecPoint
		{
			get
			{
				return this._dicAirRecPoint;
			}
			private set
			{
				this._dicAirRecPoint = value;
			}
		}

		public UISortieShip sortieShip
		{
			get
			{
				return this._uiSortieShip;
			}
			private set
			{
				this._uiSortieShip = value;
			}
		}

		public UISortieMapCell nowCell
		{
			get
			{
				return this._uiNowCell;
			}
			private set
			{
				this._uiNowCell = value;
			}
		}

		public UISortieMapCell nextCell
		{
			get
			{
				return this._uiNextCell;
			}
			private set
			{
				this._uiNextCell = value;
			}
		}

		public Transform rudder
		{
			get
			{
				return this._traRudder;
			}
			set
			{
				if (value.get_name() == "Rudder")
				{
					this._traRudder = value;
				}
			}
		}

		public Transform background
		{
			get
			{
				return this._traBackground;
			}
			set
			{
				if (value.get_name() == "Background")
				{
					this._traBackground = value;
				}
			}
		}

		public WobblingIcons wobblingIcons
		{
			get
			{
				return this._clsWobblingIcons;
			}
		}

		public static UIMapManager Instantiate(MapManager manager, UIMapManager prefab, Transform parent, UISortieShip sortieShip)
		{
			UIMapManager uIMapManager = (!(prefab != null)) ? Util.InstantiatePrefab(string.Format("SortieMap/AreaMap/Map{0}", manager.Map.MstId), parent.get_gameObject(), false).GetComponent<UIMapManager>() : Object.Instantiate<UIMapManager>(prefab);
			uIMapManager.get_transform().set_parent(parent);
			uIMapManager.get_transform().localPositionZero();
			uIMapManager.get_transform().set_localScale(Vector3.get_one() * 1.1f);
			uIMapManager.Init(manager, sortieShip);
			return uIMapManager;
		}

		private bool Init(MapManager manager, UISortieShip sortieShip)
		{
			if (this._traBackground == null)
			{
				this._traBackground = base.get_transform().FindChild("Background");
			}
			if (this.rudder == null)
			{
				this.rudder = base.get_transform().FindChild("Rudder");
			}
			this.MakeCellList(manager);
			this.MakeRouteList();
			this.MakeAirRecPoint(manager);
			this.MakeWobblingIconList(manager);
			this.sortieShip = UISortieShip.Instantiate(sortieShip, base.get_transform(), this._iDirection);
			this.UpdatePassedRoutesStates(manager);
			this.UpdateNowNNextCell(manager.NowCell, manager.NextCell);
			this.SetShipPosition();
			return true;
		}

		private void MakeCellList(MapManager manager)
		{
			this.cells = new List<UISortieMapCell>();
			this.cells.Add(null);
			for (int i = 1; i < base.get_transform().get_childCount(); i++)
			{
				Transform transform = base.get_transform().FindChild("UISortieMapCell" + i);
				if (!(transform != null))
				{
					break;
				}
				this.cells.Add(transform.GetComponent<UISortieMapCell>().Startup());
				this.cells.get_Item(i).Init(manager.Cells[i]);
			}
		}

		private void MakeRouteList()
		{
			this.routes = new List<UISortieMapRoot>();
			this.routes.Add(null);
			for (int i = 1; i < base.get_transform().get_childCount(); i++)
			{
				Transform transform = base.get_transform().FindChild(string.Format("Route{0}", i));
				if (!(transform != null))
				{
					break;
				}
				this.routes.Add(transform.GetComponent<UISortieMapRoot>());
				this.routes.get_Item(i).isPassed = false;
			}
		}

		private void MakeAirRecPoint(MapManager manager)
		{
			this._dicAirRecPoint = new Dictionary<int, Transform>();
			Enumerable.Skip<CellModel>(manager.Cells, 1).ForEach(delegate(CellModel x)
			{
				Transform transform = base.get_transform().FindChild(string.Format("AirRecPoint{0}", x.CellNo));
				if (transform != null)
				{
					this._dicAirRecPoint.Add(x.CellNo, transform);
				}
			});
		}

		private void MakeWobblingIconList(MapManager manager)
		{
			this._clsWobblingIcons = new WobblingIcons(manager, base.get_transform());
		}

		private void FixedUpdate()
		{
			if (this._clsWobblingIcons.wobblingIcons != null && this._clsWobblingIcons.wobblingIcons.get_Count() != 0)
			{
				this._clsWobblingIcons.FixedRun();
			}
		}

		private void OnDestroy()
		{
			Mem.Del<UISortieShip.Direction>(ref this._iDirection);
			Mem.Del<bool>(ref this._isStartDirection);
			Mem.Del<Transform>(ref this._traRudder);
			Mem.Del<Transform>(ref this._traBackground);
			Mem.Del<UITexture>(ref this._uiBackground);
			Mem.Del<UISortieShip>(ref this._uiSortieShip);
			Mem.Del<UISortieMapCell>(ref this._uiNowCell);
			Mem.Del<UISortieMapCell>(ref this._uiNextCell);
			Mem.DelListSafe<UISortieMapCell>(ref this._listMapCell);
			Mem.DelList<UISortieMapRoot>(ref this._listMapRoute);
			Mem.DelDictionarySafe<int, Transform>(ref this._dicAirRecPoint);
			Mem.DelIDisposableSafe<WobblingIcons>(ref this._clsWobblingIcons);
		}

		public void UpdatePassedRoutesStates(MapManager manager)
		{
			manager.Passed.ForEach(delegate(int x)
			{
				this.routes.get_Item(x).isPassed = true;
			});
		}

		public void UpdateRouteState(int CellNo)
		{
			this.routes.get_Item(CellNo).Passed(true);
		}

		public void UpdateCellState(int CellNo, bool isPassed)
		{
			this.cells.get_Item(CellNo).isPassedCell = isPassed;
		}

		public void SetShipPosition()
		{
			if (this.nowCell != null)
			{
				this.sortieShip.get_transform().set_localPosition(this.nowCell.get_transform().get_localPosition());
			}
			else
			{
				this.sortieShip.get_transform().set_position(this._traRudder.get_position());
			}
		}

		public void InitAfterBattle()
		{
			if (this.nowCell != null)
			{
				this.nowCell.isPassedCell = true;
			}
			this.cells.get_Item(this.nextCell.cellModel.CellNo).isPassedCell = true;
			this.routes.get_Item(this.nextCell.cellModel.CellNo).isPassed = true;
			this.sortieShip.get_transform().set_localPosition(this.nextCell.get_transform().get_localPosition());
		}

		public void UpdateNowNNextCell(CellModel now, CellModel next)
		{
			this.SetNowCell(now.CellNo);
			this.SetNextCell(next.CellNo);
		}

		private void SetNowCell(int cellNo)
		{
			if (this.cells.get_Count() <= cellNo)
			{
				cellNo = 1;
			}
			this.nowCell = this.cells.get_Item(cellNo);
		}

		private void SetNextCell(int cellNo)
		{
			if (this.cells.get_Count() <= cellNo)
			{
				cellNo = 1;
			}
			this.nextCell = this.cells.get_Item(cellNo);
		}

		private void SetBackgroundSize()
		{
			this._traBackground.GetComponent<UITexture>().localSize = Defines.SORTIEMAP_MAP_BG_SIZE;
		}
	}
}
