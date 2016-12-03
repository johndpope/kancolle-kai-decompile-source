using System;

namespace local.models
{
	public interface IAlbumModel
	{
		int Id
		{
			get;
		}

		string ToString(bool detail);
	}
}
