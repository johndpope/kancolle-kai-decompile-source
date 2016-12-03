using KCV.Strategy.Deploy;
using KCV.Tutorial.Guide;
using KCV.Utils;
using local.managers;
using local.utils;
using Server_Common;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyTopTaskManager : SceneTaskMono
	{
		public enum StrategyTopTaskManagerMode
		{
			StrategyTopTaskManagerMode_ST,
			StrategyTopTaskManagerMode_BEF = -1,
			StrategyTopTaskManagerMode_NONE = -1,
			SailSelect,
			CommandMenu,
			ShipMove,
			MapSelect,
			Deploy,
			Debug,
			Info,
			TurnEnd,
			StrategyTopTaskManagerMode_AFT,
			StrategyTopTaskManagerMode_NUM = 8,
			StrategyTopTaskManagerMode_ED = 7
		}

		public delegate void CreateTutorialGuideInstance(TutorialGuide ins);

		[SerializeField]
		private StrategyUIModel uiModel;

		private GameObject InfoLayer;

		public GameObject InfoRoot;

		public StrategyCamera strategyCamera;

		public AsyncObjects InfoAsyncObj;

		public Action initAction;

		public static StrategyTopTaskManager Instance;

		private Coroutine StartCor;

		private SceneTasksMono _clsTasks;

		private static StrategyTopTaskManager.StrategyTopTaskManagerMode _iMode;

		private static StrategyTopTaskManager.StrategyTopTaskManagerMode _iModeReq;

		private static TaskStrategySailSelect _clsSailSelect;

		private static TaskStrategyCommandMenu _clsCommandMenuTask;

		private static TaskStrategyShipMove _clsShipMove;

		private static TaskStrategyMapSelect _clsMapSelect;

		private static TaskDeployTop _clsDeploy;

		private static TaskStrategyAreaInfo _clsAreaInfo;

		private static TaskStrategyDebug _clsDebug;

		private static StrategyTurnEnd _clsTurnEnd;

		private static StrategyMapManager StrategyLogicManager;

		private bool AnimationEnd;

		private GameObject AlertToastCamera;

		public StrategyUIModel UIModel
		{
			get
			{
				return this.uiModel;
			}
			private set
			{
				this.uiModel = value;
			}
		}

		public StrategyHexTileManager TileManager
		{
			get
			{
				return this.uiModel.UIMapManager.TileManager;
			}
		}

		public StrategyShipManager ShipIconManager
		{
			get
			{
				return this.uiModel.UIMapManager.ShipIconManager;
			}
		}

		public TutorialGuide TutorialGuide6_1
		{
			get;
			private set;
		}

		public TutorialGuide TutorialGuide6_2
		{
			get;
			private set;
		}

		public TutorialGuide TutorialGuide8_1
		{
			get;
			private set;
		}

		public TutorialGuide TutorialGuide9_1
		{
			get;
			private set;
		}

		public static TaskStrategySailSelect GetSailSelect()
		{
			return StrategyTopTaskManager._clsSailSelect;
		}

		public static TaskStrategyCommandMenu GetCommandMenu()
		{
			return StrategyTopTaskManager._clsCommandMenuTask;
		}

		public static TaskStrategyShipMove GetShipMove()
		{
			return StrategyTopTaskManager._clsShipMove;
		}

		public static TaskStrategyMapSelect GetMapSelect()
		{
			return StrategyTopTaskManager._clsMapSelect;
		}

		public static TaskDeployTop GetDeploy()
		{
			return StrategyTopTaskManager._clsDeploy;
		}

		public static TaskStrategyAreaInfo GetAreaInfoTask()
		{
			return StrategyTopTaskManager._clsAreaInfo;
		}

		public static TaskStrategyDebug GetDebug()
		{
			return StrategyTopTaskManager._clsDebug;
		}

		public static void SetDebug(TaskStrategyDebug debug)
		{
			StrategyTopTaskManager._clsDebug = debug;
		}

		public static StrategyTurnEnd GetTurnEnd()
		{
			return StrategyTopTaskManager._clsTurnEnd;
		}

		public static StrategyMapManager GetLogicManager()
		{
			return StrategyTopTaskManager.StrategyLogicManager;
		}

		public static void CreateLogicManager()
		{
			StrategyTopTaskManager.StrategyLogicManager = new StrategyMapManager();
		}

		public StrategyInfoManager GetInfoMng()
		{
			return this.uiModel.InfoManager;
		}

		public StrategyAreaManager GetAreaMng()
		{
			return this.uiModel.AreaManager;
		}

		protected override void Awake()
		{
			DebugUtils.SLog("+++ StrategyTopTask +++");
			if (!AppInitializeManager.IsInitialize)
			{
				return;
			}
			StrategyTopTaskManager.StrategyLogicManager = new StrategyMapManager();
			StrategyTopTaskManager.Instance = this;
			this.GetRef();
		}

		private void OnValidate()
		{
			if (Application.get_isPlaying())
			{
				this.GetRef();
			}
		}

		private void GetRef()
		{
			StrategyTopTaskManager._clsCommandMenuTask = base.get_transform().FindChild("CommandMenu").GetComponent<TaskStrategyCommandMenu>();
			StrategyTopTaskManager._clsSailSelect = base.get_transform().FindChild("SailSelect").GetComponent<TaskStrategySailSelect>();
			StrategyTopTaskManager._clsShipMove = base.get_transform().FindChild("ShipMove").GetComponent<TaskStrategyShipMove>();
			StrategyTopTaskManager._clsMapSelect = base.get_transform().FindChild("MapSelect").GetComponent<TaskStrategyMapSelect>();
			StrategyTopTaskManager._clsDeploy = base.get_transform().FindChild("Deploy").GetComponent<TaskDeployTop>();
			StrategyTopTaskManager._clsAreaInfo = base.get_transform().FindChild("Record").GetComponent<TaskStrategyAreaInfo>();
			StrategyTopTaskManager._clsDebug = base.get_transform().FindChild("Debug").GetComponent<TaskStrategyDebug>();
			StrategyTopTaskManager._clsTurnEnd = base.get_transform().FindChild("TurnEnd").GetComponent<StrategyTurnEnd>();
		}

		private void Start()
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
			SingletonMonoBehaviour<FadeCamera>.Instance.SetTransitionRule(FadeCamera.TransitionRule.Transition1);
			this.DelayActionFrame(1, delegate
			{
				int deckID = (!this.isNoneCurrentFlagShip()) ? SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.Id : 1;
				SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck = StrategyTopTaskManager.GetSailSelect().changeDeck(deckID);
				StrategyTopTaskManager.GetSailSelect().sailSelectFirstInit();
				this.GetAreaMng().init();
				this.GetInfoMng().init();
				SingletonMonoBehaviour<FadeCamera>.Instance.isDrawNowLoading = true;
				SingletonMonoBehaviour<NowLoadingAnimation>.Instance.isNowLoadingAnimation = false;
				Debug.Log("StrategyTopTaskManager Start");
				this.GetAreaMng().UpdateSelectArea(StrategyAreaManager.FocusAreaID, true);
				this.TileManager.setVisibleFocusObject(false);
			});
		}

		private bool isNoneCurrentFlagShip()
		{
			return SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck == null || SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.GetFlagShip() == null;
		}

		protected override bool Init()
		{
			this._clsTasks = this.SafeGetComponent<SceneTasksMono>();
			if (this._clsTasks.tasks == null)
			{
				this._clsTasks.Init();
				this.AnimationEnd = false;
				if (Server_Common.Utils.IsGameClear())
				{
					SoundUtils.StopBGM();
				}
				else
				{
					SoundUtils.SwitchBGM(BGMFileInfos.Strategy);
				}
				this.StartCor = base.StartCoroutine(this.StrategyStart());
			}
			else
			{
				StrategyTopTaskManager._iMode = (StrategyTopTaskManager._iModeReq = StrategyTopTaskManager.StrategyTopTaskManagerMode.StrategyTopTaskManagerMode_ST);
			}
			return true;
		}

		protected override bool UnInit()
		{
			return true;
		}

		protected override bool Run()
		{
			if (!this.AnimationEnd)
			{
				return true;
			}
			if (this._clsTasks == null)
			{
				this._clsTasks = this.SafeGetComponent<SceneTasksMono>();
			}
			this._clsTasks.Run();
			this.UpdateMode();
			return StrategyTaskManager.GetMode() == StrategyTaskManager.StrategyTaskManagerMode.StrategyTaskManagerMode_BEF || StrategyTaskManager.GetMode() == StrategyTaskManager.StrategyTaskManagerMode.StrategyTaskManagerMode_ST;
		}

		private void OnDestroy()
		{
			StrategyTopTaskManager.Instance = null;
			this.uiModel = null;
			StrategyTopTaskManager._clsCommandMenuTask = null;
			StrategyTopTaskManager._clsSailSelect = null;
			StrategyTopTaskManager._clsShipMove = null;
			StrategyTopTaskManager._clsMapSelect = null;
			StrategyTopTaskManager._clsDeploy = null;
			StrategyTopTaskManager._clsAreaInfo = null;
			StrategyTopTaskManager._clsDebug = null;
			StrategyTopTaskManager._clsTurnEnd = null;
			StrategyTopTaskManager.StrategyLogicManager = null;
			if (this.AlertToastCamera != null)
			{
				this.AlertToastCamera.SetActive(true);
			}
			this.AlertToastCamera = null;
			this.TutorialGuide6_1 = null;
			this.TutorialGuide6_2 = null;
			this.TutorialGuide8_1 = null;
			this.TutorialGuide9_1 = null;
			this.StartCor = null;
		}

		public static StrategyTopTaskManager.StrategyTopTaskManagerMode GetMode()
		{
			return StrategyTopTaskManager._iModeReq;
		}

		public static void ReqMode(StrategyTopTaskManager.StrategyTopTaskManagerMode iMode)
		{
			StrategyTopTaskManager._iModeReq = iMode;
		}

		protected void UpdateMode()
		{
			if (StrategyTopTaskManager._iModeReq == StrategyTopTaskManager.StrategyTopTaskManagerMode.StrategyTopTaskManagerMode_BEF)
			{
				return;
			}
			switch (StrategyTopTaskManager._iModeReq)
			{
			case StrategyTopTaskManager.StrategyTopTaskManagerMode.StrategyTopTaskManagerMode_ST:
				if (this._clsTasks.Open(StrategyTopTaskManager._clsSailSelect) < 0)
				{
					return;
				}
				break;
			case StrategyTopTaskManager.StrategyTopTaskManagerMode.CommandMenu:
				if (this._clsTasks.Open(StrategyTopTaskManager._clsCommandMenuTask) < 0)
				{
					return;
				}
				break;
			case StrategyTopTaskManager.StrategyTopTaskManagerMode.ShipMove:
				if (this._clsTasks.Open(StrategyTopTaskManager._clsShipMove) < 0)
				{
					return;
				}
				break;
			case StrategyTopTaskManager.StrategyTopTaskManagerMode.MapSelect:
				if (this._clsTasks.Open(StrategyTopTaskManager._clsMapSelect) < 0)
				{
					return;
				}
				break;
			case StrategyTopTaskManager.StrategyTopTaskManagerMode.Deploy:
				if (this._clsTasks.Open(StrategyTopTaskManager._clsDeploy) < 0)
				{
					return;
				}
				break;
			case StrategyTopTaskManager.StrategyTopTaskManagerMode.Debug:
				if (this._clsTasks.Open(StrategyTopTaskManager._clsDebug) < 0)
				{
					return;
				}
				break;
			case StrategyTopTaskManager.StrategyTopTaskManagerMode.Info:
				if (this._clsTasks.Open(StrategyTopTaskManager._clsAreaInfo) < 0)
				{
					return;
				}
				break;
			case StrategyTopTaskManager.StrategyTopTaskManagerMode.TurnEnd:
				if (this._clsTasks.Open(StrategyTopTaskManager._clsTurnEnd) < 0)
				{
					return;
				}
				break;
			}
			StrategyTopTaskManager._iMode = StrategyTopTaskManager._iModeReq;
			StrategyTopTaskManager._iModeReq = StrategyTopTaskManager.StrategyTopTaskManagerMode.StrategyTopTaskManagerMode_BEF;
		}

		public void ButtonClick()
		{
			int index = KeyControlManager.Instance.KeyController.Index;
		}

		[DebuggerHidden]
		private IEnumerator StrategyStart()
		{
			StrategyTopTaskManager.<StrategyStart>c__Iterator188 <StrategyStart>c__Iterator = new StrategyTopTaskManager.<StrategyStart>c__Iterator188();
			<StrategyStart>c__Iterator.<>f__this = this;
			return <StrategyStart>c__Iterator;
		}

		public void setActiveStrategy(bool isActive)
		{
			this.TileManager.setActivePositionAnimations(isActive);
			this.TileManager.setVisibleFocusObject(isActive);
			this.uiModel.UIMapManager.ShipIconManager.DeckSelectCursol.SetActive(isActive);
		}

		[DebuggerHidden]
		public IEnumerator TutorialCheck()
		{
			StrategyTopTaskManager.<TutorialCheck>c__Iterator189 <TutorialCheck>c__Iterator = new StrategyTopTaskManager.<TutorialCheck>c__Iterator189();
			<TutorialCheck>c__Iterator.<>f__this = this;
			return <TutorialCheck>c__Iterator;
		}

		[DebuggerHidden]
		private IEnumerator LoadStepTutorial(string prefabPath, StrategyTopTaskManager.CreateTutorialGuideInstance del, int tutorialID = -1)
		{
			StrategyTopTaskManager.<LoadStepTutorial>c__Iterator18A <LoadStepTutorial>c__Iterator18A = new StrategyTopTaskManager.<LoadStepTutorial>c__Iterator18A();
			<LoadStepTutorial>c__Iterator18A.prefabPath = prefabPath;
			<LoadStepTutorial>c__Iterator18A.del = del;
			<LoadStepTutorial>c__Iterator18A.tutorialID = tutorialID;
			<LoadStepTutorial>c__Iterator18A.<$>prefabPath = prefabPath;
			<LoadStepTutorial>c__Iterator18A.<$>del = del;
			<LoadStepTutorial>c__Iterator18A.<$>tutorialID = tutorialID;
			<LoadStepTutorial>c__Iterator18A.<>f__this = this;
			return <LoadStepTutorial>c__Iterator18A;
		}

		[DebuggerHidden]
		private IEnumerator WaitforLoad(string prefabPath)
		{
			StrategyTopTaskManager.<WaitforLoad>c__Iterator18B <WaitforLoad>c__Iterator18B = new StrategyTopTaskManager.<WaitforLoad>c__Iterator18B();
			<WaitforLoad>c__Iterator18B.prefabPath = prefabPath;
			<WaitforLoad>c__Iterator18B.<$>prefabPath = prefabPath;
			return <WaitforLoad>c__Iterator18B;
		}

		public void GameOver()
		{
			SoundUtils.StopBGM();
			if (this.StartCor != null)
			{
				base.StopCoroutine(this.StartCor);
			}
			GameObject gameObject = Resources.Load("Prefabs/Ending/EndPhase") as GameObject;
			bool clear = false;
			bool isTurnOver = Server_Common.Utils.IsTurnOver();
			KeyControl keyControl = new KeyControl(0, 0, 0.4f, 0.1f);
			App.OnlyController = keyControl;
			SoundUtils.PlaySE(SEFIleInfos.FanfareE);
			ProdEndPhase prodEndPhase = ProdEndPhase.Instantiate(gameObject.GetComponent<ProdEndPhase>(), this.uiModel.OverView, keyControl, clear, isTurnOver);
			prodEndPhase.Play(delegate
			{
				App.OnlyController = null;
				base.StartCoroutine(this.GoToTitle());
			});
		}

		[DebuggerHidden]
		private IEnumerator GoToTitle()
		{
			StrategyTopTaskManager.<GoToTitle>c__Iterator18C <GoToTitle>c__Iterator18C = new StrategyTopTaskManager.<GoToTitle>c__Iterator18C();
			<GoToTitle>c__Iterator18C.<>f__this = this;
			return <GoToTitle>c__Iterator18C;
		}

		public void GameClear()
		{
			SoundUtils.StopBGM();
			SoundUtils.StopSEAll(0.5f);
			TrophyUtil.Unlock_At_GameClear();
			base.StartCoroutine(this.GameClearCoroutine());
		}

		[DebuggerHidden]
		private IEnumerator GameClearCoroutine()
		{
			StrategyTopTaskManager.<GameClearCoroutine>c__Iterator18D <GameClearCoroutine>c__Iterator18D = new StrategyTopTaskManager.<GameClearCoroutine>c__Iterator18D();
			<GameClearCoroutine>c__Iterator18D.<>f__this = this;
			return <GameClearCoroutine>c__Iterator18D;
		}

		[DebuggerHidden]
		private IEnumerator GoToEnding()
		{
			return new StrategyTopTaskManager.<GoToEnding>c__Iterator18E();
		}
	}
}
