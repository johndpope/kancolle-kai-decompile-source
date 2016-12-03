using System;
using UnityEngine;

namespace KCV.Loading
{
	public class UILoadingShip : MonoBehaviour
	{
		[SerializeField]
		private UISprite mSpriteLoadingShip;

		[SerializeField]
		private UIRipples mRipples;

		[SerializeField]
		private Transform mLoadingText;

		private AsyncOperation mAsyncOperation;

		private bool b;

		private void Start()
		{
			this.StopLoadingAnimation();
		}

		private void Update()
		{
			if (this.mAsyncOperation != null)
			{
				Debug.Log("%" + this.mAsyncOperation.get_progress());
				if (this.mAsyncOperation.get_progress() >= 0.9f && !this.b)
				{
					this.mAsyncOperation.set_allowSceneActivation(true);
					this.mAsyncOperation = null;
				}
			}
		}

		private void OnDestroy()
		{
			DebugUtils.SLog(":(");
		}

		public void LoadNextScene(Generics.Scene nextScene)
		{
			DebugUtils.SLog("LoadNextScene");
			this.StartLoadingAnimation();
			if (this.mAsyncOperation != null)
			{
				this.mAsyncOperation = null;
			}
			string text = nextScene.ToString();
			this.mAsyncOperation = Application.LoadLevelAsync(text);
			this.mAsyncOperation.set_allowSceneActivation(false);
			DebugUtils.SLog("LoadNextScene END");
		}

		private void StartLoadingAnimation()
		{
			DebugUtils.SLog("StartLoadingAnimation");
			this.mSpriteLoadingShip.get_gameObject().SetActive(true);
			this.mRipples.get_gameObject().SetActive(true);
			this.mLoadingText.SetActive(true);
			this.mSpriteLoadingShip.GetComponent<Animation>().Play("Anim_LoadingShip");
			this.mRipples.PlayRipple();
			DebugUtils.SLog("StartLoadingAnimation\u3000END");
		}

		private void StopLoadingAnimation()
		{
			DebugUtils.SLog("StopLoadingAnimation");
			this.mSpriteLoadingShip.get_gameObject().SetActive(false);
			this.mRipples.get_gameObject().SetActive(false);
			this.mLoadingText.SetActive(false);
			DebugUtils.SLog("StopLoadingAnimation\u3000END");
		}
	}
}
