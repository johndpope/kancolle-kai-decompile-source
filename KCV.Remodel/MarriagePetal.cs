using System;
using UnityEngine;

namespace KCV.Remodel
{
	public class MarriagePetal : MonoBehaviour
	{
		private const float I_VEL_MAX = 300f;

		private const float I_VEL_MIN = 200f;

		private const float I_R_VEL_MAX = 70f;

		private const float I_R_VEL_MIN = 50f;

		private const float SCALE_MAX = 1f;

		private const float SCALE_MIN = 0.8f;

		private const int TYPES = 2;

		private bool on;

		private float delay;

		private bool loop;

		[SerializeField]
		private UISprite sprite;

		private readonly string[] NAMES = new string[]
		{
			"Petal_1",
			"Petal_2"
		};

		private Vector3 vel;

		private float rVel;

		private Vector3 rPivot;

		public void Awake()
		{
			this.on = false;
			this.delay = 0f;
			this.loop = false;
		}

		public void Update()
		{
			if (this.on && Time.get_time() > this.delay)
			{
				Transform expr_21 = base.get_transform();
				expr_21.set_localPosition(expr_21.get_localPosition() + (this.vel + 0.35f * Mathf.Sin(2f * Time.get_time()) * Vector3.Magnitude(this.vel) * Vector3.get_left()) * Time.get_deltaTime());
				base.get_transform().Rotate(this.rPivot, this.rVel * Time.get_deltaTime() * Mathf.Sin(6.28318548f * Time.get_time()));
				if (base.get_transform().get_localPosition().x < -550f || base.get_transform().get_localPosition().y < -350f)
				{
					if (!this.loop)
					{
						Object.Destroy(base.get_gameObject());
					}
					else
					{
						base.get_transform().set_localPosition(new Vector3(500f, 50f + 300f * Random.get_value(), 0f));
					}
				}
			}
		}

		public void Initialize(bool loop)
		{
			this.on = true;
			this.sprite.spriteName = this.NAMES[(int)(Random.get_value() * 2f)];
			this.sprite.MakePixelPerfect();
			if (loop)
			{
				this.delay = Time.get_time() + Random.get_value() * 4f;
			}
			else
			{
				this.delay = Time.get_time() + Random.get_value() * 2f;
			}
			this.loop = loop;
			float num = Random.get_value() * 0.199999988f + 0.8f;
			base.get_transform().set_localScale(new Vector3(num, num, 1f));
			if (loop)
			{
				base.get_transform().set_localPosition(new Vector3(500f, 50f + 300f * Random.get_value(), 0f));
			}
			else
			{
				base.get_transform().set_localPosition(new Vector3(-100f + 300f * Random.get_value(), 300f, 0f));
			}
			base.get_transform().Rotate(Random.get_value() * 359.99f * Vector3.get_forward());
			num = Random.get_value() * 100f + 200f;
			float num2;
			if (loop)
			{
				num2 = (21f + Random.get_value()) * 3.14159274f / 18f;
			}
			else
			{
				num2 = (25f + Random.get_value()) * 3.14159274f / 18f;
			}
			this.vel = num * new Vector3(Mathf.Cos(num2), Mathf.Sin(num2), 0f);
			this.rVel = Random.get_value() * 20f + 50f;
			this.rPivot = Random.get_onUnitSphere();
		}
	}
}
