using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle
{
	[RequireComponent(typeof(Animation)), RequireComponent(typeof(UIButton)), RequireComponent(typeof(BoxCollider2D))]
	public class UIHexButton : BaseAnimation
	{
		public enum AnimationList
		{
			HexButtonShow,
			ProdTranscendenceAttackHex,
			HexButtonSelect
		}

		protected UISprite _uiHexBtn;

		protected List<UIButton> _listBtns;

		protected UIToggle _toggle;

		protected int _nIndex;

		protected int _nSpriteIndex;

		protected bool _isFocus;

		protected string[] _strSpriteNames;

		protected UIHexButton.AnimationList _iList;

		public virtual int index
		{
			get
			{
				return this._nIndex;
			}
		}

		public virtual int spriteIndex
		{
			get
			{
				return this._nSpriteIndex;
			}
			set
			{
				this._nSpriteIndex = value;
				this._uiHexBtn.spriteName = this._strSpriteNames[this._nSpriteIndex];
			}
		}

		public virtual bool isFocus
		{
			get
			{
				return this._isFocus;
			}
			set
			{
				if (value)
				{
					if (!this._isFocus)
					{
						this._isFocus = true;
						this.SetButtonState(UIButtonColor.State.Pressed);
					}
				}
				else if (this._isFocus)
				{
					this._isFocus = false;
					this.SetButtonState(UIButtonColor.State.Normal);
				}
			}
		}

		public bool isColliderEnabled
		{
			get
			{
				return base.GetComponent<Collider2D>().get_enabled();
			}
			set
			{
				base.GetComponent<Collider2D>().set_enabled(value);
			}
		}

		public virtual UIHexButton.AnimationList list
		{
			get
			{
				return this._iList;
			}
		}

		public virtual UIButton uiButton
		{
			get
			{
				return this._listBtns.get_Item(0);
			}
		}

		public virtual List<UIButton> buttonList
		{
			get
			{
				return this._listBtns;
			}
		}

		protected virtual void Awake()
		{
			base.Awake();
			this.Init();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Mem.Del(ref this._uiHexBtn);
			Mem.DelListSafe<UIButton>(ref this._listBtns);
			Mem.Del<UIToggle>(ref this._toggle);
			Mem.Del<int>(ref this._nIndex);
			Mem.Del<int>(ref this._nSpriteIndex);
			Mem.Del<bool>(ref this._isFocus);
			Mem.DelArySafe<string>(ref this._strSpriteNames);
			Mem.Del<UIHexButton.AnimationList>(ref this._iList);
		}

		public virtual bool Init()
		{
			this._uiHexBtn = base.get_transform().FindChild("HexBtn").GetComponent<UISprite>();
			this._uiHexBtn.spriteName = string.Empty;
			this._toggle = base.get_transform().GetComponent<UIToggle>();
			this._listBtns = new List<UIButton>();
			this._listBtns.AddRange(base.GetComponents<UIButton>());
			this._animAnimation = base.GetComponent<Animation>();
			this._animAnimation.Stop();
			this._nSpriteIndex = 0;
			this._strSpriteNames = new string[16];
			for (int i = 0; i < 16; i++)
			{
				string text = string.Format("{0:D2}", i + 4);
				this._strSpriteNames[i] = "hex_000" + text;
			}
			this.SetActive(false);
			this.isFocus = true;
			return true;
		}

		public virtual bool Run()
		{
			if (!this.isFocus && this.uiButton.state != UIButtonColor.State.Normal)
			{
				using (List<UIButton>.Enumerator enumerator = this._listBtns.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						UIButton current = enumerator.get_Current();
						if (current.state != UIButtonColor.State.Normal)
						{
							current.state = UIButtonColor.State.Normal;
						}
					}
				}
			}
			return true;
		}

		public virtual void Play(UIHexButton.AnimationList iList, Action callback)
		{
			this._iList = iList;
			this._isFinished = false;
			this._actCallback = callback;
			this._nSpriteIndex = 0;
			this._uiHexBtn.spriteName = this._strSpriteNames[this._nSpriteIndex];
			this.SetActive(true);
			this._animAnimation.Play(iList.ToString());
		}

		public void SetFocusAnimation()
		{
			this._animAnimation.Stop();
			this._animAnimation.set_wrapMode(0);
			UISprite component = base.get_transform().FindChild("HexSelect").GetComponent<UISprite>();
			component.alpha = 0.03f;
			this._toggle.startsActive = false;
			if (this.isFocus)
			{
				this._toggle.startsActive = true;
				this._animAnimation.set_wrapMode(2);
				this._iList = UIHexButton.AnimationList.HexButtonSelect;
				this._animAnimation.Play(this._iList.ToString());
			}
		}

		public virtual void SetIndex(int nIndex)
		{
			this._nIndex = nIndex;
		}

		public virtual void SetDepth(int nDepth)
		{
		}

		protected virtual void SetButtonState(UIButtonColor.State iState)
		{
			using (List<UIButton>.Enumerator enumerator = this._listBtns.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					UIButton current = enumerator.get_Current();
					if (current.state != iState)
					{
						current.state = iState;
					}
				}
			}
		}

		public void ChangeSprite(int nIndex)
		{
			if (this._strSpriteNames[nIndex] != string.Empty)
			{
				this._nSpriteIndex = nIndex;
				this._uiHexBtn.spriteName = this._strSpriteNames[this._nSpriteIndex];
			}
		}

		protected override void onAnimationFinished()
		{
			base.onAnimationFinished();
			this.isColliderEnabled = true;
		}
	}
}
