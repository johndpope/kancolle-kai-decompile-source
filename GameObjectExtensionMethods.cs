using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public static class GameObjectExtensionMethods
{
	private static Hashtable _hash = new Hashtable();

	public static void MoveTo(this GameObject obj, Hashtable hash)
	{
		GameObjectExtensionMethods._hash = hash;
		iTween.MoveTo(obj, hash);
	}

	public static void MoveTo(this GameObject obj, Vector3 v0, float ftime)
	{
		iTween.MoveTo(obj, v0, ftime);
	}

	public static void MoveTo(this GameObject obj, Vector3 v0, float ftime, bool local)
	{
		GameObjectExtensionMethods._hash.Clear();
		GameObjectExtensionMethods._hash.Add("position", v0);
		GameObjectExtensionMethods._hash.Add("time", ftime);
		GameObjectExtensionMethods._hash.Add("isLocal", local);
		iTween.MoveTo(obj, GameObjectExtensionMethods._hash);
	}

	public static void MoveTo(this GameObject obj, Vector3 from, Vector3 to, float ftime, bool local)
	{
		obj.get_transform().position(from);
		GameObjectExtensionMethods._hash.Clear();
		GameObjectExtensionMethods._hash.Add("position", to);
		GameObjectExtensionMethods._hash.Add("time", ftime);
		GameObjectExtensionMethods._hash.Add("isLocal", local);
		iTween.MoveTo(obj, GameObjectExtensionMethods._hash);
	}

	public static void MoveTo(this GameObject obj, Vector3 from, Vector3 to, float ftime, float fdelay, bool local)
	{
		obj.get_transform().position(from);
		GameObjectExtensionMethods._hash.Clear();
		GameObjectExtensionMethods._hash.Add("position", to);
		GameObjectExtensionMethods._hash.Add("time", ftime);
		GameObjectExtensionMethods._hash.Add("delay", fdelay);
		GameObjectExtensionMethods._hash.Add("isLocal", local);
		iTween.MoveTo(obj, GameObjectExtensionMethods._hash);
	}

	public static void MoveToBezier(this GameObject obj, float ftime, Vector3 start, Vector3 mid1, Vector3 mid2, Vector3 end)
	{
		Vector3 position = obj.get_transform().get_position();
		Bezier.Interpolate(ref position, start, end, Mathe.MinMax2F01(ftime), mid1, mid2);
		obj.get_transform().position(position);
	}

	public static void ScaleTo(this GameObject obj, Hashtable hash)
	{
		GameObjectExtensionMethods._hash = hash;
		iTween.ScaleTo(obj, hash);
	}

	public static void ScaleTo(this GameObject obj, Vector3 scale, float ftime)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("scale", scale);
		hashtable.Add("time", ftime);
		iTween.ScaleTo(obj, hashtable);
	}

	public static void ScaleTo(this GameObject obj, Vector3 scale, float ftime, float fdelay)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("scale", scale);
		hashtable.Add("time", ftime);
		hashtable.Add("delay", fdelay);
		iTween.ScaleTo(obj, hashtable);
	}

	public static void ValueTo(this GameObject obj, Hashtable hash)
	{
		iTween.ValueTo(obj, hash);
	}

	public static void ValueTo(this GameObject obj, float from = 0f, float to = 0f, float time = 0f, string method = "")
	{
		GameObjectExtensionMethods._hash.Clear();
		GameObjectExtensionMethods._hash.Add("from", from);
		GameObjectExtensionMethods._hash.Add("to", to);
		GameObjectExtensionMethods._hash.Add("time", time);
		GameObjectExtensionMethods._hash.Add("onupdate", method);
		iTween.ValueTo(obj, GameObjectExtensionMethods._hash);
	}

	public static void RotateTo(this GameObject obj, Hashtable hash)
	{
		GameObjectExtensionMethods._hash = hash;
		iTween.RotateTo(obj, hash);
	}

	public static void RotateTo(this GameObject obj, Vector3 rot, float time)
	{
		iTween.RotateTo(obj, rot, time);
	}

	public static void RotatoTo(this GameObject obj, Vector3 rot, float time, Action callback)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("rotation", rot);
		if (callback != null)
		{
			hashtable.Add("oncomplate", callback);
		}
		hashtable.Add("time", time);
		iTween.RotateTo(obj, hashtable);
	}

	public static void ResourcesLoad(this GameObject obj, string path)
	{
		obj = (Resources.Load(path) as GameObject);
	}

	public static void FindParentToChild(this GameObject obj, Transform parent, string objName)
	{
		obj = parent.FindChild(objName).get_gameObject();
		if (obj == null)
		{
			DebugUtils.NullReferenceException(objName + " not found. parent is " + parent.get_name());
		}
	}

	public static GameObject[] GetChildren(this GameObject self, bool includeInactive = false)
	{
		return Enumerable.ToArray<GameObject>(Enumerable.Select<Transform, GameObject>(Enumerable.Where<Transform>(self.GetComponentsInChildren<Transform>(includeInactive), (Transform c) => c != self.get_transform()), (Transform c) => c.get_gameObject()));
	}

	public static T GetComponent<T>(this GameObject obj) where T : Component
	{
		return obj.GetComponent<T>() ?? obj.AddComponent<T>();
	}

	public static Component GetComponent(this GameObject gameObject, Type componentType)
	{
		return gameObject.GetComponent(componentType) ?? gameObject.AddComponent(componentType);
	}

	public static T SafeGetComponent<T>(this GameObject gameObject) where T : Component
	{
		if (gameObject.GetComponent<T>() != null)
		{
			return gameObject.GetComponent<T>();
		}
		return gameObject.AddComponent<T>();
	}

	public static void Discard(this GameObject gameObject)
	{
		using (IEnumerator enumerator = gameObject.get_transform().GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.get_Current();
				if (transform.get_gameObject().GetComponent<Renderer>() != null)
				{
					Object.Destroy(transform.get_gameObject().GetComponent<Renderer>().get_material());
				}
				Object.Destroy(transform.get_gameObject());
			}
		}
		if (gameObject.GetComponent<Renderer>() != null)
		{
			Object.Destroy(gameObject.GetComponent<Renderer>().get_material());
		}
		Object.Destroy(gameObject);
	}

	public static void SetActiveChildren(this GameObject gameObject, bool isActive)
	{
		GameObject[] children = gameObject.GetChildren(true);
		GameObject[] array = children;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject2 = array[i];
			gameObject2.SetActive(isActive);
		}
	}

	public static void SafeGetTweenAlpha(this GameObject obj, float from, float to, float duration, float delay, UITweener.Method method = UITweener.Method.Linear, UITweener.Style style = UITweener.Style.Once, GameObject eventReceiver = null, string callWhenFinished = "")
	{
		if (obj.GetComponent<TweenAlpha>())
		{
			TweenAlpha tweenAlpha = obj.GetComponent<TweenAlpha>();
			tweenAlpha.ResetToBeginning();
			tweenAlpha.from = from;
			tweenAlpha.to = to;
			tweenAlpha.duration = duration;
			tweenAlpha.delay = delay;
			tweenAlpha.method = method;
			tweenAlpha.style = style;
			tweenAlpha.eventReceiver = eventReceiver;
			if (tweenAlpha.eventReceiver != null)
			{
				tweenAlpha.callWhenFinished = callWhenFinished;
			}
			tweenAlpha.PlayForward();
		}
		else
		{
			TweenAlpha tweenAlpha = obj.AddComponent<TweenAlpha>();
			tweenAlpha.from = from;
			tweenAlpha.to = to;
			tweenAlpha.duration = duration;
			tweenAlpha.delay = delay;
			tweenAlpha.method = method;
			tweenAlpha.style = style;
			tweenAlpha.eventReceiver = eventReceiver;
			if (tweenAlpha.eventReceiver != null)
			{
				tweenAlpha.callWhenFinished = callWhenFinished;
			}
			tweenAlpha.PlayForward();
		}
	}

	public static void SafeGetTweenScale(this GameObject obj, Vector3 from, Vector3 to, float duration, float delay, UITweener.Method method = UITweener.Method.Linear, UITweener.Style style = UITweener.Style.Once, GameObject eventReceiver = null, string callWhenFinished = "")
	{
		if (obj.GetComponent<TweenScale>())
		{
			TweenScale tweenScale = obj.GetComponent<TweenScale>();
			tweenScale.ResetToBeginning();
			tweenScale.from = from;
			tweenScale.to = to;
			tweenScale.duration = duration;
			tweenScale.delay = delay;
			tweenScale.method = method;
			tweenScale.style = style;
			tweenScale.eventReceiver = eventReceiver;
			if (tweenScale.eventReceiver != null)
			{
				tweenScale.callWhenFinished = callWhenFinished;
			}
			tweenScale.PlayForward();
		}
		else
		{
			TweenScale tweenScale = obj.AddComponent<TweenScale>();
			tweenScale.from = from;
			tweenScale.to = to;
			tweenScale.duration = duration;
			tweenScale.method = method;
			tweenScale.delay = delay;
			tweenScale.style = style;
			tweenScale.eventReceiver = eventReceiver;
			if (tweenScale.eventReceiver != null)
			{
				tweenScale.callWhenFinished = callWhenFinished;
			}
			tweenScale.PlayForward();
		}
	}

	public static void SafeGetTweenPosition(this GameObject obj, Vector3 from, Vector3 to, float duration, float delay, UITweener.Method method = UITweener.Method.Linear, UITweener.Style style = UITweener.Style.Once, GameObject eventReceiver = null, string callWhenFinished = "")
	{
		if (obj.GetComponent<TweenPosition>())
		{
			TweenPosition tweenPosition = obj.GetComponent<TweenPosition>();
			tweenPosition.ResetToBeginning();
			tweenPosition.from = from;
			tweenPosition.to = to;
			tweenPosition.duration = duration;
			tweenPosition.delay = delay;
			tweenPosition.method = method;
			tweenPosition.style = style;
			tweenPosition.eventReceiver = eventReceiver;
			if (tweenPosition.eventReceiver != null)
			{
				tweenPosition.callWhenFinished = callWhenFinished;
			}
			tweenPosition.PlayForward();
		}
		else
		{
			TweenPosition tweenPosition = obj.SafeGetComponent<TweenPosition>();
			tweenPosition.from = from;
			tweenPosition.to = to;
			tweenPosition.duration = duration;
			tweenPosition.delay = delay;
			tweenPosition.method = method;
			tweenPosition.style = style;
			tweenPosition.eventReceiver = eventReceiver;
			if (tweenPosition.eventReceiver != null)
			{
				tweenPosition.callWhenFinished = callWhenFinished;
			}
			tweenPosition.Play(true);
		}
	}

	public static void SafeGetTweenRotation(this GameObject obj, Vector3 from, Vector3 to, float duration, float delay, UITweener.Method method = UITweener.Method.Linear, UITweener.Style style = UITweener.Style.Once, GameObject eventReceiver = null, string callWhenFinished = "")
	{
		if (obj.GetComponent<TweenRotation>())
		{
			TweenRotation tweenRotation = obj.GetComponent<TweenRotation>();
			tweenRotation.ResetToBeginning();
			tweenRotation.from = from;
			tweenRotation.to = to;
			tweenRotation.duration = duration;
			tweenRotation.delay = delay;
			tweenRotation.method = method;
			tweenRotation.style = style;
			tweenRotation.eventReceiver = eventReceiver;
			if (tweenRotation.eventReceiver != null)
			{
				tweenRotation.callWhenFinished = callWhenFinished;
			}
			tweenRotation.PlayForward();
		}
		else
		{
			TweenRotation tweenRotation = obj.AddComponent<TweenRotation>();
			tweenRotation.from = from;
			tweenRotation.to = to;
			tweenRotation.duration = duration;
			tweenRotation.delay = delay;
			tweenRotation.method = method;
			tweenRotation.style = style;
			tweenRotation.eventReceiver = eventReceiver;
			if (tweenRotation.eventReceiver != null)
			{
				tweenRotation.callWhenFinished = callWhenFinished;
			}
			tweenRotation.Play(true);
		}
	}
}
