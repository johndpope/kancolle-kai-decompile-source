using KCV.Production;
using KCV.Utils;
using local.models;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Scene.Revamp
{
	[RequireComponent(typeof(UIPanel))]
	public class UIRevampIcon : MonoBehaviour
	{
		private const int MAX_LEVEL_ITEM = 10;

		private const int MIN_LEVEL_ITEM = 0;

		[SerializeField]
		private ProdRevampReceiveItem mPreafab_ProdRevampReceiveItem;

		[SerializeField]
		private UITexture mTexture_Icon;

		[SerializeField]
		private UITexture mTexture_Icon2;

		[SerializeField]
		private GameObject mGameObject_LevelBackgroundTextImage;

		[SerializeField]
		private GameObject mGameObject_LevelTag;

		[SerializeField]
		private GameObject mGameObject_LevelMaxTag;

		[SerializeField]
		private UILabel mLabel_Level;

		[SerializeField]
		private Animation mAnimation_Effect;

		private UIPanel mPanelThis;

		private Coroutine mAnimationCoroutine;

		private SlotitemModel mSlotitemModel;

		private bool mIsGradeUpItem;

		private Camera mCameraProduction;

		private int _before;

		public int mBeforeSlotItemMasterId
		{
			get;
			private set;
		}

		public int mBeforeSlotItemLevel
		{
			get;
			private set;
		}

		public int mAfterSlotItemMasterId
		{
			get;
			private set;
		}

		public int mAfterSlotItemLevel
		{
			get;
			private set;
		}

		public string mAfterSlotItemName
		{
			get;
			private set;
		}

		private void Awake()
		{
			this.mPanelThis = base.GetComponent<UIPanel>();
			this.mPanelThis.alpha = 0.01f;
		}

		public void Initialize(int beforeMasterId, int beforeLevel, Camera cameraProduction)
		{
			this.mCameraProduction = cameraProduction;
			this.mBeforeSlotItemMasterId = beforeMasterId;
			this.mBeforeSlotItemLevel = beforeLevel;
			this._before = beforeLevel;
			this.UpdateIcon(this.mBeforeSlotItemMasterId);
			this.UpdateLevel(this.mBeforeSlotItemLevel, false);
			this.mPanelThis.alpha = 1f;
		}

		private void UpdateLevel(int slotItemLevel, bool anime)
		{
			if (slotItemLevel <= 0)
			{
				this.mGameObject_LevelBackgroundTextImage.SetActive(false);
				this.mGameObject_LevelMaxTag.SetActive(false);
				this.mGameObject_LevelTag.SetActive(false);
				this.mLabel_Level.SetActive(false);
			}
			else if (10 <= slotItemLevel)
			{
				this.mGameObject_LevelBackgroundTextImage.SetActive(false);
				this.mGameObject_LevelMaxTag.SetActive(true);
				this.mGameObject_LevelTag.SetActive(false);
				this.mLabel_Level.SetActive(false);
				if (anime && this._before != slotItemLevel)
				{
					this.LevelIconAnime(this.mLabel_Level.get_transform());
				}
			}
			else
			{
				this.mGameObject_LevelTag.SetActive(true);
				this.mGameObject_LevelBackgroundTextImage.SetActive(true);
				this.mGameObject_LevelMaxTag.SetActive(false);
				this.mLabel_Level.SetActive(true);
				this.mLabel_Level.text = slotItemLevel.ToString();
				if (anime && this._before != slotItemLevel)
				{
					this.LevelIconAnime(this.mLabel_Level.get_transform());
				}
			}
			this._before = slotItemLevel;
		}

		private void LevelIconAnime(Transform tf)
		{
			tf.get_parent().set_localScale(Vector3.get_one() * 2f);
			TweenScale tweenScale = TweenScale.Begin(tf.get_parent().get_gameObject(), 0.3f, Vector3.get_one());
			tweenScale.animationCurve = UtilCurves.TweenEaseOutBack;
			tweenScale.delay = 0.1f;
		}

		private void UpdateIcon(int slotItemMasterId)
		{
			this.mTexture_Icon.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.SlotItemTexture.Load(slotItemMasterId, 1);
			this.mTexture_Icon.SetDimensions((int)ResourceManager.SLOTITEM_TEXTURE_SIZE.get_Item(1).x, (int)ResourceManager.SLOTITEM_TEXTURE_SIZE.get_Item(1).y);
		}

		public void StartRevamp(int afterSlotItemMasterId, int afterSlotItemLevel, string afterSlotItemName, Action finishedCallBack)
		{
			this.mAfterSlotItemLevel = afterSlotItemLevel;
			this.mAfterSlotItemMasterId = afterSlotItemMasterId;
			this.mAfterSlotItemName = afterSlotItemName;
			this.mIsGradeUpItem = (this.mBeforeSlotItemMasterId != this.mAfterSlotItemMasterId);
			if (this.mAnimationCoroutine != null)
			{
				base.StopCoroutine(this.mAnimationCoroutine);
			}
			this.mAnimationCoroutine = base.StartCoroutine(this.StartRevampCoroutine(delegate
			{
				this.UpdateLevel(this.mAfterSlotItemLevel, true);
				this.UpdateIcon(this.mAfterSlotItemMasterId);
				this.mAnimationCoroutine = null;
				if (finishedCallBack != null)
				{
					finishedCallBack.Invoke();
				}
			}));
		}

		public void StartFailRevamp(Action finishedCallBack)
		{
			if (this.mAnimationCoroutine != null)
			{
				base.StopCoroutine(this.mAnimationCoroutine);
			}
			this.mAnimationCoroutine = base.StartCoroutine(this.StartRevampCoroutine(delegate
			{
				this.mAnimationCoroutine = null;
				if (finishedCallBack != null)
				{
					finishedCallBack.Invoke();
				}
			}));
		}

		[DebuggerHidden]
		private IEnumerator StartRevampCoroutine(Action finishedCallBack)
		{
			UIRevampIcon.<StartRevampCoroutine>c__IteratorC6 <StartRevampCoroutine>c__IteratorC = new UIRevampIcon.<StartRevampCoroutine>c__IteratorC6();
			<StartRevampCoroutine>c__IteratorC.finishedCallBack = finishedCallBack;
			<StartRevampCoroutine>c__IteratorC.<$>finishedCallBack = finishedCallBack;
			<StartRevampCoroutine>c__IteratorC.<>f__this = this;
			return <StartRevampCoroutine>c__IteratorC;
		}

		private void PlaySE(SEFIleInfos seType)
		{
			if (SingletonMonoBehaviour<SoundManager>.Instance != null)
			{
				bool flag = true;
				if (flag)
				{
					SoundUtils.PlaySE(seType);
				}
			}
			else
			{
				Debug.LogError("Plz Place SoundManager GameObject !! X(");
			}
		}

		private void OnDestroy()
		{
			this.mPreafab_ProdRevampReceiveItem = null;
			this.mTexture_Icon = null;
			this.mTexture_Icon2 = null;
			this.mGameObject_LevelBackgroundTextImage = null;
			this.mGameObject_LevelTag = null;
			this.mGameObject_LevelMaxTag = null;
			this.mLabel_Level = null;
			this.mAnimation_Effect = null;
			this.mPanelThis = null;
			this.mSlotitemModel = null;
			this.mCameraProduction = null;
		}
	}
}
