using Sony.Vita.Dialog;
using System;
using UnityEngine;

namespace KCV.Startup
{
	[RequireComponent(typeof(UIPanel))]
	public class CtrlAdmiralNameInput : MonoBehaviour
	{
		[SerializeField]
		private UITexture _uiBackground;

		[SerializeField]
		private UIInput _uiNameInput;

		[SerializeField]
		private UILabel _uiTitle;

		[SerializeField]
		private Animation _animFeather;

		[SerializeField]
		private UIButton _uiDecideButton;

		private UIPanel _uiPanel;

		private string _strEditName;

		private Action _actOnCancel;

		private UIPanel panel
		{
			get
			{
				return this.GetComponentThis(ref this._uiPanel);
			}
		}

		public static CtrlAdmiralNameInput Instantiate(CtrlAdmiralNameInput prefab, Transform parent, Action onCancel)
		{
			CtrlAdmiralNameInput ctrlAdmiralNameInput = Object.Instantiate<CtrlAdmiralNameInput>(prefab);
			ctrlAdmiralNameInput.Init(onCancel);
			return ctrlAdmiralNameInput;
		}

		private void OnDestroy()
		{
			base.get_transform().GetComponentsInChildren<UIWidget>().ForEach(delegate(UIWidget x)
			{
				if (x is UISprite)
				{
					((UISprite)x).Clear();
				}
				Mem.Del<UIWidget>(ref x);
			});
		}

		private bool Init(Action onCancel)
		{
			this._actOnCancel = onCancel;
			this._strEditName = string.Empty;
			Ime.add_OnGotIMEDialogResult(new Messages.EventHandler(this.OnGotIMEDialogResult));
			Main.Initialise();
			return true;
		}

		public bool UnInit()
		{
			Ime.remove_OnGotIMEDialogResult(new Messages.EventHandler(this.OnGotIMEDialogResult));
			return true;
		}

		public bool Run()
		{
			KeyControl keyControl = StartupTaskManager.GetKeyControl();
			Main.Update();
			if (keyControl.GetDown(KeyControl.KeyName.SELECT))
			{
				this.OnNameSubmit();
				return true;
			}
			if (keyControl.GetDown(KeyControl.KeyName.MARU))
			{
				if (this._uiNameInput.value == string.Empty || this._uiNameInput.value.Replace(" ", string.Empty).Replace("\u3000", string.Empty) == string.Empty)
				{
					this._strEditName = string.Empty;
					this._uiNameInput.value = string.Empty;
					this.OnClickInputLabel();
				}
				else if (Utils.ChkNGWard(this._uiNameInput.value))
				{
					this._uiNameInput.value = string.Empty;
					this._animFeather.Play();
					this.OnClickInputLabel();
				}
				else
				{
					this._uiNameInput.isSelected = false;
					this.OnNameSubmit();
				}
			}
			else if (keyControl.GetDown(KeyControl.KeyName.BATU))
			{
				Dlg.Call(ref this._actOnCancel);
			}
			return true;
		}

		private void OnGotIMEDialogResult(Messages.PluginMessage msg)
		{
			Ime.ImeDialogResult result = Ime.GetResult();
			if (result.result == null)
			{
				string text = result.get_text();
				if (Utils.ChkNGWard(text) || Utils.ChkNGWard(result.get_text()))
				{
					text = string.Empty;
					this._animFeather.Play();
				}
				this._strEditName = text;
				this._uiNameInput.value = this._strEditName;
			}
		}

		private void OnNameSubmit()
		{
		}

		public void OnClickInputLabel()
		{
			if (!this._uiNameInput.isSelected)
			{
				return;
			}
			Ime.ImeDialogParams imeDialogParams = new Ime.ImeDialogParams();
			imeDialogParams.supportedLanguages = 270336;
			imeDialogParams.languagesForced = true;
			imeDialogParams.type = 0;
			imeDialogParams.option = 0;
			imeDialogParams.canCancel = true;
			imeDialogParams.textBoxMode = 2;
			imeDialogParams.enterLabel = 0;
			imeDialogParams.maxTextLength = 12;
			imeDialogParams.set_title("提督名を入力してください。（" + 12 + "文字まで）");
			imeDialogParams.set_initialText(this._strEditName);
			Ime.Open(imeDialogParams);
		}
	}
}
