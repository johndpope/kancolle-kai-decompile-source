using System;
using System.Collections;
using System.Diagnostics;
using UniRx;

namespace KCV.Battle.Production
{
	public class ProdBufferWithdrawal : BaseProdBuffer
	{
		[DebuggerHidden]
		protected override IEnumerator AnimationObserver(IObserver<bool> observer)
		{
			ProdBufferWithdrawal.<AnimationObserver>c__IteratorDD <AnimationObserver>c__IteratorDD = new ProdBufferWithdrawal.<AnimationObserver>c__IteratorDD();
			<AnimationObserver>c__IteratorDD.observer = observer;
			<AnimationObserver>c__IteratorDD.<$>observer = observer;
			<AnimationObserver>c__IteratorDD.<>f__this = this;
			return <AnimationObserver>c__IteratorDD;
		}
	}
}
