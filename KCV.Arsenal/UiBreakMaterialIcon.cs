using System;
using UnityEngine;

namespace KCV.Arsenal
{
	public class UiBreakMaterialIcon : MonoBehaviour
	{
		[SerializeField]
		private Animation _anim;

		private string[] animeType;

		public void init()
		{
			base.get_transform().get_gameObject().SetActive(false);
			this.animeType = new string[5];
			this.animeType[0] = "ShipMaterial1";
			this.animeType[1] = "ShipMaterial2";
			this.animeType[2] = "ShipMaterial3";
			this.animeType[3] = "ShipMaterial4";
			this.animeType[4] = "ShipMaterial5";
			this._anim = base.GetComponent<Animation>();
			this._anim.Stop();
		}

		public void startAnimation()
		{
			base.get_transform().get_gameObject().SetActive(true);
			this.changeItems();
			this._anim.Stop();
			this._anim.Play(this.animeType[Random.Range(0, 5)]);
		}

		public void endAnimation()
		{
			this._anim.Stop();
			base.get_transform().get_gameObject().SetActive(false);
		}

		public void compMaterialAnimation()
		{
			this.changeItems();
			this._anim.Stop();
			this._anim.Play(this.animeType[Random.Range(0, 5)]);
		}

		public void changeItems()
		{
			this._anim.GetComponent<UISprite>().spriteName = "icon2_m" + Random.Range(1, 5);
		}

		private void OnDestroy()
		{
			this._anim = null;
			this.animeType = null;
		}
	}
}
