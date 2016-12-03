using KCV.View.ScrollView;
using Server_Models;
using System;
using UnityEngine;

namespace KCV.Furniture.JukeBox
{
	public class UIJukeBoxPlayListChild : MonoBehaviour, UIScrollListItem<Mst_bgm_jukebox, UIJukeBoxPlayListChild>
	{
		[SerializeField]
		private UITexture mTexture_Background;

		[SerializeField]
		private UILabel mLabel_Title;

		[SerializeField]
		private UILabel mLabel_Price;

		[SerializeField]
		private UILabel mLabel_Description;

		private Mst_bgm_jukebox mMst_bgm_jukebox;

		private Action<UIJukeBoxPlayListChild> mOnTouchListener;

		private Transform mTransform;

		private int mRealIndex;

		public void Initialize(int realIndex, Mst_bgm_jukebox model)
		{
			this.mRealIndex = realIndex;
			this.Initialize(model, model.Name, model.R_coins.ToString(), model.Remarks);
		}

		public void InitializeDefault(int realIndex)
		{
			this.mRealIndex = realIndex;
			this.Initialize(null, string.Empty, string.Empty, string.Empty);
		}

		[Obsolete("Inspector上で設定して使用します")]
		private void OnClick()
		{
			if (this.mOnTouchListener != null)
			{
				this.mOnTouchListener.Invoke(this);
			}
		}

		private void Initialize(Mst_bgm_jukebox jukeBoxModel, string musicTitle, string price, string description)
		{
			this.mMst_bgm_jukebox = jukeBoxModel;
			this.mLabel_Title.text = musicTitle;
			this.mLabel_Price.text = price;
			this.mLabel_Description.text = description;
			this.mTexture_Background.alpha = 0.0001f;
		}

		public int GetRealIndex()
		{
			return this.mRealIndex;
		}

		public Mst_bgm_jukebox GetModel()
		{
			return this.mMst_bgm_jukebox;
		}

		public int GetHeight()
		{
			return 36;
		}

		public void SetOnTouchListener(Action<UIJukeBoxPlayListChild> onTouchListener)
		{
			this.mOnTouchListener = onTouchListener;
		}

		public void Hover()
		{
			UISelectedObject.SelectedOneObjectBlink(this.mTexture_Background.get_gameObject(), true);
		}

		public void RemoveHover()
		{
			UISelectedObject.SelectedOneObjectBlink(this.mTexture_Background.get_gameObject(), false);
			this.mTexture_Background.alpha = 1E-05f;
		}

		public Transform GetTransform()
		{
			if (this.mTransform == null)
			{
				this.mTransform = base.get_transform();
			}
			return this.mTransform;
		}

		private void OnDestroy()
		{
			this.mTexture_Background = null;
			this.mLabel_Title = null;
			this.mLabel_Price = null;
			this.mLabel_Description = null;
			this.mMst_bgm_jukebox = null;
			this.mOnTouchListener = null;
			this.mTransform = null;
		}
	}
}
