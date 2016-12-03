using KCV;
using KCV.Remodel;
using KCV.Scene.Port;
using local.managers;
using local.models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;

public class UserInterfaceAlbumManager : MonoBehaviour
{
	public enum State
	{
		None,
		AlbumSelectGate,
		ShipAlbum,
		SlotItemAlbum,
		MoveToNextScene
	}

	private class StateManager<State>
	{
		private Stack<State> mStateStack;

		private State mEmptyState;

		public Action<State> OnPush
		{
			private get;
			set;
		}

		public Action<State> OnPop
		{
			private get;
			set;
		}

		public Action<State> OnResume
		{
			private get;
			set;
		}

		public Action<State> OnSwitch
		{
			private get;
			set;
		}

		public State CurrentState
		{
			get
			{
				if (0 < this.mStateStack.get_Count())
				{
					return this.mStateStack.Peek();
				}
				return this.mEmptyState;
			}
		}

		public StateManager(State emptyState)
		{
			this.mEmptyState = emptyState;
			this.mStateStack = new Stack<State>();
		}

		public void PushState(State state)
		{
			this.mStateStack.Push(state);
			this.Notify(this.OnPush, this.mStateStack.Peek());
			this.Notify(this.OnSwitch, this.mStateStack.Peek());
		}

		public void ReplaceState(State state)
		{
			if (0 < this.mStateStack.get_Count())
			{
				this.PopState();
			}
			this.mStateStack.Push(state);
			this.Notify(this.OnPush, this.mStateStack.Peek());
			this.Notify(this.OnSwitch, this.mStateStack.Peek());
		}

		public void PopState()
		{
			if (0 < this.mStateStack.get_Count())
			{
				State state = this.mStateStack.Pop();
				this.Notify(this.OnPop, state);
			}
		}

		public void ResumeState()
		{
			if (0 < this.mStateStack.get_Count())
			{
				this.Notify(this.OnResume, this.mStateStack.Peek());
				this.Notify(this.OnSwitch, this.mStateStack.Peek());
			}
		}

		public override string ToString()
		{
			this.mStateStack.ToArray();
			string text = string.Empty;
			using (Stack<State>.Enumerator enumerator = this.mStateStack.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					State current = enumerator.get_Current();
					text = current + " > " + text;
				}
			}
			return text;
		}

