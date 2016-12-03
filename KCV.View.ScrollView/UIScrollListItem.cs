using System;
using UnityEngine;

namespace KCV.View.ScrollView
{
	public interface UIScrollListItem<Model, View> where Model : class where View : MonoBehaviour, UIScrollListItem<Model, View>
	{
		void Initialize(int realIndex, Model model);

		void InitializeDefault(int realIndex);

		int GetRealIndex();

		Model GetModel();

		int GetHeight();

		void SetOnTouchListener(Action<View> onTouchListener);

		void Hover();

		Transform GetTransform();

		void RemoveHover();
	}
}
