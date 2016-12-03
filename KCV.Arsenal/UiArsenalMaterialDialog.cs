using KCV.Utils;
using System;
using UnityEngine;

namespace KCV.Arsenal
{
	public class UiArsenalMaterialDialog : MonoBehaviour
	{
		private const int MAX_FRAME_COUNT = 4;

		[SerializeField]
		private UITexture[] _uiSelect;

		private int moveSlotIndex;

		private int[] materialPow;

		private TweenPosition _tp;

		[SerializeField]
		public GameObject[] _uiMaterialFrame;

		public int _frameIndex;

		public void init(int number)
		{
			this._uiSelect = new UITexture[4];
			this._uiMaterialFrame = new GameObject[4];
			for (int i = 0; i < 4; i++)
			{
				this._uiMaterialFrame[i] = base.get_transform().FindChild("MaterialFrame" + (i + 1)).get_gameObject();
				Util.FindParentToChild<UITexture>(ref this._uiSelect[i], this._uiMaterialFrame[i].get_transform(), "Select");
				UISelectedObject.SelectedOneObjectBlinkArsenal(this._uiSelect[i].get_gameObject(), true);
			}
			this.materialPow = new int[4];
			this.materialPow[0] = 1000;
			this.materialPow[1] = 100;
			this.materialPow[2] = 10;
			this.materialPow[3] = 1;
		}

		private void OnDestroy()
		{
			Mem.Del<UITexture[]>(ref this._uiSelect);
			Mem.Del<int[]>(ref this.materialPow);
			Mem.Del<TweenPosition>(ref this._tp);
			Mem.Del<GameObject[]>(ref this._uiMaterialFrame);
		}

		private void setButtonMsg(UIButton obj, GameObject targetObj, string functionName)
		{
			UIButtonMessage component = obj.GetComponent<UIButtonMessage>();
			component.target = targetObj;
			component.functionName = functionName;
			component.trigger = UIButtonMessage.Trigger.OnClick;
		}

		public void ShowDialog(int materialIndex)
		{
			base.get_transform().set_localPosition(Vector3.get_zero());
			ArsenalTaskManager._clsConstruct.dialogPopUp.Open(base.get_gameObject(), 0f, 0f, 1f, 1f);
			this.SafeGetTweenAlpha(0f, 1f, 0.125f, 0f, UITweener.Method.Linear, UITweener.Style.Once, base.get_gameObject(), "compShowDialog");
			UISprite component = base.get_transform().FindChild("Icon").GetComponent<UISprite>();
			UILabel component2 = base.get_transform().FindChild("Label").GetComponent<UILabel>();
			component.spriteName = ArsenalTaskManager._clsConstruct._uiMaterialIcon[materialIndex].spriteName;
			component.MakePixelPerfect();
			if (component.spriteName != "icon_item4")
			{
				component.get_transform().set_localPosition(Vector3.get_left() * 40f + Vector3.get_up() * 119f);
				component2.get_transform().set_localPosition(Vector3.get_right() * 40f + Vector3.get_up() * 119f);
				component2.spacingX = 7;
				if (component.spriteName == "icon_item1")
				{
					component2.text = "燃料";
				}
				else if (component.spriteName == "icon_item2")
				{
					component2.text = "弾薬";
				}
				else if (component.spriteName == "icon_item3")
				{
					component2.text = "鋼材";
				}
			}
			else
			{
				component.get_transform().set_localPosition(Vector3.get_left() * 110f + Vector3.get_up() * 119f);
				component2.get_transform().set_localPosition(Vector3.get_right() * 40f + Vector3.get_up() * 119f);
				component2.spacingX = -1;
				component2.text = "ボーキサイト";
			}
			this.UpdateFrameSelect();
			ArsenalTaskManager._clsConstruct.UpdateDialogMaterialCount();
		}

		private void compShowDialog()
		{
		}

