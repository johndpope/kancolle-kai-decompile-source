using System;
using System.Runtime.Serialization;

namespace Common.Enum
{
	[DataContract(Namespace = "")]
	public enum BattleCommand
	{
		[EnumMember]
		None = -1,
		[EnumMember]
		Sekkin,
		[EnumMember]
		Hougeki,
		[EnumMember]
		Raigeki,
		[EnumMember]
		Ridatu,
		[EnumMember]
		Taisen,
		[EnumMember]
		Kaihi,
		[EnumMember]
		Kouku,
		[EnumMember]
		Totugeki,
		[EnumMember]
		Tousha
	}
}
