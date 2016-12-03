using KCV.Strategy;
using KCV.Utils;
using local.managers;
using local.models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.PSVita;

namespace KCV
{
	public class UIFlagShip : BaseShipTexture
	{
		private const float SMOOTHING_VAL = 80f;

		private ManagerBase _clsBase;

		private int _clicked_count;

		private long _last_clicked_time;

		private bool _isPortTop;

		private bool _isPlayTimeSignalVoice;

		private Animation TouchAnimation;

		private List<string> AnimationList;

		private int BackRubCount;

		private TweenScale ApproachScale;

		private TweenPosition ApproachPosition;

		private bool isNear;

		private bool isPlayed;

		public bool isEnableBackTouch;

		private Action<bool, bool, bool> OnBackTouch;

		private float[] StartPosY;

		private float[] StartTime;

		[SerializeField]
		private UITexture HeadArea;

		public bool debugBackSlash;

		[Button("PlayBackTouch", "PlayBackTouch", new object[]
		{

		})]
		public int button11;

		[Button("PlayApproach", "PlayApproach", new object[]
		{

		})]
		public int button22;

		private ShipModel shipModel
		{
			get
			{
				return (ShipModel)this._clsIShipModel;
			}
		}

		private void OnDestroy()
		{
			this.HeadArea = null;
			this._clsBase = null;
			this.TouchAnimation = null;
			this.AnimationList.Clear();
			this.AnimationList = null;
			this.ApproachScale = null;
			this.ApproachPosition = null;
			this.StartPosY = null;
			this.StartTime = null;
			PSVitaInput.set_secondaryTouchIsScreenSpace(false);
		}

		private void Awake()
		{
			this._clicked_count = 0;
			this._last_clicked_time = DateTime.get_Now().get_Ticks();
			this._isPortTop = false;
			this._isPlayTimeSignalVoice = false;
			this.TouchAnimation = base.GetComponent<Animation>();
			this.AnimationList = new List<string>();
			using (IEnumerator enumerator = this.TouchAnimation.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					AnimationState animationState = (AnimationState)enumerator.get_Current();
					this.AnimationList.Add(animationState.get_name());
				}
			}
			this.ApproachScale = base.get_transform().GetChild(0).GetComponent<TweenScale>();
			this.ApproachPosition = base.get_transform().GetChild(0).GetComponent<TweenPosition>();
			this.StartPosY = new float[]
			{
				-1f,
				-1f,
				-1f,
				-1f
			};
			float realtimeSinceStartup = Time.get_realtimeSinceStartup();
			this.StartTime = new float[]
			{
				realtimeSinceStartup,
				realtimeSinceStartup,
				realtimeSinceStartup,
				realtimeSinceStartup
			};
		}

		public bool SetData(ShipModel ship, ManagerBase manager)
		{
			if (ship != null)
			{
				this._clsIShipModel = ship;
				int arg_26_0 = (!this.shipModel.IsDamaged()) ? 9 : 10;
			}
			if (manager is PortManager)
			{
				this._isPortTop = true;
			}
			else
			{
				this._isPortTop = false;
			}
			return true;
		}

		private void Update()
		{
			if (SingletonMonoBehaviour<Live2DModel>.exist() && !SingletonMonoBehaviour<Live2DModel>.Instance.isLive2DModel)
			{
				this._uiShipTex.get_transform().set_localScale(this.Smoothing());
			}
			if (this.isEnableBackTouch && !SingletonMonoBehaviour<Live2DModel>.Instance.IsStop)
			{
				this.InputBackTouch();
			}
		}

		private Vector3 Smoothing()
		{
			return new Vector3(Vector3.get_one().x + Mathf.Sin(Time.get_time()) / 80f, Vector3.get_one().y + Mathf.Sin(Time.get_time()) / 80f, Vector3.get_one().z + Mathf.Sin(Time.get_time()) / 80f);
		}

		private bool _chkTimeSignalVoice(ShipModel model, int hour)
		{
			if (App.SystemDateTime.get_Minute() == 0 && App.SystemDateTime.get_Second() == 0)
			{
				base.StartCoroutine(this._playTimeSignalVoice(model, hour));
				return true;
			}
			return false;
		}

		[DebuggerHidden]
		private IEnumerator _playTimeSignalVoice(ShipModel model, int hour)
		{
			UIFlagShip.<_playTimeSignalVoice>c__Iterator47 <_playTimeSignalVoice>c__Iterator = new UIFlagShip.<_playTimeSignalVoice>c__Iterator47();
			<_playTimeSignalVoice>c__Iterator.model = model;
			<_playTimeSignalVoice>c__Iterator.hour = hour;
			<_playTimeSignalVoice>c__Iterator.<$>model = model;
			<_playTimeSignalVoice>c__Iterator.<$>hour = hour;
			<_playTimeSignalVoice>c__Iterator.<>f__this = this;
			return <_playTimeSignalVoice>c__Iterator;
		}

