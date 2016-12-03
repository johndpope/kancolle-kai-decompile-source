using Common.Enum;
using local.models;
using Server_Common.Formats;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Strategy
{
	public class TileAnimationManager : MonoBehaviour
	{
		private UITexture textBG;

		private UITexture text;

		private Camera cam;

		private GameObject tankInit;

		private GameObject[] tankers;

		private GameObject[] tiles;

		private UISprite[] tileSprites;

		private TileAnimationAttack taa;

		private bool waitingForTiles;

		private bool attacking;

		private bool on;

		private float timer;

		private int curTile;

		private bool tankerAnim;

		[SerializeField]
		private GameObject tileAnimPrefab;

		private GameObject tileAnimInstance;

		public bool isFinished;

		private void Start()
		{
			this.tileAnimInstance = Object.Instantiate<GameObject>(this.tileAnimPrefab);
			this.tileAnimInstance.set_name("TileAnimations");
			this.tileAnimInstance.get_transform().set_parent(StrategyTopTaskManager.Instance.UIModel.OverView);
			this.tileAnimInstance.get_transform().set_localScale(Vector3.get_one());
			this.tileAnimInstance.get_transform().set_localPosition(Vector3.get_zero());
			this.cam = StrategyTopTaskManager.Instance.UIModel.MapCamera.myCamera;
			this.waitingForTiles = true;
			this.attacking = false;
			this.on = false;
			this.isFinished = false;
			this.curTile = 0;
			this.tankerAnim = false;
			this.textBG = GameObject.Find("/StrategyTaskManager/OverView/TileAnimations/TextBG").GetComponent<UITexture>();
			this.textBG.get_transform().set_localScale(new Vector3(1f, 0f, 1f));
			this.textBG.alpha = 0f;
			this.text = GameObject.Find("/StrategyTaskManager/OverView/TileAnimations/Text").GetComponent<UITexture>();
			this.taa = GameObject.Find("/StrategyTaskManager/Map Root/TileAnimationAttack").GetComponent<TileAnimationAttack>();
			this.taa.SetActive(false);
			this.tileAnimInstance.SetActive(false);
			this.tiles = new GameObject[17];
			this.GetTiles();
		}

		public void Initialize(RadingResultData d, MapAreaModel m, bool isFirst)
		{
			if (d == null)
			{
				this.isFinished = true;
				return;
			}
			this.curTile = d.AreaId - 1;
			this.taa.SetActive(true);
			this.tileAnimInstance.SetActive(true);
			this.taa.Initialize(d, m);
			StrategyTopTaskManager.Instance.GetInfoMng().MoveScreenOut(delegate
			{
				StrategyTopTaskManager.Instance.UIModel.MapCamera.MoveToTargetTile(d.AreaId, false);
				this.StartAnimation(m.GetEscortDeck().GetFlagShip() != null, d.AttackKind, isFirst);
			}, true, true);
		}

		public void StartAnimation(bool friendly, RadingKind type, bool isFirst)
		{
			float num = 0f;
			if (isFirst)
			{
				this.textBG.alpha = 1f;
				iTween.ScaleTo(this.textBG.get_gameObject(), iTween.Hash(new object[]
				{
					"scale",
					Vector3.get_one(),
					"time",
					0.5f,
					"easeType",
					iTween.EaseType.easeOutQuad
				}));
				iTween.MoveTo(this.text.get_gameObject(), iTween.Hash(new object[]
				{
					"position",
					new Vector3(0f, 0f, 0f),
					"islocal",
					true,
					"time",
					1,
					"delay",
					0.4f,
					"easeType",
					iTween.EaseType.easeOutExpo
				}));
				iTween.ValueTo(base.get_gameObject(), iTween.Hash(new object[]
				{
					"from",
					0,
					"to",
					1,
					"time",
					0.5f,
					"delay",
					0.4f,
					"onupdate",
					"TextAlpha",
					"onupdatetarget",
					base.get_gameObject()
				}));
				iTween.MoveTo(this.text.get_gameObject(), iTween.Hash(new object[]
				{
					"position",
					new Vector3(680f, 0f, 0f),
					"islocal",
					true,
					"time",
					1,
					"delay",
					1.4f,
					"easeType",
					iTween.EaseType.easeInExpo
				}));
				iTween.ValueTo(base.get_gameObject(), iTween.Hash(new object[]
				{
					"from",
					1,
					"to",
					0,
					"time",
					0.5f,
					"delay",
					1.9f,
					"onupdate",
					"TextAlpha",
					"onupdatetarget",
					base.get_gameObject()
				}));
				iTween.ScaleTo(this.textBG.get_gameObject(), iTween.Hash(new object[]
				{
					"scale",
					new Vector3(1f, 0f, 1f),
					"time",
					0.5f,
					"delay",
					2,
					"easeType",
					iTween.EaseType.easeInQuad,
					"oncomplete",
					"TextBGAlphaOff",
					"oncompletetarget",
					base.get_gameObject()
				}));
				num = 2.4f;
			}
			base.StartCoroutine(StrategyTopTaskManager.Instance.UIModel.MapCamera.MoveToTargetTile(this.curTile + 1, 1f));
			iTween.ValueTo(base.get_gameObject(), iTween.Hash(new object[]
			{
				"from",
				1,
				"to",
				0.5f,
				"time",
				0.2f,
				"delay",
				num,
				"onupdate",
				"TileColor",
				"onupdatetarget",
				base.get_gameObject()
			}));
			this.DelayAction(num, delegate
			{
				this.StartCoroutine(this.Attack(3.4f, friendly, type));
			});
		}

		public void TextAlpha(float f)
		{
			this.text.alpha = f;
		}

		public void TextBGAlpha(float f)
		{
			this.textBG.alpha = f;
		}

		public void TextBGAlphaOff()
		{
			this.textBG.alpha = 0f;
		}

		public void TileColor(float f)
		{
			this.tileSprites[this.curTile].color = new Color(1f, f, f, 1f);
		}

		[DebuggerHidden]
		public IEnumerator Attack(float delay, bool friendly, RadingKind type)
		{
			TileAnimationManager.<Attack>c__Iterator19F <Attack>c__Iterator19F = new TileAnimationManager.<Attack>c__Iterator19F();
			<Attack>c__Iterator19F.friendly = friendly;
			<Attack>c__Iterator19F.type = type;
			<Attack>c__Iterator19F.<$>friendly = friendly;
			<Attack>c__Iterator19F.<$>type = type;
			<Attack>c__Iterator19F.<>f__this = this;
			return <Attack>c__Iterator19F;
		}

		private void GetTiles()
		{
			this.tileSprites = new UISprite[17];
			for (int i = 0; i < 17; i++)
			{
				this.tiles[i] = StrategyTopTaskManager.Instance.TileManager.Tiles[i + 1].get_gameObject();
				this.tileSprites[i] = StrategyTopTaskManager.Instance.TileManager.Tiles[i + 1].getSprite();
			}
		}
	}
}
