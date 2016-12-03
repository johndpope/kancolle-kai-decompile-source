using System;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyUIModel : MonoBehaviour
	{
		[SerializeField]
		private StrategyUIMapManager uiMapManager;

		[SerializeField]
		private StrategyInfoManager infoManager;

		[SerializeField]
		private StrategyAreaManager areaManager;

		[SerializeField]
		private StrategyCamera mapCamera;

		[SerializeField]
		private Transform overView;

		[SerializeField]
		private Camera overCamera;

		[SerializeField]
		private CommonDialog commonDialog;

		[SerializeField]
		private StrategyShipCharacter character;

		[SerializeField]
		private UIHowToStrategy howToStrategy;

		public StrategyUIMapManager UIMapManager
		{
			get
			{
				return this.uiMapManager;
			}
			private set
			{
				this.uiMapManager = value;
			}
		}

		public StrategyInfoManager InfoManager
		{
			get
			{
				return this.infoManager;
			}
		}

		public StrategyAreaManager AreaManager
		{
			get
			{
				return this.areaManager;
			}
		}

		public StrategyCamera MapCamera
		{
			get
			{
				return this.mapCamera;
			}
		}

		public Transform OverView
		{
			get
			{
				return this.overView;
			}
		}

		public Camera OverCamera
		{
			get
			{
				return this.overCamera;
			}
		}

		public CommonDialog CommonDialog
		{
			get
			{
				return this.commonDialog;
			}
		}

		public StrategyShipCharacter Character
		{
			get
			{
				return this.character;
			}
		}

		public UIHowToStrategy HowToStrategy
		{
			get
			{
				return this.howToStrategy;
			}
		}

		private void OnDestroy()
		{
			this.uiMapManager = null;
			this.infoManager = null;
			this.areaManager = null;
			this.mapCamera = null;
			this.overView = null;
			this.overCamera = null;
			this.commonDialog = null;
			this.character = null;
			this.howToStrategy = null;
		}
	}
}
