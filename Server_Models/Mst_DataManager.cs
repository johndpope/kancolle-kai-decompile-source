using Server_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_DataManager
	{
		private delegate KeyValuePair<string, IEnumerable<XElement>> masterAsyncDelegate(string mst_name);

		private static Mst_DataManager _instance;

		private Dictionary<int, Mst_ship> _mst_ship;

		private Dictionary<int, Mst_slotitem> _mst_slotitem;

		private Dictionary<int, Mst_slotitem_cost> _mst_slotitem_cost;

		private Dictionary<int, Mst_maparea> _mst_maparea;

		private Dictionary<int, Mst_mapinfo> _mst_mapinfo;

		private Dictionary<int, Mst_mapcell2> _mst_mapcell;

		private Dictionary<int, Mst_mapenemy2> _mst_mapenemy;

		private IEnumerable<XElement> _mst_shipget;

		private Dictionary<int, Mst_useitem> _mst_useitem;

		private Dictionary<int, Mst_stype> _mst_stype;

		private Dictionary<int, Mst_mission2> _mst_mission;

		private Dictionary<int, Mst_shipupgrade> _mst_upgrade;

		private Dictionary<int, Mst_furniture> _mst_furniture;

		private Dictionary<int, Mst_shipgraph> _mst_shipgraph;

		private Dictionary<int, Mst_item_limit> _mst_item_limit;

		private Dictionary<int, Mst_ship_resources> _mst_ship_resources;

		private Dictionary<int, Mst_equip_category> _mst_equip_category;

		private Dictionary<int, Mst_equip_ship> _mst_equip_ship;

		private Dictionary<int, Mst_shipgraphbattle> _mst_shipgraphbattle;

		private Dictionary<MstConstDataIndex, Mst_const> _mst_const;

		private Dictionary<int, Mst_questcount> _mst_questcount;

		private Dictionary<int, Mst_rebellionpoint> _mst_rebellionpoint;

		private Dictionary<int, Mst_bgm_jukebox> _mst_jukebox;

		private Dictionary<int, int> _mst_bgm_season;

		private Dictionary<int, List<Mst_radingtype>> _mst_RadingType;

		private Dictionary<int, Dictionary<int, Mst_radingrate>> _mst_RadingRate;

		private UIBattleRequireMaster _uiBattleMaster;

		private int callCount;

		private int isMasterInit;

		private Action initMasterCallback;

		private Dictionary<string, IEnumerable<XElement>> startMasterElement;

		private object lockObj = new object();

		public static Mst_DataManager Instance
		{
			get
			{
				if (Mst_DataManager._instance == null)
				{
					Mst_DataManager._instance = new Mst_DataManager();
				}
				return Mst_DataManager._instance;
			}
			private set
			{
				Mst_DataManager._instance = value;
			}
		}

		public Dictionary<int, Mst_ship> Mst_ship
		{
			get
			{
				return this._mst_ship;
			}
			private set
			{
				this._mst_ship = value;
			}
		}

		public Dictionary<int, Mst_slotitem> Mst_Slotitem
		{
			get
			{
				return this._mst_slotitem;
			}
			private set
			{
				this._mst_slotitem = value;
			}
		}

		public Dictionary<int, Mst_slotitem_cost> Mst_slotitem_cost
		{
			get
			{
				return this._mst_slotitem_cost;
			}
			private set
			{
				this._mst_slotitem_cost = value;
			}
		}

		public Dictionary<int, Mst_maparea> Mst_maparea
		{
			get
			{
				return this._mst_maparea;
			}
			private set
			{
				this._mst_maparea = value;
			}
		}

		public Dictionary<int, Mst_mapinfo> Mst_mapinfo
		{
			get
			{
				return this._mst_mapinfo;
			}
			private set
			{
				this._mst_mapinfo = value;
			}
		}

		public Dictionary<int, Mst_mapcell2> Mst_mapcell
		{
			get
			{
				return this._mst_mapcell;
			}
			private set
			{
				this._mst_mapcell = value;
			}
		}

		public Dictionary<int, Mst_mapenemy2> Mst_mapenemy
		{
			get
			{
				return this._mst_mapenemy;
			}
			private set
			{
				this._mst_mapenemy = value;
			}
		}

		public IEnumerable<XElement> Mst_shipget
		{
			get
			{
				return this._mst_shipget;
			}
			private set
			{
				this._mst_shipget = value;
			}
		}

		public Dictionary<int, Mst_useitem> Mst_useitem
		{
			get
			{
				return this._mst_useitem;
			}
			private set
			{
				this._mst_useitem = value;
			}
		}

		public Dictionary<int, Mst_stype> Mst_stype
		{
			get
			{
				return this._mst_stype;
			}
			private set
			{
				this._mst_stype = value;
			}
		}

		public Dictionary<int, Mst_mission2> Mst_mission
		{
			get
			{
				return this._mst_mission;
			}
			private set
			{
				this._mst_mission = value;
			}
		}

		public Dictionary<int, Mst_shipupgrade> Mst_upgrade
		{
			get
			{
				return this._mst_upgrade;
			}
			private set
			{
				this._mst_upgrade = value;
			}
		}

		public Dictionary<int, Mst_furniture> Mst_furniture
		{
			get
			{
				return this._mst_furniture;
			}
			private set
			{
				this._mst_furniture = value;
			}
		}

		public Dictionary<int, Mst_shipgraph> Mst_shipgraph
		{
			get
			{
				return this._mst_shipgraph;
			}
			private set
			{
				this._mst_shipgraph = value;
			}
		}

		public Dictionary<int, Mst_item_limit> Mst_item_limit
		{
			get
			{
				return this._mst_item_limit;
			}
			private set
			{
				this._mst_item_limit = value;
			}
		}

		public Dictionary<int, Mst_ship_resources> Mst_ship_resources
		{
			get
			{
				return this._mst_ship_resources;
			}
			private set
			{
				this._mst_ship_resources = value;
			}
		}

		public Dictionary<int, Mst_equip_category> Mst_equip_category
		{
			get
			{
				return this._mst_equip_category;
			}
			private set
			{
				this._mst_equip_category = value;
			}
		}

		public Dictionary<int, Mst_equip_ship> Mst_equip_ship
		{
			get
			{
				return this._mst_equip_ship;
			}
			private set
			{
				this._mst_equip_ship = value;
			}
		}

		public Dictionary<int, Mst_shipgraphbattle> Mst_shipgraphbattle
		{
			get
			{
				return this._mst_shipgraphbattle;
			}
			private set
			{
				this._mst_shipgraphbattle = value;
			}
		}

		public Dictionary<MstConstDataIndex, Mst_const> Mst_const
		{
			get
			{
				return this._mst_const;
			}
			private set
			{
				this._mst_const = value;
			}
		}

		public Dictionary<int, Mst_questcount> Mst_questcount
		{
			get
			{
				return this._mst_questcount;
			}
			private set
			{
				this._mst_questcount = value;
			}
		}

		public Dictionary<int, Mst_rebellionpoint> Mst_RebellionPoint
		{
			get
			{
				return this._mst_rebellionpoint;
			}
			private set
			{
				this._mst_rebellionpoint = value;
			}
		}

		public Dictionary<int, int> Mst_bgm_season
		{
			get
			{
				return this._mst_bgm_season;
			}
			private set
			{
				this._mst_bgm_season = value;
			}
		}

		public Dictionary<int, List<Mst_radingtype>> Mst_RadingType
		{
			get
			{
				return this._mst_RadingType;
			}
			private set
			{
				this._mst_RadingType = value;
			}
		}

		public Dictionary<int, Dictionary<int, Mst_radingrate>> Mst_RadingRate
		{
			get
			{
				return this._mst_RadingRate;
			}
			private set
			{
				this._mst_RadingRate = value;
			}
		}

		public UIBattleRequireMaster UiBattleMaster
		{
			get
			{
				return this._uiBattleMaster;
			}
			private set
			{
				this._uiBattleMaster = value;
			}
		}

		private Mst_DataManager()
		{
			Utils.initMasterPath();
			this.Mst_ship = new Dictionary<int, Mst_ship>();
			this.Mst_ship_resources = new Dictionary<int, Mst_ship_resources>();
			this.Mst_shipgraph = new Dictionary<int, Mst_shipgraph>();
			this.Mst_shipgraphbattle = new Dictionary<int, Mst_shipgraphbattle>();
			this.Mst_Slotitem = new Dictionary<int, Mst_slotitem>();
			this.Mst_slotitem_cost = new Dictionary<int, Mst_slotitem_cost>();
			this.Mst_maparea = new Dictionary<int, Mst_maparea>();
			this.Mst_mapinfo = new Dictionary<int, Mst_mapinfo>();
			this.Mst_mapcell = new Dictionary<int, Mst_mapcell2>();
			this.Mst_mapenemy = new Dictionary<int, Mst_mapenemy2>();
			this.Mst_useitem = new Dictionary<int, Mst_useitem>();
			this.Mst_stype = new Dictionary<int, Mst_stype>();
			this.Mst_mission = new Dictionary<int, Mst_mission2>();
			this.Mst_upgrade = new Dictionary<int, Mst_shipupgrade>();
			this.Mst_furniture = new Dictionary<int, Mst_furniture>();
			this.Mst_item_limit = new Dictionary<int, Mst_item_limit>();
			this.Mst_equip_category = new Dictionary<int, Mst_equip_category>();
			this.Mst_equip_ship = new Dictionary<int, Mst_equip_ship>();
			this.Mst_const = new Dictionary<MstConstDataIndex, Mst_const>();
			this.Mst_questcount = new Dictionary<int, Mst_questcount>();
			this.Mst_RebellionPoint = new Dictionary<int, Mst_rebellionpoint>();
			this._mst_jukebox = new Dictionary<int, Mst_bgm_jukebox>();
			this.Mst_bgm_season = new Dictionary<int, int>();
			this.UiBattleMaster = new UIBattleRequireMaster();
			this.Mst_RadingRate = new Dictionary<int, Dictionary<int, Mst_radingrate>>();
			this.Mst_RadingType = new Dictionary<int, List<Mst_radingtype>>();
		}

		public void LoadStartMaster(Action callBack)
		{
			if (this.isMasterInit == 2 && this.startMasterElement == null)
			{
				callBack.Invoke();
			}
			else if (this.isMasterInit == 0)
			{
				this.initMasterCallback = callBack;
				this.isMasterInit = 1;
				this.startMasterElement = new Dictionary<string, IEnumerable<XElement>>();
				this.loadStartMstData();
			}
		}

		public void SetStartMasterData()
		{
			if (this.startMasterElement == null)
			{
				return;
			}
			Dictionary<string, Action<Model_Base, XElement>> masterSetter = this.getMasterSetter();
			using (Dictionary<string, IEnumerable<XElement>>.Enumerator enumerator = this.startMasterElement.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, IEnumerable<XElement>> current = enumerator.get_Current();
					string key2 = current.get_Key();
					if (masterSetter.ContainsKey(key2))
					{
						using (IEnumerator<XElement> enumerator2 = current.get_Value().GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								XElement current2 = enumerator2.get_Current();
								Model_Base model_Base = null;
								masterSetter.get_Item(key2).Invoke(model_Base, current2);
							}
						}
						Extensions.Remove<XElement>(current.get_Value());
					}
				}
			}
			Dictionary<int, string> dictionary = Enumerable.ToDictionary<XElement, int, string>(this.startMasterElement.get_Item("mst_equip"), (XElement x) => int.Parse(x.Element("Ship_type").get_Value()), (XElement y) => y.Element("Equip_type").get_Value());
			using (Dictionary<int, Mst_stype>.ValueCollection.Enumerator enumerator3 = this.Mst_stype.get_Values().GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					Mst_stype current3 = enumerator3.get_Current();
					string text = null;
					if (dictionary.TryGetValue(current3.Id, ref text))
					{
						List<int> equip = Enumerable.ToList<int>(Array.ConvertAll<string, int>(text.Split(new char[]
						{
							','
						}), (string eqp_val) => int.Parse(eqp_val)));
						current3.SetEquip(equip);
					}
					else
					{
						current3.SetEquip(new List<int>());
					}
				}
			}
			Extensions.Remove<XElement>(this.startMasterElement.get_Item("mst_equip"));
			dictionary.Clear();
			this.Mst_bgm_season = Enumerable.ToDictionary<XElement, int, int>(this.startMasterElement.get_Item("mst_bgm_season"), (XElement key) => int.Parse(key.Element("Id").get_Value()), (XElement val) => int.Parse(val.Element("Bgm_id").get_Value()));
			Extensions.Remove<XElement>(this.startMasterElement.get_Item("mst_bgm_season"));
			this.startMasterElement.Clear();
			this.startMasterElement = null;
		}

		private void loadStartMstData()
		{
			string tableDirMaster = Utils.getTableDirMaster("mst_files");
			string text = tableDirMaster + "mst_files.xml";
			IEnumerable<XElement> enumerable = Enumerable.Select<XElement, XElement>(XElement.Load(text).Elements("item"), (XElement file) => file);
			List<XElement> list = Enumerable.ToList<XElement>(enumerable);
			int num = Enumerable.Count<XElement>(list);
			this.callCount = num;
			Mst_DataManager.masterAsyncDelegate masterAsyncDelegate = new Mst_DataManager.masterAsyncDelegate(this.LoadElements);
			for (int i = 0; i < num; i++)
			{
				string value = list.get_Item(i).get_Value();
				IAsyncResult asyncResult = masterAsyncDelegate.BeginInvoke(value, new AsyncCallback(this.loadCompleteAsynch), masterAsyncDelegate);
			}
		}

		private KeyValuePair<string, IEnumerable<XElement>> LoadElements(string name)
		{
			return new KeyValuePair<string, IEnumerable<XElement>>(name, Utils.Xml_Result(name, name, null));
		}

		private void loadCompleteAsynch(IAsyncResult ar)
		{
			Mst_DataManager.masterAsyncDelegate masterAsyncDelegate = (Mst_DataManager.masterAsyncDelegate)ar.get_AsyncState();
			KeyValuePair<string, IEnumerable<XElement>> keyValuePair = masterAsyncDelegate.EndInvoke(ar);
			object obj = this.lockObj;
			lock (obj)
			{
				this.startMasterElement.Add(keyValuePair.get_Key(), keyValuePair.get_Value());
			}
			Interlocked.Decrement(ref this.callCount);
			if (this.callCount <= 0)
			{
				this.isMasterInit = 2;
				this.initMasterCallback.Invoke();
				this.initMasterCallback = null;
			}
		}

		private Dictionary<string, Action<Model_Base, XElement>> getMasterSetter()
		{
			Dictionary<string, Action<Model_Base, XElement>> dictionary = new Dictionary<string, Action<Model_Base, XElement>>();
			dictionary.Add("mst_ship", delegate(Model_Base x, XElement y)
			{
				Mst_ship mst_ship = (Mst_ship)x;
				Model_Base.SetMaster<Mst_ship>(out mst_ship, y);
				this.Mst_ship.Add(mst_ship.Id, mst_ship);
			});
			dictionary.Add("mst_ship_resources", delegate(Model_Base x, XElement y)
			{
				Mst_ship_resources mst_ship_resources = (Mst_ship_resources)x;
				Model_Base.SetMaster<Mst_ship_resources>(out mst_ship_resources, y);
				this.Mst_ship_resources.Add(mst_ship_resources.Id, mst_ship_resources);
			});
			dictionary.Add("mst_slotitem", delegate(Model_Base x, XElement y)
			{
				Mst_slotitem mst_slotitem = (Mst_slotitem)x;
				Model_Base.SetMaster<Mst_slotitem>(out mst_slotitem, y);
				this.Mst_Slotitem.Add(mst_slotitem.Id, mst_slotitem);
			});
			dictionary.Add("mst_maparea", delegate(Model_Base x, XElement y)
			{
				Mst_maparea mst_maparea = (Mst_maparea)x;
				Model_Base.SetMaster<Mst_maparea>(out mst_maparea, y);
				this.Mst_maparea.Add(mst_maparea.Id, mst_maparea);
			});
			dictionary.Add("mst_mapinfo", delegate(Model_Base x, XElement y)
			{
				Mst_mapinfo mst_mapinfo = (Mst_mapinfo)x;
				Model_Base.SetMaster<Mst_mapinfo>(out mst_mapinfo, y);
				this.Mst_mapinfo.Add(mst_mapinfo.Id, mst_mapinfo);
			});
			dictionary.Add("mst_useitem", delegate(Model_Base x, XElement y)
			{
				Mst_useitem mst_useitem = (Mst_useitem)x;
				Model_Base.SetMaster<Mst_useitem>(out mst_useitem, y);
				this.Mst_useitem.Add(mst_useitem.Id, mst_useitem);
			});
			dictionary.Add("mst_stype", delegate(Model_Base x, XElement y)
			{
				Mst_stype mst_stype = (Mst_stype)x;
				Model_Base.SetMaster<Mst_stype>(out mst_stype, y);
				this.Mst_stype.Add(mst_stype.Id, mst_stype);
			});
			dictionary.Add("mst_mission2", delegate(Model_Base x, XElement y)
			{
				Mst_mission2 mst_mission = (Mst_mission2)x;
				Model_Base.SetMaster<Mst_mission2>(out mst_mission, y);
				this.Mst_mission.Add(mst_mission.Id, mst_mission);
			});
			dictionary.Add("mst_shipupgrade", delegate(Model_Base x, XElement y)
			{
				Mst_shipupgrade mst_shipupgrade = (Mst_shipupgrade)x;
				Model_Base.SetMaster<Mst_shipupgrade>(out mst_shipupgrade, y);
				this.Mst_upgrade.Add(mst_shipupgrade.Id, mst_shipupgrade);
			});
			dictionary.Add("mst_furniture", delegate(Model_Base x, XElement y)
			{
				Mst_furniture mst_furniture = (Mst_furniture)x;
				Model_Base.SetMaster<Mst_furniture>(out mst_furniture, y);
				this.Mst_furniture.Add(mst_furniture.Id, mst_furniture);
			});
			dictionary.Add("mst_shipgraph", delegate(Model_Base x, XElement y)
			{
				Mst_shipgraph mst_shipgraph = (Mst_shipgraph)x;
				Model_Base.SetMaster<Mst_shipgraph>(out mst_shipgraph, y);
				this.Mst_shipgraph.Add(mst_shipgraph.Id, mst_shipgraph);
			});
			dictionary.Add("mst_item_limit", delegate(Model_Base x, XElement y)
			{
				Mst_item_limit mst_item_limit = (Mst_item_limit)x;
				Model_Base.SetMaster<Mst_item_limit>(out mst_item_limit, y);
				this.Mst_item_limit.Add(mst_item_limit.Id, mst_item_limit);
			});
			dictionary.Add("mst_equip_category", delegate(Model_Base x, XElement y)
			{
				Mst_equip_category mst_equip_category = (Mst_equip_category)x;
				Model_Base.SetMaster<Mst_equip_category>(out mst_equip_category, y);
				this.Mst_equip_category.Add(mst_equip_category.Id, mst_equip_category);
			});
			dictionary.Add("mst_equip_ship", delegate(Model_Base x, XElement y)
			{
				Mst_equip_ship mst_equip_ship = (Mst_equip_ship)x;
				Model_Base.SetMaster<Mst_equip_ship>(out mst_equip_ship, y);
				this.Mst_equip_ship.Add(mst_equip_ship.Id, mst_equip_ship);
			});
			dictionary.Add("mst_shipgraphbattle", delegate(Model_Base x, XElement y)
			{
				Mst_shipgraphbattle mst_shipgraphbattle = (Mst_shipgraphbattle)x;
				Model_Base.SetMaster<Mst_shipgraphbattle>(out mst_shipgraphbattle, y);
				this.Mst_shipgraphbattle.Add(mst_shipgraphbattle.Id, mst_shipgraphbattle);
			});
			dictionary.Add("mst_const", delegate(Model_Base x, XElement y)
			{
				Mst_const mst_const = (Mst_const)x;
				Model_Base.SetMaster<Mst_const>(out mst_const, y);
				this.Mst_const.Add(mst_const.Id, mst_const);
			});
			dictionary.Add("mst_questcount", delegate(Model_Base x, XElement y)
			{
				Mst_questcount mst_questcount = (Mst_questcount)x;
				Model_Base.SetMaster<Mst_questcount>(out mst_questcount, y);
				this.Mst_questcount.Add(mst_questcount.Id, mst_questcount);
			});
			dictionary.Add("mst_rebellionpoint", delegate(Model_Base x, XElement y)
			{
				Mst_rebellionpoint mst_rebellionpoint = (Mst_rebellionpoint)x;
				Model_Base.SetMaster<Mst_rebellionpoint>(out mst_rebellionpoint, y);
				this.Mst_RebellionPoint.Add(mst_rebellionpoint.Id, mst_rebellionpoint);
			});
			dictionary.Add(Mst_bgm_jukebox.tableName, delegate(Model_Base x, XElement y)
			{
				Mst_bgm_jukebox mst_bgm_jukebox = (Mst_bgm_jukebox)x;
				Model_Base.SetMaster<Mst_bgm_jukebox>(out mst_bgm_jukebox, y);
				this._mst_jukebox.Add(mst_bgm_jukebox.Bgm_id, mst_bgm_jukebox);
			});
			dictionary.Add(Mst_radingtype.tableName, delegate(Model_Base x, XElement y)
			{
				Mst_radingtype mst_radingtype = (Mst_radingtype)x;
				Model_Base.SetMaster<Mst_radingtype>(out mst_radingtype, y);
				List<Mst_radingtype> list = null;
				if (!this.Mst_RadingType.TryGetValue(mst_radingtype.Difficult, ref list))
				{
					list = new List<Mst_radingtype>();
					this.Mst_RadingType.Add(mst_radingtype.Difficult, list);
				}
				list.Add(mst_radingtype);
			});
			dictionary.Add(Mst_radingrate.tableName, delegate(Model_Base x, XElement y)
			{
				Mst_radingrate mst_radingrate = (Mst_radingrate)x;
				Model_Base.SetMaster<Mst_radingrate>(out mst_radingrate, y);
				if (!this.Mst_RadingRate.ContainsKey(mst_radingrate.Maparea_id))
				{
					Dictionary<int, Mst_radingrate> dictionary2 = new Dictionary<int, Mst_radingrate>();
					dictionary2.Add(mst_radingrate.Rading_type, mst_radingrate);
					this.Mst_RadingRate.Add(mst_radingrate.Maparea_id, dictionary2);
				}
				else
				{
					this.Mst_RadingRate.get_Item(mst_radingrate.Maparea_id).Add(mst_radingrate.Rading_type, mst_radingrate);
				}
			});
			return dictionary;
		}

		public void Make_MapCell(int maparea_id, int mapinfo_no)
		{
			string text = Utils.getTableDirMaster(Mst_mapcell2.tableName) + "mst_mapcell/";
			string path = string.Concat(new string[]
			{
				text,
				Mst_mapcell2.tableName,
				"_",
				maparea_id.ToString(),
				mapinfo_no.ToString(),
				".xml"
			});
			IEnumerable<XElement> enumerable = Utils.Xml_Result_To_Path(path, Mst_mapcell2.tableName, string.Empty);
			if (enumerable == null)
			{
				return;
			}
			this.Mst_mapcell.Clear();
			using (IEnumerator<XElement> enumerator = enumerable.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					XElement current = enumerator.get_Current();
					Mst_mapcell2 mst_mapcell = null;
					Model_Base.SetMaster<Mst_mapcell2>(out mst_mapcell, current);
					this.Mst_mapcell.Add(mst_mapcell.No, mst_mapcell);
				}
			}
		}

		public void Make_Mapenemy(int maparea_id, int mapinfo_no)
		{
			string text = Utils.getTableDirMaster(Mst_mapenemy2.tableName) + "mst_mapenemy/";
			string path = string.Concat(new string[]
			{
				text,
				Mst_mapenemy2.tableName,
				"_",
				maparea_id.ToString(),
				mapinfo_no.ToString(),
				".xml"
			});
			IEnumerable<XElement> enumerable = Utils.Xml_Result_To_Path(path, Mst_mapenemy2.tableName, string.Empty);
			if (enumerable == null)
			{
				return;
			}
			this.Mst_mapenemy.Clear();
			using (IEnumerator<XElement> enumerator = enumerable.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					XElement current = enumerator.get_Current();
					Mst_mapenemy2 mst_mapenemy = null;
					Model_Base.SetMaster<Mst_mapenemy2>(out mst_mapenemy, current);
					this.Mst_mapenemy.Add(mst_mapenemy.Id, mst_mapenemy);
				}
			}
		}

		public ILookup<int, Mst_mapenemylevel> GetMapenemyLevel(int maparea_id, int mapinfo_no)
		{
			string text = Utils.getTableDirMaster(Mst_mapenemylevel.tableName) + "mst_mapenemylevel/";
			string path = string.Concat(new string[]
			{
				text,
				Mst_mapenemylevel.tableName,
				"_",
				maparea_id.ToString(),
				mapinfo_no.ToString(),
				".xml"
			});
			IEnumerable<XElement> enumerable = Utils.Xml_Result_To_Path(path, Mst_mapenemylevel.tableName, string.Empty);
			if (enumerable == null)
			{
				return null;
			}
			List<Mst_mapenemylevel> list = new List<Mst_mapenemylevel>();
			using (IEnumerator<XElement> enumerator = enumerable.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					XElement current = enumerator.get_Current();
					Mst_mapenemylevel mst_mapenemylevel = null;
					Model_Base.SetMaster<Mst_mapenemylevel>(out mst_mapenemylevel, current);
					list.Add(mst_mapenemylevel);
				}
			}
			return Enumerable.ToLookup<Mst_mapenemylevel, int>(list, (Mst_mapenemylevel x) => x.Enemy_list_id);
		}

		public void Make_Mapshipget(int maparea_id, int mapinfo_no)
		{
			string text = Utils.getTableDirMaster(Mst_shipget2.tableName) + "mst_shipget/";
			string path = string.Concat(new string[]
			{
				text,
				Mst_shipget2.tableName,
				"_",
				maparea_id.ToString(),
				mapinfo_no.ToString(),
				".xml"
			});
			IEnumerable<XElement> enumerable = Utils.Xml_Result_To_Path(path, Mst_shipget2.tableName, null);
			if (enumerable == null)
			{
				return;
			}
			if (this.Mst_shipget != null)
			{
				Extensions.Remove<XElement>(this.Mst_shipget);
			}
			this.Mst_shipget = enumerable;
		}

		public Dictionary<int, List<int>> GetMaproute(int mapinfoId)
		{
			Dictionary<int, List<int>> dictionary = new Dictionary<int, List<int>>();
			string text = "mst_maproute";
			string text2 = Utils.getTableDirMaster(text) + text + "/";
			string path = string.Concat(new string[]
			{
				text2,
				text,
				"_",
				mapinfoId.ToString(),
				".xml"
			});
			IEnumerable<XElement> enumerable = Utils.Xml_Result_To_Path(path, text, null);
			if (enumerable == null)
			{
				return dictionary;
			}
			char c = ',';
			using (IEnumerator<XElement> enumerator = enumerable.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					XElement current = enumerator.get_Current();
					int num = int.Parse(current.Element("No").get_Value());
					string[] array = current.Element("Next").get_Value().Split(new char[]
					{
						c
					});
					List<int> list = Enumerable.ToList<int>(Array.ConvertAll<string, int>(array, (string x) => int.Parse(x)));
					list.RemoveAll((int x) => x == 0);
					dictionary.Add(num, list);
				}
			}
			return dictionary;
		}

		public Dictionary<int, List<Mst_mapincentive>> GetMapIncentive(int mapinfoId)
		{
			Dictionary<int, List<Mst_mapincentive>> dictionary = new Dictionary<int, List<Mst_mapincentive>>();
			string tableName = Mst_mapincentive.tableName;
			string text = Utils.getTableDirMaster(tableName) + tableName + "/";
			string path = string.Concat(new string[]
			{
				text,
				tableName,
				"_",
				mapinfoId.ToString(),
				".xml"
			});
			IEnumerable<XElement> enumerable = Utils.Xml_Result_To_Path(path, tableName, "Id");
			if (enumerable == null)
			{
				return dictionary;
			}
			List<Mst_mapincentive> list = new List<Mst_mapincentive>();
			using (IEnumerator<XElement> enumerator = enumerable.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					XElement current = enumerator.get_Current();
					Mst_mapincentive mst_mapincentive = null;
					Model_Base.SetMaster<Mst_mapincentive>(out mst_mapincentive, current);
					list.Add(mst_mapincentive);
				}
			}
			ILookup<int, Mst_mapincentive> lookup = Enumerable.ToLookup<Mst_mapincentive, int>(list, (Mst_mapincentive x) => x.Map_cleared);
			using (IEnumerator<IGrouping<int, Mst_mapincentive>> enumerator2 = lookup.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					IGrouping<int, Mst_mapincentive> current2 = enumerator2.get_Current();
					dictionary.Add(current2.get_Key(), Enumerable.ToList<Mst_mapincentive>(Enumerable.OrderBy<Mst_mapincentive, int>(current2, (Mst_mapincentive x) => x.Incentive_no)));
				}
			}
			return dictionary;
		}

		public Dictionary<int, List<Mst_mapcellincentive>> GetMapCellIncentive(int mapinfoId)
		{
			Dictionary<int, List<Mst_mapcellincentive>> dictionary = new Dictionary<int, List<Mst_mapcellincentive>>();
			string tableName = Mst_mapcellincentive.tableName;
			string text = Utils.getTableDirMaster(tableName) + tableName + "/";
			string path = string.Concat(new string[]
			{
				text,
				tableName,
				"_",
				mapinfoId.ToString(),
				".xml"
			});
			IEnumerable<XElement> enumerable = Utils.Xml_Result_To_Path(path, tableName, "Id");
			if (enumerable == null)
			{
				return dictionary;
			}
			List<Mst_mapcellincentive> list = new List<Mst_mapcellincentive>();
			using (IEnumerator<XElement> enumerator = enumerable.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					XElement current = enumerator.get_Current();
					Mst_mapcellincentive mst_mapcellincentive = null;
					Model_Base.SetMaster<Mst_mapcellincentive>(out mst_mapcellincentive, current);
					list.Add(mst_mapcellincentive);
				}
			}
			ILookup<int, Mst_mapcellincentive> lookup = Enumerable.ToLookup<Mst_mapcellincentive, int>(list, (Mst_mapcellincentive x) => x.Mapcell_id);
			using (IEnumerator<IGrouping<int, Mst_mapcellincentive>> enumerator2 = lookup.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					IGrouping<int, Mst_mapcellincentive> current2 = enumerator2.get_Current();
					dictionary.Add(current2.get_Key(), Enumerable.ToList<Mst_mapcellincentive>(Enumerable.OrderBy<Mst_mapcellincentive, int>(current2, (Mst_mapcellincentive x) => x.Incentive_no)));
				}
			}
			return dictionary;
		}

		public Dictionary<int, int> Get_MstLevel(bool shipTable)
		{
			return (!shipTable) ? ArrayMaster.GetMstLevelUser() : ArrayMaster.GetMstLevel();
		}

		public Dictionary<int, List<Mst_item_shop>> GetMstCabinet()
		{
			IEnumerable<XElement> enumerable = Utils.Xml_Result(Mst_item_shop.tableName, Mst_item_shop.tableName, null);
			if (enumerable == null)
			{
				return null;
			}
			var enumerable2 = Enumerable.Distinct(Enumerable.Select(Extensions.Elements<XElement>(enumerable, "Cabinet_no"), (XElement key) => new
			{
				no = int.Parse(key.get_Value())
			}));
			Dictionary<int, List<Mst_item_shop>> dictionary = Enumerable.ToDictionary(enumerable2, key => key.no, val => new List<Mst_item_shop>());
			using (IEnumerator<XElement> enumerator = enumerable.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					XElement current = enumerator.get_Current();
					Mst_item_shop mst_item_shop = null;
					Model_Base.SetMaster<Mst_item_shop>(out mst_item_shop, current);
					dictionary.get_Item((int)mst_item_shop.Cabinet_no).Add(mst_item_shop);
				}
			}
			if (!Enumerable.Any<Mem_ship>(Comm_UserDatas.Instance.User_ship.get_Values(), (Mem_ship x) => x.Stype == 22))
			{
				Mst_item_shop mst_item_shop2 = Enumerable.FirstOrDefault<Mst_item_shop>(dictionary.get_Item(1), (Mst_item_shop x) => x.Item1_id == 23);
				dictionary.get_Item(1).Remove(mst_item_shop2);
			}
			return dictionary;
		}

		public Dictionary<int, List<Mst_slotitem_remodel>> Get_Mst_slotitem_remodel()
		{
			IEnumerable<XElement> enumerable = Utils.Xml_Result(Mst_slotitem_remodel.tableName, Mst_slotitem_remodel.tableName, "Id");
			if (enumerable == null)
			{
				return null;
			}
			List<Mst_slotitem_remodel> list = new List<Mst_slotitem_remodel>();
			using (IEnumerator<XElement> enumerator = enumerable.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					XElement current = enumerator.get_Current();
					Mst_slotitem_remodel mst_slotitem_remodel = null;
					Model_Base.SetMaster<Mst_slotitem_remodel>(out mst_slotitem_remodel, current);
					if (mst_slotitem_remodel.Enabled == 1)
					{
						list.Add(mst_slotitem_remodel);
					}
				}
			}
			return Enumerable.ToDictionary<IGrouping<int, Mst_slotitem_remodel>, int, List<Mst_slotitem_remodel>>(Enumerable.ToLookup<Mst_slotitem_remodel, int>(list, (Mst_slotitem_remodel x) => x.Position), (IGrouping<int, Mst_slotitem_remodel> g_id) => g_id.get_Key(), (IGrouping<int, Mst_slotitem_remodel> values) => Enumerable.ToList<Mst_slotitem_remodel>(values));
		}

		public Dictionary<int, List<Mst_slotitem_remodel_detail>> Get_Mst_slotitem_remodel_detail()
		{
			IEnumerable<XElement> enumerable = Utils.Xml_Result(Mst_slotitem_remodel_detail.tableName, Mst_slotitem_remodel_detail.tableName, string.Empty);
			if (enumerable == null)
			{
				return null;
			}
			List<Mst_slotitem_remodel_detail> list = new List<Mst_slotitem_remodel_detail>();
			using (IEnumerator<XElement> enumerator = enumerable.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					XElement current = enumerator.get_Current();
					Mst_slotitem_remodel_detail mst_slotitem_remodel_detail = null;
					Model_Base.SetMaster<Mst_slotitem_remodel_detail>(out mst_slotitem_remodel_detail, current);
					list.Add(mst_slotitem_remodel_detail);
				}
			}
			return Enumerable.ToDictionary<IGrouping<int, Mst_slotitem_remodel_detail>, int, List<Mst_slotitem_remodel_detail>>(Enumerable.ToLookup<Mst_slotitem_remodel_detail, int>(list, (Mst_slotitem_remodel_detail x) => x.Id), (IGrouping<int, Mst_slotitem_remodel_detail> g_id) => g_id.get_Key(), (IGrouping<int, Mst_slotitem_remodel_detail> values) => Enumerable.ToList<Mst_slotitem_remodel_detail>(values));
		}

		public Dictionary<int, string> GetUseitemText()
		{
			IEnumerable<XElement> enumerable = Utils.Xml_Result("mst_useitemtext", "mst_useitemtext", null);
			return Enumerable.ToDictionary<XElement, int, string>(enumerable, (XElement key) => int.Parse(key.Element("Id").get_Value()), (XElement value) => value.Element("Description").get_Value());
		}

		public Dictionary<int, string> GetFurnitureText()
		{
			IEnumerable<XElement> enumerable = Utils.Xml_Result("mst_furnituretext", "mst_furnituretext", null);
			return Enumerable.ToDictionary<XElement, int, string>(enumerable, (XElement key) => int.Parse(key.Element("Id").get_Value()), (XElement value) => value.Element("Description").get_Value());
		}

		public Dictionary<int, Mst_payitem> GetPayitem()
		{
			IEnumerable<XElement> enumerable = Utils.Xml_Result(Mst_payitem.tableName, Mst_payitem.tableName, null);
			IEnumerable<XElement> enumerable2 = Utils.Xml_Result("mst_payitemtext", "mst_payitemtext", null);
			Dictionary<int, string> dictionary = Enumerable.ToDictionary<XElement, int, string>(enumerable2, (XElement key) => int.Parse(key.Element("Id").get_Value()), (XElement value) => value.Element("Description").get_Value());
			Dictionary<int, Mst_payitem> dictionary2 = new Dictionary<int, Mst_payitem>(Enumerable.Count<XElement>(enumerable));
			using (IEnumerator<XElement> enumerator = enumerable.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					XElement current = enumerator.get_Current();
					Mst_payitem mst_payitem = null;
					Model_Base.SetMaster<Mst_payitem>(out mst_payitem, current);
					mst_payitem.setText(dictionary.get_Item(mst_payitem.Id));
					dictionary2.Add(mst_payitem.Id, mst_payitem);
				}
			}
			return dictionary2;
		}

		public Dictionary<int, KeyValuePair<int, string>> GetSlotItemEquipTypeName()
		{
			IEnumerable<XElement> enumerable = Utils.Xml_Result("mst_slotitem_equiptype", "mst_slotitem_equiptype", null);
			return Enumerable.ToDictionary<XElement, int, KeyValuePair<int, string>>(enumerable, (XElement key) => int.Parse(key.Element("Id").get_Value()), (XElement value) => new KeyValuePair<int, string>(int.Parse(value.Element("Show_flg").get_Value()), value.Element("Name").get_Value()));
		}

		public List<Mst_mission2> GetSupportResistedData(int maparea_id)
		{
			List<Mst_mission2> list = new List<Mst_mission2>();
			XElement element = new XElement(new XElement("mst_mission2", new object[]
			{
				new XElement("Id", "100000"),
				new XElement("Maparea_id", maparea_id.ToString()),
				new XElement("Name", "前線反抗支援"),
				new XElement("Details", "前線反抗支援"),
				new XElement("Mission_type", "2"),
				new XElement("Time", "2"),
				new XElement("Rp_sub", "0"),
				new XElement("Difficulty", "1"),
				new XElement("Use_mat", "0.5,0.8"),
				new XElement("Required_ids", string.Empty),
				new XElement("Win_exp", "0,0"),
				new XElement("Win_mat", "0,0,0,0"),
				new XElement("Win_item1", "0,0"),
				new XElement("Win_item2", "0,0"),
				new XElement("Win_spoint", "0,0"),
				new XElement("Level", "0"),
				new XElement("Flagship_level_check_type", "1"),
				new XElement("Flagship_level", "0"),
				new XElement("Stype_num", "0,0,0,0,0,0,0,0,0"),
				new XElement("Deck_num", "0"),
				new XElement("Drum_num", "0,0,0"),
				new XElement("Flagship_stype", "0,0"),
				new XElement("Tanker_num", "0,0")
			}));
			Mst_mission2 mst_mission = null;
			Model_Base.SetMaster<Mst_mission2>(out mst_mission, element);
			list.Add(mst_mission);
			element = new XElement(new XElement("mst_mission2", new object[]
			{
				new XElement("Id", "100001"),
				new XElement("Maparea_id", maparea_id.ToString()),
				new XElement("Name", "決戦反抗支援"),
				new XElement("Details", "決戦反抗支援"),
				new XElement("Mission_type", "3"),
				new XElement("Time", "2"),
				new XElement("Rp_sub", "0"),
				new XElement("Difficulty", "1"),
				new XElement("Use_mat", "0.5,0.8"),
				new XElement("Required_ids", string.Empty),
				new XElement("Win_exp", "0,0"),
				new XElement("Win_mat", "0,0,0,0"),
				new XElement("Win_item1", "0,0"),
				new XElement("Win_item2", "0,0"),
				new XElement("Win_spoint", "0,0"),
				new XElement("Level", "0"),
				new XElement("Flagship_level_check_type", "1"),
				new XElement("Flagship_level", "0"),
				new XElement("Stype_num", "0,0,0,0,0,0,0,0,0"),
				new XElement("Deck_num", "0"),
				new XElement("Drum_num", "0,0,0"),
				new XElement("Flagship_stype", "0,0"),
				new XElement("Tanker_num", "0,0")
			}));
			Mst_mission2 mst_mission2 = null;
			Model_Base.SetMaster<Mst_mission2>(out mst_mission2, element);
			list.Add(mst_mission2);
			return list;
		}

		public List<Mst_bgm_jukebox> GetJukeBoxList()
		{
			return Enumerable.ToList<Mst_bgm_jukebox>(Enumerable.OrderBy<Mst_bgm_jukebox, int>(this._mst_jukebox.get_Values(), (Mst_bgm_jukebox x) => x.Id));
		}

		public Mst_bgm_jukebox GetJukeBoxItem(int bgmId)
		{
			Mst_bgm_jukebox result = null;
			this._mst_jukebox.TryGetValue(bgmId, ref result);
			return result;
		}

		public Dictionary<int, string> GetMstBgm()
		{
			IEnumerable<XElement> enumerable = Utils.Xml_Result("mst_bgm", "mst_bgm", null);
			char c = ',';
			Dictionary<int, string> dictionary = new Dictionary<int, string>();
			using (IEnumerator<XElement> enumerator = enumerable.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					XElement current = enumerator.get_Current();
					string[] array = current.Element("bgm_record").get_Value().Split(new char[]
					{
						c
					});
					int num = int.Parse(array[0]);
					string text = array[1];
					dictionary.Add(num, text);
				}
			}
			return dictionary;
		}

		public bool MakeUIBattleMaster(int mapinfo_id)
		{
			if (this.UiBattleMaster != null && this.UiBattleMaster.IsAllive())
			{
				return true;
			}
			this.UiBattleMaster = new UIBattleRequireMaster(mapinfo_id);
			if (!this.UiBattleMaster.IsAllive())
			{
				this.UiBattleMaster.PurgeCollection();
				return false;
			}
			return true;
		}

		public void PurgeUIBattleMaster()
		{
			if (this.UiBattleMaster == null)
			{
				return;
			}
			this.UiBattleMaster.PurgeCollection();
		}
	}
}
