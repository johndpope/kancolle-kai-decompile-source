using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class UnloadAtlas : MonoBehaviour
{
	[SerializeField]
	private UIAtlas[] ManualDestroyAtlases;

	[SerializeField]
	private Texture[] ManualDestroyTextures;

	[Button("UnusedUnload", "UnusedUnload", new object[]
	{

	})]
	public int button;

	[Button("EmptyScene", "EmptyScene", new object[]
	{

	})]
	public int button2;

	[DebuggerHidden]
	public IEnumerator Unload()
	{
		UnloadAtlas.<Unload>c__Iterator3E <Unload>c__Iterator3E = new UnloadAtlas.<Unload>c__Iterator3E();
		<Unload>c__Iterator3E.<>f__this = this;
		return <Unload>c__Iterator3E;
	}

	private void UnusedUnload()
	{
		Resources.UnloadUnusedAssets();
	}

	private void EmptyScene()
	{
		Application.LoadLevel("TestEmptyScene");
	}
}
