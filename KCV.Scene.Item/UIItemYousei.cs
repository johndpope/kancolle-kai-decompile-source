using KCV.Scene.Port;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Scene.Item
{
	public class UIItemYousei : MonoBehaviour
	{
		[SerializeField]
		private UITexture mTexture_Yousei;

		[SerializeField]
		private Texture[] mTextures_Frame;

		private IEnumerator mAnimationCoroutine;

		private void Awake()
		{
			this.mAnimationCoroutine = this.InitializeAnimationCoroutine();
		}

		[DebuggerHidden]
		private IEnumerator InitializeAnimationCoroutine()
		{
			UIItemYousei.<InitializeAnimationCoroutine>c__Iterator94 <InitializeAnimationCoroutine>c__Iterator = new UIItemYousei.<InitializeAnimationCoroutine>c__Iterator94();
			<InitializeAnimationCoroutine>c__Iterator.<>f__this = this;
			return <InitializeAnimationCoroutine>c__Iterator;
		}

		private void OnEnable()
		{
			if (this.mAnimationCoroutine != null)
			{
				base.StartCoroutine(this.mAnimationCoroutine);
			}
		}

		private void OnDisable()
		{
			if (this.mAnimationCoroutine != null)
			{
				base.StopCoroutine(this.mAnimationCoroutine);
			}
		}

		[DebuggerHidden]
		private IEnumerator Blink()
		{
			UIItemYousei.<Blink>c__Iterator95 <Blink>c__Iterator = new UIItemYousei.<Blink>c__Iterator95();
			<Blink>c__Iterator.<>f__this = this;
			return <Blink>c__Iterator;
		}

		[DebuggerHidden]
		private IEnumerator DoubleBlink()
		{
			UIItemYousei.<DoubleBlink>c__Iterator96 <DoubleBlink>c__Iterator = new UIItemYousei.<DoubleBlink>c__Iterator96();
			<DoubleBlink>c__Iterator.<>f__this = this;
			return <DoubleBlink>c__Iterator;
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Releases(ref this.mTextures_Frame, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_Yousei, false);
			this.mAnimationCoroutine = null;
		}
	}
}
