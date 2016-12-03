using LT.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.SortieMap
{
	public class UIWobblingIcon : MonoBehaviour
	{
		private const int ORIGIN_NUM = 11;

		[SerializeField]
		private int _nMstID = 500;

		[SerializeField]
		private Material _material;

		[SerializeField]
		private MeshFilter _mfFilter;

		[SerializeField]
		private MeshRenderer _mrRenderer;

		[Button("SetShipTexture", "画像設定", new object[]
		{

		}), SerializeField]
		private int _nSetShipTexture;

		private int _nRenderQueue = 4;

		private List<Vector3> _listCenterOrigin;

		private List<Vector3> _listTempVertex;

		private List<Vector3> _listVertex;

		private List<Vector2> _listUVs;

		private List<Vector3> _listNormals;

		private bool _isWobblling;

		public bool isWobbling
		{
			get
			{
				return this._isWobblling;
			}
			set
			{
				this._isWobblling = value;
				this._mrRenderer.set_enabled(value);
			}
		}

		private void Awake()
		{
			this._listCenterOrigin = new List<Vector3>(11);
			for (int i = 0; i < 11; i++)
			{
				this._listCenterOrigin.Add(Vector3.get_zero());
			}
			this._mfFilter.get_sharedMesh().MarkDynamic();
			this._listVertex = new List<Vector3>(this._mfFilter.get_sharedMesh().get_vertices());
			this._listUVs = new List<Vector2>(this._mfFilter.get_mesh().get_uv());
			this._listNormals = new List<Vector3>(this._mfFilter.get_mesh().get_normals());
			this._listTempVertex = new List<Vector3>(this._mfFilter.get_mesh().get_vertices());
			this.isWobbling = false;
		}

		private void OnDestroy()
		{
			Transform transform = this._mrRenderer.get_transform();
			Mem.DelMeshSafe(ref transform);
			Mem.Del<Material>(ref this._material);
			Mem.Del<MeshFilter>(ref this._mfFilter);
			Mem.Del<MeshRenderer>(ref this._mrRenderer);
			Mem.DelListSafe<Vector3>(ref this._listCenterOrigin);
			Mem.DelListSafe<Vector3>(ref this._listTempVertex);
			Mem.DelListSafe<Vector3>(ref this._listVertex);
			Mem.DelListSafe<Vector2>(ref this._listUVs);
			Mem.DelListSafe<Vector3>(ref this._listNormals);
			Mem.Del<Transform>(ref transform);
		}

		public bool FixedRun()
		{
			if (!this.isWobbling)
			{
				return true;
			}
			for (int i = 0; i < this._listCenterOrigin.get_Count(); i++)
			{
				this._listCenterOrigin.set_Item(i, Mathf.Sin(2f * (Time.get_time() - 0.2f * (float)i)) * Vector3.get_right());
				for (int j = 0; j < this._listCenterOrigin.get_Count(); j++)
				{
					this._listTempVertex.set_Item(this._listCenterOrigin.get_Count() * i + j, new Vector3(0.6f + this._listCenterOrigin.get_Item(i).x + 5f - (float)j, 0f, this._listTempVertex.get_Item(this._listCenterOrigin.get_Count() * i + j).z));
				}
			}
			this._mfFilter.get_mesh().set_vertices(this._listTempVertex.ToArray());
			this._mfFilter.get_mesh().set_uv(this._listUVs.ToArray());
			this._mfFilter.get_mesh().set_normals(this._listNormals.ToArray());
			this._mfFilter.get_mesh().Optimize();
			this._mfFilter.get_mesh().RecalculateBounds();
			return true;
		}

		public LTDescr Show()
		{
			this.SetShipTexture();
			this.isWobbling = true;
			Color white = Color.get_white();
			white.a = 0f;
			this._mrRenderer.get_sharedMaterial().set_color(white);
			return this._mrRenderer.get_transform().LTValue(0f, 1f, 0.3f).setOnUpdate(delegate(float x)
			{
				Color color = this._mrRenderer.get_sharedMaterial().get_color();
				color.a = x;
				this._mrRenderer.get_sharedMaterial().set_color(color);
			});
		}

		public LTDescr Hide()
		{
			return this._mrRenderer.get_transform().LTValue(1f, 0f, 0.3f).setOnUpdate(delegate(float x)
			{
				Color color = this._mrRenderer.get_sharedMaterial().get_color();
				color.a = x;
				this._mrRenderer.get_sharedMaterial().set_color(color);
			}).setOnComplete(delegate
			{
				this.isWobbling = false;
			});
		}

		private void SetShipTexture()
		{
			Texture2D texture2D = Resources.Load<Texture2D>(string.Format("Textures/Ships/{0}/{1}", this._nMstID, 9));
			this._mrRenderer.set_material(new Material(this._material));
			this._mrRenderer.get_sharedMaterial().set_color(Color.get_clear());
			this._mrRenderer.get_sharedMaterial().set_mainTexture(texture2D);
			if (texture2D != null)
			{
				this._mrRenderer.get_transform().set_localScale(new Vector3((float)texture2D.get_width(), 0f, (float)texture2D.get_height()));
			}
			this._mrRenderer.get_sharedMaterial().set_renderQueue(3000 + this._nRenderQueue);
		}
	}
}
