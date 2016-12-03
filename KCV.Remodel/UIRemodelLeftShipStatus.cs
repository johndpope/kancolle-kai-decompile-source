using KCV.Scene.Port;
using local.models;
using System;
using UnityEngine;

namespace KCV.Remodel
{
	[SelectionBase]
	public class UIRemodelLeftShipStatus : MonoBehaviour, UIRemodelView
	{
		private const float ANIMATION_DURATION = 0.2f;

		[SerializeField]
		private UITexture shipTypeMarkIcon;

		[SerializeField]
		private UILabel labelName;

		[SerializeField]
		private UILabel labelLevel;

		[SerializeField]
		private UiStarManager stars;

		[SerializeField]
		private UISprite background;

		[SerializeField]
		private UITable paramTable;

		[SerializeField]
		private UILabel labelKaryoku;

		[SerializeField]
		private UILabel labelSoukou;

		[SerializeField]
		private UILabel labelRaiso;

		[SerializeField]
		private UILabel labelTaiku;

		private ShipModel ship;

		private Vector3 showPos = new Vector3(-210f, -170f);

		private Vector3 showPos4Expand = new Vector3(-210f, -90f);

		private Vector3 hidePos = new Vector3(-840f, -170f);

		private Vector3 hidePos4Expand = new Vector3(-840f, -90f);

		private int EXPANDED_HEIGHT = 180;

		private int NORMAL_HEIGHT = 100;

		private bool expand;

		private void Awake()
		{
			base.get_transform().set_localPosition(this.hidePos);
		}

		public void Start()
		{
			base.get_transform().set_localPosition(this.hidePos);
		}

		public void Init(ShipModel ship)
		{
			this.ship = ship;
			this.labelName.text = ship.Name;
			this.labelLevel.text = ship.Level.ToString();
			this.stars.init(ship.Srate);
			this.shipTypeMarkIcon.mainTexture = ResourceManager.LoadShipTypeIcon(ship);
			this.labelKaryoku.text = ship.Karyoku.ToString();
			this.labelSoukou.text = ship.Soukou.ToString();
			this.labelRaiso.text = ship.Raisou.ToString();
			this.labelTaiku.text = ship.Taiku.ToString();
		}

		public void SetExpand(bool expand)
		{
			this.expand = expand;
			if (expand)
			{
				this.paramTable.GetComponent<UIWidget>().alpha = 1f;
			}
			else
			{
				this.paramTable.GetComponent<UIWidget>().alpha = 0.001f;
			}
			this.background.height = ((!expand) ? this.NORMAL_HEIGHT : this.EXPANDED_HEIGHT);
		}

		public void Show()
		{
			this.Show(true);
		}

		public void Show(bool animation)
		{
			base.get_gameObject().SetActive(true);
			Vector3 vector = (!this.expand) ? this.showPos : this.showPos4Expand;
			if (animation)
			{
				RemodelUtils.MoveWithManual(base.get_gameObject(), vector, 0.2f, null);
			}
			else
			{
				base.get_transform().set_localPosition(vector);
			}
		}

		public void Hide()
		{
			this.Hide(true);
		}

		public void Hide(bool animation)
		{
			Vector3 vector = (!this.expand) ? this.hidePos : this.hidePos4Expand;
			if (animation)
			{
				RemodelUtils.MoveWithManual(base.get_gameObject(), vector, 0.2f, delegate
				{
					base.get_gameObject().SetActive(false);
				});
			}
			else
			{
				base.get_transform().set_localPosition(vector);
				base.get_gameObject().SetActive(false);
			}
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref this.shipTypeMarkIcon, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.labelName);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.background);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.labelKaryoku);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.labelSoukou);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.labelRaiso);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.labelTaiku);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.shipTypeMarkIcon, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.shipTypeMarkIcon, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.shipTypeMarkIcon, false);
			this.stars = null;
			this.paramTable = null;
			this.ship = null;
		}
	}
}
