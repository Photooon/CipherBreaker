using System;
using System.Collections.Generic;
using System.Text;

namespace CipherBreaker
{
	class OperationRecord
	{
		public int Type;
		public string OriginText;
		public string Key;
		public string ResultText;
		public DateTime Date;

		OperationRecord(int type,string originText,string key,string resultText,string date)
		{
			this.Type = type;
			this.OriginText = originText;
			this.Key = key;
			this.ResultText = resultText;
			this.Date = DateTime.Parse(date);
		}
	}
}
