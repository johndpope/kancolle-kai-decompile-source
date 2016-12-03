using System;
using System.Collections;
using System.Diagnostics;
using UniRx;

namespace KCV.Battle.Production
{
	public class ProdBufferAssault : BaseProdBuffer
	{
		private Action _actOnPlayLookAtLine2Assult;

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Mem.Del<Action>(ref this._actOnPlayLookAtLine2Assult);
		}

		public bool Init(Action onPlayLookAtLine2Assult)
		{
			this._actOnPlayLookAtLine2Assult = onPlayLookAtLine2Assult;
			return true;
		}

		[DebuggerHidden]
		protected override IEnumerator AnimationObserver(IObserver<bool> observer)
		{
			ProdBufferAssault.<AnimationObserver>c__IteratorD6 <AnimationObserver>c__IteratorD = new ProdBufferAssault.<AnimationObserver>c__IteratorD6();
			<AnimationObserver>c__IteratorD.observer = observer;
			<AnimationObserver>c__IteratorD.<$>observer = observer;
			<AnimationObserver>c__IteratorD.<>f__this = this;
			return <AnimationObserver>c__IteratorD;
		}

		private void PlayLookAtLine2Assult()
		{
			Dlg.Call(ref this._actOnPlayLookAtLine2Assult);
		}
	}
}
