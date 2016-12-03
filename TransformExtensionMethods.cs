using System;
using System.Collections;
using UnityEngine;

public static class TransformExtensionMethods
{
	private static Vector3 _vVec;

	public static T AddComponent<T>(this Transform transform) where T : Component
	{
		return transform.get_gameObject().AddComponent<T>();
	}

	public static T GetComponent<T>(this Transform transform) where T : Component
	{
		return transform.GetComponent<T>() ?? transform.AddComponent<T>();
	}

	public static Component GetComponent(this Transform transform, Type componentType)
	{
		return transform.GetComponent(componentType) ?? transform.get_gameObject().AddComponent(componentType);
	}

	public static Transform Sync(this Transform transform, Transform target)
	{
		return target;
	}

	public static Transform Sync(this Transform transform, GameObject target)
	{
		return target.get_transform();
	}

	public static Transform LookAt2D(this Transform transform, Transform target)
	{
		Vector3 vector = target.get_transform().get_position() - transform.get_position();
		float num = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
		transform.set_rotation(Quaternion.AngleAxis(num, Vector3.get_forward()));
		return transform;
	}

	public static void LookAt2D(this Transform self, Transform target, Vector2 forward)
	{
		self.LookAt2D(target.get_position(), forward);
	}

	public static void LookAt2D(this Transform self, Vector3 target, Vector2 forward)
	{
		float forwardDiffPoint = TransformExtensionMethods.GetForwardDiffPoint(forward);
		Vector3 vector = target - self.get_position();
		float num = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
		self.set_rotation(Quaternion.AngleAxis(num - forwardDiffPoint, Vector3.get_forward()));
	}

	private static float GetForwardDiffPoint(Vector2 forward)
	{
		if (object.Equals(forward, Vector2.get_up()))
		{
			return 90f;
		}
		if (object.Equals(forward, Vector2.get_right()))
		{
			return 0f;
		}
		return 0f;
	}

	public static Vector3 ScreenPoint(this Transform transform, Camera cam)
	{
		return cam.WorldToScreenPoint(transform.get_position());
	}

	public static Vector3 position(this Transform transform, float x, float y, float z)
	{
		TransformExtensionMethods._vVec.Set(x, y, z);
		Vector3 vVec = TransformExtensionMethods._vVec;
		transform.set_position(vVec);
		return vVec;
	}

	public static Vector3 position(this Transform transform, Vector3 vpos)
	{
		return transform.position(vpos.x, vpos.y, vpos.z);
	}

	public static Vector3 position(this Transform transform, ExtensionUtils.Axis iaxis, float pos)
	{
		switch (iaxis)
		{
		case ExtensionUtils.Axis.AxisX:
			TransformExtensionMethods._vVec = transform.position(pos, transform.get_position().y, transform.get_position().z);
			break;
		case ExtensionUtils.Axis.AxisY:
			TransformExtensionMethods._vVec = transform.position(transform.get_position().x, pos, transform.get_position().z);
			break;
		case ExtensionUtils.Axis.AxisZ:
			TransformExtensionMethods._vVec = transform.position(transform.get_position().x, transform.get_position().y, pos);
			break;
		case ExtensionUtils.Axis.AxisAll:
			TransformExtensionMethods._vVec = transform.position(pos, pos, pos);
			break;
		default:
			TransformExtensionMethods._vVec = transform.get_position();
			break;
		}
		return transform.position(TransformExtensionMethods._vVec);
	}

	public static Vector3 positionX(this Transform transform, float x = 0f)
	{
		return transform.position(x, transform.get_position().y, transform.get_position().z);
	}

	public static Vector3 positionY(this Transform transform, float y = 0f)
	{
		return transform.position(transform.get_position().x, y, transform.get_position().z);
	}

	public static Vector3 positionZ(this Transform transdorm, float z = 0f)
	{
		return transdorm.position(transdorm.get_position().x, transdorm.get_position().y, z);
	}

	public static Vector3 positionZero(this Transform transform)
	{
		Vector3 zero = Vector3.get_zero();
		transform.set_position(zero);
		return zero;
	}

	public static Vector3 AddPos(this Transform transform, float x = 0f, float y = 0f, float z = 0f)
	{
		TransformExtensionMethods._vVec.Set(transform.get_position().x + x, transform.get_position().y + y, transform.get_position().z + z);
		return transform.position(TransformExtensionMethods._vVec);
	}

	public static Vector3 AddPos(this Transform transform, Vector3 vpos)
	{
		return transform.AddPos(vpos.x, vpos.y, vpos.z);
	}

