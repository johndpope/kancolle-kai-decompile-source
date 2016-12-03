using Common.Enum;
using local.models;
using Server_Common.Formats;
using Server_Controllers;
using Server_Models;
using System;
using System.Collections.Generic;

namespace local.managers
{
	public class PracticeManager : ManagerBase
	{
		private Api_req_PracticeDeck _reqPrac;

		private DeckModel _current_deck;

		private Dictionary<DeckPracticeType, bool> _valid_deck_prac_type_dic;

		private List<DeckModel> _rival_decks;

		private Dictionary<int, List<IsGoCondition>> _validation_results;

		public DeckModel CurrentDeck
		{
			get
			{
				return this._current_deck;
			}
		}

		public Dictionary<DeckPracticeType, bool> DeckPracticeTypeDic
		{
			get
			{
				return this._valid_deck_prac_type_dic;
			}
		}

		public List<DeckModel> RivalDecks
		{
			get
			{
				return this._rival_decks;
			}
		}

		public MapAreaModel MapArea
		{
			get
			{
				return ManagerBase._area.get_Item(this._current_deck.AreaId);
			}
		}

		public PracticeManager(int deck_id)
		{
			this._current_deck = base.UserInfo.GetDeck(deck_id);
			base._CreateMapAreaModel();
			this._reqPrac = new Api_req_PracticeDeck();
			this._valid_deck_prac_type_dic = new Dictionary<DeckPracticeType, bool>();
			Api_Result<Mem_deckpractice> api_Result = new Api_get_Member().DeckPractice();
			if (api_Result.state == Api_Result_State.Success)
			{
				Mem_deckpractice data = api_Result.data;
				this._InitValidDeckPracType(DeckPracticeType.Normal, data, ref this._valid_deck_prac_type_dic);
				this._InitValidDeckPracType(DeckPracticeType.Hou, data, ref this._valid_deck_prac_type_dic);
				this._InitValidDeckPracType(DeckPracticeType.Rai, data, ref this._valid_deck_prac_type_dic);
				this._InitValidDeckPracType(DeckPracticeType.Taisen, data, ref this._valid_deck_prac_type_dic);
				this._InitValidDeckPracType(DeckPracticeType.Kouku, data, ref this._valid_deck_prac_type_dic);
				this._InitValidDeckPracType(DeckPracticeType.Sougou, data, ref this._valid_deck_prac_type_dic);
			}
			DeckModel[] decks = this.MapArea.GetDecks();
			this._rival_decks = new List<DeckModel>();
			this._validation_results = new Dictionary<int, List<IsGoCondition>>();
			for (int i = 0; i < decks.Length; i++)
			{
				DeckModel deckModel = decks[i];
				if (deckModel.Count != 0)
				{
					if (deckModel.Id != this._current_deck.Id)
					{
						this._rival_decks.Add(deckModel);
						this._validation_results.Add(deckModel.Id, deckModel.IsValidPractice());
					}
				}
			}
			this._validation_results.Add(this._current_deck.Id, this._current_deck.IsValidPractice());
		}

		public List<IsGoCondition> IsValidPractice(int deck_id)
		{
			List<IsGoCondition> result;
			if (this._validation_results.TryGetValue(deck_id, ref result))
			{
				return result;
			}
			List<IsGoCondition> list = new List<IsGoCondition>();
			list.Add(IsGoCondition.Invalid);
			return list;
		}

		public bool IsValidDeckPractice()
		{
			return this.IsValidPractice(this._current_deck.Id).get_Count() == 0;
		}

		public DeckPracticeResultModel StartDeckPractice(DeckPracticeType type)
		{
			Dictionary<int, int> exp_rates_before = new Dictionary<int, int>();
			this.CurrentDeck.__CreateShipExpRatesDictionary__(ref exp_rates_before);
			Api_Result<PracticeDeckResultFmt> resultData = this._reqPrac.GetResultData(type, this.CurrentDeck.Id);
			if (resultData.state != Api_Result_State.Success)
			{
				return null;
			}
			return new DeckPracticeResultModel(type, resultData.data, base.UserInfo, exp_rates_before);
		}

