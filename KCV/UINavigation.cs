using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV
{
	[RequireComponent(typeof(UIPanel))]
	public class UINavigation<T> : MonoBehaviour where T : UINavigation<T>
	{
		public enum Anchor
		{
			TopLeft,
			TopRight,
			BottomLeft,
			BottomRight
		}

		[SerializeField]
		protected UISprite _uiBackground;

		[SerializeField]
		protected UIHowTo _uiHowTo;

		private UINavigation<T>.Anchor _iAnchor;

		private UIPanel _uiPanel;

		private SettingModel _clsSettingModel;

		private List<UIHowToDetail> _listDetails;

		public virtual UINavigation<T>.Anchor anchor
		{
			get
			{
				return this._iAnchor;
			}
		}

		public virtual UIPanel panel
		{
			get
			{
				return this.GetComponentThis(ref this._uiPanel);
			}
		}

		protected virtual SettingModel settingModel
		{
			get
			{
				if (this._clsSettingModel == null)
				{
					this._clsSettingModel = new SettingModel();
				}
				return this._clsSettingModel;
			}
		}

		protected virtual List<UIHowToDetail> details
		{
			get
			{
				if (this._listDetails == null)
				{
					this._listDetails = new List<UIHowToDetail>();
				}
				return this._listDetails;
			}
		}

		protected void OnDestroy()
		{
			Mem.Del(ref this._uiBackground);
			Mem.Del<UIHowTo>(ref this._uiHowTo);
			Mem.Del<UIPanel>(ref this._uiPanel);
			Mem.Del<SettingModel>(ref this._clsSettingModel);
			Mem.DelListSafe<UIHowToDetail>(ref this._listDetails);
			this.OnUnInit();
		}

		protected virtual T VirtualCtor(SettingModel model)
		{
			this._clsSettingModel = model;
			this._listDetails = new List<UIHowToDetail>();
			return (T)((object)this);
		}

		protected virtual void OnUnInit()
		{
		}

		public virtual T SetNavigation(List<UIHowToDetail> details)
		{
			this._listDetails = details;
			this.DrawRefresh();
			return (T)((object)this);
		}

		public virtual T SetAnchor(UINavigation<T>.Anchor iAnchor)
		{
			if (this._iAnchor == iAnchor)
			{
				return (T)((object)this);
			}
			switch (iAnchor)
			{
			case UINavigation<T>.Anchor.TopLeft:
				this._uiHowTo.SetHorizontalAlign(UIHowToHorizontalAlign.left);
				this._uiBackground.flip = UIBasicSprite.Flip.Horizontally;
				this._uiBackground.pivot = UIWidget.Pivot.Left;
				this._uiBackground.get_transform().localPositionZero();
				base.get_transform().set_localPosition(new Vector3(-480f, 256f, 0f));
				break;
			case UINavigation<T>.Anchor.TopRight:
				this._uiHowTo.SetHorizontalAlign(UIHowToHorizontalAlign.right);
				this._uiBackground.flip = UIBasicSprite.Flip.Nothing;
				this._uiBackground.pivot = UIWidget.Pivot.Right;
				this._uiBackground.get_transform().localPositionZero();
				base.get_transform().set_localPosition(new Vector3(480f, 256f, 0f));
				break;
			case UINavigation<T>.Anchor.BottomLeft:
				this._uiHowTo.SetHorizontalAlign(UIHowToHorizontalAlign.left);
				this._uiBackground.flip = UIBasicSprite.Flip.Horizontally;
				this._uiBackground.pivot = UIWidget.Pivot.Left;
				this._uiBackground.get_transform().localPositionZero();
				base.get_transform().set_localPosition(new Vector3(-480f, -256f, 0f));
				break;
			case UINavigation<T>.Anchor.BottomRight:
				this._uiHowTo.SetHorizontalAlign(UIHowToHorizontalAlign.right);
				this._uiBackground.flip = UIBasicSprite.Flip.Nothing;
				this._uiBackground.pivot = UIWidget.Pivot.Right;
				this._uiBackground.get_transform().localPositionZero();
				base.get_transform().set_localPosition(new Vector3(480f, -256f, 0f));
				break;
			}
			this._iAnchor = iAnchor;
			return (T)((object)this);
		}

		protected virtual void AddDetail(HowToKey iKey, string strDescription)
		{
			this._listDetails.Add(new UIHowToDetail(iKey, strDescription));
		}

		protected virtual T DrawRefresh()
		{
			this._uiHowTo.Refresh(this._listDetails.ToArray());
			this._listDetails.Clear();
			return (T)((object)this);
		}
	}
}
