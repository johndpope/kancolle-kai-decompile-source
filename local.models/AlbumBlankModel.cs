using System;

namespace local.models
{
	public class AlbumBlankModel : IAlbumModel
	{
		private int _id;

		public int Id
		{
			get
			{
				return this._id;
			}
		}

		public AlbumBlankModel(int id)
		{
			this._id = id;
		}

		public override string ToString()
		{
			return this.ToString(false);
		}

		public string ToString(bool detail)
		{
			return string.Format("図鑑ID:{0} --- ", this.Id);
		}
	}
}
