using System;

namespace KCV.Battle.Utils
{
	public struct BufferCorrection : IDisposable
	{
		private BufferCorrectionType _iType;

		private int _nCorrectionFactor;

		public BufferCorrectionType type
		{
			get
			{
				return this._iType;
			}
		}

		public int collectionFactor
		{
			get
			{
				return this._nCorrectionFactor;
			}
		}

		public BufferCorrection(BufferCorrectionType iType, int nCorrectionFactor)
		{
			this._iType = iType;
			this._nCorrectionFactor = nCorrectionFactor;
		}

		public void Dispose()
		{
			Mem.Del<BufferCorrectionType>(ref this._iType);
			Mem.Del<int>(ref this._nCorrectionFactor);
		}
	}
}
