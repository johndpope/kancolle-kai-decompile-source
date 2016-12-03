using Server_Common.Formats;
using System;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_mapincentiveBase : Model_Base
	{
		protected int _id;

		protected int _incentive_no;

		protected double _choose_rate;

		protected MapItemGetFmt.enumCategory _getCategory;

		protected int _get_id;

		protected int _get_count;

		public int Id
		{
			get
			{
				return this._id;
			}
			protected set
			{
				this._id = value;
			}
		}

		public int Incentive_no
		{
			get
			{
				return this._incentive_no;
			}
			protected set
			{
				this._incentive_no = value;
			}
		}

		public double Choose_rate
		{
			get
			{
				return this._choose_rate;
			}
			protected set
			{
				this._choose_rate = value;
			}
		}

		public MapItemGetFmt.enumCategory GetCategory
		{
			get
			{
				return this._getCategory;
			}
			protected set
			{
				this._getCategory = value;
			}
		}

		public int Get_id
		{
			get
			{
				return this._get_id;
			}
			protected set
			{
				this._get_id = value;
			}
		}

		public int Get_count
		{
			get
			{
				return this._get_count;
			}
			protected set
			{
				this._get_count = value;
			}
		}

		protected override void setProperty(XElement element)
		{
			this.Id = int.Parse(element.Element("Id").get_Value());
			this.Incentive_no = int.Parse(element.Element("Incentive_no").get_Value());
			this.Choose_rate = double.Parse(element.Element("Choose_rate").get_Value());
			this.setIncentiveItem(element);
		}

		protected virtual void setIncentiveItem(XElement element)
		{
			if (element.Element("Ship_id") != null)
			{
				this.GetCategory = MapItemGetFmt.enumCategory.Ship;
				this.Get_id = int.Parse(element.Element("Ship_id").get_Value());
				this.Get_count = int.Parse(element.Element("Ship_count").get_Value());
				return;
			}
			if (element.Element("Slotitem_id") != null)
			{
				this.GetCategory = MapItemGetFmt.enumCategory.Slotitem;
				this.Get_id = int.Parse(element.Element("Slotitem_id").get_Value());
				this.Get_count = int.Parse(element.Element("Slotitem_count").get_Value());
				return;
			}
			if (element.Element("Useitem_id") != null)
			{
				this.GetCategory = MapItemGetFmt.enumCategory.UseItem;
				this.Get_id = int.Parse(element.Element("Useitem_id").get_Value());
				this.Get_count = int.Parse(element.Element("Useitem_count").get_Value());
				return;
			}
			if (element.Element("Material_id") != null)
			{
				this.GetCategory = MapItemGetFmt.enumCategory.Material;
				this.Get_id = int.Parse(element.Element("Material_id").get_Value());
				this.Get_count = int.Parse(element.Element("Material_count").get_Value());
				return;
			}
			if (element.Element("Furniture_id") != null)
			{
				this.GetCategory = MapItemGetFmt.enumCategory.Furniture;
				this.Get_id = int.Parse(element.Element("Furniture_id").get_Value());
				this.Get_count = int.Parse(element.Element("Furniture_count").get_Value());
				return;
			}
		}
	}
}
