using LT.Tweening;
using System;
using UnityEngine;

namespace KCV
{
	public class UIShortCutSwitch : MonoBehaviour
	{
		[Serializable]
		private struct Param
		{
			public float showPosX;

			public float hidePosX;

			public float moveTime;
		}

		[SerializeField]
		private UITexture _uiBackground;

		[SerializeField]
		private UITexture _uiGear;

		[SerializeField]
		private UILabel _uiStatus;

		[SerializeField]
		private UIShortCutSwitch.Param _strAnimParam;

		private bool _isShortCut;

		private bool _isValid;

		private Color _colHalfRed = new Color(1f, 0f, 0f, 0.5f);

		private Color _colHalfBlack = new Color(0f, 0f, 0f, 0.5f);

		public bool isShortCut
		{
			get
			{
				return this._isShortCut;
			}
			private set
			{
				this._isShortCut = value;
			}
		}

		public bool isValid
		{
			get
			{
				return this._isValid;
			}
			private set
			{
				this._isValid = value;
			}
		}

		private void Awake()
		{
			this._isShortCut = false;
			base.get_transform().localPositionX(this._strAnimParam.hidePosX);
		}

		private void OnDestroy()
		{
			Mem.Del<UITexture>(ref this._uiBackground);
			Mem.Del<UITexture>(ref this._uiGear);
			Mem.Del<UILabel>(ref this._uiStatus);
			Mem.Del<UIShortCutSwitch.Param>(ref this._strAnimParam);
			Mem.Del<bool>(ref this._isShortCut);
			Mem.Del<Color>(ref this._colHalfRed);
			Mem.Del<Color>(ref this._colHalfBlack);
		}

		private void Update()
		{
			if (Input.GetKeyDown(49))
			{
				this.SetIsValid(true, true);
			}
			else if (Input.GetKeyDown(50))
			{
				this.SetIsValid(false, true);
			}
		}

		public void SetDefault(bool isShortCut)
		{
			this.isShortCut = isShortCut;
			base.get_transform().localPositionX((!isShortCut) ? this._strAnimParam.hidePosX : this._strAnimParam.showPosX);
			this.SetIsValid(true, false);
		}

		public void SetIsValid(bool isValid, bool isAnimation)
		{
			this._isValid = isValid;
			this._uiStatus.text = ((!isValid) ? "決戦！ショートカット不可" : "戦闘ショートカット");
			if (isAnimation)
			{
				this._uiBackground.get_transform().LTCancel();
				this._uiBackground.get_transform().LTValue(this._uiBackground.color, (!isValid) ? this._colHalfRed : this._colHalfBlack, 0.25f).setEase(LeanTweenType.linear).setOnUpdate(delegate(Color x)
				{
					this._uiBackground.color = x;
				});
			}
			else
			{
				this._uiBackground.color = ((!isValid) ? this._colHalfRed : this._colHalfBlack);
			}
		}

		public void Switch()
		{
			if (this.isShortCut)
			{
				this.Hide();
			}
			else
			{
				this.Show();
			}
		}

		public LTDescr Show()
		{
			this.isShortCut = true;
			base.get_transform().LTCancel();
			return base.get_transform().LTMoveLocalX(this._strAnimParam.showPosX, this._strAnimParam.moveTime).setEase(LeanTweenType.easeOutQuad);
		}

		public LTDescr Hide()
		{
			this.isShortCut = false;
			base.get_transform().LTCancel();
			return base.get_transform().LTMoveLocalX(this._strAnimParam.hidePosX, this._strAnimParam.moveTime).setEase(LeanTweenType.easeOutQuad);
		}
	}
}
