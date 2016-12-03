using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KCV
{
	public class KeyControl
	{
		public struct IndexMap
		{
			public int up;

			public int rightUp;

			public int right;

			public int rightDown;

			public int down;

			public int leftDown;

			public int left;

			public int leftUp;
		}

		public enum Mode
		{
			NOMAL,
			DOUBL_INDEX,
			INDEX_MAP
		}

		public enum KeyName
		{
			BATU,
			MARU,
			SHIKAKU,
			SANKAKU,
			L,
			R,
			SELECT,
			START,
			UP,
			UP_RIGHT,
			RIGHT,
			DOWN_RIGHT,
			DOWN,
			DOWN_LEFT,
			LEFT,
			UP_LEFT,
			RS_UP,
			RS_UP_RIGHT,
			RS_RIGHT,
			RS_DOWN_RIGHT,
			RS_DOWN,
			RS_DOWN_LEFT,
			RS_LEFT,
			RS_UP_LEFT,
			KEY_NUM
		}

		public class KeyState
		{
			public bool down;

			public bool press;

			public bool hold;

			public float holdTime;

			public bool up;

			public bool wClick;
		}

		public const int NONE_INDEX = -99999;

		public int maxIndex;

		public int maxIndex2;

		public int minIndex;

		public int minIndex2;

		private int index;

		private int index2;

		public int prevIndex;

		public int prevIndexChangeValue;

		public float upKeyChangeValue;

		public float rightKeyChangeValue;

		public float downKeyChangeValue;

		public float leftKeyChangeValue;

		private bool isChangeIndex;

		private bool isUpdateIndex;

		private bool isAnyKey;

		private bool isDirectKeyHold;

		private KeyControl.KeyName HoldLockKey;

		private bool isStickNeutral;

		private bool _isRun;

		public bool firstUpdate;

		private bool isCleared;

		private List<KeyControl.IndexMap> indexMapList;

		private int[] indexMapBuf;

		private int[,] orignalIndexMap;

		private KeyControl.Mode controllerMode;

		public bool isLoopIndex;

		public bool isStopIndex;

		private bool isLeftStickStandAlone;

		private bool isRightStickStandAlone;

		private GameObject origine;

		private GameObject[] searchObjs;

		private List<GameObject> targetList;

		private float range;

		private float holdJudgeTime;

		private float intervalTime;

		private float keyInputInterval;

		private float keyInputIntervalButton;

		private List<KeyControl.KeyName> AutoDownKeys;

		public Dictionary<int, KeyControl.KeyState> keyState;

		private KeyCode[] keyCodeArray;

		public int Index
		{
			get
			{
				return this.index;
			}
			set
			{
				this.prevIndex = this.index;
				this.index = value;
				this.prevIndexChangeValue = this.index - this.prevIndex;
				this.index = ((!this.isLoopIndex) ? ((int)Util.RangeValue(this.index, (float)this.minIndex, (float)this.maxIndex)) : ((int)Util.LoopValue(this.index, (float)this.minIndex, (float)this.maxIndex)));
				if (this.index != this.prevIndex)
				{
					this.isChangeIndex = true;
				}
				this.isUpdateIndex = true;
				this.isAnyKey = true;
			}
		}

		public int Index2
		{
			get
			{
				return this.index2;
			}
			set
			{
				int num = this.index2;
				this.index2 = value;
				if (this.index2 > this.maxIndex2)
				{
					this.index2 = this.maxIndex2;
					if (this.controllerMode == KeyControl.Mode.INDEX_MAP)
					{
						this.index2 = num;
					}
				}
				if (this.index2 < this.minIndex2 && this.index2 != -99999)
				{
					this.index2 = this.minIndex2;
					if (this.controllerMode == KeyControl.Mode.INDEX_MAP)
					{
						this.index2 = num;
					}
				}
				if (this.index2 != num)
				{
					this.isChangeIndex = true;
				}
				this.isUpdateIndex = true;
			}
		}

		public bool IsChangeIndex
		{
			get
			{
				return this.isChangeIndex;
			}
		}

		public bool IsUpdateIndex
		{
			get
			{
				return this.isUpdateIndex;
			}
		}

		public bool IsAnyKey
		{
			get
			{
				return this.isAnyKey;
			}
		}

		public bool IsDirectKeyHold
		{
			get
			{
				return this.isDirectKeyHold;
			}
		}

		public bool IsRun
		{
			get
			{
				return this._isRun;
			}
			set
			{
				if (!value)
				{
					this.ClearKeyAll();
				}
				this._isRun = value;
			}
		}

		public float HoldJudgeTime
		{
			set
			{
				this.holdJudgeTime = value;
			}
		}

		public float KeyInputInterval
		{
			get
			{
				return this.keyInputInterval;
			}
			set
			{
				this.keyInputInterval = value;
			}
		}

		public float KeyInputIntervalButton
		{
			set
			{
				this.keyInputIntervalButton = value;
			}
		}

		public KeyControl(int min = 0, int max = 0, float holdJudgeTime = 0.4f, float keyInputInterval = 0.1f)
		{
			this.keyState = new Dictionary<int, KeyControl.KeyState>(24);
			for (int i = 0; i < 24; i++)
			{
				this.keyState.set_Item(i, new KeyControl.KeyState());
				this.initKeyState(this.keyState.get_Item(i));
			}
			this.controllerMode = KeyControl.Mode.NOMAL;
			this.index = 0;
			this.minIndex = min;
			this.maxIndex = max;
			this.isChangeIndex = false;
			this.holdJudgeTime = holdJudgeTime;
			this.keyInputInterval = keyInputInterval;
			this.keyInputIntervalButton = 0.2f;
			this.intervalTime = 0f;
			this.upKeyChangeValue = -1f;
			this.rightKeyChangeValue = 1f;
			this.downKeyChangeValue = 1f;
			this.leftKeyChangeValue = -1f;
			this.firstUpdate = true;
			this.isLoopIndex = true;
			this._isRun = true;
			this.keyCodeArray = new KeyCode[24];
			this.keyCodeArray[0] = 350;
			this.keyCodeArray[1] = 351;
			this.keyCodeArray[2] = 352;
			this.keyCodeArray[3] = 353;
			this.keyCodeArray[4] = 354;
			this.keyCodeArray[5] = 355;
			this.keyCodeArray[6] = 356;
			this.keyCodeArray[7] = 357;
			this.keyCodeArray[8] = 358;
			this.keyCodeArray[10] = 359;
			this.keyCodeArray[12] = 360;
			this.keyCodeArray[14] = 361;
			this.isLeftStickStandAlone = false;
			this.isRightStickStandAlone = false;
			this.AutoDownKeys = new List<KeyControl.KeyName>();
			this.AutoDownKeys.Add(KeyControl.KeyName.UP);
			this.AutoDownKeys.Add(KeyControl.KeyName.RIGHT);
			this.AutoDownKeys.Add(KeyControl.KeyName.DOWN);
			this.AutoDownKeys.Add(KeyControl.KeyName.LEFT);
			if (KeyControlManager.exist())
			{
				KeyControlManager.Instance.KeyController = this;
			}
		}

		public void SilentChangeIndex(int index)
		{
			this.index = index;
		}

		public void initKeyState(KeyControl.KeyState keyState)
		{
			keyState.down = false;
			keyState.press = false;
			keyState.hold = false;
			keyState.holdTime = 0f;
			keyState.up = false;
			keyState.wClick = false;
		}

		public void Update()
		{
			if (!this._isRun)
			{
				return;
			}
			if (this.isOnlyControllerExist())
			{
				return;
			}
			this.isAnyKey = false;
			this.checkKeyState();
			this.isChangeIndex = false;
			this.isUpdateIndex = false;
			if (!this.isStopIndex)
			{
				this.updateIndex();
			}
			this.checkAllFirstUpdate();
			if (this.firstUpdate)
			{
				for (int i = 0; i < 24; i++)
				{
					this.keyState.get_Item(i).down = false;
				}
				this.firstUpdate = false;
			}
		}

		private bool isOnlyControllerExist()
		{
			if (App.OnlyController != null && App.OnlyController != this)
			{
				if (!this.isCleared)
				{
					this.ClearKeyAll();
				}
				return true;
			}
			return false;
		}

		private void checkAllFirstUpdate()
		{
			if (App.isFirstUpdate)
			{
				this.firstUpdate = true;
				if (SingletonMonoBehaviour<AppInformation>.exist())
				{
					SingletonMonoBehaviour<AppInformation>.Instance.FirstUpdateEnd();
				}
				else
				{
					App.isFirstUpdate = false;
				}
			}
		}

		public void LeftStickUpdate()
		{
			if (!this._isRun)
			{
				return;
			}
			if (App.OnlyController != null && App.OnlyController != this)
			{
				if (!this.isCleared)
				{
					this.ClearKeyAll();
				}
				return;
			}
			this.checkStickState();
			this.updateIndex();
			for (int i = 0; i < 24; i++)
			{
				if (this.checkStickState() == i)
				{
					this.setKeyState(i, true);
				}
				else
				{
					this.setKeyState(i, false);
				}
			}
		}

		public void RightStickUpdate()
		{
			if (!this._isRun)
			{
				return;
			}
			if (App.OnlyController != null && App.OnlyController != this)
			{
				if (!this.isCleared)
				{
					this.ClearKeyAll();
				}
				return;
			}
			this.RightStickToDigital();
			this.updateIndex();
			for (int i = 0; i < 24; i++)
			{
				if (this.RightStickToDigital() == i)
				{
					this.setKeyState(i, true);
				}
				else
				{
					this.setKeyState(i, false);
				}
			}
		}

		private void checkKeyState()
		{
			int num = this.checkStickState();
			int num2 = this.checkRightStickState();
			for (int i = 0; i < 24; i++)
			{
				if (Input.GetKey(this.keyCodeArray[i]) || (!this.isLeftStickStandAlone && num == i) || (!this.isRightStickStandAlone && num2 == i))
				{
					this.setKeyState(i, true);
				}
				else
				{
					this.setKeyState(i, false);
				}
			}
			this.OnlyOneDirectKey();
			this.isCleared = false;
		}

		private void OnlyOneDirectKey()
		{
			bool flag = false;
			for (int i = 8; i <= 15; i++)
			{
				if (flag)
				{
					this.keyState.get_Item(i).down = false;
				}
				if (this.keyState.get_Item(i).down)
				{
					flag = true;
				}
			}
		}

		private void setKeyState(int keyName, bool press)
		{
			KeyControl.KeyState keyState = this.keyState.get_Item(keyName);
			if (press)
			{
				this.isAnyKey = true;
				if (!keyState.press)
				{
					keyState.down = true;
					if (LogDrawer.exist())
					{
						SingletonMonoBehaviour<LogDrawer>.Instance.addDebugText(keyName.ToString());
					}
				}
				else
				{
					keyState.down = false;
				}
				keyState.holdTime += Time.get_deltaTime();
				if (keyState.holdTime > this.holdJudgeTime)
				{
					keyState.hold = true;
				}
				keyState.up = false;
				keyState.press = true;
				if (this.AutoDownKeys.IndexOf((KeyControl.KeyName)keyName) != -1 && keyState.hold && (this.HoldLockKey == KeyControl.KeyName.KEY_NUM || keyName == (int)this.HoldLockKey))
				{
					this.HoldLockKey = (KeyControl.KeyName)keyName;
					float num;
					if (keyName != 8 && keyName != 10 && keyName != 12 && keyName != 14)
					{
						num = this.keyInputIntervalButton;
						this.isDirectKeyHold = true;
					}
					else
					{
						num = this.keyInputInterval;
					}
					if (this.intervalTime > num)
					{
						keyState.down = true;
						this.intervalTime = 0f;
					}
					else
					{
						this.intervalTime += Time.get_deltaTime();
					}
				}
			}
			else
			{
				keyState.down = false;
				keyState.holdTime = 0f;
				keyState.hold = false;
				if (keyState.press)
				{
					keyState.up = true;
				}
				else
				{
					keyState.up = false;
				}
				keyState.press = false;
				if (this.HoldLockKey == (KeyControl.KeyName)keyName)
				{
					this.HoldLockKey = KeyControl.KeyName.KEY_NUM;
				}
			}
		}

		private int checkStickState()
		{
			float axisRaw = Input.GetAxisRaw("Left Stick Horizontal");
			float axisRaw2 = Input.GetAxisRaw("Left Stick Vertical");
			if (0f < axisRaw && axisRaw2 < 0f)
			{
				return 9;
			}
			if (0f < axisRaw && axisRaw2 > 0f)
			{
				return 11;
			}
			if (0f > axisRaw && axisRaw2 < 0f)
			{
				return 15;
			}
			if (0f > axisRaw && axisRaw2 > 0f)
			{
				return 13;
			}
			if (axisRaw > 0f)
			{
				return 10;
			}
			if (axisRaw < 0f)
			{
				return 14;
			}
			if (axisRaw2 > 0f)
			{
				return 12;
			}
			if (axisRaw2 < 0f)
			{
				return 8;
			}
			return 24;
		}

		private int checkStickStateHEX(bool isUseLeftStick)
		{
			float num = (!isUseLeftStick) ? Input.GetAxisRaw("Right Stick Horizontal") : Input.GetAxisRaw("Left Stick Horizontal");
			float num2 = (!isUseLeftStick) ? Input.GetAxisRaw("Right Stick Vertical") : Input.GetAxisRaw("Left Stick Vertical");
			if (0.8f < num && num2 < 0f)
			{
				return 9;
			}
			if (0.8f < num && num2 > 0f)
			{
				return 11;
			}
			if (-0.8f > num && num2 < 0f)
			{
				return 15;
			}
			if (-0.8f > num && num2 > 0f)
			{
				return 13;
			}
			if (num2 > 0f)
			{
				return 12;
			}
			if (num2 < 0f)
			{
				return 8;
			}
			return 24;
		}

		private int checkRightStickState()
		{
			float axisRaw = Input.GetAxisRaw("Right Stick Horizontal");
			float axisRaw2 = Input.GetAxisRaw("Right Stick Vertical");
			if (0f < axisRaw && axisRaw2 < 0f)
			{
				return 17;
			}
			if (0f < axisRaw && axisRaw2 > 0f)
			{
				return 19;
			}
			if (0f > axisRaw && axisRaw2 < 0f)
			{
				return 23;
			}
			if (0f > axisRaw && axisRaw2 > 0f)
			{
				return 21;
			}
			if (axisRaw > 0f)
			{
				return 18;
			}
			if (axisRaw < 0f)
			{
				return 22;
			}
			if (axisRaw2 > 0f)
			{
				return 20;
			}
			if (axisRaw2 < 0f)
			{
				return 16;
			}
			return 24;
		}

		private int RightStickToDigital()
		{
			float axisRaw = Input.GetAxisRaw("Right Stick Horizontal");
			float axisRaw2 = Input.GetAxisRaw("Right Stick Vertical");
			if (0f < axisRaw && axisRaw2 < 0f)
			{
				return 9;
			}
			if (0f < axisRaw && axisRaw2 > 0f)
			{
				return 11;
			}
			if (0f > axisRaw && axisRaw2 < 0f)
			{
				return 15;
			}
			if (0f > axisRaw && axisRaw2 > 0f)
			{
				return 13;
			}
			if (axisRaw > 0f)
			{
				return 10;
			}
			if (axisRaw < 0f)
			{
				return 14;
			}
			if (axisRaw2 > 0f)
			{
				return 12;
			}
			if (axisRaw2 < 0f)
			{
				return 8;
			}
			return 24;
		}

		private void updateIndex()
		{
			this.isChangeIndex = false;
			this.isUpdateIndex = false;
			int num = this.Index;
			int num2 = this.Index2;
			if (this.controllerMode == KeyControl.Mode.NOMAL)
			{
				if (this.keyState.get_Item(8).down)
				{
					this.Index += (int)this.upKeyChangeValue;
				}
				if (this.keyState.get_Item(10).down)
				{
					this.Index += (int)this.rightKeyChangeValue;
				}
				if (this.keyState.get_Item(12).down)
				{
					this.Index += (int)this.downKeyChangeValue;
				}
				if (this.keyState.get_Item(14).down)
				{
					this.Index += (int)this.leftKeyChangeValue;
				}
			}
			else if (this.controllerMode == KeyControl.Mode.INDEX_MAP)
			{
				if (this.keyState.get_Item(8).down)
				{
					this.Index = this.getIndexMapValue(this.index, KeyControl.KeyName.UP);
				}
				if (this.keyState.get_Item(10).down)
				{
					this.Index = this.getIndexMapValue(this.index, KeyControl.KeyName.RIGHT);
				}
				if (this.keyState.get_Item(12).down)
				{
					this.Index = this.getIndexMapValue(this.index, KeyControl.KeyName.DOWN);
				}
				if (this.keyState.get_Item(14).down)
				{
					this.Index = this.getIndexMapValue(this.index, KeyControl.KeyName.LEFT);
				}
				if (this.keyState.get_Item(9).down)
				{
					this.Index = this.getIndexMapValue(this.index, KeyControl.KeyName.UP_RIGHT);
				}
				if (this.keyState.get_Item(15).down)
				{
					this.Index = this.getIndexMapValue(this.index, KeyControl.KeyName.UP_LEFT);
				}
				if (this.keyState.get_Item(11).down)
				{
					this.Index = this.getIndexMapValue(this.index, KeyControl.KeyName.DOWN_RIGHT);
				}
				if (this.keyState.get_Item(13).down)
				{
					this.Index = this.getIndexMapValue(this.index, KeyControl.KeyName.DOWN_LEFT);
				}
			}
			else if (this.controllerMode == KeyControl.Mode.DOUBL_INDEX)
			{
				if (this.keyState.get_Item(8).down)
				{
					this.Index += (int)this.upKeyChangeValue;
				}
				if (this.keyState.get_Item(10).down)
				{
					this.Index2 += (int)this.rightKeyChangeValue;
				}
				if (this.keyState.get_Item(12).down)
				{
					this.Index += (int)this.downKeyChangeValue;
				}
				if (this.keyState.get_Item(14).down)
				{
					this.Index2 += (int)this.leftKeyChangeValue;
				}
			}
			if (this.isChangeIndex)
			{
			}
		}

		public void setChangeValue(float up, float right, float down, float left)
		{
			this.upKeyChangeValue = up;
			this.rightKeyChangeValue = right;
			this.downKeyChangeValue = down;
			this.leftKeyChangeValue = left;
		}

		public void setMinMaxIndex(int min = 0, int max = 0)
		{
			this.minIndex = min;
			this.maxIndex = max;
		}

		public void setMinIndex(int min = 0)
		{
			this.setMinMaxIndex(min, this.maxIndex);
		}

		public void setMaxIndex(int max = 0)
		{
			this.setMinMaxIndex(this.minIndex, max);
		}

		public void useDoubleIndex(int min, int max)
		{
			this.controllerMode = KeyControl.Mode.DOUBL_INDEX;
			this.index2 = min;
			this.minIndex2 = min;
			this.maxIndex2 = max;
		}

		private void addIndexMap(int[,] IndexMapArray)
		{
			for (int i = 0; i < IndexMapArray.GetLength(0); i++)
			{
				KeyControl.IndexMap indexMap = default(KeyControl.IndexMap);
				indexMap.up = IndexMapArray[i, 0];
				indexMap.rightUp = IndexMapArray[i, 1];
				indexMap.right = IndexMapArray[i, 2];
				indexMap.rightDown = IndexMapArray[i, 3];
				indexMap.down = IndexMapArray[i, 4];
				indexMap.leftDown = IndexMapArray[i, 5];
				indexMap.left = IndexMapArray[i, 6];
				indexMap.leftUp = IndexMapArray[i, 7];
				this.indexMapList.Add(indexMap);
			}
		}

		public void setUseIndexMap(int[,] indexMapArray)
		{
			this.controllerMode = KeyControl.Mode.INDEX_MAP;
			this.indexMapList = new List<KeyControl.IndexMap>();
			this.addIndexMap(indexMapArray);
			this.orignalIndexMap = new int[indexMapArray.GetLength(0), indexMapArray.GetLength(1)];
			Array.Copy(indexMapArray, 0, this.orignalIndexMap, 0, indexMapArray.get_Length());
			this.indexMapBuf = new int[8];
		}

		public void setEnableIndex(int[] indexArray)
		{
			int[,] array = new int[this.orignalIndexMap.GetLength(0), this.orignalIndexMap.GetLength(1)];
			Array.Copy(this.orignalIndexMap, 0, array, 0, this.orignalIndexMap.get_Length());
			for (int i = 0; i < array.GetLength(0); i++)
			{
				for (int j = 0; j < array.GetLength(1); j++)
				{
					if (array[i, j] != 0)
					{
						bool flag = false;
						for (int k = 0; k < indexArray.Length; k++)
						{
							if (array[i, j] == indexArray[k])
							{
								flag = true;
							}
						}
						if (!flag)
						{
							array[i, j] = 0;
						}
					}
				}
			}
			this.controllerMode = KeyControl.Mode.INDEX_MAP;
			this.indexMapList = new List<KeyControl.IndexMap>();
			this.addIndexMap(array);
		}

		private int getIndexMapValue(int nowIndex, KeyControl.KeyName keyName)
		{
			int num = keyName - KeyControl.KeyName.UP;
			int num2 = num + 1;
			int num3 = num - 1;
			if (num2 > 7)
			{
				num2 = 0;
			}
			if (num3 < 0)
			{
				num3 = 7;
			}
			this.indexMapBuf[0] = this.indexMapList.get_Item(nowIndex).up;
			this.indexMapBuf[1] = this.indexMapList.get_Item(nowIndex).rightUp;
			this.indexMapBuf[2] = this.indexMapList.get_Item(nowIndex).right;
			this.indexMapBuf[3] = this.indexMapList.get_Item(nowIndex).rightDown;
			this.indexMapBuf[4] = this.indexMapList.get_Item(nowIndex).down;
			this.indexMapBuf[5] = this.indexMapList.get_Item(nowIndex).leftDown;
			this.indexMapBuf[6] = this.indexMapList.get_Item(nowIndex).left;
			this.indexMapBuf[7] = this.indexMapList.get_Item(nowIndex).leftUp;
			if (this.indexMapBuf[num] != 0)
			{
				return this.indexMapBuf[num];
			}
			if (this.indexMapBuf[num2] != 0)
			{
				return this.indexMapBuf[num2];
			}
			if (this.indexMapBuf[num3] != 0)
			{
				return this.indexMapBuf[num3];
			}
			return nowIndex;
		}

		public void ClearKeyAll()
		{
			for (int i = 0; i < 24; i++)
			{
				this.initKeyState(this.keyState.get_Item(i));
			}
			this.isChangeIndex = false;
			this.isUpdateIndex = false;
			this.isCleared = true;
		}

		public void reset(int min = 0, int max = 0, float holdJudgeTime = 0.4f, float keyInputInterval = 0.1f)
		{
			this.controllerMode = KeyControl.Mode.NOMAL;
			this.index = 0;
			this.minIndex = min;
			this.maxIndex = max;
			this.isChangeIndex = false;
			this.holdJudgeTime = holdJudgeTime;
			this.keyInputInterval = keyInputInterval;
			this.intervalTime = 0f;
			this.upKeyChangeValue = -1f;
			this.rightKeyChangeValue = 1f;
			this.downKeyChangeValue = 1f;
			this.leftKeyChangeValue = -1f;
			this.firstUpdate = true;
			this.isLoopIndex = true;
			this._isRun = true;
			this.isLeftStickStandAlone = false;
			this.isRightStickStandAlone = false;
		}

		public void selectButton(GameObject[] Buttons)
		{
			for (int i = 0; i < Buttons.Length; i++)
			{
				if (this.Index == i)
				{
					TweenColor.Begin(Buttons[i], 0.2f, Util.CursolColor);
				}
				else
				{
					TweenColor.Begin(Buttons[i], 0.2f, Color.get_white());
				}
			}
		}

		public void setStickStandAlone(bool LeftStick, bool RightStick)
		{
			this.isLeftStickStandAlone = LeftStick;
			this.isRightStickStandAlone = RightStick;
		}

		public void unselectButton(UIButton[] Buttons)
		{
			for (int i = 0; i < Buttons.Length; i++)
			{
				TweenColor.Begin(Buttons[i].get_gameObject(), 0.2f, Color.get_white());
			}
		}

		public void addHoldAutoDownKey(KeyControl.KeyName keyname)
		{
			this.AutoDownKeys.Add(keyname);
		}

		public void InitSeachFocus(GameObject[] objs)
		{
			this.searchObjs = objs;
			this.targetList = new List<GameObject>();
			this.range = 100f;
		}

		public GameObject SeachFocusObject(GameObject origine)
		{
			Vector3 originalPos = origine.get_transform().get_position();
			if (Input.GetAxisRaw("Left Stick Vertical") == 1f)
			{
				GameObject[] array = this.searchObjs;
				for (int i = 0; i < array.Length; i++)
				{
					GameObject gameObject = array[i];
					if (gameObject != origine && gameObject.get_transform().get_position().y > originalPos.y + 0.01f && gameObject.get_transform().get_position().x >= originalPos.x - this.range && gameObject.get_transform().get_position().x <= originalPos.x + this.range)
					{
						this.targetList.Add(gameObject);
					}
				}
			}
			else if (Input.GetAxisRaw("Left Stick Vertical") == -1f)
			{
				GameObject[] array2 = this.searchObjs;
				for (int j = 0; j < array2.Length; j++)
				{
					GameObject gameObject2 = array2[j];
					if (gameObject2 != origine && gameObject2.get_transform().get_position().y + 0.01f < originalPos.y && gameObject2.get_transform().get_position().x >= originalPos.x - this.range && gameObject2.get_transform().get_position().x <= originalPos.x + this.range)
					{
						this.targetList.Add(gameObject2);
					}
				}
			}
			else if (Input.GetAxisRaw("Left Stick Horizontal") == -1f)
			{
				GameObject[] array3 = this.searchObjs;
				for (int k = 0; k < array3.Length; k++)
				{
					GameObject gameObject3 = array3[k];
					if (gameObject3 != origine && gameObject3.get_transform().get_position().x + 0.01f < originalPos.x && gameObject3.get_transform().get_position().y >= originalPos.y - this.range && gameObject3.get_transform().get_position().y <= originalPos.y + this.range)
					{
						this.targetList.Add(gameObject3);
					}
				}
			}
			else if (Input.GetAxisRaw("Left Stick Horizontal") == 1f)
			{
				GameObject[] array4 = this.searchObjs;
				for (int l = 0; l < array4.Length; l++)
				{
					GameObject gameObject4 = array4[l];
					if (gameObject4 != origine && gameObject4.get_transform().get_position().x > originalPos.x + 0.01f && gameObject4.get_transform().get_position().y >= originalPos.y - this.range && gameObject4.get_transform().get_position().y <= originalPos.y + this.range)
					{
						this.targetList.Add(gameObject4);
					}
				}
			}
			if (this.targetList.get_Count() <= 0)
			{
				return origine;
			}
			GameObject gameObject5 = Enumerable.First<GameObject>(Enumerable.OrderBy<GameObject, float>(this.targetList, (GameObject obj) => (obj.get_transform().get_position() - originalPos).get_magnitude())).get_gameObject();
			this.targetList.Clear();
			return gameObject5;
		}

		public bool IsMaruDown()
		{
			return this.keyState.get_Item(1).down;
		}

		public bool IsBatuDown()
		{
			return this.keyState.get_Item(0).down;
		}

		public bool IsSankakuDown()
		{
			return this.keyState.get_Item(3).down;
		}

		public bool IsShikakuDown()
		{
			return this.keyState.get_Item(2).down;
		}

		public bool IsUpDown()
		{
			return this.keyState.get_Item(8).down;
		}

		public bool IsDownDown()
		{
			return this.keyState.get_Item(12).down;
		}

		public bool IsLeftDown()
		{
			return this.keyState.get_Item(14).down;
		}

		public bool IsRightDown()
		{
			return this.keyState.get_Item(10).down;
		}

		public bool IsLDown()
		{
			return this.keyState.get_Item(4).down;
		}

		public bool IsRDown()
		{
			return this.keyState.get_Item(5).down;
		}

		public bool GetDown(int nKey)
		{
			return this.keyState.get_Item(nKey).down;
		}

		public bool GetDown(KeyControl.KeyName iName)
		{
			return this.GetDown((int)iName);
		}

		public bool GetPress(int nKey)
		{
			return this.keyState.get_Item(nKey).press;
		}

		public bool GetPress(KeyControl.KeyName iName)
		{
			return this.GetPress((int)iName);
		}

		public bool GetHold(int nKey)
		{
			return this.keyState.get_Item(nKey).hold;
		}

		public bool GetHold(KeyControl.KeyName iName)
		{
			return this.GetHold((int)iName);
		}

		public float GetHoldTime(int nKey)
		{
			return this.keyState.get_Item(nKey).holdTime;
		}

		public float GetHoldTime(KeyControl.KeyName iName)
		{
			return this.GetHoldTime((int)iName);
		}

		public bool GetUp(int nKey)
		{
			return this.keyState.get_Item(nKey).up;
		}

		public bool GetUp(KeyControl.KeyName iName)
		{
			return this.GetUp((int)iName);
		}

		public bool GetWClick(int nKey)
		{
			return this.keyState.get_Item(nKey).wClick;
		}

		public bool GetWClick(KeyControl.KeyName iName)
		{
			return this.GetWClick((int)iName);
		}

		internal bool IsRSLeftDown()
		{
			return this.keyState.get_Item(22).down;
		}

		internal bool IsRSRightDown()
		{
			return this.keyState.get_Item(18).down;
		}
	}
}
