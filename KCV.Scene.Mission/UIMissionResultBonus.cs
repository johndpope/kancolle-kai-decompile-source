using Common.Struct;
using DG.Tweening;
using KCV.Scene.Port;
using local.models;
using System;
using UnityEngine;

namespace KCV.Scene.Mission
{
	public class UIMissionResultBonus : MonoBehaviour
	{
		[SerializeField]
		private UILabel mLabel_Exp;

		[SerializeField]
		private UILabel mLabel_Fuel;

		[SerializeField]
		private UILabel mLabel_Steel;

		[SerializeField]
		private UILabel mLabel_Ammo;

		[SerializeField]
		private UILabel mLabel_SPoint;

		[SerializeField]
		private UILabel mLabel_Bauxite;

		[SerializeField]
		private Transform mTransform_ShipFrame;

		[SerializeField]
		private Transform mTransform_ResultBonusFrame;

		[SerializeField]
		private Transform[] mTransforms_Reward;

		private MissionResultModel mMissionResultModel;

		public void Inititalize(MissionResultModel missionResultModel)
		{
			this.mMissionResultModel = missionResultModel;
			Transform[] array = this.mTransforms_Reward;
			for (int i = 0; i < array.Length; i++)
			{
				Transform transform = array[i];
				transform.SetActive(false);
				transform.Find("Label_Value").GetComponent<UILabel>().text = string.Empty;
				transform.Find("Sprite_RewardIcon").GetComponent<UISprite>().spriteName = string.Empty;
			}
			int extraItemCount = missionResultModel.ExtraItemCount;
			int num = 0;
			while (num < extraItemCount && num < this.mTransforms_Reward.Length)
			{
				Transform transform2 = this.mTransforms_Reward[num];
				transform2.SetActive(true);
				transform2.Find("Label_Value").GetComponent<UILabel>().text = missionResultModel.GetItemCount(num).ToString();
				transform2.Find("Sprite_RewardIcon").GetComponent<UISprite>().spriteName = "item_" + missionResultModel.GetItemID(num).ToString();
				num++;
			}
		}

		public void Play(Action onFinished)
		{
			this.PlayResult(onFinished);
		}

		public void PlayResult(Action onFinished)
		{
			this.mLabel_Exp.text = "0";
			this.mLabel_Fuel.text = "0";
			this.mLabel_Ammo.text = "0";
			this.mLabel_Steel.text = "0";
			this.mLabel_Bauxite.text = "0";
			this.mLabel_SPoint.text = "0";
			Sequence sequence = TweenSettingsExtensions.SetId<Sequence>(DOTween.Sequence(), this);
			Tween tween = ShortcutExtensions.DOLocalMove(this.mTransform_ShipFrame, new Vector3(265f, 150f), 0.8f, false);
			this.mTransform_ResultBonusFrame.get_transform().localPositionX(128f);
			Tween tween2 = TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMove(this.mTransform_ResultBonusFrame, Vector3.get_zero(), 0.4f, false), 21);
			UIWidget resultBonusAlpha = this.mTransform_ResultBonusFrame.GetComponent<UIWidget>();
			resultBonusAlpha.alpha = 0f;
			Tween tween3 = TweenSettingsExtensions.SetEase<Tweener>(DOVirtual.Float(0f, 1f, 0.3f, delegate(float alpha)
			{
				resultBonusAlpha.alpha = alpha;
			}), 20);
			Tween tween4 = DOVirtual.Float(0f, 1f, 0.8f, delegate(float part)
			{
				MaterialInfo materialInfo = this.mMissionResultModel.GetMaterialInfo();
				this.mLabel_Exp.text = ((int)((float)this.mMissionResultModel.Exp * part)).ToString();
				this.mLabel_Fuel.text = ((int)((float)materialInfo.Fuel * part)).ToString();
				this.mLabel_Ammo.text = ((int)((float)materialInfo.Ammo * part)).ToString();
				this.mLabel_Steel.text = ((int)((float)materialInfo.Steel * part)).ToString();
				this.mLabel_Bauxite.text = ((int)((float)materialInfo.Baux * part)).ToString();
				this.mLabel_SPoint.text = ((int)((float)this.mMissionResultModel.Spoint * part)).ToString();
			});
			TweenSettingsExtensions.Append(sequence, tween);
			TweenSettingsExtensions.AppendInterval(sequence, 0.5f);
			TweenSettingsExtensions.Append(sequence, tween4);
			TweenSettingsExtensions.Join(sequence, tween3);
			TweenSettingsExtensions.Join(sequence, tween2);
			TweenSettingsExtensions.OnComplete<Sequence>(sequence, delegate
			{
				if (onFinished != null)
				{
					onFinished.Invoke();
				}
			});
		}

		private void OnDestroy()
		{
			bool flag = DOTween.IsTweening(this);
			if (flag)
			{
				DOTween.Kill(this, false);
			}
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabel_Exp);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabel_Fuel);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabel_Steel);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabel_Ammo);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabel_SPoint);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabel_Bauxite);
			this.mTransform_ShipFrame = null;
			this.mTransform_ResultBonusFrame = null;
			this.mTransforms_Reward = null;
			this.mMissionResultModel = null;
		}
	}
}
