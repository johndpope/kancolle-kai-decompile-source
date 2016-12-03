using Common.Enum;
using Common.SaveManager;
using KCV.Loading;
using KCV.Title;
using local.managers;
using ModeProc;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Inherit
{
	public class TaskInheritLoadSelect : SceneTaskMono
	{
		public enum Mode
		{
			DoLoadSelect,
			DifficultySelect
		}

		private VitaSaveManager saveManager;

		private ModeProcessor ModeProc;

		[SerializeField]
		private UILabel Message;

		private KeyControl key;

		[SerializeField]
		private GameObject DifficultySelectPrefab;

		private CtrlDifficultySelect _ctrlDifficultySelect;

		private bool isInherit;

		private UILoadingShip _LoadingShip;

		private DifficultKind diffculty;

		protected override void Start()
		{
			if (App.GetTitleManager() == null)
			{
				App.SetTitleManager(new TitleManager());
			}
			this.Message.alpha = 0f;
			this.isInherit = true;
			this.ModeProc = base.GetComponent<ModeProcessor>();
			this.ModeProc.addMode(TaskInheritLoadSelect.Mode.DoLoadSelect.ToString(), new ModeProc.Mode.ModeRun(this.DoLoadSelectRun), new ModeProc.Mode.ModeChange(this.DoLoadSelectEnter), new ModeProc.Mode.ModeChange(this.DoLoadSelectExit));
			this.ModeProc.addMode(TaskInheritLoadSelect.Mode.DifficultySelect.ToString(), new ModeProc.Mode.ModeRun(this.DifficultySelectRun), new ModeProc.Mode.ModeChange(this.DifficultySelectEnter), new ModeProc.Mode.ModeChange(this.DifficultySelectExit));
		}

		protected override bool Init()
		{
			this.key = new KeyControl(0, 0, 0.4f, 0.1f);
			this.ModeProc.FirstModeEnter();
			this.Message.alpha = 0f;
			return true;
		}

		protected override bool UnInit()
		{
			if (this._ctrlDifficultySelect != null)
			{
				Object.Destroy(this._ctrlDifficultySelect.get_gameObject());
			}
			Mem.Del<CtrlDifficultySelect>(ref this._ctrlDifficultySelect);
			return true;
		}

		protected override bool Run()
		{
			this.key.Update();
			this.ModeProc.ModeRun();
			return true;
		}

		private void DoLoadSelectRun()
		{
		}

		[DebuggerHidden]
		private IEnumerator DoLoadSelectEnter()
		{
			TaskInheritLoadSelect.<DoLoadSelectEnter>c__Iterator62 <DoLoadSelectEnter>c__Iterator = new TaskInheritLoadSelect.<DoLoadSelectEnter>c__Iterator62();
			<DoLoadSelectEnter>c__Iterator.<>f__this = this;
			return <DoLoadSelectEnter>c__Iterator;
		}

		[DebuggerHidden]
		private IEnumerator DoLoadSelectExit()
		{
			TaskInheritLoadSelect.<DoLoadSelectExit>c__Iterator63 <DoLoadSelectExit>c__Iterator = new TaskInheritLoadSelect.<DoLoadSelectExit>c__Iterator63();
			<DoLoadSelectExit>c__Iterator.<>f__this = this;
			return <DoLoadSelectExit>c__Iterator;
		}

		public void OnYesDesideLoadSelect()
		{
			this.ModeProc.ChangeMode(1);
		}

		public void OnNoDesideLoadSelect()
		{
			this.ModeProc.ChangeMode(1);
		}

		private void DifficultySelectRun()
		{
			this._ctrlDifficultySelect.Run();
		}

		[DebuggerHidden]
		private IEnumerator DifficultySelectEnter()
		{
			TaskInheritLoadSelect.<DifficultySelectEnter>c__Iterator64 <DifficultySelectEnter>c__Iterator = new TaskInheritLoadSelect.<DifficultySelectEnter>c__Iterator64();
			<DifficultySelectEnter>c__Iterator.<>f__this = this;
			return <DifficultySelectEnter>c__Iterator;
		}

		[DebuggerHidden]
		private IEnumerator DifficultySelectExit()
		{
			TaskInheritLoadSelect.<DifficultySelectExit>c__Iterator65 <DifficultySelectExit>c__Iterator = new TaskInheritLoadSelect.<DifficultySelectExit>c__Iterator65();
			<DifficultySelectExit>c__Iterator.<>f__this = this;
			return <DifficultySelectExit>c__Iterator;
		}

		public void OnDecideDifficulty(DifficultKind iKind)
		{
			this.diffculty = iKind;
			base.StartCoroutine(this.GotoNextScene());
		}

		public void OnCancel()
		{
			this.ModeProc.ChangeMode(0);
			Object.Destroy(this._ctrlDifficultySelect.get_gameObject());
		}

		[DebuggerHidden]
		private IEnumerator GotoNextScene()
		{
			TaskInheritLoadSelect.<GotoNextScene>c__Iterator66 <GotoNextScene>c__Iterator = new TaskInheritLoadSelect.<GotoNextScene>c__Iterator66();
			<GotoNextScene>c__Iterator.<>f__this = this;
			return <GotoNextScene>c__Iterator;
		}

		private void OnDestroy()
		{
			this._ctrlDifficultySelect = null;
		}
	}
}
