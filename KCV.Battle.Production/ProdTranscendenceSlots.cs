using KCV.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Production
{
	[RequireComponent(typeof(Animation))]
	public class ProdTranscendenceSlots : MonoBehaviour
	{
		private Animation _anim;

		private List<UISlotItemHexButton> _listHexBtn;

		public List<UISlotItemHexButton> hexButtonList
		{
			get
			{
				return this._listHexBtn;
			}
		}

		private void OnDestroy()
		{
			Mem.Del<Animation>(ref this._anim);
			Mem.DelListSafe<UISlotItemHexButton>(ref this._listHexBtn);
		}

		public bool Init()
		{
			this._anim = base.GetComponent<Animation>();
			this._anim.Stop();
			this._listHexBtn = new List<UISlotItemHexButton>();
			for (int i = 0; i < 3; i++)
			{
				this._listHexBtn.Add(base.get_transform().FindChild(string.Format("TAHexBtn{0}", i + 1)).GetComponent<UISlotItemHexButton>());
			}
			return true;
		}

		public void Play(ProdTranscendenceCutIn.AnimationList iList)
		{
			this._anim.Play(string.Format("{0}Slots", iList.ToString()));
		}

		private void playSlotItem(int nSlotNum)
		{
			this._listHexBtn.get_Item(nSlotNum).SetActive(true);
			this._listHexBtn.get_Item(nSlotNum).Play(UIHexButton.AnimationList.ProdTranscendenceAttackHex, null);
		}

		private void playProdTATorpedox2Slots()
		{
			this._listHexBtn.ForEach(delegate(UISlotItemHexButton x)
			{
				x.SetActive(true);
				x.Play(UIHexButton.AnimationList.ProdTranscendenceAttackHex, null);
			});
		}

		private void PlaySlotSE()
		{
			SoundUtils.PlaySE(SEFIleInfos.SE_939a);
		}
	}
}
