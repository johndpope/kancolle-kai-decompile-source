using System;
using UnityEngine;

namespace KCV.Port.Repair
{
	public class debug_redraw : MonoBehaviour
	{
		public void OnClick()
		{
			repair component = GameObject.Find("Repair Root").GetComponent<repair>();
			board component2 = GameObject.Find("board1_top/board").GetComponent<board>();
			component2.DockStatus();
		}
	}
}
