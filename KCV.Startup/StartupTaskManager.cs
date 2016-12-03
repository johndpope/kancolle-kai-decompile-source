using Common.Enum;
using local.models;
using System;
using System.Collections;
using UnityEngine;

namespace KCV.Startup
{
	public class StartupTaskManager : MonoBehaviour
	{
		public enum StartupTaskManagerMode
		{
			StartupTaskManagerMode_ST,
			StartupTaskManagerMode_BEF = -1,
			StartupTaskManagerMode_NONE = -1,
			AdmiralInfo,
			FirstShipSelect,
			PictureStoryShow,
			StartupTaskManagerMode_AFT,
			StartupTaskManagerMode_NUM = 3,
			StartupTaskManagerMode_ED = 2
		}

		private static StartupTaskManager instance;

		[SerializeField]
		private UIStartupHeader _uiStartupHeader;

		[SerializeField]
		private UIStartupNavigation _uiStartupNavigation;

		[SerializeField]
		private Transform _sharedPlace;

		[SerializeField]
		private PSVitaMovie _clsPSVitaMovie;

		[SerializeField]
		private FirstMeetingManager _clsFirstMeetingManager;

		[SerializeField]
		private StartupPrefabFile _clsStartupPrefabFile;

		private static Defines _clsDefines;

		private static StartupData _clsData;

		private static KeyControl _clsInput;

		private static SceneTasksMono _clsTasks;

		private static StartupTaskManager.StartupTaskManagerMode _iMode;

		private static StartupTaskManager.StartupTaskManagerMode _iModeReq;

		private static TaskStartupAdmiralInfo _clsTaskAdmiralInfo;

		private static TaskStartupFirstShipSelect _clsTaskFirstShipSelect;

		private static TaskStartupPictureStoryShow _clsTaskPictureStoryShow;

		private static StartupTaskManager Instance
		{
			get
			{
				if (StartupTaskManager.instance == null)
				{
					StartupTaskManager.instance = Object.FindObjectOfType<StartupTaskManager>();
				}
				return StartupTaskManager.instance;
			}
		}

		private void Awake()
		{
			StartupTaskManager._clsDefines = new Defines();
			StartupTaskManager._clsInput = new KeyControl(0, 0, 0.4f, 0.1f);
			StartupTaskManager._clsData = new StartupData();
			this.SetStartupData();
			StartupTaskManager._clsTasks = base.get_gameObject().SafeGetComponent<SceneTasksMono>();
			Transform transform = GameObject.Find("Tasks").get_transform();
			StartupTaskManager._clsTaskAdmiralInfo = transform.GetComponentInChildren<TaskStartupAdmiralInfo>();
			StartupTaskManager._clsTaskFirstShipSelect = transform.GetComponentInChildren<TaskStartupFirstShipSelect>();
			StartupTaskManager._clsTaskPictureStoryShow = transform.GetComponentInChildren<TaskStartupPictureStoryShow>();
			StartupTaskManager._clsTaskAdmiralInfo.Setup();
			this._uiStartupNavigation.Startup(StartupTaskManager._clsData.isInherit, new SettingModel());
		}

		private void Start()
		{
			StartupTaskManager._iMode = (StartupTaskManager._iModeReq = StartupTaskManager.StartupTaskManagerMode.StartupTaskManagerMode_ST);
			StartupTaskManager._clsTasks.Init();
		}

		private void OnDestroy()
		{
			Mem.Del<UIStartupHeader>(ref this._uiStartupHeader);
			Mem.Del<UIStartupNavigation>(ref this._uiStartupNavigation);
			Mem.Del<Transform>(ref this._sharedPlace);
			Mem.DelIDisposableSafe<StartupPrefabFile>(ref this._clsStartupPrefabFile);
			Mem.Del<PSVitaMovie>(ref this._clsPSVitaMovie);
			Mem.Del<FirstMeetingManager>(ref this._clsFirstMeetingManager);
			Mem.DelIDisposableSafe<Defines>(ref StartupTaskManager._clsDefines);
			Mem.Del<KeyControl>(ref StartupTaskManager._clsInput);
			StartupTaskManager._clsTasks.UnInit();
			Mem.Del<SceneTasksMono>(ref StartupTaskManager._clsTasks);
			Mem.Del<StartupTaskManager.StartupTaskManagerMode>(ref StartupTaskManager._iMode);
			Mem.Del<StartupTaskManager.StartupTaskManagerMode>(ref StartupTaskManager._iModeReq);
			Mem.Del<TaskStartupAdmiralInfo>(ref StartupTaskManager._clsTaskAdmiralInfo);
			Mem.Del<TaskStartupFirstShipSelect>(ref StartupTaskManager._clsTaskFirstShipSelect);
			Mem.Del<TaskStartupPictureStoryShow>(ref StartupTaskManager._clsTaskPictureStoryShow);
			Mem.Del<StartupTaskManager>(ref StartupTaskManager.instance);
			UIDrawCall.ReleaseInactive();
		}

