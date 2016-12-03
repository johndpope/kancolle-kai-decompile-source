using Common.Enum;
using KCV.Utils;
using KCV.View.ScrollView;
using local.managers;
using local.models;
using System;
using System.Collections;
using UnityEngine;

namespace KCV.Organize
{
	[RequireComponent(typeof(UIWidget))]
	public class OrganizeScrollListChild : MonoBehaviour, UIScrollListItem<ShipModel, OrganizeScrollListChild>
	{
		[SerializeField]
		protected UITexture _uiDeckFlag;

		[SerializeField]
		protected UISprite mSprite_TypeIcon;

		[SerializeField]
		protected UILabel mLabel_Name;

		[SerializeField]
		protected UILabel mLabel_Speed;

		[SerializeField]
		protected UILabel mLabel_Level;

		[SerializeField]
		protected UILabel mLabel_Taikyu;

		[SerializeField]
		protected UILabel mLabel_Karyoku;

		[SerializeField]
		protected UILabel mLabel_Taiku;

		[SerializeField]
		protected UISprite mSprite_LockIcon;

		[SerializeField]
		protected UISprite mSprite_ShipState;

		[SerializeField]
		protected Animation mAnimation_MarriagedRing;

		[SerializeField]
		private Transform mTransform_Overlay;

		[SerializeField]
		private Transform mTransform_Bling;

		[SerializeField]
		private Transform mTransform_Deploy;

		[SerializeField]
		private UISprite mSprite_ActiveState;

		private int mRealIndex;

		private ShipModel mShipModel;

		private UIWidget mWidgetThis;

		private IOrganizeManager _OrganizeManager;

		private Transform mTransform;

		private Action<OrganizeScrollListChild> mOnTouchListener;

		private void OnDisable()
		{
			this.mLabel_Name.text = string.Empty;
			this.mLabel_Speed.text = string.Empty;
		}

		private void OnEnable()
		{
			if (this.GetModel() != null)
			{
				this.mLabel_Name.text = this.GetModel().Name;
				this.InitializeShipSpeed(this.GetModel());
			}
		}

		private void Awake()
		{
			this.mWidgetThis = base.GetComponent<UIWidget>();
			this.mWidgetThis.alpha = 1E-09f;
		}

		public void setManager(IOrganizeManager manager)
		{
			this._OrganizeManager = manager;
		}

		public void Initialize(int realIndex, ShipModel shipModel)
		{
			this.mShipModel = shipModel;
			this.mRealIndex = realIndex;
			this.InitializeDeckFlag(shipModel);
			this.InitializeActiveStateIcon(shipModel);
			this.InitializeShipTypeIcon(shipModel);
			this.InitializeShipStateIcon(shipModel);
			this.mLabel_Name.text = shipModel.Name;
			this.mLabel_Level.text = shipModel.Level.ToString();
			this.mLabel_Taikyu.text = shipModel.MaxHp.ToString();
			this.mLabel_Karyoku.text = shipModel.Karyoku.ToString();
			this.mLabel_Taiku.text = shipModel.Taiku.ToString();
			this.InitializeShipSpeed(shipModel);
			this.InitializeLockIcon(shipModel);
			this.InitializeRing(shipModel);
			if (shipModel.IsMarriage())
			{
				this.StartRingAnimation();
			}
			else
			{
				this.StopRingAnimation();
			}
			this.mWidgetThis.alpha = 1f;
		}

		public void InitializeDefault(int realIndex)
		{
			this.mRealIndex = realIndex;
			this.mShipModel = null;
			this.mWidgetThis.alpha = 1E-09f;
		}

		private void InitializeShipStateIcon(ShipModel shipModel)
		{
			string spriteName = string.Empty;
			bool flag = shipModel.IsInRepair();
			if (flag)
			{
				spriteName = "icon-ss_syufuku";
			}
			else
			{
				switch (shipModel.DamageStatus)
				{
				case DamageState.Shouha:
					spriteName = "icon-ss_shoha";
					break;
				case DamageState.Tyuuha:
					spriteName = "icon-ss_chuha";
					break;
				case DamageState.Taiha:
					spriteName = "icon-ss_taiha";
					break;
				}
			}
			this.mSprite_ShipState.spriteName = spriteName;
		}

