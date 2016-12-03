using System;
using UnityEngine;

namespace KCV.Dialog
{
	public class ModalCamera : MonoBehaviour
	{
		[SerializeField]
		private UITexture mTextureBackground;

		private bool mShowFlag;

		private void Awake()
		{
			this.Close();
		}

		public void Show()
		{
			TweenAlpha tweenAlpha = UITweener.Begin<TweenAlpha>(this.mTextureBackground.get_gameObject(), 0.1f);
			tweenAlpha.from = this.mTextureBackground.alpha;
			tweenAlpha.to = 0.5f;
			this.mTextureBackground.GetComponent<Collider2D>().set_enabled(true);
		}

		public void Close()
		{
			TweenAlpha tweenAlpha = UITweener.Begin<TweenAlpha>(this.mTextureBackground.get_gameObject(), 0.1f);
			tweenAlpha.from = this.mTextureBackground.alpha;
			tweenAlpha.to = 0.01f;
			this.mTextureBackground.GetComponent<Collider2D>().set_enabled(false);
		}
	}
}
