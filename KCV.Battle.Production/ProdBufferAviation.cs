using System;
using System.Collections;
using System.Diagnostics;
using UniRx;

namespace KCV.Battle.Production
{
	public class ProdBufferAviation : BaseProdBuffer
	{
		[DebuggerHidden]
		protected override IEnumerator AnimationObserver(IObserver<bool> observer)
		{
			ProdBufferAviation.<AnimationObserver>c__IteratorD7 <AnimationObserver>c__IteratorD = new ProdBufferAviation.<AnimationObserver>c__IteratorD7();
			<AnimationObserver>c__IteratorD.observer = observer;
			<AnimationObserver>c__IteratorD.<$>observer = observer;
			<AnimationObserver>c__IteratorD.<>f__this = this;
			return <AnimationObserver>c__IteratorD;
		}
	}
}
