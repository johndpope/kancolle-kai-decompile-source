using System;
using UnityEngine;

namespace KCV.Remodel
{
	public class MarriageFeather : MonoBehaviour
	{
		private const float I_R_VEL_MAX = 1.83259583f;

		private const float I_R_VEL_MIN = 1.308997f;

		private const float I_C_VEL_MAX = 0.15f;

		private const float I_C_VEL_MIN = 0.12f;

		private const float SCALE_MAX = 1f;

		private const float SCALE_MIN = 0.6f;

		private const float ALIGN_STRENGTH_MAX = 1.5f;

		private const float ALIGN_STRENGTH_MIN = 0.8f;

		private const int TYPES = 3;

		private bool on;

		private float startTime;

		private float delay;

		private Vector3 vel;

		private float rot;

		private float rVel;

		private float cVel;

		private float sca;

		private float alStr;

		[SerializeField]
		private UISprite sprite;

		[SerializeField]
		private Transform bgTrans;

		private Vector3 lastPos;

		private readonly string[] NAMES = new string[]
		{
			"Feather_1_hor",
			"Feather_2_hor",
			"Feather_3_hor"
		};

		public void Awake()
		{
			this.on = false;
			this.startTime = 0f;
			this.delay = 0f;
		}

		public void Update()
		{
			if (this.on && Time.get_time() > this.delay)
			{
				Vector3 vector = base.get_transform().get_localPosition() - this.bgTrans.get_localPosition() - Vector3.get_up() * 610f;
				Vector3 vector2 = new Vector3(vector.x * Mathf.Cos(this.rVel) - vector.y * Mathf.Sin(this.rVel), vector.x * Mathf.Sin(this.rVel) + vector.y * Mathf.Cos(this.rVel), 0f);
				Vector3 vector3 = -Mathf.Pow(vector.get_magnitude() * this.cVel, 1.4f) * vector.get_normalized();
				this.vel = vector3 + vector2;
				if (Time.get_time() > this.startTime + 3.5f)
				{
					this.vel *= Mathf.Sqrt(Time.get_time() - this.startTime - 2.5f);
				}
				this.rot = 57.2957764f * Mathf.Atan2(this.vel.y, this.vel.x) - base.get_transform().get_localEulerAngles().z;
				if (this.rot > 360f)
				{
					this.rot -= 360f;
				}
				else if (this.rot < 0f)
				{
					this.rot += 360f;
				}
				Transform expr_1A0 = base.get_transform();
				expr_1A0.set_localPosition(expr_1A0.get_localPosition() + (this.vel * Time.get_deltaTime() + (this.bgTrans.get_localPosition() - this.lastPos)));
				this.lastPos = this.bgTrans.get_localPosition();
				base.get_transform().Rotate(Vector3.get_forward(), this.rot * Time.get_deltaTime() * this.alStr);
				base.get_transform().set_localScale(Vector3.get_one() * this.sca * Mathf.Pow(vector.get_magnitude() / 600f, 0.65f));
			}
		}

		public void Initialize()
		{
			this.on = true;
			this.sprite.spriteName = this.NAMES[(int)(Random.get_value() * 3f)];
			this.sprite.MakePixelPerfect();
			this.startTime = Time.get_time();
			this.delay = Time.get_time() + Random.get_value() * 4.5f;
			this.sca = Random.get_value() * 0.399999976f + 0.6f;
			base.get_transform().set_localScale(new Vector3(this.sca, this.sca, 1f));
			this.lastPos = this.bgTrans.get_localPosition();
			base.get_transform().set_localPosition(new Vector3(-600f, -400f - 500f * Random.get_value(), 0f) + this.lastPos);
			base.get_transform().Rotate(Random.get_value() * 359.99f * Vector3.get_forward());
			this.rVel = Random.get_value() * 0.5235988f + 1.308997f;
			this.cVel = Random.get_value() * 0.0300000086f + 0.12f;
			this.alStr = Random.get_value() * 0.7f + 0.8f;
		}
	}
}
