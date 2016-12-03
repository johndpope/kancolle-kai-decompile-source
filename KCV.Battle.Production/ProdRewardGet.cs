using KCV.Utils;
using System;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdRewardGet : BaseAnimation
	{
		public enum RewardType
		{
			Ship,
			SlotItem,
			UseItem
		}

		private UIPanel _uiPanel;

		private UITexture _uiBackground;

		private UITexture _uiLabel;

		private UITexture _uiOverlay;

		protected override void Awake()
		{
			base.Awake();
			Util.FindParentToChild<UIPanel>(ref this._uiPanel, base.get_transform(), "Panel");
			Util.FindParentToChild<UITexture>(ref this._uiBackground, this._uiPanel.get_transform(), "Background");
			Util.FindParentToChild<UITexture>(ref this._uiLabel, this._uiPanel.get_transform(), "Label");
			Util.FindParentToChild<UITexture>(ref this._uiOverlay, this._uiPanel.get_transform(), "Overlay");
			this._uiOverlay.mainTexture = (Resources.Load("Textures/Common/Overlay") as Texture2D);
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Mem.Del<UIPanel>(ref this._uiPanel);
			Mem.Del<UITexture>(ref this._uiBackground);
			Mem.Del<UITexture>(ref this._uiLabel);
			Mem.Del<UITexture>(ref this._uiOverlay);
		}

		public static ProdRewardGet Instantiate(ProdRewardGet prefab, Transform parent, int nPanelDepth, ProdRewardGet.RewardType iType)
		{
			ProdRewardGet prodRewardGet = Object.Instantiate<ProdRewardGet>(prefab);
			prodRewardGet.get_transform().set_parent(parent);
			prodRewardGet.get_transform().set_localScale(Vector3.get_one());
			prodRewardGet.get_transform().set_localPosition(Vector3.get_zero());
			prodRewardGet._uiPanel.depth = nPanelDepth;
			return prodRewardGet;
		}

		private void _playRewardSE()
		{
			SoundUtils.PlaySE(SEFIleInfos.RewardGet);
		}
	}
}
