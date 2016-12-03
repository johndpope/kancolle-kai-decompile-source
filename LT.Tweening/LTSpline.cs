using System;
using UnityEngine;

namespace LT.Tweening
{
	[Serializable]
	public class LTSpline
	{
		public static int DISTANCE_COUNT = 30;

		public static int SUBLINE_COUNT = 50;

		public Vector3[] pts;

		public Vector3[] ptsAdj;

		public int ptsAdjLength;

		public bool orientToPath;

		public bool orientToPath2d;

		private int numSections;

		private int currPt;

		private float totalLength;

		public LTSpline(params Vector3[] pts)
		{
			this.pts = new Vector3[pts.Length];
			Array.Copy(pts, this.pts, pts.Length);
			this.numSections = pts.Length - 3;
			float num = float.PositiveInfinity;
			Vector3 vector = this.pts[1];
			float num2 = 0f;
			for (int i = 2; i < this.pts.Length - 2; i++)
			{
				float num3 = Vector3.Distance(this.pts[i], vector);
				if (num3 < num)
				{
					num = num3;
				}
				num2 += num3;
			}
			float num4 = num / (float)LTSpline.SUBLINE_COUNT;
			int num5 = (int)Mathf.Ceil(num2 / num4) * LTSpline.DISTANCE_COUNT;
			this.ptsAdj = new Vector3[num5];
			vector = this.interp(0f);
			int num6 = 0;
			for (int j = 0; j < num5; j++)
			{
				float t = ((float)j + 1f) / (float)num5;
				Vector3 vector2 = this.interp(t);
				float num7 = Vector3.Distance(vector2, vector);
				if (num7 >= num4)
				{
					this.ptsAdj[num6] = vector2;
					vector = vector2;
					num6++;
				}
			}
			this.ptsAdjLength = num6;
		}

		public Vector3 map(float u)
		{
			if (u >= 1f)
			{
				return this.pts[this.pts.Length - 2];
			}
			float num = u * (float)(this.ptsAdjLength - 1);
			int num2 = (int)Mathf.Floor(num);
			int num3 = (int)Mathf.Ceil(num);
			Vector3 vector = this.ptsAdj[num2];
			Vector3 vector2 = this.ptsAdj[num3];
			float num4 = num - (float)num2;
			return vector + (vector2 - vector) * num4;
		}

		public Vector3 interp(float t)
		{
			this.currPt = Mathf.Min(Mathf.FloorToInt(t * (float)this.numSections), this.numSections - 1);
			float num = t * (float)this.numSections - (float)this.currPt;
			Vector3 vector = this.pts[this.currPt];
			Vector3 vector2 = this.pts[this.currPt + 1];
			Vector3 vector3 = this.pts[this.currPt + 2];
			Vector3 vector4 = this.pts[this.currPt + 3];
			return 0.5f * ((-vector + 3f * vector2 - 3f * vector3 + vector4) * (num * num * num) + (2f * vector - 5f * vector2 + 4f * vector3 - vector4) * (num * num) + (-vector + vector3) * num + 2f * vector2);
		}

		public Vector3 point(float ratio)
		{
			float u = (ratio <= 1f) ? ratio : 1f;
			return this.map(u);
		}

		public void place2d(Transform transform, float ratio)
		{
			transform.set_position(this.point(ratio));
			ratio += 0.001f;
			if (ratio <= 1f)
			{
				Vector3 vector = this.point(ratio) - transform.get_position();
				float num = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
				transform.set_eulerAngles(new Vector3(0f, 0f, num));
			}
		}

		public void placeLocal2d(Transform transform, float ratio)
		{
			transform.set_localPosition(this.point(ratio));
			ratio += 0.001f;
			if (ratio <= 1f)
			{
				Vector3 vector = transform.get_parent().TransformPoint(this.point(ratio)) - transform.get_localPosition();
				float num = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
				transform.set_eulerAngles(new Vector3(0f, 0f, num));
			}
		}

		public void place(Transform transform, float ratio)
		{
			this.place(transform, ratio, Vector3.get_up());
		}

		public void place(Transform transform, float ratio, Vector3 worldUp)
		{
			transform.set_position(this.point(ratio));
			ratio += 0.001f;
			if (ratio <= 1f)
			{
				transform.LookAt(this.point(ratio), worldUp);
			}
		}

		public void placeLocal(Transform transform, float ratio)
		{
			this.placeLocal(transform, ratio, Vector3.get_up());
		}

		public void placeLocal(Transform transform, float ratio, Vector3 worldUp)
		{
			transform.set_localPosition(this.point(ratio));
			ratio += 0.001f;
			if (ratio <= 1f)
			{
				transform.LookAt(transform.get_parent().TransformPoint(this.point(ratio)), worldUp);
			}
		}

		public void gizmoDraw(float t = -1f)
		{
			Vector3 vector = this.point(0f);
			for (int i = 1; i <= 120; i++)
			{
				float ratio = (float)i / 120f;
				Vector3 vector2 = this.point(ratio);
				Gizmos.DrawLine(vector2, vector);
				vector = vector2;
			}
		}
	}
}
