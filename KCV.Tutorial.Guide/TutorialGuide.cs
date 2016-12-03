using KCV.Strategy;
using System;
using UnityEngine;

namespace KCV.Tutorial.Guide
{
	public class TutorialGuide : MonoBehaviour
	{
		public int tutorialID;

		public UILabel[] Text;

		private Transform Number;

		private Transform TutorialTex;

		private UILabel Title;

		private UILabel MainText;

		private static readonly Color32 MainTextColor = new Color32(227, 227, 227, 255);

		private static readonly Color32 TitleTextColor = new Color32(255, 255, 255, 255);

		private static readonly Vector3 CatPos = new Vector3(45f, -4f, 0f);

		private void Start()
		{
			if (SingletonMonoBehaviour<PortObjectManager>.exist())
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.SetTutorialGuide(this);
			}
			this.tutorialID = -1;
			Transform transform = base.get_transform().FindChild("TutorialGuide");
			transform.FindChild("TutorialTex").set_localPosition(new Vector3(62f, 88f, 0f));
			Transform transform2 = transform.FindChild("TutorialNumber");
			if (transform2 != null)
			{
				transform2.SetActive(false);
			}
			Util.InstantiatePrefab("StrategyPrefab/StepTutorialYousei", transform.get_gameObject(), false);
			this.Number = base.get_transform().FindChild("TutorialGuide/TutorialNumber");
			this.TutorialTex = base.get_transform().FindChild("TutorialGuide/TutorialTex");
			Transform transform3 = base.get_transform().FindChild("TutorialGuide/Title");
			if (transform3 != null)
			{
				this.Title = base.get_transform().FindChild("TutorialGuide/Title").GetComponent<UILabel>();
				this.Title.color = TutorialGuide.TitleTextColor;
				this.Title.get_transform().localPositionY(36f);
			}
			this.MainText = base.get_transform().FindChild("TutorialGuide/Label").GetComponent<UILabel>();
			this.MainText.color = TutorialGuide.MainTextColor;
			this.Number.SetActive(false);
			this.TutorialTex.get_transform().localPositionX(63f);
			this.TutorialTex.get_transform().localPositionY(88f);
			Transform transform4 = base.get_transform().FindChild("TutorialGuide/ArrowAchor/Arrow");
			UISprite uISprite = (!(transform4 != null)) ? null : transform4.GetComponent<UISprite>();
			uISprite.MakePixelPerfect();
			GameObject gameObject = new GameObject();
			gameObject.get_transform().set_parent(uISprite.get_transform());
			UISprite uISprite2 = gameObject.AddComponent<UISprite>();
			uISprite2.atlas = uISprite.atlas;
			uISprite2.spriteName = "tutorial_cat";
			uISprite2.MakePixelPerfect();
			uISprite2.get_transform().set_localPosition(TutorialGuide.CatPos);
		}

		public void Show()
		{
			if (SingletonMonoBehaviour<TutorialGuideManager>.exist() && SingletonMonoBehaviour<TutorialGuideManager>.Instance.model != null && this.tutorialID != -1 && SingletonMonoBehaviour<TutorialGuideManager>.Instance.model.GetStepTutorialFlg(this.tutorialID))
			{
				Object.Destroy(base.get_gameObject());
			}
			else
			{
				if (StrategyTopTaskManager.Instance != null && !StrategyTopTaskManager.GetSailSelect().isRun)
				{
					return;
				}
				TweenAlpha.Begin(base.get_gameObject(), 0.5f, 1f);
			}
		}

		public TweenAlpha Hide()
		{
			TweenAlpha tweenAlpha = TweenAlpha.Begin(base.get_gameObject(), 0.2f, 0f);
			tweenAlpha.ResetToBeginning();
			return tweenAlpha;
		}

		public void HideAndDestroy()
		{
			this.Hide().SetOnFinished(delegate
			{
				Object.Destroy(base.get_gameObject());
			});
		}

		public void InitText()
		{
			for (int i = 1; i < this.Text.Length; i++)
			{
				this.Text[i].alpha = 0f;
			}
			this.Text[0].alpha = 1f;
		}

		private void OnDestroy()
		{
			Mem.DelAry<UILabel>(ref this.Text);
		}
	}
}
