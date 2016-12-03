using System;

namespace KCV.BattleCut
{
	public class HPData
	{
		private int _nMaxHP;

		private int _nStartHP;

		private int _nNowHP;

		private int _nNextHP;

		private int _nEndHP;

		private int _nAttackCnt = 4;

		private int[] _nOneAttackDamage;

		private int _nHPDifFmNow2End;

		public int maxHP
		{
			get
			{
				return this._nMaxHP;
			}
			set
			{
				this._nMaxHP = value;
			}
		}

		public int startHP
		{
			get
			{
				return this._nStartHP;
			}
			set
			{
				this._nStartHP = value;
			}
		}

		public int nowHP
		{
			get
			{
				return this._nNowHP;
			}
			set
			{
				this._nNowHP = value;
			}
		}

		public int nextHP
		{
			get
			{
				return this._nNextHP;
			}
			set
			{
				this._nNextHP = value;
			}
		}

		public int endHP
		{
			get
			{
				return this._nEndHP;
			}
			set
			{
				this._nEndHP = value;
			}
		}

		public int attackCnt
		{
			get
			{
				return this._nAttackCnt;
			}
			set
			{
				this._nAttackCnt = value;
			}
		}

		public int[] oneAttackDamage
		{
			get
			{
				return this._nOneAttackDamage;
			}
			set
			{
				this._nOneAttackDamage = value;
			}
		}

		public HPData(int maxHp, int nowHp)
		{
			this.maxHP = maxHp;
			this.startHP = nowHp;
			this.nowHP = nowHp;
			this.nextHP = nowHp;
			this.oneAttackDamage = new int[this.attackCnt];
		}

		public void SetEndHP(int endHP)
		{
			this.endHP = endHP;
			this._nHPDifFmNow2End = this.nowHP - endHP;
			this.CalcOneAttackDamage();
		}

		public void ClearOneAttackDamage()
		{
			this._nOneAttackDamage = new int[this.attackCnt];
		}

		private void CalcOneAttackDamage()
		{
			for (int i = 0; i < this.oneAttackDamage.Length - 1; i++)
			{
				this.oneAttackDamage[i] = XorRandom.GetILim(0, (int)((float)this._nHPDifFmNow2End * 0.5f));
				this._nHPDifFmNow2End -= this.oneAttackDamage[i];
			}
			this.oneAttackDamage[this.oneAttackDamage.Length - 1] = this._nHPDifFmNow2End;
		}
	}
}
