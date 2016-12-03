using LT.Tweening;
using System;
using UnityEngine;

namespace KCV.BattleCut
{
	[RequireComponent(typeof(UIPanel))]
	public class BtlCut_UICircleHPGauge : BaseAnimation
	{
		private UITexture _uiBackground;

		private UITexture _uiForeground;

		private UILabel _uiHPLabel;

		private UIPanel _uiPanel;

		private int _nFromHP;

		private int _nToHP;

		private int _nMaxHP;

		public UIPanel panel
		{
			get
			{
				return this.GetComponentThis(ref this._uiPanel);
			}
		}

		protected override void Awake()
		{
			Util.FindParentToChild<UITexture>(ref this._uiBackground, base.get_transform(), "Background");
			Util.FindParentToChild<UITexture>(ref this._uiForeground, base.get_transform(), "Foreground");
			Util.FindParentToChild<UILabel>(ref this._uiHPLabel, base.get_transform(), "Hp");
			this._uiForeground.type = UIBasicSprite.Type.Filled;
			this._uiForeground.fillAmount = 0f;
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			this._uiBackground = null;
			this._uiForeground = null;
			this._uiHPLabel = null;
		}

		public void SetHPGauge(int maxHP, int beforeHP, int afterHP)
		{
			this.SetHPGauge(maxHP, beforeHP, afterHP, true);
		}

		public void SetHPGauge(int maxHP, int beforeHP, int afterHP, bool isChangeColor)
		{
			this._uiHPLabel.textInt = beforeHP;
			this._nMaxHP = maxHP;
			this._nFromHP = beforeHP;
			this._nToHP = afterHP;
			this._uiHPLabel.textInt = beforeHP;
			this._uiForeground.fillAmount = Mathe.Rate(0f, (float)this._nMaxHP, (float)beforeHP);
			if (isChangeColor)
			{
				this._uiForeground.color = Util.HpGaugeColor2(this._nMaxHP, beforeHP);
			}
		}

		public override void Play(Action callback)
		{
			this._actCallback = callback;
			base.get_transform().LTValue((float)this._nFromHP, (float)this._nToHP, 0.45f).setEase(LeanTweenType.easeOutExpo).setOnUpdate(delegate(float x)
			{
				int num = (int)Math.Floor((double)x);
				this._uiHPLabel.textInt = num;
				this._uiForeground.fillAmount = Mathe.Rate(0f, (float)this._nMaxHP, x);
				this._uiForeground.color = Util.HpGaugeColor2(this._nMaxHP, num);
			}).setOnComplete(delegate
			{
				Dlg.Call(ref callback);
			});
		}

		public LTDescr PlayNonColor()
		{
			return base.get_transform().LTValue((float)this._nFromHP, (float)this._nToHP, 1f).setEase(LeanTweenType.easeOutExpo).setOnUpdate(delegate(float x)
			{
				int textInt = (int)Math.Floor((double)x);
				this._uiHPLabel.textInt = textInt;
				this._uiForeground.fillAmount = Mathe.Rate(0f, (float)this._nMaxHP, x);
			});
		}

		public LTDescr Show(float duration)
		{
			return LeanTween.value(base.get_gameObject(), 0f, 1f, duration).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				this.panel.alpha = x;
			});
		}

		public LTDescr Hide(float duration)
		{
			return LeanTween.value(base.get_gameObject(), 1f, 0f, duration).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				this.panel.alpha = x;
			});
		}
	}
}
