using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Toggled Components"), ExecuteInEditMode, RequireComponent(typeof(UIToggle))]
public class UIToggledComponents : MonoBehaviour
{
	public List<MonoBehaviour> activate;

	public List<MonoBehaviour> deactivate;

	[HideInInspector, SerializeField]
	private MonoBehaviour target;

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
		if (base.get_enabled())
		{
			for (int i = 0; i < this.activate.get_Count(); i++)
			{
				MonoBehaviour monoBehaviour = this.activate.get_Item(i);
				monoBehaviour.set_enabled(UIToggle.current.value);
			}
			for (int j = 0; j < this.deactivate.get_Count(); j++)
			{
				MonoBehaviour monoBehaviour2 = this.deactivate.get_Item(j);
				monoBehaviour2.set_enabled(!UIToggle.current.value);
			}
		}
	}
}
