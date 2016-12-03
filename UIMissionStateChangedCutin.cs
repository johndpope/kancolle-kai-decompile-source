using KCV;
using local.models;
using System;
using UnityEngine;

[RequireComponent(typeof(Animation)), RequireComponent(typeof(UIPanel))]
public class UIMissionStateChangedCutin : MonoBehaviour
{
	[SerializeField]
	private CommonShipBanner[] mShipBannerSlots;

	[SerializeField]
	private UISprite[] mSpriteFlashes;

	[SerializeField]
	private UISprite mSprite_CutinMessage;

	[SerializeField]
	private UITexture[] mTextures_Flag;

	private UIPanel mPanelThis;

	private DeckModel mDeckModel;

	private Action mAnimationFinishedCallBack;

	private int mCallFlashCount;

	private void Awake()
	{
		this.mPanelThis = base.GetComponent<UIPanel>();
		this.mPanelThis.alpha = 0.01f;
	}

	public void Initialize(DeckModel deck)
	{
		this.mDeckModel = deck;
		ShipModel[] ships = this.mDeckModel.GetShips();
		UITexture[] array = this.mTextures_Flag;
		for (int i = 0; i < array.Length; i++)
		{
			UITexture component = array[i];
			component.SetActive(false);
		}
		CommonShipBanner[] array2 = this.mShipBannerSlots;
		for (int j = 0; j < array2.Length; j++)
		{
			CommonShipBanner commonShipBanner = array2[j];
			commonShipBanner.StopParticle();
		}
		switch (ships.Length)
		{
		case 1:
			this.mShipBannerSlots[0].get_gameObject().SetActive(true);
			this.mShipBannerSlots[0].SetShipData(ships[0]);
			this.mTextures_Flag[0].mainTexture = Resources.Load<Texture>("Textures/Common/DeckFlag/icon_deck" + deck.Id + "_fs");
			this.mTextures_Flag[0].SetActive(true);
			break;
		case 2:
			this.mShipBannerSlots[0].get_gameObject().SetActive(true);
			this.mShipBannerSlots[5].get_gameObject().SetActive(true);
			this.mShipBannerSlots[0].SetShipData(ships[0]);
			this.mShipBannerSlots[5].SetShipData(ships[1]);
			this.mTextures_Flag[0].mainTexture = Resources.Load<Texture>("Textures/Common/DeckFlag/icon_deck" + deck.Id + "_fs");
			this.mTextures_Flag[0].SetActive(true);
			break;
		case 3:
			this.mShipBannerSlots[0].get_gameObject().SetActive(true);
			this.mShipBannerSlots[1].get_gameObject().SetActive(true);
			this.mShipBannerSlots[5].get_gameObject().SetActive(true);
			this.mShipBannerSlots[0].SetShipData(ships[2]);
			this.mShipBannerSlots[1].SetShipData(ships[0]);
			this.mShipBannerSlots[5].SetShipData(ships[1]);
			this.mTextures_Flag[1].mainTexture = Resources.Load<Texture>("Textures/Common/DeckFlag/icon_deck" + deck.Id + "_fs");
			this.mTextures_Flag[1].SetActive(true);
			break;
		case 4:
			this.mShipBannerSlots[0].get_gameObject().SetActive(true);
			this.mShipBannerSlots[1].get_gameObject().SetActive(true);
			this.mShipBannerSlots[4].get_gameObject().SetActive(true);
			this.mShipBannerSlots[5].get_gameObject().SetActive(true);
			this.mShipBannerSlots[0].SetShipData(ships[2]);
			this.mShipBannerSlots[1].SetShipData(ships[0]);
			this.mShipBannerSlots[4].SetShipData(ships[1]);
			this.mShipBannerSlots[5].SetShipData(ships[3]);
			this.mTextures_Flag[1].mainTexture = Resources.Load<Texture>("Textures/Common/DeckFlag/icon_deck" + deck.Id + "_fs");
			this.mTextures_Flag[1].SetActive(true);
			break;
		case 5:
			this.mShipBannerSlots[0].get_gameObject().SetActive(true);
			this.mShipBannerSlots[1].get_gameObject().SetActive(true);
			this.mShipBannerSlots[2].get_gameObject().SetActive(true);
			this.mShipBannerSlots[4].get_gameObject().SetActive(true);
			this.mShipBannerSlots[5].get_gameObject().SetActive(true);
			this.mShipBannerSlots[0].SetShipData(ships[2]);
			this.mShipBannerSlots[1].SetShipData(ships[0]);
			this.mShipBannerSlots[2].SetShipData(ships[4]);
			this.mShipBannerSlots[4].SetShipData(ships[1]);
			this.mShipBannerSlots[5].SetShipData(ships[3]);
			this.mTextures_Flag[1].mainTexture = Resources.Load<Texture>("Textures/Common/DeckFlag/icon_deck" + deck.Id + "_fs");
			this.mTextures_Flag[1].SetActive(true);
			break;
		case 6:
			this.mShipBannerSlots[0].get_gameObject().SetActive(true);
			this.mShipBannerSlots[1].get_gameObject().SetActive(true);
			this.mShipBannerSlots[2].get_gameObject().SetActive(true);
			this.mShipBannerSlots[3].get_gameObject().SetActive(true);
			this.mShipBannerSlots[4].get_gameObject().SetActive(true);
			this.mShipBannerSlots[5].get_gameObject().SetActive(true);
			this.mShipBannerSlots[0].SetShipData(ships[2]);
			this.mShipBannerSlots[1].SetShipData(ships[0]);
			this.mShipBannerSlots[2].SetShipData(ships[4]);
			this.mShipBannerSlots[3].SetShipData(ships[5]);
			this.mShipBannerSlots[4].SetShipData(ships[1]);
			this.mShipBannerSlots[5].SetShipData(ships[3]);
			this.mTextures_Flag[1].mainTexture = Resources.Load<Texture>("Textures/Common/DeckFlag/icon_deck" + deck.Id + "_fs");
			this.mTextures_Flag[1].SetActive(true);
			break;
		}
	}

