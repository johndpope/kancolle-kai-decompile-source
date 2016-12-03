using KCV.Battle.Utils;
using local.managers;
using local.models;
using System;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdAerialTouchPlane : MonoBehaviour
	{
		[SerializeField]
		private GameObject _uiGameObjF;

		[SerializeField]
		private GameObject _uiGameObjE;

		[SerializeField]
		private UITexture _uiAircraftF;

		[SerializeField]
		private UITexture _uiAircraftE;

		private Action _actCallback;

		private int _shipNum;

		public void Init(SlotitemModel_Battle slotModelF, SlotitemModel_Battle slotModelE)
		{
			base.get_gameObject().SetActive(true);
			if (this._uiGameObjF == null)
			{
				this._uiGameObjF = base.get_transform().FindChild("AircraftF").get_gameObject();
			}
			if (this._uiGameObjE == null)
			{
				this._uiGameObjE = base.get_transform().FindChild("AircraftE").get_gameObject();
			}
			if (this._uiAircraftF == null)
			{
				this._uiAircraftF = this._uiGameObjF.get_transform().FindChild("Swing/Aircraft").GetComponent<UITexture>();
			}
			if (this._uiAircraftE == null)
			{
				this._uiAircraftE = this._uiGameObjE.get_transform().FindChild("Swing/Aircraft").GetComponent<UITexture>();
			}
			if (slotModelF != null)
			{
				this._uiGameObjF.SetActive(true);
				this._uiAircraftF.flip = UIBasicSprite.Flip.Nothing;
				this._uiAircraftF.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.SlotItemTexture.Load(slotModelF.MstId, 6);
			}
			else
			{
				this._uiGameObjF.SetActive(false);
			}
			if (slotModelE != null)
			{
				this._uiGameObjE.SetActive(true);
				this._uiAircraftE.flip = UIBasicSprite.Flip.Horizontally;
				if (BattleTaskManager.GetBattleManager() is PracticeBattleManager)
				{
					this._uiAircraftE.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.SlotItemTexture.Load(slotModelE.MstId, 6);
					this._uiAircraftE.MakePixelPerfect();
				}
				else
				{
					this._uiAircraftE.mainTexture = SlotItemUtils.LoadTexture(slotModelE);
					this._uiAircraftE.MakePixelPerfect();
					this._uiAircraftE.get_transform().set_localScale((slotModelE.MstId > 500) ? new Vector3(0.7f, 0.7f, 0.7f) : new Vector3(0.8f, 0.8f, 0.8f));
					AircraftOffsetInfo aircraftOffsetInfo = SlotItemUtils.GetAircraftOffsetInfo(slotModelE.MstId);
					this._uiAircraftE.flip = UIBasicSprite.Flip.Nothing;
				}
			}
			else
			{
				this._uiGameObjE.SetActive(false);
			}
		}

		private void OnDestroy()
		{
			Mem.Del<GameObject>(ref this._uiGameObjF);
			Mem.Del<GameObject>(ref this._uiGameObjE);
			Mem.Del<UITexture>(ref this._uiAircraftF);
			Mem.Del<UITexture>(ref this._uiAircraftE);
			Mem.Del<Action>(ref this._actCallback);
		}

		public void Hide()
		{
			base.get_gameObject().SetActive(false);
		}

		private void _onFinishedInjection()
		{
		}

		public static ProdAerialTouchPlane Instantiate(ProdAerialTouchPlane _aerial, Transform fromParent)
		{
			ProdAerialTouchPlane prodAerialTouchPlane = Object.Instantiate<ProdAerialTouchPlane>(_aerial);
			prodAerialTouchPlane.get_transform().set_parent(fromParent);
			prodAerialTouchPlane.get_transform().set_localScale(Vector3.get_one());
			prodAerialTouchPlane.get_transform().set_position(Vector3.get_zero());
			return prodAerialTouchPlane;
		}
	}
}
