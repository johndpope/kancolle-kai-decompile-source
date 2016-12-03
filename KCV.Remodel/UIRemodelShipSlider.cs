using DG.Tweening;
using KCV.Scene.Port;
using KCV.Utils;
using local.models;
using System;
using UnityEngine;

namespace KCV.Remodel
{
	[SelectionBase]
	public class UIRemodelShipSlider : MonoBehaviour, UIRemodelView
	{
		private const int MAX_DECK_IN_SHIPS = 6;

		[SerializeField]
		private UIWidget mWidget_SliderBackground;

		[SerializeField]
		private UIWidget mWidget_SliderLimit;

		[SerializeField]
		private UIRemodelShipSliderThumb mUIRemodelShipSliderThumb_Thumb;

		[SerializeField]
		private UILabel mLabel_Index;

		private DeckModel mDeckModel;

		private int mShipCount;

		private int mIndex;

		private KeyControl mKeyController;

		private bool initialized;

		private Vector3 showPos = new Vector3(-435f, 16f);

		private Vector3 hidePos = new Vector3(-530f, 16f);

		private int mCellHeight;

		private bool isShown;

		private void Start()
		{
			this.mUIRemodelShipSliderThumb_Thumb.SetOnUIRemodelShipSliderThumbActionListener(new UIRemodelShipSliderThumb.UIRemodelShipSliderThumbAction(this.ProcessSliderThumbAction));
		}

		public void Init(KeyControl keyController)
		{
			if (keyController != null)
			{
				keyController.ClearKeyAll();
				keyController.firstUpdate = true;
			}
			this.mKeyController = keyController;
			this.Hide(false);
			this.initialized = true;
			this.isShown = true;
			this.Show();
		}

		private void Update()
		{
			if (this.mKeyController != null && base.get_enabled() && this.isShown)
			{
				if (this.mKeyController.IsUpDown())
				{
					this.Prev();
				}
				else if (this.mKeyController.IsDownDown())
				{
					this.Next();
				}
				else if (this.mKeyController.IsMaruDown())
				{
					UserInterfaceRemodelManager.instance.Forward2ModeSelect();
				}
			}
		}

		private void ChangeShip()
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			this.mLabel_Index.text = (this.mIndex + 1).ToString();
			int index = this.mIndex;
			ShipModel ship = this.mDeckModel.GetShip(index);
			UserInterfaceRemodelManager.instance.ChangeFocusShip(ship);
		}

		private void ProcessSliderThumbAction(UIRemodelShipSliderThumb.ActionType actionType, UIRemodelShipSliderThumb calledObject)
		{
			if (!base.get_enabled())
			{
				return;
			}
			if (actionType != UIRemodelShipSliderThumb.ActionType.Move)
			{
				if (actionType == UIRemodelShipSliderThumb.ActionType.FingerUp)
				{
					calledObject.get_transform().localPositionY((float)(-(float)this.mIndex * this.mCellHeight));
				}
			}
			else
			{
				calledObject.get_transform().set_position(calledObject.mNextDragWorldPosition);
				if (0f < calledObject.get_transform().get_localPosition().y)
				{
					calledObject.get_transform().localPositionY(0f);
				}
				else if (calledObject.get_transform().get_localPosition().y < (float)(-(float)((this.mShipCount - 1) * this.mCellHeight)))
				{
					calledObject.get_transform().localPositionY((float)(-(float)((this.mShipCount - 1) * this.mCellHeight)));
				}
				this.OnUpdateIndex(this.ThumbLocalPositionToIndex(calledObject.get_transform().get_localPosition()));
			}
		}

		private int ThumbLocalPositionToIndex(Vector3 localPosition)
		{
			int num = (int)localPosition.y / (this.mCellHeight / 2);
			int result;
			if (num != 0 && num % 2 != 0)
			{
				result = Mathf.Abs((num - 1) / 2);
			}
			else
			{
				result = Mathf.Abs(num / 2);
			}
			return result;
		}

		private void OnUpdateIndex(int index)
		{
			if (this.mIndex < index)
			{
				this.mIndex = index;
				this.ChangeShip();
			}
			else if (index < this.mIndex)
			{
				this.mIndex = index;
				this.ChangeShip();
			}
		}

		public void Initialize(DeckModel deckModel)
		{
			this.mUIRemodelShipSliderThumb_Thumb.get_transform().set_localPosition(Vector3.get_zero());
			this.mDeckModel = deckModel;
			this.mShipCount = this.mDeckModel.GetShips().Length;
			this.mCellHeight = 46;
			DOVirtual.Float((float)this.mWidget_SliderLimit.height, (float)(this.mCellHeight * (this.mShipCount - 1)), 0.3f, delegate(float value)
			{
				this.mWidget_SliderLimit.height = (int)value;
			});
			this.mIndex = 0;
			this.mLabel_Index.text = (this.mIndex + 1).ToString();
		}

		public void Next()
		{
			int shipCount = this.mDeckModel.GetShipCount();
			if (this.mIndex + 1 < shipCount)
			{
				this.mIndex++;
				this.mUIRemodelShipSliderThumb_Thumb.get_transform().localPositionY((float)(-(float)this.mCellHeight * this.mIndex));
				this.ChangeShip();
			}
		}

		public void Prev()
		{
			if (0 <= this.mIndex - 1)
			{
				this.mIndex--;
				this.mUIRemodelShipSliderThumb_Thumb.get_transform().localPositionY((float)(-(float)this.mCellHeight * this.mIndex));
				this.ChangeShip();
			}
		}

		public void Show()
		{
			base.get_gameObject().SetActive(true);
			if (!this.initialized)
			{
				return;
			}
			RemodelUtils.MoveWithManual(base.get_gameObject(), this.showPos, 0.3f, delegate
			{
				this.isShown = true;
			});
			base.set_enabled(true);
		}

		public void Hide()
		{
			this.Hide(true);
			base.set_enabled(false);
		}

		public void Hide(bool animation)
		{
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
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mWidget_SliderBackground);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mWidget_SliderLimit);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabel_Index);
			this.mUIRemodelShipSliderThumb_Thumb = null;
			this.mDeckModel = null;
			this.mKeyController = null;
		}
	}
}
