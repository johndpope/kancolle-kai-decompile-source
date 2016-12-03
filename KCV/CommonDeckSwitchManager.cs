using Common.Enum;
using KCV.Utils;
using local.managers;
using local.models;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KCV
{
	public class CommonDeckSwitchManager : MonoBehaviour
	{
		private const string SPRITE_ON = "pin_on";

		private const string SPRITE_OFF = "pin_off";

		private const string SPRITE_NONE = "pin_none";

		private const string OTHER_SPRITE_ON = "other_on";

		private const string OTHER_SPRITE_OFF = "other_off";

		private const string OTHER_SPRITE_NONE = "other_none";

		private CommonDeckSwitchHandler handler;

		[SerializeField]
		private UISprite templateSprite;

		private List<UISprite> switchableIconSprites = new List<UISprite>();

		private int horizontalIconMargin;

		private bool otherEnabled;

		private KeyControl keyController;

		private int currentIdx;

		private DeckModel[] decks;

		private int switchableIconCount
		{
			get
			{
				return (!this.otherEnabled) ? this.validDeckCount : (this.validDeckCount + 1);
			}
		}

		public DeckModel currentDeck
		{
			get
			{
				return (!this.otherEnabled || this.currentIdx != this.switchableIconCount - 1) ? this.decks[this.currentIdx] : null;
			}
		}

		public bool keyControlEnable
		{
			get;
			set;
		}

		private int validDeckCount
		{
			get
			{
				return this.decks.Length;
			}
		}

		public bool isChangeRight
		{
			get
			{
				return this.currentIdx < this.switchableIconCount - 1;
			}
		}

		public bool isChangeLeft
		{
			get
			{
				return 0 < this.currentIdx;
			}
		}

		public virtual void Init(ManagerBase manager, DeckModel[] decks, CommonDeckSwitchHandler handler, KeyControl keyController, bool otherEnabled)
		{
			this.Init(manager, decks, handler, keyController, otherEnabled, 0, 50);
		}

		public virtual void Init(ManagerBase manager, DeckModel[] decks, CommonDeckSwitchHandler handler, KeyControl keyController, bool otherEnabled, DeckModel currentDeck, int horizontalIconMargin = 50)
		{
			DeckModel[] array = Enumerable.ToArray<DeckModel>(Enumerable.Where<DeckModel>(decks, (DeckModel e) => e.MissionState == MissionStates.NONE));
			int num = 0;
			int num2 = 0;
			DeckModel[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				DeckModel deckModel = array2[i];
				if (deckModel.Id == currentDeck.Id)
				{
					num2 = num;
					break;
				}
				num++;
			}
			this.Init(manager, array, handler, keyController, otherEnabled, num2, horizontalIconMargin);
		}

		protected void Init(ManagerBase manager, DeckModel[] srcDecks, CommonDeckSwitchHandler handler, KeyControl keyController, bool otherEnabled, int currentIdx, int horizontalIconMargin)
		{
			this.decks = srcDecks;
			this.handler = handler;
			this.keyController = keyController;
			this.currentIdx = currentIdx;
			this.otherEnabled = otherEnabled;
			this.horizontalIconMargin = horizontalIconMargin;
			int deckCount = manager.UserInfo.DeckCount;
			this.keyControlEnable = true;
			int num = deckCount + ((!otherEnabled) ? 0 : 1);
			int num2 = -(num - 1) * horizontalIconMargin / 2;
			HashSet<int> validIndices = new HashSet<int>();
			this.decks.ForEach(delegate(DeckModel e)
			{
				validIndices.Add(e.Id - 1);
			});
			if (otherEnabled)
			{
				validIndices.Add(num - 1);
			}
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = Util.Instantiate(this.templateSprite.get_gameObject(), base.get_gameObject(), false, false);
				gameObject.get_transform().localPosition(new Vector3((float)(num2 + horizontalIconMargin * i), 0f, 0f));
				UISprite component = gameObject.GetComponent<UISprite>();
				if (validIndices.Contains(i))
				{
					this.switchableIconSprites.Add(component);
				}
				else
				{
					component.spriteName = "pin_none";
				}
			}
			this.templateSprite.SetActive(false);
			if (!handler.IsDeckSelectable(currentIdx, this.currentDeck))
			{
				this.ProcessNext(0);
			}
			handler.OnDeckChange(this.currentDeck);
			this.RefleshIcons();
		}

		private void Update()
		{
			if (!this.keyControlEnable || this.keyController == null)
			{
				return;
			}
			if (this.keyController.IsRSLeftDown())
			{
				this.ChangePrevDeck();
			}
			else if (this.keyController.keyState.get_Item(18).down)
			{
				this.ChangeNextDeck();
			}
		}

		public void ChangePrevDeck()
		{
			if (this.ProcessPrev())
			{
				SoundUtils.PlayOneShotSE(SEFIleInfos.CommonEnter1);
				this.RefleshIcons();
				this.handler.OnDeckChange(this.currentDeck);
			}
		}

		public void ChangeNextDeck()
		{
			if (this.ProcessNext())
			{
				SoundUtils.PlayOneShotSE(SEFIleInfos.CommonEnter1);
				this.RefleshIcons();
				this.handler.OnDeckChange(this.currentDeck);
			}
		}

		private bool ProcessNext()
		{
			return this.ProcessNext(this.currentIdx + 1);
		}

		private bool ProcessNext(int startIdx)
		{
			for (int i = startIdx; i < this.switchableIconCount; i++)
			{
				if (this.handler.IsDeckSelectable(i, (i < this.decks.Length) ? this.decks[i] : null))
				{
					this.currentIdx = i;
					return true;
				}
			}
			return false;
		}

		private bool ProcessPrev()
		{
			for (int i = this.currentIdx - 1; i >= 0; i--)
			{
				if (this.handler.IsDeckSelectable(i, this.decks[i]))
				{
					this.currentIdx = i;
					return true;
				}
			}
			return false;
		}

		public void RefleshIcons()
		{
			for (int i = 0; i < this.switchableIconCount; i++)
			{
				bool flag = this.otherEnabled && i == this.switchableIconCount - 1;
				string spriteName;
				if (i == this.currentIdx)
				{
					spriteName = ((!flag) ? "pin_on" : "other_on");
				}
				else if (this.handler.IsDeckSelectable(i, (i < this.decks.Length) ? this.decks[i] : null))
				{
					spriteName = ((!flag) ? "pin_off" : "other_off");
				}
				else
				{
					spriteName = ((!flag) ? "pin_none" : "other_none");
				}
				this.switchableIconSprites.get_Item(i).spriteName = spriteName;
			}
		}

		protected virtual void OnCallDestroy()
		{
		}

		private void OnDestroy()
		{
			this.OnCallDestroy();
			this.handler = null;
			this.templateSprite = null;
			this.switchableIconSprites = null;
			this.keyController = null;
			this.decks = null;
		}
	}
}
