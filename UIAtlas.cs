using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Atlas")]
public class UIAtlas : MonoBehaviour
{
	[Serializable]
	private class Sprite
	{
		public string name = "Unity Bug";

		public Rect outer = new Rect(0f, 0f, 1f, 1f);

		public Rect inner = new Rect(0f, 0f, 1f, 1f);

		public bool rotated;

		public float paddingLeft;

		public float paddingRight;

		public float paddingTop;

		public float paddingBottom;

		public bool hasPadding
		{
			get
			{
				return this.paddingLeft != 0f || this.paddingRight != 0f || this.paddingTop != 0f || this.paddingBottom != 0f;
			}
		}
	}

	private enum Coordinates
	{
		Pixels,
		TexCoords
	}

	[HideInInspector, SerializeField]
	private Material material;

	[HideInInspector, SerializeField]
	private List<UISpriteData> mSprites = new List<UISpriteData>();

	[HideInInspector, SerializeField]
	private float mPixelSize = 1f;

	[HideInInspector, SerializeField]
	private UIAtlas mReplacement;

	[HideInInspector, SerializeField]
	private UIAtlas.Coordinates mCoordinates;

	[HideInInspector, SerializeField]
	private List<UIAtlas.Sprite> sprites = new List<UIAtlas.Sprite>();

	private int mPMA = -1;

	private Dictionary<string, int> mSpriteIndices = new Dictionary<string, int>();

