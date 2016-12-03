using System;
using UniRx;
using UnityEngine;

public class TouchEventManager : MonoBehaviour
{
	private IDisposable _disStream;

	private Vector2 _vTouchPos;

	private ParticleSystem[] _touchParticles;

	private bool isCancel;

	private void Awake()
	{
		this.Init();
		this.StartTouchStream();
	}

	public bool Init()
	{
		this._disStream = null;
		this._touchParticles = new ParticleSystem[1];
		for (int i = 0; i < 1; i++)
		{
			Util.FindParentToChild<ParticleSystem>(ref this._touchParticles[i], base.get_transform(), "TouchParticle" + (i + 1));
		}
		return true;
	}

	private void OnDestroy()
	{
		Mem.DelIDisposableSafe<IDisposable>(ref this._disStream);
		Mem.Del<Vector2>(ref this._vTouchPos);
		Mem.Del<ParticleSystem[]>(ref this._touchParticles);
	}

	public void StartTouchStream()
	{
		if (this._disStream != null)
		{
			this._disStream.Dispose();
		}
		this._disStream = (from x in Observable.EveryUpdate()
		where Input.get_touchCount() > 0 && Input.GetTouch(0).get_phase() == 0
		select (Input.get_touchCount() <= 0) ? new Vector2(1000f, 0f) : Input.GetTouch(0).get_position()).Subscribe(delegate(Vector2 pos)
		{
			this._startParticle(pos);
		});
	}

	public void StopStream()
	{
		this._disStream.Dispose();
	}

	public void _startParticle(Vector2 pos)
	{
		if (this._touchParticles == null)
		{
			return;
		}
		if (this.isCancel)
		{
			this.isCancel = false;
			return;
		}
		this._touchParticles[0].Stop();
		this._touchParticles[0].Clear();
		this._touchParticles[0].set_time(0f);
		Vector2 vector = new Vector2(pos.x - 480f, pos.y - 272f);
		this._touchParticles[0].get_transform().set_localPosition(vector);
		this._touchParticles[0].Play();
	}

	public void stopParticle()
	{
		for (int i = 0; i < 5; i++)
		{
			if (!(this._touchParticles[i] == null))
			{
				if (this._touchParticles[i].get_isPlaying())
				{
					this._touchParticles[i].Stop();
					this._touchParticles[i].Clear();
					this._touchParticles[i].set_time(0f);
				}
			}
		}
		this.isCancel = true;
	}
}
