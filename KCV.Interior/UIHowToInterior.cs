using local.models;
using System;
using UnityEngine;

namespace KCV.Interior
{
	public class UIHowToInterior : MonoBehaviour
	{
		private KeyControl key;

		private KeyControl key2;

		private float time;

		private bool isShow;

		[SerializeField]
		private GameObject SpriteButtonX;

		[SerializeField]
		private GameObject SpriteButtonO;

		[SerializeField]
		private GameObject SpriteStickR;

		private bool now_mode;

		private static readonly Vector3 ShowPos = new Vector3(-450f, -253f, 0f);

		private static readonly Vector3 HidePos = new Vector3(-450f, -289f, 0f);

		private SettingModel model;

		private UILabel _uil;

		private UISprite _uis;

		private GameObject _tes;

		private void Awake()
		{
			base.get_transform().localPositionY(UIHowToInterior.HidePos.y);
			this.model = new SettingModel();
			this.key2 = new KeyControl(0, 1, 0.4f, 0.1f);
			this.key2.setChangeValue(0f, 0f, 0f, 0f);
			this.now_mode = true;
			if (this.SpriteButtonO == null)
			{
				Util.FindParentToChild(ref this.SpriteButtonO, this.SpriteButtonX.get_transform().get_parent(), "SpriteButtonO");
			}
			if (this.SpriteStickR == null)
			{
				Util.FindParentToChild(ref this.SpriteStickR, this.SpriteButtonX.get_transform().get_parent(), "SpriteStickR");
			}
		}

		private bool isRuse()
		{
			if (GameObject.Find("CategoryTabs") != null)
			{
				return !(GameObject.Find("CategoryTabs/Tab1").GetComponent<UISprite>().color != Color.get_white()) && !(GameObject.Find("CategoryTabs/Tab2").GetComponent<UISprite>().color != Color.get_white());
			}
			return !(GameObject.Find("UIInteriorFurnitureDetail") != null) || GameObject.Find("UIInteriorFurnitureDetail").GetComponent<Transform>().get_localPosition().x >= 10f;
		}

		private bool isPreview()
		{
			return GameObject.Find("UIInteriorFurnitureDetail") != null && GameObject.Find("UIInteriorFurnitureDetail").GetComponent<Transform>().get_localScale().x != 1f;
		}

		private void change_guide()
		{
			if (this.isRuse())
			{
				this.SpriteStickR.get_transform().localPositionX(334f);
				this.SpriteStickR.get_transform().set_localScale(Vector3.get_one());
				this.SpriteButtonO.get_transform().localPositionX(457f);
				this.SpriteButtonX.get_transform().localPositionX(542f);
			}
			else
			{
				this.SpriteStickR.get_transform().localPositionX(334f);
				this.SpriteStickR.get_transform().set_localScale(Vector3.get_zero());
				this.SpriteButtonO.get_transform().localPositionX(334f);
				this.SpriteButtonX.get_transform().localPositionX(418f);
			}
			if (this.isPreview())
			{
				this.Hide();
			}
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
					if (this.isShow || this.isPreview())
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
			if (this.isPreview())
			{
				return;
			}
			Util.MoveTo(base.get_gameObject(), 0.4f, UIHowToInterior.ShowPos, iTween.EaseType.easeInSine);
			this.isShow = true;
		}

		public void Hide()
		{
			Util.MoveTo(base.get_gameObject(), 0.4f, UIHowToInterior.HidePos, iTween.EaseType.easeInSine);
			this.isShow = false;
		}

		private void OnDestroy()
		{
			this.key = null;
			this.key2 = null;
			this.SpriteButtonX = null;
			this.SpriteButtonO = null;
			this.SpriteStickR = null;
			this.model = null;
			this._uil = null;
			this._uis = null;
		}
	}
}
