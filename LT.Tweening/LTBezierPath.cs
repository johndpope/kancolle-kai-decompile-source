using System;
using UnityEngine;

namespace LT.Tweening
{
	public class LTBezierPath
	{
		public Vector3[] pts;

		public float length;

		public bool orientToPath;

		public bool orientToPath2d;

		private LTBezier[] beziers;

		private float[] lengthRatio;

		private int currentBezier;

		private int previousBezier;

		public LTBezierPath()
		{
		}

		public LTBezierPath(Vector3[] pts_)
		{
			this.setPoints(pts_);
		}

		public void setPoints(Vector3[] pts_)
		{
			if (pts_.Length < 4)
			{
				LeanTween.logError("LeanTween - When passing values for a vector path, you must pass four or more values!");
			}
			if (pts_.Length % 4 != 0)
			{
				LeanTween.logError("LeanTween - When passing values for a vector path, they must be in sets of four: controlPoint1, controlPoint2, endPoint2, controlPoint2, controlPoint2...");
			}
			this.pts = pts_;
			int num = 0;
			this.beziers = new LTBezier[this.pts.Length / 4];
			this.lengthRatio = new float[this.beziers.Length];
			this.length = 0f;
			for (int i = 0; i < this.pts.Length; i += 4)
			{
				this.beziers[num] = new LTBezier(this.pts[i], this.pts[i + 2], this.pts[i + 1], this.pts[i + 3], 0.05f);
				this.length += this.beziers[num].length;
				num++;
			}
			for (int i = 0; i < this.beziers.Length; i++)
			{
				this.lengthRatio[i] = this.beziers[i].length / this.length;
			}
		}

		public Vector3 point(float ratio)
		{
			float num = 0f;
			for (int i = 0; i < this.lengthRatio.Length; i++)
			{
				num += this.lengthRatio[i];
				if (num >= ratio)
				{
					return this.beziers[i].point((ratio - (num - this.lengthRatio[i])) / this.lengthRatio[i]);
				}
			}
			return this.beziers[this.lengthRatio.Length - 1].point(1f);
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
				Gizmos.set_color((this.previousBezier != this.currentBezier) ? Color.get_grey() : Color.get_magenta());
				Gizmos.DrawLine(vector2, vector);
				vector = vector2;
				this.previousBezier = this.currentBezier;
			}
		}
	}
}
