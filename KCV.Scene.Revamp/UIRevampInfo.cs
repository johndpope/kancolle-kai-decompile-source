using local.managers;
using System;
using UnityEngine;

namespace KCV.Scene.Revamp
{
	public class UIRevampInfo : MonoBehaviour
	{
		private class RevampMaterial
		{
			private Transform _traMaterial;

			private UILabel _uiDevKit;

			private UILabel _uiRevKit;

			public RevampMaterial(Transform parent, string objName)
			{
				Util.FindParentToChild<Transform>(ref this._traMaterial, parent, objName);
				Util.FindParentToChild<UILabel>(ref this._uiDevKit, this._traMaterial, "DevKit");
				Util.FindParentToChild<UILabel>(ref this._uiRevKit, this._traMaterial, "RevKit");
			}

			public void SetMaterial(RevampManager manager)
			{
				this._uiDevKit.textInt = manager.Material.Devkit;
				this._uiRevKit.textInt = manager.Material.Revkit;
			}
		}

		private UIRevampInfo.RevampMaterial _clsMaterial;

		private void Awake()
		{
			this._clsMaterial = new UIRevampInfo.RevampMaterial(base.get_transform(), "RevampMaterial");
		}

		private void Start()
		{
		}

		private void Update()
		{
		}

		public void SetMaterial(RevampManager manager)
		{
			this._clsMaterial.SetMaterial(manager);
		}
	}
}
