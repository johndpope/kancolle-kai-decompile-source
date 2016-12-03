using DG.Tweening;
using KCV.Scene.Duty.Reward;
using KCV.Utils;
using KCV.View;
using local.models;
using System;
using UnityEngine;

namespace KCV.Scene.Duty
{
	public class UIGetRewardDialog : BoardDialog
	{
		[Button("PlayKiraAnimation", "キラキラ", new object[]
		{

		})]
		public int button;

		[SerializeField]
		private Transform mTransform_Contents;

		[SerializeField]
		private UIDutyOpenDeckRewardGet mPrefab_UIDutyOpenDeckRewardGet;

		[SerializeField]
		private UIDutyGetRewardUseItem mPrefab_UIDutyGetRewardUseItem;

		[SerializeField]
		private UIDutyGetRewardOpenCreateLargeTanker mPrefab_UIDutyGetRewardOpenCreateLargeTanker;

		[SerializeField]
		private UIDutyGetRewardSlotItem mPrefab_UIDutyGetRewardSlotItem;

		[SerializeField]
		private UIRewardMaterialsGrid mDutyRewardMaterialsGrid;

		[SerializeField]
		private UIRewardUseItemsGrid mDutyRewardUseItemsGrid;

		[SerializeField]
		private UIDutyRewardExchangeItem mPrefab_UIDutyRewardExchangeItem;

		[SerializeField]
		private UIDutyGetRewardFurniture mPrefab_UIDutyGetRewardFurniture;

		[SerializeField]
		private UIDutyOpenDeckPracticeRewardGet mPrefab_UIDutyOpenDeckPracticeRewardGet;

		[SerializeField]
		private UIDutyGetTransportCraftRewardGet mPrefab_UIDutyGetTransportCraftRewardGet;

		[SerializeField]
		private UIPanel KiraPanel;

		[SerializeField]
		private UIPanel mPanel_RewardArea;

		private KeyControl mKeyController;

		private Action mClosedCallBack;

		public void Initialize(IReward[] materials)
		{
			if (materials.Length == 1)
			{
				this.mDutyRewardMaterialsGrid.get_transform().set_localScale(new Vector3(1.25f, 1.25f));
			}
			else
			{
				this.mDutyRewardMaterialsGrid.get_transform().set_localScale(Vector3.get_one());
			}
			this.mDutyRewardMaterialsGrid.Initialize(materials);
			this.mDutyRewardMaterialsGrid.GoToPage(0);
		}

		public void Initialize(Reward_LargeBuild largeBuildObject)
		{
			Util.Instantiate(this.mPrefab_UIDutyGetRewardOpenCreateLargeTanker.get_gameObject(), this.mPanel_RewardArea.get_gameObject(), false, false);
		}

		public void Initialize(Reward_Deck rewardDeck)
		{
			Util.Instantiate(this.mPrefab_UIDutyOpenDeckRewardGet.get_gameObject(), this.mPanel_RewardArea.get_gameObject(), false, false).GetComponent<UIDutyOpenDeckRewardGet>().Initialize(rewardDeck);
		}

		public void Initialize(Reward_Furniture rewardFurniture)
		{
			Util.Instantiate(this.mPrefab_UIDutyGetRewardFurniture.get_gameObject(), this.mPanel_RewardArea.get_gameObject(), false, false).GetComponent<UIDutyGetRewardFurniture>().Initialize(rewardFurniture);
		}

		public void Initialize(Reward_Useitem reward)
		{
			Util.Instantiate(this.mPrefab_UIDutyGetRewardUseItem.get_gameObject(), this.mPanel_RewardArea.get_gameObject(), false, false).GetComponent<UIDutyGetRewardUseItem>().Initialize(reward);
		}

		public void Initialize(IReward_Slotitem reward_Slotitem)
		{
			Util.Instantiate(this.mPrefab_UIDutyGetRewardSlotItem.get_gameObject(), this.mPanel_RewardArea.get_gameObject(), false, false).GetComponent<UIDutyGetRewardSlotItem>().Initialize((Reward_Slotitem)reward_Slotitem);
		}

		public void Initialize(Reward_DeckPracitce reward)
		{
			Util.Instantiate(this.mPrefab_UIDutyOpenDeckPracticeRewardGet.get_gameObject(), this.mPanel_RewardArea.get_gameObject(), false, false).GetComponent<UIDutyOpenDeckPracticeRewardGet>().Initialize(reward);
		}

		public void Initialize(Reward_TransportCraft reward)
		{
			Util.Instantiate(this.mPrefab_UIDutyGetTransportCraftRewardGet.get_gameObject(), this.mPanel_RewardArea.get_gameObject(), false, false).GetComponent<UIDutyGetTransportCraftRewardGet>().Initialize(reward);
		}

		public void SetOnDialogClosedCallBack(Action callBack)
		{
			this.mClosedCallBack = callBack;
		}

		private void Update()
		{
			if (this.mKeyController != null)
			{
				if (this.mKeyController.keyState.get_Item(0).down)
				{
					this.Close();
				}
				else if (this.mKeyController.keyState.get_Item(1).down)
				{
					this.Close();
				}
			}
		}

		public KeyControl Show()
		{
			base.Show();
			this.mTransform_Contents.get_transform().set_localScale(new Vector3(0.5f, 0.5f));
			ShortcutExtensions.DOScale(this.mTransform_Contents.get_transform(), Vector3.get_one(), 0.3f);
			this.mKeyController = new KeyControl(0, 0, 0.4f, 0.1f);
			this.PlayKiraAnimation();
			return this.mKeyController;
		}

		public void Show(KeyControl keyController)
		{
			base.Show();
			this.mTransform_Contents.get_transform().set_localScale(new Vector3(0.5f, 0.5f));
			ShortcutExtensions.DOScale(this.mTransform_Contents.get_transform(), Vector3.get_one(), 0.3f);
			this.mKeyController = keyController;
			this.PlayKiraAnimation();
		}

		public void Close()
		{
			if (this.mKeyController != null && this.mKeyController.IsRun)
			{
				base.Hide(this.mClosedCallBack);
				this.mKeyController = null;
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			}
		}

		public void PlayKiraAnimation()
		{
			iTween.ValueTo(base.get_gameObject(), iTween.Hash(new object[]
			{
				"from",
				-970,
				"to",
				1000,
				"time",
				2f,
				"onupdate",
				"UpdateHandler",
				"looptype",
				iTween.LoopType.loop
			}));
		}

		private void UpdateHandler(float value)
		{
			this.KiraPanel.clipOffset = new Vector2(value, 0f);
		}

		private void OnDestroy()
		{
			this.mTransform_Contents = null;
			this.mPrefab_UIDutyOpenDeckRewardGet = null;
			this.mPrefab_UIDutyGetRewardUseItem = null;
			this.mPrefab_UIDutyGetRewardOpenCreateLargeTanker = null;
			this.mPrefab_UIDutyGetRewardSlotItem = null;
			this.mDutyRewardMaterialsGrid = null;
			this.mDutyRewardUseItemsGrid = null;
			this.mPrefab_UIDutyRewardExchangeItem = null;
			this.mPrefab_UIDutyGetRewardFurniture = null;
			this.mPrefab_UIDutyOpenDeckPracticeRewardGet = null;
			this.mPrefab_UIDutyGetTransportCraftRewardGet = null;
			this.KiraPanel = null;
			this.mPanel_RewardArea = null;
			this.mKeyController = null;
			this.mClosedCallBack = null;
		}
	}
}
