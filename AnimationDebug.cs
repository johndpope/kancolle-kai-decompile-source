using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationDebug : MonoBehaviour
{
	public Animation anim;

	public int animNo;

	[Button("Play", "Play", new object[]
	{

	})]
	public int Button1;

	public List<string> AnimationList;

	private void Start()
	{
		this.anim = base.GetComponent<Animation>();
		this.AnimationList = new List<string>();
		using (IEnumerator enumerator = this.anim.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				AnimationState animationState = (AnimationState)enumerator.get_Current();
				this.AnimationList.Add(animationState.get_name());
			}
		}
	}

	public void Play()
	{
		this.anim.Play(this.AnimationList.get_Item(this.animNo));
	}

	private void OnDestroy()
	{
		this.anim = null;
		this.AnimationList.Clear();
		this.AnimationList = null;
	}
}
