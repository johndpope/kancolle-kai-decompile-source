using KCV.Battle.Utils;
using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdShellingSlot : BaseProdLine
	{
		[Serializable]
		private class Slot : IDisposable
		{
			[SerializeField]
			private Transform _tra;

			[SerializeField]
			private UITexture _uiSlotIcon;

			[SerializeField]
			private UILabel _uiSlotName;

			private bool _isPlane;

			public UIWidget.Pivot slotNamePivot
			{
				get
				{
					return this._uiSlotName.pivot;
				}
				set
				{
					this._uiSlotName.pivot = value;
				}
			}

			public UILabel slotName
			{
				get
				{
					return this._uiSlotName;
				}
			}

			public bool isSlotIconActive
			{
				get
				{
					return this._uiSlotIcon.get_gameObject().get_activeSelf();
				}
				set
				{
					this._uiSlotIcon.SetActive(value);
				}
			}

			public Vector3 slotIconScale
			{
				get
				{
					return this._uiSlotIcon.get_transform().get_localScale();
				}
				set
				{
					this._uiSlotIcon.get_transform().set_localScale(value);
				}
			}

			public Transform transform
			{
				get
				{
					return this._tra;
				}
				set
				{
					if (this._tra != value)
					{
						this._tra = value;
					}
				}
			}

			public UITexture slotIcon
			{
				get
				{
					return this._uiSlotIcon;
				}
			}

			public int depth
			{
				get
				{
					return this._uiSlotIcon.depth;
				}
				set
				{
					this._uiSlotIcon.depth = value;
					this._uiSlotName.depth = value + 1;
				}
			}

			public Slot(Transform parent, string objName)
			{
				Util.FindParentToChild<Transform>(ref this._tra, parent, objName);
				Util.FindParentToChild<UITexture>(ref this._uiSlotIcon, this._tra, "SlotIcon");
				Util.FindParentToChild<UILabel>(ref this._uiSlotName, this._tra, "SlotName");
				this._uiSlotIcon.localSize = ResourceManager.SLOTITEM_TEXTURE_SIZE.get_Item(4);
				this._uiSlotIcon.get_transform().set_localPosition(Vector3.get_up() * 120f);
			}

			public bool Init()
			{
				this._uiSlotIcon.localSize = ResourceManager.SLOTITEM_TEXTURE_SIZE.get_Item(4);
				this._uiSlotIcon.get_transform().set_localPosition(Vector3.get_up() * 120f);
				return true;
			}

			public void Dispose()
			{
				Mem.Del<Transform>(ref this._tra);
				Mem.Del<UITexture>(ref this._uiSlotIcon);
				Mem.Del<UILabel>(ref this._uiSlotName);
				Mem.Del<bool>(ref this._isPlane);
			}

			public bool SetSlotItem(SlotitemModel_Battle model)
			{
				return this.SetSlotItem(model, true);
			}

			public bool SetSlotItem(SlotitemModel_Battle model, bool isSlotIconActive)
			{
				bool flag = model != null && model.IsPlane();
				this._isPlane = flag;
				this._uiSlotIcon.mainTexture = ((model != null) ? ((!flag) ? SlotItemUtils.LoadTexture(model) : SlotItemUtils.LoadUniDirTexture(model)) : null);
				this._uiSlotIcon.localSize = ((!flag) ? ResourceManager.SLOTITEM_TEXTURE_SIZE.get_Item(4) : ResourceManager.SLOTITEM_TEXTURE_SIZE.get_Item(7));
				this._uiSlotName.text = ((model != null && !flag) ? model.Name : string.Empty);
				this.isSlotIconActive = isSlotIconActive;
				return true;
			}

			public void SetFlipHorizontal(bool isNotFlipHorizontal)
			{
				this._uiSlotName.get_transform().set_localRotation((!isNotFlipHorizontal) ? Quaternion.Euler(0f, 180f, 0f) : Quaternion.get_identity());
				this._uiSlotIcon.get_transform().set_localRotation((!this._isPlane) ? Quaternion.get_identity() : ((!isNotFlipHorizontal) ? Quaternion.Euler(0f, 180f, 0f) : Quaternion.get_identity()));
			}
		}

		[Serializable]
		private class GlowAircraftIcon : IDisposable
		{
			[SerializeField]
			private Transform _transform;

			private UITexture _uiTexture;

			private MoveWith _clsMoveWith;

			public Transform transform
			{
				get
				{
					return this._transform;
				}
			}

			public UITexture uiTexture
			{
				get
				{
					if (this._uiTexture == null)
					{
						this._uiTexture = this.transform.GetComponent<UITexture>();
					}
					return this._uiTexture;
				}
			}

			public MoveWith moveWith
			{
				get
				{
					if (this._clsMoveWith == null)
					{
						this._clsMoveWith = this.transform.GetComponent<MoveWith>();
					}
					return this._clsMoveWith;
				}
			}

			public bool isActive
			{
				get
				{
					return this.transform.get_gameObject().get_activeInHierarchy();
				}
				set
				{
					this.transform.SetActive(value);
				}
			}

			public bool Init(SlotitemModel_Battle model)
			{
				if (model == null || !model.IsPlane())
				{
					return false;
				}
				this.uiTexture.mainTexture = SlotItemUtils.LoadUniDirGlowTexture(model);
				this.uiTexture.localSize = ResourceManager.SLOTITEM_TEXTURE_SIZE.get_Item(7);
				return true;
			}

			public bool Init(SlotitemModel_Battle model, bool isFlip)
			{
				if (!this.Init(model))
				{
					return false;
				}
				this.uiTexture.flip = ((!isFlip) ? UIBasicSprite.Flip.Nothing : UIBasicSprite.Flip.Horizontally);
				return true;
			}

			public void Dispose()
			{
				Mem.Del<Transform>(ref this._transform);
				Mem.Del<UITexture>(ref this._uiTexture);
				Mem.Del<MoveWith>(ref this._clsMoveWith);
			}
		}

		[SerializeField]
		private Transform _slotIconGrow;

		[SerializeField]
		private NoiseMove _clsNoiseMove;

		[SerializeField]
		private ProdShellingSlot.GlowAircraftIcon _clsGlowIcon;

		[SerializeField]
		private List<UITexture> _listOverlay;

		[SerializeField]
		private UILabel _uiFlashText;

		[SerializeField]
		private List<ProdShellingSlot.Slot> _listSlots;

		private UIPanel _uiPanel;

		private Dictionary<BaseProdLine.AnimationName, Vector3> _dicSlotSize;

		private bool _isFriend;

		public UIPanel panel
		{
			get
			{
				if (this._uiPanel == null)
				{
					this._uiPanel = base.GetComponent<UIPanel>();
				}
				return this._uiPanel;
			}
		}

		public bool isFinished
		{
			get
			{
				return this._isFinished;
			}
		}

		public bool isNotFlipHorizontal
		{
			set
			{
				base.get_transform().set_rotation((!value) ? Quaternion.Euler(0f, 180f, 0f) : Quaternion.get_identity());
				this._listSlots.ForEach(delegate(ProdShellingSlot.Slot x)
				{
					x.SetFlipHorizontal(value);
				});
			}
		}

		public BaseProdLine.AnimationName slotNamePivot
		{
			set
			{
				if (value == BaseProdLine.AnimationName.ProdLine)
				{
					this._listSlots.get_Item(0).slotNamePivot = UIWidget.Pivot.Left;
					this._listSlots.get_Item(0).slotName.get_transform().localPositionZero();
				}
				else if (value == BaseProdLine.AnimationName.ProdNormalAttackLine)
				{
					this._listSlots.get_Item(0).slotNamePivot = ((!this._isFriend) ? UIWidget.Pivot.Left : UIWidget.Pivot.Right);
					this._listSlots.get_Item(0).slotName.get_transform().set_localPosition(Vector3.get_right() * 375f);
				}
				else if (value == BaseProdLine.AnimationName.ProdTripleLine || value == BaseProdLine.AnimationName.ProdSuccessiveLine)
				{
					this._listSlots.ForEach(delegate(ProdShellingSlot.Slot x)
					{
						x.slotNamePivot = UIWidget.Pivot.Center;
					});
					this._listSlots.get_Item(0).slotName.get_transform().localPositionZero();
				}
			}
		}

		public BaseProdLine.AnimationName slotIconActive
		{
			set
			{
				this._listSlots.ForEach(delegate(ProdShellingSlot.Slot x)
				{
					if (value == BaseProdLine.AnimationName.ProdLine)
					{
						x.isSlotIconActive = true;
					}
					else if (value == BaseProdLine.AnimationName.ProdNormalAttackLine || value == BaseProdLine.AnimationName.ProdAircraftAttackLine)
					{
						x.isSlotIconActive = true;
					}
					else
					{
						x.isSlotIconActive = false;
					}
				});
			}
		}

		private BaseProdLine.AnimationName depth
		{
			set
			{
				switch (value)
				{
				case BaseProdLine.AnimationName.ProdLine:
				case BaseProdLine.AnimationName.ProdTripleLine:
				{
					int j;
					int i = j = 0;
					this._listOverlay.ForEach(delegate(UITexture x)
					{
						x.depth = i * 10;
						i++;
					});
					this._listSlots.ForEach(delegate(ProdShellingSlot.Slot x)
					{
						x.depth = j * 10 + 1;
						j++;
					});
					break;
				}
				case BaseProdLine.AnimationName.ProdNormalAttackLine:
				{
					int j;
					int i = j = 0;
					this._listOverlay.ForEach(delegate(UITexture x)
					{
						x.depth = i * 10;
						i++;
					});
					this._listSlots.ForEach(delegate(ProdShellingSlot.Slot x)
					{
						x.depth = j * 10 + 100;
						j++;
					});
					break;
				}
				}
			}
		}

		public static ProdShellingSlot Instantiate(ProdShellingSlot prefab, Transform parent)
		{
			ProdShellingSlot prodShellingSlot = Object.Instantiate<ProdShellingSlot>(prefab);
			prodShellingSlot.get_transform().set_parent(parent);
			prodShellingSlot.get_transform().set_localScale(Vector3.get_one());
			prodShellingSlot.get_transform().set_localPosition(Vector3.get_zero());
			return prodShellingSlot;
		}

		private void Awake()
		{
			this._isFinished = false;
			this._listSlots.ForEach(delegate(ProdShellingSlot.Slot x)
			{
				x.Init();
				x.SetSlotItem(null, false);
			});
			this._listOverlay.ForEach(delegate(UITexture x)
			{
				x.get_transform().localScaleZero();
			});
			this._dicSlotSize = new Dictionary<BaseProdLine.AnimationName, Vector3>();
			this._dicSlotSize.Add(BaseProdLine.AnimationName.ProdLine, Vector3.get_one() * 0.65f);
			this._clsGlowIcon.isActive = false;
			this.panel.widgetsAreStatic = true;
			base.get_transform().localScaleZero();
		}

		private void OnDestroy()
		{
			Mem.Del<Transform>(ref this._slotIconGrow);
			Mem.Del<NoiseMove>(ref this._clsNoiseMove);
			Mem.DelIDisposableSafe<ProdShellingSlot.GlowAircraftIcon>(ref this._clsGlowIcon);
			Mem.DelListSafe<UITexture>(ref this._listOverlay);
			Mem.Del<UILabel>(ref this._uiFlashText);
			if (this._listSlots != null)
			{
				this._listSlots.ForEach(delegate(ProdShellingSlot.Slot x)
				{
					x.Dispose();
				});
			}
			Mem.DelListSafe<ProdShellingSlot.Slot>(ref this._listSlots);
			Mem.Del<UIPanel>(ref this._uiPanel);
			Mem.DelDictionarySafe<BaseProdLine.AnimationName, Vector3>(ref this._dicSlotSize);
			Mem.Del<bool>(ref this._isFriend);
		}

		public void SetSlotData(SlotitemModel_Battle model)
		{
			this._listSlots.get_Item(0).SetSlotItem(model);
			this._clsGlowIcon.Init(model);
		}

		public void SetSlotData(SlotitemModel_Battle[] models)
		{
			int num = 0;
			for (int i = 0; i < models.Length; i++)
			{
				SlotitemModel_Battle slotItem = models[i];
				if (this._listSlots.get_Item(num) != null)
				{
					this._listSlots.get_Item(num).SetSlotItem(slotItem);
					num++;
				}
			}
		}

		public void SetSlotData(SlotitemModel_Battle[] models, ProdTranscendenceCutIn.AnimationList iList)
		{
			switch (iList)
			{
			case ProdTranscendenceCutIn.AnimationList.ProdTAMainBatteryx3:
			{
				int num = 0;
				for (int i = 0; i < models.Length; i++)
				{
					SlotitemModel_Battle slotItem = models[i];
					if (this._listSlots.get_Item(num) != null)
					{
						this._listSlots.get_Item(num).SetSlotItem(slotItem);
						num++;
					}
				}
				break;
			}
			case ProdTranscendenceCutIn.AnimationList.ProdTATorpedox2:
			{
				int num2 = 0;
				using (List<ProdShellingSlot.Slot>.Enumerator enumerator = this._listSlots.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ProdShellingSlot.Slot current = enumerator.get_Current();
						if (num2 == 0)
						{
							current.SetSlotItem(models[0]);
						}
						else if (num2 > 0)
						{
							current.SetSlotItem(models[1]);
						}
						num2++;
					}
				}
				break;
			}
			case ProdTranscendenceCutIn.AnimationList.ProdTAMainBatteryNTorpedo:
			{
				int num3 = 0;
				for (int j = 0; j < models.Length; j++)
				{
					SlotitemModel_Battle slotItem2 = models[j];
					if (this._listSlots.get_Item(num3) != null)
					{
						this._listSlots.get_Item(num3).SetSlotItem(slotItem2);
						num3++;
					}
				}
				break;
			}
			}
		}

		public void Play(BaseProdLine.AnimationName iName, bool isNotFlipHorizontal, Action callback)
		{
			this._isFriend = isNotFlipHorizontal;
			this.panel.widgetsAreStatic = false;
			this._listOverlay.ForEach(delegate(UITexture x)
			{
				Color color = (!this._isFriend) ? new Color(1f, 0f, 0f, x.alpha) : new Color(0f, 0.31875f, 1f, x.alpha);
				x.get_transform().localScaleOne();
				x.color = color;
			});
			this.isNotFlipHorizontal = isNotFlipHorizontal;
			this.slotIconActive = iName;
			this.depth = iName;
			this.ResetRotation();
			this.SetFlashText(iName);
			this._clsGlowIcon.isActive = (iName == BaseProdLine.AnimationName.ProdAircraftAttackLine);
			this._clsNoiseMove.set_enabled(iName == BaseProdLine.AnimationName.ProdAircraftAttackLine);
			this.SetSlotIconPos(iName);
			this.slotNamePivot = iName;
			base.get_transform().localScaleOne();
			base.Play(iName, callback);
		}

		private void SetFlashText(BaseProdLine.AnimationName iName)
		{
			if (iName == BaseProdLine.AnimationName.ProdNormalAttackLine)
			{
				this._uiFlashText.SetActive(true);
				this._uiFlashText.pivot = UIWidget.Pivot.Center;
				float num = this._listSlots.get_Item(0).slotName.localSize.x / 2f;
				this._uiFlashText.get_transform().set_localPosition(new Vector3(num * (float)((!this._isFriend) ? 1 : -1), 0f, 0f));
				this._uiFlashText.text = this._listSlots.get_Item(0).slotName.text;
			}
			else
			{
				this._uiFlashText.SetActive(false);
			}
		}

		private void SetSlotIconPos(BaseProdLine.AnimationName iName)
		{
			Vector3 localPosition = Vector3.get_zero();
			if (iName == BaseProdLine.AnimationName.ProdNormalAttackLine)
			{
				localPosition = new Vector3(138f * (float)((!this._isFriend) ? 1 : -1), 194f, 0f);
			}
			else if (iName == BaseProdLine.AnimationName.ProdAircraftAttackLine)
			{
				localPosition = Vector3.get_zero();
			}
			else
			{
				localPosition = Vector3.get_up() * 30f;
			}
			this._listSlots.get_Item(0).slotIcon.get_transform().set_localPosition(localPosition);
		}

		private void ResetRotation()
		{
			using (List<ProdShellingSlot.Slot>.Enumerator enumerator = this._listSlots.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ProdShellingSlot.Slot current = enumerator.get_Current();
					current.transform.set_localRotation(Quaternion.get_identity());
				}
			}
			using (List<UITexture>.Enumerator enumerator2 = this._listOverlay.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					UITexture current2 = enumerator2.get_Current();
					current2.get_transform().set_localRotation(Quaternion.get_identity());
				}
			}
		}

		protected override void onFinished()
		{
			base.onFinished();
			base.get_transform().localScaleZero();
			using (List<UITexture>.Enumerator enumerator = this._listOverlay.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					UITexture current = enumerator.get_Current();
					current.get_transform().set_localScale(Vector3.get_zero());
				}
			}
			this._listSlots.ForEach(delegate(ProdShellingSlot.Slot x)
			{
				x.slotName.text = string.Empty;
			});
			this.panel.widgetsAreStatic = true;
		}
	}
}
