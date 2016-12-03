using local.models;
using System;
using UnityEngine;

namespace KCV
{
	[RequireComponent(typeof(UIWidget))]
	public class UIHowTo : MonoBehaviour
	{
		[SerializeField]
		private UISprite container;

		[SerializeField]
		private Vector3 showPos;

		[SerializeField]
		private Vector3 hidePos;

		[SerializeField]
		private UIHowToItem itemPrefab;

		[SerializeField]
		private float animationDuration = 0.2f;

		[SerializeField]
		private float horizontalStartMargin = 30f;

		[SerializeField]
		private float horizontalItemMargin = 30f;

		[SerializeField]
		private UIHowToHorizontalAlign horizontalAlign;

		private SettingModel _model;

		private void Start()
		{
			this._model = new SettingModel();
			this.Hide(false);
		}

		private void OnDestroy()
		{
			Mem.Del(ref this.container);
			Mem.Del<UIHowToItem>(ref this.itemPrefab);
			Mem.Del<SettingModel>(ref this._model);
		}

		public void Refresh(params UIHowToDetail[] details)
		{
			float horizontalDirection = (float)((this.horizontalAlign != UIHowToHorizontalAlign.left) ? -1 : 1);
			float x = this.horizontalStartMargin * horizontalDirection;
			int childCount = base.get_transform().get_childCount();
			for (int i = 0; i < childCount; i++)
			{
				NGUITools.Destroy(base.get_transform().GetChild(0));
			}
			int childDepth = base.GetComponent<UIWidget>().depth + 1;
			GameObject parent = base.get_gameObject();
			details.ForEach(delegate(UIHowToDetail e)
			{
				UIHowToItem component = Util.Instantiate(this.itemPrefab.get_gameObject(), parent, false, false).GetComponent<UIHowToItem>();
				component.Init(Enum.GetName(typeof(HowToKey), e.key), e.label, childDepth);
				component.get_transform().localPositionY(0f);
				UIHowToHorizontalAlign uIHowToHorizontalAlign = this.horizontalAlign;
				if (uIHowToHorizontalAlign != UIHowToHorizontalAlign.left)
				{
					if (uIHowToHorizontalAlign == UIHowToHorizontalAlign.right)
					{
						component.get_transform().localPositionX(x - (float)component.GetWidth());
					}
				}
				else
				{
					component.get_transform().localPositionX(x);
				}
				x += ((float)component.GetWidth() + this.horizontalItemMargin) * horizontalDirection;
			});
		}

		public void Show()
		{
			this.Show(true);
		}

		public void Show(bool animation)
		{
			if (!this._model.GuideDisplay)
			{
				return;
			}
			if (animation)
			{
				this.Move(this.showPos);
			}
			else
			{
				base.get_transform().set_localPosition(this.showPos);
			}
		}

		public void Hide()
		{
			this.Hide(true);
		}

		public void Hide(bool animation)
		{
			if (animation)
			{
				this.Move(this.hidePos);
			}
			else
			{
				base.get_transform().set_localPosition(this.hidePos);
			}
		}

		private void Move(Vector3 to)
		{
			TweenPosition tweenPosition = UITweener.Begin<TweenPosition>(base.get_gameObject(), this.animationDuration);
			tweenPosition.from = base.get_gameObject().get_transform().get_localPosition();
			tweenPosition.to = to;
			tweenPosition.PlayForward();
		}

		public void SetHorizontalAlign(UIHowToHorizontalAlign iHorizontalAlign)
		{
			this.horizontalAlign = iHorizontalAlign;
		}
	}
}
