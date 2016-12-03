using Common.Enum;
using KCV.View.Scroll;
using local.managers;
using local.models;
using System;
using System.Linq;

namespace KCV.InteriorStore
{
	public class InteriorStoreList : UIScrollListParent<FurnitureModel, InteriorStoreListChild>
	{
		private FurnitureKinds nowCategory;

		private FurnitureStoreManager manager;

		private Action mOnRequestChangeMode;

		protected override void OnAwake()
		{
			base.EnableBottomUpMode();
		}

		public void ChangeCategory(FurnitureKinds kinds)
		{
			if (this.nowCategory == kinds)
			{
				return;
			}
			this.nowCategory = kinds;
			FurnitureModel[] storeItem = this.manager.GetStoreItem(kinds);
			if (this.Views == null)
			{
				this.Initialize(storeItem);
			}
			else
			{
				base.RefreshAndFirstFocus(storeItem);
			}
		}

		public void Refresh()
		{
			FurnitureModel[] storeItem = this.manager.GetStoreItem(this.nowCategory);
			base.Refresh(storeItem);
		}

		public void setManager(FurnitureStoreManager manager)
		{
			this.manager = manager;
		}

		protected override void OnChildAction(ActionType actionType, UIScrollListChild<FurnitureModel> actionChild)
		{
			if (this.mKeyController == null)
			{
				this.OnRequestChangeMode();
			}
			if (actionType == ActionType.OnTouch)
			{
				if (actionChild.Model != null && actionChild.mIsClickable)
				{
					base.ChangeFocusView((InteriorStoreListChild)actionChild, false);
					this.OnAction(actionType, this, (InteriorStoreListChild)actionChild);
				}
			}
		}

		public void SetOnRequestChangeMode(Action onRequestChangeMode)
		{
			this.mOnRequestChangeMode = onRequestChangeMode;
		}

		private void OnRequestChangeMode()
		{
			if (this.mOnRequestChangeMode != null)
			{
				this.mOnRequestChangeMode.Invoke();
			}
		}

		private FurnitureModel[] FurnitureFilter(FurnitureModel[] models, FurnitureKinds kinds)
		{
			int[] checkTarget8 = new int[]
			{
				164,
				165,
				166
			};
			int[] checkTarget2 = new int[]
			{
				1,
				2,
				4,
				5
			};
			int[] checkTarget3 = new int[]
			{
				72,
				73,
				74,
				77
			};
			int[] checkTarget4 = new int[]
			{
				102,
				103,
				104,
				105
			};
			int[] checkTarget5 = new int[]
			{
				142,
				143,
				141,
				144
			};
			int[] checkTarget6 = new int[]
			{
				38,
				39,
				40,
				41
			};
			int[] checkTarget = new int[0];
			switch (kinds)
			{
			case FurnitureKinds.Floor:
				checkTarget = checkTarget2;
				break;
			case FurnitureKinds.Wall:
				checkTarget = checkTarget6;
				break;
			case FurnitureKinds.Window:
				checkTarget = checkTarget3;
				break;
			case FurnitureKinds.Hangings:
				checkTarget = checkTarget4;
				break;
			case FurnitureKinds.Chest:
				checkTarget = checkTarget5;
				break;
			case FurnitureKinds.Desk:
				checkTarget = checkTarget8;
				break;
			}
			return Enumerable.ToArray<FurnitureModel>(Enumerable.Where<FurnitureModel>(models, delegate(FurnitureModel model)
			{
				int[] checkTarget7 = checkTarget;
				for (int i = 0; i < checkTarget7.Length; i++)
				{
					int num = checkTarget7[i];
					if (num == model.MstId)
					{
						return true;
					}
				}
				return false;
			}));
		}
	}
}
