using Common.Enum;
using DG.Tweening;
using KCV.Base;
using System;
using UnityEngine;

namespace KCV.Battle.Formation
{
	public class UIBattleFormationKind : UICircleCategory<BattleFormationKinds1>
	{
		[SerializeField]
		private UISprite mSprite_Formation;

		[SerializeField]
		private UITexture mTexture_Background_Circle;

		private Vector3 mOnCenterScale = new Vector3(1.1f, 1.1f, 1f);

		private Vector3 mOnOtherThanCenterScale = new Vector3(0.7f, 0.7f, 1f);

		private Vector3 mOnOutDisplayScale = new Vector3(0.7f, 0.7f, 1f);

		public override void Initialize(int index, BattleFormationKinds1 category)
		{
			base.Initialize(index, category);
			this.mSprite_Formation.spriteName = this.GetSpriteNameFormation(category);
			this.mSprite_Formation.get_transform().set_localScale(Vector3.get_zero());
			this.mSprite_Formation.alpha = 0.7f;
			this.mTexture_Background_Circle.alpha = 0f;
		}

		private string GetSpriteNameFormation(BattleFormationKinds1 kind)
		{
			string result = string.Empty;
			switch (kind)
			{
			case BattleFormationKinds1.TanJuu:
				result = "jin_tanjyu";
				break;
			case BattleFormationKinds1.FukuJuu:
				result = "jin_fukujyu";
				break;
			case BattleFormationKinds1.Rinkei:
				result = "jin_rinkei";
				break;
			case BattleFormationKinds1.Teikei:
				result = "jin_teikei";
				break;
			case BattleFormationKinds1.TanOu:
				result = "jin_tanou";
				break;
			}
			return result;
		}

		public override void OnCenter(float animationTime, Ease animationEase)
		{
			base.OnCenter(animationTime, animationEase);
			TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.mSprite_Formation.get_transform(), this.mOnCenterScale, animationTime), animationEase);
		}

		public override void OnOtherThanCenter(float animationTime, Ease easeType)
		{
			base.OnOtherThanCenter(animationTime, easeType);
			TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.mSprite_Formation.get_transform(), this.mOnOtherThanCenterScale, animationTime), easeType);
		}

		public override void OnOutDisplay(float animationTime, Ease easeType, Action onFinished)
		{
			base.OnOutDisplay(animationTime, easeType, onFinished);
			TweenSettingsExtensions.OnComplete<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.mSprite_Formation.get_transform(), this.mOnOutDisplayScale, animationTime), easeType), delegate
			{
				if (onFinished != null)
				{
					onFinished.Invoke();
				}
				this.mSprite_Formation.alpha = 0.5f;
			});
		}

		public override void OnInDisplay(float animationTime, Ease animationEase, Action onFinished)
		{
			base.OnInDisplay(animationTime, animationEase, onFinished);
			this.mSprite_Formation.alpha = 0.7f;
			TweenSettingsExtensions.OnComplete<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.mSprite_Formation.get_transform(), this.mOnOtherThanCenterScale, animationTime), animationEase), delegate
			{
				if (onFinished != null)
				{
					onFinished.Invoke();
				}
			});
		}

		public override void OnFirstDisplay(float animationTime, bool isCenter, Ease easeType)
		{
			base.OnFirstDisplay(animationTime, isCenter, easeType);
			if (isCenter)
			{
				TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.mSprite_Formation.get_transform(), this.mOnCenterScale, animationTime), easeType);
			}
			else
			{
				TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.mSprite_Formation.get_transform(), this.mOnOtherThanCenterScale, animationTime), easeType);
			}
		}

		public override void OnSelectAnimation(Action onAnimationFinished)
		{
			Sequence sequence = DOTween.Sequence();
			UISprite spriteFormation = Util.Instantiate(this.mSprite_Formation.get_gameObject(), base.get_transform().get_gameObject(), false, false).GetComponent<UISprite>();
			spriteFormation.get_transform().set_localScale(this.mSprite_Formation.get_transform().get_localScale());
			spriteFormation.get_transform().set_localPosition(this.mSprite_Formation.get_transform().get_localPosition());
			Tween tween = DOVirtual.Float(spriteFormation.alpha, 0f, 1f, delegate(float alpha)
			{
				spriteFormation.alpha = alpha;
			});
			Tween tween2 = ShortcutExtensions.DOScale(spriteFormation.get_transform(), new Vector3(1.5f, 1.5f), 1f);
			this.mTexture_Background_Circle.get_transform().localScale(new Vector3(0.3f, 0.3f));
			ShortcutExtensions.DOScale(this.mTexture_Background_Circle.get_transform(), Vector3.get_one(), 0.3f);
			Tween tween3 = DOVirtual.Float(0f, 1f, 0.3f, delegate(float alpha)
			{
				this.mTexture_Background_Circle.alpha = alpha;
			});
			Tween tween4 = ShortcutExtensions.DOScale(this.mTexture_Background_Circle.get_transform(), Vector3.get_one(), 0.3f);
			TweenSettingsExtensions.SetEase<Tween>(tween4, 27);
			TweenCallback tweenCallback = delegate
			{
				if (onAnimationFinished != null)
				{
					onAnimationFinished.Invoke();
				}
			};
			TweenSettingsExtensions.OnComplete<Sequence>(TweenSettingsExtensions.AppendInterval(TweenSettingsExtensions.Join(TweenSettingsExtensions.Append(sequence, tween3), tween4), 1f), tweenCallback);
		}
	}
}
