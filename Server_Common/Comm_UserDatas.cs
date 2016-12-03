using Common.Enum;
using Common.SaveManager;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Server_Common
{
	public class Comm_UserDatas
	{
		private static Comm_UserDatas _instance;

		private Dictionary<int, Mem_ship> _user_ship;

		private Dictionary<int, Mem_slotitem> _user_slot;

		private Dictionary<int, Mem_deck> _user_deck;

		private Dictionary<int, Mem_esccort_deck> _user_escortdeck;

		private Dictionary<enumMaterialCategory, Mem_material> _user_material;

		private Dictionary<int, Mem_useitem> _user_useItem;

		private Mem_basic _user_basic;

		private Mem_record _user_record;

		private Dictionary<int, Mem_ndock> _user_ndock;

		private Dictionary<int, Mem_kdock> _user_kdock;

		private Dictionary<int, Mem_room> _user_room;

		private Mem_turn _user_turn;

		private Dictionary<int, Mem_book> _ship_book;

		private Dictionary<int, Mem_book> _slot_book;

		private Dictionary<int, Mem_mapcomp> _user_mapcomp;

		private Dictionary<int, Mem_mapclear> _user_mapclear;

		private Dictionary<int, Mem_missioncomp> _user_missioncomp;

		private Dictionary<int, Mem_furniture> _user_furniture;

		private Dictionary<int, Mem_quest> _user_quest;

		private Dictionary<int, Mem_tanker> _user_tanker;

		private Dictionary<int, Mem_rebellion_point> _user_rebellion_point;

		private Mem_deckpractice _user_deckpractice;

		private Dictionary<int, Mem_questcount> _user_questcount;

		private HashSet<int> _temp_escortship;

		private HashSet<int> _temp_deckship;

		private Dictionary<int, List<Mem_history>> _user_history;

		private Mem_trophy _user_trophy;

		private Mem_newgame_plus _user_plus;

		public static Comm_UserDatas Instance
		{
			get
			{
				if (Comm_UserDatas._instance == null)
				{
					Comm_UserDatas._instance = new Comm_UserDatas();
				}
				return Comm_UserDatas._instance;
			}
			private set
			{
				Comm_UserDatas._instance = value;
			}
		}

		public Dictionary<int, Mem_ship> User_ship
		{
			get
			{
				return this._user_ship;
			}
			private set
			{
				this._user_ship = value;
			}
		}

		public Dictionary<int, Mem_slotitem> User_slot
		{
			get
			{
				return this._user_slot;
			}
			private set
			{
				this._user_slot = value;
			}
		}

		public Dictionary<int, Mem_deck> User_deck
		{
			get
			{
				return this._user_deck;
			}
			private set
			{
				this._user_deck = value;
			}
		}

		public Dictionary<int, Mem_esccort_deck> User_EscortDeck
		{
			get
			{
				return this._user_escortdeck;
			}
			private set
			{
				this._user_escortdeck = value;
			}
		}

		public Dictionary<enumMaterialCategory, Mem_material> User_material
		{
			get
			{
				return this._user_material;
			}
			private set
			{
				this._user_material = value;
			}
		}

		public Dictionary<int, Mem_useitem> User_useItem
		{
			get
			{
				return this._user_useItem;
			}
			private set
			{
				this._user_useItem = value;
			}
		}

		public Mem_basic User_basic
		{
			get
			{
				return this._user_basic;
			}
			private set
			{
				this._user_basic = value;
			}
		}

		public Mem_record User_record
		{
			get
			{
				return this._user_record;
			}
			private set
			{
				this._user_record = value;
			}
		}

		public Dictionary<int, Mem_ndock> User_ndock
		{
			get
			{
				return this._user_ndock;
			}
			private set
			{
				this._user_ndock = value;
			}
		}

		public Dictionary<int, Mem_kdock> User_kdock
		{
			get
			{
				return this._user_kdock;
			}
			private set
			{
				this._user_kdock = value;
			}
		}

		public Dictionary<int, Mem_room> User_room
		{
			get
			{
				return this._user_room;
			}
			private set
			{
				this._user_room = value;
			}
		}

		public Mem_turn User_turn
		{
			get
			{
				return this._user_turn;
			}
			private set
			{
				this._user_turn = value;
			}
		}

		public Dictionary<int, Mem_book> Ship_book
		{
			get
			{
				return this._ship_book;
			}
			private set
			{
				this._ship_book = value;
			}
		}

		public Dictionary<int, Mem_book> Slot_book
		{
			get
			{
				return this._slot_book;
			}
			private set
			{
				this._slot_book = value;
			}
		}

		public Dictionary<int, Mem_mapcomp> User_mapcomp
		{
			get
			{
				return this._user_mapcomp;
			}
			private set
			{
				this._user_mapcomp = value;
			}
		}

		public Dictionary<int, Mem_mapclear> User_mapclear
		{
			get
			{
				return this._user_mapclear;
			}
			private set
			{
				this._user_mapclear = value;
			}
		}

		public Dictionary<int, Mem_missioncomp> User_missioncomp
		{
			get
			{
				return this._user_missioncomp;
			}
			private set
			{
				this._user_missioncomp = value;
			}
		}

		public Dictionary<int, Mem_furniture> User_furniture
		{
			get
			{
				return this._user_furniture;
			}
			private set
			{
				this._user_furniture = value;
			}
		}

		public Dictionary<int, Mem_quest> User_quest
		{
			get
			{
				return this._user_quest;
			}
			private set
			{
				this._user_quest = value;
			}
		}

		public Dictionary<int, Mem_tanker> User_tanker
		{
			get
			{
				return this._user_tanker;
			}
			private set
			{
				this._user_tanker = value;
			}
		}

		public Dictionary<int, Mem_rebellion_point> User_rebellion_point
		{
			get
			{
				return this._user_rebellion_point;
			}
			private set
			{
				this._user_rebellion_point = value;
			}
		}

		public Mem_deckpractice User_deckpractice
		{
			get
			{
				return this._user_deckpractice;
			}
			private set
			{
				this._user_deckpractice = value;
			}
		}

		public Dictionary<int, Mem_questcount> User_questcount
		{
			get
			{
				return this._user_questcount;
			}
			private set
			{
				this._user_questcount = value;
			}
		}

		public HashSet<int> Temp_escortship
		{
			get
			{
				return this._temp_escortship;
			}
			private set
			{
				this._temp_escortship = value;
			}
		}

		public HashSet<int> Temp_deckship
		{
			get
			{
				return this._temp_deckship;
			}
			private set
			{
				this._temp_deckship = value;
			}
		}

		public Dictionary<int, List<Mem_history>> User_history
		{
			get
			{
				return this._user_history;
			}
			set
			{
				this._user_history = value;
			}
		}

		public Mem_trophy User_trophy
		{
			get
			{
				return this._user_trophy;
			}
			private set
			{
				this._user_trophy = value;
			}
		}

		public Mem_newgame_plus User_plus
		{
			get
			{
				return this._user_plus;
			}
			private set
			{
				this._user_plus = value;
			}
		}

		private Comm_UserDatas()
		{
			this.User_ship = new Dictionary<int, Mem_ship>();
			this.User_deck = new Dictionary<int, Mem_deck>(8);
			this.User_EscortDeck = new Dictionary<int, Mem_esccort_deck>(20);
			this.User_slot = new Dictionary<int, Mem_slotitem>();
			this.User_material = new Dictionary<enumMaterialCategory, Mem_material>();
			this.User_useItem = new Dictionary<int, Mem_useitem>();
			this.User_ndock = new Dictionary<int, Mem_ndock>();
			this.User_kdock = new Dictionary<int, Mem_kdock>(4);
			this.User_tanker = new Dictionary<int, Mem_tanker>();
			this.Ship_book = new Dictionary<int, Mem_book>();
			this.Slot_book = new Dictionary<int, Mem_book>();
			this.User_mapcomp = new Dictionary<int, Mem_mapcomp>();
			this.User_mapclear = new Dictionary<int, Mem_mapclear>();
			this.User_missioncomp = new Dictionary<int, Mem_missioncomp>();
			this.User_furniture = new Dictionary<int, Mem_furniture>();
			this.User_quest = new Dictionary<int, Mem_quest>();
			this.User_questcount = new Dictionary<int, Mem_questcount>();
			this.User_rebellion_point = new Dictionary<int, Mem_rebellion_point>();
			this.User_room = new Dictionary<int, Mem_room>();
			this.Temp_escortship = new HashSet<int>();
			this.Temp_deckship = new HashSet<int>();
			this.User_history = new Dictionary<int, List<Mem_history>>();
		}

		public bool SetUserData()
		{
			XElement elements = VitaSaveManager.Instance.Elements;
			if (elements == null)
			{
				return false;
			}
			Mem_basic user_basic = Model_Base.SetUserData<Mem_basic>(Enumerable.First<XElement>(elements.Elements(Mem_basic.tableName)));
			Mem_record user_record = Model_Base.SetUserData<Mem_record>(Enumerable.First<XElement>(elements.Elements(Mem_record.tableName)));
			Mem_turn user_turn = Model_Base.SetUserData<Mem_turn>(Enumerable.First<XElement>(elements.Elements(Mem_turn.tableName)));
			Mem_deckpractice user_deckpractice = Model_Base.SetUserData<Mem_deckpractice>(Enumerable.First<XElement>(elements.Elements(Mem_deckpractice.tableName)));
			XElement xElement = Enumerable.FirstOrDefault<XElement>(elements.Elements(Mem_trophy.tableName));
			Mem_trophy user_trophy;
			if (xElement == null)
			{
				user_trophy = new Mem_trophy();
			}
			else
			{
				user_trophy = Model_Base.SetUserData<Mem_trophy>(xElement);
			}
			XElement xElement2 = Enumerable.FirstOrDefault<XElement>(elements.Elements(Mem_newgame_plus.tableName));
			Mem_newgame_plus user_plus;
			if (xElement2 == null)
			{
				user_plus = new Mem_newgame_plus();
			}
			else
			{
				user_plus = Model_Base.SetUserData<Mem_newgame_plus>(xElement2);
			}
			this.User_basic = null;
			this.User_basic = user_basic;
			this.User_record = null;
			this.User_record = user_record;
			this.User_turn = null;
			this.User_turn = user_turn;
			this.User_deckpractice = null;
			this.User_deckpractice = user_deckpractice;
			this.User_trophy = user_trophy;
			this.User_plus = user_plus;
			Dictionary<int, Mem_book> dictionary = new Dictionary<int, Mem_book>();
			Dictionary<int, Mem_book> dictionary2 = new Dictionary<int, Mem_book>();
			Dictionary<int, Mem_deck> dictionary3 = new Dictionary<int, Mem_deck>();
			Dictionary<int, Mem_esccort_deck> dictionary4 = new Dictionary<int, Mem_esccort_deck>();
			Dictionary<int, Mem_furniture> dictionary5 = new Dictionary<int, Mem_furniture>();
			Dictionary<int, Mem_kdock> dictionary6 = new Dictionary<int, Mem_kdock>();
			Dictionary<int, Mem_mapcomp> dictionary7 = new Dictionary<int, Mem_mapcomp>();
			Dictionary<int, Mem_mapclear> dictionary8 = new Dictionary<int, Mem_mapclear>();
			Dictionary<enumMaterialCategory, Mem_material> dictionary9 = new Dictionary<enumMaterialCategory, Mem_material>();
			Dictionary<int, Mem_missioncomp> dictionary10 = new Dictionary<int, Mem_missioncomp>();
			Dictionary<int, Mem_ndock> dictionary11 = new Dictionary<int, Mem_ndock>();
			Dictionary<int, Mem_quest> dictionary12 = new Dictionary<int, Mem_quest>();
			Dictionary<int, Mem_questcount> dictionary13 = new Dictionary<int, Mem_questcount>();
			Dictionary<int, Mem_ship> dictionary14 = new Dictionary<int, Mem_ship>();
			Dictionary<int, Mem_slotitem> dictionary15 = new Dictionary<int, Mem_slotitem>();
			Dictionary<int, Mem_tanker> dictionary16 = new Dictionary<int, Mem_tanker>();
			Dictionary<int, Mem_useitem> dictionary17 = new Dictionary<int, Mem_useitem>();
			Dictionary<int, Mem_rebellion_point> dictionary18 = new Dictionary<int, Mem_rebellion_point>();
			Dictionary<int, Mem_room> dictionary19 = new Dictionary<int, Mem_room>();
			HashSet<int> hashSet = new HashSet<int>();
			HashSet<int> hashSet2 = new HashSet<int>();
			List<Mem_history> list = new List<Mem_history>();
			using (IEnumerator<XElement> enumerator = Extensions.Elements<XElement>(elements.Elements("ship_books"), "mem_book").GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					XElement current = enumerator.get_Current();
					Mem_book mem_book = Model_Base.SetUserData<Mem_book>(current);
					dictionary.Add(mem_book.Table_id, mem_book);
				}
			}
			this.Ship_book.Clear();
			this.Ship_book = dictionary;
			using (IEnumerator<XElement> enumerator2 = Extensions.Elements<XElement>(elements.Elements("slot_books"), "mem_book").GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					XElement current2 = enumerator2.get_Current();
					Mem_book mem_book2 = Model_Base.SetUserData<Mem_book>(current2);
					dictionary2.Add(mem_book2.Table_id, mem_book2);
				}
			}
			this.Slot_book.Clear();
			this.Slot_book = dictionary2;
			using (IEnumerator<XElement> enumerator3 = Extensions.Elements<XElement>(elements.Elements(Mem_deck.tableName + "s"), Mem_deck.tableName).GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					XElement current3 = enumerator3.get_Current();
					Mem_deck mem_deck = Model_Base.SetUserData<Mem_deck>(current3);
					dictionary3.Add(mem_deck.Rid, mem_deck);
				}
			}
			this.User_deck.Clear();
			this.User_deck = dictionary3;
			using (IEnumerator<XElement> enumerator4 = Extensions.Elements<XElement>(elements.Elements(Mem_esccort_deck.tableName + "s"), Mem_esccort_deck.tableName).GetEnumerator())
			{
				while (enumerator4.MoveNext())
				{
					XElement current4 = enumerator4.get_Current();
					Mem_esccort_deck mem_esccort_deck = Model_Base.SetUserData<Mem_esccort_deck>(current4);
					dictionary4.Add(mem_esccort_deck.Rid, mem_esccort_deck);
				}
			}
			this.User_EscortDeck.Clear();
			this.User_EscortDeck = dictionary4;
			using (IEnumerator<XElement> enumerator5 = Extensions.Elements<XElement>(elements.Elements(Mem_furniture.tableName + "s"), Mem_furniture.tableName).GetEnumerator())
			{
				while (enumerator5.MoveNext())
				{
					XElement current5 = enumerator5.get_Current();
					Mem_furniture mem_furniture = Model_Base.SetUserData<Mem_furniture>(current5);
					dictionary5.Add(mem_furniture.Rid, mem_furniture);
				}
			}
			this.User_furniture.Clear();
			this.User_furniture = dictionary5;
			using (IEnumerator<XElement> enumerator6 = Extensions.Elements<XElement>(elements.Elements(Mem_kdock.tableName + "s"), Mem_kdock.tableName).GetEnumerator())
			{
				while (enumerator6.MoveNext())
				{
					XElement current6 = enumerator6.get_Current();
					Mem_kdock mem_kdock = Model_Base.SetUserData<Mem_kdock>(current6);
					dictionary6.Add(mem_kdock.Rid, mem_kdock);
				}
			}
			this.User_kdock.Clear();
			this.User_kdock = dictionary6;
			using (IEnumerator<XElement> enumerator7 = Extensions.Elements<XElement>(elements.Elements(Mem_mapcomp.tableName + "s"), Mem_mapcomp.tableName).GetEnumerator())
			{
				while (enumerator7.MoveNext())
				{
					XElement current7 = enumerator7.get_Current();
					Mem_mapcomp mem_mapcomp = Model_Base.SetUserData<Mem_mapcomp>(current7);
					dictionary7.Add(mem_mapcomp.Rid, mem_mapcomp);
				}
			}
			this.User_mapcomp.Clear();
			this.User_mapcomp = dictionary7;
			using (IEnumerator<XElement> enumerator8 = Extensions.Elements<XElement>(elements.Elements(Mem_mapclear.tableName + "s"), Mem_mapclear.tableName).GetEnumerator())
			{
				while (enumerator8.MoveNext())
				{
					XElement current8 = enumerator8.get_Current();
					Mem_mapclear mem_mapclear = Model_Base.SetUserData<Mem_mapclear>(current8);
					dictionary8.Add(mem_mapclear.Rid, mem_mapclear);
				}
			}
			this.User_mapclear.Clear();
			this.User_mapclear = dictionary8;
			using (IEnumerator<XElement> enumerator9 = Extensions.Elements<XElement>(elements.Elements(Mem_material.tableName + "s"), Mem_material.tableName).GetEnumerator())
			{
				while (enumerator9.MoveNext())
				{
					XElement current9 = enumerator9.get_Current();
					Mem_material mem_material = Model_Base.SetUserData<Mem_material>(current9);
					dictionary9.Add(mem_material.Rid, mem_material);
				}
			}
			this.User_material.Clear();
			this.User_material = dictionary9;
			using (IEnumerator<XElement> enumerator10 = Extensions.Elements<XElement>(elements.Elements(Mem_missioncomp.tableName + "s"), Mem_missioncomp.tableName).GetEnumerator())
			{
				while (enumerator10.MoveNext())
				{
					XElement current10 = enumerator10.get_Current();
					Mem_missioncomp mem_missioncomp = Model_Base.SetUserData<Mem_missioncomp>(current10);
					dictionary10.Add(mem_missioncomp.Rid, mem_missioncomp);
				}
			}
			this.User_missioncomp.Clear();
			this.User_missioncomp = dictionary10;
			using (IEnumerator<XElement> enumerator11 = Extensions.Elements<XElement>(elements.Elements(Mem_ndock.tableName + "s"), Mem_ndock.tableName).GetEnumerator())
			{
				while (enumerator11.MoveNext())
				{
					XElement current11 = enumerator11.get_Current();
					Mem_ndock mem_ndock = Model_Base.SetUserData<Mem_ndock>(current11);
					dictionary11.Add(mem_ndock.Rid, mem_ndock);
				}
			}
			this.User_ndock.Clear();
			this.User_ndock = dictionary11;
			using (IEnumerator<XElement> enumerator12 = Extensions.Elements<XElement>(elements.Elements(Mem_quest.tableName + "s"), Mem_quest.tableName).GetEnumerator())
			{
				while (enumerator12.MoveNext())
				{
					XElement current12 = enumerator12.get_Current();
					Mem_quest mem_quest = Model_Base.SetUserData<Mem_quest>(current12);
					dictionary12.Add(mem_quest.Rid, mem_quest);
				}
			}
			this.User_quest.Clear();
			this.User_quest = dictionary12;
			using (IEnumerator<XElement> enumerator13 = Extensions.Elements<XElement>(elements.Elements(Mem_questcount.tableName + "s"), Mem_questcount.tableName).GetEnumerator())
			{
				while (enumerator13.MoveNext())
				{
					XElement current13 = enumerator13.get_Current();
					Mem_questcount mem_questcount = Model_Base.SetUserData<Mem_questcount>(current13);
					dictionary13.Add(mem_questcount.Rid, mem_questcount);
				}
			}
			this.User_questcount.Clear();
			this.User_questcount = dictionary13;
			using (IEnumerator<XElement> enumerator14 = Extensions.Elements<XElement>(elements.Elements(Mem_slotitem.tableName + "s"), Mem_slotitem.tableName).GetEnumerator())
			{
				while (enumerator14.MoveNext())
				{
					XElement current14 = enumerator14.get_Current();
					Mem_slotitem mem_slotitem = Model_Base.SetUserData<Mem_slotitem>(current14);
					dictionary15.Add(mem_slotitem.Rid, mem_slotitem);
				}
			}
			this.User_slot.Clear();
			this.User_slot = dictionary15;
			using (IEnumerator<XElement> enumerator15 = Extensions.Elements<XElement>(elements.Elements(Mem_ship.tableName + "s"), Mem_ship.tableName).GetEnumerator())
			{
				while (enumerator15.MoveNext())
				{
					XElement current15 = enumerator15.get_Current();
					Mem_ship mem_ship = Model_Base.SetUserData<Mem_ship>(current15);
					dictionary14.Add(mem_ship.Rid, mem_ship);
				}
			}
			this.User_ship.Clear();
			this.User_ship = dictionary14;
			using (IEnumerator<XElement> enumerator16 = Extensions.Elements<XElement>(elements.Elements(Mem_tanker.tableName + "s"), Mem_tanker.tableName).GetEnumerator())
			{
				while (enumerator16.MoveNext())
				{
					XElement current16 = enumerator16.get_Current();
					Mem_tanker mem_tanker = Model_Base.SetUserData<Mem_tanker>(current16);
					dictionary16.Add(mem_tanker.Rid, mem_tanker);
				}
			}
			this.User_tanker.Clear();
			this.User_tanker = dictionary16;
			using (IEnumerator<XElement> enumerator17 = Extensions.Elements<XElement>(elements.Elements(Mem_useitem.tableName + "s"), Mem_useitem.tableName).GetEnumerator())
			{
				while (enumerator17.MoveNext())
				{
					XElement current17 = enumerator17.get_Current();
					Mem_useitem mem_useitem = Model_Base.SetUserData<Mem_useitem>(current17);
					dictionary17.Add(mem_useitem.Rid, mem_useitem);
				}
			}
			this.User_useItem.Clear();
			this.User_useItem = dictionary17;
			using (IEnumerator<XElement> enumerator18 = Extensions.Elements<XElement>(elements.Elements(Mem_rebellion_point.tableName + "s"), Mem_rebellion_point.tableName).GetEnumerator())
			{
				while (enumerator18.MoveNext())
				{
					XElement current18 = enumerator18.get_Current();
					Mem_rebellion_point mem_rebellion_point = Model_Base.SetUserData<Mem_rebellion_point>(current18);
					dictionary18.Add(mem_rebellion_point.Rid, mem_rebellion_point);
				}
			}
			this.User_rebellion_point.Clear();
			this.User_rebellion_point = dictionary18;
			using (IEnumerator<XElement> enumerator19 = Extensions.Elements<XElement>(elements.Elements(Mem_room.tableName + "s"), Mem_room.tableName).GetEnumerator())
			{
				while (enumerator19.MoveNext())
				{
					XElement current19 = enumerator19.get_Current();
					Mem_room mem_room = Model_Base.SetUserData<Mem_room>(current19);
					dictionary19.Add(mem_room.Rid, mem_room);
				}
			}
			this.User_room.Clear();
			this.User_room = dictionary19;
			using (IEnumerator<XElement> enumerator20 = elements.Element("temp_escortships").Elements().GetEnumerator())
			{
				while (enumerator20.MoveNext())
				{
					XElement current20 = enumerator20.get_Current();
					string value = current20.get_Value();
					hashSet.Add(int.Parse(value));
				}
			}
			this.Temp_escortship.Clear();
			this.Temp_escortship = hashSet;
			using (IEnumerator<XElement> enumerator21 = elements.Element("temp_deckships").Elements().GetEnumerator())
			{
				while (enumerator21.MoveNext())
				{
					XElement current21 = enumerator21.get_Current();
					string value2 = current21.get_Value();
					hashSet2.Add(int.Parse(value2));
				}
			}
			this.Temp_deckship.Clear();
			this.Temp_deckship = hashSet2;
			using (IEnumerator<XElement> enumerator22 = Extensions.Elements<XElement>(elements.Elements(Mem_history.tableName + "s"), Mem_history.tableName).GetEnumerator())
			{
				while (enumerator22.MoveNext())
				{
					XElement current22 = enumerator22.get_Current();
					Mem_history mem_history = Model_Base.SetUserData<Mem_history>(current22);
					list.Add(mem_history);
				}
			}
			this.User_history.Clear();
			list.ForEach(delegate(Mem_history x)
			{
				this.Add_History(x);
			});
			return true;
		}

		public void PurgeUserData(ICreateNewUser createInstance, bool plusGame)
		{
			if (!plusGame)
			{
				this.User_trophy = null;
				this.User_plus = null;
			}
			this.User_basic = null;
			this.User_record = null;
			this.Ship_book.Clear();
			this.Slot_book.Clear();
			this.User_ship.Clear();
			this.User_slot.Clear();
			this.User_useItem.Clear();
			this.User_turn = null;
			this.User_deckpractice = null;
			this.User_deck.Clear();
			this.User_EscortDeck.Clear();
			this.User_furniture.Clear();
			this.User_ndock.Clear();
			this.User_kdock.Clear();
			this.User_mapcomp.Clear();
			this.User_mapclear.Clear();
			this.User_material.Clear();
			this.User_missioncomp.Clear();
			this.User_quest.Clear();
			this.User_questcount.Clear();
			this.User_tanker.Clear();
			this.User_rebellion_point.Clear();
			this.User_room.Clear();
			this.Temp_escortship.Clear();
			this.Temp_deckship.Clear();
			this.User_history.Clear();
		}

		public bool CreateNewUser(ICreateNewUser createInstance, DifficultKind difficult, int firstShip)
		{
			if (this.User_basic != null || createInstance == null)
			{
				return false;
			}
			this.User_basic = new Mem_basic();
			this.User_basic.SetDifficult(difficult);
			this.User_record = new Mem_record();
			this.User_turn = new Mem_turn();
			this.User_trophy = new Mem_trophy();
			this.User_plus = new Mem_newgame_plus();
			this.User_deckpractice = new Mem_deckpractice();
			if (this.User_ndock.get_Count() == 0)
			{
				this.Add_Ndock(1);
				this.Add_Ndock(1);
			}
			if (this.User_kdock.get_Count() == 0)
			{
				this.Add_Kdock();
				this.Add_Kdock();
			}
			this.initMaterials(difficult);
			this.Add_Deck(1);
			Comm_UserDatas arg_D2_0 = Comm_UserDatas.Instance;
			List<int> list = new List<int>();
			list.Add(firstShip);
			List<int> list2 = arg_D2_0.Add_Ship(list);
			Comm_UserDatas.Instance.User_deck.get_Item(1).Ship[0] = list2.get_Item(0);
			List<Mst_furniture> furnitureDatas = this.User_room.get_Item(1).getFurnitureDatas();
			Mem_furniture furniture = null;
			furnitureDatas.ForEach(delegate(Mst_furniture x)
			{
				furniture = new Mem_furniture(x.Id);
				this.User_furniture.Add(furniture.Rid, furniture);
			});
			list = new List<int>();
			list.Add(42);
			list.Add(43);
			this.Add_Slot(list);
			this.User_quest = new Dictionary<int, Mem_quest>();
			using (Dictionary<int, Mst_maparea>.KeyCollection.Enumerator enumerator = Mst_DataManager.Instance.Mst_maparea.get_Keys().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int current = enumerator.get_Current();
					this.Add_EscortDeck(current, current);
				}
			}
			this.initTanker();
			this.UpdateDeckShipLocale();
			return true;
		}

		public bool NewGamePlus(ICreateNewUser createInstance, string nickName, DifficultKind selectedRank, int firstShipId)
		{
			if (createInstance == null)
			{
				return false;
			}
			bool flag = Utils.IsGameClear();
			List<DifficultKind> kind = Enumerable.ToList<DifficultKind>(this.User_record.ClearDifficult);
			this.PurgeUserData(createInstance, true);
			if (flag)
			{
				this.Add_Useitem(55, 1);
			}
			using (List<Mem_book>.Enumerator enumerator = this.User_plus.Ship_book.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mem_book current = enumerator.get_Current();
					this.Ship_book.Add(current.Table_id, current);
				}
			}
			using (List<Mem_book>.Enumerator enumerator2 = this.User_plus.Slot_book.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					Mem_book current2 = enumerator2.get_Current();
					this.Slot_book.Add(current2.Table_id, current2);
				}
			}
			this.User_basic = new Mem_basic();
			this.User_basic.UpdateNickName(nickName);
			this.User_basic.SetDifficult(selectedRank);
			this.User_record = new Mem_record(createInstance, this.User_plus, kind);
			this.User_turn = new Mem_turn();
			this.User_deckpractice = new Mem_deckpractice();
			if (this.User_ndock.get_Count() == 0)
			{
				this.Add_Ndock(1);
				this.Add_Ndock(1);
			}
			if (this.User_kdock.get_Count() == 0)
			{
				this.Add_Kdock();
				this.Add_Kdock();
			}
			this.initMaterials(selectedRank);
			this.Add_Deck(1);
			List<Mst_furniture> furnitureDatas = this.User_room.get_Item(1).getFurnitureDatas();
			using (List<Mem_furniture>.Enumerator enumerator3 = this.User_plus.Furniture.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					Mem_furniture current3 = enumerator3.get_Current();
					this.User_furniture.Add(current3.Rid, current3);
				}
			}
			using (List<Mem_slotitem>.Enumerator enumerator4 = this.User_plus.Slotitem.GetEnumerator())
			{
				while (enumerator4.MoveNext())
				{
					Mem_slotitem current4 = enumerator4.get_Current();
					this.User_slot.Add(current4.Rid, current4);
				}
			}
			using (List<Mem_shipBase>.Enumerator enumerator5 = this.User_plus.Ship.GetEnumerator())
			{
				while (enumerator5.MoveNext())
				{
					Mem_shipBase current5 = enumerator5.get_Current();
					Mem_ship mem_ship = new Mem_ship();
					Mst_ship mst_data = Mst_DataManager.Instance.Mst_ship.get_Item(current5.Ship_id);
					mem_ship.Set_ShipParam(current5, mst_data, false);
					mem_ship.Set_ShipParamNewGamePlus(createInstance);
					this.User_ship.Add(mem_ship.Rid, mem_ship);
				}
			}
			List<int> list = new List<int>();
			list.Add(firstShipId);
			List<int> list2 = this.Add_Ship(list);
			Comm_UserDatas.Instance.User_deck.get_Item(1).Ship[0] = list2.get_Item(0);
			this.User_quest = new Dictionary<int, Mem_quest>();
			using (Dictionary<int, Mst_maparea>.KeyCollection.Enumerator enumerator6 = Mst_DataManager.Instance.Mst_maparea.get_Keys().GetEnumerator())
			{
				while (enumerator6.MoveNext())
				{
					int current6 = enumerator6.get_Current();
					this.Add_EscortDeck(current6, current6);
				}
			}
			this.initTanker();
			this.UpdateDeckShipLocale();
			return true;
		}

		public void InitQuest(IQuestOperator instance, List<Mst_quest> mst_quset)
		{
			if (this.User_quest.get_Count() != 0 || instance == null)
			{
				return;
			}
			this.User_quest = Mem_quest.GetData(mst_quset);
		}

		private void initMaterials(DifficultKind difficult)
		{
			if (this.User_material == null)
			{
				this.User_material = new Dictionary<enumMaterialCategory, Mem_material>();
			}
			else
			{
				this.User_material.Clear();
			}
			int num = 1500;
			if (difficult == DifficultKind.KOU)
			{
				num = 2000;
			}
			else if (difficult == DifficultKind.OTU)
			{
				num = 3000;
			}
			else if (difficult == DifficultKind.HEI)
			{
				num = 3000;
			}
			else if (difficult == DifficultKind.TEI)
			{
				num = 6000;
			}
			Dictionary<enumMaterialCategory, int> dictionary = new Dictionary<enumMaterialCategory, int>();
			dictionary.Add(enumMaterialCategory.Fuel, num);
			dictionary.Add(enumMaterialCategory.Bull, 3000);
			dictionary.Add(enumMaterialCategory.Steel, 3000);
			dictionary.Add(enumMaterialCategory.Bauxite, 3000);
			dictionary.Add(enumMaterialCategory.Build_Kit, 5);
			dictionary.Add(enumMaterialCategory.Repair_Kit, 5);
			dictionary.Add(enumMaterialCategory.Dev_Kit, 10);
			dictionary.Add(enumMaterialCategory.Revamp_Kit, 0);
			Dictionary<enumMaterialCategory, int> dictionary2 = dictionary;
			using (Dictionary<enumMaterialCategory, int>.Enumerator enumerator = dictionary2.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<enumMaterialCategory, int> current = enumerator.get_Current();
					enumMaterialCategory key = current.get_Key();
					int value = current.get_Value();
					Mem_material mem_material = new Mem_material(key, value);
					this.User_material.Add(key, mem_material);
				}
			}
		}

		private void initTanker()
		{
			if (this.User_tanker == null)
			{
				this.User_tanker = new Dictionary<int, Mem_tanker>();
			}
			else
			{
				this.User_tanker.Clear();
			}
			List<int> list = this.Add_Tanker(4);
			using (Dictionary<int, Mem_tanker>.ValueCollection.Enumerator enumerator = this.User_tanker.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mem_tanker current = enumerator.get_Current();
					current.GoArea(1);
				}
			}
			this.Add_Tanker(2);
		}

		public List<int> Add_Slot(List<int> slot_ids)
		{
			if (slot_ids == null)
			{
				return null;
			}
			List<int> ret_rids = new List<int>();
			int nextSortNo = this.getNextSortNo<Mem_slotitem>(this.User_slot.get_Values(), slot_ids.get_Count());
			slot_ids.ForEach(delegate(int x)
			{
				Mem_slotitem mem_slotitem = new Mem_slotitem();
				int newRid = this.getNewRid(Enumerable.ToList<int>(this.User_slot.get_Keys()));
				if (mem_slotitem.Set_New_SlotData(newRid, nextSortNo, x))
				{
					nextSortNo++;
					this.User_slot.Add(newRid, mem_slotitem);
					ret_rids.Add(newRid);
					this.Add_Book(2, mem_slotitem.Slotitem_id);
				}
			});
			return ret_rids;
		}

		public List<int> Add_Ship(List<int> ship_ids)
		{
			if (ship_ids == null)
			{
				return null;
			}
			List<int> ret_rids = new List<int>();
			int nextSortNo = this.getNextSortNo<Mem_ship>(this.User_ship.get_Values(), ship_ids.get_Count());
			ship_ids.ForEach(delegate(int x)
			{
				Mem_ship mem_ship = new Mem_ship();
				int newRid = this.getNewRid(Enumerable.ToList<int>(this.User_ship.get_Keys()));
				if (mem_ship.Set_New_ShipData(newRid, nextSortNo, x))
				{
					nextSortNo++;
					this.User_ship.Add(newRid, mem_ship);
					ret_rids.Add(newRid);
					this.Add_Book(1, mem_ship.Ship_id);
				}
			});
			return ret_rids;
		}

		public List<int> Add_Tanker(int num)
		{
			List<int> list = new List<int>();
			for (int i = 0; i < num; i++)
			{
				int newRid = this.getNewRid(Enumerable.ToList<int>(this.User_tanker.get_Keys()));
				Mem_tanker mem_tanker = new Mem_tanker(newRid);
				this.User_tanker.Add(newRid, mem_tanker);
				list.Add(newRid);
			}
			return list;
		}

		public void Remove_Tanker(int rid)
		{
			this.User_tanker.Remove(rid);
		}

		public bool Add_EscortDeck(int rid, int area_id)
		{
			if (rid > 20 || this.User_EscortDeck.ContainsKey(rid))
			{
				return false;
			}
			Mem_esccort_deck mem_esccort_deck = new Mem_esccort_deck(rid, area_id);
			this.User_EscortDeck.Add(rid, mem_esccort_deck);
			return true;
		}

		public Mem_book Add_Book(int type, int mst_id)
		{
			Dictionary<int, Mem_book> dictionary = (type != 1) ? this.Slot_book : this.Ship_book;
			Mem_book mem_book = null;
			if (dictionary.TryGetValue(mst_id, ref mem_book))
			{
				return mem_book;
			}
			mem_book = new Mem_book(type, mst_id);
			if (type != 1)
			{
				dictionary.Add(mst_id, mem_book);
				return mem_book;
			}
			string yomi = Mst_DataManager.Instance.Mst_ship.get_Item(mem_book.Table_id).Yomi;
			bool flag = Enumerable.Any<Mem_book>(this.Ship_book.get_Values(), (Mem_book x) => Mst_DataManager.Instance.Mst_ship.get_Item(x.Table_id).Yomi.Equals(yomi) && x.Flag2 == 1);
			if (flag)
			{
				mem_book.UpdateShipBook(true, false);
			}
			dictionary.Add(mst_id, mem_book);
			return mem_book;
		}

		public Mem_ndock Add_Ndock(int area_id)
		{
			int no = Enumerable.Count<Mem_ndock>(Enumerable.Where<Mem_ndock>(this.User_ndock.get_Values(), (Mem_ndock data) => data.Area_id == area_id)) + 1;
			int num = int.Parse(area_id.ToString() + no.ToString());
			Mem_ndock mem_ndock = new Mem_ndock(num, area_id, no);
			this.User_ndock.Add(num, mem_ndock);
			return mem_ndock;
		}

		public Mem_kdock Add_Kdock()
		{
			if (this.User_kdock.get_Count() >= 4)
			{
				return null;
			}
			int num = this.User_kdock.get_Count() + 1;
			Mem_kdock mem_kdock = new Mem_kdock(num);
			this.User_kdock.Add(num, mem_kdock);
			return mem_kdock;
		}

		public void Remove_Ship(List<int> ship_ids)
		{
			if (ship_ids == null)
			{
				return;
			}
			ship_ids.ForEach(delegate(int x)
			{
				this.Remove_Slot(this.User_ship.get_Item(x).Slot);
				this.User_ship.Remove(x);
			});
		}

		public void Remove_Ship(List<Mem_ship> ships)
		{
			if (ships == null)
			{
				return;
			}
			ships.ForEach(delegate(Mem_ship x)
			{
				this.Remove_Slot(x.Slot);
				if (x.IsOpenExSlot())
				{
					List<int> list = new List<int>();
					list.Add(x.Exslot);
					this.Remove_Slot(list);
				}
				this.User_ship.Remove(x.Rid);
			});
		}

		public void Remove_Slot(List<int> slot_ids)
		{
			slot_ids.ForEach(delegate(int x)
			{
				if (x > 0)
				{
					this.User_slot.Remove(x);
				}
			});
		}

		public void Add_Deck(int rid)
		{
			if (rid > 8 || this.User_deck.ContainsKey(rid))
			{
				return;
			}
			Mem_deck mem_deck = new Mem_deck(rid);
			this.User_deck.Add(rid, mem_deck);
			Mem_room mem_room = new Mem_room(rid);
			this.User_room.Add(rid, mem_room);
		}

		public void Add_Useitem(int rid, int count)
		{
			if (Mst_DataManager.Instance.Mst_useitem.get_Item(rid).Usetype == 6 && Mst_DataManager.Instance.Mst_useitem.get_Item(rid).Category == 21)
			{
				this.User_basic.AddCoin(count);
				return;
			}
			Mem_useitem mem_useitem = null;
			if (!this.User_useItem.TryGetValue(rid, ref mem_useitem))
			{
				mem_useitem = new Mem_useitem(rid, count);
				this.User_useItem.Add(mem_useitem.Rid, mem_useitem);
				return;
			}
			mem_useitem.Add_UseItem(count);
		}

		public bool Add_Furniture(int furnitureId)
		{
			if (this.User_furniture.ContainsKey(furnitureId))
			{
				return false;
			}
			this.User_furniture.Add(furnitureId, new Mem_furniture(furnitureId));
			return true;
		}

		public void Add_History(Mem_history history)
		{
			List<Mem_history> list = null;
			if (!this.User_history.TryGetValue(history.Type, ref list))
			{
				list = new List<Mem_history>();
				list.Add(history);
				this.User_history.Add(history.Type, list);
			}
			else
			{
				list.Add(history);
			}
		}

		public void UpdateEscortShipLocale()
		{
			this.Temp_escortship.Clear();
			using (Dictionary<int, Mem_esccort_deck>.ValueCollection.Enumerator enumerator = Comm_UserDatas.Instance.User_EscortDeck.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mem_esccort_deck current = enumerator.get_Current();
					for (int i = 0; i < current.Ship.Count(); i++)
					{
						this.Temp_escortship.Add(current.Ship[i]);
					}
				}
			}
		}

		public void UpdateDeckShipLocale()
		{
			this.Temp_deckship.Clear();
			using (Dictionary<int, Mem_deck>.ValueCollection.Enumerator enumerator = Comm_UserDatas.Instance.User_deck.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mem_deck current = enumerator.get_Current();
					for (int i = 0; i < current.Ship.Count(); i++)
					{
						this.Temp_deckship.Add(current.Ship[i]);
					}
				}
			}
		}

		public void UpdateShipBookBrokenClothState(List<int> targetShipIds)
		{
			HashSet<string> check_yomi = new HashSet<string>();
			targetShipIds.ForEach(delegate(int x)
			{
				check_yomi.Add(Mst_DataManager.Instance.Mst_ship.get_Item(x).Yomi);
			});
			if (check_yomi.get_Count() == 0)
			{
				return;
			}
			List<Mst_ship> list = Enumerable.ToList<Mst_ship>(Enumerable.Where<Mst_ship>(Mst_DataManager.Instance.Mst_ship.get_Values(), (Mst_ship y) => check_yomi.Contains(y.Yomi)));
			list.ForEach(delegate(Mst_ship up_item)
			{
				Mem_book mem_book = null;
				if (Comm_UserDatas.Instance.Ship_book.TryGetValue(up_item.Id, ref mem_book))
				{
					mem_book.UpdateShipBook(true, false);
				}
			});
		}

		private int getNewRid(List<int> target)
		{
			target.Sort((int x, int y) => x.CompareTo(y));
			int i;
			for (i = 1; i <= Enumerable.Count<int>(target); i++)
			{
				if (target.get_Item(i - 1) != i)
				{
					return i;
				}
			}
			return i;
		}

		private int getNextSortNo<T>(IEnumerable<T> targets, int addRecordNum) where T : IReqNewGetNo
		{
			if (Enumerable.Count<T>(targets) == 0)
			{
				return 1;
			}
			List<T> list = Enumerable.ToList<T>(targets);
			list.Sort((T x, T y) => x.GetSortNo().CompareTo(y.GetSortNo()));
			T t = Enumerable.Last<T>(list);
			int num = t.GetSortNo() + 1;
			int num2 = num + (addRecordNum - 1);
			int num3 = 10000;
			if (num < num3 && num2 < num3)
			{
				return num;
			}
			int num4 = 1;
			using (List<T>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					T current = enumerator.get_Current();
					current.ChangeSortNo(num4);
					num4++;
				}
			}
			return num4;
		}
	}
}