		public void PlayShipVoice(int shipID, int voiceNum)
		{
			SingletonMonoBehaviour<SoundManager>.Instance.PlayVoice(SingletonMonoBehaviour<ResourceManager>.Instance.ShipVoice.Load(shipID, voiceNum), 0);
		}

		private void UIFlagShipEL()
		{
			ShipModel flagShipModel = SingletonMonoBehaviour<AppInformation>.Instance.FlagShipModel;
			if (flagShipModel != null)
			{
				ShipUtils.PlayShipVoice(flagShipModel, this.TouchedPartnerShip(this.shipModel));
			}
		}

		public int TouchedPartnerShip(ShipModel _partnerShip)
		{
			this._clicked_count++;
			this._last_clicked_time = DateTime.get_Now().get_Ticks();
			if (this._clicked_count > 4)
			{
				return 4;
			}
			int num = new Random().Next(1, 11);
			if (this._clicked_count == 1 && _partnerShip != null && _partnerShip.IsMarriage())
			{
				return 28;
			}
			if (num <= 6)
			{
				return 2;
			}
			if (num == 7 || num == 8 || num == 9)
			{
				return 3;
			}
			return 4;
		}

		public uint GetNoActionMSec()
		{
			long num = DateTime.get_Now().get_Ticks() - this._last_clicked_time;
			return (uint)num / 10000u;
		}

		public void ResetClickedCount()
		{
			this._clicked_count = 0;
			this.BackRubCount = 0;
		}

		public int getClickedCount()
		{
			return this._clicked_count;
		}

		private void InputBackTouch()
		{
			if (StrategyShipCharacter.nowShipModel == null)
			{
				return;
			}
			PSVitaInput.set_secondaryTouchIsScreenSpace(true);
			bool flag = false;
			Touch[] touchesSecondary = PSVitaInput.get_touchesSecondary();
			for (int i = 0; i < touchesSecondary.Length; i++)
			{
				Touch touch = touchesSecondary[i];
				if (touch.get_fingerId() != -1)
				{
					if (touch.get_phase() == null)
					{
						this.StartPosY[touch.get_fingerId()] = touch.get_position().y;
						this.StartTime[touch.get_fingerId()] = Time.get_realtimeSinceStartup();
					}
					else if (touch.get_phase() == 3 && this.StartPosY[touch.get_fingerId()] != -1f && -420f > this.StartPosY[touch.get_fingerId()] - touch.get_position().y)
					{
						flag = true;
					}
					if (0.2f < Time.get_realtimeSinceStartup() - this.StartTime[touch.get_fingerId()] && this.StartPosY[touch.get_fingerId()] != -1f)
					{
						this.StartPosY[touch.get_fingerId()] = -1f;
					}
				}
			}
			if (!this.isPlayed && (flag || this.debugBackSlash))
			{
				this.PlayBackTouch();
				this.isPlayed = true;
				this.debugBackSlash = false;
			}
			if (PSVitaInput.get_touchesSecondary().Length == 0)
			{
				this.isPlayed = false;
			}
		}

		private void PlayBackTouch()
		{
			if (this.TouchAnimation.get_isPlaying())
			{
				return;
			}
			int num = (this.BackRubCount != 0) ? 4 : 3;
			this.BackRubCount++;
			ShipUtils.PlayShipVoice(StrategyShipCharacter.nowShipModel, num);
			this.PlayJumpAnimation(1);
			int lov = StrategyShipCharacter.nowShipModel.Lov;
			bool flag = false;
			bool flag2 = false;
			StrategyShipCharacter.nowShipModel.LovAction(1, num);
			if (StrategyShipCharacter.nowShipModel.Lov > lov)
			{
				flag = true;
			}
			if (StrategyShipCharacter.nowShipModel.Lov < lov)
			{
				flag2 = true;
			}
			if (this.OnBackTouch != null)
			{
				this.OnBackTouch.Invoke(flag, flag2, true);
			}
		}

		private void PlayJumpAnimation(int type)
		{
			this.TouchAnimation.Play(this.AnimationList.get_Item(type));
		}

		private void PlayApproach()
		{
			this.ApproachScale.Play(!this.isNear);
			this.ApproachPosition.Play(!this.isNear);
			this.isNear = !this.isNear;
		}

		public void SetOnBackTouchCallBack(Action<bool, bool, bool> act)
		{
			this.OnBackTouch = act;
		}
	}
}
