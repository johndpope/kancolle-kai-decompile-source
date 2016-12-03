using KCV.Scene.Port;
using System;
using UnityEngine;

namespace KCV.Remodel
{
	public class PortUpgradesModernizeShipLeaf : MonoBehaviour
	{
		private const float A_VEL = 1f;

		private const float DIST = 40f;

		private PortUpgradesModernizeShipManager manager;

		private GameObject dashInit;

		private GameObject[] dashes;

		private UISprite[] sprites;

		private bool on;

		private int pos;

		private float timer;

		private Vector3 prevPos;

		public void Awake()
		{
			this.manager = base.get_transform().get_parent().get_parent().GetComponent<PortUpgradesModernizeShipManager>();
			this.dashInit = base.get_transform().get_parent().Find("DashInit").get_gameObject();
			this.dashes = new GameObject[20];
			this.sprites = new UISprite[20];
			this.on = false;
			this.pos = 19;
			this.timer = 0f;
			base.GetComponent<Animation>().Stop();
		}

		public void Update()
		{
			if (this.on)
			{
				if (this.dashes[this.pos] == null || Vector3.Distance(this.dashes[this.pos].get_transform().get_localPosition(), base.get_transform().get_localPosition()) > 40f)
				{
					this.pos = ++this.pos % 20;
					this.dashes[this.pos] = Object.Instantiate<GameObject>(this.dashInit);
					this.sprites[this.pos] = this.dashes[this.pos].GetComponent<UISprite>();
					this.dashes[this.pos].get_transform().set_parent(base.get_transform().get_parent());
					this.dashes[this.pos].get_transform().set_localScale(new Vector3(1f, 1f, 1f));
					this.dashes[this.pos].get_transform().set_localPosition(base.get_transform().get_localPosition());
					Vector3 vector = base.get_transform().get_localPosition() - this.prevPos;
					this.dashes[this.pos].get_transform().Rotate(Vector3.get_forward(), 57.2957764f * Mathf.Atan2(vector.y, vector.x));
					this.sprites[this.pos].alpha = 1f;
				}
				for (int i = 0; i < 20; i++)
				{
					if (this.dashes[i] != null)
					{
						this.sprites[i].alpha -= Mathf.Min(1f * Time.get_deltaTime(), this.sprites[i].alpha);
						if (this.sprites[i].alpha < 1f * Time.get_deltaTime())
						{
							Object.Destroy(this.dashes[i]);
						}
					}
				}
				this.prevPos = base.get_transform().get_localPosition();
				bool flag = true;
				for (int j = 0; j < 20; j++)
				{
					if (this.dashes[j] != null)
					{
						flag = false;
						break;
					}
				}
				if (flag && base.get_transform().get_localPosition().x > 500f)
				{
					Object.Destroy(base.get_gameObject());
				}
			}
		}

		public void Initialize()
		{
			this.on = true;
			base.GetComponent<Animation>().Play("fail_leaf");
		}

		private void OnDestroy()
		{
			this.manager = null;
			this.dashInit = null;
			if (this.dashes != null)
			{
				for (int i = 0; i < this.dashes.Length; i++)
				{
					this.dashes[i] = null;
				}
			}
			this.dashes = null;
			if (this.sprites != null)
			{
				for (int j = 0; j < this.sprites.Length; j++)
				{
					UserInterfacePortManager.ReleaseUtils.Release(ref this.sprites[j]);
				}
			}
			this.sprites = null;
		}
	}
}
