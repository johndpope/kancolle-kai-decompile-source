using KCV.Scene.Port;
using local.models;
using System;
using UnityEngine;

namespace KCV.Scene.Duty
{
	public class UIHowToDuty : MonoBehaviour
	{
		private KeyControl key;

		private KeyControl key2;

		private float time;

		private bool isShow;

		[SerializeField]
		private UserInterfaceDutyManager _dm;

		[SerializeField]
		private GameObject SpriteButtonX;

		[SerializeField]
		private GameObject SpriteButtonShikaku;

		private bool now_mode;

		private readonly Vector3 ShowPos = new Vector3(-450f, -259f, 0f);

		private readonly Vector3 HidePos = new Vector3(-450f, -289f, 0f);

		private SettingModel model;

		private UILabel _uil;

		private UISprite _uis;

		private void Awake()
		{
			base.get_transform().localPositionY(this.HidePos.y);
			this.model = new SettingModel();
			this.key2 = new KeyControl(0, 1, 0.4f, 0.1f);
			this.key2.setChangeValue(0f, 0f, 0f, 0f);
			this.now_mode = false;
			this.SpriteButtonShikaku.get_transform().set_localScale(Vector3.get_one());
			this._uil = this.SpriteButtonX.get_transform().FindChild("Label").GetComponent<UILabel>();
			this._setButtonX("戻る", 605f);
			base.GetComponent<UIPanel>().depth = 0;
		}

		private void change_guide()
		{
			if (this.now_mode == this._dm._DeteilMode)
			{
				return;
			}
			this.now_mode = this._dm._DeteilMode;
			if (this._dm._DeteilMode)
			{
				this._setButtonX("戻る", 432f);
				this.SpriteButtonShikaku.get_transform().set_localScale(Vector3.get_zero());
			}
			else
			{
				this._setButtonX("戻る", 605f);
				this.SpriteButtonShikaku.get_transform().set_localScale(Vector3.get_one());
			}
		}

		private void _setButtonX(string text, float posX)
		{
			this.SpriteButtonX.get_transform().set_localScale((!(text == string.Empty)) ? Vector3.get_one() : Vector3.get_zero());
			this.SpriteButtonX.get_transform().localPositionX(posX);
			this._uil.text = text;
		}

		private void Update()
		{
			this.key2.Update();
			this.SetKeyController(this.key2);
			if (this.key != null && this.key.IsRun)
			{
				this.time += Time.get_deltaTime();
				if (this.key.IsAnyKey)
				{
					this.time = 0f;
					if (this.isShow)
					{
						this.Hide();
					}
				}
				else if (2f < this.time && !this.isShow)
				{
					this.Show();
				}
			}
			this.change_guide();
		}

		public void SetKeyController(KeyControl key)
		{
			this.key = key;
			if (key == null && this.isShow)
			{
				this.Hide();
			}
		}

		public void Show()
		{
			if (!this.model.GuideDisplay)
			{
				return;
			}
			Util.MoveTo(base.get_gameObject(), 0.4f, this.ShowPos, iTween.EaseType.easeInSine);
			this.isShow = true;
		}

		public void Hide()
		{
			Util.MoveTo(base.get_gameObject(), 0.4f, this.HidePos, iTween.EaseType.easeInSine);
			this.isShow = false;
		}

		private void OnDestriy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref this._uil);
			UserInterfacePortManager.ReleaseUtils.Release(ref this._uis);
			this.key = null;
			this.key2 = null;
			this._dm = null;
			this.SpriteButtonX = null;
			this.SpriteButtonShikaku = null;
			this.model = null;
		}
	}
}