	public static Vector3 AddPos(this Transform transform, ExtensionUtils.Axis iaxis, float pos)
	{
		switch (iaxis)
		{
		case ExtensionUtils.Axis.AxisX:
			TransformExtensionMethods._vVec.Set(pos, 0f, 0f);
			break;
		case ExtensionUtils.Axis.AxisY:
			TransformExtensionMethods._vVec.Set(0f, pos, 0f);
			break;
		case ExtensionUtils.Axis.AxisZ:
			TransformExtensionMethods._vVec.Set(0f, 0f, pos);
			break;
		case ExtensionUtils.Axis.AxisAll:
			TransformExtensionMethods._vVec.Set(pos, pos, pos);
			break;
		default:
			TransformExtensionMethods._vVec.Set(0f, 0f, 0f);
			break;
		}
		return transform.AddPos(TransformExtensionMethods._vVec);
	}

	public static Vector3 AddPosX(this Transform transform, float x)
	{
		return transform.AddPos(x, 0f, 0f);
	}

	public static Vector3 AddPosY(this Transform transform, float y)
	{
		return transform.AddPos(0f, y, 0f);
	}

	public static Vector3 AddPosZ(this Transform transform, float z)
	{
		return transform.AddPos(0f, 0f, z);
	}

	public static Vector3 localPosition(this Transform transform, float x, float y, float z)
	{
		TransformExtensionMethods._vVec.Set(x, y, z);
		Vector3 vVec = TransformExtensionMethods._vVec;
		transform.set_localPosition(vVec);
		return vVec;
	}

	public static Vector3 localPosition(this Transform transform, Vector3 vpos)
	{
		return transform.localPosition(vpos.x, vpos.y, vpos.z);
	}

	public static Vector3 localPosition(this Transform transform, ExtensionUtils.Axis iaxis, float pos)
	{
		switch (iaxis)
		{
		case ExtensionUtils.Axis.AxisX:
			TransformExtensionMethods._vVec.Set(pos, transform.get_localPosition().y, transform.get_localPosition().z);
			break;
		case ExtensionUtils.Axis.AxisY:
			TransformExtensionMethods._vVec.Set(transform.get_localPosition().x, pos, transform.get_localPosition().z);
			break;
		case ExtensionUtils.Axis.AxisZ:
			TransformExtensionMethods._vVec.Set(transform.get_localPosition().x, transform.get_localPosition().y, pos);
			break;
		case ExtensionUtils.Axis.AxisAll:
			TransformExtensionMethods._vVec.Set(pos, pos, pos);
			break;
		default:
			TransformExtensionMethods._vVec = transform.get_localPosition();
			break;
		}
		return transform.localPosition(TransformExtensionMethods._vVec);
	}

	public static Vector3 localPositionX(this Transform transform, float x)
	{
		return transform.localPosition(ExtensionUtils.Axis.AxisX, x);
	}

	public static Vector3 localPositionY(this Transform transform, float y)
	{
		return transform.localPosition(ExtensionUtils.Axis.AxisY, y);
	}

	public static Vector3 localPositionZ(this Transform transform, float z)
	{
		return transform.localPosition(ExtensionUtils.Axis.AxisZ, z);
	}

	public static Vector3 localPositionZero(this Transform transform)
	{
		return transform.localPosition(0f, 0f, 0f);
	}

	public static Vector3 AddLocalPosition(this Transform transform, float x = 0f, float y = 0f, float z = 0f)
	{
		TransformExtensionMethods._vVec.Set(transform.get_localPosition().x + x, transform.get_localPosition().y + y, transform.get_localPosition().z + z);
		Vector3 vVec = TransformExtensionMethods._vVec;
		transform.set_localPosition(vVec);
		return vVec;
	}

	public static Vector3 AddLocalPosition(this Transform transform, Vector3 vpos)
	{
		return transform.AddLocalPosition(vpos.x, vpos.y, vpos.z);
	}

	public static Vector3 AddLocalPosition(this Transform transform, ExtensionUtils.Axis iaxis = ExtensionUtils.Axis.None, float pos = 0f)
	{
		switch (iaxis)
		{
		case ExtensionUtils.Axis.AxisX:
			TransformExtensionMethods._vVec.Set(pos, 0f, 0f);
			break;
		case ExtensionUtils.Axis.AxisY:
			TransformExtensionMethods._vVec.Set(0f, pos, 0f);
			break;
		case ExtensionUtils.Axis.AxisZ:
			TransformExtensionMethods._vVec.Set(0f, 0f, pos);
			break;
		case ExtensionUtils.Axis.AxisAll:
			TransformExtensionMethods._vVec.Set(pos, pos, pos);
			break;
		default:
			TransformExtensionMethods._vVec.Set(0f, 0f, 0f);
			break;
		}
		return transform.AddLocalPosition(TransformExtensionMethods._vVec);
	}

