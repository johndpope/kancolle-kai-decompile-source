using Common.Enum;
using KCV.Battle.Utils;
using KCV.BattleCut;
using local.models;
using local.models.battle;
using LT.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdWinRankJudge : MonoBehaviour
	{
		[Serializable]
		private class UIBackground : IDisposable
		{
			[SerializeField]
			private UIPanel _uiBackground;

			[SerializeField]
			private UITexture _uiWhite;

			public void Dispose()
			{
				Mem.Del<UIPanel>(ref this._uiBackground);
				Mem.Del<UITexture>(ref this._uiWhite);
			}

			public bool Init()
			{
				this._uiBackground.alpha = 0f;
				this._uiBackground.widgetsAreStatic = true;
				return true;
			}

			public void Show(float fTime, Action onFinished)
			{
				this._uiBackground.widgetsAreStatic = false;
				this._uiBackground.get_transform().LTValue(this._uiBackground.alpha, 1f, fTime).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
				{
					this._uiBackground.alpha = x;
				}).setOnComplete(delegate
				{
					this._uiBackground.widgetsAreStatic = true;
					Dlg.Call(ref onFinished);
				});
			}

			public void Hide(float fTime, Action onFinished)
			{
				this._uiBackground.widgetsAreStatic = false;
				this._uiBackground.get_transform().LTValue(this._uiBackground.alpha, 0f, fTime).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
				{
					this._uiBackground.alpha = x;
				}).setOnComplete(delegate
				{
					this._uiBackground.widgetsAreStatic = true;
					Dlg.Call(ref onFinished);
				});
			}
		}

		[SerializeField]
		private ProdWinRankJudge.UIBackground _uiBackground;

		[SerializeField]
		private UIPanel _uiForeground;

		[SerializeField]
		private List<BtlCut_UICircleHPGauge> _listHPGauges;

		[SerializeField]
		private UILabel _uiJudgeLabel;

		[SerializeField]
		private UITexture _uiCongratulation;

		[SerializeField]
		private Transform _traRankBase;

		[SerializeField]
		private UITexture _uiRankTex;

		private BattleWinRankKinds _iWinRank;

		private BattleResultModel _clsResult;

		private int friendMaxHP;

		private int friendLeftHP;

		private int EnemyMaxHP;

		private int EnemyLeftHP;

		private int _nFriendFleetStartHP;

		private int _nFriendFleetEndHP;

		private bool _isBattleCut;

		private BattleWinRankKinds winRank
		{
			get
			{
				return this._iWinRank;
			}
			set
			{
				this._iWinRank = value;
				this._uiRankTex.mainTexture = Resources.Load<Texture2D>("Textures/SortieMap/BtlCut_Judge/rate_" + this._iWinRank.ToString());
				this._uiRankTex.localSize = new Vector2(90f, 102f);
				this._traRankBase.GetComponent<UITexture>().mainTexture = Resources.Load<Texture2D>("Textures/SortieMap/BtlCut_Judge/rate_" + this._iWinRank.ToString() + "_bg");
				this._traRankBase.GetComponent<UITexture>().localSize = new Vector2(194f, 194f);
			}
		}

		private bool isPerfect
		{
			get
			{
				return this._nFriendFleetStartHP == this._nFriendFleetEndHP;
			}
		}

		public static ProdWinRankJudge Instantiate(ProdWinRankJudge prefab, Transform parent, BattleResultModel model, bool isBattleCut)
		{
			ProdWinRankJudge prodWinRankJudge = Object.Instantiate<ProdWinRankJudge>(prefab);
			prodWinRankJudge.get_transform().set_parent(parent);
			prodWinRankJudge.get_transform().set_localPosition(Vector3.get_right() * 2000f);
			prodWinRankJudge.get_transform().localScaleOne();
			prodWinRankJudge.Init(model, isBattleCut);
			return prodWinRankJudge;
		}

		private void OnDestroy()
		{
			Mem.DelIDisposableSafe<ProdWinRankJudge.UIBackground>(ref this._uiBackground);
			Mem.Del<UIPanel>(ref this._uiForeground);
			Mem.DelListSafe<BtlCut_UICircleHPGauge>(ref this._listHPGauges);
			Mem.Del<UILabel>(ref this._uiJudgeLabel);
			Mem.Del<UITexture>(ref this._uiCongratulation);
			Mem.Del<Transform>(ref this._traRankBase);
			Mem.Del<UITexture>(ref this._uiRankTex);
			Mem.Del<BattleWinRankKinds>(ref this._iWinRank);
			Mem.Del<BattleResultModel>(ref this._clsResult);
			Mem.Del<int>(ref this.friendMaxHP);
			Mem.Del<int>(ref this.friendLeftHP);
			Mem.Del<int>(ref this.EnemyMaxHP);
			Mem.Del<int>(ref this.EnemyLeftHP);
			Mem.Del<int>(ref this._nFriendFleetStartHP);
			Mem.Del<int>(ref this._nFriendFleetEndHP);
		}

		private bool Init(BattleResultModel model, bool isBattleCut)
		{
			this._clsResult = model;
			this._isBattleCut = isBattleCut;
			this.winRank = model.WinRank;
			this._nFriendFleetStartHP = -1;
			this._nFriendFleetEndHP = -1;
			this._uiCongratulation.alpha = 0f;
			this.InitHPGauge(this._listHPGauges.get_Item(0), new List<ShipModel_BattleResult>(this._clsResult.Ships_f));
			this.InitHPGauge(this._listHPGauges.get_Item(1), new List<ShipModel_BattleResult>(this._clsResult.Ships_e));
			this._listHPGauges.ForEach(delegate(BtlCut_UICircleHPGauge x)
			{
				x.panel.alpha = 0f;
				x.get_transform().localScaleOne();
			});
			this._uiJudgeLabel.text = ((this._clsResult.WinRank != BattleWinRankKinds.S) ? BattleDefines.RESULT_WINRUNK_JUDGE_TEXT.get_Item((int)this._iWinRank) : BattleDefines.RESULT_WINRUNK_JUDGE_TEXT.get_Item((int)(this._iWinRank + ((!this.isPerfect) ? 0 : 1))));
			this._uiBackground.Init();
			return true;
		}

		private void InitHPGauge(BtlCut_UICircleHPGauge gauge, List<ShipModel_BattleResult> ships)
		{
			int num = Enumerable.Sum(Enumerable.Select<ShipModel_BattleResult, int>(Enumerable.Where<ShipModel_BattleResult>(ships, (ShipModel_BattleResult x) => x != null), (ShipModel_BattleResult x) => x.HpStart));
			int num2 = Enumerable.Sum(Enumerable.Select<ShipModel_BattleResult, int>(Enumerable.Where<ShipModel_BattleResult>(ships, (ShipModel_BattleResult x) => x != null), (ShipModel_BattleResult x) => x.HpEnd));
			if (ships.get_Item(0).IsFriend())
			{
				this._nFriendFleetStartHP = num;
				this._nFriendFleetEndHP = num2;
			}
			gauge.SetHPGauge(num, num, num2, false);
		}

		[DebuggerHidden]
		public IEnumerator StartBattleJudge()
		{
			ProdWinRankJudge.<StartBattleJudge>c__IteratorE9 <StartBattleJudge>c__IteratorE = new ProdWinRankJudge.<StartBattleJudge>c__IteratorE9();
			<StartBattleJudge>c__IteratorE.<>f__this = this;
			return <StartBattleJudge>c__IteratorE;
		}

		private LTDescr Hide()
		{
			if (!this._isBattleCut)
			{
				this._uiBackground.Hide(0.5f, null);
			}
			return base.get_transform().LTValue(1f, 0f, 0.5f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				this._uiForeground.alpha = x;
				this._listHPGauges.get_Item(0).panel.alpha = x;
				this._listHPGauges.get_Item(1).panel.alpha = x;
			});
		}

		private LTDescr ShowCongratulation()
		{
			return this._uiCongratulation.get_transform().LTValue(0f, 1f, 0.3f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				this._uiCongratulation.alpha = x;
			});
		}

		private LTDescr ShowHPGauge(BtlCut_UICircleHPGauge gauge)
		{
			gauge.get_transform().LTValue(0f, 1f, 0.5f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				gauge.panel.alpha = x;
			});
			return gauge.get_transform().LTScale(Vector3.get_one() * 1.2f, 0.5f).setEase(LeanTweenType.linear);
		}

		private SEFIleInfos GetFanfare()
		{
			SEFIleInfos result;
			switch (this._iWinRank)
			{
			case BattleWinRankKinds.B:
				result = SEFIleInfos.FanfareB;
				break;
			case BattleWinRankKinds.A:
				result = SEFIleInfos.FanfareA;
				break;
			case BattleWinRankKinds.S:
				result = SEFIleInfos.FanfareS;
				break;
			default:
				result = SEFIleInfos.FanfareE;
				break;
			}
			return result;
		}
	}
}
