using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV
{
	public static class SoundFile
	{
		private const string BGM_FILE_PATH = "Sounds/BGM/{0}";

		private const string SE_FILE_PATH = "Sounds/SE/{0}";

		private static Dictionary<SEFIleInfos, AudioClip> _dicSEFileDictionary = new Dictionary<SEFIleInfos, AudioClip>();

		public static AudioClip LoadSE(SEFIleInfos file)
		{
			if (SoundFile._dicSEFileDictionary.ContainsKey(file))
			{
				if (SoundFile._dicSEFileDictionary.get_Item(file) != null)
				{
					return SoundFile._dicSEFileDictionary.get_Item(file);
				}
				AudioClip audioClip = Resources.Load(string.Format("Sounds/SE/{0}", file.SEFileName())) as AudioClip;
				if (audioClip == null)
				{
					return null;
				}
				SoundFile._dicSEFileDictionary.set_Item(file, audioClip);
			}
			else
			{
				string text = string.Format("Sounds/SE/{0}", file.SEFileName());
				AudioClip audioClip = Resources.Load(string.Format("Sounds/SE/{0}", file.SEFileName())) as AudioClip;
				if (audioClip == null)
				{
					return null;
				}
				SoundFile._dicSEFileDictionary.Add(file, audioClip);
			}
			return SoundFile._dicSEFileDictionary.get_Item(file);
		}

		public static void ClearAllSE()
		{
			SoundFile._dicSEFileDictionary.Clear();
		}

		public static AudioClip LoadBGM(BGMFileInfos file)
		{
			return Resources.Load(string.Format("Sounds/BGM/{0}", file.BGMFileName())) as AudioClip;
		}

		public static AudioClip LoadBGM(int bgmNum)
		{
			return Resources.Load(string.Format("Sounds/BGM/{0}", bgmNum)) as AudioClip;
		}
	}
}
