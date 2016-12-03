using Common.Enum;
using LT.Tweening;
using System;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class UICircleHPGauge : BaseHPGauge
	{
		private int _shipNumber;

		private bool _isSmoll;

		private bool _isLight;

		private BattleHitStatus _hitType;

		[SerializeField]
		private UITexture _uiBackground;

		[SerializeField]
		private UITexture _uiForeground;

		[SerializeField]
		private UILabel _uiHPLabel;

		[SerializeField]
		private GameObject _uiDamageObj;

		[SerializeField]
		private UISprite[] _uiDamage;

		[SerializeField]
		private UISprite _uiShipNumber;

		[SerializeField]
		private Animation _anime;

		[SerializeField]
		private Animation _animeDamage;

		protected override void Awake()
		{
			Util.FindParentToChild<UITexture>(ref this._uiBackground, base.get_transform(), "Background");
			Util.FindParentToChild<UITexture>(ref this._uiForeground, base.get_transform(), "Foreground");
			Util.FindParentToChild<UISprite>(ref this._uiShipNumber, base.get_transform(), "ShipNumber");
			Util.FindParentToChild<UILabel>(ref this._uiHPLabel, base.get_transform(), "Hp");
			if (this._uiDamageObj == null)
			{
				this._uiDamageObj = base.get_transform().FindChild("DamageObj").get_gameObject();
			}
			this._uiDamage = new UISprite[5];
			for (int i = 0; i < 5; i++)
			{
				Util.FindParentToChild<UISprite>(ref this._uiDamage[i], this._uiDamageObj.get_transform(), "Damage" + (i + 1));
				this._uiDamage[i].alpha = 0f;
			}
			if (this._anime == null)
			{
				this._anime = base.get_transform().GetComponent<Animation>();
			}
			if (this._animeDamage == null)
			{
				this._animeDamage = this._uiDamageObj.get_transform().GetComponent<Animation>();
			}
			this._uiForeground.type = UIBasicSprite.Type.Filled;
			this._uiForeground.fillAmount = 1f;
			this._uiShipNumber.SetActive(false);
			base.get_transform().set_localScale(Vector3.get_zero());
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			this._uiBackground = null;
			this._uiForeground = null;
			this._uiHPLabel = null;
			this._uiDamageObj = null;
			this._uiDamage = null;
			this._anime = null;
			this._animeDamage = null;
		}

		public void SetShipNumber(int num, bool isFriend, bool isTorpedo)
		{
		}

		public void SetTextureScale(Vector3 scale, bool isSmoll)
		{
			this._isSmoll = isSmoll;
			base.get_transform().set_localScale(scale);
			if (this._isSmoll)
			{
				UITexture component = base.get_transform().FindChild("Frame1").GetComponent<UITexture>();
				UITexture component2 = base.get_transform().FindChild("Frame2").GetComponent<UITexture>();
				string text = (!this._isLight) ? string.Empty : "_bl";
				component.mainTexture = (Resources.Load("Textures/battle/Gauge/D/gaugeD_frame_m" + text) as Texture2D);
				component2.mainTexture = (Resources.Load("Textures/battle/Gauge/C/gaugeC_frame_m" + text) as Texture2D);
				this._uiBackground.mainTexture = (Resources.Load("Textures/battle/Gauge/D/gaugeD_m" + text) as Texture2D);
				this._uiForeground.mainTexture = (Resources.Load("Textures/battle/Gauge/D/gaugeD_base_m" + text) as Texture2D);
				component.MakePixelPerfect();
				component2.MakePixelPerfect();
				this._uiBackground.MakePixelPerfect();
				this._uiForeground.MakePixelPerfect();
				float num = 1f / scale.x;
				float num2 = 1f / scale.y;
				component.get_transform().set_localScale(new Vector3(num, num2, 1f));
				component2.get_transform().set_localScale(new Vector3(num, num2, 1f));
				this._uiBackground.get_transform().set_localScale(new Vector3(num, num2, 1f));
				this._uiForeground.get_transform().set_localScale(new Vector3(num, num2, 1f));
			}
		}

		public void SetTextureType(bool isLight)
		{
			this._isLight = isLight;
			UITexture component = base.get_transform().FindChild("Frame1").GetComponent<UITexture>();
			UITexture component2 = base.get_transform().FindChild("Frame2").GetComponent<UITexture>();
			UITexture component3 = base.get_transform().FindChild("CriticallObj/CriticallFlash").GetComponent<UITexture>();
			UISprite component4 = component3.get_transform().FindChild("CriticallBack2").GetComponent<UISprite>();
			if (this._isLight)
			{
				component3.color = new Color(1f, 0.3f, 0f, component3.color.a);
				component4.color = new Color(1f, 0.3f, 0f, component4.color.a);
				if (this._isSmoll)
				{
					component.mainTexture = (Resources.Load("Textures/battle/Gauge/D/gaugeD_frame_m_bl") as Texture2D);
					component.alpha = 1f;
				}
				else
				{
					component.mainTexture = (Resources.Load("Textures/battle/Gauge/D/gaugeD_frame_bl") as Texture2D);
					component.alpha = 1f;
					component.material = null;
				}
			}
		}

		private void setDamageLabelPos()
		{
			int num = 0;
			int[] array = new int[]
			{
				0,
				10,
				100,
				1000,
				10000
			};
			float[] array2 = new float[]
			{
				-130f,
				-97.5f,
				-65f,
				-32.5f,
				0f
			};
			float[] array3 = new float[]
			{
				-160f,
				-120f,
				-80f,
				-40f,
				0f
			};
			for (int i = 0; i < 5; i++)
			{
				if (this._nDamage >= array[i])
				{
					num = i;
				}
			}
			int num2 = this._nDamage;
			int[] array4 = new int[5];
			array4[0] = num2 % 10;
			num2 /= 10;
			array4[1] = num2 % 10;
			num2 /= 10;
			array4[2] = num2 % 10;
			num2 /= 10;
			array4[3] = num2 % 10;
			num2 /= 10;
			array4[4] = num2 % 10;
			num2 /= 10;
			for (int j = 0; j < 5; j++)
			{
				if (this._nDamage >= array[j])
				{
					num = j;
				}
			}
			this._uiDamageObj.get_transform().set_localPosition((this._hitType != BattleHitStatus.Clitical) ? new Vector3(array2[num], 0f, 0f) : new Vector3(array3[num], 0f, 0f));
			for (int k = 0; k < 5; k++)
			{
				this._uiDamage[k].SetActive(false);
			}
		}

		private void setDamageSprite(int value)
		{
			int[] array = new int[5];
			int[] array2 = new int[]
			{
				0,
				10,
				100,
				1000,
				10000
			};
			int num = 0;
			array[0] = value % 10;
			int num2 = value / 10;
			array[1] = num2 % 10;
			num2 /= 10;
			array[2] = num2 % 10;
			num2 /= 10;
			array[3] = num2 % 10;
			num2 /= 10;
			array[4] = num2 % 10;
			num2 /= 10;
			for (int i = 0; i < 5; i++)
			{
				if (this._nDamage >= array2[i])
				{
					num = i + 1;
				}
			}
			string text = (this._hitType != BattleHitStatus.Clitical) ? "txt_d" : "txt_c";
			for (int j = 0; j < num; j++)
			{
				this._uiDamage[j].SetActive(true);
				this._uiDamage[j].spriteName = text + array[j];
				this._uiDamage[j].MakePixelPerfect();
			}
		}

		public void SetDamagePosition(Vector3 vec)
		{
			this._uiDamageObj.get_transform().set_localPosition(vec);
		}

		public Vector3 GetDamagePosition()
		{
			return this._uiDamageObj.get_transform().get_localPosition();
		}

		public void SetHPGauge(int maxHP, int beforeHP, int afterHP, int damage, BattleHitStatus status, bool isFriend)
		{
			this._uiHPLabel.textInt = beforeHP;
			this._nMaxHP = maxHP;
			this._nFromHP = beforeHP;
			this._nToHP = ((afterHP <= 0) ? 0 : afterHP);
			this._nDamage = ((damage < 100000) ? damage : 99999);
			this._hitType = ((this._nDamage <= 0) ? status : status);
			base.get_transform().set_localPosition((!isFriend) ? (Vector3.get_left() * 200f) : (Vector3.get_right() * 200f));
			if (this._hitType == BattleHitStatus.Miss)
			{
				this._nDamage = -1;
			}
			int now = (int)Math.Floor((double)((float)this._nFromHP));
			this._uiHPLabel.textInt = this._nFromHP;
			this._uiForeground.color = Util.HpGaugeColor2(this._nMaxHP, now);
			this._uiHPLabel.color = Util.HpLabelColor(this._nMaxHP, now);
			this._uiForeground.fillAmount = Mathe.Rate(0f, (float)this._nMaxHP, (float)this._nFromHP);
			this.setDamageLabelPos();
		}

		public override void Play(Action callback)
		{
			base.get_transform().set_localScale(Vector3.get_one());
			this._actCallback = callback;
			if (this._nFromHP <= 0)
			{
				return;
			}
			if (this._nDamage >= 0)
			{
				base.get_transform().LTValue(0f, (float)this._nDamage, 0.65f).setEase(LeanTweenType.easeOutExpo).setOnUpdate(delegate(float x)
				{
					int damageSprite = (int)Math.Round((double)x);
					this.setDamageSprite(damageSprite);
				}).setOnComplete(new Action(this.compGaugeDamage));
				this._animeDamage.Stop();
				if (this._hitType == BattleHitStatus.Clitical)
				{
					this._animeDamage.Play("ShowDamageCriticall");
				}
				else
				{
					this._animeDamage.Play("ShowDamageNormal");
				}
			}
			base.get_transform().LTValue((float)this._nFromHP, (float)this._nToHP, 0.7f).setEase(LeanTweenType.easeOutExpo).setOnUpdate(delegate(float x)
			{
				int num = (int)Math.Round((double)x);
				this._uiHPLabel.textInt = num;
				this._uiForeground.fillAmount = Mathe.Rate(0f, (float)this._nMaxHP, x);
				this._uiForeground.color = Util.HpGaugeColor2(this._nMaxHP, num);
				this._uiHPLabel.color = Util.HpLabelColor(this._nMaxHP, num);
			}).setOnComplete(new Action(this.onAnimationFinished));
			switch (this._hitType)
			{
			case BattleHitStatus.Miss:
				this.PlayMiss();
				break;
			case BattleHitStatus.Clitical:
				this.PlayCriticall();
				break;
			}
		}

		public void compGaugeDamage()
		{
			this.setDamageSprite(this._nDamage);
		}

		public void Plays(Action callback)
		{
			this._actCallback = callback;
			if (this._nFromHP <= 0)
			{
				return;
			}
			base.get_transform().LTValue((float)this._nFromHP, (float)this._nToHP, 0.634f).setEase(LeanTweenType.easeOutExpo).setOnUpdate(delegate(float x)
			{
				int num = (int)Math.Floor((double)x);
				this._uiHPLabel.textInt = num;
				this._uiForeground.fillAmount = Mathe.Rate(0f, (float)this._nMaxHP, x);
				this._uiForeground.color = Util.HpGaugeColor2(this._nMaxHP, num);
				this._uiHPLabel.color = Util.HpLabelColor(this._nMaxHP, num);
			}).setOnComplete(new Action(this.onAnimationFinished));
			if (this._nDamage >= 0)
			{
				base.get_transform().LTValue(0f, (float)this._nDamage, 0.634f).setEase(LeanTweenType.easeOutExpo).setOnUpdate(delegate(float x)
				{
					int damageSprite = (int)Math.Floor((double)x);
					this.setDamageSprite(damageSprite);
				}).setOnComplete(new Action(this.compGaugeDamage));
				this._animeDamage.Stop();
				if (this._hitType == BattleHitStatus.Clitical)
				{
					this._animeDamage.Play("ShowDamageCriticall");
				}
				else
				{
					this._animeDamage.Play("ShowDamageNormal");
				}
			}
			switch (this._hitType)
			{
			case BattleHitStatus.Miss:
				this.PlayMiss();
				break;
			case BattleHitStatus.Clitical:
				this.PlayCriticall();
				break;
			}
		}

		public void PlayMiss()
		{
			this._anime.Stop();
			this._anime.Play("ShowMissText");
		}

		public void PlayCriticall()
		{
			this._anime.Stop();
			this._anime.Play("ShowCriticallText");
		}
	}
}
