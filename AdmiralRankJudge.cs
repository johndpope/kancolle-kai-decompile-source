using Common.Enum;
using KCV;
using KCV.Utils;
using local.managers;
using LT.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AdmiralRankJudge : MonoBehaviour
{
	[SerializeField]
	private UILabel lostShipLabel;

	[SerializeField]
	private UILabel TurnLabel;

	[SerializeField]
	private UISprite DiffSprite;

	[SerializeField]
	private UITexture DiffMedal;

	[SerializeField]
	private UISprite RankSprite;

	[SerializeField]
	private Transform PlusIconParent;

	[SerializeField]
	private UISprite[] PlusIcon;

	[SerializeField]
	private ParticleSystem PetalParticle;

	[SerializeField]
	private ParticleSystem MedalParticle;

	private Dictionary<OverallRank, int> RankPlusPosXDic;

	private int _turn;

	private uint _lostShip;

	private Action _callback;

	private Animation _anime;

	public uint Debug_lostShip;

	public int Debug_Turn;

	public OverallRank Debug_rank;

	public int Debug_plus;

	public DifficultKind Debug_diff;

	[Button("DebugInit", "DebugInit", new object[]
	{

	})]
	public int button;

	public void Initialize(EndingManager manager)
	{
		uint lostShipCount = manager.GetLostShipCount();
		int turn = manager.Turn;
		int plus = 0;
		DifficultKind difficulty = manager.UserInfo.Difficulty;
		OverallRank overallRank;
		manager.CalculateTotalRank(out overallRank, out plus);
		this.TurnLabel.textInt = turn;
		this.lostShipLabel.text = lostShipCount.ToString();
		this.SetDiffSprite(difficulty);
		this.SetDiffMedal(difficulty);
		this.SetRankSprite(overallRank);
		this.SetPlus(plus, overallRank);
		this._turn = turn;
		this._lostShip = lostShipCount;
		this.PetalParticle.SetActive(false);
		this.MedalParticle.SetActive(false);
	}

	public void DebugInit()
	{
		this.Initialize(new EndingManager());
		this.Play(null);
	}

	public void Play(Action CallBack)
	{
		this._callback = CallBack;
		this._anime = base.GetComponent<Animation>();
		this._anime.Stop();
		this._anime.Play();
	}

	public void StopParticle()
	{
		this.PetalParticle.Stop();
		this.PetalParticle.SetActive(false);
		this.MedalParticle.Stop();
		this.PetalParticle.SetActive(false);
	}

	private void stratParticle()
	{
		this.PetalParticle.SetActive(true);
		this.PetalParticle.Play();
		this.MedalParticle.SetActive(true);
		this.MedalParticle.Play();
	}

	private void startCountUp(int index)
	{
		if (index == 0 && this._turn > 0)
		{
			base.get_transform().LTValue(0f, (float)this._turn, 0.5f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				int textInt = (int)Math.Round((double)x);
				this.TurnLabel.textInt = textInt;
			});
		}
		if (index == 1 && this._lostShip > 0u)
		{
			base.get_transform().LTValue(0f, this._lostShip, 0.5f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				int textInt = (int)Math.Round((double)x);
				this.lostShipLabel.textInt = textInt;
			}).setOnComplete(new Action(this.compTurnCountUp));
		}
	}

	private void compTurnCountUp()
	{
		this.TurnLabel.textInt = this._turn;
	}

	private void compLostCountUp()
	{
		this.lostShipLabel.text = this._lostShip.ToString();
	}

	private void startAnimationFinished()
	{
		if (this._callback != null)
		{
			this._callback.Invoke();
		}
	}

	private void playSE()
	{
		SoundUtils.PlayOneShotSE(SEFIleInfos.BattleNightMessage);
	}

	private void playSEPlus(int index)
	{
		if (this.PlusIcon[index].get_isActiveAndEnabled())
		{
			SoundUtils.PlayOneShotSE(SEFIleInfos.BattleNightMessage);
		}
	}

	private void SetDiffSprite(DifficultKind diff)
	{
		this.DiffSprite.spriteName = "txt_diff" + (int)diff + "_gray";
	}

	private void SetDiffMedal(DifficultKind diff)
	{
		this.DiffMedal.mainTexture = Resources.Load<Texture2D>("Textures/Record/medals/reward_" + (int)diff);
	}

	private void SetRankSprite(OverallRank rank)
	{
		this.RankSprite.spriteName = "rate_" + rank.ToString();
		this.RankSprite.MakePixelPerfect();
	}

	private void SetPlus(int plus, OverallRank rank)
	{
		for (int i = 0; i < 4; i++)
		{
			if (i < Mathf.Abs(plus))
			{
				this.PlusIcon[i].SetActive(true);
				this.PlusIcon[i].spriteName = ((0 >= plus) ? "rate_-" : "rate_+");
				this.PlusIcon[i].MakePixelPerfect();
			}
			else
			{
				this.PlusIcon[i].SetActive(false);
			}
		}
		this.RankPlusPosXDic = new Dictionary<OverallRank, int>();
		this.RankPlusPosXDic.Add(OverallRank.EX, 222);
		this.RankPlusPosXDic.Add(OverallRank.S, 177);
		this.RankPlusPosXDic.Add(OverallRank.A, 177);
		this.RankPlusPosXDic.Add(OverallRank.B, 190);
		this.RankPlusPosXDic.Add(OverallRank.C, 190);
		this.RankPlusPosXDic.Add(OverallRank.D, 190);
		this.RankPlusPosXDic.Add(OverallRank.E, 190);
		this.RankPlusPosXDic.Add(OverallRank.F, 190);
		this.PlusIconParent.localPositionX((float)this.RankPlusPosXDic.get_Item(rank));
	}

	private void OnDestroy()
	{
		this.lostShipLabel = null;
		this.TurnLabel = null;
		this.DiffSprite = null;
		this.DiffMedal = null;
		this.RankSprite = null;
		this.PlusIconParent = null;
		this.PlusIcon = null;
		this.PetalParticle = null;
		this.MedalParticle = null;
		this.RankPlusPosXDic.Clear();
		this.RankPlusPosXDic = null;
	}
}
