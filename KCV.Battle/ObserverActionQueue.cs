using System;

namespace KCV.Battle
{
	public class ObserverActionQueue : ObserverQueue<Action>
	{
		public Action Execute()
		{
			Action result = base.observerQueue.Dequeue();
			Dlg.Call(ref result);
			return result;
		}

		public ObserverActionQueue Executions()
		{
			while (base.observerQueue.get_Count() != 0)
			{
				Action action = base.observerQueue.Dequeue();
				Dlg.Call(ref action);
			}
			return this;
		}
	}
}
