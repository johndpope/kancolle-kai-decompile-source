using System;
using UnityEngine;

public class BannerSmokes : MonoBehaviour
{
	private void Awake()
	{
		UISprite component = base.get_transform().GetChild(0).GetComponent<UISprite>();
		UISprite component2 = base.get_transform().GetChild(1).GetComponent<UISprite>();
		component.depth = 50;
		component2.depth = 50;
	}
}
