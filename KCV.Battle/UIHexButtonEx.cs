using KCV.Battle.Utils;
using System;
using UnityEngine;

namespace KCV.Battle
{
	[RequireComponent(typeof(BoxCollider2D)), RequireComponent(typeof(Animation)), RequireComponent(typeof(UIToggle))]
	public class UIHexButtonEx : MonoBehaviour
	{
		public enum AnimationList
		{
			HexButtonShow,
			HexButtonSelect
		}

		[SerializeField]
		protected UISprite _uiHexSprite;

		[SerializeField]
		protected UISprite _uiHexSelector;

		[SerializeField]
		protected Transform _traForeground;

		[SerializeField]
		protected UIWidget _uiWidgetLabel;

		protected int _nIndex;

		protected int _nSpriteIndex;

		protected bool _isFocus;

		protected UIToggle _uiToggle;

		protected BoxCollider2D _colBoxCollider2D;

		protected Animation _anim;

		protected UIHexButtonEx.AnimationList _iList;

		protected Action _actCallback;

		protected DelDecideHexButtonEx _delDecideAdvancingWithdrawalButtonEx;

		public virtual int index
		{
			get
			{
				return this._nIndex;
			}
			private set
			{
				this._nIndex = value;
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
				this._nSpriteIndex = Mathe.MinMax2(value, 0, 16);
				this._uiHexSprite.spriteName = string.Format("hex_{0:D5}", this._nSpriteIndex + 4);
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
						this.toggle.Set(true);
						this.SetForeground();
						this.PlayFocusAnimation();
					}
				}
				else if (this._isFocus)
				{
					this._isFocus = false;
					this.toggle.Set(false);
					this.SetForeground();
					this.StopFocusAnimatiom();
				}
			}
		}

		public virtual UIToggle toggle
		{
			get
			{
				if (this._uiToggle == null)
				{
					this._uiToggle = base.GetComponent<UIToggle>();
				}
				return this._uiToggle;
			}
		}

		public virtual BoxCollider2D collider2D
		{
			get
			{
				if (this._colBoxCollider2D == null)
				{
					this._colBoxCollider2D = base.GetComponent<BoxCollider2D>();
				}
				return this._colBoxCollider2D;
			}
		}

		public virtual bool isColliderEnabled
		{
			get
			{
				return this.collider2D.get_enabled();
			}
			set
			{
				this.collider2D.set_enabled(value);
			}
		}

		public virtual int toggleGroup
		{
			get
			{
				return this.toggle.group;
			}
			private set
			{
				this.toggle.group = value;
			}
		}

		public virtual Animation animation
		{
			get
			{
				if (this._anim == null)
				{
					this._anim = base.GetComponent<Animation>();
				}
				return this._anim;
			}
		}

		private void Awake()
		{
			this._nIndex = 0;
			this._nSpriteIndex = 0;
			this.isFocus = false;
			this._actCallback = null;
			this._delDecideAdvancingWithdrawalButtonEx = null;
			this.toggle.startsActive = false;
			this.toggle.validator = new UIToggle.Validate(this.OnValidator);
			this.toggle.onDecide = new Action(this.OnDecide);
			this._uiHexSelector.alpha = 0.01f;
			this.spriteIndex = 0;
			this.toggle.Set(false);
			this.SetForeground();
			this.StopFocusAnimatiom();
			base.get_transform().localScaleZero();
			this.toggle.onDecide = new Action(this.OnDecide);
		}

		private void OnDestroy()
		{
			this.OnUnInit();
			Mem.Del(ref this._uiHexSprite);
			Mem.Del(ref this._uiHexSelector);
			Mem.Del<Transform>(ref this._traForeground);
			Mem.Del<int>(ref this._nIndex);
			Mem.Del<int>(ref this._nSpriteIndex);
			Mem.Del<bool>(ref this._isFocus);
			Mem.Del<UIToggle>(ref this._uiToggle);
			Mem.Del<BoxCollider2D>(ref this._colBoxCollider2D);
			Mem.Del<Animation>(ref this._anim);
			Mem.Del<Action>(ref this._actCallback);
			Mem.Del<DelDecideHexButtonEx>(ref this._delDecideAdvancingWithdrawalButtonEx);
		}

		public bool Init(int nIndex, bool isColliderEnabled, int nGroup, DelDecideHexButtonEx decideDelegate)
		{
			this.index = nIndex;
			this.isColliderEnabled = isColliderEnabled;
			this.toggleGroup = nGroup;
			this._delDecideAdvancingWithdrawalButtonEx = decideDelegate;
			this.OnInit();
			return true;
		}

		public bool Init(int nIndex, bool isColliderEnabled, int nGroup, Action onDecide)
		{
			this.index = nIndex;
			this.isColliderEnabled = isColliderEnabled;
			this.toggleGroup = nGroup;
			this.toggle.onDecide = onDecide;
			this.OnInit();
			return true;
		}

		protected virtual void OnInit()
		{
		}

		protected virtual void OnUnInit()
		{
		}

		public void Play(Action callback)
		{
			base.get_transform().localScaleOne();
			this.isFocus = false;
			this._actCallback = callback;
			this._uiHexSprite.color = Color.get_white();
			this.animation.Play(UIHexButtonEx.AnimationList.HexButtonShow.ToString());
		}

		protected virtual void SetForeground()
		{
			UILabel component = this._traForeground.GetComponent<UILabel>();
			if (component != null)
			{
				component.color = ((!this.toggle.value) ? Color.get_gray() : Color.get_white());
			}
			if (this._uiWidgetLabel != null)
			{
				this._uiWidgetLabel.color = ((!this.toggle.value) ? Color.get_gray() : Color.get_white());
			}
		}

		private void PlayFocusAnimation()
		{
			this.animation.set_wrapMode(2);
			this._uiHexSelector.SetActive(true);
			this.animation.Play(UIHexButtonEx.AnimationList.HexButtonSelect.ToString());
		}

		private void StopFocusAnimatiom()
		{
			this._uiHexSelector.SetActive(false);
			this.animation.Stop();
			this.animation.set_wrapMode(0);
		}

		private void ChangeSprite(int nIndex)
		{
			this.spriteIndex = nIndex;
		}

		private void onAnimationFinished()
		{
			this.spriteIndex = 16;
			this._uiWidgetLabel.alpha = 1f;
			this._uiHexSprite.color = Color.get_gray();
			this._isFocus = false;
			this.SetForeground();
			if (this._actCallback != null)
			{
				this._actCallback.Invoke();
			}
		}

		private bool OnValidator(bool isChoice)
		{
			this.isFocus = isChoice;
			return true;
		}

		public void OnDecide()
		{
			Dlg.Call(ref this.toggle.onDecide);
			if (this._delDecideAdvancingWithdrawalButtonEx != null)
			{
				this._delDecideAdvancingWithdrawalButtonEx(this);
			}
		}
	}
}
