using System;
using System.Collections;
using System.Diagnostics;
using UniRx;

namespace KCV.Battle.Production
{
	public class ProdBufferClose : BaseProdBuffer
	{
		[DebuggerHidden]
		protected override IEnumerator AnimationObserver(IObserver<bool> observer)
		{
			ProdBufferClose.<AnimationObserver>c__IteratorD9 <AnimationObserver>c__IteratorD = new ProdBufferClose.<AnimationObserver>c__IteratorD9();
			<AnimationObserver>c__IteratorD.observer = observer;
			<AnimationObserver>c__IteratorD.<$>observer = observer;
			<AnimationObserver>c__IteratorD.<>f__this = this;
			return <AnimationObserver>c__IteratorD;
		}
	}
}
