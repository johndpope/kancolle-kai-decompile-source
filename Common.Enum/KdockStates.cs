using System;
using System.Runtime.Serialization;

namespace Common.Enum
{
	[DataContract(Namespace = "")]
	public enum KdockStates
	{
		[EnumMember]
		NOTUSE,
		[EnumMember]
		EMPTY,
		[EnumMember]
		CREATE,
		[EnumMember]
		COMPLETE
	}
}
