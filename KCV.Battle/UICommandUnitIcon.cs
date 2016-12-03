using Common.Enum;
using KCV.Battle.Production;
using KCV.Utils;
using LT.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KCV.Battle
{
	[RequireComponent(typeof(BoxCollider2D))]
	public class UICommandUnitIcon : UIDragDropItem
	{
		[SerializeField]
		private UISprite _uiIcon;

		private int _nDefaultIconDepth;

		private bool _isBorderOrver;

		private bool _isValid;

		private Vector3 _vStartPos;

		private BoxCollider2D _colBox2D;

		private BattleCommand _iCommandType;

		private Action<BattleCommand> _actOnDragStart;

		private Action _actOnDragAndDropRelease;

		public BattleCommand commandType
		{
			get
			{
				return this._iCommandType;
			}
			set
			{
				this._iCommandType = value;
				this._uiIcon.spriteName = string.Format("command_{0}", (int)value);
				this._uiIcon.MakePixelPerfect();
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
				this._uiIcon.color = ((!this._isValid) ? Color.get_gray() : Color.get_white());
				this.colliderBox2D.set_enabled(this._isValid);
			}
		}

		public bool isActiveIcon
		{
			get
			{
				return this._uiIcon.depth == 100;
			}
			set
			{
				if (value)
				{
					this._uiIcon.depth = 100;
					this._uiIcon.spriteName = string.Format("command_{0}_on", (int)this._iCommandType);
				}
				else
				{
					this._uiIcon.spriteName = string.Format("command_{0}", (int)this._iCommandType);
					this._uiIcon.depth = this._nDefaultIconDepth;
				}
			}
		}

		public bool isFocus
		{
			get
			{
				return this.isActiveIcon;
			}
			set
			{
				if (value)
				{
					this.isActiveIcon = true;
					base.get_transform().LTCancel();
					base.get_transform().LTScale(Vector3.get_one() * 1.05f, 0.2f).setEase(LeanTweenType.easeOutSine);
				}
				else
				{
					this.isActiveIcon = false;
					base.get_transform().LTCancel();
					base.get_transform().LTScale(Vector3.get_one() * 0.95f, 0.2f).setEase(LeanTweenType.easeOutSine);
				}
			}
		}

		public BoxCollider2D colliderBox2D
		{
			get
			{
				return this.GetComponentThis(ref this._colBox2D);
			}
		}

		public static UICommandUnitIcon Instantiate(UICommandUnitIcon prefab, Transform parent, Vector3 pos, int nType)
		{
			UICommandUnitIcon uICommandUnitIcon = Object.Instantiate<UICommandUnitIcon>(prefab);
			uICommandUnitIcon.get_transform().set_parent(parent);
			uICommandUnitIcon.get_transform().set_localPosition(pos);
			uICommandUnitIcon.get_transform().localScaleOne();
			return uICommandUnitIcon;
		}

		private void Awake()
		{
			this._nDefaultIconDepth = this._uiIcon.depth;
			this._isBorderOrver = false;
		}

		private void OnDestroy()
		{
			Mem.Del(ref this._uiIcon);
			Mem.Del<Vector3>(ref this._vStartPos);
			Mem.Del<BoxCollider2D>(ref this._colBox2D);
			Mem.Del<int>(ref this._nDefaultIconDepth);
			Mem.Del<BattleCommand>(ref this._iCommandType);
			Mem.Del<bool>(ref this._isBorderOrver);
			Mem.Del<Action<BattleCommand>>(ref this._actOnDragStart);
			Mem.Del<Action>(ref this._actOnDragAndDropRelease);
		}

		public bool Init(BattleCommand nType, bool isValid, Action<BattleCommand> onDragStart, Action onDragDropRelease)
		{
			this._vStartPos = base.get_transform().get_localPosition();
			this.commandType = nType;
			this.isValid = isValid;
			this._actOnDragStart = onDragStart;
			this._actOnDragAndDropRelease = onDragDropRelease;
			return true;
		}

		protected override void OnDragStart()
		{
			this.isFocus = true;
			Dlg.Call<BattleCommand>(ref this._actOnDragStart, this._iCommandType);
			SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			base.OnDragStart();
		}

		protected override void OnDragDropMove(Vector2 delta)
		{
			ProdBattleCommandSelect prodBattleCommandSelect = BattleTaskManager.GetTaskCommand().prodBattleCommandSelect;
			List<UICommandSurface> listCommandSurfaces = prodBattleCommandSelect.commandBox.listCommandSurfaces;
			UICommandSurface mainTarget = Enumerable.First<UICommandSurface>(Enumerable.OrderBy<UICommandSurface, float>(listCommandSurfaces, (UICommandSurface x) => (x.get_transform().get_position() - this.get_transform().get_position()).get_magnitude()));
			this.ChkBorderLine();
			listCommandSurfaces.ForEach(delegate(UICommandSurface x)
			{
				if (x == mainTarget)
				{
					x.ChkSurfaceMagnifyDistance(this);
				}
				else
				{
					x.Reduction();
				}
			});
			if (!mainTarget.isAbsorded)
			{
				base.OnDragDropMove(delta);
			}
		}

		protected override void OnDragDropRelease(GameObject surface)
		{
			if (surface != null)
			{
				List<UICommandSurface> list = BattleTaskManager.GetTaskCommand().prodBattleCommandSelect.commandBox.listCommandSurfaces.FindAll((UICommandSurface x) => x.isAbsorded);
				if (list != null && list.get_Count() != 0)
				{
					list.get_Item(0).SetCommandUnit(this);
				}
			}
			base.OnDragDropRelease(surface);
			this.Reset();
		}

		private void ChkBorderLine()
		{
			ProdBattleCommandSelect prodBattleCommandSelect = BattleTaskManager.GetTaskCommand().prodBattleCommandSelect;
			float unitIconLabelDrawBorderLineLocalPosX = prodBattleCommandSelect.commandUnitList.unitIconLabelDrawBorderLineLocalPosX;
			if (base.get_transform().get_localPosition().x < unitIconLabelDrawBorderLineLocalPosX && !this._isBorderOrver)
			{
				this._isBorderOrver = true;
				this.isActiveIcon = false;
			}
			else if (base.get_transform().get_localPosition().x >= unitIconLabelDrawBorderLineLocalPosX && this._isBorderOrver)
			{
				this._isBorderOrver = false;
				this.isActiveIcon = true;
			}
		}

		public bool Reset()
		{
			this._isBorderOrver = false;
			this.isActiveIcon = false;
			base.get_transform().set_localPosition(this._vStartPos);
			base.get_transform().set_localScale(Vector3.get_one() * 0.95f);
			Dlg.Call(ref this._actOnDragAndDropRelease);
			return true;
		}

		public void ResetPosition()
		{
			if (base.get_transform().get_localPosition() != this._vStartPos)
			{
				base.get_transform().set_localPosition(this._vStartPos);
				this.isActiveIcon = false;
				this.isFocus = false;
			}
		}
	}
}
