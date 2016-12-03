using local.models;
using System;
using System.Collections;
using System.Diagnostics;
using UniRx;

namespace KCV.SortieMap
{
	public class EventMailstrom : BaseEvent
	{
		private MapEventHappeningModel _clsEventHappeningModel;

		public EventMailstrom(MapEventHappeningModel eventHappeningModel)
		{
			this._clsEventHappeningModel = eventHappeningModel;
		}

		protected override void Dispose(bool disposing)
		{
			Mem.Del<MapEventHappeningModel>(ref this._clsEventHappeningModel);
			base.Dispose(disposing);
		}

		[DebuggerHidden]
		protected override IEnumerator AnimationObserver(IObserver<bool> observer)
		{
			EventMailstrom.<AnimationObserver>c__Iterator11C <AnimationObserver>c__Iterator11C = new EventMailstrom.<AnimationObserver>c__Iterator11C();
			<AnimationObserver>c__Iterator11C.observer = observer;
			<AnimationObserver>c__Iterator11C.<$>observer = observer;
			<AnimationObserver>c__Iterator11C.<>f__this = this;
			return <AnimationObserver>c__Iterator11C;
		}
	}
}
