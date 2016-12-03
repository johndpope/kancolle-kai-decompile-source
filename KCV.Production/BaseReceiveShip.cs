using local.models;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace KCV.Production
{
	public class BaseReceiveShip : MonoBehaviour
	{
		protected enum CharType
		{
			Sigle,
			Any
		}

		[SerializeField]
		protected UITexture _uiBg;

		[SerializeField]
		protected UITexture _uiShip;

		[SerializeField]
		protected UILabel _clsShipName;

		[SerializeField]
		protected UILabel _clsSType;

		[SerializeField]
		protected Animation _getIconAnim;

		[SerializeField]
		protected UISprite _uiGear;

		[SerializeField]
		protected Animation _anim;

		[SerializeField]
		protected Animation _gearAnim;

		protected Generics.Message _clsShipMessage;

		protected AudioSource _Se;

		protected int debugIndex;

		protected IReward_Ship _clsRewardShip;

		protected Action _actCallback;

		protected bool _isFinished;

		protected bool _isInput;

		protected KeyControl _clsInput;

		protected virtual void init()
		{
			Util.FindParentToChild<UITexture>(ref this._uiBg, base.get_transform(), "BG");
			Util.FindParentToChild<UITexture>(ref this._uiShip, base.get_transform(), "ShipLayoutOffset/Ship");
			Util.FindParentToChild<UILabel>(ref this._clsShipName, base.get_transform(), "MessageWindow/ShipName");
			Util.FindParentToChild<UILabel>(ref this._clsSType, base.get_transform(), "MessageWindow/ShipType");
			Util.FindParentToChild<Animation>(ref this._getIconAnim, base.get_transform(), "MessageWindow/Get");
			Util.FindParentToChild<UISprite>(ref this._uiGear, base.get_transform(), "MessageWindow/NextBtn");
			if (this._anim == null)
			{
				this._anim = base.GetComponent<Animation>();
			}
			if (this._gearAnim == null)
			{
				this._gearAnim = this._uiGear.GetComponent<Animation>();
			}
			this._clsShipMessage = new Generics.Message(base.get_transform(), "MessageWindow/ShipMessage");
			this._uiShip.alpha = 0f;
			UIButtonMessage component = this._uiGear.GetComponent<UIButtonMessage>();
			component.target = base.get_gameObject();
			component.functionName = "prodReceiveShipEL";
			component.trigger = UIButtonMessage.Trigger.OnClick;
			UIButtonMessage component2 = this._uiBg.GetComponent<UIButtonMessage>();
			component2.target = base.get_gameObject();
			component2.functionName = "backgroundEL";
			component2.trigger = UIButtonMessage.Trigger.OnClick;
			this._isInput = false;
			this._isFinished = false;
			this.debugIndex = 0;
			this._uiGear.GetComponent<Collider2D>().set_enabled(false);
		}

		protected virtual void OnDestroy()
		{
			this._uiShip = null;
			Mem.Del<UITexture>(ref this._uiBg);
			Mem.Del<UITexture>(ref this._uiShip);
			Mem.Del<UILabel>(ref this._clsShipName);
			Mem.Del<UILabel>(ref this._clsSType);
			Mem.Del<Animation>(ref this._getIconAnim);
			Mem.Del(ref this._uiGear);
			Mem.Del<Animation>(ref this._anim);
			Mem.Del<Animation>(ref this._gearAnim);
			if (this._clsShipMessage != null)
			{
				this._clsShipMessage.UnInit();
			}
			Mem.Del<Generics.Message>(ref this._clsShipMessage);
			Mem.Del<IReward_Ship>(ref this._clsRewardShip);
			Mem.Del<Action>(ref this._actCallback);
			Mem.Del<KeyControl>(ref this._clsInput);
			Mem.Del<AudioSource>(ref this._Se);
		}

		protected bool Run()
		{
			this._clsShipMessage.Update();
			if (this._isInput)
			{
			}
			return false;
		}

		protected void _setRewardShip()
		{
			this._uiShip.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(this._clsRewardShip.Ship.GetGraphicsMstId(), 9);
			this._uiShip.MakePixelPerfect();
			this._uiShip.get_transform().set_localPosition(Util.Poi2Vec(new ShipOffset(this._clsRewardShip.Ship.GetGraphicsMstId()).GetShipDisplayCenter(false)));
			this._clsShipMessage.Init(this._clsRewardShip.GreetingText, 0.08f, null);
			this._clsShipName.text = this._clsRewardShip.Ship.Name;
			this._clsSType.text = this._clsRewardShip.Ship.ShipTypeName;
			this._clsShipName.SetActive(false);
			this._clsSType.SetActive(false);
			this._getIconAnim.get_gameObject().SetActive(false);
		}

		protected void _debugRewardShip()
		{
			Debug.Log("ShipID:" + this.debugIndex);
			if (Mst_DataManager.Instance.Mst_shipgraph.ContainsKey(this.debugIndex))
			{
				IReward_Ship reward_Ship = new Reward_Ship(this.debugIndex);
				this._uiShip.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(reward_Ship.Ship.GetGraphicsMstId(), 9);
				this._uiShip.MakePixelPerfect();
				this._uiShip.get_transform().set_localPosition(Util.Poi2Vec(new ShipOffset(reward_Ship.Ship.GetGraphicsMstId()).GetShipDisplayCenter(false)));
				string text = this.NormalizeDescription(26, 1, reward_Ship.GreetingText);
				this._clsShipMessage.Init(reward_Ship.GreetingText, 0.01f, null);
				this._clsShipName.text = reward_Ship.Ship.Name;
				this._clsSType.text = reward_Ship.Ship.ShipTypeName;
				this._clsShipName.SetActive(false);
				this._clsSType.SetActive(false);
				this._getIconAnim.get_gameObject().SetActive(false);
				this._uiBg.mainTexture = TextureFile.LoadRareBG(reward_Ship.Ship.Rare);
				Debug.Log(reward_Ship.GreetingText);
			}
			this._anim.Stop();
			this._anim.Play("comp_GetShip");
		}

		private string NormalizeDescription(int maxLineInFullWidthChar, int fullWidthCharBuffer, string targetText)
		{
			int num = maxLineInFullWidthChar * 2;
			int num2 = fullWidthCharBuffer * 2;
			int num3 = num * num2;
			string text = "、。！？」』)";
			string text2 = targetText.Replace("\r\n", "\n");
			text2 = text2.Replace("\\n", "\n");
			text2 = text2.Replace("<br>", "\n");
			string[] array = text2.Split(new char[]
			{
				'\n'
			});
			List<string> list = new List<string>();
			for (int i = 0; i < array.Length; i++)
			{
				int num4 = 0;
				string text3 = array[i];
				StringBuilder stringBuilder = new StringBuilder();
				string text4 = text3;
				for (int j = 0; j < text4.get_Length(); j++)
				{
					char c = text4.get_Chars(j);
					int num5 = 0;
					BaseReceiveShip.CharType charType = this.GetCharType(c);
					BaseReceiveShip.CharType charType2 = charType;
					if (charType2 != BaseReceiveShip.CharType.Sigle)
					{
						if (charType2 == BaseReceiveShip.CharType.Any)
						{
							num5 = 2;
						}
					}
					else
					{
						num5 = 1;
					}
					if (num4 + num5 <= num)
					{
						stringBuilder.Append(c);
						num4 += num5;
					}
					else
					{
						string text5 = stringBuilder.ToString();
						list.Add(text5);
						stringBuilder.set_Length(0);
						stringBuilder.Append(c);
						num4 = num5;
					}
				}
				if (0 < stringBuilder.get_Length())
				{
					list.Add(stringBuilder.ToString());
					stringBuilder.set_Length(0);
				}
			}
			StringBuilder stringBuilder2 = new StringBuilder();
			for (int k = 0; k < list.get_Count(); k++)
			{
				if (k == 0)
				{
					stringBuilder2.Append(list.get_Item(k));
				}
				else if (-1 < text.IndexOf(list.get_Item(k).get_Chars(0)))
				{
					string text6 = list.get_Item(k);
					string text7 = text6.Substring(0, 1);
					stringBuilder2.Append(text7);
					if (1 < text6.get_Length())
					{
						stringBuilder2.Append('\n');
						string text8 = text6.Substring(1);
						stringBuilder2.Append(text8);
					}
				}
				else
				{
					stringBuilder2.Append('\n');
					stringBuilder2.Append(list.get_Item(k));
				}
			}
			return stringBuilder2.ToString();
		}

		private BaseReceiveShip.CharType GetCharType(char character)
		{
			int num = -1;
			if (int.TryParse(character.ToString(), ref num))
			{
				return BaseReceiveShip.CharType.Any;
			}
			Encoding encoding = new UTF8Encoding();
			int byteCount = encoding.GetByteCount(character.ToString());
			return (byteCount != 1) ? BaseReceiveShip.CharType.Any : BaseReceiveShip.CharType.Sigle;
		}

		protected void Discard()
		{
			this._uiShip = null;
			Object.Destroy(base.get_gameObject(), 0.1f);
		}
	}
}
