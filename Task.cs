using System;
using System.Collections.Generic;
using System.Text;

namespace CipherBreaker
{
	public class Task
	{
		private long id;
		public SchemeType type;
		public DateTime Date;
		public string Name;
		public OperationType OptType;
		public string OriginText;
		public string Key;
		public string ResultText;
		public override string ToString()
		{
			return this.Name;
		}
	}
}
