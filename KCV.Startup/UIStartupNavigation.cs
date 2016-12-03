using local.models;
using LT.Tweening;
using System;
using UnityEngine;

namespace KCV.Startup
{
	public class UIStartupNavigation : UINavigation<UIStartupNavigation>
	{
		[SerializeField]
		private struct Params
		{
			public float animationTime;
		}

		[Header("Animarion Parameter"), SerializeField]
		private UIStartupNavigation.Params _strParams;

		public void Startup(bool isInherit, SettingModel model)
		{
			this.VirtualCtor(isInherit, model);
		}

		protected UIStartupNavigation VirtualCtor(bool isInherit, SettingModel model)
		{
			base.VirtualCtor(model);
			this.SetNavigationInAdmiralInfo(isInherit);
			this.panel.alpha = ((!this.settingModel.GuideDisplay) ? 0f : 1f);
			this.panel.widgetsAreStatic = true;
			return this;
		}

		public override UIStartupNavigation SetAnchor(UINavigation<UIStartupNavigation>.Anchor iAnchor)
		{
			if (!this.settingModel.GuideDisplay)
			{
				return this;
			}
			return base.SetAnchor(iAnchor);
		}

		public UIStartupNavigation SetNavigationInAdmiralInfo(bool isInherit)
		{
			if (!this.settingModel.GuideDisplay)
			{
				return this;
			}
			this.SetAnchor(UINavigation<UIStartupNavigation>.Anchor.BottomLeft);
			if (isInherit)
			{
				this.AddDetail(HowToKey.btn_start, "決定");
			}
			else
			{
				this.AddDetail(HowToKey.btn_start, "決定");
				this.AddDetail(HowToKey.btn_batsu, "タイトルに戻る");
			}
			this.DrawRefresh();
			return this;
		}

		public UIStartupNavigation SetNavigationInStarterSelect()
		{
			if (!this.settingModel.GuideDisplay)
			{
				return this;
			}
			this.SetAnchor(UINavigation<UIStartupNavigation>.Anchor.BottomLeft);
			this.AddDetail(HowToKey.arrow_LR, "選択");
			this.AddDetail(HowToKey.btn_maru, "決定");
			this.AddDetail(HowToKey.btn_batsu, "戻る");
			this.DrawRefresh();
			return this;
		}

		public UIStartupNavigation SetNavigationInPartnerSelect()
		{
			if (!this.settingModel.GuideDisplay)
			{
				return this;
			}
			this.SetAnchor(UINavigation<UIStartupNavigation>.Anchor.BottomLeft);
			this.AddDetail(HowToKey.arrow_LR, "選択");
			this.AddDetail(HowToKey.btn_maru, "決定");
			this.AddDetail(HowToKey.btn_batsu, "戻る");
			this.DrawRefresh();
			return this;
		}

		public void Show(Action onFinished)
		{
			this.PreparaAnimation(true, onFinished);
		}

		public void Hide(Action onFinished)
		{
			this.PreparaAnimation(false, onFinished);
		}

		private void PreparaAnimation(bool isFoward, Action onFinished)
		{
			if (!this.settingModel.GuideDisplay)
			{
				return;
			}
			float to = (!isFoward) ? 0f : 1f;
			base.get_transform().LTValue(this.panel.alpha, to, 0.15f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				this.panel.alpha = x;
			}).setOnComplete(delegate
			{
				Dlg.Call(ref onFinished);
			});
		}
	}
}
