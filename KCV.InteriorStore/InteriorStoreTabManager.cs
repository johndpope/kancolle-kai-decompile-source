using Common.Enum;
using System;
using UnityEngine;

namespace KCV.InteriorStore
{
	public class InteriorStoreTabManager : MonoBehaviour
	{
		public enum CategoryName
		{
			Wall,
			Floor,
			Chair,
			Window,
			Decoration,
			Interior
		}

		[SerializeField]
		private OnClickEventSender mOnClickEventSender_Determine;

		private UIButtonManager btnMng;

		private InteriorStoreTabManager.CategoryName nowCategory;

		private Action OnSelectAction;

		private Action OnDesideAction;

		public FurnitureKinds GetCurrentCategory()
		{
			switch (this.nowCategory)
			{
			case InteriorStoreTabManager.CategoryName.Wall:
				return FurnitureKinds.Wall;
			case InteriorStoreTabManager.CategoryName.Floor:
				return FurnitureKinds.Floor;
			case InteriorStoreTabManager.CategoryName.Chair:
				return FurnitureKinds.Desk;
			case InteriorStoreTabManager.CategoryName.Window:
				return FurnitureKinds.Window;
			case InteriorStoreTabManager.CategoryName.Decoration:
				return FurnitureKinds.Hangings;
			case InteriorStoreTabManager.CategoryName.Interior:
				return FurnitureKinds.Chest;
			default:
				throw new Exception("家具選択が不正");
			}
		}

		public void StartState()
		{
			this.mOnClickEventSender_Determine.SetClickable(true);
			this.InitTab();
			this.showUnselectTabs();
		}

		public void PopState()
		{
			this.mOnClickEventSender_Determine.SetClickable(false);
			this.hideUnselectTabs();
		}

		private void Awake()
		{
			this.btnMng = base.GetComponent<UIButtonManager>();
			this.btnMng.setFocus(0);
			this.nowCategory = InteriorStoreTabManager.CategoryName.Wall;
			this.btnMng.setButtonDelegate(Util.CreateEventDelegate(this, "ChangeTabOnTouch", null));
			this.mOnClickEventSender_Determine.SetClickable(false);
		}

		public void Init(Action OnSelect, Action OnDeside)
		{
			this.OnSelectAction = OnSelect;
			this.OnDesideAction = OnDeside;
		}

		public void ChangeTabOnTouch()
		{
			if (this.nowCategory != (InteriorStoreTabManager.CategoryName)this.btnMng.nowForcusIndex)
			{
				this.OnSelectAction.Invoke();
			}
			else
			{
				this.OnDesideAction.Invoke();
			}
		}

		public void changeNowCategory()
		{
			this.nowCategory = (InteriorStoreTabManager.CategoryName)this.btnMng.nowForcusIndex;
		}

		public void InitTab()
		{
			this.btnMng.setFocus(0);
		}

		public void NextTab()
		{
			bool flag = this.btnMng.moveNextButton();
			if (flag)
			{
				this.OnSelectAction.Invoke();
			}
		}

		public void PrevTab()
		{
			bool flag = this.btnMng.movePrevButton();
			if (flag)
			{
				this.OnSelectAction.Invoke();
			}
		}

		public void setAllButtonEnable(bool isEnable)
		{
			this.btnMng.setAllButtonEnable(isEnable);
		}

		public void hideUnselectTabs()
		{
			this.mOnClickEventSender_Determine.SetClickable(false);
			UIButton[] focusableButtons = this.btnMng.GetFocusableButtons();
			UIButton[] array = focusableButtons;
			for (int i = 0; i < array.Length; i++)
			{
				UIButton uIButton = array[i];
				if (uIButton != this.btnMng.nowForcusButton)
				{
					TweenAlpha.Begin(uIButton.get_gameObject(), 0.2f, 0.5f);
				}
			}
		}

		public void showUnselectTabs()
		{
			this.mOnClickEventSender_Determine.SetClickable(true);
			UIButton[] focusableButtons = this.btnMng.GetFocusableButtons();
			UIButton[] array = focusableButtons;
			for (int i = 0; i < array.Length; i++)
			{
				UIButton uIButton = array[i];
				if (uIButton != this.btnMng.nowForcusButton)
				{
					TweenAlpha.Begin(uIButton.get_gameObject(), 0.2f, 1f);
				}
			}
		}

		public FurnitureKinds getFurnitureKinds()
		{
			switch (this.nowCategory)
			{
			case InteriorStoreTabManager.CategoryName.Wall:
				return FurnitureKinds.Wall;
			case InteriorStoreTabManager.CategoryName.Floor:
				return FurnitureKinds.Floor;
			case InteriorStoreTabManager.CategoryName.Chair:
				return FurnitureKinds.Desk;
			case InteriorStoreTabManager.CategoryName.Window:
				return FurnitureKinds.Window;
			case InteriorStoreTabManager.CategoryName.Decoration:
				return FurnitureKinds.Hangings;
			case InteriorStoreTabManager.CategoryName.Interior:
				return FurnitureKinds.Chest;
			default:
				return FurnitureKinds.Wall;
			}
		}

		[Obsolete("Inspector上で設定して使用します。")]
		public void OnTouchDetermine()
		{
			this.ChangeTabOnTouch();
		}

		internal void ResumeState()
		{
			this.mOnClickEventSender_Determine.SetClickable(true);
			this.showUnselectTabs();
		}
	}
}
