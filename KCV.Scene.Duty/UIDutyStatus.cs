using Common.Enum;
using local.models;
using System;
using UnityEngine;

namespace KCV.Scene.Duty
{
	public class UIDutyStatus : MonoBehaviour
	{
		[SerializeField]
		private UISprite mProgress;

		[SerializeField]
		private UISprite[] mStars;

		[SerializeField]
		private UISprite mYousei;

		[SerializeField]
		private UISpriteAnimation mSpriteYouseiAnimation;

		[SerializeField]
		private UISprite mSprite_ClearLamp;

		private bool isYouseiAnimation;

		private bool isStarAnimation;

		public void Initialize(DutyModel dutyModel)
		{
			this.mProgress.spriteName = this.GetSpriteNameProgress(dutyModel.State, dutyModel.Progress);
			string spriteNameYouseiPrefix = this.GetSpriteNameYouseiPrefix(dutyModel.State, dutyModel.Progress);
			if (string.IsNullOrEmpty(spriteNameYouseiPrefix))
			{
				this.mYousei.spriteName = string.Empty;
				this.mSpriteYouseiAnimation.namePrefix = string.Empty;
				this.mSpriteYouseiAnimation.framesPerSecond = 0;
			}
			else
			{
				this.mYousei.spriteName = string.Format(spriteNameYouseiPrefix, 1);
				this.mSpriteYouseiAnimation.namePrefix = spriteNameYouseiPrefix;
				this.mSpriteYouseiAnimation.framesPerSecond = 3;
			}
			if (dutyModel.State == QuestState.COMPLETE)
			{
				UISprite[] array = this.mStars;
				for (int i = 0; i < array.Length; i++)
				{
					UISprite component = array[i];
					this.mSprite_ClearLamp.SetActive(true);
					component.SetActive(true);
				}
			}
			else
			{
				UISprite[] array2 = this.mStars;
				for (int j = 0; j < array2.Length; j++)
				{
					UISprite component2 = array2[j];
					component2.SetActive(false);
				}
			}
		}

		private string GetSpriteNameYouseiPrefix(QuestState state, QuestProgressKinds progress)
		{
			switch (state)
			{
			case QuestState.WAITING_START:
				if (progress != QuestProgressKinds.NOT_DISP)
				{
					return string.Empty;
				}
				return string.Empty;
			case QuestState.RUNNING:
				return "mini_06_a_0";
			case QuestState.COMPLETE:
				return "mini_06_b_0";
			default:
				return string.Empty;
			}
		}

		private string GetSpriteNameProgress(QuestState state, QuestProgressKinds progress)
		{
			switch (state)
			{
			case QuestState.WAITING_START:
				return this.GetSpriteNameWaitingProgress(progress);
			case QuestState.RUNNING:
				return this.GetSpriteNameRunningProgress(progress);
			case QuestState.COMPLETE:
				return "btn_progress_8";
			default:
				return string.Empty;
			}
		}

		private string GetSpriteNameRunningProgress(QuestProgressKinds progress)
		{
			if (progress == QuestProgressKinds.MORE_THAN_50)
			{
				return "btn_progress_5";
			}
			if (progress != QuestProgressKinds.MORE_THAN_80)
			{
				return "btn_progress_4";
			}
			return "btn_progress_7";
		}

		private string GetSpriteNameWaitingProgress(QuestProgressKinds progress)
		{
			if (progress == QuestProgressKinds.MORE_THAN_50)
			{
				return "btn_progress_2";
			}
			if (progress != QuestProgressKinds.MORE_THAN_80)
			{
				return "btn_progress_1";
			}
			return "btn_progress_3";
		}
	}
}
