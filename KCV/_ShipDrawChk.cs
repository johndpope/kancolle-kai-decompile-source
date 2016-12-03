using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace KCV
{
	public class _ShipDrawChk : MonoBehaviour
	{
		public AudioClip clip;

		private void Awake()
		{
			XorRandom.Init(0u);
			Rand.Init(1);
			Debug.Log(string.Empty + this.GetEnum().get_Current());
		}

		[DebuggerHidden]
		public IEnumerator<int> GetEnum()
		{
			return new _ShipDrawChk.<GetEnum>c__Iterator1C8();
		}

		private void Start()
		{
			Debug.Log("file://" + Application.get_dataPath() + "/AssetBundle/Editor/Sounds/BGM/1.unity3d");
			base.StartCoroutine(AssetBundleData.LoadAsync<AudioClip>("file://" + Application.get_dataPath() + "/AssetBundle/Editor/Sounds/BGM/1.unity3d", 0, delegate(AudioClip clip)
			{
				this.clip = clip;
			}));
		}

		private void Update()
		{
			if (Input.GetKeyDown(97))
			{
				Debug.Log(string.Empty + Application.get_dataPath());
				string text = Application.get_dataPath() + string.Empty;
				using (new WWW("=?time=" + DateTime.get_Now().GetHashCode()))
				{
				}
			}
			if (Input.GetKeyDown(98))
			{
			}
			if (Input.GetKeyDown(99))
			{
			}
		}
	}
}
