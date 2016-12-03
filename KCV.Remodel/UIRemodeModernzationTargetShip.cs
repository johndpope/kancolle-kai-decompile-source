using local.models;
using System;
using UnityEngine;

namespace KCV.Remodel
{
	public class UIRemodeModernzationTargetShip : MonoBehaviour
	{
		public enum ActionType
		{
			OnTouch
		}

		public delegate void UIRemodeModernzationTargetShipAction(UIRemodeModernzationTargetShip.ActionType actionType, UIRemodeModernzationTargetShip calledObject);

		[SerializeField]
		private CommonShipBanner mCommonShipBanner;

		[SerializeField]
		private Transform mTransformLEVEL;

		[SerializeField]
		private UILabel mLabel_Level;

		[SerializeField]
		private UILabel mLabel_Name;

		[SerializeField]
		private UISprite mSprite_Karyoku;

		[SerializeField]
		private UISprite mSprite_Raisou;

		[SerializeField]
		private UISprite mSprite_Soukou;

		[SerializeField]
		private UISprite mSprite_Taikuu;

		[SerializeField]
		private UISprite mSprite_Luck;

		[SerializeField]
		private UISprite mSprite_Add;

		[SerializeField]
		private UIButton mButton_Action;

		private ShipModel mShipModel;

		private UIRemodeModernzationTargetShip.UIRemodeModernzationTargetShipAction mUIRemodeModernzationTargetShipAction;

		private void Start()
		{
			this.InitializeButtonColor();
		}

		public void SetOnUIRemodeModernzationTargetShipActionListener(UIRemodeModernzationTargetShip.UIRemodeModernzationTargetShipAction actionCallBAck)
		{
			this.mUIRemodeModernzationTargetShipAction = actionCallBAck;
		}

		public ShipModel GetSlotInShip()
		{
			return this.mShipModel;
		}

		public void Initialize(ShipModel shipModel)
		{
			this.mShipModel = shipModel;
			if (this.mShipModel != null)
			{
				this.SetShip(this.mShipModel);
			}
			else
			{
				this.UnSet();
			}
		}

		public void OnTouchItem()
		{
			this.OnAction(UIRemodeModernzationTargetShip.ActionType.OnTouch, this);
		}

		public void Hover()
		{
			this.mButton_Action.SetState(UIButtonColor.State.Hover, true);
			UISelectedObject.SelectedOneObjectBlink(this.mButton_Action.get_gameObject(), true);
		}

		public void RemoveHover()
		{
			this.mButton_Action.SetState(UIButtonColor.State.Normal, true);
			UISelectedObject.SelectedOneObjectBlink(this.mButton_Action.get_gameObject(), false);
		}

		public void UnSet()
		{
			Texture mainTexture = this.mCommonShipBanner.GetUITexture().mainTexture;
			this.mCommonShipBanner.GetUITexture().mainTexture = null;
			UserInterfaceRemodelManager.instance.ReleaseRequestBanner(ref mainTexture);
			this.mShipModel = null;
			this.mCommonShipBanner.SetActive(false);
			this.mTransformLEVEL.SetActive(false);
			this.mSprite_Karyoku.alpha = 0f;
			this.mSprite_Raisou.alpha = 0f;
			this.mSprite_Soukou.alpha = 0f;
			this.mSprite_Taikuu.alpha = 0f;
			this.mSprite_Luck.alpha = 0f;
			this.mLabel_Level.alpha = 0f;
			this.mSprite_Add.alpha = 1f;
			this.mLabel_Name.text = string.Empty;
		}

		private void SetShip(ShipModel shipModel)
		{
			Texture mainTexture = this.mCommonShipBanner.GetUITexture().mainTexture;
			this.mCommonShipBanner.GetUITexture().mainTexture = null;
			UserInterfaceRemodelManager.instance.ReleaseRequestBanner(ref mainTexture);
			this.mShipModel = shipModel;
			this.mCommonShipBanner.SetActive(true);
			this.mTransformLEVEL.SetActive(true);
			this.mSprite_Karyoku.alpha = 1f;
			this.mSprite_Raisou.alpha = 1f;
			this.mSprite_Soukou.alpha = 1f;
			this.mSprite_Taikuu.alpha = 1f;
			this.mSprite_Luck.alpha = 1f;
			this.mLabel_Level.alpha = 1f;
			this.mSprite_Add.alpha = 0f;
			if (0 < shipModel.PowUpKaryoku)
			{
				this.mSprite_Karyoku.spriteName = "icon_1_on";
			}
			else
			{
				this.mSprite_Karyoku.spriteName = "icon_1";
			}
			if (0 < shipModel.PowUpRaisou)
			{
				this.mSprite_Raisou.spriteName = "icon_2_on";
			}
			else
			{
				this.mSprite_Raisou.spriteName = "icon_2";
			}
			if (0 < shipModel.PowUpSoukou)
			{
				this.mSprite_Soukou.spriteName = "icon_3_on";
			}
			else
			{
				this.mSprite_Soukou.spriteName = "icon_3";
			}
			if (0 < shipModel.PowUpTaikuu)
			{
				this.mSprite_Taikuu.spriteName = "icon_4_on";
			}
			else
			{
				this.mSprite_Taikuu.spriteName = "icon_4";
			}
			if (0 < shipModel.PowUpLucky)
			{
				this.mSprite_Luck.spriteName = "icon_5_on";
			}
			else
			{
				this.mSprite_Luck.spriteName = "icon_5";
			}
			this.mCommonShipBanner.SetShipData(shipModel);
			this.mCommonShipBanner.StopParticle();
			this.mLabel_Level.text = shipModel.Level.ToString();
			this.mLabel_Name.text = shipModel.Name;
		}

		private void OnAction(UIRemodeModernzationTargetShip.ActionType actionType, UIRemodeModernzationTargetShip calledObject)
		{
			if (base.get_enabled() && this.mUIRemodeModernzationTargetShipAction != null)
			{
				this.mUIRemodeModernzationTargetShipAction(actionType, calledObject);
			}
		}

		private void InitializeButtonColor()
		{
			this.mButton_Action.hover = Util.CursolColor;
			this.mButton_Action.defaultColor = Color.get_white();
			this.mButton_Action.pressed = Color.get_white();
			this.mButton_Action.disabledColor = Color.get_white();
		}

		public void SetEnabled(bool enabled)
		{
			base.set_enabled(enabled);
			this.mButton_Action.set_enabled(enabled);
		}

		internal void Release()
		{
			this.mUIRemodeModernzationTargetShipAction = null;
			this.mCommonShipBanner = null;
			this.mTransformLEVEL = null;
			this.mLabel_Level = null;
			this.mLabel_Name = null;
			this.mSprite_Karyoku.RemoveFromPanel();
			this.mSprite_Karyoku = null;
			this.mSprite_Raisou.RemoveFromPanel();
			this.mSprite_Raisou = null;
			this.mSprite_Soukou.RemoveFromPanel();
			this.mSprite_Soukou = null;
			this.mSprite_Taikuu.RemoveFromPanel();
			this.mSprite_Taikuu = null;
			this.mSprite_Luck.RemoveFromPanel();
			this.mSprite_Luck = null;
			this.mSprite_Add.RemoveFromPanel();
			this.mSprite_Add = null;
			this.mButton_Action.Release();
			this.mButton_Action = null;
			this.mShipModel = null;
		}

		internal void Refresh()
		{
			this.Initialize(this.mShipModel);
		}

		internal CommonShipBanner GetBanner()
		{
			return this.mCommonShipBanner;
		}
	}
}
