using DG.Tweening;
using KCV.Scene.Port;
using KCV.Utils;
using local.models;
using System;
using UnityEngine;

namespace KCV.Scene.Item
{
	public class UIItemList : MonoBehaviour
	{
		private const int SPLIT_COUNT = 7;

		[SerializeField]
		private UIItemListChild[] mItemListChildren;

		[SerializeField]
		private Transform mTransform_Focus;

		private AudioClip mAudioClip_SE_001;

		private AudioClip mAudioClip_SE_013;

		private KeyControl mKeyController;

		private Action<ItemlistModel> mOnSelectListener;

		private Action<ItemlistModel> mOnFocusChangeListener;

		private UIItemListChild mFocusListChild;

		private void Awake()
		{
			UIItemListChild[] array = this.mItemListChildren;
			for (int i = 0; i < array.Length; i++)
			{
				UIItemListChild uIItemListChild = array[i];
				uIItemListChild.SetOnTouchListener(new Action<UIItemListChild>(this.OnTouchListChild));
			}
			this.mAudioClip_SE_001 = SoundFile.LoadSE(SEFIleInfos.SE_001);
			this.mAudioClip_SE_013 = SoundFile.LoadSE(SEFIleInfos.SE_013);
		}

		private void OnTouchListChild(UIItemListChild child)
		{
			bool flag = this.mKeyController != null;
			if (flag && child != null && child.get_isActiveAndEnabled())
			{
				this.ChangeFocus(child, true);
			}
		}

		private void Update()
		{
			if (this.mKeyController != null)
			{
				if (this.mKeyController.keyState.get_Item(14).down)
				{
					int num = Array.IndexOf<UIItemListChild>(this.mItemListChildren, this.mFocusListChild);
					int num2 = num - 1;
					if (0 <= num2)
					{
						this.ChangeFocus(this.mItemListChildren[num2], true);
					}
				}
				else if (this.mKeyController.keyState.get_Item(10).down)
				{
					int num3 = Array.IndexOf<UIItemListChild>(this.mItemListChildren, this.mFocusListChild);
					int num4 = num3 + 1;
					if (num4 < this.mItemListChildren.Length)
					{
						this.ChangeFocus(this.mItemListChildren[num4], true);
					}
				}
				else if (this.mKeyController.keyState.get_Item(8).down)
				{
					int num5 = Array.IndexOf<UIItemListChild>(this.mItemListChildren, this.mFocusListChild);
					int num6 = num5 - 7;
					if (0 <= num6)
					{
						this.ChangeFocus(this.mItemListChildren[num6], true);
					}
				}
				else if (this.mKeyController.keyState.get_Item(12).down)
				{
					int num7 = Array.IndexOf<UIItemListChild>(this.mItemListChildren, this.mFocusListChild);
					int num8 = num7 + 7;
					if (num8 < this.mItemListChildren.Length)
					{
						this.ChangeFocus(this.mItemListChildren[num8], true);
					}
				}
				else if (this.mKeyController.keyState.get_Item(1).down)
				{
					if (this.mFocusListChild != null)
					{
						ItemlistModel mModel = this.mFocusListChild.mModel;
						if (0 < mModel.Count && mModel.IsUsable())
						{
							SoundUtils.PlaySE(this.mAudioClip_SE_013);
							this.OnSelect(this.mFocusListChild.mModel);
						}
					}
				}
				else if (this.mKeyController.keyState.get_Item(0).down)
				{
					SingletonMonoBehaviour<PortObjectManager>.Instance.BackToActiveScene();
				}
			}
		}

		public void SetKeyController(KeyControl keyController)
		{
			this.mKeyController = keyController;
		}

		public void SetOnSelectListener(Action<ItemlistModel> onSelectListener)
		{
			this.mOnSelectListener = onSelectListener;
		}

		private void OnSelect(ItemlistModel model)
		{
			if (this.mOnSelectListener != null)
			{
				this.mOnSelectListener.Invoke(model);
			}
		}

		public void SetOnFocusChangeListener(Action<ItemlistModel> onFocusChangeListener)
		{
			this.mOnFocusChangeListener = onFocusChangeListener;
		}

		private void OnFocusChange(ItemlistModel model)
		{
			if (this.mOnFocusChangeListener != null)
			{
				this.mOnFocusChangeListener.Invoke(model);
			}
		}

