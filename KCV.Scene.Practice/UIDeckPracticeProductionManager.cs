using Common.Struct;
using DG.Tweening;
using KCV.Scene.Practice.Deck;
using local.models;
using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace KCV.Scene.Practice
{
	public class UIDeckPracticeProductionManager : MonoBehaviour
	{
		public enum State
		{
			None,
			Production,
			EndOfPractice,
			WaitNext
		}

		[SerializeField]
		private UIDeckPracticeBanner[] mUIDeckPracticeBanner_Banners;

		[SerializeField]
		private Transform mTransform_DeckPracticeProductionArea;

		[SerializeField]
		private UIButton mButton_Next;

		[SerializeField]
		private UITexture mTexture_Frame;

		[SerializeField]
		private UIDeckPracticeProductionShipParameterResult mUIDeckPracticeProductionShipParameterResult;

		[SerializeField]
		private UIDeckPracticeShutter mUIDeckPracticeShutter;

		[SerializeField]
		private UIDeckPracticeProductionMovieClip mPrefab_UIDeckPracticeProductionMovieClip;

		private UIDeckPracticeProductionMovieClip mUIDeckPracticeProductionMovieClip;

		[SerializeField]
		private UITexture mTexture_EndMessage;

		[SerializeField]
		private TweenAlpha mTween_EndMessage;

		private UIDeckPracticeBanner[] mUIDeckPracticeBanners;

		private Action mOnFinishedProduction;

		private DeckPracticeResultModel mDeckPracticeResultModel;

		private StateManager<UIDeckPracticeProductionManager.State> mStateManager;

		private KeyControl mKeyController;

		private Action<UIDeckPracticeProductionManager.State> mOnChangeStateListener;

		public void SetOnChangeStateListener(Action<UIDeckPracticeProductionManager.State> onChangeStateListener)
		{
			this.mOnChangeStateListener = onChangeStateListener;
		}

		private void OnChangeStateListener(UIDeckPracticeProductionManager.State state)
		{
			if (this.mOnChangeStateListener != null)
			{
				this.mOnChangeStateListener.Invoke(state);
			}
		}

		private void Awake()
		{
			this.mButton_Next.SetActive(false);
			this.mTexture_Frame.alpha = 0f;
		}

		public void SetKeyController(KeyControl keyController)
		{
			this.mKeyController = keyController;
		}

		[DebuggerHidden]
		public IEnumerator InitializeCoroutine(DeckModel deckModel, DeckPracticeResultModel deckPracticeResultModel)
		{
			UIDeckPracticeProductionManager.<InitializeCoroutine>c__Iterator14B <InitializeCoroutine>c__Iterator14B = new UIDeckPracticeProductionManager.<InitializeCoroutine>c__Iterator14B();
			<InitializeCoroutine>c__Iterator14B.deckPracticeResultModel = deckPracticeResultModel;
			<InitializeCoroutine>c__Iterator14B.deckModel = deckModel;
			<InitializeCoroutine>c__Iterator14B.<$>deckPracticeResultModel = deckPracticeResultModel;
			<InitializeCoroutine>c__Iterator14B.<$>deckModel = deckModel;
			<InitializeCoroutine>c__Iterator14B.<>f__this = this;
			return <InitializeCoroutine>c__Iterator14B;
		}

		private void OnPushState(UIDeckPracticeProductionManager.State state)
		{
			if (state == UIDeckPracticeProductionManager.State.EndOfPractice)
			{
				this.OnPushEndOfPracticeState();
			}
		}

		private void OnSwitchState(UIDeckPracticeProductionManager.State state)
		{
			this.OnChangeStateListener(state);
		}

		private void OnShipParameterUpEventListener(ShipModel shipModel, ShipExpModel shipExpModel, PowUpInfo powUpInfo)
		{
			UIDeckPracticeBanner uIDeckPracticeBanner = Enumerable.First<UIDeckPracticeBanner>(this.mUIDeckPracticeBanners, (UIDeckPracticeBanner shipBanner) => shipBanner.Model.MemId == shipModel.MemId);
			bool flag = !powUpInfo.IsAllZero();
			if (flag)
			{
				uIDeckPracticeBanner.PlayPracticeWithLevelUp();
			}
			else
			{
				uIDeckPracticeBanner.PlayPractice();
			}
		}

		public void PlayShipBannerIn()
		{
			DOVirtual.DelayedCall(1.3f, delegate
			{
				Sequence sequence = DOTween.Sequence();
				for (int i = 0; i < this.mUIDeckPracticeBanners.Length; i++)
				{
					UIDeckPracticeBanner banner = this.mUIDeckPracticeBanners[i];
					float x = banner.get_transform().get_localPosition().x;
					banner.get_transform().localPositionX(banner.get_transform().get_localPosition().x - 80f);
					Tween tween = TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMoveX(banner.get_transform(), x, 0.5f, false), 21);
					Tween tween2 = DOVirtual.Float(banner.alpha, 1f, 0.3f, delegate(float alpha)
					{
						banner.alpha = alpha;
					});
					Sequence sequence2 = DOTween.Sequence();
					TweenSettingsExtensions.Join(sequence2, tween);
					TweenSettingsExtensions.Join(sequence2, tween2);
					TweenSettingsExtensions.SetDelay<Sequence>(sequence2, 0.05f);
					TweenSettingsExtensions.Join(sequence, sequence2);
				}
			}, true);
		}

		public void PlayProduction()
		{
			this.mStateManager.PushState(UIDeckPracticeProductionManager.State.Production);
			TweenSettingsExtensions.SetDelay<Tweener>(DOVirtual.Float(this.mTexture_Frame.alpha, 1f, 0.5f, delegate(float alpha)
			{
				this.mTexture_Frame.alpha = alpha;
			}), 0.3f);
			this.mUIDeckPracticeProductionMovieClip.Play();
		}

		public void SetOnFinishedProduction(Action onFinishedProduction)
		{
			this.mOnFinishedProduction = onFinishedProduction;
		}

		private void OnFinishedProduction()
		{
			this.mStateManager.PushState(UIDeckPracticeProductionManager.State.EndOfPractice);
		}

		private void OnPushEndOfPracticeState()
		{
			if (SingletonMonoBehaviour<Live2DModel>.exist())
			{
				SingletonMonoBehaviour<Live2DModel>.Instance.Disable();
			}
			this.mUIDeckPracticeShutter.SetOnFinishedCloseShutterAnimationListener(new Action(this.OnClosedShutterEventForEndOfPractice));
			this.mUIDeckPracticeShutter.CloseShutter();
		}

		private void OnClosedShutterEventForEndOfPractice()
		{
			this.mUIDeckPracticeProductionMovieClip.Stop();
			this.mUIDeckPracticeShutter.SetOnFinishedCloseShutterAnimationListener(null);
			DOVirtual.DelayedCall(0.8f, delegate
			{
				this.mTween_EndMessage.SetOnFinished(delegate
				{
					this.ChangeNextMovableState();
				});
				this.mTween_EndMessage.get_gameObject().SetActive(true);
				this.mTween_EndMessage.PlayForward();
			}, true);
			this.mUIDeckPracticeProductionMovieClip.SetActive(false);
		}

		protected void ChangeNextMovableState()
		{
			bool flag = this.mStateManager.CurrentState == UIDeckPracticeProductionManager.State.EndOfPractice;
			if (flag)
			{
				this.mUIDeckPracticeShutter.SetOnFinishedOpenShutterAnimationListener(new Action(this.OnOpendShutterEventForResult));
				this.mUIDeckPracticeProductionShipParameterResult.SetBackGroundAlpha(1f);
				this.mUIDeckPracticeShutter.OpenShutter();
			}
		}

		private void OnOpendShutterEventForResult()
		{
			this.mUIDeckPracticeShutter.SetOnFinishedOpenShutterAnimationListener(null);
			this.mUIDeckPracticeProductionShipParameterResult.SetOnProductionFinishedListener(delegate
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.SceneLoad(Generics.Scene.Strategy);
			});
			this.mUIDeckPracticeProductionShipParameterResult.SetKeyController(this.mKeyController);
			this.mUIDeckPracticeProductionShipParameterResult.StartProduction();
		}

		[DebuggerHidden]
		private IEnumerator WaitKeyOrTouch()
		{
			UIDeckPracticeProductionManager.<WaitKeyOrTouch>c__Iterator14C <WaitKeyOrTouch>c__Iterator14C = new UIDeckPracticeProductionManager.<WaitKeyOrTouch>c__Iterator14C();
			<WaitKeyOrTouch>c__Iterator14C.<>f__this = this;
			return <WaitKeyOrTouch>c__Iterator14C;
		}

		private void OnClickNext()
		{
			bool flag = this.mStateManager.CurrentState == UIDeckPracticeProductionManager.State.WaitNext;
			if (flag)
			{
				this.mStateManager.PopState();
				this.OnMoveNext();
			}
		}

		[Obsolete("UI側から呼ぶためのメソッドです")]
		public void OnTouchNext()
		{
			bool flag = this.mStateManager.CurrentState == UIDeckPracticeProductionManager.State.WaitNext;
			if (flag)
			{
				this.mStateManager.PopState();
				this.OnMoveNext();
			}
		}

		private void OnMoveNext()
		{
			if (this.mOnFinishedProduction != null)
			{
				this.mOnFinishedProduction.Invoke();
			}
		}

		private void OnDestroy()
		{
			this.mPrefab_UIDeckPracticeProductionMovieClip = null;
			this.mUIDeckPracticeProductionMovieClip = null;
			this.mUIDeckPracticeBanner_Banners = null;
			this.mTransform_DeckPracticeProductionArea = null;
			this.mButton_Next = null;
			this.mTexture_Frame = null;
			this.mUIDeckPracticeProductionShipParameterResult = null;
			this.mUIDeckPracticeShutter = null;
			this.mTexture_EndMessage = null;
			this.mTween_EndMessage = null;
			this.mUIDeckPracticeBanners = null;
			this.mOnFinishedProduction = null;
			this.mDeckPracticeResultModel = null;
			this.mStateManager = null;
			this.mKeyController = null;
			this.mOnChangeStateListener = null;
		}
	}
}
