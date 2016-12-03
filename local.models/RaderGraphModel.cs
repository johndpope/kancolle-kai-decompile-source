using System;

namespace local.models
{
	public class RaderGraphModel
	{
		private double[] _graph_values;

		private int[] _disp_values;

		private double[] _raw_values;

		public double[] GraphValues
		{
			get
			{
				return this._graph_values;
			}
		}

		public int[] DispValues
		{
			get
			{
				return this._disp_values;
			}
		}

		public double[] RawValues
		{
			get
			{
				return this._raw_values;
			}
		}

		public RaderGraphModel(ShipModel[] ships)
		{
			this._graph_values = new double[5];
			this._disp_values = new int[5];
			this._raw_values = new double[5];
			for (int i = 0; i < ships.Length; i++)
			{
				this._raw_values[0] += (double)ships[i].Karyoku;
				this._raw_values[1] += (double)ships[i].Raisou;
				this._raw_values[2] += (double)ships[i].Taiku;
				this._raw_values[3] += (double)ships[i].Kaihi;
				this._raw_values[4] += (double)ships[i].MaxHp;
			}
			if (ships.Length > 0)
			{
				double num = Math.Sqrt((double)(ships.Length - 1));
				for (int j = 0; j < 5; j++)
				{
					int num2 = (int)Math.Round(this._raw_values[j] / (double)ships.Length);
					int num3 = (int)Math.Round((double)num2 * num);
					this._disp_values[j] = num2 + num3;
					this._graph_values[j] = (double)this._disp_values[j] * 100.0 / 350.0;
				}
			}
		}

		public override string ToString()
		{
			string empty = string.Empty;
			return empty + string.Format("火力:{0}({1:f}) 雷装:{2}({3:f}) 対空:{4}({5:f}) 回避:{6}({7:f}) 耐久:{8}({9:f})", new object[]
			{
				this._disp_values[0],
				this._graph_values[0],
				this._disp_values[1],
				this._graph_values[1],
				this._disp_values[2],
				this._graph_values[2],
				this._disp_values[3],
				this._graph_values[3],
				this._disp_values[4],
				this._graph_values[4]
			});
		}
	}
}
