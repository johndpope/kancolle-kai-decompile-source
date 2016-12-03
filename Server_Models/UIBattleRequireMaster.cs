using Server_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Server_Models
{
	public class UIBattleRequireMaster
	{
		private int mapBgm;

		private Dictionary<int, List<int>> battleBgm;

		private Dictionary<int, int> mst_slotitem_comvert;

		public int MapBgm
		{
			get
			{
				return this.mapBgm;
			}
			private set
			{
				this.mapBgm = value;
			}
		}

		public Dictionary<int, List<int>> BattleBgm
		{
			get
			{
				return this.battleBgm;
			}
			private set
			{
				this.battleBgm = value;
			}
		}

		public Dictionary<int, int> Mst_slotitem_comvert
		{
			get
			{
				return this.mst_slotitem_comvert;
			}
			private set
			{
				this.mst_slotitem_comvert = value;
			}
		}

		public UIBattleRequireMaster()
		{
			this.MapBgm = 103;
		}

		public UIBattleRequireMaster(int mapinfo_id)
		{
			this.makeMapBgm(mapinfo_id);
			this.makeSlotConvert();
		}

		public bool IsAllive()
		{
			return this.BattleBgm != null && this.Mst_slotitem_comvert != null;
		}

		public void PurgeCollection()
		{
			if (this.BattleBgm != null)
			{
				this.BattleBgm.Clear();
				this.BattleBgm = null;
			}
			if (this.Mst_slotitem_comvert != null)
			{
				this.Mst_slotitem_comvert.Clear();
				this.Mst_slotitem_comvert = null;
			}
		}

		private void makeMapBgm(int mapInfoId)
		{
			if (this.BattleBgm != null)
			{
				return;
			}
			string text = "mst_mapbgm";
			string text2 = Utils.getTableDirMaster(text) + text + "/";
			string path = string.Concat(new string[]
			{
				text2,
				text,
				"_",
				mapInfoId.ToString(),
				".xml"
			});
			IEnumerable<XElement> enumerable = Utils.Xml_Result_To_Path(path, text, null);
			if (enumerable == null)
			{
				return;
			}
			Dictionary<int, List<int>> dictionary = new Dictionary<int, List<int>>();
			dictionary.Add(0, new List<int>());
			dictionary.Add(1, new List<int>());
			this.BattleBgm = dictionary;
			XElement xElement = Enumerable.First<XElement>(enumerable);
			this.BattleBgm.get_Item(0).Add(int.Parse(xElement.Element("Mapd_bgm_id").get_Value()));
			this.BattleBgm.get_Item(0).Add(int.Parse(xElement.Element("Bossd_bgm_id").get_Value()));
			this.BattleBgm.get_Item(1).Add(int.Parse(xElement.Element("Mapn_bgm_id").get_Value()));
			this.BattleBgm.get_Item(1).Add(int.Parse(xElement.Element("Bossn_bgm_id").get_Value()));
			int num = int.Parse(xElement.Element("Map_bgm").get_Value());
			if (num != 0)
			{
				this.MapBgm = num;
			}
		}

		private void makeSlotConvert()
		{
			if (this.Mst_slotitem_comvert != null)
			{
				return;
			}
			IEnumerable<XElement> enumerable = Utils.Xml_Result("mst_slotitem_convert", "mst_slotitem_convert", null);
			if (enumerable == null)
			{
				return;
			}
			this.Mst_slotitem_comvert = Enumerable.ToDictionary<XElement, int, int>(enumerable, (XElement key) => int.Parse(key.Element("Slotitem_id").get_Value()), (XElement value) => int.Parse(value.Element("Convert_id").get_Value()));
		}
	}
}
