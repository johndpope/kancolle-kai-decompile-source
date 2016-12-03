using KCV.View.ScrollView;
using local.managers;
using Server_Models;
using System;
using UnityEngine;

namespace KCV.Furniture.JukeBox
{
	public class UIJukeBoxPlayListParent : UIScrollList<Mst_bgm_jukebox, UIJukeBoxPlayListChild>
	{
		[SerializeField]
		private UILabel mLabel_WalletInCoin;

		private KeyControl mKeyController;

		private ManagerBase mManager;

		private Action mOnRequestBackToRoot;

		private Action mOnBackListener;

		private Action mOnRequestChangeScene;

		private Action<Mst_bgm_jukebox> mOnSelectedMusicListener;

		public void Initialize(ManagerBase manager, Mst_bgm_jukebox[] models, Camera camera)
		{
			this.mManager = manager;
			base.ChangeImmediateContentPosition(UIScrollList<Mst_bgm_jukebox, UIJukeBoxPlayListChild>.ContentDirection.Hell);
			base.Initialize(models);
			base.SetSwipeEventCamera(camera);
		}

		protected override void OnUpdate()
		{
			if (this.mKeyController != null)
			{
				if (this.mKeyController.keyState.get_Item(8).down)
				{
					base.PrevFocus();
				}
				else if (this.mKeyController.keyState.get_Item(12).down)
				{
					base.NextFocus();
				}
				else if (this.mKeyController.keyState.get_Item(14).down)
				{
					base.PrevPageOrHeadFocus();
				}
				else if (this.mKeyController.keyState.get_Item(10).down)
				{
					base.NextPageOrTailFocus();
				}
				else if (this.mKeyController.keyState.get_Item(1).down)
				{
					base.Select();
				}
				else if (this.mKeyController.keyState.get_Item(0).down)
				{
					this.OnBack();
				}
				else if (this.mKeyController.IsRDown())
				{
					this.OnRequestChangeScene();
				}
				else if (this.mKeyController.IsLDown())
				{
					this.OnRequestBackToRoot();
				}
			}
		}

		public void SetOnRequestBackToRoot(Action onRequestBackToRoot)
		{
			this.mOnRequestBackToRoot = onRequestBackToRoot;
		}

		private void OnRequestBackToRoot()
		{
			if (this.mOnRequestBackToRoot != null)
			{
				this.mOnRequestBackToRoot.Invoke();
			}
		}

		public void Refresh(ManagerBase manager, Mst_bgm_jukebox[] models, Camera camera)
		{
			this.Initialize(manager, models, camera);
		}

		protected override bool OnSelectable(UIJukeBoxPlayListChild view)
		{
			return true;
		}

		protected override void OnSelect(UIJukeBoxPlayListChild view)
		{
			this.mOnSelectedMusicListener.Invoke(view.GetModel());
		}

		[Obsolete("Inspector上で設定して使用します")]
		public void OnTouchNext()
		{
			base.NextPageOrTailFocus();
		}

		[Obsolete("Inspector上で設定して使用します")]
		public void OnTouchPrev()
		{
			base.PrevPageOrHeadFocus();
		}

		private void OnBack()
		{
			if (this.mOnBackListener != null)
			{
				this.mOnBackListener.Invoke();
			}
		}

		public void SetOnBackListener(Action onBackListener)
		{
			this.mOnBackListener = onBackListener;
		}

		public void SetOnRequestChangeScene(Action onRequestChangeScene)
		{
			this.mOnRequestChangeScene = onRequestChangeScene;
		}

		private void OnRequestChangeScene()
		{
			if (this.mOnRequestChangeScene != null)
			{
				this.mOnRequestChangeScene.Invoke();
			}
		}

		public void SetOnSelectedMusicListener(Action<Mst_bgm_jukebox> onSelectedMusicListener)
		{
			this.mOnSelectedMusicListener = onSelectedMusicListener;
		}

		public void StartState()
		{
			this.UpdateWalletInCoin();
			base.HeadFocus();
			base.StartControl();
		}

		public void ResumeState()
		{
			this.UpdateWalletInCoin();
			base.ResumeFocus();
		}

		public void LockState()
		{
			base.LockControl();
		}

		public void UpdateWalletInCoin()
		{
			this.mLabel_WalletInCoin.text = this.mManager.UserInfo.FCoin.ToString();
		}

		public void SetKeyController(KeyControl keyController)
		{
			this.mKeyController = keyController;
		}

		private void OnSelect(Mst_bgm_jukebox selectedJukeBoxBGM)
		{
			if (this.mOnSelectedMusicListener != null)
			{
				this.mOnSelectedMusicListener.Invoke(selectedJukeBoxBGM);
			}
		}

		[Obsolete("Inspector上で設定して使用します")]
		public void OnTouchBack()
		{
			this.OnBack();
		}

		internal void CloseState()
		{
			this.OnBack();
		}

		protected override void OnCallDestroy()
		{
			this.mLabel_WalletInCoin = null;
			this.mKeyController = null;
			this.mManager = null;
			this.mOnRequestBackToRoot = null;
			this.mOnBackListener = null;
			this.mOnRequestChangeScene = null;
			this.mOnSelectedMusicListener = null;
		}
	}
}
