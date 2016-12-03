using System;
using UnityEngine;

namespace KCV
{
	public class IndexButton : MonoBehaviour
	{
		public int myIndexNo;

		public KeyControl keyController;

		public bool isClicked;

		private void Awake()
		{
			base.get_gameObject().set_tag("IndexButton");
			this.isClicked = false;
			UIButton component = base.get_gameObject().get_transform().GetComponent<UIButton>();
			EventDelegate.Add(component.onClick, new EventDelegate.Callback(this.onClick));
		}

		private void Start()
		{
		}

		private void Update()
		{
		}

		public void changeIndex()
		{
			if (this.keyController != null)
			{
				KeyControlManager.Instance.KeyController = this.keyController;
			}
			KeyControlManager.Instance.KeyController.Index = this.myIndexNo;
		}

		public void addIndex()
		{
			if (KeyControlManager.exist())
			{
				Debug.Log("addindex");
				Debug.Log(KeyControlManager.Instance.KeyController.Index);
				KeyControlManager.Instance.KeyController.Index += this.myIndexNo;
				Debug.Log(KeyControlManager.Instance.KeyController.Index);
			}
		}

		public void onClick()
		{
			this.isClicked = true;
		}
	}
}
