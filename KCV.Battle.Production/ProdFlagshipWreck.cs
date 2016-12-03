using KCV.Battle.Utils;
using KCV.BattleCut;
using KCV.Utils;
using local.models;
using Server_Models;
using System;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdFlagshipWreck : BaseAnimation
	{
		[SerializeField]
		private UITexture _uiShip;

		[SerializeField]
		private Generics.Message _clsMessage1;

		[SerializeField]
		private Generics.Message _clsMessage2;

		[SerializeField]
		private UIButton _uiBackBtn;

		[SerializeField]
		private Animation _backBtnAnim;

		[SerializeField]
		private ParticleSystem _uiSmokePar;

		private int debugIndex;

		private bool _isControl;

		private bool _isPlayMsg1;

		private bool _isPlayMsg2;

		private bool _isBattleCut;

		private ShipModel_BattleAll _clsShipModel;

		private DeckModel _clsDeck;

		private KeyControl _keyControl;

		private UILabel _bbLabel;

		public static ProdFlagshipWreck Instantiate(ProdFlagshipWreck prefab, Transform parent, ShipModel_BattleAll flagShip, DeckModel deck, KeyControl input, bool isBattleCut)
		{
			ProdFlagshipWreck prodFlagshipWreck = Object.Instantiate<ProdFlagshipWreck>(prefab);
			prodFlagshipWreck.get_transform().set_parent(parent);
			prodFlagshipWreck.get_transform().set_localPosition(Vector3.get_zero());
			prodFlagshipWreck.get_transform().set_localScale(Vector3.get_zero());
			prodFlagshipWreck._init(flagShip, deck, input, isBattleCut);
			return prodFlagshipWreck;
		}

		private bool _init(ShipModel_BattleAll flagShip, DeckModel deck, KeyControl input, bool isBattleCut)
		{
			this.debugIndex = 0;
			this._isControl = false;
			this._isFinished = false;
			this._isPlayMsg1 = false;
			this._isPlayMsg2 = false;
			this._isBattleCut = isBattleCut;
			this._clsShipModel = flagShip;
			this._clsDeck = deck;
			this._keyControl = input;
			Util.FindParentToChild<UITexture>(ref this._uiShip, base.get_transform(), "ShipObj/Ship");
			Util.FindParentToChild<UIButton>(ref this._uiBackBtn, base.get_transform(), "BackIcon");
			Util.FindParentToChild<ParticleSystem>(ref this._uiSmokePar, base.get_transform(), "Smoke");
			if (this._backBtnAnim == null)
			{
				this._backBtnAnim = this._uiBackBtn.GetComponent<Animation>();
			}
			this._uiBackBtn.onClick = Util.CreateEventDelegateList(this, "compFlagshipWreck", null);
			this._setObject();
			Util.FindParentToChild<UILabel>(ref this._bbLabel, base.get_transform(), "Message1");
			this._bbLabel.supportEncoding = false;
			return true;
		}

		private void OnDestroy()
		{
			this._clsMessage1.UnInit();
			this._clsMessage2.UnInit();
			Mem.Del<UITexture>(ref this._uiShip);
			Mem.Del<Generics.Message>(ref this._clsMessage1);
			Mem.Del<Generics.Message>(ref this._clsMessage2);
			Mem.Del<UIButton>(ref this._uiBackBtn);
			Mem.Del<Animation>(ref this._backBtnAnim);
			Mem.Del(ref this._uiSmokePar);
			Mem.Del<ShipModel_BattleAll>(ref this._clsShipModel);
			Mem.Del<DeckModel>(ref this._clsDeck);
			Mem.Del<KeyControl>(ref this._keyControl);
			Mem.Del<UILabel>(ref this._bbLabel);
		}

		private void _setShipTexture()
		{
			this._uiShip.mainTexture = KCV.Battle.Utils.ShipUtils.LoadTexture(this._clsShipModel, false);
			this._uiShip.MakePixelPerfect();
			this._uiShip.get_transform().set_localPosition(Util.Poi2Vec(new ShipOffset(this._clsShipModel.GetGraphicsMstId()).GetShipDisplayCenter(true)));
		}

		private void _setObject()
		{
			this._setShipTexture();
			this._clsMessage1 = new Generics.Message(base.get_transform(), "Message1");
			this._clsMessage1.Init(string.Format("『{0}』艦隊<br>旗艦「{1}」が<br>大破しました。", this._clsDeck.Name, this._clsShipModel.Name), 0.05f, null);
			this._clsMessage2 = new Generics.Message(base.get_transform(), "Message2");
			this._clsMessage2.Init("進撃は困難です……帰投します。", 0.05f, null);
		}

		private void _debugObject()
		{
			if (Mst_DataManager.Instance.Mst_shipgraph.ContainsKey(this.debugIndex))
			{
				ShipModelMst shipModelMst = new ShipModelMst(this.debugIndex);
				this._uiShip.mainTexture = KCV.Battle.Utils.ShipUtils.LoadTexture(this.debugIndex, true);
				this._uiShip.MakePixelPerfect();
				this._uiShip.get_transform().set_localPosition(Util.Poi2Vec(new ShipOffset(this.debugIndex).GetShipDisplayCenter(true)));
				this._clsMessage1.UnInit();
				this._clsMessage2.UnInit();
				this._clsMessage1 = new Generics.Message(base.get_transform(), "Message1");
				this._clsMessage1.Init(string.Format("『{0}』艦隊<br>旗艦「{1}」が<br>大破しました。", this._clsDeck.Name, shipModelMst.Name), 0.05f, null);
				this._clsMessage2 = new Generics.Message(base.get_transform(), "Message2");
				this._clsMessage2.Init("進撃は困難です……帰投します。", 0.05f, null);
			}
		}

		public bool Run()
		{
			if (this._isPlayMsg1)
			{
				this._clsMessage1.Update();
				if (this._clsMessage1.IsMessageEnd)
				{
					this._clsMessage2.Play();
					this._isPlayMsg1 = false;
					this._isPlayMsg2 = true;
				}
			}
			if (this._isPlayMsg2)
			{
				this._clsMessage2.Update();
				if (this._clsMessage2.IsMessageEnd)
				{
					if (this._isBattleCut)
					{
						UIBattleCutNavigation navigation = BattleCutManager.GetNavigation();
						navigation.SetNavigationInFlagshipWreck();
						navigation.Show(0.2f, null);
					}
					else
					{
						UIBattleNavigation battleNavigation = BattleTaskManager.GetPrefabFile().battleNavigation;
						battleNavigation.SetNavigationInFlagshipWreck();
						battleNavigation.Show(0.2f, null);
					}
					this._isPlayMsg2 = false;
					this._isControl = true;
					this._uiBackBtn.GetComponent<UISprite>().alpha = 1f;
					this._uiBackBtn.get_transform().set_localPosition(new Vector3(420f, -205f, 0f));
					this._backBtnAnim.Play();
				}
			}
			if (this._isFinished)
			{
				return true;
			}
			if (!this._isControl)
			{
				return false;
			}
			if (this._keyControl.keyState.get_Item(1).down)
			{
				this.compFlagshipWreck();
			}
			return false;
		}

		public override void Play(Action callback)
		{
			base.get_transform().set_localScale(Vector3.get_one());
			base.Play(callback);
			KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.EnemyComming);
		}

		private void DebugPlay()
		{
			this._isControl = false;
			this._isPlayMsg1 = false;
			this._isPlayMsg2 = false;
			this._debugObject();
			this.Play(this._actCallback);
		}

		private void _messageStart()
		{
			this._isPlayMsg1 = true;
			this._clsMessage1.Play();
			this._playFShipVoice();
		}

		private void _playParticle()
		{
			this._uiSmokePar.Stop();
			this._uiSmokePar.Play();
		}

		private void _playPlaneAdmission()
		{
			KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.IA_Failure);
		}

		private void _playFShipVoice()
		{
			KCV.Battle.Utils.ShipUtils.PlayFlagshipWreckVoice(BattleTaskManager.GetBattleManager().Ships_f[0]);
		}

		private void BackBtnEL(GameObject obj)
		{
			this.compFlagshipWreck();
		}

		private void compFlagshipWreck()
		{
			if (!this._isControl)
			{
				return;
			}
			this._isFinished = true;
			this._isControl = false;
			if (this._isBattleCut)
			{
				UIBattleCutNavigation navigation = BattleCutManager.GetNavigation();
				navigation.Hide(0.2f, null);
			}
			else
			{
				UIBattleNavigation battleNavigation = BattleTaskManager.GetPrefabFile().battleNavigation;
				battleNavigation.Hide(0.2f, null);
			}
			Dlg.Call(ref this._actCallback);
		}
	}
}
