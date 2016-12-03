using System;
using System.Collections;
using System.Diagnostics;
using UniRx;

namespace KCV.Battle.Production
{
	public class ProdBufferShelling : BaseProdBuffer
	{
		[DebuggerHidden]
		protected override IEnumerator AnimationObserver(IObserver<bool> observer)
		{
			ProdBufferShelling.<AnimationObserver>c__IteratorDA <AnimationObserver>c__IteratorDA = new ProdBufferShelling.<AnimationObserver>c__IteratorDA();
			<AnimationObserver>c__IteratorDA.observer = observer;
			<AnimationObserver>c__IteratorDA.<$>observer = observer;
			<AnimationObserver>c__IteratorDA.<>f__this = this;
			return <AnimationObserver>c__IteratorDA;
		}
	}
}
