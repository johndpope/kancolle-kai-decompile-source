using local.managers;
using System;
using UnityEngine;

namespace KCV.InteriorStore
{
	public class InteriorStoreFrame : MonoBehaviour
	{
		[SerializeField]
		private UILabel FCoin;

		[SerializeField]
		private UILabel Worker;

		[SerializeField]
		private UIButton GoHomeButton;

		public void updateUserInfo(FurnitureStoreManager manager)
		{
			this.FCoin.textInt = manager.UserInfo.FCoin;
			this.Worker.textInt = manager.GetWorkerCount();
		}

		public void setGoHomeButtonEnable(bool isEnable)
		{
			this.GoHomeButton.isEnabled = isEnable;
		}
	}
}
