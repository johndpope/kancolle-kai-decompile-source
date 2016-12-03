using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Tutorial.Guide
{
	public class TutorialDialog : MonoBehaviour
	{
		private DialogAnimation dialogAnimation;

		public Transform View;

		private UIPanel panel;

		private KeyControl key;

		public Action OnClosed;

		private Action OnLoaded;

		[SerializeField]
		private UIWidget[] page;

		private int nowPage;

		[SerializeField]
		private Blur Blur;

		private bool isClosed;

		private TutorialGuideManager.TutorialID tutorialId;

		private AudioSource playingTutorialVoiceAudioClip;

		public void SetOnLoaded(Action onloaded)
		{
			this.OnLoaded = onloaded;
		}

		public bool getIsClosed()
		{
			return this.isClosed;
		}

		private void Awake()
		{
			this.dialogAnimation = base.get_transform().FindChild("Dialog").GetComponent<DialogAnimation>();
			this.panel = base.GetComponent<UIPanel>();
			this.nowPage = 0;
			if (this.Blur != null)
			{
				this.Blur.set_enabled(false);
			}
		}

		private void OnDestroy()
		{
			if (this.playingTutorialVoiceAudioClip != null)
			{
				this.playingTutorialVoiceAudioClip.Stop();
			}
			this.playingTutorialVoiceAudioClip = null;
			this.dialogAnimation = null;
			this.View = null;
			this.panel = null;
			this.key = null;
			this.OnClosed = null;
			this.OnLoaded = null;
			for (int i = 0; i < this.page.Length; i++)
			{
				this.page[i] = null;
			}
			Mem.DelAry<UIWidget>(ref this.page);
			this.Blur = null;
		}

		internal void SetTutorialId(TutorialGuideManager.TutorialID tutorialId)
		{
			this.tutorialId = tutorialId;
		}

		[DebuggerHidden]
		private IEnumerator Start()
		{
			TutorialDialog.<Start>c__Iterator40 <Start>c__Iterator = new TutorialDialog.<Start>c__Iterator40();
			<Start>c__Iterator.<>f__this = this;
			return <Start>c__Iterator;
		}

		private void Update()
		{
			if (this.key == null)
			{
				return;
			}
			this.key.Update();
			if (this.key.IsMaruDown() || this.key.IsBatuDown() || this.key.IsShikakuDown() || this.key.IsSankakuDown())
			{
				this.NextPage();
			}
		}

		public void Show(TutorialGuideManager.TutorialID tutorialId, Action OnFinished)
		{
			Time.set_timeScale(1f);
			this.dialogAnimation.OpenAction = delegate
			{
				if (OnFinished != null)
				{
					if (this.playingTutorialVoiceAudioClip != null)
					{
						this.playingTutorialVoiceAudioClip.Stop();
					}
					this.playingTutorialVoiceAudioClip = this.PlayTutorialVoice(tutorialId, 0);
					OnFinished.Invoke();
				}
			};
			this.dialogAnimation.fadeTime = 0.5f;
			this.dialogAnimation.StartAnim(DialogAnimation.AnimType.FEAD, true);
		}

		private AudioSource PlayTutorialVoice(TutorialGuideManager.TutorialID tutorialId, int pageIndex)
		{
			AudioClip audioClip = this.RequestTutorialVoice(tutorialId, pageIndex);
			if (audioClip != null)
			{
				return SingletonMonoBehaviour<SoundManager>.Instance.PlayVoice(audioClip);
			}
			return null;
		}

		private AudioClip RequestTutorialVoice(TutorialGuideManager.TutorialID tutorialId, int pageIndex)
		{
			int voiceNum = this.TutorialIdToVoiceId(tutorialId, pageIndex);
			return SingletonMonoBehaviour<ResourceManager>.Instance.ShipVoice.Load(0, voiceNum);
		}

		private int TutorialIdToVoiceId(TutorialGuideManager.TutorialID tutorialId, int pageIndex)
		{
			switch (tutorialId)
			{
			case TutorialGuideManager.TutorialID.StrategyText:
				return -1;
			case TutorialGuideManager.TutorialID.PortTopText:
				return -1;
			case TutorialGuideManager.TutorialID.BattleCommand:
				return -1;
			case TutorialGuideManager.TutorialID.RepairInfo:
				return 415;
			case TutorialGuideManager.TutorialID.SupplyInfo:
				return 416;
			case TutorialGuideManager.TutorialID.StrategyPoint:
				return 417;
			case TutorialGuideManager.TutorialID.BattleShortCutInfo:
				return 418;
			case TutorialGuideManager.TutorialID.Raider:
				return 421;
			case TutorialGuideManager.TutorialID.RebellionPreparation:
				return 422;
			case TutorialGuideManager.TutorialID.Rebellion_EnableIntercept:
				return 425;
			case TutorialGuideManager.TutorialID.Rebellion_DisableIntercept:
				return 423;
			case TutorialGuideManager.TutorialID.Rebellion_CombinedFleet:
				if (pageIndex == 0)
				{
					return 426;
				}
				if (pageIndex == 1)
				{
					return 427;
				}
				return -1;
			case TutorialGuideManager.TutorialID.Rebellion_Lose:
				return 424;
			case TutorialGuideManager.TutorialID.ResourceRecovery:
				if (pageIndex == 0)
				{
					return 428;
				}
				if (pageIndex == 1)
				{
					return 429;
				}
				return -1;
			case TutorialGuideManager.TutorialID.TankerDeploy:
				return 413;
			case TutorialGuideManager.TutorialID.EscortOrganize:
				return 414;
			case TutorialGuideManager.TutorialID.Bring:
				if (pageIndex == 0)
				{
					return 419;
				}
				if (pageIndex == 1)
				{
					return 420;
				}
				return -1;
			case TutorialGuideManager.TutorialID.BuildShip:
				return 404;
			case TutorialGuideManager.TutorialID.SpeedBuild:
				return 406;
			case TutorialGuideManager.TutorialID.Organize:
				return -1;
			case TutorialGuideManager.TutorialID.EndGame:
				return -1;
			default:
				return -1;
			}
		}

		public void Hide(Action OnFinished)
		{
			if (!this.key.IsRun)
			{
				return;
			}
			if (this.playingTutorialVoiceAudioClip != null)
			{
				this.playingTutorialVoiceAudioClip.Stop();
			}
			this.playingTutorialVoiceAudioClip = null;
			this.dialogAnimation.CloseAction = delegate
			{
				if (this.OnClosed != null)
				{
					this.OnClosed.Invoke();
				}
				this.isClosed = true;
				Object.Destroy(base.get_gameObject());
			};
			this.dialogAnimation.fadeTime = 0.5f;
			this.dialogAnimation.StartAnim(DialogAnimation.AnimType.FEAD, false);
			App.OnlyController = null;
			App.isFirstUpdate = true;
			this.key.IsRun = false;
		}

		public void NextPage()
		{
			if (this.page != null && this.nowPage < this.page.Length - 1)
			{
				this.key.IsRun = false;
				this.PageChange();
			}
			else
			{
				this.Hide(null);
			}
		}

		private void PageChange()
		{
			TweenAlpha tweenAlpha = TweenAlpha.Begin(this.page[this.nowPage].get_gameObject(), 0.5f, 0f);
			this.nowPage++;
			if (this.playingTutorialVoiceAudioClip != null && this.playingTutorialVoiceAudioClip.get_isPlaying())
			{
				this.playingTutorialVoiceAudioClip.Stop();
			}
			this.playingTutorialVoiceAudioClip = this.PlayTutorialVoice(this.tutorialId, this.nowPage);
			TweenAlpha.Begin(this.page[this.nowPage].get_gameObject(), 0.5f, 1f);
			this.DelayAction(0.5f, delegate
			{
				this.key.IsRun = true;
			});
		}

		[DebuggerHidden]
		public IEnumerator WaitForDialogClosed()
		{
			TutorialDialog.<WaitForDialogClosed>c__Iterator41 <WaitForDialogClosed>c__Iterator = new TutorialDialog.<WaitForDialogClosed>c__Iterator41();
			<WaitForDialogClosed>c__Iterator.<>f__this = this;
			return <WaitForDialogClosed>c__Iterator;
		}
	}
}
