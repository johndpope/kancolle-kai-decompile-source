using System;
using System.Runtime.Serialization;

namespace Common.Enum
{
	[DataContract(Namespace = "")]
	public enum DifficultKind
	{
		[EnumMember]
		TEI = 1,
		[EnumMember]
		HEI,
		[EnumMember]
		OTU,
		[EnumMember]
		KOU,
		[EnumMember]
		SHI
	}
}
