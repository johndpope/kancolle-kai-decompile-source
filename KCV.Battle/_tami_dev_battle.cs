using System;
using UnityEngine;

namespace KCV.Battle
{
	public class _tami_dev_battle : MonoBehaviour
	{
		private GameObject _go;

		private Animation _ani;

		private UITexture _tex;

		private bool _startUp;

		private void Start()
		{
			this._startUp = false;
			this.startUp();
		}

		private void startUp()
		{
			this._go = GameObject.Find("ProdTorpedoCutIn");
			this._ani = this._go.GetComponent<Animation>();
			this._tex = this._go.get_transform().FindChild("FriendShip/Panel/Deg30/Anchor/Object2D").GetComponent<UITexture>();
			this._tex.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(1, 9);
			this._tex.MakePixelPerfect();
			this._tex = this._go.get_transform().FindChild("EnemyShip/Panel/Deg30/Anchor/Object2D").GetComponent<UITexture>();
			this._tex.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(511, 9);
			this._tex.MakePixelPerfect();
			this._startUp = true;
		}

		private void Update()
		{
			if (!this._startUp)
			{
				return;
			}
			if (Input.GetKey(49))
			{
				this._ani.Play("ProdTorpedoCutIn");
			}
			if (Input.GetKey(50))
			{
				this._ani.Play("ProdTorpedoCutInFriend");
			}
			if (Input.GetKey(51))
			{
				this._ani.Play("ProdTorpedoCutInEnemy");
			}
		}
	}
}
