using DG.Tweening;
using System;
using UnityEngine;

[RequireComponent(typeof(UIWidget))]
public class UIPentagonChart : MonoBehaviour
{
	private int mKaryoku;

	private int mRaisou;

	private int mTaiku;

	private int mKaihi;

	private int mTaikyu;

	private Action mOnCompleteListener;

	private Mesh mesh;

	private Material mat;

	[SerializeField]
	private GameObject outline;

	private Mesh outMesh;

	private Material outMat;

	private void Awake()
	{
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (Input.GetKeyDown(99))
		{
			this.TEST();
		}
	}

	public void TEST()
	{
		int num = 0;
		int num2 = 100;
		int karyoku = Random.Range(num, num2);
		int raisou = Random.Range(num, num2);
		int taiku = Random.Range(num, num2);
		int kaihi = Random.Range(num, num2);
		int taikyu = Random.Range(num, num2);
		this.Initialize(karyoku, raisou, taiku, kaihi, taikyu);
		this.SetOnCompleteListener(delegate
		{
			Debug.Log(" OnComplete :D");
		});
		this.Play();
	}

	public void Initialize(int karyoku, int raisou, int taiku, int kaihi, int taikyu)
	{
		MeshRenderer component = base.GetComponent<MeshRenderer>();
		if (component == null)
		{
			Debug.Log("No MeshRenderer component");
			return;
		}
		this.mat = component.get_material();
		this.mat.set_renderQueue(5000);
		this.mat.set_color(Color.get_clear());
		MeshFilter component2 = base.GetComponent<MeshFilter>();
		if (component2 == null)
		{
			Debug.Log("No MeshFilter component");
			return;
		}
		this.mesh = component2.get_mesh();
		this.mesh.Clear();
		Vector3[] array = new Vector3[6];
		array[0] = new Vector3(0f, 0f, 0f);
		for (int i = 0; i < 5; i++)
		{
			array[i + 1] = new Vector3(Mathf.Cos((float)(-(float)i * 72) * 3.14159274f / 180f + 1.57079637f), Mathf.Sin((float)(-(float)i * 72) * 3.14159274f / 180f + 1.57079637f), 0f);
		}
		Vector3[] array2 = new Vector3[6];
		for (int j = 0; j < 6; j++)
		{
			array2[j] = new Vector3(0f, 0f, -1f);
		}
		int[] array3 = new int[15];
		for (int k = 0; k < 5; k++)
		{
			array3[3 * k] = 0;
			array3[3 * k + 1] = k + 1;
			array3[3 * k + 2] = k + 2;
		}
		array3[14] = 1;
		this.mesh.set_vertices(array);
		this.mesh.set_normals(array2);
		this.mesh.set_triangles(array3);
		this.mesh.RecalculateNormals();
		this.mesh.RecalculateBounds();
		this.mesh.Optimize();
		component = this.outline.GetComponent<MeshRenderer>();
		if (component == null)
		{
			Debug.Log("No MeshRenderer component on sibling GameObject StatsPentagonOutline");
			return;
		}
		this.outMat = component.get_material();
		this.outMat.set_renderQueue(5000);
		this.outMat.set_color(Color.get_clear());
		component2 = this.outline.GetComponent<MeshFilter>();
		if (component2 == null)
		{
			Debug.Log("No MeshFilter component on sibling GameObject StatsPentagonOutline");
			return;
		}
		this.outMesh = component2.get_mesh();
		this.outMesh.Clear();
		array = new Vector3[11];
		array[0] = new Vector3(0f, 0f, 0f);
		for (int l = 0; l < 5; l++)
		{
			array[2 * l + 1] = new Vector3(Mathf.Cos((float)(-(float)l * 72) * 3.14159274f / 180f + 1.59534f), Mathf.Sin((float)(-(float)l * 72) * 3.14159274f / 180f + 1.59534f), 0f);
			array[2 * l + 2] = new Vector3(Mathf.Cos((float)(-(float)l * 72) * 3.14159274f / 180f + 1.54625273f), Mathf.Sin((float)(-(float)l * 72) * 3.14159274f / 180f + 1.54625273f), 0f);
		}
		array2 = new Vector3[11];
		for (int m = 0; m < 11; m++)
		{
			array2[m] = new Vector3(0f, 0f, -1f);
		}
		array3 = new int[30];
		for (int n = 0; n < 10; n++)
		{
			array3[3 * n] = 0;
			array3[3 * n + 1] = n + 1;
			array3[3 * n + 2] = n + 2;
		}
		array3[29] = 1;
		this.outMesh.set_vertices(array);
		this.outMesh.set_normals(array2);
		this.outMesh.set_triangles(array3);
		this.outMesh.RecalculateNormals();
		this.outMesh.RecalculateBounds();
		this.outMesh.Optimize();
		this.mat.set_color(Color.get_clear());
		this.outMat.set_color(Color.get_clear());
		this.mKaryoku = karyoku;
		this.mRaisou = raisou;
		this.mTaiku = taiku;
		this.mKaihi = kaihi;
		this.mTaikyu = taikyu;
	}

