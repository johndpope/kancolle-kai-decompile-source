using System;
using UnityEngine;

namespace KCV
{
	public class MiscCalculateUtils
	{
		public static Vector3[] CalculateBezierPoint(Vector3 src1, Vector3 src2, float length)
		{
			DebugUtils.SLog("CalculateBezierPoint");
			Vector3 center = (src1 + src2) / 2f;
			float num = length / Vector3.Distance(src1, src2);
			Vector3[] array = new Vector3[]
			{
				MiscCalculateUtils.Rotate90AndScale(src1, center, num),
				MiscCalculateUtils.Rotate90AndScale(src2, center, num)
			};
			DebugUtils.SLog("=========================================");
			DebugUtils.SLog(string.Concat(new object[]
			{
				"p1 x,y=",
				src1.x,
				",",
				src1.y
			}));
			DebugUtils.SLog(string.Concat(new object[]
			{
				"p2 x,y=",
				src2.x,
				",",
				src2.y
			}));
			DebugUtils.SLog(string.Concat(new object[]
			{
				"[center]x,y=",
				center.x,
				",",
				center.y,
				"[scale]",
				num
			}));
			DebugUtils.SLog(string.Concat(new object[]
			{
				"x,y=",
				array[0].x,
				",",
				array[0].y,
				", x,y=",
				array[1].x,
				",",
				array[1].y
			}));
			DebugUtils.SLog("=========================================");
			return array;
		}

		private static Vector3 Rotate90AndScale(Vector3 src, Vector3 center, float scale)
		{
			float num = -(src.y - center.y) * scale + center.x;
			float num2 = (src.x - center.x) * scale + center.y;
			return new Vector3(num, num2, 0f);
		}
	}
}
