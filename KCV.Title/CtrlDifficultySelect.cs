using Common.Enum;
using KCV.Utils;
using LT.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace KCV.Title
{
	[RequireComponent(typeof(UIPanel))]
	public class CtrlDifficultySelect : MonoBehaviour
	{
		[SerializeField]
		private Transform _prefabUIDifficultBtn;

		[SerializeField]
		private UIWritingBrush _prefabUIWritingBrush;

		[SerializeField]
		private UIGrid _uiGridBtns;

		[SerializeField]
		private UIGrid _uiGridDifficulty;

		[SerializeField]
		private List<float> _listArrowLength;

		[SerializeField]
		private UISprite _uiArrow;

		[SerializeField]
		private UITexture _uiBackground;

		[SerializeField]
		private UILabel _uiDescription;

		[SerializeField]
		private UIInvisibleCollider _uiInvisibleCollider;

		private UIPanel _uiPanel;

		private bool _isInputPossible;

		private int _nIndex;

		private Action _actOnCancel;

		private Action<DifficultKind> _actOnDecideDifficulty;

		private List<UIDifficultyBtn> _listDifficultyBtn;

		private KeyControl _clsInput;

		public UIPanel panel
		{
			get
			{
				return this.GetComponentThis(ref this._uiPanel);
			}
		}

		public static CtrlDifficultySelect Instantiate(CtrlDifficultySelect prefab, Transform parent, KeyControl input, HashSet<DifficultKind> difficulty, Action<DifficultKind> onDecideDifficulty, Action onCancel)
		{
			CtrlDifficultySelect ctrlDifficultySelect = Object.Instantiate<CtrlDifficultySelect>(prefab);
			ctrlDifficultySelect.get_transform().set_parent(parent);
			ctrlDifficultySelect.get_transform().localScaleOne();
			ctrlDifficultySelect.get_transform().localPositionZero();
			ctrlDifficultySelect.Init(input, difficulty, onDecideDifficulty, onCancel);
			return ctrlDifficultySelect;
		}

		private bool Init(KeyControl input, HashSet<DifficultKind> difficulty, Action<DifficultKind> onDecideDifficulty, Action onCancel)
		{
			this.panel.alpha = 0f;
			this._actOnDecideDifficulty = onDecideDifficulty;
			this._actOnCancel = onCancel;
			this._nIndex = 0;
			this._isInputPossible = false;
			this._clsInput = input;
			this.CreateDifficultyBtns(this.GetEnabledDifficulty(difficulty));
			this.SetArrowLength(this._listDifficultyBtn.get_Count());
			this.SetDifficultyLabelSpace(this._listDifficultyBtn.get_Count());
			this._listDifficultyBtn.get_Item(0).isFocus = true;
			this._uiInvisibleCollider.SetOnTouch(new Action(this.OnCancel));
			this._uiInvisibleCollider.button.set_enabled(false);
			this.Show().setOnComplete(delegate
			{
				this._listDifficultyBtn.ForEach(delegate(UIDifficultyBtn x)
				{
					x.toggle.set_enabled(true);
				});
				this._uiInvisibleCollider.button.set_enabled(true);
				this._isInputPossible = true;
			});
			return true;
		}

		private void CreateDifficultyBtns(List<DifficultKind> list)
		{
			this._listDifficultyBtn = new List<UIDifficultyBtn>();
			int cnt = 0;
			list.ForEach(delegate(DifficultKind x)
			{
				this._listDifficultyBtn.Add(UIDifficultyBtn.Instantiate(this._prefabUIDifficultBtn.GetComponent<UIDifficultyBtn>(), this._uiGridBtns.get_transform(), x, cnt));
				this._listDifficultyBtn.get_Item(cnt).set_name(string.Format("DifficultyBtn{0}", (int)x));
				this._listDifficultyBtn.get_Item(cnt).isFocus = false;
				this._listDifficultyBtn.get_Item(cnt).toggle.group = 1;
				this._listDifficultyBtn.get_Item(cnt).toggle.set_enabled(false);
				this._listDifficultyBtn.get_Item(cnt).toggle.onDecide = delegate
				{
					this.OnDecideDifficulty(this._listDifficultyBtn.get_Item(this._nIndex).difficultKind);
				};
				this._listDifficultyBtn.get_Item(cnt).toggle.onActive = Util.CreateEventDelegateList(this, "OnActive", cnt);
				if (this._listDifficultyBtn.get_Item(cnt).index == 0)
				{
					this._listDifficultyBtn.get_Item(cnt).toggle.startsActive = true;
				}
				cnt++;
			});
			this._uiGridBtns.Reposition();
		}

		private void OnDestroy()
		{
			Mem.Del<Transform>(ref this._prefabUIDifficultBtn);
			Mem.Del<UIGrid>(ref this._uiGridBtns);
			Mem.Del<UIGrid>(ref this._uiGridDifficulty);
			Mem.DelListSafe<float>(ref this._listArrowLength);
			Mem.Del(ref this._uiArrow);
			Mem.Del<UITexture>(ref this._uiBackground);
			Mem.Del<UILabel>(ref this._uiDescription);
			Mem.Del<UIInvisibleCollider>(ref this._uiInvisibleCollider);
			Mem.Del<UIPanel>(ref this._uiPanel);
			Mem.Del<bool>(ref this._isInputPossible);
			Mem.Del<Action>(ref this._actOnCancel);
			Mem.Del<Action<DifficultKind>>(ref this._actOnDecideDifficulty);
			Mem.DelListSafe<UIDifficultyBtn>(ref this._listDifficultyBtn);
			Mem.Del<KeyControl>(ref this._clsInput);
		}

		public bool Run()
		{
			if (!this._isInputPossible)
			{
				return true;
			}
			if (this._clsInput == null)
			{
				return true;
			}
			if (this._clsInput.GetDown(KeyControl.KeyName.RIGHT))
			{
				this.PreparaNext(true);
			}
			else if (this._clsInput.GetDown(KeyControl.KeyName.LEFT))
			{
				this.PreparaNext(false);
			}
			else if (this._clsInput.GetDown(KeyControl.KeyName.MARU))
			{
				this.OnDecideDifficulty(this._listDifficultyBtn.get_Item(this._nIndex).difficultKind);
			}
			else if (this._clsInput.GetDown(KeyControl.KeyName.BATU))
			{
				this.OnCancel();
			}
			return true;
		}

		private void PreparaNext(bool isFoward)
		{
			int nIndex = this._nIndex;
			this._nIndex = Mathe.NextElement(this._nIndex, 0, this._listDifficultyBtn.get_Count() - 1, isFoward);
			if (nIndex != this._nIndex)
			{
				this.ChangeFocus(this._nIndex, true);
			}
		}

		private void SetArrowLength(int nDiffCnt)
		{
			this._uiArrow.width = 436 + (nDiffCnt - 3) * 130;
		}

		private void SetDifficultyLabelSpace(int nDiffCnt)
		{
			this._uiGridDifficulty.cellWidth = (float)(380 + (nDiffCnt - 3) * 130);
			this._uiGridDifficulty.Reposition();
		}

		private void ChangeFocus(int nIndex, bool isPlaySE)
		{
			if (isPlaySE)
			{
				TitleUtils.PlayDifficultyVoice(this._listDifficultyBtn.get_Item(this._nIndex).difficultKind, false, null);
			}
			this._listDifficultyBtn.ForEach(delegate(UIDifficultyBtn x)
			{
				x.isFocus = (x.index == nIndex);
			});
		}

		private List<DifficultKind> GetEnabledDifficulty(HashSet<DifficultKind> difficulty)
		{
			List<DifficultKind> list = new List<DifficultKind>();
			if (difficulty.Contains(DifficultKind.TEI))
			{
				list.Add(DifficultKind.TEI);
			}
			if (difficulty.Contains(DifficultKind.HEI))
			{
				list.Add(DifficultKind.HEI);
			}
			if (difficulty.Contains(DifficultKind.OTU))
			{
				list.Add(DifficultKind.OTU);
			}
			if (difficulty.Contains(DifficultKind.KOU))
			{
				list.Add(DifficultKind.KOU);
			}
			if (difficulty.Contains(DifficultKind.SHI))
			{
				list.Add(DifficultKind.SHI);
			}
			return list;
		}

		private LTDescr Show()
		{
			TitleUtils.PlayOpenDifficultyVoice();
			return this.panel.get_transform().LTValue(0f, 1f, 0.5f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				this.panel.alpha = x;
			});
		}

		private LTDescr Hide()
		{
			this._isInputPossible = false;
			this._listDifficultyBtn.ForEach(delegate(UIDifficultyBtn x)
			{
				x.toggle.set_enabled(false);
			});
			this._uiInvisibleCollider.button.set_enabled(false);
			return this.panel.get_transform().LTValue(1f, 0f, 0.35f).setEase(LeanTweenType.easeInBack).setOnUpdate(delegate(float x)
			{
				this.panel.alpha = x;
			});
		}

		[DebuggerHidden]
		private IEnumerator PlayDecideAnimation(IObserver<bool> observer)
		{
			CtrlDifficultySelect.<PlayDecideAnimation>c__Iterator1A0 <PlayDecideAnimation>c__Iterator1A = new CtrlDifficultySelect.<PlayDecideAnimation>c__Iterator1A0();
			<PlayDecideAnimation>c__Iterator1A.observer = observer;
			<PlayDecideAnimation>c__Iterator1A.<$>observer = observer;
			<PlayDecideAnimation>c__Iterator1A.<>f__this = this;
			return <PlayDecideAnimation>c__Iterator1A;
		}

		private void OnCancel()
		{
			this.Hide().setOnComplete(delegate
			{
				Dlg.Call(ref this._actOnCancel);
			});
		}

		private void OnActive(int nIndex)
		{
			if (this._nIndex != nIndex)
			{
				this._nIndex = nIndex;
				this.ChangeFocus(this._nIndex, true);
			}
		}

		private void OnDecideDifficulty(DifficultKind iKind)
		{
			this._listDifficultyBtn.ForEach(delegate(UIDifficultyBtn x)
			{
				x.toggle.set_enabled(false);
			});
			this._isInputPossible = false;
			this._uiInvisibleCollider.button.set_enabled(false);
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			TitleUtils.PlayDifficultyVoice(iKind, true, delegate
			{
				Dlg.Call<DifficultKind>(ref this._actOnDecideDifficulty, iKind);
			});
			Observable.FromCoroutine<bool>((IObserver<bool> observer) => this.PlayDecideAnimation(observer)).Subscribe(delegate(bool _)
			{
			}).AddTo(base.get_gameObject());
		}
	}
}
