using LT.Tweening;
using System;
using UnityEngine;

namespace KCV.BattleCut
{
	[RequireComponent(typeof(BoxCollider2D)), RequireComponent(typeof(UIWidget)), RequireComponent(typeof(UIToggle))]
	public class UILabelButton : MonoBehaviour, ISelectedObject<int>
	{
		[SerializeField]
		private UIWidget _uiForeground;

		private int _nIndex;

		private bool _isFocus;

		private bool _isValid;

		private UIWidget _uiBackground;

		private BoxCollider2D _colBox2D;

		private UIToggle _uiToggle;

		private Color _cValidColor;

		private Color _cInvalidColor;

		public int index
		{
			get
			{
				return this._nIndex;
			}
			private set
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
				if (value && this.isValid)
				{
					this._isFocus = true;
					this._uiForeground.get_transform().LTCancel();
					this._uiForeground.get_transform().LTValue(this._uiForeground.alpha, 1f, Defines.FORMATION_FORMATIONLABEL_ALPHA_TIME).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
					{
						this._uiForeground.alpha = x;
					});
					this.toggle.value = true;
				}
				else
				{
					this._isFocus = false;
					this._uiForeground.get_transform().LTCancel();
					this._uiForeground.get_transform().LTValue(this._uiForeground.alpha, 0f, Defines.FORMATION_FORMATIONLABEL_ALPHA_TIME).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
					{
						this._uiForeground.alpha = x;
					});
					this.toggle.value = false;
				}
			}
		}

		public bool isValid
		{
			get
			{
				return this._isValid;
			}
		}

		protected UIWidget background
		{
			get
			{
				return this.GetComponentThis(ref this._uiBackground);
			}
		}

		protected UIWidget foreground
		{
			get
			{
				return this._uiForeground;
			}
		}

		public UIToggle toggle
		{
			get
			{
				return this.GetComponentThis(ref this._uiToggle);
			}
		}

		private void OnDestroy()
		{
			Mem.Del<UIWidget>(ref this._uiForeground);
			Mem.Del<Color>(ref this._cValidColor);
			Mem.Del<int>(ref this._nIndex);
			Mem.Del<bool>(ref this._isFocus);
			Mem.Del<bool>(ref this._isValid);
			Mem.Del<UIWidget>(ref this._uiBackground);
			Mem.Del<BoxCollider2D>(ref this._colBox2D);
			Mem.Del<UIToggle>(ref this._uiToggle);
		}

		public bool Init(int nIndex, bool isValid)
		{
			return this.Init(nIndex, isValid, this.background.color);
		}

		public bool Init(int nIndex, bool isValid, Color validColor)
		{
			return this.Init(nIndex, isValid, validColor, new Color(1f, 1f, 1f, 0.5f));
		}

		public bool Init(int nIndex, bool isValid, Color validColor, Color invalidColor)
		{
			this._nIndex = nIndex;
			this._isValid = isValid;
			this._cValidColor = validColor;
			this._cInvalidColor = invalidColor;
			this.SetValidColor(this.isValid);
			return true;
		}

		private void SetValidColor(bool isValid)
		{
			this.background.color = ((!isValid) ? this._cInvalidColor : this._cValidColor);
		}

		public void SetValid(bool isValid)
		{
			this._isValid = isValid;
			this.SetValidColor(this._isValid);
		}
	}
}
