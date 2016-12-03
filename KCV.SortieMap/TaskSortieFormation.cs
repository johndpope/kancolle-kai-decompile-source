using Common.Enum;
using KCV.Battle.Formation;
using KCV.SortieBattle;
using Librarys.State;
using local.managers;
using System;
using System.Collections;
using UnityEngine;

namespace KCV.SortieMap
{
	public class TaskSortieFormation : Task
	{
		private UIBattleFormationKindSelectManager _uiBattleFormationSelector;

		protected override void Dispose(bool isDisposing)
		{
			Mem.Del<UIBattleFormationKindSelectManager>(ref this._uiBattleFormationSelector);
		}

		protected override bool Init()
		{
			App.TimeScale(1f);
			UIShortCutSwitch shortCutSwitch = SortieMapTaskManager.GetShortCutSwitch();
			shortCutSwitch.Hide();
			SortieMapTaskManager.GetUIShipCharacter().ShowInFormation(50, null);
			BattleFormationKinds1[] formationArray = SortieUtils.GetFormationArray(SortieBattleTaskManager.GetMapManager().Deck);
			if (1 < formationArray.Length)
			{
				this._uiBattleFormationSelector = Util.Instantiate(SortieMapTaskManager.GetPrefabFile().prefabUIBattleFormationKindSelectManager.get_gameObject(), SortieMapTaskManager.GetSharedPlace().get_gameObject(), false, false).GetComponent<UIBattleFormationKindSelectManager>();
				SortieMapTaskManager.GetUIAreaMapFrame().SetMessage("陣形を選択してください。");
				this._uiBattleFormationSelector.Initialize(GameObject.Find("SortieAreaCamera").GetComponent<Camera>(), formationArray);
				this._uiBattleFormationSelector.SetOnUIBattleFormationKindSelectManagerAction(new UIBattleFormationKindSelectManager.UIBattleFormationKindSelectManagerAction(this.UIBattleFormationKindSelectManagerActionCallBack));
				this._uiBattleFormationSelector.SetKeyController(SortieBattleTaskManager.GetKeyControl());
			}
			else
			{
				this.OnFormationSelected(BattleFormationKinds1.TanJuu);
			}
			return true;
		}

		protected override bool UnInit()
		{
			Mem.DelComponentSafe<UIBattleFormationKindSelectManager>(ref this._uiBattleFormationSelector);
			return true;
		}

		protected override bool Update()
		{
			if (this._uiBattleFormationSelector != null)
			{
				this._uiBattleFormationSelector.OnUpdatedKeyController();
			}
			return SortieMapTaskManager.GetMode() == SortieMapTaskManagerMode.SortieMapTaskManagerMode_BEF || SortieMapTaskManager.GetMode() == SortieMapTaskManagerMode.Formation;
		}

		private void UIBattleFormationKindSelectManagerActionCallBack(UIBattleFormationKindSelectManager.ActionType actionType, UIBattleFormationKindSelectManager calledObject, UIBattleFormationKind centerView)
		{
			calledObject.OnReleaseKeyController();
			BattleFormationKinds1 iFormation;
			if (actionType != UIBattleFormationKindSelectManager.ActionType.Select)
			{
				iFormation = BattleFormationKinds1.TanJuu;
			}
			else
			{
				iFormation = centerView.Category;
			}
			this.OnFormationSelected(iFormation);
		}

		private void OnFormationSelected(BattleFormationKinds1 iFormation)
		{
			SortieMapTaskManager.GetUIShipCharacter().Hide(null);
			SortieMapTaskManager.GetUIAreaMapFrame().Hide();
			ProdSortieTransitionToBattle prodSortieTransitionToBattle = SortieBattleTaskManager.GetSortieBattlePrefabFile().prodSortieTransitionToBattle;
			SortieBattleTaskManager.GetTransitionCamera().set_enabled(true);
			prodSortieTransitionToBattle.Play(delegate
			{
				Hashtable hashtable = new Hashtable();
				if (SortieBattleTaskManager.GetMapManager().GetType().Equals(typeof(RebellionMapManager)))
				{
					hashtable.Add("rootType", Generics.BattleRootType.Rebellion);
					hashtable.Add("rebellionMapManager", SortieBattleTaskManager.GetMapManager());
				}
				else
				{
					hashtable.Add("rootType", Generics.BattleRootType.SortieMap);
					hashtable.Add("sortieMapManager", SortieBattleTaskManager.GetMapManager());
				}
				hashtable.Add("formation", iFormation);
				RetentionData.SetData(hashtable);
				SortieBattleTaskManager.ReqMode(SortieBattleMode.Battle);
			});
		}
	}
}
