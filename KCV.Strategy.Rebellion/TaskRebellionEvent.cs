using local.models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace KCV.Strategy.Rebellion
{
	public class TaskRebellionEvent : SceneTaskMono
	{
		[SerializeField]
		private Transform _prefabProdRebellionStart;

		[SerializeField]
		private GameObject _prefabProdAreaCheck;

		[SerializeField]
		private DeckSortieInfoManager deckInfoManager;

		[SerializeField]
		private CommonDialog commonDialog;

		[SerializeField]
		private CommonDialog StrategyDialog;

		[SerializeField]
		private UIGoSortieConfirm GoSortieConfirm;

		[SerializeField]
		private TweenAlpha TweenAlphaRedMask;

		private ProdRebellionStart _prodRebellionStart;

		private ProdRebellionAreaCheck _prodRebellionAreaCheck;

		private int SortieEnableDeckNum;

		private List<DeckModel> AreaDecks;

		private void OnDestroy()
		{
			this._prefabProdRebellionStart = null;
			this._prodRebellionAreaCheck = null;
			this.deckInfoManager = null;
			this.commonDialog = null;
			this.StrategyDialog = null;
			this.GoSortieConfirm = null;
			this.TweenAlphaRedMask = null;
			this._prodRebellionStart = null;
			this._prodRebellionAreaCheck = null;
			Mem.DelListSafe<DeckModel>(ref this.AreaDecks);
		}

		protected override bool Init()
		{
			this.DelayAction(1.5f, delegate
			{
				StrategyTopTaskManager.Instance.GetInfoMng().MoveScreenOut(null, true, false);
			});
			this._prodRebellionStart = ProdRebellionStart.Instantiate(this._prefabProdRebellionStart.GetComponent<ProdRebellionStart>(), StrategyTaskManager.GetOverView());
			this._prodRebellionAreaCheck = Util.Instantiate(this._prefabProdAreaCheck, StrategyTaskManager.GetMapRoot().get_gameObject(), false, false).GetComponent<ProdRebellionAreaCheck>();
			this._prodRebellionStart.Play(delegate
			{
				this.setActiveRedMask(true);
			}).Subscribe(delegate(bool _)
			{
				this._prodRebellionAreaCheck.Play(StrategyRebellionTaskManager.RebellionFromArea, StrategyRebellionTaskManager.RebellionArea, delegate
				{
					base.StartCoroutine(this.GoNextSceneAtDeckNum(StrategyTaskManager.GetStrategyRebellion().GetRebellionManager().Decks));
				});
			}).AddTo(base.get_gameObject());
			return true;
		}

		protected override bool UnInit()
		{
			if (this._prodRebellionStart != null && this._prodRebellionStart.get_gameObject() != null)
			{
				this._prodRebellionStart.get_gameObject().Discard();
			}
			this._prodRebellionStart = null;
			return true;
		}

		protected override bool Run()
		{
			return StrategyUtils.ChkStateRebellionTaskIsRun(StrategyRebellionTaskManagerMode.StrategyRebellionTaskManager_ST);
		}

		[DebuggerHidden]
		private IEnumerator GoNextSceneAtDeckNum(List<DeckModel> areaDecks)
		{
			TaskRebellionEvent.<GoNextSceneAtDeckNum>c__Iterator163 <GoNextSceneAtDeckNum>c__Iterator = new TaskRebellionEvent.<GoNextSceneAtDeckNum>c__Iterator163();
			<GoNextSceneAtDeckNum>c__Iterator.areaDecks = areaDecks;
			<GoNextSceneAtDeckNum>c__Iterator.<$>areaDecks = areaDecks;
			<GoNextSceneAtDeckNum>c__Iterator.<>f__this = this;
			return <GoNextSceneAtDeckNum>c__Iterator;
		}

		[DebuggerHidden]
		private IEnumerator PopupCantSortieDecksDialog(List<DeckModel> areaDecks)
		{
			TaskRebellionEvent.<PopupCantSortieDecksDialog>c__Iterator164 <PopupCantSortieDecksDialog>c__Iterator = new TaskRebellionEvent.<PopupCantSortieDecksDialog>c__Iterator164();
			<PopupCantSortieDecksDialog>c__Iterator.areaDecks = areaDecks;
			<PopupCantSortieDecksDialog>c__Iterator.<$>areaDecks = areaDecks;
			<PopupCantSortieDecksDialog>c__Iterator.<>f__this = this;
			return <PopupCantSortieDecksDialog>c__Iterator;
		}

		[DebuggerHidden]
		public IEnumerator NonDeckLose()
		{
			TaskRebellionEvent.<NonDeckLose>c__Iterator165 <NonDeckLose>c__Iterator = new TaskRebellionEvent.<NonDeckLose>c__Iterator165();
			<NonDeckLose>c__Iterator.<>f__this = this;
			return <NonDeckLose>c__Iterator;
		}

		[DebuggerHidden]
		private IEnumerator OneDeckGoSortie()
		{
			TaskRebellionEvent.<OneDeckGoSortie>c__Iterator166 <OneDeckGoSortie>c__Iterator = new TaskRebellionEvent.<OneDeckGoSortie>c__Iterator166();
			<OneDeckGoSortie>c__Iterator.<>f__this = this;
			return <OneDeckGoSortie>c__Iterator;
		}

		[DebuggerHidden]
		private IEnumerator PlayTransition(MapModel mapModel, DeckModel deck)
		{
			TaskRebellionEvent.<PlayTransition>c__Iterator167 <PlayTransition>c__Iterator = new TaskRebellionEvent.<PlayTransition>c__Iterator167();
			<PlayTransition>c__Iterator.mapModel = mapModel;
			<PlayTransition>c__Iterator.deck = deck;
			<PlayTransition>c__Iterator.<$>mapModel = mapModel;
			<PlayTransition>c__Iterator.<$>deck = deck;
			<PlayTransition>c__Iterator.<>f__this = this;
			return <PlayTransition>c__Iterator;
		}

		[DebuggerHidden]
		private IEnumerator OpenConfirmDialog(DeckModel deck)
		{
			TaskRebellionEvent.<OpenConfirmDialog>c__Iterator168 <OpenConfirmDialog>c__Iterator = new TaskRebellionEvent.<OpenConfirmDialog>c__Iterator168();
			<OpenConfirmDialog>c__Iterator.<>f__this = this;
			return <OpenConfirmDialog>c__Iterator;
		}

		public void setActiveRedMask(bool isActive)
		{
			if (isActive)
			{
				this.TweenAlphaRedMask.SetActive(true);
				this.TweenAlphaRedMask.from = 0.5f;
				this.TweenAlphaRedMask.to = 1f;
				this.TweenAlphaRedMask.tweenFactor = 0.5f;
				this.TweenAlphaRedMask.style = UITweener.Style.PingPong;
				this.TweenAlphaRedMask.duration = 1f;
				this.TweenAlphaRedMask.PlayForward();
			}
			else
			{
				TweenAlpha.Begin(this.TweenAlphaRedMask.get_gameObject(), 0.5f, 0f);
				this.DelayAction(0.5f, delegate
				{
					this.TweenAlphaRedMask.SetActive(false);
				});
			}
		}

		[DebuggerHidden]
		private IEnumerator ShowTutorial(int enableDeckNum)
		{
			TaskRebellionEvent.<ShowTutorial>c__Iterator169 <ShowTutorial>c__Iterator = new TaskRebellionEvent.<ShowTutorial>c__Iterator169();
			<ShowTutorial>c__Iterator.enableDeckNum = enableDeckNum;
			<ShowTutorial>c__Iterator.<$>enableDeckNum = enableDeckNum;
			return <ShowTutorial>c__Iterator;
		}

		[DebuggerHidden]
		private IEnumerator ShowLoseGuide()
		{
			return new TaskRebellionEvent.<ShowLoseGuide>c__Iterator16A();
		}
	}
}
