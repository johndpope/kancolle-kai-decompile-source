using local.models;
using System;
using System.Collections;
using UnityEngine;

namespace KCV
{
	public class BaseSelectMove : MonoBehaviour
	{
		public virtual bool Init(ShipModel ship, UITexture _texture)
		{
			return true;
		}

		public virtual void Move(GameObject obj, float fromX, float fromY, float toX, float toY)
		{
			Vector3 localPosition = new Vector3(fromX, fromY);
			obj.get_transform().set_localPosition(localPosition);
			Hashtable hashtable = new Hashtable();
			hashtable.Add("time", 0.1f);
			hashtable.Add("x", toX);
			hashtable.Add("y", toY);
			hashtable.Add("easeType", iTween.EaseType.linear);
			hashtable.Add("isLocal", true);
			hashtable.Add("oncompletetarget", base.get_gameObject());
			obj.MoveTo(hashtable);
		}

		public virtual void Move(GameObject obj, float toX, float toY)
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add("time", 0.1f);
			hashtable.Add("x", toX);
			hashtable.Add("y", toY);
			hashtable.Add("easeType", iTween.EaseType.linear);
			hashtable.Add("isLocal", true);
			hashtable.Add("oncompletetarget", obj);
			obj.MoveTo(hashtable);
		}
	}
}
