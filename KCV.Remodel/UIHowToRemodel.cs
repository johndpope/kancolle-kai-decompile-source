using local.models;
using System;
using UnityEngine;

namespace KCV.Remodel
{
	public class UIHowToRemodel : MonoBehaviour
	{
		[SerializeField]
		private UserInterfaceRemodelManager _rm;

		[SerializeField]
		private GameObject SpriteButtonX;

		[SerializeField]
		private GameObject SpriteButtonL;

		[SerializeField]
		private GameObject SpriteButtonR;

		[SerializeField]
		private GameObject SpriteButtonShikaku;

		[SerializeField]
		private GameObject SpriteButtonO;

		[SerializeField]
		private GameObject SpriteStickR;

		private KeyControl key;

		private KeyControl key2;

		private ScreenStatus _now_mode;

		private SettingModel model;

		private UILabel _uil;

		private float time;

		private bool isShow;

		private static readonly Vector3 ShowPos = Vector3.get_right() * -450f + Vector3.get_up() * -259f;

		private static readonly Vector3 HidePos = Vector3.get_right() * -450f + Vector3.get_up() * -289f;

		private void Awake()
		{
			base.get_transform().localPositionY(UIHowToRemodel.HidePos.y);
			this.model = new SettingModel();
			this.key2 = new KeyControl(0, 1, 0.4f, 0.1f);
			this.key2.setChangeValue(0f, 0f, 0f, 0f);
			this.SpriteButtonShikaku.get_transform().set_localScale(Vector3.get_zero());
			this._now_mode = ScreenStatus.MODE_SOUBI_HENKOU;
			if (this.SpriteButtonR == null)
			{
				Util.FindParentToChild(ref this.SpriteButtonR, this.SpriteButtonL.get_transform().get_parent(), "SpriteButtonR");
			}
			if (this.SpriteButtonO == null)
			{
				Util.FindParentToChild(ref this.SpriteButtonO, this.SpriteButtonL.get_transform().get_parent(), "SpriteButtonO");
			}
		}

		private void R_slide(bool value)
		{
			if (!value)
			{
				this.SpriteButtonL.get_transform().localPositionX(186f);
				this.SpriteButtonR.get_transform().localPositionX(347f);
				this.SpriteButtonO.get_transform().localPositionX(444f);
				this.SpriteButtonX.get_transform().localPositionX(529f);
				this.SpriteButtonShikaku.get_transform().localPositionX(609f);
			}
			else
			{
				this.SpriteButtonL.get_transform().localPositionX(66f);
				this.SpriteButtonR.get_transform().localPositionX(227f);
				this.SpriteButtonO.get_transform().localPositionX(324f);
				this.SpriteButtonX.get_transform().localPositionX(409f);
				this.SpriteButtonShikaku.get_transform().localPositionX(489f);
			}
		}

