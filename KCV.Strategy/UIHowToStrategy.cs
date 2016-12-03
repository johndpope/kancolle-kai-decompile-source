using local.models;
using System;
using UnityEngine;

namespace KCV.Strategy
{
	public class UIHowToStrategy : MonoBehaviour
	{
		private KeyControl key;

		private KeyControl StickKey;

		private float time;

		private bool isShow;

		private bool isForce;

		[SerializeField]
		private Transform Items1;

		[SerializeField]
		private Transform Items2;

		[SerializeField]
		private GameObject FooterMenu;

		private static readonly Vector3 ShowPos = new Vector3(-482f, -258f, 0f);

		private static readonly Vector3 HidePos = new Vector3(-482f, -291f, 0f);

		private static readonly Vector3 FooterShowPos = new Vector3(-485f, -214f, 0f);

		private static readonly Vector3 FooterHidePos = new Vector3(-485f, -242f, 0f);

		private static readonly Vector3 ShowPos2 = new Vector3(-3487f, -40f, 0f);

		private static readonly Vector3 HidePos2 = new Vector3(-3487f, -70f, 0f);

		private Vector3 NextPos;

		private SettingModel model;

		private Coroutine cor;

		private void Awake()
		{
			base.get_transform().localPositionY(-291f);
			this.model = new SettingModel();
		}

		private void Update()
		{
			if (this.key != null && this.key.IsRun)
			{
				this.time += Time.get_deltaTime();
				if (this.key.IsAnyKey || this.StickKey.IsUpdateIndex)
				{
					this.time = 0f;
					if (this.isShow)
					{
						this.Hide();
					}
				}
				else if (5f < this.time && !this.isShow)
				{
					this.Show();
				}
			}
		}

		public void SetKeyController(KeyControl key, KeyControl stickkey)
		{
			this.key = key;
			this.StickKey = stickkey;
			if (key == null && this.isShow)
			{
				this.Hide();
			}
		}

		public void isForceShow()
		{
			this.isForce = true;
			base.get_transform().set_localPosition(UIHowToStrategy.HidePos2);
			this.NextPos = UIHowToStrategy.HidePos2;
			this.Show();
		}

		public void isForceHide()
		{
			this.isForce = false;
			this.Hide();
		}

		public void Show()
		{
			if (!this.model.GuideDisplay)
			{
				return;
			}
			Vector3 vector = (!this.isForce) ? UIHowToStrategy.ShowPos : UIHowToStrategy.ShowPos2;
			this.Reset(vector);
			Util.MoveTo(base.get_gameObject(), 0.4f, vector, iTween.EaseType.easeInSine);
			Util.MoveTo(this.FooterMenu, 0.4f, UIHowToStrategy.FooterShowPos, iTween.EaseType.easeInSine);
			this.isShow = true;
			if (this.cor != null)
			{
				base.StopCoroutine(this.cor);
			}
		}

		public void Hide()
		{
			if (this.isForce || !this.isShow)
			{
				return;
			}
			Vector3 vector = (!(this.NextPos == UIHowToStrategy.ShowPos2)) ? UIHowToStrategy.HidePos : UIHowToStrategy.HidePos2;
			this.Reset(vector);
			Util.MoveTo(base.get_gameObject(), 0.4f, vector, iTween.EaseType.easeInSine);
			Util.MoveTo(this.FooterMenu, 0.4f, UIHowToStrategy.FooterHidePos, iTween.EaseType.easeInSine);
			this.isShow = false;
			if (this.cor != null)
			{
				base.StopCoroutine(this.cor);
			}
		}

		private void Reset(Vector3 NewNextPos)
		{
			iTween.Stop(base.get_gameObject());
			if (NewNextPos == UIHowToStrategy.ShowPos && this.NextPos != UIHowToStrategy.HidePos)
			{
				base.get_transform().set_localPosition(UIHowToStrategy.HidePos);
			}
			else if (NewNextPos == UIHowToStrategy.ShowPos2 && this.NextPos != UIHowToStrategy.HidePos2)
			{
				base.get_transform().set_localPosition(UIHowToStrategy.HidePos2);
			}
			this.NextPos = NewNextPos;
			if (this.NextPos == UIHowToStrategy.ShowPos || this.NextPos == UIHowToStrategy.ShowPos2)
			{
				this.Items1.SetActive(!this.isForce);
				this.Items2.SetActive(this.isForce);
			}
		}

		private void OnDestroy()
		{
			this.key = null;
			this.StickKey = null;
			this.Items1 = null;
			this.Items2 = null;
			this.FooterMenu = null;
			this.model = null;
			this.cor = null;
		}
	}
}