	public static Vector3 AddLocalPositionX(this Transform transform, float x = 0f)
	{
		return transform.AddLocalPosition(x, 0f, 0f);
	}

	public static Vector3 AddLocalPositionY(this Transform transform, float y = 0f)
	{
		return transform.AddLocalPosition(0f, y, 0f);
	}

	public static Vector3 AddLocalPositionZ(this Transform transform, float z = 0f)
	{
		return transform.AddLocalPosition(0f, 0f, z);
	}

	public static Vector3 localScale(this Transform transform, float x, float y, float z)
	{
		TransformExtensionMethods._vVec.Set(x, y, z);
		Vector3 vVec = TransformExtensionMethods._vVec;
		transform.set_localScale(vVec);
		return vVec;
	}

	public static Vector3 localScale(this Transform transform, Vector3 scale)
	{
		return transform.localScale(scale.x, scale.y, scale.z);
	}

	public static Vector3 localScale(this Transform transform, ExtensionUtils.Axis iaxis, float scale)
	{
		switch (iaxis)
		{
		case ExtensionUtils.Axis.AxisX:
			TransformExtensionMethods._vVec.Set(scale, transform.get_localScale().y, transform.get_localScale().z);
			break;
		case ExtensionUtils.Axis.AxisY:
			TransformExtensionMethods._vVec.Set(transform.get_localScale().x, scale, transform.get_localScale().z);
			break;
		case ExtensionUtils.Axis.AxisZ:
			TransformExtensionMethods._vVec.Set(transform.get_localScale().x, transform.get_localScale().y, scale);
			break;
		case ExtensionUtils.Axis.AxisAll:
			TransformExtensionMethods._vVec.Set(scale, scale, scale);
			break;
		default:
			TransformExtensionMethods._vVec.Set(transform.get_localScale().x, transform.get_localScale().y, transform.get_localScale().z);
			break;
		}
		return transform.localScale(TransformExtensionMethods._vVec);
	}

	public static Vector3 localScaleX(this Transform transform, float x)
	{
		return transform.localScale(x, transform.get_localScale().y, transform.get_localScale().z);
	}

	public static Vector3 localScaleY(this Transform transform, float y)
	{
		return transform.localScale(transform.get_localScale().x, y, transform.get_localScale().z);
	}

	public static Vector3 localScaleZ(this Transform transform, float z)
	{
		return transform.localScale(transform.get_localScale().x, transform.get_localScale().y, z);
	}

	public static void localScaleZero(this Transform transform)
	{
		transform.set_localScale(Vector3.get_zero());
	}

	public static void localScaleOne(this Transform transform)
	{
		transform.set_localScale(Vector3.get_one());
	}

	public static Vector3 AddLocalScale(this Transform transform, float x, float y, float z)
	{
		TransformExtensionMethods._vVec.Set(transform.get_localScale().x + x, transform.get_localScale().y + y, transform.get_localScale().z + z);
		Vector3 vVec = TransformExtensionMethods._vVec;
		transform.set_localScale(vVec);
		return vVec;
	}

	public static Vector3 AddLocalScale(this Transform transform, Vector3 vscale)
	{
		return transform.AddLocalScale(vscale.x, vscale.y, vscale.z);
	}

	public static Vector3 AddLocalScale(this Transform transform, ExtensionUtils.Axis iaxis, float fscale)
	{
		switch (iaxis)
		{
		case ExtensionUtils.Axis.AxisX:
			TransformExtensionMethods._vVec.Set(fscale, 0f, 0f);
			break;
		case ExtensionUtils.Axis.AxisY:
			TransformExtensionMethods._vVec.Set(0f, fscale, 0f);
			break;
		case ExtensionUtils.Axis.AxisZ:
			TransformExtensionMethods._vVec.Set(0f, 0f, fscale);
			break;
		case ExtensionUtils.Axis.AxisAll:
			TransformExtensionMethods._vVec.Set(fscale, fscale, fscale);
			break;
		default:
			TransformExtensionMethods._vVec = transform.get_localScale();
			break;
		}
		return transform.AddLocalScale(TransformExtensionMethods._vVec);
	}

	public static Vector3 AddLocalScaleX(this Transform transform, float fx)
	{
		return transform.AddLocalScale(fx, 0f, 0f);
	}

	public static Vector3 AddLocalScaleY(this Transform transform, float fy)
	{
		return transform.AddLocalScale(0f, fy, 0f);
	}

	public static Vector3 AddLocalScaleZ(this Transform transform, float fz)
	{
		return transform.AddLocalScale(0f, 0f, fz);
	}

