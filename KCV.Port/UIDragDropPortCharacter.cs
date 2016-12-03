using System;
using UnityEngine;

namespace KCV.Port
{
	public class UIDragDropPortCharacter : UIDragDropItem
	{
		protected override void OnDragDropMove(Vector2 delta)
		{
			delta.y = 0f;
			base.OnDragDropMove(delta);
			base.get_transform().localPositionX(Util.RangeValue(base.get_transform().get_localPosition().x, -350f, 400f));
		}
	}
}
