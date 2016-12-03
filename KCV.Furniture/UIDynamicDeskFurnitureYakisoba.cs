using DG.Tweening;
using KCV.Scene.Port;
using System;
using UnityEngine;

namespace KCV.Furniture
{
	public class UIDynamicDeskFurnitureYakisoba : UIDynamicFurniture
	{
		[SerializeField]
		private UITexture mTexture_Smoke_0;

		[SerializeField]
		private UITexture mTexture_Smoke_1;

		[SerializeField]
		private UITexture mTexture_Smoke_2;

		protected override void OnAwake()
		{
			this.mTexture_Smoke_0.alpha = 0f;
			this.mTexture_Smoke_1.alpha = 0f;
			this.mTexture_Smoke_2.alpha = 0f;
		}

		protected override void OnInitialize(UIFurniture.UIFurnitureModel uiFurnitureModel)
		{
			base.OnInitialize(uiFurnitureModel);
			this.Animation();
		}

		private void Animation()
		{
			bool flag = DOTween.IsTweening(this);
			if (flag)
			{
				DOTween.Kill(this, false);
			}
			Sequence sequence = DOTween.Sequence();
			TweenSettingsExtensions.SetId<Sequence>(sequence, this);
			Tween tween = this.GenerateTweenSmoke(this.mTexture_Smoke_0, 1f);
			Tween tween2 = this.GenerateTweenSmoke(this.mTexture_Smoke_1, 1f);
			Tween tween3 = this.GenerateTweenSmoke(this.mTexture_Smoke_2, 1f);
			TweenSettingsExtensions.Append(sequence, tween3);
			TweenSettingsExtensions.Append(sequence, tween);
			TweenSettingsExtensions.Append(sequence, tween2);
			TweenSettingsExtensions.AppendInterval(sequence, 1f);
			TweenSettingsExtensions.SetLoops<Sequence>(sequence, 2147483647, 0);
		}

		private Tween GenerateTweenSmoke(UITexture smokeTexture, float duration)
		{
			bool flag = DOTween.IsTweening(smokeTexture);
			if (flag)
			{
				DOTween.Kill(this, true);
			}
			Sequence sequence = TweenSettingsExtensions.SetId<Sequence>(DOTween.Sequence(), this);
			TweenSettingsExtensions.SetId<Sequence>(sequence, sequence);
			Sequence sequence2 = TweenSettingsExtensions.SetId<Sequence>(DOTween.Sequence(), this);
			Tween tween = DOVirtual.Float(0f, 1f, duration * 0.5f, delegate(float alpha)
			{
				smokeTexture.alpha = alpha;
			});
			Tween tween2 = DOVirtual.Float(1f, 0f, duration * 0.5f, delegate(float alpha)
			{
				smokeTexture.alpha = alpha;
			});
			TweenSettingsExtensions.Append(sequence2, tween);
			TweenSettingsExtensions.Append(sequence2, tween2);
			Tween tween3 = ShortcutExtensions.DOLocalMoveY(smokeTexture.get_transform(), smokeTexture.get_transform().get_localPosition().y + 10f, duration, false);
			TweenSettingsExtensions.Append(sequence, sequence2);
			TweenSettingsExtensions.Join(sequence, tween3);
			return sequence;
		}

		protected override void OnDestroyEvent()
		{
			base.OnDestroyEvent();
			if (DOTween.IsTweening(this))
			{
				DOTween.Kill(this, false);
			}
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_Smoke_0, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_Smoke_1, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_Smoke_2, false);
		}
	}
}
