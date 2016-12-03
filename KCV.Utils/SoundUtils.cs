using System;
using UnityEngine;

namespace KCV.Utils
{
	public class SoundUtils
	{
		private static SEFIleInfos prevSE;

		private static int prevSETime;

		public static AudioClip PreloadBGMFile(BGMFileInfos iInfos)
		{
			return SoundFile.LoadBGM(iInfos);
		}

		public static AudioSource PlaySaveBGM(AudioClip clip, bool isLoop, bool isSave)
		{
			if (isSave)
			{
				SoundUtils.SaveBGM(clip);
			}
			if (SingletonMonoBehaviour<SoundManager>.Instance != null)
			{
				return SingletonMonoBehaviour<SoundManager>.Instance.PlayBGM(clip, isLoop);
			}
			return null;
		}

		public static AudioSource PlaySaveBGM(BGMFileInfos info, bool isLoop, bool isSave)
		{
			if (isSave)
			{
				SoundUtils.SaveBGM(info);
			}
			if (SingletonMonoBehaviour<SoundManager>.Instance != null)
			{
				return SingletonMonoBehaviour<SoundManager>.Instance.PlayBGM(info, isLoop);
			}
			return null;
		}

		public static AudioSource PlaySceneBGM(BGMFileInfos info)
		{
			if (SingletonMonoBehaviour<AppInformation>.Instance != null && SingletonMonoBehaviour<AppInformation>.Instance.currentPlayingBgmID != (int)info)
			{
				return SoundUtils.PlayBGM(info, true);
			}
			return null;
		}

		public static AudioSource PlayBGM(AudioClip clip, bool isLoop)
		{
			return SoundUtils.PlaySaveBGM(clip, isLoop, false);
		}

		public static AudioSource SwitchBGM(int nIndex)
		{
			if (SingletonMonoBehaviour<SoundManager>.Instance != null)
			{
				return SingletonMonoBehaviour<SoundManager>.Instance.SwitchBGM((BGMFileInfos)nIndex);
			}
			return null;
		}

		public static AudioSource SwitchBGM(BGMFileInfos bgmFileInfo)
		{
			if (SingletonMonoBehaviour<SoundManager>.Instance != null)
			{
				return SingletonMonoBehaviour<SoundManager>.Instance.SwitchBGM(bgmFileInfo);
			}
			return null;
		}

		public static AudioSource SwitchBGM(AudioClip bgmFile)
		{
			if (SingletonMonoBehaviour<SoundManager>.Instance != null)
			{
				return SingletonMonoBehaviour<SoundManager>.Instance.SwitchBGM(bgmFile);
			}
			return null;
		}

		public static AudioSource PlayBGM(BGMFileInfos info, bool isLoop)
		{
			return SoundUtils.PlaySaveBGM(info, isLoop, false);
		}

		private static bool SaveBGM(AudioClip clip)
		{
			if (SingletonMonoBehaviour<AppInformation>.Instance != null && clip != null)
			{
				int currentPlayingBgmID = -1;
				int.TryParse(clip.get_name(), ref currentPlayingBgmID);
				SingletonMonoBehaviour<AppInformation>.Instance.currentPlayingBgmID = currentPlayingBgmID;
				return true;
			}
			return false;
		}

		private static bool SaveBGM(BGMFileInfos info)
		{
			if (SingletonMonoBehaviour<AppInformation>.Instance != null)
			{
				SingletonMonoBehaviour<AppInformation>.Instance.currentPlayingBgmID = (int)info;
				return true;
			}
			return false;
		}

		public static AudioSource StopBGM()
		{
			if (SingletonMonoBehaviour<SoundManager>.Instance != null)
			{
				return SingletonMonoBehaviour<SoundManager>.Instance.StopBGM();
			}
			return null;
		}

		public static AudioSource StopFadeBGM(float duration, Action onFinished)
		{
			if (SingletonMonoBehaviour<SoundManager>.Instance != null)
			{
				return SingletonMonoBehaviour<SoundManager>.Instance.StopFadeBGM(duration, onFinished);
			}
			return null;
		}

		public static AudioClip PreloadSEFile(SEFIleInfos iInfos)
		{
			return SoundFile.LoadSE(iInfos);
		}

		public static AudioSource PlaySE(AudioClip clip)
		{
			if (SingletonMonoBehaviour<SoundManager>.Instance != null)
			{
				return SingletonMonoBehaviour<SoundManager>.Instance.PlaySE(clip, false, null);
			}
			return null;
		}

		public static AudioSource PlayOneShotSE(SEFIleInfos info)
		{
			if (SingletonMonoBehaviour<SoundManager>.Instance != null)
			{
				return SingletonMonoBehaviour<SoundManager>.Instance.PlayOneShotSE(info);
			}
			return null;
		}

		public static AudioSource PlaySE(SEFIleInfos info)
		{
			return SoundUtils.PlaySE(info, null);
		}

		public static AudioSource PlaySE(SEFIleInfos info, Action onFinished)
		{
			if (SingletonMonoBehaviour<SoundManager>.Instance != null)
			{
				if (Time.get_frameCount() != SoundUtils.prevSETime || SoundUtils.prevSE == info)
				{
				}
				SoundUtils.prevSE = info;
				SoundUtils.prevSETime = Time.get_frameCount();
				return SingletonMonoBehaviour<SoundManager>.Instance.PlaySE(info, onFinished);
			}
			return null;
		}

		public static void StopSE(float fDuration, params AudioSource[] sources)
		{
			if (SingletonMonoBehaviour<SoundManager>.Instance != null)
			{
				sources.ForEach(delegate(AudioSource x)
				{
					SingletonMonoBehaviour<SoundManager>.Instance.StopSE(x, false, fDuration);
				});
			}
		}

		public static void StopSEAll(float fDuration)
		{
			if (SingletonMonoBehaviour<SoundManager>.Instance != null)
			{
				SingletonMonoBehaviour<SoundManager>.Instance.StopAllSE(fDuration);
			}
		}
	}
}
