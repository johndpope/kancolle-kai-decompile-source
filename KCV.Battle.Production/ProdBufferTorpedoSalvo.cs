using System;
using System.Collections;
using System.Diagnostics;
using UniRx;

namespace KCV.Battle.Production
{
	public class ProdBufferTorpedoSalvo : BaseProdBuffer
	{
		[DebuggerHidden]
		protected override IEnumerator AnimationObserver(IObserver<bool> observer)
		{
			ProdBufferTorpedoSalvo.<AnimationObserver>c__IteratorDB <AnimationObserver>c__IteratorDB = new ProdBufferTorpedoSalvo.<AnimationObserver>c__IteratorDB();
			<AnimationObserver>c__IteratorDB.observer = observer;
			<AnimationObserver>c__IteratorDB.<$>observer = observer;
			<AnimationObserver>c__IteratorDB.<>f__this = this;
			return <AnimationObserver>c__IteratorDB;
		}
	}
}
