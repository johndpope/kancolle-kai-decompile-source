using Common.Enum;
using Server_Controllers;
using Server_Models;
using System;
using System.Collections.Generic;

namespace local.models
{
	public class MaterialModel
	{
		private Dictionary<enumMaterialCategory, Mem_material> _materialData;

		public int Fuel
		{
			get
			{
				return this._materialData.get_Item(enumMaterialCategory.Fuel).Value;
			}
		}

		public int Ammo
		{
			get
			{
				return this._materialData.get_Item(enumMaterialCategory.Bull).Value;
			}
		}

		public int Steel
		{
			get
			{
				return this._materialData.get_Item(enumMaterialCategory.Steel).Value;
			}
		}

		public int Baux
		{
			get
			{
				return this._materialData.get_Item(enumMaterialCategory.Bauxite).Value;
			}
		}

		public int BuildKit
		{
			get
			{
				return this._materialData.get_Item(enumMaterialCategory.Build_Kit).Value;
			}
		}

		public int RepairKit
		{
			get
			{
				return this._materialData.get_Item(enumMaterialCategory.Repair_Kit).Value;
			}
		}

		public int Devkit
		{
			get
			{
				return this._materialData.get_Item(enumMaterialCategory.Dev_Kit).Value;
			}
		}

		public int Revkit
		{
			get
			{
				return this._materialData.get_Item(enumMaterialCategory.Revamp_Kit).Value;
			}
		}

		public bool Update()
		{
			Api_Result<Dictionary<enumMaterialCategory, Mem_material>> api_Result = new Api_get_Member().Material();
			if (api_Result.state == Api_Result_State.Success)
			{
				this.Update(api_Result.data);
				return true;
			}
			return false;
		}

		public bool Update(Dictionary<enumMaterialCategory, Mem_material> materialData)
		{
			this._materialData = materialData;
			return true;
		}

		public int GetCount(enumMaterialCategory category)
		{
			return this._materialData.get_Item(category).Value;
		}

		public override string ToString()
		{
			string text = string.Empty;
			text += string.Format("[資材]燃/弾/鋼/ボ: {0}/{1}/{2}/{3}\t", new object[]
			{
				this.Fuel,
				this.Ammo,
				this.Steel,
				this.Baux
			});
			return text + string.Format("高速建造材/高速修復材/開発資材/改修資材: {0}/{1}/{2}/{3}", new object[]
			{
				this.BuildKit,
				this.RepairKit,
				this.Devkit,
				this.Revkit
			});
		}
	}
}
