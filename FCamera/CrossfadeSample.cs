using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace FCamera
{
	public class CrossfadeSample : MonoBehaviour
	{
		private Texture2D crossfadeTexture;

		public Texture maskTexture;

		public float fadeoutTime = 1.4f;

		private int nextScene;

		private void Start()
		{
			SingletonMonoBehaviour<FadeCamera>.Instance.UpdateMaskTexture(this.maskTexture);
		}

		private void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				this.LoadLevel(this.nextScene);
			}
		}

		private void LoadLevel(int nextLevel)
		{
			base.StartCoroutine(this.CaptureScreen(delegate
			{
				SingletonMonoBehaviour<FadeCamera>.Instance.UpdateTexture(this.crossfadeTexture);
				Application.LoadLevel(nextLevel);
				SingletonMonoBehaviour<FadeCamera>.Instance.FadeIn(this.fadeoutTime, delegate
				{
					Object.Destroy(this.crossfadeTexture);
				});
			}));
		}

		[DebuggerHidden]
		private IEnumerator CaptureScreen(Action action)
		{
			CrossfadeSample.<CaptureScreen>c__Iterator0 <CaptureScreen>c__Iterator = new CrossfadeSample.<CaptureScreen>c__Iterator0();
			<CaptureScreen>c__Iterator.action = action;
			<CaptureScreen>c__Iterator.<$>action = action;
			<CaptureScreen>c__Iterator.<>f__this = this;
			return <CaptureScreen>c__Iterator;
		}
	}
}
