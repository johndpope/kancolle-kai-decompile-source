using local.managers;
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(UIPanel))]
public class UIRevampMaterialsInfo : MonoBehaviour
{
	private readonly float MOVE_TIME = 1f;

	[SerializeField]
	private Vector3 mVector3_ShowPosition;

	[SerializeField]
	private Vector3 mVector3_HidePosition;

	[SerializeField]
	private UILabel mLabel_DevKitValue;

	[SerializeField]
	private UILabel mLabel_RevampKitValue;

	private UIPanel mPanelThis;

	private void Awake()
	{
		this.mPanelThis = base.GetComponent<UIPanel>();
		this.mPanelThis.alpha = 0.01f;
	}

	public void Initialize(ManagerBase manager)
	{
		this.UpdateInfo(manager);
	}

	public void UpdateInfo(ManagerBase manager)
	{
		this.mLabel_DevKitValue.text = manager.Material.Devkit.ToString();
		this.mLabel_RevampKitValue.text = manager.Material.Revkit.ToString();
	}

	public void Show()
	{
		base.get_gameObject().get_transform().set_localPosition(this.mVector3_HidePosition);
		this.mPanelThis.alpha = 1f;
		this.Move(this.mVector3_ShowPosition);
	}

	public void Hide()
	{
		this.Move(this.mVector3_HidePosition);
	}

	private void Move(Vector3 moveTo)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("position", moveTo);
		hashtable.Add("time", this.MOVE_TIME);
		hashtable.Add("isLocal", true);
		hashtable.Add("easeType", iTween.EaseType.easeOutExpo);
		iTween.MoveTo(base.get_gameObject(), hashtable);
	}

	private void OnDestroy()
	{
		this.mLabel_DevKitValue = null;
		this.mLabel_RevampKitValue = null;
		this.mPanelThis = null;
	}
}
