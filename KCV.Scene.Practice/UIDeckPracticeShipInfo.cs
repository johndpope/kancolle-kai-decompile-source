using DG.Tweening;
using local.models;
using System;
using System.Linq;
using UnityEngine;

namespace KCV.Scene.Practice
{
	public class UIDeckPracticeShipInfo : MonoBehaviour
	{
		[SerializeField]
		private UILabel mLabel_ShipName;

		[SerializeField]
		private UILabel mLabel_Level;

		[SerializeField]
		private UILabel mLabel_ShipType;

		[SerializeField]
		private UISlider mSlider_Exp;

		private ShipModel mShipModel;

		private ShipExpModel mShipExpModel;

		private Vector3 mVector3_DefaultLocalPosition;

		private void Awake()
		{
			this.mVector3_DefaultLocalPosition = base.get_transform().get_localPosition();
		}

		public void Initialize(ShipModel shipModel, ShipExpModel exp)
		{
			this.mShipModel = shipModel;
			this.mShipExpModel = exp;
			this.mLabel_Level.text = exp.LevelBefore.ToString();
			this.mLabel_ShipName.text = shipModel.Name;
			this.mLabel_ShipType.text = shipModel.ShipTypeName;
			this.mSlider_Exp.value = ((exp.ExpRateBefore != 0) ? ((float)exp.ExpRateBefore / 100f) : 0f);
		}

		public Tween GenerateTweenExpAndLevel()
		{
			bool flag = DOTween.IsTweening(this);
			if (flag)
			{
				DOTween.Kill(this, false);
			}
			int expRateAfterAll = Enumerable.Sum(Enumerable.Where<int>(this.mShipExpModel.ExpRateAfter, (int num) => true));
			return TweenSettingsExtensions.SetId<Tweener>(DOVirtual.Float(0f, 1f, 1.3f, delegate(float percentage)
			{
				float num = (float)(this.mShipExpModel.ExpRateBefore + (int)((float)(expRateAfterAll - this.mShipExpModel.ExpRateBefore) * percentage));
				float value = (num != 0f) ? (num / 100f % 1f) : 0f;
				int num2 = (int)num / 100;
				this.mSlider_Exp.value = value;
				if (this.mLabel_Level.text != (this.mShipExpModel.LevelBefore + num2).ToString())
				{
					this.mLabel_Level.text = (this.mShipExpModel.LevelBefore + num2).ToString();
				}
			}), this);
		}

		public void Reposition()
		{
			base.get_transform().set_localPosition(this.mVector3_DefaultLocalPosition);
		}

		private void OnDestroy()
		{
			this.mLabel_ShipName = null;
			this.mLabel_Level = null;
			this.mLabel_ShipType = null;
			this.mSlider_Exp = null;
			this.mShipModel = null;
			this.mShipExpModel = null;
		}
	}
}
