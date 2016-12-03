using System;
using UnityEngine;

[AddComponentMenu("NGUI/UI/NGUI Texture"), ExecuteInEditMode]
public class UITexture : UIBasicSprite
{
	[HideInInspector, SerializeField]
	private Rect mRect = new Rect(0f, 0f, 1f, 1f);

	[HideInInspector, SerializeField]
	private Texture mTexture;

	[HideInInspector, SerializeField]
	private Material mMat;

	[HideInInspector, SerializeField]
	private Shader mShader;

	[HideInInspector, SerializeField]
	private Vector4 mBorder = Vector4.get_zero();

	[HideInInspector, SerializeField]
	private bool mFixedAspect;

	[NonSerialized]
	private int mPMA = -1;

	public override Texture mainTexture
	{
		get
		{
			if (this.mTexture != null)
			{
				return this.mTexture;
			}
			if (this.mMat != null)
			{
				return this.mMat.get_mainTexture();
			}
			return null;
		}
		set
		{
			if (this.mTexture != value)
			{
				if (this.drawCall != null && this.drawCall.widgetCount == 1 && this.mMat == null)
				{
					this.mTexture = value;
					this.drawCall.mainTexture = value;
				}
				else
				{
					base.RemoveFromPanel();
					this.mTexture = value;
					this.mPMA = -1;
					this.MarkAsChanged();
				}
			}
		}
	}

	public override Material material
	{
		get
		{
			return this.mMat;
		}
		set
		{
			if (this.mMat != value)
			{
				base.RemoveFromPanel();
				this.mShader = null;
				this.mMat = value;
				this.mPMA = -1;
				this.MarkAsChanged();
			}
		}
	}

	public override Shader shader
	{
		get
		{
			if (this.mMat != null)
			{
				return this.mMat.get_shader();
			}
			if (this.mShader == null)
			{
				this.mShader = Shader.Find("Unlit/Transparent Colored");
			}
			return this.mShader;
		}
		set
		{
			if (this.mShader != value)
			{
				if (this.drawCall != null && this.drawCall.widgetCount == 1 && this.mMat == null)
				{
					this.mShader = value;
					this.drawCall.shader = value;
				}
				else
				{
					base.RemoveFromPanel();
					this.mShader = value;
					this.mPMA = -1;
					this.mMat = null;
					this.MarkAsChanged();
				}
			}
		}
	}

	public override bool premultipliedAlpha
	{
		get
		{
			if (this.mPMA == -1)
			{
				Material material = this.material;
				this.mPMA = ((!(material != null) || !(material.get_shader() != null) || !material.get_shader().get_name().Contains("Premultiplied")) ? 0 : 1);
			}
			return this.mPMA == 1;
		}
	}

	public override Vector4 border
	{
		get
		{
			return this.mBorder;
		}
		set
		{
			if (this.mBorder != value)
			{
				this.mBorder = value;
				this.MarkAsChanged();
			}
		}
	}

	public Rect uvRect
	{
		get
		{
			return this.mRect;
		}
		set
		{
			if (this.mRect != value)
			{
				this.mRect = value;
				this.MarkAsChanged();
			}
		}
	}

	public override Vector4 drawingDimensions
	{
		get
		{
			Vector2 pivotOffset = base.pivotOffset;
			float num = -pivotOffset.x * (float)this.mWidth;
			float num2 = -pivotOffset.y * (float)this.mHeight;
			float num3 = num + (float)this.mWidth;
			float num4 = num2 + (float)this.mHeight;
			if (this.mTexture != null && this.mType != UIBasicSprite.Type.Tiled)
			{
				int width = this.mTexture.get_width();
				int height = this.mTexture.get_height();
				int num5 = 0;
				int num6 = 0;
				float num7 = 1f;
				float num8 = 1f;
				if (width > 0 && height > 0 && (this.mType == UIBasicSprite.Type.Simple || this.mType == UIBasicSprite.Type.Filled))
				{
					if ((width & 1) != 0)
					{
						num5++;
					}
					if ((height & 1) != 0)
					{
						num6++;
					}
					num7 = 1f / (float)width * (float)this.mWidth;
					num8 = 1f / (float)height * (float)this.mHeight;
				}
				if (this.mFlip == UIBasicSprite.Flip.Horizontally || this.mFlip == UIBasicSprite.Flip.Both)
				{
					num += (float)num5 * num7;
				}
				else
				{
					num3 -= (float)num5 * num7;
				}
				if (this.mFlip == UIBasicSprite.Flip.Vertically || this.mFlip == UIBasicSprite.Flip.Both)
				{
					num2 += (float)num6 * num8;
				}
				else
				{
					num4 -= (float)num6 * num8;
				}
			}
			float num9;
			float num10;
			if (this.mFixedAspect)
			{
				num9 = 0f;
				num10 = 0f;
			}
			else
			{
				Vector4 border = this.border;
				num9 = border.x + border.z;
				num10 = border.y + border.w;
			}
			float num11 = Mathf.Lerp(num, num3 - num9, this.mDrawRegion.x);
			float num12 = Mathf.Lerp(num2, num4 - num10, this.mDrawRegion.y);
			float num13 = Mathf.Lerp(num + num9, num3, this.mDrawRegion.z);
			float num14 = Mathf.Lerp(num2 + num10, num4, this.mDrawRegion.w);
			return new Vector4(num11, num12, num13, num14);
		}
	}