		private void Notify(Action<State> target, State state)
		{
			if (target != null)
			{
				target.Invoke(state);
			}
		}
	}

	public class Utils
	{
		public enum CharType
		{
			Sigle,
			Any
		}

		public static string NormalizeDescription(int maxLineInFullWidthChar, int fullWidthCharBuffer, string targetText)
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
					UserInterfaceAlbumManager.Utils.CharType charType = UserInterfaceAlbumManager.Utils.GetCharType(c);
					UserInterfaceAlbumManager.Utils.CharType charType2 = charType;
					if (charType2 != UserInterfaceAlbumManager.Utils.CharType.Sigle)
					{
						if (charType2 == UserInterfaceAlbumManager.Utils.CharType.Any)
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

		private static UserInterfaceAlbumManager.Utils.CharType GetCharType(char character)
		{
			int num = -1;
			if (int.TryParse(character.ToString(), ref num))
			{
				return UserInterfaceAlbumManager.Utils.CharType.Any;
			}
			Encoding encoding = new UTF8Encoding();
			int byteCount = encoding.GetByteCount(character.ToString());
			if (byteCount == 1)
			{
				return UserInterfaceAlbumManager.Utils.CharType.Sigle;
			}
			return UserInterfaceAlbumManager.Utils.CharType.Any;
		}
	}

	[SerializeField]
	private UIHowToAlbum mUIHowToAlbum;

	[SerializeField]
	private Texture[] mTextures_Preload;

	[SerializeField]
	private UIAlbumSelectGate mUIAlbumSelectGate;

	[SerializeField]
	private UserInterfaceShipAlbumManager mUserInterfaceShipAlbumManager;

	[SerializeField]
	private UserInterfaceSlotItemAlbumManager mUserInterfaceSlotItemAlbumManager;

	private AlbumManager mAlbumManager;

	private UserInterfaceAlbumManager.StateManager<UserInterfaceAlbumManager.State> mStateManager;

	private KeyControl mKeyController;

	[DebuggerHidden]
	private IEnumerator Start()
	{
		UserInterfaceAlbumManager.<Start>c__Iterator2C <Start>c__Iterator2C = new UserInterfaceAlbumManager.<Start>c__Iterator2C();
		<Start>c__Iterator2C.<>f__this = this;
		return <Start>c__Iterator2C;
	}

	private void OnChangeStateUserInterfaceShipAlbumManager(UserInterfaceShipAlbumManager.State state)
	{
		switch (state)
		{
		case UserInterfaceShipAlbumManager.State.ShipList:
			this.mUIHowToAlbum.ChangeGuideStatus(UIHowToAlbum.GuideState.List);
			break;
		case UserInterfaceShipAlbumManager.State.ShipDetail:
		case UserInterfaceShipAlbumManager.State.ShipDetailMarriaged:
			this.mUIHowToAlbum.ChangeGuideStatus(UIHowToAlbum.GuideState.Detail);
			break;
		case UserInterfaceShipAlbumManager.State.MarriagedMovie:
			this.mUIHowToAlbum.ChangeGuideStatus(UIHowToAlbum.GuideState.Quiet);
			break;
		}
	}

	private void OnChangeStateUserInterfaceSlotItemAlbumManager(UserInterfaceSlotItemAlbumManager.State state)
	{
		if (state != UserInterfaceSlotItemAlbumManager.State.SlotItemList)
		{
			if (state == UserInterfaceSlotItemAlbumManager.State.SlotItemDetail)
			{
				this.mUIHowToAlbum.ChangeGuideStatus(UIHowToAlbum.GuideState.Detail);
			}
		}
		else
		{
			this.mUIHowToAlbum.ChangeGuideStatus(UIHowToAlbum.GuideState.List);
		}
	}

	private void Update()
	{
		if (this.mKeyController != null)
		{
			this.mKeyController.Update();
		}
	}

	private void OnDestroy()
	{
		UserInterfacePortManager.ReleaseUtils.Releases(ref this.mTextures_Preload, false);
		this.mTextures_Preload = null;
		if (SingletonMonoBehaviour<UIPortFrame>.exist())
		{
			SingletonMonoBehaviour<UIPortFrame>.Instance.get_gameObject().SetActive(true);
		}
		this.mUIHowToAlbum = null;
		this.mUIAlbumSelectGate = null;
		this.mUserInterfaceShipAlbumManager = null;
		this.mUserInterfaceSlotItemAlbumManager = null;
		this.mAlbumManager = null;
		this.mStateManager = null;
		this.mKeyController = null;
	}

	private void OnBackShipAlbumListener()
	{
		bool flag = this.mStateManager.CurrentState == UserInterfaceAlbumManager.State.ShipAlbum;
		if (flag)
		{
			this.mKeyController.ClearKeyAll();
			this.mKeyController.firstUpdate = true;
			this.mStateManager.PopState();
			this.mStateManager.PushState(UserInterfaceAlbumManager.State.AlbumSelectGate);
		}
	}

	private void OnBackSlotItemAlbumListener()
	{
		bool flag = this.mStateManager.CurrentState == UserInterfaceAlbumManager.State.SlotItemAlbum;
		if (flag)
		{
			this.mKeyController.ClearKeyAll();
			this.mKeyController.firstUpdate = true;
			this.mStateManager.PopState();
			this.mStateManager.PushState(UserInterfaceAlbumManager.State.AlbumSelectGate);
		}
	}

	private void OnSelectedShipAlbumListener()
	{
		IEnumerator enumerator = this.OnSelectedShipAlbumListenerCoroutine();
		base.StartCoroutine(enumerator);
	}

	[DebuggerHidden]
	private IEnumerator OnSelectedShipAlbumListenerCoroutine()
	{
		UserInterfaceAlbumManager.<OnSelectedShipAlbumListenerCoroutine>c__Iterator2D <OnSelectedShipAlbumListenerCoroutine>c__Iterator2D = new UserInterfaceAlbumManager.<OnSelectedShipAlbumListenerCoroutine>c__Iterator2D();
		<OnSelectedShipAlbumListenerCoroutine>c__Iterator2D.<>f__this = this;
		return <OnSelectedShipAlbumListenerCoroutine>c__Iterator2D;
	}

	private void OnSelectedSlotItemAlbumListener()
	{
		IEnumerator enumerator = this.OnSelectedSlotItemAlbumListenerCoroutine();
		base.StartCoroutine(enumerator);
	}

	[DebuggerHidden]
	private IEnumerator OnSelectedSlotItemAlbumListenerCoroutine()
	{
		UserInterfaceAlbumManager.<OnSelectedSlotItemAlbumListenerCoroutine>c__Iterator2E <OnSelectedSlotItemAlbumListenerCoroutine>c__Iterator2E = new UserInterfaceAlbumManager.<OnSelectedSlotItemAlbumListenerCoroutine>c__Iterator2E();
		<OnSelectedSlotItemAlbumListenerCoroutine>c__Iterator2E.<>f__this = this;
		return <OnSelectedSlotItemAlbumListenerCoroutine>c__Iterator2E;
	}

	private void OnSelectedBackListener()
	{
		bool flag = this.mStateManager.CurrentState == UserInterfaceAlbumManager.State.AlbumSelectGate;
		if (flag)
		{
			this.mStateManager.PushState(UserInterfaceAlbumManager.State.MoveToNextScene);
		}
	}

	private void OnPushState(UserInterfaceAlbumManager.State state)
	{
		SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
		switch (state)
		{
		case UserInterfaceAlbumManager.State.AlbumSelectGate:
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
			this.mUIAlbumSelectGate.Initialize(SingletonMonoBehaviour<AppInformation>.Instance.FlagShipModel);
			this.mUIAlbumSelectGate.SetActive(true);
			this.mUIAlbumSelectGate.SetKeyController(this.mKeyController);
			this.mUIHowToAlbum.ChangeGuideStatus(UIHowToAlbum.GuideState.Gate);
			break;
		case UserInterfaceAlbumManager.State.ShipAlbum:
			this.mUserInterfaceShipAlbumManager.SetActive(true);
			this.mUserInterfaceShipAlbumManager.SetKeyController(this.mKeyController);
			this.mUserInterfaceShipAlbumManager.StartState();
			break;
		case UserInterfaceAlbumManager.State.SlotItemAlbum:
			this.mUserInterfaceSlotItemAlbumManager.SetActive(true);
			this.mUserInterfaceSlotItemAlbumManager.SetKeyController(this.mKeyController);
			this.mUserInterfaceSlotItemAlbumManager.StartState();
			break;
		case UserInterfaceAlbumManager.State.MoveToNextScene:
			this.mKeyController.ClearKeyAll();
			this.mKeyController.firstUpdate = true;
			this.mKeyController = null;
			this.OnPushMoveToNextScene();
			break;
		}
	}

	private void OnPushMoveToNextScene()
	{
		SingletonMonoBehaviour<PortObjectManager>.Instance.BackToActiveScene();
	}

	private void OnResumeState(UserInterfaceAlbumManager.State state)
	{
		SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
		if (state == UserInterfaceAlbumManager.State.AlbumSelectGate)
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
			this.mUIAlbumSelectGate.SetActive(true);
			this.mUIAlbumSelectGate.SetKeyController(this.mKeyController);
		}
	}

	private void OnPopState(UserInterfaceAlbumManager.State state)
	{
		switch (state)
		{
		case UserInterfaceAlbumManager.State.AlbumSelectGate:
			this.mUIAlbumSelectGate.SetKeyController(null);
			this.mUIAlbumSelectGate.SetActive(false);
			break;
		case UserInterfaceAlbumManager.State.ShipAlbum:
			this.mUserInterfaceShipAlbumManager.SetActive(false);
			break;
		case UserInterfaceAlbumManager.State.SlotItemAlbum:
			this.mUserInterfaceSlotItemAlbumManager.SetActive(false);
			break;
		}
	}

	internal static bool CheckMarriaged(AlbumShipModel albumShipModel)
	{
		bool flag = albumShipModel.IsMarriage();
		int[] mstIDs = albumShipModel.MstIDs;
		int[] array = mstIDs;
		for (int i = 0; i < array.Length; i++)
		{
			int mst_id = array[i];
			flag |= albumShipModel.IsMarriage(mst_id);
		}
		return flag;
	}
}
