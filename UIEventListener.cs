using System;
using UnityEngine;

[AddComponentMenu("NGUI/Internal/Event Listener")]
public class UIEventListener : MonoBehaviour
{
	public delegate void VoidDelegate(GameObject go);

	public delegate void BoolDelegate(GameObject go, bool state);

	public delegate void FloatDelegate(GameObject go, float delta);

	public delegate void VectorDelegate(GameObject go, Vector2 delta);

	public delegate void ObjectDelegate(GameObject go, GameObject obj);

	public delegate void KeyCodeDelegate(GameObject go, KeyCode key);

	public object parameter;

	public UIEventListener.VoidDelegate onSubmit;

	public UIEventListener.VoidDelegate onClick;

	public UIEventListener.VoidDelegate onDoubleClick;

	public UIEventListener.BoolDelegate onHover;

	public UIEventListener.BoolDelegate onPress;

	public UIEventListener.BoolDelegate onSelect;

	public UIEventListener.FloatDelegate onScroll;

	public UIEventListener.VoidDelegate onDragStart;

	public UIEventListener.VectorDelegate onDrag;

	public UIEventListener.VoidDelegate onDragOver;

	public UIEventListener.VoidDelegate onDragOut;

	public UIEventListener.VoidDelegate onDragEnd;

	public UIEventListener.ObjectDelegate onDrop;

	public UIEventListener.KeyCodeDelegate onKey;

	public UIEventListener.BoolDelegate onTooltip;

	private void OnSubmit()
	{
		if (this.onSubmit != null)
		{
			this.onSubmit(base.get_gameObject());
		}
	}

	private void OnClick()
	{
		if (this.onClick != null)
		{
			this.onClick(base.get_gameObject());
		}
	}

	private void OnDoubleClick()
	{
		if (this.onDoubleClick != null)
		{
			this.onDoubleClick(base.get_gameObject());
		}
	}

	private void OnHover(bool isOver)
	{
		if (this.onHover != null)
		{
			this.onHover(base.get_gameObject(), isOver);
		}
	}

	private void OnPress(bool isPressed)
	{
		if (this.onPress != null)
		{
			this.onPress(base.get_gameObject(), isPressed);
		}
	}

	private void OnSelect(bool selected)
	{
		if (this.onSelect != null)
		{
			this.onSelect(base.get_gameObject(), selected);
		}
	}

	private void OnScroll(float delta)
	{
		if (this.onScroll != null)
		{
			this.onScroll(base.get_gameObject(), delta);
		}
	}

	private void OnDragStart()
	{
		if (this.onDragStart != null)
		{
			this.onDragStart(base.get_gameObject());
		}
	}

	private void OnDrag(Vector2 delta)
	{
		if (this.onDrag != null)
		{
			this.onDrag(base.get_gameObject(), delta);
		}
	}

	private void OnDragOver()
	{
		if (this.onDragOver != null)
		{
			this.onDragOver(base.get_gameObject());
		}
	}

	private void OnDragOut()
	{
		if (this.onDragOut != null)
		{
			this.onDragOut(base.get_gameObject());
		}
	}

	private void OnDragEnd()
	{
		if (this.onDragEnd != null)
		{
			this.onDragEnd(base.get_gameObject());
		}
	}

	private void OnDrop(GameObject go)
	{
		if (this.onDrop != null)
		{
			this.onDrop(base.get_gameObject(), go);
		}
	}

	private void OnKey(KeyCode key)
	{
		if (this.onKey != null)
		{
			this.onKey(base.get_gameObject(), key);
		}
	}

	private void OnTooltip(bool show)
	{
		if (this.onTooltip != null)
		{
			this.onTooltip(base.get_gameObject(), show);
		}
	}

	public static UIEventListener Get(GameObject go)
	{
		UIEventListener uIEventListener = go.GetComponent<UIEventListener>();
		if (uIEventListener == null)
		{
			uIEventListener = go.AddComponent<UIEventListener>();
		}
		return uIEventListener;
	}
}
