using local.models;
using System;
using System.Linq;
using UnityEngine;

namespace KCV.Remodel
{
	[SelectionBase]
	public class UIRemodelDeckSwitchManager : CommonDeckSwitchManager, UIRemodelView
	{
		private const float ANIMATION_DURATION = 0.2f;

		private Vector3 showPos = new Vector3(-240f, -257f);

		private Vector3 hidePos = new Vector3(-240f, -300f);

		private void Awake()
		{
			base.get_transform().set_localPosition(this.hidePos);
			this.Show(false);
		}

		public void Init(DeckModel[] decks, CommonDeckSwitchHandler handler, KeyControl keyController, bool otherEnabled)
		{
			decks = Enumerable.ToArray<DeckModel>(Enumerable.Where<DeckModel>(decks, (DeckModel x) => !x.HasBling()));
			base.Init(UserInterfaceRemodelManager.instance.mRemodelManager, decks, handler, keyController, otherEnabled, SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck, 25);
		}

		public void Show()
		{
			this.Show(true);
		}

		public void Show(bool animation)
		{
			base.keyControlEnable = true;
			base.get_gameObject().SetActive(true);
			if (animation)
			{
				RemodelUtils.MoveWithManual(base.get_gameObject(), this.showPos, 0.2f, delegate
				{
				});
			}
			else
			{
				base.get_transform().set_localPosition(this.showPos);
			}
		}

		public void Hide()
		{
			this.Hide(true);
		}

		public void Hide(bool animation)
		{
			base.keyControlEnable = false;
			if (animation)
			{
				RemodelUtils.MoveWithManual(base.get_gameObject(), this.hidePos, 0.2f, delegate
				{
					base.get_gameObject().SetActive(false);
				});
			}
			else
			{
				base.get_transform().set_localPosition(this.hidePos);
				base.get_gameObject().SetActive(false);
			}
		}
	}
}
