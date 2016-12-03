using System;
using UnityEngine;

namespace KCV.Arsenal
{
	public class UiArsenalDockMini : MonoBehaviour
	{
		public enum AnimationType
		{
			ConstStart,
			Const
		}

		private Animation _anim;

		[SerializeField]
		private ParticleSystem par;

		[SerializeField]
		private ParticleSystem fire;

		[SerializeField]
		private Animation mini4anim;

		[SerializeField]
		private ParticleSystem mini4psHit;

		private UiArsenalMiniManager miniManager1;

		private UiArsenalMiniManager miniManager2;

		private UiArsenalMiniManager miniManager3;

		private UiArsenalMiniManager miniManager4;

		private int _index;

		private bool isCreate;

		private bool _isParticle;

		private bool isCreateAnim;

		private bool isHightAnim;

		private bool isFirstHight;

		private Action _callBack;

		private void OnDestroy()
		{
			this._anim = null;
			this.par = null;
			this.fire = null;
			this.mini4anim = null;
			this.mini4psHit = null;
			this.miniManager1 = null;
			this.miniManager2 = null;
			this.miniManager3 = null;
			this.miniManager4 = null;
			this._callBack = null;
		}

		public void init(int num)
		{
			this._index = num;
			this._isParticle = false;
			this.isCreateAnim = false;
			this.isHightAnim = false;
			this.isFirstHight = false;
			this.isCreate = true;
			GameObject gameObject = base.get_transform().get_parent().get_parent().FindChild("ParPanel").get_gameObject();
			Util.FindParentToChild<ParticleSystem>(ref this.par, gameObject.get_transform(), "Par");
			Util.FindParentToChild<ParticleSystem>(ref this.fire, gameObject.get_transform(), "Fire");
			Util.FindParentToChild<UiArsenalMiniManager>(ref this.miniManager1, base.get_transform(), "Mini1");
			Util.FindParentToChild<UiArsenalMiniManager>(ref this.miniManager2, base.get_transform(), "Mini2");
			Util.FindParentToChild<UiArsenalMiniManager>(ref this.miniManager3, base.get_transform(), "Mini3");
			Util.FindParentToChild<UiArsenalMiniManager>(ref this.miniManager4, base.get_transform(), "Mini4");
			if (this._anim == null)
			{
				this._anim = base.GetComponent<Animation>();
			}
			this.par.Stop();
			this.fire.Stop();
			this.par.SetActive(false);
			this.fire.SetActive(false);
			this._anim.Stop();
			this.miniManager1.init(false);
			this.miniManager2.init(false);
			this.miniManager3.init(false);
			this.miniManager4.init(true);
			this.mini4psHit.Stop();
			for (int i = 0; i < 3; i++)
			{
				this.miniManager1.addSprite("mini_03_a_0" + (i + 1));
				this.miniManager2.addSprite("mini_01_a_0" + (i + 1));
				this.miniManager3.addSprite("mini_04_a_0" + (i + 1));
			}
			this.miniManager3.get_transform().set_localPosition(new Vector3(250f, 0f, 0f));
		}

		private void Update()
		{
			if (this.isCreateAnim)
			{
				if (this.miniManager1.GetIndex() == 1 && !this._isParticle)
				{
					this._isParticle = true;
					this.par.SetActive(true);
					this.par.set_time(0f);
					this.par.Stop();
					this.par.Play();
				}
				else if (this.miniManager1.GetIndex() == 0)
				{
					this._isParticle = false;
				}
			}
			if (this.miniManager1 != null)
			{
				this.miniManager1.Run();
			}
			if (this.miniManager2 != null)
			{
				this.miniManager2.Run();
			}
			if (this.miniManager3 != null)
			{
				this.miniManager3.Run();
			}
			if (this.miniManager4 != null)
			{
				this.miniManager4.Run();
			}
		}

		public void DisableParticles()
		{
			this.miniManager4.DisableParticles();
		}

		public void EnableParticles()
		{
			this.miniManager4.EnableParticles();
		}

		public void PlayIdleAnimation()
		{
			this.miniManager4.get_transform().set_localPosition(new Vector3(0f, 0f, 0f));
			this.miniManager4.get_gameObject().SetActive(true);
			if (Random.get_value() < 0.5f)
			{
				this.miniManager4.StartAnimation("DockMini4Idle", 2, Random.get_value() * 8f);
			}
			else
			{
				this.miniManager4.StartAnimation("DockMini4Sleep", 2, Random.get_value() * 8f);
			}
		}

		public void StopIdleAnimation()
		{
			this._anim.Stop();
			this.miniManager4.stopAnimate();
			this.DisableParticles();
		}

		public void PlayConstStartAnimation()
		{
			this._anim.Play("DockMini1Start");
			this.miniManager1.StartAnimation("DockMini1StartLoop", 2, 0f);
			this.miniManager2.StartAnimation("DockMiniRunLoop", 2, 0f);
			this.miniManager4.StartAnimation("DockMini4Enter", 1, 0f);
			this.miniManager4.DisableParticles();
		}

