using System;
using UnityEngine;

namespace KCV.Generic
{
	[Serializable]
	public class UIStatus
	{
		protected Transform _tra;

		protected UILabel _uiVal;

		protected UISprite _uiLabel;

		public virtual Transform transform
		{
			get
			{
				return this._tra;
			}
			set
			{
				this._tra = value;
			}
		}

		public virtual UILabel valueLabel
		{
			get
			{
				return this._uiVal;
			}
			set
			{
				this._uiVal = value;
			}
		}

		public virtual UISprite labelSprite
		{
			get
			{
				return this._uiLabel;
			}
			set
			{
				this._uiLabel = value;
			}
		}

		public UIStatus(Transform parent, string objName)
		{
			Util.FindParentToChild<Transform>(ref this._tra, parent, objName);
			Util.FindParentToChild<UILabel>(ref this._uiVal, this._tra, "Val");
			Util.FindParentToChild<UISprite>(ref this._uiLabel, this._tra, "Label");
		}

		public virtual bool UnInit()
		{
			this._tra = null;
			this._uiLabel = null;
			this._uiVal = null;
			return true;
		}
	}
}
