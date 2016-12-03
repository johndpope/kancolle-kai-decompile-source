using System;
using UnityEngine;

namespace KCV.Battle.Utils
{
	[Serializable]
	public struct AircraftOffsetInfo
	{
		[SerializeField]
		private int _nMstId;

		[SerializeField]
		private bool _isFlipHorizontal;

		[SerializeField]
		private float _fRot;

		[SerializeField]
		private Vector3 _vPos;

		public int mstID
		{
			get
			{
				return this._nMstId;
			}
		}

		public bool isFlipHorizontal
		{
			get
			{
				return this._isFlipHorizontal;
			}
		}

		public float rot
		{
			get
			{
				return this._fRot;
			}
		}

		public Vector3 pos
		{
			get
			{
				return this._vPos;
			}
		}

		public AircraftOffsetInfo(int mstID, bool flipHorizontal, float rot, Vector3 pos)
		{
			this._nMstId = mstID;
			this._isFlipHorizontal = flipHorizontal;
			this._fRot = rot;
			this._vPos = pos;
		}

		public override string ToString()
		{
			string empty = string.Empty;
			return empty + string.Format("MstID:{0}|反転:{1}|回転:{2}|位置:{3}", new object[]
			{
				this.mstID,
				this.isFlipHorizontal,
				this.rot,
				this.pos
			});
		}
	}
}
