using local.models;
using local.utils;
using Server_Controllers;
using Server_Models;
using System;

namespace local.managers
{
	public class RemodelGradeUpManager : ManagerBase
	{
		private ShipModel _ship;

		private Api_req_Kaisou.RemodelingChkResult _chkResult;

		private int _required_design_specifications;

		private int _design_specifications;

		public ShipModel TargetShip
		{
			get
			{
				return this._ship;
			}
		}

		public int DesignSpecificationsForGradeup
		{
			get
			{
				return this._required_design_specifications;
			}
		}

		public int DesignSpecifications
		{
			get
			{
				return this._design_specifications;
			}
		}

		public bool GradeupBtnEnabled
		{
			get
			{
				return this._chkResult == Api_req_Kaisou.RemodelingChkResult.OK;
			}
		}

		public RemodelGradeUpManager(ShipModel ship)
		{
			this._ship = ship;
			this._IsValid();
		}

		public bool GradeUp()
		{
			if (this.GradeupBtnEnabled)
			{
				Api_Result<Mem_ship> api_Result = new Api_req_Kaisou().Remodeling(this._ship.MemId, this._required_design_specifications);
				if (api_Result.state == Api_Result_State.Success)
				{
					this._ship.SetMemData(api_Result.data);
					ShipModel ship = base.UserInfo.GetShip(this._ship.MemId);
					if (ship != this._ship)
					{
						ship.SetMemData(api_Result.data);
					}
					return true;
				}
			}
			return false;
		}

		private bool _IsValid()
		{
			this._chkResult = new Api_req_Kaisou().ValidRemodeling(this._ship.MemId, out this._required_design_specifications);
			this._design_specifications = new UseitemUtil().GetCount(58);
			return this.GradeupBtnEnabled;
		}

		public override string ToString()
		{
			string text = "== 改造マネージャ ==\n";
			text += string.Format("対象艦: {0} Lv{1}(必要レベル:{2}) 所持弾薬:{3}(必要弾薬:{4}) 所持鋼材:{5}(必要鋼材:{6}) 所持開発資材:{7}(必要開発資材:{8}) \n", new object[]
			{
				this.TargetShip.ShortName,
				this.TargetShip.Level,
				this.TargetShip.AfterLevel,
				ManagerBase._materialModel.Ammo,
				this.TargetShip.AfterAmmo,
				ManagerBase._materialModel.Steel,
				this.TargetShip.AfterSteel,
				ManagerBase._materialModel.Devkit,
				this.TargetShip.AfterDevkit
			});
			text += string.Format("所持改装設計書:{0}(必要改装設計書:{1})\n", this.DesignSpecifications, this.DesignSpecificationsForGradeup);
			text += string.Format("改装開始可能:{0} 改造チェック:{1}\n", this.GradeupBtnEnabled, this._chkResult);
			return text + "== ＝＝＝＝＝＝＝ ==";
		}
	}
}
