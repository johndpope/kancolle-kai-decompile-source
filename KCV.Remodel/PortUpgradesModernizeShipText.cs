using System;
using UnityEngine;

namespace KCV.Remodel
{
	public class PortUpgradesModernizeShipText : MonoBehaviour
	{
		private PortUpgradesModernizeShipManager manager;

		private UILabel label;

		private string text;

		private float speed;

		private float left;

		private bool texting;

		private int pos;

		private float timer;

		public void Awake()
		{
			try
			{
				this.manager = base.get_transform().get_parent().get_parent().GetComponent<PortUpgradesModernizeShipManager>();
			}
			catch (NullReferenceException)
			{
				Debug.Log("../.. not found in PortUpgradesModernizeShipText.cs");
			}
			if (this.manager == null)
			{
				Debug.Log("PortUpgradesModernizeShipManager.cs is not attached to ../..");
			}
			this.label = base.GetComponent<UILabel>();
			if (this.label == null)
			{
				Debug.Log("UILabel.cs is not attached to .");
			}
			this.label.alpha = 0f;
			this.text = string.Empty;
			this.speed = 0f;
			this.left = base.get_transform().get_localPosition().x;
			this.texting = false;
			this.pos = 0;
			this.timer = 0f;
		}

		public void Update()
		{
			if (this.texting)
			{
				if (this.pos >= this.text.get_Length())
				{
					this.texting = false;
				}
				else
				{
					this.timer += Time.get_deltaTime();
					if (this.timer > this.speed)
					{
						UILabel expr_56 = this.label;
						expr_56.text += this.text.get_Chars(this.pos);
						base.get_transform().set_localPosition(new Vector3(this.left, base.get_transform().get_localPosition().y, base.get_transform().get_localPosition().z));
						this.pos++;
						this.timer -= this.speed;
					}
				}
			}
		}

		public void Initialize(string text, float speed, int width)
		{
			this.label.alpha = 1f;
			this.text = text;
			this.speed = speed;
			this.label.width = width;
		}

		public void Reset()
		{
			this.label.text = string.Empty;
			this.pos = 0;
			this.texting = false;
		}

		public void Text()
		{
			this.label.text = string.Empty;
			this.pos = 0;
			this.texting = true;
		}

		private void OnDestroy()
		{
			this.manager = null;
			this.label = null;
		}
	}
}
