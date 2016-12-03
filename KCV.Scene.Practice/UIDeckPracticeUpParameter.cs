using DG.Tweening;
using System;
using UnityEngine;

namespace KCV.Scene.Practice
{
	public class UIDeckPracticeUpParameter : MonoBehaviour
	{
		private int mBefore;

		private int mAfter;

		private bool mIsAleadyMax;

		[SerializeField]
		private UITexture mTexture_Left;

		[SerializeField]
		private UITexture mTexture_Right;

		[SerializeField]
		private UILabel mLabel_Before;

		[SerializeField]
		private UILabel mLabel_After;

		[SerializeField]
		private UITexture mTexture_ParameterUp;

		[SerializeField]
		private Transform mTransform_ParameterMax;

		private Vector3 mVector3_DefaultLocalPosition;

		private void Awake()
		{
			this.mVector3_DefaultLocalPosition = base.get_transform().get_localPosition();
		}

		public void Initialize(int before, int after, bool aleadyMax)
		{
			this.mBefore = before;
			this.mAfter = after;
			this.mIsAleadyMax = aleadyMax;
			TweenAlpha component = this.mTexture_Left.get_gameObject().GetComponent<TweenAlpha>();
			if (component != null)
			{
				Object.Destroy(component);
			}
			TweenAlpha component2 = this.mTexture_Right.get_gameObject().GetComponent<TweenAlpha>();
			if (component2 != null)
			{
				Object.Destroy(component2);
			}
			this.mLabel_Before.alpha = 1E-07f;
			this.mLabel_After.alpha = 1E-07f;
			this.mTexture_Left.alpha = 1E-07f;
			this.mTexture_Right.alpha = 1E-07f;
			this.mLabel_Before.text = this.mBefore.ToString();
			this.mLabel_After.text = this.mAfter.ToString();
			if (this.mBefore == this.mAfter)
			{
				this.mLabel_After.alpha = 1E-06f;
				this.mTexture_ParameterUp.alpha = 1E-07f;
			}
			this.mTransform_ParameterMax.SetActive(false);
		}

		public Tween GenerateParameterUpAnimation(float duration)
		{
			Sequence sequence = DOTween.Sequence();
			bool flag = this.mAfter != this.mBefore;
			Tween tween = DOVirtual.Float(0f, 1f, duration, delegate(float alpha)
			{
				this.mLabel_Before.alpha = alpha;
				if (this.mAfter != this.mBefore)
				{
					this.mLabel_After.alpha = alpha;
				}
			});
			TweenSettingsExtensions.OnPlay<Sequence>(sequence, this.GenerateArrowTween());
			TweenSettingsExtensions.AppendCallback(sequence, this.GenerateArrowAlphaTweenCallBack(flag, this.mIsAleadyMax));
			TweenSettingsExtensions.Append(sequence, tween);
			if (flag)
			{
				Tween tween2 = this.GenerateTweenParameterUp();
				TweenSettingsExtensions.Join(sequence, tween2);
			}
			return sequence;
		}

		private TweenCallback GenerateArrowAlphaTweenCallBack(bool isParamUp, bool aleadyMax)
		{
			if (aleadyMax)
			{
				return delegate
				{
					if (this.mIsAleadyMax)
					{
						this.mTransform_ParameterMax.SetActive(true);
					}
					this.mTexture_Left.color = Color.get_clear();
					this.mTexture_Right.color = Color.get_clear();
				};
			}
			if (isParamUp)
			{
				return delegate
				{
					if (this.mIsAleadyMax)
					{
						this.mTransform_ParameterMax.SetActive(true);
					}
					this.mTexture_Left.color = Color.get_white();
					this.mTexture_Right.color = Color.get_white();
				};
			}
			return delegate
			{
				if (this.mIsAleadyMax)
				{
					this.mTransform_ParameterMax.SetActive(true);
				}
				this.mTexture_Left.color = new Color(0.266f, 0.266f, 0.266f, 0.13f);
				this.mTexture_Right.color = new Color(0.266f, 0.266f, 0.266f, 0.13f);
			};
		}

		private TweenCallback GenerateArrowTween()
		{
			return delegate
			{
				TweenAlpha component = this.mTexture_Left.get_gameObject().GetComponent<TweenAlpha>();
				if (component != null)
				{
					Object.Destroy(component);
				}
				TweenAlpha component2 = this.mTexture_Right.get_gameObject().GetComponent<TweenAlpha>();
				if (component2 != null)
				{
					Object.Destroy(component2);
				}
				if (this.mAfter != this.mBefore)
				{
					this.mTexture_Left.color = Color.get_white();
					this.mTexture_Right.color = Color.get_white();
					TweenAlpha tweenAlpha = TweenAlpha.Begin(this.mTexture_Left.get_gameObject(), 1f, 0f);
					tweenAlpha.method = UITweener.Method.EaseInOut;
					tweenAlpha.style = UITweener.Style.PingPong;
					TweenAlpha tweenAlpha2 = TweenAlpha.Begin(this.mTexture_Right.get_gameObject(), 1f, 0f);
					tweenAlpha2.method = UITweener.Method.EaseInOut;
					tweenAlpha2.style = UITweener.Style.PingPong;
					tweenAlpha2.delay = 0.5f;
				}
			};
		}

		public Tween GenerateSlotInAnimation()
		{
			return TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.SetDelay<Tweener>(ShortcutExtensions.DOLocalMoveX(base.get_transform(), this.mVector3_DefaultLocalPosition.x - 296f, 0.15f, false), 0.075f), 9);
		}

		public Tween GenerateSlotOutAnimation()
		{
			return TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.SetDelay<Tweener>(ShortcutExtensions.DOLocalMoveX(base.get_transform(), this.mVector3_DefaultLocalPosition.x, 0.15f, false), 0.075f), 9);
		}

		private Tween GenerateTweenParameterUp()
		{
			float y = 0f;
			float num = 20f;
			float num2 = 1.5f;
			float num3 = 0.5f;
			Sequence sequence = TweenSettingsExtensions.SetId<Sequence>(DOTween.Sequence(), this);
			this.mTexture_ParameterUp.alpha = 0f;
			this.mTexture_ParameterUp.get_transform().localPositionY(y);
			Tween tween = TweenSettingsExtensions.SetId<Tweener>(TweenSettingsExtensions.SetDelay<Tweener>(DOVirtual.Float(1f, 0f, num2, delegate(float alpha)
			{
				this.mTexture_ParameterUp.alpha = alpha;
			}), num3), this);
			TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetId<Tweener>(ShortcutExtensions.DOLocalMoveY(this.mTexture_ParameterUp.get_transform(), num, num2, false), this));
			TweenSettingsExtensions.Join(sequence, tween);
			TweenSettingsExtensions.OnPlay<Sequence>(sequence, delegate
			{
				this.mTexture_ParameterUp.alpha = 1f;
			});
			return sequence;
		}

		public void Reposition()
		{
			base.get_transform().set_localPosition(this.mVector3_DefaultLocalPosition);
		}

		private void OnDestroy()
		{
			this.mTexture_Left = null;
			this.mTexture_Right = null;
			this.mLabel_Before = null;
			this.mLabel_After = null;
			this.mTexture_ParameterUp = null;
		}
	}
}
