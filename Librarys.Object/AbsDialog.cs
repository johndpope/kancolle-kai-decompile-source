using System;
using System.Collections.Generic;
using UnityEngine;

namespace Librarys.Object
{
	[RequireComponent(typeof(UIPanel))]
	public abstract class AbsDialog<CallbackType, ButtonType> : MonoBehaviour where CallbackType : struct where ButtonType : AbsDialogButton<CallbackType>
	{
		[SerializeField]
		protected List<ButtonType> _listButtons;

		private int _nIndex;

		private bool _isOpen;

		private UIPanel _uiPanel;

		protected Action _actOnCancel;

		protected Action<CallbackType> _actOnDecide;

		public int currentIndex
		{
			get
			{
				return this._nIndex;
			}
			protected set
			{
				this._nIndex = value;
			}
		}

		public bool isOpen
		{
			get
			{
				return this._isOpen;
			}
			protected set
			{
				this._isOpen = value;
			}
		}

		public ButtonType currentButton
		{
			get
			{
				return this._listButtons.get_Item(this.currentIndex);
			}
		}

		public UIPanel panel
		{
			get
			{
				return this.GetComponentThis(ref this._uiPanel);
			}
		}

		protected virtual void OnDestroy()
		{
			this.OnUnInit();
			Mem.DelListSafe<ButtonType>(ref this._listButtons);
			Mem.Del<int>(ref this._nIndex);
			Mem.Del<Action>(ref this._actOnCancel);
			Mem.Del<Action<CallbackType>>(ref this._actOnDecide);
		}

		public virtual bool Init(Action onCancel, Action<CallbackType> onDecide)
		{
			this._actOnCancel = onCancel;
			this._actOnDecide = onDecide;
			int cnt = 0;
			this._listButtons.ForEach(delegate(ButtonType x)
			{
				x.Init(cnt, true, false, 10, Util.CreateEventDelegateList(this, "OnActive", x.index), delegate
				{
					this.OnDecide();
				});
				cnt++;
			});
			this.OnInit();
			return true;
		}

		protected abstract void PreparaNext(bool isFoward);

		public void Next()
		{
			this.PreparaNext(true);
		}

		public void Prev()
		{
			this.PreparaNext(false);
		}

		public virtual void Open(Action onFinished)
		{
			if (this.isOpen)
			{
				return;
			}
			this.isOpen = true;
			this.OpenAnimation(delegate
			{
				this._listButtons.ForEach(delegate(ButtonType x)
				{
					x.toggle.set_enabled(true);
				});
			});
		}

		public virtual void Close(Action onFinished)
		{
			if (!this.isOpen)
			{
				return;
			}
			this.CloseAimation(delegate
			{
				this.isOpen = false;
				Dlg.Call(ref onFinished);
			});
		}

		protected abstract void OpenAnimation(Action onFinished);

		protected abstract void CloseAimation(Action onFinished);

		protected virtual void ChangeFocus(int nIndex)
		{
			this.OnChangeFocus();
			this._listButtons.ForEach(delegate(ButtonType x)
			{
				x.isFocus = (x.index == nIndex);
			});
		}

		protected virtual void OnInit()
		{
		}

		protected virtual void OnUnInit()
		{
		}

		protected virtual void OnChangeFocus()
		{
		}

		protected abstract void OnActive(int nIndex);

		public virtual void OnCancel()
		{
			Dlg.Call(ref this._actOnCancel);
		}

		public virtual void OnDecide()
		{
			this._listButtons.ForEach(delegate(ButtonType x)
			{
				x.toggle.set_enabled(false);
			});
			if (this._actOnDecide != null && this.currentButton != null)
			{
				ButtonType currentButton = this.currentButton;
				Dlg.Call<CallbackType>(ref this._actOnDecide, currentButton.value);
			}
		}
	}
}
