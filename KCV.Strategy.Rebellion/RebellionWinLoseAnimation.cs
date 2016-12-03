using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Strategy.Rebellion
{
	public class RebellionWinLoseAnimation : MonoBehaviour
	{
		[SerializeField]
		private Transform gei;

		[SerializeField]
		private Transform geki;

		[SerializeField]
		private Transform shippai;

		[SerializeField]
		private Transform seikou;

		[SerializeField]
		private Transform obi;

		[SerializeField]
		private Transform Labels;

		[Button("StartAnimation", "StartAnimation", new object[]
		{
			true
		})]
		public int button1;

		public iTween.EaseType ease;

		public iTween.EaseType ease2;

		private Vector3 scale;

		public Coroutine StartAnimation(bool isWin)
		{
			return base.StartCoroutine(this.StartAnimationCor(isWin));
		}

		[DebuggerHidden]
		private IEnumerator StartAnimationCor(bool isWin)
		{
			RebellionWinLoseAnimation.<StartAnimationCor>c__Iterator15C <StartAnimationCor>c__Iterator15C = new RebellionWinLoseAnimation.<StartAnimationCor>c__Iterator15C();
			<StartAnimationCor>c__Iterator15C.isWin = isWin;
			<StartAnimationCor>c__Iterator15C.<$>isWin = isWin;
			<StartAnimationCor>c__Iterator15C.<>f__this = this;
			return <StartAnimationCor>c__Iterator15C;
		}
	}
}
