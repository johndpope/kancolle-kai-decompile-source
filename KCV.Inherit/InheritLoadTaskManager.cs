using System;
using UnityEngine;

namespace KCV.Inherit
{
	public class InheritLoadTaskManager : MonoBehaviour
	{
		public enum InheritTaskManagerMode
		{
			InheritTaskManagerMode_ST,
			InheritTaskManagerMode_BEF = -1,
			InheritTaskManagerMode_NONE = -1,
			LoadSelect,
			Privilege,
			InheritTaskManagerMode_AFT,
			InheritTaskManagerMode_NUM = 2,
			InheritTaskManagerMode_ED = 1
		}

		private static SceneTasksMono _clsTasks;

		private static InheritLoadTaskManager.InheritTaskManagerMode _iMode;

		private static InheritLoadTaskManager.InheritTaskManagerMode _iModeReq;

		[SerializeField]
		private TaskInheritLoadSelect _clsLoadSelect;

		[SerializeField]
		private TaskInheritPrivilege _clsPrivilege;

		private void Awake()
		{
			InheritLoadTaskManager._clsTasks = base.get_gameObject().SafeGetComponent<SceneTasksMono>();
		}

		private void Start()
		{
			InheritLoadTaskManager._iMode = (InheritLoadTaskManager._iModeReq = InheritLoadTaskManager.InheritTaskManagerMode.Privilege);
			InheritLoadTaskManager._clsTasks.Init();
		}

		private void OnDestroy()
		{
			InheritLoadTaskManager._clsTasks.UnInit();
			this._clsLoadSelect = null;
			this._clsPrivilege = null;
		}

		private void Update()
		{
			InheritLoadTaskManager._clsTasks.Run();
			this.UpdateMode();
		}

		public static InheritLoadTaskManager.InheritTaskManagerMode GetMode()
		{
			return InheritLoadTaskManager._iModeReq;
		}

		public static void ReqMode(InheritLoadTaskManager.InheritTaskManagerMode iMode)
		{
			InheritLoadTaskManager._iModeReq = iMode;
		}

		protected void UpdateMode()
		{
			if (InheritLoadTaskManager._iModeReq == InheritLoadTaskManager.InheritTaskManagerMode.InheritTaskManagerMode_BEF)
			{
				return;
			}
			InheritLoadTaskManager.InheritTaskManagerMode iModeReq = InheritLoadTaskManager._iModeReq;
			if (iModeReq != InheritLoadTaskManager.InheritTaskManagerMode.InheritTaskManagerMode_ST)
			{
				if (iModeReq == InheritLoadTaskManager.InheritTaskManagerMode.Privilege)
				{
					if (InheritLoadTaskManager._clsTasks.Open(this._clsPrivilege) < 0)
					{
						return;
					}
				}
			}
			else if (InheritLoadTaskManager._clsTasks.Open(this._clsLoadSelect) < 0)
			{
				return;
			}
			InheritLoadTaskManager._iMode = InheritLoadTaskManager._iModeReq;
			InheritLoadTaskManager._iModeReq = InheritLoadTaskManager.InheritTaskManagerMode.InheritTaskManagerMode_BEF;
		}
	}
}