		public void ActiveMaterialFrame(bool isBigConstruct)
		{
			this._uiMaterialFrame[0].SetActive(isBigConstruct);
		}

		public void HidelDialog()
		{
			this.SafeGetTweenAlpha(1f, 0f, 0.125f, 0f, UITweener.Method.Linear, UITweener.Style.Once, base.get_gameObject(), string.Empty);
			ArsenalTaskManager._clsConstruct.updateStartBtn();
		}

		public bool SetFrameIndex(bool isLeft, bool isBigConstruct)
		{
			if (isLeft)
			{
				if (this._frameIndex < 3)
				{
					this._frameIndex++;
					this.UpdateFrameSelect();
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
					return true;
				}
			}
			else if (this._frameIndex > 0)
			{
				if (this._frameIndex == 1 && !isBigConstruct)
				{
					this._frameIndex = 1;
				}
				else
				{
					this._frameIndex--;
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				}
				this.UpdateFrameSelect();
				return true;
			}
			return false;
		}

		public int SetMaterialCount()
		{
			return this.materialPow[this._frameIndex];
		}

		public void UpdateFrameSelect()
		{
			for (int i = 0; i < 4; i++)
			{
				if (i == this._frameIndex)
				{
					this._uiSelect[i].get_transform().set_localScale(Vector3.get_one());
				}
				else
				{
					this._uiSelect[i].get_transform().set_localScale(Vector3.get_zero());
				}
			}
		}

		public void MoveMaterialSlot(bool isUp)
		{
			this.MoveMaterialSlot(isUp, false);
		}

		public void MoveMaterialSlot(bool isUp, bool isAnime)
		{
			UIPanel component = this._uiMaterialFrame[this._frameIndex].get_transform().FindChild("Panel").GetComponent<UIPanel>();
			this.moveSlotIndex = this._frameIndex;
			float num = (!isUp) ? 85f : -85f;
			float duration = (!isAnime) ? 0.0625f : 0.01f;
			Transform component2 = component.get_transform().FindChild("LabelGrp").GetComponent<Transform>();
			TweenPosition tweenPosition = TweenPosition.Begin(component2.get_transform().get_gameObject(), duration, Vector3.get_up() * num);
			tweenPosition.animationCurve = UtilCurves.TweenEaseInOutQuad;
			tweenPosition.AddOnFinished(new EventDelegate.Callback(this.CompMoveMaterialSlot));
		}

		public void CompMoveMaterialSlot()
		{
			ArsenalTaskManager._clsConstruct.CompMoveMaterialSlot();
			UIPanel component = this._uiMaterialFrame[this.moveSlotIndex].get_transform().FindChild("Panel").GetComponent<UIPanel>();
			component.get_transform().FindChild("LabelGrp").GetComponent<Transform>().set_localPosition(Vector3.get_zero());
			for (int i = 0; i < 5; i++)
			{
				UILabel component2 = component.get_transform().FindChild("LabelGrp/Label" + (i + 1)).GetComponent<UILabel>();
				Vector3 localPosition = Vector3.get_right() * component2.get_transform().get_localPosition().x + Vector3.get_up() * (170f - 85f * (float)i);
				component2.get_transform().set_localPosition(localPosition);
			}
		}

		public void MoveMaterialCount(int setMaterial, int index, int nowMaterial)
		{
			UIPanel component = this._uiMaterialFrame[index].get_transform().FindChild("Panel").GetComponent<UIPanel>();
			int num = setMaterial;
			for (int i = 0; i < 3 - index; i++)
			{
				num /= 10;
			}
			num %= 10;
			for (int j = 0; j < 5; j++)
			{
				UILabel component2 = component.get_transform().FindChild("LabelGrp/Label" + (j + 1)).GetComponent<UILabel>();
				int num2 = num + (2 - j);
				if (num2 > 9)
				{
					num2 -= 10;
				}
				if (num2 < 0)
				{
					num2 += 10;
				}
				component2.textInt = num2;
			}
		}
	}
}
