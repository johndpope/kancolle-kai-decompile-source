using Common.Enum;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.BattleCut
{
	public class BtlCut_FormationAnimation : MonoBehaviour
	{
		[SerializeField]
		private UISprite[] Formations;

		[SerializeField]
		private TextureFlash[] flash;

		[SerializeField]
		private Transform IconPanel;

		private Vector3 DecidePosition = new Vector3(0f, -288f, 0f);

		private Vector3[] defaultPos;

		[Header("[Animation Properties]"), SerializeField]
		private float _duration = 0.3f;

		[SerializeField]
		private float _delay = 0.1f;

		[SerializeField]
		private iTween.EaseType _easeType = iTween.EaseType.easeInBack;

		public float duration
		{
			get
			{
				return this._duration;
			}
			set
			{
				this._duration = value;
			}
		}

		public float delay
		{
			get
			{
				return this._delay;
			}
			set
			{
				this._delay = value;
			}
		}

		public iTween.EaseType easeType
		{
			get
			{
				return this._easeType;
			}
			set
			{
				this._easeType = value;
			}
		}

		private void Awake()
		{
			this.defaultPos = new Vector3[this.Formations.Length];
			int cnt = 0;
			this.defaultPos.ForEach(delegate(Vector3 x)
			{
				x = this.Formations[cnt].get_transform().get_position();
				cnt++;
			});
		}

		private void OnDestroy()
		{
			Mem.DelArySafe<UISprite>(ref this.Formations);
			Mem.DelArySafe<TextureFlash>(ref this.flash);
			Mem.Del<Transform>(ref this.IconPanel);
			Mem.Del<Vector3>(ref this.DecidePosition);
			Mem.DelArySafe<Vector3>(ref this.defaultPos);
		}

		[DebuggerHidden]
		public IEnumerator StartAnimation(BattleFormationKinds1 iKind)
		{
			BtlCut_FormationAnimation.<StartAnimation>c__Iterator10D <StartAnimation>c__Iterator10D = new BtlCut_FormationAnimation.<StartAnimation>c__Iterator10D();
			<StartAnimation>c__Iterator10D.iKind = iKind;
			<StartAnimation>c__Iterator10D.<$>iKind = iKind;
			<StartAnimation>c__Iterator10D.<>f__this = this;
			return <StartAnimation>c__Iterator10D;
		}
	}
}
