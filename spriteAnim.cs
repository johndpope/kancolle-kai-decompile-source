using System;
using UnityEngine;

public class spriteAnim : MonoBehaviour
{
	private UISprite sprite;

	private int count;

	private void Start()
	{
		this.sprite = base.GetComponent<UISprite>();
	}

	private void Update()
	{
		this.count++;
		this.sprite.spriteName = this.sprite.atlas.spriteList.get_Item(this.count % 220 / 2).name;
	}
}