		public void StopConstAnimation()
		{
			this._anim.Stop();
			this._anim.Play("DockMiniEmpty");
			this.miniManager1.stopAnimate();
			this.miniManager2.stopAnimate();
			this.miniManager1.get_transform().set_localPosition(new Vector3(250f, 0f, 0f));
			this.miniManager2.get_transform().set_localPosition(new Vector3(250f, 0f, 0f));
			this.init(this._index);
		}

		public void PlayConstCompAnimation()
		{
			this.isCreateAnim = false;
			this.miniManager1.init(false);
			this.miniManager2.init(false);
			for (int i = 0; i < 2; i++)
			{
				this.miniManager1.addSprite("mini_03_c_0" + (i + 1));
				this.miniManager2.addSprite("mini_01_c_0" + (i + 1));
			}
			this._anim.Stop();
			this._anim.Play("DockMiniComp");
			this.miniManager1.StartAnimation("DockMini1CmpLoop", 2, 0f);
			this.miniManager2.StartAnimation("DockMini2CmpLoop", 2, 0f);
			this.miniManager4.StartAnimation("DockMini4Jump", 2, 0f);
			this.miniManager4.DisableParticles();
		}

		public void PlayHalfwayHightAnimation()
		{
			this.isCreateAnim = false;
			this.miniManager3.init(false);
			for (int i = 0; i < 3; i++)
			{
				this.miniManager3.addSprite("mini_04_a_0" + (i + 1));
			}
			this._anim.Stop();
			this._anim.Play("DockMiniEnd");
			this.miniManager1.StartAnimation("DockMini1StartLoop", 2, 0f);
			this.miniManager2.StartAnimation("DockMiniRunLoop", 2, 0f);
			this.miniManager3.StartAnimation("DockMiniRunLoop", 2, 0f);
			this.miniManager4.StartAnimation("DockMini4Bask", 1, 0f);
			this.miniManager4.DisableParticles();
		}

		public void PlayFirstHighAnimation()
		{
			this.isCreateAnim = false;
			this.isFirstHight = true;
			this._anim.Stop();
			this._anim.Play("DockMiniFirstHight");
			this.miniManager3.StartAnimation("DockMiniRunLoop", 2, 0f);
			this.miniManager4.DisableParticles();
			this.miniManager4.stopAnimate();
			this.miniManager4.get_transform().set_localPosition(new Vector3(400f, 0f, 0f));
		}

		public void CompStartMini1()
		{
			this.miniManager1.stopAnimate();
			this.miniManager1.init(false);
			for (int i = 0; i < 2; i++)
			{
				this.miniManager1.addSprite("mini_03_b_0" + (i + 1));
			}
			this.miniManager1.StartAnimation("DockMini1CreateLoop", 2, 0f);
			this.isCreateAnim = true;
			this.miniManager4.StartAnimation("DockMini4Work", 2, 0f);
			this.mini4psHit.Play();
		}

		public void CompStartMini2()
		{
			this._anim.Stop();
			this._anim.Play("DockMiniRightRun");
			this.miniManager2.stopAnimate();
			this.miniManager2.StartAnimation("DockMiniRunLoop", 2, 0f);
		}

		public void CompRightRun()
		{
			this._anim.Stop();
			this._anim.Play("DockMiniLeftRun");
		}

		public void CompLeftRun()
		{
			this._anim.Stop();
			this._anim.Play("DockMiniRightRun");
		}

		public void CompEndAnimate()
		{
			this._anim.Stop();
			this.miniManager3.stopAnimate();
			this.miniManager3.init(false);
			for (int i = 0; i < 2; i++)
			{
				this.miniManager3.addSprite("mini_04_b_0" + (i + 1));
			}
			this._anim.Play("DockMiniHight");
			this.fire.SetActive(true);
			this.fire.set_time(0f);
			this.fire.Play();
			this.miniManager3.StartAnimation("DockMiniRunLoop", 2, 0f);
		}

		public void CompHightAnimate()
		{
			this._anim.Stop();
			this.fire.Stop();
			this.fire.SetActive(false);
			TaskMainArsenalManager.dockMamager[this._index].endSpeedUpAnimate();
		}

		public void PlayEndHightAnimate()
		{
			this._anim.Stop();
			this.miniManager3.stopAnimate();
			this.miniManager3.init(false);
			for (int i = 0; i < 2; i++)
			{
				this.miniManager3.addSprite("mini_04_c_0" + (i + 1));
			}
			this.miniManager3.StartAnimation("DockMini3CmpLoop", 2, 0f);
		}

		public void CompCmpAnimate()
		{
			this._anim.Stop();
			this._anim.Play("DockMiniComp");
		}

		public void CompCmpHighAnimate()
		{
			this._anim.Stop();
			this._anim.Play("DockMiniCompHigh");
			this.miniManager3.StartAnimation("DockMiniRunLoop", 2, 0f);
		}
	}
}
