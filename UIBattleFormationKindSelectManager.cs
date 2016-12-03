using AnimationOrTween;
using Common.Enum;
using DG.Tweening;
using KCV;
using KCV.Battle.Formation;
using KCV.Display;
using KCV.Interface;
using System;
using UnityEngine;

public class UIBattleFormationKindSelectManager : MonoBehaviour, IUIKeyControllable
{
	public enum ActionType
	{
		Select,
		SelectAuto
	}

	[Serializable]
	private class Arrow
	{
		[SerializeField]
		public UITexture mTexture_ArrowFront;

		[SerializeField]
		public UITexture mTexture_ArrowBack;
	}

	public delegate void UIBattleFormationKindSelectManagerAction(UIBattleFormationKindSelectManager.ActionType actionType, UIBattleFormationKindSelectManager calledObject, UIBattleFormationKind centerView);

	[SerializeField]
	private UIBattleFormationKindSelectManager.Arrow mArrow_Left;

	[SerializeField]
	private UIBattleFormationKindSelectManager.Arrow mArrow_Right;

	[SerializeField]
	private UIBattleFormationKindSelector mUIBattleFormationKindSelector;

	[SerializeField]
	private UIDisplaySwipeEventRegion mUIDisplaySwipeEventRegion_FormationChange;

	[SerializeField]
	private UILabel mLabel_Formation_Center;

	[SerializeField]
	private UILabel mLabel_Formation_Right;

	[SerializeField]
	private UILabel mLabel_Formation_Left;

	[SerializeField]
	private UILabel mLabel_Formation_Temp;

	[SerializeField]
	private UIWidget mWidget_FormationSelecteNames;

	private UILabel mLabelCenter;

	private UILabel mLabelRight;

	private UILabel mLabelLeft;

	private UILabel mLabelTemp;

	private Vector3 mVector3_DefaultPositionCenter;

	private Vector3 mVector3_DefaultPositionRight;

	private Vector3 mVector3_DefaultPositionLeft;

	private Vector3 mVector3_DefaultPositionTemp;

	private Vector3 mVector3_DefaultScaleCenter;

	private Vector3 mVector3_DefaultScaleRight;

	private Vector3 mVector3_DefaultScaleLeft;

	private Vector3 mVector3_DefaultScaleTemp;

	private float mDefaultCenterAlpha;

	private float mDefaultLeftAlpha;

	private float mDefaultRightAlpha;

	private float mDefaultTempAlpha;

	private UIBattleFormationKindSelectManager.UIBattleFormationKindSelectManagerAction mUIBattleFormationKindSelectManagerAction;

	private bool mManualUpdate;

	[SerializeField]
	private float moveDuration = 0.3f;

	[SerializeField]
	private float arrowDuration = 0.3f;

	[SerializeField]
	private float arrowAlpha = 0.6f;

	private void Start()
	{
		BattleFormationKinds1[] expr_06 = new BattleFormationKinds1[]
		{
			BattleFormationKinds1.FukuJuu,
			BattleFormationKinds1.Rinkei,
			BattleFormationKinds1.TanJuu,
			BattleFormationKinds1.TanOu
		};
	}

	private void Awake()
	{
		this.mUIBattleFormationKindSelector.SetActive(false);
		this.mLabelCenter = this.mLabel_Formation_Center;
		this.mLabelRight = this.mLabel_Formation_Right;
		this.mLabelLeft = this.mLabel_Formation_Left;
		this.mLabelTemp = this.mLabel_Formation_Temp;
		this.mVector3_DefaultPositionCenter = this.mLabelCenter.get_transform().get_localPosition();
		this.mVector3_DefaultPositionRight = this.mLabelRight.get_transform().get_localPosition();
		this.mVector3_DefaultPositionLeft = this.mLabelLeft.get_transform().get_localPosition();
		this.mVector3_DefaultPositionTemp = this.mLabelTemp.get_transform().get_localPosition();
		this.mVector3_DefaultScaleCenter = this.mLabelCenter.get_transform().get_localScale();
		this.mVector3_DefaultScaleRight = this.mLabelRight.get_transform().get_localScale();
		this.mVector3_DefaultScaleLeft = this.mLabelLeft.get_transform().get_localScale();
		this.mVector3_DefaultScaleTemp = this.mLabelTemp.get_transform().get_localScale();
		this.mDefaultCenterAlpha = this.mLabelCenter.alpha;
		this.mDefaultRightAlpha = this.mLabelRight.alpha;
		this.mDefaultLeftAlpha = this.mLabelLeft.alpha;
		this.mDefaultTempAlpha = this.mLabelTemp.alpha;
	}

