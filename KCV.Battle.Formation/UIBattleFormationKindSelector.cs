using Common.Enum;
using KCV.Base;
using KCV.Display;
using KCV.Interface;
using KCV.Utils;
using System;
using UnityEngine;

namespace KCV.Battle.Formation
{
	public class UIBattleFormationKindSelector : UICircleMenu<BattleFormationKinds1, UIBattleFormationKind>, IUIKeyControllable
	{
		public enum ActionType
		{
			Select,
			SelectAuto,
			OnNextChanged,
			OnPrevChanged
		}

		public delegate void UIBattleFormationKindSelectorAction(UIBattleFormationKindSelector.ActionType actionType, UIBattleFormationKind centerObject);

		private const int MIN_FORMATION = 1;

		private UIBattleFormationKindSelector.UIBattleFormationKindSelectorAction mUIBattleFormationKindSelectorAction;

		private UIDisplaySwipeEventRegion mUIDisplaySwipeEventRegion;

		private KeyControl mKeyController;

		public void Initialize(BattleFormationKinds1[] kinds)
		{
			if (1 < kinds.Length)
			{
				base.Initialize(kinds, 3);
			}
			else
			{
				this.CallBackAction(UIBattleFormationKindSelector.ActionType.SelectAuto, null);
			}
		}

		public void SetOnUIBattleFormationKindSelectorAction(UIBattleFormationKindSelector.UIBattleFormationKindSelectorAction callBackMethod)
		{
			this.mUIBattleFormationKindSelectorAction = callBackMethod;
		}

		protected override void InitOriginalDefaultPosition(ref Vector3[] defaultPositions)
		{
			base.InitOriginalDefaultPosition(ref defaultPositions);
			defaultPositions[defaultPositions.Length - 1] += new Vector3(0f, 75f);
			defaultPositions[1] += new Vector3(0f, 75f);
			defaultPositions[defaultPositions.Length - 2] = new Vector3(-700f, 350f);
			defaultPositions[2] += new Vector3(700f, 200f);
		}

		public void Prev()
		{
			if (!base.mIsAnimationNow)
			{
				base.Prev();
				this.CallBackAction(UIBattleFormationKindSelector.ActionType.OnPrevChanged, this.mCenterView);
			}
		}

		public void Next()
		{
			if (!base.mIsAnimationNow)
			{
				base.Next();
				this.CallBackAction(UIBattleFormationKindSelector.ActionType.OnNextChanged, this.mCenterView);
			}
		}

		[Obsolete("Inspector上で設定して使用します")]
		public void OnTouchSelectCenter()
		{
			this.SelectCenter();
		}

		public void SelectCenter()
		{
			if (!base.mIsAnimationNow)
			{
				this.mKeyController = null;
				if (this.mUIDisplaySwipeEventRegion != null)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
					this.mUIDisplaySwipeEventRegion.set_enabled(false);
					this.mUIDisplaySwipeEventRegion = null;
					this.mCenterView.OnSelectAnimation(delegate
					{
						this.CallBackAction(UIBattleFormationKindSelector.ActionType.Select, this.mCenterView);
						base.mIsAnimationNow = false;
					});
				}
			}
		}

		private void CallBackAction(UIBattleFormationKindSelector.ActionType actionType, UIBattleFormationKind centerObject)
		{
			if (this.mUIBattleFormationKindSelectorAction != null)
			{
				this.mUIBattleFormationKindSelectorAction(actionType, centerObject);
			}
		}

		public void SetKeyController(KeyControl keyController, UIDisplaySwipeEventRegion displaySwipeEventRegion)
		{
			this.mKeyController = keyController;
			this.mUIDisplaySwipeEventRegion = displaySwipeEventRegion;
		}

		public void OnUpdatedKeyController()
		{
			if (this.mKeyController != null)
			{
				if (this.mKeyController.keyState.get_Item(4).down || this.mKeyController.keyState.get_Item(14).down)
				{
					Debug.Log("PRev");
					this.Prev();
				}
				else if (this.mKeyController.keyState.get_Item(5).down || this.mKeyController.keyState.get_Item(10).down)
				{
					this.Next();
				}
				else if (this.mKeyController.keyState.get_Item(1).down)
				{
					this.SelectCenter();
				}
			}
		}

		public void OnReleaseKeyController()
		{
			this.mKeyController = null;
			this.mUIDisplaySwipeEventRegion = null;
		}

		public void SetKeyController(KeyControl keyController)
		{
			this.mKeyController = keyController;
		}

		private void OnDestroy()
		{
			this.mUIBattleFormationKindSelectorAction = null;
			this.mUIDisplaySwipeEventRegion = null;
			this.mKeyController = null;
		}
	}
}
