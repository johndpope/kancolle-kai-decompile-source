using System;
using System.Runtime.Serialization;

namespace Common.Enum
{
	[DataContract(Namespace = "")]
	public enum QuestState
	{
		[EnumMember]
		NOT_DISP,
		[EnumMember]
		WAITING_START,
		[EnumMember]
		RUNNING,
		[EnumMember]
		COMPLETE,
		[EnumMember]
		END
	}
}
