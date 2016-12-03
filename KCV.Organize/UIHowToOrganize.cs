using KCV.Remodel;
using local.models;
using System;
using UnityEngine;

namespace KCV.Organize
{
	public class UIHowToOrganize : MonoBehaviour
	{
		[SerializeField]
		private TaskOrganizeTop _tot;

		[SerializeField]
		private OrganizeTender _tod;

		[SerializeField]
		private GameObject SpriteButtonX;

		[SerializeField]
		private GameObject _buttonSR;

		[SerializeField]
		private GameObject _buttonL;

		[SerializeField]
		private GameObject _buttonR;

		[SerializeField]
		private GameObject _buttonMaru;

		[SerializeField]
		private GameObject _buttonBatu;

		[SerializeField]
		private GameObject _buttonShikaku;

		[SerializeField]
		private GameObject _buttonSankaku;

		private UILabel _uiLabelShikaku;

		private UILabel _uiLabelBatu;

		private ScreenStatus _now_mode;

		private KeyControl key;

		private KeyControl key2;

		private float time;

		private bool isShow;

		private static readonly Vector3 ShowPos = Vector3.get_right() * 600f + Vector3.get_up() * -959f;

		private static readonly Vector3 HidePos = Vector3.get_right() * 600f + Vector3.get_up() * -989f;

		private SettingModel model;

		private void Awake()
		{
			base.get_transform().localPositionY(UIHowToOrganize.HidePos.y);
			this.model = new SettingModel();
			this.key2 = new KeyControl(0, 1, 0.4f, 0.1f);
			this.key2.setChangeValue(0f, 0f, 0f, 0f);
			this._buttonShikaku.get_transform().set_localScale(Vector3.get_zero());
			Util.FindParentToChild<UILabel>(ref this._uiLabelShikaku, this._buttonShikaku.get_transform(), "Label");
			Util.FindParentToChild<UILabel>(ref this._uiLabelBatu, this._buttonBatu.get_transform(), "Label");
			this._uiLabelShikaku.text = "はずす";
		}

		private void change_guide()
		{
			switch (this._tot._state2)
			{
			case TaskOrganizeTop.OrganizeState.Top:
				this._setButtonX("戻る", 656f);
				this._setButtonShikaku("一括解除", 530f);
				this._buttonL.get_transform().localPositionX(186f);
				this._buttonR.get_transform().localPositionX(347f);
				this._buttonMaru.get_transform().localPositionX(447f);
				this._buttonSankaku.get_transform().set_localScale(Vector3.get_zero());
				this._buttonSR.get_transform().set_localScale(Vector3.get_one());
				break;
			case TaskOrganizeTop.OrganizeState.Detail:
				this._setButtonX("戻る", 413f);
				this._setButtonShikaku(string.Empty, 0f);
				this._buttonL.get_transform().localPositionX(67f);
				this._buttonR.get_transform().localPositionX(229f);
				this._buttonMaru.get_transform().localPositionX(328f);
				this._buttonSR.get_transform().set_localScale(Vector3.get_zero());
				this._buttonSankaku.get_transform().set_localScale(Vector3.get_zero());
				break;
			case TaskOrganizeTop.OrganizeState.DetailList:
				this._setButtonX("戻る", 413f);
				this._setButtonShikaku("ロック", 490f);
				this._buttonL.get_transform().localPositionX(67f);
				this._buttonR.get_transform().localPositionX(229f);
				this._buttonMaru.get_transform().localPositionX(328f);
				this._buttonSR.get_transform().set_localScale(Vector3.get_zero());
				this._buttonSankaku.get_transform().set_localScale(Vector3.get_zero());
				break;
			case TaskOrganizeTop.OrganizeState.List:
				this._setButtonX("戻る", 413f);
				this._setButtonShikaku("ロック", 490f);
				this._buttonL.get_transform().localPositionX(67f);
				this._buttonR.get_transform().localPositionX(229f);
				this._buttonMaru.get_transform().localPositionX(328f);
				this._buttonSR.get_transform().set_localScale(Vector3.get_zero());
				this._buttonSankaku.get_transform().set_localScale(Vector3.get_one());
				this._buttonSankaku.get_transform().localPositionX(583f);
				break;
			case TaskOrganizeTop.OrganizeState.System:
				this._setButtonX("戻る", 530f);
				this._setButtonShikaku(string.Empty, 0f);
				this._buttonL.get_transform().localPositionX(186f);
				this._buttonR.get_transform().localPositionX(347f);
				this._buttonMaru.get_transform().localPositionX(447f);
				this._buttonSankaku.get_transform().set_localScale(Vector3.get_zero());
				this._buttonSR.get_transform().set_localScale(Vector3.get_one());
				break;
			case TaskOrganizeTop.OrganizeState.Tender:
				this._setButtonX("戻る", 413f);
				this._setButtonShikaku(string.Empty, 0f);
				this._buttonL.get_transform().localPositionX(67f);
				this._buttonR.get_transform().localPositionX(229f);
				this._buttonMaru.get_transform().localPositionX(328f);
				this._buttonSR.get_transform().set_localScale(Vector3.get_zero());
				this._buttonSankaku.get_transform().set_localScale(Vector3.get_zero());
				break;
			}
		}

		private void _setButtonShikaku(string text, float posX)
		{
			this._buttonShikaku.get_transform().set_localScale((!(text == string.Empty)) ? Vector3.get_one() : Vector3.get_zero());
			this._buttonShikaku.get_transform().localPositionX(posX);
			this._uiLabelShikaku.text = text;
		}

		private void _setButtonX(string text, float posX)
		{
			this._buttonBatu.get_transform().set_localScale((!(text == string.Empty)) ? Vector3.get_one() : Vector3.get_zero());
			this._buttonBatu.get_transform().localPositionX(posX);
			this._uiLabelBatu.text = text;
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
					if (this.isShow || this._tod._GuideOff)
					{
						this.Hide();
					}
				}
				else if (2f < this.time && !this.isShow && !this._tod._GuideOff)
				{
					this.Show();
				}
			}
			this.change_guide();
			if (this._tod._GuideOff)
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
			Util.MoveTo(base.get_gameObject(), 0.4f, UIHowToOrganize.ShowPos, iTween.EaseType.easeInSine);
			this.isShow = true;
		}

		public void Hide()
		{
			Util.MoveTo(base.get_gameObject(), 0.4f, UIHowToOrganize.HidePos, iTween.EaseType.easeInSine);
			this.isShow = false;
		}

		private void OnDestroy()
		{
			this._tot = null;
			this._tod = null;
			this.SpriteButtonX = null;
			this._buttonSR = null;
			this._buttonL = null;
			this._buttonR = null;
			this._buttonMaru = null;
			this._buttonBatu = null;
			this._buttonShikaku = null;
			this._buttonSankaku = null;
			this._uiLabelShikaku = null;
			this._uiLabelBatu = null;
			this.key = null;
			this.key2 = null;
			this.model = null;
		}
	}
}
