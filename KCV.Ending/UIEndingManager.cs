using local.managers;
using local.models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Ending
{
	public class UIEndingManager : MonoBehaviour
	{
		[SerializeField]
		private EndingShipView ShipView;

		[SerializeField]
		private EndingStaffRoll StaffRoll;

		private PSVitaMovie movie;

		private bool isStartEnd;

		private bool isMovieEnd;

		private Vector3 ShipViewDefaultPos;

		private Vector3 StaffRollDefaultPos;

		private bool isMovedPosition;

		public float ChangeCount;

		public float EndingTime;

		public float ChangeTime;

		public float Totaltime;

		private List<ShipModel> ShipModels;

		private bool isSideChanging;

		private bool isChangeTime;

		private bool isChangeWaiting;

		private int shipIndex;

		private Coroutine UpdateCharacterCor;

		private Coroutine UpdateSideChangeCor;

		private KeyControl key;

		[SerializeField]
		private UILabel foreverLabel;

		[Button("Play", "DebugPlay", new object[]
		{

		})]
		public int Button1;

		[SerializeField]
		private UIPanel MaskPanel;

		private bool isChangeNo;

		[Button("DebugSideChange", "DebugSideChange", new object[]
		{

		})]
		public int button1;

		private Coroutine DebugSideChangeCor;

		private bool isShipChanging
		{
			get
			{
				return this.ShipView.isShipChanging;
			}
		}

		private bool isVoice
		{
			get
			{
				return this.ShipView.isVoicePlaying;
			}
		}

		[DebuggerHidden]
		private IEnumerator Start()
		{
			UIEndingManager.<Start>c__Iterator5A <Start>c__Iterator5A = new UIEndingManager.<Start>c__Iterator5A();
			<Start>c__Iterator5A.<>f__this = this;
			return <Start>c__Iterator5A;
		}

		private void Play()
		{
			SingletonMonoBehaviour<SoundManager>.Instance.PlayBGM(BGMFileInfos.Ending);
			this.StaffRoll.StartStaffRoll();
			this.UpdateCharacterCor = base.StartCoroutine(this.UpdateShipCharacter());
			this.UpdateSideChangeCor = base.StartCoroutine(this.UpdateSideChange());
			this.StaffRoll.set_enabled(true);
		}

		private void Update()
		{
			if (!this.isStartEnd)
			{
				return;
			}
			this.key.Update();
			if (this.key.keyState.get_Item(1).press && !this.StaffRoll.isFinishRoll)
			{
				Time.set_timeScale(10f);
			}
			else
			{
				Time.set_timeScale(1f);
			}
		}

		[DebuggerHidden]
		private IEnumerator PlayMovie(EndingManager manager)
		{
			UIEndingManager.<PlayMovie>c__Iterator5B <PlayMovie>c__Iterator5B = new UIEndingManager.<PlayMovie>c__Iterator5B();
			<PlayMovie>c__Iterator5B.manager = manager;
			<PlayMovie>c__Iterator5B.<$>manager = manager;
			<PlayMovie>c__Iterator5B.<>f__this = this;
			return <PlayMovie>c__Iterator5B;
		}

		[DebuggerHidden]
		private IEnumerator UpdateShipCharacter()
		{
			UIEndingManager.<UpdateShipCharacter>c__Iterator5C <UpdateShipCharacter>c__Iterator5C = new UIEndingManager.<UpdateShipCharacter>c__Iterator5C();
			<UpdateShipCharacter>c__Iterator5C.<>f__this = this;
			return <UpdateShipCharacter>c__Iterator5C;
		}

		[DebuggerHidden]
		private IEnumerator UpdateSideChange()
		{
			UIEndingManager.<UpdateSideChange>c__Iterator5D <UpdateSideChange>c__Iterator5D = new UIEndingManager.<UpdateSideChange>c__Iterator5D();
			<UpdateSideChange>c__Iterator5D.<>f__this = this;
			return <UpdateSideChange>c__Iterator5D;
		}

		[DebuggerHidden]
		public IEnumerator SideChange()
		{
			UIEndingManager.<SideChange>c__Iterator5E <SideChange>c__Iterator5E = new UIEndingManager.<SideChange>c__Iterator5E();
			<SideChange>c__Iterator5E.<>f__this = this;
			return <SideChange>c__Iterator5E;
		}

		private void UpdateHandler(float value)
		{
			this.ShipView.get_transform().localPositionX(this.ShipViewDefaultPos.x + value * 0.85f);
			this.StaffRoll.get_transform().localPositionX(this.StaffRollDefaultPos.x - value * 0.8f);
		}

		private void OnSideChangeFinish()
		{
			this.isSideChanging = false;
		}

		private void OnDestroy()
		{
			if (this.UpdateCharacterCor != null)
			{
				base.StopCoroutine(this.UpdateCharacterCor);
				this.UpdateCharacterCor = null;
			}
			if (this.UpdateSideChangeCor != null)
			{
				base.StopCoroutine(this.UpdateSideChangeCor);
				this.UpdateSideChangeCor = null;
			}
			this.ShipView = null;
			this.StaffRoll = null;
			if (this.ShipModels != null)
			{
				this.ShipModels.Clear();
			}
			this.UpdateCharacterCor = null;
			this.UpdateSideChangeCor = null;
			this.key = null;
			this.foreverLabel = null;
			if (SingletonMonoBehaviour<Live2DModel>.exist())
			{
				SingletonMonoBehaviour<Live2DModel>.Instance.isDontRelease = false;
			}
		}

		private void DebugSideChange()
		{
			if (this.DebugSideChangeCor == null)
			{
				this.DebugSideChangeCor = base.StartCoroutine(this.SideChange());
			}
		}

		private void GotoInheritScene()
		{
			SingletonMonoBehaviour<Live2DModel>.Instance.Disable();
			Application.LoadLevel(Generics.Scene.InheritSave.ToString());
		}
	}
}