	public bool fixedAspect
	{
		get
		{
			return this.mFixedAspect;
		}
		set
		{
			if (this.mFixedAspect != value)
			{
				this.mFixedAspect = value;
				this.mDrawRegion = new Vector4(0f, 0f, 1f, 1f);
				this.MarkAsChanged();
			}
		}
	}

	public override void MakePixelPerfect()
	{
		base.MakePixelPerfect();
		if (this.mType == UIBasicSprite.Type.Tiled)
		{
			return;
		}
		Texture mainTexture = this.mainTexture;
		if (mainTexture == null)
		{
			return;
		}
		if ((this.mType == UIBasicSprite.Type.Simple || this.mType == UIBasicSprite.Type.Filled || !base.hasBorder) && mainTexture != null)
		{
			int num = mainTexture.get_width();
			int num2 = mainTexture.get_height();
			if ((num & 1) == 1)
			{
				num++;
			}
			if ((num2 & 1) == 1)
			{
				num2++;
			}
			base.width = num;
			base.height = num2;
		}
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
		if (this.mFixedAspect)
		{
			Texture mainTexture = this.mainTexture;
			if (mainTexture != null)
			{
				int num = mainTexture.get_width();
				int num2 = mainTexture.get_height();
				if ((num & 1) == 1)
				{
					num++;
				}
				if ((num2 & 1) == 1)
				{
					num2++;
				}
				float num3 = (float)this.mWidth;
				float num4 = (float)this.mHeight;
				float num5 = num3 / num4;
				float num6 = (float)num / (float)num2;
				if (num6 < num5)
				{
					float num7 = (num3 - num4 * num6) / num3 * 0.5f;
					base.drawRegion = new Vector4(num7, 0f, 1f - num7, 1f);
				}
				else
				{
					float num8 = (num4 - num3 / num6) / num4 * 0.5f;
					base.drawRegion = new Vector4(0f, num8, 1f, 1f - num8);
				}
			}
		}
	}

	public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		Texture mainTexture = this.mainTexture;
		if (mainTexture == null)
		{
			return;
		}
		Rect rect = new Rect(this.mRect.get_x() * (float)mainTexture.get_width(), this.mRect.get_y() * (float)mainTexture.get_height(), (float)mainTexture.get_width() * this.mRect.get_width(), (float)mainTexture.get_height() * this.mRect.get_height());
		Rect inner = rect;
		Vector4 border = this.border;
		inner.set_xMin(inner.get_xMin() + border.x);
		inner.set_yMin(inner.get_yMin() + border.y);
		inner.set_xMax(inner.get_xMax() - border.z);
		inner.set_yMax(inner.get_yMax() - border.w);
		float num = 1f / (float)mainTexture.get_width();
		float num2 = 1f / (float)mainTexture.get_height();
		rect.set_xMin(rect.get_xMin() * num);
		rect.set_xMax(rect.get_xMax() * num);
		rect.set_yMin(rect.get_yMin() * num2);
		rect.set_yMax(rect.get_yMax() * num2);
		inner.set_xMin(inner.get_xMin() * num);
		inner.set_xMax(inner.get_xMax() * num);
		inner.set_yMin(inner.get_yMin() * num2);
		inner.set_yMax(inner.get_yMax() * num2);
		int size = verts.size;
		base.Fill(verts, uvs, cols, rect, inner);
		if (this.onPostFill != null)
		{
			this.onPostFill(this, size, verts, uvs, cols);
		}
	}
}
