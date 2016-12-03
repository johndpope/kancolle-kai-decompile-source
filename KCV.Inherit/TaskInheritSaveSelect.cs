using local.managers;
using ModeProc;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Inherit
{
	public class TaskInheritSaveSelect : SceneTaskMono
	{
		public enum Mode
		{
			AdmiralRankJudge,
			DoSaveSelect,
			InheritTypeSelect,
			Confirm
		}

		private ModeProcessor ModeProc;

		[SerializeField]
		private UIWidget SaveSelect;

		[SerializeField]
		private UIWidget TypeSelect;

		[SerializeField]
		private UIWidget ConfirmSelect;

		[SerializeField]
		private UIButtonManager SaveSelectBtnMng;

		[SerializeField]
		private UIButtonManager ConfirmSelectBtnMng;

		[SerializeField]
		private UIButtonManager TypeSelectBtnMng;

		private KeyControl key;

		[SerializeField]
		private UITexture ShipTexture;

		[SerializeField]
		private UILabel ShipNum;

		private EndingManager manager;

		public bool isSaved;

		[SerializeField]
		private AdmiralRankJudge rankJudge;

		private bool JudgeFinished;

		protected override void Start()
		{
			SingletonMonoBehaviour<SoundManager>.Instance.StopBGM();
			this.manager = new EndingManager();
			this.SaveSelect.alpha = 0f;
			this.ConfirmSelect.alpha = 0f;
			this.ModeProc = base.GetComponent<ModeProcessor>();
			this.ModeProc.addMode(TaskInheritSaveSelect.Mode.AdmiralRankJudge.ToString(), new ModeProc.Mode.ModeRun(this.AdmiralRankJudgeRun), new ModeProc.Mode.ModeChange(this.AdmiralRankJudgeEnter), new ModeProc.Mode.ModeChange(this.AdmiralRankJudgeExit));
			this.ModeProc.addMode(TaskInheritSaveSelect.Mode.DoSaveSelect.ToString(), new ModeProc.Mode.ModeRun(this.DoSaveSelectRun), new ModeProc.Mode.ModeChange(this.DoSaveSelectEnter), new ModeProc.Mode.ModeChange(this.DoSaveSelectExit));
			this.ModeProc.addMode(TaskInheritSaveSelect.Mode.InheritTypeSelect.ToString(), new ModeProc.Mode.ModeRun(this.InheritTypeSelectRun), new ModeProc.Mode.ModeChange(this.InheritTypeSelectEnter), new ModeProc.Mode.ModeChange(this.InheritTypeSelectExit));
			this.ModeProc.addMode(TaskInheritSaveSelect.Mode.Confirm.ToString(), new ModeProc.Mode.ModeRun(this.ConfirmRun), new ModeProc.Mode.ModeChange(this.ConfirmEnter), new ModeProc.Mode.ModeChange(this.ConfirmExit));
			this.ShipNum.textInt = this.manager.GetTakeoverShipCountMax();
		}

		protected override bool Init()
		{
			Debug.Log("SaveSelect Init");
			this.key = new KeyControl(0, 0, 0.4f, 0.1f);
			this.SaveSelectBtnMng.setFocus(0);
			this.ConfirmSelectBtnMng.setFocus(0);
			this.SaveSelect.alpha = 0f;
			this.ConfirmSelect.alpha = 0f;
			this.ModeProc.FirstModeEnter();
			return true;
		}

		protected override bool Run()
		{
			this.key.Update();
			this.ModeProc.ModeRun();
			return true;
		}

		private void OnDestroy()
		{
			this.ModeProc = null;
			this.SaveSelect = null;
			this.TypeSelect = null;
			this.ConfirmSelect = null;
			this.SaveSelectBtnMng = null;
			this.ConfirmSelectBtnMng = null;
			this.TypeSelectBtnMng = null;
			this.key = null;
			this.ShipTexture = null;
			this.ShipNum = null;
			this.manager = null;
		}

		private void AdmiralRankJudgeRun()
		{
			if (this.key.IsMaruDown() && this.JudgeFinished)
			{
				if (this.rankJudge != null)
				{
					this.rankJudge.StopParticle();
				}
				this.ModeProc.ChangeMode(1);
			}
		}

		[DebuggerHidden]
		private IEnumerator AdmiralRankJudgeEnter()
		{
			TaskInheritSaveSelect.<AdmiralRankJudgeEnter>c__Iterator69 <AdmiralRankJudgeEnter>c__Iterator = new TaskInheritSaveSelect.<AdmiralRankJudgeEnter>c__Iterator69();
			<AdmiralRankJudgeEnter>c__Iterator.<>f__this = this;
			return <AdmiralRankJudgeEnter>c__Iterator;
		}

		[DebuggerHidden]
		private IEnumerator AdmiralRankJudgeExit()
		{
			TaskInheritSaveSelect.<AdmiralRankJudgeExit>c__Iterator6A <AdmiralRankJudgeExit>c__Iterator6A = new TaskInheritSaveSelect.<AdmiralRankJudgeExit>c__Iterator6A();
			<AdmiralRankJudgeExit>c__Iterator6A.<>f__this = this;
			return <AdmiralRankJudgeExit>c__Iterator6A;
		}

		private void DoSaveSelectRun()
		{
			if (this.key.IsRightDown())
			{
				this.SaveSelectBtnMng.moveNextButton();
			}
			else if (this.key.IsLeftDown())
			{
				this.SaveSelectBtnMng.movePrevButton();
			}
			else if (this.key.IsMaruDown())
			{
				this.SaveSelectBtnMng.Decide();
			}
		}

		[DebuggerHidden]
		private IEnumerator DoSaveSelectEnter()
		{
			TaskInheritSaveSelect.<DoSaveSelectEnter>c__Iterator6B <DoSaveSelectEnter>c__Iterator6B = new TaskInheritSaveSelect.<DoSaveSelectEnter>c__Iterator6B();
			<DoSaveSelectEnter>c__Iterator6B.<>f__this = this;
			return <DoSaveSelectEnter>c__Iterator6B;
		}

		[DebuggerHidden]
		private IEnumerator DoSaveSelectExit()
		{
			TaskInheritSaveSelect.<DoSaveSelectExit>c__Iterator6C <DoSaveSelectExit>c__Iterator6C = new TaskInheritSaveSelect.<DoSaveSelectExit>c__Iterator6C();
			<DoSaveSelectExit>c__Iterator6C.<>f__this = this;
			return <DoSaveSelectExit>c__Iterator6C;
		}

		public void OnYesDesideSaveSelect()
		{
			this.ModeProc.ChangeMode(2);
		}

		public void OnNoDesideSaveSelect()
		{
			if (this.isSaved)
			{
				base.StartCoroutine(this.GotoTitle());
			}
			else
			{
				this.ModeProc.ChangeMode(3);
			}
		}

		private void InheritTypeSelectRun()
		{
			if (this.key.IsUpDown())
			{
				this.TypeSelectBtnMng.movePrevButton();
			}
			else if (this.key.IsDownDown())
			{
				this.TypeSelectBtnMng.moveNextButton();
			}
			else if (this.key.IsMaruDown())
			{
				this.TypeSelectBtnMng.Decide();
			}
			else if (this.key.IsBatuDown())
			{
				this.ModeProc.ChangeMode(1);
			}
		}

		[DebuggerHidden]
		private IEnumerator InheritTypeSelectEnter()
		{
			TaskInheritSaveSelect.<InheritTypeSelectEnter>c__Iterator6D <InheritTypeSelectEnter>c__Iterator6D = new TaskInheritSaveSelect.<InheritTypeSelectEnter>c__Iterator6D();
			<InheritTypeSelectEnter>c__Iterator6D.<>f__this = this;
			return <InheritTypeSelectEnter>c__Iterator6D;
		}

		[DebuggerHidden]
		private IEnumerator InheritTypeSelectExit()
		{
			TaskInheritSaveSelect.<InheritTypeSelectExit>c__Iterator6E <InheritTypeSelectExit>c__Iterator6E = new TaskInheritSaveSelect.<InheritTypeSelectExit>c__Iterator6E();
			<InheritTypeSelectExit>c__Iterator6E.<>f__this = this;
			return <InheritTypeSelectExit>c__Iterator6E;
		}

		public void OnDesideYuusenButton()
		{
			this.manager.CreatePlusData(false);
			base.StartCoroutine(this.ReqDoSaveMode());
		}

		public void OnDesideNoYusenButton()
		{
			this.manager.CreatePlusData(true);
			base.StartCoroutine(this.ReqDoSaveMode());
		}

		[DebuggerHidden]
		private IEnumerator ReqDoSaveMode()
		{
			TaskInheritSaveSelect.<ReqDoSaveMode>c__Iterator6F <ReqDoSaveMode>c__Iterator6F = new TaskInheritSaveSelect.<ReqDoSaveMode>c__Iterator6F();
			<ReqDoSaveMode>c__Iterator6F.<>f__this = this;
			return <ReqDoSaveMode>c__Iterator6F;
		}

		private void ConfirmRun()
		{
			if (this.key.IsRightDown())
			{
				this.ConfirmSelectBtnMng.moveNextButton();
			}
			else if (this.key.IsLeftDown())
			{
				this.ConfirmSelectBtnMng.movePrevButton();
			}
			else if (this.key.IsMaruDown())
			{
				this.ConfirmSelectBtnMng.Decide();
			}
		}

		[DebuggerHidden]
		private IEnumerator ConfirmEnter()
		{
			TaskInheritSaveSelect.<ConfirmEnter>c__Iterator70 <ConfirmEnter>c__Iterator = new TaskInheritSaveSelect.<ConfirmEnter>c__Iterator70();
			<ConfirmEnter>c__Iterator.<>f__this = this;
			return <ConfirmEnter>c__Iterator;
		}

		[DebuggerHidden]
		private IEnumerator ConfirmExit()
		{
			TaskInheritSaveSelect.<ConfirmExit>c__Iterator71 <ConfirmExit>c__Iterator = new TaskInheritSaveSelect.<ConfirmExit>c__Iterator71();
			<ConfirmExit>c__Iterator.<>f__this = this;
			return <ConfirmExit>c__Iterator;
		}

		public void OnYesDesideConfirm()
		{
			base.StartCoroutine(this.GotoTitle());
			base.Close();
		}

		public void OnNoDesideConfirm()
		{
			this.ModeProc.ChangeMode(1);
		}

		[DebuggerHidden]
		private IEnumerator GotoTitle()
		{
			TaskInheritSaveSelect.<GotoTitle>c__Iterator72 <GotoTitle>c__Iterator = new TaskInheritSaveSelect.<GotoTitle>c__Iterator72();
			<GotoTitle>c__Iterator.<>f__this = this;
			return <GotoTitle>c__Iterator;
		}

		protected override bool UnInit()
		{
			return true;
		}
	}
}
