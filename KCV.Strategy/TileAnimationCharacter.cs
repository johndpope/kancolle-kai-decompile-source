using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Strategy
{
	public class TileAnimationCharacter : MonoBehaviour
	{
		public enum STATE
		{
			NONE,
			IDLE,
			POPUP,
			WAVE,
			FLOAT
		}

		private TileAnimationCharacter.STATE state;

		private GameObject meshGO;

		private Mesh mesh;

		private MeshRenderer renderer;

		private Vector3[] workingVertSet;

		private GameObject[] guides;

		private static Vector3[] VERTS;

		private static Vector2[] UVS;

		private static Vector3[] NORMS;

		private float timer;

		private void Awake()
		{
			this.meshGO = base.get_transform().Find("Mesh").get_gameObject();
			if (this.meshGO == null)
			{
				Debug.Log("Warning: ./Mesh not found");
			}
			try
			{
				this.mesh = this.meshGO.GetComponent<MeshFilter>().get_mesh();
			}
			catch (NullReferenceException)
			{
				Debug.Log("Warning: No mesh specified for MeshFilter component of ./Mesh");
			}
			if (this.mesh == null)
			{
				Debug.Log("Warning: MeshFilter component not attached to ./Mesh");
			}
			this.renderer = this.meshGO.GetComponent<MeshRenderer>();
			if (this.renderer == null)
			{
				Debug.Log("Warning: MeshRenderer not attached to ./Mesh");
			}
			this.guides = new GameObject[11];
			for (int i = 0; i < 11; i++)
			{
				this.guides[i] = base.get_transform().Find("Guide" + (i + 1)).get_gameObject();
				if (this.guides[i] == null)
				{
					Debug.Log("Warning: ./Guide" + (i + 1) + " not found");
				}
				else
				{
					this.guides[i].get_transform().set_parent(base.get_transform());
					this.guides[i].get_transform().set_localPosition(Vector3.get_zero());
				}
			}
			this.state = TileAnimationCharacter.STATE.NONE;
			this.workingVertSet = new Vector3[121];
			this.mesh.MarkDynamic();
			TileAnimationCharacter.VERTS = this.mesh.get_vertices();
			TileAnimationCharacter.UVS = this.mesh.get_uv();
			TileAnimationCharacter.NORMS = this.mesh.get_normals();
			this.renderer.set_enabled(false);
			this.renderer.get_material().set_renderQueue(5000);
			this.timer = 0f;
		}

		private void Update()
		{
			this.renderer.set_enabled(true);
			if (this.state == TileAnimationCharacter.STATE.NONE)
			{
				this.renderer.set_enabled(false);
			}
			else if (this.state == TileAnimationCharacter.STATE.POPUP)
			{
				this.PopUpUpdate();
			}
			else if (this.state == TileAnimationCharacter.STATE.WAVE)
			{
				this.WaveUpdate();
			}
			else if (this.state == TileAnimationCharacter.STATE.FLOAT)
			{
				this.FloatUpdate();
			}
		}

		public void PopUp(TileAnimationCharacter.STATE next = TileAnimationCharacter.STATE.WAVE, bool isSkip = false)
		{
			this.state = TileAnimationCharacter.STATE.POPUP;
			this.workingVertSet = this.mesh.get_vertices();
			float num = (!isSkip) ? 1f : 0.1f;
			float num2 = (!isSkip) ? 0.06f : 0f;
			float num3 = (!isSkip) ? 0.2f : 0f;
			float d = (!isSkip) ? 1.6f : 0.3f;
			for (int i = 0; i < 11; i++)
			{
				for (int j = 0; j < 11; j++)
				{
					this.workingVertSet[11 * i + j] = new Vector3(this.workingVertSet[11 * i + j].x * 0f, 0f, this.workingVertSet[11 * i + j].z);
				}
				this.guides[i].get_transform().set_localPosition(new Vector3(0.01f, 0f, 0f));
				iTween.MoveTo(this.guides[i], iTween.Hash(new object[]
				{
					"position",
					Vector3.get_right(),
					"islocal",
					true,
					"time",
					num,
					"delay",
					num2 * (float)(10 - i),
					"easeType",
					iTween.EaseType.easeOutExpo
				}));
			}
			this.mesh.set_vertices(this.workingVertSet);
			this.mesh.set_uv(TileAnimationCharacter.UVS);
			this.mesh.set_normals(TileAnimationCharacter.NORMS);
			base.get_gameObject().get_transform().set_localPosition(new Vector3(0f, 0f, 5f));
			iTween.MoveTo(base.get_gameObject(), iTween.Hash(new object[]
			{
				"position",
				Vector3.get_zero(),
				"islocal",
				true,
				"time",
				num,
				"delay",
				num3,
				"easeType",
				iTween.EaseType.easeOutQuad
			}));
			base.StartCoroutine(this.FinishAnimation(next, d));
		}

		public void PopUpUpdate()
		{
			for (int i = 0; i < 11; i++)
			{
				for (int j = 0; j < 11; j++)
				{
					this.workingVertSet[11 * i + j] = new Vector3(this.guides[i].get_transform().get_localPosition().x * (float)(5 - j), 0f, this.workingVertSet[11 * i + j].z);
				}
			}
			this.mesh.set_vertices(this.workingVertSet);
			this.mesh.set_uv(TileAnimationCharacter.UVS);
			this.mesh.set_normals(TileAnimationCharacter.NORMS);
			this.mesh.Optimize();
			this.mesh.RecalculateBounds();
		}

		public void Wave()
		{
			this.state = TileAnimationCharacter.STATE.WAVE;
			this.workingVertSet = this.mesh.get_vertices();
			for (int i = 0; i < 11; i++)
			{
				this.guides[i].get_transform().set_localPosition(Vector3.get_zero());
			}
			this.timer = Time.get_time();
		}

		public void WaveUpdate()
		{
			for (int i = 0; i < 11; i++)
			{
				this.guides[i].get_transform().set_localPosition(Mathf.Min(Time.get_time() - this.timer, 1f) * Mathf.Sin(2f * (Time.get_time() - this.timer - 0.2f * (float)i)) * Vector3.get_right());
				for (int j = 0; j < 11; j++)
				{
					this.workingVertSet[11 * i + j] = new Vector3(0.6f * this.guides[i].get_transform().get_localPosition().x + 5f - (float)j, 0f, this.workingVertSet[11 * i + j].z);
				}
			}
			this.mesh.set_vertices(this.workingVertSet);
			this.mesh.set_uv(TileAnimationCharacter.UVS);
			this.mesh.set_normals(TileAnimationCharacter.NORMS);
			this.mesh.Optimize();
			this.mesh.RecalculateBounds();
		}

		public void Float()
		{
			this.state = TileAnimationCharacter.STATE.FLOAT;
			this.timer = Time.get_time();
		}

		public void FloatUpdate()
		{
			this.meshGO.get_transform().set_localPosition(new Vector3(0f, 0f, 0.2f * Mathf.Sin(6.28318548f * (Time.get_time() - this.timer))));
		}

		public void Reset()
		{
			this.state = TileAnimationCharacter.STATE.NONE;
		}

		public void UnloadTexture()
		{
			if (this.renderer.get_material().get_mainTexture() != null)
			{
				Resources.UnloadAsset(this.renderer.get_material().get_mainTexture());
			}
		}

		public void SetTexture(Texture t)
		{
			this.renderer.get_material().set_mainTexture(t);
			base.get_transform().get_parent().set_localScale(new Vector3(0.025f * (float)this.renderer.get_material().get_mainTexture().get_width(), 1f, 0.025f * (float)this.renderer.get_material().get_mainTexture().get_height()));
		}

		[DebuggerHidden]
		public IEnumerator FinishAnimation(TileAnimationCharacter.STATE next = TileAnimationCharacter.STATE.WAVE, float d = 0f)
		{
			TileAnimationCharacter.<FinishAnimation>c__Iterator19E <FinishAnimation>c__Iterator19E = new TileAnimationCharacter.<FinishAnimation>c__Iterator19E();
			<FinishAnimation>c__Iterator19E.d = d;
			<FinishAnimation>c__Iterator19E.next = next;
			<FinishAnimation>c__Iterator19E.<$>d = d;
			<FinishAnimation>c__Iterator19E.<$>next = next;
			<FinishAnimation>c__Iterator19E.<>f__this = this;
			return <FinishAnimation>c__Iterator19E;
		}

		private void OnDestroy()
		{
			this.meshGO = null;
			this.mesh = null;
			this.renderer = null;
			this.workingVertSet = null;
			this.guides = null;
		}
	}
}
