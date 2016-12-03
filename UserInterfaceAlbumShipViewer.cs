using DG.Tweening;
using KCV;
using KCV.Strategy;
using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UserInterfaceAlbumShipViewer : MonoBehaviour
{
	public enum State
	{
		None,
		SwitchRotation,
		Waiting,
		MovingCamera
	}

	public enum Orientation
	{
		Vertical,
		Horizontal
	}

	public enum AnimationType
	{
		RotateDisplay
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

	[SerializeField]
	private Camera mCamera_Main;

	[SerializeField]
	private StrategyShipCharacter mStrategyShipCharacter;

	[SerializeField]
	private UIAlbumShipViewerCameraController mUIAlbumShipViewerCameraController;

	[SerializeField]
	private UILabel mLabel_ShipName;

	private Vector3 mShipDefaultOffset;

	private UserInterfaceAlbumShipViewer.StateManager<UserInterfaceAlbumShipViewer.State> mStateManager;

	private int DISPLAY_WIDTH = 960;

	private int DISPLAY_HEIGHT = 544;

	private float ASPECT = 0.566666663f;

	private UserInterfaceAlbumShipViewer.Orientation mDisplayOrientation = UserInterfaceAlbumShipViewer.Orientation.Horizontal;

	private KeyControl mKeyController;

	private void Start()
	{
		this.mKeyController = new KeyControl(0, 0, 0.4f, 0.1f);
		this.mStrategyShipCharacter.DebugChange(1);
		this.mStateManager = new UserInterfaceAlbumShipViewer.StateManager<UserInterfaceAlbumShipViewer.State>(UserInterfaceAlbumShipViewer.State.None);
		this.mStateManager.OnPush = new Action<UserInterfaceAlbumShipViewer.State>(this.OnPushState);
		this.mStateManager.OnPop = new Action<UserInterfaceAlbumShipViewer.State>(this.OnPopState);
		this.mStateManager.OnResume = new Action<UserInterfaceAlbumShipViewer.State>(this.OnResumeState);
		this.mUIAlbumShipViewerCameraController.Initialize(1024, 1024, 1f);
		this.mStateManager.PushState(UserInterfaceAlbumShipViewer.State.Waiting);
	}

	private void Update()
	{
		if (this.mKeyController != null)
		{
			this.mKeyController.Update();
			if (this.mKeyController.IsLDown())
			{
				this.SwitchOrientation();
			}
			else if (this.mKeyController.IsSankakuDown())
			{
				this.mStrategyShipCharacter.DebugChange(-1);
			}
			else if (this.mKeyController.IsShikakuDown())
			{
				this.mStrategyShipCharacter.DebugChange(1);
			}
		}
	}

	private void UpdateShipInfo(ShipModelMst shipModelMst)
	{
		this.mLabel_ShipName.text = shipModelMst.Name;
	}

	private void ChangeOrientation(UserInterfaceAlbumShipViewer.Orientation changeToOrientation, Action onFinished)
	{
		this.mDisplayOrientation = changeToOrientation;
		Sequence sequence = TweenSettingsExtensions.SetId<Sequence>(DOTween.Sequence(), UserInterfaceAlbumShipViewer.AnimationType.RotateDisplay);
		Tween tween = null;
		if (changeToOrientation != UserInterfaceAlbumShipViewer.Orientation.Vertical)
		{
			if (changeToOrientation == UserInterfaceAlbumShipViewer.Orientation.Horizontal)
			{
				Screen.set_orientation(3);
				Tween tween2 = ShortcutExtensions.DOLocalMove(this.mCamera_Main.get_transform(), Vector3.get_zero(), 0.3f, false);
				Tween tween3 = DOVirtual.Float(this.mCamera_Main.get_orthographicSize(), 1f, 0.9f, delegate(float size)
				{
					this.mCamera_Main.set_orthographicSize(size);
				});
				Tween tween4 = TweenSettingsExtensions.OnComplete<Tweener>(ShortcutExtensions.DORotate(this.mCamera_Main.get_transform(), new Vector3(0f, 0f, 0f), 0.9f, 0), delegate
				{
					this.mCamera_Main.get_transform().set_rotation(Quaternion.Euler(0f, 0f, 0f));
				});
				TweenSettingsExtensions.Append(sequence, tween4);
				TweenSettingsExtensions.Join(sequence, tween);
				TweenSettingsExtensions.Join(sequence, tween2);
				TweenSettingsExtensions.Join(sequence, tween3);
			}
		}
		else
		{
			Screen.set_orientation(1);
			Tween tween2 = ShortcutExtensions.DOLocalMove(this.mCamera_Main.get_transform(), Vector3.get_zero(), 0.3f, false);
			Tween tween3 = DOVirtual.Float(this.mCamera_Main.get_orthographicSize(), (float)this.DISPLAY_HEIGHT / (float)this.DISPLAY_WIDTH, 0.9f, delegate(float size)
			{
				this.mCamera_Main.set_orthographicSize(size);
			});
			Tween tween4 = TweenSettingsExtensions.OnComplete<Tweener>(ShortcutExtensions.DORotate(this.mCamera_Main.get_transform(), new Vector3(0f, 0f, 90f), 0.9f, 0), delegate
			{
				this.mCamera_Main.get_transform().set_rotation(Quaternion.Euler(0f, 0f, 90f));
			});
			TweenSettingsExtensions.Append(sequence, tween3);
			TweenSettingsExtensions.Join(sequence, tween);
			TweenSettingsExtensions.Join(sequence, tween2);
			TweenSettingsExtensions.Join(sequence, tween4);
		}
		TweenSettingsExtensions.OnComplete<Sequence>(sequence, delegate
		{
			if (onFinished != null)
			{
				onFinished.Invoke();
			}
		});
	}

	private void OnCallShipDragListener(Vector2 delta)
	{
		bool flag = this.mStateManager.CurrentState == UserInterfaceAlbumShipViewer.State.Waiting;
		if (flag)
		{
			UserInterfaceAlbumShipViewer.Orientation orientation = this.mDisplayOrientation;
			if (orientation != UserInterfaceAlbumShipViewer.Orientation.Vertical)
			{
				if (orientation != UserInterfaceAlbumShipViewer.Orientation.Horizontal)
				{
				}
			}
			else
			{
				Vector2 vector = delta * this.ASPECT;
			}
		}
	}

	private void OnPushState(UserInterfaceAlbumShipViewer.State pushState)
	{
		if (pushState != UserInterfaceAlbumShipViewer.State.SwitchRotation)
		{
			if (pushState == UserInterfaceAlbumShipViewer.State.Waiting)
			{
				this.mUIAlbumShipViewerCameraController.SetKeyController(this.mKeyController);
			}
		}
		else
		{
			this.OnPushSwitchRotation();
		}
	}

	private void OnPushSwitchRotation()
	{
		UserInterfaceAlbumShipViewer.Orientation orientation = this.mDisplayOrientation;
		if (orientation != UserInterfaceAlbumShipViewer.Orientation.Vertical)
		{
			if (orientation == UserInterfaceAlbumShipViewer.Orientation.Horizontal)
			{
				this.ChangeOrientation(UserInterfaceAlbumShipViewer.Orientation.Vertical, delegate
				{
					this.mUIAlbumShipViewerCameraController.Initialize(1024, 1024, 1f);
					this.mStateManager.PopState();
					this.mStateManager.ResumeState();
				});
			}
		}
		else
		{
			this.ChangeOrientation(UserInterfaceAlbumShipViewer.Orientation.Horizontal, delegate
			{
				this.mUIAlbumShipViewerCameraController.Initialize(1024, 1024, 1f);
				this.mStateManager.PopState();
				this.mStateManager.ResumeState();
			});
		}
	}

	private void OnPopState(UserInterfaceAlbumShipViewer.State popState)
	{
	}

	private void OnResumeState(UserInterfaceAlbumShipViewer.State resumeState)
	{
		if (resumeState != UserInterfaceAlbumShipViewer.State.Waiting)
		{
		}
	}

	private void SwitchOrientation()
	{
		bool flag = this.mStateManager.CurrentState == UserInterfaceAlbumShipViewer.State.Waiting;
		if (flag)
		{
			this.mStateManager.PushState(UserInterfaceAlbumShipViewer.State.SwitchRotation);
		}
	}
}
