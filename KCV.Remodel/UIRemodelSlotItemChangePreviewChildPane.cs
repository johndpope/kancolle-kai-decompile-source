using KCV.Scene.Port;
using local.models;
using System;
using UnityEngine;

namespace KCV.Remodel
{
	public class UIRemodelSlotItemChangePreviewChildPane : MonoBehaviour
	{
		private Vector3 showPos = new Vector3(36f, 193.5f);

		private Vector3 hidePos = new Vector3(550f, 193.5f);

		[SerializeField]
		private UIRemodelSlotItemChangePreviewInParameter mPrefab_UIRemodelSlotItemChangePreviewInParameter;

		[SerializeField]
		private UITable paramTable;

		[SerializeField]
		private UILabel weaponName;

		[SerializeField]
		private UISprite weaponTypeIcon;

		[SerializeField]
		private UITexture weaponImage;

		[SerializeField]
		private UISprite mLock_Icon;

		public UITexture BackGround;

		private string[] paramLavels = new string[]
		{
			"装甲",
			"火力",
			"雷装",
			"爆撃",
			"対空",
			"対潜",
			"命中",
			"回避",
			"索敵",
			"射程"
		};

		public void Init4Upper(SlotitemModel dstSlotItem)
		{
			this.InitViews(dstSlotItem);
			if (dstSlotItem != null)
			{
				this.processWithoutComparison(dstSlotItem, 0);
			}
		}

		public void Init4Lower(SlotitemModel dstSlotItem, SlotitemModel srcSlotItem)
		{
			this.InitViews(dstSlotItem);
			if (srcSlotItem == null)
			{
				this.processWithoutComparison(dstSlotItem, 0);
			}
			else
			{
				this.processWithComparison(dstSlotItem, srcSlotItem);
			}
		}

		public Texture GetSlotItemTexture()
		{
			if (this.weaponImage != null && this.weaponImage.mainTexture != null)
			{
				return this.weaponImage.mainTexture;
			}
			return null;
		}

		public void InitViews(SlotitemModel dstSlotItem)
		{
			if (dstSlotItem == null)
			{
				this.SwitchActive(false);
				this.weaponName.text = "\u3000-";
				this.mLock_Icon.get_transform().set_localScale(Vector3.get_zero());
			}
			else
			{
				this.SwitchActive(true);
				this.weaponName.text = dstSlotItem.Name;
				this.weaponTypeIcon.spriteName = string.Format("icon_slot{0}", dstSlotItem.Type4);
				this.UnloadSlotItemTexture(false);
				this.weaponImage.mainTexture = this.BgTextureResourceLoad(dstSlotItem.MstId);
				if (dstSlotItem.IsLocked())
				{
					this.mLock_Icon.get_transform().set_localScale(Vector3.get_one());
				}
				else
				{
					this.mLock_Icon.get_transform().set_localScale(Vector3.get_zero());
				}
				this.paramTable.GetChildList().ForEach(delegate(Transform e)
				{
					NGUITools.Destroy(e);
				});
			}
		}

		private void SwitchActive(bool active)
		{
			this.weaponTypeIcon.SetActive(active);
			this.weaponImage.SetActive(active);
			this.paramTable.SetActive(active);
		}

		private UIRemodelSlotItemChangePreviewInParameter CreateParamObj()
		{
			return Util.Instantiate(this.mPrefab_UIRemodelSlotItemChangePreviewInParameter.get_gameObject(), this.paramTable.get_gameObject(), false, false).GetComponent<UIRemodelSlotItemChangePreviewInParameter>();
		}

		private void processWithoutComparison(SlotitemModel dstSlotItem, int fixedSabun)
		{
			int num = 0;
			int[] array = this.createSlotItemValues(dstSlotItem);
			for (int i = 0; i < this.paramLavels.Length; i++)
			{
				if (array[i] != 0)
				{
					this.CreateParamObj().Init(this.paramLavels[i], (array[i] <= 0) ? (-array[i]) : array[i], array[i]);
					num++;
				}
			}
			if (num == 0)
			{
				this.CreateParamObj().Init(string.Empty, 0, 0);
			}
			this.paramTable.Reposition();
		}

		private void processWithComparison(SlotitemModel dstSlotItem, SlotitemModel srcSlotItem)
		{
			int num = 0;
			int[] array = this.createSlotItemValues(dstSlotItem);
			int[] array2 = this.createSlotItemValues(srcSlotItem);
			for (int i = 0; i < this.paramLavels.Length; i++)
			{
				if (array[i] != array2[i])
				{
					this.CreateParamObj().Init(this.paramLavels[i], (array[i] <= array2[i]) ? (array2[i] - array[i]) : (array[i] - array2[i]), array[i] - array2[i]);
					num++;
				}
			}
			if (num == 0)
			{
				this.CreateParamObj().Init(string.Empty, 0, 0);
			}
			this.paramTable.Reposition();
		}

		private int[] createSlotItemValues(SlotitemModel slotItem)
		{
			return new int[]
			{
				slotItem.Soukou,
				slotItem.Hougeki,
				slotItem.Raigeki,
				slotItem.Bakugeki,
				slotItem.Taikuu,
				slotItem.Taisen,
				slotItem.HouMeityu,
				slotItem.Kaihi,
				slotItem.Sakuteki,
				slotItem.Syatei
			};
		}

		private Texture2D BgTextureResourceLoad(int masterId)
		{
			return Resources.Load(string.Format("Textures/SlotItems/{0}/2", masterId)) as Texture2D;
		}

		internal void Release()
		{
			this.mPrefab_UIRemodelSlotItemChangePreviewInParameter = null;
			NGUITools.Destroy(this.paramTable);
			this.paramTable = null;
			NGUITools.Destroy(this.weaponName);
			this.weaponName = null;
			if (this.weaponTypeIcon != null)
			{
				this.weaponTypeIcon.Clear();
				NGUITools.Destroy(this.weaponTypeIcon);
			}
			this.weaponTypeIcon = null;
			if (this.weaponImage != null)
			{
				this.weaponImage.mainTexture = null;
				NGUITools.Destroy(this.weaponImage);
			}
			this.weaponImage = null;
			if (this.mLock_Icon != null)
			{
				this.mLock_Icon.Clear();
			}
			this.mLock_Icon = null;
			this.paramLavels = null;
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref this.BackGround, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.weaponName);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.weaponTypeIcon);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.weaponImage, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLock_Icon);
			this.mPrefab_UIRemodelSlotItemChangePreviewInParameter = null;
			this.paramTable = null;
		}

		internal void UnloadSlotItemTexture(bool unloadTexture = false)
		{
			if (this.weaponImage != null)
			{
				if (this.weaponImage.mainTexture != null && unloadTexture)
				{
					Resources.UnloadAsset(this.weaponImage.mainTexture);
				}
				this.weaponImage.mainTexture = null;
			}
		}
	}
}
