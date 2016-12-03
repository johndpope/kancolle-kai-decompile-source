using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Center Scroll View on Click")]
public class UICenterOnClick : MonoBehaviour
{
	private void OnClick()
	{
		UICenterOnChild uICenterOnChild = NGUITools.FindInParents<UICenterOnChild>(base.get_gameObject());
		UIPanel uIPanel = NGUITools.FindInParents<UIPanel>(base.get_gameObject());
		if (uICenterOnChild != null)
		{
			if (uICenterOnChild.get_enabled())
			{
				uICenterOnChild.CenterOn(base.get_transform());
			}
		}
		else if (uIPanel != null && uIPanel.clipping != UIDrawCall.Clipping.None)
		{
			UIScrollView component = uIPanel.GetComponent<UIScrollView>();
			Vector3 pos = -uIPanel.cachedTransform.InverseTransformPoint(base.get_transform().get_position());
			if (!component.canMoveHorizontally)
			{
				pos.x = uIPanel.cachedTransform.get_localPosition().x;
			}
			if (!component.canMoveVertically)
			{
				pos.y = uIPanel.cachedTransform.get_localPosition().y;
			}
			SpringPanel.Begin(uIPanel.cachedGameObject, pos, 6f);
		}
	}
}
