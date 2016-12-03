using System;
using UnityEngine;

namespace KCV.Remodel
{
	public class PortUpgradesConvertShipReturnButton : MonoBehaviour
	{
		private PortUpgradesConvertShipManager manager;

		private GameObject cog;

		private bool active;

		private bool hit;

		private KeyControl keyController;

		public void Awake()
		{
			try
			{
				this.manager = base.get_transform().get_parent().get_parent().GetComponent<PortUpgradesConvertShipManager>();
			}
			catch (NullReferenceException)
			{
				Debug.Log("../.. not found in PortUpgradesConvertShipReturnButton.cs");
			}
			if (this.manager == null)
			{
				Debug.Log("PortUpgradesConvertShipManager.cs is not attached to ../..");
			}
			this.cog = base.get_transform().GetChild(1).get_gameObject();
			if (this.cog == null)
			{
				Debug.Log("/Cog not found in ReceiveShipNextButton.cs");
			}
			try
			{
				this.cog.GetComponent<UISprite>().alpha = 0f;
			}
			catch (NullReferenceException)
			{
				Debug.Log("UISprite.cs is not attached to /Cog");
			}
			try
			{
				base.get_transform().GetChild(0).GetComponent<UISprite>().alpha = 0f;
			}
			catch (NullReferenceException)
			{
				Debug.Log("UISprite.cs is not attached to /Inner");
			}
			this.active = false;
			this.keyController = new KeyControl(0, 0, 0.4f, 0.1f);
		}

		public void Update()
		{
			if (this.active && !this.hit)
			{
				this.cog.get_transform().Rotate(50f * Time.get_deltaTime() * Vector3.get_forward());
			}
		}

		public void OnHover(bool isOver)
		{
			this.hit = isOver;
		}

		public void OnClick()
		{
			this.manager.Finish();
		}

		public void Activate()
		{
			NGUITools.AddWidgetCollider(base.get_gameObject());
			try
			{
				this.cog.GetComponent<UISprite>().alpha = 1f;
			}
			catch (NullReferenceException)
			{
				Debug.Log("UISprite.cs is not attached to /Cog");
			}
			try
			{
				base.get_transform().GetChild(0).GetComponent<UISprite>().alpha = 1f;
			}
			catch (NullReferenceException)
			{
				Debug.Log("UISprite.cs is not attached to /Inner");
			}
			this.active = true;
		}

		private void OnDestroy()
		{
			this.manager = null;
			this.cog = null;
			this.keyController = null;
		}
	}
}
