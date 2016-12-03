using Common.Enum;
using DG.Tweening;
using KCV.Scene.Port;
using System;
using UnityEngine;

public class UIMissionJudgeCutIn : MonoBehaviour
{
	[SerializeField]
	private UITexture mTexture_Background;

	[SerializeField]
	private UITexture mTexture_Text;

	[SerializeField]
	private Transform mTransform_Result;

	[SerializeField]
	private UITexture mTexture_Result;

	private Action mOnFinishedAnimationListener;

	private MissionResultKinds mMissionResultKind;

	private void Awake()
	{
		this.Reset();
	}

	public void Reset()
	{
		this.mTexture_Text.alpha = 0.001f;
		this.mTexture_Text.get_transform().set_localPosition(new Vector3(600f, 0f));
		this.mTransform_Result.localPositionX(284f);
		this.mTexture_Result.get_transform().set_localScale(new Vector3(1.2f, 1.2f));
		this.mTexture_Result.alpha = 0.001f;
		this.mTexture_Background.get_transform().set_localScale(new Vector3(1f, 0f, 1f));
	}

	public void Initialize(MissionResultKinds missionResultKind)
	{
		this.mMissionResultKind = missionResultKind;
		Vector3 zero = Vector3.get_zero();
		int num;
		switch (missionResultKind)
		{
		case MissionResultKinds.FAILE:
			num = 3;
			this.mTexture_Result.SetDimensions(276, 145);
			break;
		case MissionResultKinds.SUCCESS:
			num = 2;
			this.mTexture_Result.SetDimensions(276, 145);
			break;
		case MissionResultKinds.GREAT:
			num = 5;
			this.mTexture_Result.SetDimensions(377, 146);
			break;
		default:
			num = 1;
			break;
		}
		string text = string.Format("Textures/Mission/operation_judge_txt_0{0}", num);
		this.mTexture_Result.mainTexture = Resources.Load<Texture2D>(text);
	}

	public void SetOnFinishedAnimationListener(Action action)
	{
		this.mOnFinishedAnimationListener = action;
	}

	public void Play()
	{
		ShortcutExtensions.DOKill(base.get_transform(), false);
		Sequence sequence = DOTween.Sequence();
		Sequence sequence2 = DOTween.Sequence();
		ShortcutExtensions.DOKill(this.mTexture_Background.get_transform(), false);
		TweenSettingsExtensions.Append(sequence2, TweenSettingsExtensions.OnPlay<Tweener>(ShortcutExtensions.DOScaleY(this.mTexture_Background.get_transform(), 1f, 0.8f), delegate
		{
			this.mTexture_Background.alpha = 1f;
		}));
		Sequence sequence3 = DOTween.Sequence();
		ShortcutExtensions.DOKill(this.mTexture_Text.get_transform(), false);
		TweenSettingsExtensions.Join(sequence3, TweenSettingsExtensions.OnPlay<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMoveX(this.mTexture_Text.get_transform(), -208f, 0.8f, false), 21), delegate
		{
			this.mTexture_Text.alpha = 1f;
		}));
		ShortcutExtensions.DOKill(this.mTexture_Result.get_transform(), false);
		TweenSettingsExtensions.Join(sequence3, TweenSettingsExtensions.OnPlay<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.SetDelay<Tweener>(ShortcutExtensions.DOScale(this.mTexture_Result.get_transform(), Vector3.get_one(), 0.5f), 0.9f), 30), delegate
		{
			this.mTexture_Result.alpha = 1f;
		}));
		TweenSettingsExtensions.Append(sequence, sequence2);
		TweenSettingsExtensions.Append(sequence, sequence3);
		TweenSettingsExtensions.OnComplete<Sequence>(sequence, delegate
		{
			if (this.mOnFinishedAnimationListener != null)
			{
				this.mOnFinishedAnimationListener.Invoke();
			}
		});
	}

	private void OnDestroy()
	{
		UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_Background, false);
		UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_Text, false);
		UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_Result, false);
		this.mTransform_Result = null;
		this.mOnFinishedAnimationListener = null;
	}
}
