using DG.Tweening;
using KCV;
using KCV.Utils;
using local.models;
using System;
using UnityEngine;

[RequireComponent(typeof(UIButtonManager))]
public class UIAlbumSelectGate : MonoBehaviour
{
	private UIButtonManager mUIButtonManager;

	[SerializeField]
	private UITexture mTexture_Focus;

	[SerializeField]
	private UIButton mButton_ShipAlbum;

	[SerializeField]
	private UIButton mButton_SlotItemAlbum;

	[SerializeField]
	private UITexture mTexture_FlagShip;

	private UIButton mButton_CurrentFocus;

	private KeyControl mKeyController;

	private Action mOnSelectedBackListener;

	private Action mOnSelectedSlotItemAlbumListener;

	private Action mOnSelectedShipAlbumListener;

	private void Awake()
	{
		this.mUIButtonManager = base.GetComponent<UIButtonManager>();
		this.mUIButtonManager.IndexChangeAct = delegate
		{
			if (this.mKeyController != null)
			{
				this.ChangeFocusButton(this.mUIButtonManager.nowForcusButton);
			}
		};
		this.mButton_CurrentFocus = this.mButton_ShipAlbum;
		this.mTexture_Focus.get_transform().set_localPosition(new Vector3(-240f, 0f, 0f));
	}

	private void Update()
	{
		if (this.mKeyController != null)
		{
			if (this.mKeyController.keyState.get_Item(14).down)
			{
				this.ChangeFocusButton(this.mButton_ShipAlbum);
			}
			else if (this.mKeyController.keyState.get_Item(10).down)
			{
				this.ChangeFocusButton(this.mButton_SlotItemAlbum);
			}
			else if (this.mKeyController.keyState.get_Item(1).down)
			{
				this.OnSelectCurrentFocus();
			}
			else if (this.mKeyController.IsRDown())
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.BackToStrategy();
			}
			else if (this.mKeyController.IsBatuDown())
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.BackToActiveScene();
			}
		}
	}

	private void ChangeFocusButton(UIButton button)
	{
		if (this.mButton_CurrentFocus != null)
		{
			bool flag = this.mButton_CurrentFocus.Equals(button);
			if (flag)
			{
				return;
			}
		}
		this.mButton_CurrentFocus = button;
		if (this.mButton_CurrentFocus != null)
		{
			bool flag2 = this.mButton_ShipAlbum.Equals(this.mButton_CurrentFocus);
			if (flag2)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				TweenSettingsExtensions.SetId<Tweener>(ShortcutExtensions.DOLocalMoveX(this.mTexture_Focus.get_transform(), -240f, 0.3f, false), this.mTexture_Focus);
			}
			else
			{
				bool flag3 = this.mButton_SlotItemAlbum.Equals(this.mButton_CurrentFocus);
				if (flag3)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
					TweenSettingsExtensions.SetId<Tweener>(ShortcutExtensions.DOLocalMoveX(this.mTexture_Focus.get_transform(), 240f, 0.3f, false), this.mTexture_Focus);
				}
			}
		}
	}

	private void OnSelectedShipAlbum()
	{
		if (this.mOnSelectedShipAlbumListener != null)
		{
			this.mOnSelectedShipAlbumListener.Invoke();
		}
	}

	private void OnSelectedSlotItemAlbum()
	{
		if (this.mOnSelectedSlotItemAlbumListener != null)
		{
			this.mOnSelectedSlotItemAlbumListener.Invoke();
		}
	}

	private void OnSelectedBack()
	{
		if (this.mOnSelectedBackListener != null)
		{
			this.mOnSelectedBackListener.Invoke();
		}
	}

	private void OnSelectCurrentFocus()
	{
		if (this.mButton_CurrentFocus != null)
		{
			SoundUtils.PlayOneShotSE(SEFIleInfos.CommonEnter1);
			bool flag = this.mButton_CurrentFocus.Equals(this.mButton_ShipAlbum);
			if (flag)
			{
				this.OnSelectedShipAlbum();
			}
			else
			{
				bool flag2 = this.mButton_CurrentFocus.Equals(this.mButton_SlotItemAlbum);
				if (flag2)
				{
					this.OnSelectedSlotItemAlbum();
				}
			}
		}
	}

	[Obsolete("Inspector上で設定して使用します")]
	public void OnTouchShipAlbum()
	{
		this.ChangeFocusButton(this.mButton_ShipAlbum);
		this.OnSelectCurrentFocus();
	}

	[Obsolete("Inspector上で設定して使用します")]
	public void OnTouchSlotItemAlbum()
	{
		this.ChangeFocusButton(this.mButton_SlotItemAlbum);
		this.OnSelectCurrentFocus();
	}

	[Obsolete("Inspector上で設定して使用します")]
	public void OnTouchBack()
	{
		this.OnSelectedBack();
	}

	public void SetOnSelectedShipAlbumListener(Action onSelectedShipAlbumListener)
	{
		this.mOnSelectedShipAlbumListener = onSelectedShipAlbumListener;
	}

	public void SetOnSelectedSlotItemAlbumListener(Action onSelectedSlotItemAlbumListener)
	{
		this.mOnSelectedSlotItemAlbumListener = onSelectedSlotItemAlbumListener;
	}

	public void SetKeyController(KeyControl keyController)
	{
		this.mKeyController = keyController;
	}

	public void SetOnSelectedBackListener(Action onSelectedBackListener)
	{
		this.mOnSelectedBackListener = onSelectedBackListener;
	}

	public void Initialize(ShipModel shipModel)
	{
		this.mTexture_FlagShip.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(shipModel.GetGraphicsMstId(), (!shipModel.IsDamaged()) ? 9 : 10);
		this.mTexture_FlagShip.get_transform().set_localPosition(Util.Poi2Vec(shipModel.Offsets.GetFace(shipModel.IsDamaged())));
		this.mTexture_FlagShip.MakePixelPerfect();
	}
}
