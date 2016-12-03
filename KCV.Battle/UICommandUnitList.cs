using Common.Enum;
using KCV.Utils;
using local.models.battle;
using LT.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle
{
	[RequireComponent(typeof(UIPanel))]
	public class UICommandUnitList : MonoBehaviour
	{
		[Serializable]
		private struct Params : IDisposable
		{
			public float showTime;

			public LeanTweenType showEase;

			public Vector3 unitListPos;

			public void Dispose()
			{
				Mem.Del<float>(ref this.showTime);
				Mem.Del<LeanTweenType>(ref this.showEase);
				Mem.Del<Vector3>(ref this.unitListPos);
			}
		}

		[SerializeField]
		private float _fUnitIconLabelDrawBorderLineLocalPosX = -100f;

		[SerializeField]
		private UISelectCommandInfo _uiSelectCommandInfo;

		private List<UICommandUnitIcon> _listCommandUnits;

		private List<UISprite> _listCommandUnitOrigs;

		private UIPanel _uiPanel;

		private Vector2 _vSelectCommandPos;

		private BattleCommand[,] _aryCommandsPos;

		private Action _actOnDragAndDropRelease;

		[Header("[Animation Properties]"), SerializeField]
		private UICommandUnitList.Params _strParams;

		public UIPanel panel
		{
			get
			{
				return this.GetComponentThis(ref this._uiPanel);
			}
		}

		public List<UICommandUnitIcon> listCommandUnits
		{
			get
			{
				return this._listCommandUnits;
			}
		}

		public float unitIconLabelDrawBorderLineLocalPosX
		{
			get
			{
				return this._fUnitIconLabelDrawBorderLineLocalPosX;
			}
		}

		public UICommandUnitIcon focusUnitIcon
		{
			get
			{
				BattleCommand target = this.GetCommandType(this._vSelectCommandPos);
				return this.listCommandUnits.Find((UICommandUnitIcon x) => x.commandType == target);
			}
		}

		public bool isColliderEnabled
		{
			set
			{
				this._listCommandUnits.ForEach(delegate(UICommandUnitIcon x)
				{
					x.colliderBox2D.set_enabled(x.isValid && value);
				});
			}
		}

		public bool isAnyDrag
		{
			get
			{
				using (List<UICommandUnitIcon>.Enumerator enumerator = this._listCommandUnits.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						UICommandUnitIcon current = enumerator.get_Current();
						if (current.isFocus)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		private void Awake()
		{
			base.get_transform().set_localPosition(this._strParams.unitListPos);
			this.panel.alpha = 0f;
			this._uiSelectCommandInfo.ClearInfo();
		}

		private void OnDestroy()
		{
			Mem.Del<float>(ref this._fUnitIconLabelDrawBorderLineLocalPosX);
			Mem.Del<UISelectCommandInfo>(ref this._uiSelectCommandInfo);
			Mem.DelListSafe<UICommandUnitIcon>(ref this._listCommandUnits);
			Mem.DelListSafe<UISprite>(ref this._listCommandUnitOrigs);
			Mem.Del<UIPanel>(ref this._uiPanel);
			Mem.Del<Vector2>(ref this._vSelectCommandPos);
			Mem.Del<BattleCommand[,]>(ref this._aryCommandsPos);
			Mem.Del<Action>(ref this._actOnDragAndDropRelease);
			Mem.DelIDisposableSafe<UICommandUnitList.Params>(ref this._strParams);
		}

		public bool Init(CommandPhaseModel model, Action onDragAndDropRelease)
		{
			this._actOnDragAndDropRelease = onDragAndDropRelease;
			bool flag = !model.GetSelectableCommands().Contains(BattleCommand.Kouku) && !model.GetSelectableCommands().Contains(BattleCommand.Totugeki) && !model.GetSelectableCommands().Contains(BattleCommand.Tousha);
			Transform transform = base.get_transform().Find("CommandOrig");
			transform.set_localPosition((!flag) ? Vector3.get_zero() : (Vector3.get_right() * 100f));
			this._listCommandUnits = new List<UICommandUnitIcon>();
			this._listCommandUnitOrigs = new List<UISprite>();
			using (IEnumerator enumerator = Enum.GetValues(typeof(BattleCommand)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BattleCommand battleCommand = (BattleCommand)((int)enumerator.get_Current());
					if (battleCommand != BattleCommand.None)
					{
						this._listCommandUnits.Add(base.get_transform().FindChild(string.Format("CommandUnit{0}", (int)battleCommand)).GetComponent<UICommandUnitIcon>());
						this._listCommandUnitOrigs.Add(base.get_transform().FindChild(string.Format("CommandOrig/Icon{0}", (int)battleCommand)).GetComponent<UISprite>());
						Vector3 localPosition = this._listCommandUnits.get_Item((int)battleCommand).get_transform().get_localPosition();
						localPosition.x = ((!flag) ? localPosition.x : (localPosition.x + 100f));
						this._listCommandUnits.get_Item((int)battleCommand).get_transform().set_localPosition(localPosition);
						this._listCommandUnits.get_Item((int)battleCommand).Init(battleCommand, model.GetSelectableCommands().Contains(battleCommand), new Action<BattleCommand>(this.OnDragStart), new Action(this.OnDragAndDropRelease));
						this._listCommandUnits.get_Item((int)battleCommand).SetActive(this._listCommandUnits.get_Item((int)battleCommand).isValid);
						this._listCommandUnitOrigs.get_Item((int)battleCommand).SetActive(this._listCommandUnits.get_Item((int)battleCommand).isValid);
					}
				}
			}
			this._vSelectCommandPos = new Vector2(0f, 0f);
			BattleCommand[,] expr_231 = new BattleCommand[3, 3];
			expr_231[0, 1] = BattleCommand.Ridatu;
			expr_231[0, 2] = BattleCommand.Kouku;
			expr_231[1, 0] = BattleCommand.Hougeki;
			expr_231[1, 1] = BattleCommand.Taisen;
			expr_231[1, 2] = BattleCommand.Totugeki;
			expr_231[2, 0] = BattleCommand.Raigeki;
			expr_231[2, 1] = BattleCommand.Kaihi;
			expr_231[2, 2] = BattleCommand.Tousha;
			this._aryCommandsPos = expr_231;
			return true;
		}

		public LTDescr Show()
		{
			return base.get_transform().LTValue(this.panel.alpha, 1f, this._strParams.showTime).setEase(this._strParams.showEase).setOnUpdate(delegate(float x)
			{
				this.panel.alpha = x;
			});
		}

		public void Active2FocusUnit2(UICommandSurface surface, List<BattleCommand> invalidCommands)
		{
			if (surface.commandType == BattleCommand.None)
			{
				this._vSelectCommandPos = Vector2.get_zero();
				this.ChangeFocus(this._vSelectCommandPos);
			}
			else
			{
				BattleCommand iCommand = (!invalidCommands.Contains(surface.commandType)) ? surface.commandType : BattleCommand.Sekkin;
				this._vSelectCommandPos = this.GetCommandPos(iCommand);
				this.ChangeFocus(this._vSelectCommandPos);
			}
		}

		public void ActiveAll2Unit(bool isActive)
		{
			this._listCommandUnits.ForEach(delegate(UICommandUnitIcon x)
			{
				x.isFocus = isActive;
			});
			this._uiSelectCommandInfo.ClearInfo();
		}

		public void Reset2Unit()
		{
			BattleCommand target = this.GetCommandType(this._vSelectCommandPos);
			this._listCommandUnits.Find((UICommandUnitIcon x) => x.commandType == target).Reset();
		}

		public void PrevLine()
		{
			this.PreparaNext(true, false);
		}

		public void NextLine()
		{
			this.PreparaNext(true, true);
		}

		public void PrevColumn()
		{
			this.PreparaNext(false, false);
		}

		public void NextColumn()
		{
			this.PreparaNext(false, true);
		}

		private void PreparaNext(bool isLine, bool isFoward)
		{
			if (isLine)
			{
				float y2 = this._vSelectCommandPos.y;
				this._vSelectCommandPos.y = (float)Mathe.NextElement((int)this._vSelectCommandPos.y, 0, 2, isFoward, delegate(int y)
				{
					Vector2 vPos = new Vector2(this._vSelectCommandPos.x, (float)y);
					return this._listCommandUnits.get_Item((int)this.GetCommandType(vPos)).isValid;
				});
				if (y2 != this._vSelectCommandPos.y)
				{
					this.ChangeFocus(this._vSelectCommandPos);
				}
			}
			else
			{
				float x2 = this._vSelectCommandPos.x;
				this._vSelectCommandPos.x = (float)Mathe.NextElement((int)this._vSelectCommandPos.x, 0, 2, isFoward, delegate(int x)
				{
					Vector2 vPos = new Vector2((float)x, this._vSelectCommandPos.y);
					return this._listCommandUnits.get_Item((int)this.GetCommandType(vPos)).isValid;
				});
				if (x2 != this._vSelectCommandPos.x)
				{
					this.ChangeFocus(this._vSelectCommandPos);
				}
			}
		}

		private void ChangeFocus(Vector2 vPos)
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove2);
			BattleCommand target = this.GetCommandType(vPos);
			this._listCommandUnits.ForEach(delegate(UICommandUnitIcon x)
			{
				x.isFocus = (x.commandType == target);
			});
			this._uiSelectCommandInfo.SetInfo(target);
		}

		private BattleCommand GetCommandType(Vector2 vPos)
		{
			return this._aryCommandsPos[(int)vPos.x, (int)vPos.y];
		}

		private Vector2 GetCommandPos(BattleCommand iCommand)
		{
			Vector2 zero = Vector2.get_zero();
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					if (this._aryCommandsPos[i, j] == iCommand)
					{
						zero = new Vector2((float)i, (float)j);
						break;
					}
				}
			}
			return zero;
		}

		private void OnDragStart(BattleCommand iCommand)
		{
			this._uiSelectCommandInfo.SetInfo(iCommand);
		}

		private void OnDragAndDropRelease()
		{
			Dlg.Call(ref this._actOnDragAndDropRelease);
			this._uiSelectCommandInfo.ClearInfo();
		}
	}
}
