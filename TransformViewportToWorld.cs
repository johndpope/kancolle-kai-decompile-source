using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class TransformViewportToWorld : MonoBehaviour
{
	public Vector3 lb;

	public Vector3 rt;

	public Vector3 lt;

	public Vector3 rb;

	public Transform center;

	private GameObject mapBG;

	private float mapX1;

	private float mapX2;

	private float mapY1;

	private float mapY2;

	private Vector3[] worldPosBG;

	private void Start()
	{
		this.mapBG = GameObject.Find("Map_BG");
		Vector3 position = this.mapBG.get_transform().get_position();
		UITexture component = this.mapBG.GetComponent<UITexture>();
		float num = (float)component.width;
		float num2 = (float)component.height;
		this.mapX1 = position.x - num / 2f;
		this.mapX2 = position.x + num / 2f;
		this.mapY1 = position.y - num2 / 2f;
		this.mapY2 = position.y + num2 / 2f;
		this.worldPosBG = new Vector3[4];
		this.worldPosBG[0] = component.worldCorners[0];
		this.worldPosBG[1] = component.worldCorners[1];
		this.worldPosBG[2] = component.worldCorners[2];
		this.worldPosBG[3] = component.worldCorners[3];
	}

	private void Update()
	{
		float num = Vector3.Distance(base.get_transform().get_position(), this.center.get_position());
		this.lb = base.GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0f, 0f, num));
		this.rt = base.GetComponent<Camera>().ViewportToWorldPoint(new Vector3(1f, 1f, num));
		this.lt = new Vector3(this.lb.x, this.rt.y, this.lb.z);
		this.rb = new Vector3(this.rt.x, this.lb.y, this.rt.z);
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawSphere(this.lb, 0.3f);
		Gizmos.DrawSphere(this.rt, 0.3f);
		Gizmos.DrawSphere(this.rb, 0.3f);
		Gizmos.DrawSphere(this.lt, 0.3f);
	}
}
