using KCV.Utils;
using LT.Tweening;
using System;
using UniRx;
using UnityEngine;

namespace KCV.Title
{
	public class TaskTitleOpening : SceneTaskMono
	{
		private IDisposable _disMovieSubscription;

		private bool _isInputPossible;

		protected override bool Init()
		{
			PSVitaMovie movie = TitleTaskManager.GetPSVitaMovie();
			UIPanel maskPanel = TitleTaskManager.GetMaskPanel();
			this._isInputPossible = false;
			SoundUtils.StopBGM();
			if (!movie.isPlaying)
			{
				movie.SetLooping(0).SetMode(0).SetOnWarningID(delegate
				{
					movie.ImmediateOnFinished();
				}).SetOnPlay(delegate
				{
					maskPanel.get_transform().LTCancel();
					maskPanel.get_transform().LTValue(maskPanel.alpha, 1f, 0.15f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
					{
						maskPanel.alpha = x;
					});
				}).SetOnBuffering(delegate
				{
					Observable.Timer(TimeSpan.FromSeconds(1.0)).Subscribe(delegate(long _)
					{
						this._isInputPossible = true;
					});
					maskPanel.get_transform().GetChild(0).GetComponent<UITexture>().color = Color.get_white();
				}).SetOnFinished(new Action(this.OnMovieFinished)).Play(MovieFileInfos.MOVIE_FILE_INFOS_ID_ST.GetFilePath());
			}
			if (SingletonMonoBehaviour<FadeCamera>.Instance != null && SingletonMonoBehaviour<FadeCamera>.Instance.isFadeOut)
			{
				SingletonMonoBehaviour<FadeCamera>.Instance.FadeIn(0.2f, null);
			}
			return true;
		}

		protected override bool UnInit()
		{
			Mem.Del<IDisposable>(ref this._disMovieSubscription);
			return true;
		}

		protected override bool Run()
		{
			KeyControl keyControl = TitleTaskManager.GetKeyControl();
			PSVitaMovie pSVitaMovie = TitleTaskManager.GetPSVitaMovie();
			if (this._isInputPossible && pSVitaMovie.isPlaying && (keyControl.GetDown(KeyControl.KeyName.MARU) || keyControl.GetDown(KeyControl.KeyName.START) || Input.get_touchCount() != 0))
			{
				this._isInputPossible = false;
				UIPanel maskPanel = TitleTaskManager.GetMaskPanel();
				maskPanel.get_transform().GetChild(0).GetComponent<UITexture>().color = Color.get_black();
				pSVitaMovie.ImmediateOnFinished();
			}
			return TitleTaskManager.GetMode() == TitleTaskManagerMode.TitleTaskManagerMode_BEF || TitleTaskManager.GetMode() == TitleTaskManagerMode.TitleTaskManagerMode_ST;
		}

		private void OnMovieFinished()
		{
			TitleTaskManager.ReqMode(TitleTaskManagerMode.SelectMode);
		}

		public void PlayImmediateOpeningMovie()
		{
			PSVitaMovie movie = TitleTaskManager.GetPSVitaMovie();
			UIPanel maskPanel = TitleTaskManager.GetMaskPanel();
			movie = TitleTaskManager.GetPSVitaMovie();
			movie.SetLooping(0).SetMode(0).SetOnPlay(delegate
			{
				maskPanel.get_transform().LTCancel();
				maskPanel.get_transform().LTValue(maskPanel.alpha, 1f, 0.15f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
				{
					maskPanel.alpha = x;
				});
			}).SetOnBuffering(delegate
			{
				Observable.Timer(TimeSpan.FromSeconds(1.0)).Subscribe(delegate(long _)
				{
					this._isInputPossible = true;
				});
				maskPanel.get_transform().GetChild(0).GetComponent<UITexture>().color = Color.get_white();
			}).SetOnWarningID(delegate
			{
				movie.ImmediateOnFinished();
			}).SetOnFinished(new Action(this.OnMovieFinished)).Play(MovieFileInfos.MOVIE_FILE_INFOS_ID_ST.GetFilePath());
		}
	}
}
