using local.models;
using System;
using UnityEngine;

namespace KCV.Remodel
{
	public class UIHowToAlbum : MonoBehaviour
	{
		public enum GuideState
		{
			Gate,
			List,
			Detail,
			Quiet
		}

		private enum KeyType
		{
			MARU,
			BATU,
			L,
			Arrow,
			RS
		}

		[SerializeField]
		private GameObject SpriteStickR;

		[SerializeField]
		private GameObject SpriteButtonX;

		[SerializeField]
		private GameObject SpriteButtonL;

		[SerializeField]
		private GameObject SpriteButtonO;

		private Vector3 ShowPos = Vector3.get_right() * -450f + Vector3.get_up() * -259f;

		private Vector3 HidePos = Vector3.get_right() * -450f + Vector3.get_up() * -289f;

		private ScreenStatus _now_mode;

		private KeyControl key;

		private float time;

		private bool isShow;

		private SettingModel model;

		private UILabel _uil;

		private UIHowToAlbum.GuideState mCurrentGuideState;

		private void Awake()
		{
			this.key = new KeyControl(0, 0, 0.4f, 0.1f);
			base.get_transform().localPositionY(this.HidePos.y);
			this.model = new SettingModel();
			this._now_mode = ScreenStatus.MODE_SOUBI_HENKOU;
		}

		private void HideKey(UIHowToAlbum.KeyType keyType)
		{
			switch (keyType)
			{
			case UIHowToAlbum.KeyType.RS:
				this.SpriteStickR.get_transform().set_localScale(Vector3.get_zero());
				this.SpriteButtonL.get_transform().localPositionX(71f);
				this.SpriteButtonO.get_transform().localPositionX(330f);
				this.SpriteButtonX.get_transform().localPositionX(415f);
				break;
			}
		}

		private void ShowKey(UIHowToAlbum.KeyType keyType)
		{
			switch (keyType)
			{
			case UIHowToAlbum.KeyType.RS:
				this.SpriteStickR.get_transform().set_localScale(Vector3.get_one());
				this.SpriteButtonL.get_transform().localPositionX(164f);
				this.SpriteButtonO.get_transform().localPositionX(425f);
				this.SpriteButtonX.get_transform().localPositionX(512f);
				break;
			}
		}

		public void ChangeGuideStatus(UIHowToAlbum.GuideState state)
		{
			switch (this.mCurrentGuideState)
			{
			case UIHowToAlbum.GuideState.Gate:
				this.HideKey(UIHowToAlbum.KeyType.Arrow);
				this.HideKey(UIHowToAlbum.KeyType.MARU);
				this.HideKey(UIHowToAlbum.KeyType.BATU);
				this.HideKey(UIHowToAlbum.KeyType.L);
				break;
			case UIHowToAlbum.GuideState.List:
				this.HideKey(UIHowToAlbum.KeyType.Arrow);
				this.HideKey(UIHowToAlbum.KeyType.MARU);
				this.HideKey(UIHowToAlbum.KeyType.BATU);
				this.HideKey(UIHowToAlbum.KeyType.L);
				this.HideKey(UIHowToAlbum.KeyType.RS);
				break;
			case UIHowToAlbum.GuideState.Detail:
				this.HideKey(UIHowToAlbum.KeyType.Arrow);
				this.HideKey(UIHowToAlbum.KeyType.MARU);
				this.HideKey(UIHowToAlbum.KeyType.BATU);
				this.HideKey(UIHowToAlbum.KeyType.L);
				break;
			}
			this.mCurrentGuideState = state;
			switch (this.mCurrentGuideState)
			{
			case UIHowToAlbum.GuideState.Gate:
				this.ShowKey(UIHowToAlbum.KeyType.Arrow);
				this.ShowKey(UIHowToAlbum.KeyType.MARU);
				this.ShowKey(UIHowToAlbum.KeyType.BATU);
				this.ShowKey(UIHowToAlbum.KeyType.L);
				this.HideKey(UIHowToAlbum.KeyType.RS);
				break;
			case UIHowToAlbum.GuideState.List:
				this.ShowKey(UIHowToAlbum.KeyType.Arrow);
				this.ShowKey(UIHowToAlbum.KeyType.MARU);
				this.ShowKey(UIHowToAlbum.KeyType.BATU);
				this.ShowKey(UIHowToAlbum.KeyType.L);
				this.ShowKey(UIHowToAlbum.KeyType.RS);
				break;
			case UIHowToAlbum.GuideState.Detail:
				this.ShowKey(UIHowToAlbum.KeyType.Arrow);
				this.ShowKey(UIHowToAlbum.KeyType.MARU);
				this.ShowKey(UIHowToAlbum.KeyType.BATU);
				this.ShowKey(UIHowToAlbum.KeyType.L);
				break;
			case UIHowToAlbum.GuideState.Quiet:
				this.Hide();
				break;
			}
		}

		private void Update()
		{
			this.key.Update();
			if (this.key == null)
			{
				if (this.isShow)
				{
					this.Hide();
				}
			}
			else if (this.key.IsRun)
			{
				this.time += Time.get_deltaTime();
				if (this.key.IsAnyKey)
				{
					this.time = 0f;
					if (this.isShow)
					{
						this.isShow = false;
						this.Hide();
					}
				}
				else if (2f < this.time && !this.isShow && this.mCurrentGuideState != UIHowToAlbum.GuideState.Quiet)
				{
					this.isShow = true;
					this.Show();
				}
			}
		}

		public void Show()
		{
			if (!this.model.GuideDisplay)
			{
				return;
			}
			Util.MoveTo(base.get_gameObject(), 0.4f, this.ShowPos, iTween.EaseType.easeInSine);
		}

		public void Hide()
		{
			Util.MoveTo(base.get_gameObject(), 0.4f, this.HidePos, iTween.EaseType.easeInSine);
		}

		private void OnDestroy()
		{
			this.SpriteStickR = null;
			this.SpriteButtonX = null;
			this.SpriteButtonL = null;
			this.SpriteButtonO = null;
			this.key = null;
			this.model = null;
			this._uil = null;
		}
	}
}
