using Common.Enum;
using KCV.Utils;
using local.models;
using Server_Common.Formats;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Strategy
{
	public class TileAnimationAttack : MonoBehaviour
	{
		private TileAnimationCharacter friendly;

		private TileAnimationCharacter enemy;

		private UISprite ship;

		private UITexture explosion;

		private TileAnimationAttackShell shell;

		private ParticleSystem partExplosion;

		private UILabel count;

		private UIWidget GetMaterial;

		private UITexture[] arrows;

		private UILabel[] downTexts;

		private bool on;

		private bool flicker;

		private float timer;

		private int cnt;

		private int tot;

		private int[] resFrom;

		private int[] resTo;

		public bool isFinished;

		private bool SkipFlag;

		private bool isGuard;

		private RadingResultData RadingData;

		[SerializeField]
		private TileAnimationHukidashi hukidashi;

		private Coroutine DelayedActionsCor;

		private Coroutine ResultPhaseCor;

		private Coroutine AttackCor;

		private Coroutine NowWaitCor;

		private bool CutMode;

		private void Awake()
		{
			base.get_transform().set_parent(GameObject.Find("/StrategyTaskManager/Map Root").get_transform());
			base.get_transform().set_localScale(Vector3.get_one());
			base.get_transform().set_localPosition(new Vector3(-459.45f, 144f, -24.8f));
			this.friendly = base.get_transform().Find("Friendly/Container").GetComponent<TileAnimationCharacter>();
			this.enemy = base.get_transform().Find("Enemy/Container").GetComponent<TileAnimationCharacter>();
			this.ship = base.get_transform().Find("Ship/ShipInner").GetComponent<UISprite>();
			this.explosion = base.get_transform().Find("Explosion").GetComponent<UITexture>();
			this.shell = base.get_transform().Find("Shell").GetComponent<TileAnimationAttackShell>();
			this.partExplosion = base.get_transform().Find("ParticleExplosion").GetComponent<ParticleSystem>();
			this.count = base.get_transform().Find("Count").GetComponent<UILabel>();
			this.GetMaterial = base.get_transform().Find("GetMaterial").GetComponent<UIWidget>();
			this.arrows = new UITexture[4];
			for (int i = 0; i < 4; i++)
			{
				this.arrows[i] = base.get_transform().Find("GetMaterial/Label_GetMaterial/Grid/GetMaterial" + (i + 1) + "/Arrow").GetComponent<UITexture>();
			}
			this.downTexts = new UILabel[4];
			for (int j = 0; j < 4; j++)
			{
				this.downTexts[j] = base.get_transform().Find("GetMaterial/Label_GetMaterial/Grid/GetMaterial" + (j + 1) + "/num").GetComponent<UILabel>();
			}
			this.timer = 0f;
			this.flicker = false;
			this.on = false;
			this.ship.alpha = 0f;
			this.count.alpha = 0f;
			this.count.text = string.Empty;
			this.GetMaterial.alpha = 0f;
			for (int k = 0; k < 4; k++)
			{
				this.arrows[k].alpha = 0f;
				this.downTexts[k].text = string.Empty;
			}
			this.isFinished = false;
			this.cnt = App.rand.Next(7) + 3;
			this.tot = this.cnt;
			this.partExplosion.Pause(true);
		}

		private void Start()
		{
			for (int i = 0; i < 4; i++)
			{
				iTween.MoveTo(this.arrows[i].get_gameObject(), iTween.Hash(new object[]
				{
					"position",
					this.arrows[i].get_transform().get_localPosition() + new Vector3(0f, -30f, 0f),
					"islocal",
					true,
					"time",
					0.75f,
					"easeType",
					iTween.EaseType.linear,
					"loopType",
					iTween.LoopType.loop
				}));
			}
		}

		private void Update()
		{
			if (this.on)
			{
				for (int i = 0; i < 4; i++)
				{
					if (this.resFrom[i] != this.resTo[i])
					{
						this.arrows[i].alpha = Mathf.Min(this.GetMaterial.alpha, (170f + this.arrows[0].get_transform().get_localPosition().y) / 45f);
					}
				}
			}
			if ((Input.GetKey(122) || Input.GetKey(351)) && this.DelayedActionsCor != null)
			{
				this.CutMode = true;
			}
		}

		public void Initialize(RadingResultData radingData, MapAreaModel m)
		{
			this.hukidashi.Init();
			TweenAlpha tweenAlpha = TweenAlpha.Begin(base.get_gameObject(), 0.2f, 1f);
			tweenAlpha.onFinished.Clear();
			this.RadingData = radingData;
			int beforeNum = radingData.BeforeNum;
			int tanker_count = radingData.BeforeNum - radingData.BreakNum;
			Debug.Log(m);
			Debug.Log(m.GetEscortDeck());
			if (radingData.FlagShipMstId != 0)
			{
				bool isDamaged = radingData.FlagShipDamageState == DamageState.Taiha || radingData.FlagShipDamageState == DamageState.Tyuuha;
				this.friendly.UnloadTexture();
				this.DelayActionFrame(1, delegate
				{
					this.friendly.SetTexture(ShipUtils.LoadTexture(m.GetEscortDeck().GetFlagShip(), isDamaged));
				});
			}
			if (radingData.AttackKind == RadingKind.AIR_ATTACK)
			{
				this.enemy.UnloadTexture();
				this.DelayActionFrame(1, delegate
				{
					this.enemy.SetTexture(ShipUtils.LoadTexture(512, 9));
				});
			}
			else
			{
				this.enemy.UnloadTexture();
				this.DelayActionFrame(1, delegate
				{
					this.enemy.SetTexture(ShipUtils.LoadTexture(530, 9));
				});
			}
			this.tot = beforeNum;
			this.cnt = tanker_count;
			this.resFrom = new int[4];
			this.resFrom[0] = m.GetResources(beforeNum).get_Item(enumMaterialCategory.Fuel);
			this.resFrom[1] = m.GetResources(beforeNum).get_Item(enumMaterialCategory.Bull);
			this.resFrom[2] = m.GetResources(beforeNum).get_Item(enumMaterialCategory.Steel);
			this.resFrom[3] = m.GetResources(beforeNum).get_Item(enumMaterialCategory.Bauxite);
			this.resTo = new int[4];
			this.resTo[0] = m.GetResources(tanker_count).get_Item(enumMaterialCategory.Fuel);
			this.resTo[1] = m.GetResources(tanker_count).get_Item(enumMaterialCategory.Bull);
			this.resTo[2] = m.GetResources(tanker_count).get_Item(enumMaterialCategory.Steel);
			this.resTo[3] = m.GetResources(tanker_count).get_Item(enumMaterialCategory.Bauxite);
			for (int i = 0; i < 4; i++)
			{
				this.downTexts[i].text = " ×" + Convert.ToString(this.resFrom[i]);
			}
		}

		public void Attack(bool guard, RadingKind type)
		{
			this.timer = Time.get_time();
			this.on = true;
			this.isFinished = false;
			this.count.text = "ｘ " + this.tot;
			this.isGuard = guard;
			this.DelayedActionsCor = base.StartCoroutine(this.DelayedActions(guard, type));
		}

		[DebuggerHidden]
		public IEnumerator DelayedActions(bool guard, RadingKind type)
		{
			TileAnimationAttack.<DelayedActions>c__Iterator195 <DelayedActions>c__Iterator = new TileAnimationAttack.<DelayedActions>c__Iterator195();
			<DelayedActions>c__Iterator.guard = guard;
			<DelayedActions>c__Iterator.type = type;
			<DelayedActions>c__Iterator.<$>guard = guard;
			<DelayedActions>c__Iterator.<$>type = type;
			<DelayedActions>c__Iterator.<>f__this = this;
			return <DelayedActions>c__Iterator;
		}

		public void CountMove()
		{
			this.count.get_transform().set_localPosition(this.ship.get_transform().get_parent().get_localPosition() + new Vector3(0f, 30f, 0f));
		}

		public void ShipAlpha(float f)
		{
			this.ship.alpha = f;
		}

		public void CountAlpha(float f)
		{
			this.count.alpha = f;
		}

		public void ExplosionAlpha(float f)
		{
			this.explosion.alpha = f;
		}

		public void InfoAlpha(float f)
		{
			this.GetMaterial.alpha = f;
		}

		[DebuggerHidden]
		private IEnumerator TankerPopUp()
		{
			TileAnimationAttack.<TankerPopUp>c__Iterator196 <TankerPopUp>c__Iterator = new TileAnimationAttack.<TankerPopUp>c__Iterator196();
			<TankerPopUp>c__Iterator.<>f__this = this;
			return <TankerPopUp>c__Iterator;
		}

		[DebuggerHidden]
		private IEnumerator EnemyPopUp(bool guard)
		{
			TileAnimationAttack.<EnemyPopUp>c__Iterator197 <EnemyPopUp>c__Iterator = new TileAnimationAttack.<EnemyPopUp>c__Iterator197();
			<EnemyPopUp>c__Iterator.<>f__this = this;
			return <EnemyPopUp>c__Iterator;
		}

		[DebuggerHidden]
		private IEnumerator GuardPopUp(bool guard)
		{
			TileAnimationAttack.<GuardPopUp>c__Iterator198 <GuardPopUp>c__Iterator = new TileAnimationAttack.<GuardPopUp>c__Iterator198();
			<GuardPopUp>c__Iterator.guard = guard;
			<GuardPopUp>c__Iterator.<$>guard = guard;
			<GuardPopUp>c__Iterator.<>f__this = this;
			return <GuardPopUp>c__Iterator;
		}

		[DebuggerHidden]
		private IEnumerator BattleAnimWithGuard(RadingKind type, float cur, int died)
		{
			TileAnimationAttack.<BattleAnimWithGuard>c__Iterator199 <BattleAnimWithGuard>c__Iterator = new TileAnimationAttack.<BattleAnimWithGuard>c__Iterator199();
			<BattleAnimWithGuard>c__Iterator.type = type;
			<BattleAnimWithGuard>c__Iterator.died = died;
			<BattleAnimWithGuard>c__Iterator.cur = cur;
			<BattleAnimWithGuard>c__Iterator.<$>type = type;
			<BattleAnimWithGuard>c__Iterator.<$>died = died;
			<BattleAnimWithGuard>c__Iterator.<$>cur = cur;
			<BattleAnimWithGuard>c__Iterator.<>f__this = this;
			return <BattleAnimWithGuard>c__Iterator;
		}

		private Coroutine MyWaitForSeconds(float time)
		{
			if (this.CutMode)
			{
				return null;
			}
			this.NowWaitCor = base.StartCoroutine(this.WaitForSeconds(time));
			return this.NowWaitCor;
		}

		[DebuggerHidden]
		private IEnumerator WaitForSeconds(float time)
		{
			TileAnimationAttack.<WaitForSeconds>c__Iterator19A <WaitForSeconds>c__Iterator19A = new TileAnimationAttack.<WaitForSeconds>c__Iterator19A();
			<WaitForSeconds>c__Iterator19A.time = time;
			<WaitForSeconds>c__Iterator19A.<$>time = time;
			return <WaitForSeconds>c__Iterator19A;
		}

		[DebuggerHidden]
		private IEnumerator BattleAnimWithNone(RadingKind type, float cur, int died)
		{
			TileAnimationAttack.<BattleAnimWithNone>c__Iterator19B <BattleAnimWithNone>c__Iterator19B = new TileAnimationAttack.<BattleAnimWithNone>c__Iterator19B();
			<BattleAnimWithNone>c__Iterator19B.type = type;
			<BattleAnimWithNone>c__Iterator19B.died = died;
			<BattleAnimWithNone>c__Iterator19B.cur = cur;
			<BattleAnimWithNone>c__Iterator19B.<$>type = type;
			<BattleAnimWithNone>c__Iterator19B.<$>died = died;
			<BattleAnimWithNone>c__Iterator19B.<$>cur = cur;
			<BattleAnimWithNone>c__Iterator19B.<>f__this = this;
			return <BattleAnimWithNone>c__Iterator19B;
		}

		[DebuggerHidden]
		private IEnumerator ShowResult()
		{
			TileAnimationAttack.<ShowResult>c__Iterator19C <ShowResult>c__Iterator19C = new TileAnimationAttack.<ShowResult>c__Iterator19C();
			<ShowResult>c__Iterator19C.<>f__this = this;
			return <ShowResult>c__Iterator19C;
		}

		[DebuggerHidden]
		private IEnumerator ResultPhase()
		{
			TileAnimationAttack.<ResultPhase>c__Iterator19D <ResultPhase>c__Iterator19D = new TileAnimationAttack.<ResultPhase>c__Iterator19D();
			<ResultPhase>c__Iterator19D.<>f__this = this;
			return <ResultPhase>c__Iterator19D;
		}

		private void EndAnimation()
		{
			this.flicker = false;
			this.on = false;
			this.isFinished = true;
			this.ship.get_transform().get_parent().set_localPosition(Vector3.get_zero());
			this.cnt = App.rand.Next(7) + 3;
			this.tot = this.cnt;
			this.ship.alpha = 0f;
			this.count.alpha = 0f;
			this.count.text = string.Empty;
			this.GetMaterial.alpha = 0f;
			for (int i = 0; i < 4; i++)
			{
				this.arrows[i].alpha = 0f;
				if (this.cnt == this.tot)
				{
					iTween.Resume(this.arrows[i].get_gameObject());
				}
				this.downTexts[i].color = Color.get_white();
				this.downTexts[i].text = string.Empty;
			}
			this.enemy.Reset();
			this.friendly.Reset();
			this.DelayedActionsCor = null;
			this.ResultPhaseCor = null;
		}

		private void OnDestroy()
		{
			this.friendly = null;
			this.enemy = null;
			this.ship = null;
			this.explosion = null;
			this.shell = null;
			this.partExplosion = null;
			this.count = null;
			this.GetMaterial = null;
			this.arrows = null;
			this.downTexts = null;
			this.DelayedActionsCor = null;
			this.ResultPhaseCor = null;
			this.hukidashi = null;
			this.AttackCor = null;
			this.NowWaitCor = null;
		}
	}
}
