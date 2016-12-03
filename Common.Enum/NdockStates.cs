using System;
using System.Runtime.Serialization;

namespace Common.Enum
{
	[DataContract(Namespace = "")]
	public enum NdockStates
	{
		[EnumMember]
		NOTUSE,
		[EnumMember]
		EMPTY,
		[EnumMember]
		RESTORE
	}
}
