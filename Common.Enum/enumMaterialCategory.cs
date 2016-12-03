using System;
using System.Runtime.Serialization;

namespace Common.Enum
{
	[DataContract(Namespace = "")]
	public enum enumMaterialCategory
	{
		[EnumMember]
		Fuel = 1,
		[EnumMember]
		Bull,
		[EnumMember]
		Steel,
		[EnumMember]
		Bauxite,
		[EnumMember]
		Build_Kit,
		[EnumMember]
		Repair_Kit,
		[EnumMember]
		Dev_Kit,
		[EnumMember]
		Revamp_Kit
	}
}
