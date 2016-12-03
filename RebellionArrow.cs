using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class RebellionArrow : MonoBehaviour
{
	private UITexture Arrow;

	public Vector3 FromTilePos;

	public Vector3 TargetTilePos;

	[Button("DebugAnimation", "START", new object[]
	{

	})]
	public int button1;

	[Button("EndAnimation", "END", new object[]
	{

	})]
	public int button2;

	private float movedValue;

	public float speed = 0.1f;

	public float moveDistance = 5f;

	private bool isEnd;

	[DebuggerHidden]
	private IEnumerator update()
	{
		RebellionArrow.<update>c__Iterator161 <update>c__Iterator = new RebellionArrow.<update>c__Iterator161();
		<update>c__Iterator.<>f__this = this;
		return <update>c__Iterator;
	}

	private void moveArrow()
	{
		Transform expr_0B = this.Arrow.get_transform();
		expr_0B.set_position(expr_0B.get_position() + this.Arrow.get_transform().TransformDirection(Vector2.get_up()) * this.speed);
		this.movedValue += this.speed;
		if (this.movedValue > this.moveDistance)
		{
			this.Arrow.get_transform().set_position(this.FromTilePos);
			this.movedValue = 0f;
		}
		float num = this.movedValue / this.moveDistance;
		this.Arrow.alpha = num * 1.5f;
	}

	public void StartAnimation(Vector3 fromTile, Vector3 targetTile)
	{
		this.movedValue = 0f;
		this.Arrow = base.GetComponent<UITexture>();
		this.isEnd = false;
		this.FromTilePos = fromTile;
		this.TargetTilePos = targetTile;
		this.Arrow.get_transform().set_position(this.FromTilePos);
		base.StartCoroutine(this.update());
	}

	public void EndAnimation()
	{
		this.isEnd = true;
	}

	private void DebugAnimation()
	{
		this.StartAnimation(this.FromTilePos, this.TargetTilePos);
	}
}
