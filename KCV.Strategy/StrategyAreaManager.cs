using local.managers;
using local.models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyAreaManager : MonoBehaviour
	{
		private const int TILE_NUM = 18;

		[Button("CloseAreaDebug", "CloseArea", new object[]
		{

		})]
		public int button1;

		private GameObject Prefab_AreaOpenAnimation;

		[SerializeField]
		private GameObject Red_Sea;

		public StrategyTileRoutes tileRouteManager;

		public int DebugOpenAreaID;

		private bool isShipMoveWait;

		private ShipModel flagship;

		private int[] TogetherCloseTiles;

		public MapAreaModel FocusAreaModel;

		public int DebugRebellionArea;

		public int DebugCloseAreaID;

		public static KeyControl sailKeyController
		{
			get;
			private set;
		}

		public static int FocusAreaID
		{
			get
			{
				return StrategyAreaManager.sailKeyController.Index;
			}
		}

		private StrategyHexTileManager tileManager
		{
			get
			{
				return StrategyTopTaskManager.Instance.TileManager;
			}
		}

		private StrategyCamera mapCamera
		{
			get
			{
				return StrategyTopTaskManager.Instance.UIModel.MapCamera;
			}
		}

		public void init()
		{
			this.tileManager.Init();
			this.tileManager.setAreaModels(StrategyTopTaskManager.GetLogicManager());
			this.makeSailSelectController();
			if (StrategyTopTaskManager.GetLogicManager().IsOpenedLastAreaAtLeastOnce())
			{
				this.Red_Sea.SetActive(true);
				this.Red_Sea.GetComponent<UIPanel>().alpha = 1f;
			}
		}

		public int[] getNewOpenArea()
		{
			int[] result = null;
			if (RetentionData.GetData() != null)
			{
				result = (int[])RetentionData.GetData().get_Item("newOpenAreaIDs");
			}
			return result;
		}

		public void ChangeFocusTile(int areaID, bool immediate = false)
		{
			if (this.tileManager.FocusTile == null || areaID != this.tileManager.FocusTile.areaID)
			{
				this.tileManager.changeFocus(areaID);
				this.mapCamera.MoveToTargetTile(areaID, immediate);
				StrategyAreaManager.sailKeyController.Index = this.tileManager.FocusTile.areaID;
				StrategyTopTaskManager.Instance.GetAreaMng().FocusAreaModel = StrategyTopTaskManager.GetLogicManager().Area.get_Item(areaID);
			}
		}

		public void UpdateSelectArea(int focusAreaID, bool immediate = false)
		{
			this.ChangeFocusTile(focusAreaID, immediate);
			StrategyTopTaskManager.Instance.GetInfoMng().updateFooterInfo(false);
			StrategyTopTaskManager.Instance.GetInfoMng().updateInfoPanel(focusAreaID);
		}

		[DebuggerHidden]
		public IEnumerator OpenArea(int[] newOpenAreaID)
		{
			StrategyAreaManager.<OpenArea>c__Iterator17A <OpenArea>c__Iterator17A = new StrategyAreaManager.<OpenArea>c__Iterator17A();
			<OpenArea>c__Iterator17A.newOpenAreaID = newOpenAreaID;
			<OpenArea>c__Iterator17A.<$>newOpenAreaID = newOpenAreaID;
			<OpenArea>c__Iterator17A.<>f__this = this;
			return <OpenArea>c__Iterator17A;
		}

		public void setShipMove(bool isWait, ShipModel flagShip)
		{
			this.flagship = flagShip;
			this.isShipMoveWait = isWait;
		}

		[DebuggerHidden]
		public IEnumerator RebellionResult(RebellionMapManager RmapManager, bool isWin, int areaID)
		{
			StrategyAreaManager.<RebellionResult>c__Iterator17B <RebellionResult>c__Iterator17B = new StrategyAreaManager.<RebellionResult>c__Iterator17B();
			<RebellionResult>c__Iterator17B.RmapManager = RmapManager;
			<RebellionResult>c__Iterator17B.areaID = areaID;
			<RebellionResult>c__Iterator17B.isWin = isWin;
			<RebellionResult>c__Iterator17B.<$>RmapManager = RmapManager;
			<RebellionResult>c__Iterator17B.<$>areaID = areaID;
			<RebellionResult>c__Iterator17B.<$>isWin = isWin;
			<RebellionResult>c__Iterator17B.<>f__this = this;
			return <RebellionResult>c__Iterator17B;
		}

		[DebuggerHidden]
		private IEnumerator ShowRebellionResult(bool isWin, int areaID)
		{
			StrategyAreaManager.<ShowRebellionResult>c__Iterator17C <ShowRebellionResult>c__Iterator17C = new StrategyAreaManager.<ShowRebellionResult>c__Iterator17C();
			<ShowRebellionResult>c__Iterator17C.isWin = isWin;
			<ShowRebellionResult>c__Iterator17C.areaID = areaID;
			<ShowRebellionResult>c__Iterator17C.<$>isWin = isWin;
			<ShowRebellionResult>c__Iterator17C.<$>areaID = areaID;
			<ShowRebellionResult>c__Iterator17C.<>f__this = this;
			return <ShowRebellionResult>c__Iterator17C;
		}

		[DebuggerHidden]
		private IEnumerator ShowLoseGuide()
		{
			return new StrategyAreaManager.<ShowLoseGuide>c__Iterator17D();
		}

		public RebellionMapManager checkRebellionResult()
		{
			Hashtable data = RetentionData.GetData();
			RebellionMapManager result = null;
			if (data != null)
			{
				result = (data.get_Item("rebellionMapManager") as RebellionMapManager);
			}
			return result;
		}

		[DebuggerHidden]
		public IEnumerator ClearRedSeaColor()
		{
			StrategyAreaManager.<ClearRedSeaColor>c__Iterator17E <ClearRedSeaColor>c__Iterator17E = new StrategyAreaManager.<ClearRedSeaColor>c__Iterator17E();
			<ClearRedSeaColor>c__Iterator17E.<>f__this = this;
			return <ClearRedSeaColor>c__Iterator17E;
		}

		[DebuggerHidden]
		public IEnumerator RefreshArea()
		{
			StrategyAreaManager.<RefreshArea>c__Iterator17F <RefreshArea>c__Iterator17F = new StrategyAreaManager.<RefreshArea>c__Iterator17F();
			<RefreshArea>c__Iterator17F.<>f__this = this;
			return <RefreshArea>c__Iterator17F;
		}

		[DebuggerHidden]
		public IEnumerator CloseArea(int closeAreaID, Action Onfinished)
		{
			StrategyAreaManager.<CloseArea>c__Iterator180 <CloseArea>c__Iterator = new StrategyAreaManager.<CloseArea>c__Iterator180();
			<CloseArea>c__Iterator.closeAreaID = closeAreaID;
			<CloseArea>c__Iterator.Onfinished = Onfinished;
			<CloseArea>c__Iterator.<$>closeAreaID = closeAreaID;
			<CloseArea>c__Iterator.<$>Onfinished = Onfinished;
			<CloseArea>c__Iterator.<>f__this = this;
			return <CloseArea>c__Iterator;
		}

		private void CloseAreaDebug()
		{
			base.StartCoroutine(this.CloseArea(this.DebugCloseAreaID, null));
		}

		public void MakeTogetherCloseTilesList(int closeAreaID, int[] beforeOpenAreasID, int[] afterOpenAreasID)
		{
			this.TogetherCloseTiles = Enumerable.ToArray<int>(Enumerable.Where<int>(Enumerable.Where<int>(beforeOpenAreasID, (int x) => !Enumerable.Any<int>(afterOpenAreasID, (int y) => x == y)), (int x) => x != closeAreaID));
		}

		public static int[] DicToIntArray(Dictionary<int, MapAreaModel> AreasID)
		{
			AreasID = Enumerable.ToDictionary<KeyValuePair<int, MapAreaModel>, int, MapAreaModel>(Enumerable.Where<KeyValuePair<int, MapAreaModel>>(AreasID, (KeyValuePair<int, MapAreaModel> x) => x.get_Value().IsOpen()), (KeyValuePair<int, MapAreaModel> Pair) => Pair.get_Key(), (KeyValuePair<int, MapAreaModel> Pair) => Pair.get_Value());
			return Enumerable.ToArray<int>(AreasID.get_Keys());
		}

		[DebuggerHidden]
		private IEnumerator CloseTogetherTile()
		{
			StrategyAreaManager.<CloseTogetherTile>c__Iterator181 <CloseTogetherTile>c__Iterator = new StrategyAreaManager.<CloseTogetherTile>c__Iterator181();
			<CloseTogetherTile>c__Iterator.<>f__this = this;
			return <CloseTogetherTile>c__Iterator;
		}

		private void makeOpenAreaIdArray(List<int> openAreaID)
		{
			for (int i = 1; i < StrategyTopTaskManager.GetLogicManager().Area.get_Count(); i++)
			{
				if (StrategyTopTaskManager.GetLogicManager().Area.get_Item(i).IsOpen())
				{
					openAreaID.Add(i);
				}
			}
		}

		public void OpenAllArea()
		{
			int[,] useIndexMap = new int[,]
			{
				{
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0
				},
				{
					8,
					8,
					8,
					11,
					7,
					9,
					9,
					0
				},
				{
					9,
					7,
					7,
					0,
					0,
					0,
					10,
					10
				},
				{
					13,
					13,
					13,
					0,
					12,
					8,
					8,
					0
				},
				{
					10,
					10,
					10,
					0,
					0,
					0,
					0,
					0
				},
				{
					11,
					6,
					6,
					14,
					14,
					0,
					7,
					7
				},
				{
					12,
					17,
					17,
					0,
					14,
					5,
					5,
					11
				},
				{
					1,
					11,
					11,
					5,
					5,
					2,
					2,
					9
				},
				{
					3,
					3,
					3,
					12,
					11,
					1,
					1,
					0
				},
				{
					1,
					1,
					1,
					7,
					2,
					10,
					10,
					0
				},
				{
					9,
					9,
					9,
					2,
					2,
					4,
					4,
					0
				},
				{
					8,
					12,
					12,
					6,
					5,
					7,
					7,
					1
				},
				{
					3,
					0,
					17,
					17,
					6,
					11,
					11,
					8
				},
				{
					0,
					0,
					15,
					15,
					3,
					3,
					3,
					0
				},
				{
					6,
					16,
					16,
					0,
					0,
					0,
					5,
					5
				},
				{
					13,
					0,
					0,
					0,
					16,
					17,
					17,
					13
				},
				{
					15,
					0,
					0,
					0,
					14,
					14,
					17,
					17
				},
				{
					15,
					15,
					15,
					16,
					16,
					6,
					6,
					12
				}
			};
			StrategyAreaManager.sailKeyController.setUseIndexMap(useIndexMap);
		}

		private void makeSailSelectController()
		{
			StrategyAreaManager.sailKeyController = new KeyControl(1, 17, 0.4f, 0.1f);
			int[,] useIndexMap = new int[,]
			{
				{
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0
				},
				{
					8,
					8,
					8,
					11,
					7,
					9,
					9,
					0
				},
				{
					9,
					7,
					7,
					0,
					0,
					0,
					10,
					10
				},
				{
					13,
					13,
					13,
					0,
					12,
					8,
					8,
					0
				},
				{
					10,
					10,
					10,
					0,
					0,
					0,
					0,
					0
				},
				{
					11,
					6,
					6,
					14,
					14,
					0,
					7,
					7
				},
				{
					12,
					17,
					17,
					0,
					14,
					5,
					5,
					11
				},
				{
					1,
					11,
					11,
					5,
					5,
					2,
					2,
					9
				},
				{
					3,
					3,
					3,
					12,
					11,
					1,
					1,
					0
				},
				{
					1,
					1,
					1,
					7,
					2,
					10,
					10,
					0
				},
				{
					9,
					9,
					9,
					2,
					2,
					4,
					4,
					0
				},
				{
					8,
					12,
					12,
					6,
					5,
					7,
					7,
					1
				},
				{
					3,
					0,
					17,
					17,
					6,
					11,
					11,
					8
				},
				{
					0,
					0,
					15,
					15,
					3,
					3,
					3,
					0
				},
				{
					6,
					16,
					16,
					0,
					0,
					0,
					5,
					5
				},
				{
					13,
					0,
					0,
					0,
					16,
					17,
					17,
					13
				},
				{
					15,
					0,
					0,
					0,
					14,
					14,
					17,
					17
				},
				{
					15,
					15,
					15,
					16,
					16,
					6,
					6,
					12
				}
			};
			StrategyAreaManager.sailKeyController.setUseIndexMap(useIndexMap);
			StrategyMapManager logicManager = StrategyTopTaskManager.GetLogicManager();
			List<int> list = new List<int>();
			for (int i = 1; i < logicManager.Area.get_Count(); i++)
			{
				if (logicManager.Area.get_Item(i).IsOpen())
				{
					list.Add(i);
				}
			}
			StrategyAreaManager.sailKeyController.setEnableIndex(list.ToArray());
			StrategyAreaManager.sailKeyController.Index = SingletonMonoBehaviour<AppInformation>.Instance.CurrentAreaID;
		}

		private void OnDestroy()
		{
			this.Prefab_AreaOpenAnimation = null;
		}
	}
}
