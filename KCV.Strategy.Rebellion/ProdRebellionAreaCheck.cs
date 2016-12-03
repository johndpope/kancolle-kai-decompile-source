using KCV.Utils;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Strategy.Rebellion
{
	public class ProdRebellionAreaCheck : MonoBehaviour
	{
		private StrategyCamera strategyCamera;

		[SerializeField]
		private GameObject ArrowPrefab;

		private RebellionArrow ArrowInstance;

		private UITexture EnemyTex;

		private UISprite FromTile;

		private UISprite TargetTile;

		private Vector2 offset;

		private Vector2 stoppos;

		private Vector2 endpos;

		private Vector2 camerapos;

		private void Start()
		{
			this.strategyCamera = StrategyTaskManager.GetStrategyTop().strategyCamera;
			this.EnemyTex = base.get_transform().FindChild("Enemy").GetComponent<UITexture>();
			this.EnemyTex.mainTexture = ShipUtils.LoadTexture(512, 9);
		}

		public void Play(int fromAreaNo, int targetAreaNo, Action OnFinish)
		{
			base.StartCoroutine(this.StartAnimation(fromAreaNo, targetAreaNo, OnFinish));
		}

		[DebuggerHidden]
		private IEnumerator StartAnimation(int fromAreaNo, int targetAreaNo, Action OnFinish)
		{
			ProdRebellionAreaCheck.<StartAnimation>c__Iterator15E <StartAnimation>c__Iterator15E = new ProdRebellionAreaCheck.<StartAnimation>c__Iterator15E();
			<StartAnimation>c__Iterator15E.fromAreaNo = fromAreaNo;
			<StartAnimation>c__Iterator15E.targetAreaNo = targetAreaNo;
			<StartAnimation>c__Iterator15E.OnFinish = OnFinish;
			<StartAnimation>c__Iterator15E.<$>fromAreaNo = fromAreaNo;
			<StartAnimation>c__Iterator15E.<$>targetAreaNo = targetAreaNo;
			<StartAnimation>c__Iterator15E.<$>OnFinish = OnFinish;
			<StartAnimation>c__Iterator15E.<>f__this = this;
			return <StartAnimation>c__Iterator15E;
		}

		private void CameraMove(int targetAreaNo)
		{
			this.strategyCamera.MoveToTargetTile(targetAreaNo, false);
		}

		private void ArrowAnimation(int fromAreaNo, int targetAreaNo)
		{
			this.ArrowInstance = Util.Instantiate(this.ArrowPrefab, StrategyTaskManager.GetMapRoot().get_gameObject(), false, false).GetComponent<RebellionArrow>();
			Vector3 fromTile;
			if (this.FromTile != null && fromAreaNo != 15 && fromAreaNo != 16 && fromAreaNo != 17)
			{
				fromTile = this.FromTile.get_transform().get_position();
			}
			else
			{
				fromTile = this.TargetTile.get_transform().get_parent().TransformPoint(this.TargetTile.get_transform().get_localPosition() + new Vector3(185f, -106f));
			}
			this.ArrowInstance.StartAnimation(fromTile, this.TargetTile.get_transform().get_position());
		}

		[DebuggerHidden]
		private IEnumerator EnemyCutIn()
		{
			ProdRebellionAreaCheck.<EnemyCutIn>c__Iterator15F <EnemyCutIn>c__Iterator15F = new ProdRebellionAreaCheck.<EnemyCutIn>c__Iterator15F();
			<EnemyCutIn>c__Iterator15F.<>f__this = this;
			return <EnemyCutIn>c__Iterator15F;
		}
	}
}
