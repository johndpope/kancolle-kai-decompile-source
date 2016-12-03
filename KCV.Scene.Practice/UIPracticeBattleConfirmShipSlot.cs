using local.models;
using System;
using UnityEngine;

namespace KCV.Scene.Practice
{
	[RequireComponent(typeof(UIWidget))]
	public class UIPracticeBattleConfirmShipSlot : MonoBehaviour
	{
		[SerializeField]
		private UILabel mLabel_DeckInNumber;

		[SerializeField]
		private UILabel mLabel_ShipName;

		[SerializeField]
		private UISprite[] mSprites_RemodelLevel;

		[SerializeField]
		private UILabel mLabel_ShipLevel;

		[SerializeField]
		private CommonShipBanner mCommonShipBanner_Ship;

		private ShipModel mShipModel;

		private int mDeckInNumber;

		private UIWidget mWidgetThis;

		private void Awake()
		{
			this.mWidgetThis = base.GetComponent<UIWidget>();
			this.mWidgetThis.alpha = 0f;
		}

		public void Initialize(int deckInNumber, ShipModel shipModel)
		{
			this.mDeckInNumber = deckInNumber;
			this.mShipModel = shipModel;
			this.mLabel_DeckInNumber.text = deckInNumber.ToString();
			this.mCommonShipBanner_Ship.SetShipData(shipModel);
			this.mLabel_ShipName.text = shipModel.Name;
			this.mLabel_ShipLevel.text = this.mShipModel.Level.ToString();
			for (int i = 0; i < this.mSprites_RemodelLevel.Length; i++)
			{
				bool flag = i <= this.mShipModel.Srate;
				if (flag)
				{
					this.mSprites_RemodelLevel[i].spriteName = "star_on";
				}
				else
				{
					this.mSprites_RemodelLevel[i].spriteName = "star";
				}
			}
			this.mWidgetThis.alpha = 1f;
		}

		public void InitializeDefault()
		{
			this.mWidgetThis.alpha = 0f;
		}

		private void OnDestroy()
		{
			this.mLabel_DeckInNumber = null;
			this.mLabel_ShipName = null;
			this.mSprites_RemodelLevel = null;
			this.mLabel_ShipLevel = null;
			this.mCommonShipBanner_Ship = null;
			this.mShipModel = null;
			this.mWidgetThis = null;
		}
	}
}
