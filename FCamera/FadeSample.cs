using System;
using UnityEngine;

namespace FCamera
{
	public class FadeSample : MonoBehaviour
	{
		public Texture2D texture;

		public Texture2D startMask;

		public Texture2D endMask;

		[Range(0f, 3f)]
		public float fadeinTime = 0.4f;

		[Range(0f, 3f)]
		public float fadeoutTime = 1.4f;

		private int nextScene;

		private void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				this.LoadLevel();
			}
		}

		private void LoadLevel()
		{
			SingletonMonoBehaviour<FadeCamera>.Instance.UpdateTexture(this.texture);
			SingletonMonoBehaviour<FadeCamera>.Instance.UpdateMaskTexture(this.startMask);
			SingletonMonoBehaviour<FadeCamera>.Instance.FadeOut(this.fadeinTime, delegate
			{
				Application.LoadLevel(this.nextScene);
				SingletonMonoBehaviour<FadeCamera>.Instance.UpdateMaskTexture(this.endMask);
				SingletonMonoBehaviour<FadeCamera>.Instance.FadeIn(this.fadeoutTime, null);
			});
		}
	}
}
