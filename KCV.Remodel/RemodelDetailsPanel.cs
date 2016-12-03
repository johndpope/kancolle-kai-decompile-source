using local.models;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace KCV.Remodel
{
	public class RemodelDetailsPanel : MonoBehaviour
	{
		private UIPanel myPanel;

		[SerializeField]
		private UITexture nowItemBase;

		[SerializeField]
		private UITexture nextItemBase;

		[SerializeField]
		private UILabel nowItemParams;

		[SerializeField]
		private UILabel nextItemParams;

		private StringBuilder sb;

		private List<int> paramList;

		private Vector3 enterPos;

		private Vector3 exitPos;

		private readonly string[] itemTypeName = new string[]
		{
			"火力",
			"雷撃",
			"爆装",
			"対空",
			"対潜",
			"索敵",
			"装甲",
			"回避",
			"命中"
		};

		private void Start()
		{
			this.sb = new StringBuilder();
			this.paramList = new List<int>();
			this.enterPos = new Vector3(270f, -40f, 0f);
			this.exitPos = new Vector3(690f, -40f, 0f);
		}

		private void OnDestroy()
		{
			Mem.Del<UIPanel>(ref this.myPanel);
			Mem.Del<UITexture>(ref this.nowItemBase);
			Mem.Del<UITexture>(ref this.nextItemBase);
			Mem.Del<UILabel>(ref this.nowItemParams);
			Mem.Del<UITexture>(ref this.nextItemBase);
			Mem.Del<StringBuilder>(ref this.sb);
			Mem.DelListSafe<int>(ref this.paramList);
			Mem.Del<Vector3>(ref this.enterPos);
			Mem.Del<Vector3>(ref this.exitPos);
		}

		public void Initialize(SlotitemModel nowItem, SlotitemModel nextItem)
		{
			this.EnterPanel();
		}

		public void EnterPanel()
		{
			Util.MoveTo(base.get_gameObject(), 0.2f, this.enterPos, iTween.EaseType.easeOutQuint);
		}

		public void ExitPanel()
		{
			Util.MoveTo(base.get_gameObject(), 0.2f, this.exitPos, iTween.EaseType.easeOutQuint);
		}

		public string makeParamText(SlotitemModel item)
		{
			this.sb.set_Length(0);
			this.paramList.Clear();
			if (0 < item.Syatei)
			{
				this.sb.Append("射程：" + App.SYATEI_TEXT[item.Syatei]);
				this.sb.AppendLine();
			}
			this.paramList.Add(item.Hougeki);
			this.paramList.Add(item.Raigeki);
			this.paramList.Add(item.Bakugeki);
			this.paramList.Add(item.Taikuu);
			this.paramList.Add(item.Taisen);
			this.paramList.Add(item.Sakuteki);
			this.paramList.Add(item.Soukou);
			this.paramList.Add(item.Kaihi);
			this.paramList.Add(item.HouMeityu);
			int num = 0;
			for (int i = 0; i < this.paramList.get_Count(); i++)
			{
				if (0 < this.paramList.get_Item(i))
				{
					string text = (this.paramList.get_Item(i) <= 0) ? " -" : " +";
					this.sb.Append(string.Concat(new object[]
					{
						this.itemTypeName[i],
						text,
						this.paramList.get_Item(i),
						"\u3000"
					}));
					num++;
					if (num % 3 == 0)
					{
						this.sb.AppendLine();
					}
				}
			}
			return this.sb.ToString();
		}
	}
}
