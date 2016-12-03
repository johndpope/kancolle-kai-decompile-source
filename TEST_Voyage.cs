using KCV.Strategy;
using local.models;
using System;
using System.Collections;
using System.Diagnostics;

public class TEST_Voyage : SingletonMonoBehaviour<TEST_Voyage>
{
	private Stopwatch mStopWatch;

	private IEnumerator mStartVoyageCoroutine;

	private int mCycleCount;

	public void StartVoyage()
	{
		if (this.mStartVoyageCoroutine == null)
		{
			this.SuppressTutorials();
			this.mStopWatch = new Stopwatch();
			this.mStopWatch.Reset();
			this.mStopWatch.Start();
			this.mStartVoyageCoroutine = this.StartVoyageCoroutine();
			base.StartCoroutine(this.mStartVoyageCoroutine);
		}
	}

	private void SuppressTutorials()
	{
		TutorialModel tutorial = StrategyTopTaskManager.GetLogicManager().UserInfo.Tutorial;
		for (int i = 0; i < 20; i++)
		{
			tutorial.SetStepTutorialFlg(i);
		}
		for (int j = 0; j < 99; j++)
		{
			tutorial.SetKeyTutorialFlg(j);
		}
	}

	[DebuggerHidden]
	private IEnumerator StartVoyageCoroutine()
	{
		TEST_Voyage.<StartVoyageCoroutine>c__Iterator1CD <StartVoyageCoroutine>c__Iterator1CD = new TEST_Voyage.<StartVoyageCoroutine>c__Iterator1CD();
		<StartVoyageCoroutine>c__Iterator1CD.<>f__this = this;
		return <StartVoyageCoroutine>c__Iterator1CD;
	}

	[DebuggerHidden]
	private IEnumerator DeckTeaTime(DeckModel deckModel)
	{
		TEST_Voyage.<DeckTeaTime>c__Iterator1CE <DeckTeaTime>c__Iterator1CE = new TEST_Voyage.<DeckTeaTime>c__Iterator1CE();
		<DeckTeaTime>c__Iterator1CE.deckModel = deckModel;
		<DeckTeaTime>c__Iterator1CE.<$>deckModel = deckModel;
		<DeckTeaTime>c__Iterator1CE.<>f__this = this;
		return <DeckTeaTime>c__Iterator1CE;
	}

	[DebuggerHidden]
	private IEnumerator GoBackMenues()
	{
		TEST_Voyage.<GoBackMenues>c__Iterator1CF <GoBackMenues>c__Iterator1CF = new TEST_Voyage.<GoBackMenues>c__Iterator1CF();
		<GoBackMenues>c__Iterator1CF.<>f__this = this;
		return <GoBackMenues>c__Iterator1CF;
	}

	[DebuggerHidden]
	private IEnumerator WaitForCount(string message, int seconds)
	{
		TEST_Voyage.<WaitForCount>c__Iterator1D0 <WaitForCount>c__Iterator1D = new TEST_Voyage.<WaitForCount>c__Iterator1D0();
		<WaitForCount>c__Iterator1D.seconds = seconds;
		<WaitForCount>c__Iterator1D.message = message;
		<WaitForCount>c__Iterator1D.<$>seconds = seconds;
		<WaitForCount>c__Iterator1D.<$>message = message;
		return <WaitForCount>c__Iterator1D;
	}

	[DebuggerHidden]
	private IEnumerator GoFrontMenues()
	{
		TEST_Voyage.<GoFrontMenues>c__Iterator1D1 <GoFrontMenues>c__Iterator1D = new TEST_Voyage.<GoFrontMenues>c__Iterator1D1();
		<GoFrontMenues>c__Iterator1D.<>f__this = this;
		return <GoFrontMenues>c__Iterator1D;
	}

	[DebuggerHidden]
	private IEnumerator GoSortie()
	{
		TEST_Voyage.<GoSortie>c__Iterator1D2 <GoSortie>c__Iterator1D = new TEST_Voyage.<GoSortie>c__Iterator1D2();
		<GoSortie>c__Iterator1D.<>f__this = this;
		return <GoSortie>c__Iterator1D;
	}
}
