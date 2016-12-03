using Common.Enum;
using LT.Tweening;
using System;
using UniRx;
using UnityEngine;

namespace KCV.Title
{
	[RequireComponent(typeof(UIToggle)), RequireComponent(typeof(BoxCollider2D)), RequireComponent(typeof(UISprite))]
	public class UIDifficultyBtn : MonoBehaviour
	{
		private const int PAPER_PAGE_MAX = 21;

		[SerializeField]
		private UISprite _uiForeground;

		[SerializeField]
		private UISprite _uiForegroundPaper;

		[SerializeField]
		private UISprite _uiDifficultyLabel;

		[SerializeField]
		private UISprite _uiDifficultyRedLabel;

		private UISprite _uiBackground;

		private DifficultKind _iKind;

		private bool _isFocus;

		private int _nIndex;

		private int _nPaperIndex;

		private UIWidget _uiWidget;

		private UIToggle _uiToggle;

		private BoxCollider2D _colBox2D;

		private IDisposable _disPaperAnimCancel;

		private UISprite background
		{
			get
			{
				return this.GetComponentThis(ref this._uiBackground);
			}
		}

		public DifficultKind difficultKind
		{
			get
			{
				return this._iKind;
			}
			private set
			{
				this._iKind = value;
				this._uiForeground.spriteName = string.Format("txt_diff{0}_gray", (int)this._iKind);
				this._uiForeground.MakePixelPerfect();
				this._uiDifficultyLabel.spriteName = string.Format("btn_diff{0}_txt", (int)this._iKind);
				this._uiDifficultyLabel.MakePixelPerfect();
				this._uiDifficultyRedLabel.spriteName = string.Format("btn_diff{0}_txt_red", (int)this._iKind);
				this._uiDifficultyRedLabel.MakePixelPerfect();
			}
		}

		public int index
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

		private int paperIndex
		{
			get
			{
				return this._nPaperIndex;
			}
			set
			{
				this._nPaperIndex = Mathe.MinMax2(value, 2, 23);
				this.background.spriteName = string.Format("btn_diff1_{0:D5}", this._nPaperIndex + 2);
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
					if (this._isFocus != value)
					{
						this._isFocus = true;
						this._uiForeground.spriteName = string.Format("txt_diff{0}", (int)this._iKind);
						this._uiForeground.MakePixelPerfect();
						base.get_transform().LTCancel();
						base.get_transform().LTScale(Vector3.get_one() * 1.1f, Mathe.Frame2Sec(5, 60f)).setEase(LeanTweenType.easeOutSine);
						base.get_transform().LTMoveLocalY(15f, Mathe.Frame2Sec(5, 60f)).setEase(LeanTweenType.linear);
						this.PlayPaperAnimation();
						this.toggle.value = true;
					}
				}
				else if (this._isFocus != value)
				{
					this._isFocus = false;
					this._uiForeground.spriteName = string.Format("txt_diff{0}_gray", (int)this._iKind);
					this._uiForeground.MakePixelPerfect();
					this._uiBackground.localSize = new Vector2(138f, 234f);
					base.get_transform().LTCancel();
					base.get_transform().LTScale(Vector3.get_one(), Mathe.Frame2Sec(22, 60f)).setEase(LeanTweenType.easeOutSine);
					base.get_transform().LTMoveLocalY(0f, Mathe.Frame2Sec(22, 60f)).setEase(LeanTweenType.easeOutCubic);
					if (this._disPaperAnimCancel != null)
					{
						this._disPaperAnimCancel.Dispose();
					}
					this.background.spriteName = "btn_diff1_gray";
					this.toggle.value = false;
					this.HideForegroundObjests();
				}
			}
		}

		public UIWidget widget
		{
			get
			{
				return this.GetComponentThis(ref this._uiWidget);
			}
		}

		public UIToggle toggle
		{
			get
			{
				return this.GetComponentThis(ref this._uiToggle);
			}
		}

		public BoxCollider2D collider
		{
			get
			{
				return this.GetComponentThis(ref this._colBox2D);
			}
		}

