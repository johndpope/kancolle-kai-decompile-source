using System;
using UnityEngine;
using UnityEngine.PSVita;

public class ExampleRenderTexturePlayback : MonoBehaviour
{
	public string MoviePath;

	public RenderTexture renderTexture;

	public GUISkin skin;

	private bool isPlaying;

	private void Start()
	{
		PSVitaVideoPlayer.Init(this.renderTexture);
		PSVitaVideoPlayer.Play(this.MoviePath, 1, 1);
	}

	private void OnPreRender()
	{
		PSVitaVideoPlayer.Update();
	}

	private void OnGUI()
	{
		GUI.set_skin(this.skin);
		GUILayout.BeginArea(new Rect(10f, 10f, 200f, (float)Screen.get_height()));
		if (GUILayout.Button("Stop/Play", new GUILayoutOption[0]))
		{
			if (this.isPlaying)
			{
				PSVitaVideoPlayer.Stop();
			}
			else
			{
				PSVitaVideoPlayer.Play(this.MoviePath, 1, 1);
			}
		}
		GUILayout.EndArea();
	}

	private void OnMovieEvent(int eventID)
	{
		switch (eventID)
		{
		case 1:
			this.isPlaying = false;
			break;
		case 3:
			this.isPlaying = true;
			break;
		}
	}
}
