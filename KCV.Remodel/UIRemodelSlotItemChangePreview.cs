using KCV.Scene.Port;
using KCV.Utils;
using local.models;
using System;
using UnityEngine;

namespace KCV.Remodel
{
	[SelectionBase]
	public class UIRemodelSlotItemChangePreview : MonoBehaviour, UIRemodelView
	{
		private Vector3 showPos = new Vector3(6f, 277f);

		private Vector3 hidePos = new Vector3(550f, 277f);

		[SerializeField]
		private UIRemodelSlotItemChangePreviewChildPane srcItemPane;

		[SerializeField]
		private UIRemodelSlotItemChangePreviewChildPane dstItemPane;

		[SerializeField]
		private UIButton mButton_TouchBackArea;

		private UIButton mButtonFocus;

		private int[] voiceTypes = new int[]
		{
			9,
			10,
			26
		};

		private int voiceTypeIdx;

		private KeyControl mKeyController;

		private bool isShown;

		public ShipModel mTargetShipModel
		{
			get;
			private set;
		}

		public SlotitemModel dstSlotItemModel
		{
			get;
			private set;
		}

		public int mSlotIndex
		{
			get;
			private set;
		}

		public void Initialize(KeyControl keyController, ShipModel targetShipModel, SlotitemModel srcSlotItemModel, SlotitemModel dstSlotItemModel, int slotIndex)
		{
			this.mKeyController = keyController;
			this.mTargetShipModel = targetShipModel;
			this.dstSlotItemModel = dstSlotItemModel;
			this.mSlotIndex = slotIndex;
			Texture slotItemTexture = this.srcItemPane.GetSlotItemTexture();
			Texture slotItemTexture2 = this.dstItemPane.GetSlotItemTexture();
			if (slotItemTexture != null && slotItemTexture2 != null && slotItemTexture.Equals(slotItemTexture2))
			{
				this.srcItemPane.UnloadSlotItemTexture(true);
				this.dstItemPane.UnloadSlotItemTexture(false);
			}
			else
			{
				this.srcItemPane.UnloadSlotItemTexture(true);
				this.dstItemPane.UnloadSlotItemTexture(true);
			}
			this.srcItemPane.Init4Upper(srcSlotItemModel);
			this.dstItemPane.Init4Lower(dstSlotItemModel, srcSlotItemModel);
			if (srcSlotItemModel != null)
			{
				this.srcItemPane.BackGround.mainTexture = Resources.Load<Texture2D>("Textures/remodel/PlaneSkillTex/weapon_info_lv" + srcSlotItemModel.SkillLevel);
			}
			if (dstSlotItemModel != null)
			{
				this.dstItemPane.BackGround.mainTexture = Resources.Load<Texture2D>("Textures/remodel/PlaneSkillTex/weapon_info_lv" + dstSlotItemModel.SkillLevel);
			}
		}

		private void Awake()
		{
			base.get_transform().set_localPosition(this.hidePos);
		}

		public void Start()
		{
			base.get_transform().set_localPosition(this.hidePos);
			this.mButton_TouchBackArea.SetActive(false);
		}

		public void ReleaseSlotItemInfo()
		{
			this.mTargetShipModel = null;
			this.dstSlotItemModel = null;
		}

		private void ProcessChange()
		{
			if (this.isShown)
			{
				this.Hide();
				bool isExSlot = this.mTargetShipModel.SlotCount <= this.mSlotIndex && this.mTargetShipModel.HasExSlot();
				UserInterfaceRemodelManager.instance.ProcessChangeSlotItem(this.mSlotIndex, this.dstSlotItemModel, this.getNextVoiceType(), isExSlot);
			}
		}

		private void Back()
		{
			if (this.isShown)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
				UserInterfaceRemodelManager.instance.Back2SoubiHenkouItemSelect();
				this.Hide();
			}
		}

		public void OnTouchForward()
		{
			if (base.get_enabled())
			{
				this.ProcessChange();
			}
		}

		public void OnTouchBack()
		{
			if (this.isShown)
			{
				this.Back();
			}
		}

		private void SetKeyController(KeyControl keyController)
		{
			if (keyController != null)
			{
				keyController.ClearKeyAll();
				keyController.firstUpdate = true;
			}
			this.mKeyController = keyController;
		}

		private void Update()
		{
			if (this.mKeyController != null && base.get_enabled() && this.isShown)
			{
				if (this.mKeyController.IsMaruDown())
				{
					this.ProcessChange();
				}
				else if (this.mKeyController.IsBatuDown())
				{
					this.Back();
				}
			}
		}

		public void Show()
		{
			base.get_gameObject().SetActive(true);
			base.set_enabled(true);
			this.mButton_TouchBackArea.SetActive(true);
			RemodelUtils.MoveWithManual(base.get_gameObject(), this.showPos, 0.2f, delegate
			{
				this.isShown = true;
			});
		}

		public void Hide()
		{
			this.Hide(true);
		}

		public void Hide(bool animation)
		{
			base.set_enabled(false);
			this.isShown = false;
			this.mButton_TouchBackArea.SetActive(false);
			if (animation)
			{
				RemodelUtils.MoveWithManual(base.get_gameObject(), this.hidePos, 0.2f, delegate
				{
					base.get_gameObject().SetActive(false);
				});
			}
			else
			{
				base.get_transform().set_localPosition(this.hidePos);
				base.get_gameObject().SetActive(false);
			}
		}

		private int getNextVoiceType()
		{
			this.voiceTypeIdx++;
			if (this.voiceTypes.Length <= this.voiceTypeIdx)
			{
				this.voiceTypeIdx = 0;
			}
			return this.voiceTypes[this.voiceTypeIdx];
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mButton_TouchBackArea);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mButtonFocus);
			Texture slotItemTexture = this.srcItemPane.GetSlotItemTexture();
			Texture slotItemTexture2 = this.dstItemPane.GetSlotItemTexture();
			if (slotItemTexture != null && slotItemTexture2 != null && slotItemTexture.Equals(slotItemTexture2))
			{
				this.srcItemPane.UnloadSlotItemTexture(true);
				this.dstItemPane.UnloadSlotItemTexture(false);
			}
			else
			{
				this.srcItemPane.UnloadSlotItemTexture(true);
				this.dstItemPane.UnloadSlotItemTexture(true);
			}
			this.srcItemPane = null;
			this.dstItemPane = null;
			this.mTargetShipModel = null;
			this.dstSlotItemModel = null;
		}
	}
}