		public void Initialize(ItemlistModel[] models)
		{
			int num = 0;
			int num2 = models.Length;
			UIItemListChild[] array = this.mItemListChildren;
			for (int i = 0; i < array.Length; i++)
			{
				UIItemListChild uIItemListChild = array[i];
				uIItemListChild.SetActive(true);
				if (num < num2)
				{
					uIItemListChild.Initialize(models[num]);
				}
				else
				{
					uIItemListChild.Initialize(new ItemlistModel(null, null, string.Empty));
				}
				num++;
			}
			this.ChangeFocus(null, false);
		}

		public void FirstFocus()
		{
			this.ChangeFocus(this.mItemListChildren[0], false);
		}

		public void Refresh(ItemlistModel[] models)
		{
			int num = Array.IndexOf<UIItemListChild>(this.mItemListChildren, this.mFocusListChild);
			int num2 = 0;
			int num3 = models.Length;
			UIItemListChild[] array = this.mItemListChildren;
			for (int i = 0; i < array.Length; i++)
			{
				UIItemListChild uIItemListChild = array[i];
				if (num2 < num3)
				{
					uIItemListChild.SetActive(true);
					uIItemListChild.Initialize(models[num2]);
				}
				else
				{
					uIItemListChild.SetActive(false);
				}
				num2++;
			}
			this.ChangeFocus(this.mItemListChildren[num], false);
		}

		private void ChangeFocus(UIItemListChild child, bool playSE)
		{
			if (this.mFocusListChild != null)
			{
				this.mFocusListChild.RemoveFocus();
				ShortcutExtensions.DOScale(this.mFocusListChild.get_transform(), new Vector3(1f, 1f), 0.3f);
				ShortcutExtensions.DOScale(this.mTransform_Focus, new Vector3(1f, 1f), 0.3f);
			}
			this.mFocusListChild = child;
			if (this.mFocusListChild != null)
			{
				this.OnFocusChange(this.mFocusListChild.mModel);
				this.mFocusListChild.Focus();
				if (child.IsFosable())
				{
					this.mTransform_Focus.set_localPosition(new Vector3(this.mFocusListChild.get_transform().get_localPosition().x, this.mFocusListChild.get_transform().get_localPosition().y - 12f));
					ShortcutExtensions.DOScale(this.mTransform_Focus, new Vector3(1.2f, 1.2f), 0.3f);
					ShortcutExtensions.DOScale(this.mFocusListChild.get_transform(), new Vector3(1.2f, 1.2f), 0.3f);
				}
				else
				{
					this.mTransform_Focus.set_localPosition(new Vector3(this.mFocusListChild.get_transform().get_localPosition().x, this.mFocusListChild.get_transform().get_localPosition().y - 12f));
				}
				if (playSE)
				{
					this.SafePlaySEOneShot(this.mAudioClip_SE_001);
				}
			}
			else
			{
				this.mTransform_Focus.get_transform().set_localPosition(new Vector3(this.mItemListChildren[0].get_transform().get_localPosition().x, this.mItemListChildren[0].get_transform().get_localPosition().y - 12f));
				this.OnFocusChange(null);
			}
		}

		private void SafePlaySEOneShot(AudioClip audioClip)
		{
			if (SingletonMonoBehaviour<SoundManager>.exist() && SingletonMonoBehaviour<SoundManager>.Instance.seSourceObserver != null)
			{
				int num = SingletonMonoBehaviour<SoundManager>.Instance.seSourceObserver.get_Count() - 1;
				if (SingletonMonoBehaviour<SoundManager>.Instance.seSourceObserver.get_Item(num) != null && SingletonMonoBehaviour<SoundManager>.Instance.seSourceObserver.get_Item(num).source != null)
				{
					SingletonMonoBehaviour<SoundManager>.Instance.seSourceObserver.get_Item(num).source.PlayOneShot(audioClip, SingletonMonoBehaviour<SoundManager>.Instance.soundVolume.SE);
				}
			}
		}

		public void Clean()
		{
			this.mKeyController = null;
			this.ChangeFocus(null, false);
		}

		private void OnDestroy()
		{
			if (this.mItemListChildren != null)
			{
				for (int i = 0; i < this.mItemListChildren.Length; i++)
				{
					this.mItemListChildren[i] = null;
				}
				this.mItemListChildren = null;
			}
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mAudioClip_SE_001, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mAudioClip_SE_013, false);
		}
	}
}
