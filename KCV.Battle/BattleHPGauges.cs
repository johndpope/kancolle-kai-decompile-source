using Common.Enum;
using KCV.Battle.Production;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle
{
	public class BattleHPGauges : IDisposable
	{
		private List<UICircleHPGauge> _listHpGauge;

		private UICircleHPGauge _hpGauge;

		private int _count;

		public BattleHPGauges()
		{
			this._listHpGauge = new List<UICircleHPGauge>();
			this._hpGauge = null;
		}

		public string DRF()
		{
			if (this._listHpGauge == null)
			{
				return "null";
			}
			return " " + this._listHpGauge.get_Count();
		}

		public bool Init()
		{
			using (List<UICircleHPGauge>.Enumerator enumerator = this._listHpGauge.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					UICircleHPGauge current = enumerator.get_Current();
					if (current != null)
					{
						current.get_transform().set_localScale(Vector3.get_zero());
					}
				}
			}
			return true;
		}

		public void Dispose()
		{
			if (this._listHpGauge != null)
			{
				using (List<UICircleHPGauge>.Enumerator enumerator = this._listHpGauge.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						UICircleHPGauge current = enumerator.get_Current();
						if (current != null)
						{
							current.get_gameObject().Discard();
						}
					}
				}
				this._listHpGauge.Clear();
			}
			this._listHpGauge = null;
		}

		public void AddInstantiates(GameObject obj, bool isFriend, bool isLight, bool isT, bool isNumber)
		{
			this._listHpGauge.Add(this._instantiate(obj, isFriend, isLight, isT, isNumber));
		}

		public void AddInstantiatesSafe(GameObject obj, bool isFriend, bool isLight, bool isT, bool isNumber, int index)
		{
			if (index + 1 <= this._listHpGauge.get_Count())
			{
				return;
			}
			this._listHpGauge.Add(this._instantiate(obj, isFriend, isLight, isT, isNumber));
		}

		public bool SetGauge(int index, bool isFriend, bool isLight, bool isT, bool isNumber)
		{
			if (this._listHpGauge.get_Item(index) == null)
			{
				return false;
			}
			this._listHpGauge.get_Item(index).SetTextureType(isLight);
			if (isNumber)
			{
				this._listHpGauge.get_Item(index).SetShipNumber(index + 1, isFriend, isT);
			}
			return true;
		}

		public void SetHp(int num, int maxHp, int nowHp, int toHp, int damage, BattleHitStatus status, bool isFriend)
		{
			if (this._listHpGauge.get_Item(num) == null)
			{
				return;
			}
			this._listHpGauge.get_Item(num).SetHPGauge(maxHp, nowHp, toHp, damage, status, isFriend);
		}

		public void SetDamagePosition(int num, Vector3 vec)
		{
			if (this._listHpGauge.get_Item(num) != null)
			{
				this._listHpGauge.get_Item(num).SetDamagePosition(vec);
			}
		}

		public Vector3 GetDamagePosition(int num)
		{
			if (this._listHpGauge.get_Item(num) != null)
			{
				return this._listHpGauge.get_Item(num).GetDamagePosition();
			}
			return Vector3.get_zero();
		}

		public void ShowAll(Vector3 scale, bool isScale)
		{
			using (List<UICircleHPGauge>.Enumerator enumerator = this._listHpGauge.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					UICircleHPGauge current = enumerator.get_Current();
					if (current != null)
					{
						current.SetTextureScale(scale, isScale);
					}
				}
			}
		}

		public void Show(int num, Vector3 pos, Vector3 scale, bool isScale)
		{
			if (this._listHpGauge.get_Item(num) != null)
			{
				this._listHpGauge.get_Item(num).get_transform().set_localPosition(pos);
				this._listHpGauge.get_Item(num).SetTextureScale(scale, isScale);
			}
		}

		public void PlayHpAll(Action callBack)
		{
			using (List<UICircleHPGauge>.Enumerator enumerator = this._listHpGauge.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					UICircleHPGauge current = enumerator.get_Current();
					if (current != null)
					{
						current.Plays(callBack);
					}
				}
			}
		}

		public void PlayHp(int num, Action callBack)
		{
			if (this._listHpGauge.get_Item(num) != null)
			{
				this._listHpGauge.get_Item(num).Plays(callBack);
			}
		}

		public void PlayMiss(int num)
		{
			if (this._listHpGauge.get_Item(num) != null)
			{
				this._listHpGauge.get_Item(num).PlayMiss();
			}
		}

		public void PlayCritical(int num)
		{
			if (this._listHpGauge.get_Item(num) != null)
			{
				this._listHpGauge.get_Item(num).PlayCriticall();
			}
		}

		private UICircleHPGauge _instantiate(GameObject obj, bool isFriend, bool isLight, bool isT, bool isNumber)
		{
			if (this._hpGauge == null)
			{
				this._hpGauge = Resources.Load<UICircleHPGauge>("Prefabs/Battle/UI/UICircleHPGaugeS");
			}
			UICircleHPGauge component = Util.Instantiate(this._hpGauge.get_gameObject(), obj, false, false).GetComponent<UICircleHPGauge>();
			component.set_name((!isFriend) ? ("HpGaugeE" + this._listHpGauge.get_Count()) : ("HpGaugeF" + this._listHpGauge.get_Count()));
			component.SetTextureType(isLight);
			if (isNumber)
			{
				component.SetShipNumber(this._listHpGauge.get_Count() + 1, isFriend, isT);
			}
			return component;
		}
	}
}
