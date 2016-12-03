using KCV.Scene.Port;
using KCV.Utils;
using local.models;
using System;
using UnityEngine;

namespace KCV.Remodel
{
	[SelectionBase]
	public class UIRemodelKaizou : MonoBehaviour, UIRemodelView
	{
		[SerializeField]
		private UILabel mLabel_Name;

		[SerializeField]
		private UILabel mLabel_Level;

		[SerializeField]
		private UILabel mLabel_Ammo;

		[SerializeField]
		private UILabel mLabel_Steel;

		[SerializeField]
		private Transform mTransform_DevKit;

		[SerializeField]
		private Transform mTransform_BluePrint;

		[SerializeField]
		private UISprite mSprite_Background;

		[SerializeField]
		private UIButton mButton_GradeUp;

		[SerializeField]
		private UIButton mButton_TouchBackArea;

		private Vector3 showPos = new Vector3(240f, 0f);

		private Vector3 hidePos = new Vector3(1000f, 0f);

		private KeyControl mKeyController;

		private ShipModel mTargetShipModel;

		private int needBP;

		private bool isShown;

		public void Initialize(ShipModel shipModel, int needBluePrint)
		{
			this.mTargetShipModel = shipModel;
			this.mLabel_Level.text = this.mTargetShipModel.Level.ToString();
			this.mLabel_Ammo.text = this.mTargetShipModel.AfterAmmo.ToString();
			this.mLabel_Steel.text = this.mTargetShipModel.AfterSteel.ToString();
			this.mLabel_Name.text = this.mTargetShipModel.Name;
			this.needBP = needBluePrint;
			bool flag = 0 < this.mTargetShipModel.AfterDevkit;
			if (flag)
			{
				this.mSprite_Background.height = 315;
				this.mTransform_DevKit.SetActive(true);
			}
			else
			{
				this.mSprite_Background.height = 245;
				this.mTransform_DevKit.SetActive(false);
			}
			if (needBluePrint != 0)
			{
				this.mTransform_BluePrint.SetActive(true);
			}
			else
			{
				this.mTransform_BluePrint.SetActive(false);
			}
		}

		private void Awake()
		{
			this.mButton_TouchBackArea.SetActive(false);
			base.get_transform().set_localPosition(this.hidePos);
		}

		private void Update()
		{
			if (this.mKeyController != null && base.get_enabled() && this.isShown)
			{
				if (this.mKeyController.IsMaruDown())
				{
					this.Forward();
				}
				else if (this.mKeyController.IsBatuDown())
				{
					this.Back();
				}
			}
		}

		private void Forward()
		{
			if (!UserInterfaceRemodelManager.instance.mRemodelGradeUpManager.GradeupBtnEnabled)
			{
				if (UserInterfaceRemodelManager.instance.mRemodelGradeUpManager.DesignSpecificationsForGradeup > UserInterfaceRemodelManager.instance.mRemodelGradeUpManager.DesignSpecifications)
				{
					CommonPopupDialog.Instance.StartPopup("この改装には「改装設計図」が" + UserInterfaceRemodelManager.instance.mRemodelGradeUpManager.DesignSpecificationsForGradeup + "枚必要です");
				}
				else if (this.mTargetShipModel.AfterAmmo > UserInterfaceRemodelManager.instance.mRemodelGradeUpManager.Material.Ammo || this.mTargetShipModel.AfterSteel > UserInterfaceRemodelManager.instance.mRemodelGradeUpManager.Material.Steel)
				{
					CommonPopupDialog.Instance.StartPopup("資材が不足しています");
				}
				else
				{
					CommonPopupDialog.Instance.StartPopup("現在、改装が出来ません");
				}
				SoundUtils.PlaySE(SEFIleInfos.CommonCancel2);
				return;
			}
			this.Hide();
			UserInterfaceRemodelManager.instance.Forward2KaizoAnimation(this.mTargetShipModel);
		}

		private void Back()
		{
			this.RemoveFocus();
			UserInterfaceRemodelManager.instance.Back2ModeSelect();
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
		}

		public void Show()
		{
			this.Show(true);
		}

		public void Show(bool animation)
		{
			base.get_gameObject().SetActive(true);
			base.set_enabled(true);
			if (animation)
			{
				RemodelUtils.MoveWithManual(base.get_gameObject(), this.showPos, 0.3f, delegate
				{
					this.isShown = true;
				});
			}
			else
			{
				this.isShown = true;
				base.get_transform().set_localPosition(this.showPos);
			}
			this.mButton_TouchBackArea.SetActive(true);
		}

		public void Hide()
		{
			this.Hide(true);
		}

		public void Hide(bool animation)
		{
			base.set_enabled(true);
			this.isShown = false;
			if (animation)
			{
				RemodelUtils.MoveWithManual(base.get_gameObject(), this.hidePos, 0.3f, delegate
				{
					base.get_gameObject().SetActive(false);
				});
			}
			else
			{
				base.get_transform().set_localPosition(this.hidePos);
				base.get_gameObject().SetActive(false);
			}
			this.mButton_TouchBackArea.SetActive(false);
		}

		public void OnTouchStart()
		{
			this.Forward();
		}

		public void OnTouchBackArea()
		{
			this.Back();
		}

		public void SetKeyController(KeyControl keyController)
		{
			if (keyController != null)
			{
				keyController.ClearKeyAll();
				keyController.firstUpdate = true;
			}
			this.mKeyController = keyController;
			if (this.mKeyController != null)
			{
				this.mButton_TouchBackArea.SetActive(true);
				this.mButton_GradeUp.SetState(UIButtonColor.State.Hover, true);
			}
			else
			{
				this.mButton_TouchBackArea.SetActive(false);
				this.mButton_GradeUp.SetState(UIButtonColor.State.Normal, true);
			}
		}

		public void RemoveFocus()
		{
			this.mButton_GradeUp.SetState(UIButtonColor.State.Normal, true);
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabel_Name);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabel_Level);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabel_Ammo);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabel_Steel);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mSprite_Background);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mButton_GradeUp);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mButton_TouchBackArea);
			this.mTransform_DevKit = null;
			this.mTransform_BluePrint = null;
			this.mKeyController = null;
			this.mTargetShipModel = null;
		}
	}
}
