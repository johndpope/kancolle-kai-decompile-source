using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class OnDoubleClickEventSender : MonoBehaviour
{
	[SerializeField]
	private EventDelegate mOnDoubleClickEventListener;

	private void OnDoubleClick()
	{
		if (this.mOnDoubleClickEventListener != null)
		{
			this.mOnDoubleClickEventListener.Execute();
		}
	}
}
