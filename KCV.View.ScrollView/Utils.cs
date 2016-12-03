using System;
using UnityEngine;

namespace KCV.View.ScrollView
{
	internal class Utils
	{
		public static void RangeTest(int start, int counter, int from, int to)
		{
			counter = Math.Abs(counter);
			for (int i = start; i < counter; i++)
			{
				Debug.Log(string.Concat(new object[]
				{
					"From:",
					from,
					" To:",
					to,
					" Value:",
					i,
					" Range",
					(!Utils.RangeEqualsIn((float)i, (float)from, (float)to)) ? "Over" : "In"
				}));
			}
		}

		public static bool RangeEqualsIn(float currentPosition, float from, float to)
		{
			float num = (float)((int)currentPosition);
			float num2;
			float num3;
			if (from < to)
			{
				num2 = from;
				num3 = to;
			}
			else
			{
				num2 = to;
				num3 = from;
			}
			num2 = (float)((int)num2);
			num3 = (float)((int)num3);
			return num2 <= currentPosition && currentPosition <= num3;
		}

		public static int LoopValue(int value, int min, int max)
		{
			max--;
			if (value < min)
			{
				value = max - (min - value) + 1;
			}
			if (value > max)
			{
				value = min + (value - max) - 1;
			}
			return value;
		}
	}
}
