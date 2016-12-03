using System;
using UnityEngine;

namespace KCV.Port.Repair
{
	public class board2_top_mask : MonoBehaviour
	{
		private repair rep;

		private board3 bd3;

		private sw sw;

		private board2_top_mask bd2m;

		private GameObject board_mask2;

		private dialog dia;

		private void Start()
		{
			this._init_repair();
		}

		private void OnDestroy()
		{
			Mem.Del<repair>(ref this.rep);
			Mem.Del<board3>(ref this.bd3);
			Mem.Del<sw>(ref this.sw);
			Mem.Del<board2_top_mask>(ref this.bd2m);
			Mem.Del<GameObject>(ref this.board_mask2);
			Mem.Del<dialog>(ref this.dia);
		}

		public void _init_repair()
		{
			this.rep = GameObject.Find("Repair Root").GetComponent<repair>();
			this.board_mask2 = GameObject.Find("Repair Root/board2_top_mask");
			this.bd3 = GameObject.Find("board3").GetComponent<board3>();
			this.board_mask2.GetComponent<UIPanel>().depth = 132;
			this.dia = GameObject.Find("dialog_top/dialog").GetComponent<dialog>();
		}

		private void OnClick()
		{
			Debug.Log("mask2に触られた。");
			if (this.dia.get_dialog_anime() || this.bd3.get_board3_anime())
			{
				return;
			}
			this.rep.set_mode(-2);
			this.bd3.board3_appear(false, true);
			this.sw = GameObject.Find("board3/sw01").GetComponent<sw>();
			this.sw.setSW(false);
			this.bd3.Cancelled(false);
			this.rep = GameObject.Find("Repair Root").GetComponent<repair>();
			this.rep.setmask(2, false);
			this.rep.set_mode(2);
		}
	}
}
