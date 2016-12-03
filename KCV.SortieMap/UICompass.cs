using KCV.Utils;
using LT.Tweening;
using System;
using UnityEngine;

namespace KCV.SortieMap
{
	[RequireComponent(typeof(UIWidget))]
	public class UICompass : MonoBehaviour
	{
		public enum Power
		{
			Low,
			High
		}

		[SerializeField]
		private UISprite _uiBase;

		private UIWidget _uiWidget;

		private Action _actOnStopCompass;

		private float _fCompassPoint;

		private UIWidget widget
		{
			get
			{
				return this.GetComponentThis(ref this._uiWidget);
			}
		}

		private float targetCompassPoint
		{
			get
			{
				return Mathe.Euler2Deg(this._fCompassPoint);
			}
		}

		private float nowCompassPoint
		{
			get
			{
				return Mathe.Euler2Deg(this._uiBase.get_transform().get_localRotation().get_eulerAngles().z);
			}
		}

		private void OnDestroy()
		{
			Mem.Del<UIWidget>(ref this._uiWidget);
			Mem.Del<Action>(ref this._actOnStopCompass);
			Mem.Del(ref this._uiBase);
		}

		public bool Init(Action onStop, Transform from, Transform to)
		{
			this._actOnStopCompass = onStop;
			this.widget.alpha = 0f;
			base.get_transform().set_localScale(Vector3.get_one());
			this._uiBase.get_transform().set_rotation(Quaternion.get_identity());
			this._fCompassPoint = this.CalcTargetCompassPoint(from.get_position(), to.get_position());
			return true;
		}

		private float CalcTargetCompassPoint(Vector3 from, Vector3 to)
		{
			return Mathe.Rad2Deg(Mathf.Atan2(to.y - from.y, to.x - from.x));
		}

		public LTDescr Show()
		{
			base.get_transform().set_localScale(Vector3.get_one() * 1.1f);
			base.get_transform().localPositionY(50f);
			this.widget.get_transform().LTValue(this.widget.alpha, 1f, 0.5f).setEase(LeanTweenType.easeOutQuad).setOnUpdate(delegate(float x)
			{
				this.widget.alpha = x;
			});
			this.widget.get_transform().LTMoveLocalY(0f, 0.5f).setEase(LeanTweenType.easeInCubic);
			return this.widget.get_transform().LTScale(Vector3.get_one(), 0.5f);
		}

		public LTDescr Hide()
		{
			this.widget.get_transform().LTValue(this.widget.alpha, 0f, 0.5f).setEase(LeanTweenType.easeOutQuad).setOnUpdate(delegate(float x)
			{
				this.widget.alpha = x;
			});
			this.widget.get_transform().LTMoveLocalY(50f, 0.5f).setEase(LeanTweenType.easeOutCubic);
			return this.widget.get_transform().LTScale(Vector3.get_one() * 1.1f, 0.5f).setEase(LeanTweenType.easeOutQuad);
		}

		public void StopRoll(UICompass.Power iPower)
		{
			SoundUtils.PlaySE(SEFIleInfos.SE_031);
			if (iPower != UICompass.Power.Low)
			{
				if (iPower == UICompass.Power.High)
				{
					this._uiBase.get_transform().LTRotateAroundLocal(Vector3.get_back(), this.targetCompassPoint - this.nowCompassPoint + (float)(360 * XorRandom.GetILim(3, 4)) - 90f, 2f).setEase(LeanTweenType.easeOutElastic).setOnComplete(delegate
					{
						Dlg.Call(ref this._actOnStopCompass);
					});
				}
			}
			else
			{
				this._uiBase.get_transform().LTRotateAroundLocal(Vector3.get_back(), this.targetCompassPoint - this.nowCompassPoint + (float)(360 * XorRandom.GetILim(2, 3)) - 90f, 2f).setEase(LeanTweenType.easeOutQuad).setOnComplete(delegate
				{
					Dlg.Call(ref this._actOnStopCompass);
				});
			}
		}
	}
}
