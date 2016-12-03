using System;
using UnityEngine;

namespace KCV.Port.record
{
	public class RecordMiniCharacter : MonoBehaviour
	{
		[SerializeField]
		private UIButton _uiBtn;

		[SerializeField]
		private UISprite _uiCharacter1;

		[SerializeField]
		private Animation _anim;

		private bool _isNormal;

		private bool isControl;

		private void Awake()
		{
			this._isNormal = true;
			this.isControl = true;
			if (this._uiBtn == null)
			{
				this._uiBtn = base.GetComponent<UIButton>();
			}
			EventDelegate.Add(this._uiBtn.onClick, new EventDelegate.Callback(this.onMiniCharacterEL));
			Util.FindParentToChild<UISprite>(ref this._uiCharacter1, base.get_transform(), "Character");
			if (this._anim == null)
			{
				this._anim = base.GetComponent<Animation>();
			}
			this._anim.Stop();
			this._anim.Play("miniCharacter_wait1");
		}

		private void OnDestroy()
		{
			Mem.Del<UIButton>(ref this._uiBtn);
			Mem.Del(ref this._uiCharacter1);
			Mem.Del<Animation>(ref this._anim);
		}

		private void onMiniCharacterEL()
		{
			if (!this.isControl)
			{
				return;
			}
			this.isControl = false;
			this._isNormal = !this._isNormal;
			this._uiCharacter1.spriteName = ((!this._isNormal) ? "m_2" : "m_1");
			this._anim.Stop();
			this._anim.Play("miniCharacter_up1");
		}

		private void compAnimation()
		{
			this.isControl = true;
			this._anim.Stop();
			this._anim.Play("miniCharacter_wait1");
		}

		private void compWaitAnimation()
		{
			int iLim = XorRandom.GetILim(1, 100);
			this._anim.Stop();
			if (iLim >= 75)
			{
				this._anim.Play("miniCharacter_wait1");
			}
			else
			{
				this._anim.Play("miniCharacter_wait2");
			}
		}
	}
}
