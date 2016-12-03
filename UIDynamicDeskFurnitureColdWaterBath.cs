using KCV.Furniture;
using System;
using UnityEngine;

public class UIDynamicDeskFurnitureColdWaterBath : UIDynamicFurniture
{
	[SerializeField]
	private UISpriteAnimation mSpriteAnimation_Kouhyoteki;

	protected override void OnAwake()
	{
		this.mSpriteAnimation_Kouhyoteki.Pause();
		this.mSpriteAnimation_Kouhyoteki.set_enabled(false);
		this.mSpriteAnimation_Kouhyoteki.framesPerSecond = 0;
	}

	protected override void OnCalledActionEvent()
	{
		if (!this.mSpriteAnimation_Kouhyoteki.isPlaying)
		{
			this.mSpriteAnimation_Kouhyoteki.framesPerSecond = 6;
			this.mSpriteAnimation_Kouhyoteki.set_enabled(true);
			this.mSpriteAnimation_Kouhyoteki.ResetToBeginning();
			this.mSpriteAnimation_Kouhyoteki.Play();
		}
	}

	protected override void OnDestroyEvent()
	{
		base.OnDestroyEvent();
		this.mSpriteAnimation_Kouhyoteki = null;
	}
}
