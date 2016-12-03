using DG.Tweening;
using System;
using UnityEngine;

namespace KCV.Furniture.JukeBox
{
	[RequireComponent(typeof(UIPanel))]
	public class UIJukeBoxMusicPlayingRollLabel : MonoBehaviour
	{
		[SerializeField]
		private UIPanel mPanelThis;

		[SerializeField]
		private UILabel mLabel_Title;

		private float mLeft;

		private float mRight;

		private void Awake()
		{
			this.mPanelThis = base.GetComponent<UIPanel>();
			this.mLeft = -(this.mPanelThis.width / 2f);
			this.mRight = this.mPanelThis.width;
		}

		public void Initialize(string title)
		{
			this.mLabel_Title.text = title;
		}

		public void StartRoll()
		{
			bool flag = DOTween.IsTweening(this);
			if (flag)
			{
				DOTween.Kill(this, false);
			}
			Sequence sequence = TweenSettingsExtensions.SetId<Sequence>(DOTween.Sequence(), this);
			this.mLabel_Title.get_transform().localPositionX(this.mRight + (float)(this.mLabel_Title.width / 2));
			Tween tween = ShortcutExtensions.DOLocalMoveX(this.mLabel_Title.get_transform(), (float)(this.mLabel_Title.width / 2), 3f, false);
			Tween tween2 = TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.OnComplete<Tweener>(ShortcutExtensions.DOLocalMoveX(this.mLabel_Title.get_transform(), this.mLeft, 3f, false), delegate
			{
				this.mLabel_Title.get_transform().localPositionX(this.mRight + (float)(this.mLabel_Title.width / 2));
			}), 1);
			TweenSettingsExtensions.Append(sequence, tween);
			TweenSettingsExtensions.AppendInterval(sequence, 1.5f);
			TweenSettingsExtensions.Append(sequence, tween2);
			TweenSettingsExtensions.AppendInterval(sequence, 0.5f);
			TweenSettingsExtensions.SetLoops<Sequence>(sequence, 2147483647, 0);
		}

		public void StopRoll()
		{
			bool flag = DOTween.IsTweening(this);
			if (flag)
			{
				DOTween.Kill(this, false);
			}
		}
	}
}
