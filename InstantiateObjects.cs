using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class InstantiateObjects : MonoBehaviour
{
	[Range(0.1f, 1f)]
	public float interval = 1f;

	public GameObject prefab;

	private void Awake()
	{
		this.prefab.SetActive(false);
	}

	[DebuggerHidden]
	private IEnumerator Start()
	{
		InstantiateObjects.<Start>c__Iterator1 <Start>c__Iterator = new InstantiateObjects.<Start>c__Iterator1();
		<Start>c__Iterator.<>f__this = this;
		return <Start>c__Iterator;
	}
}
