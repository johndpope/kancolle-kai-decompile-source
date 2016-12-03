using System;
using UnityEngine;

public class WeaponSelectPanel : MonoBehaviour
{
	private GameObject ListItem;

	private void Awake()
	{
		this.ListItem = base.get_transform().FindChild("Item").get_gameObject();
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
