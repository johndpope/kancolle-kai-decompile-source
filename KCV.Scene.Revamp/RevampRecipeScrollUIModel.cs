using local.models;
using System;

namespace KCV.Scene.Revamp
{
	public class RevampRecipeScrollUIModel
	{
		public bool Clickable
		{
			get;
			private set;
		}

		public RevampRecipeModel Model
		{
			get;
			private set;
		}

		public RevampRecipeScrollUIModel(RevampRecipeModel model, bool clickable)
		{
			this.Model = model;
			this.Clickable = clickable;
		}
	}
}
