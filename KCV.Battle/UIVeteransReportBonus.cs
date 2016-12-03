using local.models;
using local.models.battle;
using LT.Tweening;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace KCV.Battle
{
	[RequireComponent(typeof(UIPanel))]
	public class UIVeteransReportBonus : MonoBehaviour
	{
		[Serializable]
		private class EXP : IDisposable
		{
			[SerializeField]
			private Transform _tra;

			[SerializeField]
			private UILabel _uiEXPLabel;

			[SerializeField]
			private UILabel _uiEXPVal;

			[SerializeField]
			private UISprite _uiPlus;

			private int _nEXP;

			public Transform transform
			{
				get
				{
					return this._tra;
				}
			}

			public EXP(Transform obj)
			{
				this._tra = obj;
				Util.FindParentToChild<UILabel>(ref this._uiEXPLabel, this.transform, "EXPLabel");
				Util.FindParentToChild<UILabel>(ref this._uiEXPVal, this.transform, "EXPVal");
				Util.FindParentToChild<UISprite>(ref this._uiPlus, this.transform, "Plus");
			}

			public void Dispose()
			{
				Mem.Del<Transform>(ref this._tra);
				Mem.Del<UILabel>(ref this._uiEXPLabel);
				Mem.Del<UILabel>(ref this._uiEXPVal);
				if (this._uiPlus != null)
				{
					this._uiPlus.Clear();
				}
				Mem.Del(ref this._uiPlus);
				Mem.Del<int>(ref this._nEXP);
			}

			public void SetEXP(ShipModel_BattleResult model)
			{
				this._uiEXPVal.textInt = this._nEXP;
				this._nEXP = model.ExpFromBattle;
			}

			public void PlayUpdateEXP()
			{
				this.transform.LTValue(0f, (float)this._nEXP, 0.5f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
				{
					this._uiEXPVal.textInt = Convert.ToInt32(x);
				});
			}
		}

		[Serializable]
		private class SpecialVeterans : IDisposable
		{
			[SerializeField]
			private Transform _tra;

			[SerializeField]
			private UILabel _uiEXPLabel;

			[SerializeField]
			private UILabel _uiEXPVal;

			[SerializeField]
			private UISprite _uiItem;

			[SerializeField]
			private UISprite _uiLabel;

			[SerializeField]
			private UITexture _uiLine;

			[SerializeField]
			private UISprite _uiPlus;

			private int _nRate;

			private List<IReward> _listRewards;

			public Transform transform
			{
				get
				{
					return this._tra;
				}
			}

			public UIWidget widget
			{
				get
				{
					return this.transform.GetComponent<UIWidget>();
				}
			}

			public bool isReward
			{
				get
				{
					return this._nRate != 0 && this._listRewards.get_Count() != 0;
				}
			}

			public SpecialVeterans(Transform obj)
			{
				this._tra = obj;
				this._nRate = 0;
				this._listRewards = new List<IReward>();
				Util.FindParentToChild<UILabel>(ref this._uiEXPLabel, this.transform, "EXPLabel");
				Util.FindParentToChild<UILabel>(ref this._uiEXPVal, this.transform, "EXPVal");
				Util.FindParentToChild<UISprite>(ref this._uiItem, this.transform, "Item");
				Util.FindParentToChild<UISprite>(ref this._uiLabel, this.transform, "Label");
				Util.FindParentToChild<UITexture>(ref this._uiLine, this.transform, "Line");
				Util.FindParentToChild<UISprite>(ref this._uiPlus, this.transform, "Plus");
			}

			public bool Init()
			{
				this.widget.alpha = 0f;
				this._uiItem.SetActive(false);
				return true;
			}

			public bool Init(int nRate, List<IReward> models)
			{
				this._nRate = nRate;
				this._listRewards = models;
				if (nRate == 0 && models.get_Count() == 0)
				{
					this.widget.alpha = 0f;
					return true;
				}
				this._uiEXPVal.textInt = 0;
				if (models != null && models.get_Count() != 0 && models is IReward_Useitem)
				{
					IReward_Useitem reward_Useitem = (IReward_Useitem)models.get_Item(0);
					this._uiItem.spriteName = string.Format("item_57", reward_Useitem.Id);
				}
				else
				{
					this._uiItem.SetActive(false);
				}
				return true;
			}

			public void Dispose()
			{
				Mem.Del<Transform>(ref this._tra);
				Mem.Del<UILabel>(ref this._uiEXPLabel);
				Mem.Del<UILabel>(ref this._uiEXPVal);
				if (this._uiItem != null)
				{
					this._uiItem.Clear();
				}
				Mem.Del(ref this._uiItem);
				if (this._uiLabel != null)
				{
					this._uiLabel.Clear();
				}
				Mem.Del(ref this._uiLabel);
				Mem.Del<UITexture>(ref this._uiLine);
				if (this._uiPlus != null)
				{
					this._uiPlus.Clear();
				}
				Mem.Del(ref this._uiPlus);
				Mem.Del<int>(ref this._nRate);
				Mem.DelListSafe<IReward>(ref this._listRewards);
			}

			public void PlayDrawSpecialVeterans()
			{
				if (!this.isReward)
				{
					return;
				}
				this.transform.LTValue(0f, 1f, 0.5f).setEase(LeanTweenType.easeInSine).setOnUpdate(delegate(float x)
				{
					this.widget.alpha = x;
				});
				this._uiEXPVal.get_transform().LTValue(0f, (float)this._nRate, 0.5f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
				{
					this._uiEXPVal.textInt = Convert.ToInt32(x);
				});
			}
		}

		[SerializeField]
		private UITexture _uiBackground;

		[SerializeField]
		private UISprite _uiBaseEXPLabel;

		[SerializeField]
		private UISprite _uiBonusLabel;

		[SerializeField]
		private UITexture _uiBonusLine;

		[SerializeField]
		private UILabel _uiEXPLabel;

		[SerializeField]
		private UILabel _uiEXPValue;

		[SerializeField]
		private List<UIVeteransReportBonus.EXP> _listEXPs;

		[SerializeField]
		private UIVeteransReportBonus.SpecialVeterans _clsSpecialVeterans;

		[SerializeField]
		private float _fShoBonusPosX = 240f;

		private UIPanel _uiPanel;

		private BattleResultModel _clsResultModel;

		public UIPanel panel
		{
			get
			{
				if (this._uiPanel == null)
				{
					this._uiPanel = base.GetComponent<UIPanel>();
				}
				return this._uiPanel;
			}
		}

		private int baseEXP
		{
			get
			{
				return this._uiEXPValue.textInt;
			}
			set
			{
				this._uiEXPValue.textInt = value;
			}
		}

		public static UIVeteransReportBonus Instantiate(UIVeteransReportBonus prefab, Transform parent, Vector3 pos, BattleResultModel model, bool isPractice)
		{
			UIVeteransReportBonus uIVeteransReportBonus = Object.Instantiate<UIVeteransReportBonus>(prefab);
			uIVeteransReportBonus.get_transform().set_parent(parent);
			uIVeteransReportBonus.get_transform().set_localPosition(pos);
			uIVeteransReportBonus.get_transform().localScaleOne();
			uIVeteransReportBonus._clsResultModel = model;
			uIVeteransReportBonus._uiBaseEXPLabel.spriteName = ((!isPractice) ? "exp_txt2" : "exp_txt3");
			uIVeteransReportBonus.Init();
			return uIVeteransReportBonus;
		}

		private void OnDestroy()
		{
			Mem.Del<UITexture>(ref this._uiBackground);
			Mem.Del(ref this._uiBaseEXPLabel);
			Mem.Del(ref this._uiBonusLabel);
			Mem.Del<UITexture>(ref this._uiBonusLine);
			Mem.Del<UILabel>(ref this._uiEXPLabel);
			Mem.Del<UILabel>(ref this._uiEXPValue);
			Mem.DelListSafe<UIVeteransReportBonus.EXP>(ref this._listEXPs);
			Mem.DelIDisposableSafe<UIVeteransReportBonus.SpecialVeterans>(ref this._clsSpecialVeterans);
			Mem.Del<float>(ref this._fShoBonusPosX);
			Mem.Del<UIPanel>(ref this._uiPanel);
			Mem.Del<BattleResultModel>(ref this._clsResultModel);
		}

		private bool Init()
		{
			this._uiEXPValue.textInt = this._clsResultModel.BaseExp;
			int num = 0;
			ShipModel_BattleResult[] ships_f = this._clsResultModel.Ships_f;
			for (int i = 0; i < ships_f.Length; i++)
			{
				ShipModel_BattleResult shipModel_BattleResult = ships_f[i];
				if (shipModel_BattleResult == null)
				{
					this._listEXPs.get_Item(num).transform.SetActive(false);
					num++;
				}
				else
				{
					this._listEXPs.get_Item(num).SetEXP(shipModel_BattleResult);
					num++;
				}
			}
			this._clsSpecialVeterans.Init();
			return true;
		}

		public void Show(Action callback)
		{
			Vector3 localPosition = base.get_transform().get_localPosition();
			localPosition.x = this._fShoBonusPosX;
			base.get_transform().LTMoveLocal(localPosition, 0.5f).setEase(LeanTweenType.easeOutCubic).setOnComplete(callback).setDelay(1f);
			base.get_transform().LTValue(0f, 1f, 0.5f).setEase(LeanTweenType.easeOutCubic).setOnUpdate(delegate(float x)
			{
				this.panel.alpha = x;
			}).setDelay(1f);
			this.PlayBonusEXP();
		}

		public void PlayBonusEXP()
		{
			base.get_transform().LTValue(0f, (float)this._clsResultModel.BaseExp, 0.5f).setEase(LeanTweenType.linear).setDelay(1f).setOnUpdate(delegate(float x)
			{
				this.baseEXP = Convert.ToInt32(x);
			});
			this._listEXPs.ForEach(delegate(UIVeteransReportBonus.EXP x)
			{
				x.PlayUpdateEXP();
			});
			Observable.Timer(TimeSpan.FromSeconds(0.5)).Subscribe(delegate(long _)
			{
				this._clsSpecialVeterans.PlayDrawSpecialVeterans();
			});
		}
	}
}
