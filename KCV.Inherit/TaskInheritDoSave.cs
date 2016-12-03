using Common.SaveManager;
using System;
using UnityEngine;

namespace KCV.Inherit
{
	public class TaskInheritDoSave : SceneTaskMono, ISaveDataOperator
	{
		private VitaSaveManager vitaSaveManager;

		public Action OnSavedCallBack;

		[SerializeField]
		private UILabel Message;

		protected override void Start()
		{
			this.vitaSaveManager = VitaSaveManager.Instance;
		}

		protected override bool Run()
		{
			return true;
		}

		protected override bool Init()
		{
			this.vitaSaveManager.Open(this);
			Debug.Log("OpenSaveManager");
			this.vitaSaveManager.Save();
			return true;
		}

		public void SaveManOpen()
		{
			Debug.Log("SaveManOpen");
		}

		public void SaveManClose()
		{
			Debug.Log("SaveManClose");
		}

		public void Canceled()
		{
			Debug.Log("Save Cancel");
			this.vitaSaveManager.Close();
			InheritSaveTaskManager.ReqMode(InheritSaveTaskManager.InheritTaskManagerMode.InheritTaskManagerMode_ST);
			base.Close();
		}

		public void SaveError()
		{
			Debug.Log("SaveError");
		}

		public void SaveComplete()
		{
			Debug.Log("OnSaved");
			this.OnSavedCallBack.Invoke();
			this.vitaSaveManager.Save();
		}

		public void LoadError()
		{
		}

		public void LoadComplete()
		{
		}

		public void LoadNothing()
		{
		}

		public void DeleteComplete()
		{
		}
	}
}
