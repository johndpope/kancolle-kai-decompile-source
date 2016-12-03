using local.models;
using System;
using UnityEngine;

namespace KCV.Production
{
	public class Test_BattleResultsReceiveShip : MonoBehaviour
	{
		private ProdReceiveShip _prodReceievShip;

		private KeyControl _clsInput;

		private void Awake()
		{
			this._clsInput = new KeyControl(0, 0, 0.4f, 0.1f);
		}

		private void Update()
		{
			this._clsInput.Update();
			if (Input.GetKeyDown(98))
			{
				Reward_Ship rewardShip = new Reward_Ship(131);
				this._prodReceievShip = ProdReceiveShip.Instantiate(PrefabFile.Load<ProdReceiveShip>(PrefabFileInfos.CommonProdReceiveShip), base.get_transform().get_parent(), rewardShip, 1, this._clsInput);
				this._prodReceievShip.Play(delegate
				{
					Debug.Log("艦娘ドロップ演出終了");
				});
			}
		}

		private void _onFinished()
		{
		}
	}
}
