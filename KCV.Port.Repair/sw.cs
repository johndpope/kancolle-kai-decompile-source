using KCV.PopupString;
using KCV.Utils;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Port.Repair
{
	public class sw : MonoBehaviour
	{
		private UISprite sw_ball;

		private UISprite sw_base;

		private Animation _Ani;

		private board3 bd3;

		private dialog dia;

		private bool sw_enable = true;

		private repair rep;

		private GameObject _zzz;

		public bool sw_stat;

		public bool _fairy_onoff;

		private void Awake()
		{
			this.sw_ball = GameObject.Find("sw_mini/switch_ball").GetComponent<UISprite>();
			this.sw_base = GameObject.Find("sw_mini/switch_use").GetComponent<UISprite>();
			this._Ani = GameObject.Find("sw_mini").GetComponent<Animation>();
			this._zzz = GameObject.Find("SleepPar");
		}

		private void Start()
		{
			this.rep = GameObject.Find("Repair Root").GetComponent<repair>();
			this._init_repair();
		}

		public void _init_repair()
		{
			this.sw_enable = true;
			this.sw_stat = false;
			this._fairy_onoff = false;
			this.sw_ball = GameObject.Find("sw_mini/switch_ball").GetComponent<UISprite>();
			this.sw_base = GameObject.Find("sw_mini/switch_use").GetComponent<UISprite>();
			this._Ani = GameObject.Find("sw_mini").GetComponent<Animation>();
			this._zzz = GameObject.Find("SleepPar");
			this.rep = GameObject.Find("Repair Root").GetComponent<repair>();
			this.bd3 = GameObject.Find("board3_top/board3").GetComponent<board3>();
		}

		private void OnDestroy()
		{
			Mem.Del(ref this.sw_ball);
			Mem.Del(ref this.sw_base);
			Mem.Del<Animation>(ref this._Ani);
			Mem.Del<board3>(ref this.bd3);
			Mem.Del<dialog>(ref this.dia);
			Mem.Del<repair>(ref this.rep);
			Mem.Del<GameObject>(ref this._zzz);
		}

		public void OnClick()
		{
			if (this.rep.now_repairkit() < 1)
			{
				CommonPopupDialog.Instance.StartPopup(Util.getPopupMessage(PopupMess.NotEnoughHighSpeedRepairKit));
				SoundUtils.PlaySE(SEFIleInfos.CommonCancel2);
				return;
			}
			this.togggleSW();
		}

		[DebuggerHidden]
		public IEnumerator switch_ONOFF()
		{
			sw.<switch_ONOFF>c__IteratorBF <switch_ONOFF>c__IteratorBF = new sw.<switch_ONOFF>c__IteratorBF();
			<switch_ONOFF>c__IteratorBF.<>f__this = this;
			return <switch_ONOFF>c__IteratorBF;
		}

		public void set_sw_stat(bool val)
		{
			this.sw_enable = val;
		}

		public void togggleSW()
		{
			this.togggleSW(false);
		}

		public void togggleSW(bool value)
		{
			if (this._zzz == null)
			{
				this._zzz = GameObject.Find("SleepPar");
			}
			if (!this.sw_enable)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonWrong);
				return;
			}
			if (!this.sw_stat)
			{
				this._Ani.Play("mini_up");
				GameObject.Find("sw_mini/switch_ball").GetComponent<UISprite>().spriteName = "switch_on_pin";
				GameObject.Find("sw_mini/switch_use").GetComponent<UISprite>().spriteName = "switch_m_on";
				this._zzz.GetComponent<ParticleSystem>().Stop();
				this._zzz.get_transform().set_localScale(Vector3.get_zero());
				this.sw_stat = true;
			}
			else
			{
				this._Ani.Play("mini_down");
				GameObject.Find("sw_mini/switch_ball").GetComponent<UISprite>().spriteName = "switch_off_pin";
				GameObject.Find("sw_mini/switch_use").GetComponent<UISprite>().spriteName = "switch_m_off";
				this._zzz.GetComponent<ParticleSystem>().Play();
				this._zzz.get_transform().set_localScale(Vector3.get_one());
				this.sw_stat = false;
			}
			this.dia = GameObject.Find("dialog").GetComponent<dialog>();
			this.dia.UpdateSW(this.sw_stat);
			this.dia.SetSpeed(this.sw_stat);
			SoundUtils.PlaySE(SEFIleInfos.MainMenuOnClick);
		}

		public bool getSW()
		{
			return this.sw_stat;
		}

		public void setSW(bool stat)
		{
			this.setSW(stat, false);
		}

		public void setSW(bool stat, bool isSlow)
		{
			if (!this.sw_enable)
			{
				return;
			}
			if (stat == this.sw_stat)
			{
				return;
			}
			if (stat)
			{
				this._Ani.Play("mini_up");
				GameObject.Find("sw_mini/switch_ball").GetComponent<UISprite>().spriteName = "switch_on_pin";
				GameObject.Find("sw_mini/switch_use").GetComponent<UISprite>().spriteName = "switch_m_on";
				this._zzz.GetComponent<ParticleSystem>().Stop();
				this._zzz.SetActive(false);
				this.sw_stat = true;
			}
			else
			{
				if (isSlow)
				{
					this._Ani.Play("mini_down");
				}
				else
				{
					this._Ani.Play("mini_down");
				}
				GameObject.Find("sw_mini/switch_ball").GetComponent<UISprite>().spriteName = "switch_off_pin";
				GameObject.Find("sw_mini/switch_use").GetComponent<UISprite>().spriteName = "switch_m_off";
				this._zzz.GetComponent<ParticleSystem>().Play();
				this._zzz.SetActive(true);
				this.sw_stat = false;
			}
			this.dia = GameObject.Find("dialog").GetComponent<dialog>();
			this.dia.UpdateSW(this.sw_stat);
			this.dia.SetSpeed(this.sw_stat);
		}
	}
}