		private void InitializeActiveStateIcon(ShipModel shipModel)
		{
			this.mTransform_Overlay.get_transform().SetActive(false);
			this.mTransform_Bling.SetActive(false);
			this.mTransform_Deploy.SetActive(false);
			this.mSprite_ActiveState.SetActive(false);
			bool flag = shipModel.IsBling();
			if (flag)
			{
				this.mTransform_Overlay.get_transform().SetActive(true);
				this.mTransform_Bling.SetActive(true);
				this.mSprite_ActiveState.spriteName = "icon_kaikou";
				this.mSprite_ActiveState.SetDimensions(32, 43);
				return;
			}
			bool flag2 = shipModel.IsBlingWait();
			if (flag2)
			{
				this.mTransform_Overlay.get_transform().SetActive(true);
				this.mTransform_Deploy.SetActive(true);
			}
			bool flag3 = shipModel.IsInMission();
			if (flag3)
			{
				this.mTransform_Overlay.get_transform().SetActive(true);
				this.mSprite_ActiveState.SetActive(true);
				this.mSprite_ActiveState.spriteName = "shipicon_ensei";
				this.mSprite_ActiveState.SetDimensions(32, 43);
				return;
			}
			bool flag4 = shipModel.IsEscaped();
			if (flag4)
			{
				this.mTransform_Overlay.get_transform().SetActive(true);
				this.mSprite_ActiveState.SetActive(true);
				this.mSprite_ActiveState.spriteName = "shipicon_withdraw";
				this.mSprite_ActiveState.SetDimensions(32, 43);
				return;
			}
			bool flag5 = shipModel.IsInActionEndDeck();
			if (flag5)
			{
				this.mTransform_Overlay.get_transform().SetActive(true);
				this.mSprite_ActiveState.SetActive(true);
				this.mSprite_ActiveState.spriteName = "icon-s_done";
				this.mSprite_ActiveState.SetDimensions(49, 41);
				return;
			}
		}

		private void InitializeLockIcon(ShipModel shipModel)
		{
			bool flag = shipModel.IsLocked();
			if (flag)
			{
				this.mSprite_LockIcon.spriteName = "lock_ship";
			}
			else
			{
				this.mSprite_LockIcon.spriteName = "lock_ship_open";
			}
		}

