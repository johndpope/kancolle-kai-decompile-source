using Server_Common.Formats;
using System;

namespace local.models
{
	public abstract class PhaseResultModel
	{
		protected TurnWorkResult _data;

		public PhaseResultModel(TurnWorkResult data)
		{
			this._data = data;
		}
	}
}
