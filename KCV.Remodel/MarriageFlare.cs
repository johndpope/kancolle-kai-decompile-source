using System;
using UnityEngine;

namespace KCV.Remodel
{
	public class MarriageFlare : MonoBehaviour
	{
		private const float SCALE_MAX = 1f;

		private const float SCALE_MIN = 0.6f;

		private const float A_VEL_MAX = 2.5f;

		private const float A_VEL_MIN = 1.5f;

		private const int TYPES = 4;

		private bool on;

		private float delay;

		private float speed;

		[SerializeField]
		private UISprite sprite;

		private readonly string[] NAMES = new string[]
		{
			"WhiteLight_2",
			"WhiteLight_3",
			"WhiteLight_4",
			"WhiteLight_7"
		};

		public void Awake()
		{
			this.on = false;
			this.delay = 0f;
			this.speed = 0f;
			this.sprite.alpha = 0f;
		}

		public void Update()
		{
		}

		public void Initialize()
		{
			this.on = true;
			this.Loop();
		}

		public void Loop()
		{
			this.sprite.spriteName = this.NAMES[(int)(Random.get_value() * 4f)];
			this.sprite.MakePixelPerfect();
			this.delay = Random.get_value() * 4f;
			this.speed = Random.get_value() * 1f + 1.5f;
			float num = Random.get_value() * 0.399999976f + 0.6f;
			base.get_transform().set_localScale(new Vector3(num, num, 1f));
			base.get_transform().set_localPosition(new Vector3(430f - 860f * Random.get_value(), 222f - 444f * Random.get_value(), 0f));
			base.get_transform().Rotate(Random.get_value() * 359.99f * Vector3.get_forward());
			iTween.ValueTo(base.get_gameObject(), iTween.Hash(new object[]
			{
				"from",
				0,
				"to",
				1,
				"time",
				this.speed / 2f,
				"delay",
				this.delay,
				"onupdate",
				"Alpha",
				"onupdatetarget",
				base.get_gameObject()
			}));
			iTween.ValueTo(base.get_gameObject(), iTween.Hash(new object[]
			{
				"from",
				1,
				"to",
				0,
				"time",
				this.speed / 2f,
				"delay",
				this.delay + this.speed / 2f,
				"onupdate",
				"Alpha",
				"onupdatetarget",
				base.get_gameObject(),
				"oncomplete",
				"Loop",
				"oncompletetarget",
				base.get_gameObject()
			}));
		}

		public void Alpha(float f)
		{
			this.sprite.alpha = f;
		}
	}
}
