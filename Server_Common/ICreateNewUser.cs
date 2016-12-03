using Common.Enum;
using System;

namespace Server_Common
{
	public interface ICreateNewUser
	{
		bool CreateNewUser(DifficultKind difficult, int firstShip);

		void PurgeUserData();
	}
}
