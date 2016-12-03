using System;
using System.Collections.Generic;

public class StatementMachine
{
	private class Statement
	{
		private bool _isInitialize;

		private bool _isFinished;

		private float _fStartDuration;

		private string _strGroupName;

		private object _objData;

		private StatementMachine.StatementMachineInitialize _delInitialize;

		private StatementMachine.StatementMachineUpdate _delUpdate;

		private StatementMachine.StatementMachineTerminate _delTerminate;

		public bool isInitialize
		{
			get
			{
				return this._isInitialize;
			}
			set
			{
				this._isInitialize = value;
			}
		}

		public bool isFinished
		{
			get
			{
				return this._isFinished;
			}
			set
			{
				this._isFinished = value;
			}
		}

		public float startDuration
		{
			get
			{
				return this._fStartDuration;
			}
			set
			{
				this._fStartDuration = value;
			}
		}

		public string groupName
		{
			get
			{
				return this._strGroupName;
			}
			set
			{
				this._strGroupName = value;
			}
		}

		public object data
		{
			get
			{
				return this._objData;
			}
			set
			{
				this._objData = value;
			}
		}

		public StatementMachine.StatementMachineInitialize initializeDelegate
		{
			get
			{
				return this._delInitialize;
			}
			set
			{
				this._delInitialize = value;
			}
		}

		public StatementMachine.StatementMachineUpdate updateDelegate
		{
			get
			{
				return this._delUpdate;
			}
			set
			{
				this._delUpdate = value;
			}
		}

		public StatementMachine.StatementMachineTerminate terminateDelegate
		{
			get
			{
				return this._delTerminate;
			}
			set
			{
				this._delTerminate = value;
			}
		}

		internal void Release()
		{
			this._delInitialize = null;
			this._delUpdate = null;
			this._delTerminate = null;
			this._objData = null;
			this._strGroupName = null;
		}
	}

	public delegate bool StatementMachineInitialize(object obj);

	public delegate bool StatementMachineUpdate(object obj);

	public delegate bool StatementMachineTerminate(object obj);

	private Stack<StatementMachine.Statement> _stackState;

	private List<StatementMachine.Statement> _listStateNew;

	public StatementMachine()
	{
		this._stackState = new Stack<StatementMachine.Statement>(16);
		this._listStateNew = new List<StatementMachine.Statement>(4);
	}

	public void Clear()
	{
		this._stackState.Clear();
		this._listStateNew.Clear();
	}

	public void AddState(StatementMachine.StatementMachineInitialize init, StatementMachine.StatementMachineUpdate update, StatementMachine.StatementMachineTerminate terminate, string groupName, object data, float startDuration)
	{
		StatementMachine.Statement statement = new StatementMachine.Statement();
		statement.isInitialize = false;
		statement.isFinished = false;
		statement.groupName = groupName;
		statement.startDuration = startDuration;
		statement.data = data;
		statement.initializeDelegate = init;
		statement.updateDelegate = update;
		statement.terminateDelegate = terminate;
		this._listStateNew.Add(statement);
	}

	public void AddState(StatementMachine.StatementMachineInitialize init, StatementMachine.StatementMachineUpdate update, StatementMachine.StatementMachineTerminate terminate)
	{
		StatementMachine.Statement statement = new StatementMachine.Statement();
		statement.isInitialize = false;
		statement.isFinished = false;
		statement.groupName = null;
		statement.startDuration = 0f;
		statement.data = null;
		statement.initializeDelegate = init;
		statement.updateDelegate = update;
		statement.terminateDelegate = terminate;
		this._listStateNew.Add(statement);
	}

	public void AddState(StatementMachine.StatementMachineInitialize init, StatementMachine.StatementMachineUpdate update)
	{
		StatementMachine.Statement statement = new StatementMachine.Statement();
		statement.isInitialize = false;
		statement.isFinished = false;
		statement.groupName = null;
		statement.startDuration = 0f;
		statement.data = null;
		statement.initializeDelegate = init;
		statement.updateDelegate = update;
		statement.terminateDelegate = null;
		this._listStateNew.Add(statement);
	}

	public void AddState(StatementMachine.StatementMachineInitialize init, StatementMachine.StatementMachineUpdate update, object data)
	{
		StatementMachine.Statement statement = new StatementMachine.Statement();
		statement.isInitialize = false;
		statement.isFinished = false;
		statement.groupName = null;
		statement.startDuration = 0f;
		statement.data = data;
		statement.initializeDelegate = init;
		statement.updateDelegate = update;
		statement.terminateDelegate = null;
		this._listStateNew.Add(statement);
	}

	public void OnUpdate(float deltaTime)
	{
		if (this._stackState.get_Count() > 0)
		{
			StatementMachine.Statement statement = this._stackState.Peek();
			if (!statement.isInitialize)
			{
				statement.startDuration -= deltaTime;
				if (statement.startDuration > 0f)
				{
					return;
				}
				if (statement.initializeDelegate != null)
				{
					statement.isFinished = statement.initializeDelegate(statement.data);
				}
				statement.isInitialize = true;
			}
			if (statement.updateDelegate != null && !statement.isFinished)
			{
				statement.isFinished = statement.updateDelegate(statement.data);
			}
			else
			{
				statement.isFinished = true;
			}
			this.GarbageFinished();
		}
		if (this._listStateNew.get_Count() > 0)
		{
			StatementMachine.Statement[] array = this._listStateNew.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				this._stackState.Push(array[i]);
			}
			this._listStateNew.Clear();
		}
	}

	private void GarbageFinished()
	{
		while (this._stackState.get_Count() > 0)
		{
			StatementMachine.Statement statement = this._stackState.Peek();
			if (!statement.isFinished)
			{
				break;
			}
			if (statement.terminateDelegate != null)
			{
				statement.terminateDelegate(statement.data);
			}
			statement.Release();
			this._stackState.Pop();
		}
	}

	public void DeleteGroup(string groupName)
	{
		StatementMachine.Statement[] array = this._stackState.ToArray();
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].groupName != groupName)
			{
				break;
			}
			array[i].isFinished = true;
		}
	}
}