		private void change_guide()
		{
			if (this._now_mode == this._rm.status)
			{
				return;
			}
			switch (this._now_mode)
			{
			case ScreenStatus.SELECT_OTHER_SHIP:
			case ScreenStatus.MODE_KINDAIKA_KAISHU_SOZAI_SENTAKU:
				this.SpriteStickR.SetActive(true);
				this.SpriteButtonShikaku.GetComponent<UISprite>().spriteName = "btn_shikaku";
				this.SpriteButtonShikaku.get_transform().Find("Label").GetComponent<UILabel>().text = "全てはずす";
				this.SpriteButtonShikaku.get_transform().set_localScale(Vector3.get_zero());
				this.R_slide(false);
				break;
			case ScreenStatus.SELECT_SETTING_MODE:
				this.SpriteStickR.SetActive(true);
				this.R_slide(false);
				break;
			case ScreenStatus.MODE_SOUBI_HENKOU:
				this.SpriteStickR.SetActive(true);
				this.SpriteButtonShikaku.SetActive(false);
				this.R_slide(false);
				break;
			case ScreenStatus.MODE_SOUBI_HENKOU_ITEM_SELECT:
				this.SpriteButtonShikaku.SetActive(false);
				this.R_slide(true);
				break;
			case ScreenStatus.MODE_KINDAIKA_KAISHU:
				this.SpriteStickR.SetActive(true);
				this.R_slide(false);
				break;
			}
			this._now_mode = this._rm.status;
			switch (this._rm.status)
			{
			case ScreenStatus.SELECT_OTHER_SHIP:
				this.SpriteButtonShikaku.SetActive(true);
				this.SpriteButtonShikaku.GetComponent<UISprite>().spriteName = "btn_sankaku";
				this.SpriteButtonShikaku.get_transform().Find("Label").GetComponent<UILabel>().text = "ソート";
				this.SpriteButtonShikaku.get_transform().set_localScale(Vector3.get_one());
				return;
			case ScreenStatus.SELECT_SETTING_MODE:
				this.SpriteStickR.SetActive(false);
				this.R_slide(true);
				return;
			case ScreenStatus.MODE_SOUBI_HENKOU:
				this.SpriteButtonShikaku.get_transform().set_localScale(Vector3.get_one());
				this._uil = this.SpriteButtonShikaku.get_transform().FindChild("Label").GetComponent<UILabel>();
				this.SpriteStickR.SetActive(false);
				this.R_slide(true);
				this.SpriteButtonShikaku.SetActive(true);
				this._uil.text = "全てはずす";
				return;
			case ScreenStatus.MODE_SOUBI_HENKOU_TYPE_SELECT:
				this.SpriteStickR.SetActive(false);
				this.R_slide(true);
				return;
			case ScreenStatus.MODE_SOUBI_HENKOU_ITEM_SELECT:
				this.SpriteButtonShikaku.SetActive(true);
				this.R_slide(true);
				this.SpriteButtonShikaku.get_transform().set_localScale(Vector3.get_one());
				this._uil = this.SpriteButtonShikaku.get_transform().FindChild("Label").GetComponent<UILabel>();
				this._uil.text = "装備ロック";
				return;
			case ScreenStatus.MODE_KINDAIKA_KAISHU:
				this.SpriteStickR.SetActive(false);
				this.R_slide(true);
				return;
			case ScreenStatus.MODE_KINDAIKA_KAISHU_SOZAI_SENTAKU:
				this.SpriteStickR.SetActive(false);
				this.R_slide(true);
				this.SpriteButtonShikaku.SetActive(true);
				this.SpriteButtonShikaku.GetComponent<UISprite>().spriteName = "btn_sankaku";
				this.SpriteButtonShikaku.get_transform().Find("Label").GetComponent<UILabel>().text = "ソート";
				this.SpriteButtonShikaku.get_transform().set_localScale(Vector3.get_one());
				return;
			case ScreenStatus.MODE_KINDAIKA_KAISHU_KAKUNIN:
				this.SpriteStickR.SetActive(false);
				this.R_slide(true);
				return;
			case ScreenStatus.MODE_KAIZO:
				this.SpriteStickR.SetActive(false);
				this.R_slide(true);
				return;
			}
			this.SpriteButtonShikaku.get_transform().set_localScale(Vector3.get_zero());
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
				else if (2f < this.time && !this.isShow && !this._rm.guideoff)
				{
					this.Show();
				}
			}
			this.change_guide();
			if (this._rm.guideoff)
			{
				this.Hide();
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
			if (!this.model.GuideDisplay)
			{
				return;
			}
			Util.MoveTo(base.get_gameObject(), 0.4f, UIHowToRemodel.ShowPos, iTween.EaseType.easeInSine);
			this.isShow = true;
		}

		public void Hide()
		{
			Util.MoveTo(base.get_gameObject(), 0.4f, UIHowToRemodel.HidePos, iTween.EaseType.easeInSine);
			this.isShow = false;
		}

		private void OnDestroy()
		{
			this._rm = null;
			this.SpriteButtonX = null;
			this.SpriteButtonL = null;
			this.SpriteButtonR = null;
			this.SpriteButtonShikaku = null;
			this.SpriteButtonO = null;
			this.SpriteStickR = null;
			this.key = null;
			this.key2 = null;
			this.model = null;
			this._uil = null;
		}
	}
}
