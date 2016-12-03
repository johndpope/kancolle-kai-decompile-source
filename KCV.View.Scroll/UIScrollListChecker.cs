using local.managers;
using local.models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace KCV.View.Scroll
{
	public class UIScrollListChecker : MonoBehaviour
	{
		public UIScrollShipListParent mUIScrollShipListParent;

		[SerializeField]
		private UILabel mLabel_Debug;

		[SerializeField]
		private UIScrollListShipInfo mUIScrollListShipInfo;

		private KeyControl mKeyController;

		private bool mGCSwitch;

		private float UPDATE_GC_COLLECT_MILLI_SEC = 2f;

		private void Start()
		{
			base.StartCoroutine(this.ManualGCCollection());
			ShipModel[] shipList = new OrganizeManager(1).GetShipList();
			List<ShipModel> list = new List<ShipModel>();
			list.AddRange(shipList);
			this.mUIScrollShipListParent.Initialize(list.ToArray());
			this.UpdateDebugLabel();
			this.mUIScrollShipListParent.SetOnUIScrollListParentAction(delegate(ActionType actionType, UIScrollListParent<ShipModel, UIScrollShipListChild> calledOjbect, UIScrollListChild<ShipModel> child)
			{
				switch (actionType)
				{
				case ActionType.OnButtonSelect:
					Debug.Log("Called ListItemSelect" + child.Model.ToString());
					break;
				case ActionType.OnTouch:
					Debug.Log("Called ListItemTouch Name:" + child.Model.ToString());
					break;
				case ActionType.OnChangeFocus:
				case ActionType.OnChangeFirstFocus:
					Debug.Log("Called OnChangeFocus" + child.Model.ToString());
					this.mUIScrollListShipInfo.Initialize(child.Model);
					break;
				}
			});
			this.mKeyController = new KeyControl(0, 0, 0.4f, 0.1f);
			this.mUIScrollShipListParent.SetKeyController(this.mKeyController);
		}

		private void Update()
		{
			if (this.mKeyController != null)
			{
				this.mKeyController.Update();
				if (this.mKeyController.keyState.get_Item(8).down)
				{
					this.mGCSwitch = true;
					this.UpdateDebugLabel();
				}
				else if (this.mKeyController.keyState.get_Item(12).down)
				{
					this.mGCSwitch = false;
					this.UpdateDebugLabel();
				}
				else if (this.mKeyController.keyState.get_Item(3).down)
				{
					this.UPDATE_GC_COLLECT_MILLI_SEC -= 0.1f;
					this.UpdateDebugLabel();
				}
				else if (this.mKeyController.keyState.get_Item(0).down)
				{
					this.UPDATE_GC_COLLECT_MILLI_SEC += 0.1f;
					this.UpdateDebugLabel();
				}
				else if (this.mKeyController.keyState.get_Item(6).down)
				{
					Application.LoadLevel("Strategy");
				}
			}
		}

		[DebuggerHidden]
		private IEnumerator ManualGCCollection()
		{
			UIScrollListChecker.<ManualGCCollection>c__Iterator50 <ManualGCCollection>c__Iterator = new UIScrollListChecker.<ManualGCCollection>c__Iterator50();
			<ManualGCCollection>c__Iterator.<>f__this = this;
			return <ManualGCCollection>c__Iterator;
		}

		private void UpdateDebugLabel()
		{
			this.mLabel_Debug.text = string.Concat(new object[]
			{
				"GC:",
				(!this.mGCSwitch) ? "Off" : "On",
				" GCSpeed::",
				this.UPDATE_GC_COLLECT_MILLI_SEC
			});
		}
	}
}
