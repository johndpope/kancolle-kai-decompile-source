using System;
using UnityEngine;

public class FadeGUI : MonoBehaviour
{
	public Texture maskTexture;

	private void Start()
	{
		SingletonMonoBehaviour<FadeCamera>.Instance.UpdateMaskTexture(this.maskTexture);
	}

	private void OnGUI()
	{
		if (!SingletonMonoBehaviour<FadeCamera>.Instance.fading)
		{
			if (GUILayout.Button("switch", new GUILayoutOption[0]))
			{
				FadeSwitch.IsFadeIn = !FadeSwitch.IsFadeIn;
			}
			FadeSwitch.FadeTime = GUILayout.HorizontalSlider(FadeSwitch.FadeTime, 0f, 5f, new GUILayoutOption[]
			{
				GUILayout.Width(300f)
			});
			GUILayout.Label("fade time:" + FadeSwitch.FadeTime, new GUILayoutOption[0]);
		}
	}

	private void FadeFinished()
	{
		Debug.Log("finished");
	}
}
