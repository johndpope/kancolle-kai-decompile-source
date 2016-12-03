using System;
using UnityEngine;

namespace KCV
{
	public class StrategyTopText : MonoBehaviour
	{
		private UILabel label;

		private UILabel labelDup;

		private UILabel labelFade;

		public string text;

		public float speed;

		public bool texting;

		public int pos;

		public float timer;

		public void Awake()
		{
			this.label = base.get_gameObject().GetComponent<UILabel>();
			if (this.label == null)
			{
				Debug.Log("Warning: script not attached");
			}
			try
			{
				this.labelDup = base.get_gameObject().get_transform().get_parent().Find("OperationDetailsText2").GetComponent<UILabel>();
			}
			catch (Exception)
			{
				Debug.Log("Warning: OperationDetailsText2 not found");
			}
			if (this.labelDup == null)
			{
				Debug.Log("Warning: script not attached");
			}
			try
			{
				this.labelFade = base.get_gameObject().get_transform().get_parent().Find("OperationDetailsText3").GetComponent<UILabel>();
			}
			catch (Exception)
			{
				Debug.Log("Warning: OperationDetailsText3 not found");
			}
			if (this.labelFade == null)
			{
				Debug.Log("Warning: script not attached");
			}
			this.text = string.Empty;
			this.speed = 0.02f;
			this.texting = false;
			this.pos = 0;
			this.timer = 0f;
			this.label.alpha = 0f;
			this.labelDup.alpha = 0f;
			this.labelFade.alpha = 0f;
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
					while (this.timer > this.speed)
					{
						UILabel expr_4A = this.label;
						expr_4A.text += this.text.get_Chars(this.pos);
						UILabel expr_76 = this.labelDup;
						expr_76.text += this.text.get_Chars(this.pos);
						if (this.text.get_Chars(this.pos) != '\n')
						{
							this.timer -= this.speed;
						}
						this.pos++;
					}
					if (this.pos < this.text.get_Length() - 1)
					{
						this.labelFade.text = this.label.text + this.text.get_Chars(this.pos);
					}
					this.labelFade.alpha = this.timer / this.speed;
				}
			}
		}

		public void Reset()
		{
			this.label.text = string.Empty;
			this.labelDup.text = string.Empty;
			this.labelFade.text = string.Empty;
			this.pos = 0;
			this.texting = false;
		}

		public void Text(string s)
		{
			if (s.get_Length() == 0)
			{
				return;
			}
			this.label.alpha = 1f;
			this.labelDup.alpha = 1f;
			this.labelFade.alpha = 0f;
			this.text = s.Replace("\\n", "\n");
			this.label.text = string.Empty;
			this.labelDup.text = string.Empty;
			this.labelFade.text = string.Empty + s.get_Chars(0);
			this.pos = 0;
			this.texting = true;
		}

		public void Stop()
		{
			this.texting = false;
			this.labelFade.alpha = this.label.alpha;
			this.label.alpha = 0f;
			this.labelDup.alpha = 0f;
			iTween.ValueTo(base.get_gameObject(), iTween.Hash(new object[]
			{
				"from",
				this.labelFade.alpha,
				"to",
				0,
				"time",
				0.2f,
				"onupdate",
				"TextAlpha",
				"onupdatetarget",
				base.get_gameObject()
			}));
		}

		public void TextAlpha(float f)
		{
			this.labelFade.alpha = f;
		}
	}
}
