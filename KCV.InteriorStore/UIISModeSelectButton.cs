using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.InteriorStore
{
	public class UIISModeSelectButton : MonoBehaviour
	{
		private DelDecideISMode _delDecideISMode;

		private List<UIButton> _listBtns;

		private bool _isFocus;

		private bool _isDecide;

		private ISMode _iMode;

		public bool isFocus
		{
			get
			{
				return this._isFocus;
			}
			set
			{
				if (value && !this._isFocus)
				{
					this._isFocus = true;
					this.setBtnsState(this._isFocus);
				}
				else if (!value && this.isFocus)
				{
					this._isFocus = false;
					this.setBtnsState(this._isFocus);
				}
			}
		}

		public ISMode mode
		{
			get
			{
				return this._iMode;
			}
		}

		public bool isColliderEnabled
		{
			get
			{
				return base.GetComponent<Collider2D>().get_enabled();
			}
			set
			{
				base.GetComponent<Collider2D>().set_enabled(value);
			}
		}

		private void Awake()
		{
			this._isFocus = false;
			this._isDecide = false;
			this._iMode = ISMode.None;
			this._listBtns = new List<UIButton>();
			this._listBtns.AddRange(base.GetComponents<UIButton>());
			this.setBtnsState(this._isFocus);
			this._listBtns.get_Item(0).onClick = Util.CreateEventDelegateList(this, "decideButton", null);
			this.isColliderEnabled = true;
		}

		public void Reset()
		{
			this._isDecide = false;
			this.isFocus = false;
			this.isColliderEnabled = true;
		}

		public bool Init(ISMode iMode, DelDecideISMode delDecide)
		{
			this._iMode = iMode;
			this._delDecideISMode = delDecide;
			return true;
		}

		private void Update()
		{
			if (!this.isFocus && this._listBtns.get_Item(0).state != UIButtonColor.State.Normal)
			{
				this.setBtnsState(false);
			}
		}

		private void setBtnsState(bool isFocus)
		{
			UIButtonColor.State state = (!isFocus) ? UIButtonColor.State.Normal : UIButtonColor.State.Pressed;
			using (List<UIButton>.Enumerator enumerator = this._listBtns.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					UIButton current = enumerator.get_Current();
					current.state = state;
				}
			}
		}

		private void decideButton()
		{
			this._isDecide = true;
			if (this._delDecideISMode != null)
			{
				this._delDecideISMode(this._iMode);
			}
		}
	}
}