	private void Update()
	{
		if (this.mUIBattleFormationKindSelector != null && this.mManualUpdate)
		{
			this.mUIBattleFormationKindSelector.OnUpdatedKeyController();
		}
	}

	public void Initialize(Camera eventCamera, BattleFormationKinds1[] formations)
	{
		this.Initialize(eventCamera, formations, false);
	}

	public void Initialize(Camera eventCamera, BattleFormationKinds1[] formations, bool manualUpdate)
	{
		this.mWidget_FormationSelecteNames.alpha = 1f;
		this.mManualUpdate = manualUpdate;
		this.mLabelCenter.text = App.GetFormationText(formations[0]);
		this.mLabelRight.text = App.GetFormationText(formations[1]);
		this.mLabelLeft.text = App.GetFormationText(formations[formations.Length - 1]);
		this.mLabelTemp.text = string.Empty;
		this.mUIBattleFormationKindSelector.SetActive(true);
		this.mUIBattleFormationKindSelector.SetOnUIBattleFormationKindSelectorAction(delegate(UIBattleFormationKindSelector.ActionType actionType, UIBattleFormationKind centerView)
		{
			UIBattleFormationKind[] views = this.mUIBattleFormationKindSelector.GetViews();
			switch (actionType)
			{
			case UIBattleFormationKindSelector.ActionType.Select:
			{
				UIBattleFormationKind[] array = views;
				for (int i = 0; i < array.Length; i++)
				{
					UIBattleFormationKind uIBattleFormationKind = array[i];
					if (!(centerView == uIBattleFormationKind))
					{
						uIBattleFormationKind.Hide();
					}
				}
				this.mArrow_Left.mTexture_ArrowBack.alpha = 0f;
				this.mArrow_Left.mTexture_ArrowFront.alpha = 0f;
				this.mArrow_Right.mTexture_ArrowBack.alpha = 0f;
				this.mArrow_Right.mTexture_ArrowFront.alpha = 0f;
				this.mLabelRight.text = string.Empty;
				this.mLabelLeft.text = string.Empty;
				this.mLabelTemp.text = string.Empty;
				this.SetKeyController(null);
				this.mUIDisplaySwipeEventRegion_FormationChange.set_enabled(false);
				this.CallBack(UIBattleFormationKindSelectManager.ActionType.Select, this, centerView);
				break;
			}
			case UIBattleFormationKindSelector.ActionType.SelectAuto:
				this.CallBack(UIBattleFormationKindSelectManager.ActionType.SelectAuto, this, null);
				break;
			case UIBattleFormationKindSelector.ActionType.OnNextChanged:
			{
				UILabel uILabel = this.mLabelTemp;
				UILabel from = this.mLabelLeft;
				UILabel from2 = this.mLabelCenter;
				UILabel from3 = this.mLabelRight;
				int num = Array.IndexOf<BattleFormationKinds1>(formations, centerView.Category);
				int num2 = (int)Util.LoopValue(num + 1, 0f, (float)(formations.Length - 1));
				uILabel.text = App.GetFormationText(formations[num2]);
				bool flag = DOTween.IsTweening(this);
				if (flag)
				{
					DOTween.Kill(this, true);
				}
				Sequence sequence = TweenSettingsExtensions.SetId<Sequence>(DOTween.Sequence(), this);
				Tween tween = this.GenerateTweenMove(from2, this.mDefaultLeftAlpha, this.mVector3_DefaultPositionLeft, this.mVector3_DefaultScaleLeft, this.moveDuration);
				Tween tween2 = this.GenerateTweenMove(from, this.mDefaultTempAlpha, this.mVector3_DefaultPositionTemp, this.mVector3_DefaultScaleTemp, this.moveDuration);
				Tween tween3 = this.GenerateTweenMove(uILabel, this.mDefaultRightAlpha, this.mVector3_DefaultPositionRight, this.mVector3_DefaultScaleRight, this.moveDuration);
				Tween tween4 = this.GenerateTweenMove(from3, this.mDefaultCenterAlpha, this.mVector3_DefaultPositionCenter, this.mVector3_DefaultScaleCenter, this.moveDuration);
				TweenCallback tweenCallback = delegate
				{
				};
				TweenSettingsExtensions.Append(sequence, tween);
				TweenSettingsExtensions.Join(sequence, tween2);
				TweenSettingsExtensions.Join(sequence, tween3);
				TweenSettingsExtensions.Join(sequence, tween4);
				TweenSettingsExtensions.OnComplete<Sequence>(sequence, tweenCallback);
				this.mLabelCenter = from3;
				this.mLabelLeft = from2;
				this.mLabelTemp = from;
				this.mLabelRight = uILabel;
				this.mLabelCenter.depth = 2;
				this.mLabelLeft.depth = 1;
				this.mLabelRight.depth = 1;
				this.mLabelTemp.depth = 0;
				this.GenerateTweenArrow(this.mArrow_Right, Direction.Reverse);
				break;
			}
			case UIBattleFormationKindSelector.ActionType.OnPrevChanged:
			{
				UILabel uILabel = this.mLabelTemp;
				UILabel from = this.mLabelLeft;
				UILabel from2 = this.mLabelCenter;
				UILabel from3 = this.mLabelRight;
				int num = Array.IndexOf<BattleFormationKinds1>(formations, centerView.Category);
				int num2 = (int)Util.LoopValue(num - 1, 0f, (float)(formations.Length - 1));
				uILabel.text = App.GetFormationText(formations[num2]);
				bool flag = DOTween.IsTweening(this);
				if (flag)
				{
					DOTween.Kill(this, true);
				}
				Sequence sequence2 = TweenSettingsExtensions.SetId<Sequence>(DOTween.Sequence(), this);
				Tween tween5 = this.GenerateTweenMove(from2, this.mDefaultRightAlpha, this.mVector3_DefaultPositionRight, this.mVector3_DefaultScaleRight, this.moveDuration);
				Tween tween6 = this.GenerateTweenMove(from3, this.mDefaultTempAlpha, this.mVector3_DefaultPositionTemp, this.mVector3_DefaultScaleTemp, this.moveDuration);
				Tween tween7 = this.GenerateTweenMove(uILabel, this.mDefaultLeftAlpha, this.mVector3_DefaultPositionLeft, this.mVector3_DefaultScaleLeft, this.moveDuration);
				Tween tween8 = this.GenerateTweenMove(from, this.mDefaultCenterAlpha, this.mVector3_DefaultPositionCenter, this.mVector3_DefaultScaleCenter, this.moveDuration);
				TweenCallback tweenCallback2 = delegate
				{
				};
				TweenSettingsExtensions.Append(sequence2, tween5);
				TweenSettingsExtensions.Join(sequence2, tween6);
				TweenSettingsExtensions.Join(sequence2, tween7);
				TweenSettingsExtensions.Join(sequence2, tween8);
				TweenSettingsExtensions.OnComplete<Sequence>(sequence2, tweenCallback2);
				this.mLabelRight = from2;
				this.mLabelTemp = from3;
				this.mLabelLeft = uILabel;
				this.mLabelCenter = from;
				this.mLabelCenter.depth = 2;
				this.mLabelLeft.depth = 1;
				this.mLabelRight.depth = 1;
				this.mLabelTemp.depth = 0;
				this.GenerateTweenArrow(this.mArrow_Left, Direction.Forward);
				break;
			}
			}
		});
		this.mUIBattleFormationKindSelector.Initialize(formations);
		this.mUIBattleFormationKindSelector.Show();
		this.mUIDisplaySwipeEventRegion_FormationChange.SetEventCatchCamera(eventCamera);
		this.mUIDisplaySwipeEventRegion_FormationChange.SetOnSwipeActionJudgeCallBack(new UIDisplaySwipeEventRegion.SwipeJudgeDelegate(this.OnSwipeEvent));
	}

