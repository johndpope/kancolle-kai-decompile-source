using Common.Enum;
using local.models;
using local.utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Battle.Formation
{
	[RequireComponent(typeof(UIPanel))]
	public class UIBattleFormationSelector : MonoBehaviour
	{
		public delegate void UIBattleFormationSelectorAction(UIBattleFormationSelector calledObject, BattleFormationKinds1 selectedFormation);

		private const float MOVE_ANIMATION_TIME = 0.5f;

		[SerializeField]
		private UISprite[] mSprites_Formations;

		private UISprite mSpriteCurrentFormation;

		private UIPanel mPanelThis;

		private Vector3[] mVector3DefaultPosition;

		private Vector3[] mVector3DefaultScale;

		private HashSet<BattleFormationKinds1> mSelectableFormations;

		private UIBattleFormationSelector.UIBattleFormationSelectorAction mUIBattleFormationSelectorAction;

		private KeyControl mKeyController;

		public void SetOnUIBattleFormationSelectorAction(UIBattleFormationSelector.UIBattleFormationSelectorAction callBack)
		{
			this.mUIBattleFormationSelectorAction = callBack;
		}

		private void CallBackAction(UIBattleFormationSelector calledObject, BattleFormationKinds1 selectedFormation)
		{
			if (this.mUIBattleFormationSelectorAction != null)
			{
				this.mUIBattleFormationSelectorAction(this, selectedFormation);
			}
		}

		private void Awake()
		{
			this.mPanelThis = base.GetComponent<UIPanel>();
			this.mPanelThis.alpha = 0.01f;
		}

		public void Initialize(DeckModel deck)
		{
			this.mSelectableFormations = DeckUtil.GetSelectableFormations(deck);
			this.mVector3DefaultPosition = new Vector3[this.mSprites_Formations.Length];
			this.mVector3DefaultScale = new Vector3[this.mSprites_Formations.Length];
			int num = 0;
			UISprite[] array = this.mSprites_Formations;
			for (int i = 0; i < array.Length; i++)
			{
				UISprite uISprite = array[i];
				uISprite.alpha = 0.01f;
				this.mVector3DefaultPosition[num] = uISprite.get_transform().get_localPosition();
				this.mVector3DefaultScale[num] = uISprite.get_transform().get_localScale();
				uISprite.get_transform().set_localScale(new Vector3(0.01f, 0.01f, 0.01f));
				num++;
			}
			this.ChangeFocusIndex(0);
		}

		private void Update()
		{
			if (this.mKeyController != null)
			{
				if (this.mKeyController.keyState.get_Item(14).down)
				{
					int currentFocusFormationIndex = Array.IndexOf<UISprite>(this.mSprites_Formations, this.mSpriteCurrentFormation);
					this.ChangeNextFormation(currentFocusFormationIndex);
				}
				else if (this.mKeyController.keyState.get_Item(1).down)
				{
					this.SelectFormation();
				}
			}
		}

		private void SelectFormation()
		{
			base.StartCoroutine(this.SelectFormationCoroutine());
		}

		[DebuggerHidden]
		private IEnumerator SelectFormationCoroutine()
		{
			UIBattleFormationSelector.<SelectFormationCoroutine>c__Iterator1C0 <SelectFormationCoroutine>c__Iterator1C = new UIBattleFormationSelector.<SelectFormationCoroutine>c__Iterator1C0();
			<SelectFormationCoroutine>c__Iterator1C.<>f__this = this;
			return <SelectFormationCoroutine>c__Iterator1C;
		}

		public KeyControl GetKeyController()
		{
			this.mKeyController = new KeyControl(0, 0, 0.4f, 0.1f);
			return this.mKeyController;
		}

		public void RemoveKeyController()
		{
			this.mKeyController = null;
		}

		public void Show(Action shownCallBack)
		{
			this.mPanelThis.alpha = 1f;
			float duration = 1f;
			base.StartCoroutine(this.ShowCoroutine(shownCallBack, duration));
		}

		[DebuggerHidden]
		private IEnumerator ShowCoroutine(Action shownCallBack, float duration)
		{
			UIBattleFormationSelector.<ShowCoroutine>c__Iterator1C1 <ShowCoroutine>c__Iterator1C = new UIBattleFormationSelector.<ShowCoroutine>c__Iterator1C1();
			<ShowCoroutine>c__Iterator1C.duration = duration;
			<ShowCoroutine>c__Iterator1C.shownCallBack = shownCallBack;
			<ShowCoroutine>c__Iterator1C.<$>duration = duration;
			<ShowCoroutine>c__Iterator1C.<$>shownCallBack = shownCallBack;
			<ShowCoroutine>c__Iterator1C.<>f__this = this;
			return <ShowCoroutine>c__Iterator1C;
		}

		public void Hide(Action hiddenCallBack)
		{
			int num = 0;
			UISprite[] array = this.mSprites_Formations;
			for (int i = 0; i < array.Length; i++)
			{
				UISprite uISprite = array[i];
				TweenAlpha tweenAlpha = UITweener.Begin<TweenAlpha>(uISprite.get_gameObject(), 0.5f);
				tweenAlpha.from = uISprite.alpha;
				tweenAlpha.to = 0.01f;
				tweenAlpha.PlayForward();
				if (num == this.mSprites_Formations.Length - 1)
				{
					tweenAlpha.SetOnFinished(delegate
					{
						if (hiddenCallBack != null)
						{
							hiddenCallBack.Invoke();
						}
					});
				}
				num++;
			}
		}

		private void ChangeNextFormation(int currentFocusFormationIndex)
		{
			int num = this.mSprites_Formations.Length;
			int loopIndex = this.GetLoopIndex(num, currentFocusFormationIndex + 1);
			int loopIndex2 = this.GetLoopIndex(num, num - 1 - currentFocusFormationIndex);
			this.ChangeFocusIndex(loopIndex);
			for (int i = 0; i < num; i++)
			{
				int loopIndex3 = this.GetLoopIndex(num, loopIndex2 + i);
				Hashtable args = this.GenerateMoveAnimationHash(this.mVector3DefaultPosition[loopIndex3]);
				Hashtable args2 = this.GenerateScaleAnimationHash(this.mVector3DefaultScale[loopIndex3]);
				iTween.MoveTo(this.mSprites_Formations[i].get_gameObject(), args);
				iTween.ScaleTo(this.mSprites_Formations[i].get_gameObject(), args2);
			}
		}

		private void ChangePrevFormation(int currentFocusFormationIndex)
		{
			int num = this.mSprites_Formations.Length;
			int loopIndex = this.GetLoopIndex(num, currentFocusFormationIndex + 1);
			int loopIndex2 = this.GetLoopIndex(num, num - 1 - currentFocusFormationIndex);
			this.ChangeFocusIndex(loopIndex);
			for (int i = 0; i < num; i++)
			{
				int loopIndex3 = this.GetLoopIndex(num, loopIndex2 + i);
				Hashtable args = this.GenerateMoveAnimationHash(this.mVector3DefaultPosition[loopIndex3]);
				Hashtable args2 = this.GenerateScaleAnimationHash(this.mVector3DefaultScale[loopIndex3]);
				iTween.MoveTo(this.mSprites_Formations[i].get_gameObject(), args);
				iTween.ScaleTo(this.mSprites_Formations[i].get_gameObject(), args2);
			}
		}

		private int GetLoopIndex(int arrayLength, int value)
		{
			if (value == 0)
			{
				return 0;
			}
			if (0 < value)
			{
				return value % arrayLength;
			}
			return arrayLength + value % arrayLength;
		}

		private void ChangeFocusIndex(int index)
		{
			this.mSpriteCurrentFormation = this.mSprites_Formations[index];
		}

		private Hashtable GenerateMoveAnimationHash(Vector3 toPosition)
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add("x", toPosition.x);
			hashtable.Add("y", toPosition.y);
			hashtable.Add("time", 0.5f);
			hashtable.Add("isLocal", true);
			hashtable.Add("ignoreTimeScale", false);
			hashtable.Add("easetype", iTween.EaseType.easeInOutExpo);
			return hashtable;
		}

		private Hashtable GenerateScaleAnimationHash(Vector3 toScale)
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add("x", toScale.x);
			hashtable.Add("y", toScale.y);
			hashtable.Add("isLocal", true);
			hashtable.Add("time", 0.5f);
			hashtable.Add("easetype", iTween.EaseType.easeInBack);
			return hashtable;
		}

		private Hashtable GenerateShowScaleAnimationHash(Vector3 toScale, float duration)
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add("x", toScale.x);
			hashtable.Add("y", toScale.y);
			hashtable.Add("isLocal", true);
			hashtable.Add("time", duration);
			hashtable.Add("easetype", iTween.EaseType.easeOutElastic);
			hashtable.Add("ignoretimescale", false);
			return hashtable;
		}

		private Hashtable GenerateSelectedScaleAnimationHash(Vector3 toScale, float duration)
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add("x", toScale.x);
			hashtable.Add("y", toScale.y);
			hashtable.Add("isLocal", true);
			hashtable.Add("time", duration);
			hashtable.Add("easetype", iTween.EaseType.easeInOutElastic);
			hashtable.Add("ignoretimescale", false);
			return hashtable;
		}
	}
}
