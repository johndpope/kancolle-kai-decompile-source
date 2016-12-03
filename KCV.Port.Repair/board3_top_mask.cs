using System;
using UnityEngine;

namespace KCV.Port.Repair
{
	public class board3_top_mask : MonoBehaviour
	{
		public GameObject board_mask3;

		private void Start()
		{
			this._init_repair();
		}

		private void OnDestroy()
		{
			Mem.Del<GameObject>(ref this.board_mask3);
		}

		public void _init_repair()
		{
			this.board_mask3 = GameObject.Find("Repair Root/board3_top_mask");
			this.board_mask3.GetComponent<UIPanel>().depth = 139;
		}
	}
}
