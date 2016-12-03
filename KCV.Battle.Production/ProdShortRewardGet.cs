using KCV.Utils;
using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdShortRewardGet : BaseAnimation
	{
		[SerializeField]
		private UIPanel _uiPanel;

		[SerializeField]
		private UITexture _uiBackground;

		[SerializeField]
		private UISprite _uiItemIcon;

		[SerializeField]
		private UITexture _uiOverlay;

		private Queue<IReward> _queRewards;

		public static ProdShortRewardGet Instantiate(ProdShortRewardGet prefab, Transform parent, List<IReward> rewards)
		{
			ProdShortRewardGet prodShortRewardGet = Object.Instantiate<ProdShortRewardGet>(prefab);
			prodShortRewardGet.get_transform().set_parent(parent);
			prodShortRewardGet.get_transform().localScaleOne();
			prodShortRewardGet.get_transform().localPositionZero();
			return prodShortRewardGet.VirtualCtor(rewards);
		}

		private ProdShortRewardGet VirtualCtor(List<IReward> rewards)
		{
			this._uiPanel.depth = 120;
			this._queRewards = new Queue<IReward>(rewards);
			this.SetReward(this._queRewards.Dequeue());
			return this;
		}

		private bool SetReward(IReward reward)
		{
			if (reward is IReward_Material)
			{
				return this.SetMaterial((IReward_Material)reward);
			}
			if (reward is IReward_Materials)
			{
				return this.SetMaterials((IReward_Materials)reward);
			}
			if (reward is IReward_Ship)
			{
				return this.SetShip((IReward_Ship)reward);
			}
			if (reward is IReward_Slotitem)
			{
				return this.SetSlotItem((IReward_Slotitem)reward);
			}
			return reward is IReward_Useitem && this.SetUseItem((IReward_Useitem)reward);
		}

		private bool SetMaterial(IReward_Material iMaterial)
		{
			return true;
		}

		private bool SetMaterials(IReward_Materials iMaterials)
		{
			return true;
		}

		private bool SetShip(IReward_Ship iShip)
		{
			return true;
		}

		private bool SetSlotItem(IReward_Slotitem iSlotItem)
		{
			return true;
		}

		private bool SetUseItem(IReward_Useitem iUseItem)
		{
			this._uiItemIcon.spriteName = string.Format("item_{0}", iUseItem.Id);
			this._uiItemIcon.localSize = new Vector2(128f, 128f);
			return true;
		}

		protected override void OnDestroy()
		{
			Mem.Del<UIPanel>(ref this._uiPanel);
			Mem.Del<UITexture>(ref this._uiBackground);
			Mem.Del(ref this._uiItemIcon);
			Mem.Del<UITexture>(ref this._uiOverlay);
			Mem.DelQueueSafe<IReward>(ref this._queRewards);
			base.OnDestroy();
		}

		public override void Play(Action callback)
		{
			base.Init();
			this._actCallback = callback;
			this.Play();
		}

		private void Play()
		{
			SoundUtils.PlaySE(SEFIleInfos.RewardGet);
			base.animation.get_Item("ProdShortRewardGet").set_time(0f);
			base.animation.Play();
		}

		private void OnFinished()
		{
			if (this._queRewards.get_Count() == 0)
			{
				base.onAnimationFinished();
			}
			else
			{
				this.SetReward(this._queRewards.Dequeue());
				this.Play();
			}
		}
	}
}
