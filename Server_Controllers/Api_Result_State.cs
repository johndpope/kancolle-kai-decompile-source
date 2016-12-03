using System;

namespace Server_Controllers
{
	public enum Api_Result_State
	{
		Success,
		Parameter_Error = -1,
		Login_Error = -2,
		Io_Error = -3,
		Not_Found = -4
	}
}
