using System;
using System.Runtime.Serialization;

namespace Common.Enum
{
	[DataContract(Namespace = "")]
	public enum MissionStates
	{
		[EnumMember]
		NONE,
		[EnumMember]
		RUNNING,
		[EnumMember]
		STOP,
		[EnumMember]
		END
	}
}
