using KCV.Supply;
using local.models;
using System;
using UnityEngine;

namespace KCV.Port.Supply
{
	public class UIHowToSupply : MonoBehaviour
	{
		private KeyControl key;

		private KeyControl key2;

		private float time;

		private bool isShow;

		[SerializeField]
		private GameObject SpriteButtonX;

		[SerializeField]
		private GameObject SpriteButtonL;

		[SerializeField]
		private GameObject SpriteButtonR;

		[SerializeField]
		private GameObject SpriteButtonShikaku;

		[SerializeField]
		private GameObject SpriteStickR;

		private UILabel _uiLabelX;

		private int now_mode;

		private static readonly Vector3 ShowPos = new Vector3(-450f, -259f, 0f);

		private static readonly Vector3 HidePos = new Vector3(-450f, -289f, 0f);

		private SettingModel model;

		private SupplyMainManager _smm;

		private UILabel _uil;

		private UISprite _uis;

		private void Awake()
		{
			base.get_transform().localPositionY(UIHowToSupply.HidePos.y);
			this.model = new SettingModel();
			this.key2 = new KeyControl(0, 1, 0.4f, 0.1f);
			this.key2.setChangeValue(0f, 0f, 0f, 0f);
			this.now_mode = 1;
			this.SpriteButtonX.get_transform().set_localScale(Vector3.get_zero());
			this._uil = this.SpriteButtonL.get_transform().FindChild("Label").GetComponent<UILabel>();
			this._uil.text = "提督コマンド";
			this._uil = this.SpriteButtonR.get_transform().FindChild("Label").GetComponent<UILabel>();
			this._smm = base.get_transform().get_parent().GetComponent<SupplyMainManager>();
			Util.FindParentToChild<UILabel>(ref this._uiLabelX, this.SpriteButtonX.get_transform(), "Label");
		}

		private void OnDestroy()
		{
			this.key = null;
			this.key2 = null;
			this.SpriteButtonX = null;
			this.SpriteButtonL = null;
			this.SpriteButtonR = null;
			this.SpriteButtonShikaku = null;
			this.SpriteStickR = null;
			this.model = null;
			this._smm = null;
			this._uil = null;
			this._uis = null;
		}

		private void change_guide()
		{
			if (this._smm.isNowRightFocus())
			{
				this.SpriteButtonShikaku.get_transform().set_localScale(Vector3.get_zero());
				this._setButtonX("戻る", 572f);
			}
			else if (this._smm.isNowDeckIsOther())
			{
				this.SpriteButtonShikaku.get_transform().set_localScale(Vector3.get_one());
				this._uil = this.SpriteButtonShikaku.get_transform().FindChild("Label").GetComponent<UILabel>();
				this._uil.text = "ソート";
				this._uis = this.SpriteButtonShikaku.GetComponent<UISprite>();
				this._uis.spriteName = "btn_sankaku";
				this._setButtonX("戻る", 662f);
			}
			else
			{
				this.SpriteButtonShikaku.get_transform().set_localScale(Vector3.get_one());
				this._uil = this.SpriteButtonShikaku.get_transform().FindChild("Label").GetComponent<UILabel>();
				this._uil.text = "まとめて選択";
				this._uis = this.SpriteButtonShikaku.GetComponent<UISprite>();
				this._uis.spriteName = "btn_shikaku";
				this._setButtonX("戻る", 727f);
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
			this.change_guide();
			if (!this.model.GuideDisplay)
			{
				return;
			}
			Util.MoveTo(base.get_gameObject(), 0.4f, UIHowToSupply.ShowPos, iTween.EaseType.easeInSine);
			this.isShow = true;
		}

		public void Hide()
		{
			Util.MoveTo(base.get_gameObject(), 0.4f, UIHowToSupply.HidePos, iTween.EaseType.easeInSine);
			this.isShow = false;
		}
	}
}
