using KCV.Utils;
using System;
using UnityEngine;

namespace KCV.Arsenal
{
	public class UiArsenalDockOpenDialog : MonoBehaviour
	{
		[SerializeField]
		private GameObject _dialogObj;

		[SerializeField]
		private UITexture _dialogBg;

		[SerializeField]
		private UISprite _maskBg;

		[SerializeField]
		private UILabel _keyLabel_b;

		[SerializeField]
		private UILabel _keyLabel_a;

		[SerializeField]
		private UISprite _yesBtn;

		[SerializeField]
		private UISprite _noBtn;

		[SerializeField]
		private Animation _openInfoAnim;

		public bool IsShow;

		public int Index;

		public int _dockIndex;

		public void init()
		{
			this.IsShow = false;
			this.Index = 0;
			this._dockIndex = 0;
			if (this._dialogObj == null)
			{
				this._dialogObj = base.get_transform().FindChild("DialogObj").get_gameObject();
			}
			if (this._dialogBg == null)
			{
				this._dialogBg = this._dialogObj.get_transform().FindChild("bg/dialog_window").GetComponent<UITexture>();
			}
			if (this._maskBg == null)
			{
				this._maskBg = base.get_transform().FindChild("DockOverlayBtn/Background").GetComponent<UISprite>();
			}
			if (this._keyLabel_b == null)
			{
				this._keyLabel_b = this._dialogObj.get_transform().FindChild("Text_b").GetComponent<UILabel>();
			}
			if (this._keyLabel_a == null)
			{
				this._keyLabel_a = this._dialogObj.get_transform().FindChild("Text_a").GetComponent<UILabel>();
			}
			if (this._yesBtn == null)
			{
				this._yesBtn = this._dialogObj.get_transform().FindChild("YesBtn").GetComponent<UISprite>();
			}
			if (this._noBtn == null)
			{
				this._noBtn = this._dialogObj.get_transform().FindChild("NoBtn").GetComponent<UISprite>();
			}
			if (this._openInfoAnim == null)
			{
				this._openInfoAnim = base.get_transform().GetComponent<Animation>();
			}
			UIButtonMessage component = this._yesBtn.GetComponent<UIButtonMessage>();
			component.target = base.get_gameObject();
			component.functionName = "OnYesButtonEL";
			component.trigger = UIButtonMessage.Trigger.OnClick;
			UIButtonMessage component2 = this._noBtn.GetComponent<UIButtonMessage>();
			component2.target = base.get_gameObject();
			component2.functionName = "OnNoButtonEL";
			component2.trigger = UIButtonMessage.Trigger.OnClick;
			UIButtonMessage component3 = this._maskBg.GetComponent<UIButtonMessage>();
			component3.target = base.get_gameObject();
			component3.functionName = "_onClickOverlayButton";
			component3.trigger = UIButtonMessage.Trigger.OnClick;
			base.GetComponent<UIPanel>().alpha = 0f;
		}

		public void showDialog(int num)
		{
			this.IsShow = true;
			this.Index = 0;
			this._dockIndex = num;
			base.GetComponent<UIPanel>().alpha = 1f;
			this.updateDialogBtn(0);
			TaskMainArsenalManager.IsControl = false;
			this._dialogObj.get_transform().set_localScale(Vector3.get_zero());
			this._dialogObj.get_transform().set_localPosition(Vector3.get_zero());
			this._maskBg.get_transform().set_localPosition(Vector3.get_zero());
			ArsenalTaskManager.GetDialogPopUp().Open(this._dialogObj, 0f, 0f, 1f, 1f);
			this._maskBg.SafeGetTweenAlpha(0f, 0.5f, 0.5f, 0f, UITweener.Method.Linear, UITweener.Style.Once, base.get_gameObject(), "compShowDialog");
			int numOfKeyPossessions = TaskMainArsenalManager.arsenalManager.NumOfKeyPossessions;
			this._keyLabel_b.text = numOfKeyPossessions.ToString();
			this._keyLabel_a.text = (numOfKeyPossessions - 1).ToString();
		}

		public void compShowDialog()
		{
			Debug.Log("compShowDialog");
			TaskMainArsenalManager.IsControl = true;
		}

		public void updateDialogBtn(int num)
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

		public void hideDialog()
		{
			if (!this.IsShow)
			{
				return;
			}
			this.IsShow = false;
			this._maskBg.SafeGetTweenAlpha(0.5f, 0f, 0.5f, 0f, UITweener.Method.Linear, UITweener.Style.Once, base.get_gameObject(), "_compHideDialog");
			BaseDialogPopup.Close(this._dialogObj, 0.5f, UITweener.Method.Linear);
			SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
			ArsenalTaskManager._clsArsenal.hideDockOpenDialog();
		}

		private void _compHideDialog()
		{
			if (this.Index == 1)
			{
				this._onOpenInfoAnimationFinished();
			}
			else
			{
				this._openInfoAnim.Play();
			}
		}

		public void OnYesButtonEL(GameObject obj)
		{
			if (!TaskMainArsenalManager.IsControl)
			{
				return;
			}
			Debug.Log("OnYesButtonEL:" + this.Index);
			TaskMainArsenalManager.IsControl = false;
			this.updateDialogBtn(0);
			TaskMainArsenalManager.dockMamager[this._dockIndex].StartDockOpen();
			if (this._dockIndex + 1 < TaskMainArsenalManager.dockMamager.Length)
			{
				TaskMainArsenalManager.dockMamager[this._dockIndex + 1].ShowKeyLock();
			}
			this.hideDialog();
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
		}

		public void OnNoButtonEL(GameObject obj)
		{
			if (!TaskMainArsenalManager.IsControl)
			{
				return;
			}
			TaskMainArsenalManager.IsControl = false;
			this.updateDialogBtn(1);
			this.hideDialog();
		}

		private void _onOpenInfoAnimationFinished()
		{
			base.GetComponent<UIPanel>().alpha = 0f;
			TaskMainArsenalManager.IsControl = true;
		}

		private void _onClickOverlayButton()
		{
			if (TaskMainArsenalManager.IsControl)
			{
				Debug.Log("_onClickOverlayButton");
				this.OnNoButtonEL(null);
			}
		}

		private void OnDestroy()
		{
			this._dialogObj = null;
			this._dialogBg = null;
			this._maskBg = null;
			this._keyLabel_b = null;
			this._keyLabel_a = null;
			this._yesBtn = null;
			this._noBtn = null;
			this._openInfoAnim = null;
		}
	}
}
