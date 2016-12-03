using System;
using UnityEngine;

[ExecuteInEditMode]
public class NoiseMove : MonoBehaviour
{
	public float _noiseSpeed = 8f;

	public float _noisePower;

	private void OnDestroy()
	{
		Mem.Del<float>(ref this._noiseSpeed);
		Mem.Del<float>(ref this._noisePower);
	}

	private void Update()
	{
		float time = Time.get_time();
		float num = time;
		float num2 = (Mathf.PerlinNoise(num * this._noiseSpeed, 0f) * 2f - 1f) * this._noisePower;
		float num3 = (Mathf.PerlinNoise(num * this._noiseSpeed, 40f) * 2f - 1f) * this._noisePower;
		base.get_transform().set_localPosition(new Vector3(num2, num3));
	}
}