	public Material spriteMaterial
	{
		get
		{
			return (!(this.mReplacement != null)) ? this.material : this.mReplacement.spriteMaterial;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.spriteMaterial = value;
			}
			else if (this.material == null)
			{
				this.mPMA = 0;
				this.material = value;
			}
			else
			{
				this.MarkAsChanged();
				this.mPMA = -1;
				this.material = value;
				this.MarkAsChanged();
			}
		}
	}

	public bool premultipliedAlpha
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.premultipliedAlpha;
			}
			if (this.mPMA == -1)
			{
				Material spriteMaterial = this.spriteMaterial;
				this.mPMA = ((!(spriteMaterial != null) || !(spriteMaterial.get_shader() != null) || !spriteMaterial.get_shader().get_name().Contains("Premultiplied")) ? 0 : 1);
			}
			return this.mPMA == 1;
		}
	}

	public List<UISpriteData> spriteList
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.spriteList;
			}
			if (this.mSprites.get_Count() == 0)
			{
				this.Upgrade();
			}
			return this.mSprites;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.spriteList = value;
			}
			else
			{
				this.mSprites = value;
			}
		}
	}

	public Texture texture
	{
		get
		{
			return (!(this.mReplacement != null)) ? ((!(this.material != null)) ? null : this.material.get_mainTexture()) : this.mReplacement.texture;
		}
	}

	public float pixelSize
	{
		get
		{
			return (!(this.mReplacement != null)) ? this.mPixelSize : this.mReplacement.pixelSize;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.pixelSize = value;
			}
			else
			{
				float num = Mathf.Clamp(value, 0.25f, 4f);
				if (this.mPixelSize != num)
				{
					this.mPixelSize = num;
					this.MarkAsChanged();
				}
			}
		}
	}

	public UIAtlas replacement
	{
		get
		{
			return this.mReplacement;
		}
		set
		{
			UIAtlas uIAtlas = value;
			if (uIAtlas == this)
			{
				uIAtlas = null;
			}
			if (this.mReplacement != uIAtlas)
			{
				if (uIAtlas != null && uIAtlas.replacement == this)
				{
					uIAtlas.replacement = null;
				}
				if (this.mReplacement != null)
				{
					this.MarkAsChanged();
				}
				this.mReplacement = uIAtlas;
				if (uIAtlas != null)
				{
					this.material = null;
				}
				this.MarkAsChanged();
			}
		}
	}

	public UISpriteData GetSprite(string name)
	{
		if (this.mReplacement != null)
		{
			return this.mReplacement.GetSprite(name);
		}
		if (!string.IsNullOrEmpty(name))
		{
			if (this.mSprites.get_Count() == 0)
			{
				this.Upgrade();
			}
			if (this.mSprites.get_Count() == 0)
			{
				return null;
			}
			if (this.mSpriteIndices.get_Count() != this.mSprites.get_Count())
			{
				this.MarkSpriteListAsChanged();
			}
			int num;
			if (this.mSpriteIndices.TryGetValue(name, ref num))
			{
				if (num > -1 && num < this.mSprites.get_Count())
				{
					return this.mSprites.get_Item(num);
				}
				this.MarkSpriteListAsChanged();
				return (!this.mSpriteIndices.TryGetValue(name, ref num)) ? null : this.mSprites.get_Item(num);
			}
			else
			{
				int i = 0;
				int count = this.mSprites.get_Count();
				while (i < count)
				{
					UISpriteData uISpriteData = this.mSprites.get_Item(i);
					if (!string.IsNullOrEmpty(uISpriteData.name) && name == uISpriteData.name)
					{
						this.MarkSpriteListAsChanged();
						return uISpriteData;
					}
					i++;
				}
			}
		}
		return null;
	}

	public string GetRandomSprite(string startsWith)
	{
		if (this.GetSprite(startsWith) == null)
		{
			List<UISpriteData> spriteList = this.spriteList;
			List<string> list = new List<string>();
			using (List<UISpriteData>.Enumerator enumerator = spriteList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					UISpriteData current = enumerator.get_Current();
					if (current.name.StartsWith(startsWith))
					{
						list.Add(current.name);
					}
				}
			}
			return (list.get_Count() <= 0) ? null : list.get_Item(Random.Range(0, list.get_Count()));
		}
		return startsWith;
	}

	public void MarkSpriteListAsChanged()
	{
		this.mSpriteIndices.Clear();
		int i = 0;
		int count = this.mSprites.get_Count();
		while (i < count)
		{
			this.mSpriteIndices.set_Item(this.mSprites.get_Item(i).name, i);
			i++;
		}
	}

	public void SortAlphabetically()
	{
		this.mSprites.Sort((UISpriteData s1, UISpriteData s2) => s1.name.CompareTo(s2.name));
	}

	public BetterList<string> GetListOfSprites()
	{
		if (this.mReplacement != null)
		{
			return this.mReplacement.GetListOfSprites();
		}
		if (this.mSprites.get_Count() == 0)
		{
			this.Upgrade();
		}
		BetterList<string> betterList = new BetterList<string>();
		int i = 0;
		int count = this.mSprites.get_Count();
		while (i < count)
		{
			UISpriteData uISpriteData = this.mSprites.get_Item(i);
			if (uISpriteData != null && !string.IsNullOrEmpty(uISpriteData.name))
			{
				betterList.Add(uISpriteData.name);
			}
			i++;
		}
		return betterList;
	}

	public BetterList<string> GetListOfSprites(string match)
	{
		if (this.mReplacement)
		{
			return this.mReplacement.GetListOfSprites(match);
		}
		if (string.IsNullOrEmpty(match))
		{
			return this.GetListOfSprites();
		}
		if (this.mSprites.get_Count() == 0)
		{
			this.Upgrade();
		}
		BetterList<string> betterList = new BetterList<string>();
		int i = 0;
		int count = this.mSprites.get_Count();
		while (i < count)
		{
			UISpriteData uISpriteData = this.mSprites.get_Item(i);
			if (uISpriteData != null && !string.IsNullOrEmpty(uISpriteData.name) && string.Equals(match, uISpriteData.name, 5))
			{
				betterList.Add(uISpriteData.name);
				return betterList;
			}
			i++;
		}
		string[] array = match.Split(new char[]
		{
			' '
		}, 1);
		for (int j = 0; j < array.Length; j++)
		{
			array[j] = array[j].ToLower();
		}
		int k = 0;
		int count2 = this.mSprites.get_Count();
		while (k < count2)
		{
			UISpriteData uISpriteData2 = this.mSprites.get_Item(k);
			if (uISpriteData2 != null && !string.IsNullOrEmpty(uISpriteData2.name))
			{
				string text = uISpriteData2.name.ToLower();
				int num = 0;
				for (int l = 0; l < array.Length; l++)
				{
					if (text.Contains(array[l]))
					{
						num++;
					}
				}
				if (num == array.Length)
				{
					betterList.Add(uISpriteData2.name);
				}
			}
			k++;
		}
		return betterList;
	}

	private bool References(UIAtlas atlas)
	{
		return !(atlas == null) && (atlas == this || (this.mReplacement != null && this.mReplacement.References(atlas)));
	}

	public static bool CheckIfRelated(UIAtlas a, UIAtlas b)
	{
		return !(a == null) && !(b == null) && (a == b || a.References(b) || b.References(a));
	}

	public void MarkAsChanged()
	{
		if (this.mReplacement != null)
		{
			this.mReplacement.MarkAsChanged();
		}
		UISprite[] array = NGUITools.FindActive<UISprite>();
		int i = 0;
		int num = array.Length;
		while (i < num)
		{
			UISprite uISprite = array[i];
			if (UIAtlas.CheckIfRelated(this, uISprite.atlas))
			{
				UIAtlas atlas = uISprite.atlas;
				uISprite.atlas = null;
				uISprite.atlas = atlas;
			}
			i++;
		}
		UIFont[] array2 = Resources.FindObjectsOfTypeAll(typeof(UIFont)) as UIFont[];
		int j = 0;
		int num2 = array2.Length;
		while (j < num2)
		{
			UIFont uIFont = array2[j];
			if (UIAtlas.CheckIfRelated(this, uIFont.atlas))
			{
				UIAtlas atlas2 = uIFont.atlas;
				uIFont.atlas = null;
				uIFont.atlas = atlas2;
			}
			j++;
		}
		UILabel[] array3 = NGUITools.FindActive<UILabel>();
		int k = 0;
		int num3 = array3.Length;
		while (k < num3)
		{
			UILabel uILabel = array3[k];
			if (uILabel.bitmapFont != null && UIAtlas.CheckIfRelated(this, uILabel.bitmapFont.atlas))
			{
				UIFont bitmapFont = uILabel.bitmapFont;
				uILabel.bitmapFont = null;
				uILabel.bitmapFont = bitmapFont;
			}
			k++;
		}
	}

	private bool Upgrade()
	{
		if (this.mReplacement)
		{
			return this.mReplacement.Upgrade();
		}
		if (this.mSprites.get_Count() == 0 && this.sprites.get_Count() > 0 && this.material)
		{
			Texture mainTexture = this.material.get_mainTexture();
			int width = (!(mainTexture != null)) ? 512 : mainTexture.get_width();
			int height = (!(mainTexture != null)) ? 512 : mainTexture.get_height();
			for (int i = 0; i < this.sprites.get_Count(); i++)
			{
				UIAtlas.Sprite sprite = this.sprites.get_Item(i);
				Rect outer = sprite.outer;
				Rect inner = sprite.inner;
				if (this.mCoordinates == UIAtlas.Coordinates.TexCoords)
				{
					NGUIMath.ConvertToPixels(outer, width, height, true);
					NGUIMath.ConvertToPixels(inner, width, height, true);
				}
				UISpriteData uISpriteData = new UISpriteData();
				uISpriteData.name = sprite.name;
				uISpriteData.x = Mathf.RoundToInt(outer.get_xMin());
				uISpriteData.y = Mathf.RoundToInt(outer.get_yMin());
				uISpriteData.width = Mathf.RoundToInt(outer.get_width());
				uISpriteData.height = Mathf.RoundToInt(outer.get_height());
				uISpriteData.paddingLeft = Mathf.RoundToInt(sprite.paddingLeft * outer.get_width());
				uISpriteData.paddingRight = Mathf.RoundToInt(sprite.paddingRight * outer.get_width());
				uISpriteData.paddingBottom = Mathf.RoundToInt(sprite.paddingBottom * outer.get_height());
				uISpriteData.paddingTop = Mathf.RoundToInt(sprite.paddingTop * outer.get_height());
				uISpriteData.borderLeft = Mathf.RoundToInt(inner.get_xMin() - outer.get_xMin());
				uISpriteData.borderRight = Mathf.RoundToInt(outer.get_xMax() - inner.get_xMax());
				uISpriteData.borderBottom = Mathf.RoundToInt(outer.get_yMax() - inner.get_yMax());
				uISpriteData.borderTop = Mathf.RoundToInt(inner.get_yMin() - outer.get_yMin());
				this.mSprites.Add(uISpriteData);
			}
			this.sprites.Clear();
			return true;
		}
		return false;
	}
}
