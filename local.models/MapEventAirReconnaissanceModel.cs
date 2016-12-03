using Common.Enum;
using Server_Common.Formats;
using System;

namespace local.models
{
	public class MapEventAirReconnaissanceModel : MapEventItemModel
	{
		private MapAirReconnaissanceKind _aircraft_type;

		private MissionResultKinds _result;

		public MapAirReconnaissanceKind AircraftType
		{
			get
			{
				return this._aircraft_type;
			}
		}

		public MissionResultKinds SearchResult
		{
			get
			{
				return this._result;
			}
		}

		public MapEventAirReconnaissanceModel(MapItemGetFmt fmt, AirReconnaissanceFmt fmt2) : base(fmt)
		{
			this._aircraft_type = fmt2.AirKind;
			this._result = fmt2.SearchResult;
		}

		public override string ToString()
		{
			return string.Format("[航空偵察] {0} 偵察機:{1} 結果:{2}", base.ToString(), this.AircraftType, this.SearchResult);
		}
	}
}
