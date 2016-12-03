using LT.Tweening;
using System;
using UnityEngine;

namespace KCV.Title
{
	[RequireComponent(typeof(UIPanel))]
	public class UIPressAnyKey : MonoBehaviour
	{
		[Serializable]
		private struct Param
		{
			[SerializeField]
			private float _fInterval;

			[SerializeField]
			private Vector3 _vPos;

			[SerializeField]
			private AnimationCurve _acCurve;

			public float interval
			{
				get
				{
					return this._fInterval;
				}
			}

			public Vector3 pos
			{
				get
				{
					return this._vPos;
				}
			}

			public AnimationCurve curve
			{
				get
				{
					return this._acCurve;
				}
			}

			public Param(float interval, Vector3 pos, AnimationCurve curve)
			{
				this._fInterval = interval;
				this._vPos = pos;
				this._acCurve = curve;
			}
		}

		[SerializeField]
		private UISprite _uiPressAnyKey;

		[SerializeField]
		private UIPressAnyKey.Param _strParam;

		[SerializeField]
		private UIInvisibleCollider _uiInvisivleCollider;

		private bool _isPress;

		private Action _actOnPress;

		public static UIPressAnyKey Instantiate(UIPressAnyKey prefab, Transform parent, Action onPress)
		{
			UIPressAnyKey uIPressAnyKey = Object.Instantiate<UIPressAnyKey>(prefab);
			uIPressAnyKey.get_transform().set_parent(parent);
			uIPressAnyKey.get_transform().localScaleOne();
			uIPressAnyKey.get_transform().set_localPosition(uIPressAnyKey._strParam.pos);
			uIPressAnyKey.Init(onPress);
			return uIPressAnyKey;
		}

		private void OnDestroy()
		{
			Mem.Del(ref this._uiPressAnyKey);
			Mem.Del<UIPressAnyKey.Param>(ref this._strParam);
			Mem.Del<UIInvisibleCollider>(ref this._uiInvisivleCollider);
			Mem.Del<Action>(ref this._actOnPress);
		}

		private bool Init(Action onPress)
		{
			this._isPress = false;
			this._actOnPress = onPress;
			this._uiInvisivleCollider.SetOnTouch(new Action(this.OnPress));
			return true;
		}

		public bool Run()
		{
			KeyControl keyControl = TitleTaskManager.GetKeyControl();
			if (keyControl.GetDown(KeyControl.KeyName.MARU) || keyControl.GetDown(KeyControl.KeyName.BATU) || keyControl.GetDown(KeyControl.KeyName.SHIKAKU) || keyControl.GetDown(KeyControl.KeyName.SANKAKU) || keyControl.GetDown(KeyControl.KeyName.START))
			{
				this.OnPress();
				return true;
			}
			this._uiPressAnyKey.alpha = (float)Math.Abs(Math.Sin((double)(Time.get_time() * this._strParam.interval)));
			return this._isPress;
		}

		private void OnPress()
		{
			this._isPress = true;
			this._uiInvisivleCollider.collider2D.set_enabled(false);
			this.PlayPressAnyKey().setOnComplete(this._actOnPress);
		}

		private LTDescr PlayPressAnyKey()
		{
			this._uiPressAnyKey.alpha = 1f;
			return this._uiPressAnyKey.get_transform().LTValue(0f, 1f, 1.25f).setOnUpdate(delegate(float x)
			{
				this._uiPressAnyKey.alpha = x;
			}).setEase(this._strParam.curve);
		}
	}
}
