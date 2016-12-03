using System;
using UnityEngine;

namespace KCV
{
	public class SWmini : MonoBehaviour
	{
		private string _WhoamI;

		private Animation _Ani;

		private bool _Now_State;

		private KeyControl KeyController;

		public bool _get_nowstate()
		{
			return this._Now_State;
		}

		public void _change_state(bool val)
		{
			this._Now_State = val;
		}

		private void Start()
		{
			this._init_repair();
		}

		public void _init_repair()
		{
			this._Now_State = false;
			this._Ani = base.get_gameObject().GetComponent<Animation>();
			this._Ani.Play();
			this.KeyController = new KeyControl(0, 0, 0.4f, 0.1f);
			this.KeyController.setChangeValue(0f, 0f, 0f, 0f);
		}

		public void _toggle_state()
		{
			if (this._Now_State)
			{
				this._Now_State = false;
			}
			else
			{
				this._Now_State = true;
			}
		}

		public void OnClick()
		{
			this._toggle_state();
			this._sw_draw();
		}

		private void _sw_draw()
		{
			Debug.Log(base.get_gameObject().get_name() + "/switch_ball");
			if (this._Now_State)
			{
				this._Ani.Play("mini_up");
				GameObject.Find(base.get_gameObject().get_name() + "/switch_ball").GetComponent<UISprite>().spriteName = "switch_on_pin";
				GameObject.Find(base.get_gameObject().get_name() + "/switch_use").GetComponent<UISprite>().spriteName = "switch_m_on";
				GameObject.Find(base.get_gameObject().get_name() + "/z").ScaleTo(new Vector3(0f, 0f, 0f), 0.01f);
			}
			else
			{
				this._Ani.Play("mini_down");
				GameObject.Find(base.get_gameObject().get_name() + "/switch_ball").GetComponent<UISprite>().spriteName = "switch_off_pin";
				GameObject.Find(base.get_gameObject().get_name() + "/switch_use").GetComponent<UISprite>().spriteName = "switch_m_off";
				GameObject.Find(base.get_gameObject().get_name() + "/z").ScaleTo(new Vector3(1f, 1f, 1f), 0.01f);
			}
		}

		private void Update()
		{
			this.KeyController.Update();
			if (this.KeyController.keyState.get_Item(1).down)
			{
				this.OnClick();
			}
			else if (this.KeyController.keyState.get_Item(2).down)
			{
				this.OnClick();
			}
		}
	}
}
