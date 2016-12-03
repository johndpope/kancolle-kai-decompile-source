using KCV.Battle.Production;
using System;
using UniRx;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyHexBreak : MonoBehaviour
	{
		[SerializeField]
		private ExplodeChild[] _explodeChild;

		[SerializeField]
		private ParticleSystem _uiParticle;

		private void OnDestroy()
		{
			Mem.Del<ExplodeChild[]>(ref this._explodeChild);
		}

		private void init()
		{
			this._explodeChild = new ExplodeChild[2];
			Util.FindParentToChild<ExplodeChild>(ref this._explodeChild[0], base.get_transform(), "RHex1");
			Util.FindParentToChild<ExplodeChild>(ref this._explodeChild[1], base.get_transform(), "RHex2");
			Util.FindParentToChild<ParticleSystem>(ref this._uiParticle, base.get_transform(), "Ring");
		}

		private void LateUpdate()
		{
			for (int i = 0; i < 2; i++)
			{
				if (this._explodeChild != null)
				{
					this._explodeChild[i].LateRun();
				}
			}
		}

		public void Play(Action callback)
		{
			for (int i = 0; i < 2; i++)
			{
				this._explodeChild[i].PlayAnimation().Subscribe(delegate(int _)
				{
					Dlg.Call(ref callback);
				}).AddTo(base.get_gameObject());
			}
			this._uiParticle.Stop();
			this._uiParticle.Play();
		}

		public static StrategyHexBreak Instantiate(StrategyHexBreak prefab, Transform fromParent)
		{
			StrategyHexBreak strategyHexBreak = Object.Instantiate<StrategyHexBreak>(prefab);
			strategyHexBreak.get_transform().set_parent(fromParent);
			strategyHexBreak.get_transform().set_localScale(Vector3.get_one());
			strategyHexBreak.get_transform().set_localPosition(new Vector3(0f, 0f, -1f));
			strategyHexBreak.init();
			return strategyHexBreak;
		}
	}
}
