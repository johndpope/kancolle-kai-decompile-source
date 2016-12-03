using System;

public static class LayersEnumExtensions
{
	public static int IntLayer(this Generics.Layers layer)
	{
		switch (layer)
		{
		case Generics.Layers.Default:
			return 0;
		case Generics.Layers.TransparentFX:
			return 1;
		case Generics.Layers.Default | Generics.Layers.TransparentFX:
			IL_1A:
			if (layer == Generics.Layers.Water)
			{
				return 3;
			}
			if (layer == Generics.Layers.UI)
			{
				return 5;
			}
			if (layer == Generics.Layers.Background)
			{
				return 8;
			}
			if (layer == Generics.Layers.UI2D)
			{
				return 9;
			}
			if (layer == Generics.Layers.UI3D)
			{
				return 10;
			}
			if (layer == Generics.Layers.Transition)
			{
				return 11;
			}
			if (layer == Generics.Layers.ShipGirl)
			{
				return 12;
			}
			if (layer == Generics.Layers.TopMost)
			{
				return 13;
			}
			if (layer == Generics.Layers.CutIn)
			{
				return 14;
			}
			if (layer == Generics.Layers.SaveData)
			{
				return 15;
			}
			if (layer == Generics.Layers.Effects)
			{
				return 16;
			}
			if (layer == Generics.Layers.FocusDim)
			{
				return 17;
			}
			if (layer == Generics.Layers.UnRefrectEffects)
			{
				return 18;
			}
			if (layer != Generics.Layers.SplitWater)
			{
				return 0;
			}
			return 19;
		case Generics.Layers.IgnoreRaycast:
			return 2;
		}
		goto IL_1A;
	}
}
