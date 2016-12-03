using Common.Enum;
using Server_Common.Formats;
using System;

namespace local.models
{
	public class AlbumSlotModel : SlotitemModel_Mst, IAlbumModel
	{
		private User_BookFmt<BookSlotData> _fmt;

		public int Id
		{
			get
			{
				return this._fmt.IndexNo;
			}
		}

		public string Detail
		{
			get
			{
				return this._fmt.Detail.Info;
			}
		}

		public AlbumSlotModel(User_BookFmt<BookSlotData> fmt) : base(fmt.Detail.MstData)
		{
			this._fmt = fmt;
		}

		public bool CanEquip(SType ship_type)
		{
			return this._fmt.Detail.EnableStype.IndexOf((int)ship_type) != -1;
		}

		public override string ToString()
		{
			return this.ToString(false);
		}

		public string ToString(bool detail)
		{
			string text = string.Format("図鑑ID:{0} {1}(MstId:{2})", this.Id, base.Name, this.MstId);
			if (detail)
			{
				text += string.Format(" 図鑑背景タイプ:{0}  装備タイプ:{1}  装備アイコンタイプ:{2}\n", base.Type2, base.Type3, base.Type4);
				text += string.Format("装甲:{0} 火力:{1} 雷装:{2} 爆撃:{3} 対空:{4} 対潜:{5} 命中(砲):{6}", new object[]
				{
					base.Soukou,
					base.Hougeki,
					base.Raigeki,
					base.Bakugeki,
					base.Taikuu,
					base.Taisen,
					base.HouMeityu
				});
				text += string.Format(" 回避:{0} 索敵:{1} 射程:{2}\n", base.Kaihi, base.Sakuteki, base.Syatei);
				text += string.Format("駆逐艦:装備{0}", (!this.CanEquip(SType.Destroyter)) ? "不可" : "可能");
				text += string.Format(" 軽巡洋艦:装備{0}", (!this.CanEquip(SType.LightCruiser)) ? "不可" : "可能");
				text += string.Format(" 重巡洋艦:装備{0}", (!this.CanEquip(SType.HeavyCruiser)) ? "不可" : "可能");
				text += string.Format(" 戦艦:装備{0}", (!this.CanEquip(SType.BattleShip)) ? "不可" : "可能");
				text += string.Format(" 軽空母:装備{0}", (!this.CanEquip(SType.LightAircraftCarrier)) ? "不可" : "可能");
				text += string.Format(" 正規空母:装備{0}", (!this.CanEquip(SType.AircraftCarrier)) ? "不可" : "可能");
				text += string.Format(" 水上機母艦:装備{0}", (!this.CanEquip(SType.SeaplaneTender)) ? "不可" : "可能");
				text += string.Format(" 航空戦艦:装備{0}\n", (!this.CanEquip(SType.AviationBattleShip)) ? "不可" : "可能");
				text += this.Detail;
			}
			return text;
		}
	}
}
