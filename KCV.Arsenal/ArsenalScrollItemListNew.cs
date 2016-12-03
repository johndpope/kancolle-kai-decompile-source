using KCV.Utils;
using KCV.View.ScrollView;
using local.managers;
using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Arsenal
{
	public class ArsenalScrollItemListNew : UIScrollList<ArsenalScrollSlotItemListChoiceModel, ArsenalScrollItemListChildNew>
	{
		[SerializeField]
		private Transform MessageSlot;

		private ArsenalManager mArsenalManager;

		private bool _isFirst = true;

		private int _before_focus;

		private Action mOnBackListener;

		private KeyControl mKeyController;

		private Action<ArsenalScrollItemListChildNew> mOnItemSelectedListener;

		protected override void OnUpdate()
		{
			if (base.mState == UIScrollList<ArsenalScrollSlotItemListChoiceModel, ArsenalScrollItemListChildNew>.ListState.Waiting && this.mKeyController != null)
			{
				if (this.mKeyController.IsDownDown())
				{
					base.NextFocus();
				}
				else if (this.mKeyController.IsUpDown())
				{
					base.PrevFocus();
				}
				else if (this.mKeyController.IsLeftDown())
				{
					base.PrevPageOrHeadFocus();
				}
				else if (this.mKeyController.IsRightDown())
				{
					base.NextPageOrTailFocus();
				}
				else if (this.mKeyController.IsMaruDown())
				{
					base.Select();
				}
				else if (this.mKeyController.IsBatuDown())
				{
					this.Back();
				}
			}
		}

		protected override void OnCallDestroy()
		{
			base.OnCallDestroy();
			this.MessageSlot = null;
			this.mArsenalManager = null;
			this.mOnBackListener = null;
			this.mKeyController = null;
			this.mOnItemSelectedListener = null;
		}

		private void SetOnBackListener(Action onBackListener)
		{
			this.mOnBackListener = onBackListener;
		}

		private void Back()
		{
			if (this.mOnBackListener != null)
			{
				this.mOnBackListener.Invoke();
			}
		}

		protected override void OnChangedFocusView(ArsenalScrollItemListChildNew focusToView)
		{
			if (base.mState == UIScrollList<ArsenalScrollSlotItemListChoiceModel, ArsenalScrollItemListChildNew>.ListState.Waiting)
			{
				if (this.mModels == null)
				{
					return;
				}
				if (this.mModels.Length <= 0)
				{
					return;
				}
				if (!this._isFirst && this._before_focus != focusToView.GetRealIndex())
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				}
				this._isFirst = false;
				this._before_focus = focusToView.GetRealIndex();
				CommonPopupDialog.Instance.StartPopup(focusToView.GetRealIndex() + 1 + "/" + this.mModels.Length, 0, CommonPopupDialogMessage.PlayType.Short);
			}
		}

		public void Message(SlotitemModel[] slotItemModels)
		{
			if (slotItemModels.Length == 0)
			{
				this.MessageSlot.set_localScale(Vector3.get_one());
			}
			else
			{
				this.MessageSlot.set_localScale(Vector3.get_zero());
			}
		}

		internal void StartControl()
		{
			base.StartControl();
			base.HeadFocus();
		}

		internal void SetKeyController(KeyControl keyController)
		{
			this.mKeyController = keyController;
		}

		internal void LockControl()
		{
			base.LockControl();
		}

		public void SetOnSelectedListener(Action<ArsenalScrollItemListChildNew> onItemSelectedListener)
		{
			this.mOnItemSelectedListener = onItemSelectedListener;
		}

		public SlotitemModel[] GetSlotItemModels()
		{
			List<SlotitemModel> list = new List<SlotitemModel>();
			ArsenalScrollSlotItemListChoiceModel[] mModels = this.mModels;
			for (int i = 0; i < mModels.Length; i++)
			{
				ArsenalScrollSlotItemListChoiceModel arsenalScrollSlotItemListChoiceModel = mModels[i];
				list.Add(arsenalScrollSlotItemListChoiceModel.GetSlotItemModel());
			}
			return list.ToArray();
		}

		internal void Initialize(ArsenalManager arsenalManager, SlotitemModel[] items, Camera camera)
		{
			this._isFirst = true;
			this._before_focus = 0;
			this.mArsenalManager = arsenalManager;
			ArsenalScrollSlotItemListChoiceModel[] models = this.GenerateModels(arsenalManager, items);
			base.ChangeImmediateContentPosition(UIScrollList<ArsenalScrollSlotItemListChoiceModel, ArsenalScrollItemListChildNew>.ContentDirection.Hell);
			base.Initialize(models);
			base.SetSwipeEventCamera(camera);
			this.Message(items);
		}

		internal void Refresh(SlotitemModel[] items)
		{
			this.mModels = this.GenerateModels(this.mArsenalManager, items);
			base.RefreshViews();
			if (this.mCurrentFocusView.GetModel() == null && this.mModels.Length != 0)
			{
				base.TailFocus();
			}
			else if (this.mCurrentFocusView.GetRealIndex() == this.mModels.Length - 1)
			{
				base.TailFocus();
			}
			else if (this.mViews_WorkSpace[this.mUserViewCount - 1].GetModel() == null && this.mModels.Length != 0)
			{
				int num = Array.IndexOf<ArsenalScrollItemListChildNew>(this.mViews_WorkSpace, this.mCurrentFocusView);
				base.TailFocusPage();
				base.ChangeFocusView(this.mViews_WorkSpace[num]);
			}
			this.Message(items);
		}

		protected override void OnSelect(ArsenalScrollItemListChildNew view)
		{
			if (view == null)
			{
				return;
			}
			if (view.GetModel() == null)
			{
				return;
			}
			if (this.mOnItemSelectedListener != null)
			{
				this.mOnItemSelectedListener.Invoke(view);
			}
		}

		internal void ResumeControl()
		{
			this.mKeyController.ClearKeyAll();
			this.mKeyController.firstUpdate = true;
			base.StartControl();
		}

		private ArsenalScrollSlotItemListChoiceModel[] GenerateModels(ArsenalManager manager, SlotitemModel[] slotItemModels)
		{
			List<ArsenalScrollSlotItemListChoiceModel> list = new List<ArsenalScrollSlotItemListChoiceModel>();
			for (int i = 0; i < slotItemModels.Length; i++)
			{
				SlotitemModel slotitemModel = slotItemModels[i];
				bool selected = manager.IsSelected(slotitemModel.MemId);
				ArsenalScrollSlotItemListChoiceModel arsenalScrollSlotItemListChoiceModel = new ArsenalScrollSlotItemListChoiceModel(slotitemModel, selected);
				list.Add(arsenalScrollSlotItemListChoiceModel);
			}
			return list.ToArray();
		}

		public void UpdateChoiceModelAndView(int realIndex, SlotitemModel slotItemModel)
		{
			this.mModels[realIndex] = new ArsenalScrollSlotItemListChoiceModel(slotItemModel, this.mArsenalManager.IsSelected(slotItemModel.MemId));
			base.RefreshViews();
		}

		internal void ClearChecked()
		{
			for (int i = 0; i < this.mModels.Length; i++)
			{
				this.mModels[i] = new ArsenalScrollSlotItemListChoiceModel(this.mModels[i].GetSlotItemModel(), false);
			}
			base.RefreshViews();
		}
	}
}
