using Common.Enum;
using System;

namespace KCV.Startup
{
	public class StartupData
	{
		private string _strAdmiralName;

		private int _nPartnerShipID;

		private DifficultKind _iDifficultKind;

		private bool _isInherit;

		public string AdmiralName
		{
			get
			{
				return this._strAdmiralName;
			}
			set
			{
				this._strAdmiralName = value;
			}
		}

		public int PartnerShipID
		{
			get
			{
				return this._nPartnerShipID;
			}
			set
			{
				this._nPartnerShipID = value;
			}
		}

		public DifficultKind Difficlty
		{
			get
			{
				return this._iDifficultKind;
			}
			set
			{
				this._iDifficultKind = value;
			}
		}

		public bool isInherit
		{
			get
			{
				return this._isInherit;
			}
			set
			{
				this._isInherit = value;
			}
		}

		public StartupData()
		{
			this._strAdmiralName = string.Empty;
			this._nPartnerShipID = -1;
			this._iDifficultKind = DifficultKind.OTU;
			this._isInherit = false;
		}

		public bool UnInit()
		{
			Mem.Del<string>(ref this._strAdmiralName);
			Mem.Del<int>(ref this._nPartnerShipID);
			Mem.Del<DifficultKind>(ref this._iDifficultKind);
			Mem.Del<bool>(ref this._isInherit);
			return true;
		}
	}
}
