using Common.Enum;
using System;

namespace Server_Common.Formats
{
	public class AirReconnaissanceFmt
	{
		public MapAirReconnaissanceKind AirKind;

		public MissionResultKinds SearchResult;

		public AirReconnaissanceFmt(MapAirReconnaissanceKind kind, MissionResultKinds result)
		{
			this.AirKind = kind;
			this.SearchResult = result;
		}
	}
}
