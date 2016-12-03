using Common.Enum;
using System;
using UnityEngine;

namespace KCV.BattleCut
{
	[RequireComponent(typeof(UILabel))]
	public class UICommandLabelButton : UILabelButton
	{
		private BattleCommand _iBattleCommand;

		private Func<bool> _actOnSetCommand;

		public BattleCommand battleCommand
		{
			get
			{
				return this._iBattleCommand;
			}
			private set
			{
				this._iBattleCommand = value;
			}
		}

		public static UICommandLabelButton Instantiate(UICommandLabelButton prefab, Transform parent, Vector3 pos, int nIndex, BattleCommand iCommand, Func<bool> onSetCommand)
		{
			UICommandLabelButton uICommandLabelButton = Object.Instantiate<UICommandLabelButton>(prefab);
			uICommandLabelButton.get_transform().set_parent(parent);
			uICommandLabelButton.get_transform().localScaleOne();
			uICommandLabelButton.get_transform().set_localPosition(pos);
			uICommandLabelButton.Init(nIndex, true, iCommand, onSetCommand);
			return uICommandLabelButton;
		}

		public bool Init(int nIndex, bool isValid, BattleCommand iCommand, Func<bool> onSetCommand)
		{
			this._actOnSetCommand = onSetCommand;
			this._iBattleCommand = iCommand;
			base.Init(nIndex, isValid);
			this.SetLabel(nIndex, iCommand);
			return true;
		}

		private void SetLabel(int nIndex, BattleCommand iCommand)
		{
			string text = string.Format("{0}.{1}", nIndex + 1, iCommand.GetString());
			UILabel arg_38_0 = base.background.GetComponent<UILabel>();
			string text2 = text;
			base.foreground.GetComponent<UILabel>().text = text2;
			arg_38_0.text = text2;
		}

		public void SetCommand(BattleCommand iCommand)
		{
			if (this._iBattleCommand == iCommand)
			{
				return;
			}
			this._iBattleCommand = iCommand;
			this.SetLabel(this.index, this._iBattleCommand);
			if (this._actOnSetCommand != null)
			{
				this._actOnSetCommand.Invoke();
			}
		}
	}
}
