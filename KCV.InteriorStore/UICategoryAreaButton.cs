using Common.Enum;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.InteriorStore
{
	public class UICategoryAreaButton : MonoBehaviour
	{
		private class Fairy
		{
			private UISprite _uiSprite;

			private bool _isEyeOpen;

			private float _fEyeInterval;

			private Dictionary<bool, Vector3> _dicFairyPos;

			public UISprite uiSprite
			{
				get
				{
					return this._uiSprite;
				}
			}

			public bool isEyeOpen
			{
				get
				{
					return this._isEyeOpen;
				}
			}

			public Fairy(Transform parent, string objName)
			{
				this._isEyeOpen = true;
				this._fEyeInterval = XorRandom.GetFLim(1f, 3f);
				Util.FindParentToChild<UISprite>(ref this._uiSprite, parent, objName);
				this._uiSprite.type = UIBasicSprite.Type.Filled;
				this._uiSprite.flip = UIBasicSprite.Flip.Nothing;
				this._uiSprite.fillDirection = UIBasicSprite.FillDirection.Vertical;
				this._uiSprite.fillAmount = 0.55f;
				this._uiSprite.invert = true;
				this._uiSprite.spriteName = "mini_08_a_01";
				this._dicFairyPos = new Dictionary<bool, Vector3>();
				this._dicFairyPos.Add(false, Vector3.get_down() * 20f);
				this._dicFairyPos.Add(true, Vector3.get_up() * 40f);
			}

			public bool Init()
			{
				return true;
			}

			public bool Reset()
			{
				this._isEyeOpen = true;
				this._fEyeInterval = XorRandom.GetFLim(1f, 3f);
				this._uiSprite.spriteName = "mini_08_a_01";
				return true;
			}

			public bool UnInit()
			{
				this._dicFairyPos.Clear();
				this._dicFairyPos = null;
				return true;
			}

			public void ShowYousei()
			{
				Vector3 vector = this._dicFairyPos.get_Item(true);
				float num = 1f;
				TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMove(this._uiSprite.get_transform(), vector, 0.25f, false), 18);
				TweenSettingsExtensions.SetEase<Tweener>(DOVirtual.Float(this._uiSprite.fillAmount, num, 0.1f, delegate(float amount)
				{
					this._uiSprite.fillAmount = amount;
				}), 18);
			}

			public void HideYousei()
			{
				Vector3 vector = this._dicFairyPos.get_Item(false);
				float num = 0.55f;
				TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMove(this._uiSprite.get_transform(), vector, 0.25f, false), 18);
				TweenSettingsExtensions.SetEase<Tweener>(DOVirtual.Float(this._uiSprite.fillAmount, num, 0.1f, delegate(float amount)
				{
					this._uiSprite.fillAmount = amount;
				}), 18);
			}

			public void Update(bool isDecide)
			{
				if (isDecide && this._fEyeInterval >= 0f && (this._uiSprite.spriteName == "mini_08_a_01" || this._uiSprite.spriteName == "mini_08_a_02"))
				{
					this._uiSprite.spriteName = ((!this._isEyeOpen) ? "mini_08_a_04" : "mini_08_a_03");
				}
				this._fEyeInterval -= Time.get_deltaTime();
				if (this._fEyeInterval <= 0f)
				{
					this._isEyeOpen = !this._isEyeOpen;
					if (isDecide)
					{
						this._uiSprite.spriteName = ((!this._isEyeOpen) ? "mini_08_a_04" : "mini_08_a_03");
					}
					else
					{
						this._uiSprite.spriteName = ((!this._isEyeOpen) ? "mini_08_a_02" : "mini_08_a_01");
					}
					this._fEyeInterval = ((!this._isEyeOpen) ? XorRandom.GetFLim(0.1f, 0.5f) : XorRandom.GetFLim(0.5f, 1.5f));
				}
			}
		}

		private DelDecideCategoryAreaBtn _delDecideCategoryAreaBtn;

		[SerializeField]
		private Generics.NextIndexInfos _clsNextInfos;

		private int _nIndex;

		private bool _isFocus;

		private bool _isDecide;

		private UICategoryAreaButton.Fairy _clsFairy;

		private List<UIButton> _listBtns;

		private FurnitureKinds _iKind;

		public Generics.NextIndexInfos nextIndexInfos
		{
			get
			{
				return this._clsNextInfos;
			}
		}

		public int index
		{
			get
			{
				return this._nIndex;
			}
		}

		public bool isFocus
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
						this.setBtnState(true);
						this._clsFairy.ShowYousei();
						this._isFocus = true;
					}
				}
				else if (this._isFocus)
				{
					this.setBtnState(false);
					this._clsFairy.HideYousei();
					this._isFocus = false;
				}
			}
		}

		public bool isDecide
		{
			get
			{
				return this._isDecide;
			}
			set
			{
				this._isDecide = value;
			}
		}

		public FurnitureKinds kind
		{
			get
			{
				return this._iKind;
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

		private void Awake()
		{
			if (this._clsNextInfos == null)
			{
				this._clsNextInfos = new Generics.NextIndexInfos();
			}
			this._isFocus = false;
			this._isDecide = false;
			this._listBtns = new List<UIButton>();
			this._listBtns.AddRange(base.GetComponents<UIButton>());
			this._listBtns.get_Item(0).onClick = Util.CreateEventDelegateList(this, "decideCategoryAreaBtn", null);
			this._clsFairy = new UICategoryAreaButton.Fairy(base.get_transform(), "Fairy");
		}

		private void OnDestroy()
		{
			this._delDecideCategoryAreaBtn = null;
			this._listBtns.Clear();
			this._clsFairy.UnInit();
		}

		private bool Update()
		{
			if (this._isFocus)
			{
				if (this._listBtns.get_Item(0).state == UIButtonColor.State.Normal)
				{
					this.setBtnState(true);
				}
				if (this._clsFairy != null)
				{
					this._clsFairy.Update(this._isDecide);
				}
			}
			return true;
		}

		public bool Init(int nIndex, FurnitureKinds iKind, DelDecideCategoryAreaBtn decideEvent)
		{
			this._nIndex = nIndex;
			this._iKind = iKind;
			this._delDecideCategoryAreaBtn = decideEvent;
			return true;
		}

		public bool Reset()
		{
			this._isFocus = false;
			this._isDecide = false;
			this._clsFairy.Reset();
			return true;
		}

		public int GetNextIndex(KeyControl.KeyName iName, int defVal)
		{
			return this._clsNextInfos.GetIndex(iName, defVal);
		}

		private void setBtnState(bool isFocus)
		{
			UIButtonColor.State state = (!isFocus) ? UIButtonColor.State.Normal : UIButtonColor.State.Pressed;
			using (List<UIButton>.Enumerator enumerator = this._listBtns.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					UIButton current = enumerator.get_Current();
					current.state = state;
				}
			}
		}

		private void decideCategoryAreaBtn()
		{
			if (this._delDecideCategoryAreaBtn != null)
			{
				this._delDecideCategoryAreaBtn(this);
			}
		}
	}
}
