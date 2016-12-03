using local.models;
using System;
using UnityEngine;

namespace KCV.Scene.Revamp
{
	public class UIHowToRevamp : MonoBehaviour
	{
		private KeyControl keyController;

		private float time;

		private bool isShow;

		[SerializeField]
		private UserInterfaceRevampManager _revm;

		[SerializeField]
		private GameObject SpriteButtonX;

		[SerializeField]
		private GameObject SpriteButtonShikaku;

		private bool now_mode;

		private static readonly Vector3 ShowPos = new Vector3(-450f, -253f, 0f);

		private static readonly Vector3 HidePos = new Vector3(-450f, -289f, 0f);

		private SettingModel model;

		private UILabel _uil;

		private UISprite _uis;

		private void Awake()
		{
			base.get_transform().localPositionY(UIHowToRevamp.HidePos.y);
			this.model = new SettingModel();
			this.keyController = new KeyControl(0, 0, 0.4f, 0.1f);
			this.now_mode = true;
			this.SpriteButtonX.get_transform().set_localScale(Vector3.get_one());
			this.SpriteButtonShikaku.get_transform().localPositionX(469f);
		}

		private void Update()
		{
			if (this.keyController == null)
			{
				if (this.isShow)
				{
					this.Hide();
				}
			}
			else
			{
				this.keyController.Update();
				if (this.keyController.IsRun)
				{
					this.time += Time.get_deltaTime();
					if (this.keyController.IsAnyKey)
					{
						this.time = 0f;
						if (this.isShow)
						{
							this.Hide();
						}
					}
					else if (2f < this.time && !this.isShow && !this._revm._isAnimation)
					{
						this.Show();
					}
				}
			}
			if (this._revm._isSettingMode)
			{
				this.SpriteButtonShikaku.SetActive(true);
			}
			else
			{
				this.SpriteButtonShikaku.SetActive(false);
			}
			if (this._revm._isAnimation)
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
			Util.MoveTo(base.get_gameObject(), 0.4f, UIHowToRevamp.ShowPos, iTween.EaseType.easeInSine);
			this.isShow = true;
		}

		public void Hide()
		{
			Util.MoveTo(base.get_gameObject(), 0.4f, UIHowToRevamp.HidePos, iTween.EaseType.easeInSine);
			this.isShow = false;
		}

		private void OnDestroy()
		{
			this.keyController = null;
			this._revm = null;
			this.SpriteButtonX = null;
			this.SpriteButtonShikaku = null;
			this.model = null;
			this._uil = null;
			this._uis = null;
		}
	}
}
