using Common.Enum;
using KCV.Utils;
using System;
using UnityEngine;

namespace KCV.Strategy
{
	public class TileAnimationAttackShell : MonoBehaviour
	{
		private UITexture tex;

		public void Awake()
		{
			this.tex = base.GetComponent<UITexture>();
			if (this.tex == null)
			{
				Debug.Log("Warning: UITexture not attached");
			}
			this.tex.alpha = 0f;
		}

		public void Initialize(Vector3 origin, Vector3 target, RadingKind type)
		{
			base.get_transform().set_localPosition(origin);
			base.get_transform().set_localScale(Vector3.get_one());
			base.get_transform().set_eulerAngles(Vector3.get_zero());
			this.tex.alpha = 0f;
			if (type == RadingKind.AIR_ATTACK)
			{
				if (StrategyTopTaskManager.GetLogicManager().Turn >= 500)
				{
					this.tex.mainTexture = (Resources.Load("Textures/TileAnimations/item_up_e54") as Texture);
					this.tex.width = 60;
					this.tex.height = 60;
				}
				else
				{
					this.tex.mainTexture = (Resources.Load("Textures/TileAnimations/item_up_506_2") as Texture);
					this.tex.width = 50;
					this.tex.height = 100;
				}
				this.tex.alpha = 1f;
				base.get_transform().set_localScale(0.001f * Vector3.get_one());
				iTween.ScaleTo(base.get_gameObject(), iTween.Hash(new object[]
				{
					"scale",
					Vector3.get_one(),
					"islocal",
					true,
					"time",
					0.2f,
					"delay",
					0.5f,
					"easeType",
					iTween.EaseType.easeInOutQuad
				}));
				iTween.ValueTo(base.get_gameObject(), iTween.Hash(new object[]
				{
					"from",
					0,
					"to",
					1,
					"time",
					0.05f,
					"delay",
					0.5f,
					"onupdate",
					"Alpha",
					"onupdatetarget",
					base.get_gameObject()
				}));
				iTween.MoveTo(base.get_gameObject(), iTween.Hash(new object[]
				{
					"path",
					new Vector3[]
					{
						origin,
						origin + 0.5f * (target - origin) + 50f * Vector3.get_down(),
						1.2f * target - 0.2f * origin
					},
					"islocal",
					true,
					"time",
					1.2f,
					"easeType",
					iTween.EaseType.linear
				}));
				iTween.ValueTo(base.get_gameObject(), iTween.Hash(new object[]
				{
					"from",
					1,
					"to",
					0,
					"time",
					0.05f,
					"delay",
					1.15f,
					"onupdate",
					"Alpha",
					"onupdatetarget",
					base.get_gameObject()
				}));
				this.DelayAction(0.2f, delegate
				{
					SoundUtils.PlaySE(SEFIleInfos.BattleTookOffAircraft);
				});
			}
			else if (type == RadingKind.SUBMARINE_ATTACK)
			{
				this.tex.mainTexture = (Resources.Load("Textures/TileAnimations/kouseki") as Texture);
				this.tex.MakePixelPerfect();
				Vector3 vector = target - origin;
				base.get_transform().Rotate(Vector3.get_forward(), 57.2957764f * Mathf.Atan2(vector.y, vector.x));
				base.get_transform().set_localScale(new Vector3(0.001f, 0.75f, 0.75f));
				iTween.ValueTo(base.get_gameObject(), iTween.Hash(new object[]
				{
					"from",
					0,
					"to",
					1,
					"time",
					0.05f,
					"onupdate",
					"Alpha",
					"onupdatetarget",
					base.get_gameObject()
				}));
				iTween.ScaleTo(base.get_gameObject(), iTween.Hash(new object[]
				{
					"scale",
					0.75f * Vector3.get_one(),
					"islocal",
					true,
					"time",
					0.4f,
					"easeType",
					iTween.EaseType.linear
				}));
				iTween.MoveTo(base.get_gameObject(), iTween.Hash(new object[]
				{
					"position",
					0.8f * target + 0.2f * origin,
					"islocal",
					true,
					"time",
					1,
					"easeType",
					iTween.EaseType.linear
				}));
				iTween.ValueTo(base.get_gameObject(), iTween.Hash(new object[]
				{
					"from",
					1,
					"to",
					0,
					"time",
					0.05f,
					"delay",
					0.95f,
					"onupdate",
					"Alpha",
					"onupdatetarget",
					base.get_gameObject()
				}));
				SoundUtils.PlaySE(SEFIleInfos.BattleTorpedo);
			}
			else
			{
				this.tex.mainTexture = (Resources.Load("Textures/TileAnimations/fire_5") as Texture);
				this.tex.MakePixelPerfect();
				Vector3 vector2 = target - origin;
				base.get_transform().Rotate(Vector3.get_forward(), 57.2957764f * Mathf.Atan2(vector2.y, vector2.x));
				iTween.ValueTo(base.get_gameObject(), iTween.Hash(new object[]
				{
					"from",
					0,
					"to",
					1,
					"time",
					0.05f,
					"onupdate",
					"Alpha",
					"onupdatetarget",
					base.get_gameObject()
				}));
				iTween.MoveTo(base.get_gameObject(), iTween.Hash(new object[]
				{
					"position",
					target,
					"islocal",
					true,
					"time",
					0.25f,
					"easeType",
					iTween.EaseType.linear
				}));
				iTween.ValueTo(base.get_gameObject(), iTween.Hash(new object[]
				{
					"from",
					1,
					"to",
					0,
					"time",
					0.05f,
					"delay",
					0.2f,
					"onupdate",
					"Alpha",
					"onupdatetarget",
					base.get_gameObject()
				}));
				SoundUtils.PlaySE(SEFIleInfos.SE_901);
			}
		}

		public void Alpha(float f)
		{
			this.tex.alpha = f;
		}

		private void OnDestroy()
		{
			this.tex = null;
		}
	}
}
