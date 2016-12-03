using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Toggled Objects")]
public class UIToggledObjects : MonoBehaviour
{
	public List<GameObject> activate;

	public List<GameObject> deactivate;

	[HideInInspector, SerializeField]
	private GameObject target;

	[HideInInspector, SerializeField]
	private bool inverse;

	private void Awake()
	{
		if (this.target != null)
		{
			if (this.activate.get_Count() == 0 && this.deactivate.get_Count() == 0)
			{
				if (this.inverse)
				{
					this.deactivate.Add(this.target);
				}
				else
				{
					this.activate.Add(this.target);
				}
			}
			else
			{
				this.target = null;
			}
		}
		UIToggle component = base.GetComponent<UIToggle>();
		EventDelegate.Add(component.onChange, new EventDelegate.Callback(this.Toggle));
	}

	public void Toggle()
	{
		bool value = UIToggle.current.value;
		if (base.get_enabled())
		{
			for (int i = 0; i < this.activate.get_Count(); i++)
			{
				this.Set(this.activate.get_Item(i), value);
			}
			for (int j = 0; j < this.deactivate.get_Count(); j++)
			{
				this.Set(this.deactivate.get_Item(j), !value);
			}
		}
	}

	private void Set(GameObject go, bool state)
	{
		if (go != null)
		{
			NGUITools.SetActive(go, state);
		}
	}
}
