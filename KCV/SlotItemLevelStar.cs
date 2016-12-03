using KCV.Scene.Port;
using local.models;
using System;
using UnityEngine;

namespace KCV
{
	public class SlotItemLevelStar : MonoBehaviour
	{
		[SerializeField]
		private UIWidget LevelNotMaxContainer;

		[SerializeField]
		private UILabel labelLevel;

		[SerializeField]
		private UISprite levelMax;

		private int MAX_LEVEL = 10;

		public void Init(SlotitemModel m)
		{
			if (m == null || m.Level == 0)
			{
				this.LevelNotMaxContainer.SetActive(false);
				this.levelMax.SetActive(false);
			}
			else if (m.Level == this.MAX_LEVEL)
			{
				this.LevelNotMaxContainer.SetActive(false);
				this.levelMax.SetActive(true);
			}
			else
			{
				this.LevelNotMaxContainer.SetActive(true);
				this.levelMax.SetActive(false);
				this.labelLevel.text = m.Level.ToString();
			}
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref this.LevelNotMaxContainer);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.labelLevel);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.levelMax);
		}
	}
}
