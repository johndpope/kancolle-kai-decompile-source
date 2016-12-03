using System;
using UnityEngine;

namespace KCV.Battle
{
	[RequireComponent(typeof(GlowEffect)), RequireComponent(typeof(MotionBlur))]
	public class BattleCutInEffectCamera : BaseCamera
	{
		private GlowEffect _clsGlowEffect;

		private MotionBlur _clsMotionBlur;

		private Blur _clsBlur;

		public GlowEffect glowEffect
		{
			get
			{
				if (this._clsGlowEffect == null)
				{
					this._clsGlowEffect = base.GetComponent<GlowEffect>();
				}
				return this._clsGlowEffect;
			}
		}

		public MotionBlur motionBlur
		{
			get
			{
				if (this._clsMotionBlur == null)
				{
					this._clsMotionBlur = base.GetComponent<MotionBlur>();
				}
				if (this._clsMotionBlur.shader == null)
				{
					this._clsMotionBlur.shader = Shader.Find("Hidden/MotionBlur");
				}
				return this._clsMotionBlur;
			}
		}

		public Blur blur
		{
			get
			{
				if (this._clsBlur == null)
				{
					this._clsBlur = base.GetComponent<Blur>();
				}
				return this._clsBlur;
			}
		}

		protected override void Awake()
		{
			base.Awake();
			this.glowEffect.glowIntensity = 1.5f;
			this.glowEffect.blurIterations = 3;
			this.glowEffect.blurSpread = 0.7f;
			this.glowEffect.set_enabled(false);
			this.motionBlur.blurAmount = 0.35f;
			this.motionBlur.set_enabled(false);
			this.blur.downsample = 0;
			this.blur.blurSize = 1.36f;
			this.blur.blurIterations = 3;
			this.blur.set_enabled(false);
		}

		protected override void OnUnInit()
		{
			Mem.Del<GlowEffect>(ref this._clsGlowEffect);
			Mem.Del<MotionBlur>(ref this._clsMotionBlur);
			Mem.Del<Blur>(ref this._clsBlur);
			base.OnUnInit();
		}
	}
}
