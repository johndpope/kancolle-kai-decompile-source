using System;
using UnityEngine;

namespace KCV.Port.Repair
{
	public class board1_top_mask : MonoBehaviour
	{
		private repair rep;

		private board2 bd2;

		private board1_top_mask bd1m;

		private GameObject board_mask1;

		private void Start()
		{
			this._init_repair();
		}

		private void OnDestroy()
		{
			Mem.Del<repair>(ref this.rep);
			Mem.Del<board2>(ref this.bd2);
			Mem.Del<board1_top_mask>(ref this.bd1m);
			Mem.Del<GameObject>(ref this.board_mask1);
		}

		public void _init_repair()
		{
			this.rep = GameObject.Find("Repair Root").GetComponent<repair>();
			this.board_mask1 = GameObject.Find("Repair Root/board1_top_mask");
			this.board_mask1.GetComponent<UIPanel>().depth = 50;
			this.bd2 = GameObject.Find("board2").GetComponent<board2>();
		}

		private void OnClick()
		{
			if (this.bd2.get_board2_anime())
			{
				return;
			}
			this.rep = GameObject.Find("Repair Root").GetComponent<repair>();
			this.rep.set_mode(-1);
			this.bd2.board2_appear(false);
			this.rep.setmask(1, false);
			this.rep.set_mode(1);
		}
	}
}
