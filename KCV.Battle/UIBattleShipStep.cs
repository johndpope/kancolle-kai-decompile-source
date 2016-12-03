using System;
using UnityEngine;

namespace KCV.Battle
{
	public class UIBattleShipStep : MonoBehaviour
	{
		[SerializeField]
		private Animation _anim;

		private void Awake()
		{
			if (this._anim == null)
			{
				this._anim = base.GetComponent<Animation>();
			}
			this._anim.set_playAutomatically(false);
			this._anim.Stop();
			base.get_transform().set_localScale(new Vector3(500f, 500f, 0f));
		}

		private void OnDestroy()
		{
		}

		public void Play()
		{
			this._anim.Play("BattleShipStep");
		}

		private void changeOffset(int num)
		{
			switch (num)
			{
			case 0:
				base.GetComponent<Renderer>().get_material().set_mainTextureOffset(new Vector2(0f, 0.5f));
				break;
			case 1:
				base.GetComponent<Renderer>().get_material().set_mainTextureOffset(new Vector2(0.5f, 0.5f));
				break;
			case 2:
				base.GetComponent<Renderer>().get_material().set_mainTextureOffset(new Vector2(0f, 0f));
				break;
			case 3:
				base.GetComponent<Renderer>().get_material().set_mainTextureOffset(new Vector2(0.5f, 0f));
				break;
			}
		}
	}
}
