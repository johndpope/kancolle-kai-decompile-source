using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class iTweenTask
{
	public delegate void FinishedHandler(params object[] param);

	private Hashtable tweenArguments;

	public event iTweenTask.FinishedHandler Finished
	{
		[MethodImpl(32)]
		add
		{
			this.Finished = (iTweenTask.FinishedHandler)Delegate.Combine(this.Finished, value);
		}
		[MethodImpl(32)]
		remove
		{
			this.Finished = (iTweenTask.FinishedHandler)Delegate.Remove(this.Finished, value);
		}
	}

	public void Task(GameObject go, Hashtable hash)
	{
		this.Finished = (iTweenTask.FinishedHandler)Delegate.Combine(this.Finished, new iTweenTask.FinishedHandler(this.TaskFnished));
	}

	private void TaskFnished(params object[] param)
	{
		iTweenTask.FinishedHandler finished = this.Finished;
		if (finished != null)
		{
			finished(param);
		}
	}
}
