using KCV.Utils;
using LT.Tweening;
using System;
using UniRx;
using UnityEngine;

namespace KCV.Startup
{
	[RequireComponent(typeof(UIPanel))]
	public class ProdSecretaryShipMovie : MonoBehaviour
	{
		[SerializeField]
		private UITexture _uiOverlay;

		private UIPanel _uiPanel;

		private Action _actOnFinished;

		private int _nMstId;

		private UIPanel panel
		{
			get
			{
				return this.GetComponentThis(ref this._uiPanel);
			}
		}

		public static ProdSecretaryShipMovie Instantiate(ProdSecretaryShipMovie prefab, Transform parent, int nSecretaryShipMstId)
		{
			ProdSecretaryShipMovie prodSecretaryShipMovie = Object.Instantiate<ProdSecretaryShipMovie>(prefab);
			prodSecretaryShipMovie.get_transform().set_parent(parent);
			prodSecretaryShipMovie.get_transform().localScaleOne();
			prodSecretaryShipMovie.get_transform().localPositionZero();
			prodSecretaryShipMovie.VirtualCtor(nSecretaryShipMstId);
			return prodSecretaryShipMovie;
		}

		private bool VirtualCtor(int nSecretaryShipMstId)
		{
			this._nMstId = nSecretaryShipMstId;
			this.panel.alpha = 0f;
			this.panel.widgetsAreStatic = true;
			return true;
		}

		public void Play(Action onFinished)
		{
			this._actOnFinished = onFinished;
			this.ShowOverlay(delegate
			{
				this.PlayMovie();
			});
		}

		private void ShowOverlay(Action onFinished)
		{
			this._uiOverlay.color = Color.get_black();
			this.panel.widgetsAreStatic = false;
			this.panel.get_transform().LTValue(this.panel.alpha, 1f, 0.25f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				this.panel.alpha = x;
			}).setOnComplete(delegate
			{
				Dlg.Call(ref onFinished);
			});
		}

		private void HideOverlay(Action onFinished)
		{
			this._uiOverlay.color = Color.get_white();
			this.panel.get_transform().LTValue(this.panel.alpha, 0f, 0.25f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				this.panel.alpha = x;
			}).setOnComplete(delegate
			{
				Dlg.Call(ref onFinished);
			});
		}

		private void PlayMovie()
		{
			PSVitaMovie movie = StartupTaskManager.GetPSVitaMovie();
			movie.SetLooping(0).SetMode(0).SetOnWarningID(new Action(this.OnFinishedMovie)).SetOnPlay(delegate
			{
				SoundUtils.StopFadeBGM(0.2f, null);
			}).SetOnBuffering(delegate
			{
				Observable.Timer(TimeSpan.FromMilliseconds((double)(movie.movieDuration / 2L))).Subscribe(delegate(long _)
				{
					this._uiOverlay.color = Color.get_white();
				});
			}).SetOnFinished(new Action(this.OnFinishedMovie)).Play(MovieFileInfos.Startup.GetFilePath());
		}

		private void OnFinishedMovie()
		{
			FirstMeetingManager fmm = StartupTaskManager.GetFirstMeetingManager();
			Observable.FromCoroutine(() => fmm.Play(this._nMstId, delegate
			{
				Dlg.Call(ref this._actOnFinished);
			}), false).Subscribe<Unit>();
		}
	}
}
