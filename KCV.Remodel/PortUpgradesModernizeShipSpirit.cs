using System;
using UnityEngine;

namespace KCV.Remodel
{
	public class PortUpgradesModernizeShipSpirit : MonoBehaviour
	{
		private const float VEL = 200f;

		private const float VEL2 = 20f;

		private const float VEL_JIT_MAX = 20f;

		private const float VEL_JIT_MIN = 10f;

		private const float R_VEL = 150f;

		private const float R_VEL2 = 100f;

		private const float S_VEL = 0.2f;

		private PortUpgradesModernizeShipManager manager;

		private UISprite[] elements;

		private bool on;

		private Vector3 vel;

		private Vector3[] velJit;

		private void Awake()
		{
			this.elements = new UISprite[4];
			try
			{
				this.elements[0] = base.get_transform().Find("Red").GetComponent<UISprite>();
			}
			catch (NullReferenceException)
			{
				throw new NullReferenceException("./Red not found in PortUpgradesModernizeShipSpirit.cs");
			}
			if (this.elements[0] == null)
			{
				throw new NullReferenceException("UISprite.cs is not attached to ./Red");
			}
			try
			{
				this.elements[1] = base.get_transform().Find("Blue").GetComponent<UISprite>();
			}
			catch (NullReferenceException)
			{
				throw new NullReferenceException("./Blue not found in PortUpgradesModernizeShipSpirit.cs");
			}
			if (this.elements[1] == null)
			{
				throw new NullReferenceException("UISprite.cs is not attached to ./Blue");
			}
			try
			{
				this.elements[2] = base.get_transform().Find("Orange").GetComponent<UISprite>();
			}
			catch (NullReferenceException)
			{
				throw new NullReferenceException("./Orange not found in PortUpgradesModernizeShipSpirit.cs");
			}
			if (this.elements[2] == null)
			{
				throw new NullReferenceException("UISprite.cs is not attached to ./Orange");
			}
			try
			{
				this.elements[3] = base.get_transform().Find("Yellow").GetComponent<UISprite>();
			}
			catch (NullReferenceException)
			{
				throw new NullReferenceException("./Yellow not found in PortUpgradesModernizeShipSpirit.cs");
			}
			if (this.elements[3] == null)
			{
				throw new NullReferenceException("UISprite.cs is not attached to ./Yellow");
			}
			this.on = false;
			this.vel = Vector3.get_zero();
			this.velJit = new Vector3[4];
		}

		private void Update()
		{
			if (this.on)
			{
				if (Vector3.Magnitude(base.get_transform().get_localPosition()) <= 200f * Time.get_deltaTime())
				{
					Object.Destroy(base.get_gameObject());
				}
				else
				{
					base.get_transform().RotateAround(base.get_transform().get_parent().TransformPoint(0f, 0f, 0f), Vector3.get_forward(), 150f * Time.get_deltaTime());
					base.get_transform().set_localEulerAngles(new Vector3(0f, 0f, 0f));
					Transform expr_9A = base.get_transform();
					expr_9A.set_localPosition(expr_9A.get_localPosition() + 200f * Vector3.Normalize(-base.get_transform().get_localPosition()) * Time.get_deltaTime());
					for (int i = 0; i < 4; i++)
					{
						float num = Mathf.Atan2(this.elements[i].get_transform().get_localPosition().y, this.elements[i].get_transform().get_localPosition().x);
						Transform expr_121 = this.elements[i].get_transform();
						expr_121.set_localPosition(expr_121.get_localPosition() + new Vector3(Mathf.Sin(num), -Mathf.Cos(num), 0f) * 100f * Time.get_deltaTime());
						Transform expr_169 = this.elements[i].get_transform();
						expr_169.set_localPosition(expr_169.get_localPosition() + this.velJit[i] * Mathf.Sin(Time.get_time()) * Time.get_deltaTime());
						Transform expr_1B0 = this.elements[i].get_transform();
						expr_1B0.set_localPosition(expr_1B0.get_localPosition() + 20f * Vector3.Normalize(-this.elements[i].get_transform().get_localPosition()) * Time.get_deltaTime());
						Transform expr_1FD = this.elements[i].get_transform();
						expr_1FD.set_localScale(expr_1FD.get_localScale() - new Vector3(0.2f * Time.get_deltaTime(), 0.2f * Time.get_deltaTime(), 0f));
						if (Vector3.Magnitude(base.get_transform().get_localPosition()) < 200f)
						{
							this.elements[i].alpha -= Mathf.Min(Time.get_deltaTime(), this.elements[i].alpha);
						}
					}
				}
			}
		}

		public void SetManagerReference()
		{
			try
			{
				this.manager = base.get_transform().get_parent().get_parent().GetComponent<PortUpgradesModernizeShipManager>();
			}
			catch (NullReferenceException)
			{
				Debug.Log("../.. not found in PortUpgradesModernizeShipSparkle.cs");
			}
			if (this.manager == null)
			{
				Debug.Log("PortUpgradesModernizeShipManager.cs is not attached to ../..");
			}
		}

		public void Initialize(bool fire, bool torp, bool aaa, bool armor)
		{
			this.on = true;
			for (int i = 0; i < 4; i++)
			{
				float num = (float)App.rand.NextDouble() * 10f + 10f;
				int num2 = App.rand.Next(360);
				this.velJit[i] = num * new Vector3(Mathf.Cos((float)num2), Mathf.Sin((float)num2), 0f);
			}
			if (!fire)
			{
				this.elements[0].alpha = 0f;
			}
			if (!torp)
			{
				this.elements[1].alpha = 0f;
			}
			if (!aaa)
			{
				this.elements[2].alpha = 0f;
			}
			if (!armor)
			{
				this.elements[3].alpha = 0f;
			}
		}
	}
}
