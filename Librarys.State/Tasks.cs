using System;

namespace Librarys.State
{
	public class Tasks
	{
		public const int TASK_CTRL_NUM_DEFAULT = 32;

		private int _nTask;

		private Task[] _clsTasks;

		public int taskNum
		{
			get
			{
				return this._nTask;
			}
		}

		public Task[] tasks
		{
			get
			{
				return this._clsTasks;
			}
		}

		public Tasks()
		{
			this._nTask = 0;
			this._clsTasks = null;
		}

		public bool Init(int nTask = 32)
		{
			DebugUtils.dbgAssert(nTask > 0);
			this._nTask = nTask;
			DebugUtils.dbgAssert(null == this._clsTasks);
			Mem.NewAry<Task>(ref this._clsTasks, nTask);
			for (int i = 0; i < this._nTask; i++)
			{
				this._clsTasks[i] = null;
			}
			return true;
		}

		public bool UnInit()
		{
			if (this.CloseAll())
			{
				Mem.DelArySafe<Task>(ref this._clsTasks);
				this._nTask = 0;
				return true;
			}
			return false;
		}

		public void Update()
		{
			if (this._clsTasks != null)
			{
				for (int i = 0; i < this._nTask; i++)
				{
					if (this._clsTasks[i] != null)
					{
						if (this._clsTasks[i].IsRun())
						{
							this._clsTasks[i].Run();
						}
					}
				}
			}
		}

		public int Open(ref Task pTask, Task pTaskParent = null)
		{
			if (this._clsTasks != null)
			{
				for (int i = 0; i < this._nTask; i++)
				{
					if (this._clsTasks[i] == null)
					{
						if (pTask.Open(this, i, pTaskParent))
						{
							return i;
						}
					}
				}
			}
			return -1;
		}

		public int Open(Task pTask, Task pTaskParent = null)
		{
			if (this._clsTasks != null)
			{
				for (int i = 0; i < this._nTask; i++)
				{
					if (this._clsTasks[i] == null)
					{
						if (pTask.Open(this, i, pTaskParent))
						{
							return i;
						}
					}
				}
			}
			return -1;
		}

		public bool Close(int iTask)
		{
			return this._clsTasks != null && this._clsTasks[iTask] != null && this._clsTasks[iTask].Close();
		}

		public bool CloseAll()
		{
			if (this._clsTasks != null)
			{
				for (int i = 0; i < this._nTask; i++)
				{
					this.Close(i);
				}
				return true;
			}
			return false;
		}

		public Task GetTask(int iTask)
		{
			return this._clsTasks[iTask];
		}

		public int GetTaskLength()
		{
			int num = 0;
			for (int i = 0; i < this._nTask; i++)
			{
				if (this._clsTasks[i] != null)
				{
					num++;
				}
			}
			return num;
		}

		public int GetTaskNum()
		{
			return this._nTask;
		}

		public int GetTaskNumRun()
		{
			int num = 0;
			if (this._clsTasks != null)
			{
				for (int i = 0; i < this._nTask; i++)
				{
					if (this._clsTasks[i] != null)
					{
						num++;
					}
				}
			}
			return num;
		}

		public int ChkRun(ref Task pTask)
		{
			if (this._clsTasks != null)
			{
				for (int i = 0; i < this._nTask; i++)
				{
					if (this._clsTasks[i] != null)
					{
						if (pTask == this._clsTasks[i])
						{
							return i;
						}
					}
				}
			}
			return -1;
		}
	}
}
