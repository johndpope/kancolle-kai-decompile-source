using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class TutorialYouseiAnimation : MonoBehaviour
{
	[SerializeField]
	private Texture ayeOpen;

	[SerializeField]
	private Texture ayeClose;

	private readonly Vector3 position = new Vector3(393f, -136f, 0f);

	[DebuggerHidden]
	private IEnumerator Start()
	{
		TutorialYouseiAnimation.<Start>c__Iterator43 <Start>c__Iterator = new TutorialYouseiAnimation.<Start>c__Iterator43();
		<Start>c__Iterator.<>f__this = this;
		return <Start>c__Iterator;
	}
}
