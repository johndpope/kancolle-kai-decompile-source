using System;
using UnityEngine;

[RequireComponent(typeof(UIInput))]
public class UIInputOnGUI : MonoBehaviour
{
	[NonSerialized]
	private UIInput mInput;

	private void Awake()
	{
		this.mInput = base.GetComponent<UIInput>();
	}

	private void OnGUI()
	{
		if (Event.get_current().get_rawType() == 4)
		{
			this.mInput.ProcessEvent(Event.get_current());
		}
	}
}
