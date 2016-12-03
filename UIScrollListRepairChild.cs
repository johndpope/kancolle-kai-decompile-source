using Common.Enum;
using KCV;
using KCV.View.ScrollView;
using local.models;
using System;
using UnityEngine;

public class UIScrollListRepairChild : MonoBehaviour, UIScrollListItem<ShipModel, UIScrollListRepairChild>
{
	[SerializeField]
	private UITexture mTexture_Flag;

	[SerializeField]
	private UISprite ShipTypeIcon;

	[SerializeField]
	private UILabel Label_name;

	[SerializeField]
	private UILabel Label_hp_all;

	[SerializeField]
	private UISprite HP_bar_grn;

	[SerializeField]
	private UILabel Label_lv;

	[SerializeField]
	private UISprite hakai;

	[SerializeField]
	private GameObject mBackground;

	private int mRealIndex;

	private ShipModel mShipModel;

	private UIWidget mWidgetThis;

	private Transform mTransform;

	private Action<UIScrollListRepairChild> mOnTouchListener;

	private void Awake()
	{
		this.mWidgetThis = base.GetComponent<UIWidget>();
		this.mWidgetThis.alpha = 1E-09f;
	}

	public void Initialize(int realIndex, ShipModel model)
	{
		this.mRealIndex = realIndex;
		this.mShipModel = model;
		this.InitializeDeckFlag(model);
		this.ShipTypeIcon.spriteName = "ship" + model.ShipType;
		this.Label_name.text = model.Name;
		this.Label_hp_all.text = model.NowHp + "/" + model.MaxHp;
		this.HP_bar_grn.width = model.NowHp * 1580 / model.MaxHp / 10;
		this.HP_bar_grn.color = Util.HpGaugeColor2(model.MaxHp, model.NowHp);
		this.Label_lv.text = model.Level.ToString();
		if (model.IsInRepair())
		{
			this.hakai.spriteName = "icon-s_syufuku";
		}
		else if (model.DamageStatus == DamageState.Taiha)
		{
			this.hakai.spriteName = "icon-s_taiha";
		}
		else if (model.DamageStatus == DamageState.Tyuuha)
		{
			this.hakai.spriteName = "icon-s_chuha";
		}
		else if (model.DamageStatus == DamageState.Shouha)
		{
			this.hakai.spriteName = "icon-s_shoha";
		}
		else
		{
			this.hakai.spriteName = null;
		}
		if (model.IsInMission())
		{
			this.hakai.spriteName = "icon-s_ensei";
		}
		if (model.IsInActionEndDeck())
		{
			this.hakai.spriteName = "icon-s_done";
		}
		this.mWidgetThis.alpha = 1f;
	}

	public void InitializeDefault(int realIndex)
	{
		this.mRealIndex = realIndex;
		this.mShipModel = null;
		this.mWidgetThis.alpha = 1E-09f;
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
		return 62;
	}

	public void Touch()
	{
		if (this.mOnTouchListener != null)
		{
			this.mOnTouchListener.Invoke(this);
		}
	}

	public void SetOnTouchListener(Action<UIScrollListRepairChild> onTouchListener)
	{
		this.mOnTouchListener = onTouchListener;
	}

	public void Hover()
	{
		UISelectedObject.SelectedOneObjectBlink(this.mBackground, true);
	}

	public void RemoveHover()
	{
		UISelectedObject.SelectedOneObjectBlink(this.mBackground, false);
	}

	public int GetRealIndex()
	{
		return this.mRealIndex;
	}

	private void InitializeDeckFlag(ShipModel shipModel)
	{
		bool flag = this.IsDeckInShip(shipModel);
		if (flag)
		{
			DeckModelBase deck = shipModel.getDeck();
			bool isFlagShip = deck.GetFlagShip().MemId == shipModel.MemId;
			int id = deck.Id;
			bool flag2 = deck.IsEscortDeckMyself();
			if (flag2)
			{
				this.InitializeEscortDeckFlag(id, isFlagShip);
			}
			else
			{
				this.InitializeNormalDeckFlag(id, isFlagShip);
			}
		}
		else if (shipModel.IsBling())
		{
			this.BlingDeckFlag();
		}
		else
		{
			this.RemoveDeckFlag();
		}
	}

	private bool IsDeckInShip(ShipModel shipModel)
	{
		DeckModelBase deck = shipModel.getDeck();
		return deck != null;
	}

	private void RemoveDeckFlag()
	{
		this.mTexture_Flag.SetDimensions(0, 0);
		this.mTexture_Flag.mainTexture = null;
	}

	private void BlingDeckFlag()
	{
		this.mTexture_Flag.mainTexture = (Resources.Load("Textures/repair/NewUI/icon-ss_kaiko") as Texture2D);
		this.mTexture_Flag.SetDimensions(70, 59);
	}

	private void InitializeNormalDeckFlag(int deckId, bool isFlagShip)
	{
		string text = string.Empty;
		int w = 60;
		int h = 56;
		if (isFlagShip)
		{
			text = string.Format("icon_deck{0}_fs", deckId);
		}
		else
		{
			text = string.Format("icon_deck{0}", deckId);
		}
		this.mTexture_Flag.mainTexture = (Resources.Load("Textures/Common/DeckFlag/" + text) as Texture2D);
		this.mTexture_Flag.SetDimensions(w, h);
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
		this.mTexture_Flag.mainTexture = (Resources.Load("Textures/Common/DeckFlag/" + text) as Texture2D);
		this.mTexture_Flag.SetDimensions(w, h);
	}
}
