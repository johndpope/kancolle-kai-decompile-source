using Common.Enum;
using DG.Tweening;
using KCV.View.Scroll;
using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UIInteriorFurnitureChangeScrollList : UIScrollListParent<UIInteriorFurnitureChangeScrollListChildModel, UIInteriorFurnitureChangeScrollListChild>
{
	private enum TweenAnimationType
	{
		ShowHide
	}

	[SerializeField]
	private UITexture mTexture_Header;

	[SerializeField]
	private UILabel mLabel_Genre;

	[SerializeField]
	private UITexture mTexture_TouchBackArea;

	private Dictionary<UIInteriorFurnitureChangeScrollList.TweenAnimationType, UITweener> mTweeners;

	[Obsolete("Inspector上で設定して使用します")]
	public void OnTouchBack()
	{
		base.OnAction(ActionType.OnBack, this, this.ViewFocus);
	}

	protected override void OnStart()
	{
		this.mTweeners = new Dictionary<UIInteriorFurnitureChangeScrollList.TweenAnimationType, UITweener>();
	}

	public void Show()
	{
		if (DOTween.IsTweening(UIInteriorFurnitureChangeScrollList.TweenAnimationType.ShowHide))
		{
			DOTween.Kill(UIInteriorFurnitureChangeScrollList.TweenAnimationType.ShowHide, false);
		}
		Tween tween = TweenSettingsExtensions.SetDelay<Tweener>(TweenSettingsExtensions.SetId<Tweener>(DOVirtual.Float(this.mTexture_TouchBackArea.alpha, 0.5f, 0.15f, delegate(float alpha)
		{
			this.mTexture_TouchBackArea.alpha = alpha;
		}), UIInteriorFurnitureChangeScrollList.TweenAnimationType.ShowHide), 0.3f);
		TweenPosition tweenPosition = UITweener.Begin<TweenPosition>(base.get_gameObject(), 0.3f);
		tweenPosition.from = base.get_transform().get_localPosition();
		tweenPosition.to = new Vector3(0f, base.get_transform().get_localPosition().y, base.get_transform().get_localPosition().z);
		tweenPosition.ignoreTimeScale = true;
	}

	public void Hide()
	{
		if (DOTween.IsTweening(UIInteriorFurnitureChangeScrollList.TweenAnimationType.ShowHide))
		{
			DOTween.Kill(UIInteriorFurnitureChangeScrollList.TweenAnimationType.ShowHide, false);
		}
		Sequence sequence = DOTween.Sequence();
		Tween tween = DOVirtual.Float(this.mTexture_TouchBackArea.alpha, 1E-06f, 0.15f, delegate(float alpha)
		{
			this.mTexture_TouchBackArea.alpha = alpha;
		});
		Tween tween2 = TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMoveX(base.get_transform(), -960f, 0.6f, false), 21);
		TweenSettingsExtensions.Append(sequence, tween2);
		TweenSettingsExtensions.Join(sequence, tween);
		TweenSettingsExtensions.SetId<Sequence>(sequence, UIInteriorFurnitureChangeScrollList.TweenAnimationType.ShowHide);
	}

	public void Initialize(int deckId, FurnitureModel[] models)
	{
		UIInteriorFurnitureChangeScrollListChildModel[] models2 = this.GenerateChildrenDTOModel(deckId, models);
		base.Initialize(models2);
	}

	public void Refresh()
	{
		base.Refresh(this.Models);
	}

	private UIInteriorFurnitureChangeScrollListChildModel[] GenerateChildrenDTOModel(int deckId, FurnitureModel[] models)
	{
		List<UIInteriorFurnitureChangeScrollListChildModel> list = new List<UIInteriorFurnitureChangeScrollListChildModel>();
		for (int i = 0; i < models.Length; i++)
		{
			FurnitureModel model = models[i];
			UIInteriorFurnitureChangeScrollListChildModel uIInteriorFurnitureChangeScrollListChildModel = new UIInteriorFurnitureChangeScrollListChildModel(deckId, model);
			list.Add(uIInteriorFurnitureChangeScrollListChildModel);
		}
		return list.ToArray();
	}

	public void UpdateHeader(FurnitureKinds kinds)
	{
		switch (kinds)
		{
		case FurnitureKinds.Floor:
			this.mLabel_Genre.text = "床";
			this.mTexture_Header.color = new Color(0.65882355f, 0.423529416f, 0.4117647f);
			break;
		case FurnitureKinds.Wall:
			this.mLabel_Genre.text = "壁紙";
			this.mTexture_Header.color = new Color(161f, 0.4745098f, 0.356862754f);
			break;
		case FurnitureKinds.Window:
			this.mLabel_Genre.text = "窓枠＋カーテン";
			this.mTexture_Header.color = new Color(0.392156869f, 0.596078455f, 0.5882353f);
			break;
		case FurnitureKinds.Hangings:
			this.mLabel_Genre.text = "装飾";
			this.mTexture_Header.color = new Color(0.470588237f, 0.7058824f, 0.509803951f);
			break;
		case FurnitureKinds.Chest:
			this.mLabel_Genre.text = "家具";
			this.mTexture_Header.color = new Color(0.5882353f, 0.5882353f, 0.392156869f);
			break;
		case FurnitureKinds.Desk:
			this.mLabel_Genre.text = "椅子＋机";
			this.mTexture_Header.color = new Color(0.5254902f, 0.458823532f, 0.5803922f);
			break;
		}
	}

	public void Release()
	{
		this.mTexture_Header.mainTexture = null;
		this.mTexture_Header = null;
		this.mLabel_Genre = null;
	}

	private void OnDestroy()
	{
		if (DOTween.IsTweening(UIInteriorFurnitureChangeScrollList.TweenAnimationType.ShowHide))
		{
			DOTween.Kill(UIInteriorFurnitureChangeScrollList.TweenAnimationType.ShowHide, false);
		}
	}
}
