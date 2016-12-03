using System;
using UnityEngine;

namespace KCV
{
	public static class ParticleFile
	{
		private const string PARTICLE_PATH = "Particles";

		public static T Load<T>(string path) where T : Component
		{
			return Resources.Load<T>(string.Format("{0}/{1}", "Particles", path));
		}

		public static GameObject Load(ParticleFileInfos particleInfo)
		{
			return Resources.Load(string.Format("{0}/{1}", "Particles", particleInfo.ParticleFilePath())) as GameObject;
		}

		public static T Load<T>(ParticleFileInfos particleInfo) where T : Component
		{
			return Resources.Load<T>(string.Format("{0}/{1}", "Particles", particleInfo.ParticleFilePath()));
		}

		public static T Instantiate<T>(ParticleFileInfos particleInfos) where T : Component
		{
			return Object.Instantiate<T>(ParticleFile.Load<T>(particleInfos));
		}
	}
}
