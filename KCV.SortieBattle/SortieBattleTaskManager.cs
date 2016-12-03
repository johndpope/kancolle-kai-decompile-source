using Common.Enum;
using KCV.Battle;
using KCV.BattleCut;
using KCV.SortieMap;
using local.managers;
using local.utils;
using Server_Models;
using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace KCV.SortieBattle
{
	public class SortieBattleTaskManager : MonoBehaviour
	{
		private static SortieBattleTaskManager instance;

		[SerializeField]
		private SortieMapTaskManager _clsSortieMapTaskManager;

		[SerializeField]
		private Camera _camTransitionCamera;

		[Header("[SortieBattle Prefab Files]"), SerializeField]
		private SortieBattlePrefabFile _clsSortieBattlePrefabFile;

		private static KeyControl _clsInput;

		private static ShipRecoveryType _iRecoveryType;

		private static MapManager _clsMapManager;

		private static SortieBattleMode _iMode;

		private static SortieBattleMode _iModeReq;

		private static SceneTasksMono _clsTasks;

		private BattleTaskManager _clsBattleTaskManager;

		private BattleCutManager _clsBattleCutManager;

		private static SortieBattleTaskManager Instance
		{
			get
			{
				if (SortieBattleTaskManager.instance == null)
				{
					SortieBattleTaskManager.instance = (Object.FindObjectOfType(typeof(SortieBattleTaskManager)) as SortieBattleTaskManager);
					if (SortieBattleTaskManager.instance == null)
					{
						return null;
					}
				}
				return SortieBattleTaskManager.instance;
			}
		}

		private void OnLevelWasLoaded(int nLevel)
		{
			UIDrawCall.ReleaseInactive();
			SoundFile.ClearAllSE();
		}

		private void Awake()
		{
			SortieBattleTaskManager._clsInput = new KeyControl(0, 0, 0.4f, 0.1f);
			SortieBattleTaskManager._iRecoveryType = ShipRecoveryType.None;
			SortieBattleTaskManager._clsTasks = this.SafeGetComponent<SceneTasksMono>();
			SortieBattleTaskManager._clsTasks.Init();
			SortieBattleTaskManager._iMode = (SortieBattleTaskManager._iModeReq = SortieBattleMode.SortieBattleMode_BEF);
			this._camTransitionCamera.set_enabled(false);
		}

		private void Start()
		{
			Observable.FromCoroutine<bool>((IObserver<bool> observer) => this.Startup(observer)).Subscribe(delegate(bool x)
			{
				SortieBattleTaskManager._iMode = (SortieBattleTaskManager._iModeReq = SortieBattleMode.SortieBattleMode_ST);
			}).AddTo(base.get_gameObject());
		}

		private void OnDestroy()
		{
			TrophyUtil.Unlock_Material();
			Mem.DelIDisposableSafe<SortieBattlePrefabFile>(ref this._clsSortieBattlePrefabFile);
			Mem.Del<Camera>(ref this._camTransitionCamera);
			Mem.Del<KeyControl>(ref SortieBattleTaskManager._clsInput);
			Mem.Del<ShipRecoveryType>(ref SortieBattleTaskManager._iRecoveryType);
			if (SortieBattleTaskManager._clsMapManager != null)
			{
				SortieBattleTaskManager._clsMapManager.MapEnd();
			}
			Mem.Del<MapManager>(ref SortieBattleTaskManager._clsMapManager);
			if (SortieBattleTaskManager._clsTasks != null)
			{
				SortieBattleTaskManager._clsTasks.UnInit();
			}
			Mem.Del<SceneTasksMono>(ref SortieBattleTaskManager._clsTasks);
			Mem.Del<SortieBattleMode>(ref SortieBattleTaskManager._iMode);
			Mem.Del<SortieBattleMode>(ref SortieBattleTaskManager._iModeReq);
			if (this._clsSortieMapTaskManager != null)
			{
				this._clsSortieMapTaskManager.Terminate();
			}
			Mem.Del<SortieMapTaskManager>(ref this._clsSortieMapTaskManager);
			Mem.Del<BattleTaskManager>(ref this._clsBattleTaskManager);
			Mem.Del<BattleCutManager>(ref this._clsBattleCutManager);
			Mst_DataManager.Instance.PurgeUIBattleMaster();
			Mem.Del<SortieBattleTaskManager>(ref SortieBattleTaskManager.instance);
		}

		private void Update()
		{
			if (Input.get_touchCount() == 0 && !Input.GetMouseButton(0) && (SortieBattleTaskManager._iMode != SortieBattleMode.Battle || SortieBattleTaskManager._iMode != SortieBattleMode.BattleCut) && SortieBattleTaskManager._clsInput != null)
			{
				SortieBattleTaskManager._clsInput.Update();
			}
			SortieBattleTaskManager._clsTasks.Run();
			this.UpdateMode();
		}

		[DebuggerHidden]
		private IEnumerator Startup(IObserver<bool> observer)
		{
			SortieBattleTaskManager.<Startup>c__Iterator116 <Startup>c__Iterator = new SortieBattleTaskManager.<Startup>c__Iterator116();
			<Startup>c__Iterator.observer = observer;
			<Startup>c__Iterator.<$>observer = observer;
			<Startup>c__Iterator.<>f__this = this;
			return <Startup>c__Iterator;
		}

		public static SortieBattleMode GetMode()
		{
			return SortieBattleTaskManager._iModeReq;
		}

		public static void ReqMode(SortieBattleMode iMode)
		{
			SortieBattleTaskManager._iModeReq = iMode;
		}

		protected void UpdateMode()
		{
			if (SortieBattleTaskManager._iModeReq == SortieBattleMode.SortieBattleMode_BEF)
			{
				return;
			}
			switch (SortieBattleTaskManager._iModeReq)
			{
			case SortieBattleMode.SortieBattleMode_ST:
				if (SortieBattleTaskManager._clsTasks.Open(this._clsSortieMapTaskManager) < 0)
				{
					return;
				}
				break;
			case SortieBattleMode.Battle:
				if (this._clsBattleTaskManager == null)
				{
					this._clsBattleTaskManager = BattleTaskManager.Instantiate(this._clsSortieBattlePrefabFile.prefabBattleTaskManager.GetComponent<BattleTaskManager>(), delegate(ShipRecoveryType x)
					{
						SortieBattleTaskManager._iRecoveryType = x;
						SortieBattleTaskManager.ReqMode(SortieBattleMode.SortieBattleMode_ST);
						Mem.DelComponentSafe<BattleTaskManager>(ref this._clsBattleTaskManager);
					});
					this.CheckDiscardSortieMapTaskManager();
				}
				break;
			case SortieBattleMode.BattleCut:
				if (this._clsBattleCutManager == null)
				{
					this._clsBattleCutManager = BattleCutManager.Instantiate(SortieBattleTaskManager.GetSortieBattlePrefabFile().prefabBattleCutManager.GetComponent<BattleCutManager>(), Vector3.get_left() * 20f);
					this._clsBattleCutManager.StartBattleCut(SortieBattleTaskManager.GetMapManager(), delegate
					{
						this._clsSortieMapTaskManager.GetCamera().isCulling = true;
					}, delegate(ShipRecoveryType x)
					{
						SortieBattleTaskManager._iRecoveryType = x;
						SortieBattleTaskManager.ReqMode(SortieBattleMode.SortieBattleMode_ST);
						Mem.DelComponentSafe<BattleCutManager>(ref this._clsBattleCutManager);
					});
					this.CheckDiscardSortieMapTaskManager();
				}
				break;
			}
			SortieBattleTaskManager._iMode = SortieBattleTaskManager._iModeReq;
			SortieBattleTaskManager._iModeReq = SortieBattleMode.SortieBattleMode_BEF;
		}

		public static bool IsRebellionSortieBattle()
		{
			return SortieBattleTaskManager._clsMapManager != null && SortieBattleTaskManager._clsMapManager is RebellionMapManager;
		}

		private void CheckDiscardSortieMapTaskManager()
		{
			if (SortieBattleTaskManager._clsMapManager.IsNextFinal())
			{
				Observable.NextFrame(FrameCountType.EndOfFrame).Subscribe(delegate(Unit _)
				{
					if (this._clsSortieMapTaskManager != null)
					{
						this._clsSortieMapTaskManager.Terminate();
					}
					Mem.DelComponentSafe<SortieMapTaskManager>(ref this._clsSortieMapTaskManager);
				});
			}
		}

		public static ShipRecoveryType GetBattleShipRecoveryType()
		{
			return SortieBattleTaskManager._iRecoveryType;
		}

		public static SortieBattlePrefabFile GetSortieBattlePrefabFile()
		{
			if (SortieBattleTaskManager.Instance == null)
			{
				return null;
			}
			return SortieBattleTaskManager.Instance._clsSortieBattlePrefabFile;
		}

		public static KeyControl GetKeyControl()
		{
			return SortieBattleTaskManager._clsInput;
		}

		public static MapManager GetMapManager()
		{
			return (SortieBattleTaskManager._clsMapManager == null) ? BattleTaskManager.GetMapManager() : SortieBattleTaskManager._clsMapManager;
		}

		public static Camera GetTransitionCamera()
		{
			if (SortieBattleTaskManager.Instance == null)
			{
				return null;
			}
			return SortieBattleTaskManager.Instance._camTransitionCamera;
		}

		private void OnSetMapManager(MapManager mapManager)
		{
			SortieBattleTaskManager._clsMapManager = mapManager;
		}
	}
}
