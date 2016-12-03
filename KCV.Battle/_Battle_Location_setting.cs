using System;
using UnityEngine;

namespace KCV.Battle
{
	public class _Battle_Location_setting : MonoBehaviour
	{
		private int ShipState;

		private int NowShip;

		private bool isDamaged;

		private GameObject TargetObject;

		private mst_shipgraphbattle ShipOffset;

		private float offsetX;

		private float offsetY;

		private UIBattleShip to;

		private void Start()
		{
			this.NowShip = 140;
			this.isDamaged = false;
			this.ShipState = 9;
		}

		private void _draw()
		{
			this.TargetObject = GameObject.Find("/BattleTaskManager/Stage/BattleField/FriendFleetAnchor/吹雪/ShipTexture/Object3D");
			this.to = GameObject.Find("/BattleTaskManager/Stage/BattleField/FriendFleetAnchor/吹雪").GetComponent<UIBattleShip>();
			this.ShipOffset = Resources.Load<mst_shipgraphbattle>("Data/mst_shipgraphbattle");
			if (!this.isDamaged)
			{
				this.offsetX = (float)this.ShipOffset.param.get_Item(this.NowShip).foot_x;
				this.offsetY = (float)this.ShipOffset.param.get_Item(this.NowShip).foot_y;
				this.ShipState = 9;
			}
			else
			{
				this.offsetX = (float)this.ShipOffset.param.get_Item(this.NowShip).foot_d_x;
				this.offsetY = (float)this.ShipOffset.param.get_Item(this.NowShip).foot_d_y;
				this.ShipState = 10;
			}
			this.TargetObject.get_transform().set_localPosition(new Vector3(this.offsetX, this.offsetY, 0f));
			Debug.Log(string.Concat(new object[]
			{
				"ShipNo: ",
				this.NowShip,
				" /ShipState =",
				this.ShipState,
				"  offset(x,y)=",
				this.offsetX,
				",",
				this.offsetY
			}));
			this.to.object3D.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(this.NowShip, this.ShipState);
			if (this.to.object3D.mainTexture == null)
			{
				Debug.Log("Null Texture.");
			}
			this.to.object3D.MakePixelPerfect();
		}

		private void Update()
		{
			if (Input.GetKeyDown("3"))
			{
				if (this.isDamaged)
				{
					this.isDamaged = false;
				}
				else
				{
					this.isDamaged = true;
				}
				this._draw();
			}
			if (Input.GetKeyDown("1"))
			{
				if (this.NowShip != 1)
				{
					this.NowShip--;
				}
				this.isDamaged = false;
				this._draw();
			}
			if (Input.GetKeyDown("2"))
			{
				if (this.NowShip != 999)
				{
					this.NowShip++;
				}
				this.isDamaged = false;
				this._draw();
			}
		}
	}
}
