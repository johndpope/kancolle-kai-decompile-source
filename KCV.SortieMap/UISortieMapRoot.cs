using LT.Tweening;
using System;
using UnityEngine;

namespace KCV.SortieMap
{
	[RequireComponent(typeof(UISprite))]
	public class UISortieMapRoot : MonoBehaviour
	{
		[SerializeField]
		private bool _isRebellionRoute;

		[SerializeField]
		private UISprite _uiRoute;

		[SerializeField]
		private UISprite _uiRebellionRoute;

		[SerializeField]
		private UISprite _uiRebellionBrightPoint;

		private bool _isPassed;

		[Button("SameRoute", "Sprite設定", new object[]
		{

		}), SerializeField]
		private int _nSameRoute = 1;

		public UISprite route
		{
			get
			{
				return this.GetComponentThis(ref this._uiRoute);
			}
		}

		public UISprite rebellionRoute
		{
			get
			{
				if (this._uiRebellionRoute == null)
				{
					if (base.get_transform().FindChild("RebellionRoute"))
					{
						this._uiRebellionRoute = base.get_transform().FindChild("RebellionRoute").GetComponent<UISprite>();
					}
					else
					{
						GameObject gameObject = new GameObject("RebellionRoute");
						gameObject.get_transform().set_parent(base.get_transform());
						gameObject.get_transform().localScaleOne();
						gameObject.get_transform().localPositionZero();
						UISprite uiRebellionRoute = gameObject.AddComponent<UISprite>();
						this._uiRebellionRoute = uiRebellionRoute;
					}
				}
				return this._uiRebellionRoute;
			}
		}

		public UISprite rebellionBrightPoint
		{
			get
			{
				if (this._uiRebellionBrightPoint == null)
				{
					if (base.get_transform().FindChild("RebellionBrightPoint"))
					{
						this._uiRebellionBrightPoint = base.get_transform().FindChild("RebellionBrightPoint").GetComponent<UISprite>();
					}
					else
					{
						GameObject gameObject = new GameObject("RebellionBrightPoint");
						gameObject.get_transform().set_parent(base.get_transform());
						gameObject.get_transform().localScaleOne();
						gameObject.get_transform().localPositionZero();
						UISprite uiRebellionBrightPoint = gameObject.AddComponent<UISprite>();
						this._uiRebellionBrightPoint = uiRebellionBrightPoint;
					}
				}
				return this._uiRebellionBrightPoint;
			}
		}

		public bool isRebellionRoute
		{
			get
			{
				return this._isRebellionRoute;
			}
		}

		public bool isPassed
		{
			get
			{
				return this._isPassed;
			}
			set
			{
				if (value)
				{
					this.route.set_enabled(true);
					if (this.isRebellionRoute)
					{
						this.rebellionBrightPoint.get_transform().LTCancel();
						this.rebellionRoute.SetActive(false);
						this.rebellionBrightPoint.SetActive(false);
						Behaviour arg_5C_0 = this.rebellionRoute;
						bool enabled = false;
						this.rebellionBrightPoint.set_enabled(enabled);
						arg_5C_0.set_enabled(enabled);
					}
				}
				else
				{
					this.route.set_enabled(false);
					if (this.isRebellionRoute)
					{
						this.rebellionRoute.SetActive(true);
						this.rebellionBrightPoint.SetActive(true);
						Behaviour arg_AC_0 = this.rebellionRoute;
						bool enabled = true;
						this.rebellionBrightPoint.set_enabled(enabled);
						arg_AC_0.set_enabled(enabled);
						this.rebellionBrightPoint.get_transform().LTValue(0f, 1f, 1f).setEase(LeanTweenType.linear).setLoopPingPong().setOnUpdate(delegate(float x)
						{
							this.rebellionBrightPoint.alpha = x;
						});
					}
				}
			}
		}

		private void Awake()
		{
			this._isPassed = false;
		}

		private void OnDestroy()
		{
			Mem.Del(ref this._uiRoute);
			Mem.Del(ref this._uiRebellionRoute);
			Mem.Del(ref this._uiRebellionBrightPoint);
		}

		public void Passed(bool isPassed)
		{
			this.route.set_enabled(isPassed);
			if (this.isRebellionRoute)
			{
				Behaviour arg_2C_0 = this.rebellionRoute;
				bool enabled = false;
				this.rebellionBrightPoint.set_enabled(enabled);
				arg_2C_0.set_enabled(enabled);
			}
		}
	}
}
