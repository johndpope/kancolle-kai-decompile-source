using System;
using UnityEngine;

namespace KCV.Port.Repair
{
	public class sentaku : MonoBehaviour
	{
		private board bd1;

		private board2 bd2;

		private repair rep;

		private board1_top_mask bd1m;

		private GameObject cursor;

		private dialog dia;

		private dialog2 dia2;

		private void Start()
		{
			this._init_repair();
		}

		private void OnDestroy()
		{
			Mem.Del<board>(ref this.bd1);
			Mem.Del<board2>(ref this.bd2);
			Mem.Del<repair>(ref this.rep);
			Mem.Del<board1_top_mask>(ref this.bd1m);
			Mem.Del<GameObject>(ref this.cursor);
			Mem.Del<dialog>(ref this.dia);
			Mem.Del<dialog2>(ref this.dia2);
		}

		public void _init_repair()
		{
			this.rep = GameObject.Find("Repair Root").GetComponent<repair>();
			this.bd1 = GameObject.Find("board").GetComponent<board>();
			this.bd2 = GameObject.Find("board2").GetComponent<board2>();
			this.dia = GameObject.Find("dialog_top/dialog").GetComponent<dialog>();
			this.dia2 = GameObject.Find("dialog2_top/dialog2").GetComponent<dialog2>();
		}

		private void OnClick()
		{
			this._init_repair();
			if (this.dia.get_dialog_anime() || this.bd2.get_board2_anime() || this.dia2.get_dialog2_anime() || this.rep.now_mode() == -1)
			{
				return;
			}
			int num;
			int.TryParse(base.get_transform().get_parent().get_gameObject().get_name(), ref num);
			Debug.Log(string.Concat(new object[]
			{
				"get_dock_touchable_count: ",
				this.bd1.get_dock_touchable_count(),
				" Touched:",
				num
			}));
			if (this.bd1.get_dock_touchable_count() > num)
			{
				this.bd1.dock_selected(num);
			}
		}
	}
}
