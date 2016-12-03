using KCV.Generic;
using LT.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace KCV.BattleCut
{
	public class ProdBCWithdrawalDecision : MonoBehaviour
	{
		[SerializeField]
		private List<UILabelButton> _listLabelButton;

		private int _nIndex;

		private bool _isInputPossible;

		private UIPanel _uiPanel;

		private Action<int> _actCallback;

		public UIPanel panel
		{
			get
			{
				return this.GetComponentThis(ref this._uiPanel);
			}
		}

		public int index
		{
			get
			{
				return this._nIndex;
			}
		}

		public static ProdBCWithdrawalDecision Instantiate(ProdBCWithdrawalDecision prefab, Transform parent)
		{
			ProdBCWithdrawalDecision prodBCWithdrawalDecision = Object.Instantiate<ProdBCWithdrawalDecision>(prefab);
			prodBCWithdrawalDecision.get_transform().set_parent(parent);
			prodBCWithdrawalDecision.get_transform().localPositionZero();
			prodBCWithdrawalDecision.get_transform().localScaleOne();
			prodBCWithdrawalDecision.Init();
			return prodBCWithdrawalDecision;
		}

		private void OnDestroy()
		{
			Mem.DelListSafe<UILabelButton>(ref this._listLabelButton);
			Mem.Del<int>(ref this._nIndex);
			Mem.Del<bool>(ref this._isInputPossible);
			Mem.Del<UIPanel>(ref this._uiPanel);
			Mem.Del<Action<int>>(ref this._actCallback);
		}

		private bool Init()
		{
			this.panel.alpha = 0f;
			this._nIndex = 0;
			int cnt = 0;
			this._listLabelButton.ForEach(delegate(UILabelButton x)
			{
				x.Init(cnt, true, KCVColor.ConvertColor(110f, 110f, 110f, 255f));
				x.isFocus = false;
				x.toggle.group = 10;
				x.toggle.set_enabled(false);
				x.toggle.onDecide = delegate
				{
					this.OnDecide(x.index);
				};
				x.toggle.onActive = Util.CreateEventDelegateList(this, "OnActive", x.index);
				if (x.index == 0)
				{
					x.toggle.startsActive = true;
				}
				cnt++;
			});
			this._isInputPossible = false;
			return true;
		}

		public ProdBCWithdrawalDecision Play(Action<int> onFinished)
		{
			this._actCallback = onFinished;
			Observable.FromCoroutine(new Func<IEnumerator>(this.PlayShowAnim), false).Subscribe(delegate(Unit _)
			{
				KeyControl keyControl = BattleCutManager.GetKeyControl();
				keyControl.IsRun = true;
				this.ChangeFocus(this._nIndex);
				this._listLabelButton.ForEach(delegate(UILabelButton x)
				{
					x.toggle.set_enabled(true);
				});
				this._isInputPossible = true;
			}).AddTo(base.get_gameObject());
			return this;
		}

		[DebuggerHidden]
		private IEnumerator PlayShowAnim()
		{
			ProdBCWithdrawalDecision.<PlayShowAnim>c__Iterator10E <PlayShowAnim>c__Iterator10E = new ProdBCWithdrawalDecision.<PlayShowAnim>c__Iterator10E();
			<PlayShowAnim>c__Iterator10E.<>f__this = this;
			return <PlayShowAnim>c__Iterator10E;
		}

		public bool Run()
		{
			KeyControl keyControl = BattleCutManager.GetKeyControl();
			if (this._isInputPossible)
			{
				if (keyControl.GetDown(KeyControl.KeyName.LEFT))
				{
					this.PreparaNext(false);
				}
				else if (keyControl.GetDown(KeyControl.KeyName.RIGHT))
				{
					this.PreparaNext(true);
				}
				if (keyControl.GetDown(KeyControl.KeyName.MARU))
				{
					this.OnDecide(this._nIndex);
					return true;
				}
			}
			return true;
		}

		private void Show(Action onFinished)
		{
			LeanTween.value(base.get_gameObject(), 0f, 1f, 0.35f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				this.panel.alpha = x;
			}).setOnComplete(delegate
			{
				Dlg.Call(ref onFinished);
			});
		}

		private void Hide(Action onFinished)
		{
			LeanTween.value(base.get_gameObject(), 1f, 0f, Defines.PHASE_FADE_TIME).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				this.panel.alpha = x;
			}).setOnComplete(delegate
			{
				Dlg.Call(ref onFinished);
			});
		}

		private void PreparaNext(bool isFoward)
		{
			int nIndex = this._nIndex;
			this._nIndex = Mathe.NextElement(this._nIndex, 0, 1, isFoward);
			if (nIndex != this._nIndex)
			{
				this.ChangeFocus(this._nIndex);
			}
		}

		private void ChangeFocus(int nIndex)
		{
			this._listLabelButton.ForEach(delegate(UILabelButton x)
			{
				x.isFocus = (x.index == nIndex);
			});
		}

		private void OnActive(int nIndex)
		{
			if (this._nIndex != nIndex)
			{
				this._nIndex = nIndex;
				this.ChangeFocus(this._nIndex);
			}
		}

		private void OnDecide(int nIndex)
		{
			UIBattleCutNavigation navigation = BattleCutManager.GetNavigation();
			this._nIndex = nIndex;
			this._isInputPossible = false;
			this._listLabelButton.ForEach(delegate(UILabelButton x)
			{
				x.toggle.set_enabled(false);
			});
			this.Hide(delegate
			{
				if (this._actCallback != null)
				{
					this._actCallback.Invoke(this._nIndex);
				}
			});
			navigation.Hide(Defines.PHASE_FADE_TIME, null);
			BattleCutManager.GetStateBattle().prodBCBattle.Hide(Defines.PHASE_FADE_TIME);
		}
	}
}
