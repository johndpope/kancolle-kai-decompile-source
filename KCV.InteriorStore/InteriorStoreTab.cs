using System;
using UnityEngine;

namespace KCV.InteriorStore
{
	public class InteriorStoreTab : MonoBehaviour
	{
		public UIButton button
		{
			get;
			private set;
		}

		private void Awake()
		{
			this.button = base.GetComponent<UIButton>();
		}
	}
}