	public void PlayStartCutin(Action onFinishedCallBack)
	{
		this.mSprite_CutinMessage.spriteName = "expedition_txt_go";
		this.mAnimationFinishedCallBack = onFinishedCallBack;
		this.mPanelThis.alpha = 1f;
		base.GetComponent<Animation>().Play("Anim_MissionStartCutinShutter");
		base.GetComponent<Animation>().Blend("Anim_MissionStartCutinLevel" + this.mDeckModel.GetShips().Length);
	}

	public void PlayFinishedCutin(Action onFinishedCallBack)
	{
		this.mSprite_CutinMessage.spriteName = "expedition_txt_finish";
		this.mAnimationFinishedCallBack = onFinishedCallBack;
		this.mPanelThis.alpha = 1f;
		base.GetComponent<Animation>().Play("Anim_MissionStartCutinShutter");
		base.GetComponent<Animation>().Blend("Anim_MissionStartCutinLevel" + this.mDeckModel.GetShips().Length);
	}

	public void OnFinishedAnimation()
	{
		Debug.Log("Call:OnFinishedAnimation");
		if (this.mAnimationFinishedCallBack != null)
		{
			this.mAnimationFinishedCallBack.Invoke();
		}
	}

	public void AnimFlash()
	{
		this.mSpriteFlashes[this.mCallFlashCount++].GetComponent<Animation>().Play("Anim_Flash");
	}

	public void OnFinishParticle()
	{
		CommonShipBanner[] array = this.mShipBannerSlots;
		for (int i = 0; i < array.Length; i++)
		{
			CommonShipBanner commonShipBanner = array[i];
			if (commonShipBanner.get_gameObject().get_activeSelf())
			{
				Debug.Log("KiraParLoop::False");
				commonShipBanner.StopParticle();
			}
		}
	}

	private void OnDestroy()
	{
		for (int i = 0; i < this.mShipBannerSlots.Length; i++)
		{
			this.mShipBannerSlots[i] = null;
		}
		this.mShipBannerSlots = null;
		for (int j = 0; j < this.mSpriteFlashes.Length; j++)
		{
			this.mSpriteFlashes[j] = null;
		}
		this.mSpriteFlashes = null;
		for (int k = 0; k < this.mTextures_Flag.Length; k++)
		{
			this.mTextures_Flag[k] = null;
		}
		this.mTextures_Flag = null;
		this.mSprite_CutinMessage = null;
		this.mPanelThis = null;
		this.mDeckModel = null;
		this.mAnimationFinishedCallBack = null;
	}
}
