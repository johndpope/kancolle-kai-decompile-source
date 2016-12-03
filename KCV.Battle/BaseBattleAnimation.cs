using System;
using System.Collections;
using UnityEngine;

namespace KCV.Battle
{
	public class BaseBattleAnimation : BaseAnimation
	{
		protected virtual void setGlowEffects()
		{
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			cutInEffectCamera.glowEffect.set_enabled(true);
			cutInEffectCamera.glowEffect.glowTint = Color.get_black();
		}

		protected void playGlowEffect()
		{
			this.playGlowEffect(0.3f);
		}

		protected void playGlowEffect(float time)
		{
			iTween.ValueTo(base.get_gameObject(), this.getGlowTIntHash(time));
		}

		protected virtual Hashtable getGlowTIntHash(float time)
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add("from", 1f);
			hashtable.Add("to", 0f);
			hashtable.Add("time", time);
			hashtable.Add("easetype", iTween.EaseType.linear);
			hashtable.Add("onupdatetarget", base.get_gameObject());
			hashtable.Add("onupdate", "onUpadteGlowInt");
			return hashtable;
		}

		protected virtual void onUpadteGlowInt(float val)
		{
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			cutInEffectCamera.glowEffect.glowTint.r = val;
			cutInEffectCamera.glowEffect.glowTint.g = val;
			cutInEffectCamera.glowEffect.glowTint.b = val;
		}
	}
}
