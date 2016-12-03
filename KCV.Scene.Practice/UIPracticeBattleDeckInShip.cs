using local.models;
using System;
using UnityEngine;

namespace KCV.Scene.Practice
{
	[RequireComponent(typeof(UIWidget))]
	public class UIPracticeBattleDeckInShip : MonoBehaviour
	{
		private UIWidget mWidgetThis;

		private ShipModel mShipModel;

		[SerializeField]
		private UILabel mLabel_Type;

		[SerializeField]
		private UILabel mLabel_Name;

		[SerializeField]
		private UILabel mLabel_Level;

		private void Awake()
		{
			this.mWidgetThis = base.GetComponent<UIWidget>();
			this.mWidgetThis.alpha = 0f;
		}

		public void Initialize(ShipModel shipModel)
		{
			this.mShipModel = shipModel;
			this.mLabel_Type.text = shipModel.ShipTypeName;
			this.mLabel_Name.text = shipModel.Name;
			this.mLabel_Level.text = string.Format("Lv{0}", shipModel.Level);
			this.mWidgetThis.alpha = 1f;
		}

		private void OnDestroy()
		{
			this.mWidgetThis = null;
			this.mShipModel = null;
			this.mLabel_Type = null;
			this.mLabel_Name = null;
			this.mLabel_Level = null;
		}
	}
}
