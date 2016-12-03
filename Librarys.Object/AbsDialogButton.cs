using System;
using System.Collections.Generic;
using UnityEngine;

namespace Librarys.Object
{
	[RequireComponent(typeof(UIToggle))]
	public abstract class AbsDialogButton<CallbackType> : MonoBehaviour
	{
		private int _nIndex;

		private bool _isFocus;

		private bool _isValid;

		private UIToggle _uiToggle;

		private BoxCollider2D _col2D;

		private CallbackType _pValue;

		public int index
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

		public bool isFocus
		{
			get
			{
				return this._isFocus;
			}
			set
			{
				this._isFocus = value;
			}
		}

		public bool isValid
		{
			get
			{
				return this._isValid;
			}
			protected set
			{
				this._isValid = value;
			}
		}

		public UIToggle toggle
		{
			get
			{
				return this.GetComponentThis(ref this._uiToggle);
			}
		}

		public CallbackType value
		{
			get
			{
				return this._pValue;
			}
			protected set
			{
				this._pValue = value;
			}
		}

		public virtual bool Init(int nIndex, bool isValid, bool isColliderEnabled, int nToggleGroup, List<EventDelegate> onActive, Action onDecide)
		{
			this.index = nIndex;
			this.isValid = isValid;
			this.toggle.set_enabled(isColliderEnabled);
			this.toggle.group = nToggleGroup;
			this.toggle.onActive = onActive;
			this.toggle.onDecide = onDecide;
			this.OnInit();
			return true;
		}

		private void OnDestroy()
		{
			Mem.Del<int>(ref this._nIndex);
			Mem.Del<bool>(ref this._isFocus);
			Mem.Del<bool>(ref this._isValid);
			Mem.Del<UIToggle>(ref this._uiToggle);
			Mem.Del<BoxCollider2D>(ref this._col2D);
			Mem.Del<CallbackType>(ref this._pValue);
			this.OnUnInit();
		}

		protected virtual void OnInit()
		{
		}

		protected virtual void OnUnInit()
		{
		}

		protected virtual void OnFocus(bool isFocus)
		{
		}
	}
}
