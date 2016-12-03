using System;

namespace Librarys.State
{
	public class Task : IDisposable
	{
		public enum TaskMode
		{
			TaskMode_ST,
			TaskMode_BEF = -1,
			TaskMode_Free,
			TaskMode_Init,
			TaskMode_Update,
			TaskMode_UnInit,
			TaskMode_AFT,
			TaskMode_NUM = 4,
			TaskMode_ED = 3
		}

		private Tasks _pTasks;

		private int _iTask;

		private Task _pTaskParent;

		private Task.TaskMode _iMode;

		public Task()
		{
			this._pTasks = null;
			this._iTask = -1;
			this._pTaskParent = null;
			this._iMode = Task.TaskMode.TaskMode_ST;
		}

		public void Dispose()
		{
			Mem.Del<Tasks>(ref this._pTasks);
			Mem.Del<int>(ref this._iTask);
			Mem.Del<Task>(ref this._pTaskParent);
			Mem.Del<Task.TaskMode>(ref this._iMode);
			this.Dispose(true);
		}

		public int GetId()
		{
			return this._iTask;
		}

		public Task.TaskMode GetMode()
		{
			return this._iMode;
		}

		public bool IsRun()
		{
			return this._iMode != Task.TaskMode.TaskMode_ST;
		}

		public bool Open(Tasks pTasks, int iTask, Task pTaskParent)
		{
			this._pTasks = pTasks;
			DebugUtils.dbgAssert(this._iMode == Task.TaskMode.TaskMode_ST);
			if (pTasks != null)
			{
				pTasks.tasks[iTask] = this;
			}
			this._iTask = iTask;
			this._pTaskParent = pTaskParent;
			this._iMode = Task.TaskMode.TaskMode_Init;
			return true;
		}

		public bool Close()
		{
			if (this._pTasks != null)
			{
				for (int i = 0; i < this._pTasks.taskNum; i++)
				{
					DebugUtils.dbgAssert(null != this._pTasks.tasks);
					if (this._pTasks.tasks[i] != null)
					{
						if (this == this._pTasks.tasks[i]._pTaskParent && !this._pTasks.tasks[i].Close())
						{
							return false;
						}
					}
				}
			}
			this._iMode = Task.TaskMode.TaskMode_UnInit;
			if (!this.UnInit())
			{
				return false;
			}
			if (this._pTasks != null)
			{
				DebugUtils.dbgAssert(null != this._pTasks.tasks);
				DebugUtils.dbgAssert(this._iTask >= 0 && this._iTask < this._pTasks.taskNum);
				this._pTasks.tasks[this._iTask] = null;
			}
			this._pTasks = null;
			this._iTask = -1;
			this._pTaskParent = null;
			this._iMode = Task.TaskMode.TaskMode_ST;
			return true;
		}

		public bool Run()
		{
			switch (this._iMode)
			{
			case Task.TaskMode.TaskMode_Init:
				if (this.Init())
				{
					this._iMode = Task.TaskMode.TaskMode_Update;
				}
				break;
			case Task.TaskMode.TaskMode_Update:
				if (!this.Update())
				{
					this._iMode = Task.TaskMode.TaskMode_UnInit;
				}
				break;
			case Task.TaskMode.TaskMode_UnInit:
				if (!this.Close())
				{
				}
				break;
			}
			return this.IsRun();
		}

		protected virtual void Dispose(bool isDisposing)
		{
		}

		protected virtual bool Init()
		{
			return true;
		}

		protected virtual bool UnInit()
		{
			return true;
		}

		protected virtual bool Update()
		{
			return true;
		}

		protected void ImmediateTermination()
		{
			this.Close();
		}
	}
}
