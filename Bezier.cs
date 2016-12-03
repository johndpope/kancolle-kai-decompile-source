using System;
using UnityEngine;

public class Bezier
{
	public enum BezierType
	{
		Quadratic,
		Cubic
	}

	private Vector3 _vPosStart;

	private Vector3 _vPosEnd;

	private Vector3 _vPosMid1;

	private Vector3 _vPosMid2;

	private Bezier.BezierType _iType;

	public Bezier(Bezier.BezierType type, Vector3 start, Vector3 end, Vector3 mid1, Vector3 mid2)
	{
		this._iType = type;
		this._vPosStart = start;
		this._vPosEnd = end;
		this._vPosMid1 = mid1;
		this._vPosMid2 = mid2;
	}

	public Vector3 Interpolate(float t)
	{
		float num = 1f - t;
		float num2 = t * t;
		float num3 = num * num;
		Bezier.BezierType iType = this._iType;
		Vector3 zero;
		if (iType != Bezier.BezierType.Quadratic)
		{
			if (iType != Bezier.BezierType.Cubic)
			{
				zero = Vector3.get_zero();
			}
			else
			{
				float num4 = num * num3;
				float num5 = 3f * t * num3;
				float num6 = 3f * num2 * num;
				float num7 = num2 * t;
				zero.x = num4 * this._vPosStart.x + num5 * this._vPosMid1.x + num6 * this._vPosMid2.x + num7 * this._vPosEnd.x;
				zero.y = num4 * this._vPosStart.y + num5 * this._vPosMid1.y + num6 * this._vPosMid2.y + num7 * this._vPosEnd.y;
				zero.z = num4 * this._vPosStart.z + num5 * this._vPosMid1.z + num6 * this._vPosMid2.z + num7 * this._vPosEnd.z;
			}
		}
		else
		{
			zero.x = num2 * this._vPosEnd.x + 2f * t * num * this._vPosMid1.x + num3 * this._vPosStart.x;
			zero.y = num2 * this._vPosEnd.y + 2f * t * num * this._vPosMid1.y + num3 * this._vPosStart.y;
			zero.z = num2 * this._vPosEnd.z + 2f * t * num * this._vPosMid1.z + num3 * this._vPosStart.z;
		}
		return zero;
	}

	public float DistanceLinear()
	{
		return Vector3.Distance(this._vPosStart, this._vPosEnd);
	}

	public static Vector3 Interpolate(ref Vector3 pvr, Vector3 startPos, Vector3 endPos, float ft, Vector3 midPos1, Vector3 midPos2)
	{
		float num = 1f - ft;
		float num2 = ft * ft;
		float num3 = num * num;
		float num4 = num * num3;
		float num5 = 3f * ft * num3;
		float num6 = 3f * num2 * num;
		float num7 = num2 * ft;
		pvr.x = num4 * startPos.x + num5 * midPos1.x + num6 * midPos2.x + num7 * endPos.x;
		pvr.y = num4 * startPos.y + num5 * midPos1.y + num6 * midPos2.y + num7 * endPos.y;
		pvr.z = num4 * startPos.z + num5 * midPos1.z + num6 * midPos2.z + num7 * endPos.z;
		return pvr;
	}
}
