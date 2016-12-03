using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class TestEmptyScene : MonoBehaviour
{
	private AsyncOperation async;

	private void Update()
	{
		if (Input.GetKeyDown(350) || Input.GetKeyDown(120))
		{
			Resources.UnloadUnusedAssets();
		}
		else if (Input.GetKeyDown(351) || Input.GetKeyDown(122))
		{
			Application.LoadLevel(Generics.Scene.Strategy.ToString());
		}
		else if (Input.GetKeyDown(352) || Input.GetKeyDown(99))
		{
			Application.LoadLevel(1);
		}
		if (this.async != null)
		{
			Debug.Log(this.async.get_progress());
			if (this.async.get_progress() >= 0.9f && (Input.GetKeyDown(353) || Input.GetKeyDown(118)))
			{
				this.async.set_allowSceneActivation(true);
			}
		}
	}

	[DebuggerHidden]
	private IEnumerator LoadScene(AsyncOperation async)
	{
		TestEmptyScene.<LoadScene>c__Iterator194 <LoadScene>c__Iterator = new TestEmptyScene.<LoadScene>c__Iterator194();
		<LoadScene>c__Iterator.async = async;
		<LoadScene>c__Iterator.<$>async = async;
		return <LoadScene>c__Iterator;
	}
}
