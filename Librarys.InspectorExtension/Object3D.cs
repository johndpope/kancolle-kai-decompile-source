using System;
using UnityEngine;

namespace Librarys.InspectorExtension
{
	[AddComponentMenu("GameObject/3DObject"), RequireComponent(typeof(MeshFilter)), RequireComponent(typeof(MeshRenderer))]
	public class Object3D : MonoBehaviour
	{
		[SerializeField]
		private Texture _texture;

		[SerializeField]
		private Material _material;

		[SerializeField]
		private Mesh _mesh;

		[SerializeField]
		private Color _color = Color.get_white();

		[SerializeField]
		private Vector2 _localSize = Vector2.get_one();

		[SerializeField]
		private int _nRenderQueue;

		[Button("MakePixelPerfect", "MakePixelPerfect.", new object[]
		{

		}), SerializeField]
		private int _isMakePixelPerfect;

		private MeshFilter _mfFiler;

		private MeshRenderer _mrRenderer;

		public virtual Texture mainTexture
		{
			get
			{
				if (this._texture != null)
				{
					return this._texture;
				}
				return null;
			}
			set
			{
				if (this._texture != value)
				{
					this._texture = value;
					this.MarkAsChanged();
				}
			}
		}

		public virtual Material material
		{
			get
			{
				if (this._material == null)
				{
					return this.meshRenderer.get_material();
				}
				return this._material;
			}
			set
			{
				if (this._material != value)
				{
					this._material = value;
					this.MarkAsChanged();
				}
			}
		}

		public Color color
		{
			get
			{
				return this._color;
			}
			set
			{
				if (this._color != value)
				{
					this._color = value;
					this.MarkAsChanged();
				}
			}
		}

		public Mesh mesh
		{
			get
			{
				return this._mesh;
			}
			set
			{
				if (this._mesh != value)
				{
					this._mesh = value;
					this.MarkAsChanged();
				}
			}
		}

		public MeshFilter meshFilter
		{
			get
			{
				if (this._mfFiler == null)
				{
					this._mfFiler = this.SafeGetComponent<MeshFilter>();
				}
				return this._mfFiler;
			}
			set
			{
				if (this._mfFiler != value)
				{
					this._mfFiler = value;
				}
			}
		}

		public MeshRenderer meshRenderer
		{
			get
			{
				if (this._mrRenderer == null)
				{
					this._mrRenderer = this.SafeGetComponent<MeshRenderer>();
				}
				return this._mrRenderer;
			}
			set
			{
				if (this._mrRenderer != value)
				{
					this._mrRenderer = value;
				}
			}
		}

		public Vector2 localSize
		{
			get
			{
				return this._localSize;
			}
			set
			{
				if (value != this._localSize)
				{
					this._localSize = value;
				}
			}
		}

		public int renderQueue
		{
			get
			{
				if (this.meshRenderer == null)
				{
					return -1;
				}
				if (this.meshRenderer.get_sharedMaterial() == null)
				{
					return -1;
				}
				return this.meshRenderer.get_sharedMaterial().get_renderQueue();
			}
			set
			{
				if (this.meshRenderer == null)
				{
					return;
				}
				if (this.meshRenderer.get_sharedMaterial() != null)
				{
					this._nRenderQueue = value;
					this.meshRenderer.get_sharedMaterial().set_renderQueue(this._nRenderQueue);
				}
			}
		}

		private void OnDestroy()
		{
			Mem.Del<Texture>(ref this._texture);
			Mem.Del<Material>(ref this._material);
			Mem.Del<Mesh>(ref this._mesh);
			Mem.Del<Color>(ref this._color);
			Mem.Del<int>(ref this._nRenderQueue);
			Mem.Del<MeshFilter>(ref this._mfFiler);
			Mem.Del<MeshRenderer>(ref this._mrRenderer);
			Transform transform = base.get_transform();
			Mem.DelMeshSafe(ref transform);
		}

		public virtual void Release()
		{
			if (this._texture != null)
			{
				Resources.UnloadAsset(this._texture);
			}
		}

		public virtual void MarkAsChanged()
		{
			if (this.material != null && this.meshRenderer.get_sharedMaterial() != this.material)
			{
				this.meshRenderer.set_sharedMaterial(this.material);
			}
			if (this.mesh != null && this.meshFilter != null)
			{
				this.meshFilter.set_mesh(this.mesh);
				if (this.meshRenderer.get_material().get_mainTexture() != this.mainTexture)
				{
					this.meshRenderer.get_material().set_mainTexture(this.mainTexture);
				}
				if (this.meshRenderer.get_material().HasProperty("_Color"))
				{
					if (this.meshRenderer.get_material().get_color() != this.color)
					{
						this.meshRenderer.get_material().set_color(this.color);
					}
				}
				else if (this.meshRenderer.get_material().HasProperty("_TintColor") && this.meshRenderer.get_material().GetColor("_TintColor") != this.color)
				{
					this.meshRenderer.get_material().SetColor("_TintColor", this.color);
				}
			}
		}

		public virtual void MakePixelPerfect()
		{
			if (this.meshRenderer.get_sharedMaterial().get_mainTexture() == null)
			{
				return;
			}
			base.get_transform().set_localScale(new Vector3((float)this.meshRenderer.get_sharedMaterial().get_mainTexture().get_width(), (float)this.meshRenderer.get_sharedMaterial().get_mainTexture().get_height(), 0f));
			this.localSize = base.get_transform().get_localScale();
		}
	}
}
