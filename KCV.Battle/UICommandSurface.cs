using Common.Enum;
using KCV.Battle.Utils;
using KCV.Generic;
using KCV.Utils;
using LT.Tweening;
using System;
using UnityEngine;

namespace KCV.Battle
{
	[RequireComponent(typeof(BoxCollider2D)), RequireComponent(typeof(UIWidget))]
	public class UICommandSurface : MonoBehaviour, IUICommandSurface
	{
		[SerializeField]
		private UISprite _uiBackground;

		[SerializeField]
		private UISprite _uiForeground;

		[SerializeField]
		private UISprite _uiGrow;

		[SerializeField]
		private UISprite _uiIcon;

		[SerializeField]
		private UISprite _uiIconGrow;

		[SerializeField]
		private UILabel _uiNum;

		[SerializeField]
		private Vector3 _vReductionSize = Vector3.get_one() * 0.7f;

		[SerializeField]
		private Vector3 _vMagnifySize = Vector3.get_one();

		[SerializeField]
		private float _fScalingTime = 0.2f;

		private BoxCollider2D _colBox2D;

		private BattleCommand _iCommandType;

		private int _nDefaultDepth;

		private int _nIndex;

		private bool _isAbsorded;

		private bool _isMagnify;

		private bool _isReduction;

		private bool _isGrowHex;

		private Action _actOnSetCommandUnit;

		private UIWidget _uiWidget;

		public int depth
		{
			get
			{
				return this._uiBackground.depth;
			}
			set
			{
				if (value != this._uiBackground.depth)
				{
					this._uiBackground.depth = value;
					this._uiGrow.depth = value + 1;
					this._uiForeground.depth = value + 2;
					this._uiNum.depth = value + 3;
					this._uiIconGrow.depth = value + 4;
					this._uiIcon.depth = value + 5;
				}
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
				this._uiNum.textInt = value + 1;
			}
		}

		public BoxCollider2D boxCollider2D
		{
			get
			{
				return this.GetComponentThis(ref this._colBox2D);
			}
		}

		public BattleCommand commandType
		{
			get
			{
				return this._iCommandType;
			}
			set
			{
				this._iCommandType = value;
				if (this._iCommandType == BattleCommand.None)
				{
					this._uiIcon.spriteName = string.Empty;
					return;
				}
				this._uiIcon.spriteName = string.Format("command_{0}_on", (int)value);
				this._uiIcon.MakePixelPerfect();
			}
		}

		public bool isSetUnit
		{
			get
			{
				return this.commandType != BattleCommand.None;
			}
		}

		private bool isGrowHex
		{
			set
			{
				if (value)
				{
					if (!this._isGrowHex)
					{
						this._isGrowHex = true;
						this._uiGrow.color = KCVColor.BattleCommandSurfaceBlue;
						this._uiGrow.alpha = 0f;
						this._uiGrow.get_transform().LTValue(0f, 1f, 1f).setLoopPingPong(2147483647).setOnUpdate(delegate(float x)
						{
							this._uiGrow.alpha = x;
						});
					}
				}
				else if (this._isGrowHex)
				{
					this._isGrowHex = false;
					this._uiGrow.get_transform().LTCancel();
					this._uiGrow.get_transform().LTValue(this._uiGrow.alpha, 0f, 0.2f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
					{
						this._uiGrow.alpha = x;
					});
				}
			}
		}

		public bool isMagnify
		{
			get
			{
				return this._isMagnify;
			}
			private set
			{
				if (value)
				{
					if (!this._isMagnify)
					{
						this._isMagnify = true;
					}
				}
				else if (this._isMagnify)
				{
					this._isMagnify = false;
				}
			}
		}

