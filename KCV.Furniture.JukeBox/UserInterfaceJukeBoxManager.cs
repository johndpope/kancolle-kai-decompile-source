using DG.Tweening;
using KCV.Utils;
using local.managers;
using Server_Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Furniture.JukeBox
{
	[RequireComponent(typeof(UIPanel))]
	public class UserInterfaceJukeBoxManager : MonoBehaviour
	{
		public enum State
		{
			NONE,
			MusicSelect,
			BuyMusicConfirm,
			Playing
		}

		public class StateManager<State>
		{
			private Stack<State> mStateStack;

			private State mEmptyState;

			public Action<State> OnPush
			{
				private get;
				set;
			}

			public Action<State> OnPop
			{
				private get;
				set;
			}

			public Action<State> OnResume
			{
				private get;
				set;
			}

			public Action<State> OnSwitch
			{
				private get;
				set;
			}

			public State CurrentState
			{
				get
				{
					if (0 < this.mStateStack.get_Count())
					{
						return this.mStateStack.Peek();
					}
					return this.mEmptyState;
				}
			}

			public StateManager(State emptyState)
			{
				this.mEmptyState = emptyState;
				this.mStateStack = new Stack<State>();
			}

			public void PushState(State state)
			{
				this.mStateStack.Push(state);
				this.Notify(this.OnPush, this.mStateStack.Peek());
				this.Notify(this.OnSwitch, this.mStateStack.Peek());
			}

			public void ReplaceState(State state)
			{
				if (0 < this.mStateStack.get_Count())
				{
					this.PopState();
				}
				this.mStateStack.Push(state);
				this.Notify(this.OnPush, this.mStateStack.Peek());
				this.Notify(this.OnSwitch, this.mStateStack.Peek());
			}

			public void PopState()
			{
				if (0 < this.mStateStack.get_Count())
				{
					State state = this.mStateStack.Pop();
					this.Notify(this.OnPop, state);
				}
			}

			public void ResumeState()
			{
				if (0 < this.mStateStack.get_Count())
				{
					this.Notify(this.OnResume, this.mStateStack.Peek());
					this.Notify(this.OnSwitch, this.mStateStack.Peek());
				}
			}

			public override string ToString()
			{
				this.mStateStack.ToArray();
				string text = string.Empty;
				using (Stack<State>.Enumerator enumerator = this.mStateStack.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						State current = enumerator.get_Current();
						text = current + " > " + text;
					}
				}
				return text;
			}

			private void Notify(Action<State> target, State state)
			{
				if (target != null)
				{
					target.Invoke(state);
				}
			}
		}

		public class Context
		{
			private Mst_bgm_jukebox mBgmJukeBox;

			public void SetJukeBoxBGM(Mst_bgm_jukebox jukeBoxBGM)
			{
				this.mBgmJukeBox = jukeBoxBGM;
			}

			public Mst_bgm_jukebox GetJukeBoxBGM()
			{
				return this.mBgmJukeBox;
			}
		}

		private BGMFileInfos mConfiguredBGM;

		[SerializeField]
		private UIJukeBoxMusicBuyConfirm mUIJukeBoxMusicBuyConfirm;

		[SerializeField]
		private UIJukeBoxPlayListParent mUIJukeBoxPlayListParent;

		[SerializeField]
		private UIJukeBoxMusicPlayingDialog mUIJukeBoxMusicPlayingDialog;

		private UserInterfaceJukeBoxManager.Context mContext;

		private UIPanel mPanelThis;

		private UserInterfaceJukeBoxManager.StateManager<UserInterfaceJukeBoxManager.State> mStateManager;

		private PortManager mPortManager;

		private KeyControl mKeyController;

		private IEnumerator mPlayBGMWithCrossFadeCoroutine;

		private float mDefaultVolume;

		private int mDeckId;

		private Camera mOverlayCamera;

		private Action mOnBackListener;

		private void Awake()
		{
			this.mPanelThis = base.GetComponent<UIPanel>();
			this.mPanelThis.alpha = 0f;
			this.mUIJukeBoxMusicBuyConfirm.SetOnSelectNegativeListener(new Action(this.OnSelectNegativeJukeBoxMusicBuy));
			this.mUIJukeBoxMusicBuyConfirm.SetOnSelectPositiveListener(new Action(this.OnSelectPositiveJukeBoxMusicBuy));
			this.mUIJukeBoxMusicBuyConfirm.SetOnRequestChangeScene(new Action(this.OnRequestChangeScene));
			this.mUIJukeBoxMusicPlayingDialog.SetOnSelectNegativeListener(new Action(this.OnSelectStopPlay));
			this.mUIJukeBoxMusicPlayingDialog.SetOnSelectPositiveListener(new Action(this.OnSelectSettingBgmThis));
			this.mUIJukeBoxMusicPlayingDialog.SetOnRequestChangeScene(new Action(this.OnRequestChangeScene));
			this.mUIJukeBoxPlayListParent.SetOnSelectedMusicListener(new Action<Mst_bgm_jukebox>(this.OnSelectedMusicListener));
			this.mUIJukeBoxPlayListParent.SetOnBackListener(new Action(this.OnBack));
			this.mUIJukeBoxPlayListParent.SetOnRequestChangeScene(new Action(this.OnRequestChangeScene));
			this.mUIJukeBoxMusicBuyConfirm.SetOnRequestBackToRoot(new Action(this.OnRequestBackToRoot));
			this.mUIJukeBoxMusicPlayingDialog.SetOnRequestBackToRoot(new Action(this.OnRequestBackToRoot));
			this.mUIJukeBoxPlayListParent.SetOnRequestBackToRoot(new Action(this.OnRequestBackToRoot));
		}

		private void OnRequestBackToRoot()
		{
			while (this.mStateManager.CurrentState != UserInterfaceJukeBoxManager.State.NONE)
			{
				switch (this.mStateManager.CurrentState)
				{
				case UserInterfaceJukeBoxManager.State.MusicSelect:
					this.mUIJukeBoxPlayListParent.CloseState();
					break;
				case UserInterfaceJukeBoxManager.State.BuyMusicConfirm:
					this.mUIJukeBoxMusicBuyConfirm.CloseState();
					break;
				case UserInterfaceJukeBoxManager.State.Playing:
					this.mUIJukeBoxMusicPlayingDialog.CloseState();
					this.CrossFadeToPortBGM();
					break;
				}
				this.mStateManager.PopState();
			}
			this.OnBack();
		}

		private void OnSelectedMusicListener(Mst_bgm_jukebox jukeBoxBGM)
		{
			this.mKeyController.ClearKeyAll();
			this.mKeyController.firstUpdate = true;
			if (jukeBoxBGM.R_coins <= this.mPortManager.UserInfo.FCoin)
			{
				this.mUIJukeBoxPlayListParent.LockState();
				this.mUIJukeBoxPlayListParent.SetKeyController(null);
				this.mContext.SetJukeBoxBGM(jukeBoxBGM);
				this.mStateManager.PushState(UserInterfaceJukeBoxManager.State.BuyMusicConfirm);
			}
			else
			{
				CommonPopupDialog.Instance.StartPopup("家具コインが不足しています");
			}
		}

		private void OnSelectSettingBgmThis()
		{
			bool flag = this.mStateManager.CurrentState == UserInterfaceJukeBoxManager.State.Playing;
			if (flag)
			{
				int bgm_id = this.mContext.GetJukeBoxBGM().Bgm_id;
				this.mPortManager.SetPortBGM(this.mDeckId, bgm_id);
				BGMFileInfos bGMFileInfos = (BGMFileInfos)bgm_id;
				this.mConfiguredBGM = bGMFileInfos;
				SingletonMonoBehaviour<SoundManager>.Instance.bgmSource.set_loop(true);
				this.mUIJukeBoxMusicPlayingDialog.CloseState();
				this.OnBack();
			}
		}

		private void OnSelectStopPlay()
		{
			bool flag = this.mStateManager.CurrentState == UserInterfaceJukeBoxManager.State.Playing;
			if (flag)
			{
				this.mKeyController.ClearKeyAll();
				this.mKeyController.firstUpdate = true;
				if (this.mPlayBGMWithCrossFadeCoroutine != null)
				{
					base.StopCoroutine(this.mPlayBGMWithCrossFadeCoroutine);
					this.mPlayBGMWithCrossFadeCoroutine = null;
				}
				this.CrossFadeToPortBGM();
				this.mUIJukeBoxMusicPlayingDialog.CloseState();
				this.mStateManager.PopState();
				this.mStateManager.ResumeState();
			}
		}

		private void PlayBGMWithCrossFade(Mst_bgm_jukebox jukeBoxBGM, Action onFinished)
		{
			if (this.mPlayBGMWithCrossFadeCoroutine != null)
			{
				base.StopCoroutine(this.mPlayBGMWithCrossFadeCoroutine);
				this.mPlayBGMWithCrossFadeCoroutine = null;
			}
			this.mPlayBGMWithCrossFadeCoroutine = this.CrossFadeToBGMCoroutine(jukeBoxBGM, onFinished);
			base.StartCoroutine(this.mPlayBGMWithCrossFadeCoroutine);
		}

		[DebuggerHidden]
		private IEnumerator CrossFadeToBGMCoroutine(Mst_bgm_jukebox jukeBoxBGM, Action onFinishedPlayBGM)
		{
			UserInterfaceJukeBoxManager.<CrossFadeToBGMCoroutine>c__IteratorA1 <CrossFadeToBGMCoroutine>c__IteratorA = new UserInterfaceJukeBoxManager.<CrossFadeToBGMCoroutine>c__IteratorA1();
			<CrossFadeToBGMCoroutine>c__IteratorA.jukeBoxBGM = jukeBoxBGM;
			<CrossFadeToBGMCoroutine>c__IteratorA.onFinishedPlayBGM = onFinishedPlayBGM;
			<CrossFadeToBGMCoroutine>c__IteratorA.<$>jukeBoxBGM = jukeBoxBGM;
			<CrossFadeToBGMCoroutine>c__IteratorA.<$>onFinishedPlayBGM = onFinishedPlayBGM;
			<CrossFadeToBGMCoroutine>c__IteratorA.<>f__this = this;
			return <CrossFadeToBGMCoroutine>c__IteratorA;
		}

		private void CrossFadeToPortBGM()
		{
			float volume2 = SingletonMonoBehaviour<SoundManager>.Instance.bgmSource.get_volume();
			bool flag = DOTween.IsTweening(SingletonMonoBehaviour<SoundManager>.Instance);
			if (flag)
			{
				DOTween.Kill(SingletonMonoBehaviour<SoundManager>.Instance, true);
			}
			Sequence sequence = TweenSettingsExtensions.SetId<Sequence>(DOTween.Sequence(), SingletonMonoBehaviour<SoundManager>.Instance);
			Tween tween = TweenSettingsExtensions.SetId<Tweener>(DOVirtual.Float(this.mDefaultVolume, 0f, 0.3f, delegate(float volume)
			{
				SingletonMonoBehaviour<SoundManager>.Instance.bgmSource.set_volume(volume);
			}), SingletonMonoBehaviour<SoundManager>.Instance);
			TweenCallback tweenCallback = delegate
			{
				SingletonMonoBehaviour<SoundManager>.Instance.bgmSource.Stop();
				SoundUtils.PlaySceneBGM(this.mConfiguredBGM);
			};
			Tween tween2 = TweenSettingsExtensions.SetId<Tweener>(TweenSettingsExtensions.OnPlay<Tweener>(DOVirtual.Float(SingletonMonoBehaviour<SoundManager>.Instance.bgmSource.get_volume(), this.mDefaultVolume, 0.3f, delegate(float volume)
			{
				SingletonMonoBehaviour<SoundManager>.Instance.bgmSource.set_volume(volume);
			}), tweenCallback), SingletonMonoBehaviour<SoundManager>.Instance);
			TweenSettingsExtensions.Append(sequence, tween);
			TweenSettingsExtensions.Append(sequence, tween2);
		}

		public override string ToString()
		{
			if (this.mStateManager != null)
			{
				Debug.Log(this.mStateManager.ToString());
			}
			return base.ToString();
		}

		private void OnSelectNegativeJukeBoxMusicBuy()
		{
			if (this.mStateManager.CurrentState == UserInterfaceJukeBoxManager.State.BuyMusicConfirm)
			{
				this.mKeyController.ClearKeyAll();
				this.mKeyController.firstUpdate = true;
				this.mStateManager.PopState();
				this.mStateManager.ResumeState();
			}
		}

		private void OnSelectPositiveJukeBoxMusicBuy()
		{
			bool flag = this.mStateManager.CurrentState == UserInterfaceJukeBoxManager.State.BuyMusicConfirm;
			if (flag)
			{
				Mst_bgm_jukebox jukeBoxBGM = this.mContext.GetJukeBoxBGM();
				int fCoin = this.mPortManager.UserInfo.FCoin;
				bool flag2 = jukeBoxBGM.R_coins <= fCoin;
				if (flag2)
				{
					this.mPortManager.PlayJukeboxBGM(this.mDeckId, jukeBoxBGM.Bgm_id);
					this.mKeyController.ClearKeyAll();
					this.mKeyController.firstUpdate = true;
					this.mStateManager.PopState();
					this.mStateManager.PushState(UserInterfaceJukeBoxManager.State.Playing);
				}
				else
				{
					CommonPopupDialog.Instance.StartPopup("家具コインが不足しています");
				}
			}
		}

		public void Initialize(PortManager portManager, int deckId, Camera overlayCamera)
		{
			this.mPortManager = portManager;
			this.mOverlayCamera = overlayCamera;
			this.mDeckId = deckId;
			this.mConfiguredBGM = (BGMFileInfos)this.mPortManager.UserInfo.GetPortBGMId(deckId);
			this.mContext = new UserInterfaceJukeBoxManager.Context();
			this.mDefaultVolume = SingletonMonoBehaviour<SoundManager>.Instance.bgmSource.get_volume();
			this.mStateManager = new UserInterfaceJukeBoxManager.StateManager<UserInterfaceJukeBoxManager.State>(UserInterfaceJukeBoxManager.State.NONE);
			this.mStateManager.OnPush = new Action<UserInterfaceJukeBoxManager.State>(this.OnPushState);
			this.mStateManager.OnPop = new Action<UserInterfaceJukeBoxManager.State>(this.OnPopState);
			this.mStateManager.OnResume = new Action<UserInterfaceJukeBoxManager.State>(this.OnResumeState);
			this.mUIJukeBoxPlayListParent.Initialize(this.mPortManager, this.mPortManager.GetJukeboxList().ToArray(), this.mOverlayCamera);
			this.mUIJukeBoxPlayListParent.StartState();
		}

		public void Release()
		{
			this.mUIJukeBoxMusicBuyConfirm.Release();
			this.mUIJukeBoxMusicPlayingDialog.Release();
			this.mPanelThis = null;
			this.mStateManager = null;
			this.mPortManager = null;
			this.mUIJukeBoxPlayListParent = null;
			this.mUIJukeBoxMusicPlayingDialog = null;
			this.mKeyController = null;
		}

		[Obsolete("Inspector上で設定して使用します")]
		public void OnTouchBack()
		{
			bool flag = this.mStateManager.CurrentState == UserInterfaceJukeBoxManager.State.BuyMusicConfirm;
			flag |= (this.mStateManager.CurrentState == UserInterfaceJukeBoxManager.State.MusicSelect);
			if (flag)
			{
				this.mStateManager.PopState();
				this.mStateManager.ResumeState();
			}
		}

		public void SetOnBackListener(Action onBackListener)
		{
			this.mOnBackListener = onBackListener;
		}

		private void OnBack()
		{
			if (this.mOnBackListener != null)
			{
				this.mOnBackListener.Invoke();
			}
			this.mStateManager.PopState();
		}

		private void OnRequestChangeScene()
		{
			SingletonMonoBehaviour<PortObjectManager>.Instance.BackToStrategy();
		}

		public void SetKeyController(KeyControl keyController)
		{
			this.mKeyController = keyController;
		}

		public void StartState()
		{
			bool flag = DOTween.IsTweening(this);
			if (flag)
			{
				DOTween.Kill(this, false);
			}
			TweenSettingsExtensions.SetId<Tweener>(DOVirtual.Float(this.mPanelThis.alpha, 1f, 0.3f, delegate(float alpha)
			{
				this.mPanelThis.alpha = alpha;
			}), this);
			this.mStateManager.PushState(UserInterfaceJukeBoxManager.State.MusicSelect);
		}

		public void CloseState()
		{
			bool flag = DOTween.IsTweening(this);
			if (flag)
			{
				DOTween.Kill(this, false);
			}
			TweenSettingsExtensions.SetId<Tweener>(DOVirtual.Float(this.mPanelThis.alpha, 0f, 0.3f, delegate(float alpha)
			{
				this.mPanelThis.alpha = alpha;
			}), this);
			this.mUIJukeBoxPlayListParent.LockState();
			this.mUIJukeBoxPlayListParent.SetKeyController(null);
		}

		private void OnPopState(UserInterfaceJukeBoxManager.State state)
		{
			if (state == UserInterfaceJukeBoxManager.State.BuyMusicConfirm)
			{
				this.mUIJukeBoxMusicBuyConfirm.CloseState();
			}
		}

		private void OnPushState(UserInterfaceJukeBoxManager.State state)
		{
			switch (state)
			{
			case UserInterfaceJukeBoxManager.State.MusicSelect:
				this.OnPushMusicSelectState();
				break;
			case UserInterfaceJukeBoxManager.State.BuyMusicConfirm:
				this.OnPushBuyMusicConfirmState();
				break;
			case UserInterfaceJukeBoxManager.State.Playing:
				this.OnPushPlayingState();
				break;
			}
		}

		private void OnResumeState(UserInterfaceJukeBoxManager.State state)
		{
			if (state == UserInterfaceJukeBoxManager.State.MusicSelect)
			{
				this.mUIJukeBoxPlayListParent.SetKeyController(this.mKeyController);
				this.mUIJukeBoxPlayListParent.ResumeState();
			}
		}

		private void OnPushPlayingState()
		{
			Mst_bgm_jukebox jukeBoxBGM = this.mContext.GetJukeBoxBGM();
			this.PlayBGMWithCrossFade(jukeBoxBGM, new Action(this.OnSelectStopPlay));
			this.mUIJukeBoxMusicPlayingDialog.Initialize(this.mContext.GetJukeBoxBGM());
			this.mUIJukeBoxMusicPlayingDialog.SetKeyController(this.mKeyController);
			this.mUIJukeBoxMusicPlayingDialog.StartState();
		}

		private void OnPushBuyMusicConfirmState()
		{
			this.mUIJukeBoxMusicBuyConfirm.Initialize(this.mContext.GetJukeBoxBGM(), this.mPortManager.UserInfo.FCoin, true);
			this.mUIJukeBoxMusicBuyConfirm.SetKeyController(this.mKeyController);
			this.mUIJukeBoxMusicBuyConfirm.StartState();
		}

		private void OnPushMusicSelectState()
		{
			this.mUIJukeBoxPlayListParent.Refresh(this.mPortManager, this.mPortManager.GetJukeboxList().ToArray(), this.mOverlayCamera);
			this.mUIJukeBoxPlayListParent.SetKeyController(this.mKeyController);
			this.mUIJukeBoxPlayListParent.StartState();
		}
	}
}
