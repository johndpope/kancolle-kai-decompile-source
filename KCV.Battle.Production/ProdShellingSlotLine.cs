using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdShellingSlotLine : MonoBehaviour
	{
		[SerializeField]
		private ProdShellingLine _prodShellingLine;

		[SerializeField]
		private ProdShellingSlot _prodShellingSlot;

		public int baseDepth
		{
			get
			{
				return this._prodShellingLine.panel.depth;
			}
			set
			{
				this._prodShellingLine.panel.depth = value;
				this._prodShellingSlot.panel.depth = value + 1;
			}
		}

		public static ProdShellingSlotLine Instantiate(ProdShellingSlotLine prefab, Transform parent)
		{
			ProdShellingSlotLine prodShellingSlotLine = Object.Instantiate<ProdShellingSlotLine>(prefab);
			prodShellingSlotLine.get_transform().set_parent(parent);
			prodShellingSlotLine.get_transform().localScaleOne();
			prodShellingSlotLine.get_transform().localPositionZero();
			return prodShellingSlotLine;
		}

		private void Awake()
		{
			this._prodShellingLine.get_transform().localScaleZero();
			this._prodShellingSlot.get_transform().localScaleZero();
			this.baseDepth = 0;
		}

		private void OnDestroy()
		{
			Mem.Del<ProdShellingLine>(ref this._prodShellingLine);
			Mem.Del<ProdShellingSlot>(ref this._prodShellingSlot);
		}

		public void SetSlotData(SlotitemModel_Battle model, bool isFriend)
		{
			this._prodShellingSlot.SetSlotData(model);
		}

		public void SetSlotData(List<SlotitemModel_Battle> models, bool isFriend)
		{
			this._prodShellingSlot.SetSlotData(models.ToArray());
		}

		public void SetSlotData(SlotitemModel_Battle[] models, ProdTranscendenceCutIn.AnimationList iList)
		{
			this._prodShellingSlot.SetSlotData(models, iList);
		}

		public void Play(BaseProdLine.AnimationName iName, bool isFriend, Action callback)
		{
			base.get_transform().localScaleOne();
			if (iName != BaseProdLine.AnimationName.ProdSuccessiveLine)
			{
				this._prodShellingLine.Play(isFriend);
			}
			this._prodShellingSlot.Play(iName, isFriend, callback);
		}

		public void PlayTranscendenceLine(BaseProdLine.AnimationName iName, bool isFriend, Action callback)
		{
			base.get_transform().localScaleOne();
			this._prodShellingSlot.Play(iName, isFriend, callback);
		}
	}
}
