using Common.Enum;
using KCV.Utils;
using LT.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.BattleCut
{
	[RequireComponent(typeof(UIWidget))]
	public class UICommandUnitSelect : MonoBehaviour
	{
		[SerializeField]
		private UITexture _uiOverlay;

		[SerializeField]
		private UIInvisibleCollider _uiInvisibleCollider;

		[SerializeField]
		private UIGrid _uiCommandAnchor;

		private int _nIndex;

		private UIWidget _uiWidget;

		private List<UICommandLabelButton> _listCommandUnitLabel;

		private Action<BattleCommand> _actOnDecide;

		private Action _actOnCancel;

		private UIWidget widget
		{
			get
			{
				return this.GetComponentThis(ref this._uiWidget);
			}
		}

		private UICommandLabelButton selectedUnit
		{
			get
			{
				return this._listCommandUnitLabel.get_Item(this._nIndex);
			}
		}

		public bool isColliderEnabled
		{
			set
			{
				this._listCommandUnitLabel.ForEach(delegate(UICommandLabelButton x)
				{
					x.toggle.set_enabled(x.isValid && value);
				});
				this._uiInvisibleCollider.set_enabled(value);
			}
		}

		public static UICommandUnitSelect Instantiate(UICommandUnitSelect prefab, Transform parent, HashSet<BattleCommand> validCommands, Action<BattleCommand> onDecide, Action onCancel)
		{
			UICommandUnitSelect uICommandUnitSelect = Object.Instantiate<UICommandUnitSelect>(prefab);
			uICommandUnitSelect.get_transform().set_parent(parent);
			uICommandUnitSelect.get_transform().set_localPosition(Vector2.get_down() * 12f);
			uICommandUnitSelect.get_transform().set_localScale(Vector3.get_one() * 0.8f);
			uICommandUnitSelect.Init(validCommands, onDecide, onCancel);
			return uICommandUnitSelect;
		}

		private bool Init(HashSet<BattleCommand> validCommands, Action<BattleCommand> onDecide, Action onCancel)
		{
			this._actOnDecide = onDecide;
			this._actOnCancel = onCancel;
			this.widget.alpha = 0f;
			this._nIndex = 0;
			this._uiInvisibleCollider.SetOnTouch(new Action(this.OnTouchInvisible));
			this.SetCommandUnit(validCommands);
			this.isColliderEnabled = false;
			return true;
		}

		private void SetCommandUnit(HashSet<BattleCommand> validCommands)
		{
			this._listCommandUnitLabel = new List<UICommandLabelButton>();
			using (IEnumerator enumerator = Enum.GetValues(typeof(BattleCommand)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BattleCommand battleCommand = (BattleCommand)((int)enumerator.get_Current());
					if (battleCommand != BattleCommand.None)
					{
						this._listCommandUnitLabel.Add(this._uiCommandAnchor.get_transform().FindChild(string.Format("CommandUnit{0}", (int)battleCommand)).GetComponent<UICommandLabelButton>());
						this._listCommandUnitLabel.get_Item((int)battleCommand).Init((int)battleCommand, validCommands.Contains(battleCommand), battleCommand, null);
						this._listCommandUnitLabel.get_Item((int)battleCommand).toggle.onActive = Util.CreateEventDelegateList(this, "OnActive", this._listCommandUnitLabel.get_Item((int)battleCommand).index);
						this._listCommandUnitLabel.get_Item((int)battleCommand).toggle.onDecide = delegate
						{
							this.OnDecide();
						};
						this._listCommandUnitLabel.get_Item((int)battleCommand).toggle.group = 10;
						this._listCommandUnitLabel.get_Item((int)battleCommand).toggle.set_enabled(this._listCommandUnitLabel.get_Item((int)battleCommand).isValid);
					}
				}
			}
		}

		public void Prev()
		{
			this.PreparaNext(false);
		}

		public void Next()
		{
			this.PreparaNext(true);
		}

		private void PreparaNext(bool isFoward)
		{
			int nIndex = this._nIndex;
			this._nIndex = Mathe.NextElement(this._nIndex, 0, this._listCommandUnitLabel.get_Count() - 1, isFoward, (int x) => this._listCommandUnitLabel.get_Item(x).isValid);
			if (nIndex != this._nIndex)
			{
				this.ChangeFocus(this._nIndex);
			}
		}

		private void ChangeFocus(int nIndex)
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove2);
			this._listCommandUnitLabel.ForEach(delegate(UICommandLabelButton x)
			{
				x.isFocus = (x.index == nIndex);
			});
		}

		public void Show(BattleCommand iCommand, Action onFinished)
		{
			int nIndex = (iCommand != BattleCommand.None) ? this._listCommandUnitLabel.Find((UICommandLabelButton x) => x.battleCommand == iCommand).index : 0;
			this._nIndex = nIndex;
			this._listCommandUnitLabel.ForEach(delegate(UICommandLabelButton x)
			{
				x.isFocus = (x.index == this._nIndex);
			});
			base.get_transform().LTCancel();
			float time = 0.15f;
			LeanTweenType ease = LeanTweenType.linear;
			base.get_transform().set_localScale(Vector3.get_one() * 0.8f);
			base.get_transform().LTScale(Vector3.get_one(), time).setEase(ease);
			base.get_transform().LTValue(this.widget.alpha, 1f, time).setEase(ease).setOnUpdate(delegate(float x)
			{
				this.widget.alpha = x;
			}).setOnComplete(delegate
			{
				this.isColliderEnabled = true;
				Dlg.Call(ref onFinished);
			});
		}

		public void Hide(Action onFinished)
		{
			this.isColliderEnabled = false;
			base.get_transform().LTCancel();
			float time = 0.15f;
			LeanTweenType ease = LeanTweenType.linear;
			base.get_transform().set_localScale(Vector3.get_one());
			base.get_transform().LTScale(Vector3.get_one() * 0.8f, time).setEase(ease);
			base.get_transform().LTValue(this.widget.alpha, 0f, time).setEase(ease).setOnUpdate(delegate(float x)
			{
				this.widget.alpha = x;
			}).setOnComplete(delegate
			{
				Dlg.Call(ref onFinished);
			});
		}

		private void OnTouchInvisible()
		{
			this.OnCancel();
		}

		private void OnActive(int nIndex)
		{
			if (this._nIndex != nIndex)
			{
				this._nIndex = nIndex;
				this.ChangeFocus(nIndex);
			}
		}

		public bool OnDecide()
		{
			Dlg.Call<BattleCommand>(ref this._actOnDecide, this.selectedUnit.battleCommand);
			return true;
		}

		public bool OnCancel()
		{
			Dlg.Call(ref this._actOnCancel);
			return true;
		}
	}
}
