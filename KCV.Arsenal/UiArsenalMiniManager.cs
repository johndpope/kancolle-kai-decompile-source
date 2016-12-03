using System;
using UnityEngine;

namespace KCV.Arsenal
{
	public class UiArsenalMiniManager : MonoBehaviour
	{
		[SerializeField]
		private UISprite _uiMini;

		[SerializeField]
		private ParticleSystem _sleepPar;

		[SerializeField]
		private ParticleSystem _starPar;

		private Animation _anim;

		private int index;

		private float timer;

		private bool isInit;

		private bool isLoop;

		private bool idle;

		private string _animType;

		private string _animTypeNext;

		private string[] spriteNames;

		public bool isChange;

		public bool IsDefault;

		public int GetIndex()
		{
			return this.index;
		}

		public void init(bool isDefault)
		{
			this.IsDefault = isDefault;
			if (!isDefault)
			{
				this._uiMini = base.get_transform().FindChild("Mini").GetComponent<UISprite>();
			}
			if (this.IsDefault)
			{
				Util.FindParentToChild<ParticleSystem>(ref this._sleepPar, base.get_transform().get_parent().get_parent().get_parent(), "ParPanel/SleepPart");
				Util.FindParentToChild<ParticleSystem>(ref this._starPar, base.get_transform(), "Working/StarPart");
			}
			this._anim = base.GetComponent<Animation>();
			this._anim.Stop();
			this.index = -1;
			this.isChange = false;
			this.isLoop = false;
			this._animType = string.Empty;
			this._animTypeNext = string.Empty;
			this.spriteNames = new string[10];
			for (int i = 0; i < 10; i++)
			{
				this.spriteNames[i] = string.Empty;
			}
			this.idle = false;
			this.timer = 0f;
			this.isInit = true;
		}

		private void OnDestroy()
		{
			Mem.Del(ref this._uiMini);
			Mem.Del(ref this._sleepPar);
			Mem.Del(ref this._starPar);
			Mem.Del<Animation>(ref this._anim);
			Mem.Del<string[]>(ref this.spriteNames);
		}

		public bool addSprite(string name)
		{
			for (int i = 0; i < 10; i++)
			{
				if (this.spriteNames[i] == string.Empty)
				{
					this.spriteNames[i] = name;
					return true;
				}
			}
			return false;
		}

		public void Run()
		{
			if (!this.isInit)
			{
				return;
			}
			if (this.IsDefault && !this._starPar.get_isPlaying() && this._animType == "DockMini4Work")
			{
				this._starPar.Play();
			}
			if (this.IsDefault && !this._sleepPar.get_isPlaying() && this._animType == "DockMini4Sleep")
			{
				this._sleepPar.Play();
			}
			if (this.idle)
			{
				this._animTypeNext = this._animType;
				if (this._animType != "DockMini4Doze")
				{
					this.timer += Time.get_deltaTime();
					if (this.timer > 15f && Random.get_value() < Time.get_deltaTime())
					{
						this.timer = 0f;
						if (this._animType == "DockMini4Sleep")
						{
							this._animTypeNext = "DockMini4Idle";
						}
						else
						{
							this._animTypeNext = ((Random.get_value() >= 0.5f) ? "DockMini4Sleep" : "DockMini4Doze");
						}
					}
				}
				if (!this._anim.get_isPlaying())
				{
					this._animTypeNext = ((Random.get_value() >= 0.5f) ? "DockMini4Sleep" : "DockMini4Idle");
				}
				if (this._animTypeNext != this._animType || !this._anim.get_isPlaying())
				{
					if (this._animType == "DockMini4Sleep")
					{
						this._sleepPar.set_enableEmission(false);
					}
					if (this._animTypeNext == "DockMini4Sleep")
					{
						this._sleepPar.set_enableEmission(true);
					}
					this._animType = this._animTypeNext;
					this._anim.get_Item(this._animTypeNext).set_wrapMode((!(this._animTypeNext == "DockMini4Doze")) ? 2 : 1);
					this._anim.Play(this._animTypeNext);
				}
			}
		}

		private void _playStarParticle()
		{
			if (this.IsDefault && !this._starPar.get_isPlaying() && this._animType == "DockMini4Work")
			{
				this._starPar.Play();
			}
		}

		public void DisableParticles()
		{
			if (this.IsDefault)
			{
				this._sleepPar.SetActive(false);
				this._starPar.SetActive(false);
				this._sleepPar.get_transform().set_localPosition(new Vector3(600f, -10f));
			}
		}

		public void EnableParticles()
		{
			if (this.IsDefault)
			{
				this._sleepPar.SetActive(true);
				this._starPar.SetActive(true);
			}
		}

		public void StartAnimation(string anim, WrapMode wrap, float time)
		{
			this._anim.Stop();
			this._animType = anim;
			this._anim.Play(anim);
			this._anim.get_Item(anim).set_wrapMode(wrap);
			this._anim.get_Item(anim).set_time(time);
			if (this.IsDefault)
			{
				this._sleepPar.SetActive(false);
				this._starPar.SetActive(false);
				if (anim == "DockMini4Sleep")
				{
					this._sleepPar.get_transform().set_localPosition(new Vector3(100f, -10f));
					this._sleepPar.SetActive(true);
					this._sleepPar.Play();
				}
				if (anim == "DockMini4Work")
				{
					this._starPar.SetActive(true);
					this._starPar.Play();
				}
			}
			if (anim == "DockMini4Idle" || anim == "DockMini4Doze" || anim == "DockMini4Sleep")
			{
				this.idle = true;
				this.timer = 0f;
			}
			else
			{
				this.idle = false;
			}
		}

		public void ChangeSprite()
		{
			if (this.spriteNames == null)
			{
				return;
			}
			if (!string.IsNullOrEmpty(this.spriteNames[this.index + 1]))
			{
				this.index++;
				this._uiMini.spriteName = this.spriteNames[this.index];
			}
			else
			{
				this.index = 0;
				this._uiMini.spriteName = this.spriteNames[this.index];
			}
			this.isChange = true;
		}

		public void CompAnimate()
		{
			this._anim.Stop();
			if (this.isLoop)
			{
				this._anim.Play(this._animType);
			}
		}

		public void stopAnimate()
		{
			this._anim.Stop();
		}

		public void CompAnimateEnter4()
		{
			this._anim.Stop();
			if (this.isLoop)
			{
				this._anim.Play(this._animType);
			}
		}
	}
}
