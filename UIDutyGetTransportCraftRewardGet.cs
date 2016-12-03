using KCV.Scene.Port;
using local.models;
using System;
using UnityEngine;

public class UIDutyGetTransportCraftRewardGet : MonoBehaviour
{
	[SerializeField]
	private UILabel mLabel_Message;

	public void Initialize(Reward_TransportCraft reward)
	{
		this.mLabel_Message.text = string.Format("輸送船 x {0}", reward.Num);
	}

	private void OnDestroy()
	{
		UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabel_Message);
	}
}
