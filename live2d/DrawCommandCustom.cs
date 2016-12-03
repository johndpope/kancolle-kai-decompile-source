using System;
using UnityEngine;

namespace live2d
{
	internal class DrawCommandCustom
	{
		public string name;

		public CombineInstance combine;

		public int opacity;

		public Color color;

		public Vector3[] vertices;

		public Color[] colors;

		public DrawCommandCustom()
		{
			this.combine = default(CombineInstance);
			this.combine.set_transform(Matrix4x4.get_identity());
			this.color.a = 0f;
			this.opacity = -1;
		}

		public void setMesh(Mesh mesh)
		{
			this.combine.set_mesh(mesh);
			this.vertices = mesh.get_vertices();
			this.colors = mesh.get_colors();
		}

		public override string ToString()
		{
			return string.Format("DrawCommand {0} opacity:{1} color{2}", this.name, this.opacity, this.color.ToString());
		}
	}
}
