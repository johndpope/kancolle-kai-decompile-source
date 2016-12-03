using System;
using System.Collections;
using System.Diagnostics;
using UniRx;

namespace KCV.Battle.Production
{
	public class ProdBufferAntiSubmarine : BaseProdBuffer
	{
		[DebuggerHidden]
		protected override IEnumerator AnimationObserver(IObserver<bool> observer)
		{
			ProdBufferAntiSubmarine.<AnimationObserver>c__IteratorD5 <AnimationObserver>c__IteratorD = new ProdBufferAntiSubmarine.<AnimationObserver>c__IteratorD5();
			<AnimationObserver>c__IteratorD.observer = observer;
			<AnimationObserver>c__IteratorD.<$>observer = observer;
			<AnimationObserver>c__IteratorD.<>f__this = this;
			return <AnimationObserver>c__IteratorD;
		}
	}
}
