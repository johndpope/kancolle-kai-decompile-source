using System;
using System.Collections;
using UnityEngine;

namespace KCV.InteriorStore
{
	public class UIISModeInfo : MonoBehaviour
	{
		private static readonly float MODE_MOVE_TIME = 1f;

		private UISprite _uiMode;

		private Vector3[] _vPos = new Vector3[]
		{
			new Vector3(372f, 238f, 0f),
			new Vector3(590f, 238f, 0f)
		};

		private void Awake()
		{
			base.get_transform().set_localPosition(this._vPos[1]);
			Util.FindParentToChild<UISprite>(ref this._uiMode, base.get_transform(), "Mode");
		}

		private void Start()
		{
		}

		private void Update()
		{
		}

		public void ReqMode(ISTaskManagerMode iMode)
		{
			Vector3 vector = (iMode != ISTaskManagerMode.ISTaskManagerMode_ST) ? this._vPos[0] : this._vPos[1];
			Hashtable hashtable = new Hashtable();
			hashtable.Add("position", vector);
			hashtable.Add("time", UIISModeInfo.MODE_MOVE_TIME);
			hashtable.Add("isLocal", true);
			hashtable.Add("easeType", iTween.EaseType.easeOutExpo);
			if (iMode != ISTaskManagerMode.Interior)
			{
				if (iMode == ISTaskManagerMode.Store)
				{
					this._uiMode.spriteName = "header_shop";
				}
			}
			else
			{
				this._uiMode.spriteName = "header_change";
			}
			iTween.MoveTo(base.get_gameObject(), hashtable);
			hashtable.Clear();
		}
	}
}
