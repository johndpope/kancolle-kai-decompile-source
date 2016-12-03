using DG.Tweening;
using KCV.Utils;
using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV
{
	public class Option : MonoBehaviour
	{
		public enum OptionMode
		{
			Show,
			Hide
		}

		public enum OptionMenu
		{
			BGM,
			SE,
			VOICE,
			GUIDE
		}

		private bool _Switch_Guide;

		[SerializeField]
		private UISlider _Slider_Volume_BGM;

		[SerializeField]
		private UISlider _Slider_Volume_SE;

		[SerializeField]
		private UISlider _Slider_Volume_Voice;

		[SerializeField]
		private UIButton _Button_Volume_BGM;

		[SerializeField]
		private UIButton _Button_Volume_SE;

		[SerializeField]
		private UIButton _Button_Volume_Voice;

		[SerializeField]
		private UIButton _Button_Guide;

		[SerializeField]
		private UISprite[] _sw_ball = new UISprite[2];

		[SerializeField]
		private UISprite[] _sw_base = new UISprite[2];

		[SerializeField]
		private GameObject[] _Cursor_bar = new GameObject[4];

		[SerializeField]
		private UISprite chara_arm;

		[SerializeField]
		private Transform _Guide;

		private float _Slider_Volume_BGM_temp;

		private float _Slider_Volume_SE_temp;

		private float _Slider_Volume_Voice_temp;

		private bool _AlreadyOpened;

		private Animation _bighand;

		private KeyControl mKeyController;

		private Dictionary<Option.OptionMenu, float> ARM_ANGLEMAP;

		private SettingModel _Volumes;

		private Option.OptionMenu mCurrentFocusSetting;

		private Action mOnBackListener;

		public Option()
		{
			Dictionary<Option.OptionMenu, float> dictionary = new Dictionary<Option.OptionMenu, float>();
			dictionary.Add(Option.OptionMenu.BGM, 5f);
			dictionary.Add(Option.OptionMenu.SE, 23f);
			dictionary.Add(Option.OptionMenu.VOICE, 43f);
			dictionary.Add(Option.OptionMenu.GUIDE, 65f);
			this.ARM_ANGLEMAP = dictionary;
			base..ctor();
		}

		private void OnEnable()
		{
			if (this._AlreadyOpened)
			{
				TweenAlpha tweenAlpha = TweenAlpha.Begin(base.get_gameObject(), 0.2f, 1f);
			}
		}

		private void ChangeFocus(Option.OptionMenu menu, bool needSe)
		{
			this.mCurrentFocusSetting = menu;
			switch (this.mCurrentFocusSetting)
			{
			case Option.OptionMenu.BGM:
				UISelectedObject.SelectedObjectBlink(this._Cursor_bar, 0);
				break;
			case Option.OptionMenu.SE:
				UISelectedObject.SelectedObjectBlink(this._Cursor_bar, 1);
				break;
			case Option.OptionMenu.VOICE:
				UISelectedObject.SelectedObjectBlink(this._Cursor_bar, 2);
				break;
			case Option.OptionMenu.GUIDE:
				UISelectedObject.SelectedObjectBlink(this._Cursor_bar, 3);
				break;
			}
			if (needSe)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			}
			float num = this.ARM_ANGLEMAP.get_Item(this.mCurrentFocusSetting);
			iTween.RotateTo(this.chara_arm.get_gameObject(), iTween.Hash(new object[]
			{
				"z",
				num,
				"time",
				0.25f
			}));
		}

		private void Start()
		{
			this._Volumes = new SettingModel();
			this.UpdateUIState();
			TweenAlpha tweenAlpha = TweenAlpha.Begin(base.get_gameObject(), 0.2f, 1f);
			tweenAlpha.delay = 0.2f;
			this._AlreadyOpened = true;
			EventDelegate.Add(this._Slider_Volume_BGM.onChange, delegate
			{
				this.UpdateVolumeParams();
			});
			UISlider expr_5C = this._Slider_Volume_BGM;
			expr_5C.onDragFinished = (UIProgressBar.OnDragFinished)Delegate.Combine(expr_5C.onDragFinished, delegate
			{
				this.ChangeFocus(Option.OptionMenu.BGM, false);
			});
			EventDelegate.Add(this._Slider_Volume_SE.onChange, delegate
			{
				this.UpdateVolumeParams();
			});
			UISlider expr_A0 = this._Slider_Volume_SE;
			expr_A0.onDragFinished = (UIProgressBar.OnDragFinished)Delegate.Combine(expr_A0.onDragFinished, delegate
			{
				this.ChangeFocus(Option.OptionMenu.SE, false);
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			});
			EventDelegate.Add(this._Slider_Volume_Voice.onChange, delegate
			{
				this.UpdateVolumeParams();
			});
			UISlider expr_E4 = this._Slider_Volume_Voice;
			expr_E4.onDragFinished = (UIProgressBar.OnDragFinished)Delegate.Combine(expr_E4.onDragFinished, delegate
			{
				this.ChangeFocus(Option.OptionMenu.VOICE, false);
				this.PlayVoiceCheck();
			});
			this.ChangeFocus(Option.OptionMenu.BGM, false);
			this._bighand = base.GetComponent<Animation>();
		}

		private void UpdateUIState()
		{
			this._Slider_Volume_BGM.value = (float)this._Volumes.VolumeBGM / 100f;
			this._Slider_Volume_SE.value = (float)this._Volumes.VolumeSE / 100f;
			this._Slider_Volume_Voice.value = (float)this._Volumes.VolumeVoice / 100f;
			this._Switch_Guide = this._Volumes.GuideDisplay;
			this._Slider_Volume_BGM_temp = this._Slider_Volume_BGM.value;
			this._Slider_Volume_SE_temp = this._Slider_Volume_SE.value;
			this._Slider_Volume_Voice_temp = this._Slider_Volume_Voice.value;
			this.togggleSW(false, 1);
		}

		public void togggleSW(bool change_sw, int sw_ch)
		{
			if (change_sw)
			{
				if (sw_ch != 0)
				{
					if (sw_ch == 1)
					{
						if (!this._Switch_Guide)
						{
							this._sw_ball[sw_ch].get_gameObject().MoveTo(new Vector3(52f, -5f, 0f), 0.3f, true);
							this._sw_ball[sw_ch].spriteName = "switch_on_pin";
							this._sw_base[sw_ch].spriteName = "switch_on";
							this._Button_Guide.normalSprite = "switch_on";
							this._Switch_Guide = true;
							this._Guide.localScaleOne();
						}
						else
						{
							this._sw_ball[sw_ch].get_gameObject().MoveTo(new Vector3(-52f, -5f, 0f), 0.3f, true);
							this._sw_ball[sw_ch].spriteName = "switch_off_pin";
							this._sw_base[sw_ch].spriteName = "switch_off";
							this._Button_Guide.normalSprite = "switch_off";
							this._Switch_Guide = false;
							this._Guide.localScaleZero();
						}
					}
				}
			}
			else
			{
				base.get_gameObject().GetComponent<UIPanel>().set_enabled(false);
				if (sw_ch != 0)
				{
					if (sw_ch == 1)
					{
						if (this._Switch_Guide)
						{
							this._sw_ball[sw_ch].get_gameObject().get_transform().set_localPosition(new Vector3(52f, -5f, 0f));
							this._sw_ball[sw_ch].spriteName = "switch_on_pin";
							this._sw_base[sw_ch].spriteName = "switch_on";
							this._Button_Guide.normalSprite = "switch_on";
							this._Guide.get_transform().localScaleOne();
						}
						else
						{
							this._sw_ball[sw_ch].get_gameObject().get_transform().set_localPosition(new Vector3(-52f, -5f, 0f));
							this._sw_ball[sw_ch].spriteName = "switch_off_pin";
							this._sw_base[sw_ch].spriteName = "switch_off";
							this._Button_Guide.normalSprite = "switch_off";
							this._Guide.localScaleZero();
						}
					}
				}
				base.get_gameObject().GetComponent<UIPanel>().set_enabled(true);
			}
			if (change_sw)
			{
				SoundUtils.PlaySE(SEFIleInfos.MainMenuOnClick);
			}
		}

		private void UpdateVolumeParams()
		{
			if (this._Slider_Volume_BGM.value < 0.002f)
			{
				this._Slider_Volume_BGM.value = 0f;
				this._Button_Volume_BGM.normalSprite = "speaker_off";
			}
			else
			{
				this._Button_Volume_BGM.normalSprite = "speaker_on";
			}
			if (this._Slider_Volume_SE.value < 0.002f)
			{
				this._Slider_Volume_SE.value = 0f;
				this._Button_Volume_SE.normalSprite = "speaker_off";
			}
			else
			{
				this._Button_Volume_SE.normalSprite = "speaker_on";
			}
			if (this._Slider_Volume_Voice.value < 0.002f)
			{
				this._Slider_Volume_Voice.value = 0f;
				this._Button_Volume_Voice.normalSprite = "speaker_off";
			}
			else
			{
				this._Button_Volume_Voice.normalSprite = "speaker_on";
			}
			SingletonMonoBehaviour<SoundManager>.Instance.soundVolume.BGM = this._Slider_Volume_BGM.value;
			SingletonMonoBehaviour<SoundManager>.Instance.soundVolume.SE = this._Slider_Volume_SE.value;
			SingletonMonoBehaviour<SoundManager>.Instance.soundVolume.Voice = this._Slider_Volume_Voice.value;
			SingletonMonoBehaviour<SoundManager>.Instance.rawBGMVolume = this._Slider_Volume_BGM.value;
			this._Volumes.VolumeBGM = (int)(this._Slider_Volume_BGM.value * 100f);
			this._Volumes.VolumeSE = (int)(this._Slider_Volume_SE.value * 100f);
			if (this._Volumes.VolumeVoice != (int)(this._Slider_Volume_Voice.value * 100f))
			{
				this._Volumes.VolumeVoice = (int)(this._Slider_Volume_Voice.value * 100f);
			}
			this._Volumes.GuideDisplay = this._Switch_Guide;
		}

		public void SetKeyController(KeyControl keyController)
		{
			bool flag = keyController == null;
			if (flag)
			{
				bool flag2 = this.mKeyController != null;
				if (flag2)
				{
					this.mKeyController.reset(0, 0, 0.4f, 0.1f);
				}
				this.mKeyController = null;
			}
			else
			{
				this.mKeyController = keyController;
				this.mKeyController.reset(0, 3, 0.4f, 0.1f);
				this.mKeyController.setChangeValue(-1f, 0f, 1f, 0f);
			}
		}

		public void SetOnBackListener(Action onBackListener)
		{
			this.mOnBackListener = onBackListener;
		}

		private void OnBack()
		{
			if (this.mOnBackListener != null)
			{
				this.mOnBackListener.Invoke();
			}
		}

		private void Update()
		{
			if (this.mKeyController != null)
			{
				if (this.mKeyController.keyState.get_Item(1).down)
				{
					this.OnPressDownButtonCircle();
					this.UpdateVolumeParams();
				}
				else if (this.mKeyController.keyState.get_Item(0).down)
				{
					this.OnPressDownButtonCross();
					this.UpdateVolumeParams();
				}
				else if (this.mKeyController.keyState.get_Item(8).down)
				{
					this.OnPressDownButtonUp();
				}
				else if (this.mKeyController.keyState.get_Item(12).down)
				{
					this.OnPressDownButtonDown();
				}
				else if (this.mKeyController.keyState.get_Item(10).down)
				{
					this.OnPressDownButtonRight();
					this.UpdateVolumeParams();
				}
				else if (this.mKeyController.keyState.get_Item(10).up)
				{
					this.OnPressUpButtonRight();
					this.UpdateVolumeParams();
				}
				else if (this.mKeyController.keyState.get_Item(14).down)
				{
					this.OnPressDownButtonLeft();
					this.UpdateVolumeParams();
				}
				else if (this.mKeyController.keyState.get_Item(14).up)
				{
					this.OnPressUpButtonLeft();
					this.UpdateVolumeParams();
				}
			}
		}

		private void OnPressUpButtonLeft()
		{
			if (this.mCurrentFocusSetting == Option.OptionMenu.VOICE)
			{
				this.PlayVoiceCheck();
			}
		}

		private void ChangeValueEffect()
		{
			DOTween.Kill(this, false);
			TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.SetId<Tweener>(TweenSettingsExtensions.OnComplete<Tweener>(ShortcutExtensions.DOScale(this.chara_arm.get_transform(), new Vector3(1.1f, 1.1f), 0.1f), delegate
			{
				this.chara_arm.get_transform().set_localScale(Vector3.get_one());
			}), this), 1);
			SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
		}

		private void OnPressDownButtonLeft()
		{
			switch (this.mCurrentFocusSetting)
			{
			case Option.OptionMenu.BGM:
				if (this._Slider_Volume_BGM.value - 0.05f < 0f)
				{
					this._Slider_Volume_BGM.value = 0f;
				}
				else
				{
					this._Slider_Volume_BGM.value -= 0.05f;
					this.ChangeValueEffect();
				}
				break;
			case Option.OptionMenu.SE:
				if (this._Slider_Volume_SE.value - 0.05f < 0f)
				{
					this._Slider_Volume_SE.value = 0f;
				}
				else
				{
					this._Slider_Volume_SE.value -= 0.05f;
					this.ChangeValueEffect();
				}
				break;
			case Option.OptionMenu.VOICE:
				if (this._Slider_Volume_Voice.value - 0.05f < 0f)
				{
					this._Slider_Volume_Voice.value = 0f;
				}
				else
				{
					this._Slider_Volume_Voice.value -= 0.05f;
					this.ChangeValueEffect();
				}
				break;
			}
		}

		private void OnPressUpButtonRight()
		{
			if (this.mCurrentFocusSetting == Option.OptionMenu.VOICE)
			{
				this.PlayVoiceCheck();
			}
		}

		private void OnPressDownButtonRight()
		{
			switch (this.mCurrentFocusSetting)
			{
			case Option.OptionMenu.BGM:
				if (this._Slider_Volume_BGM.value + 0.05f > 1f)
				{
					this._Slider_Volume_BGM.value = 1f;
				}
				else
				{
					this._Slider_Volume_BGM.value += 0.05f;
					this.ChangeValueEffect();
				}
				break;
			case Option.OptionMenu.SE:
				if (this._Slider_Volume_SE.value + 0.05f > 1f)
				{
					this._Slider_Volume_SE.value = 1f;
				}
				else
				{
					this._Slider_Volume_SE.value += 0.05f;
					this.ChangeValueEffect();
				}
				break;
			case Option.OptionMenu.VOICE:
				if (this._Slider_Volume_Voice.value + 0.05f > 1f)
				{
					this._Slider_Volume_Voice.value = 1f;
				}
				else
				{
					this._Slider_Volume_Voice.value += 0.05f;
					this.ChangeValueEffect();
				}
				break;
			}
		}

		private void OnPressDownButtonUp()
		{
			switch (this.mCurrentFocusSetting)
			{
			case Option.OptionMenu.SE:
				this.ChangeFocus(Option.OptionMenu.BGM, true);
				break;
			case Option.OptionMenu.VOICE:
				this.ChangeFocus(Option.OptionMenu.SE, true);
				break;
			case Option.OptionMenu.GUIDE:
				this.ChangeFocus(Option.OptionMenu.VOICE, true);
				break;
			}
		}

		private void OnPressDownButtonDown()
		{
			switch (this.mCurrentFocusSetting)
			{
			case Option.OptionMenu.BGM:
				this.ChangeFocus(Option.OptionMenu.SE, true);
				break;
			case Option.OptionMenu.SE:
				this.ChangeFocus(Option.OptionMenu.VOICE, true);
				break;
			case Option.OptionMenu.VOICE:
				this.ChangeFocus(Option.OptionMenu.GUIDE, true);
				break;
			}
		}

		private void OnPressDownButtonCross()
		{
			this.bighand(0.1f);
			this.Pressed_Button_close();
		}

		private void OnPressDownButtonCircle()
		{
			this.bighand(0.1f);
			switch (this.mCurrentFocusSetting)
			{
			case Option.OptionMenu.BGM:
				this.Pressed_Button_Volume_BGM();
				break;
			case Option.OptionMenu.SE:
				this.Pressed_Button_Volume_SE();
				break;
			case Option.OptionMenu.VOICE:
				this.Pressed_Button_Volume_Voice();
				break;
			case Option.OptionMenu.GUIDE:
				this.togggleSW(true, 1);
				break;
			}
		}

		private void PlayVoiceCheck()
		{
			ShipUtils.PlayShipVoice(SingletonMonoBehaviour<AppInformation>.Instance.FlagShipModel, 8);
		}

		private void bighand(float time)
		{
			this._bighand.Stop();
			this._bighand.Play();
		}

		public void Pressed_Button_Volume_BGM()
		{
			this.ChangeFocus(Option.OptionMenu.BGM, true);
			if (this._Slider_Volume_BGM.value != 0f)
			{
				this._Slider_Volume_BGM_temp = this._Slider_Volume_BGM.value;
				this._Slider_Volume_BGM.value = 0f;
			}
			else
			{
				this._Slider_Volume_BGM.value = this._Slider_Volume_BGM_temp;
			}
		}

		public void Pressed_Button_Volume_SE()
		{
			this.ChangeFocus(Option.OptionMenu.SE, true);
			if (this._Slider_Volume_SE.value != 0f)
			{
				this._Slider_Volume_SE_temp = this._Slider_Volume_SE.value;
				this._Slider_Volume_SE.value = 0f;
			}
			else
			{
				this._Slider_Volume_SE.value = this._Slider_Volume_SE_temp;
			}
		}

		public void Pressed_Button_Volume_Voice()
		{
			this.ChangeFocus(Option.OptionMenu.VOICE, true);
			if (this._Slider_Volume_Voice.value != 0f)
			{
				this._Slider_Volume_Voice_temp = this._Slider_Volume_Voice.value;
				this._Slider_Volume_Voice.value = 0f;
			}
			else
			{
				this._Slider_Volume_Voice.value = this._Slider_Volume_Voice_temp;
			}
		}

		public void Pressed_Button_Guide()
		{
			this.ChangeFocus(Option.OptionMenu.GUIDE, true);
			this.bighand(0.1f);
			this.togggleSW(true, 1);
		}

		public void Pressed_Button_voice_time()
		{
		}

		public void Pressed_Button_close()
		{
			this._onClickOverlayButton();
		}

		public void _onClickOverlayButton()
		{
			this._Volumes.Save();
			this.OnBack();
		}

		private void OnDestroy()
		{
			this._Slider_Volume_BGM = null;
			this._Slider_Volume_SE = null;
			this._Slider_Volume_Voice = null;
			this._Button_Volume_BGM = null;
			this._Button_Volume_SE = null;
			this._Button_Volume_Voice = null;
			this._Button_Guide = null;
			this._sw_ball = null;
			this._sw_base = null;
			this._Cursor_bar = null;
			this.chara_arm = null;
			this._Guide = null;
			this._bighand = null;
			this.mKeyController = null;
			this.ARM_ANGLEMAP = null;
			this._Volumes = null;
		}
	}
}
