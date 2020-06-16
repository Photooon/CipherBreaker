using System;

namespace CipherBreaker
{
	public enum OperationType : int
	{
		Encode = 0,
		Decode = 1,
		Break = 2
	}
	class OperationRecord
	{
		public OperationType Type;
		public string OriginText;
		public string Key;
		public string ResultText;
		public DateTime Date;

		public OperationRecord(int type, string originText, string key, string resultText, string date)
		{
			this.Type = (OperationType)type;
			this.OriginText = originText;
			this.Key = key;
			this.ResultText = resultText;
			this.Date = DateTime.Parse(date);
		}
	}
}
