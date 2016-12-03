using local.models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Resource
{
	internal class ShipResource
	{
		private StateType mStateType;

		private TextureType mTextureType;

		private Dictionary<int, Texture> mResources;

		public ShipResource(TextureType textureType, StateType stateType)
		{
			this.mStateType = stateType;
			this.mTextureType = textureType;
			this.mResources = new Dictionary<int, Texture>();
		}

		[DebuggerHidden]
		public IEnumerator GenerateLoadAsync(ShipModel[] shipModels)
		{
			ShipResource.<GenerateLoadAsync>c__Iterator1CC <GenerateLoadAsync>c__Iterator1CC = new ShipResource.<GenerateLoadAsync>c__Iterator1CC();
			<GenerateLoadAsync>c__Iterator1CC.shipModels = shipModels;
			<GenerateLoadAsync>c__Iterator1CC.<$>shipModels = shipModels;
			<GenerateLoadAsync>c__Iterator1CC.<>f__this = this;
			return <GenerateLoadAsync>c__Iterator1CC;
		}

		public Texture GetResource(int masterId)
		{
			Texture result = null;
			if (this.mResources.TryGetValue(masterId, ref result))
			{
				return result;
			}
			string text = ShipResource.GenerateResourcePath(masterId, this.mTextureType, this.mStateType);
			Texture texture = Resources.Load<Texture>(text);
			this.mResources.Add(masterId, texture);
			return texture;
		}

		public void ReleaseTextures()
		{
			using (Dictionary<int, Texture>.ValueCollection.Enumerator enumerator = this.mResources.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Texture current = enumerator.get_Current();
					Resources.UnloadAsset(current);
				}
			}
			using (Dictionary<int, Texture>.KeyCollection.Enumerator enumerator2 = this.mResources.get_Keys().GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					int current2 = enumerator2.get_Current();
					this.mResources.Remove(current2);
				}
			}
			this.mResources.Clear();
		}

		private void SetResource(int masterId, Texture texture)
		{
			if (!this.mResources.ContainsKey(masterId))
			{
				this.mResources.Add(masterId, texture);
			}
		}

		private static string GenerateResourcePath(int masterId, TextureType textureType, StateType stateType)
		{
			int num = ShipResource.FindResourceNo(textureType, stateType);
			return string.Format("Textures/Ships/{0}/{1}", masterId, num);
		}

		private static int FindResourceNo(TextureType textureType, StateType stateType)
		{
			switch (textureType)
			{
			case TextureType.Card:
				if (stateType == StateType.Normal)
				{
					return 3;
				}
				if (stateType == StateType.Damaged)
				{
					return 4;
				}
				break;
			case TextureType.Banner:
				if (stateType == StateType.Normal)
				{
					return 1;
				}
				if (stateType == StateType.Damaged)
				{
					return 2;
				}
				break;
			case TextureType.Full:
				if (stateType == StateType.Normal)
				{
					return 9;
				}
				if (stateType == StateType.Damaged)
				{
					return 10;
				}
				break;
			}
			return -1;
		}
	}
}
