using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV
{
	public class UIShortCutCrane : MonoBehaviour
	{
		private class HookYousei
		{
			public int GraphicID;

			public Vector2 LocalPos;

			public HookYousei(int GraphicID, Vector2 LocalPos)
			{
				this.GraphicID = GraphicID;
				this.LocalPos = LocalPos;
			}
		}

		private Animation anim;

		[SerializeField]
		private UISprite HookYouseiSprite;

		private List<UIShortCutCrane.HookYousei> HookYouseis;

		private List<int[]> YouseiGroups;

		[Button("StartAnimation", "StartAnimation", new object[]
		{

		})]
		public int button1;

		private void Awake()
		{
			this.anim = base.GetComponent<Animation>();
			this.HookYouseis = new List<UIShortCutCrane.HookYousei>();
			this.HookYouseis.Add(new UIShortCutCrane.HookYousei(0, Vector2.get_zero()));
			this.HookYouseis.Add(new UIShortCutCrane.HookYousei(1, new Vector2(-31f, -75f)));
			this.HookYouseis.Add(new UIShortCutCrane.HookYousei(2, new Vector2(-58f, -55f)));
			this.HookYouseis.Add(new UIShortCutCrane.HookYousei(3, new Vector2(-29f, 85f)));
			this.HookYouseis.Add(new UIShortCutCrane.HookYousei(4, new Vector2(33f, 89f)));
			this.HookYouseis.Add(new UIShortCutCrane.HookYousei(5, new Vector2(-60f, -60f)));
			this.HookYouseis.Add(new UIShortCutCrane.HookYousei(6, new Vector2(-85f, -36f)));
			this.HookYouseis.Add(new UIShortCutCrane.HookYousei(7, new Vector2(-58f, -4f)));
			this.HookYouseis.Add(new UIShortCutCrane.HookYousei(8, new Vector2(-38f, 78f)));
			this.HookYouseis.Add(new UIShortCutCrane.HookYousei(9, new Vector2(-49f, -64f)));
			this.YouseiGroups = new List<int[]>();
			this.YouseiGroups.Add(new int[]
			{
				1,
				2
			});
			this.YouseiGroups.Add(new int[]
			{
				3,
				4
			});
			this.YouseiGroups.Add(new int[]
			{
				5,
				6
			});
			this.YouseiGroups.Add(new int[]
			{
				6,
				7
			});
			this.YouseiGroups.Add(new int[]
			{
				6,
				8
			});
			this.YouseiGroups.Add(new int[]
			{
				6,
				9
			});
			this.YouseiGroups.Add(new int[]
			{
				default(int),
				1,
				8
			});
		}

		private void StartAnimationPrivate(bool isStop)
		{
			this.ChangeYousei();
			int num = (Random.Range(0, 3) != 0) ? 2 : 1;
			string text = "SCMenuYousei" + num;
			if (isStop)
			{
				this.anim.Stop();
			}
			if (!this.anim.get_isPlaying())
			{
				this.anim.Play(text);
			}
		}

		public void StartAnimation()
		{
			this.StartAnimationPrivate(true);
		}

		public void StartAnimationNoReset()
		{
			this.StartAnimationPrivate(false);
		}

		public void AnimStop()
		{
			base.get_transform().localPositionX(550f);
			this.anim.Stop();
		}

		private void ChangeYousei()
		{
			ShipModel flagShipModel = SingletonMonoBehaviour<AppInformation>.Instance.FlagShipModel;
			int[] array = (flagShipModel == null) ? this.YouseiGroups.get_Item(6) : this.getYouseiGroup(flagShipModel.ShipType);
			int num = array[Random.Range(0, array.Length)];
			this.HookYouseiSprite.spriteName = "hookyousei_" + num;
			this.HookYouseiSprite.MakePixelPerfect();
			this.HookYouseiSprite.get_transform().set_localPosition(this.HookYouseis.get_Item(num).LocalPos);
		}

		private int[] getYouseiGroup(int Stype)
		{
			int num;
			if (Stype == 7 || Stype == 11 || Stype == 16 || Stype == 17 || Stype == 18)
			{
				num = 0;
			}
			else if (Stype == 8 || Stype == 9 || Stype == 10 || Stype == 12)
			{
				num = 1;
			}
			else if (Stype == 5 || Stype == 6)
			{
				num = 2;
			}
			else if (Stype == 3 || Stype == 4 || Stype == 21)
			{
				num = 3;
			}
			else if (Stype == 2)
			{
				num = 4;
			}
			else if (Stype == 13 || Stype == 14 || Stype == 20)
			{
				num = 5;
			}
			else
			{
				num = 6;
			}
			return this.YouseiGroups.get_Item(num);
		}

		private void OnDestroy()
		{
			this.anim = null;
			this.HookYouseiSprite = null;
			if (this.HookYouseis != null)
			{
				this.HookYouseis.Clear();
			}
			if (this.YouseiGroups != null)
			{
				this.YouseiGroups.Clear();
			}
			this.HookYouseis = null;
			this.YouseiGroups = null;
		}
	}
}
