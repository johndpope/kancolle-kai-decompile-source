using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Key Navigation")]
public class UIKeyNavigation : MonoBehaviour
{
	public enum Constraint
	{
		None,
		Vertical,
		Horizontal,
		Explicit
	}

	public static BetterList<UIKeyNavigation> list = new BetterList<UIKeyNavigation>();

	public UIKeyNavigation.Constraint constraint;

	public GameObject onUp;

	public GameObject onDown;

	public GameObject onLeft;

	public GameObject onRight;

	public GameObject onClick;

	public bool startsSelected;

	protected virtual void OnEnable()
	{
		UIKeyNavigation.list.Add(this);
		if (this.startsSelected && (UICamera.selectedObject == null || !NGUITools.GetActive(UICamera.selectedObject)))
		{
			UICamera.currentScheme = UICamera.ControlScheme.Controller;
			UICamera.selectedObject = base.get_gameObject();
		}
	}

	protected virtual void OnDisable()
	{
		UIKeyNavigation.list.Remove(this);
	}

	protected GameObject GetLeft()
	{
		if (NGUITools.GetActive(this.onLeft))
		{
			return this.onLeft;
		}
		if (this.constraint == UIKeyNavigation.Constraint.Vertical || this.constraint == UIKeyNavigation.Constraint.Explicit)
		{
			return null;
		}
		return this.Get(Vector3.get_left(), true);
	}

	private GameObject GetRight()
	{
		if (NGUITools.GetActive(this.onRight))
		{
			return this.onRight;
		}
		if (this.constraint == UIKeyNavigation.Constraint.Vertical || this.constraint == UIKeyNavigation.Constraint.Explicit)
		{
			return null;
		}
		return this.Get(Vector3.get_right(), true);
	}

	protected GameObject GetUp()
	{
		if (NGUITools.GetActive(this.onUp))
		{
			return this.onUp;
		}
		if (this.constraint == UIKeyNavigation.Constraint.Horizontal || this.constraint == UIKeyNavigation.Constraint.Explicit)
		{
			return null;
		}
		return this.Get(Vector3.get_up(), false);
	}

	protected GameObject GetDown()
	{
		if (NGUITools.GetActive(this.onDown))
		{
			return this.onDown;
		}
		if (this.constraint == UIKeyNavigation.Constraint.Horizontal || this.constraint == UIKeyNavigation.Constraint.Explicit)
		{
			return null;
		}
		return this.Get(Vector3.get_down(), false);
	}

	protected GameObject Get(Vector3 myDir, bool horizontal)
	{
		Transform transform = base.get_transform();
		myDir = transform.TransformDirection(myDir);
		Vector3 center = UIKeyNavigation.GetCenter(base.get_gameObject());
		float num = 3.40282347E+38f;
		GameObject result = null;
		for (int i = 0; i < UIKeyNavigation.list.size; i++)
		{
			UIKeyNavigation uIKeyNavigation = UIKeyNavigation.list[i];
			if (!(uIKeyNavigation == this))
			{
				UIButton component = uIKeyNavigation.GetComponent<UIButton>();
				if (!(component != null) || component.isEnabled)
				{
					Vector3 vector = UIKeyNavigation.GetCenter(uIKeyNavigation.get_gameObject()) - center;
					float num2 = Vector3.Dot(myDir, vector.get_normalized());
					if (num2 >= 0.707f)
					{
						vector = transform.InverseTransformDirection(vector);
						if (horizontal)
						{
							vector.y *= 2f;
						}
						else
						{
							vector.x *= 2f;
						}
						float sqrMagnitude = vector.get_sqrMagnitude();
						if (sqrMagnitude <= num)
						{
							result = uIKeyNavigation.get_gameObject();
							num = sqrMagnitude;
						}
					}
				}
			}
		}
		return result;
	}

	protected static Vector3 GetCenter(GameObject go)
	{
		UIWidget component = go.GetComponent<UIWidget>();
		UICamera uICamera = UICamera.FindCameraForLayer(go.get_layer());
		if (uICamera != null)
		{
			Vector3 vector = go.get_transform().get_position();
			if (component != null)
			{
				Vector3[] worldCorners = component.worldCorners;
				vector = (worldCorners[0] + worldCorners[2]) * 0.5f;
			}
			vector = uICamera.cachedCamera.WorldToScreenPoint(vector);
			vector.z = 0f;
			return vector;
		}
		if (component != null)
		{
			Vector3[] worldCorners2 = component.worldCorners;
			return (worldCorners2[0] + worldCorners2[2]) * 0.5f;
		}
		return go.get_transform().get_position();
	}

	protected virtual void OnKey(KeyCode key)
	{
		if (!NGUITools.GetActive(this))
		{
			return;
		}
		GameObject gameObject = null;
		switch (key)
		{
		case 273:
			gameObject = this.GetUp();
			break;
		case 274:
			gameObject = this.GetDown();
			break;
		case 275:
			gameObject = this.GetRight();
			break;
		case 276:
			gameObject = this.GetLeft();
			break;
		default:
			if (key == 9)
			{
				if (Input.GetKey(304) || Input.GetKey(303))
				{
					gameObject = this.GetLeft();
					if (gameObject == null)
					{
						gameObject = this.GetUp();
					}
					if (gameObject == null)
					{
						gameObject = this.GetDown();
					}
					if (gameObject == null)
					{
						gameObject = this.GetRight();
					}
				}
				else
				{
					gameObject = this.GetRight();
					if (gameObject == null)
					{
						gameObject = this.GetDown();
					}
					if (gameObject == null)
					{
						gameObject = this.GetUp();
					}
					if (gameObject == null)
					{
						gameObject = this.GetLeft();
					}
				}
			}
			break;
		}
		if (gameObject != null)
		{
			UICamera.selectedObject = gameObject;
		}
	}

	protected virtual void OnClick()
	{
		if (NGUITools.GetActive(this) && NGUITools.GetActive(this.onClick))
		{
			UICamera.selectedObject = this.onClick;
		}
	}
}
