using System;
using System.Collections;
using UnityEngine;

public static class ComponentExtensions
{
	public static T AddComponent<T>(this Component component) where T : Component
	{
		return component.get_gameObject().AddComponent<T>();
	}

	public static Component AddComponent(this Component component, Type componentType)
	{
		return component.get_gameObject().AddComponent(componentType);
	}

	public static Component AddChild(this Component component)
	{
		GameObject gameObject = new GameObject();
		gameObject.get_transform().set_parent(component.get_transform());
		return component;
	}

	public static Component AddChild(this Component component, string childName)
	{
		GameObject gameObject = new GameObject();
		gameObject.get_transform().set_parent(component.get_transform());
		gameObject.set_name(childName);
		gameObject.get_transform().set_localScale(Vector3.get_one());
		gameObject.get_transform().set_localPosition(Vector3.get_zero());
		return component;
	}

	public static Component AddChild<T>(this Component component, string childName) where T : Component
	{
		component.AddChild(childName);
		T t = component.get_transform().FindChild(childName).AddComponent<T>();
		return component;
	}

	public static Component AddChild<T>(this Component component, string childName, ref T instance) where T : Component
	{
		component.AddChild(childName);
		instance = component.get_transform().FindChild(childName).AddComponent<T>();
		return component;
	}

	public static Component SetActive(this Component component, bool isActive)
	{
		component.get_gameObject().SetActive(isActive);
		return component;
	}

	public static void SetActiveChildren(this Component component, bool isActive)
	{
		component.get_gameObject().SetActiveChildren(isActive);
	}

	public static Component SetLayer(this Component component, int layer)
	{
		if (component.get_gameObject().get_layer() != layer)
		{
			component.get_gameObject().set_layer(layer);
		}
		return component;
	}

	public static Component SetLayer(this Component component, int layer, bool includeChildren)
	{
		component = component.SetLayer(layer);
		if (includeChildren)
		{
			using (IEnumerator enumerator = component.get_transform().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Transform transform = (Transform)enumerator.get_Current();
					if (transform.get_gameObject().get_layer() != layer)
					{
						transform.SetLayer(layer);
					}
				}
			}
		}
		return component;
	}

	public static Component SetRenderQueue(this Component component, int queue)
	{
		if (component.GetComponent<Renderer>() != null && component.GetComponent<Renderer>().get_material() != null)
		{
			component.GetComponent<Renderer>().get_material().set_renderQueue(queue);
		}
		return component;
	}

	public static Component SetRenderQueue(this Component component, int queue, bool includeChildren)
	{
		component = component.SetRenderQueue(queue);
		if (includeChildren)
		{
			using (IEnumerator enumerator = component.get_transform().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Transform component2 = (Transform)enumerator.get_Current();
					component2.SetRenderQueue(queue);
				}
			}
		}
		return component;
	}

	public static T SafeGetComponent<T>(this Component component) where T : Component
	{
		if (component.GetComponent<T>() != null)
		{
			return component.GetComponent<T>();
		}
		return component.AddComponent<T>();
	}

	public static Component SafeGetComponent(this Component component, Type componentType)
	{
		if (component.GetComponent(componentType) != null)
		{
			return component.GetComponent(componentType);
		}
		return component.AddComponent(componentType);
	}

	public static void SafeGetTweenAlpha(this Component component, float from, float to, float duration, float delay, UITweener.Method method = UITweener.Method.Linear, UITweener.Style style = UITweener.Style.Once, GameObject eventReceiver = null, string callWhenFinished = "")
	{
		component.get_gameObject().SafeGetTweenAlpha(from, to, duration, delay, method, style, eventReceiver, callWhenFinished);
	}

	public static void SafeGetTweenScale(this Component component, Vector3 from, Vector3 to, float duration, float delay, UITweener.Method method = UITweener.Method.Linear, UITweener.Style style = UITweener.Style.Once, GameObject eventReceiver = null, string callWhenFinished = "")
	{
		component.get_gameObject().SafeGetTweenScale(from, to, duration, delay, method, style, eventReceiver, callWhenFinished);
	}

	public static void SafeGetTweenPosition(this Component component, Vector3 from, Vector3 to, float duration, float delay, UITweener.Method method = UITweener.Method.Linear, UITweener.Style style = UITweener.Style.Once, GameObject eventReceiver = null, string callWhenFinished = "")
	{
		component.get_gameObject().SafeGetTweenPosition(from, to, duration, delay, method, style, eventReceiver, callWhenFinished);
	}

	public static void SafeGetTweenRotation(this Component component, Vector3 from, Vector3 to, float duration, UITweener.Method method = UITweener.Method.Linear, UITweener.Style style = UITweener.Style.Once, GameObject eventReceiver = null, string callWhenFinished = "")
	{
		TweenRotation tweenRotation = component.SafeGetComponent<TweenRotation>();
		tweenRotation.from = from;
		tweenRotation.to = to;
		tweenRotation.duration = duration;
		tweenRotation.method = method;
		tweenRotation.style = style;
		tweenRotation.eventReceiver = eventReceiver;
		tweenRotation.ResetToBeginning();
		if (tweenRotation.eventReceiver != null)
		{
			tweenRotation.callWhenFinished = callWhenFinished;
		}
		tweenRotation.Play(true);
	}
}
