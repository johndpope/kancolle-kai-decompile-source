using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace UniRx
{
	public class WWWErrorException : Exception
	{
		public string RawErrorMessage
		{
			get;
			private set;
		}

		public bool HasResponse
		{
			get;
			private set;
		}

		public HttpStatusCode StatusCode
		{
			get;
			private set;
		}

		public Dictionary<string, string> ResponseHeaders
		{
			get;
			private set;
		}

		public WWW WWW
		{
			get;
			private set;
		}

		public WWWErrorException(WWW www)
		{
			this.WWW = www;
			this.RawErrorMessage = www.get_error();
			this.ResponseHeaders = www.get_responseHeaders();
			this.HasResponse = false;
			string[] array = this.RawErrorMessage.Split(new char[]
			{
				' '
			});
			int statusCode;
			if (array.Length != 0 && int.TryParse(array[0], ref statusCode))
			{
				this.HasResponse = true;
				this.StatusCode = statusCode;
			}
		}

		public override string ToString()
		{
			string text = this.WWW.get_text();
			if (string.IsNullOrEmpty(text))
			{
				return this.RawErrorMessage;
			}
			return this.RawErrorMessage + " " + text;
		}
	}
}
