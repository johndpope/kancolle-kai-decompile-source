using System;
using UnityEngine;

namespace KCV.Inherit
{
	public class InheritSaveTaskManager : MonoBehaviour
	{
		public enum InheritTaskManagerMode
		{
			InheritTaskManagerMode_ST,
			InheritTaskManagerMode_BEF = -1,
			InheritTaskManagerMode_NONE = -1,
			SaveSelect,
			DoSave,
			InheritTaskManagerMode_AFT,
			InheritTaskManagerMode_NUM = 2,
			InheritTaskManagerMode_ED = 1
		}

		private static SceneTasksMono _clsTasks;

		private static InheritSaveTaskManager.InheritTaskManagerMode _iMode;

		private static InheritSaveTaskManager.InheritTaskManagerMode _iModeReq;

		[SerializeField]
		private TaskInheritSaveSelect _clsSaveSelect;

		[SerializeField]
		private TaskInheritDoSave _clsDoSave;

		public bool isSaved;

		private void OnSaved()
		{
			this._clsSaveSelect.isSaved = true;
		}

		private void Awake()
		{
			InheritSaveTaskManager._clsTasks = base.get_gameObject().SafeGetComponent<SceneTasksMono>();
			this._clsDoSave.OnSavedCallBack = new Action(this.OnSaved);
		}

		private void Start()
		{
			InheritSaveTaskManager._iMode = (InheritSaveTaskManager._iModeReq = InheritSaveTaskManager.InheritTaskManagerMode.InheritTaskManagerMode_ST);
			InheritSaveTaskManager._clsTasks.Init();
		}

		private void OnDestroy()
		{
			InheritSaveTaskManager._clsTasks.UnInit();
			this._clsSaveSelect = null;
			this._clsDoSave = null;
		}

		private void Update()
		{
			InheritSaveTaskManager._clsTasks.Run();
			this.UpdateMode();
		}

		public static InheritSaveTaskManager.InheritTaskManagerMode GetMode()
		{
			return InheritSaveTaskManager._iModeReq;
		}

		public static void ReqMode(InheritSaveTaskManager.InheritTaskManagerMode iMode)
		{
			InheritSaveTaskManager._iModeReq = iMode;
		}

		protected void UpdateMode()
		{
			if (InheritSaveTaskManager._iModeReq == InheritSaveTaskManager.InheritTaskManagerMode.InheritTaskManagerMode_BEF)
			{
				return;
			}
			InheritSaveTaskManager.InheritTaskManagerMode iModeReq = InheritSaveTaskManager._iModeReq;
			if (iModeReq != InheritSaveTaskManager.InheritTaskManagerMode.InheritTaskManagerMode_ST)
			{
				if (iModeReq == InheritSaveTaskManager.InheritTaskManagerMode.DoSave)
				{
					if (InheritSaveTaskManager._clsTasks.Open(this._clsDoSave) < 0)
					{
						return;
					}
				}
			}
			else if (InheritSaveTaskManager._clsTasks.Open(this._clsSaveSelect) < 0)
			{
				return;
			}
			InheritSaveTaskManager._iMode = InheritSaveTaskManager._iModeReq;
			InheritSaveTaskManager._iModeReq = InheritSaveTaskManager.InheritTaskManagerMode.InheritTaskManagerMode_BEF;
		}
	}
}
