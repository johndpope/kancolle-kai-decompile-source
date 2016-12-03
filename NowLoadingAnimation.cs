using System;
using UnityEngine;

public class NowLoadingAnimation : SingletonMonoBehaviour<NowLoadingAnimation>
{
	[SerializeField]
	private Camera myCamera;

	[SerializeField]
	private Transform Anchor;

	[SerializeField]
	private NowLoadingText textAnimation;

	private NowLoadingYousei yousei;

	[Button("StartAnimation", "StartAnimation", new object[]
	{
		1
	})]
	public int button1;

	[Button("EndAnimation", "EndAnimation", new object[]
	{

	})]
	public int button2;

	public bool isNowLoadingAnimation;

	public bool isYouseiExist
	{
		get
		{
			return this.yousei != null;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		if (this.myCamera != null)
		{
			this.myCamera.SetActive(false);
		}
		this.Anchor.SetActive(false);
	}

	public void StartTextAnimation()
	{
	}

	public void StartAnimation(int youseiNo)
	{
		if (this.isNowLoadingAnimation)
		{
			this.yousei = Util.Instantiate(Resources.Load("Prefabs/Loading/NowLoadingPrefabs/NowLoadingYousei_" + youseiNo), this.Anchor.get_gameObject(), false, false).GetComponent<NowLoadingYousei>();
			this.myCamera.SetActive(true);
			this.Anchor.SetActive(true);
			this.textAnimation.StartAnimation();
		}
		else
		{
			this.CreateNotAnimation(youseiNo);
		}
	}

	public void CreateNotAnimation(int youseiNo)
	{
		this.yousei = Util.Instantiate(Resources.Load("Prefabs/Loading/NowLoadingPrefabs/NowLoadingYousei_" + youseiNo), this.Anchor.get_gameObject(), false, false).GetComponent<NowLoadingYousei>();
		this.myCamera.SetActive(true);
		this.Anchor.SetActive(true);
		this.textAnimation.StopAnimation();
	}

	public void EndAnimation()
	{
		if (this.yousei != null)
		{
			Object.Destroy(this.yousei.get_gameObject());
		}
		this.yousei = null;
		this.textAnimation.StopAnimation();
		this.myCamera.SetActive(false);
		this.Anchor.SetActive(false);
	}

	public void Hide()
	{
		if (this.myCamera != null)
		{
			this.myCamera.SetActive(false);
		}
		this.Anchor.SetActive(false);
	}

	private void OnDestroy()
	{
		this.myCamera = null;
		this.Anchor = null;
		this.textAnimation = null;
	}
}
