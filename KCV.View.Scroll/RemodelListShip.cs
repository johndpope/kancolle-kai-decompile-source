using local.models;
using System;

namespace KCV.View.Scroll
{
	public class RemodelListShip
	{
		public enum ListItemOption
		{
			Model,
			Option
		}

		public RemodelListShip.ListItemOption Option
		{
			get;
			private set;
		}

		public ShipModel shipModel
		{
			get;
			private set;
		}

		public RemodelListShip(RemodelListShip.ListItemOption option, ShipModel model)
		{
			this.Option = option;
			this.shipModel = model;
		}
	}
}
