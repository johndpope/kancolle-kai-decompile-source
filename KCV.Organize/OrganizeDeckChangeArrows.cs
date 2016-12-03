using System;
using UnityEngine;

namespace KCV.Organize
{
	public class OrganizeDeckChangeArrows : MonoBehaviour
	{
		[SerializeField]
		private UITexture NextArrow;

		[SerializeField]
		private UITexture PrevArrow;

		[SerializeField]
		private CommonDeckSwitchManager deckSwitcher;

		private void OnDestroy()
		{
			Mem.Del<UITexture>(ref this.NextArrow);
			Mem.Del<UITexture>(ref this.PrevArrow);
			Mem.Del<CommonDeckSwitchManager>(ref this.deckSwitcher);
		}

		public void UpdateView()
		{
			this.NextArrow.SetActive(this.deckSwitcher.isChangeRight);
			this.PrevArrow.SetActive(this.deckSwitcher.isChangeLeft);
		}
	}
}
