using System;
using UnityEngine;
using UnityEngine.PSVita;

public class ExampleFullScreenPlayback : MonoBehaviour
{
	public string MoviePath;

	public RenderTexture renderTexture;

	public GUISkin skin;

	public float volume = 1f;

	public int audioStreamIndex;

	private int audioStreamMaxIndex = 4;

	public int textStreamIndex;

	private int textStreamMaxIndex = 4;

	private GUIStyle timeTextStyle;

	private GUIStyle subtitleTextStyle;

	private string subtitleText = string.Empty;

	private long subtitleTimeStamp;

	public bool isPlaying;

	private void Start()
	{
		PSVitaVideoPlayer.Init(this.renderTexture);
		PSVitaVideoPlayer.PlayParams playParams = default(PSVitaVideoPlayer.PlayParams);
		playParams.volume = this.volume;
		playParams.loopSetting = 1;
		playParams.modeSetting = 0;
		playParams.audioStreamIndex = this.audioStreamIndex;
		playParams.textStreamIndex = this.textStreamIndex;
		PSVitaVideoPlayer.PlayEx(this.MoviePath, playParams);
	}

	private void OnPostRender()
	{
		PSVitaVideoPlayer.Update();
	}

	private void OnMovieEvent(int eventID)
	{
		switch (eventID)
		{
		case 1:
			this.isPlaying = false;
			this.subtitleText = string.Empty;
			return;
		case 2:
			IL_18:
			if (eventID != 16)
			{
				return;
			}
			this.subtitleText = PSVitaVideoPlayer.get_subtitleText();
			this.subtitleTimeStamp = PSVitaVideoPlayer.get_subtitleTimeStamp();
			return;
		case 3:
			this.isPlaying = true;
			return;
		}
		goto IL_18;
	}
}
