using DG.Tweening;
using Server_Models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Furniture.JukeBox
{
	[RequireComponent(typeof(UIPanel)), RequireComponent(typeof(UIButtonManager))]
	public class UIJukeBoxMusicPlayingDialog : MonoBehaviour
	{
		private UIButtonManager mButtonManager;

		private UIPanel mPanelThis;

		[SerializeField]
		private Transform mTransform_YouseiOffset;

		[SerializeField]
		private UITexture mTexture_Yousei;

		[SerializeField]
		private Texture mTexture2d_Yousei_Frame_0;

		[SerializeField]
		private Texture mTexture2d_Yousei_Frame_1;

		[SerializeField]
		private Texture mTexture2d_Yousei_Frame_2;

		[SerializeField]
		private UIButton mButton_Negative;

		[SerializeField]
		private UIButton mButton_Positive;

		[SerializeField]
		private UIJukeBoxMusicPlayingRollLabel mUIJukeBoxMusicPlayingRollLabel;

		private Action mOnRequestBackToRoot;

		private Action mOnSelectPositiveListener;

		private Action mOnSelectNegativeListener;

		private UIButton mButtonCurrentFocus;

		private Action mOnRequestChangeScene;

		private Mst_bgm_jukebox mMst_bgm_jukebox;

		private KeyControl mKeyController;

		private UIButton[] mButtonFocasable;

		private void Awake()
		{
			this.mPanelThis = base.GetComponent<UIPanel>();
			this.mPanelThis.alpha = 0f;
			this.mButtonManager = base.GetComponent<UIButtonManager>();
			this.mButtonManager.IndexChangeAct = delegate
			{
				int num = Array.IndexOf<UIButton>(this.mButtonFocasable, this.mButtonManager.nowForcusButton);
				if (0 <= num)
				{
					this.ChangeFocus(this.mButtonManager.nowForcusButton);
				}
			};
		}

		private void Update()
		{
			if (this.mKeyController != null)
			{
				if (this.mKeyController.keyState.get_Item(14).down)
				{
					this.ChangeFocus(this.mButton_Negative);
				}
				else if (this.mKeyController.keyState.get_Item(10).down)
				{
					int num = Array.IndexOf<UIButton>(this.mButtonFocasable, this.mButton_Positive);
					if (0 <= num)
					{
						this.ChangeFocus(this.mButton_Positive);
					}
				}
				else if (this.mKeyController.keyState.get_Item(1).down)
				{
					if (this.mButton_Negative.Equals(this.mButtonCurrentFocus))
					{
						this.OnClickNegative();
					}
					else if (this.mButton_Positive.Equals(this.mButtonCurrentFocus))
					{
						this.OnClickPositive();
					}
				}
				else if (this.mKeyController.keyState.get_Item(0).down)
				{
					this.OnClickNegative();
				}
				else if (this.mKeyController.IsRDown())
				{
					this.OnRequestChangeScene();
				}
				else if (this.mKeyController.IsLDown())
				{
					this.OnRequestBackToRoot();
				}
			}
		}

		public void SetOnRequestBackToRoot(Action onRequestBackToRoot)
		{
			this.mOnRequestBackToRoot = onRequestBackToRoot;
		}

		private void OnRequestBackToRoot()
		{
			if (this.mOnRequestBackToRoot != null)
			{
				this.mOnRequestBackToRoot.Invoke();
			}
		}

		public void Initialize(Mst_bgm_jukebox bgmJukeBoxModel)
		{
			this.mMst_bgm_jukebox = bgmJukeBoxModel;
			string title = string.Format("「{0}」リクエスト中♪", this.mMst_bgm_jukebox.Name);
			this.mUIJukeBoxMusicPlayingRollLabel.Initialize(title);
			List<UIButton> list = new List<UIButton>();
			list.Add(this.mButton_Negative);
			bool flag = this.mMst_bgm_jukebox.Bgm_flag == 1;
			if (flag)
			{
				list.Add(this.mButton_Positive);
				this.mButton_Positive.SetState(UIButtonColor.State.Normal, true);
				this.mButton_Positive.set_enabled(true);
				this.mButton_Positive.isEnabled = true;
			}
			else
			{
				this.mButton_Positive.SetState(UIButtonColor.State.Disabled, true);
				this.mButton_Positive.set_enabled(false);
				this.mButton_Positive.isEnabled = false;
			}
			this.mButtonFocasable = list.ToArray();
			this.mButtonManager.UpdateButtons(this.mButtonFocasable);
			this.ChangeFocus(this.mButton_Negative);
		}

		public void SetKeyController(KeyControl keyController)
		{
			this.mKeyController = keyController;
		}

		private void OnClickPositive()
		{
			this.SelectPositive();
		}

		private void OnClickNegative()
		{
			this.SelectNegative();
		}

		[Obsolete("Inspector上で設定して使用します")]
		public void OnTouchPositive()
		{
			this.SelectPositive();
		}

		[Obsolete("Inspector上で設定して使用します")]
		public void OnTouchNegative()
		{
			this.SelectNegative();
		}

		public void SetOnSelectPositiveListener(Action listener)
		{
			this.mOnSelectPositiveListener = listener;
		}

		public void SetOnSelectNegativeListener(Action listener)
		{
			this.mOnSelectNegativeListener = listener;
		}

		private void SelectPositive()
		{
			if (this.mOnSelectPositiveListener != null)
			{
				this.mOnSelectPositiveListener.Invoke();
			}
		}

		private void SelectNegative()
		{
			if (this.mOnSelectNegativeListener != null)
			{
				this.mOnSelectNegativeListener.Invoke();
			}
		}

		private void ChangeFocus(UIButton changeTarget)
		{
			if (this.mButtonCurrentFocus != null)
			{
				if (this.mButtonCurrentFocus.isEnabled)
				{
					this.mButtonCurrentFocus.SetState(UIButtonColor.State.Normal, true);
				}
				else
				{
					this.mButtonCurrentFocus.SetState(UIButtonColor.State.Disabled, true);
				}
			}
			this.mButtonCurrentFocus = changeTarget;
			if (this.mButtonCurrentFocus != null)
			{
				this.mButtonCurrentFocus.SetState(UIButtonColor.State.Hover, true);
			}
		}

		public void StartState()
		{
			bool flag = this.mMst_bgm_jukebox.Bgm_flag == 1;
			if (flag)
			{
				this.mButton_Positive.SetState(UIButtonColor.State.Normal, true);
				this.mButton_Positive.set_enabled(true);
				this.mButton_Positive.isEnabled = true;
			}
			else
			{
				this.mButton_Positive.SetState(UIButtonColor.State.Disabled, true);
				this.mButton_Positive.set_enabled(false);
				this.mButton_Positive.isEnabled = false;
			}
			bool flag2 = DOTween.IsTweening(this);
			if (flag2)
			{
				DOTween.Kill(this, false);
			}
			TweenSettingsExtensions.SetId<Tweener>(DOVirtual.Float(this.mPanelThis.alpha, 1f, 0.3f, delegate(float alpha)
			{
				this.mPanelThis.alpha = alpha;
			}), this);
			this.mUIJukeBoxMusicPlayingRollLabel.StartRoll();
			Tween tween = this.GenerateTweenYouseiSwing();
			Tween tween2 = this.GenerateTweenYouseiMarch();
			Tween tween3 = this.GenerateTweenYouseiMove();
		}

		public void CloseState()
		{
			bool flag = DOTween.IsTweening(this);
			if (flag)
			{
				DOTween.Kill(this, false);
			}
			this.mButtonFocasable = null;
			this.mKeyController = null;
			TweenSettingsExtensions.SetId<Tweener>(DOVirtual.Float(this.mPanelThis.alpha, 0f, 0.3f, delegate(float alpha)
			{
				this.mPanelThis.alpha = alpha;
			}), this);
		}

		private Tween GenerateTweenYouseiSwing()
		{
			TweenCallback tweenCallback = delegate
			{
				this.mTexture_Yousei.get_transform().set_localRotation(Quaternion.Euler(new Vector3(0f, 0f, 0f)));
			};
			TweenCallback tweenCallback2 = delegate
			{
				this.mTexture_Yousei.get_transform().set_localRotation(Quaternion.Euler(Vector3.get_zero()));
			};
			TweenCallback tweenCallback3 = delegate
			{
				this.mTexture_Yousei.get_transform().set_localRotation(Quaternion.Euler(new Vector3(0f, 0f, 6f)));
			};
			Sequence sequence = TweenSettingsExtensions.SetId<Sequence>(DOTween.Sequence(), this);
			TweenSettingsExtensions.OnPlay<Sequence>(sequence, tweenCallback);
			TweenSettingsExtensions.AppendInterval(sequence, 1f);
			TweenSettingsExtensions.AppendCallback(sequence, tweenCallback3);
			TweenSettingsExtensions.AppendInterval(sequence, 1f);
			TweenSettingsExtensions.AppendCallback(sequence, tweenCallback2);
			TweenSettingsExtensions.SetLoops<Sequence>(sequence, 2147483647, 0);
			return sequence;
		}

		private Tween GenerateTweenYouseiMove()
		{
			Sequence sequence = DOTween.Sequence();
			TweenSettingsExtensions.SetId<Sequence>(sequence, this);
			TweenCallback tweenCallback = delegate
			{
				this.mTransform_YouseiOffset.localPositionX(250f);
				this.mTransform_YouseiOffset.set_localRotation(Quaternion.Euler(Vector3.get_zero()));
			};
			Tween tween = TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMoveX(this.mTransform_YouseiOffset, -380f, 5f, false), 1);
			Tween tween2 = ShortcutExtensions.DOLocalRotate(this.mTransform_YouseiOffset, new Vector3(0f, 180f, 0f), 1f, 0);
			Tween tween3 = TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMoveX(this.mTransform_YouseiOffset, 250f, 5f, false), 1);
			Tween tween4 = ShortcutExtensions.DOLocalRotate(this.mTransform_YouseiOffset, new Vector3(0f, 0f, 0f), 1f, 0);
			TweenSettingsExtensions.OnPlay<Sequence>(sequence, tweenCallback);
			TweenSettingsExtensions.Append(sequence, tween);
			TweenSettingsExtensions.Append(sequence, tween2);
			TweenSettingsExtensions.Append(sequence, tween3);
			TweenSettingsExtensions.Append(sequence, tween4);
			TweenSettingsExtensions.SetLoops<Sequence>(sequence, 2147483647, 0);
			return sequence;
		}

		private Tween GenerateTweenYouseiMarch()
		{
			TweenCallback tweenCallback = delegate
			{
				this.mTexture_Yousei.mainTexture = this.mTexture2d_Yousei_Frame_0;
			};
			TweenCallback tweenCallback2 = delegate
			{
				this.mTexture_Yousei.mainTexture = this.mTexture2d_Yousei_Frame_0;
			};
			TweenCallback tweenCallback3 = delegate
			{
				this.mTexture_Yousei.mainTexture = this.mTexture2d_Yousei_Frame_1;
			};
			TweenCallback tweenCallback4 = delegate
			{
				this.mTexture_Yousei.mainTexture = this.mTexture2d_Yousei_Frame_2;
			};
			Sequence sequence = DOTween.Sequence();
			TweenSettingsExtensions.SetId<Sequence>(sequence, this);
			TweenSettingsExtensions.OnPlay<Sequence>(sequence, tweenCallback);
			TweenSettingsExtensions.AppendCallback(sequence, tweenCallback2);
			TweenSettingsExtensions.AppendInterval(sequence, 0.1f);
			TweenSettingsExtensions.AppendCallback(sequence, tweenCallback3);
			TweenSettingsExtensions.AppendInterval(sequence, 0.1f);
			TweenSettingsExtensions.AppendCallback(sequence, tweenCallback4);
			TweenSettingsExtensions.AppendInterval(sequence, 0.1f);
			TweenSettingsExtensions.SetLoops<Sequence>(sequence, 2147483647, 0);
			return null;
		}

		public void Release()
		{
			this.mTexture_Yousei.mainTexture = null;
			this.mTexture_Yousei = null;
			this.mTexture2d_Yousei_Frame_0 = null;
			this.mTexture2d_Yousei_Frame_1 = null;
			this.mTexture2d_Yousei_Frame_2 = null;
			this.mButtonManager = null;
			this.mPanelThis = null;
			this.mButton_Negative.onClick.Clear();
			this.mButton_Negative = null;
			this.mButton_Positive.onClick.Clear();
			this.mButton_Positive = null;
			this.mKeyController = null;
			this.mButtonFocasable = null;
		}

		[Obsolete("Inspector上で設定して使用します")]
		public void OnTouchYousei()
		{
			if (!DOTween.IsTweening(this.mTexture_Yousei))
			{
				Tween tween = TweenSettingsExtensions.SetId<Tweener>(TweenSettingsExtensions.SetLoops<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMoveY(this.mTransform_YouseiOffset, (float)Random.Range(55, 80), 0.5f, false), 9), 2, 1), this.mTexture_Yousei);
			}
		}

		public void SetOnRequestChangeScene(Action onRequestChangeScene)
		{
			this.mOnRequestChangeScene = onRequestChangeScene;
		}

		private void OnRequestChangeScene()
		{
			if (this.mOnRequestChangeScene != null)
			{
				this.mOnRequestChangeScene.Invoke();
			}
		}

		private void OnDestroy()
		{
			this.mButtonManager = null;
			this.mPanelThis = null;
			this.mTransform_YouseiOffset = null;
			this.mTexture_Yousei = null;
			this.mTexture2d_Yousei_Frame_0 = null;
			this.mTexture2d_Yousei_Frame_1 = null;
			this.mTexture2d_Yousei_Frame_2 = null;
			this.mButton_Negative = null;
			this.mButton_Positive = null;
			this.mUIJukeBoxMusicPlayingRollLabel = null;
			this.mOnRequestBackToRoot = null;
			this.mOnSelectPositiveListener = null;
			this.mOnSelectNegativeListener = null;
			this.mButtonCurrentFocus = null;
			this.mOnRequestChangeScene = null;
			this.mMst_bgm_jukebox = null;
			this.mKeyController = null;
			this.mButtonFocasable = null;
		}
	}
}
