using KCV.Utils;
using local.models;
using Server_Models;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.PortTop
{
	public class MarriageCutManager : MonoBehaviour
	{
		private int _debugIndex;

		private bool isControl;

		private Action _callback;

		private ShipModelMst _shipModelMst;

		private KeyControl _keyController;

		[SerializeField]
		private GameObject _objScene2;

		[SerializeField]
		private GameObject _objScene3;

		[SerializeField]
		private GameObject _objScene5;

		[SerializeField]
		private UITexture _uiShip;

		[SerializeField]
		private UITexture _uiBg;

		[SerializeField]
		private ParticleSystem _uiFeatherPar;

		[SerializeField]
		private ParticleSystem _uiLightPar;

		[SerializeField]
		private ParticleSystem _uiPetalPar1;

		[SerializeField]
		private ParticleSystem _uiPetalPar2;

		[SerializeField]
		private Animation _btnAnime;

		[SerializeField]
		private Animation _ringAnime;

		[SerializeField]
		private Animation _anime;

		[SerializeField]
		private Camera _blurCamera;

		public float Alpha
		{
			get
			{
				return base.GetComponent<UIPanel>().alpha;
			}
			set
			{
				base.GetComponent<UIPanel>().alpha = this.Alpha;
			}
		}

		private void init()
		{
			this.isControl = false;
			base.GetComponent<UIPanel>().alpha = 0f;
			if (this._objScene2 == null)
			{
				this._objScene2 = base.get_transform().FindChild("Camera/GameObject").get_gameObject();
			}
			if (this._objScene3 == null)
			{
				this._objScene3 = base.get_transform().FindChild("Camera/Scene3").get_gameObject();
			}
			if (this._objScene5 == null)
			{
				this._objScene5 = base.get_transform().FindChild("Camera/Scene5").get_gameObject();
			}
			Util.FindParentToChild<UITexture>(ref this._uiShip, base.get_transform(), "Camera/ShipObject/Character");
			Util.FindParentToChild<UITexture>(ref this._uiBg, base.get_transform(), "Camera/GameObject/Bg");
			Util.FindParentToChild<ParticleSystem>(ref this._uiFeatherPar, base.get_transform(), "CameraBlur/FeatherPar");
			Util.FindParentToChild<ParticleSystem>(ref this._uiLightPar, base.get_transform(), "Camera/LightPar");
			Util.FindParentToChild<ParticleSystem>(ref this._uiPetalPar1, base.get_transform(), "Camera/GameObject/MarriagePetal");
			Util.FindParentToChild<ParticleSystem>(ref this._uiPetalPar2, base.get_transform(), "Camera/MarriagePetal2");
			Util.FindParentToChild<Animation>(ref this._btnAnime, base.get_transform(), "Camera/Button");
			Util.FindParentToChild<Animation>(ref this._ringAnime, base.get_transform(), "Camera/Scene5/RingObj");
			Util.FindParentToChild<Camera>(ref this._blurCamera, base.get_transform(), "CameraBlur");
			if (this._anime == null)
			{
				this._anime = base.GetComponent<Animation>();
			}
			UIButtonMessage component = base.get_transform().FindChild("Camera/Button").GetComponent<UIButtonMessage>();
			component.target = base.get_gameObject();
			component.functionName = "ButtonClick";
			component.trigger = UIButtonMessage.Trigger.OnClick;
			this._objScene3.SetActive(false);
			this._objScene5.SetActive(false);
		}

		private void OnDestroy()
		{
			Mem.Del<Action>(ref this._callback);
			Mem.Del<ShipModelMst>(ref this._shipModelMst);
			Mem.Del<GameObject>(ref this._objScene2);
			Mem.Del<GameObject>(ref this._objScene3);
			Mem.Del<UITexture>(ref this._uiShip);
			Mem.Del(ref this._uiFeatherPar);
			Mem.Del(ref this._uiLightPar);
			Mem.Del(ref this._uiPetalPar1);
			Mem.Del<Animation>(ref this._btnAnime);
			Mem.Del<Animation>(ref this._ringAnime);
			Mem.Del<Animation>(ref this._anime);
			Mem.Del<Camera>(ref this._blurCamera);
		}

		private void Update()
		{
			if (this.isControl && this._keyController.keyState.get_Item(1).down)
			{
				this.isControl = false;
				this.ButtonClick();
			}
		}

		public void Initialize(ShipModelMst model, KeyControl kCtrl, Action callback)
		{
			this.init();
			this._callback = callback;
			this._shipModelMst = model;
			this._keyController = kCtrl;
			this.setShipTexture();
		}

		public void Initialize(int graphicShipId, KeyControl kCtrl, Action callback)
		{
			this.init();
			this._callback = callback;
			this._shipModelMst = new ShipModelMst(graphicShipId);
			this._keyController = kCtrl;
			this.setShipTexture();
		}

		[DebuggerHidden]
		public IEnumerator Play()
		{
			MarriageCutManager.<Play>c__Iterator73 <Play>c__Iterator = new MarriageCutManager.<Play>c__Iterator73();
			<Play>c__Iterator.<>f__this = this;
			return <Play>c__Iterator;
		}

		private void setShipTexture()
		{
			int num = (this._shipModelMst != null) ? this._shipModelMst.GetGraphicsMstId() : 1;
			this._uiShip.mainTexture = ShipUtils.LoadTexture(num, 9);
			this._uiShip.MakePixelPerfect();
			this._uiShip.get_transform().set_localPosition(Util.Poi2Vec(new ShipOffset(num).GetFace(false)));
		}

		private void _setShipPosition(int index)
		{
			switch (index)
			{
			case 0:
				this._uiShip.get_transform().set_localPosition(Util.Poi2Vec(this._shipModelMst.Offsets.GetFoot_InBattle(false)));
				break;
			case 1:
				this._uiShip.get_transform().set_localPosition(Util.Poi2Vec(new ShipOffset(this._shipModelMst.GetGraphicsMstId()).GetFace(false)));
				break;
			case 2:
				this._uiShip.get_transform().set_localPosition(Util.Poi2Vec(new ShipOffset(this._shipModelMst.GetGraphicsMstId()).GetFace(false)));
				break;
			}
		}

		private void playFeatherParticle()
		{
			this._uiFeatherPar.Stop();
			this._uiFeatherPar.Play();
		}

		private void playPetalParticle1()
		{
			this._uiPetalPar1.Stop();
			this._uiPetalPar1.Play();
		}

		private void playPetalParticle2()
		{
			this._uiPetalPar2.Stop();
			this._uiPetalPar2.Play();
		}

		private void playShipVoice()
		{
			ShipUtils.PlayShipVoice(this._shipModelMst, 24);
		}

		private void playUpDownLoopAnime()
		{
			this._ringAnime.Stop();
			this._ringAnime.Play("MarriageUpDown");
		}

		private void _debugShipPos()
		{
			Debug.Log("ShipID:" + this._debugIndex);
			if (Mst_DataManager.Instance.Mst_shipgraph.ContainsKey(this._debugIndex))
			{
				IReward_Ship reward_Ship = new Reward_Ship(this._debugIndex);
				this._uiShip.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(reward_Ship.Ship.GetGraphicsMstId(), 9);
				this._uiShip.MakePixelPerfect();
				this._uiShip.get_transform().set_localPosition(Util.Poi2Vec(new ShipOffset(reward_Ship.Ship.GetGraphicsMstId()).GetShipDisplayCenter(false)));
				this._uiShip.alpha = 1f;
			}
		}

		private void sceneFinished(int index)
		{
			this._anime.Stop();
			switch (index)
			{
			case 1:
				this._uiFeatherPar.Stop();
				this._blurCamera.SetActive(false);
				this._uiLightPar.Stop();
				this._uiLightPar.Play();
				this._anime.Play("Marriage2");
				break;
			case 2:
				this._objScene3.SetActive(true);
				this._uiLightPar.Stop();
				this._uiLightPar.Play();
				this._anime.Play("Marriage3");
				break;
			case 3:
				this._objScene2.SetActive(false);
				this._anime.Play("Marriage4");
				break;
			case 4:
			{
				UITexture component = base.get_transform().FindChild("Camera/Vignette").GetComponent<UITexture>();
				component.alpha = 0f;
				this._objScene5.SetActive(true);
				this._objScene2.SetActive(true);
				this._anime.Play("Marriage5");
				break;
			}
			case 5:
				this._objScene5.SetActive(false);
				this._ringAnime.get_gameObject().SetActive(false);
				this._uiLightPar.Stop();
				this._uiBg.mainTexture = (Resources.Load("Textures/Marriage/bg_seane6") as Texture2D);
				this._anime.Play("Marriage6");
				break;
			case 6:
				this.isControl = true;
				this._btnAnime.get_transform().set_localPosition(new Vector3(410f, -200f, 0f));
				this._btnAnime.Stop();
				this._btnAnime.Play("NextIcon");
				break;
			}
		}

		public void ButtonClick()
		{
			base.GetComponent<UIPanel>().get_gameObject().SafeGetTweenAlpha(1f, 0f, 4f, 1f, UITweener.Method.Linear, UITweener.Style.Once, base.get_gameObject(), string.Empty);
			if (this._callback != null)
			{
				this._callback.Invoke();
			}
			this.isControl = false;
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
		}

		private void _finishedMarriage()
		{
		}
	}
}
