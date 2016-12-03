using KCV.Utils;
using System;
using UnityEngine;

namespace KCV.Remodel
{
	public class PortUpgradesModernizeShipReturnButton : MonoBehaviour
	{
		private PortUpgradesModernizeShipManager manager;

		private GameObject cog;

		private bool active;

		private bool hit;

		private bool done;

		private KeyControl keyController;

		public void Awake()
		{
			try
			{
				this.manager = base.get_transform().get_parent().GetComponent<PortUpgradesModernizeShipManager>();
			}
			catch (NullReferenceException)
			{
				Debug.Log(".. not found in PortUpgradesModernizeShipReturnButton.cs");
			}
			if (this.manager == null)
			{
				Debug.Log("PortUpgradesModernizeShipManager.cs is not attached to ..");
			}
			this.cog = base.get_transform().GetChild(0).get_gameObject();
			if (this.cog == null)
			{
				Debug.Log("/Cog not found in PortUpgradesModernizeShipReturnButton.cs");
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
				base.get_transform().GetChild(1).GetComponent<UISprite>().alpha = 0f;
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
			this.keyController.Update();
			if (this.active && !this.hit)
			{
				this.cog.get_transform().Rotate(50f * Time.get_deltaTime() * Vector3.get_forward());
			}
			if (this.active && !this.done && this.keyController.IsMaruDown())
			{
				this.done = true;
				SingletonMonoBehaviour<FadeCamera>.Instance.FadeOut(0.2f, delegate
				{
					this.manager.isFinished = true;
				});
			}
		}

		public void OnHover(bool isOver)
		{
			this.hit = isOver;
		}

		public void OnClick()
		{
			SingletonMonoBehaviour<FadeCamera>.Instance.FadeOut(0.2f, delegate
			{
				this.manager.isFinished = true;
			});
			this.manager.Finish();
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
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
				base.get_transform().GetChild(1).GetComponent<UISprite>().alpha = 1f;
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
		}
	}
}
