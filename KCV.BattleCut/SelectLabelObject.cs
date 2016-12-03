using System;
using UnityEngine;

namespace KCV.BattleCut
{
	[Serializable]
	public class SelectLabelObject
	{
		[SerializeField]
		private Transform _tra;

		[SerializeField]
		private UILabel _uiLabel;

		[SerializeField]
		private int _nIndex;

		private Color _cDefaultColor;

		private bool _isActive;

		private bool _isFocus;

		public Transform transform
		{
			get
			{
				return this._tra;
			}
		}

		public UILabel label
		{
			get
			{
				return this._uiLabel;
			}
		}

		public int index
		{
			get
			{
				return this._nIndex;
			}
		}

		public bool isActive
		{
			get
			{
				return this._isActive;
			}
			set
			{
				this._isActive = value;
				this._uiLabel.alpha = ((!value) ? 0.5f : 1f);
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
				this._uiLabel.color = ((!value) ? ((!this.isActive) ? new Color(0.5f, 0.5f, 0.5f, 0.5f) : this._cDefaultColor) : Util.CursolColor);
			}
		}

		public SelectLabelObject(Transform transform, int nIndex)
		{
			this._tra = transform;
			this._uiLabel = this._tra.GetComponent<UILabel>();
			this._nIndex = nIndex;
			this._cDefaultColor = this._uiLabel.color;
			this.isFocus = false;
			this.isActive = false;
		}

		public bool Init(int nIndex)
		{
			this._nIndex = nIndex;
			return true;
		}
	}
}
