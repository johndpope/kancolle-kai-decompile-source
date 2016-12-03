using KCV.Battle.Utils;
using System;
using UnityEngine;

namespace KCV.Battle
{
	public class UIWithdrawalButton : UIHexButtonEx
	{
		[SerializeField]
		private UISprite _uiLabelSprite;

		protected override void OnInit()
		{
			WithdrawalDecisionType index = (WithdrawalDecisionType)this.index;
			if (index != WithdrawalDecisionType.Withdrawal)
			{
				if (index == WithdrawalDecisionType.Chase)
				{
					this._uiLabelSprite.spriteName = "txt_yasen_off";
				}
			}
			else
			{
				this._uiLabelSprite.spriteName = "txt_return_off";
			}
		}

		protected override void OnUnInit()
		{
			Mem.Del(ref this._uiLabelSprite);
		}

		protected override void SetForeground()
		{
			WithdrawalDecisionType index = (WithdrawalDecisionType)this.index;
			if (index != WithdrawalDecisionType.Withdrawal)
			{
				if (index == WithdrawalDecisionType.Chase)
				{
					this._uiLabelSprite.spriteName = ((!this.toggle.value) ? "txt_yasen_off" : "txt_yasen_on");
				}
			}
			else
			{
				this._uiLabelSprite.spriteName = ((!this.toggle.value) ? "txt_return_off" : "txt_return_on");
			}
		}
	}
}
