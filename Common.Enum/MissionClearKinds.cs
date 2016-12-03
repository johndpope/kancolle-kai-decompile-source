using System;
using System.Runtime.Serialization;

namespace Common.Enum
{
	[DataContract(Namespace = "")]
	public enum MissionClearKinds
	{
		[EnumMember]
		NEW,
		[EnumMember]
		NOTCLEAR,
		[EnumMember]
		CLEARED
	}
}