	private Tween GenerateTweenArrow(UIBattleFormationKindSelectManager.Arrow arrow, Direction direction)
	{
		int num = 0;
		UITexture arrowBack = Object.Instantiate<UITexture>(arrow.mTexture_ArrowBack);
		arrowBack.get_transform().set_parent(arrow.mTexture_ArrowBack.get_transform().get_parent());
		arrowBack.get_transform().set_localScale(arrow.mTexture_ArrowBack.get_transform().get_localScale());
		arrowBack.get_transform().set_localPosition(arrow.mTexture_ArrowBack.get_transform().get_localPosition());
		UITexture arrowFront = Object.Instantiate<UITexture>(arrow.mTexture_ArrowFront);
		arrowFront.get_transform().set_parent(arrow.mTexture_ArrowFront.get_transform().get_parent());
		arrowFront.get_transform().set_localScale(arrow.mTexture_ArrowFront.get_transform().get_localScale());
		arrowFront.get_transform().set_localPosition(arrow.mTexture_ArrowFront.get_transform().get_localPosition());
		switch (direction + 1)
		{
		case Direction.Toggle:
			num = 20;
			break;
		case (Direction)2:
			num = -20;
			break;
		}
		Sequence sequence = DOTween.Sequence();
		Tween tween = ShortcutExtensions.DOLocalMoveX(arrowBack.get_transform(), arrowBack.get_transform().get_localPosition().x + (float)num, this.arrowDuration, false);
		Tween tween2 = ShortcutExtensions.DOLocalMoveX(arrowFront.get_transform(), arrowFront.get_transform().get_localPosition().x + (float)num, this.arrowDuration, false);
		Tween tween3 = DOVirtual.Float(arrowBack.alpha, 0f, this.arrowAlpha, delegate(float alpha)
		{
			arrowBack.alpha = alpha;
			arrowFront.alpha = alpha;
		});
		TweenCallback tweenCallback = delegate
		{
			Object.Destroy(arrowBack.get_gameObject());
			Object.Destroy(arrowFront.get_gameObject());
		};
		TweenSettingsExtensions.Append(sequence, tween);
		TweenSettingsExtensions.Join(sequence, tween2);
		TweenSettingsExtensions.Join(sequence, tween3);
		TweenSettingsExtensions.OnComplete<Sequence>(sequence, tweenCallback);
		return sequence;
	}

