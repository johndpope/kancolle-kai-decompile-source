using local.managers;
using System;
using UnityEngine;

namespace KCV.InteriorStore
{
	public class UIStoreUserInfo : MonoBehaviour
	{
		private static readonly float MOVE_TIME = 1f;

		private UILabel _uiWorkerVal;

		private UILabel _uiFCoinVal;

		private Vector3[] _vPos = new Vector3[]
		{
			new Vector3(0f, -272f, 0f),
			new Vector3(-1000f, -272f, 0f)
		};

		private void Awake()
		{
			base.get_transform().set_localPosition(this._vPos[1]);
			Util.FindParentToChild<UILabel>(ref this._uiWorkerVal, base.get_transform(), "Worker/Val");
			Util.FindParentToChild<UILabel>(ref this._uiFCoinVal, base.get_transform(), "FCoin/Val");
		}

		private void Start()
		{
		}

		private void Update()
		{
		}

		public void SetUserInfos(FurnitureStoreManager manager)
		{
			this._uiWorkerVal.textInt = manager.GetWorkerCount();
			this._uiFCoinVal.textInt = manager.UserInfo.FCoin;
		}

		public void ReqMode(ISTaskManagerMode iMode)
		{
			base.get_transform().set_localPosition((iMode != ISTaskManagerMode.Store) ? this._vPos[1] : this._vPos[0]);
		}
	}
}
