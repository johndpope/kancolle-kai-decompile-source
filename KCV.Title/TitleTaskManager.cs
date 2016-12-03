using local.managers;
using local.models;
using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace KCV.Title
{
	public class TitleTaskManager : MonoBehaviour
	{
		private static TitleTaskManager instance;

		[SerializeField]
		private UITitleBackground _uiTitleBackground;

		[SerializeField]
		private UITitleLogo _uiTitleLogo;

		[SerializeField]
		private Transform _sharedPlace;

		[SerializeField]
		private TitlePrefabFile _clsPrefabFile;

		[SerializeField]
		private PSVitaMovie _clsVitaMovie;

		[SerializeField]
		private UIPanel _uiMaskPanel;

		[SerializeField]
		private UILabel _uiMasterVersion;

		private Generics.InnerCamera _camTitle;

		private static KeyControl _clsInput;

		private static bool _isPlayOpening = true;

		private static SceneTasksMono _clsTasks;

		private static TitleTaskManagerMode _iMode;

		private static TitleTaskManagerMode _iModeReq;

		private static TaskTitleOpening _clsTaskOpening;

		private static TaskTitleSelectMode _clsTaskSelectMode;

		private static TaskTitleNewGame _clsTaskNewGame;

		private static TitleTaskManager Instance
		{
			get
			{
				if (TitleTaskManager.instance == null)
				{
					TitleTaskManager.instance = Object.FindObjectOfType<TitleTaskManager>();
				}
				return TitleTaskManager.instance;
			}
		}

		private void Awake()
		{
			if (App.GetTitleManager() == null)
			{
				App.SetTitleManager(new TitleManager());
			}
			TitleTaskManager._clsTasks = base.get_gameObject().SafeGetComponent<SceneTasksMono>();
			TitleTaskManager._clsInput = new KeyControl(0, 0, 0.4f, 0.1f);
			this._camTitle = new Generics.InnerCamera(base.get_transform().FindChild("TitleCamera").get_transform());
			TitleTaskManager._clsTaskOpening = base.get_transform().GetComponentInChildren<TaskTitleOpening>();
			TitleTaskManager._clsTaskSelectMode = base.get_transform().GetComponentInChildren<TaskTitleSelectMode>();
			TitleTaskManager._clsTaskNewGame = base.get_transform().GetComponentInChildren<TaskTitleNewGame>();
			Util.SetRootContentSize(base.GetComponent<UIRoot>(), App.SCREEN_RESOLUTION);
			this._uiMasterVersion.text = "Version 1.02";
			App.SetSoundSettings(new SettingModel());
			if (TitleTaskManager._isPlayOpening)
			{
				this._uiMaskPanel.alpha = 1f;
				TitleTaskManager._clsTaskOpening.PlayImmediateOpeningMovie();
				TitleTaskManager._iMode = (TitleTaskManager._iModeReq = TitleTaskManagerMode.TitleTaskManagerMode_ST);
				TitleTaskManager._isPlayOpening = false;
			}
			else
			{
				this._uiMaskPanel.alpha = 0f;
				if (SingletonMonoBehaviour<FadeCamera>.Instance != null && SingletonMonoBehaviour<FadeCamera>.Instance.isFadeOut)
				{
					SingletonMonoBehaviour<FadeCamera>.Instance.FadeIn(0.2f, null);
				}
				TitleTaskManager._iMode = (TitleTaskManager._iModeReq = TitleTaskManagerMode.SelectMode);
			}
		}

		private void Start()
		{
			TitleTaskManager._clsTasks.Init();
			Observable.FromCoroutine(new Func<IEnumerator>(this._uiTitleBackground.StartBackgroundAnim), false).Subscribe<Unit>().AddTo(base.get_gameObject());
		}

		private void OnDestroy()
		{
			Mem.Del<UITitleBackground>(ref this._uiTitleBackground);
			Mem.Del<UITitleLogo>(ref this._uiTitleLogo);
			Mem.Del<Transform>(ref this._sharedPlace);
			this._clsPrefabFile.Dispose();
			Mem.Del<TitlePrefabFile>(ref this._clsPrefabFile);
			Mem.Del<PSVitaMovie>(ref this._clsVitaMovie);
			Mem.Del<UIPanel>(ref this._uiMaskPanel);
			Mem.Del<UILabel>(ref this._uiMasterVersion);
			this._camTitle.UnInit();
			Mem.Del<Generics.InnerCamera>(ref this._camTitle);
			Mem.Del<KeyControl>(ref TitleTaskManager._clsInput);
			TitleTaskManager._clsTasks.UnInit();
			Mem.Del<TitleTaskManagerMode>(ref TitleTaskManager._iMode);
			Mem.Del<TitleTaskManagerMode>(ref TitleTaskManager._iModeReq);
			Mem.Del<TaskTitleOpening>(ref TitleTaskManager._clsTaskOpening);
			Mem.Del<TaskTitleSelectMode>(ref TitleTaskManager._clsTaskSelectMode);
			Mem.Del<TaskTitleNewGame>(ref TitleTaskManager._clsTaskNewGame);
			Mem.Del<SceneTasksMono>(ref TitleTaskManager._clsTasks);
			Mem.Del<TitleTaskManager>(ref TitleTaskManager.instance);
		}

		private void Update()
		{
			if (Input.get_touchCount() == 0 && !Input.GetMouseButton(0) && TitleTaskManager._clsInput != null)
			{
				TitleTaskManager._clsInput.Update();
			}
			TitleTaskManager._clsTasks.Run();
			this.UpdateMode();
		}

		public static TitleTaskManagerMode GetMode()
		{
			return TitleTaskManager._iModeReq;
		}

		public static void ReqMode(TitleTaskManagerMode iMode)
		{
			TitleTaskManager._iModeReq = iMode;
		}

		protected void UpdateMode()
		{
			if (TitleTaskManager._iModeReq == TitleTaskManagerMode.TitleTaskManagerMode_BEF)
			{
				return;
			}
			switch (TitleTaskManager._iModeReq)
			{
			case TitleTaskManagerMode.TitleTaskManagerMode_ST:
				if (TitleTaskManager._clsTasks.Open(TitleTaskManager._clsTaskOpening) < 0)
				{
					return;
				}
				break;
			case TitleTaskManagerMode.SelectMode:
				if (TitleTaskManager._clsTasks.Open(TitleTaskManager._clsTaskSelectMode) < 0)
				{
					return;
				}
				break;
			case TitleTaskManagerMode.NewGame:
				if (TitleTaskManager._clsTasks.Open(TitleTaskManager._clsTaskNewGame) < 0)
				{
					return;
				}
				break;
			}
			TitleTaskManager._iMode = TitleTaskManager._iModeReq;
			TitleTaskManager._iModeReq = TitleTaskManagerMode.TitleTaskManagerMode_BEF;
		}

		public static KeyControl GetKeyControl()
		{
			return TitleTaskManager._clsInput;
		}

		public static Transform GetSharedPlace()
		{
			return TitleTaskManager.Instance._sharedPlace;
		}

		public static UIPanel GetMaskPanel()
		{
			return TitleTaskManager.Instance._uiMaskPanel;
		}

		public static PSVitaMovie GetPSVitaMovie()
		{
			return TitleTaskManager.Instance._clsVitaMovie;
		}

		public static UITitleLogo GetUITitleLogo()
		{
			return TitleTaskManager.Instance._uiTitleLogo;
		}

		public static TitlePrefabFile GetPrefabFile()
		{
			return TitleTaskManager.Instance._clsPrefabFile;
		}

		[DebuggerHidden]
		public static IEnumerator GotoLoadScene(IObserver<AsyncOperation> observer)
		{
			TitleTaskManager.<GotoLoadScene>c__Iterator1A3 <GotoLoadScene>c__Iterator1A = new TitleTaskManager.<GotoLoadScene>c__Iterator1A3();
			<GotoLoadScene>c__Iterator1A.observer = observer;
			<GotoLoadScene>c__Iterator1A.<$>observer = observer;
			return <GotoLoadScene>c__Iterator1A;
		}
	}
}