	private Tween GenerateTweenMove(UILabel from, float toAlpha, Vector3 toPosition, Vector3 toScale, float duration)
	{
		Sequence sequence = DOTween.Sequence();
		Tween tween = ShortcutExtensions.DOLocalMoveX(from.get_transform(), toPosition.x, duration, false);
		Tween tween2 = ShortcutExtensions.DOScale(from.get_transform(), toScale, duration);
		Tween tween3 = DOVirtual.Float(from.alpha, toAlpha, duration, delegate(float alpha)
		{
			from.alpha = alpha;
		});
		TweenSettingsExtensions.Append(sequence, tween);
		TweenSettingsExtensions.Join(sequence, tween2);
		TweenSettingsExtensions.Join(sequence, tween3);
		return sequence;
	}

	public void SetOnUIBattleFormationKindSelectManagerAction(UIBattleFormationKindSelectManager.UIBattleFormationKindSelectManagerAction actionMethod)
	{
		this.mUIBattleFormationKindSelectManagerAction = actionMethod;
	}

	private void OnSwipeEvent(UIDisplaySwipeEventRegion.ActionType actionType, float deltaX, float deltaY, float movedPercentageX, float movedPercentageY, float elapsedTime)
	{
		if (actionType == UIDisplaySwipeEventRegion.ActionType.FingerUp)
		{
			float num = 0.2f;
			if (num < Math.Abs(movedPercentageX))
			{
				if (0f < movedPercentageX)
				{
					this.mUIBattleFormationKindSelector.Prev();
				}
				else
				{
					this.mUIBattleFormationKindSelector.Next();
				}
			}
		}
	}

	private void CallBack(UIBattleFormationKindSelectManager.ActionType actionType, UIBattleFormationKindSelectManager calledObject, UIBattleFormationKind centerView)
	{
		if (this.mUIBattleFormationKindSelectManagerAction != null)
		{
			this.mUIBattleFormationKindSelectManagerAction(actionType, this, centerView);
		}
	}

	public void SetKeyController(KeyControl keyController)
	{
		if (this.mUIBattleFormationKindSelector != null)
		{
			this.mUIBattleFormationKindSelector.SetKeyController(keyController, this.mUIDisplaySwipeEventRegion_FormationChange);
		}
	}

	public void SetKeyController(KeyControl keyController, UIDisplaySwipeEventRegion region)
	{
		if (this.mUIBattleFormationKindSelector != null)
		{
			this.mUIBattleFormationKindSelector.SetKeyController(keyController, this.mUIDisplaySwipeEventRegion_FormationChange);
		}
	}

	public void OnUpdatedKeyController()
	{
		if (this.mUIBattleFormationKindSelector != null)
		{
			this.mUIBattleFormationKindSelector.OnUpdatedKeyController();
		}
	}

	public void OnReleaseKeyController()
	{
		if (this.mUIBattleFormationKindSelector != null)
		{
			this.mUIBattleFormationKindSelector.OnReleaseKeyController();
		}
	}

	private void OnDestroy()
	{
		if (DOTween.IsTweening(this))
		{
			DOTween.Kill(this, false);
		}
		this.mArrow_Left = null;
		this.mArrow_Right = null;
		this.mUIBattleFormationKindSelector = null;
		this.mUIDisplaySwipeEventRegion_FormationChange = null;
		this.mLabel_Formation_Center = null;
		this.mLabel_Formation_Right = null;
		this.mLabel_Formation_Left = null;
		this.mLabel_Formation_Temp = null;
		this.mWidget_FormationSelecteNames = null;
		this.mLabelCenter = null;
		this.mLabelRight = null;
		this.mLabelLeft = null;
		this.mLabelTemp = null;
		this.mUIBattleFormationKindSelectManagerAction = null;
	}
}
