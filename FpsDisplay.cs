using System;
using UnityEngine;

public class FpsDisplay : MonoBehaviour
{
	private float updateInterval = 0.5f;

	private float accum;

	private float frames;

	private float timeleft;

	private void Start()
	{
		if (!base.GetComponent<GUIText>())
		{
			MonoBehaviour.print("FramesPerSecond needs a GUIText component!");
			base.set_enabled(false);
			return;
		}
		this.timeleft = this.updateInterval;
	}

	private void Update()
	{
		this.timeleft -= Time.get_deltaTime();
		this.accum += Time.get_timeScale() / Time.get_deltaTime();
		this.frames += 1f;
		if ((double)this.timeleft <= 0.0)
		{
			base.GetComponent<GUIText>().set_text("FrameRate = " + (this.accum / this.frames).ToString("f2"));
			this.timeleft = this.updateInterval;
			this.accum = 0f;
			this.frames = 0f;
		}
	}
}
