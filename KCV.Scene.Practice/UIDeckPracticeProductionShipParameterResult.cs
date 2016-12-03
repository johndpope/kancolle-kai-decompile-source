using Common.Struct;
using DG.Tweening;
using KCV.Scene.Port;
using local.models;
using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace KCV.Scene.Practice
{
	[RequireComponent(typeof(UIPanel))]
	public class UIDeckPracticeProductionShipParameterResult : MonoBehaviour
	{
		[SerializeField]
		private UIDeckPracticeUpParameter[] mUIDeckPracticeUpParameters;

		[SerializeField]
		private UIDeckPracticeShipInfo mUIDeckPracticeShipInfo;

		[SerializeField]
		private Transform mTransfrom_ShipOffset;

		[SerializeField]
		private Transform mTransform_TouchControlArea;

		[SerializeField]
		private UITexture mTexture_Ship;

		[SerializeField]
		private UITexture mTexture_Bg;

		private UIPanel mPanelThis;

		private Vector3 mVector3_ShipDefaultLocalPosition;

		private KeyControl mKeyController;

		private DeckPracticeResultModel mDeckPracticeResultModel;

		private Action mOnFinishedProductionListener;

		private void Awake()
		{
			this.mPanelThis = base.GetComponent<UIPanel>();
			this.mVector3_ShipDefaultLocalPosition = this.mTransfrom_ShipOffset.get_localPosition();
			this.mTexture_Bg.alpha = 0f;
		}

		public void SetKeyController(KeyControl keyController)
		{
			this.mKeyController = keyController;
		}

		public void SetOnProductionFinishedListener(Action onFinishedProductionListener)
		{
			this.mOnFinishedProductionListener = onFinishedProductionListener;
		}

		private void OnProductionFinished()
		{
			if (this.mOnFinishedProductionListener != null)
			{
				this.mOnFinishedProductionListener.Invoke();
			}
		}

		public void Initialize(DeckPracticeResultModel deckPracticeResultModel)
		{
			this.mDeckPracticeResultModel = deckPracticeResultModel;
		}

		public void SetBackGroundAlpha(float alpha)
		{
			this.mTexture_Bg.alpha = alpha;
		}

		public void StartProduction()
		{
			this.mPanelThis.alpha = 1f;
			IEnumerator enumerator = this.StartProductionCoroutine(this.mDeckPracticeResultModel, delegate
			{
				this.OnProductionFinished();
			});
			base.StartCoroutine(enumerator);
		}

		[DebuggerHidden]
		private IEnumerator TEST_StartProductionCoroutine(object o, Action onFinished)
		{
			UIDeckPracticeProductionShipParameterResult.<TEST_StartProductionCoroutine>c__Iterator14D <TEST_StartProductionCoroutine>c__Iterator14D = new UIDeckPracticeProductionShipParameterResult.<TEST_StartProductionCoroutine>c__Iterator14D();
			<TEST_StartProductionCoroutine>c__Iterator14D.onFinished = onFinished;
			<TEST_StartProductionCoroutine>c__Iterator14D.<$>onFinished = onFinished;
			<TEST_StartProductionCoroutine>c__Iterator14D.<>f__this = this;
			return <TEST_StartProductionCoroutine>c__Iterator14D;
		}

		[DebuggerHidden]
		private IEnumerator StartProductionCoroutine(DeckPracticeResultModel deckPracticeResultModel, Action onFinished)
		{
			UIDeckPracticeProductionShipParameterResult.<StartProductionCoroutine>c__Iterator14E <StartProductionCoroutine>c__Iterator14E = new UIDeckPracticeProductionShipParameterResult.<StartProductionCoroutine>c__Iterator14E();
			<StartProductionCoroutine>c__Iterator14E.deckPracticeResultModel = deckPracticeResultModel;
			<StartProductionCoroutine>c__Iterator14E.onFinished = onFinished;
			<StartProductionCoroutine>c__Iterator14E.<$>deckPracticeResultModel = deckPracticeResultModel;
			<StartProductionCoroutine>c__Iterator14E.<$>onFinished = onFinished;
			<StartProductionCoroutine>c__Iterator14E.<>f__this = this;
			return <StartProductionCoroutine>c__Iterator14E;
		}

		[DebuggerHidden]
		private IEnumerator StartProductionShipResult(ShipModel shipModel, ShipExpModel expModel, PowUpInfo powUpInfo)
		{
			UIDeckPracticeProductionShipParameterResult.<StartProductionShipResult>c__Iterator14F <StartProductionShipResult>c__Iterator14F = new UIDeckPracticeProductionShipParameterResult.<StartProductionShipResult>c__Iterator14F();
			<StartProductionShipResult>c__Iterator14F.shipModel = shipModel;
			<StartProductionShipResult>c__Iterator14F.powUpInfo = powUpInfo;
			<StartProductionShipResult>c__Iterator14F.expModel = expModel;
			<StartProductionShipResult>c__Iterator14F.<$>shipModel = shipModel;
			<StartProductionShipResult>c__Iterator14F.<$>powUpInfo = powUpInfo;
			<StartProductionShipResult>c__Iterator14F.<$>expModel = expModel;
			<StartProductionShipResult>c__Iterator14F.<>f__this = this;
			return <StartProductionShipResult>c__Iterator14F;
		}

		[DebuggerHidden]
		private IEnumerator InitializeCoroutine(ShipModel shipModel, PowUpInfo powUpInfo)
		{
			UIDeckPracticeProductionShipParameterResult.<InitializeCoroutine>c__Iterator150 <InitializeCoroutine>c__Iterator = new UIDeckPracticeProductionShipParameterResult.<InitializeCoroutine>c__Iterator150();
			<InitializeCoroutine>c__Iterator.shipModel = shipModel;
			<InitializeCoroutine>c__Iterator.powUpInfo = powUpInfo;
			<InitializeCoroutine>c__Iterator.<$>shipModel = shipModel;
			<InitializeCoroutine>c__Iterator.<$>powUpInfo = powUpInfo;
			<InitializeCoroutine>c__Iterator.<>f__this = this;
			return <InitializeCoroutine>c__Iterator;
		}

		[Obsolete("インスペクタ上のUIButtonに紐付けて使用します。")]
		public void OnTouchNext()
		{
			this.OnCallNext();
		}

		private void OnCallNext()
		{
			if (this.mKeyController != null && this.mKeyController.IsRun)
			{
				this.mTransform_TouchControlArea.SetActive(false);
				this.mKeyController.IsRun = false;
			}
		}

		[DebuggerHidden]
		private IEnumerator WaitForKeyOrTouch(KeyControl keyController, Action onKeyAction)
		{
			UIDeckPracticeProductionShipParameterResult.<WaitForKeyOrTouch>c__Iterator151 <WaitForKeyOrTouch>c__Iterator = new UIDeckPracticeProductionShipParameterResult.<WaitForKeyOrTouch>c__Iterator151();
			<WaitForKeyOrTouch>c__Iterator.keyController = keyController;
			<WaitForKeyOrTouch>c__Iterator.onKeyAction = onKeyAction;
			<WaitForKeyOrTouch>c__Iterator.<$>keyController = keyController;
			<WaitForKeyOrTouch>c__Iterator.<$>onKeyAction = onKeyAction;
			<WaitForKeyOrTouch>c__Iterator.<>f__this = this;
			return <WaitForKeyOrTouch>c__Iterator;
		}

		private void ResetPositionShip()
		{
			this.mTransfrom_ShipOffset.set_localPosition(this.mVector3_ShipDefaultLocalPosition);
		}

		private void Chain(Action onFinished, Action<Action> chainFrom, Action<Action> chainTo)
		{
			chainFrom.Invoke(delegate
			{
				chainTo.Invoke(delegate
				{
					onFinished.Invoke();
				});
			});
		}

		private void Chain(params Action<Action>[] actions)
		{
			Action<Action> chainFrom = null;
			Action<Action> chainTo = null;
			int num = actions.Length;
			if (2 <= num)
			{
				chainFrom = actions[0];
				chainTo = actions[1];
				Array.Clear(actions, 0, 2);
			}
			else if (1 <= num)
			{
				chainFrom = actions[0];
				chainTo = null;
				Array.Clear(actions, 0, 1);
			}
			actions = Enumerable.ToArray<Action<Action>>(Enumerable.Where<Action<Action>>(actions, (Action<Action> action) => action != null));
			this.Chain(delegate
			{
				this.Chain(actions);
			}, chainFrom, chainTo);
		}

		private Tween GenerateTweenParameterResult()
		{
			Sequence sequence = TweenSettingsExtensions.SetId<Sequence>(DOTween.Sequence(), this);
			float num = 0.1f;
			float num2 = (0.6f - num) / (float)this.mUIDeckPracticeUpParameters.Length;
			for (int i = 0; i < this.mUIDeckPracticeUpParameters.Length; i++)
			{
				Tween tween = TweenSettingsExtensions.SetId<Tween>(this.mUIDeckPracticeUpParameters[i].GenerateParameterUpAnimation(num), this);
				TweenSettingsExtensions.SetDelay<Tween>(tween, num2 / 2f);
				TweenSettingsExtensions.Join(sequence, tween);
			}
			return sequence;
		}

		private Tween GenerateTweenShipIn()
		{
			return TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMove(this.mTransfrom_ShipOffset, new Vector3(290f, 70f), 0.6f, false), 18);
		}

		private Tween GenerateTweenShipOut()
		{
			int num = Random.Range(0, 2);
			if (num == 0)
			{
				return this.GenerateTweenShipLeftBottomOutAlphaOut();
			}
			if (num != 1)
			{
				return null;
			}
			return this.GenerateTweenShipOutAlphInaScaleIn();
		}

		private Tween GenerateTweenShipLeftBottomOutAlphaOut()
		{
			float defaultHeight = (float)this.mTexture_Ship.height;
			float slotOutHeight = defaultHeight * 0.5f;
			return TweenSettingsExtensions.OnComplete<Tweener>(DOVirtual.Float(0f, 1f, 0.3f, delegate(float percentage)
			{
				this.mTexture_Ship.height = (int)(defaultHeight + slotOutHeight * percentage);
				this.mTexture_Ship.alpha = 1f - percentage;
			}), delegate
			{
				this.mTexture_Ship.alpha = 1f;
				this.mTexture_Ship.height = (int)defaultHeight;
				this.mTransfrom_ShipOffset.set_localPosition(this.mVector3_ShipDefaultLocalPosition);
			});
		}

		private Tween GenerateTweenShipOutAlphInaScaleIn()
		{
			Sequence sequence = DOTween.Sequence();
			TweenSettingsExtensions.Append(sequence, DOVirtual.Float(0f, 1f, 0.3f, delegate(float percentage)
			{
				this.mTexture_Ship.alpha = 1f - percentage;
			}));
			TweenSettingsExtensions.Join(sequence, ShortcutExtensions.DOScale(this.mTransfrom_ShipOffset, new Vector3(1.2f, 1.2f, 1.2f), 0.3f));
			TweenSettingsExtensions.OnComplete<Sequence>(sequence, delegate
			{
				this.mTexture_Ship.alpha = 1f;
				this.mTransfrom_ShipOffset.set_localScale(Vector3.get_one());
				this.mTransfrom_ShipOffset.set_localPosition(this.mVector3_ShipDefaultLocalPosition);
			});
			return sequence;
		}

		private void ResetPositionParameterSlot()
		{
			UIDeckPracticeUpParameter[] array = this.mUIDeckPracticeUpParameters;
			for (int i = 0; i < array.Length; i++)
			{
				UIDeckPracticeUpParameter uIDeckPracticeUpParameter = array[i];
				uIDeckPracticeUpParameter.Reposition();
			}
		}

		private void OnDestroy()
		{
			this.mOnFinishedProductionListener = null;
			this.mUIDeckPracticeUpParameters = null;
			this.mUIDeckPracticeShipInfo = null;
			this.mTransfrom_ShipOffset = null;
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_Ship, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_Bg, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mPanelThis);
		}
	}
}
