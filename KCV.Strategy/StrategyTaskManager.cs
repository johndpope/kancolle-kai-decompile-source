using KCV.Strategy.Rebellion;
using local.managers;
using System;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyTaskManager : SceneTaskMono
	{
		public enum StrategyTaskManagerMode
		{
			StrategyTaskManagerMode_ST,
			StrategyTaskManagerMode_BEF = -1,
			StrategyTaskManagerMode_NONE = -1,
			StrategyTop,
			MapSelect,
			ExercisesPartnerSelection,
			Expedition,
			TransportShipDeployment,
			EscortFleetOrganization,
			Rebellion,
			StrategyTaskManagerMode_AFT,
			StrategyTaskManagerMode_NUM = 7,
			StrategyTaskManagerMode_ED = 6
		}

		private static Transform _traOverView;

		private static Camera _traOverViewCamera;

		private static Transform _traMapRoot;

		private SceneTasksMono _clsTasks;

		private static StrategyTaskManager.StrategyTaskManagerMode _iMode;

		private static StrategyTaskManager.StrategyTaskManagerMode _iModeReq;

		private static StrategyMapManager _clsStrategyMapManager;

		private static StrategyTopTaskManager _clsTopTask;

		private static StrategyRebellionTaskManager _clsRebellionTask;

		private static Action callBack;

		[SerializeField]
		private GameObject KeyManager;

		public static Transform GetOverView()
		{
			return StrategyTaskManager._traOverView;
		}

		public static Camera GetOverViewCamera()
		{
			return StrategyTaskManager._traOverViewCamera;
		}

		public static Transform GetMapRoot()
		{
			return StrategyTaskManager._traMapRoot;
		}

		public static StrategyMapManager GetStrategyMapManager()
		{
			if (StrategyTaskManager._clsStrategyMapManager == null)
			{
				if (StrategyTopTaskManager.GetLogicManager() != null)
				{
					StrategyTaskManager._clsStrategyMapManager = StrategyTopTaskManager.GetLogicManager();
				}
				else
				{
					StrategyTaskManager._clsStrategyMapManager = new StrategyMapManager();
				}
			}
			return StrategyTaskManager._clsStrategyMapManager;
		}

		public static StrategyTopTaskManager GetStrategyTop()
		{
			return StrategyTaskManager._clsTopTask;
		}

		public static StrategyRebellionTaskManager GetStrategyRebellion()
		{
			return StrategyTaskManager._clsRebellionTask;
		}

		protected override void Awake()
		{
			this._clsTasks = this.SafeGetComponent<SceneTasksMono>();
			this._clsTasks.Init();
			GameObject gameObject = base.get_transform().FindChild("Task").get_gameObject();
			StrategyTaskManager._clsTopTask = gameObject.get_transform().FindChild("StrategyTop").GetComponent<StrategyTopTaskManager>();
			StrategyTaskManager._clsRebellionTask = gameObject.get_transform().FindChild("Rebellion").GetComponent<StrategyRebellionTaskManager>();
			if (SingletonMonoBehaviour<FadeCamera>.Instance != null)
			{
				SingletonMonoBehaviour<FadeCamera>.Instance.SetActive(true);
			}
			if (!KeyControlManager.exist())
			{
				Util.Instantiate(this.KeyManager, null, false, false);
			}
			StrategyTaskManager._traOverView = base.get_transform().FindChild("OverView");
			StrategyTaskManager._traOverViewCamera = StrategyTaskManager._traOverView.FindChild("OverViewCamera").GetComponent<Camera>();
			StrategyTaskManager._traMapRoot = base.get_transform().FindChild("Map Root");
		}

		private void OnDestroy()
		{
			this._clsTasks.UnInit();
			StrategyTaskManager._clsTopTask = null;
			StrategyTaskManager._clsRebellionTask = null;
			StrategyTaskManager._traOverView = null;
			StrategyTaskManager._traOverViewCamera = null;
			StrategyTaskManager._traMapRoot = null;
			this._traScenePrefab = null;
			StrategyTaskManager._clsStrategyMapManager = null;
			StrategyTaskManager.callBack = null;
			this.KeyManager = null;
		}

		protected override void Start()
		{
			SingletonMonoBehaviour<PortObjectManager>.Instance.SceneObject = base.get_gameObject();
			SingletonMonoBehaviour<PortObjectManager>.Instance.EnterStrategy();
			StrategyTaskManager._iMode = (StrategyTaskManager._iModeReq = StrategyTaskManager.StrategyTaskManagerMode.StrategyTaskManagerMode_ST);
		}

		private void Update()
		{
			this._clsTasks.Run();
			this.UpdateMode();
		}

		public static StrategyTaskManager.StrategyTaskManagerMode GetMode()
		{
			return StrategyTaskManager._iModeReq;
		}

		public static void ReqMode(StrategyTaskManager.StrategyTaskManagerMode iMode)
		{
			StrategyTaskManager._iModeReq = iMode;
		}

		protected void UpdateMode()
		{
			if (StrategyTaskManager._iModeReq == StrategyTaskManager.StrategyTaskManagerMode.StrategyTaskManagerMode_BEF)
			{
				return;
			}
			switch (StrategyTaskManager._iModeReq)
			{
			case StrategyTaskManager.StrategyTaskManagerMode.StrategyTaskManagerMode_ST:
				if (this._clsTasks.Open(StrategyTaskManager._clsTopTask) < 0)
				{
					return;
				}
				break;
			case StrategyTaskManager.StrategyTaskManagerMode.Rebellion:
				if (this._clsTasks.Open(StrategyTaskManager._clsRebellionTask) < 0)
				{
					return;
				}
				break;
			}
			StrategyTaskManager._iMode = StrategyTaskManager._iModeReq;
			StrategyTaskManager._iModeReq = StrategyTaskManager.StrategyTaskManagerMode.StrategyTaskManagerMode_BEF;
		}

		public static void setCallBack(Action act)
		{
			StrategyTaskManager.callBack = act;
		}

		public static void SceneCallBack()
		{
			StrategyTopTaskManager.GetCommandMenu().DeckEnableCheck();
			if (StrategyTaskManager.callBack != null)
			{
				StrategyTaskManager.callBack.Invoke();
				StrategyTaskManager.callBack = null;
			}
		}
	}
}
