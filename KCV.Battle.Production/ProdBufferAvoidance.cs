using System;
using System.Collections;
using System.Diagnostics;
using UniRx;

namespace KCV.Battle.Production
{
	public class ProdBufferAvoidance : BaseProdBuffer
	{
		[DebuggerHidden]
		protected override IEnumerator AnimationObserver(IObserver<bool> observer)
		{
			ProdBufferAvoidance.<AnimationObserver>c__IteratorD8 <AnimationObserver>c__IteratorD = new ProdBufferAvoidance.<AnimationObserver>c__IteratorD8();
			<AnimationObserver>c__IteratorD.observer = observer;
			<AnimationObserver>c__IteratorD.<$>observer = observer;
			<AnimationObserver>c__IteratorD.<>f__this = this;
			return <AnimationObserver>c__IteratorD;
		}
	}
}