		private void InitializeRing(ShipModel shipModel)
		{
			bool flag = shipModel.IsMarriage();
			this.mAnimation_MarriagedRing.set_playAutomatically(true);
			if (flag)
			{
				this.mAnimation_MarriagedRing.get_gameObject().SetActive(true);
				using (IEnumerator enumerator = this.mAnimation_MarriagedRing.get_transform().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Transform component = (Transform)enumerator.get_Current();
						component.SetActive(true);
					}
				}
			}
			else
			{
				this.StopRingAnimation();
				using (IEnumerator enumerator2 = this.mAnimation_MarriagedRing.get_transform().GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Transform component2 = (Transform)enumerator2.get_Current();
						component2.SetActive(false);
					}
				}
			}
		}

		private void InitializeShipSpeed(ShipModel shipModel)
		{
			bool flag = shipModel.Soku == 10;
			if (flag)
			{
				this.mLabel_Speed.text = "高速";
			}
			else
			{
				this.mLabel_Speed.text = "低速";
			}
		}

		private void InitializeShipTypeIcon(ShipModel shipIcon)
		{
			this.mSprite_TypeIcon.spriteName = "ship" + shipIcon.ShipType;
		}

		private void InitializeDeckFlag(ShipModel shipModel)
		{
			bool flag = this.IsDeckInShip(shipModel);
			if (flag)
			{
				DeckModelBase deckFromShip = this.GetDeckFromShip(shipModel);
				bool isFlagShip = deckFromShip.GetFlagShip().MemId == shipModel.MemId;
				int id = deckFromShip.Id;
				bool flag2 = deckFromShip.IsEscortDeckMyself();
				if (flag2)
				{
					this.InitializeEscortDeckFlag(id, isFlagShip);
				}
				else
				{
					this.InitializeNormalDeckFlag(id, isFlagShip);
				}
			}
			else
			{
				this.RemoveDeckFlag();
			}
		}

		protected virtual bool IsDeckInShip(ShipModel shipModel)
		{
			DeckModelBase deckFromShip = this.GetDeckFromShip(shipModel);
			return deckFromShip != null;
		}

		public void SwitchShipLockState()
		{
			this._OrganizeManager.Lock(this.mShipModel.MemId);
			if (this.mShipModel.IsLocked())
			{
				this.mSprite_LockIcon.spriteName = "lock_ship";
				SoundUtils.PlaySE(SEFIleInfos.SE_005);
			}
			else
			{
				this.mSprite_LockIcon.spriteName = "lock_ship_open";
				SoundUtils.PlaySE(SEFIleInfos.SE_006);
			}
		}

		public void StartRingAnimation()
		{
			bool flag = this.mShipModel.IsMarriage();
			if (flag)
			{
				bool flag2 = !this.mAnimation_MarriagedRing.get_isPlaying();
				if (flag2)
				{
					this.mAnimation_MarriagedRing.Play();
				}
			}
		}

		public void StopRingAnimation()
		{
			bool isPlaying = this.mAnimation_MarriagedRing.get_isPlaying();
			if (isPlaying)
			{
				this.mAnimation_MarriagedRing.Stop();
			}
		}

		protected virtual DeckModelBase GetDeckFromShip(ShipModel shipModel)
		{
			return shipModel.getDeck();
		}

		private void InitializeNormalDeckFlag(int deckId, bool isFlagShip)
		{
			string text = string.Empty;
			if (isFlagShip)
			{
				text = string.Format("icon_deck{0}_fs", deckId);
			}
			else
			{
				text = string.Format("icon_deck{0}", deckId);
			}
			this._uiDeckFlag.mainTexture = (Resources.Load("Textures/Common/DeckFlag/" + text) as Texture2D);
			this._uiDeckFlag.SetDimensions(60, 56);
		}

		private void RemoveDeckFlag()
		{
			this._uiDeckFlag.SetDimensions(0, 0);
			this._uiDeckFlag.mainTexture = null;
		}

		private void InitializeEscortDeckFlag(int deckId, bool isFlagShip)
		{
			string text = string.Empty;
			int w = 56;
			int h = 64;
			if (isFlagShip)
			{
				text = "icon_guard_fs";
			}
			else
			{
				text = "icon_guard";
			}
			this._uiDeckFlag.mainTexture = (Resources.Load("Textures/Common/DeckFlag/" + text) as Texture2D);
			this._uiDeckFlag.SetDimensions(w, h);
		}

		public Transform GetTransform()
		{
			if (this.mTransform == null)
			{
				this.mTransform = base.get_transform();
			}
			return this.mTransform;
		}

		public ShipModel GetModel()
		{
			return this.mShipModel;
		}

		public int GetHeight()
		{
			return 61;
		}

		private void OnClick()
		{
			if (this.mOnTouchListener != null)
			{
				this.mOnTouchListener.Invoke(this);
			}
		}

		public void SetOnTouchListener(Action<OrganizeScrollListChild> onTouchListener)
		{
			this.mOnTouchListener = onTouchListener;
		}

		public void Hover()
		{
			UISelectedObject.SelectedOneObjectBlink(base.get_transform().FindChild("Background").get_gameObject(), true);
		}

		public void RemoveHover()
		{
			UISelectedObject.SelectedOneObjectBlink(base.get_transform().FindChild("Background").get_gameObject(), false);
		}

		public float GetBottomPositionY()
		{
			return this.GetTransform().get_localPosition().y + (float)this.GetHeight();
		}

		public float GetHeadPositionY()
		{
			return this.GetTransform().get_localPosition().y;
		}

		public int GetRealIndex()
		{
			return this.mRealIndex;
		}
	}
}
