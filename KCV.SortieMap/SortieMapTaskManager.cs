using Common.Enum;
using KCV.SortieBattle;
using KCV.Utils;
using Librarys.State;
using local.managers;
using System;
using UniRx;
using UnityEngine;

namespace KCV.SortieMap
{
	public class SortieMapTaskManager : SceneTaskMono
	{
		private static SortieMapTaskManager instance;

		[SerializeField]
		private UIRoot _uiRoot;

		[SerializeField]
		private UISortieMapName _uiSortieMapName;

		[SerializeField]
		private UIAreaMapFrame _uiAreaMapFrame;

		[SerializeField]
		private UISortieShipCharacter _uiShipCharacter;

		[SerializeField]
		private Transform _sharedPlace;

		[SerializeField]
		private UIShortCutSwitch _shortCutSwitch;

		[SerializeField]
		private BaseCamera _cam;

		[Header("[SortieMap Prefab Files]"), SerializeField]
		private SortiePrefabFile _clsSortiePrefabFile;

		private bool _isFirstInit;

		private Action<MapManager> _actOnSetMapManager;

		private CtrlActiveBranching _ctrlActiveBranching;

		private static UIMapManager _uiMapManager;

		private static SortieMapTaskManagerMode _iMode;

		private static SortieMapTaskManagerMode _iModeReq;

		private static Tasks _clsTasks;

		private static TaskSortieMoveShip _clsTaskMoveShip;

		private static TaskSortieEvent _clsTaskEvent;

		private static TaskSortieFormation _clsTaskFormation;

		private static TaskSortieResult _clsTaskResult;

		[Header("[Debug Properties]"), SerializeField]
		private SortieDebugProperties _strSortieDebugProperties = default(SortieDebugProperties);

		private static SortieMapTaskManager Instance
		{
			get
			{
				if (SortieMapTaskManager.instance == null)
				{
					SortieMapTaskManager.instance = Object.FindObjectOfType<SortieMapTaskManager>();
					if (SortieMapTaskManager.instance == null)
					{
						throw new Exception();
					}
				}
				return SortieMapTaskManager.instance;
			}
		}

		public void Startup(Transform prefabAreaMap, MapManager mapManager, Action<MapManager> onSetMapManager)
		{
			SortieMapTaskManager._clsTasks = new Tasks();
			SortieMapTaskManager._clsTaskMoveShip = new TaskSortieMoveShip();
			SortieMapTaskManager._clsTaskEvent = new TaskSortieEvent(new Action<ShipRecoveryType>(this.GoNext));
			SortieMapTaskManager._clsTaskFormation = new TaskSortieFormation();
			SortieMapTaskManager._clsTaskResult = new TaskSortieResult();
			this._actOnSetMapManager = onSetMapManager;
			this._isFirstInit = true;
			this.GetComponentThis(ref this._uiRoot);
			Util.SetRootContentSize(this._uiRoot, App.SCREEN_RESOLUTION);
			this.InitSortieMapData(prefabAreaMap, mapManager);
			this.DrawDefaultShip();
		}

		public void Terminate()
		{
			Mem.Del<UIRoot>(ref this._uiRoot);
			Mem.Del<UISortieMapName>(ref this._uiSortieMapName);
			Mem.Del<UIAreaMapFrame>(ref this._uiAreaMapFrame);
			Mem.Del<UISortieShipCharacter>(ref this._uiShipCharacter);
			Mem.Del<Transform>(ref this._sharedPlace);
			Mem.Del<UIShortCutSwitch>(ref this._shortCutSwitch);
			Mem.Del<BaseCamera>(ref this._cam);
			Mem.DelIDisposableSafe<SortiePrefabFile>(ref this._clsSortiePrefabFile);
			Mem.Del<bool>(ref this._isFirstInit);
			Mem.Del<Action<MapManager>>(ref this._actOnSetMapManager);
			Mem.DelIDisposableSafe<CtrlActiveBranching>(ref this._ctrlActiveBranching);
			Mem.Del<UIMapManager>(ref SortieMapTaskManager._uiMapManager);
			Mem.Del<SortieMapTaskManagerMode>(ref SortieMapTaskManager._iModeReq);
			Mem.Del<SortieMapTaskManagerMode>(ref SortieMapTaskManager._iMode);
			if (SortieMapTaskManager._clsTasks != null)
			{
				SortieMapTaskManager._clsTasks.UnInit();
			}
			Mem.Del<Tasks>(ref SortieMapTaskManager._clsTasks);
			Mem.DelIDisposableSafe<TaskSortieMoveShip>(ref SortieMapTaskManager._clsTaskMoveShip);
			Mem.DelIDisposableSafe<TaskSortieEvent>(ref SortieMapTaskManager._clsTaskEvent);
			Mem.DelIDisposableSafe<TaskSortieFormation>(ref SortieMapTaskManager._clsTaskFormation);
			Mem.DelIDisposableSafe<TaskSortieResult>(ref SortieMapTaskManager._clsTaskResult);
			Mem.Del<SortieMapTaskManager>(ref SortieMapTaskManager.instance);
		}

