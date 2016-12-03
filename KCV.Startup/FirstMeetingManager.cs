using local.models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Startup
{
	[RequireComponent(typeof(UIPanel)), RequireComponent(typeof(UIRoot))]
	public class FirstMeetingManager : MonoBehaviour
	{
		[SerializeField]
		private TweenPosition CameraZoomTween;

		[SerializeField]
		private TweenAlpha WhiteMaskTweenAlpha;

		[SerializeField]
		private UIShipCharacter ShipCharacter;

		[SerializeField]
		private TweenAlpha BGTweenAlpha;

		private Dictionary<int, Live2DModel.MotionType> MotionList;

		private UIPanel _uiPanel;

		public Live2DModel.MotionType DebugType;

		[Button("DebugPlay", "DebugPlay", new object[]
		{

		})]
		public int __Button1;

		public int DebugMstID;

		private UIPanel panel
		{
			get
			{
				return this.GetComponentThis(ref this._uiPanel);
			}
		}

		public static FirstMeetingManager Instantiate(FirstMeetingManager prefab, int nMstId)
		{
			FirstMeetingManager firstMeetingManager = Object.Instantiate<FirstMeetingManager>(prefab);
			firstMeetingManager.get_transform().set_position(Vector3.get_right() * 50f);
			firstMeetingManager.get_transform().localScaleZero();
			firstMeetingManager.VirtualCtor(nMstId);
			return firstMeetingManager;
		}

		private void Awake()
		{
			this.panel.alpha = 0f;
		}

		private void OnDestroy()
		{
			Mem.Del<TweenPosition>(ref this.CameraZoomTween);
			Mem.Del<TweenAlpha>(ref this.WhiteMaskTweenAlpha);
			Mem.Del<UIShipCharacter>(ref this.ShipCharacter);
			Mem.Del<UIPanel>(ref this._uiPanel);
			if (this.MotionList != null)
			{
				this.MotionList.Clear();
			}
			this.MotionList = null;
		}

		private bool VirtualCtor(int nMstId)
		{
			this.panel.alpha = 0f;
			this.ShipCharacter.ChangeCharacter(new ShipModelMst(nMstId));
			return true;
		}

		[DebuggerHidden]
		public IEnumerator Play(int MstID, Action OnFinished)
		{
			FirstMeetingManager.<Play>c__Iterator135 <Play>c__Iterator = new FirstMeetingManager.<Play>c__Iterator135();
			<Play>c__Iterator.MstID = MstID;
			<Play>c__Iterator.OnFinished = OnFinished;
			<Play>c__Iterator.<$>MstID = MstID;
			<Play>c__Iterator.<$>OnFinished = OnFinished;
			<Play>c__Iterator.<>f__this = this;
			return <Play>c__Iterator;
		}

		private void DebugPlay()
		{
			base.StopAllCoroutines();
			base.StartCoroutine(this.Play(this.DebugMstID, null));
		}
	}
}
