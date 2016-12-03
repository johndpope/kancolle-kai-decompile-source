using KCV.Furniture;
using KCV.Scene.Port;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class UIDynamicDeskFurnitureShootingGallery : UIDynamicFurniture
{
	private const float RELOCATION_INTERVAL_SECONDS = 2f;

	[SerializeField]
	private UISprite mSprite;

	[SerializeField]
	private UISpriteAnimation mSpriteAnimation_Cat;

	private bool mIsWaitingRelocation;

	protected override void OnAwake()
	{
		this.mSpriteAnimation_Cat.Pause();
		this.mSpriteAnimation_Cat.set_enabled(false);
		this.mSpriteAnimation_Cat.framesPerSecond = 0;
		this.mSpriteAnimation_Cat.SetOnFinishedAnimationListener(new Action(this.OnFinishedAnimation));
	}

	private void OnFinishedAnimation()
	{
		this.mSprite.alpha = 1E-05f;
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
	}

	protected override void OnCalledActionEvent()
	{
		if (!this.mSpriteAnimation_Cat.isPlaying && !this.mIsWaitingRelocation)
		{
			this.mSpriteAnimation_Cat.framesPerSecond = 12;
			this.mSpriteAnimation_Cat.set_enabled(true);
			this.mSpriteAnimation_Cat.ResetToBeginning();
			this.mSpriteAnimation_Cat.Play();
			IEnumerator enumerator = this.RelocationIntervalCoroutine();
			base.StartCoroutine(enumerator);
		}
	}

	[DebuggerHidden]
	private IEnumerator RelocationIntervalCoroutine()
	{
		UIDynamicDeskFurnitureShootingGallery.<RelocationIntervalCoroutine>c__Iterator60 <RelocationIntervalCoroutine>c__Iterator = new UIDynamicDeskFurnitureShootingGallery.<RelocationIntervalCoroutine>c__Iterator60();
		<RelocationIntervalCoroutine>c__Iterator.<>f__this = this;
		return <RelocationIntervalCoroutine>c__Iterator;
	}

	protected override void OnDestroyEvent()
	{
		base.OnDestroyEvent();
		UserInterfacePortManager.ReleaseUtils.Release(ref this.mSprite);
		this.mSpriteAnimation_Cat = null;
	}
}