		public bool isReduction
		{
			get
			{
				return this._isReduction;
			}
			private set
			{
				if (value)
				{
					if (!this._isReduction)
					{
						this._isReduction = true;
					}
				}
				else if (this._isReduction)
				{
					this._isReduction = false;
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

		public bool isAbsorded
		{
			get
			{
				return this._isAbsorded;
			}
			set
			{
				if (value)
				{
					if (!this._isAbsorded)
					{
						KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_999);
						this._uiIconGrow.get_transform().LTValue(1f, 0f, 0.35f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
						{
							this._uiIconGrow.alpha = x;
						});
						this._isAbsorded = true;
					}
				}
				else if (this._isAbsorded)
				{
					this._isAbsorded = false;
				}
			}
		}

		public static UICommandSurface Instantiate(UICommandSurface prefab, Transform parent, Vector3 pos, int nIndex, BattleCommand iCommand, Action onSetCommandUnit)
		{
			UICommandSurface uICommandSurface = Object.Instantiate<UICommandSurface>(prefab);
			uICommandSurface.get_transform().set_parent(parent);
			uICommandSurface.get_transform().set_localScale(uICommandSurface._vReductionSize);
			uICommandSurface.get_transform().set_localPosition(pos);
			uICommandSurface.Init(nIndex, iCommand, onSetCommandUnit);
			return uICommandSurface;
		}

		private void OnDestroy()
		{
			Mem.Del(ref this._uiBackground);
			Mem.Del(ref this._uiForeground);
			Mem.Del(ref this._uiGrow);
			Mem.Del(ref this._uiIcon);
			Mem.Del(ref this._uiIconGrow);
			Mem.Del<UILabel>(ref this._uiNum);
			Mem.Del<Vector3>(ref this._vReductionSize);
			Mem.Del<Vector3>(ref this._vMagnifySize);
			Mem.Del<float>(ref this._fScalingTime);
			Mem.Del<BoxCollider2D>(ref this._colBox2D);
			Mem.Del<BattleCommand>(ref this._iCommandType);
			Mem.Del<int>(ref this._nDefaultDepth);
			Mem.Del<int>(ref this._nIndex);
			Mem.Del<bool>(ref this._isAbsorded);
			Mem.Del<bool>(ref this._isMagnify);
			Mem.Del<bool>(ref this._isReduction);
			Mem.Del<bool>(ref this._isGrowHex);
			Mem.Del<Action>(ref this._actOnSetCommandUnit);
			Mem.Del<UIWidget>(ref this._uiWidget);
		}

		private bool Init(int nIndex, BattleCommand iCommand, Action onSetCommandUnit)
		{
			this.index = nIndex;
			this._actOnSetCommandUnit = onSetCommandUnit;
			this._isAbsorded = false;
			this._isMagnify = false;
			this._isReduction = false;
			this._isGrowHex = false;
			this.commandType = iCommand;
			this._nDefaultDepth = 1;
			this.depth = this._nDefaultDepth;
			this._uiGrow.alpha = 0f;
			this._uiIconGrow.alpha = 0f;
			this.isGrowHex = this.isSetUnit;
			return true;
		}

		public void SetCommandUnit(UICommandUnitIcon unit)
		{
			KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_003);
			this.commandType = unit.commandType;
			this.isGrowHex = true;
			Dlg.Call(ref this._actOnSetCommandUnit);
			this.ReductionUnitSet();
		}

		public void RemoveCommandUnit()
		{
			if (this.commandType == BattleCommand.None)
			{
				return;
			}
			KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.CommonCancel2);
			this.commandType = BattleCommand.None;
			this.isAbsorded = false;
			this.isGrowHex = false;
			Dlg.Call(ref this._actOnSetCommandUnit);
		}

		public void ChkSurfaceMagnifyDistance(UICommandUnitIcon unit)
		{
			if (Vector3.Distance(base.get_transform().get_position(), unit.get_transform().get_position()) < 0.09f)
			{
				this.Absorded(unit);
			}
			else if (Vector3.Distance(base.get_transform().get_position(), unit.get_transform().get_position()) < 0.5f)
			{
				this.isAbsorded = false;
				this.Magnify();
			}
			else
			{
				this.isAbsorded = false;
				this.Reduction();
			}
		}

		public void Absorded(UICommandUnitIcon unit)
		{
			this.isAbsorded = true;
			this.Magnify();
			unit.get_transform().set_position(this._uiIcon.get_transform().get_position());
			unit.isActiveIcon = true;
		}

		public LTDescr Magnify()
		{
			if (!(base.get_transform().get_localScale() != this._vMagnifySize))
			{
				return null;
			}
			if (!this.isMagnify)
			{
				this.depth = 10;
				this.isMagnify = true;
				return base.get_transform().LTScale(this._vMagnifySize, this._fScalingTime).setEase(LeanTweenType.easeOutSine).setOnComplete(delegate
				{
					this.isMagnify = false;
					this.isGrowHex = false;
				});
			}
			return null;
		}

		public LTDescr Reduction()
		{
			if (!(base.get_transform().get_localScale() != this._vReductionSize))
			{
				return null;
			}
			if (!this.isReduction)
			{
				this.isReduction = true;
				this.depth = this._nDefaultDepth;
				return base.get_transform().LTScale(this._vReductionSize, this._fScalingTime).setEase(LeanTweenType.easeOutSine).setOnComplete(delegate
				{
					if (this.isSetUnit)
					{
						this.isGrowHex = true;
					}
					this.isReduction = false;
				});
			}
			return null;
		}

		private LTDescr ReductionUnitSet()
		{
			if (base.get_transform().get_localScale() != Vector3.get_one() * 1.1f)
			{
				base.get_transform().set_localScale(Vector3.get_one() * 1.1f);
				return base.get_transform().LTScale(this._vReductionSize, 0.3f).setEase(LeanTweenType.easeOutBounce).setOnComplete(delegate
				{
					this.depth = this._nDefaultDepth;
				});
			}
			return null;
		}

		public LTDescr Show(float time)
		{
			return this.widget.get_transform().LTValue(this.widget.alpha, 1f, time).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				this.widget.alpha = x;
			});
		}

		public LTDescr Hide(float time)
		{
			return this.widget.get_transform().LTValue(this.widget.alpha, 0f, time).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				this.widget.alpha = x;
			});
		}

		private void OnClick()
		{
			this.RemoveCommandUnit();
		}
	}
}
