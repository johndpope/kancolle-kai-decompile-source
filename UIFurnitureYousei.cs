using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class UIFurnitureYousei : MonoBehaviour
{
	public enum YouseiType
	{
		Store,
		Room
	}

	[SerializeField]
	public Texture[] mTextures_RoomYouseiWalkFrames;

	[SerializeField]
	public Texture[] mTextures_StoreYouseiWalkFrames;

	[SerializeField]
	private UITexture mTexture_Yousei;

	private UIFurnitureYousei.YouseiType mYouseiType;

	private IEnumerator mWalkCoroutine;

	public float alpha
	{
		get
		{
			return this.mTexture_Yousei.alpha;
		}
		set
		{
			this.mTexture_Yousei.alpha = value;
		}
	}

	public void Initialize(UIFurnitureYousei.YouseiType type)
	{
		this.mYouseiType = type;
		this.mTexture_Yousei.alpha = 1f;
	}

	public void StartWalk()
	{
		if (this.mWalkCoroutine != null)
		{
			base.StopCoroutine(this.mWalkCoroutine);
			this.mWalkCoroutine = null;
		}
		this.mWalkCoroutine = this.WalkCoroutine();
		base.StartCoroutine(this.mWalkCoroutine);
	}

	public void StopWalk()
	{
		if (this.mWalkCoroutine != null)
		{
			base.StopCoroutine(this.mWalkCoroutine);
		}
	}

	[DebuggerHidden]
	private IEnumerator WalkCoroutine()
	{
		UIFurnitureYousei.<WalkCoroutine>c__Iterator88 <WalkCoroutine>c__Iterator = new UIFurnitureYousei.<WalkCoroutine>c__Iterator88();
		<WalkCoroutine>c__Iterator.<>f__this = this;
		return <WalkCoroutine>c__Iterator;
	}

	private void OnFrameChange(UIFurnitureYousei.YouseiType youseiType, int frameCount)
	{
		if (youseiType != UIFurnitureYousei.YouseiType.Store)
		{
			if (youseiType == UIFurnitureYousei.YouseiType.Room)
			{
				this.mTexture_Yousei.mainTexture = this.mTextures_RoomYouseiWalkFrames[frameCount];
				this.mTexture_Yousei.SetDimensions(242, 266);
			}
		}
		else
		{
			this.mTexture_Yousei.mainTexture = this.mTextures_StoreYouseiWalkFrames[frameCount];
			this.mTexture_Yousei.SetDimensions(246, 262);
		}
	}

	public void Release()
	{
		if (this.mWalkCoroutine != null)
		{
			base.StopCoroutine(this.mWalkCoroutine);
		}
		for (int i = 0; i < this.mTextures_RoomYouseiWalkFrames.Length; i++)
		{
			this.mTextures_RoomYouseiWalkFrames[i] = null;
		}
		for (int j = 0; j < this.mTextures_StoreYouseiWalkFrames.Length; j++)
		{
			this.mTextures_StoreYouseiWalkFrames[j] = null;
		}
		this.mTextures_RoomYouseiWalkFrames = null;
		this.mTextures_StoreYouseiWalkFrames = null;
		this.mTexture_Yousei = null;
		this.mWalkCoroutine = null;
	}
}
