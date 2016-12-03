using DG.Tweening;
using local.models;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Scene.Practice
{
	[RequireComponent(typeof(UIPanel))]
	public class UIPracticeBattleStartProduction : MonoBehaviour
	{
		private UIPanel mPanelThis;

		[SerializeField]
		private UIPanel mPanel_HideCover;

		[SerializeField]
		private Transform mTransform_FriendFlagShipFrame;

		[SerializeField]
		private Transform mTransform_TargetFlagShipFrame;

		[SerializeField]
		private UITexture mTexture_FriendFlagShip;

		[SerializeField]
		private UITexture mTexture_TargetFlagShip;

		[SerializeField]
		private UIPracticeBattleDeckInShip[] mUIPracticeBattleDeckInShips_Friend;

		[SerializeField]
		private UIPracticeBattleDeckInShip[] mUIPracticeBattleDeckInShips_Enemy;

		[SerializeField]
		private Transform mTransform_UIPracticeShortCutSwitch;

		[SerializeField]
		private Vector3 mVector3_FriendShipShowPosition;

		[SerializeField]
		private Vector3 mVector3_TargetShipShowPosition;

		private DeckModel mFriendDeck;

		private DeckModel mTargetDeck;

		private bool mIsShortCutPlayMode;

		private KeyControl mKeyController;

		private Action<bool> mOnAnimationFinished;

		private void Awake()
		{
			this.mPanelThis = base.GetComponent<UIPanel>();
			this.mPanelThis.alpha = 0f;
			this.mPanel_HideCover.alpha = 1E-08f;
		}

		public void Initialize(DeckModel friend, DeckModel target)
		{
			this.mFriendDeck = friend;
			this.mTargetDeck = target;
		}

		public void SetKeyController(KeyControl keyController)
		{
			this.mKeyController = keyController;
		}

		private void Update()
		{
			if (this.mKeyController != null && this.mKeyController.IsLDown())
			{
				this.mIsShortCutPlayMode = !this.mIsShortCutPlayMode;
				if (this.mIsShortCutPlayMode)
				{
					this.ShowShortCutStateView();
				}
				else
				{
					this.HideShortCutStateView();
				}
			}
		}

		private void ShowShortCutStateView()
		{
			ShortcutExtensions.DOKill(this.mTransform_UIPracticeShortCutSwitch, false);
			ShortcutExtensions.DOLocalMoveX(this.mTransform_UIPracticeShortCutSwitch, -325f, 0.3f, false);
		}

		private void HideShortCutStateView()
		{
			ShortcutExtensions.DOKill(this.mTransform_UIPracticeShortCutSwitch, false);
			ShortcutExtensions.DOLocalMoveX(this.mTransform_UIPracticeShortCutSwitch, -600f, 0.3f, false);
		}

		public void Play()
		{
			base.StartCoroutine(this.PlayCoroutine());
		}

		[DebuggerHidden]
		private IEnumerator PlayCoroutine()
		{
			UIPracticeBattleStartProduction.<PlayCoroutine>c__Iterator154 <PlayCoroutine>c__Iterator = new UIPracticeBattleStartProduction.<PlayCoroutine>c__Iterator154();
			<PlayCoroutine>c__Iterator.<>f__this = this;
			return <PlayCoroutine>c__Iterator;
		}

		private void InitializeFriendDeckInShips(DeckModel deckModel)
		{
			this.InitializeDeckInShips(deckModel, this.mUIPracticeBattleDeckInShips_Friend);
		}

		private void InitializeTargetDeckInShips(DeckModel deckModel)
		{
			this.InitializeDeckInShips(deckModel, this.mUIPracticeBattleDeckInShips_Enemy);
		}

		private void InitializeDeckInShips(DeckModel deckModel, UIPracticeBattleDeckInShip[] deckInShipViews)
		{
			int num = 0;
			int count = deckModel.Count;
			for (int i = 0; i < deckInShipViews.Length; i++)
			{
				UIPracticeBattleDeckInShip uIPracticeBattleDeckInShip = deckInShipViews[i];
				if (num < count)
				{
					uIPracticeBattleDeckInShip.get_transform().SetActive(true);
					uIPracticeBattleDeckInShip.Initialize(deckModel.GetShip(num));
				}
				else
				{
					uIPracticeBattleDeckInShip.get_transform().SetActive(false);
				}
				num++;
			}
		}

		public void SetOnAnimationFinishedListener(Action<bool> onAnimationFinishedListener)
		{
			this.mOnAnimationFinished = onAnimationFinishedListener;
		}

		private void OnAnimationFinished()
		{
			if (this.mOnAnimationFinished != null)
			{
				this.mOnAnimationFinished.Invoke(this.mIsShortCutPlayMode);
			}
		}

		private void OnDestroy()
		{
			if (DOTween.IsTweening(this))
			{
				DOTween.Kill(this, false);
			}
			this.mPanelThis = null;
			this.mTransform_FriendFlagShipFrame = null;
			this.mTransform_TargetFlagShipFrame = null;
			this.mTexture_FriendFlagShip = null;
			this.mTexture_TargetFlagShip = null;
			this.mUIPracticeBattleDeckInShips_Friend = null;
			this.mUIPracticeBattleDeckInShips_Enemy = null;
			this.mTransform_UIPracticeShortCutSwitch = null;
			this.mFriendDeck = null;
			this.mTargetDeck = null;
		}

		internal void ShowCover()
		{
			this.HideShortCutStateView();
			this.mPanel_HideCover.alpha = 1f;
		}
	}
}
