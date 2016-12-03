using System;
using System.Runtime.Serialization;
using System.Xml.Linq;
using UnityEngine;

namespace Server_Models
{
	[DataContract(Name = "mem_option", Namespace = "")]
	public class Mem_option : Model_Base
	{
		private int _volumeBGM;

		private int _volumeSE;

		private int _volumeVoice;

		private bool _guideDisplay;

		private static string _tableName = "mem_option";

		[DataMember]
		public int VolumeBGM
		{
			get
			{
				return this._volumeBGM;
			}
			set
			{
				this._volumeBGM = value;
			}
		}

		[DataMember]
		public int VolumeSE
		{
			get
			{
				return this._volumeSE;
			}
			set
			{
				this._volumeSE = value;
			}
		}

		[DataMember]
		public int VolumeVoice
		{
			get
			{
				return this._volumeVoice;
			}
			set
			{
				this._volumeVoice = value;
			}
		}

		[DataMember]
		public bool GuideDisplay
		{
			get
			{
				return this._guideDisplay;
			}
			set
			{
				this._guideDisplay = value;
			}
		}

		public static string tableName
		{
			get
			{
				return Mem_option._tableName;
			}
		}

		public Mem_option()
		{
			this.VolumeBGM = PlayerPrefs.GetInt("VolumeBGM", 50);
			this.VolumeSE = PlayerPrefs.GetInt("VolumeSE", 50);
			this.VolumeVoice = PlayerPrefs.GetInt("VolumeVoice", 50);
			int @int = PlayerPrefs.GetInt("GuideDisplay", 1);
			this.GuideDisplay = (@int == 1);
		}

		public bool UpdateSetting()
		{
			PlayerPrefs.SetInt("VolumeBGM", this.VolumeBGM);
			PlayerPrefs.SetInt("VolumeSE", this.VolumeSE);
			PlayerPrefs.SetInt("VolumeVoice", this.VolumeVoice);
			PlayerPrefs.SetInt("GuideDisplay", this.GuideDisplay ? 1 : 0);
			PlayerPrefs.Save();
			return true;
		}

		protected override void setProperty(XElement element)
		{
			this.VolumeBGM = int.Parse(element.Element("VolumeBGM").get_Value());
			this.VolumeSE = int.Parse(element.Element("VolumeSE").get_Value());
			this.VolumeVoice = int.Parse(element.Element("VolumeVoice").get_Value());
			this.GuideDisplay = bool.Parse(element.Element("GuideDisplay").get_Value());
		}
	}
}