		private void InitSortieMapData(Transform prefabAreaMap, MapManager mapManager)
		{
			if (mapManager != null)
			{
				Dlg.Call<MapManager>(ref this._actOnSetMapManager, mapManager);
				this.StartupUIMapManager(prefabAreaMap);
				this.GetGoNextData();
			}
		}

		private void StartupUIMapManager(Transform prefabAreaMap)
		{
			MapManager mapManager = SortieBattleTaskManager.GetMapManager();
			SortieMapTaskManager._uiMapManager = UIMapManager.Instantiate(mapManager, (!(prefabAreaMap != null)) ? null : prefabAreaMap.GetComponent<UIMapManager>(), this._uiRoot.get_transform().FindChild("MapGenerator"), this._clsSortiePrefabFile.prefabUISortieShip.GetComponent<UISortieShip>());
			this._uiSortieMapName.SetMapInformation(mapManager);
		}

		private void GetGoNextData()
		{
			ShipRecoveryType recovery = ShipRecoveryType.None;
			if (RetentionData.GetData().ContainsKey("shipRecoveryType"))
			{
				recovery = (ShipRecoveryType)((int)RetentionData.GetData().get_Item("shipRecoveryType"));
			}
			if ((int)RetentionData.GetData().get_Item("rootType") == 0)
			{
				SingletonMonoBehaviour<FadeCamera>.Instance.SetActive(false);
				this.UpdateUIMapManager();
			}
			else
			{
				this.UpdateUIMapManager();
				SortieMapTaskManager._uiMapManager.InitAfterBattle();
				if (SingletonMonoBehaviour<FadeCamera>.Instance.isFadeOut)
				{
					Observable.Timer(TimeSpan.FromSeconds(0.5)).Subscribe(delegate(long _)
					{
						SingletonMonoBehaviour<FadeCamera>.Instance.FadeIn(0.2f, delegate
						{
							this.GoNext(recovery);
						});
					});
				}
			}
		}

		private void GoNext(ShipRecoveryType iRecoveryType)
		{
			MapManager mm = SortieBattleTaskManager.GetMapManager();
			if (mm.GetNextCellCandidate().get_Count() != 0)
			{
				SortieMapTaskManager.Instance._ctrlActiveBranching = new CtrlActiveBranching(mm.GetNextCellCandidate(), delegate(int x)
				{
					mm.GoNext(iRecoveryType, x);
					SortieMapTaskManager.Instance.UpdateUIMapManager();
					SortieMapTaskManager.ReqMode(SortieMapTaskManagerMode.SortieMapTaskManagerMode_ST);
					Mem.DelIDisposableSafe<CtrlActiveBranching>(ref SortieMapTaskManager.Instance._ctrlActiveBranching);
				});
			}
			else
			{
				mm.GoNext(iRecoveryType);
				SortieMapTaskManager.Instance.UpdateUIMapManager();
				SortieMapTaskManager.ReqMode(SortieMapTaskManagerMode.SortieMapTaskManagerMode_ST);
			}
		}

		private void UpdateUIMapManager()
		{
			MapManager mapManager = SortieBattleTaskManager.GetMapManager();
			SortieMapTaskManager._uiMapManager.UpdatePassedRoutesStates(mapManager);
			SortieMapTaskManager._uiMapManager.UpdateNowNNextCell(mapManager.NowCell, mapManager.NextCell);
			if (SortieMapTaskManager._uiMapManager.nowCell != null)
			{
				SortieMapTaskManager._uiMapManager.nowCell.isPassedCell = true;
			}
			SortieMapTaskManager._uiMapManager.SetShipPosition();
		}

		private void DrawDefaultShip()
		{
			MapManager mapManager = SortieBattleTaskManager.GetMapManager();
			this._uiShipCharacter.SetShipData(mapManager.Deck.GetFlagShip());
			this._uiShipCharacter.DrawDefault();
		}

