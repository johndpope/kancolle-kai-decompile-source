using KCV.Utils;
using System;
using UnityEngine;

namespace KCV.Port.Repair
{
	public class board3_btn : MonoBehaviour
	{
		private repair rep;

		private dialog dia;

		private UISprite ele_s;

		private UIPortFrame UIP;

		private int number;

		private board3_top_mask bd3m;

		private board3 bd3;

		private GameObject cursor;

		private void Start()
		{
			this._init_repair();
		}

		private void OnDestroy()
		{
			Mem.Del<repair>(ref this.rep);
			Mem.Del<board3>(ref this.bd3);
			Mem.Del(ref this.ele_s);
			Mem.Del<UIPortFrame>(ref this.UIP);
			Mem.Del<board3_top_mask>(ref this.bd3m);
			Mem.Del<board3>(ref this.bd3);
			Mem.Del<GameObject>(ref this.cursor);
		}

		public void _init_repair()
		{
			this.rep = GameObject.Find("Repair Root").GetComponent<repair>();
			this.dia = GameObject.Find("dialog").GetComponent<dialog>();
			this.bd3 = GameObject.Find("board3_top/board3").GetComponent<board3>();
		}

		public void UpdateInfo(int no)
		{
		}

		public void OnClick()
		{
			if (this.rep.now_mode() != 3 || this.dia.get_dialog_anime() || this.bd3.get_board3_anime())
			{
				return;
			}
			this.rep.set_mode(-1);
			this.dia.set_dialog_anime(true);
			this.bd3.Set_Button_Sprite(false);
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter2);
			this.dia.dialog_appear(true);
			this.rep.setmask(3, true);
			this.rep.set_mode(4);
		}
	}
}