	public static Vector3 eulerAngles(this Transform transform, float x, float y, float z)
	{
		TransformExtensionMethods._vVec.Set(x, y, z);
		Vector3 vVec = TransformExtensionMethods._vVec;
		transform.set_eulerAngles(vVec);
		return vVec;
	}

	public static Vector3 eulerAngles(this Transform transform, Vector3 vrot)
	{
		return transform.eulerAngles(vrot.x, vrot.y, vrot.z);
	}

	public static Vector3 eulerAngles(this Transform transform, ExtensionUtils.Axis iaxis, float frot)
	{
		switch (iaxis)
		{
		case ExtensionUtils.Axis.AxisX:
			TransformExtensionMethods._vVec.Set(frot, transform.get_localEulerAngles().y, transform.get_localEulerAngles().z);
			break;
		case ExtensionUtils.Axis.AxisY:
			TransformExtensionMethods._vVec.Set(transform.get_localEulerAngles().x, frot, transform.get_localEulerAngles().z);
			break;
		case ExtensionUtils.Axis.AxisZ:
			TransformExtensionMethods._vVec.Set(transform.get_localEulerAngles().x, transform.get_localEulerAngles().y, frot);
			break;
		case ExtensionUtils.Axis.AxisAll:
			TransformExtensionMethods._vVec.Set(frot, frot, frot);
			break;
		default:
			TransformExtensionMethods._vVec.Set(transform.get_localEulerAngles().x, transform.get_localEulerAngles().y, transform.get_localEulerAngles().z);
			break;
		}
		return transform.eulerAngles(TransformExtensionMethods._vVec);
	}

	public static Vector3 eulerAnglesX(this Transform transform, float fx)
	{
		return transform.eulerAngles(fx, transform.get_localEulerAngles().y, transform.get_localEulerAngles().z);
	}

	public static Vector3 eulerAnglesY(this Transform transform, float fy)
	{
		return transform.eulerAngles(transform.get_localEulerAngles().x, fy, transform.get_localEulerAngles().z);
	}

	public static Vector3 eulerAnglesZ(this Transform transform, float fz)
	{
		return transform.eulerAngles(transform.get_localEulerAngles().x, transform.get_localEulerAngles().y, fz);
	}

	public static Vector3 AddEulerAngles(this Transform transform, float x, float y, float z)
	{
		TransformExtensionMethods._vVec.Set(transform.get_eulerAngles().x + x, transform.get_eulerAngles().y + y, transform.get_eulerAngles().z + z);
		Vector3 vVec = TransformExtensionMethods._vVec;
		transform.set_eulerAngles(vVec);
		return vVec;
	}

	public static Vector3 AddEulerAngles(this Transform transform, Vector3 vrot)
	{
		return transform.AddEulerAngles(vrot.x, vrot.y, vrot.z);
	}

	public static Vector3 AddEulerAngles(this Transform transform, ExtensionUtils.Axis iaxis, float frot)
	{
		switch (iaxis)
		{
		case ExtensionUtils.Axis.AxisX:
			TransformExtensionMethods._vVec.Set(frot, 0f, 0f);
			break;
		case ExtensionUtils.Axis.AxisY:
			TransformExtensionMethods._vVec.Set(0f, frot, 0f);
			break;
		case ExtensionUtils.Axis.AxisZ:
			TransformExtensionMethods._vVec.Set(0f, 0f, frot);
			break;
		case ExtensionUtils.Axis.AxisAll:
			TransformExtensionMethods._vVec.Set(frot, frot, frot);
			break;
		default:
			TransformExtensionMethods._vVec.Set(0f, 0f, 0f);
			break;
		}
		return transform.AddEulerAngles(TransformExtensionMethods._vVec);
	}

	public static Vector3 AddEulerAnglesX(this Transform transform, float fx)
	{
		return transform.AddEulerAngles(fx, 0f, 0f);
	}

	public static Vector3 AddEulerAnglesY(this Transform transform, float fy)
	{
		return transform.AddEulerAngles(0f, fy, 0f);
	}

	public static Vector3 AddEulerAnglesZ(this Transform transform, float fz)
	{
		return transform.AddEulerAngles(0f, 0f, fz);
	}

	public static Vector3 localEulerAngles(this Transform transform, float x, float y, float z)
	{
		TransformExtensionMethods._vVec.Set(x, y, z);
		Vector3 vVec = TransformExtensionMethods._vVec;
		transform.set_localEulerAngles(vVec);
		return vVec;
	}

	public static Vector3 localEulerAngles(this Transform transform, Vector3 vrot)
	{
		return transform.localEulerAngles(vrot.x, vrot.y, vrot.z);
	}

