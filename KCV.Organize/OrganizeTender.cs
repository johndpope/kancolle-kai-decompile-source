using Common.Enum;
using KCV.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Organize
{
	public class OrganizeTender : MonoBehaviour
	{
		public enum TenderState
		{
			None,
			Select,
			Maimiya,
			Irako,
			Twin
		}

		public enum BtnType
		{
			OFF,
			NORMAL,
			ON
		}

		private readonly Vector3 SWEETS_FOCUS_SCALE = new Vector3(1f, 1f, 1f);

		private readonly Vector3 SWEETS_UNFOCUS_SCALE = new Vector3(0.7f, 0.7f, 0.7f);

		private readonly Vector3 SWEETS_POSITION_CENTER = new Vector3(0f, -140f, 0f);

		private readonly Vector3 SWEETS_POSITION_LEFT = new Vector3(-265f, -40f, 0f);

		private readonly Vector3 SWEETS_POSITION_RIGHT = new Vector3(265f, -40f, 0f);

		[SerializeField]
		private GameObject _tenderPanel;

		[SerializeField]
		private GameObject _mamiyaPanel;

		[SerializeField]
		private GameObject _twinPanel;

		[SerializeField]
		private GameObject _animatePanel;

		[SerializeField]
		private UISprite _allBtn;

		[SerializeField]
		private UISprite _mamiyaBtn;

		[SerializeField]
		private UISprite _irakoBtn;

		[SerializeField]
		private UITexture _allC1;

		[SerializeField]
		private UITexture _allC2;

		[SerializeField]
		private UITexture _mainBg;

		[SerializeField]
		private UITexture _otherBg;

		[SerializeField]
		private UITexture _ship;

		[SerializeField]
		private UISprite _item;

		[SerializeField]
		private UISprite _btnYes;

		[SerializeField]
		private UISprite _btnNo;

		[SerializeField]
		private UILabel _labelFrom;

		[SerializeField]
		private UILabel _labelTo;

		[SerializeField]
		private UISprite _tBtnYes;

		[SerializeField]
		private UISprite _tBtnNo;

		[SerializeField]
		private UILabel _labelFrom1;

		[SerializeField]
		private UILabel _labelTo1;

		[SerializeField]
		private UILabel _labelFrom2;

		[SerializeField]
		private UILabel _labelTo2;

		[SerializeField]
		private UITexture _ship1;

		[SerializeField]
		private UITexture _ship2;

		[SerializeField]
		private ParticleSystem _parSystem;

		[SerializeField]
		private ParticleSystem _parSystem2;

		[SerializeField]
		private Animation _animation;

		[SerializeField]
		private UILabel _topLabel;

		private bool IsMoveMamiya;

		private bool IsMoveIrako;

		private bool IsMoveAll;

		public int setIndex;

		public int setIndex2;

		public bool isAnimation;

		public bool _GuideOff;

		public OrganizeTender.TenderState State;

		public Dictionary<SweetsType, bool> tenderDic;

		public void init()
		{
			if (this._tenderPanel == null)
			{
				this._tenderPanel = base.get_transform().FindChild("TenderDialog").get_gameObject();
			}
			if (this._mamiyaPanel == null)
			{
				this._mamiyaPanel = base.get_transform().FindChild("MamiyaDialog").get_gameObject();
			}
			if (this._twinPanel == null)
			{
				this._twinPanel = base.get_transform().FindChild("TwinDialog").get_gameObject();
			}
			if (this._animatePanel == null)
			{
				this._animatePanel = base.get_transform().FindChild("AnimatePanel").get_gameObject();
			}
			Util.FindParentToChild<UISprite>(ref this._allBtn, this._tenderPanel.get_transform(), "AllBtn");
			Util.FindParentToChild<UISprite>(ref this._mamiyaBtn, this._tenderPanel.get_transform(), "MamiyaBtn");
			Util.FindParentToChild<UISprite>(ref this._irakoBtn, this._tenderPanel.get_transform(), "IrakoBtn");
			Util.FindParentToChild<UITexture>(ref this._allC1, this._tenderPanel.get_transform(), "All1");
			Util.FindParentToChild<UITexture>(ref this._allC2, this._tenderPanel.get_transform(), "All2");
			Util.FindParentToChild<UITexture>(ref this._mainBg, base.get_transform(), "Bg");
			Util.FindParentToChild<UITexture>(ref this._otherBg, base.get_transform(), "UseBg");
			this._ship = this._mamiyaPanel.get_transform().FindChild("Ship").GetComponent<UITexture>();
			this._topLabel = this._mamiyaPanel.get_transform().FindChild("Label_Mamiya").GetComponent<UILabel>();
			this._item = this._mamiyaPanel.get_transform().FindChild("Item").GetComponent<UISprite>();
			this._btnYes = this._mamiyaPanel.get_transform().FindChild("YesBtn").GetComponent<UISprite>();
			this._btnNo = this._mamiyaPanel.get_transform().FindChild("NoBtn").GetComponent<UISprite>();
			this._labelFrom = this._mamiyaPanel.get_transform().FindChild("LabelFrom").GetComponent<UILabel>();
			this._labelTo = this._mamiyaPanel.get_transform().FindChild("LabelTo").GetComponent<UILabel>();
			this._tBtnYes = this._twinPanel.get_transform().FindChild("YesBtn").GetComponent<UISprite>();
			this._tBtnNo = this._twinPanel.get_transform().FindChild("NoBtn").GetComponent<UISprite>();
			this._labelFrom1 = this._twinPanel.get_transform().FindChild("Frame1_1/LabelFrom").GetComponent<UILabel>();
			this._labelTo1 = this._twinPanel.get_transform().FindChild("Frame1_2/LabelTo").GetComponent<UILabel>();
			this._labelFrom2 = this._twinPanel.get_transform().FindChild("Frame2_1/LabelFrom").GetComponent<UILabel>();
			this._labelTo2 = this._twinPanel.get_transform().FindChild("Frame2_2/LabelTo").GetComponent<UILabel>();
			this._ship1 = this._animatePanel.get_transform().FindChild("Ship/Ship1").GetComponent<UITexture>();
			this._ship2 = this._animatePanel.get_transform().FindChild("Ship/Ship2").GetComponent<UITexture>();
			this._animation = base.get_gameObject().GetComponent<Animation>();
			this._animation.Stop();
			this._parSystem = this._animatePanel.get_transform().FindChild("Circle/Par").GetComponent<ParticleSystem>();
			this._parSystem2 = this._animatePanel.get_transform().FindChild("Par2").GetComponent<ParticleSystem>();
			this._parSystem.Stop();
			this._parSystem2.Stop();
			this.setButtonMessage(this._allBtn.get_gameObject(), "AllBtnEL");
			this.setButtonMessage(this._irakoBtn.get_gameObject(), "IrakoBtnEL");
			this.setButtonMessage(this._mamiyaBtn.get_gameObject(), "MamiyaBtnEL");
			this.setButtonMessage(this._mainBg.get_gameObject(), "MainBackEL");
			this.setButtonMessage(this._otherBg.get_gameObject(), "OtherBackEL");
			this.setButtonMessage(this._btnYes.get_gameObject(), "BtnYesEL");
			this.setButtonMessage(this._tBtnYes.get_gameObject(), "BtnYesEL");
			this.setButtonMessage(this._btnNo.get_gameObject(), "BtnNoEL");
			this.setButtonMessage(this._tBtnNo.get_gameObject(), "BtnNoEL");
			this.State = OrganizeTender.TenderState.None;
			this.setIndex = 0;
			this.setIndex2 = 0;
			this.isAnimation = false;
			this._GuideOff = false;
			base.get_transform().GetComponent<UIPanel>().alpha = 0f;
		}

		private void setButtonMessage(GameObject obj, string name)
		{
			UIButtonMessage component = obj.GetComponent<UIButtonMessage>();
			component.target = base.get_gameObject();
			component.functionName = name;
			component.trigger = UIButtonMessage.Trigger.OnClick;
		}

		private void OnDestroy()
		{
			Mem.Del<GameObject>(ref this._tenderPanel);
			Mem.Del<GameObject>(ref this._mamiyaPanel);
			Mem.Del<GameObject>(ref this._twinPanel);
			Mem.Del<GameObject>(ref this._animatePanel);
			Mem.Del(ref this._allBtn);
			Mem.Del(ref this._mamiyaBtn);
			Mem.Del(ref this._irakoBtn);
			Mem.Del<UITexture>(ref this._allC1);
			Mem.Del<UITexture>(ref this._allC2);
			Mem.Del<UITexture>(ref this._mainBg);
			Mem.Del<UITexture>(ref this._otherBg);
			Mem.Del<UITexture>(ref this._ship);
			Mem.Del(ref this._item);
			Mem.Del(ref this._btnYes);
			Mem.Del(ref this._btnNo);
			Mem.Del<UILabel>(ref this._labelFrom);
			Mem.Del<UILabel>(ref this._labelTo);
			Mem.Del(ref this._tBtnYes);
			Mem.Del(ref this._tBtnNo);
			Mem.Del<UILabel>(ref this._labelFrom1);
			Mem.Del<UILabel>(ref this._labelTo1);
			Mem.Del<UILabel>(ref this._labelFrom2);
			Mem.Del<UILabel>(ref this._labelTo2);
			Mem.Del<UITexture>(ref this._ship1);
			Mem.Del<UITexture>(ref this._ship2);
			Mem.Del(ref this._parSystem);
			Mem.Del(ref this._parSystem2);
			Mem.Del<Animation>(ref this._animation);
			Mem.Del<UILabel>(ref this._topLabel);
			Mem.Del<OrganizeTender.TenderState>(ref this.State);
			Mem.DelDictionarySafe<SweetsType, bool>(ref this.tenderDic);
		}

		public void SetMainDialog()
		{
			if (this.tenderDic.get_Item(SweetsType.Mamiya))
			{
				this._mamiyaBtn.spriteName = ((this.setIndex != 0) ? "btn_mamiya" : "btn_mamiya_on");
				UISelectedObject.SelectedOneButtonZoomUpDown(this._mamiyaBtn.get_gameObject(), this.setIndex == 0);
			}
			else
			{
				this._mamiyaBtn.spriteName = "btn_mamiya_off";
				UISelectedObject.SelectedOneButtonZoomUpDown(this._mamiyaBtn.get_gameObject(), false);
			}
			if (this.tenderDic.get_Item(SweetsType.Both))
			{
				this._allBtn.spriteName = ((this.setIndex != 1) ? "btn_m+i" : "btn_m+i_on");
				UISelectedObject.SelectedOneButtonZoomUpDown(this._allBtn.get_gameObject(), this.setIndex == 1);
			}
			else
			{
				this._allBtn.spriteName = "btn_m+i_off";
				UISelectedObject.SelectedOneButtonZoomUpDown(this._allBtn.get_gameObject(), false);
			}
			if (this.tenderDic.get_Item(SweetsType.Irako))
			{
				this._irakoBtn.spriteName = ((this.setIndex != 2) ? "btn_irako" : "btn_irako_on");
				UISelectedObject.SelectedOneButtonZoomUpDown(this._allBtn.get_gameObject(), this.setIndex == 2);
			}
			else
			{
				this._irakoBtn.spriteName = "btn_irako_off";
				UISelectedObject.SelectedOneButtonZoomUpDown(this._irakoBtn.get_gameObject(), false);
			}
			if (this.setIndex == 0)
			{
				TaskOrganizeTop.KeyController.IsRun = false;
				this.moveAnimate(this._mamiyaBtn, 0f, -140f, true);
				this.moveAnimate(this._irakoBtn, -265f, -40f, false);
				this.moveAnimate(this._allBtn, 265f, -40f, false);
				UISelectedObject.SelectedOneButtonZoomUpDown(this._mamiyaBtn.get_gameObject(), true);
			}
			else if (this.setIndex == 1)
			{
				TaskOrganizeTop.KeyController.IsRun = false;
				this.moveAnimate(this._allBtn, 0f, -140f, true);
				this.moveAnimate(this._mamiyaBtn, -265f, -40f, false);
				this.moveAnimate(this._irakoBtn, 265f, -40f, false);
				UISelectedObject.SelectedOneButtonZoomUpDown(this._allBtn.get_gameObject(), true);
			}
			else if (this.setIndex == 2)
			{
				TaskOrganizeTop.KeyController.IsRun = false;
				this.moveAnimate(this._irakoBtn, 0f, -140f, true);
				this.moveAnimate(this._allBtn, -265f, -40f, false);
				this.moveAnimate(this._mamiyaBtn, 265f, -40f, false);
				UISelectedObject.SelectedOneButtonZoomUpDown(this._irakoBtn.get_gameObject(), true);
			}
			this.SetMainDialogAnimation();
		}

		public void SetMainDialogAnimation()
		{
			this.IsMoveMamiya = true;
			this.IsMoveIrako = true;
			this.IsMoveAll = true;
			if (this.tenderDic.get_Item(SweetsType.Mamiya) && this.setIndex == 0)
			{
				this.IsMoveMamiya = false;
				this.setAlphaAnimation();
			}
			if (this.tenderDic.get_Item(SweetsType.Both) && this.setIndex == 1)
			{
				this.IsMoveAll = false;
				this.setAlphaAnimation();
			}
			if (this.tenderDic.get_Item(SweetsType.Irako) && this.setIndex == 2)
			{
				this.IsMoveIrako = false;
				this.setAlphaAnimation();
			}
			SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove2);
		}

		public void CheckMainDialog()
		{
			if (this.tenderDic.get_Item(SweetsType.Both))
			{
				this.setIndex = 1;
			}
			else if (this.tenderDic.get_Item(SweetsType.Mamiya))
			{
				this.setIndex = 0;
			}
			else
			{
				this.setIndex = 2;
			}
			if (this.tenderDic.get_Item(SweetsType.Mamiya))
			{
				this._mamiyaBtn.spriteName = ((this.setIndex != 0) ? "btn_mamiya" : "btn_mamiya_on");
				bool value = this.setIndex == 0;
				UISelectedObject.SelectedOneButtonZoomUpDown(this._mamiyaBtn.get_gameObject(), value);
			}
			else
			{
				this._mamiyaBtn.spriteName = "btn_mamiya_off";
				UISelectedObject.SelectedOneButtonZoomUpDown(this._mamiyaBtn.get_gameObject(), false);
			}
			if (this.tenderDic.get_Item(SweetsType.Both))
			{
				this._allBtn.spriteName = ((this.setIndex != 1) ? "btn_m+i" : "btn_m+i_on");
				bool value2 = this.setIndex == 1;
				UISelectedObject.SelectedOneButtonZoomUpDown(this._allBtn.get_gameObject(), value2);
			}
			else
			{
				this._allBtn.spriteName = "btn_m+i_off";
				UISelectedObject.SelectedOneButtonZoomUpDown(this._allBtn.get_gameObject(), false);
			}
			if (this.tenderDic.get_Item(SweetsType.Irako))
			{
				this._irakoBtn.spriteName = ((this.setIndex != 2) ? "btn_irako" : "btn_irako_on");
				bool value3 = this.setIndex == 2;
				UISelectedObject.SelectedOneButtonZoomUpDown(this._irakoBtn.get_gameObject(), value3);
			}
			else
			{
				this._irakoBtn.spriteName = "btn_irako_off";
				UISelectedObject.SelectedOneButtonZoomUpDown(this._irakoBtn.get_gameObject(), false);
			}
			if (this.setIndex == 0)
			{
				this.MoveToMamiya();
			}
			else if (this.setIndex == 1)
			{
				this.MoveToMamiyaAndIrako();
			}
			else if (this.setIndex == 2)
			{
				this.MoveToIrako();
			}
		}

		private void MoveToMamiya()
		{
			this.MoveToStage(this._irakoBtn, this._mamiyaBtn, this._allBtn);
			this.StopShipAnimate(this._allC1.get_transform(), -38f, 0f);
			this._allC1.alpha = 1f;
			this._allC2.alpha = 0f;
		}

		private void MoveToMamiyaAndIrako()
		{
			this.MoveToStage(this._mamiyaBtn, this._allBtn, this._irakoBtn);
			this.StopShipAnimate(this._allC1.get_transform(), -38f, 0f);
			this.StopShipAnimate(this._allC2.get_transform(), 75f, 6f);
			this._allC1.alpha = 1f;
			this._allC2.alpha = 1f;
		}

		private void MoveToIrako()
		{
			this.MoveToStage(this._allBtn, this._irakoBtn, this._mamiyaBtn);
			this.StopShipAnimate(this._allC2.get_transform(), -38f, 0f);
			this._allC1.alpha = 0f;
			this._allC2.alpha = 1f;
		}

		private void MoveToStage(UISprite left, UISprite center, UISprite right)
		{
			this.setButtonAlpha();
			left.get_transform().set_localPosition(this.SWEETS_POSITION_LEFT);
			center.get_transform().set_localPosition(this.SWEETS_POSITION_CENTER);
			right.get_transform().set_localPosition(this.SWEETS_POSITION_RIGHT);
			left.get_transform().set_localScale(this.SWEETS_UNFOCUS_SCALE);
			center.get_transform().set_localScale(this.SWEETS_FOCUS_SCALE);
			right.get_transform().set_localScale(this.SWEETS_UNFOCUS_SCALE);
			UISelectedObject.SelectedOneButtonZoomUpDown(left.get_gameObject(), false);
			UISelectedObject.SelectedOneButtonZoomUpDown(center.get_gameObject(), true);
		}

		private void setButtonAlpha()
		{
			this._mamiyaBtn.alpha = ((!this.tenderDic.get_Item(SweetsType.Mamiya)) ? 0.6f : 1f);
			this._irakoBtn.alpha = ((!this.tenderDic.get_Item(SweetsType.Irako)) ? 0.6f : 1f);
			this._allBtn.alpha = ((!this.tenderDic.get_Item(SweetsType.Both)) ? 0.6f : 1f);
		}

		private void setUseDialog()
		{
			if (this.State == OrganizeTender.TenderState.Maimiya)
			{
				this._topLabel.text = "給糧艦「間宮」を使用しますか？";
				this._item.spriteName = "icon_mamiya";
				this._ship.mainTexture = (Resources.Load("Textures/Organize/sozai/popup2/img_mamiya") as Texture2D);
				this._ship.width = 256;
				this._ship.height = 512;
				this._labelFrom.textInt = OrganizeTaskManager.Instance.GetLogicManager().GetMamiyaCount();
				this._labelTo.textInt = OrganizeTaskManager.Instance.GetLogicManager().GetMamiyaCount() - 1;
			}
			else if (this.State == OrganizeTender.TenderState.Irako)
			{
				this._topLabel.text = "給糧艦「伊良湖」を使用しますか？";
				this._item.spriteName = "icon_irako";
				this._ship.mainTexture = (Resources.Load("Textures/Organize/sozai/popup2/img_irako") as Texture2D);
				this._ship.width = 256;
				this._ship.height = 512;
				this._labelFrom.textInt = OrganizeTaskManager.Instance.GetLogicManager().GetIrakoCount();
				this._labelTo.textInt = OrganizeTaskManager.Instance.GetLogicManager().GetIrakoCount() - 1;
			}
		}

		private void setTwinUseDialog()
		{
			this._labelFrom1.textInt = OrganizeTaskManager.Instance.GetLogicManager().GetMamiyaCount();
			this._labelTo1.textInt = OrganizeTaskManager.Instance.GetLogicManager().GetMamiyaCount() - 1;
			this._labelFrom2.textInt = OrganizeTaskManager.Instance.GetLogicManager().GetIrakoCount();
			this._labelTo2.textInt = OrganizeTaskManager.Instance.GetLogicManager().GetIrakoCount() - 1;
		}

		public void updateSubBtn()
		{
			if (this.setIndex2 == 1)
			{
				this._btnYes.spriteName = "btn_yes";
				this._btnNo.spriteName = "btn_no_on";
				UISelectedObject.SelectedOneButtonZoomUpDown(this._btnYes.get_gameObject(), false);
				UISelectedObject.SelectedOneButtonZoomUpDown(this._btnNo.get_gameObject(), true);
			}
			else
			{
				this._btnYes.spriteName = "btn_yes_on";
				this._btnNo.spriteName = "btn_no";
				UISelectedObject.SelectedOneButtonZoomUpDown(this._btnYes.get_gameObject(), true);
				UISelectedObject.SelectedOneButtonZoomUpDown(this._btnNo.get_gameObject(), false);
			}
		}

		public void updateTwinBtn()
		{
			if (this.setIndex2 == 1)
			{
				this._tBtnYes.spriteName = "btn_yes";
				this._tBtnNo.spriteName = "btn_no_on";
				UISelectedObject.SelectedOneButtonZoomUpDown(this._tBtnYes.get_gameObject(), false);
				UISelectedObject.SelectedOneButtonZoomUpDown(this._tBtnNo.get_gameObject(), true);
			}
			else
			{
				this._tBtnYes.spriteName = "btn_yes_on";
				this._tBtnNo.spriteName = "btn_no";
				UISelectedObject.SelectedOneButtonZoomUpDown(this._tBtnYes.get_gameObject(), true);
				UISelectedObject.SelectedOneButtonZoomUpDown(this._tBtnNo.get_gameObject(), false);
			}
		}

		public void ShowSelectTender()
		{
			base.get_transform().GetComponent<UIPanel>().alpha = 1f;
			this.State = OrganizeTender.TenderState.Select;
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			base.get_transform().GetComponent<UIPanel>().widgetsAreStatic = false;
			this.tenderDic = OrganizeTaskManager.Instance.GetLogicManager().GetAvailableSweets(OrganizeTaskManager.Instance.GetTopTask().GetDeckID());
			this._tenderPanel.get_transform().set_localPosition(new Vector3(0f, 0f));
			this._mainBg.get_transform().set_localPosition(new Vector3(0f, 0f));
			this._mainBg.get_gameObject().SafeGetTweenAlpha(0f, 0.6f, 0.5f, 0f, UITweener.Method.Linear, UITweener.Style.Once, this._mainBg.get_gameObject(), string.Empty);
			this._tenderPanel.get_gameObject().SafeGetTweenAlpha(0f, 1f, 0.5f, 0f, UITweener.Method.Linear, UITweener.Style.Once, this._tenderPanel.get_gameObject(), string.Empty);
			OrganizeTaskManager.GetDialogPopUp().Open(this._tenderPanel.get_gameObject(), 0f, 0f, 1f, 1f);
			this.CheckMainDialog();
		}

		public void Hide()
		{
			this._mainBg.get_gameObject().SafeGetTweenAlpha(0.6f, 0f, 0.5f, 0f, UITweener.Method.Linear, UITweener.Style.Once, base.get_gameObject(), "compHideAnimation");
			this._tenderPanel.get_gameObject().SafeGetTweenAlpha(1f, 0f, 0.5f, 0f, UITweener.Method.Linear, UITweener.Style.Once, this._tenderPanel.get_gameObject(), string.Empty);
			OrganizeTaskManager.GetDialogPopUp().Close(this._tenderPanel.get_gameObject(), 1f, 1f, 0f, 0f);
			this.State = OrganizeTender.TenderState.None;
		}

		private void compHideAnimation()
		{
			if (!this.isAnimation)
			{
				base.get_transform().GetComponent<UIPanel>().alpha = 0f;
			}
		}

		public void ShowUseDialog()
		{
			switch (this.setIndex)
			{
			case 0:
				this.ShowOther(OrganizeTender.TenderState.Maimiya);
				break;
			case 1:
				this.ShowTwinOther();
				break;
			case 2:
				this.ShowOther(OrganizeTender.TenderState.Irako);
				break;
			}
		}

		public void ShowOther(OrganizeTender.TenderState state)
		{
			this._mamiyaPanel.get_transform().set_localPosition(Vector3.get_zero());
			this._otherBg.get_transform().set_localPosition(Vector3.get_zero());
			this._mamiyaPanel.get_gameObject().SafeGetTweenAlpha(0f, 1f, 0.5f, 0f, UITweener.Method.Linear, UITweener.Style.Once, this._mamiyaPanel.get_gameObject(), string.Empty);
			this._otherBg.get_gameObject().SafeGetTweenAlpha(0f, 0.6f, 0.5f, 0f, UITweener.Method.Linear, UITweener.Style.Once, this._otherBg.get_gameObject(), string.Empty);
			OrganizeTaskManager.GetDialogPopUp().Open(this._mamiyaPanel.get_gameObject(), 0f, 0f, 1f, 1f);
			this.setIndex2 = 0;
			this.State = state;
			if (this.State == OrganizeTender.TenderState.Maimiya)
			{
				int num = Random.Range(0, 2);
				int voiceNum = -1;
				int num2 = num;
				if (num2 != 0)
				{
					if (num2 == 1)
					{
						voiceNum = 12;
					}
				}
				else
				{
					voiceNum = 11;
				}
				ShipUtils.PlayPortVoice(voiceNum);
				this.setUseDialog();
			}
			else if (this.State == OrganizeTender.TenderState.Irako)
			{
				this.State = OrganizeTender.TenderState.Irako;
				int num3 = Random.Range(0, 2);
				int voiceNum2 = -1;
				int num2 = num3;
				if (num2 != 0)
				{
					if (num2 == 1)
					{
						voiceNum2 = 14;
					}
				}
				else
				{
					voiceNum2 = 13;
				}
				ShipUtils.PlayPortVoice(voiceNum2);
				this.setUseDialog();
			}
			this.updateSubBtn();
		}

		public void HideOther()
		{
			this._mamiyaPanel.get_gameObject().SafeGetTweenAlpha(1f, 0f, 0.5f, 0f, UITweener.Method.Linear, UITweener.Style.Once, this._mamiyaPanel.get_gameObject(), string.Empty);
			this._otherBg.get_gameObject().SafeGetTweenAlpha(0.6f, 0f, 0.5f, 0f, UITweener.Method.Linear, UITweener.Style.Once, this._otherBg.get_gameObject(), string.Empty);
			OrganizeTaskManager.GetDialogPopUp().Close(this._mamiyaPanel.get_gameObject(), 1f, 1f, 0f, 0f);
			this.State = OrganizeTender.TenderState.Select;
		}

		public void ShowTwinOther()
		{
			this._twinPanel.get_transform().set_localPosition(Vector3.get_zero());
			this._otherBg.get_transform().set_localPosition(Vector3.get_zero());
			this._twinPanel.get_gameObject().SafeGetTweenAlpha(0f, 1f, 0.5f, 0f, UITweener.Method.Linear, UITweener.Style.Once, this._twinPanel.get_gameObject(), string.Empty);
			this._otherBg.get_gameObject().SafeGetTweenAlpha(0f, 0.6f, 0.5f, 0f, UITweener.Method.Linear, UITweener.Style.Once, this._otherBg.get_gameObject(), string.Empty);
			OrganizeTaskManager.GetDialogPopUp().Open(this._twinPanel.get_gameObject(), 0f, 0f, 1f, 1f);
			int num = Random.Range(0, 2);
			int voiceNum = -1;
			int num2 = num;
			if (num2 != 0)
			{
				if (num2 == 1)
				{
					voiceNum = 16;
				}
			}
			else
			{
				voiceNum = 15;
			}
			ShipUtils.PlayPortVoice(voiceNum);
			this.setIndex2 = 0;
			this.State = OrganizeTender.TenderState.Twin;
			this.setTwinUseDialog();
			this.updateTwinBtn();
		}

		public void HideTwinOther()
		{
			this._twinPanel.get_gameObject().SafeGetTweenAlpha(1f, 0f, 0.5f, 0f, UITweener.Method.Linear, UITweener.Style.Once, this._twinPanel.get_gameObject(), string.Empty);
			this._otherBg.get_gameObject().SafeGetTweenAlpha(0.6f, 0f, 0.5f, 0f, UITweener.Method.Linear, UITweener.Style.Once, this._otherBg.get_gameObject(), string.Empty);
			OrganizeTaskManager.GetDialogPopUp().Close(this._twinPanel.get_gameObject(), 1f, 1f, 0f, 0f);
			this.State = OrganizeTender.TenderState.Select;
		}

		[DebuggerHidden]
		private IEnumerator _HeadButtonEnable(float time = 4.5f)
		{
			OrganizeTender.<_HeadButtonEnable>c__Iterator9F <_HeadButtonEnable>c__Iterator9F = new OrganizeTender.<_HeadButtonEnable>c__Iterator9F();
			<_HeadButtonEnable>c__Iterator9F.time = time;
			<_HeadButtonEnable>c__Iterator9F.<$>time = time;
			return <_HeadButtonEnable>c__Iterator9F;
		}

		public void PlayAnimation()
		{
			this.isAnimation = true;
			this.State = OrganizeTender.TenderState.None;
			if (SingletonMonoBehaviour<UIPortFrame>.exist())
			{
				SingletonMonoBehaviour<UIPortFrame>.Instance.isColliderEnabled = false;
			}
			base.StartCoroutine(this._HeadButtonEnable(4.5f));
			if (this.setIndex == 0)
			{
				int num = Random.Range(0, 2);
				int voiceNum = -1;
				int num2 = num;
				if (num2 != 0)
				{
					if (num2 == 1)
					{
						voiceNum = 22;
					}
				}
				else
				{
					voiceNum = 21;
				}
				ShipUtils.PlayPortVoice(voiceNum);
				OrganizeTaskManager.Instance.GetLogicManager().UseSweets(OrganizeTaskManager.Instance.GetTopTask().GetDeckID(), SweetsType.Mamiya);
			}
			else if (this.setIndex == 1)
			{
				int num3 = Random.Range(0, 2);
				int voiceNum2 = -1;
				int num2 = num3;
				if (num2 != 0)
				{
					if (num2 == 1)
					{
						voiceNum2 = 26;
					}
				}
				else
				{
					voiceNum2 = 25;
				}
				ShipUtils.PlayPortVoice(voiceNum2);
				OrganizeTaskManager.Instance.GetLogicManager().UseSweets(OrganizeTaskManager.Instance.GetTopTask().GetDeckID(), SweetsType.Both);
			}
			else if (this.setIndex == 2)
			{
				int num4 = Random.Range(0, 2);
				int voiceNum3 = -1;
				int num2 = num4;
				if (num2 != 0)
				{
					if (num2 == 1)
					{
						voiceNum3 = 24;
					}
				}
				else
				{
					voiceNum3 = 23;
				}
				ShipUtils.PlayPortVoice(voiceNum3);
				OrganizeTaskManager.Instance.GetLogicManager().UseSweets(OrganizeTaskManager.Instance.GetTopTask().GetDeckID(), SweetsType.Irako);
			}
			this.SetAnimationPanel();
			this._animation.Play();
			this._parSystem.Play();
		}

		public void SetAnimationPanel()
		{
			if (this.setIndex == 0)
			{
				this._ship1.get_transform().set_localPosition(Vector3.get_zero());
				this._ship1.alpha = 1f;
				this._ship2.alpha = 0f;
			}
			else if (this.setIndex == 1)
			{
				this._ship1.get_transform().set_localPosition(new Vector3(-180f, 0f));
				this._ship2.get_transform().set_localPosition(new Vector3(215f, 0f));
				this._ship1.alpha = 1f;
				this._ship2.alpha = 1f;
			}
			else if (this.setIndex == 2)
			{
				this._ship2.get_transform().set_localPosition(Vector3.get_zero());
				this._ship1.alpha = 0f;
				this._ship2.alpha = 1f;
			}
		}

		public void CompDialogClose()
		{
			this.HideOther();
			this.HideTwinOther();
			this.Hide();
		}

		public void CompUpdateBanner()
		{
			OrganizeTaskManager.Instance.GetTopTask().UpdateChangeFatigue();
		}

		public void CompParticle()
		{
			this._parSystem2.get_gameObject().SetActive(true);
			this._parSystem2.Play();
		}

		public void EndAnimation()
		{
			this.isAnimation = false;
			this._animatePanel.get_transform().set_localPosition(new Vector3(-1065f, 620f));
			OrganizeTaskManager.Instance.GetTopTask().isControl = true;
			OrganizeTaskManager.Instance.GetTopTask().UpdateDeckSwitchManager();
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
			base.get_transform().GetComponent<UIPanel>().alpha = 0f;
		}

		public void AllUpAnimate()
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add("time", 2f);
			hashtable.Add("y", 20f);
			hashtable.Add("easeType", iTween.EaseType.easeInOutSine);
			hashtable.Add("isLocal", true);
			hashtable.Add("oncomplete", "AllDownAnimate");
			hashtable.Add("oncompletetarget", base.get_gameObject());
			this._allC1.get_transform().get_gameObject().MoveTo(hashtable);
			this._allC2.get_transform().get_gameObject().MoveTo(hashtable);
		}

		public void AllDownAnimate()
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add("time", 2f);
			hashtable.Add("y", -10f);
			hashtable.Add("easeType", iTween.EaseType.easeInOutSine);
			hashtable.Add("isLocal", true);
			hashtable.Add("oncomplete", "AllUpAnimate");
			hashtable.Add("oncompletetarget", base.get_gameObject());
			this._allC1.get_transform().get_gameObject().MoveTo(hashtable);
			this._allC2.get_transform().get_gameObject().MoveTo(hashtable);
		}

		public void StopAllAnimate()
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add("time", 0.05f);
			hashtable.Add("x", -38f);
			hashtable.Add("y", 0f);
			hashtable.Add("easeType", iTween.EaseType.linear);
			hashtable.Add("isLocal", true);
			this._allC1.get_transform().get_gameObject().MoveTo(hashtable);
			Hashtable hashtable2 = new Hashtable();
			hashtable2.Add("time", 0.05f);
			hashtable2.Add("x", 76f);
			hashtable2.Add("y", 0f);
			hashtable2.Add("easeType", iTween.EaseType.linear);
			hashtable2.Add("isLocal", true);
			this._allC2.get_transform().get_gameObject().MoveTo(hashtable2);
		}

		public void AlphaInAnimate()
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add("time", 2f);
			hashtable.Add("y", 20f);
			hashtable.Add("easeType", iTween.EaseType.easeInOutSine);
			hashtable.Add("isLocal", true);
			hashtable.Add("oncomplete", "AllDownAnimate");
			hashtable.Add("oncompletetarget", base.get_gameObject());
			this._allC1.get_transform().get_gameObject().MoveTo(hashtable);
			this._allC2.get_transform().get_gameObject().MoveTo(hashtable);
		}

		public void AlphaAnimate(Transform trans, float from, float to)
		{
			trans.SafeGetTweenAlpha(from, to, 0.3f, 0f, UITweener.Method.Linear, UITweener.Style.Once, base.get_gameObject(), string.Empty);
		}

		public void moveAnimate(UISprite sprite, float _toX, float to_y, bool isSet)
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add("time", 0.3f);
			hashtable.Add("x", _toX);
			hashtable.Add("y", to_y);
			hashtable.Add("easeType", iTween.EaseType.easeInOutSine);
			hashtable.Add("isLocal", true);
			hashtable.Add("delay", 0.1f);
			hashtable.Add("oncomplete", "compMoveAnimation");
			hashtable.Add("oncompletetarget", base.get_gameObject());
			sprite.get_transform().MoveTo(hashtable);
			Hashtable hashtable2 = new Hashtable();
			hashtable2.Add("time", 0.3f);
			if (isSet)
			{
				hashtable2.Add("scale", new Vector3(1f, 1f, 1f));
				this.AlphaAnimate(sprite.get_transform(), 0.6f, 1f);
			}
			else
			{
				hashtable2.Add("scale", new Vector3(0.7f, 0.7f, 0.7f));
			}
			hashtable2.Add("easeType", iTween.EaseType.easeInOutSine);
			hashtable2.Add("isLocal", true);
			sprite.get_transform().ScaleTo(hashtable2);
		}

		public void compMoveAnimation()
		{
			TaskOrganizeTop.KeyController.IsRun = true;
		}

		private void setAlphaAnimation()
		{
			this._allC1.alpha = 0f;
			this._allC2.alpha = 0f;
			if (this.setIndex == 0)
			{
				this.StopShipAnimate(this._allC1.get_transform(), 0f, 0f);
				this.AlphaAnimate(this._allC1.get_transform(), 0f, 1f);
				this.AlphaAnimate(this._allC2.get_transform(), 0f, 0f);
			}
			else if (this.setIndex == 1)
			{
				this.StopShipAnimate(this._allC1.get_transform(), -38f, 0f);
				this.StopShipAnimate(this._allC2.get_transform(), 75f, 6f);
				this.AlphaAnimate(this._allC1.get_transform(), 0f, 1f);
				this.AlphaAnimate(this._allC2.get_transform(), 0f, 1f);
			}
			else if (this.setIndex == 2)
			{
				this.StopShipAnimate(this._allC2.get_transform(), 0f, 0f);
				this.AlphaAnimate(this._allC2.get_transform(), 0f, 1f);
				this.AlphaAnimate(this._allC1.get_transform(), 0f, 0f);
			}
		}

		public void StopShipAnimate(Transform trans, float x, float y)
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add("time", 0.02f);
			hashtable.Add("x", x);
			hashtable.Add("y", y);
			hashtable.Add("easeType", iTween.EaseType.linear);
			hashtable.Add("isLocal", true);
			trans.MoveTo(hashtable);
		}

		public void AllBtnEL(GameObject obj)
		{
			if (OrganizeTaskManager.Instance.GetDetailTask().CheckBtnEnabled() && this.setIndex == 1)
			{
				this.ShowTwinOther();
			}
		}

		public void IrakoBtnEL(GameObject obj)
		{
			if (OrganizeTaskManager.Instance.GetDetailTask().CheckBtnEnabled() && this.setIndex == 2)
			{
				this.ShowOther(OrganizeTender.TenderState.Irako);
			}
		}

		public void MamiyaBtnEL(GameObject obj)
		{
			if (OrganizeTaskManager.Instance.GetDetailTask().CheckBtnEnabled() && this.setIndex == 0)
			{
				this.ShowOther(OrganizeTender.TenderState.Maimiya);
			}
		}

		public void MainBackEL(GameObject obj)
		{
			if (OrganizeTaskManager.Instance.GetDetailTask().CheckBtnEnabled() && TaskOrganizeTop.controlState != "system")
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonRemove);
				this.Hide();
				OrganizeTaskManager.Instance.GetTopTask().setControlState();
			}
		}

		public void OtherBackEL(GameObject obj)
		{
			if (OrganizeTaskManager.Instance.GetDetailTask().CheckBtnEnabled())
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonRemove);
				if (this.State == OrganizeTender.TenderState.Maimiya || this.State == OrganizeTender.TenderState.Irako)
				{
					this.HideOther();
				}
				else if (this.State == OrganizeTender.TenderState.Twin)
				{
					this.HideTwinOther();
				}
			}
		}

		public void BtnYesEL(GameObject obj)
		{
			if (OrganizeTaskManager.Instance.GetDetailTask().CheckBtnEnabled())
			{
				this._GuideOff = true;
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
				this.PlayAnimation();
				base.StartCoroutine(this.GuideResume());
				OrganizeTaskManager.Instance.GetTopTask().setControlState();
				OrganizeTaskManager.Instance.GetTopTask().isControl = false;
				SoundUtils.PlaySE(SEFIleInfos.SE_004);
			}
		}

		[DebuggerHidden]
		private IEnumerator GuideResume()
		{
			OrganizeTender.<GuideResume>c__IteratorA0 <GuideResume>c__IteratorA = new OrganizeTender.<GuideResume>c__IteratorA0();
			<GuideResume>c__IteratorA.<>f__this = this;
			return <GuideResume>c__IteratorA;
		}

		public void BtnNoEL(GameObject obj)
		{
			if (OrganizeTaskManager.Instance.GetDetailTask().CheckBtnEnabled())
			{
				this.OtherBackEL(null);
			}
		}
	}
}
