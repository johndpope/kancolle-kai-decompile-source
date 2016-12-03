using System;

namespace Server_Controllers
{
	public class Api_Result<T>
	{
		public Api_Result_State state;

		public TurnState t_state;

		public T data;

		public Api_Result()
		{
			this.state = Api_Result_State.Success;
			this.t_state = TurnState.CONTINOUS;
		}
	}
}
