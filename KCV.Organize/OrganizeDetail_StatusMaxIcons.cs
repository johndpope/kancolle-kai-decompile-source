using local.models;
using System;
using UnityEngine;

namespace KCV.Organize
{
	public class OrganizeDetail_StatusMaxIcons : MonoBehaviour
	{
		[SerializeField]
		private Transform[] Masks;

		[SerializeField]
		private UISprite[] Icons;

		[SerializeField]
		private UISprite[] StatusLabels;

		[SerializeField]
		private UISprite[] MaxLabels;

		public void SetMaxIcons(ShipModel ship)
		{
			bool[] array = new bool[]
			{
				ship.IsMaxKaryoku(),
				ship.IsMaxSoukou(),
				ship.IsMaxRaisou(),
				ship.IsMaxTaiku()
			};
			for (int i = 0; i < 4; i++)
			{
				this.Masks[i].SetActive(!array[i]);
				this.Icons[i].spriteName = this.getSpriteName(this.Icons[i].spriteName, array[i]);
				this.StatusLabels[i].spriteName = this.getSpriteName(this.StatusLabels[i].spriteName, array[i]);
				this.MaxLabels[i].spriteName = this.getSpriteName(this.MaxLabels[i].spriteName, array[i]);
			}
		}

		private string getSpriteName(string name, bool isMax)
		{
			if (isMax)
			{
				name = name.Replace("_g_", "_b_");
			}
			else
			{
				name = name.Replace("_b_", "_g_");
			}
			return name;
		}
	}
}
