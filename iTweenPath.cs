using System;
using System.Collections.Generic;
using UnityEngine;

public class iTweenPath : MonoBehaviour
{
	public string pathName = string.Empty;

	public Color pathColor = Color.get_cyan();

	public List<Vector3> nodes;

	public int nodeCount;

	public static Dictionary<string, iTweenPath> paths = new Dictionary<string, iTweenPath>();

	public bool initialized;

	public string initialName;

	public iTweenPath()
	{
		List<Vector3> list = new List<Vector3>();
		list.Add(Vector3.get_zero());
		list.Add(Vector3.get_zero());
		this.nodes = list;
		this.initialName = string.Empty;
		base..ctor();
	}

	private void OnEnable()
	{
		if (!iTweenPath.paths.ContainsKey(this.pathName.ToLower()))
		{
			iTweenPath.paths.Add(this.pathName.ToLower(), this);
		}
	}

	private void OnDrawGizmosSelected()
	{
		if (base.get_enabled() && this.nodes.get_Count() > 0)
		{
			iTween.DrawPath(this.nodes.ToArray(), this.pathColor);
		}
	}

	public static Vector3[] GetPath(string requestedName)
	{
		requestedName = requestedName.ToLower();
		if (iTweenPath.paths.ContainsKey(requestedName))
		{
			return iTweenPath.paths.get_Item(requestedName).nodes.ToArray();
		}
		Debug.Log("No path with that name exists! Are you sure you wrote it correctly?");
		return null;
	}
}
