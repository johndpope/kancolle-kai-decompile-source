using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Loading
{
	public class UserInterfaceLoadingManager : SingletonMonoBehaviour<UserInterfaceLoadingManager>
	{
		[SerializeField]
		private UISprite mSpriteLoadingShip;

		[SerializeField]
		private UIRipples mRipples;

		[SerializeField]
		private Camera mCamera;

		private bool mSceneChanged;

		private AsyncOperation mAsyncOperation;

		private Action mParallelAction;

		private void Start()
		{
			this.StopLoadingAnimation();
			base.get_gameObject().SetActive(false);
		}

		private void Update()
		{
			if (!this.mSceneChanged && this.mAsyncOperation != null && this.mAsyncOperation.get_progress() >= 0.9f && this.mParallelAction == null)
			{
				this.mAsyncOperation.set_allowSceneActivation(true);
				this.StopLoadingAnimation();
				this.mSceneChanged = true;
				this.mCamera.set_depth(0f);
				base.get_gameObject().SetActive(false);
			}
			else
			{
				Debug.Log("Wait Tasks");
			}
		}

		public void LoadNextScene(Generics.Scene nextScene, Action parallelAction)
		{
			this.mCamera.set_depth(100f);
			base.get_gameObject().SetActive(true);
			Debug.Log(string.Empty);
			this.StartLoadingAnimation();
			this.mParallelAction = parallelAction;
			base.StartCoroutine(this.StartParallelAction());
			if (this.mAsyncOperation != null)
			{
				this.mAsyncOperation = null;
			}
			this.mAsyncOperation = Application.LoadLevelAsync(nextScene.ToString());
			this.mAsyncOperation.set_allowSceneActivation(false);
		}

		private void StartLoadingAnimation()
		{
			this.mSpriteLoadingShip.get_gameObject().SetActive(true);
			this.mRipples.get_gameObject().SetActive(true);
			this.mSpriteLoadingShip.GetComponent<Animation>().Play("Anim_LoadingShip");
			this.mRipples.PlayRipple();
		}

		private void StopLoadingAnimation()
		{
			this.mSpriteLoadingShip.get_gameObject().SetActive(false);
			this.mRipples.get_gameObject().SetActive(false);
		}

		[DebuggerHidden]
		private IEnumerator StartParallelAction()
		{
			UserInterfaceLoadingManager.<StartParallelAction>c__Iterator1C5 <StartParallelAction>c__Iterator1C = new UserInterfaceLoadingManager.<StartParallelAction>c__Iterator1C5();
			<StartParallelAction>c__Iterator1C.<>f__this = this;
			return <StartParallelAction>c__Iterator1C;
		}
	}
}
