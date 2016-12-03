using System;
using UnityEngine;

public class SceneTaskMono : MonoBehaviour
{
	public enum TaskState
	{
		TaskState_ST,
		TaskState_BEF = -1,
		TaskState_Free,
		TaskState_Init,
		TaskState_Update,
		TaskState_UnInit,
		TaskState_Exit,
		TaskState_Awake,
		TaskState_Start,
		TaskState_LateUpdate,
		TaskState_AFT,
		TaskState_NUM = 8,
		TaskState_ED = 7
	}

	private SceneTasksMono _pTasks;

	private int _nTask;

	private SceneTaskMono _pTaskParent;

	private SceneTaskMono.TaskState _iState;

	private bool _isDraw;

	private bool _isActive;

	[SerializeField]
	protected Transform _traScenePrefab;

	public bool isRun
	{
		get
		{
			return this._iState != SceneTaskMono.TaskState.TaskState_ST;
		}
	}

	public int id
	{
		get
		{
			return this._nTask;
		}
	}

	public SceneTaskMono.TaskState state
	{
		get
		{
			return this._iState;
		}
	}

	public bool isActive
	{
		get
		{
			return this._isActive;
		}
	}

	public bool isDraw
	{
		get
		{
			return this._isDraw;
		}
	}

	public Transform scenePrefab
	{
		get
		{
			return this._traScenePrefab;
		}
		set
		{
			this._traScenePrefab = value;
		}
	}

	protected virtual void Awake()
	{
		this._pTasks = null;
		this._nTask = -1;
		this._pTaskParent = null;
		this._iState = SceneTaskMono.TaskState.TaskState_ST;
		this._isDraw = false;
		this._isActive = false;
	}

	protected virtual void Start()
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

	protected virtual bool Run()
	{
		return true;
	}

	protected virtual void Active()
	{
	}

	public bool Open(SceneTasksMono pTasks, int iTask, SceneTaskMono pTaskParent)
	{
		this._pTasks = pTasks;
		if (null != pTasks)
		{
			pTasks.tasks[iTask] = this;
		}
		this._nTask = iTask;
		this._pTaskParent = pTaskParent;
		this._isDraw = false;
		this._iState = SceneTaskMono.TaskState.TaskState_Init;
		return true;
	}

	public bool Close()
	{
		if (null != this._pTasks)
		{
			for (int i = 0; i < this._pTasks.taskNum; i++)
			{
				DebugUtils.dbgAssert(null != this._pTasks.tasks);
				if (!(null == this._pTasks.tasks[i]))
				{
					if (this == this._pTasks.tasks[i]._pTaskParent && !this._pTasks.tasks[i].Close())
					{
						return false;
					}
				}
			}
		}
		this._iState = SceneTaskMono.TaskState.TaskState_UnInit;
		if (!this.UnInit())
		{
			return false;
		}
		if (null != this._pTasks)
		{
			DebugUtils.dbgAssert(null != this._pTasks.tasks);
			DebugUtils.dbgAssert(this._nTask >= 0 && this._nTask < this._pTasks.taskNum);
			this._pTasks.tasks[this._nTask] = null;
		}
		this._pTasks = null;
		this._nTask = -1;
		this._pTaskParent = null;
		this._isDraw = false;
		this._iState = SceneTaskMono.TaskState.TaskState_ST;
		return true;
	}

	public bool Running()
	{
		switch (this._iState)
		{
		case SceneTaskMono.TaskState.TaskState_Init:
			this.Active();
			if (this.Init())
			{
				this._isDraw = true;
				this._iState = SceneTaskMono.TaskState.TaskState_Update;
			}
			break;
		case SceneTaskMono.TaskState.TaskState_Update:
			if (!this.Run())
			{
				this._iState = SceneTaskMono.TaskState.TaskState_UnInit;
			}
			break;
		case SceneTaskMono.TaskState.TaskState_UnInit:
			if (!this.Close())
			{
			}
			break;
		}
		return this.isRun;
	}

	public void SetActive(bool isActive)
	{
		this._isActive = isActive;
	}

	protected void ImmediateTermination()
	{
		this.Close();
	}

	protected void InitState()
	{
		this._iState = SceneTaskMono.TaskState.TaskState_Init;
	}

	protected virtual void CreateScene(GameObject prefab)
	{
		if (base.get_transform().get_childCount() <= 0)
		{
			this._traScenePrefab = Util.InstantiateGameObject(prefab, base.get_transform()).get_transform();
		}
	}

	protected virtual void DiscardScene()
	{
		if (this._traScenePrefab != null)
		{
			Object.Destroy(this._traScenePrefab.get_gameObject());
		}
	}

	protected virtual void Discard()
	{
		Object.Destroy(base.get_gameObject());
	}

	[Obsolete("GetID() is the old format. please use the id.")]
	public int GetId()
	{
		return this._nTask;
	}

	[Obsolete("IsRun() is the old format. please use the isRun.")]
	public bool IsRun()
	{
		return this._iState != SceneTaskMono.TaskState.TaskState_ST;
	}

	[Obsolete("GetState() is the old format. please use the state.")]
	public SceneTaskMono.TaskState GetState()
	{
		return this._iState;
	}

	[Obsolete("IsActive() is the old format. please use the isActive.")]
	public bool IsActive()
	{
		return this._isActive;
	}

	[Obsolete("IsDraw() is the old format. please use the isDraw.")]
	protected bool IsDraw()
	{
		return this._isDraw;
	}
}