	public static Vector3 localEulerAngles(this Transform transform, ExtensionUtils.Axis iaxis, float frot)
	{
		switch (iaxis)
		{
		case ExtensionUtils.Axis.AxisX:
			TransformExtensionMethods._vVec.Set(frot, transform.get_localEulerAngles().y, transform.get_localEulerAngles().z);
			break;
		case ExtensionUtils.Axis.AxisY:
			TransformExtensionMethods._vVec.Set(transform.get_localEulerAngles().x, frot, transform.get_localEulerAngles().z);
			break;
		case ExtensionUtils.Axis.AxisZ:
			TransformExtensionMethods._vVec.Set(transform.get_localEulerAngles().x, transform.get_localEulerAngles().y, frot);
			break;
		case ExtensionUtils.Axis.AxisAll:
			TransformExtensionMethods._vVec.Set(frot, frot, frot);
			break;
		default:
			TransformExtensionMethods._vVec.Set(transform.get_localEulerAngles().x, transform.get_localEulerAngles().y, transform.get_localEulerAngles().z);
			break;
		}
		return transform.localEulerAngles(TransformExtensionMethods._vVec);
	}

	public static Vector3 localEulerAnglesX(this Transform transform, float fx)
	{
		return transform.localEulerAngles(fx, transform.get_localEulerAngles().y, transform.get_localEulerAngles().z);
	}

	public static Vector3 localEulerAnglesY(this Transform transform, float fy)
	{
		return transform.localEulerAngles(transform.get_localEulerAngles().x, fy, transform.get_localEulerAngles().z);
	}

	public static Vector3 localEulerAnglesZ(this Transform transform, float fz)
	{
		return transform.localEulerAngles(transform.get_localEulerAngles().x, transform.get_localEulerAngles().y, fz);
	}

	public static Vector3 AddLocalEulerAngles(this Transform transform, float x, float y, float z)
	{
		TransformExtensionMethods._vVec.Set(transform.get_localEulerAngles().x + x, transform.get_localEulerAngles().y + y, transform.get_localEulerAngles().z + z);
		Vector3 vVec = TransformExtensionMethods._vVec;
		transform.set_localEulerAngles(vVec);
		return vVec;
	}

	public static Vector3 AddLocalEulerAngles(this Transform transform, Vector3 vrot)
	{
		return transform.AddLocalEulerAngles(transform.get_localEulerAngles().x + vrot.x, transform.get_localEulerAngles().y + vrot.y, transform.get_localEulerAngles().z + vrot.z);
	}

	public static Vector3 AddLocalEulerAngles(this Transform transform, ExtensionUtils.Axis iaxis, float frot)
	{
		switch (iaxis)
		{
		case ExtensionUtils.Axis.AxisX:
			TransformExtensionMethods._vVec.Set(frot, 0f, 0f);
			break;
		case ExtensionUtils.Axis.AxisY:
			TransformExtensionMethods._vVec.Set(0f, frot, 0f);
			break;
		case ExtensionUtils.Axis.AxisZ:
			TransformExtensionMethods._vVec.Set(0f, 0f, frot);
			break;
		case ExtensionUtils.Axis.AxisAll:
			TransformExtensionMethods._vVec.Set(frot, frot, frot);
			break;
		default:
			TransformExtensionMethods._vVec.Set(0f, 0f, 0f);
			break;
		}
		return transform.AddLocalEulerAngles(TransformExtensionMethods._vVec);
	}

	public static Vector3 AddLocalEulerAnglesX(this Transform transform, float frot)
	{
		return transform.AddLocalEulerAngles(frot, 0f, 0f);
	}

	public static Vector3 AddLocalEulerAnglesY(this Transform transform, float frot)
	{
		return transform.AddLocalEulerAngles(frot, 0f, 0f);
	}

	public static Vector3 AddLocalEulerAnglesZ(this Transform transform, float frot)
	{
		return transform.AddLocalEulerAngles(frot, 0f, 0f);
	}

	public static void LookFrom(this Transform transform, Hashtable hash)
	{
		iTween.LookFrom(transform.get_gameObject(), hash);
	}

	public static void LookFrom(this Transform transform, Vector3 target, float time)
	{
		iTween.LookFrom(transform.get_gameObject(), target, time);
	}

	public static void LookTo(this Transform transform, Hashtable hash)
	{
		if (hash.ContainsKey("oncomplete") && hash.get_Item("oncomplete") == null)
		{
			hash.Remove("oncomplete");
		}
		iTween.LookTo(transform.get_gameObject(), hash);
	}

	public static void LookTo(this Transform transform, Vector3 lookTarget, float time)
	{
		iTween.LookTo(transform.get_gameObject(), lookTarget, time);
	}

