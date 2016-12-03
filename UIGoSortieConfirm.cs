using Common.Struct;
using KCV;
using KCV.Strategy;
using local.models;
using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public class UIGoSortieConfirm : MonoBehaviour
{
	[SerializeField]
	private DeckStateViews deckStateViews;

	[SerializeField]
	private YesNoButton yesNoButton;

	[SerializeField]
	private UILabel Title;

	[SerializeField]
	private UITexture BGTex;

	[SerializeField]
	private UITexture DeckIconNo;

	[SerializeField]
	private UILabel AlertMessage;

	private static readonly Color32 AlertColor = new Color32(255, 50, 50, 255);

	private static readonly Color32 WarningColor = new Color32(238, 255, 77, 255);

	private bool isInfoMode;

	private void OnDestroy()
	{
		this.deckStateViews = null;
		this.yesNoButton = null;
		this.Title = null;
		this.BGTex = null;
		this.DeckIconNo = null;
	}

	private void Awake()
	{
		this.isInfoMode = false;
	}

	[DebuggerHidden]
	private IEnumerator Start()
	{
		UIGoSortieConfirm.<Start>c__Iterator147 <Start>c__Iterator = new UIGoSortieConfirm.<Start>c__Iterator147();
		<Start>c__Iterator.<>f__this = this;
		return <Start>c__Iterator;
	}

	public void SetPushYesButton(Action act)
	{
		this.yesNoButton.SetOnSelectPositiveListener(act);
	}

	public void SetPushNoButton(Action act)
	{
		this.yesNoButton.SetOnSelectNegativeListener(act);
	}

	public void Initialize(DeckModel deckModel, bool isConfirm)
	{
		this.deckStateViews.Initialize(deckModel, !isConfirm);
		this.DeckIconNo.mainTexture = (Resources.Load("Textures/Common/DeckFlag/icon_deck" + deckModel.Id) as Texture2D);
		if (isConfirm)
		{
			this.setConfirmMode(deckModel);
		}
		else
		{
			this.setDeckInfoMode(deckModel);
		}
	}

	public void SetKeyController(KeyControl keyController)
	{
		this.yesNoButton.SetKeyController(keyController, false);
	}

	private void setConfirmMode(DeckModel deck)
	{
		base.get_transform().localPositionY(0f);
		this.yesNoButton.SetActive(true);
		this.Title.text = "この艦隊で出撃しますか？";
		this.BGTex.get_transform().localPositionY(0f);
		this.BGTex.height = 528;
		this.isInfoMode = false;
		this.AlertMessage.text = this.getAlertMessage(deck);
	}

	private void setDeckInfoMode(DeckModel deck)
	{
		base.get_transform().localPositionY(-17f);
		this.yesNoButton.SetActive(false);
		this.Title.text = deck.Name;
		this.Title.supportEncoding = false;
		this.BGTex.get_transform().localPositionY(20f);
		this.BGTex.height = 457;
		this.isInfoMode = true;
		this.AlertMessage.text = string.Empty;
	}

	private string getAlertMessage(DeckModel deck)
	{
		UserInfoModel userInfo = StrategyTopTaskManager.GetLogicManager().UserInfo;
		MemberMaxInfo memberMaxInfo = userInfo.ShipCountData();
		MemberMaxInfo memberMaxInfo2 = userInfo.SlotitemCountData();
		if (Enumerable.Any<ShipModel>(deck.GetShips(), (ShipModel x) => x.IsTaiha()))
		{
			this.AlertMessage.color = UIGoSortieConfirm.AlertColor;
			return "※大破している艦娘がいます。（被弾により喪失の危険があります）";
		}
		if (memberMaxInfo.MaxCount <= memberMaxInfo.NowCount)
		{
			this.AlertMessage.color = UIGoSortieConfirm.AlertColor;
			return "※艦娘保有数が上限に達しているため、新しい艦娘との邂逅はできません。";
		}
		if (memberMaxInfo.MaxCount - 6 <= memberMaxInfo.NowCount)
		{
			this.AlertMessage.color = UIGoSortieConfirm.WarningColor;
			return "※艦娘保有数が上限に近いため、新しい艦娘と邂逅できない可能性があります。";
		}
		if (memberMaxInfo2.MaxCount <= memberMaxInfo2.NowCount)
		{
			this.AlertMessage.color = UIGoSortieConfirm.AlertColor;
			return "※装備保有数が上限に達しているため、新しい艦娘との邂逅はできません。";
		}
		if (memberMaxInfo2.MaxCount - 24 <= memberMaxInfo2.NowCount)
		{
			this.AlertMessage.color = UIGoSortieConfirm.WarningColor;
			return "※装備保有数が上限に近いため、新しい艦娘と邂逅できない可能性があります。";
		}
		if (Enumerable.Any<ShipModel>(deck.GetShips(), (ShipModel x) => x.FuelRate < 100.0 || x.AmmoRate < 100.0))
		{
			this.AlertMessage.color = UIGoSortieConfirm.WarningColor;
			return "※十分な補給を受けていない艦娘がいます。（本来の戦闘力を発揮できません）";
		}
		return string.Empty;
	}

	private void Update()
	{
		if (this.isInfoMode && (Input.GetKeyDown(355) || Input.GetKeyDown(115)))
		{
			SingletonMonoBehaviour<PortObjectManager>.Instance.BackToPortOrOrganize();
		}
	}
}