		protected override bool Init()
		{
			SortieMapTaskManager._iMode = (SortieMapTaskManager._iModeReq = SortieMapTaskManagerMode.SortieMapTaskManagerMode_BEF);
			SortieMapTaskManager._clsTasks.Init(32);
			SoundUtils.SwitchBGM((BGMFileInfos)SortieBattleTaskManager.GetMapManager().BgmId);
			if (!this._isFirstInit)
			{
				if (SingletonMonoBehaviour<FadeCamera>.Instance.isFadeOut)
				{
					Observable.Timer(TimeSpan.FromSeconds(0.5)).Subscribe(delegate(long _)
					{
						SingletonMonoBehaviour<FadeCamera>.Instance.FadeIn(0.2f, delegate
						{
							this.GoNext(SortieBattleTaskManager.GetBattleShipRecoveryType());
						});
					});
				}
				else
				{
					Observable.Timer(TimeSpan.FromSeconds(0.5)).Subscribe(delegate(long _)
					{
						this.GoNext(SortieBattleTaskManager.GetBattleShipRecoveryType());
					});
				}
			}
			else
			{
				SingletonMonoBehaviour<FadeCamera>.Instance.SetActive(false);
				SortieMapTaskManager._iMode = (SortieMapTaskManager._iModeReq = SortieMapTaskManagerMode.SortieMapTaskManagerMode_ST);
			}
			this._uiAreaMapFrame.Show();
			this._isFirstInit = false;
			this._cam.isCulling = true;
			return true;
		}

		protected override bool UnInit()
		{
			SortieMapTaskManager._clsTasks.UnInit();
			SortieMapTaskManager._uiMapManager.wobblingIcons.DestroyDrawWobblingIcons();
			this._uiAreaMapFrame.ClearMessage();
			this._cam.isCulling = false;
			App.TimeScale(1f);
			return true;
		}

		protected override bool Run()
		{
			KeyControl keyControl = SortieBattleTaskManager.GetKeyControl();
			if (SortieMapTaskManager._iMode != SortieMapTaskManagerMode.Formation)
			{
				if (keyControl.GetDown(KeyControl.KeyName.L))
				{
					this._shortCutSwitch.Switch();
				}
			}
			else if (this._shortCutSwitch.isShortCut)
			{
				this._shortCutSwitch.Hide();
			}
			if (this._ctrlActiveBranching != null)
			{
				this._ctrlActiveBranching.Update();
			}
			SortieMapTaskManager._clsTasks.Update();
			this.UpdateMode();
			return SortieBattleTaskManager.GetMode() == SortieBattleMode.SortieBattleMode_BEF || SortieBattleTaskManager.GetMode() == SortieBattleMode.SortieBattleMode_ST;
		}

		public static SortieMapTaskManagerMode GetMode()
		{
			return SortieMapTaskManager._iModeReq;
		}

		public static void ReqMode(SortieMapTaskManagerMode iMode)
		{
			SortieMapTaskManager._iModeReq = iMode;
		}

		protected void UpdateMode()
		{
			if (SortieMapTaskManager._iModeReq == SortieMapTaskManagerMode.SortieMapTaskManagerMode_BEF)
			{
				return;
			}
			switch (SortieMapTaskManager._iModeReq)
			{
			case SortieMapTaskManagerMode.SortieMapTaskManagerMode_ST:
				if (SortieMapTaskManager._clsTasks.Open(SortieMapTaskManager._clsTaskMoveShip, null) < 0)
				{
					return;
				}
				break;
			case SortieMapTaskManagerMode.Event:
				if (SortieMapTaskManager._clsTasks.Open(SortieMapTaskManager._clsTaskEvent, null) < 0)
				{
					return;
				}
				break;
			case SortieMapTaskManagerMode.Formation:
				if (SortieMapTaskManager._clsTasks.Open(SortieMapTaskManager._clsTaskFormation, null) < 0)
				{
					return;
				}
				break;
			case SortieMapTaskManagerMode.Result:
				if (SortieMapTaskManager._clsTasks.Open(SortieMapTaskManager._clsTaskResult, null) < 0)
				{
					return;
				}
				break;
			}
			SortieMapTaskManager._iMode = SortieMapTaskManager._iModeReq;
			SortieMapTaskManager._iModeReq = SortieMapTaskManagerMode.SortieMapTaskManagerMode_BEF;
		}

		public static Transform GetSharedPlace()
		{
			return SortieMapTaskManager.Instance._sharedPlace;
		}

		public static UIShortCutSwitch GetShortCutSwitch()
		{
			return SortieMapTaskManager.Instance._shortCutSwitch;
		}

		public static SortiePrefabFile GetPrefabFile()
		{
			return SortieMapTaskManager.Instance._clsSortiePrefabFile;
		}

		public BaseCamera GetCamera()
		{
			return this._cam;
		}

		public static TaskSortieMoveShip GetTaskMoveShip()
		{
			return SortieMapTaskManager._clsTaskMoveShip;
		}

		public static TaskSortieEvent GetTaskEvent()
		{
			return SortieMapTaskManager._clsTaskEvent;
		}

		public static UIMapManager GetUIMapManager()
		{
			return SortieMapTaskManager._uiMapManager;
		}

		public static UIAreaMapFrame GetUIAreaMapFrame()
		{
			return SortieMapTaskManager.Instance._uiAreaMapFrame;
		}

		public static UISortieShipCharacter GetUIShipCharacter()
		{
			return SortieMapTaskManager.Instance._uiShipCharacter;
		}
	}
}
