using Common.Enum;
using KCV.Utils;
using Server_Common;
using Server_Common.Formats;
using Server_Controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Strategy
{
	public class ProdEndPhase : MonoBehaviour
	{
		[SerializeField]
		private GameObject _crearTxtObj;

		[SerializeField]
		private UITexture _gameoverTxt1;

		[SerializeField]
		private UITexture _gameoverTxt2;

		[SerializeField]
		private UILabel _historyLabel;

		[SerializeField]
		private ParticleSystem _petalPar;

		[SerializeField]
		private Animation _animation;

		private bool _isAnimation;

		private bool _isHistoryAnime;

		private bool _isClear;

		private bool _isTurnOver;

		private Action _callback;

		private KeyControl _keyController;

		private List<UILabel> _lstHistoryLabel;

		private bool _init()
		{
			if (this._crearTxtObj == null)
			{
				this._crearTxtObj = base.get_transform().FindChild("ClearText").get_gameObject();
			}
			Util.FindParentToChild<UITexture>(ref this._gameoverTxt1, base.get_transform(), "GameOver1");
			Util.FindParentToChild<UITexture>(ref this._gameoverTxt2, this._gameoverTxt1.get_transform(), "GameOver2");
			Util.FindParentToChild<UILabel>(ref this._historyLabel, base.get_transform(), "HistoryLabel");
			Util.FindParentToChild<ParticleSystem>(ref this._petalPar, base.get_transform(), "Petal");
			if (this._animation == null)
			{
				this._animation = base.GetComponent<Animation>();
			}
			this._crearTxtObj.SetActive(false);
			this._gameoverTxt1.SetActive(false);
			this._animation.Stop();
			this._petalPar.Stop();
			this._isAnimation = false;
			this._isHistoryAnime = false;
			this._isClear = false;
			this._isTurnOver = false;
			this._callback = null;
			this._lstHistoryLabel = new List<UILabel>();
			return true;
		}

		private void OnDestroy()
		{
			Mem.Del<GameObject>(ref this._crearTxtObj);
			Mem.Del<UILabel>(ref this._historyLabel);
			Mem.Del(ref this._petalPar);
			Mem.Del<Animation>(ref this._animation);
			if (this._lstHistoryLabel != null)
			{
				for (int i = 0; i < this._lstHistoryLabel.get_Count(); i++)
				{
					Object.Destroy(this._lstHistoryLabel.get_Item(i).get_gameObject());
				}
				this._lstHistoryLabel.Clear();
			}
			this._lstHistoryLabel = null;
			this._keyController = null;
			App.TimeScale(1f);
		}

		private void Update()
		{
			if (!this._isAnimation || this._keyController == null)
			{
				return;
			}
			this._keyController.Update();
			if (this._isHistoryAnime)
			{
				if (this._keyController.GetDown(KeyControl.KeyName.MARU))
				{
					App.TimeScale(3f);
				}
				else if (this._keyController.GetUp(KeyControl.KeyName.MARU))
				{
					App.TimeScale(1f);
				}
				else if (this._keyController.GetDown(KeyControl.KeyName.BATU))
				{
				}
			}
		}

		public void Play(Action callback)
		{
			this._isAnimation = true;
			this._callback = callback;
			this._setEndTxt();
			this._createHistoryLabel();
			this._animation.Stop();
			this._animation.Play((!this._isClear) ? "EndStartGO" : "EndStart");
			if (this._isClear)
			{
				SoundUtils.PlaySE(SEFIleInfos.ClearAAA, delegate
				{
					SoundUtils.PlayBGM((BGMFileInfos)211, true);
				});
			}
		}

		private void _setEndTxt()
		{
			if (this._isClear)
			{
				this._crearTxtObj.SetActive(true);
			}
			else
			{
				this._gameoverTxt1.SetActive(true);
			}
		}

		private void _createHistoryLabel()
		{
			if (Utils.IsGameOver())
			{
				if (this._isTurnOver)
				{
					this._gameoverTxt1.mainTexture = (Resources.Load("Textures/Ending/txt_gamoverB1") as Texture2D);
					this._gameoverTxt2.mainTexture = (Resources.Load("Textures/Ending/txt_gamoverB2") as Texture2D);
				}
				else
				{
					this._gameoverTxt1.mainTexture = (Resources.Load("Textures/Ending/txt_gamoverA1") as Texture2D);
					this._gameoverTxt2.mainTexture = (Resources.Load("Textures/Ending/txt_gamoverA2") as Texture2D);
				}
				this._gameoverTxt1.MakePixelPerfect();
				this._gameoverTxt2.MakePixelPerfect();
			}
			Api_Result<List<User_HistoryFmt>> api_Result = new Api_get_Member().HistoryList();
			for (int i = 0; i < api_Result.data.get_Count(); i++)
			{
				User_HistoryFmt user_HistoryFmt = api_Result.data.get_Item(i);
				if (user_HistoryFmt != null)
				{
					UILabel uILabel = Object.Instantiate<UILabel>(this._historyLabel);
					uILabel.get_transform().set_parent(this._historyLabel.get_transform().get_parent());
					uILabel.get_transform().set_localPosition(this._historyLabel.get_transform().get_localPosition());
					uILabel.get_transform().set_localScale(Vector3.get_one());
					uILabel.text = this._setHistoryType(user_HistoryFmt);
					uILabel.MakePixelPerfect();
					this._lstHistoryLabel.Add(uILabel);
					if (user_HistoryFmt.Type == HistoryType.GameOverLost)
					{
						UILabel uILabel2 = Object.Instantiate<UILabel>(this._historyLabel);
						uILabel2.get_transform().set_parent(this._historyLabel.get_transform().get_parent());
						uILabel2.get_transform().set_localPosition(this._historyLabel.get_transform().get_localPosition());
						uILabel2.get_transform().set_localScale(Vector3.get_one());
						uILabel2.text = "\u3000\u3000\u3000\u3000\u3000\u3000 敗戦";
						uILabel2.MakePixelPerfect();
						this._lstHistoryLabel.Add(uILabel2);
					}
				}
			}
		}

		private string _setHistoryType(User_HistoryFmt fmt)
		{
			string result = string.Empty;
			string text = string.Empty;
			switch (fmt.Type)
			{
			case HistoryType.MapClear1:
				result = this._setHistory(string.Empty, fmt.DateString.Year, fmt.DateString.Month, fmt.DateString.Day, fmt.MapInfo.Name, fmt.MapInfo.Opetext, fmt.FlagShip.Name);
				break;
			case HistoryType.MapClear2:
				result = this._setHistory("第二次", fmt.DateString.Year, fmt.DateString.Month, fmt.DateString.Day, fmt.MapInfo.Name, fmt.MapInfo.Opetext, fmt.FlagShip.Name);
				break;
			case HistoryType.MapClear3:
				result = this._setHistory("第三次", fmt.DateString.Year, fmt.DateString.Month, fmt.DateString.Day, fmt.MapInfo.Name, fmt.MapInfo.Opetext, fmt.FlagShip.Name);
				break;
			case HistoryType.TankerLostHalf:
				text = this._setHistoryDate(fmt);
				result = text + fmt.AreaName + " 輸送船団遭難";
				break;
			case HistoryType.TankerLostAll:
				text = this._setHistoryDate(fmt);
				result = text + fmt.AreaName + " 輸送船団全滅";
				break;
			case HistoryType.NewAreaOpen:
				text = this._setHistoryDate(fmt);
				result = text + fmt.AreaName + " 攻略開始";
				break;
			case HistoryType.GameOverLost:
				text = this._setHistoryDate(fmt);
				result = text + "本土沖海戦 敗北（艦隊壊滅）";
				break;
			case HistoryType.GameOverTurn:
				text = this._setHistoryDate(fmt);
				result = text + "終戦";
				break;
			case HistoryType.GameClear:
				text = this._setHistoryDate(fmt);
				result = text + "勝利";
				break;
			}
			return result;
		}

		private string _setHistory(string num, string year, string Month, string day, string areaName, string MapInfo, string shipName)
		{
			return string.Concat(new string[]
			{
				year,
				"年",
				Month,
				day,
				"日 ",
				areaName,
				" ",
				num,
				MapInfo,
				" 旗艦 ",
				shipName
			});
		}

		private string _setHistoryDate(User_HistoryFmt fmt)
		{
			return string.Concat(new string[]
			{
				fmt.DateString.Year,
				"年",
				fmt.DateString.Month,
				string.Empty,
				fmt.DateString.Day,
				"日 "
			});
		}

		private void _setHistoryMove(Transform trans, float deray, bool last)
		{
			float num = 300f;
			Hashtable hashtable = new Hashtable();
			hashtable.Add("x", trans.get_localPosition().x);
			hashtable.Add("y", num);
			hashtable.Add("isLocal", true);
			hashtable.Add("delay", deray);
			hashtable.Add("time", 7f);
			hashtable.Add("easeType", iTween.EaseType.linear);
			hashtable.Add("oncomplete", (!last) ? string.Empty : "_onCompHistoryTxt");
			hashtable.Add("oncompletetarget", base.get_gameObject());
			trans.MoveTo(hashtable);
		}

		private void _compEndStart()
		{
			this._compDecisionTxt();
		}

		private void _compDecisionTxt()
		{
			this._animation.Stop();
			this._animation.Play("EndTextMove");
		}

		private void _compEndTextMove()
		{
			if (this._isClear)
			{
				this._petalPar.Play();
			}
			float num = 270f + (float)this._historyLabel.height + 10f;
			this._isHistoryAnime = true;
			if (this._lstHistoryLabel.get_Count() == 0)
			{
				this._onCompHistoryTxt();
			}
			for (int i = 0; i < this._lstHistoryLabel.get_Count(); i++)
			{
				this._setHistoryMove(this._lstHistoryLabel.get_Item(i).get_transform(), 0.8f * (float)i, i >= this._lstHistoryLabel.get_Count() - 1);
			}
		}

		private void _onCompHistoryTxt()
		{
			if (this._isClear)
			{
				this._petalPar.Stop();
			}
			this._animation.Stop();
			this._animation.Play("EndPhaseEnd");
			SoundUtils.StopFadeBGM(2f, null);
		}

		private void _onCompFadeOut()
		{
			if (this._callback != null)
			{
				this._callback.Invoke();
			}
		}

		public static ProdEndPhase Instantiate(ProdEndPhase prefab, Transform parent, KeyControl keyController, bool clear, bool isTurnOver)
		{
			ProdEndPhase prodEndPhase = Object.Instantiate<ProdEndPhase>(prefab);
			prodEndPhase.get_transform().set_parent(parent);
			prodEndPhase.get_transform().set_localScale(Vector3.get_one());
			prodEndPhase.get_transform().set_localPosition(Vector3.get_zero());
			prodEndPhase._init();
			prodEndPhase._keyController = keyController;
			prodEndPhase._isClear = clear;
			prodEndPhase._isTurnOver = isTurnOver;
			return prodEndPhase;
		}
	}
}
