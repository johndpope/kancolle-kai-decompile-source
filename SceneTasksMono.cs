using System;
using UnityEngine;

public class SceneTasksMono : MonoBehaviour
{
	public const int TASK_CTRL_NUM_DEFAULT = 32;

	private int _nTaskNum;

	private SceneTaskMono[] _clsTasks;

	public int taskNum
	{
		get
		{
			return this._nTaskNum;
		}
	}

	public SceneTaskMono[] tasks
	{
		get
		{
			return this._clsTasks;
		}
	}

	private void Awake()
	{
		this._nTaskNum = 0;
		this._clsTasks = null;
	}

	private void OnDestroy()
	{
		this.UnInit();
	}

	public bool Init()
	{
		return this.Init(32);
	}

	public bool Init(int nTask)
	{
		DebugUtils.dbgAssert(nTask > 0);
		this._nTaskNum = nTask;
		DebugUtils.dbgAssert(null == this._clsTasks);
		Mem.NewAry<SceneTaskMono>(ref this._clsTasks, nTask);
		for (int i = 0; i < this._nTaskNum; i++)
		{
			this._clsTasks[i] = null;
		}
		return true;
	}

	public bool UnInit()
	{
		if (this.CloseAll())
		{
			Mem.DelArySafe<SceneTaskMono>(ref this._clsTasks);
			this._nTaskNum = 0;
			return true;
		}
		return false;
	}

	public void Run()
	{
		if (this._clsTasks != null)
		{
			for (int i = 0; i < this._nTaskNum; i++)
			{
				if (!(null == this._clsTasks[i]))
				{
					if (this._clsTasks[i].isRun)
					{
						this._clsTasks[i].Running();
					}
				}
			}
		}
	}

	public int Open(SceneTaskMono pTask)
	{
		return this.Open(pTask, null);
	}

	public int Open(SceneTaskMono pTask, SceneTaskMono pTaskParent)
	{
		if (this._clsTasks != null)
		{
			for (int i = 0; i < this._nTaskNum; i++)
			{
				if (!(null != this._clsTasks[i]))
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
		return this._clsTasks == null || !(null != this._clsTasks[iTask]) || this._clsTasks[iTask].Close();
	}

	public bool CloseAll()
	{
		if (this._clsTasks != null)
		{
			for (int i = 0; i < this._nTaskNum; i++)
			{
				this.Close(i);
			}
			return true;
		}
		return true;
	}

	public SceneTaskMono GetTask(int iTask)
	{
		return this._clsTasks[iTask];
	}

	public int GetTaskNumRun()
	{
		int num = 0;
		if (this._clsTasks != null)
		{
			for (int i = 0; i < this._nTaskNum; i++)
			{
				if (!(null == this._clsTasks[i]))
				{
					num++;
				}
			}
		}
		return num;
	}

	public int ChkRun(SceneTaskMono pTask)
	{
		if (this._clsTasks != null)
		{
			for (int i = 0; i < this._nTaskNum; i++)
			{
				if (!(null == this._clsTasks[i]))
				{
					if (!(pTask != this._clsTasks[i]))
					{
						return i;
					}
				}
			}
		}
		return -1;
	}
}
