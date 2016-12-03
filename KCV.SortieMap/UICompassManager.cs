using Common.Enum;
using LT.Tweening;
using System;
using UnityEngine;

namespace KCV.SortieMap
{
	public class UICompassManager : MonoBehaviour
	{
		[Serializable]
		private class CompassGirlMessage
		{
			[SerializeField]
			private Transform _tra;

			[SerializeField]
			private UISprite _uiBackground;

			[SerializeField]
			private UILabel _uiMessage;

			private UIWidget _uiWidget;

			public Transform transform
			{
				get
				{
					return this._tra;
				}
			}

			public UIWidget widget
			{
				get
				{
					if (this._uiWidget == null)
					{
						this._uiWidget = this.transform.GetComponent<UIWidget>();
					}
					return this._uiWidget;
				}
			}

			private CompassGirlMessage()
			{
			}

			public bool Init()
			{
				this.widget.alpha = 0f;
				this._uiMessage.text = string.Empty;
				return true;
			}

			public bool UnInit()
			{
				Mem.Del<Transform>(ref this._tra);
				Mem.Del(ref this._uiBackground);
				Mem.Del<UILabel>(ref this._uiMessage);
				Mem.Del<UIWidget>(ref this._uiWidget);
				return true;
			}

			public void ClearMessage()
			{
				this._uiMessage.text = string.Empty;
			}

			public void UpdateMessage(string message)
			{
				this._uiMessage.text = message;
			}

			public LTDescr Show()
			{
				return this.transform.LTValue(this.widget.alpha, 1f, 0.5f).setEase(LeanTweenType.easeOutQuad).setOnUpdate(delegate(float x)
				{
					this.widget.alpha = x;
				});
			}

			public LTDescr Hide()
			{
				return this.transform.LTValue(this.widget.alpha, 0f, 0.3f).setEase(LeanTweenType.easeOutQuad).setOnUpdate(delegate(float x)
				{
					this.widget.alpha = x;
				});
			}
		}

		[SerializeField]
		private UITexture _uiBackground;

		[SerializeField]
		private UICompass _uiCompass;

		[SerializeField]
		private UICompassGirl _uiCompassGirl;

		[SerializeField]
		private UICompassManager.CompassGirlMessage _clsCompassGirlMessage;

		[SerializeField]
		private UIInvisibleCollider _uiInvisibleCollider;

		private Action _actOnFinished;

		public static UICompassManager Instantiate(UICompassManager prefab, Transform parent, CompassType iCompassType, Transform ship, Transform nextCell)
		{
			UICompassManager uICompassManager = Object.Instantiate<UICompassManager>(prefab);
			uICompassManager.get_transform().set_parent(parent);
			uICompassManager.get_transform().localPositionZero();
			uICompassManager.get_transform().localScaleOne();
			uICompassManager.Init(iCompassType, ship, nextCell);
			return uICompassManager;
		}

		private bool Init(CompassType iCompassType, Transform ship, Transform nextCell)
		{
			this._uiBackground.alpha = 0f;
			this._uiCompassGirl.Init(new Action<string>(this.OnUpdateMessage), new Action<UICompass.Power>(this.OnStopRollCompass), iCompassType);
			this._clsCompassGirlMessage.Init();
			this._uiCompass.Init(new Action(this.OnStoppedCompass), ship, nextCell);
			this._uiInvisibleCollider.SetOnTouch(delegate
			{
			});
			return true;
		}

		private void OnDestroy()
		{
			Mem.Del<UICompassGirl>(ref this._uiCompassGirl);
			Mem.Del<UICompass>(ref this._uiCompass);
			Mem.Del<UITexture>(ref this._uiBackground);
			this._clsCompassGirlMessage.UnInit();
			Mem.Del<UICompassManager.CompassGirlMessage>(ref this._clsCompassGirlMessage);
			Mem.Del<Action>(ref this._actOnFinished);
		}

		public void Play(Action onFinished)
		{
			this._actOnFinished = onFinished;
			this._uiBackground.get_transform().LTValue(0f, 0.5f, 0.3f).setEase(LeanTweenType.easeOutQuad).setOnUpdate(delegate(float x)
			{
				this._uiBackground.alpha = x;
			});
			this._uiCompass.Show().setOnComplete(new Action(this.OnShowCompass));
		}

		private void OnShowCompass()
		{
			this._clsCompassGirlMessage.Show();
			this._uiCompassGirl.Play();
		}

		private void OnStoppedCompass()
		{
			this._uiBackground.get_transform().LTValue(this._uiBackground.alpha, 0f, 0.3f).setEase(LeanTweenType.easeOutQuad).setOnUpdate(delegate(float x)
			{
				this._uiBackground.alpha = x;
			});
			this._clsCompassGirlMessage.Hide();
			this._uiCompassGirl.Hide();
			this._uiCompass.Hide().setOnComplete(new Action(this.OnFinished));
		}

		private void OnUpdateMessage(string message)
		{
			this._clsCompassGirlMessage.UpdateMessage(message);
		}

		private void OnStopRollCompass(UICompass.Power power)
		{
			this._uiCompass.StopRoll(power);
		}

		private void OnFinished()
		{
			Dlg.Call(ref this._actOnFinished);
		}
	}
}
