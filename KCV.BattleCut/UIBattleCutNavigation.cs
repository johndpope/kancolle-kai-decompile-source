using local.models;
using LT.Tweening;
using System;

namespace KCV.BattleCut
{
	public class UIBattleCutNavigation : UINavigation<UIBattleCutNavigation>
	{
		public UIBattleCutNavigation Startup(SettingModel model)
		{
			this.SetAnchor(UINavigation<UIBattleCutNavigation>.Anchor.BottomLeft);
			this.panel.alpha = 0f;
			this.panel.widgetsAreStatic = true;
			return base.VirtualCtor(model);
		}

		public override UIBattleCutNavigation SetAnchor(UINavigation<UIBattleCutNavigation>.Anchor iAnchor)
		{
			if (!this.settingModel.GuideDisplay)
			{
				return this;
			}
			return base.SetAnchor(iAnchor);
		}

		public UIBattleCutNavigation SetNavigationInFormation()
		{
			if (!this.settingModel.GuideDisplay)
			{
				return this;
			}
			this.SetAnchor(UINavigation<UIBattleCutNavigation>.Anchor.BottomLeft);
			this.AddDetail(HowToKey.arrow_upDown, "選択");
			this.AddDetail(HowToKey.btn_maru, "決定");
			return this.DrawRefresh();
		}

		public UIBattleCutNavigation SetNavigationInCommand(CtrlBCCommandSelect.CtrlMode iMode)
		{
			if (!this.settingModel.GuideDisplay)
			{
				return this;
			}
			this.SetAnchor(UINavigation<UIBattleCutNavigation>.Anchor.BottomLeft);
			if (iMode != CtrlBCCommandSelect.CtrlMode.Surface)
			{
				if (iMode == CtrlBCCommandSelect.CtrlMode.Command)
				{
					this.AddDetail(HowToKey.arrow_upDown, "選択");
					this.AddDetail(HowToKey.btn_maru, "決定");
					this.AddDetail(HowToKey.btn_batsu, "戻る");
				}
			}
			else
			{
				this.AddDetail(HowToKey.arrow_upDown, "選択");
				this.AddDetail(HowToKey.btn_maru, "決定");
				this.AddDetail(HowToKey.btn_batsu, "外す");
				this.AddDetail(HowToKey.btn_shikaku, "一括解除");
			}
			return this.DrawRefresh();
		}

		public UIBattleCutNavigation SetNavigationInWithdrawalDecision()
		{
			if (!this.settingModel.GuideDisplay)
			{
				return this;
			}
			this.SetAnchor(UINavigation<UIBattleCutNavigation>.Anchor.BottomLeft);
			this.AddDetail(HowToKey.arrow_LR, "選択");
			this.AddDetail(HowToKey.btn_maru, "決定");
			this.AddDetail(HowToKey.btn_batsu, "戻る");
			return this.DrawRefresh();
		}

		public UIBattleCutNavigation SetNavigationInResult()
		{
			if (!this.settingModel.GuideDisplay)
			{
				return this;
			}
			this.SetAnchor(UINavigation<UIBattleCutNavigation>.Anchor.BottomLeft);
			this.AddDetail(HowToKey.btn_maru, "次へ");
			return this.DrawRefresh();
		}

		public UIBattleCutNavigation SetNavigationInAdvancingWithdrawal()
		{
			if (!this.settingModel.GuideDisplay)
			{
				return this;
			}
			this.SetAnchor(UINavigation<UIBattleCutNavigation>.Anchor.BottomLeft);
			this.AddDetail(HowToKey.arrow_LR, "選択");
			this.AddDetail(HowToKey.btn_maru, "決定");
			return this.DrawRefresh();
		}

		public UIBattleCutNavigation SetNavigationInFlagshipWreck()
		{
			if (!this.settingModel.GuideDisplay)
			{
				return this;
			}
			this.SetAnchor(UINavigation<UIBattleCutNavigation>.Anchor.BottomLeft);
			this.AddDetail(HowToKey.btn_maru, "決定");
			return this.DrawRefresh();
		}

		public UIBattleCutNavigation SetNavigationInEscortShipEvacuation()
		{
			if (!this.settingModel.GuideDisplay)
			{
				return this;
			}
			this.SetAnchor(UINavigation<UIBattleCutNavigation>.Anchor.BottomLeft);
			this.AddDetail(HowToKey.arrow_LR, "選択");
			this.AddDetail(HowToKey.btn_maru, "決定");
			return this.DrawRefresh();
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
