using KCV.Scene.Port;
using local.models;
using System;
using UnityEngine;

public class UIDutyGetRewardUseItem : MonoBehaviour
{
	[SerializeField]
	private UISprite mSprite_Icon;

	[SerializeField]
	private UILabel mLabel_Message;

	public void Initialize(Reward_Useitem reward)
	{
		this.mSprite_Icon.spriteName = string.Format("item_{0}", reward.Id);
		bool flag = reward.Id == 63;
		if (flag)
		{
			this.mLabel_Message.text = string.Empty;
			this.mLabel_Message.fontSize = 24;
			this.mLabel_Message.text = string.Format("任務受託数がupしました！", reward.Name);
		}
		else if (reward.Count == 1)
		{
			this.mLabel_Message.text = string.Empty;
			this.mLabel_Message.fontSize = 32;
			this.mLabel_Message.text = string.Format("{0}を\n入手しました", reward.Name);
		}
		else
		{
			this.mLabel_Message.text = string.Empty;
			this.mLabel_Message.fontSize = 32;
			this.mLabel_Message.text = string.Format("{0}を\n{1}個 入手しました", reward.Name, reward.Count);
		}
	}

	private void OnDestroy()
	{
		UserInterfacePortManager.ReleaseUtils.Release(ref this.mSprite_Icon);
		UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabel_Message);
	}
}
