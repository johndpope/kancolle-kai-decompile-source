using System;
using UnityEngine;

public class AppDataPath
{
	public static string ResourcesFilePath
	{
		get
		{
			return string.Format("{0}/{1}", Application.get_dataPath(), "Resources");
		}
	}

	public static string PrefabFilePath
	{
		get
		{
			return string.Format("{0}", "Prefabs");
		}
	}

	public static string ParticleFilePath
	{
		get
		{
			return string.Format("{0}", "Particles");
		}
	}

	public static string TextureFilePath
	{
		get
		{
			return string.Format("{0}", "Textures");
		}
	}

	public static string ShipTexturePath
	{
		get
		{
			return string.Format("{0}/{1}", AppDataPath.TextureFilePath, "Ships");
		}
	}

	public static string ShaderFilePath
	{
		get
		{
			return string.Format("{0}", "Shader");
		}
	}

	public static string AnimationFilePath
	{
		get
		{
			return string.Format("{0}", "Animations");
		}
	}

	public static string SoundFilePath
	{
		get
		{
			return string.Format("{0}", "Sounds");
		}
	}

	public static string BGMFilePath
	{
		get
		{
			return string.Format("{0}/{1}", AppDataPath.SoundFilePath, "BGM");
		}
	}

	public static string SEFilePath
	{
		get
		{
			return string.Format("{0}/{1}", AppDataPath.SoundFilePath, "SE");
		}
	}

	public static string ShipVoicePath
	{
		get
		{
			return string.Format("{0}/{1}", AppDataPath.SoundFilePath, "Voice");
		}
	}
}
