using local.models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyShipManager : MonoBehaviour
	{
		[Button("moveArea", "moveAreaShip", new object[]
		{

		})]
		public int button1;

		public int area;

		[SerializeField]
		private StrategyShip[] allShipIcons;

		private List<StrategyShip> existShipIcons;

		private int[] areaDeckCount;

		private List<float[,]> shipNoPosList;

		public GameObject DeckSelectCursol;

		private UISprite CursolSprite;

		[SerializeField]
		private StrategyOrganizeGuideMessage OrganizeMessage;

		public bool isShipMoving;

		private int nowFocusDisableDeckNo;

		public float Base = 1f;

		public float Far = 1.7f;

		public float Middle = 0.7f;

		public float near = 0.5f;

		[Button("DebugPosListChange", "DebugPosListChange", new object[]
		{

		})]
		public int button2;

		private Action CharacterMove;

		private void moveArea()
		{
			base.StartCoroutine(this.moveAreaAllShip(this.area, false));
		}

		private void Awake()
		{
			this.CursolSprite = this.DeckSelectCursol.get_transform().FindChild("Cursol").GetComponent<UISprite>();
			this.nowFocusDisableDeckNo = -1;
			this.shipNoPosList = new List<float[,]>();
			this.shipNoPosList.Add(new float[1, 2]);
			List<float[,]> arg_6E_0 = this.shipNoPosList;
			float[,] expr_51 = new float[2, 2];
			expr_51[0, 0] = -this.Base;
			expr_51[1, 0] = this.Base;
			arg_6E_0.Add(expr_51);
			List<float[,]> arg_C9_0 = this.shipNoPosList;
			float[,] expr_80 = new float[3, 2];
			expr_80[0, 1] = this.Middle;
			expr_80[1, 0] = -this.Base;
			expr_80[1, 1] = -this.Middle;
			expr_80[2, 0] = this.Base;
			expr_80[2, 1] = -this.Middle;
			arg_C9_0.Add(expr_80);
			List<float[,]> arg_14F_0 = this.shipNoPosList;
			float[,] expr_DB = new float[4, 2];
			expr_DB[0, 0] = -this.Base;
			expr_DB[0, 1] = this.near;
			expr_DB[1, 0] = this.Base;
			expr_DB[1, 1] = this.near;
			expr_DB[2, 0] = -this.Base;
			expr_DB[2, 1] = -this.near;
			expr_DB[3, 0] = this.Base;
			expr_DB[3, 1] = -this.near;
			arg_14F_0.Add(expr_DB);
			List<float[,]> arg_1D5_0 = this.shipNoPosList;
			float[,] expr_161 = new float[5, 2];
			expr_161[0, 0] = -this.Base;
			expr_161[0, 1] = this.Base;
			expr_161[1, 0] = this.Base;
			expr_161[1, 1] = this.Base;
			expr_161[2, 0] = -this.Base;
			expr_161[2, 1] = -this.Base;
			expr_161[3, 0] = this.Base;
			expr_161[3, 1] = -this.Base;
			arg_1D5_0.Add(expr_161);
			List<float[,]> arg_278_0 = this.shipNoPosList;
			float[,] expr_1E7 = new float[6, 2];
			expr_1E7[0, 0] = -this.Base;
			expr_1E7[0, 1] = this.Base;
			expr_1E7[1, 0] = this.Base;
			expr_1E7[1, 1] = this.Base;
			expr_1E7[2, 0] = -this.Far;
			expr_1E7[3, 0] = this.Far;
			expr_1E7[4, 0] = -this.Base;
			expr_1E7[4, 1] = -this.Base;
			expr_1E7[5, 0] = this.Base;
			expr_1E7[5, 1] = -this.Base;
			arg_278_0.Add(expr_1E7);
			List<float[,]> arg_31B_0 = this.shipNoPosList;
			float[,] expr_28A = new float[7, 2];
			expr_28A[0, 0] = -this.Base;
			expr_28A[0, 1] = this.Base;
			expr_28A[1, 0] = this.Base;
			expr_28A[1, 1] = this.Base;
			expr_28A[2, 0] = -this.Far;
			expr_28A[3, 0] = this.Far;
			expr_28A[4, 0] = -this.Base;
			expr_28A[4, 1] = -this.Base;
			expr_28A[5, 0] = this.Base;
			expr_28A[5, 1] = -this.Base;
			arg_31B_0.Add(expr_28A);
			List<float[,]> arg_3DB_0 = this.shipNoPosList;
			float[,] expr_32D = new float[8, 2];
			expr_32D[0, 0] = -this.Base;
			expr_32D[0, 1] = this.Base;
			expr_32D[1, 0] = this.Base;
			expr_32D[1, 1] = this.Base;
			expr_32D[2, 0] = -this.Far;
			expr_32D[3, 0] = this.Far;
			expr_32D[4, 0] = -this.Base;
			expr_32D[4, 1] = -this.Base;
			expr_32D[5, 0] = this.Base;
			expr_32D[5, 1] = -this.Base;
			expr_32D[6, 1] = this.near;
			expr_32D[7, 1] = -this.near;
			arg_3DB_0.Add(expr_32D);
		}

		private void DebugPosListChange()
		{
			this.shipNoPosList = new List<float[,]>();
			this.shipNoPosList.Add(new float[1, 2]);
			List<float[,]> arg_47_0 = this.shipNoPosList;
			float[,] expr_2A = new float[2, 2];
			expr_2A[0, 0] = -this.Base;
			expr_2A[1, 0] = this.Base;
			arg_47_0.Add(expr_2A);
			List<float[,]> arg_A2_0 = this.shipNoPosList;
			float[,] expr_59 = new float[3, 2];
			expr_59[0, 1] = this.Middle;
			expr_59[1, 0] = -this.Base;
			expr_59[1, 1] = -this.Middle;
			expr_59[2, 0] = this.Base;
			expr_59[2, 1] = -this.Middle;
			arg_A2_0.Add(expr_59);
			List<float[,]> arg_128_0 = this.shipNoPosList;
			float[,] expr_B4 = new float[4, 2];
			expr_B4[0, 0] = -this.Base;
			expr_B4[0, 1] = this.near;
			expr_B4[1, 0] = this.Base;
			expr_B4[1, 1] = this.near;
			expr_B4[2, 0] = -this.Base;
			expr_B4[2, 1] = -this.near;
			expr_B4[3, 0] = this.Base;
			expr_B4[3, 1] = -this.near;
			arg_128_0.Add(expr_B4);
			List<float[,]> arg_1AE_0 = this.shipNoPosList;
			float[,] expr_13A = new float[5, 2];
			expr_13A[0, 0] = -this.Base;
			expr_13A[0, 1] = this.Base;
			expr_13A[1, 0] = this.Base;
			expr_13A[1, 1] = this.Base;
			expr_13A[2, 0] = -this.Base;
			expr_13A[2, 1] = -this.Base;
			expr_13A[3, 0] = this.Base;
			expr_13A[3, 1] = -this.Base;
			arg_1AE_0.Add(expr_13A);
			List<float[,]> arg_251_0 = this.shipNoPosList;
			float[,] expr_1C0 = new float[6, 2];
			expr_1C0[0, 0] = -this.Base;
			expr_1C0[0, 1] = this.Base;
			expr_1C0[1, 0] = this.Base;
			expr_1C0[1, 1] = this.Base;
			expr_1C0[2, 0] = -this.Far;
			expr_1C0[3, 0] = this.Far;
			expr_1C0[4, 0] = -this.Base;
			expr_1C0[4, 1] = -this.Base;
			expr_1C0[5, 0] = this.Base;
			expr_1C0[5, 1] = -this.Base;
			arg_251_0.Add(expr_1C0);
			List<float[,]> arg_2F4_0 = this.shipNoPosList;
			float[,] expr_263 = new float[7, 2];
			expr_263[0, 0] = -this.Base;
			expr_263[0, 1] = this.Base;
			expr_263[1, 0] = this.Base;
			expr_263[1, 1] = this.Base;
			expr_263[2, 0] = -this.Far;
			expr_263[3, 0] = this.Far;
			expr_263[4, 0] = -this.Base;
			expr_263[4, 1] = -this.Base;
			expr_263[5, 0] = this.Base;
			expr_263[5, 1] = -this.Base;
			arg_2F4_0.Add(expr_263);
			List<float[,]> arg_3B4_0 = this.shipNoPosList;
			float[,] expr_306 = new float[8, 2];
			expr_306[0, 0] = -this.Base;
			expr_306[0, 1] = this.Base;
			expr_306[1, 0] = this.Base;
			expr_306[1, 1] = this.Base;
			expr_306[2, 0] = -this.Far;
			expr_306[3, 0] = this.Far;
			expr_306[4, 0] = -this.Base;
			expr_306[4, 1] = -this.Base;
			expr_306[5, 0] = this.Base;
			expr_306[5, 1] = -this.Base;
			expr_306[6, 1] = this.near;
			expr_306[7, 1] = -this.near;
			arg_3B4_0.Add(expr_306);
		}

		public static DeckModel[] getEnableDecks(DeckModel[] decks)
		{
			List<DeckModel> list = new List<DeckModel>();
			for (int i = 0; i < decks.Length; i++)
			{
				if (decks[i].Count > 0)
				{
					list.Add(decks[i]);
				}
			}
			return list.ToArray();
		}

		public static DeckModel[] getDisableDecks(DeckModel[] decks)
		{
			List<DeckModel> list = new List<DeckModel>();
			for (int i = 0; i < decks.Length; i++)
			{
				if (decks[i].Count <= 0)
				{
					list.Add(decks[i]);
				}
			}
			return list.ToArray();
		}

		public void setShipIcons(DeckModel[] decks, bool isScaleZero = true)
		{
			this.areaDeckCount = new int[18];
			this.existShipIcons = new List<StrategyShip>();
			this.makeDeckExsistList(decks);
			this.setActiveIcons(decks, isScaleZero);
			this.setShipIconsGraph();
			this.setShipIconsState();
			this.setShipIconsPosition();
		}

		private void makeDeckExsistList(DeckModel[] decks)
		{
			for (int i = 0; i < decks.Length; i++)
			{
				if (decks[i].Count > 0)
				{
					this.allShipIcons[decks[i].Id - 1].setDeckModel(decks[i]);
					this.existShipIcons.Add(this.allShipIcons[decks[i].Id - 1]);
				}
			}
		}

		private void setActiveIcons(DeckModel[] decks, bool isScaleZero)
		{
			for (int i = 1; i < this.allShipIcons.Length + 1; i++)
			{
				bool isActive = false;
				for (int j = 0; j < decks.Length; j++)
				{
					if (decks[j].Id == i && decks[j].Count != 0)
					{
						isActive = true;
					}
				}
				if (isScaleZero)
				{
					this.allShipIcons[i - 1].get_transform().localScaleZero();
				}
				else
				{
					this.allShipIcons[i - 1].get_transform().localScaleOne();
				}
				this.allShipIcons[i - 1].SetActive(isActive);
			}
		}

		public void popUpShipIcon()
		{
			for (int i = 0; i < this.existShipIcons.get_Count(); i++)
			{
				this.existShipIcons.get_Item(i).popUpShipIcon();
			}
			this.OrganizeMessage.UpdateVisible();
		}

		private void setShipIconsGraph()
		{
			for (int i = 0; i < this.existShipIcons.get_Count(); i++)
			{
				this.existShipIcons.get_Item(i).setShipGraph();
			}
		}

		private void setShipIconsPosition()
		{
			for (int i = 0; i < this.existShipIcons.get_Count(); i++)
			{
				int areaId = this.existShipIcons.get_Item(i).deck.AreaId;
				int no = this.areaDeckCount[areaId];
				this.areaDeckCount[areaId]++;
				Vector3 shipIconPosition = this.GetShipIconPosition(areaId, no);
				this.existShipIcons.get_Item(i).setShipAreaPosition(shipIconPosition);
			}
		}

		public void setShipIconsState()
		{
			for (int i = 0; i < this.existShipIcons.get_Count(); i++)
			{
				this.existShipIcons.get_Item(i).setShipState();
			}
		}

		public void unsetShipIconsStateForSupportMission()
		{
			for (int i = 0; i < this.existShipIcons.get_Count(); i++)
			{
				if (this.existShipIcons.get_Item(i).deck.IsInSupportMission())
				{
					this.existShipIcons.get_Item(i).unsetShipStateIcon();
				}
			}
		}

		public void changeFocus()
		{
			if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck == null)
			{
				App.Initialize();
				StrategyTopTaskManager.CreateLogicManager();
				SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck = StrategyTopTaskManager.GetLogicManager().UserInfo.GetDeck(1);
			}
			int id = SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.Id;
			if (this.isDisableDeck(id))
			{
				this.DeckSelectCursol.get_transform().set_parent(this.OrganizeMessage.get_transform());
			}
			else
			{
				this.DeckSelectCursol.get_transform().set_parent(this.allShipIcons[id - 1].get_transform());
			}
			this.DeckSelectCursol.get_transform().set_localScale(Vector3.get_one());
			this.DeckSelectCursol.get_transform().set_localPosition(Vector3.get_zero());
			this.CursolSprite.ParentHasChanged();
		}

		public Vector3 GetShipIconPosition(int AreaID, int No)
		{
			Vector3 position = StrategyTopTaskManager.Instance.TileManager.Tiles[AreaID].get_transform().get_position();
			int num = StrategyShipManager.getEnableDecks(StrategyTopTaskManager.GetLogicManager().Area.get_Item(AreaID).GetDecks()).Length;
			float num2 = 0.25f * this.shipNoPosList.get_Item(num - 1)[No, 0];
			float num3 = 0.25f * this.shipNoPosList.get_Item(num - 1)[No, 1];
			return position + new Vector3(num2, num3, 0f);
		}

		public void sortAreaShipIcon(int targetAreaID, bool isMoveCharacter, bool isUpdateOrganizeMessage)
		{
			DeckModel[] enableDecks = StrategyShipManager.getEnableDecks(StrategyTopTaskManager.GetLogicManager().UserInfo.GetDecksFromArea(targetAreaID));
			for (int i = 0; i < enableDecks.Length; i++)
			{
				int num = enableDecks[i].Id - 1;
				Hashtable hashtable = new Hashtable();
				hashtable.Add("position", this.GetShipIconPosition(targetAreaID, i));
				hashtable.Add("time", 2f);
				hashtable.Add("easetype", iTween.EaseType.linear);
				if (i == enableDecks.Length - 1)
				{
					hashtable.Add("oncomplete", "OnCompleteMove");
					hashtable.Add("oncompletetarget", base.get_gameObject());
					int id = SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.Id;
					hashtable.Add("oncompleteparams", id);
					if (isMoveCharacter)
					{
						this.CharacterMove = delegate
						{
							StrategyTopTaskManager.GetSailSelect().moveCharacterScreen(true, null);
						};
					}
				}
				iTween.MoveTo(this.allShipIcons[num].get_gameObject(), hashtable);
				this.allShipIcons[num].setColliderEnable(false);
			}
			if (isUpdateOrganizeMessage)
			{
				this.OrganizeMessage.UpdateVisible();
			}
		}

		[DebuggerHidden]
		public IEnumerator moveAreaAllShip(int areaID, bool isWait)
		{
			StrategyShipManager.<moveAreaAllShip>c__Iterator179 <moveAreaAllShip>c__Iterator = new StrategyShipManager.<moveAreaAllShip>c__Iterator179();
			<moveAreaAllShip>c__Iterator.areaID = areaID;
			<moveAreaAllShip>c__Iterator.isWait = isWait;
			<moveAreaAllShip>c__Iterator.<$>areaID = areaID;
			<moveAreaAllShip>c__Iterator.<$>isWait = isWait;
			<moveAreaAllShip>c__Iterator.<>f__this = this;
			return <moveAreaAllShip>c__Iterator;
		}

		private void OnCompleteMove(int deckID)
		{
			this.setShipIconsState();
			StrategyTopTaskManager.Instance.GetInfoMng().updateFooterInfo(false);
			if (this.CharacterMove != null)
			{
				this.CharacterMove.Invoke();
			}
			this.CharacterMove = null;
			StrategyShip[] array = this.allShipIcons;
			for (int i = 0; i < array.Length; i++)
			{
				StrategyShip strategyShip = array[i];
				strategyShip.setColliderEnable(true);
			}
			this.isShipMoving = false;
		}

		public void SetVisible(bool isVisible)
		{
			float alpha = (float)((!isVisible) ? 0 : 1);
			for (int i = 0; i < this.existShipIcons.get_Count(); i++)
			{
				TweenAlpha.Begin(this.existShipIcons.get_Item(i).get_gameObject(), 0.2f, alpha);
			}
			if (isVisible)
			{
				this.OrganizeMessage.UpdateVisible();
			}
			else
			{
				this.OrganizeMessage.setVisible(false);
			}
		}

		public void SetColliderEnable(bool isEnable)
		{
			StrategyShip[] array = this.allShipIcons;
			for (int i = 0; i < array.Length; i++)
			{
				StrategyShip strategyShip = array[i];
				strategyShip.setColliderEnable(isEnable);
			}
		}

		public DeckModel getNextDeck(int nowDeckID, bool isSeachLocalArea)
		{
			return this.getDeck(nowDeckID, true, isSeachLocalArea);
		}

		public DeckModel getPrevDeck(int nowDeckID, bool isSeachLocalArea)
		{
			return this.getDeck(nowDeckID, false, isSeachLocalArea);
		}

		private DeckModel getDeck(int nowDeckID, bool isNext, bool isSeachLocalArea)
		{
			int num = 0;
			List<StrategyShip> list = this.existShipIcons;
			if (isSeachLocalArea)
			{
				list = Enumerable.ToList<StrategyShip>(Enumerable.Where<StrategyShip>(this.existShipIcons, (StrategyShip x) => x.deck.AreaId == StrategyTopTaskManager.Instance.TileManager.FocusTile.areaID));
			}
			if (list.get_Count() != 0)
			{
				for (int i = 0; i < list.get_Count(); i++)
				{
					if (list.get_Item(i).deck.Id == nowDeckID)
					{
						num = i;
						break;
					}
				}
				int num2 = (!isNext) ? -1 : 1;
				int num3;
				if (this.nowFocusDisableDeckNo != -1 && !isSeachLocalArea)
				{
					num3 = ((!isNext) ? (list.get_Count() - 1) : 0);
					this.nowFocusDisableDeckNo = -1;
				}
				else
				{
					if (this.isIndexOver(num + num2, list.get_Count() - 1) && this.OrganizeMessage.isVisible && !isSeachLocalArea)
					{
						DeckModel deckModel = StrategyShipManager.getDisableDecks(StrategyTopTaskManager.GetLogicManager().UserInfo.GetDecks())[0];
						this.nowFocusDisableDeckNo = deckModel.Id;
						return deckModel;
					}
					num3 = (int)Util.LoopValue(num + num2, 0f, (float)(list.get_Count() - 1));
				}
				return list.get_Item(num3).deck;
			}
			DeckModel deckModel2 = Enumerable.FirstOrDefault<DeckModel>(StrategyTopTaskManager.GetLogicManager().UserInfo.GetDecks(), (DeckModel x) => x.Count == 0);
			if (deckModel2 != null)
			{
				this.nowFocusDisableDeckNo = deckModel2.Id;
				return deckModel2;
			}
			return SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck;
		}

		private bool isIndexOver(int value, int MaxValue)
		{
			return value < 0 || value > MaxValue;
		}

		private bool isDisableDeck(int deckNo)
		{
			return StrategyTopTaskManager.GetLogicManager().UserInfo.GetDeck(deckNo).Count == 0;
		}
	}
}
