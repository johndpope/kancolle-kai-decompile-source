using KCV.View.ScrollView;
using local.models;
using System;
using UnityEngine;

namespace KCV.Scene.Revamp
{
	[RequireComponent(typeof(UIWidget))]
	public class UIRevampRecipeScrollChildNew : MonoBehaviour, UIScrollListItem<RevampRecipeScrollUIModel, UIRevampRecipeScrollChildNew>
	{
		private RevampRecipeScrollUIModel mRevampRecipeScrollUIModel;

		private int mRealIndex;

		private UIWidget mWidgetThis;

		[SerializeField]
		private Transform mBlinkObject;

		[SerializeField]
		private UITexture mTexture_WeaponThumbnail;

		[SerializeField]
		private UISprite mSprite_WeaponTypeIcon;

		[SerializeField]
		private UILabel mLabel_WeaponName;

		[SerializeField]
		private UILabel mLabel_Fuel;

		[SerializeField]
		private UILabel mLabel_Ammo;

		[SerializeField]
		private UILabel mLabel_Steel;

		[SerializeField]
		private UILabel mLabel_DevKit;

		[SerializeField]
		private UILabel mLabel_Bauxite;

		[SerializeField]
		private UILabel mLabel_RevampKit;

		[SerializeField]
		private UIButton mButton_Select;

		[SerializeField]
		private UISprite mSprite_ButtonState;

		private Transform mCachedTransform;

		private Action<UIRevampRecipeScrollChildNew> mOnTouchListener;

		public float alpha
		{
			get
			{
				if (this.mWidgetThis != null)
				{
					return this.mWidgetThis.alpha;
				}
				return -1f;
			}
			set
			{
				if (this.mWidgetThis != null)
				{
					this.mWidgetThis.alpha = value;
				}
			}
		}

		private void Awake()
		{
			this.mWidgetThis = base.GetComponent<UIWidget>();
		}

		public void Initialize(int realIndex, RevampRecipeScrollUIModel model)
		{
			this.mRealIndex = realIndex;
			this.mRevampRecipeScrollUIModel = model;
			RevampRecipeModel model2 = this.mRevampRecipeScrollUIModel.Model;
			SlotitemModel_Mst slotitem = model2.Slotitem;
			this.mTexture_WeaponThumbnail.mainTexture = (Resources.Load("Textures/SlotItems/" + slotitem.MstId + "/2") as Texture);
			this.mLabel_WeaponName.text = slotitem.Name;
			this.mSprite_WeaponTypeIcon.spriteName = "icon_slot" + slotitem.Type4;
			this.mLabel_Fuel.text = model2.Fuel.ToString();
			this.mLabel_Steel.text = model2.Steel.ToString();
			this.mLabel_DevKit.text = model2.DevKit.ToString();
			this.mLabel_Ammo.text = model2.Ammo.ToString();
			this.mLabel_Bauxite.text = model2.Baux.ToString();
			this.mLabel_RevampKit.text = model2.RevKit.ToString();
			this.mWidgetThis.alpha = 1f;
			bool clickable = this.mRevampRecipeScrollUIModel.Clickable;
			if (clickable)
			{
				this.mSprite_ButtonState.spriteName = "btn_select";
			}
			else
			{
				this.mSprite_ButtonState.spriteName = "btn_select_off";
			}
		}

		[Obsolete("Inspector上で設定して使用します")]
		public void Touch()
		{
			if (this.mOnTouchListener != null)
			{
				this.mOnTouchListener.Invoke(this);
			}
		}

		public void InitializeDefault(int realIndex)
		{
			this.mWidgetThis.alpha = 1E-08f;
			this.mRealIndex = realIndex;
			this.mRevampRecipeScrollUIModel = null;
		}

		public int GetRealIndex()
		{
			return this.mRealIndex;
		}

		public RevampRecipeScrollUIModel GetModel()
		{
			return this.mRevampRecipeScrollUIModel;
		}

		public int GetHeight()
		{
			return 140;
		}

		public void Hover()
		{
			UISelectedObject.SelectedOneObjectBlink(this.mBlinkObject.get_gameObject(), true);
			bool clickable = this.mRevampRecipeScrollUIModel.Clickable;
			if (clickable)
			{
				this.mSprite_ButtonState.spriteName = "btn_select_on";
			}
			else
			{
				this.mSprite_ButtonState.spriteName = "btn_select_off";
			}
		}

		public Transform GetTransform()
		{
			if (this.mCachedTransform == null)
			{
				this.mCachedTransform = base.get_transform();
			}
			return this.mCachedTransform;
		}

		public void RemoveHover()
		{
			UISelectedObject.SelectedOneObjectBlink(this.mBlinkObject.get_gameObject(), false);
			bool flag = this.mRevampRecipeScrollUIModel != null && this.mRevampRecipeScrollUIModel.Clickable;
			if (flag)
			{
				this.mSprite_ButtonState.spriteName = "btn_select";
			}
			else
			{
				this.mSprite_ButtonState.spriteName = "btn_select_off";
			}
		}

		internal void Hide()
		{
			this.mWidgetThis.alpha = 1E-11f;
		}

		internal void Show()
		{
			this.mWidgetThis.alpha = 1f;
		}

		public void SetOnTouchListener(Action<UIRevampRecipeScrollChildNew> onTouchListener)
		{
			this.mOnTouchListener = onTouchListener;
		}

		private void OnDestroy()
		{
			this.mRevampRecipeScrollUIModel = null;
			this.mWidgetThis = null;
			this.mBlinkObject = null;
			this.mTexture_WeaponThumbnail = null;
			this.mSprite_WeaponTypeIcon = null;
			this.mLabel_WeaponName = null;
			this.mLabel_Fuel = null;
			this.mLabel_Ammo = null;
			this.mLabel_Steel = null;
			this.mLabel_DevKit = null;
			this.mLabel_Bauxite = null;
			this.mLabel_RevampKit = null;
			this.mButton_Select = null;
			this.mSprite_ButtonState = null;
			this.mCachedTransform = null;
			this.mOnTouchListener = null;
		}
	}
}
