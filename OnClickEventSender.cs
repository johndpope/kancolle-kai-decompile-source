using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class OnClickEventSender : MonoBehaviour
{
	[SerializeField]
	private EventDelegate onClick;

	private void OnClick()
	{
		if (this.onClick != null)
		{
			this.onClick.Execute();
		}
	}

	public void SetClickable(bool clickable)
	{
		base.GetComponent<Collider2D>().set_enabled(clickable);
	}

	private void OnDestroy()
	{
		this.onClick = null;
	}
}
