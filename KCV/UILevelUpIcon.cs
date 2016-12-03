using KCV.Utils;
using System;
using UnityEngine;

namespace KCV
{
	[RequireComponent(typeof(Animation))]
	public class UILevelUpIcon : BaseAnimation
	{
		[SerializeField]
		private UISprite _uiLevelUpIcon;

		public float alpha
		{
			get
			{
				return this._uiLevelUpIcon.alpha;
			}
			set
			{
				this._uiLevelUpIcon.alpha = value;
			}
		}

		protected override void Awake()
		{
			base.Awake();
			if (this._uiLevelUpIcon == null)
			{
				Util.FindParentToChild<UISprite>(ref this._uiLevelUpIcon, base.get_transform(), "Icon");
			}
			base.get_transform().localScaleZero();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			this._uiLevelUpIcon = null;
		}

		public override void Play()
		{
			SoundUtils.PlaySE(SEFIleInfos.SE_058);
			base.get_transform().localScaleOne();
			base.Play();
		}

		protected override void onAnimationFinished()
		{
			base.get_transform().localScaleZero();
			base.onAnimationFinished();
		}
	}
}
