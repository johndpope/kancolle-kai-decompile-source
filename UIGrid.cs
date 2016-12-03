using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Grid")]
public class UIGrid : UIWidgetContainer
{
	public enum Arrangement
	{
		Horizontal,
		Vertical,
		CellSnap
	}

	public enum Sorting
	{
		None,
		Alphabetic,
		Horizontal,
		Vertical,
		Custom
	}

	public delegate void OnReposition();

	public UIGrid.Arrangement arrangement;

	public UIGrid.Sorting sorting;

	public UIWidget.Pivot pivot;

	public int maxPerLine;

	public float cellWidth = 200f;

	public float cellHeight = 200f;

	public bool animateSmoothly;

	public bool hideInactive;

	public bool keepWithinPanel;

	public UIGrid.OnReposition onReposition;

	public Comparison<Transform> onCustomSort;

	[HideInInspector, SerializeField]
	private bool sorted;

	protected bool mReposition;

	protected UIPanel mPanel;

	protected bool mInitDone;

	public bool repositionNow
	{
		set
		{
			if (value)
			{
				this.mReposition = true;
				base.set_enabled(true);
			}
		}
	}

	public List<Transform> GetChildList()
	{
		Transform transform = base.get_transform();
		List<Transform> list = new List<Transform>();
		for (int i = 0; i < transform.get_childCount(); i++)
		{
			Transform child = transform.GetChild(i);
			if (!this.hideInactive || (child && NGUITools.GetActive(child.get_gameObject())))
			{
				list.Add(child);
			}
		}
		if (this.sorting != UIGrid.Sorting.None && this.arrangement != UIGrid.Arrangement.CellSnap)
		{
			if (this.sorting == UIGrid.Sorting.Alphabetic)
			{
				list.Sort(new Comparison<Transform>(UIGrid.SortByName));
			}
			else if (this.sorting == UIGrid.Sorting.Horizontal)
			{
				list.Sort(new Comparison<Transform>(UIGrid.SortHorizontal));
			}
			else if (this.sorting == UIGrid.Sorting.Vertical)
			{
				list.Sort(new Comparison<Transform>(UIGrid.SortVertical));
			}
			else if (this.onCustomSort != null)
			{
				list.Sort(this.onCustomSort);
			}
			else
			{
				this.Sort(list);
			}
		}
		return list;
	}

	public Transform GetChild(int index)
	{
		List<Transform> childList = this.GetChildList();
		return (index >= childList.get_Count()) ? null : childList.get_Item(index);
	}

	public int GetIndex(Transform trans)
	{
		return this.GetChildList().IndexOf(trans);
	}

	public void AddChild(Transform trans)
	{
		this.AddChild(trans, true);
	}

	public void AddChild(Transform trans, bool sort)
	{
		if (trans != null)
		{
			trans.set_parent(base.get_transform());
			this.ResetPosition(this.GetChildList());
		}
	}

	public bool RemoveChild(Transform t)
	{
		List<Transform> childList = this.GetChildList();
		if (childList.Remove(t))
		{
			this.ResetPosition(childList);
			return true;
		}
		return false;
	}

	protected virtual void Init()
	{
		this.mInitDone = true;
		this.mPanel = NGUITools.FindInParents<UIPanel>(base.get_gameObject());
	}

	protected virtual void Start()
	{
		if (!this.mInitDone)
		{
			this.Init();
		}
		bool flag = this.animateSmoothly;
		this.animateSmoothly = false;
		this.Reposition();
		this.animateSmoothly = flag;
		base.set_enabled(false);
	}

	protected virtual void Update()
	{
		this.Reposition();
		base.set_enabled(false);
	}

	private void OnValidate()
	{
		if (!Application.get_isPlaying() && NGUITools.GetActive(this))
		{
			this.Reposition();
		}
	}

	public static int SortByName(Transform a, Transform b)
	{
		return string.Compare(a.get_name(), b.get_name());
	}

	public static int SortHorizontal(Transform a, Transform b)
	{
		return a.get_localPosition().x.CompareTo(b.get_localPosition().x);
	}

	public static int SortVertical(Transform a, Transform b)
	{
		return b.get_localPosition().y.CompareTo(a.get_localPosition().y);
	}