		public bool IsValidBattlePractice()
		{
			List<IsGoCondition> list = this.IsValidPractice(this._current_deck.Id);
			if (list.get_Count() > 0)
			{
				return false;
			}
			for (int i = 0; i < this._rival_decks.get_Count(); i++)
			{
				DeckModel deckModel = this._rival_decks.get_Item(i);
				if (this.IsValidPractice(deckModel.Id).get_Count() == 0)
				{
					return true;
				}
			}
			return false;
		}

		public PracticeBattleManager StartBattlePractice(int enemy_deck_id, BattleFormationKinds1 formation_id)
		{
			if (!this.IsValidBattlePractice())
			{
				return null;
			}
			if (this._rival_decks.Find((DeckModel d) => d.Id == enemy_deck_id) == null)
			{
				return null;
			}
			PracticeBattleManager practiceBattleManager = new PracticeBattleManager();
			practiceBattleManager.__Init__(this._current_deck.Id, enemy_deck_id, formation_id);
			return practiceBattleManager;
		}

		public PracticeBattleManager StartBattlePractice_Write(int enemy_deck_id, BattleFormationKinds1 formation_id)
		{
			if (!this.IsValidBattlePractice())
			{
				return null;
			}
			if (this._rival_decks.Find((DeckModel d) => d.Id == enemy_deck_id) == null)
			{
				return null;
			}
			PracticeBattleManager practiceBattleManager = new PracticeBattleManager_Write();
			practiceBattleManager.__Init__(this._current_deck.Id, enemy_deck_id, formation_id);
			return practiceBattleManager;
		}

		public PracticeBattleManager StartBattlePractice_Read(int enemy_deck_id, BattleFormationKinds1 formation_id)
		{
			return new PracticeBattleManager_Read(this._current_deck.Id, enemy_deck_id, formation_id);
		}

		private void _InitValidDeckPracType(DeckPracticeType type, Mem_deckpractice mem_dp, ref Dictionary<DeckPracticeType, bool> list)
		{
			if (mem_dp[type])
			{
				this._valid_deck_prac_type_dic.set_Item(type, this._reqPrac.PrackticeDeckCheck(type, this.CurrentDeck.Id));
			}
		}

		public override string ToString()
		{
			string text = string.Empty;
			text += string.Format("{0}\n", base.ToString());
			text += string.Format("選択中の艦隊-{0}\n", this.CurrentDeck);
			text += string.Format("--選択可能な艦隊演習--\n", new object[0]);
			text += this.ToString(DeckPracticeType.Normal, this.DeckPracticeTypeDic);
			text += this.ToString(DeckPracticeType.Hou, this.DeckPracticeTypeDic);
			text += this.ToString(DeckPracticeType.Rai, this.DeckPracticeTypeDic);
			text += this.ToString(DeckPracticeType.Taisen, this.DeckPracticeTypeDic);
			text += this.ToString(DeckPracticeType.Kouku, this.DeckPracticeTypeDic);
			text += this.ToString(DeckPracticeType.Sougou, this.DeckPracticeTypeDic);
			text += "\n";
			text += string.Format("--選択可能な対抗演習相手--\n", new object[0]);
			for (int i = 0; i < this.RivalDecks.get_Count(); i++)
			{
				List<IsGoCondition> list = this.IsValidPractice(this.RivalDecks.get_Item(i).Id);
				if (list.get_Count() == 0)
				{
					text += string.Format("{0}\n", this.RivalDecks.get_Item(i));
				}
				else
				{
					text += string.Format("{0} - [", this.RivalDecks.get_Item(i));
					for (int j = 0; j < list.get_Count(); j++)
					{
						text = text + list.get_Item(j) + ", ";
					}
					text += "]\n";
				}
			}
			return text;
		}

		private string ToString(DeckPracticeType type, Dictionary<DeckPracticeType, bool> dic)
		{
			List<string> list = new List<string>();
			list.Add(string.Empty);
			list.Add("艦隊行動演習");
			list.Add("砲戦演習");
			list.Add("雷撃戦演習");
			list.Add("対潜戦演習");
			list.Add("航空戦演習");
			list.Add("総合演習");
			List<string> list2 = list;
			string text = string.Format("{0}-{1} ", list2.get_Item((int)type), type);
			if (dic.ContainsKey(type))
			{
				text += string.Format("{0}", (!dic.get_Item(type)) ? "[選択不可]" : string.Empty);
			}
			else
			{
				text += string.Format("[未開放]", new object[0]);
			}
			return text + "\n";
		}
	}
}
