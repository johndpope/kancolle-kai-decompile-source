using DG.Tweening;
using KCV.Scene.Port;
using local.models;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Furniture
{
	public class UIDynamicWindowFurniture : UIDynamicFurniture
	{
		[SerializeField]
		protected UITexture mTexture_WindowMain;

		[SerializeField]
		protected UITexture mTexture_WindowBackground;

		private int mOutPlaceTimeType = -1;

		private int[] WINDOW_OUTPLACE_GRAPYC_TYPE_MASTER = new int[]
		{
			1,
			1,
			1,
			4,
			4,
			1,
			4,
			4,
			4,
			2,
			4,
			3,
			1,
			1,
			4,
			1,
			1,
			3,
			3,
			1,
			1,
			4,
			1,
			1,
			3,
			1,
			4,
			3,
			3,
			4,
			4,
			4,
			4,
			4,
			4,
			4,
			4,
			4,
			4
		};

		protected override void OnUpdate()
		{
			this.UpdateWindow();
		}

		protected void UpdateWindow()
		{
			this.OnUpdateWindow();
		}

		protected virtual void OnUpdateWindow()
		{
			if (this.mFurnitureModel != null)
			{
				int outPlaceTimeType = this.GetOutPlaceTimeType(this.mFurnitureModel.GetDateTime().get_Hour());
				if (this.mOutPlaceTimeType != outPlaceTimeType)
				{
					this.mOutPlaceTimeType = outPlaceTimeType;
					base.StartCoroutine(this.UpdateWindowCoroutine());
				}
			}
		}

		[DebuggerHidden]
		private IEnumerator UpdateWindowCoroutine()
		{
			UIDynamicWindowFurniture.<UpdateWindowCoroutine>c__Iterator61 <UpdateWindowCoroutine>c__Iterator = new UIDynamicWindowFurniture.<UpdateWindowCoroutine>c__Iterator61();
			<UpdateWindowCoroutine>c__Iterator.<>f__this = this;
			return <UpdateWindowCoroutine>c__Iterator;
		}

		protected override void OnInitialize(UIFurniture.UIFurnitureModel uiFurnitureModel)
		{
			FurnitureModel furnitureModel = uiFurnitureModel.GetFurnitureModel();
			this.ChangeOffset(furnitureModel);
			DateTime dateTime = uiFurnitureModel.GetDateTime();
			int outPlaceGraphicType = this.GetOutPlaceGraphicType(furnitureModel);
			this.mOutPlaceTimeType = this.GetOutPlaceTimeType(dateTime.get_Hour());
			Texture mainTexture = UIFurniture.LoadTexture(furnitureModel);
			Texture mainTexture2 = this.RequestOutPlaceTexture(outPlaceGraphicType, this.mOutPlaceTimeType);
			this.mTexture_WindowMain.mainTexture = mainTexture;
			this.mTexture_WindowBackground.mainTexture = mainTexture2;
		}

		private void ChangeOffset(FurnitureModel furnitureModel)
		{
			int num = furnitureModel.NoInType + 1;
			switch (num)
			{
			case 1:
			case 2:
			case 3:
			case 4:
			case 6:
				base.get_transform().set_localPosition(new Vector3(25f, 12f, 0f));
				return;
			case 5:
			case 7:
			case 8:
			case 9:
			case 11:
			case 14:
			case 15:
			case 17:
				IL_59:
				switch (num)
				{
				case 25:
					base.get_transform().set_localPosition(new Vector3(0f, 14f, 0f));
					return;
				case 26:
				case 27:
				case 29:
					IL_7A:
					switch (num)
					{
					case 33:
						base.get_transform().set_localPosition(new Vector3(18f, 15f, 0f));
						return;
					case 34:
						base.get_transform().set_localPosition(new Vector3(28f, 22f, 0f));
						return;
					case 37:
						base.get_transform().set_localPosition(new Vector3(12f, 8f, 0f));
						return;
					}
					base.get_transform().set_localPosition(new Vector3(0f, 0f, 0f));
					return;
				case 28:
					base.get_transform().set_localPosition(new Vector3(10f, 15f, 0f));
					return;
				case 30:
					base.get_transform().set_localPosition(new Vector3(0f, 0f, 0f));
					this.mTexture_WindowBackground.get_transform().set_localPosition(new Vector3(0f, 35f, 0f));
					return;
				}
				goto IL_7A;
			case 10:
				base.get_transform().set_localPosition(new Vector3(0f, 18f, 0f));
				return;
			case 12:
				base.get_transform().set_localPosition(new Vector3(0f, 20f, 0f));
				return;
			case 13:
				base.get_transform().set_localPosition(new Vector3(24f, 30f, 0f));
				return;
			case 16:
				base.get_transform().set_localPosition(new Vector3(38f, 35.5f, 0f));
				return;
			case 18:
				base.get_transform().set_localPosition(new Vector3(25f, 14f, 0f));
				return;
			}
			goto IL_59;
		}

		protected int GetOutPlaceGraphicType(FurnitureModel windowFurnitureModel)
		{
			return this.WINDOW_OUTPLACE_GRAPYC_TYPE_MASTER[windowFurnitureModel.NoInType];
		}

		protected Texture RequestOutPlaceTexture(int graphicType, int timeType)
		{
			string text = string.Concat(new object[]
			{
				"window_bg_",
				graphicType,
				"-",
				timeType
			});
			string text2 = string.Concat(new object[]
			{
				"Textures/Furnitures/Capiz/",
				graphicType,
				"/",
				text
			});
			return Resources.Load(text2) as Texture;
		}

		protected int GetOutPlaceTimeType(int hour24)
		{
			bool flag = 4 <= hour24 && hour24 < 8;
			if (flag)
			{
				return 5;
			}
			bool flag2 = 8 <= hour24 && hour24 < 16;
			if (flag2)
			{
				return 1;
			}
			bool flag3 = 16 <= hour24 && hour24 < 18;
			if (flag3)
			{
				return 2;
			}
			bool flag4 = 18 <= hour24 && hour24 < 20;
			if (flag4)
			{
				return 3;
			}
			bool flag5 = 20 <= hour24 || hour24 < 4;
			if (flag5)
			{
				return 4;
			}
			return -1;
		}

		protected override void OnDestroyEvent()
		{
			base.OnDestroyEvent();
			if (DOTween.IsTweening(this.mTexture_WindowBackground))
			{
				DOTween.Kill(this.mTexture_WindowBackground, false);
			}
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_WindowMain, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_WindowBackground, false);
		}
	}
}
