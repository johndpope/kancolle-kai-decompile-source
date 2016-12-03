using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class InstantiateAsyncManager : SingletonMonoBehaviour<InstantiateAsyncManager>
{
	public GameObject test;

	public GameObject parent;

	public static void InstanceAsync(GameObject[] objects, Transform self, AsyncObjects AsyncObj)
	{
		SingletonMonoBehaviour<InstantiateAsyncManager>.Instance.StartCoroutine(SingletonMonoBehaviour<InstantiateAsyncManager>.Instance.InstanceObjects(objects, self, AsyncObj));
	}

	[DebuggerHidden]
	public IEnumerator InstanceObjects(GameObject[] objects, Transform parent, AsyncObjects AsyncObj)
	{
		InstantiateAsyncManager.<InstanceObjects>c__Iterator3D <InstanceObjects>c__Iterator3D = new InstantiateAsyncManager.<InstanceObjects>c__Iterator3D();
		<InstanceObjects>c__Iterator3D.AsyncObj = AsyncObj;
		<InstanceObjects>c__Iterator3D.objects = objects;
		<InstanceObjects>c__Iterator3D.parent = parent;
		<InstanceObjects>c__Iterator3D.<$>AsyncObj = AsyncObj;
		<InstanceObjects>c__Iterator3D.<$>objects = objects;
		<InstanceObjects>c__Iterator3D.<$>parent = parent;
		return <InstanceObjects>c__Iterator3D;
	}
}
