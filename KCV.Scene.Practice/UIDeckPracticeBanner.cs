using DG.Tweening;
using KCV.Scene.Port;
using local.models;
using System;
using UnityEngine;

namespace KCV.Scene.Practice
{
	[RequireComponent(typeof(UIWidget))]
	public class UIDeckPracticeBanner : MonoBehaviour
	{
		[SerializeField]
		private UITexture mTexture_Ship;

		[SerializeField]
		private UITexture mTexture_LevelUp;

		private UIWidget mWidgetThis;

		private Vector3 mDefaultLocalPosition;

		private Vector3 mLevelUpDefaultLocalPosition;

		private int xMovePos = 20;

		private float xMoveTime = 0.15f;

		private Ease xMoveEase = 21;

		private Ease mxMoveEase = 21;

		private float mxMoveTime = 0.5f;

		private int xxMovePos = 40;

		private float xxMoveTime = 0.15f;

		private float xxMoveTimeDelay = 1f;

		private float mxxMoveTime = 0.5f;

		private Vector3 mVector3_Show_From = new Vector3(60f, 0f);

		private Vector3 mVector3_Show_To = new Vector3(60f, 20f);

		private float slotTime = 0.7f;

		private float delay = 0.5f;

		public ShipModel Model
		{
			get;
			private set;
		}

		public float alpha
		{
			get
			{
				if (this.mWidgetThis != null)
				{
					return this.mWidgetThis.alpha;
				}
				return -1f;
			}
			set
			{
				if (this.mWidgetThis != null)
				{
					this.mWidgetThis.alpha = value;
				}
			}
		}

		private void Awake()
		{
			this.mWidgetThis = base.GetComponent<UIWidget>();
			this.mWidgetThis.alpha = 0.0001f;
			this.mTexture_LevelUp.alpha = 0.0001f;
			this.mDefaultLocalPosition = base.get_transform().get_localPosition();
			this.mLevelUpDefaultLocalPosition = this.mTexture_LevelUp.get_transform().get_localPosition();
		}

		private void Update()
		{
			if (Input.GetKeyDown(116))
			{
				this.PlayPractice();
			}
			else if (Input.GetKeyDown(121))
			{
				this.PlayPracticeWithLevelUp();
			}
		}

		public void PlayPractice()
		{
			this.GenerateTweenMoveNormal();
		}

		public void PlayPracticeWithLevelUp()
		{
			this.GenerateTweenMoveLevelUp();
		}

		private Tween GenerateTweenMoveLevelUp()
		{
			Sequence sequence = DOTween.Sequence();
			TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetId<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMoveX(base.get_transform(), base.get_transform().get_localPosition().x + (float)this.xMovePos, this.xMoveTime, false), this.xMoveEase), this));
			TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetId<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMoveX(base.get_transform(), base.get_transform().get_localPosition().x, this.mxMoveTime, false), 30), this));
			TweenSettingsExtensions.AppendInterval(sequence, this.xxMoveTimeDelay);
			TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetId<Tweener>(ShortcutExtensions.DOLocalMoveX(base.get_transform(), base.get_transform().get_localPosition().x + (float)this.xxMovePos, this.xxMoveTime, false), this));
			TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetId<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMoveX(base.get_transform(), base.get_transform().get_localPosition().x, this.mxxMoveTime, false), 30), this));
			TweenSettingsExtensions.Join(sequence, this.GenerateTweenLevelUp());
			return sequence;
		}

		private Tween GenerateTweenMoveNormal()
		{
			Sequence sequence = TweenSettingsExtensions.SetId<Sequence>(DOTween.Sequence(), this);
			TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetId<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMoveX(base.get_transform(), base.get_transform().get_localPosition().x + (float)this.xMovePos, this.xMoveTime, false), this.xMoveEase), this));
			TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetId<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMoveX(base.get_transform(), base.get_transform().get_localPosition().x, this.mxMoveTime, false), 30), this));
			return sequence;
		}

		private Tween GenerateTweenLevelUp()
		{
			Sequence sequence = TweenSettingsExtensions.SetId<Sequence>(DOTween.Sequence(), this);
			this.mTexture_LevelUp.alpha = 0f;
			this.mTexture_LevelUp.get_transform().set_localPosition(this.mVector3_Show_From);
			Tween tween = TweenSettingsExtensions.SetId<Tweener>(TweenSettingsExtensions.SetDelay<Tweener>(DOVirtual.Float(1f, 0f, this.slotTime, delegate(float alpha)
			{
				this.mTexture_LevelUp.alpha = alpha;
			}), this.delay), this);
			TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetId<Tweener>(ShortcutExtensions.DOLocalMove(this.mTexture_LevelUp.get_transform(), this.mVector3_Show_To, this.slotTime, false), this));
			TweenSettingsExtensions.Join(sequence, tween);
			TweenSettingsExtensions.OnPlay<Sequence>(sequence, delegate
			{
				this.mTexture_LevelUp.alpha = 1f;
			});
			return sequence;
		}

		public void Initialize(ShipModel model)
		{
			this.Model = model;
			int texNum = this.Model.IsDamaged() ? 2 : 1;
			this.mTexture_Ship.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(model.MstId, texNum);
		}

		private void OnDestroy()
		{
			if (DOTween.IsTweening(this))
			{
				DOTween.Kill(this, false);
			}
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_Ship, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_LevelUp, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mWidgetThis);
			this.Model = null;
		}
	}
}
