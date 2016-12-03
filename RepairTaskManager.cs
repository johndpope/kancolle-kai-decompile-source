using System;
using UnityEngine;

public class RepairTaskManager : SceneTaskMono
{
	private StatementMachine _state;

	protected override void Awake()
	{
		this._state = new StatementMachine();
		this._state.AddState(new StatementMachine.StatementMachineInitialize(this.Init), new StatementMachine.StatementMachineUpdate(this.Update_));
	}

	private bool Init(object obj)
	{
		return true;
	}

	private bool Update_(object obj)
	{
		if (Input.GetKeyDown(97))
		{
			this._state.AddState(new StatementMachine.StatementMachineInitialize(this.Init2), new StatementMachine.StatementMachineUpdate(this.Update2));
		}
		return true;
	}

	private bool Init2(object obj)
	{
		return true;
	}

	private bool Update2(object obj)
	{
		return true;
	}
}
