using System;
using System.Runtime.Serialization;

namespace Common.Enum
{
	[DataContract(Namespace = "")]
	public enum BattleWinRankKinds
	{
		[EnumMember]
		NONE,
		[EnumMember]
		E,
		[EnumMember]
		D,
		[EnumMember]
		C,
		[EnumMember]
		B,
		[EnumMember]
		A,
		[EnumMember]
		S
	}
}
