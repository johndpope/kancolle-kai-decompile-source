using System;
using UnityEngine;

namespace KCV.Battle
{
	public class BaseHPGauge : BaseAnimation
	{
		protected int _nFromHP;

		protected int _nToHP;

		protected int _nMaxHP;

		protected int _nDamage;

		protected override void OnDestroy()
		{
			Mem.Del<int>(ref this._nFromHP);
			Mem.Del<int>(ref this._nToHP);
			Mem.Del<int>(ref this._nMaxHP);
			Mem.Del<int>(ref this._nDamage);
			base.OnDestroy();
		}

		public virtual void SetHPGauge(int maxHP, int beforeHP, int afterHP, Vector3 pos, Vector3 size)
		{
			this._nMaxHP = maxHP;
			this._nFromHP = beforeHP;
			this._nToHP = afterHP;
			base.get_transform().set_localPosition(pos);
			base.get_transform().set_localScale(size);
		}

		public override void Play(Action callback)
		{
			base.Play(callback);
		}
	}
}
