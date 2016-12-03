using DG.Tweening;
using KCV.Scene.Port;
using KCV.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Furniture
{
	public class UIDynamicWindowFurnitureFuurin : UIDynamicWindowFurniture
	{
		private const int MAX_FUURIN_RANDOM_CALL = 3;

		[SerializeField]
		private Transform mTransform_Fuurin;

		[SerializeField]
		private Transform mTransform_Wing;

		[SerializeField]
		private UITexture mTexture_Wing;

		[SerializeField]
		private Texture mTexture2d_Frame_0;

		[SerializeField]
		private Texture mTexture2d_Frame_1;

		[SerializeField]
		private Texture mTexture2d_Frame_2;

		[SerializeField]
		private AudioClip mAudioClip_Fuurin;

		private int mFuurinCounter;

		private List<int> TimeTable;

		private Stack<int> mCheckTable;

		public UIDynamicWindowFurnitureFuurin()
		{
			List<int> list = new List<int>();
			list.Add(5);
			list.Add(10);
			list.Add(20);
			list.Add(25);
			list.Add(35);
			list.Add(40);
			list.Add(50);
			list.Add(55);
			this.TimeTable = list;
			this.mCheckTable = new Stack<int>();
			base..ctor();
		}

		protected override void OnAwake()
		{
			this.mCheckTable.Clear();
		}

		protected override void OnCalledActionEvent()
		{
			this.Animation();
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();
			this.DoTimmingTriggerAnimation();
		}

		private bool DoTimmingTriggerAnimation()
		{
			int minute = this.mFurnitureModel.GetDateTime().get_Minute();
			bool flag = 0 == minute;
			if (flag)
			{
				bool flag2 = 0 < this.mCheckTable.get_Count();
				if (flag2)
				{
					this.mFuurinCounter = 0;
					this.mCheckTable.Clear();
				}
				return false;
			}
			bool flag3 = this.mCheckTable.Contains(minute);
			if (flag3)
			{
				return false;
			}
			bool flag4 = DOTween.IsTweening(this);
			if (flag4)
			{
				return false;
			}
			bool flag5 = 3 <= this.mFuurinCounter;
			if (flag5)
			{
				return false;
			}
			bool flag6 = !this.TimeTable.Contains(minute);
			if (flag6)
			{
				return false;
			}
			this.mCheckTable.Push(minute);
			bool flag7 = Random.Range(0, 100) < 50;
			if (flag7)
			{
				this.Animation();
				this.mFuurinCounter++;
				return true;
			}
			return false;
		}

		private void Animation()
		{
			bool flag = DOTween.IsTweening(this);
			if (flag)
			{
				return;
			}
			Sequence sequence = DOTween.Sequence();
			TweenSettingsExtensions.SetId<Sequence>(sequence, this);
			Sequence sequence2 = DOTween.Sequence();
			TweenSettingsExtensions.SetId<Sequence>(sequence2, this);
			this.mTransform_Fuurin.set_localRotation(Quaternion.Euler(new Vector3(0f, 0f, -10f)));
			Tween tween = ShortcutExtensions.DOLocalRotate(this.mTransform_Fuurin.get_transform(), Vector3.get_zero(), 0.6f, 0);
			Tween tween2 = ShortcutExtensions.DOLocalRotate(this.mTransform_Fuurin.get_transform(), new Vector3(0f, 0f, -5f), 0.6f, 0);
			Tween tween3 = ShortcutExtensions.DOLocalRotate(this.mTransform_Fuurin.get_transform(), Vector3.get_zero(), 0.3f, 0);
			TweenSettingsExtensions.Append(sequence2, tween);
			TweenSettingsExtensions.Append(sequence2, tween2);
			TweenSettingsExtensions.Append(sequence2, tween3);
			Sequence sequence3 = DOTween.Sequence();
			TweenSettingsExtensions.SetId<Sequence>(sequence3, this);
			TweenCallback tweenCallback = delegate
			{
				this.mTexture_Wing.mainTexture = this.mTexture2d_Frame_2;
			};
			TweenSettingsExtensions.OnPlay<Sequence>(sequence3, tweenCallback);
			TweenSettingsExtensions.AppendInterval(sequence3, 0.1f);
			TweenCallback tweenCallback2 = delegate
			{
				this.mTexture_Wing.mainTexture = this.mTexture2d_Frame_0;
			};
			Tween tween4 = ShortcutExtensions.DOLocalRotate(this.mTransform_Wing.get_transform(), new Vector3(0f, 0f, -4f), 0.2f, 0);
			Tween tween5 = ShortcutExtensions.DOLocalRotate(this.mTransform_Wing.get_transform(), new Vector3(0f, 0f, 25f), 0.8f, 0);
			Tween tween6 = ShortcutExtensions.DOLocalRotate(this.mTransform_Wing.get_transform(), new Vector3(0f, 0f, -12.5f), 0.8f, 0);
			Tween tween7 = ShortcutExtensions.DOLocalRotate(this.mTransform_Wing.get_transform(), new Vector3(0f, 0f, 0f), 0.8f, 0);
			TweenSettingsExtensions.Append(sequence3, tween4);
			TweenSettingsExtensions.Append(sequence3, tween5);
			TweenSettingsExtensions.Append(sequence3, tween6);
			TweenSettingsExtensions.Append(sequence3, tween7);
			TweenSettingsExtensions.AppendCallback(sequence3, delegate
			{
				this.mTexture_Wing.mainTexture = this.mTexture2d_Frame_1;
			});
			TweenSettingsExtensions.AppendInterval(sequence3, 0.1f);
			TweenSettingsExtensions.AppendCallback(sequence3, delegate
			{
				this.mTexture_Wing.mainTexture = this.mTexture2d_Frame_0;
			});
			TweenSettingsExtensions.Append(sequence, sequence2);
			TweenSettingsExtensions.Join(sequence, sequence3);
			SoundUtils.PlaySE(this.mAudioClip_Fuurin);
		}

		protected override void OnDestroyEvent()
		{
			base.OnDestroyEvent();
			if (DOTween.IsTweening(this))
			{
				DOTween.Kill(this, false);
			}
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_Wing, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture2d_Frame_0, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture2d_Frame_1, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture2d_Frame_2, false);
			this.mTransform_Fuurin = null;
			this.mTransform_Wing = null;
		}
	}
}
