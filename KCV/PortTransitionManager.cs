using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV
{
	public class PortTransitionManager : MonoBehaviour
	{
		[SerializeField]
		private UIPanel TransitionPanel;

		[SerializeField]
		private UITexture TransitionBG;

		[SerializeField]
		private Camera TransitionCamera;

		[Button("DebugTransition", "StartTransition", new object[]
		{

		})]
		public int button1;

		[Button("Reset", "Reset", new object[]
		{

		})]
		public int button2;

		private Action onfinished;

		private Action SettingOnFinished;

		public bool isOver;

		[NonSerialized]
		public bool isTransitionNow;

		private Vector3 defaultPosition;

		private Dictionary<Generics.Scene, string> SceneBG_FilePaths;

		private void Start()
		{
			this.SceneBG_FilePaths = new Dictionary<Generics.Scene, string>();
			this.SceneBG_FilePaths.Add(Generics.Scene.Arsenal, "Textures/Arsenal/arsenal_bg");
			this.SceneBG_FilePaths.Add(Generics.Scene.ImprovementArsenal, "Textures/Common/BG/arsenal2_bg");
			this.SceneBG_FilePaths.Add(Generics.Scene.Record, "Textures/Common/BG/CommonBG");
			this.SceneBG_FilePaths.Add(Generics.Scene.Organize, "Textures/Organize/organize_bg");
			this.SceneBG_FilePaths.Add(Generics.Scene.Remodel, "Textures/Common/BG/common3");
			this.SceneBG_FilePaths.Add(Generics.Scene.Duty, "Textures/Common/BG/hex_bg");
			this.SceneBG_FilePaths.Add(Generics.Scene.Repair, "Textures/repair/NewUI/bg/supply_set");
			this.SceneBG_FilePaths.Add(Generics.Scene.Supply, "Textures/Common/BG/hex_bg");
			this.SceneBG_FilePaths.Add(Generics.Scene.Item, "Textures/Item/item_bg2");
			this.SceneBG_FilePaths.Add(Generics.Scene.Album, "Textures/Album/album_bg");
			this.defaultPosition = new Vector3(this.TransitionPanel.get_transform().get_localPosition().x, this.TransitionPanel.get_transform().get_localPosition().y, this.TransitionPanel.get_transform().get_localPosition().z);
			this.SetActiveChildren(false);
		}

		public void StartTransition(Generics.Scene NextScene, bool isPortFramePos, Action act)
		{
			this.SetActiveChildren(true);
			this.setCameraDepth(NextScene);
			if (isPortFramePos)
			{
				this.setOnPortCirclePosition();
			}
			else
			{
				this.setDefaultPosition();
			}
			if (SingletonMonoBehaviour<UIPortFrame>.exist())
			{
				SingletonMonoBehaviour<UIPortFrame>.Instance.fadeOutCircleButtonLabel();
			}
			this.Reset();
			this.setBG(NextScene);
			iTween.ValueTo(base.get_gameObject(), iTween.Hash(new object[]
			{
				"from",
				0.1,
				"to",
				2500,
				"time",
				0.4f,
				"onupdate",
				"UpdateHandler",
				"oncomplete",
				"OnFinished"
			}));
			this.onfinished = act;
			this.isOver = true;
		}

		private void setCameraDepth(Generics.Scene NextScene)
		{
			if (NextScene == Generics.Scene.Album || NextScene == Generics.Scene.Interior)
			{
				this.TransitionCamera.set_depth(5f);
			}
			else
			{
				this.TransitionCamera.set_depth(1.3f);
			}
		}

		private void setOnPortCirclePosition()
		{
			this.TransitionPanel.get_transform().set_localPosition(new Vector3(-415f, 210f, 0f));
			this.TransitionBG.get_transform().set_localPosition(new Vector3(415f, -210f, 0f));
		}

		private void setDefaultPosition()
		{
			this.TransitionPanel.get_transform().set_localPosition(this.defaultPosition);
			this.TransitionBG.get_transform().set_localPosition(-this.defaultPosition);
		}

		public void EndTransition(Action act, bool isLockTouchOff = true, bool isPortFrameColliderEnable = true)
		{
			if (!this.isOver)
			{
				SingletonMonoBehaviour<FadeCamera>.Instance.FadeIn(0.2f, delegate
				{
					if (SingletonMonoBehaviour<UIShortCutMenu>.exist())
					{
						SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
						SingletonMonoBehaviour<UIShortCutMenu>.Instance.LockOffControl();
						SingletonMonoBehaviour<UIShortCutMenu>.Instance.LockTouchControl(!isLockTouchOff);
					}
					if (act != null)
					{
						act.Invoke();
						act = null;
						this.SetActiveChildren(false);
					}
					if (SingletonMonoBehaviour<UIPortFrame>.exist() && isPortFrameColliderEnable)
					{
						SingletonMonoBehaviour<UIPortFrame>.Instance.isColliderEnabled = true;
					}
					this.isTransitionNow = false;
				});
				return;
			}
			TweenAlpha tweenAlpha = TweenAlpha.Begin(this.TransitionPanel.get_gameObject(), 0.4f, 0f);
			tweenAlpha.onFinished.Clear();
			tweenAlpha.SetOnFinished(delegate
			{
				if (SingletonMonoBehaviour<UIShortCutMenu>.exist())
				{
					SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
					Debug.Log("transitionEnd");
					SingletonMonoBehaviour<UIShortCutMenu>.Instance.LockOffControl();
					SingletonMonoBehaviour<UIShortCutMenu>.Instance.LockTouchControl(!isLockTouchOff);
				}
				this.TransitionPanel.SetRect(0f, 0f, 0.1f, 0.1f);
				if (act != null)
				{
					act.Invoke();
					act = null;
					this.SetActiveChildren(false);
				}
				if (SingletonMonoBehaviour<UIPortFrame>.exist())
				{
					SingletonMonoBehaviour<UIPortFrame>.Instance.isColliderEnabled = true;
				}
				this.isOver = false;
				this.isTransitionNow = false;
			});
		}

		private void setBG(Generics.Scene NextScene)
		{
			if (this.SceneBG_FilePaths.ContainsKey(NextScene))
			{
				this.TransitionBG.mainTexture = (Resources.Load(this.SceneBG_FilePaths.get_Item(NextScene)) as Texture);
			}
			else
			{
				this.TransitionBG.mainTexture = (Resources.Load("Textures/Common/BG/CommonBG") as Texture);
			}
			if (NextScene == Generics.Scene.Organize || NextScene == Generics.Scene.Arsenal || NextScene == Generics.Scene.Repair)
			{
				this.TransitionBG.width = 1040;
				this.TransitionBG.height = 589;
			}
			else
			{
				this.TransitionBG.width = 960;
				this.TransitionBG.height = 544;
			}
		}

		private void UpdateHandler(float value)
		{
			this.TransitionPanel.SetRect(0f, 0f, value, value);
		}

		private void OnFinished()
		{
			if (this.onfinished != null)
			{
				this.onfinished.Invoke();
				this.setDefaultPosition();
				this.onfinished = null;
			}
		}

		private void Reset()
		{
			this.TransitionPanel.alpha = 1f;
			this.TransitionPanel.SetRect(0f, 0f, 0.1f, 0.1f);
		}

		private void DebugTransition()
		{
			this.Reset();
			iTween.ValueTo(base.get_gameObject(), iTween.Hash(new object[]
			{
				"from",
				0.1,
				"to",
				2500,
				"time",
				0.4f,
				"onupdate",
				"UpdateHandler",
				"oncomplete",
				"OnFinished"
			}));
		}

		public void setOnFinished(Action onfinished)
		{
			this.SettingOnFinished = onfinished;
		}

		private void OnDestroy()
		{
			this.TransitionPanel = null;
			this.TransitionBG = null;
			this.TransitionCamera = null;
			this.onfinished = null;
			this.SettingOnFinished = null;
			if (this.SceneBG_FilePaths != null)
			{
				Mem.DelDictionarySafe<Generics.Scene, string>(ref this.SceneBG_FilePaths);
			}
		}
	}
}
