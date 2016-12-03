using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class AsyncObjects : MonoBehaviour
{
	private delegate void InstantiatePrefab(List<ResourceRequest> Reqs);

	[Button("EditorCreate", "プレハブ生成", new object[]
	{

	})]
	public int CreateInstance;

	public GameObject[] objects;

	public string[] PrefabPaths;

	[NonSerialized]
	public List<GameObject> GOs;

	[NonSerialized]
	public Dictionary<string, GameObject> GOs_Dic;

	public bool isFinished;

	public bool isActive;

	public bool isAutoLoad = true;

	public bool isAutoActive;

	public float delay;

	public List<Action> Act;

	public string[] NameChange;

	private void Awake()
	{
		DebugUtils.SLog("AsyncObjects Awake" + Time.get_realtimeSinceStartup());
		this.Act = new List<Action>();
	}

	private void Start()
	{
		DebugUtils.SLog("AsyncObjects Start" + Time.get_realtimeSinceStartup());
		this.isFinished = false;
		if (this.isAutoLoad)
		{
			this.StartLoad();
		}
	}

	public void StartLoad()
	{
		if (this.objects.Length != 0)
		{
			InstantiateAsyncManager.InstanceAsync(this.objects, base.get_transform(), this);
		}
		if (this.PrefabPaths == null)
		{
			return;
		}
		if (this.PrefabPaths.Length != 0)
		{
			this.ResourceLoadAsync(this.PrefabPaths);
		}
	}

	public void setActiveGOs()
	{
		base.StartCoroutine(this.ActiveGos());
	}

	[DebuggerHidden]
	private IEnumerator ActiveGos()
	{
		AsyncObjects.<ActiveGos>c__Iterator3B <ActiveGos>c__Iterator3B = new AsyncObjects.<ActiveGos>c__Iterator3B();
		<ActiveGos>c__Iterator3B.<>f__this = this;
		return <ActiveGos>c__Iterator3B;
	}

	private void ResourceLoadAsync(string[] PrefabPaths)
	{
		List<ResourceRequest> list = new List<ResourceRequest>(PrefabPaths.Length);
		for (int i = 0; i < PrefabPaths.Length; i++)
		{
			list.Add(Resources.LoadAsync(PrefabPaths[i]));
		}
		AsyncObjects.InstantiatePrefab dlgt = new AsyncObjects.InstantiatePrefab(this.StartInstantiate);
		base.StartCoroutine(this.WaitAllLoaded(list, dlgt));
	}

	[DebuggerHidden]
	private IEnumerator WaitAllLoaded(List<ResourceRequest> Reqs, AsyncObjects.InstantiatePrefab dlgt)
	{
		AsyncObjects.<WaitAllLoaded>c__Iterator3C <WaitAllLoaded>c__Iterator3C = new AsyncObjects.<WaitAllLoaded>c__Iterator3C();
		<WaitAllLoaded>c__Iterator3C.Reqs = Reqs;
		<WaitAllLoaded>c__Iterator3C.dlgt = dlgt;
		<WaitAllLoaded>c__Iterator3C.<$>Reqs = Reqs;
		<WaitAllLoaded>c__Iterator3C.<$>dlgt = dlgt;
		return <WaitAllLoaded>c__Iterator3C;
	}

	private void StartInstantiate(List<ResourceRequest> Reqs)
	{
		GameObject[] array = new GameObject[Reqs.get_Count()];
		for (int i = 0; i < Reqs.get_Count(); i++)
		{
			array[i] = (Reqs.get_Item(i).get_asset() as GameObject);
		}
		InstantiateAsyncManager.InstanceAsync(array, base.get_transform(), this);
	}

	private void EditorCreate()
	{
		GameObject[] array = this.objects;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject prefab = array[i];
			Util.InstantiateGameObject(prefab, base.get_transform());
		}
	}

	private void OnDestroy()
	{
	}
}
