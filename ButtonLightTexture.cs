using System;
using UnityEngine;

public class ButtonLightTexture : MonoBehaviour
{
	private UITexture tex;

	private TweenAlpha ta;

	[Button("PlayAnim", "PlayAnim", new object[]
	{

	})]
	public int button1;

	[Button("StopAnim", "StopAnim", new object[]
	{

	})]
	public int button2;

	private bool _isPlay;

	public bool NowPlay()
	{
		return this._isPlay;
	}

	private void Awake()
	{
		this.tex = base.GetComponent<UITexture>();
		this.tex.alpha = 0f;
		this.ta = base.GetComponent<TweenAlpha>();
		this.ta.set_enabled(false);
		this._isPlay = false;
	}

	private void OnDestroy()
	{
		Mem.Del<UITexture>(ref this.tex);
		Mem.Del<TweenAlpha>(ref this.ta);
		Mem.Del<bool>(ref this._isPlay);
	}

	public void PlayAnim()
	{
		this.ta.set_enabled(true);
		this.ta.ResetToBeginning();
		this.ta.PlayForward();
		this._isPlay = true;
	}

	public void StopAnim()
	{
		this.ta.set_enabled(false);
		this.tex.alpha = 0f;
		this._isPlay = false;
	}
}
