using System;
using UnityEngine;

namespace KCV.Strategy
{
	public class TileAnimationHukidashi : MonoBehaviour
	{
		public enum Type
		{
			Damage,
			Goutin,
			Damecon,
			TankerLost
		}

		[SerializeField]
		private UISprite hukidashi;

		private TweenPosition TwPos;

		private TweenAlpha TwAlpha;

		private readonly Vector3 TankerLostPos = new Vector3(0f, 30f, 0f);

		private readonly Vector3 DamagePos = new Vector3(-30f, 0f, 0f);

		private UILabel BreakNum;

		private UIWidget Wiget;

		private void Awake()
		{
			this.TwPos = base.GetComponent<TweenPosition>();
			this.TwAlpha = base.GetComponent<TweenAlpha>();
			this.Wiget = base.GetComponent<UIWidget>();
			this.BreakNum = base.get_transform().GetChild(0).GetComponent<UILabel>();
		}

		public void Init()
		{
			this.Wiget.alpha = 0f;
		}

		public void Play(TileAnimationHukidashi.Type type)
		{
			this.hukidashi.spriteName = "fuki_" + (int)(type + 1);
			this.hukidashi.MakePixelPerfect();
			this.BreakNum.text = string.Empty;
			this.Show(type);
		}

		public void Play(int breakNum)
		{
			this.hukidashi.spriteName = "fuki_4";
			this.hukidashi.MakePixelPerfect();
			this.BreakNum.textInt = breakNum;
			this.Show(TileAnimationHukidashi.Type.TankerLost);
		}

		private void Show(TileAnimationHukidashi.Type type)
		{
			this.TwPos.ResetToBeginning();
			this.TwAlpha.ResetToBeginning();
			if (type == TileAnimationHukidashi.Type.TankerLost)
			{
				this.TwPos.from = this.TankerLostPos;
			}
			else
			{
				this.TwPos.from = this.DamagePos;
			}
			this.TwPos.PlayForward();
			this.TwAlpha.PlayForward();
		}

		private void OnDestroy()
		{
			this.hukidashi = null;
			this.TwPos = null;
			this.TwAlpha = null;
			this.BreakNum = null;
		}
	}
}
