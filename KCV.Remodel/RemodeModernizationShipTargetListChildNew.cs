using local.models;
using System;

namespace KCV.Remodel
{
	public class RemodeModernizationShipTargetListChildNew
	{
		public enum ListItemOption
		{
			Model,
			UnSet
		}

		public RemodeModernizationShipTargetListChildNew.ListItemOption mOption
		{
			get;
			private set;
		}

		public ShipModel mShipModel
		{
			get;
			private set;
		}

		public int mType
		{
			get;
			private set;
		}

		public RemodeModernizationShipTargetListChildNew(RemodeModernizationShipTargetListChildNew.ListItemOption option, ShipModel model)
		{
			this.mOption = option;
			this.mShipModel = model;
			if (model != null)
			{
				this.mType = ((0 >= model.PowUpKaryoku) ? 0 : 16) + ((0 >= model.PowUpRaisou) ? 0 : 8) + ((0 >= model.PowUpSoukou) ? 0 : 4) + ((0 >= model.PowUpTaikuu) ? 0 : 2) + ((0 >= model.PowUpLucky) ? 0 : 1);
			}
			else
			{
				this.mType = 0;
			}
		}
	}
}
