using System;
using UnityEngine;

namespace KCV.Scene.Revamp.RepairShipAnime
{
	public class UIRepairShipAnime : MonoBehaviour
	{
		private UISprite _sprite;

		private Animation _ani;

		private bool _eye_move;

		private bool _pos_right;

		private void Start()
		{
			this._sprite = GameObject.Find("Eye").GetComponent<UISprite>();
			this._ani = GameObject.Find("RepairShipPanel").GetComponent<Animation>();
			this._eye_move = true;
			this._pos_right = false;
		}

		private void Update()
		{
			if (this._eye_move)
			{
				if ((int)Random.Range(0f, 100f) == 1)
				{
					this._ani.Play("akashi_eye_blink_open");
				}
				else if ((int)Random.Range(0f, 200f) == 1)
				{
					this._ani.Play("akashi_eye_kyoro_open");
				}
			}
		}

		public void eye_play(int ptn)
		{
			if (ptn < 1 || 5 < ptn)
			{
				ptn = 1;
			}
			this._sprite.spriteName = "a_eye" + ptn;
		}

		public void eye_motion(bool mode)
		{
			this._eye_move = mode;
			if (!this._eye_move)
			{
				this._sprite.spriteName = "a_eye1";
			}
		}
	}
}