	protected virtual void Sort(List<Transform> list)
	{
	}

	[ContextMenu("Execute")]
	public virtual void Reposition()
	{
		if (Application.get_isPlaying() && !this.mInitDone && NGUITools.GetActive(base.get_gameObject()))
		{
			this.Init();
		}
		if (this.sorted)
		{
			this.sorted = false;
			if (this.sorting == UIGrid.Sorting.None)
			{
				this.sorting = UIGrid.Sorting.Alphabetic;
			}
			NGUITools.SetDirty(this);
		}
		List<Transform> childList = this.GetChildList();
		this.ResetPosition(childList);
		if (this.keepWithinPanel)
		{
			this.ConstrainWithinPanel();
		}
		if (this.onReposition != null)
		{
			this.onReposition();
		}
	}

	public void ConstrainWithinPanel()
	{
		if (this.mPanel != null)
		{
			this.mPanel.ConstrainTargetToBounds(base.get_transform(), true);
			UIScrollView component = this.mPanel.GetComponent<UIScrollView>();
			if (component != null)
			{
				component.UpdateScrollbars(true);
			}
		}
	}

	protected virtual void ResetPosition(List<Transform> list)
	{
		this.mReposition = false;
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		Transform transform = base.get_transform();
		int i = 0;
		int count = list.get_Count();
		while (i < count)
		{
			Transform transform2 = list.get_Item(i);
			Vector3 vector = transform2.get_localPosition();
			float z = vector.z;
			if (this.arrangement == UIGrid.Arrangement.CellSnap)
			{
				if (this.cellWidth > 0f)
				{
					vector.x = Mathf.Round(vector.x / this.cellWidth) * this.cellWidth;
				}
				if (this.cellHeight > 0f)
				{
					vector.y = Mathf.Round(vector.y / this.cellHeight) * this.cellHeight;
				}
			}
			else
			{
				vector = ((this.arrangement != UIGrid.Arrangement.Horizontal) ? new Vector3(this.cellWidth * (float)num2, -this.cellHeight * (float)num, z) : new Vector3(this.cellWidth * (float)num, -this.cellHeight * (float)num2, z));
			}
			if (this.animateSmoothly && Application.get_isPlaying())
			{
				SpringPosition springPosition = SpringPosition.Begin(transform2.get_gameObject(), vector, 15f);
				springPosition.updateScrollView = true;
				springPosition.ignoreTimeScale = true;
			}
			else
			{
				transform2.set_localPosition(vector);
			}
			num3 = Mathf.Max(num3, num);
			num4 = Mathf.Max(num4, num2);
			if (++num >= this.maxPerLine && this.maxPerLine > 0)
			{
				num = 0;
				num2++;
			}
			i++;
		}
		if (this.pivot != UIWidget.Pivot.TopLeft)
		{
			Vector2 pivotOffset = NGUIMath.GetPivotOffset(this.pivot);
			float num5;
			float num6;
			if (this.arrangement == UIGrid.Arrangement.Horizontal)
			{
				num5 = Mathf.Lerp(0f, (float)num3 * this.cellWidth, pivotOffset.x);
				num6 = Mathf.Lerp((float)(-(float)num4) * this.cellHeight, 0f, pivotOffset.y);
			}
			else
			{
				num5 = Mathf.Lerp(0f, (float)num4 * this.cellWidth, pivotOffset.x);
				num6 = Mathf.Lerp((float)(-(float)num3) * this.cellHeight, 0f, pivotOffset.y);
			}
			for (int j = 0; j < transform.get_childCount(); j++)
			{
				Transform child = transform.GetChild(j);
				SpringPosition component = child.GetComponent<SpringPosition>();
				if (component != null)
				{
					SpringPosition expr_24F_cp_0 = component;
					expr_24F_cp_0.target.x = expr_24F_cp_0.target.x - num5;
					SpringPosition expr_264_cp_0 = component;
					expr_264_cp_0.target.y = expr_264_cp_0.target.y - num6;
				}
				else
				{
					Vector3 localPosition = child.get_localPosition();
					localPosition.x -= num5;
					localPosition.y -= num6;
					child.set_localPosition(localPosition);
				}
			}
		}
	}
}
