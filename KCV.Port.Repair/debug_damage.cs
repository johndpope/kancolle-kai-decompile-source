using KCV.Utils;
using System;
using UnityEngine;

namespace KCV.Port.Repair
{
	public class debug_damage : MonoBehaviour
	{
		public board2 bd2;

		public repair rep;

		private void OnDestroy()
		{
			Mem.Del<board2>(ref this.bd2);
			Mem.Del<repair>(ref this.rep);
		}

		public void OnClick()
		{
			this.rep = GameObject.Find("Repair Root").GetComponent<repair>();
			GameObject target = GameObject.Find("board1_top");
			int mode = this.rep.now_mode();
			SoundUtils.PlaySE(SEFIleInfos.Explode);
			iTween.Stop(target);
			iTween.MoveFrom(target, iTween.Hash(new object[]
			{
				"islocal",
				true,
				"y",
				200f,
				"time",
				0.5f,
				"easetype",
				iTween.EaseType.easeOutElastic
			}));
			iTween.MoveTo(target, iTween.Hash(new object[]
			{
				"islocal",
				true,
				"y",
				0f,
				"delay",
				0.2f
			}));
			this.bd2 = GameObject.Find("board2_top/board2").GetComponent<board2>();
			this.bd2.DBG_damage();
			this.bd2.UpdateList();
			this.rep.set_mode(mode);
		}
	}
}
