using KCV.Utils;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV
{
	public class CommonDialog : MonoBehaviour
	{
		private UIPanel myPanel;

		[SerializeField]
		private DialogAnimation dialogAnimation;

		public GameObject[] dialogMessages;

		[SerializeField]
		private BoxCollider2D BackCollider;

		public KeyControl keyController;

		[SerializeField]
		private Blur CameraBlur;

		[SerializeField]
		private CommonDialogMessage CommonMessage;

		private IEnumerator ienum;

		public bool isUseDefaultKeyController;

		[SerializeField]
		private GameObject[] Children;

		public Action ShikakuButtonAction;

		public Action BatuButtonAction;

		[Button("debugShow", "show", new object[]
		{

		})]
		public int button;

		public int showNo;

		public bool isOpen
		{
			get;
			private set;
		}

		private void Awake()
		{
			this.myPanel = base.GetComponent<UIPanel>();
			this.myPanel.alpha = 1f;
			this.isUseDefaultKeyController = true;
			if (this.CameraBlur != null)
			{
				this.CameraBlur.set_enabled(false);
			}
			if (this.CommonMessage != null)
			{
				this.CommonMessage.SetActive(false);
			}
			this.setActiveChildren(false);
		}

		private void Update()
		{
			if (this.keyController == null)
			{
				return;
			}
			if (this.keyController.IsRun)
			{
				this.keyController.Update();
				if (this.ShikakuButtonAction != null && this.keyController.IsShikakuDown())
				{
					this.ShikakuButtonAction.Invoke();
					this.ShikakuButtonAction = null;
					SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
					this.CloseDialog();
				}
				else if (this.BatuButtonAction != null && this.keyController.IsBatuDown())
				{
					this.BatuButtonAction.Invoke();
					this.BatuButtonAction = null;
					SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
					this.CloseDialog();
				}
				else if ((this.keyController.IsMaruDown() || this.keyController.IsBatuDown()) && this.ShikakuButtonAction == null)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
					this.CloseDialog();
				}
			}
		}

		public void OpenDialog(int ShowMessageNo, DialogAnimation.AnimType type = DialogAnimation.AnimType.POPUP)
		{
			for (int i = 0; i < this.dialogMessages.Length; i++)
			{
				bool active = i == ShowMessageNo;
				this.dialogMessages[i].SetActive(active);
			}
			this.OpenDialog(type);
		}

		private void OpenDialog(DialogAnimation.AnimType type)
		{
			this.setActiveChildren(true);
			if (this.ienum != null)
			{
				base.StopCoroutine(this.ienum);
			}
			this.keyController = new KeyControl(0, 0, 0.4f, 0.1f);
			this.keyController.IsRun = false;
			this.myPanel.alpha = 1f;
			if (this.isUseDefaultKeyController)
			{
				this.keyController.IsRun = true;
				App.OnlyController = this.keyController;
				App.OnlyController.ClearKeyAll();
				this.keyController.firstUpdate = true;
			}
			this.dialogAnimation.StartAnim(type, true);
			if (this.CameraBlur != null)
			{
				this.CameraBlur.set_enabled(true);
			}
			this.isOpen = true;
		}

		public void OpenDialogWithDisableKeyControl()
		{
			this.myPanel.alpha = 1f;
			this.dialogAnimation.StartAnim(DialogAnimation.AnimType.POPUP, true);
			if (this.CameraBlur != null)
			{
				this.CameraBlur.set_enabled(true);
			}
		}

		public void CloseDialogWithDisabledKeyControl()
		{
			this.dialogAnimation.StartAnim(DialogAnimation.AnimType.FEAD, false);
			if (this.CameraBlur != null)
			{
				this.CameraBlur.set_enabled(false);
			}
			if (this.CommonMessage != null)
			{
				this.CommonMessage.SetActive(false);
			}
		}

		public void CloseDialog()
		{
			if (this.keyController != null && (this.keyController.IsRun || !this.isUseDefaultKeyController))
			{
				this.keyController.IsRun = false;
				this.keyController = null;
				App.OnlyController = null;
				App.isFirstUpdate = true;
				this.isUseDefaultKeyController = true;
				this.dialogAnimation.StartAnim(DialogAnimation.AnimType.FEAD, false);
				if (this.CameraBlur != null)
				{
					this.CameraBlur.set_enabled(false);
				}
				if (this.CommonMessage != null)
				{
					this.CommonMessage.SetActive(false);
				}
				for (int i = 0; i < this.dialogMessages.Length; i++)
				{
					this.dialogMessages[i].SetActive(false);
				}
				this.BackCollider.set_enabled(true);
				this.isOpen = false;
				this.ienum = this.CloseForEndDialogAnimation();
				base.StartCoroutine(this.ienum);
			}
		}

		public void disableBackTouch()
		{
			this.BackCollider.set_enabled(false);
		}

		public void setActiveChildren(bool isActive)
		{
			for (int i = 0; i < this.Children.Length; i++)
			{
				this.Children[i].SetActive(isActive);
			}
		}

		[DebuggerHidden]
		private IEnumerator CloseForEndDialogAnimation()
		{
			CommonDialog.<CloseForEndDialogAnimation>c__Iterator44 <CloseForEndDialogAnimation>c__Iterator = new CommonDialog.<CloseForEndDialogAnimation>c__Iterator44();
			<CloseForEndDialogAnimation>c__Iterator.<>f__this = this;
			return <CloseForEndDialogAnimation>c__Iterator;
		}

		[DebuggerHidden]
		public IEnumerator WaitForDialogClose()
		{
			CommonDialog.<WaitForDialogClose>c__Iterator45 <WaitForDialogClose>c__Iterator = new CommonDialog.<WaitForDialogClose>c__Iterator45();
			<WaitForDialogClose>c__Iterator.<>f__this = this;
			return <WaitForDialogClose>c__Iterator;
		}

		public void setOpenAction(Action act)
		{
			this.dialogAnimation.OpenAction = act;
		}

		public void setCloseAction(Action act)
		{
			this.dialogAnimation.CloseAction = act;
		}

		public void SetCameraBlur(Blur blur)
		{
			this.CameraBlur = blur;
		}

		private void debugShow()
		{
			this.OpenDialog(this.showNo, DialogAnimation.AnimType.POPUP);
		}

		private void OnDestroy()
		{
			if (this.ienum != null)
			{
				base.StopCoroutine(this.ienum);
			}
			this.ienum = null;
			this.myPanel = null;
			this.dialogAnimation = null;
			this.dialogMessages = null;
			this.BackCollider = null;
			this.keyController = null;
			this.CameraBlur = null;
			this.CommonMessage = null;
			this.Children = null;
			this.ShikakuButtonAction = null;
		}
	}
}
