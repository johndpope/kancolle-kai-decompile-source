using System;
using UnityEngine;

namespace Librarys.UnitySettings
{
	public sealed class Fog
	{
		public static bool fog
		{
			get
			{
				return RenderSettings.get_fog();
			}
			set
			{
				RenderSettings.set_fog(value);
			}
		}

		public static Color fogColor
		{
			get
			{
				return RenderSettings.get_fogColor();
			}
			set
			{
				RenderSettings.set_fogColor(value);
			}
		}

		public static float fogDensity
		{
			get
			{
				return RenderSettings.get_fogDensity();
			}
			set
			{
				RenderSettings.set_fogDensity(value);
			}
		}

		public static float fogStartDistance
		{
			get
			{
				return RenderSettings.get_fogStartDistance();
			}
			set
			{
				RenderSettings.set_fogStartDistance(value);
			}
		}

		public static float fogEndDistance
		{
			get
			{
				return RenderSettings.get_fogEndDistance();
			}
			set
			{
				RenderSettings.set_fogEndDistance(value);
			}
		}

		public static FogMode fogMode
		{
			get
			{
				return RenderSettings.get_fogMode();
			}
			set
			{
				RenderSettings.set_fogMode(value);
			}
		}
	}
}
