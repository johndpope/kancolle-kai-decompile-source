using DG.Tweening;
using KCV.Interior;
using KCV.Utils;
using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.InteriorStore
{
	[RequireComponent(typeof(UIButtonManager)), RequireComponent(typeof(UIPanel))]
	public class UIFurniturePurchaseDialog : MonoBehaviour
	{
		[SerializeField]
		private UILabel mLabel_Category;

		[SerializeField]
		private UITexture mTexture_Thumbnail;

		[SerializeField]
		private UILabel mLabel_Price;

		[SerializeField]
		private UILabel mLabel_Name;

		[SerializeField]
		private Transform[] mTransforms_Rate;

		[SerializeField]
		private UILabel mLabel_WorkerCount;

		[SerializeField]
		private UIButton mButton_Negative;

		[SerializeField]
		private UIButton mButton_Positive;

		[SerializeField]
		private UIButton mButton_Preview;

		private UIButton[] mFocasableButtons;

		private UIPanel mPanelThis;

		private UIButtonManager mButtonManager;

		private UIButton mButtonFocus;

		private Action mOnSelectNegativeListener;

		private Action mOnSelectPositiveListener;

		private Action mOnSelectPreviewListener;

		private KeyControl mKeyController;

		private FurnitureModel mFurnitureModel;

		private void Awake()
		{
			this.mPanelThis = base.GetComponent<UIPanel>();
			this.mButtonManager = base.GetComponent<UIButtonManager>();
			this.mButtonManager.IndexChangeAct = delegate
			{
				if (0 <= Array.IndexOf<UIButton>(this.mFocasableButtons, this.mButtonManager.nowForcusButton))
				{
					this.ChangeFocus(this.mButtonManager.nowForcusButton, false);
				}
			};
		}

		[Obsolete("Inspector上で使用します。")]
		public void OnTouchNegative()
		{
			this.OnSelectNegative();
		}

		[Obsolete("Inspector上で使用します。")]
		public void OnTouchPositive()
		{
			this.OnSelectPositive();
		}

		[Obsolete("Inspector上で使用します。")]
		public void OnTouchPreview()
		{
			this.OnSelectPreview();
		}

		private void Update()
		{
			if (this.mKeyController != null)
			{
				if (this.mKeyController.keyState.get_Item(14).down)
				{
					int num = Array.IndexOf<UIButton>(this.mFocasableButtons, this.mButtonFocus);
					int num2 = num - 1;
					if (0 <= num2)
					{
						SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
						this.ChangeFocus(this.mFocasableButtons[num2], true);
					}
				}
				else if (this.mKeyController.keyState.get_Item(10).down)
				{
					int num3 = Array.IndexOf<UIButton>(this.mFocasableButtons, this.mButtonFocus);
					int num4 = num3 + 1;
					if (num4 < this.mFocasableButtons.Length)
					{
						SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
						this.ChangeFocus(this.mFocasableButtons[num4], true);
					}
				}
				else if (this.mKeyController.keyState.get_Item(1).down)
				{
					if (this.mButton_Negative.Equals(this.mButtonFocus))
					{
						this.OnSelectNegative();
					}
					else if (this.mButton_Positive.Equals(this.mButtonFocus))
					{
						this.OnSelectPositive();
					}
					else if (this.mButton_Preview.Equals(this.mButtonFocus))
					{
						this.OnSelectPreview();
					}
				}
				else if (this.mKeyController.keyState.get_Item(0).down)
				{
					this.OnSelectNegative();
				}
			}
		}

		private void OnSelectNegative()
		{
			if (this.mOnSelectNegativeListener != null)
			{
				this.mOnSelectNegativeListener.Invoke();
			}
		}

		public void SetOnSelectNegativeListener(Action onSelectNegativeListener)
		{
			this.mOnSelectNegativeListener = onSelectNegativeListener;
		}

		private void OnSelectPositive()
		{
			if (this.mOnSelectPositiveListener != null)
			{
				this.mOnSelectPositiveListener.Invoke();
			}
		}

		public void SetOnSelectPositiveListener(Action onSelectPositiveListener)
		{
			this.mOnSelectPositiveListener = onSelectPositiveListener;
		}

		private void OnSelectPreview()
		{
			if (this.mOnSelectPreviewListener != null)
			{
				this.mOnSelectPreviewListener.Invoke();
			}
		}

		public void SetOnSelectPreviewListener(Action onSelectPreviewListener)
		{
			this.mOnSelectPreviewListener = onSelectPreviewListener;
		}

		public void SetKeyController(KeyControl keyController)
		{
			this.mKeyController = keyController;
		}

		public void Initialize(FurnitureModel furnitureModel, bool isValidBuy)
		{
			this.mFurnitureModel = furnitureModel;
			this.mLabel_Category.text = string.Format("購入 - {0} - ", UserInterfaceInteriorManager.FurnitureKindToString(furnitureModel.Type));
			List<UIButton> list = new List<UIButton>();
			list.Add(this.mButton_Negative);
			if (isValidBuy)
			{
				list.Add(this.mButton_Positive);
			}
			list.Add(this.mButton_Preview);
			this.mFocasableButtons = list.ToArray();
			this.mButtonManager.UpdateButtons(this.mFocasableButtons);
			this.ChangeFocus(this.mFocasableButtons[0], false);
			if (isValidBuy)
			{
				this.mButton_Positive.set_enabled(true);
				this.mButton_Positive.isEnabled = true;
				this.mButton_Positive.SetState(UIButtonColor.State.Normal, true);
			}
			else
			{
				this.mButton_Positive.set_enabled(false);
				this.mButton_Positive.SetState(UIButtonColor.State.Disabled, true);
			}
			this.mTexture_Thumbnail.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.Furniture.LoadInteriorStoreFurniture(this.mFurnitureModel.Type, this.mFurnitureModel.MstId);
			this.mLabel_WorkerCount.text = ((!this.mFurnitureModel.IsNeedWorker()) ? "不要" : "必要");
			this.mLabel_Price.text = this.mFurnitureModel.Price.ToString();
			this.mLabel_Name.text = this.mFurnitureModel.Name;
			for (int i = 0; i < this.mTransforms_Rate.Length; i++)
			{
				if (i < this.mFurnitureModel.Rarity)
				{
					this.mTransforms_Rate[i].SetActive(true);
				}
				else
				{
					this.mTransforms_Rate[i].SetActive(false);
				}
			}
		}

		public void Show()
		{
			ShortcutExtensions.DOScale(base.get_transform(), Vector3.get_one(), 0.3f);
			DOVirtual.Float(this.mPanelThis.alpha, 1f, 0.3f, delegate(float alpha)
			{
				this.mPanelThis.alpha = alpha;
			});
		}

		public void Hide()
		{
			ShortcutExtensions.DOScale(base.get_transform(), Vector3.get_zero(), 0.3f);
			DOVirtual.Float(this.mPanelThis.alpha, 0f, 0.3f, delegate(float alpha)
			{
				this.mPanelThis.alpha = alpha;
			});
		}

		private void ChangeFocus(UIButton targetButton, bool needSe)
		{
			if (this.mButtonFocus != null)
			{
				this.mButtonFocus.SetState(UIButtonColor.State.Normal, true);
			}
			this.mButtonFocus = targetButton;
			if (this.mButtonFocus != null)
			{
				this.mButtonFocus.SetState(UIButtonColor.State.Hover, true);
			}
		}

		public FurnitureModel GetModel()
		{
			return this.mFurnitureModel;
		}

		internal void ResumeFocus()
		{
			if (this.mButtonFocus != null)
			{
				this.mButtonFocus.SetState(UIButtonColor.State.Hover, true);
			}
		}
	}
}
