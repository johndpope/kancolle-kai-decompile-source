using Common.Enum;
using KCV.Scene.Port;
using local.models;
using System;
using UnityEngine;

public class UIDutyOpenDeckPracticeRewardGet : MonoBehaviour
{
	[SerializeField]
	private UILabel mLabel_Message;

	[SerializeField]
	private UITexture mTexture_PracticeImage;

	private string DeckPracticeTypeToString(DeckPracticeType deckPracticeType)
	{
		switch (deckPracticeType)
		{
		case DeckPracticeType.Normal:
			return "通常";
		case DeckPracticeType.Hou:
			return "砲戦";
		case DeckPracticeType.Rai:
			return "雷撃";
		case DeckPracticeType.Taisen:
			return "対潜戦";
		case DeckPracticeType.Kouku:
			return "航空戦";
		case DeckPracticeType.Sougou:
			return "総合";
		default:
			return string.Empty;
		}
	}

	public void Initialize(Reward_DeckPracitce reward)
	{
		this.mLabel_Message.text = string.Format("{0}演習\nが開放されました！", this.DeckPracticeTypeToString(reward.type));
		this.mTexture_PracticeImage.mainTexture = this.RequestDeckPracticeImage(reward.type);
	}

	private Texture RequestDeckPracticeImage(DeckPracticeType deckPracticeType)
	{
		switch (deckPracticeType)
		{
		case DeckPracticeType.Hou:
			return Resources.Load<Texture>("Textures/Duty/open_img3");
		case DeckPracticeType.Rai:
			return Resources.Load<Texture>("Textures/Duty/open_img4");
		case DeckPracticeType.Taisen:
			return Resources.Load<Texture>("Textures/Duty/open_img5");
		case DeckPracticeType.Kouku:
			return Resources.Load<Texture>("Textures/Duty/open_img6");
		case DeckPracticeType.Sougou:
			return Resources.Load<Texture>("Textures/Duty/open_img7");
		default:
			return null;
		}
	}

	private void OnDestroy()
	{
		UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabel_Message);
		UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_PracticeImage, false);
	}
}
