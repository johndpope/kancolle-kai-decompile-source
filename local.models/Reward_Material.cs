using Common.Enum;
using local.utils;
using System;

namespace local.models
{
	public class Reward_Material : IReward, IReward_Material
	{
		private enumMaterialCategory _type;

		private int _count;

		public string Name
		{
			get
			{
				return Utils.enumMaterialCategoryToString(this._type);
			}
		}

		public enumMaterialCategory Type
		{
			get
			{
				return this._type;
			}
		}

		public int Count
		{
			get
			{
				return this._count;
			}
		}

		public Reward_Material(enumMaterialCategory type, int count)
		{
			this._type = type;
			this._count = count;
		}

		public override string ToString()
		{
			return string.Format("{0} {1}個", this.Name, this.Count);
		}
	}
}
