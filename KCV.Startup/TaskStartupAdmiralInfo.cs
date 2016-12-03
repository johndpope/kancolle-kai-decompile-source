using KCV.Utils;
using Sony.Vita.Dialog;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Startup
{
	public class TaskStartupAdmiralInfo : SceneTaskMono
	{
		[SerializeField]
		private UIButton _uiDecideButton;

		[SerializeField]
		private ButtonLightTexture _btnLight;

		[SerializeField]
		private UIInput _uiNameInput;

		private UIPanel _uiPanel;

		private string _strEditName = string.Empty;

		private int _starterShipNum;

		private Animation _ANI;

		private bool _shipSelected;

		private bool _shipCancelled;

		private void OnDestroy()
		{
			this._uiPanel = null;
			this._uiNameInput = null;
			this._ANI = null;
		}

		public void Setup()
		{
			this._uiDecideButton.state = UIButtonColor.State.Disabled;
			this._uiNameInput.value = StartupTaskManager.GetData().AdmiralName;
		}

		protected override bool Init()
		{
			Ime.add_OnGotIMEDialogResult(new Messages.EventHandler(this.OnGotIMEDialogResult));
			Main.Initialise();
			Utils.PlayAdmiralNameVoice();
			UIStartupNavigation navigation = StartupTaskManager.GetNavigation();
			navigation.SetNavigationInAdmiralInfo(StartupTaskManager.IsInheritStartup());
			Util.FindParentToChild<UIPanel>(ref this._uiPanel, base.scenePrefab, "InfoPanel");
			if (this._uiNameInput.onSubmit != null)
			{
				this._uiNameInput.onSubmit.Clear();
			}
			EventDelegate.Add(this._uiNameInput.onSubmit, new EventDelegate.Callback(this._onNameSubmit));
			if (this._uiNameInput.onChange != null)
			{
				this._uiNameInput.onChange.Clear();
			}
			this._uiNameInput.onChange.Add(new EventDelegate(delegate
			{
				this.ChkButtonState();
			}));
			if (this._uiDecideButton.onClick != null)
			{
				this._uiDecideButton.onClick.Clear();
			}
			this._uiDecideButton.onClick.Add(new EventDelegate(delegate
			{
				this._onNameSubmit();
			}));
			this._uiDecideButton.state = ((!(this._uiNameInput.value == string.Empty)) ? UIButtonColor.State.Normal : UIButtonColor.State.Disabled);
			this._uiPanel.SetActive(true);
			this._ANI = GameObject.Find("AdmiralInfoScene/InfoPanel/anchor/Feather").GetComponent<Animation>();
			this._shipSelected = false;
			this._shipCancelled = false;
			this.ChkButtonState();
			StartupTaskManager.GetStartupHeader().SetMessage("提督名入力");
			this._uiNameInput.isSelected = true;
			return true;
		}

		protected override bool UnInit()
		{
			Ime.remove_OnGotIMEDialogResult(new Messages.EventHandler(this.OnGotIMEDialogResult));
			return true;
		}

		protected override bool Run()
		{
			KeyControl keyControl = StartupTaskManager.GetKeyControl();
			Main.Update();
			if (Ime.get_IsDialogOpen())
			{
				return true;
			}
			if (!keyControl.GetDown(KeyControl.KeyName.SELECT))
			{
				if (keyControl.GetDown(KeyControl.KeyName.MARU))
				{
					this.OnClickInputLabel();
				}
				else if (keyControl.GetDown(KeyControl.KeyName.START))
				{
					if (this._uiNameInput.value == string.Empty || this._uiNameInput.value.Replace(" ", string.Empty).Replace("\u3000", string.Empty) == string.Empty)
					{
						return true;
					}
					if (Utils.ChkNGWard(this._uiNameInput.value))
					{
						this._ANI.Play("feather_ng");
						return true;
					}
					this._uiNameInput.isSelected = false;
					this._onNameSubmit();
					return false;
				}
				else if (keyControl.GetDown(KeyControl.KeyName.BATU) && !StartupTaskManager.IsInheritStartup())
				{
					this._uiNameInput.isSelected = false;
					SingletonMonoBehaviour<FadeCamera>.Instance.FadeOut(0.2f, delegate
					{
						Application.LoadLevel(Generics.Scene.Title.ToString());
						this.DelayActionFrame(2, delegate
						{
							SingletonMonoBehaviour<FadeCamera>.Instance.FadeIn(0.2f, null);
						});
					});
					return true;
				}
			}
			return StartupTaskManager.GetMode() == StartupTaskManager.StartupTaskManagerMode.StartupTaskManagerMode_BEF || StartupTaskManager.GetMode() == StartupTaskManager.StartupTaskManagerMode.StartupTaskManagerMode_ST;
		}

		private void _onNameSubmit()
		{
			this._uiNameInput.isSelected = false;
			if (this._uiNameInput.value == string.Empty || Utils.ChkNGWard(this._uiNameInput.value))
			{
				this._uiNameInput.value = "横須賀提督";
			}
			StartupTaskManager.GetData().AdmiralName = this._uiNameInput.value;
			this._uiPanel.SetActive(false);
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter2);
			StartupTaskManager.ReqMode(StartupTaskManager.StartupTaskManagerMode.FirstShipSelect);
		}

		private string han2zen(string value)
		{
			string text = value;
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add(" ", "\u3000");
			dictionary.Add("!", "！");
			dictionary.Add("\"", "”");
			dictionary.Add("#", "＃");
			dictionary.Add("$", "＄");
			dictionary.Add("%", "％");
			dictionary.Add("&", "＆");
			dictionary.Add("'", "’");
			dictionary.Add("(", "（");
			dictionary.Add(")", "）");
			dictionary.Add("*", "＊");
			dictionary.Add("+", "＋");
			dictionary.Add(",", "，");
			dictionary.Add("-", "－");
			dictionary.Add(".", "．");
			dictionary.Add("/", "／");
			dictionary.Add("0", "０");
			dictionary.Add("1", "１");
			dictionary.Add("2", "２");
			dictionary.Add("3", "３");
			dictionary.Add("4", "４");
			dictionary.Add("5", "５");
			dictionary.Add("6", "６");
			dictionary.Add("7", "７");
			dictionary.Add("8", "８");
			dictionary.Add("9", "９");
			dictionary.Add(":", "：");
			dictionary.Add(";", "；");
			dictionary.Add("<", "＜");
			dictionary.Add("=", "＝");
			dictionary.Add(">", "＞");
			dictionary.Add("?", "？");
			dictionary.Add("@", "＠");
			dictionary.Add("A", "Ａ");
			dictionary.Add("B", "Ｂ");
			dictionary.Add("C", "Ｃ");
			dictionary.Add("D", "Ｄ");
			dictionary.Add("E", "Ｅ");
			dictionary.Add("F", "Ｆ");
			dictionary.Add("G", "Ｇ");
			dictionary.Add("H", "Ｈ");
			dictionary.Add("I", "Ｉ");
			dictionary.Add("J", "Ｊ");
			dictionary.Add("K", "Ｋ");
			dictionary.Add("L", "Ｌ");
			dictionary.Add("M", "Ｍ");
			dictionary.Add("N", "Ｎ");
			dictionary.Add("O", "Ｏ");
			dictionary.Add("P", "Ｐ");
			dictionary.Add("Q", "Ｑ");
			dictionary.Add("R", "Ｒ");
			dictionary.Add("S", "Ｓ");
			dictionary.Add("T", "Ｔ");
			dictionary.Add("U", "Ｕ");
			dictionary.Add("V", "Ｖ");
			dictionary.Add("W", "Ｗ");
			dictionary.Add("X", "Ｘ");
			dictionary.Add("Y", "Ｙ");
			dictionary.Add("Z", "Ｚ");
			dictionary.Add("[", "［");
			dictionary.Add("\\", "￥");
			dictionary.Add("]", "］");
			dictionary.Add("^", "＾");
			dictionary.Add("_", "＿");
			dictionary.Add("`", "‘");
			dictionary.Add("a", "ａ");
			dictionary.Add("b", "ｂ");
			dictionary.Add("c", "ｃ");
			dictionary.Add("d", "ｄ");
			dictionary.Add("e", "ｅ");
			dictionary.Add("f", "ｆ");
			dictionary.Add("g", "ｇ");
			dictionary.Add("h", "ｈ");
			dictionary.Add("i", "ｉ");
			dictionary.Add("j", "ｊ");
			dictionary.Add("k", "ｋ");
			dictionary.Add("l", "ｌ");
			dictionary.Add("m", "ｍ");
			dictionary.Add("n", "ｎ");
			dictionary.Add("o", "ｏ");
			dictionary.Add("p", "ｐ");
			dictionary.Add("q", "ｑ");
			dictionary.Add("r", "ｒ");
			dictionary.Add("s", "ｓ");
			dictionary.Add("t", "ｔ");
			dictionary.Add("u", "ｕ");
			dictionary.Add("v", "ｖ");
			dictionary.Add("w", "ｗ");
			dictionary.Add("x", "ｘ");
			dictionary.Add("y", "ｙ");
			dictionary.Add("z", "ｚ");
			dictionary.Add("{", "｛");
			dictionary.Add("|", "｜");
			dictionary.Add("}", "｝");
			dictionary.Add("~", "～");
			dictionary.Add("｡", "。");
			dictionary.Add("｢", "「");
			dictionary.Add("｣", "」");
			dictionary.Add("､", "、");
			dictionary.Add("･", "・");
			dictionary.Add("ｶﾞ", "ガ");
			dictionary.Add("ｷﾞ", "ギ");
			dictionary.Add("ｸﾞ", "グ");
			dictionary.Add("ｹﾞ", "ゲ");
			dictionary.Add("ｺﾞ", "ゴ");
			dictionary.Add("ｻﾞ", "ザ");
			dictionary.Add("ｼﾞ", "ジ");
			dictionary.Add("ｽﾞ", "ズ");
			dictionary.Add("ｾﾞ", "ゼ");
			dictionary.Add("ｿﾞ", "ゾ");
			dictionary.Add("ﾀﾞ", "ダ");
			dictionary.Add("ﾁﾞ", "ヂ");
			dictionary.Add("ﾂﾞ", "ヅ");
			dictionary.Add("ﾃﾞ", "デ");
			dictionary.Add("ﾄﾞ", "ド");
			dictionary.Add("ﾊﾞ", "バ");
			dictionary.Add("ﾋﾞ", "ビ");
			dictionary.Add("ﾌﾞ", "ブ");
			dictionary.Add("ﾍﾞ", "ベ");
			dictionary.Add("ﾎﾞ", "ボ");
			dictionary.Add("ｳﾞ", "ヴ");
			dictionary.Add("ﾜﾞ", "ヷ");
			dictionary.Add("ｲﾞ", "ヸ");
			dictionary.Add("ｴﾞ", "ヹ");
			dictionary.Add("ｦﾞ", "ヺ");
			dictionary.Add("ﾊﾟ", "パ");
			dictionary.Add("ﾋﾟ", "ピ");
			dictionary.Add("ﾌﾟ", "プ");
			dictionary.Add("ﾍﾟ", "ペ");
			dictionary.Add("ﾎﾟ", "ポ");
			dictionary.Add("ｦ", "ヲ");
			dictionary.Add("ｧ", "ァ");
			dictionary.Add("ｨ", "ィ");
			dictionary.Add("ｩ", "ゥ");
			dictionary.Add("ｪ", "ェ");
			dictionary.Add("ｫ", "ォ");
			dictionary.Add("ｬ", "ャ");
			dictionary.Add("ｭ", "ュ");
			dictionary.Add("ｮ", "ョ");
			dictionary.Add("ｯ", "ッ");
			dictionary.Add("ｰ", "ー");
			dictionary.Add("ｱ", "ア");
			dictionary.Add("ｲ", "イ");
			dictionary.Add("ｳ", "ウ");
			dictionary.Add("ｴ", "エ");
			dictionary.Add("ｵ", "オ");
			dictionary.Add("ｶ", "カ");
			dictionary.Add("ｷ", "キ");
			dictionary.Add("ｸ", "ク");
			dictionary.Add("ｹ", "ケ");
			dictionary.Add("ｺ", "コ");
			dictionary.Add("ｻ", "サ");
			dictionary.Add("ｼ", "シ");
			dictionary.Add("ｽ", "ス");
			dictionary.Add("ｾ", "セ");
			dictionary.Add("ｿ", "ソ");
			dictionary.Add("ﾀ", "タ");
			dictionary.Add("ﾁ", "チ");
			dictionary.Add("ﾂ", "ツ");
			dictionary.Add("ﾃ", "テ");
			dictionary.Add("ﾄ", "ト");
			dictionary.Add("ﾅ", "ナ");
			dictionary.Add("ﾆ", "ニ");
			dictionary.Add("ﾇ", "ヌ");
			dictionary.Add("ﾈ", "ネ");
			dictionary.Add("ﾉ", "ノ");
			dictionary.Add("ﾊ", "ハ");
			dictionary.Add("ﾋ", "ヒ");
			dictionary.Add("ﾌ", "フ");
			dictionary.Add("ﾍ", "ヘ");
			dictionary.Add("ﾎ", "ホ");
			dictionary.Add("ﾏ", "マ");
			dictionary.Add("ﾐ", "ミ");
			dictionary.Add("ﾑ", "ム");
			dictionary.Add("ﾒ", "メ");
			dictionary.Add("ﾓ", "モ");
			dictionary.Add("ﾔ", "ヤ");
			dictionary.Add("ﾕ", "ユ");
			dictionary.Add("ﾖ", "ヨ");
			dictionary.Add("ﾗ", "ラ");
			dictionary.Add("ﾘ", "リ");
			dictionary.Add("ﾙ", "ル");
			dictionary.Add("ﾚ", "レ");
			dictionary.Add("ﾛ", "ロ");
			dictionary.Add("ﾜ", "ワ");
			dictionary.Add("ﾝ", "ン");
			dictionary.Add("ﾞ", "゛");
			dictionary.Add("ﾟ", "゜");
			Dictionary<string, string> dictionary2 = dictionary;
			using (Dictionary<string, string>.Enumerator enumerator = dictionary2.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, string> current = enumerator.get_Current();
					string text2 = text.Replace(current.get_Key(), current.get_Value());
					text = text2;
				}
			}
			return text;
		}

		public void OnClickInputLabel()
		{
			if (!this._uiNameInput.isSelected)
			{
				return;
			}
			Ime.ImeDialogParams imeDialogParams = new Ime.ImeDialogParams();
			imeDialogParams.supportedLanguages = 270336;
			imeDialogParams.languagesForced = true;
			imeDialogParams.type = 0;
			imeDialogParams.option = 0;
			imeDialogParams.canCancel = true;
			imeDialogParams.textBoxMode = 2;
			imeDialogParams.enterLabel = 0;
			imeDialogParams.maxTextLength = 12;
			imeDialogParams.set_title("  貴官の提督名をお知らせ下さい。(" + 12 + "文字まで入力可能です)");
			imeDialogParams.set_initialText(this._strEditName);
			Ime.Open(imeDialogParams);
		}

		private void OnGotIMEDialogResult(Messages.PluginMessage msg)
		{
			Ime.ImeDialogResult result = Ime.GetResult();
			if (result.result == null)
			{
				string text = result.get_text();
				if (Utils.ChkNGWard(text) || Utils.ChkNGWard(result.get_text()))
				{
					text = string.Empty;
					this._ANI.Play("feather_ng");
				}
				this._strEditName = text;
				this._uiNameInput.value = this._strEditName;
			}
			this.ChkButtonState();
		}

		private void ChkButtonState()
		{
			if (this._uiNameInput.value == string.Empty || this._uiNameInput.value.Replace(" ", string.Empty).Replace("\u3000", string.Empty) == string.Empty)
			{
				this._uiDecideButton.state = UIButtonColor.State.Disabled;
				this._btnLight.StopAnim();
			}
			else if (Utils.ChkNGWard(this._uiNameInput.value))
			{
				this._uiDecideButton.state = UIButtonColor.State.Disabled;
				this._btnLight.StopAnim();
			}
			else
			{
				this._uiDecideButton.state = UIButtonColor.State.Normal;
				this._btnLight.PlayAnim();
			}
			this._uiDecideButton.GetComponent<BoxCollider>().set_enabled(this._uiDecideButton.state == UIButtonColor.State.Normal);
		}
	}
}
