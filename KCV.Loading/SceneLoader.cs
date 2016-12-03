using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Loading
{
	public class SceneLoader : MonoBehaviour
	{
		[SerializeField]
		private UILoadingShip loadingShip;

		[SerializeField]
		private Camera MainCamera;

		private AsyncOperation mAsyncOperation;

		private Coroutine cor;

		private void Start()
		{
			Debug.Log("SceneLoader Start");
			if (SingletonMonoBehaviour<AppInformation>.Instance.NextLoadScene == Generics.Scene.Scene_BEF)
			{
				SingletonMonoBehaviour<AppInformation>.Instance.NextLoadScene = Generics.Scene.Strategy;
				Debug.LogError("エラー：次のシーンが設定されていません");
			}
			if (SingletonMonoBehaviour<AppInformation>.Instance.NextLoadType == AppInformation.LoadType.Ship)
			{
				this.loadingShip.LoadNextScene(SingletonMonoBehaviour<AppInformation>.Instance.NextLoadScene);
			}
			else if (SingletonMonoBehaviour<AppInformation>.Instance.NextLoadType == AppInformation.LoadType.Yousei)
			{
				this.loadingShip.SetActive(false);
				if (!SingletonMonoBehaviour<NowLoadingAnimation>.Instance.isYouseiExist)
				{
					SingletonMonoBehaviour<NowLoadingAnimation>.Instance.isNowLoadingAnimation = true;
					SingletonMonoBehaviour<NowLoadingAnimation>.Instance.StartTextAnimation();
					SingletonMonoBehaviour<NowLoadingAnimation>.Instance.StartAnimation(Random.Range(1, 10));
				}
				this.LoadNextScene(SingletonMonoBehaviour<AppInformation>.Instance.NextLoadScene);
			}
			else
			{
				this.loadingShip.SetActive(false);
				this.MainCamera.set_backgroundColor(Color.get_white());
				this.LoadNextScene(SingletonMonoBehaviour<AppInformation>.Instance.NextLoadScene);
			}
			SingletonMonoBehaviour<AppInformation>.Instance.NextLoadScene = Generics.Scene.Scene_BEF;
		}

		private void LoadNextScene(Generics.Scene scene)
		{
			this.mAsyncOperation = Application.LoadLevelAsync(scene.ToString());
			this.mAsyncOperation.set_allowSceneActivation(false);
			if (this.cor != null)
			{
				Debug.LogError("coroutine is not null");
			}
			this.cor = base.StartCoroutine(this.LoadStart());
		}

		[DebuggerHidden]
		private IEnumerator LoadStart()
		{
			SceneLoader.<LoadStart>c__Iterator1BF <LoadStart>c__Iterator1BF = new SceneLoader.<LoadStart>c__Iterator1BF();
			<LoadStart>c__Iterator1BF.<>f__this = this;
			return <LoadStart>c__Iterator1BF;
		}

		private void OnDestroy()
		{
			if (this.cor != null)
			{
				base.StopCoroutine(this.cor);
			}
			this.cor = null;
			this.loadingShip = null;
			this.MainCamera = null;
			this.mAsyncOperation = null;
		}
	}
}
