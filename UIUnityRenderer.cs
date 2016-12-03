using System;
using UnityEngine;

public class UIUnityRenderer : UIWidget
{
	public bool allowSharedMaterial;

	[HideInInspector, SerializeField]
	private Renderer mRenderer;

	[HideInInspector, SerializeField]
	private int renderQueue = -1;

	[HideInInspector, SerializeField]
	private Material[] mMats;

	private static readonly Vector3 Verts = new Vector3(10000f, 10000f);

	public Renderer cachedRenderer
	{
		get
		{
			if (this.mRenderer == null)
			{
				this.mRenderer = base.GetComponent<Renderer>();
			}
			return this.mRenderer;
		}
	}

	public override Material material
	{
		get
		{
			if (!this.ExistSharedMaterial0())
			{
				Debug.LogError("Renderer or Material is not found.");
				return null;
			}
			if (!this.allowSharedMaterial)
			{
				if (!this.CheckMaterial(this.mMats))
				{
					this.mMats = new Material[this.cachedRenderer.get_sharedMaterials().Length];
					for (int i = 0; i < this.cachedRenderer.get_sharedMaterials().Length; i++)
					{
						this.mMats[i] = new Material(this.cachedRenderer.get_sharedMaterials()[i]);
						this.mMats[i].set_name(this.mMats[i].get_name() + " (Copy)");
					}
				}
				if (this.CheckMaterial(this.mMats) && Application.get_isPlaying() && this.cachedRenderer.get_materials() != this.mMats)
				{
					this.cachedRenderer.set_materials(this.mMats);
				}
				return this.mMats[0];
			}
			return this.cachedRenderer.get_sharedMaterials()[0];
		}
		set
		{
			throw new NotImplementedException(base.GetType() + " has no material setter");
		}
	}

	public override Shader shader
	{
		get
		{
			if (!this.allowSharedMaterial)
			{
				if (this.CheckMaterial(this.mMats))
				{
					return this.mMats[0].get_shader();
				}
			}
			else if (this.ExistSharedMaterial0())
			{
				return this.cachedRenderer.get_sharedMaterials()[0].get_shader();
			}
			return null;
		}
		set
		{
			throw new NotImplementedException(base.GetType() + " has no shader setter");
		}
	}

	private bool ExistSharedMaterial0()
	{
		return this.cachedRenderer != null && this.CheckMaterial(this.cachedRenderer.get_sharedMaterials());
	}

	private bool CheckMaterial(Material[] mats)
	{
		if (mats != null && mats.Length > 0)
		{
			for (int i = 0; i < mats.Length; i++)
			{
				if (mats[i] == null)
				{
					return false;
				}
			}
			return true;
		}
		return false;
	}

	private void OnDestroy()
	{
		this.mMats = null;
		this.mRenderer = null;
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
		if (!this.allowSharedMaterial)
		{
			if (this.CheckMaterial(this.mMats) && this.drawCall != null)
			{
				this.renderQueue = this.drawCall.finalRenderQueue;
				for (int i = 0; i < this.mMats.Length; i++)
				{
					if (this.mMats[i].get_renderQueue() != this.renderQueue)
					{
						this.mMats[i].set_renderQueue(this.renderQueue);
					}
				}
			}
		}
		else if (this.ExistSharedMaterial0() && this.drawCall != null)
		{
			this.renderQueue = this.drawCall.finalRenderQueue;
			for (int j = 0; j < this.cachedRenderer.get_sharedMaterials().Length; j++)
			{
				this.cachedRenderer.get_sharedMaterials()[j].set_renderQueue(this.renderQueue);
			}
		}
	}

	public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		verts.Add(UIUnityRenderer.Verts);
		verts.Add(UIUnityRenderer.Verts);
		verts.Add(UIUnityRenderer.Verts);
		verts.Add(UIUnityRenderer.Verts);
		uvs.Add(Vector2.get_zero());
		uvs.Add(Vector2.get_up());
		uvs.Add(Vector2.get_one());
		uvs.Add(Vector2.get_right());
		cols.Add(base.color);
		cols.Add(base.color);
		cols.Add(base.color);
		cols.Add(base.color);
	}
}
