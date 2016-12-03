using KCV.Utils;
using LT.Tweening;
using System;
using UnityEngine;

namespace KCV.BattleCut
{
	[RequireComponent(typeof(UIPanel))]
	public class BtlCut_Live2D : MonoBehaviour
	{
		[SerializeField]
		private UIShipCharacter _uiShipCharacter;

		private UIPanel _uiPanel;

		public UIShipCharacter shipCharacter
		{
			get
			{
				return this._uiShipCharacter;
			}
		}

		public UIPanel panel
		{
			get
			{
				return this.GetComponentThis(ref this._uiPanel);
			}
		}

		public static BtlCut_Live2D Instantiate(BtlCut_Live2D prefab, Transform parent, Vector3 pos)
		{
			BtlCut_Live2D btlCut_Live2D = Object.Instantiate<BtlCut_Live2D>(prefab);
			btlCut_Live2D.get_transform().set_parent(parent);
			btlCut_Live2D.get_transform().localPositionZero();
			btlCut_Live2D.get_transform().localScaleOne();
			return btlCut_Live2D;
		}

		private void OnDestroy()
		{
			this._uiShipCharacter = null;
			this._uiPanel = null;
		}

		public BtlCut_Live2D ChangeMotion(Live2DModel.MotionType iType)
		{
			Live2DModel instance = SingletonMonoBehaviour<Live2DModel>.Instance;
			instance.ChangeMotion(iType);
			return this;
		}

		public BtlCut_Live2D Play()
		{
			Live2DModel instance = SingletonMonoBehaviour<Live2DModel>.Instance;
			instance.PlayOnce(Live2DModel.MotionType.Battle, null);
			return this;
		}

		public BtlCut_Live2D PlayShipVoice(int nVoiceNum)
		{
			Live2DModel instance = SingletonMonoBehaviour<Live2DModel>.Instance;
			ShipUtils.PlayShipVoice(BattleCutManager.GetMapManager().Deck.GetFlagShip(), nVoiceNum);
			return this;
		}

		public BtlCut_Live2D Hide(Action callback)
		{
			this.shipCharacter.get_transform().LTValue(1f, 0f, Defines.FORMATION_FORMATIONLABEL_ALPHA_TIME).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				this.shipCharacter.render.alpha = x;
			}).setOnComplete(delegate
			{
				if (callback != null)
				{
					callback.Invoke();
				}
			});
			return this;
		}

		public LTDescr Show()
		{
			this.panel.get_transform().LTValue(0f, 1f, 0.2f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				this.panel.alpha = x;
			});
			return this.shipCharacter.get_transform().LTMoveLocal(new Vector3(-200f, this.shipCharacter.get_transform().get_localPosition().y, 0f), 0.2f).setEase(LeanTweenType.easeOutQuint);
		}

		public LTDescr ShowLive2D()
		{
			this.panel.get_transform().LTValue(0f, 1f, 0.2f).setEase(LeanTweenType.easeOutCubic).setOnUpdate(delegate(float x)
			{
				this.panel.alpha = x;
			});
			return this.shipCharacter.get_transform().LTMoveLocal(new Vector3(-200f, this.shipCharacter.get_transform().get_localPosition().y, 0f), 0.5f).setEase(LeanTweenType.easeOutQuint);
		}
	}
}
