using KCV.Battle.Utils;
using KCV.Utils;
using local.models.battle;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdDetectionResultCutIn : BaseAnimation
	{
		public enum AnimationList
		{
			DetectionLost,
			DetectionNotFound,
			DetectionSucces
		}

		[SerializeField]
		private UIAtlas _uiAtlas;

		[SerializeField]
		private Transform _uiLabel;

		[SerializeField]
		private List<UISprite> _listLabels;

		[SerializeField]
		private UITexture _uiOverlay;

		private ProdDetectionResultCutIn.AnimationList _iList;

		public ProdDetectionResultCutIn.AnimationList detectionResult
		{
			get
			{
				return this._iList;
			}
		}

		public static ProdDetectionResultCutIn Instantiate(ProdDetectionResultCutIn prefab, Transform parent, SakutekiModel model)
		{
			ProdDetectionResultCutIn prodDetectionResultCutIn = Object.Instantiate<ProdDetectionResultCutIn>(prefab);
			prodDetectionResultCutIn.get_transform().set_parent(parent);
			prodDetectionResultCutIn.get_transform().set_localScale(Vector3.get_zero());
			prodDetectionResultCutIn.get_transform().set_localPosition(Vector3.get_zero());
			prodDetectionResultCutIn.setDetection(prodDetectionResultCutIn.getDetectionProductionType(model));
			return prodDetectionResultCutIn;
		}

		protected override void Awake()
		{
			base.Awake();
			if (this._uiLabel == null)
			{
				Util.FindParentToChild<Transform>(ref this._uiLabel, base.get_transform(), "Label");
			}
			if (this._uiOverlay == null)
			{
				Util.FindParentToChild<UITexture>(ref this._uiOverlay, base.get_transform(), "WhiteOverlay");
			}
			if (this._listLabels == null)
			{
				this._listLabels = new List<UISprite>();
				for (int i = 0; i < BattleDefines.DETECTION_RESULT_LABEL_POS.get_Item(DetectionProductionType.Succes).get_Count(); i++)
				{
					this._listLabels.Add(this._uiLabel.get_transform().FindChild(string.Format("Label{0}", i + 1)).GetComponent<UISprite>());
				}
			}
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Mem.Del<UIAtlas>(ref this._uiAtlas);
			Mem.Del<Transform>(ref this._uiLabel);
			if (this._listLabels != null)
			{
				this._listLabels.ForEach(delegate(UISprite x)
				{
					x.Clear();
				});
			}
			Mem.DelListSafe<UISprite>(ref this._listLabels);
			Mem.Del<UITexture>(ref this._uiOverlay);
			Mem.Del<ProdDetectionResultCutIn.AnimationList>(ref this._iList);
		}

		private DetectionProductionType getDetectionProductionType(SakutekiModel model)
		{
			if (model.IsSuccess_f())
			{
				if (model.HasPlane_f() && model.ExistLost_f())
				{
					return DetectionProductionType.SuccesLost;
				}
				return DetectionProductionType.Succes;
			}
			else
			{
				if (model.ExistLost_f())
				{
					return DetectionProductionType.Lost;
				}
				return DetectionProductionType.NotFound;
			}
		}

		private void setDetection(DetectionProductionType iType)
		{
			int num = 0;
			string text = string.Empty;
			switch (iType)
			{
			case DetectionProductionType.Succes:
			case DetectionProductionType.SuccesLost:
				this._iList = ProdDetectionResultCutIn.AnimationList.DetectionSucces;
				text = "s1";
				break;
			case DetectionProductionType.Lost:
				this._iList = ProdDetectionResultCutIn.AnimationList.DetectionLost;
				text = "s2";
				break;
			case DetectionProductionType.NotFound:
				this._iList = ProdDetectionResultCutIn.AnimationList.DetectionNotFound;
				text = "s3";
				break;
			}
			using (List<UISprite>.Enumerator enumerator = this._listLabels.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					UISprite current = enumerator.get_Current();
					current.get_transform().set_localPosition(new Vector3(BattleDefines.DETECTION_RESULT_LABEL_POS.get_Item(iType).get_Item(num), 0f, 0f));
					current.spriteName = string.Format("{0}-{1}", text, num + 1);
					if (current.spriteName == "s2-7")
					{
						current.localSize = new Vector3(80f, 18f, 0f);
					}
					else if (current.spriteName == "s1-6" || current.spriteName == "s3-5")
					{
						current.localSize = new Vector3(40f, 100f);
					}
					else
					{
						current.localSize = new Vector3(100f, 100f, 0f);
					}
					num++;
				}
			}
			this._uiLabel.get_transform().set_localPosition((this._iList != ProdDetectionResultCutIn.AnimationList.DetectionLost) ? Vector3.get_zero() : (Vector3.get_left() * 45f));
		}

		public override void Play(Action forceCallback, Action callback)
		{
			base.get_transform().set_localScale(Vector3.get_one());
			base.Play(forceCallback, callback);
		}

		private void playLabelSpacing()
		{
			KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_945);
			this._uiLabel.GetComponent<Animation>().Play(this._iList.ToString());
		}
	}
}
