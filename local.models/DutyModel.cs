using Common.Enum;
using Common.Struct;
using local.utils;
using Server_Common.Formats;
using System;
using System.Collections.Generic;

namespace local.models
{
	public class DutyModel
	{
		public enum InvalidType
		{
			MAX_SHIP,
			MAX_SLOT,
			LOCK_TARGET_SLOT
		}

		protected User_QuestFmt _fmt;

		public int No
		{
			get
			{
				return this._fmt.No;
			}
		}

		public int Category
		{
			get
			{
				return this._fmt.Category;
			}
		}

		public int Type
		{
			get
			{
				return this._fmt.Type;
			}
		}

		public QuestState State
		{
			get
			{
				return this._fmt.State;
			}
		}

		public string Title
		{
			get
			{
				return this._fmt.Title;
			}
		}

		public string Description
		{
			get
			{
				return this._fmt.Detail;
			}
		}

		public Dictionary<enumMaterialCategory, int> RewardMaterials
		{
			get
			{
				return this._fmt.GetMaterial;
			}
		}

		public int Fuel
		{
			get
			{
				return this._fmt.GetMaterial.get_Item(enumMaterialCategory.Fuel);
			}
		}

		public int Ammo
		{
			get
			{
				return this._fmt.GetMaterial.get_Item(enumMaterialCategory.Bull);
			}
		}

		public int Steel
		{
			get
			{
				return this._fmt.GetMaterial.get_Item(enumMaterialCategory.Steel);
			}
		}

		public int Baux
		{
			get
			{
				return this._fmt.GetMaterial.get_Item(enumMaterialCategory.Bauxite);
			}
		}

		public int BuildKit
		{
			get
			{
				return this._fmt.GetMaterial.get_Item(enumMaterialCategory.Build_Kit);
			}
		}

		public int RepairKit
		{
			get
			{
				return this._fmt.GetMaterial.get_Item(enumMaterialCategory.Repair_Kit);
			}
		}

		public int DevKit
		{
			get
			{
				return this._fmt.GetMaterial.get_Item(enumMaterialCategory.Dev_Kit);
			}
		}

		public int RevKit
		{
			get
			{
				return this._fmt.GetMaterial.get_Item(enumMaterialCategory.Revamp_Kit);
			}
		}

		public int SPoint
		{
			get
			{
				return this._fmt.GetSpoint;
			}
		}

		public QuestProgressKinds Progress
		{
			get
			{
				return this._fmt.Progress;
			}
		}

		public int ProgressPercent
		{
			get
			{
				if (this._fmt.Progress == QuestProgressKinds.MORE_THAN_80)
				{
					return 80;
				}
				if (this._fmt.Progress == QuestProgressKinds.MORE_THAN_50)
				{
					return 50;
				}
				return 0;
			}
		}

		[Obsolete("非推奨  GetInvalidTypes()を使用してください。", false)]
		public bool InvalidFlag
		{
			get
			{
				return this._fmt.InvalidFlag;
			}
		}

		public DutyModel(User_QuestFmt fmt)
		{
			this._fmt = fmt;
		}

		public List<DutyModel.InvalidType> GetInvalidTypes()
		{
			List<DutyModel.InvalidType> list = new List<DutyModel.InvalidType>();
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < this._fmt.RewardTypes.get_Count(); i++)
			{
				if (this._fmt.RewardTypes.get_Item(i) == QuestItemGetKind.Ship)
				{
					num += this._fmt.RewardCount.get_Item(i);
					num2 += 4;
				}
				else if (this._fmt.RewardTypes.get_Item(i) == QuestItemGetKind.SlotItem)
				{
					num2 += this._fmt.RewardCount.get_Item(i);
				}
			}
			if (num > 0)
			{
				MemberMaxInfo memberMaxInfo = Utils.ShipCountData();
				if (memberMaxInfo.NowCount + num > memberMaxInfo.MaxCount)
				{
					list.Add(DutyModel.InvalidType.MAX_SHIP);
				}
			}
			if (num2 > 0)
			{
				MemberMaxInfo memberMaxInfo2 = Utils.SlotitemCountData();
				if (memberMaxInfo2.NowCount + num2 > memberMaxInfo2.MaxCount)
				{
					list.Add(DutyModel.InvalidType.MAX_SLOT);
				}
			}
			if (this._fmt.InvalidFlag)
			{
				list.Add(DutyModel.InvalidType.LOCK_TARGET_SLOT);
			}
			return list;
		}

		public override string ToString()
		{
			string text = string.Format("[{0}] Category:{1} Type:{2} 状態:{3} {4}\n", new object[]
			{
				this.No,
				this.Category,
				this.Type,
				this.State,
				this.Title
			});
			if (this.SPoint > 0)
			{
				text += string.Format("獲得戦略ポイント:{0} ", this.SPoint);
			}
			text += string.Format("獲得資材:{0}/{1}/{2}/{3}", new object[]
			{
				this.Fuel,
				this.Ammo,
				this.Steel,
				this.Baux
			});
			text += string.Format(" 高速建造:{0} 高速修復:{1} 開発資材:{2} 改修資材:{3}", new object[]
			{
				this.BuildKit,
				this.RepairKit,
				this.DevKit,
				this.RevKit
			});
			text += string.Format(" 進行度:{0} ", this.ProgressPercent);
			List<DutyModel.InvalidType> invalidTypes = this.GetInvalidTypes();
			if (invalidTypes.get_Count() > 0)
			{
				text += "[";
				for (int i = 0; i < invalidTypes.get_Count(); i++)
				{
					text = text + invalidTypes.get_Item(i) + ",";
				}
				text = text.Remove(text.get_Length() - 1);
				text += "]";
			}
			text += "\n";
			return text + string.Format("{0}\n", this.Description);
		}
	}
}
