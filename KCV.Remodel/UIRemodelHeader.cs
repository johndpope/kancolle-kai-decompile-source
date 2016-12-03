using DG.Tweening;
using KCV.Scene.Port;
using local.managers;
using local.models;
using System;
using UnityEngine;

namespace KCV.Remodel
{
	[SelectionBase]
	public class UIRemodelHeader : MonoBehaviour
	{
		[SerializeField]
		private UILabel titleLabel;

		[SerializeField]
		private UILabel ammoLabel;

		[SerializeField]
		private UILabel steelLabel;

		[SerializeField]
		private UILabel bauxLabel;

		[SerializeField]
		private Transform mTransform_TurnEndStamp;

		public void RefreshMaterial(ManagerBase manager)
		{
			int materialMaxNum = manager.UserInfo.GetMaterialMaxNum();
			if (materialMaxNum <= manager.Material.Ammo)
			{
				this.ammoLabel.color = Color.get_yellow();
			}
			else
			{
				this.ammoLabel.color = Color.get_white();
			}
			this.ammoLabel.text = manager.Material.Ammo.ToString();
			if (materialMaxNum <= manager.Material.Steel)
			{
				this.steelLabel.color = Color.get_yellow();
			}
			else
			{
				this.steelLabel.color = Color.get_white();
			}
			this.steelLabel.text = manager.Material.Steel.ToString();
			if (materialMaxNum <= manager.Material.Baux)
			{
				this.bauxLabel.color = Color.get_yellow();
			}
			else
			{
				this.bauxLabel.color = Color.get_white();
			}
			this.bauxLabel.text = manager.Material.Baux.ToString();
		}

		public void RefreshTitle(ScreenStatus status, DeckModel deck)
		{
			string text = string.Empty;
			switch (status)
			{
			case ScreenStatus.SELECT_DECK_SHIP:
				if (deck.Name == string.Empty)
				{
					text = "艦娘選択 - 第" + deck.Id + "艦隊 -";
				}
				else
				{
					text = "艦娘選択 -" + deck.Name + "-";
				}
				break;
			case ScreenStatus.SELECT_OTHER_SHIP:
				text = "艦娘選択 - その他 -";
				break;
			case ScreenStatus.SELECT_SETTING_MODE:
				text = "メニュー選択";
				break;
			case ScreenStatus.MODE_SOUBI_HENKOU:
			case ScreenStatus.MODE_SOUBI_HENKOU_TYPE_SELECT:
			case ScreenStatus.MODE_SOUBI_HENKOU_ITEM_SELECT:
			case ScreenStatus.MODE_SOUBI_HENKOU_PREVIEW:
				text = "装備変更";
				break;
			case ScreenStatus.MODE_KINDAIKA_KAISHU:
			case ScreenStatus.MODE_KINDAIKA_KAISHU_SOZAI_SENTAKU:
			case ScreenStatus.MODE_KINDAIKA_KAISHU_KAKUNIN:
			case ScreenStatus.MODE_KINDAIKA_KAISHU_ANIMATION:
			case ScreenStatus.MODE_KINDAIKA_KAISHU_END_ANIMATION:
				text = "近代化改修";
				break;
			case ScreenStatus.MODE_KAIZO:
			case ScreenStatus.MODE_KAIZO_ANIMATION:
			case ScreenStatus.MODE_KAIZO_END_ANIMATION:
				text = "改造";
				break;
			}
			if (deck != null && deck.IsActionEnd())
			{
				this.mTransform_TurnEndStamp.SetActive(true);
				ShortcutExtensions.DOKill(this.mTransform_TurnEndStamp, false);
				ShortcutExtensions.DOLocalRotate(this.mTransform_TurnEndStamp, new Vector3(0f, 0f, 300f), 0f, 1);
				TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalRotate(this.mTransform_TurnEndStamp, new Vector3(0f, 0f, 360f), 0.8f, 1), 30);
			}
			else
			{
				this.mTransform_TurnEndStamp.SetActive(false);
			}
			this.titleLabel.text = text;
			this.titleLabel.supportEncoding = false;
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref this.titleLabel);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.ammoLabel);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.steelLabel);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.bauxLabel);
			this.mTransform_TurnEndStamp = null;
		}
	}
}
