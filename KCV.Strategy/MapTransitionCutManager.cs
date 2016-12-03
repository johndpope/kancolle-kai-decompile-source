using local.models;
using local.utils;
using LT.Tweening;
using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace KCV.Strategy
{
	public class MapTransitionCutManager : SingletonMonoBehaviour<MapTransitionCutManager>
	{
		private const int TRANSITION_CLOUD_MAX = 40;

		[SerializeField]
		private UITexture _uiSortieMapBackground;

		[SerializeField]
		private UITexture _uiBackground;

		[SerializeField]
		private UITexture _uiOverlay;

		[SerializeField]
		private UITexture _uiSortieLabel;

		[SerializeField]
		private ParticleSystem _psCloud;

		private AsyncOperation _asyncOperation;

		private GameObject cam;

		private Transform _prefabAreaMap;

		private bool _isWait;

		private Animation _anim;

		private static readonly int[] NeedPlaneCellAreaNo = new int[]
		{
			44,
			93,
			122,
			123,
			124,
			142,
			152,
			153,
			154,
			161,
			162,
			163,
			164,
			172
		};

		private Animation animation
		{
			get
			{
				return this.GetComponentThis(ref this._anim);
			}
		}

		protected override void Awake()
		{
			this._psCloud.SetActive(false);
		}

		private void OnDestroy()
		{
			Mem.Del<UITexture>(ref this._uiSortieMapBackground);
			Mem.Del<UITexture>(ref this._uiBackground);
			Mem.Del<UITexture>(ref this._uiOverlay);
			Mem.Del<UITexture>(ref this._uiSortieLabel);
			Mem.Del(ref this._psCloud);
			Mem.Del<AsyncOperation>(ref this._asyncOperation);
			Mem.Del<GameObject>(ref this.cam);
			Mem.Del<bool>(ref this._isWait);
			Mem.Del<Animation>(ref this._anim);
			Mem.Del<Transform>(ref this._prefabAreaMap);
		}

		private void Update()
		{
			if (this._isWait && this._asyncOperation.get_progress() >= 0.9f)
			{
				this._isWait = false;
				Object.DontDestroyOnLoad(this.cam);
				this._asyncOperation.set_allowSceneActivation(true);
			}
		}

		public void Discard(Action onFinished)
		{
			base.get_transform().LTValue(1f, 0f, 0.5f).setOnUpdate(delegate(float x)
			{
				this._uiBackground.alpha = x;
				this._uiSortieMapBackground.alpha = x;
			}).setOnComplete(delegate
			{
				Dlg.Call(ref onFinished);
				Object.Destroy(this.cam);
			});
		}

		public void Initialize(MapModel mapModel, AsyncOperation async)
		{
			this.cam = Object.Instantiate<GameObject>(base.get_transform().get_parent().Find("TopCamera").get_gameObject());
			this.cam.get_transform().set_localScale(new Vector3(0.001f, 0.001f, 0.001f));
			this.cam.GetComponent<Camera>().set_clearFlags(3);
			this.cam.GetComponent<Camera>().set_depth(99f);
			base.get_transform().set_parent(this.cam.get_transform());
			base.get_transform().set_localPosition(new Vector3(-26.4f, -43f, 496.4f));
			base.get_transform().set_localScale(Vector3.get_one());
			this.cam.get_transform().set_localPosition(1000f * Vector3.get_right());
			this._asyncOperation = async;
			Observable.FromCoroutine(() => this.PlayAnimation(mapModel, async), false).Subscribe<Unit>();
		}

		[DebuggerHidden]
		private IEnumerator PlayAnimation(MapModel mapModel, AsyncOperation async)
		{
			MapTransitionCutManager.<PlayAnimation>c__Iterator14A <PlayAnimation>c__Iterator14A = new MapTransitionCutManager.<PlayAnimation>c__Iterator14A();
			<PlayAnimation>c__Iterator14A.mapModel = mapModel;
			<PlayAnimation>c__Iterator14A.<$>mapModel = mapModel;
			<PlayAnimation>c__Iterator14A.<>f__this = this;
			return <PlayAnimation>c__Iterator14A;
		}

		public Transform GetPrefabAreaMap()
		{
			return this._prefabAreaMap;
		}

		private void CreatePlaneCellPrefab(MapModel mapModel)
		{
			bool flag = false;
			for (int i = 0; i < MapTransitionCutManager.NeedPlaneCellAreaNo.Length; i++)
			{
				if (MapTransitionCutManager.NeedPlaneCellAreaNo[i] == mapModel.MstId)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				Util.InstantiatePrefab("SortieMap/AreaMap/Map" + mapModel.MstId + "_PlaneCell", this._uiSortieMapBackground.get_gameObject(), false);
			}
		}

		private void CheckTrophy()
		{
			TrophyUtil.Unlock_At_MapStart();
		}
	}
}