	public static void LookTo(this Transform transform, Vector3 lookTarget, float time, Action onComplate)
	{
		transform.LookTo(lookTarget, time, iTween.EaseType.easeOutExpo, onComplate);
	}

	public static void LookTo(this Transform transform, Vector3 lookTarget, float time, iTween.EaseType easeType, Action onComplate)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("looktarget", lookTarget);
		hashtable.Add("time", time);
		hashtable.Add("easetype", easeType);
		hashtable.Add("oncomplete", onComplate);
		transform.LookTo(hashtable);
	}

	public static void LookUpdate(this Transform transform, Hashtable hash)
	{
		iTween.LookUpdate(transform.get_gameObject(), hash);
	}

	public static void LookUpdate(this Transform transform, Vector3 target, float time)
	{
		iTween.LookUpdate(transform.get_gameObject(), target, time);
	}

	public static void MoveAdd(this Transform transform, Hashtable hash)
	{
		if (hash.ContainsKey("oncomplete") && hash.get_Item("oncomplete") == null)
		{
			hash.Remove("oncomplete");
		}
		iTween.MoveAdd(transform.get_gameObject(), hash);
	}

	public static void MoveAdd(this Transform transform, Vector3 amount, float time)
	{
		iTween.MoveAdd(transform.get_gameObject(), amount, time);
	}

	public static void MoveAdd(this Transform transform, Vector3 amount, float time, iTween.EaseType easeType, Action onComplate)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("amount", amount);
		hashtable.Add("time", time);
		hashtable.Add("easetype", easeType);
		hashtable.Add("oncomplete", onComplate);
		transform.MoveAdd(hashtable);
	}

	public static void MoveBy(this Transform transform, Hashtable hash)
	{
		iTween.MoveBy(transform.get_gameObject(), hash);
	}

	public static void MoveBy(this Transform transform, Vector3 amount, float time)
	{
		iTween.MoveBy(transform.get_gameObject(), amount, time);
	}

	public static void MoveFrom(this Transform transform, Hashtable hash)
	{
		iTween.MoveFrom(transform.get_gameObject(), hash);
	}

	public static void MoveFrom(this Transform transform, Vector3 pos, float time)
	{
		iTween.MoveFrom(transform.get_gameObject(), pos, time);
	}

	public static void MoveTo(this Transform transform, Hashtable hash)
	{
		if (hash.ContainsKey("oncomplete") && hash.get_Item("oncomplete") == null)
		{
			hash.Remove("oncomplete");
		}
		iTween.MoveTo(transform.get_gameObject(), hash);
	}

	public static void MoveTo(this Transform transform, Vector3 target, float time)
	{
		iTween.MoveTo(transform.get_gameObject(), target, time);
	}

	public static void MoveTo(this Transform transform, Vector3 target, float time, Action onComplate)
	{
		transform.MoveTo(target, time, iTween.EaseType.easeOutExpo, onComplate);
	}

	public static void MoveTo(this Transform transform, Vector3 target, float time, iTween.EaseType easeType, Action onComplate)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("position", target);
		hashtable.Add("time", time);
		hashtable.Add("easetype", easeType);
		hashtable.Add("oncomplete", onComplate);
		transform.MoveTo(hashtable);
	}

	public static void LocalMoveTo(this Transform transform, Hashtable hash)
	{
		if (hash.ContainsKey("oncomplete") && hash.get_Item("oncomplete") == null)
		{
			hash.Remove("oncomplete");
		}
		hash.Add("islocal", true);
		transform.MoveTo(hash);
	}

	public static void LocalMoveTo(this Transform transform, Vector3 target, float time)
	{
		transform.LocalMoveTo(target, time, null);
	}

	public static void LocalMoveTo(this Transform transform, Vector3 target, float time, Action onComplate)
	{
		transform.LocalMoveTo(target, time, iTween.EaseType.easeOutExpo, onComplate);
	}

	public static void LocalMoveTo(this Transform transform, Vector3 target, float time, iTween.EaseType easeType, Action onComplate)
	{
		transform.LocalMoveTo(target, time, 0f, easeType, onComplate);
	}

	public static void LocalMoveTo(this Transform transform, Vector3 target, float time, float delay, iTween.EaseType easeType, Action onComplate)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("position", target);
		hashtable.Add("time", time);
		hashtable.Add("delay", delay);
		hashtable.Add("easetype", easeType);
		hashtable.Add("oncomplete", onComplate);
		transform.LocalMoveTo(hashtable);
	}

	public static void MoveUpdate(this Transform transform, Hashtable hash)
	{
		iTween.MoveUpdate(transform.get_gameObject(), hash);
	}

	public static void MoveUpdate(this Transform transform, Vector3 pos, float time)
	{
		iTween.MoveUpdate(transform.get_gameObject(), pos, time);
	}

	public static void RotateAdd(this Transform transform, Hashtable hash)
	{
		if (hash.ContainsKey("oncomplete") && hash.get_Item("oncomplete") == null)
		{
			hash.Remove("oncomplete");
		}
		iTween.RotateAdd(transform.get_gameObject(), hash);
	}

	public static void RotateAdd(this Transform transform, Vector3 amount, float time)
	{
		iTween.RotateAdd(transform.get_gameObject(), amount, time);
	}

	public static void RotateAdd(this Transform transform, Vector3 amount, float time, iTween.EaseType easeType, Action onComplate)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("amount", amount);
		hashtable.Add("time", time);
		hashtable.Add("easetype", easeType);
		hashtable.Add("oncomplete", onComplate);
		transform.RotateAdd(hashtable);
	}

	public static void RotateBy(this Transform transform, Hashtable hash)
	{
		iTween.RotateBy(transform.get_gameObject(), hash);
	}

	public static void RotateBy(this Transform transform, Vector3 amount, float time)
	{
		iTween.RotateBy(transform.get_gameObject(), amount, time);
	}

	public static void RotateFrom(this Transform transform, Hashtable hash)
	{
		iTween.RotateFrom(transform.get_gameObject(), hash);
	}

	public static void RotateFrom(this Transform transform, Vector3 rot, float time)
	{
		iTween.RotateFrom(transform.get_gameObject(), rot, time);
	}

	public static void RotateTo(this Transform transform, Hashtable hash)
	{
		if (hash.ContainsKey("oncomplete") && hash.get_Item("oncomplete") == null)
		{
			hash.Remove("oncomplete");
		}
		iTween.RotateTo(transform.get_gameObject(), hash);
	}

	public static void RotateTo(this Transform transform, Vector3 rot, float time)
	{
		iTween.RotateTo(transform.get_gameObject(), rot, time);
	}

	public static void RotateTo(this Transform transform, Vector3 rot, float time, Action onComplate)
	{
		transform.RotateTo(rot, time, iTween.EaseType.easeOutExpo, onComplate);
	}

	public static void RotateTo(this Transform transform, Vector3 rot, float time, iTween.EaseType easeType, Action onComplate)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("rotation", rot);
		hashtable.Add("time", time);
		hashtable.Add("easetype", easeType);
		hashtable.Add("oncomplete", onComplate);
		transform.RotateTo(hashtable);
	}

	public static void RotateUpdate(this Transform transform, Hashtable hash)
	{
		iTween.RotateUpdate(transform.get_gameObject(), hash);
	}

	public static void RotateUpadte(this Transform transform, Vector3 rot, float time)
	{
		iTween.RotateUpdate(transform.get_gameObject(), rot, time);
	}

	public static void ScaleAdd(this Transform transform, Hashtable hash)
	{
		iTween.ScaleAdd(transform.get_gameObject(), hash);
	}

	public static void ScaleAdd(this Transform transform, Vector3 amount, float time)
	{
		iTween.ScaleAdd(transform.get_gameObject(), amount, time);
	}

	public static void ScaleBy(this Transform transform, Hashtable hash)
	{
		iTween.ScaleBy(transform.get_gameObject(), hash);
	}

	public static void ScaleBy(this Transform transform, Vector3 amount, float time)
	{
		iTween.ScaleBy(transform.get_gameObject(), amount, time);
	}

	public static void ScaleFrom(this Transform transform, Hashtable hash)
	{
		iTween.ScaleFrom(transform.get_gameObject(), hash);
	}

	public static void ScaleFrom(this Transform transform, Vector3 scale, float time)
	{
		iTween.ScaleFrom(transform.get_gameObject(), scale, time);
	}

	public static void ScaleTo(this Transform transform, Hashtable hash)
	{
		if (hash.ContainsKey("oncomplete") && hash.get_Item("oncomplete") == null)
		{
			hash.Remove("oncomplete");
		}
		iTween.ScaleTo(transform.get_gameObject(), hash);
	}

	public static void ScaleTo(this Transform transform, Vector3 scale, float time)
	{
		iTween.ScaleTo(transform.get_gameObject(), scale, time);
	}

	public static void ScaleTo(this Transform transform, Vector3 scale, float time, Action onComplate)
	{
		transform.ScaleTo(scale, time, iTween.EaseType.easeOutExpo, onComplate);
	}

	public static void ScaleTo(this Transform transform, Vector3 scale, float time, iTween.EaseType easeType, Action onComplate)
	{
		transform.ScaleTo(scale, time, 0f, easeType, onComplate);
	}

	public static void ScaleTo(this Transform transform, Vector3 target, float time, float delay, iTween.EaseType easeType, Action onComplate)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("scale", target);
		hashtable.Add("time", time);
		hashtable.Add("delay", delay);
		hashtable.Add("easetype", easeType);
		hashtable.Add("oncomplete", onComplate);
		transform.ScaleTo(hashtable);
	}

	public static void ScaleUpdate(this Transform transform, Hashtable hash)
	{
		iTween.ScaleUpdate(transform.get_gameObject(), hash);
	}

	public static void ScaleUpdate(this Transform transform, Vector3 scale, float time)
	{
		iTween.ScaleUpdate(transform.get_gameObject(), scale, time);
	}

	public static void ColorTo(this Transform transform, Hashtable hash)
	{
		if (hash.ContainsKey("oncomplete") && hash.get_Item("oncomplete") == null)
		{
			hash.Remove("oncomplete");
		}
		iTween.ColorTo(transform.get_gameObject(), hash);
	}

	public static void ColorTo(this Transform transform, Color color, float time)
	{
		iTween.ColorTo(transform.get_gameObject(), color, time);
	}

	public static void ColorTo(this Transform transform, Color color, float time, float delay, iTween.EaseType easeType, Action onComplate)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("color", color);
		hashtable.Add("time", time);
		hashtable.Add("delay", delay);
		hashtable.Add("easetype", easeType);
		hashtable.Add("oncomplete", onComplate);
		transform.ColorTo(hashtable);
	}

	public static void ShakePosition(this Transform transform, Hashtable hash)
	{
		iTween.ShakePosition(transform.get_gameObject(), hash);
	}

	public static void ShakePosition(this Transform transform, Vector3 amount, float time)
	{
		iTween.ShakePosition(transform.get_gameObject(), amount, time);
	}

	public static void ShakeRotation(this Transform transform, Hashtable hash)
	{
		iTween.ShakeRotation(transform.get_gameObject(), hash);
	}

	public static void ShakeRotation(this Transform transform, Vector3 amount, float time)
	{
		iTween.ShakeRotation(transform.get_gameObject(), amount, time);
	}

	public static void ShakeScale(this Transform transform, Hashtable hash)
	{
		iTween.ShakeScale(transform.get_gameObject(), hash);
	}

	public static void ShakeScale(this Transform transform, Vector3 amount, float time)
	{
		iTween.ShakeScale(transform.get_gameObject(), amount, time);
	}

	public static void ValueTo(this Transform transform, Hashtable hash)
	{
		if (hash.ContainsKey("oncomplete") && hash.get_Item("oncomplete") == null)
		{
			hash.Remove("oncomplete");
		}
		iTween.ValueTo(transform.get_gameObject(), hash);
	}

	public static void ValueTo(this Transform transform, Hashtable hash, Action onComplate)
	{
		hash.Add("oncomplate", onComplate);
		transform.ValueTo(hash);
	}

	public static void ValueTo(this Transform transform, object from, object to, float time, Action<object> onUpdate, Action onComplate)
	{
		transform.ValueTo(from, to, time, iTween.EaseType.linear, onUpdate, onComplate);
	}

	public static void ValueTo(this Transform transform, object from, object to, float time, iTween.EaseType easeType, Action<object> onUpdate, Action onComplate)
	{
		transform.ValueTo(from, to, time, 0f, easeType, onUpdate, onComplate);
	}

	public static void ValueTo(this Transform transform, object from, object to, float time, float delay, iTween.EaseType easeType, Action<object> onUpdate, Action onComplate)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("from", from);
		hashtable.Add("to", to);
		hashtable.Add("time", time);
		hashtable.Add("delay", delay);
		hashtable.Add("easetype", easeType);
		hashtable.Add("onupdate", onUpdate);
		hashtable.Add("oncomplete", onComplate);
		transform.ValueTo(hashtable);
	}

	public static void iTweenPause(this Transform transform)
	{
		iTween.Pause(transform.get_gameObject());
	}

	public static void iTweenPause(this Transform transform, bool includechildren)
	{
		iTween.Pause(transform.get_gameObject(), includechildren);
	}

	public static void iTweenStop(this Transform transform)
	{
		iTween.Stop(transform.get_gameObject());
	}

	public static void iTweenStop(this Transform transform, bool includechildren)
	{
		iTween.Stop(transform.get_gameObject(), includechildren);
	}

	public static void iTweenResume(this Transform transform)
	{
		iTween.Resume(transform.get_gameObject());
	}

	public static void iTweenResume(this Transform transform, bool includechildren)
	{
		iTween.Resume(transform.get_gameObject(), includechildren);
	}
}
