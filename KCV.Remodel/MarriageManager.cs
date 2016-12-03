using KCV.Scene.Port;
using KCV.Utils;
using local.models;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Remodel
{
	[RequireComponent(typeof(UIPanel))]
	public class MarriageManager : MonoBehaviour
	{
		public delegate void OnFinish();

		private UIPanel mPanelThis;

		private bool isKeyControllable;

		private bool isButtonPressed;

		private KeyControl mKeyController;

		[SerializeField]
		private UITexture bg;

		[SerializeField]
		private UITexture white;

		[SerializeField]
		private UITexture vignette;

		[SerializeField]
		private UITexture ch;

		private int id;

		[SerializeField]
		private UISprite[] giveRing;

		[SerializeField]
		private UISprite[] ringBox;

		[SerializeField]
		private GameObject featherInit;

		private GameObject[] feathers;

		[SerializeField]
		private GameObject flareInit;

		private GameObject[] flares;

		[SerializeField]
		private GameObject petalInit;

		private GameObject[] petals;

		[SerializeField]
		private GameObject sparkleInit;

		private GameObject[] sparkles;

		[SerializeField]
		private UISprite[] letters;

		[SerializeField]
		private GameObject btn;

		[SerializeField]
		private Blur blur;

		[SerializeField]
		private Texture2D[] bgTexs;

		private bool on;

		private bool finish;

		private bool floating;

		private float startTime;

		private MarriageManager.OnFinish Callback;

		private readonly Vector3[] CHARACTER_POSITIONS = new Vector3[]
		{
			new Vector3(7f, -51f, 0f),
			new Vector3(240f, 576f, 0f),
			new Vector3(-96f, 82f, 0f),
			new Vector3(230f, -293f, 0f)
		};

		private readonly Vector3[] CHARACTER_POSITION_TOS = new Vector3[]
		{
			new Vector3(-51f, -96f, 0f),
			new Vector3(288f, 640f, 0f),
			new Vector3(-40f, 105f, 0f),
			new Vector3(112f, -216f, 0f)
		};

		private readonly Vector3[] CHARACTER_SCALES = new Vector3[]
		{
			Vector3.get_one() * 1.5f,
			Vector3.get_one() * 2f,
			Vector3.get_one() * 1.7f,
			Vector3.get_one() * 2.5f
		};

		private readonly Vector3[] CHARACTER_SCALE_TOS = new Vector3[]
		{
			Vector3.get_one() * 1.7f,
			Vector3.get_one() * 2.5f,
			Vector3.get_one() * 1.6f,
			Vector3.get_one() * 2.2f
		};

		private ShipModelMst mTargetShipModelMst;

		public float alpha
		{
			get
			{
				if (this.mPanelThis != null)
				{
					return this.mPanelThis.alpha;
				}
				return -1f;
			}
			set
			{
				if (this.mPanelThis != null)
				{
					this.mPanelThis.alpha = this.alpha;
				}
			}
		}

		private void Awake()
		{
			this.mPanelThis = base.GetComponent<UIPanel>();
			this.id = -1;
			this.on = false;
			this.finish = false;
			this.floating = false;
			this.startTime = 0f;
		}

		private void Update()
		{
			if (this.on && this.floating)
			{
				for (int i = 0; i < 3; i++)
				{
					this.giveRing[i].get_transform().set_localPosition((-100f + 10f * (float)Math.Sin((double)(Time.get_time() * 1.2f))) * Vector3.get_up());
				}
			}
			if (this.isKeyControllable && this.mKeyController.keyState.get_Item(1).down)
			{
				this.isKeyControllable = false;
				this.ButtonClick();
			}
		}

		public void Initialize(ShipModelMst targetShipModel, KeyControl kCtrl, MarriageManager.OnFinish func = null)
		{
			this.on = true;
			this.startTime = Time.get_time();
			this.Callback = func;
			this.mTargetShipModelMst = targetShipModel;
			this.ch.mainTexture = ShipUtils.LoadTexture(this.mTargetShipModelMst.GetGraphicsMstId(), 9 + Convert.ToInt32(false));
			this.ch.MakePixelPerfect();
			this.mKeyController = kCtrl;
		}

		[DebuggerHidden]
		public IEnumerator PlayAnimation()
		{
			MarriageManager.<PlayAnimation>c__IteratorAF <PlayAnimation>c__IteratorAF = new MarriageManager.<PlayAnimation>c__IteratorAF();
			<PlayAnimation>c__IteratorAF.<>f__this = this;
			return <PlayAnimation>c__IteratorAF;
		}

		public void BGAlpha(float f)
		{
			this.bg.alpha = f;
		}

		public void Blur(float f)
		{
			this.blur.blurSize = f;
		}

		public void WhiteAlpha(float f)
		{
			this.white.alpha = f;
		}

		public void CharAlpha(float f)
		{
			this.ch.alpha = f;
		}

		public void RingBoxAlpha(float f)
		{
			this.ringBox[1].alpha = f;
		}

		public void GiveRingBoxClosedAlpha(float f)
		{
			this.giveRing[0].alpha = f;
		}

		public void GiveRingBoxAlpha(float f)
		{
			this.giveRing[1].alpha = f;
		}

		public void GiveRingAlpha(float f)
		{
			this.giveRing[2].alpha = f;
		}

		public void SubtitleAlpha(float f)
		{
			this.letters[16].alpha = f;
		}

		public void ButtonClick()
		{
			if (this.isButtonPressed)
			{
				return;
			}
			this.isButtonPressed = true;
			for (int i = 0; i < this.petals.Length; i++)
			{
				Object.Destroy(this.petals[i]);
			}
			if (this.Callback != null)
			{
				this.Callback();
			}
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Releases(ref this.giveRing);
			UserInterfacePortManager.ReleaseUtils.Releases(ref this.ringBox);
			UserInterfacePortManager.ReleaseUtils.Releases(ref this.letters);
			UserInterfacePortManager.ReleaseUtils.Releases(ref this.feathers);
			UserInterfacePortManager.ReleaseUtils.Releases(ref this.flares);
			UserInterfacePortManager.ReleaseUtils.Releases(ref this.petals);
			UserInterfacePortManager.ReleaseUtils.Releases(ref this.sparkles);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mPanelThis);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.bg, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.white, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.vignette, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.ch, false);
			this.mKeyController = null;
			this.featherInit = null;
			this.flareInit = null;
			this.petalInit = null;
			this.sparkleInit = null;
			this.btn = null;
			this.blur = null;
			this.bgTexs = null;
			this.Callback = null;
		}
	}
}
