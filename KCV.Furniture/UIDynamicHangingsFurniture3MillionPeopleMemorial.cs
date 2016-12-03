using DG.Tweening;
using KCV.Scene.Port;
using KCV.Utils;
using System;
using UnityEngine;

namespace KCV.Furniture
{
	public class UIDynamicHangingsFurniture3MillionPeopleMemorial : UIDynamicFurniture
	{
		[SerializeField]
		private UITexture mTexture_Main;

		[SerializeField]
		private Texture mTexture2d_Frame_0;

		[SerializeField]
		private Texture mTexture2d_Frame_1;

		[SerializeField]
		private Texture mTexture2d_Frame_2;

		[SerializeField]
		private AudioClip mAudioClip_Up;

		[SerializeField]
		private AudioClip mAudioClip_Down;

		private bool mIsUnhappiness;

		protected override void OnCalledActionEvent()
		{
			this.Animation();
		}

		private void Update()
		{
			bool keyUp = Input.GetKeyUp(97);
			if (keyUp)
			{
				this.Animation();
			}
		}

		private void Animation()
		{
			this.mIsUnhappiness = !this.mIsUnhappiness;
			if (this.mIsUnhappiness)
			{
				this.Down();
			}
			else
			{
				this.GenerateUpTween();
			}
		}

		private Tween GenerateUpTween()
		{
			bool flag = DOTween.IsTweening(this);
			if (flag)
			{
				DOTween.Kill(this, false);
			}
			Sequence sequence = DOTween.Sequence();
			TweenSettingsExtensions.SetId<Sequence>(sequence, this);
			TweenCallback tweenCallback = delegate
			{
				SoundUtils.PlaySE(this.mAudioClip_Up);
			};
			TweenCallback tweenCallback2 = delegate
			{
				this.mTexture_Main.mainTexture = this.mTexture2d_Frame_0;
			};
			TweenCallback tweenCallback3 = delegate
			{
				this.mTexture_Main.mainTexture = this.mTexture2d_Frame_1;
			};
			TweenCallback tweenCallback4 = delegate
			{
				this.mTexture_Main.mainTexture = this.mTexture2d_Frame_2;
			};
			TweenSettingsExtensions.OnPlay<Sequence>(sequence, tweenCallback);
			TweenSettingsExtensions.AppendCallback(sequence, tweenCallback4);
			TweenSettingsExtensions.AppendInterval(sequence, 0.1f);
			TweenSettingsExtensions.AppendCallback(sequence, tweenCallback3);
			TweenSettingsExtensions.AppendInterval(sequence, 0.1f);
			TweenSettingsExtensions.AppendCallback(sequence, tweenCallback2);
			return sequence;
		}

		private void Down()
		{
			bool flag = DOTween.IsTweening(this);
			if (flag)
			{
				DOTween.Kill(this, false);
			}
			SoundUtils.PlaySE(this.mAudioClip_Down);
			this.mTexture_Main.mainTexture = this.mTexture2d_Frame_2;
		}

		protected override void OnDestroyEvent()
		{
			base.OnDestroyEvent();
			if (DOTween.IsTweening(this))
			{
				DOTween.Kill(this, false);
			}
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_Main, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture2d_Frame_0, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture2d_Frame_1, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture2d_Frame_2, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mAudioClip_Up, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mAudioClip_Down, false);
		}
	}
}
