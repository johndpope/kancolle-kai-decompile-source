using Common.Enum;
using System;
using UnityEngine;

namespace KCV.SortieMap
{
	[RequireComponent(typeof(UISprite))]
	public class ProdCommentBalloon : AbsBalloon
	{
		public static ProdCommentBalloon Instantiate(ProdCommentBalloon prefab, Transform parent, UISortieShip.Direction iDirection, MapCommentKind iKind)
		{
			ProdCommentBalloon prodCommentBalloon = Object.Instantiate<ProdCommentBalloon>(prefab);
			prodCommentBalloon.get_transform().set_parent(parent);
			prodCommentBalloon.get_transform().localScaleZero();
			prodCommentBalloon.get_transform().localPositionZero();
			prodCommentBalloon.Init(iDirection, iKind);
			return prodCommentBalloon;
		}

		public static ProdCommentBalloon Instantiate(ProdCommentBalloon prefab, Transform parent, UISortieShip.Direction iDirection)
		{
			ProdCommentBalloon prodCommentBalloon = Object.Instantiate<ProdCommentBalloon>(prefab);
			prodCommentBalloon.get_transform().set_parent(parent);
			prodCommentBalloon.get_transform().localScaleZero();
			prodCommentBalloon.get_transform().localPositionZero();
			prodCommentBalloon.Init(iDirection);
			return prodCommentBalloon;
		}

		private bool Init(UISortieShip.Direction iDirection, MapCommentKind iKind)
		{
			this.SetBalloonComment(iKind);
			this.SetBalloonPos(iDirection);
			return true;
		}

		private bool Init(UISortieShip.Direction iDirection)
		{
			base.sprite.spriteName = "fuki_yojou";
			base.sprite.MakePixelPerfect();
			this.SetBalloonPos(iDirection);
			base.get_transform().localScaleZero();
			return true;
		}

		private void SetBalloonComment(MapCommentKind iKind)
		{
			string spriteName = string.Empty;
			if (iKind != MapCommentKind.Enemy)
			{
				if (iKind == MapCommentKind.Atack)
				{
					spriteName = "fuki_ship2";
				}
			}
			else
			{
				spriteName = "fuki_ship1";
			}
			base.sprite.spriteName = spriteName;
			base.sprite.MakePixelPerfect();
			base.get_transform().localScaleZero();
		}

		protected override void SetBalloonPos(UISortieShip.Direction iDirection)
		{
			if (iDirection != UISortieShip.Direction.Left)
			{
				if (iDirection == UISortieShip.Direction.Right)
				{
					base.get_transform().set_localPosition(new Vector3(71f, 17f, 0f));
				}
			}
			else
			{
				base.get_transform().set_localPosition(new Vector3(-71f, 17f, 0f));
			}
		}
	}
}
