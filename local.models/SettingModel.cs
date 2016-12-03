using Server_Controllers;
using Server_Models;
using System;

namespace local.models
{
	public class SettingModel
	{
		private Mem_option _mem_option;

		public int VolumeBGM;

		public int VolumeSE;

		public int VolumeVoice;

		public bool GuideDisplay;

		public SettingModel()
		{
			this._mem_option = new Api_get_Member().Option();
			this.VolumeBGM = this._mem_option.VolumeBGM;
			this.VolumeSE = this._mem_option.VolumeSE;
			this.VolumeVoice = this._mem_option.VolumeVoice;
			this.GuideDisplay = this._mem_option.GuideDisplay;
		}

		public bool IsChanged()
		{
			return this.VolumeBGM != this._mem_option.VolumeBGM || this.VolumeSE != this._mem_option.VolumeSE || this.VolumeVoice != this._mem_option.VolumeVoice || this.GuideDisplay != this._mem_option.GuideDisplay;
		}

		public bool Save()
		{
			if (this.IsChanged())
			{
				this._mem_option.VolumeBGM = this.VolumeBGM;
				this._mem_option.VolumeSE = this.VolumeSE;
				this._mem_option.VolumeVoice = this.VolumeVoice;
				this._mem_option.GuideDisplay = this.GuideDisplay;
				return this._mem_option.UpdateSetting();
			}
			return false;
		}

		public override string ToString()
		{
			string text = string.Concat(new object[]
			{
				"[設定]BGM/SE/ボイス:",
				this.VolumeBGM,
				"/",
				this.VolumeSE,
				"/",
				this.VolumeVoice
			});
			return text + "\tガイド表示:" + ((!this.GuideDisplay) ? "OFF" : "ON");
		}
	}
}
