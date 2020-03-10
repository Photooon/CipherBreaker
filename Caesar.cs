using System;
using System.Collections.Generic;
using System.Text;

namespace CipherBreaker
{
	class Caesar : Scheme
	{
		public Caesar()
		{
			
		}

		~Caesar()
		{
			
		}

		public override string EncodeKey
		{
			get
			{
				return encodeKey;
			}
			set
			{
				int result;
				if (int.TryParse(value,out result))
				{
					result = (result % 26 + 26) % 26;
					encodeKey = $"{result}";
				}
				else
				{
					throw new FormatException("不可用密钥");
				}
			}
		}

		public override string DecodeKey
		{
			get;set;
		}

		public override bool Encode(string plain = "", string encodeKey = "")
		{
			throw new NotImplementedException();
		}
		public override bool Decode(string cipher = "", string decodeKey = "")
		{
			throw new NotImplementedException();
		}
		public override bool Break(string cipher = "")
		{
			throw new NotImplementedException();
		}
		public override bool Save(string fileName)
		{
			throw new NotImplementedException();
		}
		public override bool Load(string fileName)
		{
			throw new NotImplementedException();
		}

		public override string ToString()
		{
			throw new NotImplementedException();
		}
	}
}
