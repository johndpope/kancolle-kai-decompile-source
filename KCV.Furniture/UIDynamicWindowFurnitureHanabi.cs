using DG.Tweening;
using KCV.Scene.Port;
using KCV.Utils;
using System;
using UnityEngine;

namespace KCV.Furniture
{
	public class UIDynamicWindowFurnitureHanabi : UIDynamicWindowFurniture
	{
		private const int TRIGGER_HOUR = 20;

		private const int TRIGGER_MINUTE = 0;

		private AudioSource mAudioSource_Playing;

		[SerializeField]
		private AudioClip mAudioClip_Hanabi_0;

		[SerializeField]
		private UITexture mTexture_Fire;

		[SerializeField]
		private Texture mTexture2d_Red;

		[SerializeField]
		private Texture mTexture2d_Yellow;

		[SerializeField]
		private Texture mTexture2d_Blue;

		[SerializeField]
		private Texture mTexture2d_White;

		[SerializeField]
		private bool mAnimated;

		protected override void OnUpdate()
		{
			base.UpdateWindow();
			if (this.mFurnitureModel != null && this.mFurnitureModel.GetDateTime().get_Hour() == 20 && this.mFurnitureModel.GetDateTime().get_Minute() == 0 && !DOTween.IsTweening(this) && !this.mAnimated)
			{
				this.mAnimated = true;
				this.Animation();
			}
		}

		protected override void OnInitialize(UIFurniture.UIFurnitureModel uiFurnitureModel)
		{
			base.OnInitialize(uiFurnitureModel);
		}

		private void Animation()
		{
			Sequence sequence = TweenSettingsExtensions.SetId<Sequence>(DOTween.Sequence(), this);
			Tween tween = this.GenerateFirstFireWorks();
			TweenSettingsExtensions.Append(sequence, tween);
			TweenSettingsExtensions.Append(sequence, this.GenerateCommonFireWorks());
			TweenSettingsExtensions.Append(sequence, this.GenerateCommonFireWorks());
			TweenSettingsExtensions.Append(sequence, this.GenerateCommonFireWorks());
		}

		private Tween GenerateFirstFireWorks()
		{
			Sequence sequence = TweenSettingsExtensions.SetId<Sequence>(DOTween.Sequence(), this);
			TweenCallback tweenCallback = delegate
			{
				this.mTexture_Fire.mainTexture = this.mTexture2d_Red;
			};
			TweenCallback tweenCallback2 = delegate
			{
				this.mTexture_Fire.mainTexture = this.mTexture2d_Yellow;
			};
			TweenCallback tweenCallback3 = delegate
			{
				this.mTexture_Fire.mainTexture = this.mTexture2d_Blue;
			};
			TweenCallback tweenCallback4 = delegate
			{
				this.mTexture_Fire.mainTexture = this.mTexture2d_White;
			};
			Tween tween = DOVirtual.Float(0f, 1f, 0.15f, delegate(float alpha)
			{
				this.mTexture_Fire.alpha = alpha;
			});
			Tween tween2 = DOVirtual.Float(1f, 0f, 0.8f, delegate(float alpha)
			{
				this.mTexture_Fire.alpha = alpha;
			});
			Tween tween3 = DOVirtual.Float(1f, 0f, 1.6f, delegate(float alpha)
			{
				this.mTexture_Fire.alpha = alpha;
			});
			TweenCallback tweenCallback5 = delegate
			{
				this.mTexture_Fire.alpha = 0f;
				this.mAudioSource_Playing = SoundUtils.PlaySE(this.mAudioClip_Hanabi_0);
			};
			Tween tween4 = DOVirtual.Float(0f, 1f, 2f, delegate(float alpha)
			{
				this.mTexture_Fire.alpha = alpha;
			});
			TweenSettingsExtensions.OnPlay<Sequence>(sequence, tweenCallback5);
			TweenSettingsExtensions.Append(sequence, tween4);
			TweenSettingsExtensions.AppendCallback(sequence, tweenCallback);
			TweenSettingsExtensions.AppendInterval(sequence, 0.15f);
			TweenSettingsExtensions.AppendCallback(sequence, tweenCallback2);
			TweenSettingsExtensions.AppendInterval(sequence, 0.15f);
			TweenSettingsExtensions.AppendCallback(sequence, tweenCallback3);
			TweenSettingsExtensions.AppendInterval(sequence, 0f);
			TweenSettingsExtensions.Append(sequence, tween2);
			TweenSettingsExtensions.AppendCallback(sequence, tweenCallback4);
			TweenSettingsExtensions.Append(sequence, tween);
			TweenSettingsExtensions.Append(sequence, tween3);
			return sequence;
		}

		private Tween GenerateCommonFireWorks()
		{
			Sequence sequence = TweenSettingsExtensions.SetId<Sequence>(DOTween.Sequence(), this);
			TweenCallback tweenCallback = delegate
			{
				this.mTexture_Fire.mainTexture = this.mTexture2d_Red;
			};
			TweenCallback tweenCallback2 = delegate
			{
				this.mTexture_Fire.mainTexture = this.mTexture2d_Yellow;
			};
			TweenCallback tweenCallback3 = delegate
			{
				this.mTexture_Fire.mainTexture = this.mTexture2d_Blue;
			};
			TweenCallback tweenCallback4 = delegate
			{
				this.mTexture_Fire.mainTexture = this.mTexture2d_White;
			};
			Tween tween = DOVirtual.Float(0f, 1f, 0.15f, delegate(float alpha)
			{
				this.mTexture_Fire.alpha = alpha;
			});
			Tween tween2 = DOVirtual.Float(1f, 0f, 0.8f, delegate(float alpha)
			{
				this.mTexture_Fire.alpha = alpha;
			});
			Tween tween3 = DOVirtual.Float(1f, 0f, 1.6f, delegate(float alpha)
			{
				this.mTexture_Fire.alpha = alpha;
			});
			TweenCallback tweenCallback5 = delegate
			{
				this.mTexture_Fire.alpha = 0f;
			};
			Tween tween4 = DOVirtual.Float(0f, 1f, 1.5f, delegate(float alpha)
			{
				this.mTexture_Fire.alpha = alpha;
			});
			TweenSettingsExtensions.OnPlay<Sequence>(sequence, tweenCallback5);
			TweenSettingsExtensions.Append(sequence, tween4);
			TweenSettingsExtensions.AppendCallback(sequence, tweenCallback);
			TweenSettingsExtensions.AppendInterval(sequence, 0.15f);
			TweenSettingsExtensions.AppendCallback(sequence, tweenCallback2);
			TweenSettingsExtensions.AppendInterval(sequence, 0.15f);
			TweenSettingsExtensions.AppendCallback(sequence, tweenCallback3);
			TweenSettingsExtensions.AppendInterval(sequence, 0f);
			TweenSettingsExtensions.Append(sequence, tween2);
			TweenSettingsExtensions.AppendCallback(sequence, tweenCallback4);
			TweenSettingsExtensions.Append(sequence, tween);
			TweenSettingsExtensions.Append(sequence, tween3);
			return sequence;
		}

		protected override void OnDestroyEvent()
		{
			base.OnDestroyEvent();
			if (this.mAudioSource_Playing != null)
			{
				this.mAudioSource_Playing.Stop();
			}
			if (DOTween.IsTweening(this))
			{
				DOTween.Kill(this, false);
			}
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mAudioClip_Hanabi_0, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_Fire, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture2d_Red, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture2d_Yellow, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture2d_Blue, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture2d_White, false);
			this.mAudioSource_Playing = null;
		}
	}
}
