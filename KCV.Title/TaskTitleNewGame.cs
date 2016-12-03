using Common.Enum;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace KCV.Title
{
	public class TaskTitleNewGame : SceneTaskMono
	{
		private CtrlDifficultySelect _ctrlDifficultySelect;

		protected override bool Init()
		{
			HashSet<DifficultKind> hashSet = new HashSet<DifficultKind>();
			hashSet.Add(DifficultKind.HEI);
			hashSet.Add(DifficultKind.KOU);
			hashSet.Add(DifficultKind.OTU);
			this._ctrlDifficultySelect = CtrlDifficultySelect.Instantiate(TitleTaskManager.GetPrefabFile().prefabCtrlDifficultySelect.GetComponent<CtrlDifficultySelect>(), TitleTaskManager.GetSharedPlace(), TitleTaskManager.GetKeyControl(), App.GetTitleManager().GetSelectableDifficulty(), new Action<DifficultKind>(this.OnDecideDifficulty), new Action(this.OnCancel));
			return true;
		}

		protected override bool UnInit()
		{
			if (this._ctrlDifficultySelect != null)
			{
				Object.Destroy(this._ctrlDifficultySelect.get_gameObject());
			}
			Mem.Del<CtrlDifficultySelect>(ref this._ctrlDifficultySelect);
			return true;
		}

		protected override bool Run()
		{
			this._ctrlDifficultySelect.Run();
			return TitleTaskManager.GetMode() == TitleTaskManagerMode.TitleTaskManagerMode_BEF || TitleTaskManager.GetMode() == TitleTaskManagerMode.NewGame;
		}

		private void OnCancel()
		{
			TitleTaskManager.ReqMode(TitleTaskManagerMode.SelectMode);
		}

		private void OnDecideDifficulty(DifficultKind iKind)
		{
			Observable.FromCoroutine(() => this.LoadTutorial(iKind), false).Subscribe<Unit>().AddTo(base.get_gameObject());
		}

		[DebuggerHidden]
		private IEnumerator LoadTutorial(DifficultKind difficultKind)
		{
			TaskTitleNewGame.<LoadTutorial>c__Iterator1A1 <LoadTutorial>c__Iterator1A = new TaskTitleNewGame.<LoadTutorial>c__Iterator1A1();
			<LoadTutorial>c__Iterator1A.difficultKind = difficultKind;
			<LoadTutorial>c__Iterator1A.<$>difficultKind = difficultKind;
			return <LoadTutorial>c__Iterator1A;
		}
	}
}