	public void Play()
	{
		this.mat.set_color(new Color(0f, 0.75f, 0.75f, 0.4f));
		this.outMat.set_color(new Color(1f, 1f, 1f, 0.4f));
		Tween tween = this.GenerateTweenChart(this.mKaryoku, this.mRaisou, this.mTaiku, this.mKaihi, this.mTaikyu);
		TweenExtensions.Play<Tween>(tween);
	}

	public void PlayHide()
	{
		Tween tween = this.GenerateTweenChart(0, 0, 0, 0, 0);
		this.mat.set_color(new Color(0f, 0f, 0f, 0f));
		this.outMat.set_color(new Color(0f, 0f, 0f, 0f));
		TweenExtensions.Play<Tween>(tween);
	}

	public void SetOnCompleteListener(Action onCompleteListener)
	{
		this.mOnCompleteListener = onCompleteListener;
	}

	public Tween GenerateTweenChart(int karyoku, int raisou, int taiku, int kaihi, int taikyu)
	{
		float num = 0f;
		float num2 = 1f;
		float num3 = 1f;
		bool flag = DOTween.IsTweening(this);
		if (flag)
		{
			DOTween.Kill(this, false);
		}
		Sequence sequence = TweenSettingsExtensions.SetId<Sequence>(DOTween.Sequence(), this);
		Tween tween = DOVirtual.Float(num, num2, num3, delegate(float currentPercentage)
		{
			float num4 = (float)karyoku * currentPercentage;
			float num5 = (float)raisou * currentPercentage;
			float num6 = (float)taiku * currentPercentage;
			float num7 = (float)kaihi * currentPercentage;
			float num8 = (float)taikyu * currentPercentage;
			float[] array = new float[]
			{
				-1f,
				num4,
				num5,
				num6,
				num7,
				num8
			};
			Vector3[] vertices = this.mesh.get_vertices();
			for (int i = 1; i < 6; i++)
			{
				vertices[i] = Mathf.Max(0.01f, array[i]) * vertices[i].get_normalized();
			}
			this.mesh.set_vertices(vertices);
			this.outMesh.set_vertices(this.CalculateOutline(this.mesh.get_vertices()));
		});
		TweenSettingsExtensions.AppendInterval(sequence, 0.2f);
		TweenSettingsExtensions.Append(sequence, tween);
		TweenSettingsExtensions.OnComplete<Sequence>(sequence, new TweenCallback(this.OnComplete));
		return sequence;
	}

	private Vector3[] CalculateOutline(Vector3[] verts)
	{
		Vector3[] array = new Vector3[11];
		array[0] = Vector3.get_zero();
		for (int i = 1; i < 6; i++)
		{
			Vector3 normalized = (verts[i] - verts[(i + 3) % 5 + 1]).get_normalized();
			Vector3 normalized2 = (verts[i] - verts[i % 5 + 1]).get_normalized();
			Vector3 vector = new Vector3(-normalized.y, normalized.x, 0f);
			Vector3 vector2 = new Vector3(normalized2.y, -normalized2.x, 0f);
			float num = Mathf.Max(-4f, Mathf.Min(4f, Mathf.Sign(Vector3.Dot(verts[i], normalized + normalized2)) * 4f / Mathf.Tan(Mathf.Acos(Vector3.Dot(normalized, normalized2)) / 2f)));
			array[2 * i - 1] = verts[i] + num * normalized + 4f * vector;
			array[2 * i] = verts[i] + num * normalized2 + 4f * vector2;
		}
		return array;
	}

	private void OnComplete()
	{
		if (this.mOnCompleteListener != null)
		{
			this.mOnCompleteListener.Invoke();
		}
	}

	private void OnDestroy()
	{
		bool flag = DOTween.IsTweening(this);
		if (flag)
		{
			DOTween.Kill(this, false);
		}
		this.mOnCompleteListener = null;
		this.mesh = null;
		this.mat = null;
		this.outline = null;
		this.outMesh = null;
		this.outMat = null;
	}
}
