using System;
using UnityEngine;

namespace KCV.Remodel
{
	public class UIRemodelParameter : MonoBehaviour
	{
		private enum ValueStatus
		{
			DEFAULT,
			UP,
			DOWN,
			MAX
		}

		[SerializeField]
		private UISprite mSprite_ParamType;

		[SerializeField]
		private UILabel mLabel_Value;

		[SerializeField]
		private UISprite mSprite_Status;

		private int mDefaultValue;

		private ParameterType mPrameterType;

		private void OnDestroy()
		{
			if (this.mSprite_ParamType != null)
			{
				this.mSprite_ParamType.RemoveFromPanel();
			}
			this.mSprite_ParamType = null;
			if (this.mLabel_Value != null)
			{
				this.mLabel_Value.RemoveFromPanel();
			}
			this.mLabel_Value = null;
			if (this.mSprite_Status != null)
			{
				this.mSprite_Status.RemoveFromPanel();
				this.mSprite_Status.atlas = null;
			}
			this.mSprite_Status = null;
		}

		public void Initialize(ParameterType parameterType, int defaultValue)
		{
			this.mDefaultValue = defaultValue;
			this.mPrameterType = parameterType;
			this.mLabel_Value.text = this.ParamToString(this.mPrameterType, this.mDefaultValue);
			this.ChangeVirtualUpdateStatusColor(UIRemodelParameter.ValueStatus.DEFAULT);
		}

		public void StatusReset()
		{
			this.mLabel_Value.text = this.ParamToString(this.mPrameterType, this.mDefaultValue);
			this.ChangeVirtualUpdateStatusColor(UIRemodelParameter.ValueStatus.DEFAULT);
		}

		private void ChangeVirtualUpdateStatusColor(UIRemodelParameter.ValueStatus status)
		{
			switch (status)
			{
			case UIRemodelParameter.ValueStatus.DEFAULT:
				this.mSprite_Status.spriteName = string.Empty;
				this.mSprite_Status.get_transform().set_localScale(Vector3.get_zero());
				this.mSprite_Status.alpha = 0.01f;
				break;
			case UIRemodelParameter.ValueStatus.UP:
				this.mSprite_Status.spriteName = "status_up";
				this.mSprite_Status.get_transform().set_localScale(Vector3.get_one());
				this.mSprite_Status.alpha = 1f;
				break;
			case UIRemodelParameter.ValueStatus.DOWN:
				this.mSprite_Status.spriteName = "status_down";
				this.mSprite_Status.get_transform().set_localScale(Vector3.get_one());
				this.mSprite_Status.alpha = 1f;
				break;
			case UIRemodelParameter.ValueStatus.MAX:
				this.mSprite_Status.spriteName = "status_max";
				this.mSprite_Status.get_transform().set_localScale(Vector3.get_one());
				this.mSprite_Status.alpha = 1f;
				break;
			}
		}

		public void PreviewVirtualUpdatedValue(int nextParam, bool isMAX)
		{
			this.mLabel_Value.text = this.ParamToString(this.mPrameterType, nextParam);
			if (nextParam < this.mDefaultValue)
			{
				this.ChangeVirtualUpdateStatusColor(UIRemodelParameter.ValueStatus.DOWN);
			}
			else if (this.mDefaultValue < nextParam)
			{
				if (isMAX)
				{
					this.ChangeVirtualUpdateStatusColor(UIRemodelParameter.ValueStatus.MAX);
				}
				else
				{
					this.ChangeVirtualUpdateStatusColor(UIRemodelParameter.ValueStatus.UP);
				}
			}
			else
			{
				this.ChangeVirtualUpdateStatusColor(UIRemodelParameter.ValueStatus.DEFAULT);
			}
		}

		private string ParamToString(ParameterType paramType, int value)
		{
			if (paramType == ParameterType.Soku)
			{
				return this.GetSoukuText(value);
			}
			if (paramType != ParameterType.Leng)
			{
				return value.ToString();
			}
			return this.GetLengText(value);
		}

		private string GetLengText(int value)
		{
			switch (value)
			{
			case 0:
				return "無";
			case 1:
				return "短";
			case 2:
				return "中";
			case 3:
				return "長";
			case 4:
				return "超長";
			default:
				return string.Empty;
			}
		}

		private string GetSoukuText(int value)
		{
			if (value == 10)
			{
				return "高速";
			}
			return "低速";
		}

		internal void Release()
		{
			if (this.mSprite_ParamType != null)
			{
				this.mSprite_ParamType.RemoveFromPanel();
				this.mSprite_ParamType.Clear();
				NGUITools.Destroy(this.mSprite_ParamType);
			}
			this.mSprite_ParamType = null;
			NGUITools.Destroy(this.mLabel_Value);
			this.mLabel_Value = null;
			if (this.mSprite_Status != null)
			{
				this.mSprite_Status.Clear();
				NGUITools.Destroy(this.mSprite_Status);
			}
			this.mSprite_Status = null;
		}
	}
}
