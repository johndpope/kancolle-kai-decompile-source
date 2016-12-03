using System;
using UnityEngine;

namespace KCV.Remodel
{
	public class PortUpgradesModernizeShipSparkle : MonoBehaviour
	{
		private const float S_VEL_MAX = 2f;

		private const float S_VEL_MIN = 0.8f;

		private const float SCALE_MAX = 1f;

		private const float SCALE_MIN = 0.75f;

		private PortUpgradesModernizeShipManager manager;

		private UISprite sprite;

		private bool on;

		private float sVel;

		private float sMax;

		private void Awake()
		{
			this.sprite = base.GetComponent<UISprite>();
			if (this.sprite == null)
			{
				Debug.Log("UISprite.cs is not attached to .");
			}
			this.sprite.alpha = 0f;
			base.get_transform().set_localScale(new Vector3(0.01f, 0.01f, 1f));
			this.on = false;
			this.sVel = 0f;
			this.sMax = 0f;
		}

		private void Update()
		{
			if (this.on)
			{
				Transform expr_11 = base.get_transform();
				expr_11.set_localScale(expr_11.get_localScale() + new Vector3(this.sVel * Time.get_deltaTime(), this.sVel * Time.get_deltaTime(), 0f));
				if (base.get_transform().get_localScale().x >= this.sMax)
				{
					this.sVel = Mathf.Min(-this.sVel, this.sVel);
				}
				this.sprite.alpha = Mathf.Min(base.get_transform().get_localScale().x / this.sMax, 1f);
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

		public void Initialize()
		{
			this.on = true;
			this.sVel = (float)App.rand.NextDouble() * 1.2f + 0.8f;
			this.sMax = (float)App.rand.NextDouble() * 0.25f + 0.75f;
			Object.Destroy(base.get_gameObject(), 2f * this.sVel * this.sMax);
		}
	}
}
