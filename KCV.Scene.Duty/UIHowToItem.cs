using local.models;
using System;
using UnityEngine;

namespace KCV.Scene.Duty
{
	public class UIHowToItem : MonoBehaviour
	{
		private KeyControl key;

		private KeyControl key2;

		private float time;

		private bool isShow;

		[SerializeField]
		private UserInterfaceItemManager _itemm;

		[SerializeField]
		private GameObject SpriteStickR;

		[SerializeField]
		private GameObject SpriteButtonO;

		[SerializeField]
		private GameObject SpriteButtonX;

		[SerializeField]
		private DialogAnimation mDialogAnimation_Exchange;

		[SerializeField]
		private DialogAnimation mDialogAnimation_UseLimit;

		[SerializeField]
		private DialogAnimation mDialogAnimation_StoreBuy;

		[SerializeField]
		private DialogAnimation mDialogAnimation_UseConfirm;

		private UILabel _uiLabelX;

		private bool now_mode;

		private static readonly Vector3 ShowPos = new Vector3(-450f, -259f, 0f);

		private static readonly Vector3 HidePos = new Vector3(-450f, -289f, 0f);

		private SettingModel model;

		private UILabel _uil;

		private UISprite _uis;

		private void Awake()
		{
			base.get_transform().localPositionY(UIHowToItem.HidePos.y);
			this.model = new SettingModel();
			this.key2 = new KeyControl(0, 1, 0.4f, 0.1f);
			this.key2.setChangeValue(0f, 0f, 0f, 0f);
			this.now_mode = true;
			this.SpriteButtonX.get_transform().set_localScale(Vector3.get_zero());
			Util.FindParentToChild<UILabel>(ref this._uiLabelX, this.SpriteButtonX.get_transform(), "Label");
			this._setButtonX("戻る", 550f);
			this.R_hide(false);
		}

		private void R_hide(bool value)
		{
			if (!value)
			{
				this.SpriteStickR.get_transform().localPositionX(336f);
				this.SpriteStickR.get_transform().set_localScale(Vector3.get_one());
				this.SpriteButtonO.get_transform().localPositionX(466f);
				this.SpriteButtonX.get_transform().localPositionX(551f);
			}
			else
			{
				this.SpriteStickR.get_transform().localPositionX(336f);
				this.SpriteStickR.get_transform().set_localScale(Vector3.get_zero());
				this.SpriteButtonO.get_transform().localPositionX(336f);
				this.SpriteButtonX.get_transform().localPositionX(421f);
			}
		}

		private void change_guide()
		{
			if (this.now_mode == (this.mDialogAnimation_Exchange.IsOpen || this.mDialogAnimation_UseLimit.IsOpen || this.mDialogAnimation_StoreBuy.IsOpen || this.mDialogAnimation_UseConfirm.IsOpen))
			{
				return;
			}
			this.now_mode = (this.mDialogAnimation_Exchange.IsOpen || this.mDialogAnimation_UseLimit.IsOpen || this.mDialogAnimation_StoreBuy.IsOpen || this.mDialogAnimation_UseConfirm.IsOpen);
			if (this.now_mode)
			{
				this.R_hide(true);
			}
			else
			{
				this.R_hide(false);
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
			Util.MoveTo(base.get_gameObject(), 0.4f, UIHowToItem.ShowPos, iTween.EaseType.easeInSine);
			this.isShow = true;
		}

		public void Hide()
		{
			Util.MoveTo(base.get_gameObject(), 0.4f, UIHowToItem.HidePos, iTween.EaseType.easeInSine);
			this.isShow = false;
		}

		private void OnDestroy()
		{
			this.key = null;
			this.key2 = null;
			this._itemm = null;
			this.SpriteButtonO = null;
			this.SpriteButtonX = null;
			this.SpriteStickR = null;
			this.mDialogAnimation_Exchange = null;
			this.mDialogAnimation_UseLimit = null;
			this.mDialogAnimation_StoreBuy = null;
			this.mDialogAnimation_UseConfirm = null;
			this._uiLabelX = null;
			this.model = null;
			this._uil = null;
			this._uis = null;
		}
	}
}
