using KCV.Utils;
using System;
using UnityEngine;

namespace KCV.Port.Repair
{
	public class high_repair : MonoBehaviour
	{
		private repair rep;

		private board2 bd2;

		private dialog2 dia2;

		private UISprite ele_s;

		private board3_top_mask bd3m;

		private GameObject cursor;

		private void Start()
		{
			this._init_repair();
		}

		private void OnDestroy()
		{
			Mem.Del<repair>(ref this.rep);
			Mem.Del<board2>(ref this.bd2);
			Mem.Del<dialog2>(ref this.dia2);
			Mem.Del(ref this.ele_s);
			Mem.Del<board3_top_mask>(ref this.bd3m);
			Mem.Del<GameObject>(ref this.cursor);
		}

		public void _init_repair()
		{
			this.rep = GameObject.Find("Repair Root").GetComponent<repair>();
			this.bd2 = this.rep.get_transform().FindChild("board2_top/board2").GetComponent<board2>();
			this.dia2 = GameObject.Find("dialog2").GetComponent<dialog2>();
		}

		public void OnClick()
		{
			if (this.dia2.get_dialog2_anime() || this.rep.now_mode() == -1)
			{
				return;
			}
			this.rep.set_mode(-5);
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter2);
			Debug.Log("high_repair.cs 高速修復が押された!");
			GameObject gameObject = base.get_gameObject().get_transform().get_parent().get_gameObject().get_transform().get_parent().get_gameObject();
			int num;
			int.TryParse(gameObject.get_name(), ref num);
			Debug.Log("押された番号：" + num);
			this.dia2.UpdateInfo(num);
			this.dia2.SetDock(num);
			this.rep.setmask(3, true);
			this.dia2.dialog2_appear(true);
			this.rep.set_mode(5);
		}
	}
}
