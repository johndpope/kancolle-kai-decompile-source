using local.models;
using System;
using UnityEngine;

namespace KCV.Arsenal
{
	public class UIHowToArsenal : MonoBehaviour
	{
		[SerializeField]
		private GameObject _DesideBtn;

		[SerializeField]
		private GameObject _CancelBtn;

		[SerializeField]
		private GameObject _CrossBtn;

		[SerializeField]
		private TaskMainArsenalManager taskMainArsenalManager;

		[SerializeField]
		private GameObject _Btn_L;

		[SerializeField]
		private GameObject _Btn_R;

		[SerializeField]
		private GameObject _Btn_Shikaku;

		[SerializeField]
		private TaskArsenalListManager taskArsenalListManager;

		private UILabel _DesideBtnLabel;

		private UILabel _CancelBtnLabel;

		private UISprite _DesideBtnSprite;

		private UISprite _CrossBtnSprite;

		private UISprite _ShikakuBtnSprite;

		private UILabel _Btn_L_Label;

		private UILabel _Btn_R_Label;

		private UILabel _Btn_Shikaku_Label;

		private KeyControl key;

		private KeyControl key2;

		private float time;

		private bool isShow;

		private bool _now_mode;

		private static readonly Vector3 ShowPos = new Vector3(-480f, -259f, 0f);

		private static readonly Vector3 HidePos = new Vector3(-480f, -289f, 0f);

		private SettingModel model;

		private bool _IsLightFocusLeft;

		private string Now_status = string.Empty;

		private void Awake()
		{
			base.get_transform().localPositionY(UIHowToArsenal.HidePos.y);
			this.model = new SettingModel();
			this.key2 = new KeyControl(0, 1, 0.4f, 0.1f);
			this.key2.setChangeValue(0f, 0f, 0f, 0f);
			this._DesideBtnLabel = this._DesideBtn.get_transform().FindChild("Label").GetComponent<UILabel>();
			this._DesideBtnSprite = this._DesideBtn.get_transform().FindChild("Icon").GetComponent<UISprite>();
			this._IsLightFocusLeft = true;
			this.Now_status = string.Empty;
			this._DesideBtnLabel.text = "決定";
			this._DesideBtnSprite.spriteName = "btn_maru";
			this._CancelBtnLabel = this._CancelBtn.get_transform().FindChild("Label").GetComponent<UILabel>();
			this._CancelBtnLabel.text = "戻る";
			this._CrossBtnSprite = this._CrossBtn.get_transform().FindChild("Icon").GetComponent<UISprite>();
			this._CrossBtnSprite.spriteName = "arrow_UDLR";
			this._now_mode = true;
			this._Btn_L_Label = this._Btn_L.get_transform().FindChild("Label").GetComponent<UILabel>();
			this._Btn_L_Label.text = "提督コマンド";
			this._Btn_R_Label = this._Btn_R.get_transform().FindChild("Label").GetComponent<UILabel>();
			this._Btn_R_Label.text = "戦略へ";
			this._Btn_Shikaku.get_transform().set_localScale(Vector3.get_zero());
			this._Btn_Shikaku.get_transform().localPositionX(525f);
			this._ShikakuBtnSprite = this._Btn_Shikaku.get_transform().FindChild("Icon").GetComponent<UISprite>();
			this._Btn_Shikaku_Label = this._Btn_Shikaku.get_transform().FindChild("Label").GetComponent<UILabel>();
			this._Btn_Shikaku_Label.text = "高速建造材";
		}

		private void OnDestroy()
		{
			this._DesideBtn = null;
			this._CancelBtn = null;
			this._CrossBtn = null;
			this.taskMainArsenalManager = null;
			this._Btn_L = null;
			this._Btn_R = null;
			this._Btn_Shikaku = null;
			this.taskArsenalListManager = null;
			this.key = null;
			this.key2 = null;
			this._DesideBtnLabel = null;
			this._CancelBtnLabel = null;
			this._DesideBtnSprite = null;
			this._CrossBtnSprite = null;
			this._ShikakuBtnSprite = null;
			this._Btn_L_Label = null;
			this._Btn_R_Label = null;
			this._Btn_Shikaku_Label = null;
			this.model = null;
		}

		public void change_guide()
		{
			if (GameObject.Find("DismantlePanel").get_transform().get_localPosition().x < 10f && GameObject.Find("DismantlePanel/UIShipSortButton") != null && GameObject.Find("DismantlePanel/OverlayBtn4").get_transform().get_localPosition().x != -344f)
			{
				this._Btn_Shikaku.get_transform().set_localScale(Vector3.get_one());
				this._Btn_Shikaku.get_transform().localPositionX(525f);
				this._ShikakuBtnSprite.spriteName = "btn_sankaku";
				this._Btn_Shikaku_Label.text = "ソート";
			}
			else
			{
				this._Btn_Shikaku.get_transform().localPositionX(525f);
				this._ShikakuBtnSprite.spriteName = "btn_shikaku";
				this._Btn_Shikaku_Label.text = "高速建造材";
				if (this.taskMainArsenalManager.isInConstructDialog())
				{
					this._Btn_Shikaku.get_transform().set_localScale(Vector3.get_zero());
					this._Btn_Shikaku.get_transform().localPositionX(525f);
				}
				else
				{
					this._Btn_Shikaku.get_transform().set_localScale(Vector3.get_one());
					this._Btn_Shikaku.get_transform().localPositionX(525f);
				}
			}
			this._now_mode = this.taskMainArsenalManager.isInConstructDialog();
			if (this.taskArsenalListManager._ShikakuON)
			{
				this._DesideBtnSprite.spriteName = "btn_shikaku";
			}
			else
			{
				this._DesideBtnSprite.spriteName = "btn_maru";
			}
		}

		private void _setButtonX(string text, float posX)
		{
			this._CancelBtn.get_transform().set_localScale((!(text == string.Empty)) ? Vector3.get_one() : Vector3.get_zero());
			this._CancelBtn.get_transform().localPositionX(posX);
			this._CancelBtnLabel.text = text;
		}

		public void DesideBtnChange()
		{
		}

		private void Update()
		{
			this.change_guide();
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
			Util.MoveTo(base.get_gameObject(), 0.4f, UIHowToArsenal.ShowPos, iTween.EaseType.easeInSine);
			this.isShow = true;
		}

		public void Hide()
		{
			Util.MoveTo(base.get_gameObject(), 0.4f, UIHowToArsenal.HidePos, iTween.EaseType.easeInSine);
			this.isShow = false;
		}
	}
}
