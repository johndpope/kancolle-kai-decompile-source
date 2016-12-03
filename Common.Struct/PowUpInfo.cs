using System;

namespace Common.Struct
{
	public struct PowUpInfo
	{
		public int Taikyu;

		public int Karyoku;

		public int Raisou;

		public int Taiku;

		public int Soukou;

		public int Taisen;

		public int Lucky;

		public int Kaihi;

		public bool IsAllZero()
		{
			return this.Taikyu == 0 && this.Karyoku == 0 && this.Raisou == 0 && this.Taiku == 0 && this.Soukou == 0 && this.Taisen == 0 && this.Lucky == 0 && this.Kaihi == 0;
		}

		public bool HasPositive()
		{
			return this.Taikyu > 0 || this.Karyoku > 0 || this.Raisou > 0 || this.Taiku > 0 || this.Soukou > 0 || this.Taisen > 0 || this.Lucky > 0 || this.Kaihi > 0;
		}

		public void RemoveNegative()
		{
			this.Taikyu = Math.Max(0, this.Taikyu);
			this.Karyoku = Math.Max(0, this.Karyoku);
			this.Raisou = Math.Max(0, this.Raisou);
			this.Taiku = Math.Max(0, this.Taiku);
			this.Soukou = Math.Max(0, this.Soukou);
			this.Taisen = Math.Max(0, this.Taisen);
			this.Lucky = Math.Max(0, this.Lucky);
			this.Kaihi = Math.Max(0, this.Kaihi);
		}
	}
}
