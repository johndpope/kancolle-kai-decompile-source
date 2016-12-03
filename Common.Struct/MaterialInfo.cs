using Common.Enum;
using System;
using System.Collections.Generic;

namespace Common.Struct
{
	public struct MaterialInfo
	{
		public int Fuel;

		public int Ammo;

		public int Steel;

		public int Baux;

		public int BuildKit;

		public int RepairKit;

		public int Devkit;

		public int Revkit;

		public MaterialInfo(Dictionary<enumMaterialCategory, int> dic)
		{
			if (dic != null)
			{
				dic.TryGetValue(enumMaterialCategory.Fuel, ref this.Fuel);
				dic.TryGetValue(enumMaterialCategory.Bull, ref this.Ammo);
				dic.TryGetValue(enumMaterialCategory.Steel, ref this.Steel);
				dic.TryGetValue(enumMaterialCategory.Bauxite, ref this.Baux);
				dic.TryGetValue(enumMaterialCategory.Build_Kit, ref this.BuildKit);
				dic.TryGetValue(enumMaterialCategory.Repair_Kit, ref this.RepairKit);
				dic.TryGetValue(enumMaterialCategory.Dev_Kit, ref this.Devkit);
				dic.TryGetValue(enumMaterialCategory.Revamp_Kit, ref this.Revkit);
			}
			else
			{
				this.Fuel = (this.Ammo = (this.Steel = (this.Baux = (this.BuildKit = (this.RepairKit = (this.Devkit = (this.Revkit = 0)))))));
			}
		}

		public bool IsAllZero()
		{
			return this.Fuel == 0 && this.Ammo == 0 && this.Steel == 0 && this.Baux == 0 && this.BuildKit == 0 && this.RepairKit == 0 && this.Devkit == 0 && this.Revkit == 0;
		}

		public bool HasPositive()
		{
			return this.Fuel > 0 || this.Ammo > 0 || this.Steel > 0 || this.Baux > 0 || this.BuildKit > 0 || this.RepairKit > 0 || this.Devkit > 0 || this.Revkit > 0;
		}

		public void Set4(int fuel, int ammo, int steel, int baux)
		{
			this.Fuel = fuel;
			this.Ammo = ammo;
			this.Steel = steel;
			this.Baux = baux;
		}
	}
}
