using System;
using UnityEngine;

namespace KCV.Remodel
{
	public class PortUpgradesConvertShipSnowflake : MonoBehaviour
	{
		private const float I_VEL_MAX = 400f;

		private const float I_VEL_MIN = 150f;

		private const float I_R_VEL_MAX = 200f;

		private const float I_R_VEL_MIN = 100f;

		private const float GRAV = -500f;

		private const float SCALE_MAX = 0.75f;

		private const float SCALE_MIN = 0.02f;

		private const float S_VEL = 0.6f;

		private PortUpgradesConvertShipManager manager;

		private bool on;

		private Vector3 vel;

		private float rVel;

		private void Awake()
		{
			this.on = false;
			this.vel = Vector3.get_zero();
			this.rVel = 0f;
		}

		private void Update()
		{
			if (this.on)
			{
				Transform expr_11 = base.get_transform();
				expr_11.set_localPosition(expr_11.get_localPosition() + this.vel * Time.get_deltaTime());
				if (base.get_transform().get_localPosition().x > 580f || base.get_transform().get_localPosition().x < -580f || base.get_transform().get_localPosition().y < -372f)
				{
					Object.Destroy(base.get_gameObject());
				}
				else
				{
					Transform expr_9E = base.get_transform();
					expr_9E.set_localScale(expr_9E.get_localScale() + new Vector3(0.6f * Time.get_deltaTime(), 0.6f * Time.get_deltaTime(), 0f));
					this.vel += new Vector3(0f, -500f, 0f) * Time.get_deltaTime();
					base.get_transform().Rotate(this.rVel * Vector3.get_forward() * Time.get_deltaTime());
				}
			}
		}

		public void SetManagerReference()
		{
			try
			{
				this.manager = base.get_transform().get_parent().get_parent().GetComponent<PortUpgradesConvertShipManager>();
			}
			catch (NullReferenceException)
			{
				Debug.Log("../.. not found in PortUpgradesConvertShipSnowflake.cs");
			}
			if (this.manager == null)
			{
				Debug.Log("PortUpgradesConvertShipManager.cs is not attached to ../..");
			}
		}

		public void Initialize()
		{
			this.on = true;
			float num = (float)App.rand.NextDouble() * 0.73f + 0.02f;
			base.get_transform().set_localScale(new Vector3(num, num, 1f));
			base.get_transform().set_localPosition(new Vector3(0f, 200f, 0f));
			base.get_transform().Rotate((float)App.rand.Next(360) * Vector3.get_forward());
			int num2 = App.rand.Next(360);
			num = (float)App.rand.NextDouble() * 250f + 150f;
			this.vel = num * new Vector3(Mathf.Cos((float)num2), Mathf.Sin((float)num2), 0f);
			this.rVel = (float)App.rand.NextDouble() * 100f + 100f;
		}
	}
}
