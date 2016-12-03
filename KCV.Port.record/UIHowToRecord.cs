using local.models;
using System;
using UnityEngine;

namespace KCV.Port.record
{
	public class UIHowToRecord : MonoBehaviour
	{
		private KeyControl key;

		private KeyControl key2;

		private float time;

		private bool isShow;

		private static readonly Vector3 ShowPos = new Vector3(1521f, -263f, 0f);

		private static readonly Vector3 HidePos = new Vector3(1521f, -289f, 0f);

		private SettingModel model;

		private void Awake()
		{
			base.get_transform().localPositionY(UIHowToRecord.HidePos.y);
			this.model = new SettingModel();
			this.key2 = new KeyControl(0, 1, 0.4f, 0.1f);
			this.key2.setChangeValue(0f, 0f, 0f, 0f);
		}

		private void OnDestroy()
		{
			this.key = null;
			this.key2 = null;
			this.model = null;
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
			if (!this.model.GuideDisplay)
			{
				return;
			}
			Util.MoveTo(base.get_gameObject(), 0.4f, UIHowToRecord.ShowPos, iTween.EaseType.easeInSine);
			this.isShow = true;
		}

		public void Hide()
		{
			Util.MoveTo(base.get_gameObject(), 0.4f, UIHowToRecord.HidePos, iTween.EaseType.easeInSine);
			this.isShow = false;
		}
	}
}
