using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace KCV.SplashScreen
{
	public class SplashScreenTaskManager : MonoBehaviour
	{
		[SerializeField]
		private TweenAlpha _taTexture;

		private AsyncOperation _asyncOperation;

		private void Start()
		{
			Application.LoadLevel(Generics.Scene.Title.ToString());
		}

		private void OnDestroy()
		{
			Mem.Del<TweenAlpha>(ref this._taTexture);
		}

		[DebuggerHidden]
		private IEnumerator LoadNextTitle(IObserver<bool> observer)
		{
			SplashScreenTaskManager.<LoadNextTitle>c__Iterator132 <LoadNextTitle>c__Iterator = new SplashScreenTaskManager.<LoadNextTitle>c__Iterator132();
			<LoadNextTitle>c__Iterator.observer = observer;
			<LoadNextTitle>c__Iterator.<$>observer = observer;
			<LoadNextTitle>c__Iterator.<>f__this = this;
			return <LoadNextTitle>c__Iterator;
		}

		public void OnFinished()
		{
			this._asyncOperation.set_allowSceneActivation(true);
		}
	}
}
