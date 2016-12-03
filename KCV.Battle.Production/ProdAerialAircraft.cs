using KCV.Battle.Utils;
using local.managers;
using local.models.battle;
using LT.Tweening;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdAerialAircraft : MonoBehaviour
	{
		[SerializeField]
		private UITexture _uiAircraft;

		[SerializeField]
		public ParticleSystem _explosionParticle;

		[SerializeField]
		public ParticleSystem _smokeParticle;

		[SerializeField]
		private Animation _anime;

		private int _shipNum;

		private PlaneModelBase _plane;

		private Action _actCallback;

		public FleetType _fleetType;

		private int[] _baseNum;

		private Vector3[] _basePosition;

		public PlaneModelBase GetPlane()
		{
			return this._plane;
		}

		public PlaneState GetPlaneState()
		{
			return this._plane.State_Stage2End;
		}

		public void Init()
		{
			this._basePosition = new Vector3[]
			{
				new Vector3(101f, -481f, 0f),
				new Vector3(38f, -533f, 0f),
				new Vector3(105f, -419f, 0f),
				new Vector3(-39f, -581f, 0f),
				new Vector3(110f, -362f, 0f),
				new Vector3(-120f, -634f, 0f)
			};
			this._baseNum = new int[]
			{
				2,
				3,
				1,
				4,
				0,
				5
			};
			float num = 0.8f + 0.1f * (float)this._baseNum[this._shipNum];
			base.get_transform().set_localPosition(this._basePosition[this._shipNum]);
			base.get_transform().set_localScale(new Vector3(num, num, num));
			GameObject gameObject = base.get_transform().FindChild("AircraftObj").get_gameObject();
			Util.FindParentToChild<UITexture>(ref this._uiAircraft, gameObject.get_transform(), "Aircraft");
			Util.FindParentToChild<ParticleSystem>(ref this._explosionParticle, this._uiAircraft.get_transform(), "AircraftExplosion");
			Util.FindParentToChild<ParticleSystem>(ref this._smokeParticle, this._uiAircraft.get_transform(), "AircraftSmoke");
			this._uiAircraft.depth = 5 + this._baseNum[this._shipNum];
			if (this._anime == null)
			{
				this._anime = this._uiAircraft.GetComponent<Animation>().GetComponent<Animation>();
			}
			if (this._fleetType == FleetType.Friend)
			{
				this._uiAircraft.flip = UIBasicSprite.Flip.Nothing;
				this._uiAircraft.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.SlotItemTexture.Load(this._plane.MstId, 6);
				this._uiAircraft.get_transform().set_localScale(Vector3.get_one());
			}
			else if (this._fleetType == FleetType.Enemy)
			{
				this._uiAircraft.flip = UIBasicSprite.Flip.Nothing;
				if (BattleTaskManager.GetBattleManager() is PracticeBattleManager)
				{
					this._uiAircraft.flip = UIBasicSprite.Flip.Horizontally;
					this._uiAircraft.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.SlotItemTexture.Load(this._plane.MstId, 6);
					this._uiAircraft.get_transform().set_localScale(Vector3.get_one());
				}
				else
				{
					this._uiAircraft.mainTexture = SlotItemUtils.LoadTexture(this._plane);
					this._uiAircraft.MakePixelPerfect();
					this._uiAircraft.get_transform().set_localScale((this._plane.MstId > 500) ? new Vector3(0.7f, 0.7f, 0.7f) : new Vector3(0.8f, 0.8f, 0.8f));
				}
			}
			Vector3 to = new Vector3(this._basePosition[this._shipNum].x, this._basePosition[this._shipNum].y + 544f, this._basePosition[this._shipNum].z);
			base.get_transform().LTMoveLocal(to, 0.8f).setEase(LeanTweenType.easeOutBack).setDelay(0.4f + 0.1f * (float)this._baseNum[this._shipNum]);
		}

		private void OnDestroy()
		{
			Mem.Del<UITexture>(ref this._uiAircraft);
			Mem.Del(ref this._explosionParticle);
			Mem.Del(ref this._smokeParticle);
			Mem.Del<Animation>(ref this._anime);
			Mem.Del<UITexture>(ref this._uiAircraft);
			Mem.DelAry<int>(ref this._baseNum);
			Mem.DelAry<Vector3>(ref this._basePosition);
			Mem.Del<PlaneModelBase>(ref this._plane);
			Mem.Del<Action>(ref this._actCallback);
			Mem.Del<FleetType>(ref this._fleetType);
		}

		public void Injection(Action callback)
		{
			if (this._plane.State_Stage2End == PlaneState.Crush)
			{
				this._anime.Stop();
				this._anime.Play((this._fleetType != FleetType.Friend) ? "AircraftBurstEnemy" : "AircraftBurstFriend");
				this._explosionParticle.Play();
			}
		}

		public void Stop()
		{
			this._anime.Stop();
			this._explosionParticle.set_time(0f);
			this._smokeParticle.set_time(0f);
			this._explosionParticle.Stop();
			this._smokeParticle.Stop();
		}

		public void alphaOut()
		{
			this._uiAircraft.alpha = 0f;
		}

		public void SubPlay()
		{
			this._anime.Stop();
			this._anime.Play("AircraftNormal");
		}

		public void EndMove(float toX, float time)
		{
			base.get_transform().LTMoveLocal(new Vector3(base.get_transform().get_localPosition().x + toX, base.get_transform().get_localPosition().y, base.get_transform().get_localPosition().z), time).setDelay(0.4f + 0.1f * (float)this._baseNum[this._shipNum]).setEase(LeanTweenType.easeInCubic);
		}

		private void _onFinishedInjection()
		{
		}

		[DebuggerHidden]
		private IEnumerator _delayDiscard(float delay)
		{
			ProdAerialAircraft.<_delayDiscard>c__IteratorD2 <_delayDiscard>c__IteratorD = new ProdAerialAircraft.<_delayDiscard>c__IteratorD2();
			<_delayDiscard>c__IteratorD.delay = delay;
			<_delayDiscard>c__IteratorD.<$>delay = delay;
			<_delayDiscard>c__IteratorD.<>f__this = this;
			return <_delayDiscard>c__IteratorD;
		}

		public static ProdAerialAircraft Instantiate(ProdAerialAircraft _aerial, Transform fromParent, int number, int nDepth, PlaneModelBase plane, FleetType fleetType)
		{
			ProdAerialAircraft prodAerialAircraft = Object.Instantiate<ProdAerialAircraft>(_aerial);
			prodAerialAircraft.get_transform().set_parent(fromParent);
			prodAerialAircraft.get_transform().set_localScale(Vector3.get_one());
			prodAerialAircraft._shipNum = number;
			prodAerialAircraft._plane = plane;
			prodAerialAircraft._fleetType = fleetType;
			prodAerialAircraft.Init();
			return prodAerialAircraft;
		}
	}
}
