using KCV;
using KCV.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UIShipAlbumDetailForMarriaged : UIShipAlbumDetail
{
	[SerializeField]
	private Animation mAnimation_MarriagedRing;

	[SerializeField]
	private UIButton mButton_PlayMarriageMovie;

	private Action mOnRequestPlayMarriageMovieListener;

	protected override void OnSelectCircleButton()
	{
		if (this.mButton_Prev.Equals(this.mCurrentFocusButton))
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			base.PrevImage();
		}
		else if (this.mButton_Voice.Equals(this.mCurrentFocusButton))
		{
			base.PlayVoice();
		}
		else if (this.mButton_Next.Equals(this.mCurrentFocusButton))
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			base.NextImage();
		}
		else if (this.mButton_PlayMarriageMovie.Equals(this.mCurrentFocusButton))
		{
			this.RequestPlayMarriageMovie();
		}
	}

	public void SetOnRequestPlayMarriageMovieListener(Action onRequestPlayMarriageMovieListener)
	{
		this.mOnRequestPlayMarriageMovieListener = onRequestPlayMarriageMovieListener;
	}

	private void RequestPlayMarriageMovie()
	{
		if (this.mOnRequestPlayMarriageMovieListener != null)
		{
			this.mOnRequestPlayMarriageMovieListener.Invoke();
		}
	}

	[Obsolete("Inspector上で設定して使用します")]
	public void OnTouchPlayMarriageMovie()
	{
		this.RequestPlayMarriageMovie();
	}

	protected override UIButton[] GetFocasableButtons()
	{
		List<UIButton> list = new List<UIButton>();
		list.Add(this.mButton_Prev);
		list.Add(this.mButton_Voice);
		list.Add(this.mButton_PlayMarriageMovie);
		list.Add(this.mButton_Next);
		return list.ToArray();
	}
}