		public Transform wrightBrushAnchor
		{
			get
			{
				return this._uiForeground.get_transform();
			}
		}

		public static UIDifficultyBtn Instantiate(UIDifficultyBtn prefab, Transform parent, DifficultKind iKind, int nIndex)
		{
			UIDifficultyBtn uIDifficultyBtn = Object.Instantiate<UIDifficultyBtn>(prefab);
			uIDifficultyBtn.get_transform().set_parent(parent);
			uIDifficultyBtn.get_transform().localScaleOne();
			uIDifficultyBtn.get_transform().localPositionZero();
			uIDifficultyBtn.Init(iKind, nIndex);
			return uIDifficultyBtn;
		}

		private bool Init(DifficultKind iKind, int nIndex)
		{
			this.difficultKind = iKind;
			this.index = nIndex;
			this._isFocus = false;
			this._uiForeground.spriteName = string.Format("txt_diff{0}_gray", (int)iKind);
			this._uiForeground.MakePixelPerfect();
			this.background.spriteName = "btn_diff1_gray";
			this._uiForegroundPaper.alpha = 0f;
			this._uiDifficultyLabel.alpha = 0f;
			this._uiDifficultyRedLabel.alpha = 0f;
			return true;
		}

		private void OnDestroy()
		{
			Mem.Del(ref this._uiForeground);
			Mem.Del(ref this._uiForegroundPaper);
			Mem.Del(ref this._uiDifficultyLabel);
			Mem.Del(ref this._uiDifficultyRedLabel);
			Mem.Del(ref this._uiBackground);
			Mem.Del<DifficultKind>(ref this._iKind);
			Mem.Del<bool>(ref this._isFocus);
			Mem.Del<int>(ref this._nIndex);
			Mem.Del<int>(ref this._nPaperIndex);
			Mem.Del<UIWidget>(ref this._uiWidget);
			Mem.Del<UIToggle>(ref this._uiToggle);
			Mem.Del<BoxCollider2D>(ref this._colBox2D);
			if (this._disPaperAnimCancel != null)
			{
				this._disPaperAnimCancel.Dispose();
			}
			Mem.Del<IDisposable>(ref this._disPaperAnimCancel);
		}

		private void PlayPaperAnimation()
		{
			if (this._disPaperAnimCancel != null)
			{
				this._disPaperAnimCancel.Dispose();
			}
			this.paperIndex = 0;
			this._disPaperAnimCancel = Observable.IntervalFrame(0, FrameCountType.Update).Select(delegate(long x)
			{
				long expr_01 = x;
				x = expr_01 + 1L;
				return expr_01;
			}).TakeWhile((long x) => x <= 21L).Subscribe(delegate(long x)
			{
				this.paperIndex = (int)x;
				if (this.paperIndex == 15)
				{
					this.ShowForegroundObjects();
				}
			}).AddTo(base.get_gameObject());
		}

		private void ShowForegroundObjects()
		{
			this._uiForegroundPaper.get_transform().LTCancel();
			this._uiForegroundPaper.get_transform().LTValue(this._uiForegroundPaper.alpha, 1f, 0.25f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				this._uiForegroundPaper.alpha = x;
				this._uiDifficultyLabel.alpha = x;
			});
		}

		private void HideForegroundObjests()
		{
			this._uiForegroundPaper.get_transform().LTCancel();
			this._uiForegroundPaper.get_transform().LTValue(this._uiForegroundPaper.alpha, 0f, 0.15f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				this._uiForegroundPaper.alpha = x;
				this._uiDifficultyLabel.alpha = x;
			});
		}

		public void ShowDifficultyRedLabel()
		{
			this._uiDifficultyRedLabel.get_transform().LTCancel();
			this._uiDifficultyRedLabel.get_transform().LTValue(this._uiDifficultyRedLabel.alpha, 1f, 0.25f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				this._uiDifficultyRedLabel.alpha = x;
			});
		}
	}
}
