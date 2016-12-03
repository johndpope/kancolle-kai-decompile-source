using local.managers;
using local.models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace KCV.Strategy.Rebellion
{
	public class StrategyRebellionTaskManager : SceneTaskMono
	{
		[SerializeField]
		private TaskRebellionEvent _clsTaskEvent;

		[SerializeField]
		private TaskRebellionOrganize _clsTaskOrgnaize;

		[SerializeField]
		private Transform _prefabProdWaringBackground;

		private KeyControl _clsInput;

		private SceneTasksMono _clsTasks;

		private StrategyRebellionTaskManagerMode _iMode;

		private StrategyRebellionTaskManagerMode _iModeReq;

		private RebellionManager _clsRebellionManager;

		public static bool RebellionForceDebug;

		public static int RebellionArea;

		public static int RebellionFromArea;

		public KeyControl keycontrol
		{
			get
			{
				return this._clsInput;
			}
		}

		public TaskRebellionEvent taskRebellionEvent
		{
			get
			{
				return this._clsTaskEvent;
			}
		}

		public TaskRebellionOrganize taskRebellionOrganize
		{
			get
			{
				return this._clsTaskOrgnaize;
			}
		}

		protected override bool Init()
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
			int rebellionArea = StrategyRebellionTaskManager.RebellionArea;
			this._clsRebellionManager = StrategyTopTaskManager.GetLogicManager().SelectAreaForRebellion(rebellionArea);
			this._clsInput = new KeyControl(0, 0, 0.4f, 0.1f);
			this._clsTasks = this.SafeGetComponent<SceneTasksMono>();
			this._clsTasks.Init();
			this._iMode = (this._iModeReq = StrategyRebellionTaskManagerMode.StrategyRebellionTaskManager_BEF);
			Observable.FromCoroutine<bool>((IObserver<bool> observer) => this.CreateRequireInstante(observer)).Subscribe(delegate(bool x)
			{
				if (x)
				{
					this._iMode = (this._iModeReq = StrategyRebellionTaskManagerMode.StrategyRebellionTaskManager_ST);
				}
			}).AddTo(base.get_gameObject());
			return true;
		}

		protected override bool UnInit()
		{
			this._clsTasks.UnInit();
			Mem.Del<SceneTasksMono>(ref this._clsTasks);
			Mem.Del<RebellionManager>(ref this._clsRebellionManager);
			return true;
		}

		protected override bool Run()
		{
			this._clsInput.Update();
			this._clsTasks.Run();
			this.UpdateMode();
			return StrategyTaskManager.GetMode() == StrategyTaskManager.StrategyTaskManagerMode.StrategyTaskManagerMode_BEF || StrategyTaskManager.GetMode() == StrategyTaskManager.StrategyTaskManagerMode.Rebellion;
		}

		public StrategyRebellionTaskManagerMode GetMode()
		{
			return this._iModeReq;
		}

		public void ReqMode(StrategyRebellionTaskManagerMode iMode)
		{
			this._iModeReq = iMode;
		}

		protected void UpdateMode()
		{
			if (this._iModeReq == StrategyRebellionTaskManagerMode.StrategyRebellionTaskManager_BEF)
			{
				return;
			}
			StrategyRebellionTaskManagerMode iModeReq = this._iModeReq;
			if (iModeReq != StrategyRebellionTaskManagerMode.StrategyRebellionTaskManager_ST)
			{
				if (iModeReq == StrategyRebellionTaskManagerMode.Organize)
				{
					if (this._clsTasks.Open(this._clsTaskOrgnaize) < 0)
					{
						return;
					}
				}
			}
			else if (this._clsTasks.Open(this._clsTaskEvent) < 0)
			{
				return;
			}
			this._iMode = this._iModeReq;
			this._iModeReq = StrategyRebellionTaskManagerMode.StrategyRebellionTaskManager_BEF;
		}

		public RebellionManager GetRebellionManager()
		{
			return this._clsRebellionManager;
		}

		[DebuggerHidden]
		private IEnumerator CreateRequireInstante(IObserver<bool> observer)
		{
			StrategyRebellionTaskManager.<CreateRequireInstante>c__Iterator162 <CreateRequireInstante>c__Iterator = new StrategyRebellionTaskManager.<CreateRequireInstante>c__Iterator162();
			<CreateRequireInstante>c__Iterator.observer = observer;
			<CreateRequireInstante>c__Iterator.<$>observer = observer;
			return <CreateRequireInstante>c__Iterator;
		}

		public static void checkRebellionArea()
		{
			List<MapAreaModel> rebellionAreaList = StrategyTopTaskManager.GetLogicManager().GetRebellionAreaList();
			StrategyRebellionTaskManager.RebellionArea = rebellionAreaList.get_Item(0).Id;
			List<int> neighboringAreaIDs = rebellionAreaList.get_Item(0).NeighboringAreaIDs;
			int num = neighboringAreaIDs.FindIndex((int x) => !StrategyTopTaskManager.GetLogicManager().Area.get_Item(x).IsOpen());
			StrategyRebellionTaskManager.RebellionFromArea = ((num != -1) ? rebellionAreaList.get_Item(0).NeighboringAreaIDs.get_Item(num) : -1);
			DebugUtils.Log(string.Concat(new object[]
			{
				"反攻発生: 発生エリア",
				StrategyRebellionTaskManager.RebellionArea,
				" 矢印エリア",
				StrategyRebellionTaskManager.RebellionFromArea
			}));
		}

		public void Termination()
		{
			base.ImmediateTermination();
		}
	}
}
