using System;
using UnityEngine;

namespace KCV
{
	public static class TextureFile
	{
		public static Texture2D LoadRareBG(int nRare)
		{
			return Resources.Load(string.Format("Textures/Common/RareBG/s_rare_{0}", nRare)) as Texture2D;
		}

		public static Texture2D LoadCardRareBG(int nRare)
		{
			return Resources.Load<Texture2D>(string.Format("Textures/Common/RareBG/c_rare_{0}", nRare));
		}

		public static Texture2D LoadOverlay()
		{
			return Resources.Load("Textures/Common/Overlay") as Texture2D;
		}
	}
}
