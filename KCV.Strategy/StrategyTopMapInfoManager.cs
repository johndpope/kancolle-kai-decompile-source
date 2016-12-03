using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyTopMapInfoManager : MonoBehaviour
	{
		private UISprite mSprite_Background;

		private UISprite[] mSprites_Item;

		private UILabel mLabel_OperationTitle;

		private UISprite mSprite_Arrow;

		private UISprite mSprite_ArrowGlow;

		private GameObject mGameObject_Button;

		private UISprite mSprite_ButtonBack;

		private UISprite mSprite_ButtonCircle;

		private UISprite mSprite_ButtonFront;

		private UISprite mSprite_ButtonText;

		private int iCnt;

		private bool locked;

		private float timer;

		[SerializeField]
		private TypewriterEffect TypeWriter;

		private void Awake()
		{
			this.mSprite_Background = base.get_transform().Find("BG").GetComponent<UISprite>();
			this.mSprites_Item = new UISprite[4];
			for (int i = 0; i < 4; i++)
			{
				this.mSprites_Item[i] = base.get_transform().Find("BG/Item" + (i + 1)).GetComponent<UISprite>();
			}
			this.mLabel_OperationTitle = base.get_transform().Find("OperationText").GetComponent<UILabel>();
			this.mSprite_Arrow = base.get_transform().Find("Arrow").GetComponent<UISprite>();
			this.mSprite_ArrowGlow = base.get_transform().Find("ArrowGlow").GetComponent<UISprite>();
			this.mGameObject_Button = base.get_transform().Find("Btn").get_gameObject();
			this.mSprite_ButtonBack = this.mGameObject_Button.get_transform().Find("Back").GetComponent<UISprite>();
			this.mSprite_ButtonCircle = this.mGameObject_Button.get_transform().Find("Circle").GetComponent<UISprite>();
			this.mSprite_ButtonFront = this.mGameObject_Button.get_transform().Find("Front").GetComponent<UISprite>();
			this.mSprite_ButtonText = this.mGameObject_Button.get_transform().Find("Text").GetComponent<UISprite>();
			this.mSprite_Background.get_transform().set_localScale(new Vector3(0.0001f, 0.0001f, 1f));
			this.mSprite_Background.alpha = 0f;
			this.mSprites_Item[0].alpha = 0f;
			this.mSprites_Item[1].alpha = 0f;
			this.mSprites_Item[2].alpha = 0f;
			this.mSprites_Item[3].alpha = 0f;
			this.mLabel_OperationTitle.alpha = 0f;
			this.mSprite_Arrow.alpha = 0f;
			this.mSprite_ArrowGlow.alpha = 0f;
			this.mGameObject_Button.get_transform().set_localScale(new Vector3(0.0001f, 0.0001f, 1f));
			this.mSprite_ButtonBack.alpha = 0f;
			this.mSprite_ButtonCircle.alpha = 0f;
			this.mSprite_ButtonFront.alpha = 0f;
			this.mSprite_ButtonText.alpha = 0f;
			this.iCnt = 0;
			this.locked = true;
			this.timer = 0f;
			this.TypeWriter.set_enabled(false);
		}

		private void Update()
		{
			if (!this.locked)
			{
				if (this.mSprite_ButtonBack.alpha > 0f)
				{
					this.mSprite_ButtonCircle.get_transform().Rotate(-20f * Vector3.get_forward() * Time.get_deltaTime());
				}
				if (this.mSprite_Arrow.alpha == 1f)
				{
					if (this.timer == 0f)
					{
						this.timer = Time.get_time() - 4.712389f;
					}
					this.mSprite_ArrowGlow.alpha = 0.5f + 0.5f * Mathf.Sin(4f * (Time.get_time() - this.timer));
				}
			}
		}

		public void Die(bool btnPress = false)
		{
			iTween.Stop(base.get_gameObject(), true);
			this.TypeWriter.get_gameObject().SetActive(false);
			iTween.ScaleTo(this.mSprite_Background.get_gameObject(), iTween.Hash(new object[]
			{
				"scale",
				new Vector3(0.0001f, 0.0001f, 1f),
				"time",
				0.25f,
				"easetype",
				iTween.EaseType.easeInOutQuad
			}));
			if (btnPress && this.mGameObject_Button.get_transform().get_localScale().x >= 0.8f)
			{
				iTween.ScaleTo(this.mGameObject_Button, iTween.Hash(new object[]
				{
					"scale",
					new Vector3(0.6f, 0.6f, 1f),
					"time",
					0.1f,
					"easetype",
					iTween.EaseType.easeInQuad
				}));
				iTween.ScaleTo(this.mGameObject_Button, iTween.Hash(new object[]
				{
					"scale",
					new Vector3(1f, 1f, 1f),
					"time",
					0.3f,
					"delay",
					0.1f,
					"easetype",
					iTween.EaseType.easeOutElastic
				}));
				iTween.ValueTo(base.get_gameObject(), iTween.Hash(new object[]
				{
					"from",
					this.mSprite_Background.alpha,
					"to",
					0,
					"time",
					0.2f,
					"delay",
					0.2f,
					"onupdate",
					"FadePopups",
					"onupdatetarget",
					base.get_gameObject()
				}));
				Object.Destroy(base.get_gameObject(), 0.45f);
			}
			else
			{
				iTween.ScaleTo(this.mGameObject_Button, iTween.Hash(new object[]
				{
					"scale",
					new Vector3(0.0001f, 0.0001f, 1f),
					"time",
					0.25f,
					"easetype",
					iTween.EaseType.easeInOutQuad
				}));
				iTween.ValueTo(base.get_gameObject(), iTween.Hash(new object[]
				{
					"from",
					this.mSprite_Background.alpha,
					"to",
					0,
					"time",
					0.1f,
					"delay",
					0.15f,
					"onupdate",
					"FadePopups",
					"onupdatetarget",
					base.get_gameObject()
				}));
				Object.Destroy(base.get_gameObject(), 0.3f);
			}
			this.mSprite_Arrow.get_gameObject().SetActive(false);
			this.mSprite_ArrowGlow.get_gameObject().SetActive(false);
			this.mLabel_OperationTitle.get_gameObject().SetActive(false);
		}

		public void FadePopups(float f)
		{
			this.mSprite_Background.alpha = f;
			this.mSprite_ButtonBack.alpha = f;
			this.mSprite_ButtonFront.alpha = f;
			if (!this.locked)
			{
				this.mSprite_ButtonCircle.alpha = f;
				this.mSprite_ButtonText.alpha = f;
			}
		}

		public void FadeInfo(float f)
		{
			this.mLabel_OperationTitle.alpha = f;
			for (int i = 0; i < this.iCnt; i++)
			{
				this.mSprites_Item[i].alpha = f;
			}
			this.mSprite_Arrow.alpha = f;
			if (this.mSprite_ArrowGlow.alpha > 0f)
			{
				this.mSprite_ArrowGlow.alpha /= 2f;
			}
		}

		[DebuggerHidden]
		public IEnumerator ShowPopups()
		{
			StrategyTopMapInfoManager.<ShowPopups>c__Iterator183 <ShowPopups>c__Iterator = new StrategyTopMapInfoManager.<ShowPopups>c__Iterator183();
			<ShowPopups>c__Iterator.<>f__this = this;
			return <ShowPopups>c__Iterator;
		}

		[DebuggerHidden]
		public IEnumerator StartText(string det)
		{
			StrategyTopMapInfoManager.<StartText>c__Iterator184 <StartText>c__Iterator = new StrategyTopMapInfoManager.<StartText>c__Iterator184();
			<StartText>c__Iterator.<>f__this = this;
			return <StartText>c__Iterator;
		}
	}
}
