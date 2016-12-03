using System;
using UnityEngine;

[RequireComponent(typeof(FadeCamera))]
public class FadeSwitch : MonoBehaviour
{
	[Range(0f, 5f)]
	public float time = 1f;

	private static FadeSwitch Instance
	{
		get
		{
			FadeSwitch fadeSwitch = SingletonMonoBehaviour<FadeCamera>.Instance.GetComponent<FadeSwitch>();
			if (fadeSwitch == null)
			{
				fadeSwitch = SingletonMonoBehaviour<FadeCamera>.Instance.get_gameObject().AddComponent<FadeSwitch>();
			}
			return fadeSwitch;
		}
	}

	public static float FadeTime
	{
		get
		{
			return FadeSwitch.Instance.time;
		}
		set
		{
			FadeSwitch.Instance.time = value;
		}
	}

	public static bool IsFadeIn
	{
		get
		{
			return FadeSwitch.Instance.get_enabled();
		}
		set
		{
			FadeSwitch.Instance.set_enabled(value);
		}
	}

	private void OnEnable()
	{
		base.GetComponent<FadeCamera>().FadeIn(this.time, new Action(this.Finished));
	}

	private void OnDisable()
	{
		base.GetComponent<FadeCamera>().FadeOut(this.time, new Action(this.Finished));
	}

	private void Finished()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("GameController");
		if (gameObject != null)
		{
			gameObject.SendMessage("FadeFinished", 1);
		}
	}
}
