using Common.Enum;
using local.models;
using System;
using UnityEngine;

namespace KCV
{
	public class BaseShipCard : MonoBehaviour
	{
		protected ShipModel _clsShipModel;

		public virtual bool Init(ShipModel ship, UITexture _texture)
		{
			if (ship == null)
			{
				return false;
			}
			this._clsShipModel = ship;
			int texNum = (!ship.IsDamaged()) ? 3 : 4;
			this._Load(ship.MstId, texNum, _texture);
			return true;
		}

		protected virtual void _Load(int shipID, int texNum, UITexture _texture)
		{
			_texture.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(shipID, texNum);
			_texture.height = (int)ResourceManager.SHIP_TEXTURE_SIZE.get_Item(texNum).y;
			_texture.width = (int)ResourceManager.SHIP_TEXTURE_SIZE.get_Item(texNum).x;
		}

		public virtual void UpdateStateIcon(UISprite _stateIcon)
		{
			_stateIcon.get_transform().localPositionX(67f);
			if (this._clsShipModel.IsInRepair())
			{
				_stateIcon.alpha = 1f;
				_stateIcon.spriteName = "icon-s_syufuku";
			}
			else if (this._clsShipModel.IsBling())
			{
				_stateIcon.alpha = 1f;
				_stateIcon.get_transform().localPositionX(88f);
				_stateIcon.spriteName = "icon-s_kaiko";
			}
			else if (this._clsShipModel.IsInMission())
			{
				_stateIcon.alpha = 1f;
				_stateIcon.spriteName = "icon-s_ensei";
			}
			else if (this._clsShipModel.DamageStatus == DamageState.Taiha)
			{
				_stateIcon.alpha = 1f;
				_stateIcon.spriteName = "icon-s_taiha";
			}
			else if (this._clsShipModel.DamageStatus == DamageState.Tyuuha)
			{
				_stateIcon.alpha = 1f;
				_stateIcon.spriteName = "icon-s_chuha";
			}
			else if (this._clsShipModel.DamageStatus == DamageState.Shouha)
			{
				_stateIcon.alpha = 1f;
				_stateIcon.spriteName = "icon-s_shoha";
			}
			else
			{
				_stateIcon.alpha = 0f;
			}
		}

		public virtual void UpdateFatigue(FatigueState state, UISprite _fatigueIcon, UISprite _fatigueMask)
		{
			switch (state)
			{
			case FatigueState.Exaltation:
				_fatigueMask.alpha = 0f;
				_fatigueIcon.alpha = 0f;
				break;
			case FatigueState.Normal:
				_fatigueMask.alpha = 0f;
				_fatigueIcon.alpha = 0f;
				break;
			case FatigueState.Light:
				_fatigueMask.spriteName = "card-s_fatigue_01";
				_fatigueIcon.spriteName = "icon-s_fatigue1";
				_fatigueMask.alpha = 1f;
				_fatigueIcon.alpha = 1f;
				break;
			case FatigueState.Distress:
				_fatigueMask.spriteName = "card-s_fatigue_02";
				_fatigueIcon.spriteName = "icon-s_fatigue2";
				_fatigueMask.alpha = 1f;
				_fatigueIcon.alpha = 1f;
				break;
			}
		}
	}
}
