using KCV;
using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class Generics
{
	public enum Tag
	{
		Untagged,
		Respawn,
		Finish,
		EditorOverlay,
		MainCamera,
		Player,
		GameController,
		Tiles,
		MainMenuButton,
		TransitionButton,
		IndexButton,
		ItemLabel,
		DeckIcon,
		ShipGirl,
		Search_Object,
		StrategyTile,
		SuppryIcon,
		CommonShipBanner,
		CommandSurface
	}

	[Flags]
	public enum Layers
	{
		Nothing = 0,
		Everything = -1,
		Default = 1,
		TransparentFX = 2,
		IgnoreRaycast = 4,
		Water = 16,
		UI = 32,
		Background = 256,
		UI2D = 512,
		UI3D = 1024,
		Transition = 2048,
		ShipGirl = 4096,
		TopMost = 8192,
		CutIn = 16384,
		SaveData = 32768,
		Effects = 65536,
		FocusDim = 131072,
		UnRefrectEffects = 262144,
		SplitWater = 524288
	}

	public enum Scene
	{
		Scene_ST,
		Scene_BEF = -1,
		Scene_None = -1,
		SplashScreen,
		Title,
		Startup,
		Port,
		PortTop,
		Battle,
		Organize,
		Supply,
		Remodel,
		Repair,
		Arsenal,
		ImprovementArsenal,
		Duty,
		Record,
		Item,
		Interior,
		Option,
		SaveLoad,
		InheritSave,
		InheritLoad,
		Album,
		Strategy,
		MapSelect,
		SortieAreaMap,
		Practice,
		ExercisesPartnerSelection,
		Expedition,
		Deploy,
		EscortFleetOrganization,
		Marriage,
		ArsenalSelector,
		Ending,
		LoadingScene,
		Scene_Empty,
		Scene_AFT,
		Scene_NUM = 34,
		Scene_ED = 33
	}

	public enum BattleRootType
	{
		Practice,
		SortieMap,
		Rebellion
	}

	[Serializable]
	public class InnerCamera
	{
		[SerializeField]
		protected Camera _camCamera;

		[SerializeField]
		protected UICamera _camUICamera;

		public virtual Generics.Layers sameMask
		{
			set
			{
				this._camCamera.set_cullingMask(this._camUICamera.eventReceiverMask = (int)value);
			}
		}

		public virtual float depth
		{
			get
			{
				return this._camCamera.get_depth();
			}
			set
			{
				this._camCamera.set_depth(value);
			}
		}

		public InnerCamera(Transform instance)
		{
			this._camCamera = instance.GetComponent<Camera>();
			this._camUICamera = instance.GetComponent<UICamera>();
		}

		public InnerCamera(Transform parent, string objName)
		{
			Util.FindParentToChild<Camera>(ref this._camCamera, parent, objName);
			Util.FindParentToChild<UICamera>(ref this._camUICamera, parent, objName);
		}

		public bool Init()
		{
			return true;
		}

		public bool UnInit()
		{
			this._camCamera = null;
			this._camUICamera = null;
			return true;
		}

		public virtual void CullingMask(Generics.Layers layer)
		{
			this._camCamera.set_cullingMask((int)layer);
		}

		public virtual void EventMask(Generics.Layers layer)
		{
			this._camUICamera.eventReceiverMask = (int)layer;
		}

		public virtual void SameMask(Generics.Layers layer)
		{
			this._camCamera.set_cullingMask(this._camUICamera.eventReceiverMask = (int)layer);
		}
	}

	[Serializable]
	public class Message
	{
		private UILabel _uiLabel;

		private string _strTemp;

		private int _nPos;

		private float _fInterVal;

		private float _fTimer;

		private bool _isSkip;

		private bool _isTalk;

		private bool _isStartMessage;

		private Action _actCallback;

		public bool IsMessageEnd
		{
			get
			{
				return this._isTalk;
			}
		}

		public Message(Transform parent, string objName)
		{
			Util.FindParentToChild<UILabel>(ref this._uiLabel, parent, objName);
			this._uiLabel.text = string.Empty;
		}

		public Message(UILabel label)
		{
			this._uiLabel = label;
			this._uiLabel.text = string.Empty;
		}

		public bool Init(string message, float interval = 0.03f, Action callback = null)
		{
			this._strTemp = Util.Indentision(message);
			this._nPos = 0;
			this._isSkip = false;
			this._fInterVal = interval;
			this._fTimer = this._fInterVal;
			this._isTalk = false;
			this._isStartMessage = false;
			this._actCallback = callback;
			return true;
		}

		public bool UnInit()
		{
			this._uiLabel = null;
			this._actCallback = null;
			this._strTemp = string.Empty;
			return true;
		}

		public void Update()
		{
			if (this._isTalk)
			{
			}
			if (!this._isStartMessage)
			{
				return;
			}
			this._fTimer -= Time.get_deltaTime();
			if (this._fTimer <= 0f)
			{
				if (this._nPos < this._strTemp.get_Length())
				{
					this._nPos++;
					this._uiLabel.text = this._strTemp.Substring(0, this._nPos);
				}
				else
				{
					this._isTalk = true;
					this._isStartMessage = false;
					if (this._actCallback != null)
					{
						this._actCallback.Invoke();
					}
				}
				this._fTimer = this._fInterVal;
			}
		}

		public void Reset()
		{
			this._uiLabel.text = string.Empty;
			this._nPos = 0;
			this._isSkip = false;
			this._isTalk = false;
			this._isSkip = false;
		}

		public void Play()
		{
			this._isStartMessage = true;
		}
	}

	[Serializable]
	public class NextIndexInfos
	{
		public int Up = -1;

		public int UpLeft = -1;

		public int UpRight = -1;

		public int Left = -1;

		public int Right = -1;

		public int Center = -1;

		public int Down = -1;

		public int DownLeft = -1;

		public int DownRight = -1;

		public NextIndexInfos(int up, int upleft, int upright, int center, int left, int right, int down, int downleft, int downright)
		{
			this.Up = up;
			this.UpLeft = upleft;
			this.UpRight = upright;
			this.Right = right;
			this.Left = left;
			this.Center = center;
			this.Down = down;
			this.DownLeft = downleft;
			this.DownRight = downright;
		}

		public NextIndexInfos(int up, int down, int left, int right)
		{
			this.Up = up;
			this.Down = down;
			this.Left = left;
			this.Right = right;
			this.UpLeft = (this.UpLeft = (this.DownLeft = (this.DownRight = (this.Left = (this.Right = -1)))));
		}

		public NextIndexInfos()
		{
			this.Up = (this.Down = (this.Left = (this.Right = (this.UpLeft = (this.UpLeft = (this.DownLeft = (this.DownRight = (this.Left = (this.Right = -1)))))))));
		}

		public int GetIndex(KeyControl.KeyName iName, int defVal)
		{
			int num = -1;
			switch (iName)
			{
			case KeyControl.KeyName.UP:
				num = this.Up;
				break;
			case KeyControl.KeyName.UP_RIGHT:
				num = this.UpRight;
				break;
			case KeyControl.KeyName.RIGHT:
				num = this.Right;
				break;
			case KeyControl.KeyName.DOWN_RIGHT:
				num = this.DownRight;
				break;
			case KeyControl.KeyName.DOWN:
				num = this.Down;
				break;
			case KeyControl.KeyName.DOWN_LEFT:
				num = this.DownLeft;
				break;
			case KeyControl.KeyName.LEFT:
				num = this.Left;
				break;
			case KeyControl.KeyName.UP_LEFT:
				num = this.UpLeft;
				break;
			}
			return (num != -1) ? num : defVal;
		}
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct BBCodeColor
	{
		public static string kGreen
		{
			get
			{
				return "1DBDC0";
			}
		}

		public static string red
		{
			get
			{
				return "FF0000";
			}
		}

		public static string UIBlue
		{
			get
			{
				return "64C8FF";
			}
		}

		public static string kGreenDark
		{
			get
			{
				return "0E5E60";
			}
		}

		public static Color ToRGBA(uint color16, float alpha = 1f)
		{
			float num = 0.003921569f;
			Color black = Color.get_black();
			black.r = num * (color16 >> 16 & 255u);
			black.g = num * (color16 >> 8 & 255u);
			black.b = num * (color16 & 255u);
			black.a = alpha;
			return black;
		}
	}
}