		private void Update()
		{
			if (Input.get_touchCount() == 0 && !Input.GetMouseButton(0))
			{
				StartupTaskManager._clsInput.Update();
			}
			StartupTaskManager._clsTasks.Run();
			this.UpdateMode();
		}

		public static StartupTaskManager.StartupTaskManagerMode GetMode()
		{
			return StartupTaskManager._iModeReq;
		}

		public static void ReqMode(StartupTaskManager.StartupTaskManagerMode iMode)
		{
			StartupTaskManager._iModeReq = iMode;
		}

		protected void UpdateMode()
		{
			if (StartupTaskManager._iModeReq == StartupTaskManager.StartupTaskManagerMode.StartupTaskManagerMode_BEF)
			{
				return;
			}
			switch (StartupTaskManager._iModeReq)
			{
			case StartupTaskManager.StartupTaskManagerMode.StartupTaskManagerMode_ST:
				if (StartupTaskManager._clsTasks.Open(StartupTaskManager._clsTaskAdmiralInfo) < 0)
				{
					return;
				}
				break;
			case StartupTaskManager.StartupTaskManagerMode.FirstShipSelect:
				if (StartupTaskManager._clsTasks.Open(StartupTaskManager._clsTaskFirstShipSelect) < 0)
				{
					return;
				}
				break;
			case StartupTaskManager.StartupTaskManagerMode.PictureStoryShow:
				if (StartupTaskManager._clsTasks.Open(StartupTaskManager._clsTaskPictureStoryShow) < 0)
				{
					return;
				}
				break;
			}
			StartupTaskManager._iMode = StartupTaskManager._iModeReq;
			StartupTaskManager._iModeReq = StartupTaskManager.StartupTaskManagerMode.StartupTaskManagerMode_BEF;
		}

		private void SetStartupData()
		{
			if (RetentionData.GetData() != null)
			{
				Hashtable data = RetentionData.GetData();
				StartupTaskManager._clsData.Difficlty = (DifficultKind)((int)data.get_Item("difficulty"));
				StartupTaskManager._clsData.isInherit = data.ContainsKey("isInherit");
				StartupTaskManager._clsData.AdmiralName = ((!data.ContainsKey("isInherit")) ? string.Empty : App.GetTitleManager().UserName);
			}
			RetentionData.Release();
		}

		public static UIStartupHeader GetStartupHeader()
		{
			return StartupTaskManager.Instance._uiStartupHeader;
		}

		public static UIStartupNavigation GetNavigation()
		{
			return StartupTaskManager.Instance._uiStartupNavigation;
		}

		public static Transform GetSharedPlace()
		{
			return StartupTaskManager.Instance._sharedPlace;
		}

		public static StartupData GetData()
		{
			return StartupTaskManager._clsData;
		}

		public static KeyControl GetKeyControl()
		{
			return StartupTaskManager._clsInput;
		}

		public static StartupPrefabFile GetPrefabFile()
		{
			return StartupTaskManager.Instance._clsStartupPrefabFile;
		}

		public static CtrlPartnerSelect GetPartnerSelect()
		{
			return StartupTaskManager._clsTaskFirstShipSelect.ctrlPartnerSelect;
		}

		public static PSVitaMovie GetPSVitaMovie()
		{
			return StartupTaskManager.Instance._clsPSVitaMovie;
		}

		public static FirstMeetingManager GetFirstMeetingManager()
		{
			return StartupTaskManager.Instance._clsFirstMeetingManager;
		}

		public static bool IsInheritStartup()
		{
			return StartupTaskManager._clsData.isInherit;
		}
	}
}
