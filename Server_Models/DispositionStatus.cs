using System;
using System.Runtime.Serialization;

namespace Server_Models
{
	[DataContract(Namespace = "")]
	public enum DispositionStatus
	{
		[EnumMember]
		NONE = 1,
		[EnumMember]
		ARRIVAL,
		[EnumMember]
		MISSION
	}
}
