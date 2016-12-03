using System;
using UnityEngine;

public class UIRipples : MonoBehaviour
{
	[SerializeField]
	private UISprite[] mSpriteRipples;

	public void PlayRipple()
	{
		this.mSpriteRipples[0].GetComponent<Animation>().get_Item("Anim_LoadingRipple").set_time(0f);
		this.mSpriteRipples[1].GetComponent<Animation>().get_Item("Anim_LoadingRipple").set_time(0.75f);
		this.mSpriteRipples[0].GetComponent<Animation>().Play("Anim_LoadingRipple");
		this.mSpriteRipples[1].GetComponent<Animation>().Play("Anim_LoadingRipple");
	}
}
