using DG.Tweening;
using KCV.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Scene.Port
{
	[RequireComponent(typeof(UIWidget)), RequireComponent(typeof(UIButton))]
	public abstract class UIPortMenuButton : MonoBehaviour, UIButtonManager.UIButtonManagement
	{
		public interface CompositeMenu
		{
			UIPortMenuButton.UIPortMenuButtonKeyMap GetSubMenuKeyMap();
		}

		[Serializable]
		public class UIPortMenuButtonKeyMap
		{
			[SerializeField]
			public UIPortMenuButton mUIPortMenuButton_Top;

			[SerializeField]
			public UIPortMenuButton mUIPortMenuButton_Down;

			[SerializeField]
			public UIPortMenuButton mUIPortMenuButton_Left;

			[SerializeField]
			public UIPortMenuButton mUIPortMenuButton_Right;

			public void Release()
			{
				this.mUIPortMenuButton_Top = null;
				this.mUIPortMenuButton_Down = null;
				this.mUIPortMenuButton_Left = null;
				this.mUIPortMenuButton_Right = null;
			}
		}

		[SerializeField]
		protected UIPortMenuButton.UIPortMenuButtonKeyMap mUIPortMenuButtonKeyMap;

		protected UIWidget mWidgetThis;

		protected UIButton mButton_Action;

		[SerializeField]
		protected UITexture mTexture_Base;

		[SerializeField]
		protected UITexture mTexture_Glow_Back;

		[SerializeField]
		protected UITexture mTexture_Glow_Front;

		[SerializeField]
		protected UITexture mTexture_Front;

		[SerializeField]
		protected UITexture mTexture_TextShadow;

		[SerializeField]
		protected UITexture mTexture_Text;

		protected Vector3 mVector3_FrontCoverOutScale = new Vector3(0.95f, 0.95f);

		protected Vector3 mVector3_DefaultFrontScale = Vector3.get_one();

		private Vector3 mVector3_NormalLocalScale = Vector3.get_one();

		private Vector3 mVector3_DefaultLocalPosition;

		protected Vector3 mBackMinimum = new Vector3(0.94f, 0.94f);

		protected Vector3 mBackMaximum = new Vector3(1f, 1f);

		protected Vector3 mFrontMinimum = new Vector3(0.95f, 0.95f);

		protected Vector3 mFrontMaximum = new Vector3(0.98f, 0.98f);

		protected int mGlowBackWidth;

		protected int mGlowBackHeight;

		protected int mGlowFrontWidth;

		protected int mGlowFrontHeight;

		[SerializeField]
		private string mScene;

		private Action<UIPortMenuButton> mOnClickEventListener;

		public float alpha
		{
			get
			{
				if (this.mWidgetThis != null)
				{
					return this.mWidgetThis.alpha;
				}
				return -1f;
			}
			set
			{
				if (this.mWidgetThis != null)
				{
					this.mWidgetThis.alpha = value;
				}
			}
		}

		public bool IsSelectable
		{
			get;
			private set;
		}

		public UIPortMenuButton.UIPortMenuButtonKeyMap GetKeyMap()
		{
			return this.mUIPortMenuButtonKeyMap;
		}

		public Vector3 GetDefaultLocalPosition()
		{
			return this.mVector3_DefaultLocalPosition;
		}

		public UIButton GetButton()
		{
			return this.mButton_Action;
		}

		private void Start()
		{
			this.OnStart();
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mWidgetThis);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mButton_Action);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_Base, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_Glow_Back, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_Glow_Front, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_Front, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_TextShadow, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_Text, false);
			if (this.mUIPortMenuButtonKeyMap != null)
			{
				this.mUIPortMenuButtonKeyMap.Release();
			}
			this.mUIPortMenuButtonKeyMap = null;
			this.OnCallDestroy();
		}

		protected virtual void OnCallDestroy()
		{
		}

		private void Awake()
		{
			this.mWidgetThis = base.GetComponent<UIWidget>();
			this.mButton_Action = base.GetComponent<UIButton>();
			this.OnAwake();
		}

		public Generics.Scene GetScene()
		{
			string text = this.mScene;
			if (text != null)
			{
				if (UIPortMenuButton.<>f__switch$map15 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(14);
					dictionary.Add("Organize", 0);
					dictionary.Add("Remodel", 1);
					dictionary.Add("Arsenal", 2);
					dictionary.Add("Duty", 3);
					dictionary.Add("Repair", 4);
					dictionary.Add("Supply", 5);
					dictionary.Add("Strategy", 6);
					dictionary.Add("Record", 7);
					dictionary.Add("Album", 8);
					dictionary.Add("Item", 9);
					dictionary.Add("Option", 10);
					dictionary.Add("Interior", 11);
					dictionary.Add("SaveLoad", 12);
					dictionary.Add("Marriage", 13);
					UIPortMenuButton.<>f__switch$map15 = dictionary;
				}
				int num;
				if (UIPortMenuButton.<>f__switch$map15.TryGetValue(text, ref num))
				{
					switch (num)
					{
					case 0:
						return Generics.Scene.Organize;
					case 1:
						return Generics.Scene.Remodel;
					case 2:
						return Generics.Scene.Arsenal;
					case 3:
						return Generics.Scene.Duty;
					case 4:
						return Generics.Scene.Repair;
					case 5:
						return Generics.Scene.Supply;
					case 6:
						return Generics.Scene.Strategy;
					case 7:
						return Generics.Scene.Record;
					case 8:
						return Generics.Scene.Album;
					case 9:
						return Generics.Scene.Item;
					case 10:
						return Generics.Scene.Option;
					case 11:
						return Generics.Scene.Interior;
					case 12:
						return Generics.Scene.SaveLoad;
					case 13:
						return Generics.Scene.Marriage;
					}
				}
			}
			return Generics.Scene.Strategy;
		}

		public void Initialize(bool selectable)
		{
			base.get_transform().set_localScale(this.mVector3_NormalLocalScale);
			this.IsSelectable = selectable;
			this.OnInitialize(selectable);
		}

		public void SetOnClickEventListener(Action<UIPortMenuButton> onClickEventListener)
		{
			this.mOnClickEventListener = onClickEventListener;
		}

		[Obsolete("Inspector上で使用します。")]
		public void TouchEvent()
		{
			if (this.IsSelectable)
			{
				this.ClickEvent();
			}
		}

		public void ClickEvent()
		{
			if (this.mOnClickEventListener != null)
			{
				this.mOnClickEventListener.Invoke(this);
			}
		}

		public void Hover()
		{
			this.OnHoverEvent();
		}

		public void RemoveHover()
		{
			this.OnRemoveHoverEvent();
		}

		protected virtual void OnAwake()
		{
			this.mVector3_DefaultLocalPosition = base.get_transform().get_localPosition();
			this.mTexture_Text.alpha = 0f;
			this.mTexture_Glow_Back.get_transform().set_localScale(Vector3.get_zero());
			this.mTexture_Glow_Front.get_transform().set_localScale(Vector3.get_zero());
		}

		protected virtual void OnStart()
		{
		}

		public virtual Tween GenerateTweenRemoveHover()
		{
			Vector3 one = Vector3.get_one();
			return TweenSettingsExtensions.SetId<Tweener>(ShortcutExtensions.DOScale(base.get_transform(), one, 0.15f), this);
		}

		public virtual Tween GenerateTweenClick()
		{
			SoundUtils.PlaySE(SEFIleInfos.MainMenuOnClick);
			Sequence sequence = TweenSettingsExtensions.SetId<Sequence>(DOTween.Sequence(), this);
			Tween tween = TweenSettingsExtensions.SetId<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(base.get_transform(), new Vector3(0.9f, 0.9f), 0.075f), 1), this);
			Tween tween2 = TweenSettingsExtensions.SetId<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(base.get_transform(), new Vector3(1f, 1f), 0.075f), 1), this);
			TweenCallback tweenCallback = delegate
			{
			};
			TweenSettingsExtensions.Append(sequence, tween);
			TweenSettingsExtensions.Append(sequence, tween2);
			TweenSettingsExtensions.AppendCallback(sequence, tweenCallback);
			return sequence;
		}

		public virtual Tween GenerateTweenHoverScale()
		{
			Vector3 vector = new Vector3(1.15f, 1.15f);
			return TweenSettingsExtensions.SetId<Tweener>(ShortcutExtensions.DOScale(base.get_transform(), vector, 0.15f), this);
		}

		public virtual Tween GenerateTweenRemoveFocus()
		{
			Sequence sequence = TweenSettingsExtensions.SetId<Sequence>(DOTween.Sequence(), this);
			Tween tween = TweenSettingsExtensions.SetId<Tweener>(DOVirtual.Float(this.mTexture_Text.alpha, 0f, 0.15f, delegate(float alpha)
			{
				this.mTexture_Text.alpha = alpha;
			}), this);
			Tween tween2 = TweenSettingsExtensions.SetId<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.mTexture_Front.get_transform(), Vector3.get_one(), 0.15f), 18), this);
			Sequence sequence2 = TweenSettingsExtensions.SetId<Sequence>(DOTween.Sequence(), this);
			Tween tween3 = TweenSettingsExtensions.SetId<Tweener>(ShortcutExtensions.DOScale(this.mTexture_Glow_Back.get_transform(), Vector3.get_zero(), 0.2f), this);
			Tween tween4 = TweenSettingsExtensions.SetId<Tweener>(ShortcutExtensions.DOScale(this.mTexture_Glow_Front.get_transform(), Vector3.get_zero(), 0.2f), this);
			TweenSettingsExtensions.Append(sequence2, tween3);
			TweenSettingsExtensions.Join(sequence2, tween4);
			TweenSettingsExtensions.SetEase<Sequence>(sequence2, 10);
			TweenSettingsExtensions.Append(sequence, tween2);
			TweenSettingsExtensions.Join(sequence, sequence2);
			TweenSettingsExtensions.Join(sequence, tween);
			return sequence;
		}

		public virtual Tween GenerateTweenFocus()
		{
			Sequence sequence = TweenSettingsExtensions.SetId<Sequence>(DOTween.Sequence(), this);
			Tween tween = TweenSettingsExtensions.SetId<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.mTexture_Front.get_transform(), this.mVector3_FrontCoverOutScale, 0.15f), 18), this);
			Tween tween2 = TweenSettingsExtensions.SetId<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.mTexture_Front.get_transform(), this.mVector3_DefaultFrontScale, 0.15f), 1), this);
			TweenSettingsExtensions.Append(sequence, tween);
			TweenSettingsExtensions.Append(sequence, tween2);
			Sequence sequence2 = TweenSettingsExtensions.SetId<Sequence>(DOTween.Sequence(), this);
			Tween tween3 = TweenSettingsExtensions.SetId<Tweener>(ShortcutExtensions.DOScale(this.mTexture_Glow_Back.get_transform(), this.mBackMinimum, 0.1f), this);
			Tween tween4 = TweenSettingsExtensions.SetId<Tweener>(ShortcutExtensions.DOScale(this.mTexture_Glow_Front.get_transform(), this.mFrontMinimum, 0.1f), this);
			TweenSettingsExtensions.Append(sequence2, tween3);
			TweenSettingsExtensions.Join(sequence2, tween4);
			Sequence sequence3 = TweenSettingsExtensions.SetId<Sequence>(DOTween.Sequence(), this);
			Tween tween5 = TweenSettingsExtensions.SetId<Tweener>(ShortcutExtensions.DOScale(this.mTexture_Glow_Back.get_transform(), this.mBackMaximum, 1f), this);
			Tween tween6 = TweenSettingsExtensions.SetId<Tweener>(ShortcutExtensions.DOScale(this.mTexture_Glow_Front.get_transform(), this.mFrontMaximum, 1f), this);
			TweenSettingsExtensions.Append(sequence3, tween5);
			TweenSettingsExtensions.Join(sequence3, tween6);
			TweenSettingsExtensions.SetEase<Sequence>(sequence3, 4);
			Sequence sequence4 = TweenSettingsExtensions.SetId<Sequence>(DOTween.Sequence(), this);
			Tween tween7 = TweenSettingsExtensions.SetId<Tweener>(ShortcutExtensions.DOScale(this.mTexture_Glow_Back.get_transform(), this.mBackMinimum, 2f), this);
			Tween tween8 = TweenSettingsExtensions.SetId<Tweener>(ShortcutExtensions.DOScale(this.mTexture_Glow_Front.get_transform(), this.mFrontMinimum, 2f), this);
			TweenSettingsExtensions.Append(sequence4, tween7);
			TweenSettingsExtensions.Join(sequence4, tween8);
			TweenSettingsExtensions.SetEase<Sequence>(sequence4, 4);
			Sequence sequence5 = TweenSettingsExtensions.SetId<Sequence>(DOTween.Sequence(), this);
			TweenSettingsExtensions.Append(sequence5, sequence3);
			TweenSettingsExtensions.Append(sequence5, sequence4);
			TweenSettingsExtensions.SetLoops<Sequence>(sequence5, 2147483647);
			Sequence sequence6 = TweenSettingsExtensions.SetId<Sequence>(DOTween.Sequence(), this);
			Tween tween9 = TweenSettingsExtensions.SetId<Tweener>(DOVirtual.Float(this.mTexture_Text.alpha, 1f, 0.15f, delegate(float alpha)
			{
				this.mTexture_Text.alpha = alpha;
			}), this);
			TweenSettingsExtensions.Append(sequence6, sequence2);
			TweenSettingsExtensions.Join(sequence6, tween9);
			TweenSettingsExtensions.Append(sequence6, sequence);
			TweenSettingsExtensions.Append(sequence6, sequence5);
			return sequence6;
		}

		protected virtual void OnHoverEvent()
		{
		}

		protected virtual void OnRemoveHoverEvent()
		{
		}

		protected virtual void OnInitialize(bool isSelectable)
		{
			if (isSelectable)
			{
				this.mTexture_TextShadow.color = new Color(0.65882355f, 0.6627451f, 0.6784314f, this.mTexture_TextShadow.alpha);
			}
			else
			{
				this.mTexture_Front.mainTexture = (Resources.Load("Textures/PortTop/menu_bg6") as Texture);
				this.mTexture_Base.mainTexture = (Resources.Load("Textures/PortTop/menu_bg5") as Texture);
				this.mTexture_TextShadow.color = new Color(0.5647059f, 0.5647059f, 0.5647059f, this.mTexture_TextShadow.alpha);
				this.mTexture_Text.color = new Color(0.5647059f, 0.5647059f, 0.5647059f, this.mTexture_Text.alpha);
			}
		}
	}
}
