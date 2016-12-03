using System;
using UnityEngine;

public class CommonPopupDialog : MonoBehaviour
{
	protected static CommonPopupDialog instance;

	[SerializeField]
	private CommonPopupDialogMessage Message;

	public static CommonPopupDialog Instance
	{
		get
		{
			if (CommonPopupDialog.instance == null)
			{
				CommonPopupDialog.instance = (CommonPopupDialog)Object.FindObjectOfType(typeof(CommonPopupDialog));
				if (CommonPopupDialog.instance == null)
				{
					return null;
				}
			}
			return CommonPopupDialog.instance;
		}
		set
		{
			CommonPopupDialog.instance = value;
		}
	}

	private void Awake()
	{
		CommonPopupDialog.Instance = this;
		this.Message.GetComponent<UIPanel>().clipping = UIDrawCall.Clipping.None;
		this.Message.get_transform().FindChild("p_Background").GetComponent<UITexture>().mainTexture = (Resources.Load("Textures/Common/AlertMes/txtBG_white") as Texture2D);
	}

	private void OnDestroy()
	{
		Mem.Del<CommonPopupDialogMessage>(ref this.Message);
		Mem.Del<CommonPopupDialog>(ref CommonPopupDialog.instance);
	}

	public void StartPopup(string mes)
	{
		this.Message.StartPopup(mes, 0, CommonPopupDialogMessage.PlayType.Long);
	}

	public void StartPopup(string mes, int mesNo, CommonPopupDialogMessage.PlayType type)
	{
		this.Message.StartPopup(mes, mesNo, type);
	}
}
