using Common.Enum;
using Common.Struct;
using DG.Tweening;
using local.models;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PSVita;

namespace KCV.Scene.Practice.Deck
{
	[RequireComponent(typeof(Camera)), RequireComponent(typeof(UIWidget))]
	public class UIDeckPracticeProductionMovieClip : MonoBehaviour
	{
		public static class Util
		{
			public static List<int> GenerateRandomIndexMap(DeckModel deckModel)
			{
				List<int> list = new List<int>();
				while (Enumerable.Count<int>(list) < deckModel.GetShips().Length)
				{
					int num = Random.Range(0, deckModel.GetShips().Length);
					if (!list.Contains(num))
					{
						list.Add(num);
					}
				}
				return list;
			}
		}

		[SerializeField]
		private Texture mTexture2d_Overlay;

		[SerializeField]
		private UITexture mTexture_MovieClipRendrer;

		[SerializeField]
		private RenderTexture mRenderTexture_MovieClipRendrer;

		private UIWidget mWidgetThis;

		private bool mIsPlaying;

		private bool mIsCallPlay;

		protected DeckPracticeResultModel mDeckPracticeResultModel;

		protected DeckModel mDeckModel;

		private Action<ShipModel, ShipExpModel, PowUpInfo> mOnShipParameterUpEvent;

		private Action mOnFinishedProduction;

		private void Awake()
		{
			this.mTexture_MovieClipRendrer.mainTexture = this.mTexture2d_Overlay;
			this.mTexture_MovieClipRendrer.color = Color.get_clear();
			this.mWidgetThis = base.GetComponent<UIWidget>();
			this.mWidgetThis.alpha = 1E-06f;
		}

		private void OnPreRender()
		{
			if (this.mIsCallPlay)
			{
				PSVitaVideoPlayer.Update();
			}
		}

		private void OnMovieEvent(int eventID)
		{
			if (eventID == 3)
			{
				if (!this.mIsPlaying)
				{
					this.mIsPlaying = true;
					this.mTexture_MovieClipRendrer.mainTexture = this.mRenderTexture_MovieClipRendrer;
					this.mTexture_MovieClipRendrer.color = Color.get_white();
					this.mTexture_MovieClipRendrer.alpha = 1E-06f;
					TweenSettingsExtensions.SetId<Tweener>(TweenSettingsExtensions.SetDelay<Tweener>(DOVirtual.Float(0f, 1f, 0.3f, delegate(float alpha)
					{
						this.mTexture_MovieClipRendrer.alpha = alpha;
					}), 0.3f), this);
				}
			}
			Debug.Log("End OF OnMovieEvent:" + eventID);
		}

		public void Initialize(DeckModel deckModel, DeckPracticeResultModel deckPracticeResultModel)
		{
			this.mDeckModel = deckModel;
			this.mDeckPracticeResultModel = deckPracticeResultModel;
		}

		public void Play()
		{
			TweenSettingsExtensions.SetId<Tweener>(TweenSettingsExtensions.SetDelay<Tweener>(DOVirtual.Float(this.mWidgetThis.alpha, 1f, 0.5f, delegate(float alpha)
			{
				this.mWidgetThis.alpha = alpha;
			}), 0.3f), this);
			this.PlaySlotParamUpProduction();
			string text = UIDeckPracticeProductionMovieClip.FindPracticeMovieClipPath(this.mDeckPracticeResultModel.PracticeType);
			PSVitaVideoPlayer.Init(this.mRenderTexture_MovieClipRendrer);
			PSVitaVideoPlayer.Play(text, 1, 1);
			this.mIsCallPlay = true;
		}

		public void Stop()
		{
			if (this.mIsPlaying)
			{
				this.mRenderTexture_MovieClipRendrer.Release();
				PSVitaVideoPlayer.Init(null);
				this.mIsPlaying = false;
			}
		}

		private static string FindPracticeMovieClipPath(DeckPracticeType practiceType)
		{
			switch (practiceType)
			{
			case DeckPracticeType.Normal:
				return "StreamingAssets/Movies/Practice/Practice_Type_A.mp4";
			case DeckPracticeType.Hou:
				return "StreamingAssets/Movies/Practice/Practice_Type_B.mp4";
			case DeckPracticeType.Rai:
				return "StreamingAssets/Movies/Practice/Practice_Type_C.mp4";
			case DeckPracticeType.Taisen:
				return "StreamingAssets/Movies/Practice/Practice_Type_D.mp4";
			case DeckPracticeType.Kouku:
				return "StreamingAssets/Movies/Practice/Practice_Type_E.mp4";
			case DeckPracticeType.Sougou:
				return "StreamingAssets/Movies/Practice/Practice_Type_F.mp4";
			default:
				throw new Exception("Unknown DeckPracticeType Exception");
			}
		}

