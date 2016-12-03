using System;
using UnityEngine;

namespace KCV.Remodel
{
	public class MarriageSparkle : MonoBehaviour
	{
		private const float SCALE_MAX = 0.8f;

		private const float SCALE_MIN = 0.6f;

		private bool on;

		private float delay;

		private float iSca;

		private Vector3 iPos;

		private bool bobbing;

		[SerializeField]
		private UISprite sprite;

		public void Awake()
		{
			this.on = false;
			this.delay = 0f;
			this.iSca = 1f;
			this.bobbing = false;
			this.sprite.alpha = 0f;
		}

		public void Update()
		{
			if (this.on && Time.get_time() > this.delay)
			{
				this.sprite.alpha = Mathf.Max(0f, 0.25f + 0.5f * Mathf.Sin(3f * (Time.get_time() - this.delay)));
				base.get_transform().set_localScale(this.iSca * (0.85f + 0.15f * Mathf.Sin(12f * Time.get_time() - this.delay)) * Vector3.get_one());
				if (this.bobbing)
				{
					base.get_transform().set_localPosition(this.iPos + (-95f + 10f * (float)Math.Sin((double)(Time.get_time() * 1.2f))) * Vector3.get_up());
				}
			}
		}

		public void Initialize(bool bob)
		{
			this.bobbing = bob;
			if (bob)
			{
				this.iSca = Random.get_value() * 0.199999988f + 0.6f;
				this.iPos = 30f * Random.get_onUnitSphere();
				this.iPos = new Vector3(this.iPos.x, this.iPos.y, 0f);
				this.delay = Time.get_time() + Random.get_value() * 2f;
			}
			else
			{
				base.get_transform().set_localPosition(new Vector3(20f - 40f * Random.get_value(), -63f + 10f * Random.get_value(), 0f));
				this.iSca = Random.get_value() * 0.199999988f + 0.6f;
				this.delay = Time.get_time() + Random.get_value() * 0.25f;
			}
			this.on = true;
		}
	}
}
