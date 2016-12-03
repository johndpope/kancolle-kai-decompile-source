using KCV.Scene.Port;
using local.managers;
using local.models;
using System;
using UnityEngine;

namespace KCV.Arsenal
{
	public class ArsenalTaskManager : MonoBehaviour
	{
		public enum ArsenalPhase
		{
			BattlePhase_ST,
			BattlePhase_BEF = -1,
			BattlePhase_NONE = -1,
			MainArsenal,
			NormalConstruct,
			Development,
			List,
			BattlePhase_AFT,
			BattlePhase_NUM = 4,
			BattlePhase_ED = 3
		}

		private static GameObject _uiCommon;

		private static KeyControl _clsInputKey;

		private static SceneTasksMono _clsTasks;

		private static ArsenalTaskManager.ArsenalPhase _iPhase;

		private static ArsenalTaskManager.ArsenalPhase _iPhaseReq;

		public static TaskMainArsenalManager _clsArsenal;

		public static TaskConstructManager _clsConstruct;

		public static TaskArsenalListManager _clsList;

		private static ArsenalManager logicManager;

		private static SoundManager soundManager;

		private static BaseDialogPopup dialogPopUp;

		private static AsyncObjects asyncObj;

		private static CommonPopupDialog commonPopup;

		private static UIPanel _uiBgPanel;

		private static DeckModel[] _deck;

		private static ShipModel[] _ship;

		private static ShipModel[] _allShip;

		private static BuildDockModel[] dock;

		public static bool IsArsenalCreate;

		public static bool IsConstructCreate;

		private static int _dockIndex;

		[SerializeField]
		private Texture[] mTextures_Preload;

		[SerializeField]
		private ParticleSystem[] mParticleSystems_Managed;

		public static ArsenalManager GetLogicManager()
		{
			return ArsenalTaskManager.logicManager;
		}

		public static SoundManager GetSoundManager()
		{
			return ArsenalTaskManager.soundManager;
		}

		public static BaseDialogPopup GetDialogPopUp()
		{
			return ArsenalTaskManager.dialogPopUp;
		}

		public static DeckModel[] GetDeck()
		{
			return ArsenalTaskManager._deck;
		}

		public static ShipModel[] GetShip()
		{
			return ArsenalTaskManager._ship;
		}

		public static ShipModel[] GetAllShip()
		{
			return ArsenalTaskManager._allShip;
		}

		public static int GetDockIndex()
		{
			return ArsenalTaskManager._dockIndex;
		}

		private void Awake()
		{
			ArsenalTaskManager._clsInputKey = new KeyControl(0, 0, 0.4f, 0.1f);
			ArsenalTaskManager._clsInputKey.useDoubleIndex(0, 5);
			ArsenalTaskManager._clsTasks = base.get_gameObject().SafeGetComponent<SceneTasksMono>();
			GameObject gameObject = base.get_transform().FindChild("TaskArsenalMain").get_gameObject();
			GameObject gameObject2 = gameObject.get_transform().FindChild("Task").get_gameObject();
			ArsenalTaskManager._clsArsenal = gameObject2.get_transform().FindChild("Arsenal").GetComponent<TaskMainArsenalManager>();
			ArsenalTaskManager._clsConstruct = gameObject2.get_transform().FindChild("Construct").GetComponent<TaskConstructManager>();
			ArsenalTaskManager._clsList = gameObject2.get_transform().FindChild("TaskArsenalListManager").GetComponent<TaskArsenalListManager>();
			ArsenalTaskManager.logicManager = new ArsenalManager();
			ArsenalTaskManager.dialogPopUp = new BaseDialogPopup();
		}

		private void Start()
		{
			ArsenalTaskManager._clsTasks.Init();
			ArsenalTaskManager._iPhase = (ArsenalTaskManager._iPhaseReq = ArsenalTaskManager.ArsenalPhase.BattlePhase_ST);
			SingletonMonoBehaviour<UIPortFrame>.Instance.CircleUpdateInfo(ArsenalTaskManager.logicManager);
			ArsenalTaskManager._clsConstruct.firstInit();
			ArsenalTaskManager._clsList.firstInit();
		}

		private void Update()
		{
			ArsenalTaskManager._clsInputKey.Update();
			ArsenalTaskManager._clsTasks.Run();
			this.UpdateMode();
		}

		public static void ReqPhase(ArsenalTaskManager.ArsenalPhase iPhase)
		{
			ArsenalTaskManager._iPhaseReq = iPhase;
		}

		protected void UpdateMode()
		{
			if (ArsenalTaskManager._iPhaseReq == ArsenalTaskManager.ArsenalPhase.BattlePhase_BEF)
			{
				return;
			}
			switch (ArsenalTaskManager._iPhaseReq)
			{
			case ArsenalTaskManager.ArsenalPhase.BattlePhase_ST:
				if (ArsenalTaskManager._clsTasks.Open(ArsenalTaskManager._clsArsenal) < 0)
				{
					return;
				}
				break;
			case ArsenalTaskManager.ArsenalPhase.NormalConstruct:
				if (ArsenalTaskManager._clsTasks.Open(ArsenalTaskManager._clsConstruct) < 0)
				{
					return;
				}
				break;
			case ArsenalTaskManager.ArsenalPhase.List:
				if (ArsenalTaskManager._clsTasks.Open(ArsenalTaskManager._clsList) < 0)
				{
					return;
				}
				break;
			}
			ArsenalTaskManager._iPhase = ArsenalTaskManager._iPhaseReq;
			ArsenalTaskManager._iPhaseReq = ArsenalTaskManager.ArsenalPhase.BattlePhase_BEF;
		}

		public static KeyControl GetKeyControl()
		{
			return ArsenalTaskManager._clsInputKey;
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Releases(ref this.mTextures_Preload, false);
			UserInterfacePortManager.ReleaseUtils.Releases(ref this.mParticleSystems_Managed);
			ArsenalTaskManager._uiCommon = null;
			ArsenalTaskManager._clsInputKey = null;
			ArsenalTaskManager._clsTasks = null;
			ArsenalTaskManager._clsArsenal = null;
			ArsenalTaskManager._clsConstruct = null;
			ArsenalTaskManager._clsList = null;
			ArsenalTaskManager.logicManager = null;
			ArsenalTaskManager.soundManager = null;
			ArsenalTaskManager.dialogPopUp = null;
			ArsenalTaskManager.asyncObj = null;
			ArsenalTaskManager.commonPopup = null;
			ArsenalTaskManager._uiBgPanel = null;
			Mem.DelAry<DeckModel>(ref ArsenalTaskManager._deck);
			Mem.DelAry<ShipModel>(ref ArsenalTaskManager._ship);
			Mem.DelAry<ShipModel>(ref ArsenalTaskManager._allShip);
			Mem.DelAry<BuildDockModel>(ref ArsenalTaskManager.dock);
			UIDrawCall.ReleaseInactive();
		}
	}
}
