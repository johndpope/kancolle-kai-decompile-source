using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace ModeProc
{
	public class ModeProcessor : MonoBehaviour
	{
		private List<Mode> Modes;

		private Mode nowMode;

		private Mode prevMode;

		private Mode firstMode;

		private Coroutine ChangeModeInstance;

		public bool isRun
		{
			get;
			private set;
		}

		private void Awake()
		{
			this.Modes = new List<Mode>();
			this.isRun = false;
		}

		public void ModeRun()
		{
			if (this.isRun)
			{
				this.nowMode.Run();
			}
		}

		public void addMode(string ModeName, Mode.ModeRun Run, Mode.ModeChange Enter, Mode.ModeChange Exit)
		{
			Mode mode = new Mode(Run, Enter, Exit);
			mode.ModeNo = this.Modes.get_Count();
			mode.Name = ModeName;
			this.Modes.Add(mode);
			if (this.nowMode == null)
			{
				this.nowMode = this.Modes.get_Item(0);
				this.firstMode = this.Modes.get_Item(0);
				base.StartCoroutine(this.Initialize());
			}
		}

		public void ChangeMode(int modeNo)
		{
			if (this.ChangeModeInstance != null)
			{
				base.StopCoroutine(this.ChangeModeInstance);
			}
			this.ChangeModeInstance = base.StartCoroutine(this.ChangeModeCor(modeNo));
		}

		public void FirstModeEnter()
		{
			base.StartCoroutine(this.FirstModeEnterCor(this.firstMode.ModeNo));
		}

		[DebuggerHidden]
		private IEnumerator Initialize()
		{
			ModeProcessor.<Initialize>c__Iterator24 <Initialize>c__Iterator = new ModeProcessor.<Initialize>c__Iterator24();
			<Initialize>c__Iterator.<>f__this = this;
			return <Initialize>c__Iterator;
		}

		[DebuggerHidden]
		private IEnumerator ChangeModeCor(int modeNo)
		{
			ModeProcessor.<ChangeModeCor>c__Iterator25 <ChangeModeCor>c__Iterator = new ModeProcessor.<ChangeModeCor>c__Iterator25();
			<ChangeModeCor>c__Iterator.modeNo = modeNo;
			<ChangeModeCor>c__Iterator.<$>modeNo = modeNo;
			<ChangeModeCor>c__Iterator.<>f__this = this;
			return <ChangeModeCor>c__Iterator;
		}

		[DebuggerHidden]
		private IEnumerator FirstModeEnterCor(int modeNo)
		{
			ModeProcessor.<FirstModeEnterCor>c__Iterator26 <FirstModeEnterCor>c__Iterator = new ModeProcessor.<FirstModeEnterCor>c__Iterator26();
			<FirstModeEnterCor>c__Iterator.<>f__this = this;
			return <FirstModeEnterCor>c__Iterator;
		}

		public bool isNowMode(int No)
		{
			return this.nowMode.ModeNo == No;
		}

		public bool isPrevMode(int No)
		{
			return this.prevMode.ModeNo == No;
		}

		private void OnDestroy()
		{
			this.Modes.Clear();
			this.Modes = null;
			this.nowMode = null;
			this.prevMode = null;
			this.firstMode = null;
			this.ChangeModeInstance = null;
		}
	}
}
