using KCV.Battle.Production;
using KCV.Battle.Utils;
using local.models;
using LT.Tweening;
using System;
using UnityEngine;

namespace KCV.Battle
{
	public class UIBattleNavigation : UINavigation<UIBattleNavigation>
	{
		[Serializable]
		private struct Params
		{
			public float animationTime;
		}

		[Header("[Animation Properties]"), SerializeField]
		private UIBattleNavigation.Params _strParams;

		public static UIBattleNavigation Instantiate(UIBattleNavigation prefab, Transform parent, SettingModel model)
		{
			UIBattleNavigation uIBattleNavigation = Object.Instantiate<UIBattleNavigation>(prefab);
			uIBattleNavigation.get_transform().set_parent(parent);
			uIBattleNavigation.get_transform().localPositionZero();
			uIBattleNavigation.get_transform().localScaleOne();
			return uIBattleNavigation.VirtualCtor(model);
		}

		protected override UIBattleNavigation VirtualCtor(SettingModel model)
		{
			this.SetAnchor(UINavigation<UIBattleNavigation>.Anchor.BottomLeft);
			this.panel.alpha = 0f;
			this.panel.widgetsAreStatic = true;
			return base.VirtualCtor(model);
		}

		protected override void OnUnInit()
		{
		}

		public override UIBattleNavigation SetAnchor(UINavigation<UIBattleNavigation>.Anchor iAnchor)
		{
			if (!this.settingModel.GuideDisplay)
			{
				return this;
			}
			return base.SetAnchor(iAnchor);
		}

		public UIBattleNavigation SetNavigationInCommand(BattleCommandMode iCommandMode)
		{
			if (!this.settingModel.GuideDisplay)
			{
				return this;
			}
			this.SetAnchor(UINavigation<UIBattleNavigation>.Anchor.BottomLeft);
			if (iCommandMode != BattleCommandMode.SurfaceBox)
			{
				if (iCommandMode == BattleCommandMode.UnitList)
				{
					this.AddDetail(HowToKey.arrow_UDLR, "指揮コマンド選択");
					this.AddDetail(HowToKey.btn_maru, "決定");
					this.AddDetail(HowToKey.btn_batsu, "戻る");
				}
			}
			else
			{
				this.AddDetail(HowToKey.arrow_LR, "指揮ボックス選択");
				this.AddDetail(HowToKey.btn_maru, "決定");
				this.AddDetail(HowToKey.btn_batsu, "外す");
				this.AddDetail(HowToKey.btn_shikaku, "一括解除");
			}
			return this.DrawRefresh();
		}

		public UIBattleNavigation SetNavigationInWithdrawalDecision(ProdWithdrawalDecisionSelection.Mode iMode)
		{
			if (!this.settingModel.GuideDisplay)
			{
				return this;
			}
			this.SetAnchor(UINavigation<UIBattleNavigation>.Anchor.BottomLeft);
			if (iMode != ProdWithdrawalDecisionSelection.Mode.Selection)
			{
				if (iMode == ProdWithdrawalDecisionSelection.Mode.TacticalSituation)
				{
					this.AddDetail(HowToKey.btn_sankaku, "戻る");
				}
			}
			else
			{
				this.AddDetail(HowToKey.arrow_LR, "選択");
				this.AddDetail(HowToKey.btn_maru, "決定");
				this.AddDetail(HowToKey.btn_sankaku, "戦況確認");
			}
			return this.DrawRefresh();
		}

		public UIBattleNavigation SetNavigationInResult()
		{
			if (!this.settingModel.GuideDisplay)
			{
				return this;
			}
			this.SetAnchor(UINavigation<UIBattleNavigation>.Anchor.BottomLeft);
			this.AddDetail(HowToKey.btn_maru, "次へ");
			return this.DrawRefresh();
		}

		public UIBattleNavigation SetNavigationInAdvancingWithDrawal()
		{
			if (!this.settingModel.GuideDisplay)
			{
				return this;
			}
			this.SetAnchor(UINavigation<UIBattleNavigation>.Anchor.BottomLeft);
			this.AddDetail(HowToKey.arrow_LR, "選択");
			this.AddDetail(HowToKey.btn_maru, "決定");
			return this.DrawRefresh();
		}

		public UIBattleNavigation SetNavigationInEscortShipEvacuation()
		{
			if (!this.settingModel.GuideDisplay)
			{
				return this;
			}
			this.SetAnchor(UINavigation<UIBattleNavigation>.Anchor.BottomLeft);
			this.AddDetail(HowToKey.arrow_LR, "選択");
			this.AddDetail(HowToKey.btn_maru, "決定");
			return this.DrawRefresh();
		}

		public UIBattleNavigation SetNavigationInFlagshipWreck()
		{
			if (!this.settingModel.GuideDisplay)
			{
				return this;
			}
			this.SetAnchor(UINavigation<UIBattleNavigation>.Anchor.BottomLeft);
			this.AddDetail(HowToKey.btn_maru, "決定");
			return this.DrawRefresh();
		}

		public UIBattleNavigation SetNavigationInMapOpen()
		{
			if (!this.settingModel.GuideDisplay)
			{
				return this;
			}
			this.SetAnchor(UINavigation<UIBattleNavigation>.Anchor.BottomLeft);
			this.AddDetail(HowToKey.btn_maru, "次へ");
			return this.DrawRefresh();
		}

		public void Show()
		{
			this.Show(this._strParams.animationTime, null);
		}

		public void Show(Action onFinished)
		{
			this.Show(this._strParams.animationTime, onFinished);
		}

		public void Show(float fTime, Action onFinished)
		{
			if (!this.settingModel.GuideDisplay)
			{
				return;
			}
			this.panel.widgetsAreStatic = false;
			base.get_transform().LTCancel();
			base.get_transform().LTValue(this.panel.alpha, 1f, fTime).setEase(LeanTweenType.easeInSine).setOnUpdate(delegate(float x)
			{
				this.panel.alpha = x;
			}).setOnComplete(delegate
			{
				Dlg.Call(ref onFinished);
			});
		}

		public void Hide()
		{
			this.Hide(this._strParams.animationTime, null);
		}

		public void Hide(Action onFinished)
		{
			this.Hide(this._strParams.animationTime, onFinished);
		}

		public void Hide(float fTime, Action onFinished)
		{
			if (!this.settingModel.GuideDisplay)
			{
				return;
			}
			base.get_transform().LTCancel();
			base.get_transform().LTValue(this.panel.alpha, 0f, fTime).setEase(LeanTweenType.easeInSine).setOnUpdate(delegate(float x)
			{
				this.panel.alpha = x;
			}).setOnComplete(delegate
			{
				Dlg.Call(ref onFinished);
				this.panel.widgetsAreStatic = true;
			});
		}
	}
}
