using KCV.Utils;
using local.managers;
using local.models.battle;
using System;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdMapOpen : BaseAnimation
	{
		[SerializeField]
		private UITexture _uiMapImage;

		[SerializeField]
		private UITexture _uiMapOld;

		[SerializeField]
		private UISprite _uiMapIcon;

		[SerializeField]
		private UITexture _uiText;

		[SerializeField]
		private UISprite _uiNextIcon;

		[SerializeField]
		private ParticleSystem _uiLightPar;

		[SerializeField]
		private ParticleSystem _uiBackLightPar;

		[SerializeField]
		private Animation _anime;

		[SerializeField]
		private Animation _gearAnime;

		private int _nMapId;

		private int _nAreaId;

		private int[] _openMapIDs;

		private bool _isControl;

		private bool[] _isOpenMap;

		private KeyControl _keyControl;

		private BattleResultModel _resultModel;

		private void _init()
		{
			this._isControl = false;
			this._isFinished = false;
			this._nAreaId = 1;
			this._nMapId = 1;
			this._openMapIDs = null;
			Util.FindParentToChild<UITexture>(ref this._uiMapImage, base.get_transform(), "MapImage");
			Util.FindParentToChild<UITexture>(ref this._uiMapOld, base.get_transform(), "MapImageOld");
			Util.FindParentToChild<UISprite>(ref this._uiMapIcon, base.get_transform(), "MapIcon");
			Util.FindParentToChild<UITexture>(ref this._uiText, base.get_transform(), "Text");
			Util.FindParentToChild<UISprite>(ref this._uiNextIcon, base.get_transform(), "NextIcon");
			Util.FindParentToChild<ParticleSystem>(ref this._uiLightPar, base.get_transform(), "Light1");
			Util.FindParentToChild<ParticleSystem>(ref this._uiBackLightPar, base.get_transform(), "BackLight");
			if (this._anime == null)
			{
				this._anime = base.GetComponent<Animation>();
			}
			if (this._gearAnime == null)
			{
				this._gearAnime = this._uiNextIcon.GetComponent<Animation>();
			}
			this._uiBackLightPar.SetActive(false);
			this._uiNextIcon.alpha = 0f;
			this._anime.Stop();
			this._gearAnime.Stop();
			UIButtonMessage component = this._uiNextIcon.GetComponent<UIButtonMessage>();
			component.target = base.get_gameObject();
			component.functionName = "_nextIconEL";
			component.trigger = UIButtonMessage.Trigger.OnClick;
		}

		private void OnDestroy()
		{
			Mem.Del<UITexture>(ref this._uiMapImage);
			Mem.Del<UITexture>(ref this._uiMapOld);
			Mem.Del(ref this._uiMapIcon);
			Mem.Del<UITexture>(ref this._uiText);
			Mem.Del(ref this._uiLightPar);
			Mem.Del(ref this._uiBackLightPar);
			Mem.Del<Animation>(ref this._anime);
			Mem.Del<Animation>(ref this._gearAnime);
			Mem.Del<int[]>(ref this._openMapIDs);
			this._keyControl = null;
			this._resultModel = null;
		}

		public bool Run()
		{
			if (this._isControl && this._keyControl.keyState.get_Item(1).down)
			{
				this._nextIconEL(null);
			}
			return this._isFinished;
		}

		private void SetMapTexture()
		{
			string text = string.Concat(new object[]
			{
				"Textures/Strategy/MapSelectGraph/stage",
				this._nAreaId,
				"-",
				this._nMapId
			});
			this._uiMapImage.mainTexture = (Resources.Load(text) as Texture2D);
		}

		private void SetNextMapTexture()
		{
			this._uiMapOld.mainTexture = this._uiMapImage.mainTexture;
			string text = string.Concat(new object[]
			{
				"Textures/Strategy/MapSelectGraph/stage",
				this._nAreaId,
				"-",
				this._nMapId
			});
			this._uiMapImage.mainTexture = (Resources.Load(text) as Texture2D);
		}

		private void _setOpen(bool isOpen)
		{
			if (isOpen)
			{
				this._uiMapIcon.get_transform().get_gameObject().SetActive(false);
			}
			else
			{
				this._uiMapIcon.get_transform().get_gameObject().SetActive(true);
			}
		}

		private void SetNextOpenMap()
		{
			bool flag = false;
			for (int i = 0; i < this._openMapIDs.Length; i++)
			{
				if (this._isOpenMap[i])
				{
					this._isOpenMap[i] = false;
					flag = true;
					this.GetOpenIds(this._openMapIDs[i]);
					this._setOpen(false);
					break;
				}
			}
			this._animAnimation.Stop();
			if (!flag)
			{
				this._animAnimation.Play("MapOpenEnd");
			}
			else
			{
				this.SetNextMapTexture();
				this._animAnimation.Play("MapOpenNext");
			}
		}

		private void GetOpenIds(int id)
		{
			int num = 0;
			if (id >= 100)
			{
				int num2 = 0;
				for (int i = 0; i < 10; i++)
				{
					if (id >= 100 + 100 * i)
					{
						num2++;
					}
				}
				int num3 = num2 * 100 + 11;
				for (int j = 0; j < 10; j++)
				{
					if (id >= num3 + 10 * j)
					{
						num++;
					}
				}
				this._nMapId = id - num2 * 100 - num * 10;
			}
			else
			{
				for (int k = 0; k < 10; k++)
				{
					if (id >= 11 + 10 * k)
					{
						num++;
					}
				}
				this._nMapId = id - num * 10;
			}
			this._nAreaId = (id - this._nMapId) / 10;
		}

		public override void Play(Action callback)
		{
			this.SetActive(true);
			this._actCallback = callback;
			this._animAnimation.Stop();
			this._animAnimation.Play("MapOpen");
		}

		private void _nextIconEL(GameObject obj)
		{
			if (this._isControl)
			{
				this.SetNextOpenMap();
				this._uiLightPar.Stop();
				this._uiLightPar.SetActive(false);
				this._uiBackLightPar.Stop();
				this._uiBackLightPar.SetActive(false);
				this._uiNextIcon.alpha = 0f;
				this._gearAnime.Stop();
				this._isControl = false;
			}
		}

		private void _startParticle()
		{
			this._uiLightPar.Play();
		}

		private void _startControl()
		{
			this._isControl = true;
		}

		private void _onStartAnimationEnd()
		{
			this._isControl = true;
			SoundUtils.PlaySE(SEFIleInfos.SE_925);
			this._animAnimation.Stop();
			this._uiLightPar.SetActive(true);
			this._uiLightPar.Play();
			this._uiBackLightPar.SetActive(true);
			this._uiBackLightPar.Play();
			this._uiNextIcon.alpha = 1f;
			this._gearAnime.Play("NextIcon");
		}

		private void _onCompEndAnimation()
		{
			this._onFinishedMapOpen();
		}

		private void _onFinishedMapOpen()
		{
			if (this._actCallback != null)
			{
				this._actCallback.Invoke();
			}
			this._isFinished = true;
		}

		public static ProdMapOpen Instantiate(ProdMapOpen prefab, BattleResultModel resultModel, Transform parent, KeyControl keyControl, MapManager mapManager, int nPanelDepth)
		{
			ProdMapOpen prodMapOpen = Object.Instantiate<ProdMapOpen>(prefab);
			prodMapOpen.get_transform().set_parent(parent);
			prodMapOpen.get_transform().set_localScale(Vector3.get_one());
			prodMapOpen.get_transform().set_localPosition(Vector3.get_zero());
			prodMapOpen._init();
			prodMapOpen._keyControl = keyControl;
			prodMapOpen._resultModel = resultModel;
			prodMapOpen._openMapIDs = prodMapOpen._resultModel.NewOpenMapIDs;
			prodMapOpen._isOpenMap = new bool[prodMapOpen._resultModel.NewOpenMapIDs.Length];
			for (int i = 0; i < prodMapOpen._resultModel.NewOpenMapIDs.Length; i++)
			{
				prodMapOpen._isOpenMap[i] = true;
			}
			for (int j = 0; j < prodMapOpen._resultModel.NewOpenMapIDs.Length; j++)
			{
				if (prodMapOpen._isOpenMap[j])
				{
					prodMapOpen._isOpenMap[j] = false;
					prodMapOpen.GetOpenIds(prodMapOpen._resultModel.NewOpenMapIDs[j]);
					break;
				}
			}
			prodMapOpen.SetMapTexture();
			return prodMapOpen;
		}

		public static ProdMapOpen Instantiate(ProdMapOpen prefab, int[] NewOpenAreaIDs, int[] NewOpenMapIDs, Transform parent, KeyControl keyControl, int nPanelDepth)
		{
			ProdMapOpen prodMapOpen = Object.Instantiate<ProdMapOpen>(prefab);
			prodMapOpen.get_transform().set_parent(parent);
			prodMapOpen.get_transform().set_localScale(Vector3.get_one());
			prodMapOpen.get_transform().set_localPosition(Vector3.get_zero());
			prodMapOpen._init();
			prodMapOpen._keyControl = keyControl;
			prodMapOpen._openMapIDs = NewOpenMapIDs;
			prodMapOpen._isOpenMap = new bool[NewOpenMapIDs.Length];
			for (int i = 0; i < prodMapOpen._openMapIDs.Length; i++)
			{
				prodMapOpen._isOpenMap[i] = true;
			}
			for (int j = 0; j < prodMapOpen._openMapIDs.Length; j++)
			{
				if (prodMapOpen._isOpenMap[j])
				{
					prodMapOpen._isOpenMap[j] = false;
					prodMapOpen.GetOpenIds(prodMapOpen._openMapIDs[j]);
					break;
				}
			}
			prodMapOpen.SetMapTexture();
			prodMapOpen.SetActive(false);
			return prodMapOpen;
		}
	}
}
