using Common.Enum;
using KCV.Battle.Utils;
using local.models.battle;
using LT.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Production
{
	[RequireComponent(typeof(UIPanel))]
	public class ProdBufferEffect : BaseAnimation
	{
		[Serializable]
		private struct BufferLabel : IDisposable
		{
			[SerializeField]
			private Transform _tra;

			[SerializeField]
			private UILabel _uiBufferLabel;

			[SerializeField]
			private UITexture _uiSeparator;

			public Transform transform
			{
				get
				{
					return this._tra;
				}
			}

			public UILabel bufferLabel
			{
				get
				{
					return this._uiBufferLabel;
				}
			}

			public UITexture separator
			{
				get
				{
					return this._uiSeparator;
				}
			}

			public BufferLabel(Transform trans, UILabel label, UITexture separator)
			{
				this._tra = trans;
				this._uiBufferLabel = label;
				this._uiSeparator = separator;
			}

			public bool Init(EffectModel model)
			{
				this.SetLabel(model.Command);
				return true;
			}

			public void Dispose()
			{
				Mem.Del<Transform>(ref this._tra);
				Mem.Del<UILabel>(ref this._uiBufferLabel);
				Mem.Del<UITexture>(ref this._uiSeparator);
			}

			public void Clear()
			{
				this._uiBufferLabel.text = string.Empty;
			}

			private void SetLabel(BattleCommand iCommand)
			{
				string text = string.Empty;
				switch (iCommand)
				{
				case BattleCommand.Sekkin:
					text = "接近";
					break;
				case BattleCommand.Hougeki:
					text = "砲撃";
					break;
				case BattleCommand.Raigeki:
					text = "魚雷戦用意";
					break;
				case BattleCommand.Ridatu:
					text = "離脱";
					break;
				case BattleCommand.Taisen:
					text = "対潜";
					break;
				case BattleCommand.Kaihi:
					text = "回避";
					break;
				case BattleCommand.Kouku:
					text = "航空";
					break;
				case BattleCommand.Totugeki:
					text = "突撃";
					break;
				case BattleCommand.Tousha:
					text = "統射";
					break;
				}
				this._uiBufferLabel.text = text;
			}
		}

		[Serializable]
		private class BufferEffect : IDisposable
		{
			[SerializeField]
			private Transform _tra;

			[SerializeField]
			private UILabel _uiEffectType;

			[SerializeField]
			private UITexture _uiSeparator;

			[SerializeField]
			private UILabel _uiValue;

			private BufferCorrection _strCorrection;

			public Transform transform
			{
				get
				{
					return this._tra;
				}
			}

			private int correctionFactor
			{
				set
				{
					string text = (Mathf.Sign((float)value) != -1f) ? "+" : string.Empty;
					this._uiValue.text = string.Format("{0}{1}%", text, value);
				}
			}

			public BufferEffect(UILabel effectType, UITexture separator, UILabel value, BufferCorrection correction)
			{
				this._uiEffectType = effectType;
				this._uiSeparator = separator;
				this._uiValue = value;
				this._strCorrection = correction;
			}

			public bool Init(BufferCorrection correction)
			{
				this._strCorrection = correction;
				this.correctionFactor = 0;
				this.SetEffectTypeLabel(this._strCorrection.type);
				return true;
			}

			public void Dispose()
			{
				Mem.Del<Transform>(ref this._tra);
				Mem.Del<UILabel>(ref this._uiEffectType);
				Mem.Del<UITexture>(ref this._uiSeparator);
				Mem.Del<UILabel>(ref this._uiValue);
				this._strCorrection.Dispose();
				Mem.Del<BufferCorrection>(ref this._strCorrection);
			}

			public void Clear()
			{
				this.correctionFactor = 0;
			}

			public LTDescr PlayCntUp(float time)
			{
				return this._uiValue.get_transform().LTValue(0f, (float)this._strCorrection.collectionFactor, time).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
				{
					this.correctionFactor = Convert.ToInt32(x);
				});
			}

			private void SetEffectTypeLabel(BufferCorrectionType iType)
			{
				string text = string.Empty;
				switch (iType)
				{
				case BufferCorrectionType.AttackHitFactor:
					text = "攻撃命中率補正";
					break;
				case BufferCorrectionType.HitAvoianceFactor:
					text = "被弾回避率補正";
					break;
				case BufferCorrectionType.TorpedoHitFactor:
					text = "雷撃命中率補正";
					break;
				}
				this._uiEffectType.text = text;
			}
		}

		[SerializeField]
		private ProdBufferEffect.BufferLabel _strBufferLabel = default(ProdBufferEffect.BufferLabel);

		[SerializeField]
		private List<ProdBufferEffect.BufferEffect> _listBufferEffects = new List<ProdBufferEffect.BufferEffect>();

		[SerializeField]
		private UILabel _uiWithdrawalAcceptance;

		private UIPanel _uiPanel;

		public UIPanel panel
		{
			get
			{
				return this.GetComponentThis(ref this._uiPanel);
			}
		}

		public static ProdBufferEffect Instantiate(ProdBufferEffect prefab, Transform parent)
		{
			ProdBufferEffect prodBufferEffect = Object.Instantiate<ProdBufferEffect>(prefab);
			prodBufferEffect.get_transform().set_parent(parent);
			prodBufferEffect.get_transform().localScaleOne();
			prodBufferEffect.get_transform().set_localPosition(new Vector3(-420f, -80f, 0f));
			prodBufferEffect.Init();
			return prodBufferEffect;
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Mem.DelIDisposableSafe<ProdBufferEffect.BufferLabel>(ref this._strBufferLabel);
			this._listBufferEffects.ForEach(delegate(ProdBufferEffect.BufferEffect x)
			{
				x.Dispose();
			});
			Mem.DelListSafe<ProdBufferEffect.BufferEffect>(ref this._listBufferEffects);
			Mem.Del<UILabel>(ref this._uiWithdrawalAcceptance);
			Mem.Del<UIPanel>(ref this._uiPanel);
		}

		public override bool Init()
		{
			base.Init();
			this.panel.alpha = 0f;
			this.panel.widgetsAreStatic = true;
			return true;
		}

		public void SetEffectData(EffectModel model)
		{
			this._strBufferLabel.Init(model);
			this.SetCorrectionData(model);
			this.SetPosition(model.Command);
			this._uiWithdrawalAcceptance.text = ((model.Command != BattleCommand.Ridatu) ? string.Empty : ((!model.Withdrawal) ? "失敗" : "成功"));
		}

		private void SetCorrectionData(EffectModel model)
		{
			this._listBufferEffects.get_Item(0).Init(new BufferCorrection(BufferCorrectionType.AttackHitFactor, model.MeichuBuff));
			this._listBufferEffects.get_Item(1).Init(new BufferCorrection(BufferCorrectionType.HitAvoianceFactor, model.KaihiBuff));
			this._listBufferEffects.get_Item(2).Init(new BufferCorrection(BufferCorrectionType.TorpedoHitFactor, model.RaiMeichuBuff));
		}

		private void SetPosition(BattleCommand iCommand)
		{
			switch (iCommand)
			{
			case BattleCommand.Sekkin:
				this._strBufferLabel.transform.set_localPosition(Vector3.get_zero());
				this._listBufferEffects.get_Item(0).transform.set_localPosition(Vector3.get_down() * 75f);
				this._listBufferEffects.get_Item(1).transform.set_localPosition(Vector3.get_left() * 600f);
				this._listBufferEffects.get_Item(2).transform.set_localPosition(Vector3.get_left() * 600f);
				this._uiWithdrawalAcceptance.get_transform().set_localPosition(Vector3.get_left() * 600f);
				break;
			case BattleCommand.Raigeki:
				this._strBufferLabel.transform.set_localPosition(Vector3.get_zero());
				this._listBufferEffects.get_Item(0).transform.set_localPosition(Vector3.get_left() * 600f);
				this._listBufferEffects.get_Item(1).transform.set_localPosition(Vector3.get_left() * 600f);
				this._listBufferEffects.get_Item(2).transform.set_localPosition(Vector3.get_down() * 75f);
				this._uiWithdrawalAcceptance.get_transform().set_localPosition(Vector3.get_left() * 600f);
				break;
			case BattleCommand.Ridatu:
			{
				this._strBufferLabel.transform.set_localPosition(Vector3.get_zero());
				Transform arg_1CF_0 = this._listBufferEffects.get_Item(0).transform;
				Vector3 localPosition = Vector3.get_left() * 600f;
				this._listBufferEffects.get_Item(1).transform.set_localPosition(localPosition);
				arg_1CF_0.set_localPosition(localPosition);
				this._listBufferEffects.get_Item(2).transform.set_localPosition(Vector3.get_left() * 600f);
				this._uiWithdrawalAcceptance.get_transform().set_localPosition(Vector3.get_down() * 85f);
				break;
			}
			case BattleCommand.Kaihi:
				this._strBufferLabel.transform.set_localPosition(Vector3.get_zero());
				this._listBufferEffects.get_Item(0).transform.set_localPosition(Vector3.get_left() * 600f);
				this._listBufferEffects.get_Item(1).transform.set_localPosition(Vector3.get_down() * 75f);
				this._listBufferEffects.get_Item(2).transform.set_localPosition(Vector3.get_left() * 600f);
				this._uiWithdrawalAcceptance.get_transform().set_localPosition(Vector3.get_left() * 600f);
				break;
			case BattleCommand.Totugeki:
				this._strBufferLabel.transform.set_localPosition(Vector3.get_up() * 135f);
				this._listBufferEffects.get_Item(0).transform.set_localPosition(Vector3.get_up() * 60f);
				this._listBufferEffects.get_Item(1).transform.set_localPosition(Vector3.get_down() * 75f);
				this._listBufferEffects.get_Item(2).transform.set_localPosition(Vector3.get_left() * 600f);
				this._uiWithdrawalAcceptance.get_transform().set_localPosition(Vector3.get_left() * 600f);
				break;
			case BattleCommand.Tousha:
				this._strBufferLabel.transform.set_localPosition(Vector3.get_zero());
				this._listBufferEffects.get_Item(0).transform.set_localPosition(Vector3.get_down() * 75f);
				this._listBufferEffects.get_Item(1).transform.set_localPosition(Vector3.get_left() * 600f);
				this._listBufferEffects.get_Item(2).transform.set_localPosition(Vector3.get_left() * 600f);
				this._uiWithdrawalAcceptance.get_transform().set_localPosition(Vector3.get_left() * 600f);
				break;
			}
		}

		public override void Play(Action callback)
		{
			this.panel.widgetsAreStatic = false;
			this.panel.alpha = 0.01f;
			this._actCallback = callback;
			this.PlayCountUp();
			base.animation.Play();
		}

		private void PlayCountUp()
		{
			this._listBufferEffects.ForEach(delegate(ProdBufferEffect.BufferEffect x)
			{
				x.PlayCntUp(0.75f);
			});
		}

		protected override void onAnimationFinished()
		{
			this.panel.alpha = 0f;
			this._strBufferLabel.Clear();
			this.panel.widgetsAreStatic = true;
			base.onAnimationFinished();
		}
	}
}
