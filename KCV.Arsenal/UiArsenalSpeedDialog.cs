using KCV.Utils;
using local.managers;
using System;
using UnityEngine;

namespace KCV.Arsenal
{
	public class UiArsenalSpeedDialog : MonoBehaviour
	{
		[SerializeField]
		private UIPanel _maskPanel;

		[SerializeField]
		private UITexture _maskBg;

		[SerializeField]
		private UITexture _dialogBg;

		[SerializeField]
		private UILabel _fromLabel;

		[SerializeField]
		private UILabel _toLabel;

		[SerializeField]
		private UISprite _yesBtn;

		[SerializeField]
		private UISprite _noBtn;

		[SerializeField]
		private UIButton _uiOverlayBtn;

		public bool IsShow;

		public int Index;

		public void init()
		{
			this.IsShow = false;
			this.Index = 0;
			if (this._maskPanel == null)
			{
				this._maskPanel = GameObject.Find("ConstructBgPanel").GetComponent<UIPanel>();
			}
			if (this._maskBg == null)
			{
				this._maskBg = this._maskPanel.get_transform().FindChild("Bg").GetComponent<UITexture>();
			}
			if (this._dialogBg == null)
			{
				this._dialogBg = base.get_transform().FindChild("Bg").GetComponent<UITexture>();
			}
			if (this._fromLabel == null)
			{
				this._fromLabel = base.get_transform().FindChild("LabelFrom").GetComponent<UILabel>();
			}
			if (this._toLabel == null)
			{
				this._toLabel = base.get_transform().FindChild("LabelTo").GetComponent<UILabel>();
			}
			if (this._yesBtn == null)
			{
				this._yesBtn = base.get_transform().FindChild("BtnYes").GetComponent<UISprite>();
			}
			if (this._noBtn == null)
			{
				this._noBtn = base.get_transform().FindChild("BtnNo").GetComponent<UISprite>();
			}
			if (this._uiOverlayBtn == null)
			{
				this._uiOverlayBtn = base.get_transform().FindChild("OverlayBtn").GetComponent<UIButton>();
			}
			UIButtonMessage component = this._yesBtn.GetComponent<UIButtonMessage>();
			component.target = base.get_gameObject();
			component.functionName = "YesBtnEL";
			component.trigger = UIButtonMessage.Trigger.OnClick;
			UIButtonMessage component2 = this._noBtn.GetComponent<UIButtonMessage>();
			component2.target = base.get_gameObject();
			component2.functionName = "NoBtnEL";
			component2.trigger = UIButtonMessage.Trigger.OnClick;
			EventDelegate.Add(this._uiOverlayBtn.onClick, new EventDelegate.Callback(this._onClickOverlayButton));
			base.get_transform().GetComponent<UIPanel>().alpha = 0f;
		}

		private void OnDestroy()
		{
			this._maskPanel = null;
			this._maskBg = null;
			this._dialogBg = null;
			this._fromLabel = null;
			this._toLabel = null;
			this._yesBtn = null;
			this._noBtn = null;
			this._uiOverlayBtn = null;
		}

		public void showHighSpeedDialog(int dockNum)
		{
			base.get_transform().GetComponent<UIPanel>().alpha = 1f;
			this.IsShow = true;
			this.Index = 0;
			this.updateSpeedDialogBtn(0);
			this._maskPanel.get_transform().set_localPosition(Vector3.get_zero());
			this._maskBg.SafeGetTweenAlpha(0f, 0.5f, 0.5f, 0f, UITweener.Method.Linear, UITweener.Style.Once, base.get_gameObject(), string.Empty);
			base.get_transform().set_localPosition(Vector3.get_zero());
			ArsenalTaskManager.GetDialogPopUp().Open(base.get_gameObject(), 0f, 0f, 1f, 1f);
			this._uiOverlayBtn.GetComponent<Collider2D>().set_isTrigger(true);
			ArsenalManager arsenalManager = new ArsenalManager();
			arsenalManager.LargeState = arsenalManager.GetDock(dockNum + 1).IsLarge();
			int buildKit = arsenalManager.GetMaxForCreateShip().BuildKit;
			int buildKit2 = ArsenalTaskManager.GetLogicManager().Material.BuildKit;
			this._fromLabel.textInt = buildKit2;
			this._toLabel.textInt = buildKit2 - buildKit;
		}

		public void updateSpeedDialogBtn(int num)
		{
			if (this.Index != num)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			}
			this.Index = num;
			if (this.Index == 1)
			{
				this._yesBtn.spriteName = "btn_yes";
				this._noBtn.spriteName = "btn_no_on";
				UISelectedObject.SelectedOneButtonZoomUpDown(this._yesBtn.get_gameObject(), false);
				UISelectedObject.SelectedOneButtonZoomUpDown(this._noBtn.get_gameObject(), true);
			}
			else
			{
				this._yesBtn.spriteName = "btn_yes_on";
				this._noBtn.spriteName = "btn_no";
				UISelectedObject.SelectedOneButtonZoomUpDown(this._yesBtn.get_gameObject(), true);
				UISelectedObject.SelectedOneButtonZoomUpDown(this._noBtn.get_gameObject(), false);
			}
		}

		public void hideHighSpeedDialog()
		{
			if (!this.IsShow)
			{
				return;
			}
			this.IsShow = false;
			this._maskBg.SafeGetTweenAlpha(0.5f, 0f, 0.5f, 0f, UITweener.Method.Linear, UITweener.Style.Once, base.get_gameObject(), "compHideDialog");
			BaseDialogPopup.Close(base.get_gameObject(), 0.5f, UITweener.Method.Linear);
			this._uiOverlayBtn.GetComponent<Collider2D>().set_isTrigger(false);
		}

		private void compHideDialog()
		{
			base.get_transform().GetComponent<UIPanel>().alpha = 0f;
		}

		public void YesBtnEL(GameObject obj)
		{
			this.updateSpeedDialogBtn(0);
			ArsenalTaskManager._clsArsenal.StartHighSpeedProcess();
		}

		public void NoBtnEL(GameObject obj)
		{
			this.updateSpeedDialogBtn(1);
			ArsenalTaskManager._clsArsenal.StartHighSpeedProcess();
		}

		private void _onClickOverlayButton()
		{
			ArsenalTaskManager._clsArsenal.hideHighSpeedDialog();
		}
	}
}
