using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class NowLoadingYousei : MonoBehaviour
{
	[SerializeField]
	private UISprite YouseiBody;

	[SerializeField]
	private UISprite YouseiFace;

	[SerializeField]
	private UISprite YouseiOption;

	public int YouseiNo;

	private float aye1Time = 0.7f;

	private float aye2Time = 1f;

	public Vector3 position;

	private Coroutine cor;

	private void Start()
	{
		if (SingletonMonoBehaviour<NowLoadingAnimation>.exist() && SingletonMonoBehaviour<NowLoadingAnimation>.Instance.isNowLoadingAnimation)
		{
			this.StartAyeAnimation();
		}
	}

	public void SetYousei(int youseiNo)
	{
	}

	public void StartAyeAnimation()
	{
		if (this.YouseiNo != 2 && this.YouseiNo != 5)
		{
			this.cor = base.StartCoroutine(this.AyeAnimation());
		}
		if (this.YouseiNo == 2)
		{
			this.cor = base.StartCoroutine(this.AyeAnimation2());
		}
	}

	[DebuggerHidden]
	private IEnumerator AyeAnimation()
	{
		NowLoadingYousei.<AyeAnimation>c__Iterator36 <AyeAnimation>c__Iterator = new NowLoadingYousei.<AyeAnimation>c__Iterator36();
		<AyeAnimation>c__Iterator.<>f__this = this;
		return <AyeAnimation>c__Iterator;
	}

	[DebuggerHidden]
	private IEnumerator AyeAnimation2()
	{
		NowLoadingYousei.<AyeAnimation2>c__Iterator37 <AyeAnimation2>c__Iterator = new NowLoadingYousei.<AyeAnimation2>c__Iterator37();
		<AyeAnimation2>c__Iterator.<>f__this = this;
		return <AyeAnimation2>c__Iterator;
	}

	[DebuggerHidden]
	private IEnumerator AyeAnimation5()
	{
		NowLoadingYousei.<AyeAnimation5>c__Iterator38 <AyeAnimation5>c__Iterator = new NowLoadingYousei.<AyeAnimation5>c__Iterator38();
		<AyeAnimation5>c__Iterator.<>f__this = this;
		return <AyeAnimation5>c__Iterator;
	}

	[DebuggerHidden]
	private IEnumerator AyeAnimation8()
	{
		NowLoadingYousei.<AyeAnimation8>c__Iterator39 <AyeAnimation8>c__Iterator = new NowLoadingYousei.<AyeAnimation8>c__Iterator39();
		<AyeAnimation8>c__Iterator.<>f__this = this;
		return <AyeAnimation8>c__Iterator;
	}

	[DebuggerHidden]
	private IEnumerator AyeAnimation9()
	{
		NowLoadingYousei.<AyeAnimation9>c__Iterator3A <AyeAnimation9>c__Iterator3A = new NowLoadingYousei.<AyeAnimation9>c__Iterator3A();
		<AyeAnimation9>c__Iterator3A.<>f__this = this;
		return <AyeAnimation9>c__Iterator3A;
	}

	private void OnDestroy()
	{
		if (this.cor != null)
		{
			base.StopCoroutine(this.cor);
		}
		this.YouseiBody = null;
		this.YouseiFace = null;
		this.YouseiOption = null;
	}
}
