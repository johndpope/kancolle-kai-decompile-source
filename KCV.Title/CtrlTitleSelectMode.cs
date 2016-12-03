using Common.SaveManager;
using KCV.Utils;
using LT.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace KCV.Title
{
	[RequireComponent(typeof(UIPanel))]
	public class CtrlTitleSelectMode : MonoBehaviour
	{
		[SerializeField]
		private List<UISelectModeBtn> _listSelectMode;

		[SerializeField]
		private Vector3 _vPos = new Vector3(-263f, -168f, 0f);

		private SelectMode _iSelectMode;

		private bool _isInputPossible;

		private UIPanel _uiPanel;

		private Action _actOnAnyInput;

		private Action<SelectMode> _actOnDecide;

		public UIPanel panel
		{
			get
			{
				return this.GetComponentThis(ref this._uiPanel);
			}
		}

		private int maxIndex
		{
			get
			{
				return (!VitaSaveManager.Instance.IsAllEmpty()) ? 1 : 0;
			}
		}

		public static CtrlTitleSelectMode Instantiate(CtrlTitleSelectMode prefab, Transform parent, Action onAnyInput)
		{
			CtrlTitleSelectMode ctrlTitleSelectMode = Object.Instantiate<CtrlTitleSelectMode>(prefab);
			ctrlTitleSelectMode.get_transform().set_parent(parent);
			ctrlTitleSelectMode.get_transform().localScaleOne();
			ctrlTitleSelectMode.get_transform().set_localPosition(ctrlTitleSelectMode._vPos);
			ctrlTitleSelectMode.Init(onAnyInput);
			return ctrlTitleSelectMode;
		}

		private bool Init(Action onAnyInput)
		{
			this._actOnAnyInput = onAnyInput;
			this.panel.alpha = 0f;
			using (IEnumerator enumerator = Enum.GetValues(typeof(SelectMode)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SelectMode selectMode = (SelectMode)((int)enumerator.get_Current());
					this._listSelectMode.get_Item((int)selectMode).Init(selectMode);
					this._listSelectMode.get_Item((int)selectMode).isFocus = false;
					this._listSelectMode.get_Item((int)selectMode).toggle.startsActive = false;
					this._listSelectMode.get_Item((int)selectMode).toggle.onDecide = new Action(this.DecideAnim);
					this._listSelectMode.get_Item((int)selectMode).toggle.onActive = Util.CreateEventDelegateList(this, "OnActive", selectMode);
					this._listSelectMode.get_Item((int)selectMode).toggle.set_enabled(false);
					if (this.maxIndex == 0 && selectMode == SelectMode.Inheriting)
					{
						this._listSelectMode.get_Item((int)selectMode).transform.set_localPosition(Vector2.get_left() * 9999f);
					}
				}
			}
			if (this.maxIndex == 1)
			{
				this._listSelectMode.get_Item(1).toggle.startsActive = true;
			}
			else
			{
				this._listSelectMode.get_Item(0).transform.localPositionZero();
				this._listSelectMode.get_Item(0).toggle.startsActive = true;
				this.ChangeFocus(SelectMode.Appointed, false);
			}
			return true;
		}

		private void OnDestroy()
		{
			Mem.DelListSafe<UISelectModeBtn>(ref this._listSelectMode);
			Mem.Del<Vector3>(ref this._vPos);
			Mem.Del<SelectMode>(ref this._iSelectMode);
			Mem.Del<bool>(ref this._isInputPossible);
			Mem.Del<UIPanel>(ref this._uiPanel);
			Mem.Del<Action<SelectMode>>(ref this._actOnDecide);
			Mem.Del<Action>(ref this._actOnAnyInput);
		}

		public void Play(Action<SelectMode> onDecideMode)
		{
			this._actOnDecide = onDecideMode;
			this.Show().setOnComplete(delegate
			{
				this._isInputPossible = true;
				this._listSelectMode.ForEach(delegate(UISelectModeBtn x)
				{
					x.toggle.set_enabled(true);
				});
			});
		}

		public bool Run()
		{
			if (this._isInputPossible)
			{
				KeyControl keyControl = TitleTaskManager.GetKeyControl();
				if (keyControl.GetDown(KeyControl.KeyName.MARU))
				{
					this.DecideAnim();
				}
				else if (keyControl.GetDown(KeyControl.KeyName.DOWN))
				{
					this.PreparaNext(false);
				}
				else if (keyControl.GetDown(KeyControl.KeyName.UP))
				{
					this.PreparaNext(true);
				}
			}
			return true;
		}

		private void PreparaNext(bool isFoward)
		{
			Dlg.Call(ref this._actOnAnyInput);
			SelectMode iSelectMode = this._iSelectMode;
			this._iSelectMode = (SelectMode)Mathe.NextElement((int)this._iSelectMode, 0, this.maxIndex, isFoward);
			if (iSelectMode != this._iSelectMode)
			{
				this.ChangeFocus(this._iSelectMode, true);
			}
		}

		private void ChangeFocus(SelectMode iMode, bool isPlaySE)
		{
			if (isPlaySE)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			}
			this._listSelectMode.ForEach(delegate(UISelectModeBtn x)
			{
				x.isFocus = (iMode == x.mode);
			});
		}

		private LTDescr Show()
		{
			return this.panel.get_transform().LTValue(0f, 1f, 0.2f).setOnUpdate(delegate(float x)
			{
				this.panel.alpha = x;
			}).setEase(LeanTweenType.linear);
		}

		private LTDescr Hide()
		{
			return this.panel.get_transform().LTValue(1f, 0f, 1f).setOnUpdate(delegate(float x)
			{
				this.panel.alpha = x;
			}).setEase(LeanTweenType.linear);
		}

		private void DecideAnim()
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			this._listSelectMode.ForEach(delegate(UISelectModeBtn x)
			{
				x.toggle.set_enabled(false);
			});
			this._isInputPossible = false;
			this._listSelectMode.ForEach(delegate(UISelectModeBtn x)
			{
				if (x.mode == this._iSelectMode)
				{
					x.transform.LTMoveLocal(Vector3.get_zero(), 0.5f).setEase(LeanTweenType.easeOutSine).setOnComplete(delegate
					{
						this.OnDecide(this._iSelectMode);
					});
				}
				else
				{
					x.transform.LTValue(1f, 0f, 0.3f).setOnUpdate(delegate(float a)
					{
						x.alpha = a;
					});
				}
			});
		}

		private void OnActive(SelectMode iMode)
		{
			Dlg.Call(ref this._actOnAnyInput);
			if (this._iSelectMode != iMode)
			{
				this._iSelectMode = iMode;
				this.ChangeFocus(iMode, false);
			}
		}

		private void OnDecide(SelectMode iMode)
		{
			Dlg.Call(ref this._actOnAnyInput);
			Observable.Timer(TimeSpan.FromSeconds(0.30000001192092896)).Subscribe(delegate(long _)
			{
				this.Hide().setOnComplete(delegate
				{
					Dlg.Call<SelectMode>(ref this._actOnDecide, this._iSelectMode);
				});
			});
		}
	}
}
