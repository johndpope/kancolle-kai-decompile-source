using Common.Enum;
using local.models;
using System;
using UnityEngine;

namespace KCV.Scene.Revamp
{
	public class UIRepairShip : MonoBehaviour
	{
		[Serializable]
		private class Lines
		{
			private Transform _traLines;

			private UISprite _uiBG;

			private UILabel _uiLabel;

			public Lines(Transform parent, string objName)
			{
				Util.FindParentToChild<Transform>(ref this._traLines, parent, objName);
				Util.FindParentToChild<UISprite>(ref this._uiBG, this._traLines, "BG");
				Util.FindParentToChild<UILabel>(ref this._uiLabel, this._traLines, "Label");
				this._uiLabel.text = string.Empty;
			}

			public void SetLines(string lines)
			{
				this._uiLabel.text = lines;
			}
		}

		public static readonly string LINES_WELCOME = "[000000]\u3000提督、明石の工廠へようこそ！\n\u3000どの装備の改修を試みますか？[-]";

		public static readonly string LINES_CON_REVAMPSLOTITEM = "[" + Generics.BBCodeColor.kGreen + "]\u3000{0}[-]\n\u3000[000000]を、改修しますね！[-]";

		public static readonly string LINES_SUCCESS = "[000000]\u3000改修に成功しました。\n";

		public static readonly string LINES_FAILURE = "[000000]\u3000改修に失敗しました。\n";

		public static readonly string LINES_LIST = "[000000]\u3000一覧から選択してください。\n";

		private UITexture _uiShip;

		private UITexture _uiEyes;

		private UIRepairShip.Lines _uiLines;

		private void Awake()
		{
			Util.FindParentToChild<UITexture>(ref this._uiShip, base.get_transform(), "RepairShip/Ship");
			Util.FindParentToChild<UITexture>(ref this._uiEyes, base.get_transform(), "RepairShip/Eye");
			this._uiLines = new UIRepairShip.Lines(base.get_transform(), "Lines");
		}

		private void Start()
		{
		}

		private void Update()
		{
		}

		private void OnDestroy()
		{
			this._uiShip = null;
			this._uiEyes = null;
		}

		public void SetRepairShip(ShipModel model)
		{
			Debug.Log(string.Format("[ID:{0}]{1}", model.MstId, model.Name));
		}

		public void SetLines(string lines)
		{
			this._uiLines.SetLines(lines);
		}

		public void SetLines(RevampValidationResult iResult, RevampRecipeDetailModel model)
		{
			if (model == null)
			{
				return;
			}
			string text = "[000000]";
			switch (iResult)
			{
			case RevampValidationResult.OK:
				text += string.Format("[1DBDC0]\u3000{0}[-]\nを改修しますね！[-]\n\n", model.Slotitem.Name);
				text += string.Format("[000000]\u3000この改修には、無改修の\n[1DBDC0]{0}×{1}[-]\nが必要です。[-]", model.Slotitem.Name, model.RequiredSlotitemCount);
				text += string.Format("[000000]\u3000(※改修で消費します)[-]", new object[0]);
				break;
			case RevampValidationResult.Max_Level:
				text += string.Format("[FF0000]\u3000現在、選択された装備[-]\n", new object[0]);
				text += string.Format("[1DBDC0]\u3000{0}[-]\n", model.Slotitem.Name);
				text += string.Format("[FF0000]\u3000は、これ以上の改修ができません。[-]", new object[0]);
				break;
			case RevampValidationResult.Lock:
				text += string.Format("[1DBDC0]\u3000{0}[-]\nを改修しますね！\n\n", model.Slotitem.Name);
				text += string.Format("[FF0000]\u3000この装備を改修するには\n\u3000同装備のロック解除が必要です。[-]", new object[0]);
				break;
			case RevampValidationResult.Less_Fuel:
			case RevampValidationResult.Less_Ammo:
			case RevampValidationResult.Less_Steel:
			case RevampValidationResult.Less_Baux:
			case RevampValidationResult.Less_Devkit:
			case RevampValidationResult.Less_Revkit:
				text += string.Format("[FF0000]\u3000資材が足りません。", new object[0]);
				break;
			case RevampValidationResult.Less_Slotitem:
				text += string.Format("[1DBDC0]\u3000{0}[-]\nを改修しますね！\n\n", model.Slotitem.Name);
				text += string.Format("[FF0000]\u3000この改修に必要となる\n(無改修)\n[-]", new object[0]);
				text += string.Format("[1DBDC0]\u3000{0}×{1}[-]", model.Slotitem.Name, model.RequiredSlotitemCount);
				text += string.Format("[FF0000]\u3000が足りません。[-]", new object[0]);
				break;
			case RevampValidationResult.Less_Slotitem_No_Lock:
				text += string.Format("[1DBDC0]\u3000{0}[-]\nを改修しますね！\n\n", model.Slotitem.Name);
				text += string.Format("[FF0000]\u3000この改修に必要となる\n(無改修)\n[-]", new object[0]);
				text += string.Format("[1DBDC0]\u3000{0}x{1}[-]", model.Slotitem.Name, model.RequiredSlotitemCount);
				text += string.Format("[FF0000]\u3000が足りません。[-]", new object[0]);
				break;
			}
			text += "[-]";
			this._uiLines.SetLines(text);
		}
	}
}
