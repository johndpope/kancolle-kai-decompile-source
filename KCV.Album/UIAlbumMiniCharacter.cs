using System;
using UnityEngine;

namespace KCV.Album
{
	[RequireComponent(typeof(Animation))]
	public class UIAlbumMiniCharacter : MonoBehaviour
	{
		private UIButton _uiBtn;

		private UISprite _uiCharacter1;

		private UISprite _uiCharacter2;

		private UISprite _uiShadow;

		private Animation _anim;

		private Vector3 mVector3_MushiDefaultPosition;

		private Vector3 mVector3_MushiDefaultScale;

		private Vector3 mVector3_ShadowDefaultScale;

		private Quaternion mQuaternion_MushiDefaultRotation;

		private bool isControl;

		private void OnEnable()
		{
			this._uiCharacter2.get_transform().set_localScale(this.mVector3_MushiDefaultScale);
			this._uiCharacter2.get_transform().set_localPosition(this.mVector3_MushiDefaultPosition);
			this._uiCharacter2.get_transform().set_localRotation(this.mQuaternion_MushiDefaultRotation);
			this._uiShadow.get_transform().set_localScale(this.mVector3_ShadowDefaultScale);
			this._uiShadow.alpha = 1f;
			this.isControl = true;
			this._anim.Stop();
			this._anim.Play("AlbumChara_Wait2");
		}

		private void OnDisable()
		{
			this.isControl = true;
			this._anim.Stop();
		}

		private void Awake()
		{
			if (this._uiBtn == null)
			{
				this._uiBtn = base.GetComponent<UIButton>();
			}
			EventDelegate.Add(this._uiBtn.onClick, new EventDelegate.Callback(this.onMiniCharacterEL));
			Util.FindParentToChild<UISprite>(ref this._uiCharacter1, base.get_transform(), "Character1");
			Util.FindParentToChild<UISprite>(ref this._uiCharacter2, base.get_transform(), "Character2");
			Util.FindParentToChild<UISprite>(ref this._uiShadow, base.get_transform(), "Shadow");
			this.mVector3_MushiDefaultPosition = this._uiCharacter2.get_transform().get_localPosition();
			this.mVector3_MushiDefaultScale = this._uiCharacter2.get_transform().get_localScale();
			this.mVector3_ShadowDefaultScale = this._uiShadow.get_transform().get_localScale();
			this.mQuaternion_MushiDefaultRotation = this._uiCharacter2.get_transform().get_localRotation();
			if (this._anim == null)
			{
				this._anim = base.GetComponent<Animation>();
			}
		}

		private void OnDestroy()
		{
			this._uiBtn = null;
			this._uiCharacter1 = null;
			this._uiCharacter2 = null;
			this._uiShadow = null;
			this._anim = null;
		}

		private void onMiniCharacterEL()
		{
			if (!this.isControl)
			{
				return;
			}
			this.isControl = false;
			this._anim.Stop();
			int iLim = XorRandom.GetILim(1, 100);
			if (iLim >= 85)
			{
				int iLim2 = XorRandom.GetILim(1, 5);
				if (iLim >= 3)
				{
					this._anim.Play("AlbumChara_Up1");
				}
				else
				{
					this._anim.Play("AlbumChara_Up2");
				}
			}
			else
			{
				int iLim3 = XorRandom.GetILim(1, 100);
				if (iLim >= 50)
				{
					this._anim.Play("AlbumChara_Normal1");
				}
				else if (iLim >= 20)
				{
					this._anim.Play("AlbumChara_Normal2");
				}
				else
				{
					this._anim.Play("AlbumChara_Normal3");
				}
			}
		}

		private void startInCharacterAnimate()
		{
		}

		private void animationFinished()
		{
			this._anim.Stop();
			int iLim = XorRandom.GetILim(1, 2);
			if (iLim == 1)
			{
				this._anim.Play("AlbumChara_In1");
			}
			else
			{
				this._anim.Play("AlbumChara_In2");
			}
		}

		private void compAnimation()
		{
			this.isControl = true;
			this._anim.Stop();
			this._anim.Play("AlbumChara_Wait2");
		}

		private void compWaitAnimation()
		{
			int iLim = XorRandom.GetILim(1, 100);
			this._anim.Stop();
			if (iLim >= 75)
			{
				this._anim.Play("AlbumChara_Wait3");
			}
			else
			{
				this._anim.Play("AlbumChara_Wait2");
			}
		}
	}
}
