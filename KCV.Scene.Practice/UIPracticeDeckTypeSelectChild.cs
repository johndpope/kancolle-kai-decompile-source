using Common.Enum;
using System;
using UnityEngine;

namespace KCV.Scene.Practice
{
	public class UIPracticeDeckTypeSelectChild : MonoBehaviour
	{
		[SerializeField]
		private DeckPracticeType mDeckPracticeType;

		[SerializeField]
		private UIButton mButton;

		private Action<UIPracticeDeckTypeSelectChild> mDeckPracticeTypeSelectedAction;

		public void Hover()
		{
			this.mButton.SetState(UIButtonColor.State.Hover, true);
		}

		public void RemoveHover()
		{
			this.mButton.SetState(UIButtonColor.State.Normal, true);
		}

		public void Enabled(bool isEnabled)
		{
			this.mButton.SetEnableCollider2D(isEnabled);
		}

		public DeckPracticeType GetDeckPracticeType()
		{
			return this.mDeckPracticeType;
		}

		public void SetOnClickListener(Action<UIPracticeDeckTypeSelectChild> deckPracticeTypeView)
		{
			this.mDeckPracticeTypeSelectedAction = deckPracticeTypeView;
		}

		public void OnClickView()
		{
			if (this.mDeckPracticeTypeSelectedAction != null)
			{
				this.mDeckPracticeTypeSelectedAction.Invoke(this);
			}
		}

		public void ParentHasChanged()
		{
			this.mButton.GetSprite().ParentHasChanged();
		}

		public void Initialize()
		{
		}
	}
}
