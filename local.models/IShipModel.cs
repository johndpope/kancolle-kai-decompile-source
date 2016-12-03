using System;

namespace local.models
{
	public interface IShipModel
	{
		int MstId
		{
			get;
		}

		int ShipType
		{
			get;
		}

		string ShipTypeName
		{
			get;
		}

		string Name
		{
			get;
		}

		int MaxHp
		{
			get;
		}
	}
}
