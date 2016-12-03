using System;
using UnityEngine;

public class UITweenDebug : MonoBehaviour
{
	[Button("PlayTweenPosition", "PlayTweenPosition Forward", new object[]
	{
		true
	})]
	public int button1;

	[Button("PlayTweenPosition", "PlayTweenPosition Reverse", new object[]
	{
		false
	})]
	public int button11;

	[Button("PlayTweenScale", "PlayTweenScale Forward", new object[]
	{
		true
	})]
	public int button2;

	[Button("PlayTweenScale", "PlayTweenScale Reverse", new object[]
	{
		false
	})]
	public int button22;

	[Button("PlayTweenRotation", "PlayTweenRotation Forward", new object[]
	{
		true
	})]
	public int button3;

	[Button("PlayTweenRotation", "PlayTweenRotation Reverse", new object[]
	{
		false
	})]
	public int button33;

	[Button("PlayTweenAlpha", "PlayTweenAlpha Forward", new object[]
	{
		true
	})]
	public int button4;

	[Button("PlayTweenAlpha", "PlayTweenAlpha Reverse", new object[]
	{
		false
	})]
	public int button44;

	[Button("AllPlay", "AllPlay Forward", new object[]
	{
		true
	})]
	public int button5;

	[Button("AllPlay", "AllPlay Reverse", new object[]
	{
		false
	})]
	public int button55;

	public void PlayTweenPosition(bool isForward)
	{
		TweenPosition component = base.GetComponent<TweenPosition>();
		this.PlayTween(component, isForward);
	}

	public void PlayTweenScale(bool isForward)
	{
		TweenScale component = base.GetComponent<TweenScale>();
		this.PlayTween(component, isForward);
	}

	public void PlayTweenRotation(bool isForward)
	{
		TweenRotation component = base.GetComponent<TweenRotation>();
		this.PlayTween(component, isForward);
	}

	public void PlayTweenAlpha(bool isForward)
	{
		TweenAlpha component = base.GetComponent<TweenAlpha>();
		this.PlayTween(component, isForward);
	}

	public void AllPlay(bool isForward)
	{
		this.PlayTweenPosition(isForward);
		this.PlayTweenRotation(isForward);
		this.PlayTweenScale(isForward);
		this.PlayTweenAlpha(isForward);
	}

	private void PlayTween(UITweener tw, bool isForward)
	{
		if (tw != null)
		{
			tw.Play(isForward);
			tw.ResetToBeginning();
		}
	}
}
