using local.models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UniRx;

namespace KCV.SortieMap
{
	public class EventItemGet : BaseEvent
	{
		private MapEventItemModel _clsEventItemModel;

		public EventItemGet(MapEventItemModel eventItemModel)
		{
			this._clsEventItemModel = eventItemModel;
		}

		protected override void Dispose(bool disposing)
		{
			Mem.Del<MapEventItemModel>(ref this._clsEventItemModel);
			base.Dispose(disposing);
		}

		[DebuggerHidden]
		protected override IEnumerator AnimationObserver(IObserver<bool> observer)
		{
			EventItemGet.<AnimationObserver>c__Iterator11B <AnimationObserver>c__Iterator11B = new EventItemGet.<AnimationObserver>c__Iterator11B();
			<AnimationObserver>c__Iterator11B.observer = observer;
			<AnimationObserver>c__Iterator11B.<$>observer = observer;
			<AnimationObserver>c__Iterator11B.<>f__this = this;
			return <AnimationObserver>c__Iterator11B;
		}

		private ShipModel GetTargetShip(DeckModel model)
		{
			List<ShipModel> list = Enumerable.ToList<ShipModel>(Enumerable.Where<ShipModel>(model.GetShips(), (ShipModel x) => !x.IsEscaped()));
			return list.get_Item(XorRandom.GetILim(0, list.get_Count() - 1));
		}
	}
}
