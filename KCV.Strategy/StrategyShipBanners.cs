using local.models;
using System;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyShipBanners : MonoBehaviour
	{
		private Transform[] SideShips;

		private CommonShipBanner[] Banners;

		private UIGrid Grid;

		private UIPlayTween PlayTween;

		private Vector3[] DefaultPositions;

		[SerializeField]
		private UISprite DeckNoIcon;

		[Button("EnterSideShips", "EnterSideShips", new object[]
		{

		})]
		public int button1;

		[Button("ExitSideShips", "ExitSideShips", new object[]
		{

		})]
		public int button2;

		private readonly Vector3 movePosition = new Vector3(320f, 0f, 0f);

		private void Awake()
		{
			this.SideShips = new Transform[6];
			this.Banners = new CommonShipBanner[6];
			this.Grid = base.GetComponent<UIGrid>();
			this.PlayTween = base.GetComponent<UIPlayTween>();
			this.DefaultPositions = new Vector3[6];
			for (int i = 0; i < this.SideShips.Length; i++)
			{
				this.SideShips[i] = base.get_transform().FindChild("SideShipBanner" + (i + 1));
				this.Banners[i] = this.SideShips[i].FindChild("CommonShipBanner2").GetComponent<CommonShipBanner>();
				this.DefaultPositions[i] = this.SideShips[i].get_localPosition();
			}
		}

		private void Start()
		{
			this.setShipTweenPosition();
		}

		public void UpdateBanners()
		{
			DeckModel currentDeck = SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck;
			for (int i = 0; i < this.Banners.Length; i++)
			{
				if (currentDeck.GetShip(i) == null)
				{
					this.SideShips[i].SetActive(false);
				}
				else
				{
					this.SideShips[i].SetActive(true);
					this.Banners[i].isUseSmoke = false;
					this.Banners[i].SetShipData(currentDeck.GetShip(i));
				}
			}
			this.DeckNoIcon.spriteName = "icon_deck" + currentDeck.Id;
		}

		public void EnterSideShips()
		{
			this.UpdateBanners();
			base.GetComponent<UIWidget>().alpha = 1f;
			this.setPosition(true);
			this.PlayTween.resetOnPlay = true;
			this.PlayTween.Play(true);
		}

		public void ExitSideShips()
		{
			this.setPosition(false);
			this.PlayTween.resetOnPlay = true;
			this.PlayTween.Play(false);
		}

		public void setPosition(bool toScreenIn)
		{
		}

		private void setShipTweenPosition()
		{
			for (int i = 0; i < this.SideShips.Length; i++)
			{
				TweenPosition tweenPosition = TweenPosition.Begin(this.SideShips[i].get_gameObject(), 0.3f, this.DefaultPositions[i] + this.movePosition);
				tweenPosition.worldSpace = false;
				tweenPosition.delay = (float)i * 0.04f;
				tweenPosition.animationCurve = AppInformation.curveDic.get_Item(AppInformation.CurveType.Live2DEnter);
			}
		}
	}
}
