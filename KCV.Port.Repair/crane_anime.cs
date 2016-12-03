using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Port.Repair
{
	public class crane_anime : MonoBehaviour
	{
		private Animation _AM;

		private Animation _AMH;

		private Animation _AMG;

		private UISprite _Gauge;

		private GameObject[] CraneNormal = new GameObject[4];

		private GameObject[] Gauge = new GameObject[4];

		private int now_repairs;

		private bool _isHPgrow;

		private int gdock;

		private UISprite _HPG;

		private board bd1;

		private board2 bd2;

		private repair rep;

		private int _REPAIR_ITEMS_ = 99999;

		private void Start()
		{
			this._init_repair();
		}

		private void OnDestroy()
		{
			Mem.Del<Animation>(ref this._AM);
			Mem.Del<Animation>(ref this._AMH);
			Mem.Del<Animation>(ref this._AMG);
			Mem.Del(ref this._Gauge);
			Mem.Del<GameObject[]>(ref this.CraneNormal);
			Mem.Del<GameObject[]>(ref this.Gauge);
			Mem.Del(ref this._HPG);
			Mem.Del<board>(ref this.bd1);
			Mem.Del<board2>(ref this.bd2);
			Mem.Del<repair>(ref this.rep);
		}

		private void HpAnimeEnd()
		{
			this._isHPgrow = false;
		}

		public void _init_repair()
		{
			this.bd1 = GameObject.Find("board1_top/board").GetComponent<board>();
			this.bd2 = GameObject.Find("board2_top/board2").GetComponent<board2>();
			this.rep = GameObject.Find("Repair Root").GetComponent<repair>();
			for (int i = 0; i < 4; i++)
			{
				this.bd1.set_anime(i, false);
				this.bd1.set_HS_anime(i, false);
			}
			this.now_repairs = 0;
			this._isHPgrow = false;
		}

		public void HPgrowStop()
		{
			this._isHPgrow = false;
		}

		public void start_anime(int no)
		{
			string arg_13_0 = "board/Grid/0";
			int num = no;
			string text = arg_13_0 + num.ToString() + "/Anime";
			this._AM = GameObject.Find(text).GetComponent<Animation>();
			this._AM.GetComponent<UIPanel>().set_enabled(true);
			Debug.Log("★ ★ ★ Start dock_anime[no]: " + this.bd1.get_anime(no));
			if (!this.bd1.get_anime(no))
			{
				this._AM.Play();
				this.bd1.set_anime(no, true);
			}
			this.CraneNormal[no] = GameObject.Find(text + "/crane1");
		}

		public void stop_anime(int no)
		{
			Debug.Log("★ ★ ★ Stop dock_anime[no]: " + this.bd1.get_anime(no));
			string arg_34_0 = "board/Grid/0";
			int num = no;
			this._AM = GameObject.Find(arg_34_0 + num.ToString() + "/Anime").GetComponent<Animation>();
			string arg_5B_0 = "board/Grid/0";
			int num2 = no;
			GameObject gameObject = GameObject.Find(arg_5B_0 + num2.ToString() + "/Anime/crane1");
			this._AM.Stop();
			iTween.MoveTo(gameObject, iTween.Hash(new object[]
			{
				"islocal",
				true,
				"x",
				520f,
				"time",
				5f
			}));
			this.CraneNormal[no] = gameObject;
		}

		[DebuggerHidden]
		private IEnumerator change_chara_on_crane_co()
		{
			crane_anime.<change_chara_on_crane_co>c__IteratorBB <change_chara_on_crane_co>c__IteratorBB = new crane_anime.<change_chara_on_crane_co>c__IteratorBB();
			<change_chara_on_crane_co>c__IteratorBB.<>f__this = this;
			return <change_chara_on_crane_co>c__IteratorBB;
		}

		public void change_chara_on_crane()
		{
			base.StartCoroutine(this.change_chara_on_crane_co());
		}

		public void high_repair_anime(int dockno)
		{
			this.high_repair_anime(dockno, true);
		}

		public void high_repair_anime(int dockno, bool _low_anime)
		{
			string arg_13_0 = "board/Grid/0";
			int num = dockno;
			UIButton component = GameObject.Find(arg_13_0 + num.ToString() + "/repair_now/btn_high_repair").GetComponent<UIButton>();
			component.isEnabled = false;
			if (_low_anime)
			{
				Debug.Log("Crane:" + dockno + "を退場させます。");
				this._AM.Stop();
				iTween.MoveTo(this.CraneNormal[dockno].get_gameObject(), iTween.Hash(new object[]
				{
					"islocal",
					true,
					"x",
					520f,
					"time",
					5f
				}));
			}
			this.bd1.set_anime(dockno, true);
			this.bd1.set_HS_anime(dockno, true);
			string arg_DF_0 = "board/Grid/0";
			int num2 = dockno;
			string text = arg_DF_0 + num2.ToString() + "/Anime_H";
			this._AMH = GameObject.Find(text).GetComponent<Animation>();
			this._AMH.Play();
			string arg_116_0 = "board/Grid/0";
			int num3 = dockno;
			GameObject.Find(arg_116_0 + num3.ToString() + "/Anime_H/crane1").GetComponent<UIPanel>().set_enabled(true);
			string arg_13F_0 = "board/Grid/0";
			int num4 = dockno;
			text = arg_13F_0 + num4.ToString() + "/repair_now/HP_Gauge";
			this._AMG = GameObject.Find(text).GetComponent<Animation>();
			this.gdock = dockno;
			this._isHPgrow = true;
			this._HPG = GameObject.Find(text + "/panel/HP_bar_meter2").GetComponent<UISprite>();
			iTween.ValueTo(base.get_gameObject(), iTween.Hash(new object[]
			{
				"from",
				this._HPG.width,
				"to",
				210,
				"time",
				0.75f,
				"delay",
				2.8f,
				"onupdate",
				"UpdateHandler"
			}));
		}

		public void onHighAnimeDone(int dock)
		{
			this.rep = GameObject.Find("Repair Root").GetComponent<repair>();
			int mode = this.rep.now_mode();
			this.rep.set_mode(-1);
			Debug.Log(dock + "番が入渠を終えた。");
			this.bd1.set_HS_anime(dock, false);
			this.bd1.set_anime(dock, false);
			this.rep.set_mode(mode);
		}

		public void onHighAnimeDone2(int dock)
		{
			int mode = this.rep.now_mode();
			this.rep.set_mode(-1);
			this.bd1.DockStatus(this.rep.NowArea(), dock);
			this.rep.set_mode(mode);
		}

		public void high_repair_anime2()
		{
		}

		private void UpdateHandler(float value)
		{
			this._HPG.width = 210;
			this.SetHpGauge(this._HPG, (int)value, 210);
		}

		public void SetHpGauge(UISprite gauge, int Now, int Max)
		{
			float num = 566f;
			if (gauge.parent.parent.get_name() == "00" || gauge.parent.parent.get_name() == "01" || gauge.parent.parent.get_name() == "02" || gauge.parent.parent.get_name() == "03")
			{
				int dock = int.Parse(gauge.parent.parent.get_name());
				int num2 = this.bd1.get_dock_MaxHP(dock);
				gauge.parent.get_transform().FindChild("text_hp").GetComponent<UILabel>().text = Now * num2 / Max + "/" + num2;
			}
			gauge.width = gauge.width * Now / Max;
			float num3 = num * (float)(Max - Now) / (float)Max;
			if (num3 <= 55f)
			{
				float num4 = 0f;
				float num5 = Mathe.Rate(0f, 255f, 200f);
				if (num3 != 0f)
				{
					num4 = Mathe.Rate(0f, 255f, num3);
				}
				Color color = new Color(0f, num5 + num4, 0f, 1f);
				gauge.color = color;
			}
			else if (num3 <= 311f)
			{
				float num6 = Mathe.Rate(0f, 255f, num3 - 55f);
				Color color2 = new Color(num6, 1f, 0f, 1f);
				gauge.color = color2;
			}
			else
			{
				float num7 = Mathe.Rate(0f, 255f, num3 - 311f);
				Color color3 = new Color(1f, 1f - num7, 0f, 1f);
				gauge.color = color3;
			}
		}
	}
}
