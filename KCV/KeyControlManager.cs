using System;
using UnityEngine;

namespace KCV
{
	public class KeyControlManager : MonoBehaviour
	{
		protected static KeyControlManager instance;

		private KeyControl keyController;

		public KeyControl CommonKeyController;

		public static KeyControlManager Instance
		{
			get
			{
				if (KeyControlManager.instance == null)
				{
					KeyControlManager.instance = (KeyControlManager)Object.FindObjectOfType(typeof(KeyControlManager));
					if (KeyControlManager.instance == null)
					{
						return null;
					}
				}
				return KeyControlManager.instance;
			}
			set
			{
				KeyControlManager.instance = value;
			}
		}

		public KeyControl KeyController
		{
			get
			{
				return this.keyController;
			}
			set
			{
				if (this.keyController != null)
				{
					this.keyController.ClearKeyAll();
				}
				this.keyController = value;
				this.keyController.firstUpdate = true;
			}
		}

		private void Awake()
		{
			DebugUtils.SLog("KeyControlManager" + Time.get_realtimeSinceStartup());
			if (KeyControlManager.instance != null)
			{
				return;
			}
			KeyControlManager.instance = this;
		}

		public static bool exist()
		{
			return KeyControlManager.instance != null;
		}
	}
}
