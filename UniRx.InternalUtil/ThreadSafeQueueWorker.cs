using System;

namespace UniRx.InternalUtil
{
	public class ThreadSafeQueueWorker
	{
		private const int InitialSize = 10;

		private object gate = new object();

		private bool dequing;

		private int actionListCount;

		private Action[] actionList = new Action[10];

		private int waitingListCount;

		private Action[] waitingList = new Action[10];

		public void Enqueue(Action action)
		{
			object obj = this.gate;
			lock (obj)
			{
				if (this.dequing)
				{
					if (this.waitingList.Length == this.waitingListCount)
					{
						Action[] array = new Action[checked(this.waitingListCount * 2)];
						Array.Copy(this.waitingList, array, this.waitingListCount);
						this.waitingList = array;
					}
					this.waitingList[this.waitingListCount++] = action;
				}
				else
				{
					if (this.actionList.Length == this.actionListCount)
					{
						Action[] array2 = new Action[checked(this.actionListCount * 2)];
						Array.Copy(this.actionList, array2, this.actionListCount);
						this.actionList = array2;
					}
					this.actionList[this.actionListCount++] = action;
				}
			}
		}

		public void ExecuteAll(Action<Exception> unhandledExceptionCallback)
		{
			object obj = this.gate;
			lock (obj)
			{
				if (this.actionListCount == 0)
				{
					return;
				}
				this.dequing = true;
			}
			for (int i = 0; i < this.actionListCount; i++)
			{
				Action action = this.actionList[i];
				try
				{
					action.Invoke();
				}
				catch (Exception ex)
				{
					unhandledExceptionCallback.Invoke(ex);
				}
			}
			object obj2 = this.gate;
			lock (obj2)
			{
				this.dequing = false;
				Array.Clear(this.actionList, 0, this.actionListCount);
				Action[] array = this.actionList;
				this.actionListCount = this.waitingListCount;
				this.actionList = this.waitingList;
				this.waitingListCount = 0;
				this.waitingList = array;
			}
		}
	}
}
