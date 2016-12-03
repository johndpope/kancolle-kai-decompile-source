using KCV.Scene.Port;
using KCV.Utils;
using local.models;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Remodel
{
	public class PortUpgradesModernizeShipManager : MonoBehaviour
	{
		[SerializeField]
		private UITexture mTexture_Fade;

		[SerializeField]
		private GameObject mGameObject_Upgrade;

		[SerializeField]
		private UITexture mTexture_Background;

		[SerializeField]
		private UITexture mTexture_Ship;

		[SerializeField]
		private GameObject mGameObject_SparkleInit;

		[SerializeField]
		private GameObject mGameObjects_UpSpiritInit;

		[SerializeField]
		private GameObject mGameObject_UpStripe;

		[SerializeField]
		private UISprite mSprite_UpText;

		[SerializeField]
		private UITexture mSprite_UpText_S;

		[SerializeField]
		private GameObject mGameObject_Failed;

		[SerializeField]
		private UITexture mTexture_FailBackground;

		[SerializeField]
		private UISprite mSprite_FailBox;

		[SerializeField]
		private UISprite mSprite_FailDashInit;

		[SerializeField]
		private PortUpgradesModernizeShipLeaf mPortUpgradesModernizeShipLeaf_FailLeaf;

		[SerializeField]
		private UISprite mSprite_FailSpotTop;

		[SerializeField]
		private UISprite mSprite_FailSpotBottom;

		[SerializeField]
		private UISprite mSprite_FailTextbox;

		[SerializeField]
		private PortUpgradesModernizeShipText mPortUpgradesModernizeShipText_FailText;

		[SerializeField]
		private PortUpgradesModernizeShipReturnButton mPortUpgradesModernizeShipReturnButton_ReturnButton;

		[SerializeField]
		private GameObject kamihubuki;

		private GameObject[] mGameObjects_FailDashe = new GameObject[20];

		private GameObject[] mGameObjects_UpSpirit = new GameObject[5];

		private GameObject[] mGameObjects_UpSparkle = new GameObject[16];

		private bool[,] elmts;

		private int cnt;

		private bool fail;

		private bool on;

		private bool finish;

		private float timer;

		public bool isFinished;

		private ShipModelMst mTargetShipModelMst;

		private bool _isDamaged;

		private bool _isSuperSucessed;

		private KeyControl mKeyController;

		private bool enabledKey;

		private Coroutine waitAndVoiceCoroutine;

		public void Awake()
		{
			this.mTexture_Fade.alpha = 0f;
			this.mTexture_Background.alpha = 0f;
			this.mTexture_Ship.alpha = 0f;
			this.mGameObject_UpStripe.get_transform().set_localScale(new Vector3(1f, 0.01f, 1f));
			this.mGameObject_UpStripe.SetActive(false);
			this.mSprite_FailSpotTop.alpha = 0f;
			this.mSprite_FailSpotBottom.alpha = 0f;
			this.cnt = -1;
			this.fail = false;
			this.on = false;
			this.finish = false;
			this.isFinished = false;
			this.timer = 0f;
		}

		public void Initialize(ShipModelMst targetShipModelMst, int bgID, bool fail, bool SuperSuccessed, int sozai_count)
		{
			this.Initialize(targetShipModelMst, bgID, fail, SuperSuccessed, sozai_count, false);
		}

		public void Initialize(ShipModelMst targetShipModelMst, int bgID, bool fail, bool SuperSuccessed, int sozai_count, bool isDamaged)
		{
			this.fail = fail;
			this.mTargetShipModelMst = targetShipModelMst;
			this._isDamaged = isDamaged;
			this._isSuperSucessed = SuperSuccessed;
			this.on = true;
			this.timer = Time.get_time();
			if (isDamaged)
			{
				this.mTexture_Ship.GetComponent<UITexture>().mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(this.mTargetShipModelMst.GetGraphicsMstId(), 10);
			}
			else
			{
				this.mTexture_Ship.GetComponent<UITexture>().mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(this.mTargetShipModelMst.GetGraphicsMstId(), 9);
			}
			this.mTexture_Ship.GetComponent<UITexture>().MakePixelPerfect();
			this.elmts = new bool[,]
			{
				{
					true,
					true,
					true,
					true
				},
				{
					true,
					true,
					true,
					true
				},
				{
					true,
					true,
					true,
					true
				},
				{
					true,
					true,
					true,
					true
				},
				{
					true,
					true,
					true,
					true
				}
			};
			this.cnt = sozai_count;
			base.StartCoroutine(this.EnableAlphas());
			base.StartCoroutine(this.SpawnSpirits());
			if (fail)
			{
				base.StartCoroutine(this.EnableButton());
				base.StartCoroutine(this.SwapActive());
				base.StartCoroutine(this.LeafBlow());
				base.StartCoroutine(this.Text());
			}
			else
			{
				base.StartCoroutine(this.SpawnSparkle());
				base.StartCoroutine(this.EnableButton());
			}
		}

		public void Update()
		{
			if (this.mKeyController != null && this.mKeyController.keyState.get_Item(1).down && this.enabledKey)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			}
			if (this.on)
			{
				if (Time.get_time() - this.timer <= 1f)
				{
					this.mTexture_Fade.alpha += Mathf.Min(Time.get_deltaTime(), 1f - this.mTexture_Fade.alpha);
				}
				if (Time.get_time() - this.timer >= 1f && Time.get_time() - this.timer <= 2f)
				{
					this.mTexture_Fade.alpha -= Mathf.Min(Time.get_deltaTime(), this.mTexture_Fade.alpha);
				}
				if (Time.get_time() - this.timer >= 1.5f && Time.get_time() - this.timer <= 2f)
				{
					this.ShipSlide();
				}
				if (!this.fail)
				{
					if (Time.get_time() - this.timer >= 4f && Time.get_time() - this.timer <= 4.5f)
					{
						this.StripeExpand();
					}
					if (Time.get_time() - this.timer >= 4f && Time.get_time() - this.timer <= 5.5f)
					{
						if (this._isSuperSucessed)
						{
							if (!this.kamihubuki.get_activeSelf())
							{
								this.kamihubuki.SetActive(true);
							}
							this.TextSlide_S();
						}
						else
						{
							this.TextSlide();
						}
					}
				}
				if (this.fail)
				{
					if (Time.get_time() - this.timer >= 4f && Time.get_time() - this.timer <= 5f)
					{
						this.mTexture_Fade.alpha += Mathf.Min(Time.get_deltaTime(), 1f - this.mTexture_Fade.alpha);
					}
					if (Time.get_time() - this.timer >= 5f && Time.get_time() - this.timer <= 6f)
					{
						this.mTexture_Fade.alpha -= Mathf.Min(Time.get_deltaTime(), this.mTexture_Fade.alpha);
					}
					if (Time.get_time() - this.timer >= 6f && Time.get_time() - this.timer <= 6.5f)
					{
						this.SpotlightFadeIn();
					}
					if (Time.get_time() - this.timer >= 6.5f && Time.get_time() - this.timer <= 7.5f)
					{
						this.TextboxSlide();
					}
				}
				if (this.finish)
				{
					if (this.waitAndVoiceCoroutine != null)
					{
						base.StopCoroutine(this.waitAndVoiceCoroutine);
					}
					this.mTexture_Fade.alpha += Mathf.Min(Time.get_deltaTime(), 1f - this.mTexture_Fade.alpha);
					if (this.mTexture_Fade.alpha == 1f)
					{
						this.isFinished = true;
					}
				}
			}
		}

		private void ShipSlide()
		{
			Vector3 vector = Util.Poi2Vec(new ShipOffset(this.mTargetShipModelMst.GetGraphicsMstId()).GetSlotItemCategory(this._isDamaged));
			this.mTexture_Ship.get_transform().set_localPosition(new Vector3(vector.x - 270f + 44f, vector.y + 29f, 0f));
			this.mTexture_Ship.alpha += Mathf.Min(3f * Time.get_deltaTime(), 1f - this.mTexture_Ship.alpha);
		}

		private void StripeExpand()
		{
			this.mGameObject_UpStripe.SetActive(true);
			Transform expr_17 = this.mGameObject_UpStripe.get_transform();
			expr_17.set_localScale(expr_17.get_localScale() + new Vector3(0f, 2f * Time.get_deltaTime(), 0f));
		}

		private void TextSlide()
		{
			if (this.mSprite_UpText.get_transform().get_localPosition().x > 100f)
			{
				Transform expr_2D = this.mSprite_UpText.get_transform();
				expr_2D.set_localPosition(expr_2D.get_localPosition() - new Vector3(745f * Time.get_deltaTime(), 0f, 0f));
			}
			else
			{
				Transform expr_67 = this.mSprite_UpText.get_transform();
				expr_67.set_localPosition(expr_67.get_localPosition() - new Vector3(200f * Time.get_deltaTime(), 0f, 0f));
			}
		}

		private void TextSlide_S()
		{
			if (this.mSprite_UpText_S.get_transform().get_localPosition().x > 100f)
			{
				Transform expr_2D = this.mSprite_UpText_S.get_transform();
				expr_2D.set_localPosition(expr_2D.get_localPosition() - new Vector3(745f * Time.get_deltaTime(), 0f, 0f));
			}
			else
			{
				Transform expr_67 = this.mSprite_UpText_S.get_transform();
				expr_67.set_localPosition(expr_67.get_localPosition() - new Vector3(200f * Time.get_deltaTime(), 0f, 0f));
			}
		}

		private void SpotlightFadeIn()
		{
			this.mSprite_FailSpotTop.alpha += Mathf.Min(2f * Time.get_deltaTime(), 1f - this.mSprite_FailSpotTop.alpha);
			this.mSprite_FailSpotBottom.alpha += Mathf.Min(2f * Time.get_deltaTime(), 1f - this.mSprite_FailSpotBottom.alpha);
		}

		private void TextboxSlide()
		{
			Transform expr_0B = this.mSprite_FailTextbox.get_transform();
			expr_0B.set_localPosition(expr_0B.get_localPosition() + new Vector3(0f, 177f * Time.get_deltaTime(), 0f));
		}

		[DebuggerHidden]
		public IEnumerator EnableAlphas()
		{
			PortUpgradesModernizeShipManager.<EnableAlphas>c__Iterator1B7 <EnableAlphas>c__Iterator1B = new PortUpgradesModernizeShipManager.<EnableAlphas>c__Iterator1B7();
			<EnableAlphas>c__Iterator1B.<>f__this = this;
			return <EnableAlphas>c__Iterator1B;
		}

		[DebuggerHidden]
		public IEnumerator SpawnSpirits()
		{
			PortUpgradesModernizeShipManager.<SpawnSpirits>c__Iterator1B8 <SpawnSpirits>c__Iterator1B = new PortUpgradesModernizeShipManager.<SpawnSpirits>c__Iterator1B8();
			<SpawnSpirits>c__Iterator1B.<>f__this = this;
			return <SpawnSpirits>c__Iterator1B;
		}

		[DebuggerHidden]
		public IEnumerator SpawnSparkle()
		{
			PortUpgradesModernizeShipManager.<SpawnSparkle>c__Iterator1B9 <SpawnSparkle>c__Iterator1B = new PortUpgradesModernizeShipManager.<SpawnSparkle>c__Iterator1B9();
			<SpawnSparkle>c__Iterator1B.<>f__this = this;
			return <SpawnSparkle>c__Iterator1B;
		}

		[DebuggerHidden]
		public IEnumerator SwapActive()
		{
			PortUpgradesModernizeShipManager.<SwapActive>c__Iterator1BA <SwapActive>c__Iterator1BA = new PortUpgradesModernizeShipManager.<SwapActive>c__Iterator1BA();
			<SwapActive>c__Iterator1BA.<>f__this = this;
			return <SwapActive>c__Iterator1BA;
		}

		[DebuggerHidden]
		public IEnumerator LeafBlow()
		{
			PortUpgradesModernizeShipManager.<LeafBlow>c__Iterator1BB <LeafBlow>c__Iterator1BB = new PortUpgradesModernizeShipManager.<LeafBlow>c__Iterator1BB();
			<LeafBlow>c__Iterator1BB.<>f__this = this;
			return <LeafBlow>c__Iterator1BB;
		}

		[DebuggerHidden]
		public IEnumerator Text()
		{
			PortUpgradesModernizeShipManager.<Text>c__Iterator1BC <Text>c__Iterator1BC = new PortUpgradesModernizeShipManager.<Text>c__Iterator1BC();
			<Text>c__Iterator1BC.<>f__this = this;
			return <Text>c__Iterator1BC;
		}

		[DebuggerHidden]
		public IEnumerator EnableButton()
		{
			PortUpgradesModernizeShipManager.<EnableButton>c__Iterator1BD <EnableButton>c__Iterator1BD = new PortUpgradesModernizeShipManager.<EnableButton>c__Iterator1BD();
			<EnableButton>c__Iterator1BD.<>f__this = this;
			return <EnableButton>c__Iterator1BD;
		}

		private void PlayVoice()
		{
			ShipUtils.PlayShipVoice(this.mTargetShipModelMst, (Random.Range(0, 2) != 0) ? 10 : 9);
			if (this._isSuperSucessed)
			{
				this.waitAndVoiceCoroutine = base.StartCoroutine(this.WaitAndVoice());
			}
		}

		[DebuggerHidden]
		public IEnumerator WaitAndVoice()
		{
			PortUpgradesModernizeShipManager.<WaitAndVoice>c__Iterator1BE <WaitAndVoice>c__Iterator1BE = new PortUpgradesModernizeShipManager.<WaitAndVoice>c__Iterator1BE();
			<WaitAndVoice>c__Iterator1BE.<>f__this = this;
			return <WaitAndVoice>c__Iterator1BE;
		}

		public void SetKeyController(KeyControl keyController)
		{
			this.mKeyController = keyController;
		}

		public void Finish()
		{
			this.finish = true;
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_Fade, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_Background, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_Ship, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mSprite_UpText);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mSprite_UpText_S, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_FailBackground, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mSprite_FailBox);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mSprite_FailDashInit);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mSprite_FailSpotTop);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mSprite_FailSpotBottom);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mSprite_FailTextbox);
			this.mGameObject_Upgrade = null;
			this.mGameObject_SparkleInit = null;
			this.mGameObjects_UpSpiritInit = null;
			this.mGameObject_UpStripe = null;
			this.mGameObject_Failed = null;
			this.mPortUpgradesModernizeShipLeaf_FailLeaf = null;
			this.mPortUpgradesModernizeShipText_FailText = null;
			this.mPortUpgradesModernizeShipReturnButton_ReturnButton = null;
			this.kamihubuki = null;
			if (this.mGameObjects_FailDashe != null)
			{
				for (int i = 0; i < this.mGameObjects_FailDashe.Length; i++)
				{
					this.mGameObjects_FailDashe[i] = null;
				}
			}
			this.mGameObjects_FailDashe = null;
			if (this.mGameObjects_UpSpirit != null)
			{
				for (int j = 0; j < this.mGameObjects_UpSpirit.Length; j++)
				{
					this.mGameObjects_UpSpirit[j] = null;
				}
			}
			this.mGameObjects_UpSpirit = null;
			if (this.mGameObjects_UpSparkle != null)
			{
				for (int k = 0; k < this.mGameObjects_UpSparkle.Length; k++)
				{
					this.mGameObjects_UpSparkle[k] = null;
				}
			}
			this.mGameObjects_UpSparkle = null;
			this.mTargetShipModelMst = null;
			this.mKeyController = null;
			this.waitAndVoiceCoroutine = null;
		}
	}
}
