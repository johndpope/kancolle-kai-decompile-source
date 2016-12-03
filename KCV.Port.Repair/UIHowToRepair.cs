using local.models;
using System;
using UnityEngine;

namespace KCV.Port.Repair
{
	public class UIHowToRepair : MonoBehaviour
	{
		private KeyControl key;

		private KeyControl key2;

		private float time;

		private bool isShow;

		private repair _rep;

		[SerializeField]
		private GameObject SpriteButtonX;

		[SerializeField]
		private GameObject SpriteButtonL;

		[SerializeField]
		private GameObject SpriteButtonShikaku;

		[SerializeField]
		private GameObject SpriteButtonSankaku;

		private UILabel _uiLabelX;

		private int now_mode;

		private static readonly Vector3 ShowPos = Vector3.get_right() * -450f + Vector3.get_up() * -259f;

		private static readonly Vector3 HidePos = Vector3.get_right() * -450f + Vector3.get_up() * -289f;

		private SettingModel model;

		private UILabel _uil;

		private void Awake()
		{
			base.get_transform().localPositionY(UIHowToRepair.HidePos.y);
			this.model = new SettingModel();
			this.key2 = new KeyControl(0, 1, 0.4f, 0.1f);
			this.key2.setChangeValue(0f, 0f, 0f, 0f);
			this._rep = base.get_gameObject().get_transform().get_parent().GetComponent<repair>();
			this.now_mode = 1;
			this.SpriteButtonX.get_transform().set_localScale(Vector3.get_zero());
			this._uil = this.SpriteButtonL.get_transform().FindChild("Label").GetComponent<UILabel>();
			this._uil.text = "提督コマンド";
			Util.FindParentToChild<UILabel>(ref this._uiLabelX, this.SpriteButtonX.get_transform(), "Label");
			this._setButtonX("戻る", 429f);
			this.SpriteButtonShikaku.get_transform().set_localScale(Vector3.get_zero());
			this.SpriteButtonSankaku.get_transform().set_localScale(Vector3.get_zero());
		}

		private void OnDestroy()
		{
			this.key = null;
			this.key2 = null;
			this._rep = null;
			this.SpriteButtonX = null;
			this.SpriteButtonL = null;
			this.SpriteButtonShikaku = null;
			this.SpriteButtonSankaku = null;
			this._uiLabelX = null;
			this.model = null;
			this._uil = null;
		}

		private void change_guide()
		{
			if (this._rep.now_mode() != this.now_mode)
			{
				this.now_mode = this._rep.now_mode();
				if (this.now_mode == 1)
				{
					this._setButtonX("戻る", 429f);
				}
				else
				{
					this._setButtonX("戻る", 429f);
				}
				if (this.now_mode == 2)
				{
					this.SpriteButtonSankaku.get_transform().set_localScale(Vector3.get_one());
				}
				else
				{
					this.SpriteButtonSankaku.get_transform().set_localScale(Vector3.get_zero());
				}
				if (this.now_mode == 3)
				{
					this.SpriteButtonShikaku.get_transform().set_localScale(Vector3.get_one());
				}
				else
				{
					this.SpriteButtonShikaku.get_transform().set_localScale(Vector3.get_zero());
				}
			}
		}

		private void _setButtonX(string text, float posX)
		{
			this.SpriteButtonX.get_transform().set_localScale((!(text == string.Empty)) ? Vector3.get_one() : Vector3.get_zero());
			this.SpriteButtonX.get_transform().localPositionX(posX);
			this._uiLabelX.text = text;
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
			Util.MoveTo(base.get_gameObject(), 0.4f, UIHowToRepair.ShowPos, iTween.EaseType.easeInSine);
			this.isShow = true;
		}

		public void Hide()
		{
			Util.MoveTo(base.get_gameObject(), 0.4f, UIHowToRepair.HidePos, iTween.EaseType.easeInSine);
			this.isShow = false;
		}
	}
}
