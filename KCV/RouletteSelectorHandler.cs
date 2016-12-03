using System;
using UnityEngine;

namespace KCV
{
	public interface RouletteSelectorHandler
	{
		bool IsSelectable(int index);

		void OnSelect(int index, Transform transform);

		void OnUpdateIndex(int index, Transform transform);
	}
}
