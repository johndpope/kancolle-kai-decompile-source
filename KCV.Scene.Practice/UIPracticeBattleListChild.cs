using Common.Enum;
using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Scene.Practice
{
	[RequireComponent(typeof(UIWidget))]
	public class UIPracticeBattleListChild : MonoBehaviour
	{
		private UIWidget mWidgetThis;

		[SerializeField]
		private UIButton mButton;

		[SerializeField]
		private UITexture mTexture_FlagShip;

		[SerializeField]
		private UITexture mTexture_DeckNo;

		private DeckModel mDeckModel;

		private List<IsGoCondition> mConditions;

		private Action<UIPracticeBattleListChild> mOnClickListener;

		public float alpha
		{
			get
			{
				if (this.mWidgetThis != null)
				{
					return this.mWidgetThis.alpha;
				}
				return 1f;
			}
			set
			{
				if (this.mWidgetThis != null)
				{
					this.mWidgetThis.alpha = value;
				}
			}
		}

		private void Awake()
		{
			this.mWidgetThis = base.GetComponent<UIWidget>();
		}

		public void Initialize(DeckModel deckModel, List<IsGoCondition> conditions)
		{
			this.mDeckModel = deckModel;
			this.mConditions = conditions;
			this.mTexture_DeckNo.mainTexture = Resources.Load<Texture>(string.Format("Textures/Common/DeckFlag/icon_deck{0}", deckModel.Id));
			this.mTexture_FlagShip.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(deckModel.GetFlagShip().MstId, (!deckModel.GetFlagShip().IsDamaged()) ? 1 : 2);
			bool battleCondition = this.IsStartBattleCondition();
			this.InitializeCondition(battleCondition);
		}

		private void InitializeCondition(bool battleCondition)
		{
			if (battleCondition)
			{
				this.mTexture_FlagShip.color = new Color(1f, 1f, 1f);
				this.mButton.defaultColor = Color.get_white();
			}
			else
			{
				this.mTexture_FlagShip.color = new Color(0.5f, 0.5f, 0.5f);
				this.mButton.defaultColor = Color.get_gray();
			}
		}

		public DeckModel GetDeckModel()
		{
			return this.mDeckModel;
		}

		public List<IsGoCondition> GetConditions()
		{
			return this.mConditions;
		}

		public bool IsStartBattleCondition()
		{
			return 0 == this.mConditions.get_Count();
		}

		public void ParentHasChanged()
		{
			this.mWidgetThis.ParentHasChanged();
		}

		public void SetOnClickListener(Action<UIPracticeBattleListChild> onClickListener)
		{
			this.mOnClickListener = onClickListener;
		}

		public void OnClickChild()
		{
			this.ClickChild();
		}

		private void ClickChild()
		{
			if (this.mOnClickListener != null)
			{
				this.mOnClickListener.Invoke(this);
			}
		}

		public void Hover()
		{
			this.mButton.SetState(UIButtonColor.State.Hover, true);
		}

		public void RemoveHover()
		{
			this.mButton.SetState(UIButtonColor.State.Normal, true);
		}

		private void OnDestroy()
		{
			this.mWidgetThis = null;
			this.mButton = null;
			this.mTexture_FlagShip = null;
			this.mTexture_DeckNo = null;
			this.mDeckModel = null;
			this.mConditions = null;
		}
	}
}
