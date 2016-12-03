using Common.Enum;
using LT.Tweening;
using System;
using UnityEngine;

namespace KCV.Battle
{
	[RequireComponent(typeof(UISprite))]
	public class UISelectCommandInfo : MonoBehaviour
	{
		[SerializeField]
		private UISprite _uiIcon;

		[SerializeField]
		private UISprite _uiText;

		private UISprite _uiBackground;

		private BattleCommand _iCommand = BattleCommand.None;

		private UISprite background
		{
			get
			{
				return this.GetComponentThis(ref this._uiBackground);
			}
		}

		public BattleCommand command
		{
			get
			{
				return this._iCommand;
			}
			private set
			{
				this._iCommand = value;
				this._uiIcon.spriteName = string.Format("command_{0}_icon", (int)value);
				this._uiText.spriteName = string.Format("command_{0}_txt", (int)value);
				this._uiIcon.MakePixelPerfect();
				this._uiText.MakePixelPerfect();
				if (this._iCommand != BattleCommand.None)
				{
					this._uiText.alpha = 0f;
					this._uiIcon.alpha = 0f;
					this._uiText.get_transform().LTCancel();
					this._uiText.get_transform().LTValue(this._uiText.alpha, 1f, 0.2f).setEase(LeanTweenType.easeOutSine).setOnUpdate(delegate(float x)
					{
						this._uiText.alpha = x;
						this._uiIcon.alpha = x;
					});
				}
				else
				{
					this._uiText.get_transform().LTCancel();
					this._uiText.get_transform().LTValue(this._uiText.alpha, 0f, 0.2f).setEase(LeanTweenType.easeOutSine).setOnUpdate(delegate(float x)
					{
						this._uiText.alpha = x;
						this._uiIcon.alpha = x;
					});
				}
			}
		}

		private void OnDestroy()
		{
			Mem.Del(ref this._uiIcon);
			Mem.Del(ref this._uiText);
			Mem.Del(ref this._uiBackground);
			Mem.Del<BattleCommand>(ref this._iCommand);
		}

		public void ClearInfo()
		{
			this.command = BattleCommand.None;
		}

		public void SetInfo(BattleCommand iCommand)
		{
			this.command = iCommand;
		}
	}
}
