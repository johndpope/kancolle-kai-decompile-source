using KCV.Strategy;
using live2d;
using local.models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Ending
{
	public class EndingShipView : MonoBehaviour
	{
		[SerializeField]
		private UILabel ShipNameLabel;

		[SerializeField]
		private UILabel DeckNameLabel;

		[SerializeField]
		private UILabel LevelLabel;

		[SerializeField]
		private UITexture BG;

		[SerializeField]
		private TweenPosition TweenPos;

		[SerializeField]
		private TweenAlpha LableTweenAlpha;

		private List<Live2DModelUnity> Live2DCache;

		[SerializeField]
		private StrategyShipCharacter Live2DRender;

		[SerializeField]
		private ParticleSystem SakuraPar;

		[SerializeField]
		private Transform Ring;

		public float ChangeCount;

		public float EndingTime;

		public float ChangeTime;

		private Coroutine cor;

		public bool isShipChanging;

		private AudioSource ShipVoice;

		private bool isFirstCall;

		public bool isVoicePlaying
		{
			get
			{
				return this.ShipVoice != null && this.ShipVoice.get_isPlaying();
			}
		}

		public bool isLeft
		{
			get
			{
				return this.TweenPos.to.x == -25f;
			}
		}

		private void Awake()
		{
			this.isFirstCall = true;
			this.Live2DCache = new List<Live2DModelUnity>();
			this.Ring.get_transform().localPositionY(-54f);
		}

		[DebuggerHidden]
		public IEnumerator ShipChangeCoroutine(ShipModel Ship, int index)
		{
			EndingShipView.<ShipChangeCoroutine>c__Iterator56 <ShipChangeCoroutine>c__Iterator = new EndingShipView.<ShipChangeCoroutine>c__Iterator56();
			<ShipChangeCoroutine>c__Iterator.Ship = Ship;
			<ShipChangeCoroutine>c__Iterator.<$>Ship = Ship;
			<ShipChangeCoroutine>c__Iterator.<>f__this = this;
			return <ShipChangeCoroutine>c__Iterator;
		}

		[DebuggerHidden]
		public IEnumerator CharacterExit()
		{
			EndingShipView.<CharacterExit>c__Iterator57 <CharacterExit>c__Iterator = new EndingShipView.<CharacterExit>c__Iterator57();
			<CharacterExit>c__Iterator.<>f__this = this;
			return <CharacterExit>c__Iterator;
		}

		[DebuggerHidden]
		public IEnumerator ChangeFinalShip(ShipModel ship, EndingStaffRoll StaffRoll)
		{
			EndingShipView.<ChangeFinalShip>c__Iterator58 <ChangeFinalShip>c__Iterator = new EndingShipView.<ChangeFinalShip>c__Iterator58();
			<ChangeFinalShip>c__Iterator.ship = ship;
			<ChangeFinalShip>c__Iterator.StaffRoll = StaffRoll;
			<ChangeFinalShip>c__Iterator.<$>ship = ship;
			<ChangeFinalShip>c__Iterator.<$>StaffRoll = StaffRoll;
			<ChangeFinalShip>c__Iterator.<>f__this = this;
			return <ChangeFinalShip>c__Iterator;
		}

		private void SetLabel(ShipModel model)
		{
			this.ShipNameLabel.text = model.ShipTypeName + "\u3000";
			UILabel expr_21 = this.ShipNameLabel;
			expr_21.text += model.Name;
			DeckModelBase deck = model.getDeck();
			string text = (deck == null || deck.GetFlagShip().MemId != model.MemId) ? "所属" : "旗艦";
			this.DeckNameLabel.supportEncoding = false;
			if (deck != null)
			{
				this.DeckNameLabel.text = deck.Name + text;
				this.BG.height = 148;
			}
			else
			{
				this.DeckNameLabel.text = string.Empty;
				this.BG.height = 95;
			}
			this.LevelLabel.text = "練度 " + model.Level;
		}

		private void FadeLabel(bool isFadeIn)
		{
			this.LableTweenAlpha.Play(isFadeIn);
		}

		public void OnSideChange()
		{
			if (this.TweenPos.to.x == 400f)
			{
				this.TweenPos.to = new Vector3(-25f, 0f, 0f);
				this.TweenPos.from = new Vector3(-850f, 0f, 0f);
				this.SakuraPar.get_transform().localPositionX(0f);
			}
			else
			{
				this.TweenPos.to = new Vector3(400f, 0f, 0f);
				this.TweenPos.from = new Vector3(1200f, 0f, 0f);
				this.SakuraPar.get_transform().localPositionX(400f);
			}
		}

		public void CreateLive2DCache(List<ShipModel> ShipModels)
		{
			for (int i = 0; i < ShipModels.get_Count(); i++)
			{
				this.Live2DCache.Add(SingletonMonoBehaviour<Live2DModel>.Instance.CreateLive2DModelUnity(ShipModels.get_Item(i).MstId));
			}
		}

		private void OnDestroy()
		{
			if (this.cor != null)
			{
				base.StopCoroutine(this.cor);
				this.cor = null;
			}
			if (this.Live2DCache != null)
			{
				for (int i = 0; i < this.Live2DCache.get_Count(); i++)
				{
					this.Live2DCache.get_Item(i).releaseModel();
				}
			}
			this.Live2DCache.Clear();
		}
	}
}
