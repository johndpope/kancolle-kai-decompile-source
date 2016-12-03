using local.models;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class Debug_CharacterChanger : MonoBehaviour
{
	public enum GetLocation
	{
		GetBoko,
		GetFace,
		GetSlotItemCategory,
		GetShipDisplayCenter,
		GetCutinSp1_InBattle,
		GetCutin_InBattle
	}

	private UITexture texture;

	[SerializeField]
	public string _filebase;

	public int MstID = 1;

	public bool isDamaged;

	[SerializeField]
	private Debug_CharacterChanger.GetLocation LocationType;

	[Button("changeNext", "changeNext", new object[]
	{

	})]
	public int button1;

	[Button("changePrev", "changePrev", new object[]
	{

	})]
	public int button2;

	[Button("screenShot", "screenShot", new object[]
	{

	})]
	public int button3;

	private void Awake()
	{
		this.texture = base.GetComponent<UITexture>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(112) || Input.GetKeyDown(111))
		{
			int num = (!Input.GetKeyDown(112)) ? -1 : 1;
			this.MstID += num;
			int texNum = (!this.isDamaged) ? 9 : 10;
			for (int i = 0; i < 100; i++)
			{
				this.texture.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(this.MstID, texNum);
				if (this.texture.mainTexture != null)
				{
					break;
				}
				this.MstID += num;
				if (i == 99)
				{
					this.MstID = 1;
				}
			}
			Vector3 localScale = base.get_transform().get_localScale();
			this.texture.MakePixelPerfect();
			base.get_transform().set_localScale(localScale);
			ShipModelMst model = new ShipModelMst(this.MstID);
			this.draw(model);
			this.texture.Update();
		}
	}

	private void changeNext()
	{
		int num = 1;
		this.MstID += num;
		int texNum = (!this.isDamaged) ? 9 : 10;
		for (int i = 0; i < 100; i++)
		{
			this.texture.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(this.MstID, texNum);
			if (this.texture.mainTexture != null)
			{
				break;
			}
			this.MstID += num;
			if (i == 99)
			{
				this.MstID = 1;
			}
		}
		this.texture.MakePixelPerfect();
		ShipModelMst model = new ShipModelMst(this.MstID);
		this.draw(model);
		this.texture.Update();
	}

	private void changePrev()
	{
		int num = -1;
		this.MstID += num;
		int texNum = (!this.isDamaged) ? 9 : 10;
		for (int i = 0; i < 100; i++)
		{
			this.texture.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(this.MstID, texNum);
			if (this.texture.mainTexture != null)
			{
				break;
			}
			this.MstID += num;
			if (i == 99)
			{
				this.MstID = 1;
			}
		}
		this.texture.MakePixelPerfect();
		ShipModelMst model = new ShipModelMst(this.MstID);
		this.draw(model);
		this.texture.Update();
	}

	private void draw(ShipModelMst model)
	{
		switch (this.LocationType)
		{
		case Debug_CharacterChanger.GetLocation.GetFace:
			this.texture.get_transform().localPositionX((float)model.Offsets.GetFace(this.isDamaged).x);
			this.texture.get_transform().localPositionY((float)model.Offsets.GetFace(this.isDamaged).y);
			return;
		case Debug_CharacterChanger.GetLocation.GetSlotItemCategory:
			this.texture.get_transform().localPositionX((float)model.Offsets.GetSlotItemCategory(this.isDamaged).x);
			this.texture.get_transform().localPositionY((float)model.Offsets.GetSlotItemCategory(this.isDamaged).y);
			return;
		case Debug_CharacterChanger.GetLocation.GetShipDisplayCenter:
			this.texture.get_transform().localPositionX((float)model.Offsets.GetShipDisplayCenter(this.isDamaged).x);
			this.texture.get_transform().localPositionY((float)model.Offsets.GetShipDisplayCenter(this.isDamaged).y);
			return;
		case Debug_CharacterChanger.GetLocation.GetCutinSp1_InBattle:
			this.texture.get_transform().localPositionX((float)model.Offsets.GetCutinSp1_InBattle(this.isDamaged).x);
			this.texture.get_transform().localPositionY((float)model.Offsets.GetCutinSp1_InBattle(this.isDamaged).y);
			return;
		case Debug_CharacterChanger.GetLocation.GetCutin_InBattle:
			this.texture.get_transform().localPositionX((float)model.Offsets.GetCutin_InBattle(this.isDamaged).x);
			this.texture.get_transform().localPositionY((float)model.Offsets.GetCutin_InBattle(this.isDamaged).y);
			return;
		}
		this.texture.get_transform().localPositionX((float)model.Offsets.GetBoko(this.isDamaged).x);
		this.texture.get_transform().localPositionY((float)model.Offsets.GetBoko(this.isDamaged).y);
	}

	private void screenShot()
	{
		Debug.Log("called_screenShot");
		base.StartCoroutine(this.SSSS());
	}

	[DebuggerHidden]
	public IEnumerator SSSS()
	{
		Debug_CharacterChanger.<SSSS>c__Iterator1C7 <SSSS>c__Iterator1C = new Debug_CharacterChanger.<SSSS>c__Iterator1C7();
		<SSSS>c__Iterator1C.<>f__this = this;
		return <SSSS>c__Iterator1C;
	}

	private void DoSS(int MstID)
	{
		string text;
		if (this.isDamaged)
		{
			text = string.Format(this._filebase + "_D{0:0000}.png", MstID);
		}
		else
		{
			text = string.Format(this._filebase + "_N{0:0000}.png", MstID);
		}
		Application.CaptureScreenshot(text);
	}
}