		private void OnDestroy()
		{
			if (DOTween.IsTweening(this))
			{
				DOTween.Kill(this, false);
			}
			this.mDeckPracticeResultModel = null;
			this.mDeckModel = null;
			this.mOnShipParameterUpEvent = null;
			this.mOnFinishedProduction = null;
			this.mTexture2d_Overlay = null;
			if (this.mRenderTexture_MovieClipRendrer != null)
			{
				this.mRenderTexture_MovieClipRendrer.Release();
			}
			this.mTexture_MovieClipRendrer.mainTexture = null;
			this.mTexture_MovieClipRendrer = null;
			this.mRenderTexture_MovieClipRendrer = null;
		}

		private void PlaySlotParamUpProduction()
		{
			List<int> randomIndexMap = UIDeckPracticeProductionMovieClip.Util.GenerateRandomIndexMap(this.mDeckModel);
			Sequence randomPowerUpSequence = TweenSettingsExtensions.SetId<Sequence>(DOTween.Sequence(), this);
			ShipModel[] shipModels = this.mDeckPracticeResultModel.Ships;
			TweenSettingsExtensions.SetId<Tween>(DOVirtual.DelayedCall(3.5f, delegate
			{
				Sequence sequence = TweenSettingsExtensions.SetId<Sequence>(DOTween.Sequence(), this);
				int num = 0;
				using (List<int>.Enumerator enumerator = randomIndexMap.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						int current = enumerator.get_Current();
						Tween tween = TweenSettingsExtensions.SetId<Tween>(this.GeneratePowerUpNotifyTween(shipModels[current], (float)num++), this);
						TweenSettingsExtensions.Append(randomPowerUpSequence, tween);
					}
				}
				TweenSettingsExtensions.Join(sequence, randomPowerUpSequence);
				TweenSettingsExtensions.Join(sequence, TweenSettingsExtensions.SetId<Tween>(DOVirtual.DelayedCall(7f, delegate
				{
					this.FinishedProduction();
				}, true), this));
			}, true), this);
		}

		public void SetOnShipParameterUpEventListener(Action<ShipModel, ShipExpModel, PowUpInfo> onShipParameterUpEvent)
		{
			this.mOnShipParameterUpEvent = onShipParameterUpEvent;
		}

		public void SetOnFinishedProductionListener(Action onFinishedProduction)
		{
			this.mOnFinishedProduction = onFinishedProduction;
		}

		private void OnShipParameterUpEventListener(ShipModel shipModel, ShipExpModel shipExpModel, PowUpInfo powUpInfo)
		{
			if (this.mOnShipParameterUpEvent != null)
			{
				this.mOnShipParameterUpEvent.Invoke(shipModel, shipExpModel, powUpInfo);
			}
		}

		private void FinishedProduction()
		{
			if (this.mOnFinishedProduction != null)
			{
				this.mOnFinishedProduction.Invoke();
			}
		}

		private void FadeOut()
		{
			TweenSettingsExtensions.SetId<Tweener>(DOVirtual.Float(this.mWidgetThis.alpha, 0f, 0.3f, delegate(float alpha)
			{
				this.mWidgetThis.alpha = alpha;
			}), this);
		}

		private Tween GeneratePowerUpNotifyTween(ShipModel shipModel, float delay)
		{
			TweenCallback tweenCallback = delegate
			{
				this.OnPowerUpNotify(shipModel);
			};
			return TweenSettingsExtensions.SetId<Tween>(DOVirtual.DelayedCall(delay, tweenCallback, true), this);
		}

		private void OnPowerUpNotify(ShipModel shipModel)
		{
			ShipExpModel shipExpInfo = this.mDeckPracticeResultModel.GetShipExpInfo(shipModel.MemId);
			PowUpInfo shipPowupInfo = this.mDeckPracticeResultModel.GetShipPowupInfo(shipModel.MemId);
			this.OnShipParameterUpEventListener(shipModel, shipExpInfo, shipPowupInfo);
		}
	}
}
