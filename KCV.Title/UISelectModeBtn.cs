using LT.Tweening;
using System;
using UnityEngine;

namespace KCV.Title
{
	[Serializable]
	public class UISelectModeBtn : IDisposable
	{
		[SerializeField]
		private UISprite _uiSprite;

		private SelectMode _iMode;

		private UIToggle _uiToggle;

		private bool _isFocus;

		public Transform transform
		{
			get
			{
				return this._uiSprite.get_transform();
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
				if (value)
				{
					this._isFocus = true;
					this._uiSprite.spriteName = string.Format("btn_{0}_on", this._iMode.ToString());
					this._uiSprite.get_transform().LTCancel();
					this._uiSprite.get_transform().LTValue(this._uiSprite.color, Color.get_white(), 0.2f).setEase(LeanTweenType.linear).setOnUpdate(delegate(Color x)
					{
						this._uiSprite.color = x;
					});
					this.toggle.value = true;
				}
				else
				{
					this._isFocus = false;
					this._uiSprite.spriteName = string.Format("btn_{0}", this._iMode.ToString());
					this._uiSprite.get_transform().LTCancel();
					this._uiSprite.get_transform().LTValue(this._uiSprite.color, Color.get_gray(), 0.2f).setEase(LeanTweenType.linear).setOnUpdate(delegate(Color x)
					{
						this._uiSprite.color = x;
					});
					this.toggle.value = false;
				}
			}
		}

		public UIToggle toggle
		{
			get
			{
				return this._uiSprite.GetComponent<UIToggle>();
			}
		}

		public SelectMode mode
		{
			get
			{
				return this._iMode;
			}
		}

		public float alpha
		{
			get
			{
				return this._uiSprite.alpha;
			}
			set
			{
				this._uiSprite.alpha = value;
			}
		}

		public UISelectModeBtn(Transform obj)
		{
			this._uiSprite = obj.GetComponent<UISprite>();
			this._isFocus = false;
			this._iMode = SelectMode.Appointed;
		}

		public bool Init(SelectMode iMode)
		{
			this._iMode = iMode;
			this._uiSprite.color = Color.get_gray();
			return true;
		}

		public void Dispose()
		{
			Mem.Del(ref this._uiSprite);
			Mem.Del<SelectMode>(ref this._iMode);
			Mem.Del<UIToggle>(ref this._uiToggle);
			Mem.Del<bool>(ref this._isFocus);
		}
	}
}
