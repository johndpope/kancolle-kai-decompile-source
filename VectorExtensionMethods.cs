using System;
using UnityEngine;

public static class VectorExtensionMethods
{
	private static Vector3 _vVec;

	public static void Add(this Vector3 vector, Vector3 v0)
	{
		vector = Mathe.Add(vector, v0);
	}

	public static void Sub(this Vector3 vector, Vector3 v0)
	{
		vector = Mathe.Sub(vector, v0);
	}

	public static Vector3 ScrSide2Vector(this Vector3 vector, ExtensionUtils.Side side)
	{
		switch (side)
		{
		case ExtensionUtils.Side.BottomLeft:
			VectorExtensionMethods._vVec.Set(0f, (float)Screen.get_height(), 0f);
			break;
		case ExtensionUtils.Side.Left:
			VectorExtensionMethods._vVec.Set(0f, (float)(Screen.get_height() / 2), 0f);
			break;
		case ExtensionUtils.Side.TopLeft:
			VectorExtensionMethods._vVec.Set(0f, 0f, 0f);
			break;
		case ExtensionUtils.Side.Top:
			VectorExtensionMethods._vVec.Set((float)(Screen.get_width() / 2), 0f, 0f);
			break;
		case ExtensionUtils.Side.TopRight:
			VectorExtensionMethods._vVec.Set((float)Screen.get_width(), 0f, 0f);
			break;
		case ExtensionUtils.Side.Right:
			VectorExtensionMethods._vVec.Set((float)Screen.get_width(), (float)(Screen.get_height() / 2), 0f);
			break;
		case ExtensionUtils.Side.BottomRight:
			VectorExtensionMethods._vVec.Set((float)Screen.get_width(), (float)Screen.get_height(), 0f);
			break;
		case ExtensionUtils.Side.Bottom:
			VectorExtensionMethods._vVec.Set((float)(Screen.get_width() / 2), (float)Screen.get_height(), 0f);
			break;
		case ExtensionUtils.Side.Center:
			VectorExtensionMethods._vVec.Set((float)(Screen.get_width() / 2), (float)(Screen.get_height() / 2), 0f);
			break;
		default:
			VectorExtensionMethods._vVec = vector;
			break;
		}
		return vector = VectorExtensionMethods._vVec;
	}

	public static Vector3 ScrSideCenter(this Vector3 vector)
	{
		VectorExtensionMethods._vVec.Set((float)(Screen.get_width() / 2), (float)(Screen.get_height() / 2), 0f);
		return VectorExtensionMethods._vVec;
	}

	public static Vector3 Zero(this Vector3 vector)
	{
		return Vector3.get_zero();
	}

	public static Vector3 Back(this Vector3 vector)
	{
		return Vector3.get_back();
	}

	public static Vector3 Down(this Vector3 vector)
	{
		return Vector3.get_down();
	}

	public static Vector3 Forward(this Vector3 vector)
	{
		return Vector3.get_forward();
	}

	public static Vector3 Left(this Vector3 vector)
	{
		return Vector3.get_left();
	}

	public static Vector3 One(this Vector3 vector)
	{
		return Vector3.get_one();
	}

	public static Vector3 Right(this Vector3 vector)
	{
		return Vector3.get_right();
	}

	public static Vector3 Up(this Vector3 vector)
	{
		return Vector3.get_up();
	}

	public static Vector3 Sync(this Vector3 vector, Vector3 target)
	{
		return target;
	}

	public static Vector3 Sync(this Vector3 vector, GameObject obj)
	{
		return obj.get_gameObject().get_transform().get_position();
	}

	public static Vector3 Flash2Vector3(this Vector3 vector)
	{
		return new Vector3(vector.x, vector.y * -1f, vector.z);
	}
}
