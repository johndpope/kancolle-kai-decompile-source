using Common.Enum;
using LT.Tweening;
using System;
using UnityEngine;

namespace KCV.SortieMap
{
	[RequireComponent(typeof(UIPanel))]
	public class UIAreaMapFrame : MonoBehaviour
	{
		[SerializeField]
		private UILabel _uiMessageLabel;

		[SerializeField]
		private Transform _traCompass;

		[SerializeField]
		private Transform _traMessageBox;

		[SerializeField]
		private UISprite _uiInputIcon;

		private float _fShowAnimationTime = 0.7f;

		private float _fHideAnimationTime = 0.7f;

		private UIPanel _uiPanel;

		private Vector3 _vDefaultMessagePos;

		public UIPanel panel
		{
			get
			{
				return this.GetComponentThis(ref this._uiPanel);
			}
		}

		private void Awake()
		{
			this._uiInputIcon.alpha = 0f;
			this._traCompass.GetComponent<UIWidget>().alpha = 0f;
			this._traMessageBox.GetComponent<UIWidget>().alpha = 0f;
			this._vDefaultMessagePos = this._uiMessageLabel.get_transform().get_localPosition();
		}

		private void OnDestroy()
		{
			Mem.Del<UILabel>(ref this._uiMessageLabel);
			Mem.Del<Transform>(ref this._traCompass);
			Mem.Del<Transform>(ref this._traMessageBox);
			Mem.Del(ref this._uiInputIcon);
			Mem.Del<float>(ref this._fShowAnimationTime);
			Mem.Del<float>(ref this._fHideAnimationTime);
			Mem.Del<UIPanel>(ref this._uiPanel);
		}

		public void SetMessage(string message)
		{
			this._uiMessageLabel.text = message;
			this._uiInputIcon.alpha = ((!message.Equals("艦隊の針路を選択できます。\n提督、どちらの針路を選択しますか？")) ? 0f : 1f);
			this._uiMessageLabel.get_transform().set_localPosition((!message.Equals("陣形を選択してください。")) ? this._vDefaultMessagePos : new Vector3(60f, 35f, 0f));
		}

		public void SetMessage(enumMapEventType iType, enumMapWarType iWarType)
		{
			string message = string.Empty;
			if (iType == enumMapEventType.Stupid)
			{
				if (iWarType != enumMapWarType.None)
				{
					if (iWarType != enumMapWarType.Normal)
					{
						message = string.Empty;
					}
					else
					{
						message = "敵影を見ず。";
					}
				}
				else
				{
					message = "気のせいだった。";
				}
			}
			this.SetMessage(message);
		}

		public void SetMessage(MapAirReconnaissanceKind iKind)
		{
			string message = string.Empty;
			switch (iKind)
			{
			case MapAirReconnaissanceKind.Impossible:
				message = "航空偵察予定地点に到着しましたが、\n稼働偵察機がないため、偵察を中止します。";
				break;
			case MapAirReconnaissanceKind.LargePlane:
				message = "大型飛行艇による\n航空偵察を実施します。";
				break;
			case MapAirReconnaissanceKind.WarterPlane:
				message = "水上偵察機による\n航空偵察を実施します。";
				break;
			}
			this.SetMessage(message);
		}

		public void ClearMessage()
		{
			this._uiMessageLabel.text = string.Empty;
			this._uiInputIcon.alpha = 0f;
		}

		public LTDescr Show()
		{
			this._traMessageBox.LTMoveLocalY(0f, this._fShowAnimationTime).setEase(LeanTweenType.easeOutQuad);
			this._traCompass.LTMoveLocalX(-410f, this._fShowAnimationTime).setEase(LeanTweenType.easeOutQuad);
			this._traCompass.LTRotate(new Vector3(0f, 0f, -45f), this._fShowAnimationTime).setEase(LeanTweenType.easeOutQuad);
			UIWidget compass = this._traCompass.GetComponent<UIWidget>();
			UIWidget messageBox = this._traMessageBox.GetComponent<UIWidget>();
			return base.get_transform().LTValue(0f, 1f, this._fShowAnimationTime).setEase(LeanTweenType.easeOutQuad).setOnUpdate(delegate(float x)
			{
				UIWidget arg_15_0 = compass;
				messageBox.alpha = x;
				arg_15_0.alpha = x;
			});
		}

		public LTDescr Hide()
		{
			this._traMessageBox.LTMoveLocalY(-322f, this._fHideAnimationTime).setEase(LeanTweenType.easeOutQuad);
			this._traCompass.LTMoveLocalX(-570f, this._fHideAnimationTime).setEase(LeanTweenType.easeOutQuad);
			this._traCompass.LTRotateLocal(Vector3.get_zero(), this._fHideAnimationTime).setEase(LeanTweenType.easeOutQuad);
			UIWidget compass = this._traCompass.GetComponent<UIWidget>();
			UIWidget messageBox = this._traMessageBox.GetComponent<UIWidget>();
			return this._traCompass.LTValue(1f, 0f, this._fHideAnimationTime).setEase(LeanTweenType.easeOutQuad).setOnUpdate(delegate(float x)
			{
				UIWidget arg_15_0 = compass;
				messageBox.alpha = x;
				arg_15_0.alpha = x;
			});
		}
	}
}
