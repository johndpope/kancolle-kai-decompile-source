using local.models;
using System;
using UnityEngine;

namespace KCV.Remodel
{
	public class Test_PortUpgradesModernizeShip : MonoBehaviour
	{
		private GameObject go;

		public void Awake()
		{
			this.go = (Object.Instantiate(Resources.Load("Prefabs/ModernizeShip")) as GameObject);
			try
			{
				this.go.set_name("ModernizeShip");
				this.go.get_transform().set_parent(base.get_transform().get_parent());
				this.go.get_transform().set_localScale(new Vector3(1f, 1f, 1f));
				this.go.GetComponent<PortUpgradesModernizeShipManager>().set_enabled(true);
				bool[,] expr_81 = new bool[,]
				{
					{
						true,
						true,
						false,
						false
					},
					{
						true,
						true,
						true,
						true
					},
					{
						false,
						false,
						true,
						true
					},
					{
						true,
						false,
						true,
						true
					},
					{
						false,
						false,
						false,
						false
					}
				};
				this.go.GetComponent<PortUpgradesModernizeShipManager>().Initialize(new ShipModelMst(80), 5, true, false, 4);
			}
			catch (Exception)
			{
				this.go.AddComponent<PortUpgradesModernizeShipManager>();
				bool[,] expr_C1 = new bool[,]
				{
					{
						true,
						true,
						false,
						false
					},
					{
						true,
						true,
						true,
						true
					},
					{
						false,
						false,
						true,
						true
					},
					{
						true,
						false,
						true,
						true
					},
					{
						false,
						false,
						false,
						false
					}
				};
				this.go.GetComponent<PortUpgradesModernizeShipManager>().Initialize(new ShipModelMst(80), 5, true, false, 4);
			}
		}
	}
}
